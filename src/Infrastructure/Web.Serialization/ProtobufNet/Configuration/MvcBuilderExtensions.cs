namespace Skeleton.Web.Serialization.ProtobufNet.Configuration
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Options;
    using ProtoBuf.Meta;

    public static class MvcBuilderExtensions
    {
        public static IMvcBuilder WithProtobufFormatters(
            this IMvcBuilder builder,
            Func<RuntimeTypeModel, RuntimeTypeModel> serializerConfigurator = null)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            builder.Services.AddSingleton(Options.Create(new MvcProtobufOptions {SerializerConfigurator = serializerConfigurator ?? (x => x)}));
            builder.Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<MvcOptions>, MvcOptionsSetup>());
            return builder;
        }
    }
}