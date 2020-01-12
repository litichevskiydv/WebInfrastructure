namespace Skeleton.Web.Testing
{
    using System;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection;

    public class CustomAutofacServiceProviderFactory : IServiceProviderFactory<ContainerBuilder>
    {
        private readonly Action<ContainerBuilder> _overrideRegisteredDependencies;

        public CustomAutofacServiceProviderFactory(Action<ContainerBuilder> overrideRegisteredDependencies)
        {
            if (overrideRegisteredDependencies == null)
                throw new ArgumentNullException(nameof(overrideRegisteredDependencies));

            _overrideRegisteredDependencies = overrideRegisteredDependencies;
        }

        public ContainerBuilder CreateBuilder(IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            var builder = new ContainerBuilder();
            builder.Populate(services);
            return builder;
        }

        public IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder)
        {
            if (containerBuilder == null)
                throw new ArgumentNullException(nameof(containerBuilder));

            _overrideRegisteredDependencies(containerBuilder);
            var container = containerBuilder.Build();
            return new AutofacServiceProvider(container);
        }
    }
}