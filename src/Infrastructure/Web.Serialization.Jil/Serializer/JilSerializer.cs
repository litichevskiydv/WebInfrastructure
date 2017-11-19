namespace Skeleton.Web.Serialization.Jil.Serializer
{
    using System;
    using System.IO;
    using System.Linq.Expressions;
    using System.Net.Http;
    using System.Reflection;
    using System.Runtime.ExceptionServices;
    using System.Text;
    using Abstractions;
    using global::Jil;
    using Microsoft.Net.Http.Headers;

    public class JilSerializer : ISerializer
    {
        private readonly Options _options;
        private readonly MethodInfo _serializeGenericDefinition;

        public JilSerializer(Options options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            _options = options;

            Expression<Func<object, Options, string>> fakeSerializeCall =
                (data, settings) => JSON.Serialize(data, settings);
            _serializeGenericDefinition = ((MethodCallExpression)fakeSerializeCall.Body).Method.GetGenericMethodDefinition();
        }

        private string SerializeInternal(object obj)
        {
            if (obj == null)
                return "null";

            try
            {
                return (string)_serializeGenericDefinition
                    .MakeGenericMethod(obj.GetType())
                    .Invoke(null, new[] { obj, _options });
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
            }

            return default(string);
        }

        public HttpContent Serialize(object obj)
        {
            return new StringContent(SerializeInternal(obj), Encoding.UTF8, MediaTypeHeaderValues.ApplicationJson.MediaType.ToString());
        }

        public T Deserialize<T>(Stream stream)
        {
            using (var reader = new StreamReader(stream))
                return JSON.Deserialize<T>(reader, _options);
        }

        public MediaTypeHeaderValue MediaType => MediaTypeHeaderValues.ApplicationJson;
    }
}