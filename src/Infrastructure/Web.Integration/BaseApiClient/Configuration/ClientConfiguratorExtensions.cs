namespace Skeleton.Web.Integration.BaseApiClient.Configuration
{
    using System;
    using Flurl.Http.Configuration;
    using Jil;
    using Newtonsoft.Json;
    using Serializers;

    public static class ClientConfiguratorExtensions
    {
        public static IClientConfigurator WithJsonNetSerializer(
            this IClientConfigurator configurator,
            JsonSerializerSettings serializerSettings)
        {
            if(configurator == null)
                throw new ArgumentNullException(nameof(configurator));
            if(serializerSettings == null)
                throw new ArgumentNullException(nameof(serializerSettings));

            return configurator.WithSerializer(new NewtonsoftJsonSerializer(serializerSettings));
        }

        public static IClientConfigurator WithJilSerializer(
            this IClientConfigurator configurator,
            Options serializerSettings)
        {
            if (configurator == null)
                throw new ArgumentNullException(nameof(configurator));
            if (serializerSettings == null)
                throw new ArgumentNullException(nameof(serializerSettings));

            return configurator.WithSerializer(new JilJsonSerializer(serializerSettings));
        }
    }
}