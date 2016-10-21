namespace Web
{
    using System.IO;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Hosting;
    using Skeleton.Web.Configuration;

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public static class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseIISIntegration()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseConfigurationFromCommandLine(args)
                .ConfigureServices(services => services.CaptureCommandLineArguments(args))
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}