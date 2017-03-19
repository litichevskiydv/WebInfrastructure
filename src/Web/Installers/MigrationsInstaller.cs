namespace Web.Installers
{
    using System.Reflection;
    using Autofac;
    using JetBrains.Annotations;
    using Migrations;
    using Skeleton.Migrations;
    using Skeleton.Migrations.Migrator;

    [UsedImplicitly]
    public class MigrationsInstaller : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
               .RegisterAssemblyTypes(typeof(Migration201703191215).GetTypeInfo().Assembly)
               .Where(x => x.IsAssignableTo<IMigration>())
               .AsImplementedInterfaces()
               .SingleInstance();
            builder.RegisterType<SqlServerMigrator>().As<IMigrator>().SingleInstance();
        }
    }
}