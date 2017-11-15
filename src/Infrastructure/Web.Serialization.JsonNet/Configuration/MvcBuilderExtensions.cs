namespace Skeleton.Web.Serialization.JsonNet.Configuration
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;

    public static class MvcBuilderExtensions
    {
        public static IMvcBuilder WithJsonFormattersBasedOnJsonNet(
            this IMvcBuilder builder, 
            Action<JsonSerializerSettings> settingsConfigurator)
        {
            if(builder == null)
                throw new ArgumentNullException(nameof(builder));
            if (settingsConfigurator == null)
                throw new ArgumentNullException(nameof(settingsConfigurator));

            builder.AddJsonOptions(x => settingsConfigurator(x.SerializerSettings));
            return builder;
        }
    }
}