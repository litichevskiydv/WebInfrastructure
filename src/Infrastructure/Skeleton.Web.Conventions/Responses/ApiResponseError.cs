namespace Skeleton.Web.Conventions.Responses
{
    /// <summary>
    /// Contract for transmission single error what occurs during Web Api request processing
    /// </summary>
    public class ApiResponseError
    {
        /// <summary>
        /// Error code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Short error description
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Full error description
        /// </summary>
        public string Detail { get; set; }
    }
}