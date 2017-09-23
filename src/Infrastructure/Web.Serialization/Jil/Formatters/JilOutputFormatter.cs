namespace Skeleton.Web.Serialization.Jil.Formatters
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using global::Jil;
    using Microsoft.AspNetCore.Mvc.Formatters;

    public class JilOutputFormatter : TextOutputFormatter
    {
        private readonly Options _options;

        public JilOutputFormatter(Options options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            _options = options;

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
                JSON.Serialize(context.Object, writer, _options);
                await writer.FlushAsync();
            }
        }
    }
}