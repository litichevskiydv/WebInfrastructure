namespace Skeleton.Web.Serialization.ProtobufNet.Formatters
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.Extensions.Logging;
    using ProtoBuf.Meta;

    public class ProtobufInputFormatter : InputFormatter
    {
        private readonly RuntimeTypeModel _runtimeTypeModel;
        private readonly ILogger<ProtobufInputFormatter> _logger;

        private static RuntimeTypeModel CreateRuntimeTypeModel()
        {
            var runtimeTypeModel = TypeModel.Create();
            runtimeTypeModel.UseImplicitZeroDefaults = false;

            return runtimeTypeModel;
        }

        public ProtobufInputFormatter(ILogger<ProtobufInputFormatter> logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _runtimeTypeModel = CreateRuntimeTypeModel();
            _logger = logger;

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