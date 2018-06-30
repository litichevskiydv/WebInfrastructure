namespace Web
{
    using System.IO;
    using Autofac.Extensions.DependencyInjection;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Serilog;
    using Skeleton.Web.Configuration;
    using Skeleton.Web.Logging.Serilog.Configuration;

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public static class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .ConfigureServices(services => services.AddAutofac())
                .UseIISIntegration()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseConfiguration(new ConfigurationBuilder().AddCommandLine(args).Build())
                .ConfigureAppConfiguration(
                    (context, builder) =>
                    {
                        var env = context.HostingEnvironment;

                        builder
                            .AddDefaultConfigs(env)
                            .AddCommandLine(args);
                    }
                )
                .UseSerilog((context, configuration) => configuration.UseDefaultSettings(context.Configuration))
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}