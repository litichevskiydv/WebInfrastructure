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
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;
    using Skeleton.Dapper.ConnectionsFactory;
    using Skeleton.Migrations.Migrator;
    using Skeleton.Web;
    using Skeleton.Web.Authentication.JwtBearer;
    using Skeleton.Web.Authentication.JwtBearer.Configuration;
    using Skeleton.Web.Documentation;
    using Swashbuckle.AspNetCore.Swagger;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using Swashbuckle.AspNetCore.SwaggerUI;

    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class Startup : WebApiBaseStartup
    {
        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory) : base(configuration, loggerFactory)
        {
        }

        protected override void ConfigureSwaggerDocumentator(SwaggerGenOptions options)
        {
            options
                .SwaggerDoc("v1",
                    new Info
                    {
                        Version = "v1",
                        Title = "Values providing API",
                        Description = "A dummy to get configuration values",
                        TermsOfService = "None"
                    });

            options
                .AddSecurityDefinition("Bearer",
                    new ApiKeyScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                        Name = "Authorization",
                        In = "header",
                        Type = "apiKey"
                    });
            options.OperationFilter<AuthResponsesOperationFilter>();
        }

        protected override void ConfigureSwaggerUi(SwaggerUIOptions options)
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Values providing API v1");
        }

        protected override void ConfigureOptions(IServiceCollection services)
        {
            services
                .Configure<DefaultConfigurationValues>(Configuration.GetSection("DefaultConfigurationValues"))
                .Configure<SqlConnectionsFactoryOptions>(Configuration.GetSection("ConnectionStrings"))
                .AddJwtBearerAuthorisationTokens(
                    b => b
                        .ConfigureSigningKey(
                            SecurityAlgorithms.HmacSha256,
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["TokensSigningKey"])))
                        .ConfigureTokensIssuingOptions(
                            i => i
                                .WithGetEndpotint("/api/Account/Token")
                                .WithLifetime(TimeSpan.FromHours(2))
                                .WithTokenIssueEventHandler(
                                    new TokenIssueEventHandler(LoggerFactory.CreateLogger<TokenIssueEventHandler>())))
                        .ConfigureJwtBearerOptions(
                            o => o
                                .WithTokenValidationParameters(
                                    v => v
                                        .WithLifetimeValidation()
                                        .WithoutAudienceValidation()
                                        .WithoutIssuerValidation())));
        }

        protected override void RegisterDependencies(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterAssemblyModules(typeof(DataAccessInstaller).GetTypeInfo().Assembly);
        }

        protected override void MigrateEnvironment(IContainer container)
        {
            container.Resolve<IMigrator>().Migrate();
        }

        protected override Func<IApplicationBuilder, IApplicationBuilder> CreatePipelineConfigurator(
            IHostingEnvironment env,
            Func<IApplicationBuilder, IApplicationBuilder> pipelineBaseConfigurator)
        {
            return x => pipelineBaseConfigurator(x.UseJwtBearerAuthorisationTokens());
        }
    }
}