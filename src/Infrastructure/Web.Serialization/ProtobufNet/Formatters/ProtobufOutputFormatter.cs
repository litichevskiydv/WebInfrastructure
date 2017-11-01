namespace Skeleton.Web.Serialization.ProtobufNet.Formatters
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.Extensions.Logging;
    using ProtoBuf.Meta;
    using Surrogates;

    public class ProtobufOutputFormatter : OutputFormatter
    {
        private readonly RuntimeTypeModel _runtimeTypeModel;
        private readonly ILogger<ProtobufOutputFormatter> _logger;

        private static RuntimeTypeModel CreateRuntimeTypeModel()
        {
            var runtimeTypeModel = TypeModel.Create();

            runtimeTypeModel.UseImplicitZeroDefaults = false;
            runtimeTypeModel.Add(typeof(DateTimeOffset), false).SetSurrogate(typeof(DateTimeOffsetSurrogate));

            return runtimeTypeModel;
        }

        public ProtobufOutputFormatter(ILogger<ProtobufOutputFormatter> logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _runtimeTypeModel = CreateRuntimeTypeModel();
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