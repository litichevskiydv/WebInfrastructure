namespace Skeleton.Web.ExceptionsHandling
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUnhandledExceptionsStartupFilter(this IServiceCollection services)
        {
            return services.AddTransient<IStartupFilter, UnhandledExceptionsStartupFilter>();
        }
    }
}