namespace Skeleton.Web.Serialization.Protobuf.Formatters
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.Extensions.Logging;
    using ProtoBuf.Meta;

    public class ProtobufOutputFormatter : OutputFormatter
    {
        private readonly RuntimeTypeModel _runtimeTypeModel;
        private readonly ILogger<ProtobufOutputFormatter> _logger;

        public ProtobufOutputFormatter(
            ILogger<ProtobufOutputFormatter> logger, 
            Func<RuntimeTypeModel, RuntimeTypeModel> serializerConfigurator)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));
            if (serializerConfigurator == null)
                throw new ArgumentNullException(nameof(serializerConfigurator));

            _runtimeTypeModel = serializerConfigurator(TypeModel.Create());
            _logger = logger;

            SupportedMediaTypes.Add(MediaTypeHeaderValues.ApplicationProtobuf);
            SupportedMediaTypes.Add(MediaTypeHeaderValues.ApplicationProtobufSynonym);

        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var tcs = new TaskCompletionSource<object>();
            try
            {
                _runtimeTypeModel.Serialize(context.HttpContext.Response.Body, context.Object);
                tcs.SetResult(null);
            }
            catch (Exception exception)
            {
                _logger.LogDebug(exception, "Exception was occurred during serialization");
                tcs.SetException(exception);
            }
            return tcs.Task;
        }
    }
}