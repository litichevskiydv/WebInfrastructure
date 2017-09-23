namespace Skeleton.Web.Serialization.Jil.Formatters
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using global::Jil;
    using Microsoft.AspNetCore.Mvc.Formatters;
    using Microsoft.Extensions.Logging;

    public class JilInputFormatter : TextInputFormatter
    {
        private readonly ILogger<JilInputFormatter> _logger;
        private readonly Options _options;

        public JilInputFormatter(ILogger<JilInputFormatter> logger, Options options)
        {
            if(logger == null)
                throw new ArgumentNullException(nameof(logger));
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            _logger = logger;
            _options = options;

            SupportedEncodings.Add(UTF8EncodingWithoutBOM);
            SupportedEncodings.Add(UTF16EncodingLittleEndian);
            SupportedMediaTypes.Add(MediaTypeHeaderValues.ApplicationJson);
            SupportedMediaTypes.Add(MediaTypeHeaderValues.TextJson);
            SupportedMediaTypes.Add(MediaTypeHeaderValues.ApplicationAnyJsonSyntax);
        }

        public override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (encoding == null)
                throw new ArgumentNullException(nameof(encoding));

            var request = context.HttpContext.Request;
            using (var reader = context.ReaderFactory(request.Body, encoding))
            {
                object model;

                try
                {
                    model = JSON.Deserialize(reader, context.ModelType, _options);
                }
                catch (DeserializationException exception)
                {
                    _logger.LogDebug(exception, "Exception was occurred during deserialization");
                    return InputFormatterResult.FailureAsync();
                }

                return InputFormatterResult.SuccessAsync(model);
            }
        }
    }
}