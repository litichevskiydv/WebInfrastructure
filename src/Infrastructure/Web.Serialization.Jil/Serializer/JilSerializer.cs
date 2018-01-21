namespace Skeleton.Web.Serialization.Jil.Serializer
{
    using System;
    using System.IO;
    using System.Linq.Expressions;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Reflection;
    using System.Runtime.ExceptionServices;
    using System.Text;
    using Abstractions;
    using global::Jil;

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

            MediaType = MediaTypeHeaderValue.Parse("application/json; charset=utf-8");
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
            return new StringContent(
                SerializeInternal(obj),
                Encoding.GetEncoding(MediaType.CharSet),
                MediaType.MediaType
            );
        }

        public T Deserialize<T>(Stream stream)
        {
            using (var reader = new StreamReader(stream, Encoding.UTF8, true, 4096, true))
                return JSON.Deserialize<T>(reader, _options);
        }

        public MediaTypeHeaderValue MediaType { get; }
    }
}