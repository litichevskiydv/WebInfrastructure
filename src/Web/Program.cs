namespace Web
{
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Hosting;
    using WebHostBuilderExtensions = Skeleton.Web.Hosting.WebHostBuilderExtensions;

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public static class Program
    {
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHostBuilderExtensions.CreateDefaultWebApiHostBuilder<Startup>(args);

        public static void Main(string[] args) =>
            CreateWebHostBuilder(args).Build().Run();
    }
}