namespace Web.Installers
{
    using Application.Services;
    using Autofac;
    using JetBrains.Annotations;
    using Skeleton.Web.Authentication.JwtBearer;
    using Skeleton.Web.Authentication.JwtBearer.UserClaimsProvider;

    [UsedImplicitly]
    public class ServicesInstaller : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserClaimsProvider>().As<IUserClaimsProvider>().SingleInstance();
            builder.RegisterType<TokenIssueEventHandler>().As<ITokenIssueEventHandler>().SingleInstance();
        }
    }
}