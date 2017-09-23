namespace Skeleton.Web.Serialization.Jil.Formatters
{
    using System;
    using System.IO;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.ExceptionServices;
    using System.Text;
    using System.Threading.Tasks;
    using global::Jil;
    using Microsoft.AspNetCore.Mvc.Formatters;

    public class JilOutputFormatter : TextOutputFormatter
    {
        private readonly Options _options;
        private readonly MethodInfo _serializeGenericDefinition;

        public JilOutputFormatter(Options options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            _options = options;

            Expression<Action<object, TextWriter, Options>> fakeSerializeCall =
                (data, writer, settings) => JSON.Serialize(data, writer, settings);
            _serializeGenericDefinition = ((MethodCallExpression)fakeSerializeCall.Body).Method.GetGenericMethodDefinition();

            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
            SupportedMediaTypes.Add(MediaTypeHeaderValues.ApplicationJson);
            SupportedMediaTypes.Add(MediaTypeHeaderValues.TextJson);
            SupportedMediaTypes.Add(MediaTypeHeaderValues.ApplicationAnyJsonSyntax);
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (selectedEncoding == null)
                throw new ArgumentNullException(nameof(selectedEncoding));

            var response = context.HttpContext.Response;
            using (var writer = context.WriterFactory(response.Body, selectedEncoding))
            {
                try
                {
                    _serializeGenericDefinition
                        .MakeGenericMethod(context.ObjectType)
                        .Invoke(null, new[] {context.Object, writer, _options});
                }
                catch (TargetInvocationException ex)
                {
                    ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                }
                await writer.FlushAsync();
            }
        }
    }
}