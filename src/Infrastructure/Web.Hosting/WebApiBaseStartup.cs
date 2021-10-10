﻿namespace Skeleton.Web.Hosting
{
    using System;
    using System.IO;
    using Autofac;
    using ExceptionsHandling;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Routing;
    using Serialization.Jil.Configuration;
    using Serialization.Protobuf.Configuration;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using Swashbuckle.AspNetCore.SwaggerUI;
    using Validation;

    public abstract class WebApiBaseStartup
    {
        protected IConfiguration Configuration { get; }

        protected WebApiBaseStartup(IConfiguration configuration)
        {
            if(configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            Configuration = configuration;
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
                .AddControllers(mvcOptions =>
                                    mvcOptions
                                        .UseCentralRoutePrefix($"{Configuration.GetValue("api_route_preffix", "api")}/[controller]")
                                        .UseUnhandledExceptionFilter()
                                        .UseModelValidationFilter()
                                        .UseParametersValidationFilter()

                );
            ConfigureFormatters(mvcBuilder);

            services
                .AddSwaggerGen(options =>
                               {
                                   ConfigureSwaggerDocumentator(options);

                                   var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml");
                                   foreach (var xmlFile in xmlFiles)
                                       options.IncludeXmlComments(xmlFile);
                               }
                );

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
            var pipelineConfigurator = CreatePipelineConfigurator(env, x => x.UseRouting());
            pipelineConfigurator(app)
                .UseSwagger()
                .UseSwaggerUI(ConfigureSwaggerUi)
                .UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}