namespace Web.Installers
{
    using Autofac;
    using DataAccess.Repositories;
    using DataAccess.Repositories.Impl;
    using JetBrains.Annotations;

    [UsedImplicitly]
    public class DataAccessInstaller : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<InMemoryValuesRepository<string>>().As<IValuesRepository<string>>().SingleInstance();
        }
    }
}