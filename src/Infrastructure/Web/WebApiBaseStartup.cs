namespace Skeleton.Web
{
    using System;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Configuration;
    using ExceptionsHandling;
    using Logging;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Routing;
    using Swashbuckle.SwaggerGen.Application;
    using Serialization;

    public abstract class WebApiBaseStartup
    {
        protected virtual IConfigurationBuilder AddAdditionalConfigurations(IHostingEnvironment env, IConfigurationBuilder configurationBuilder)
        {
            return configurationBuilder.AddNLogConfig($"NLog.{env.EnvironmentName}.config");
        }

        protected WebApiBaseStartup(IHostingEnvironment env, CommandLineArgumentsProvider commandLineArgumentsProvider)
        {
            var builder = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddCommandLine(commandLineArgumentsProvider.Arguments)
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", false, true);

            Configuration = AddAdditionalConfigurations(env, builder).Build();
        }

        protected IConfigurationRoot Configuration { get; }

        protected virtual void ConfigureJsonSerialization(JsonSerializerSettings serializerSettings)
        {
            serializerSettings.UseDefaultSettings();
        }

        protected abstract void ConfigureSwaggerDocumentator(SwaggerGenOptions options);

        protected abstract void ConfigureOptions(IServiceCollection services);

        protected abstract void RegisterDependencies(ContainerBuilder containerBuilder);

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddUnhandledExceptionsStartupFilter();

            services
                .AddMvc(x => x
                            .UseCentralRoutePrefix($"{Configuration.GetValue("api_route_preffix", "api")}/[controller]")
                            .UseUnhandledExceptionFilter())
                .AddJsonOptions(x => ConfigureJsonSerialization(x.SerializerSettings));

            services.AddSwaggerGen();
            services.ConfigureSwaggerGen(options =>
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
            return new AutofacServiceProvider(container);
        }

        protected virtual void AddLoggerProviders(ILoggerFactory loggerFactory)
        {
            loggerFactory.AddNLog();
        }

        protected virtual Func<IApplicationBuilder, IApplicationBuilder> CreatePipelineConfigurator(
            IHostingEnvironment env, ILoggerFactory loggerFactory,
            Func<IApplicationBuilder, IApplicationBuilder> pipelineBaseConfigurator)
        {
            return pipelineBaseConfigurator;
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            AddLoggerProviders(loggerFactory.AddConsole(Configuration.GetSection("Logging")));

            var pipelineConfigurator = CreatePipelineConfigurator(
                env, loggerFactory,
                x => x.UseMvc()
                    .UseSwagger()
                    .UseSwaggerUi());
            pipelineConfigurator(app);
        }
    }
}