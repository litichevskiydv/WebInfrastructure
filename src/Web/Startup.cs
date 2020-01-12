namespace Web
{
    using System;
    using System.Reflection;
    using System.Text;
    using Application.Services;
    using Autofac;
    using Domain.Dtos;
    using Installers;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.OpenApi.Models;
    using Skeleton.Dapper.ConnectionsFactory;
    using Skeleton.Web.Authentication.JwtBearer;
    using Skeleton.Web.Authentication.JwtBearer.Configuration;
    using Skeleton.Web.Documentation;
    using Skeleton.Web.Hosting;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using Swashbuckle.AspNetCore.SwaggerUI;

    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class Startup : WebApiBaseStartup
    {
        private string _versionString;

        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory) : base(configuration, loggerFactory)
        {
            _versionString = GetType().Assembly.GetName().Version.ToString(4);
        }

        protected override void ConfigureSwaggerDocumentator(SwaggerGenOptions options)
        {
            options
                .SwaggerDoc($"v{_versionString}",
                    new OpenApiInfo
                    {
                        Version = $"v{_versionString}",
                        Title = "Values providing API",
                        Description = "A dummy to get configuration values"
                    });

            options
                .AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey
                    });
            options.OperationFilter<AuthResponsesOperationFilter>();
        }

        protected override void ConfigureSwaggerUi(SwaggerUIOptions options)
        {
            options.SwaggerEndpoint($"/swagger/v{_versionString}/swagger.json", $"Values providing API v{_versionString}");
        }

        protected override void ConfigureOptions(IServiceCollection services)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["TokensSigningKey"]));
            services
                .Configure<DefaultConfigurationValues>(Configuration.GetSection("DefaultConfigurationValues"))
                .Configure<SqlConnectionsFactoryOptions>(Configuration.GetSection("ConnectionStrings"))
                .Configure<TokensIssuingOptions>(
                    issuingOptions => issuingOptions
                        .WithSigningKey(SecurityAlgorithms.HmacSha256, securityKey)
                        .WithGetEndpoint("/api/Account/Token")
                        .WithLifetime(TimeSpan.FromHours(2))
                        .WithTokenIssueEventHandler(new TokenIssueEventHandler(LoggerFactory.CreateLogger<TokenIssueEventHandler>()))
                )
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(
                    jwtBearerOptions => jwtBearerOptions
                        .WithTokenValidationParameters(
                            parameters => parameters
                                .WithIssuerKeyValidation(securityKey)
                                .WithLifetimeValidation()
                                .WithoutAudienceValidation()
                                .WithoutIssuerValidation()
                        )
                );



        }

        protected override void RegisterDependencies(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterAssemblyModules(typeof(DataAccessInstaller).GetTypeInfo().Assembly);
        }

        protected override Func<IApplicationBuilder, IApplicationBuilder> CreatePipelineConfigurator(
            IWebHostEnvironment env,
            Func<IApplicationBuilder, IApplicationBuilder> pipelineBaseConfigurator
        )
        {
            return x => pipelineBaseConfigurator(x).UseJwtBearerAuthorizationTokens();
        }
    }
}