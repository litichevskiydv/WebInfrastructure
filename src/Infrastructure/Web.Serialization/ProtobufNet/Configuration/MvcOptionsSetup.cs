namespace Skeleton.Web.Serialization.ProtobufNet.Configuration
{
    using System;
    using Formatters;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using ProtoBuf.Meta;

    public class MvcOptionsSetup : IConfigureOptions<MvcOptions>
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly Func<RuntimeTypeModel, RuntimeTypeModel> _userSerializerConfigurator;

        public MvcOptionsSetup(ILoggerFactory loggerFactory, IOptions<MvcProtobufOptions> protobufOptions)
        {
            if (loggerFactory == null)
                throw new ArgumentNullException(nameof(loggerFactory));
            if (protobufOptions == null)
                throw new ArgumentNullException(nameof(protobufOptions));

            _loggerFactory = loggerFactory;
            _userSerializerConfigurator = protobufOptions.Value.SerializerConfigurator;
        }

        public void Configure(MvcOptions options)
        {
            options.InputFormatters.Add(new ProtobufInputFormatter(_loggerFactory.CreateLogger<ProtobufInputFormatter>()));
            options.OutputFormatters.Add(new ProtobufOutputFormatter(_loggerFactory.CreateLogger<ProtobufOutputFormatter>()));
            options.FormatterMappings.SetMediaTypeMappingForFormat("protobuf", MediaTypeHeaderValues.ApplicationProtobuf);
        }
    }
}