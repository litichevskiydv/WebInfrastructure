namespace Web.Installers
{
    using Autofac;
    using DataAccess.Repositories;
    using DataAccess.Repositories.Impl;
    using JetBrains.Annotations;
    using Skeleton.Dapper.ConnectionsFactory;
    using Skeleton.Dapper.SessionsFactory;

    [UsedImplicitly]
    public class DataAccessInstaller : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SqlConnectionsFactory>().As<IConnectionsFactory>().SingleInstance();
            builder.RegisterType<SessionsFactory>().As<ISessionsFactory>().SingleInstance();
            builder.RegisterType<InMemoryValuesRepository<string>>().As<IValuesRepository<string>>().SingleInstance();
        }
    }
}