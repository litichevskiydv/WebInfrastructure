namespace Skeleton.Web.Logging.Abstractions.ContextsCapturing.CapturedData
{
    using System.Collections.Generic;

    internal class CapturedActionExecutingContext
    {
        public string Method { get; set; }

        public string Url { get; set; }

        public IDictionary<string, string> Headers { get; set; }

        public IDictionary<string, object> ActionArguments { get; set; }
    }
}