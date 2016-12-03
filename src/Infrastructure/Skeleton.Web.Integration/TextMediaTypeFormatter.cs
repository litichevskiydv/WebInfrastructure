namespace Skeleton.Web.Integration
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class TextMediaTypeFormatter : MediaTypeFormatter
    {
        public TextMediaTypeFormatter()
        {
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/plain"));
        }

        public override bool CanReadType(Type type)
        {
            return type == typeof(string);
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            var effectiveEncoding = SelectCharacterEncoding(content?.Headers);
            using (var reader = new StreamReader(readStream, effectiveEncoding))
            {
                var tcs = new TaskCompletionSource<object>();
                reader.ReadToEndAsync()
                    .ContinueWith(x =>
                                  {
                                      if (x.IsFaulted)
                                          tcs.TrySetException(x.Exception.InnerExceptions);
                                      else if (x.IsCanceled)
                                          tcs.TrySetCanceled();
                                      else
                                          tcs.TrySetResult(x.Result);
                                  }, TaskContinuationOptions.ExecuteSynchronously);
                return tcs.Task;
            }
        }

        public override bool CanWriteType(Type type)
        {
            return type == typeof(string);
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext,
            CancellationToken cancellationToken)
        {
            var text = (string) value;
            if (string.IsNullOrEmpty(text))
                return Task.CompletedTask;

            if (cancellationToken.IsCancellationRequested)
                return new Task(() => { }, cancellationToken);

            var effectiveEncoding = SelectCharacterEncoding(content?.Headers);
            using (var writer = new StreamWriter(writeStream, effectiveEncoding))
                return writer.WriteAsync(text);
        }
    }
}