namespace Infrastructure.Web
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
    using OutputFormatting;
    using Routing;

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

        protected virtual void ConfigureOptions(IServiceCollection services)
        {
        }

        protected virtual void RegisterDependencies(ContainerBuilder containerBuilder)
        {   
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc(x => x
                            .UseCentralRoutePrefix($"{Configuration.GetValue("api_route_preffix", "api")}/[controller]")
                            .UseUnhandledExceptionFilter())
                .AddJsonOptions(x => ConfigureJsonSerialization(x.SerializerSettings));

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

        protected virtual IApplicationBuilder AddMiddlewaresToPipeLine(IApplicationBuilder app, IHostingEnvironment env)
        {
            return app;
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            AddLoggerProviders(loggerFactory.AddConsole(Configuration.GetSection("Logging")));

            AddMiddlewaresToPipeLine(app.UseUnhandledExceptionsLoggingMiddleware(), env)
                .UseMvc();
        }
    }
}