namespace Skeleton.Web
{
    using System;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using ExceptionsHandling;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Routing;
    using Serialization.Jil.Configuration;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using Swashbuckle.AspNetCore.SwaggerUI;

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
            mvcBuilder.WithJsonFormattersBasedOnJil(OptionsExtensions.Default);
        }

        protected abstract void ConfigureSwaggerDocumentator(SwaggerGenOptions options);

        protected abstract void ConfigureSwaggerUi(SwaggerUIOptions options);

        protected abstract void ConfigureOptions(IServiceCollection services);

        protected abstract void RegisterDependencies(ContainerBuilder containerBuilder);

        protected abstract void MigrateEnvironment(IContainer container);

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddUnhandledExceptionsStartupFilter();

            var mvcBuilder = services
                .AddMvc(x => x
                            .UseCentralRoutePrefix($"{Configuration.GetValue("api_route_preffix", "api")}/[controller]")
                            .UseUnhandledExceptionFilter());
            ConfigureFormatters(mvcBuilder);

            services
                .AddSwaggerGen(options =>
                               {
                                   ConfigureSwaggerDocumentator(options);

                                   var xmlDocsPath = Configuration.GetValue<string>("xml_docs");
                                   if (string.IsNullOrWhiteSpace(xmlDocsPath) == false)
                                       options.IncludeXmlComments(xmlDocsPath);

                                   options.DescribeAllEnumsAsStrings();
                               });

            ConfigureOptions(services.AddOptions());

            var containerBuilder = new ContainerBuilder();
            RegisterDependencies(containerBuilder);
            containerBuilder.Populate(services);

            var container = containerBuilder.Build();
            MigrateEnvironment(container);
            return new AutofacServiceProvider(container);
        }

        protected virtual Func<IApplicationBuilder, IApplicationBuilder> CreatePipelineConfigurator(
            IHostingEnvironment env,
            Func<IApplicationBuilder, IApplicationBuilder> pipelineBaseConfigurator)
        {
            return pipelineBaseConfigurator;
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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