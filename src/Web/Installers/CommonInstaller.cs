namespace Web.Installers
{
    using Application.Services;
    using Application.Services.Impl;
    using Autofac;
    using JetBrains.Annotations;

    [UsedImplicitly]
    public class CommonInstaller : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SimpleValuesProvider>().As<IValuesProvider>().SingleInstance();
        }
    }
}