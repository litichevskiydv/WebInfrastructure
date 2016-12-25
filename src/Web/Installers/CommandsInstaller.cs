namespace Web.Installers
{
    using System.Linq;
    using System.Reflection;
    using Autofac;
    using DataAccess.Commands;
    using JetBrains.Annotations;
    using Skeleton.CQRS.Abstractions.Commands;
    using Skeleton.CQRS.Implementations.Commands;
    using Skeleton.CQRS.Implementations.Commands.CommandsFactory;
    using Module = Autofac.Module;

    [UsedImplicitly]
    public class CommandsInstaller : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AutofacCommandsFactory>().As<ICommandsFactory>().SingleInstance();
            builder.RegisterType<CommandsDispatcher>().As<ICommandsDispatcher>().SingleInstance();

            var commandType = typeof(ICommand<>);
            var asyncCommandType = typeof(IAsyncCommand<>);
            var dataAccess = typeof(SetValueCommand).GetTypeInfo().Assembly;
            builder.RegisterAssemblyTypes(dataAccess)
                .Where(x => x.GetInterfaces()
                                .SingleOrDefault(i => i.GetGenericArguments().Length > 0
                                                      && (i.GetGenericTypeDefinition() == commandType || i.GetGenericTypeDefinition() == asyncCommandType))
                            != null)
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}