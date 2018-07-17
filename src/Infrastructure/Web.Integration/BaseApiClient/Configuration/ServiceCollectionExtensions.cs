namespace Skeleton.Web.Integration.BaseApiClient.Configuration
{
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IHttpClientBuilder AddClient<TImplementation>(this IServiceCollection services)
            where TImplementation : BaseClient
        {
            return services.AddHttpClient<TImplementation>();
        }

        public static IHttpClientBuilder AddClient<TContract, TImplementation>(this IServiceCollection services)
            where TContract : class
            where TImplementation : BaseClient, TContract
        {
            return services.AddHttpClient<TContract, TImplementation>();
        }
    }
}