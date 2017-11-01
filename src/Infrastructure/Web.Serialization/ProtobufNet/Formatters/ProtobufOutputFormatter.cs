namespace Skeleton.Web.Serialization.ProtobufNet.Formatters
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using ProtoBuf.Meta;
    using Surrogates;

    public class ProtobufOutputFormatter : OutputFormatter
    {
        private readonly RuntimeTypeModel _runtimeTypeModel;

        private static RuntimeTypeModel CreateRuntimeTypeModel()
        {
            var runtimeTypeModel = TypeModel.Create();

            runtimeTypeModel.UseImplicitZeroDefaults = false;
            runtimeTypeModel.Add(typeof(DateTimeOffset), false).SetSurrogate(typeof(DateTimeOffsetSurrogate));

            return runtimeTypeModel;
        }

        public ProtobufOutputFormatter()
        {
            _runtimeTypeModel = CreateRuntimeTypeModel();

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
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
            return tcs.Task;
        }
    }
}