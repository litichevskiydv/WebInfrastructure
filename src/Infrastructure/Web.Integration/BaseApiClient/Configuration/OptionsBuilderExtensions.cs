namespace Skeleton.Web.Integration.BaseApiClient.Configuration
{
    using System;
    using Microsoft.Extensions.Options;
    using Serialization.Abstractions;

    public static class OptionsBuilderExtensions
    {
        public static OptionsBuilder<TClientOptions> WithSerializer<TClientOptions>(
            this OptionsBuilder<TClientOptions> optionsBuilder,
            ISerializer serializer)
            where TClientOptions : BaseClientOptions
        {
            if(optionsBuilder == null)
                throw new ArgumentNullException(nameof(optionsBuilder));
            if (serializer == null)
                throw new ArgumentNullException(nameof(serializer));

            return optionsBuilder.Configure(x => x.Serializer = serializer);
        }
    }
}