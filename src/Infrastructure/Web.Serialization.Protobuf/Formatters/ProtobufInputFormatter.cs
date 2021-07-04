namespace Skeleton.Web.Serialization.Protobuf.Formatters
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.Extensions.Logging;
    using ProtoBuf.Meta;

    public class ProtobufInputFormatter : InputFormatter
    {
        private readonly ILogger<ProtobufInputFormatter> _logger;
        private readonly RuntimeTypeModel _runtimeTypeModel;

        public ProtobufInputFormatter(
            ILogger<ProtobufInputFormatter> logger, 
            Func<RuntimeTypeModel, RuntimeTypeModel> serializerConfigurator)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
            if (serializerConfigurator == null)
                throw new ArgumentNullException(nameof(serializerConfigurator));

            _logger = logger;
            _runtimeTypeModel = serializerConfigurator(RuntimeTypeModel.Create());

            SupportedMediaTypes.Add(MediaTypeHeaderValues.ApplicationProtobuf);
            SupportedMediaTypes.Add(MediaTypeHeaderValues.ApplicationProtobufSynonym);
        }

        public override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            object model;

            try
            {
                model = _runtimeTypeModel.Deserialize(context.HttpContext.Request.Body, null, context.ModelType);
            }
            catch (Exception exception)
            {
                _logger.LogDebug(exception, "Exception was occurred during deserialization");
                return InputFormatterResult.FailureAsync();
            }

            return InputFormatterResult.SuccessAsync(model);
        }
    }
}