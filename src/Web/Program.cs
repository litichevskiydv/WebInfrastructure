namespace Web
{
    using JetBrains.Annotations;
    using Microsoft.Extensions.Hosting;
    using Skeleton.Web.Hosting;

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public static class Program
    {
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            HostBuilderExtensions.CreateDefaultWebApiHostBuilder<Startup>(args);

        public static void Main(string[] args) =>
            CreateHostBuilder(args).Build().Run();
    }
}