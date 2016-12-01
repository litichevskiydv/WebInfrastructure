namespace Web.Installers
{
    using System.Linq;
    using System.Reflection;
    using Autofac;
    using DataAccess.Queries;
    using JetBrains.Annotations;
    using Skeleton.CQRS.Abstractions.Queries;
    using Skeleton.CQRS.Implementations.Queries;
    using Skeleton.CQRS.Implementations.Queries.QueriesFactory;
    using Module = Autofac.Module;

    [UsedImplicitly]
    public class QueriesInstaller : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AutofacQueriesFactory>().As<IQueriesFactory>().SingleInstance();
            builder.RegisterType<QueriesDispatcher>().As<IQueriesDispatcher>().SingleInstance();

            var commandType = typeof(IQuery<,>);
            var dataAccess = typeof(GetValueQuery).GetTypeInfo().Assembly;
            builder.RegisterAssemblyTypes(dataAccess)
                .Where(x => x.GetInterfaces()
                                .SingleOrDefault(i => i.GetGenericArguments().Length > 0 && i.GetGenericTypeDefinition() == commandType) != null)
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}