namespace Web
{
    using System.IO;
    using Infrastructure.Web.Configuration;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Hosting;

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