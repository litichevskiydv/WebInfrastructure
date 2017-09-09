namespace Skeleton.Web.Integration.BaseApiClient
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public static class HttpRequestHeadersExtensions
    {
        public static IDictionary<string, object> WithBearerToken(this IDictionary<string, object> headers, string token)
        {
            if(headers == null)
                throw new ArgumentNullException(nameof(headers));
            if(string.IsNullOrWhiteSpace(token))
                throw new ArgumentNullException(nameof(token));

            headers["Authorization"] = $"Bearer {token}";
            return headers;
        }

        public static IDictionary<string, object> WithBasicAuth(this IDictionary<string, object> headers, string login, string password)
        {
            if (headers == null)
                throw new ArgumentNullException(nameof(headers));
            if (string.IsNullOrWhiteSpace(login))
                throw new ArgumentNullException(nameof(login));
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException(nameof(password));

            headers["Authorization"] = $"Basic {Convert.ToBase64String(Encoding.UTF8.GetBytes($"{login}:{password}"))}";
            return headers;
        }
    }
}