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

    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class Startup : WebApiBaseStartup
    {
        public Startup(IHostingEnvironment env, CommandLineArgumentsProvider commandLineArgumentsProvider)
            : base(env, commandLineArgumentsProvider)
        {
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