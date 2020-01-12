namespace Skeleton.Web.Hosting
{
    using System.IO;
    using Autofac.Extensions.DependencyInjection;
    using Configuration;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Serilog;
    using Skeleton.Web.Logging.Serilog.Configuration;

    public static class HostBuilderExtensions
    {
        public static IHostBuilder CreateDefaultWebApiHostBuilder<TStartup>(string[] args) where TStartup : WebApiBaseStartup =>
            new HostBuilder()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureHostConfiguration(config => config.AddEnvironmentVariables("DOTNET_").AddCommandLine(args))
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
                .ConfigureWebHost(
                    webHostBuilder => webHostBuilder
                        .UseKestrel(x => x.AllowSynchronousIO = true)
                        .UseIISIntegration()
                        .UseStartup<TStartup>()
                );
    }
}