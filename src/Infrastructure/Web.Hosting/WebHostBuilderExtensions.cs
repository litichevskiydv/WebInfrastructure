namespace Skeleton.Web.Hosting
{
    using System.IO;
    using Autofac.Extensions.DependencyInjection;
    using Configuration;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Serilog;
    using Skeleton.Web.Logging.Serilog.Configuration;

    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder CreateDefaultWebApiHostBuilder<TStartup>(string[] args) where TStartup : WebApiBaseStartup
        {
            return new WebHostBuilder()
                .UseKestrel(x => x.AllowSynchronousIO = true)
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
                .UseStartup<TStartup>();
        }
    }
}