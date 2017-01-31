namespace Web
{
    using System.Reflection;
    using Autofac;
    using Domain.Dtos;
    using Installers;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Skeleton.Web;
    using Skeleton.Web.Configuration;
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
            services.Configure<DefaultConfigurationValues>(Configuration.GetSection("DefaultConfigurationValues"));
        }

        protected override void RegisterDependencies(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterAssemblyModules(typeof(DataAccessInstaller).GetTypeInfo().Assembly);
        }
    }
}