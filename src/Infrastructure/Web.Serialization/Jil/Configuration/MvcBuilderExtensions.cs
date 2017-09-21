namespace Skeleton.Web.Serialization.Jil.Configuration
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Options;

    public static class MvcBuilderExtensions
    {
        public static IMvcBuilder WithJsonFormattersBasedOnJil(this IMvcBuilder builder)
        {
            if(builder == null)
                throw new ArgumentNullException(nameof(builder));

            builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, MvcJilMvcOptionsSetup>());
            return builder;
        }
    }
}