namespace Skeleton.Web.Integration
{
    using System;
    using System.Net.Http.Headers;
    using System.Text;

    public static class HttpRequestHeadersExtensions
    {
        public static HttpRequestHeaders WithBearerToken(this HttpRequestHeaders headers, string token)
        {
            if(headers == null)
                throw new ArgumentNullException(nameof(headers));
            if(string.IsNullOrWhiteSpace(token))
                throw new ArgumentNullException(nameof(token));

            headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return headers;
        }

        public static HttpRequestHeaders WithBasicAuth(this HttpRequestHeaders headers, string login, string password)
        {
            if (headers == null)
                throw new ArgumentNullException(nameof(headers));
            if (string.IsNullOrWhiteSpace(login))
                throw new ArgumentNullException(nameof(login));
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException(nameof(password));

            var value = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{login}:{password}"));
            headers.Authorization = new AuthenticationHeaderValue("Basic", value);
            return headers;
        }
    }
}