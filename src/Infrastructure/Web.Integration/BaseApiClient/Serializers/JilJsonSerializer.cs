namespace Skeleton.Web.Integration.BaseApiClient.Serializers
{
    using System;
    using System.IO;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.ExceptionServices;
    using Flurl.Http.Configuration;
    using Jil;

    public class JilJsonSerializer : ISerializer
    {
        private readonly Options _options;
        private readonly MethodInfo _serializeGenericDefinition;

        public JilJsonSerializer(Options options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            _options = options;

            Expression<Func<object, Options, string>> fakeSerializeCall =
                (data, settings) => JSON.Serialize(data, settings);
            _serializeGenericDefinition = ((MethodCallExpression)fakeSerializeCall.Body).Method.GetGenericMethodDefinition();
        }

        public string Serialize(object obj)
        {
            if (obj == null)
                return "null";

            try
            {
                return (string) _serializeGenericDefinition
                    .MakeGenericMethod(obj.GetType())
                    .Invoke(null, new[] {obj, _options});
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
            }

            return default(string);
        }

        public T Deserialize<T>(string s)
        {
            return JSON.Deserialize<T>(s, _options);
        }

        public T Deserialize<T>(Stream stream)
        {
            using (var reader = new StreamReader(stream))
                return JSON.Deserialize<T>(reader, _options);
        }
    }
}