namespace Web
{
    using System.IO;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Skeleton.Web.Configuration;
    using Skeleton.Web.Logging;

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public static class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseIISIntegration()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseConfiguration(new ConfigurationBuilder().AddCommandLine(args).Build())
                .ConfigureAppConfiguration(
                    (context, builder) =>
                    {
                        var env = context.HostingEnvironment;

                        builder
                            .AddDefaultConfigs(env)
                            .AddNLogConfig($"NLog.{env.EnvironmentName}.config")
                            .AddCommandLine(args);
                    })
                .ConfigureLogging(
                    (context, builder) =>
                        builder
                            .AddConfiguration(context.Configuration.GetSection("Logging"))
                            .AddNLog()
                            .AddConsole())
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}