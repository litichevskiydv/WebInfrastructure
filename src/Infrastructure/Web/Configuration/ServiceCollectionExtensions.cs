namespace Skeleton.Web.Configuration
{
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection CaptureCommandLineArguments(this IServiceCollection serviceCollection, string[] args)
        {
            return serviceCollection.AddSingleton(new CommandLineArgumentsProvider(args));
        }
    }
}