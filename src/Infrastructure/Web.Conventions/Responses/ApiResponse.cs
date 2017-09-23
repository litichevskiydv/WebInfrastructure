namespace Skeleton.Web.Conventions.Responses
{
    using System.Collections.Generic;

    /// <summary>
    /// Contract for WebApi response transmition
    /// </summary>
    /// <typeparam name="TData">The type of response data</typeparam>
    public class ApiResponse<TData>
    {
        /// <summary>
        /// Response data
        /// </summary>
        public TData Data { get; set; }

        /// <summary>
        /// Collection of request processing errors
        /// </summary>
        public IReadOnlyList<ApiResponseError> Errors { get; set; }

        /// <summary>
        /// Constructor for serialization
        /// </summary>
        public ApiResponse()
        {
        }

        /// <summary>
        /// Constructor for WebApi response
        /// </summary>
        /// <param name="data">Response data</param>
        /// <param name="errors">Request processing errors collection</param>
        internal ApiResponse(TData data, IReadOnlyList<ApiResponseError> errors) : this()
        {
            Data = data;
            Errors = errors;
        }
    }

    /// <summary>
    /// Helper for WebApi response creation
    /// </summary>
    public static class ApiResponse
    {
        /// <summary>
        /// Creates container for WebApi response with filled fields
        /// </summary>
        /// <typeparam name="TData">Type of respones data</typeparam>
        /// <param name="data">Respones data</param>
        /// <param name="errors">Errors that happened during request processing</param>
        public static ApiResponse<TData> Create<TData>(TData data, IReadOnlyList<ApiResponseError> errors)
        {
            return new ApiResponse<TData>(data, errors);
        }

        /// <summary>
        /// Creates container for WebApi response without errors list
        /// </summary>
        /// <typeparam name="TData">Type of respones data</typeparam>
        /// <param name="data">Respones data</param>
        public static ApiResponse<TData> Success<TData>(TData data)
        {
            return new ApiResponse<TData>(data, null);
        }

        /// <summary>
        /// Creates container for WebApi response with only errors list
        /// </summary>
        /// <param name="errors">Errors that happened during request processing</param>
        public static ApiResponse<object> Error(IReadOnlyList<ApiResponseError> errors)
        {
            return new ApiResponse<object>(null, errors);
        }
    }
}