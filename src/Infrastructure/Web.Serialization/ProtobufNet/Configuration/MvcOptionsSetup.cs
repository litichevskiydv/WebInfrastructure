namespace Skeleton.Web.Serialization.ProtobufNet.Configuration
{
    using System;
    using Formatters;
    using Formatters.Surrogates;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using ProtoBuf.Meta;

    public class MvcOptionsSetup : IConfigureOptions<MvcOptions>
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly Func<RuntimeTypeModel, RuntimeTypeModel> _serializerConfigurator;

        public MvcOptionsSetup(ILoggerFactory loggerFactory, IOptions<MvcProtobufOptions> protobufOptions)
        {
            if (loggerFactory == null)
                throw new ArgumentNullException(nameof(loggerFactory));
            if (protobufOptions == null)
                throw new ArgumentNullException(nameof(protobufOptions));

            _loggerFactory = loggerFactory;
            _serializerConfigurator =
                x =>
                    protobufOptions.Value.SerializerConfigurator(x)
                        .WithDefaultValuesHandling(false)
                        .WithTypeSurrogate<DateTimeOffset, DateTimeOffsetSurrogate>();
        }

        public void Configure(MvcOptions options)
        {
            options.InputFormatters.Add(
                new ProtobufInputFormatter(_loggerFactory.CreateLogger<ProtobufInputFormatter>(), _serializerConfigurator)
            );
            options.OutputFormatters.Add(
                new ProtobufOutputFormatter(_loggerFactory.CreateLogger<ProtobufOutputFormatter>(), _serializerConfigurator)
            );
            options.FormatterMappings.SetMediaTypeMappingForFormat("protobuf", MediaTypeHeaderValues.ApplicationProtobuf);
        }
    }
}