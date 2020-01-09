namespace Skeleton.Web.Hosting
{
    using System;
    using Autofac;
    using ExceptionsHandling;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Routing;
    using Serialization.Jil.Configuration;
    using Serialization.Protobuf.Configuration;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using Swashbuckle.AspNetCore.SwaggerUI;
    using Validation;

    public abstract class WebApiBaseStartup
    {
        protected IConfiguration Configuration { get; }
        protected ILoggerFactory LoggerFactory { get; }

        protected WebApiBaseStartup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            if(configuration == null)
                throw new ArgumentNullException(nameof(configuration));
            if (loggerFactory == null)
                throw new ArgumentNullException(nameof(loggerFactory));

            Configuration = configuration;
            LoggerFactory = loggerFactory;
        }

        protected virtual void ConfigureFormatters(IMvcBuilder mvcBuilder)
        {
            mvcBuilder
                .WithJsonFormattersBasedOnJil(OptionsExtensions.Default)
                .WithProtobufFormatters();
        }

        protected abstract void ConfigureSwaggerDocumentator(SwaggerGenOptions options);

        protected abstract void ConfigureSwaggerUi(SwaggerUIOptions options);

        protected abstract void ConfigureOptions(IServiceCollection services);

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddUnhandledExceptionsStartupFilter();

            var mvcBuilder = services
                .AddMvc(mvcOptions =>
                        {
                            mvcOptions
                                .UseCentralRoutePrefix($"{Configuration.GetValue("api_route_preffix", "api")}/[controller]")
                                .UseUnhandledExceptionFilter()
                                .UseModelValidationFilter()
                                .UseParametersValidationFilter();
                            mvcOptions.EnableEndpointRouting = false;
                        }
                );
            ConfigureFormatters(mvcBuilder);

            services
                .AddSwaggerGen(options =>
                               {
                                   ConfigureSwaggerDocumentator(options);

                                   var xmlDocsPath = Configuration.GetValue<string>("xml_docs");
                                   if (string.IsNullOrWhiteSpace(xmlDocsPath) == false)
                                       options.IncludeXmlComments(xmlDocsPath);
                               });

            ConfigureOptions(services.AddOptions());
        }

        protected abstract void RegisterDependencies(ContainerBuilder containerBuilder);

        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            RegisterDependencies(containerBuilder);
        }

        protected virtual Func<IApplicationBuilder, IApplicationBuilder> CreatePipelineConfigurator(
            IWebHostEnvironment env,
            Func<IApplicationBuilder, IApplicationBuilder> pipelineBaseConfigurator
        )
        {
            return pipelineBaseConfigurator;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var pipelineConfigurator = CreatePipelineConfigurator(
                env,
                x => x.UseStaticFiles()
                    .UseSwagger()
                    .UseSwaggerUI(ConfigureSwaggerUi)
                    .UseMvc());
            pipelineConfigurator(app);
        }
    }
}