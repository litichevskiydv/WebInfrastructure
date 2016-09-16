namespace Web
{
    using System.Reflection;
    using Application.Services.Impl;
    using Autofac;
    using Infrastructure.Web;
    using Infrastructure.Web.Configuration;
    using Installers;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Swashbuckle.Swagger.Model;
    using Swashbuckle.SwaggerGen.Application;

    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class Startup : WebApiBaseStartup
    {
        public Startup(IHostingEnvironment env, CommandLineArgumentsProvider commandLineArgumentsProvider)
            : base(env, commandLineArgumentsProvider)
        {
        }

        protected override void ConfigureSwaggerDocumentator(SwaggerGenOptions options)
        {
            options.SingleApiVersion(new Info
            {
                Version = "v1",
                Title = "Values providing API",
                Description = "A dummy to get configuration values",
                TermsOfService = "None"
            });
        }

        protected override void ConfigureOptions(IServiceCollection services)
        {
            services.Configure<SimpleValuesProviderConfiguration>(Configuration.GetSection("ValuesProviderConfiguration"));
        }

        protected override void RegisterDependencies(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterAssemblyModules(typeof(CommonDependenciesModule).GetTypeInfo().Assembly);
        }
    }
}