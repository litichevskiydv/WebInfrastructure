namespace Skeleton.Web.Serialization.ProtobufNet.Formatters
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using ProtoBuf.Meta;

    public class ProtobufOutputFormatter : OutputFormatter
    {
        private readonly RuntimeTypeModel _runtimeTypeModel;

        private static RuntimeTypeModel CreateRuntimeTypeModel()
        {
            var runtimeTypeModel = TypeModel.Create();
            runtimeTypeModel.UseImplicitZeroDefaults = false;

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

            var response = context.HttpContext.Response;
            _runtimeTypeModel.Serialize(response.Body, context.Object);

            return Task.FromResult(0);
        }
    }
}