namespace Skeleton.Web.Conventions.Responses
{
    using System.Runtime.Serialization;
    using System.Text;

    /// <summary>
    /// Contract for WebApi response transmition
    /// </summary>
    /// <typeparam name="TData">Type of response data</typeparam>
    /// <typeparam name="TError">Type of the response error</typeparam>
    [DataContract]
    public class ApiResponse<TData, TError>
    {
        /// <summary>
        /// Response data
        /// </summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false, Order = 1)]
        public TData Data { get; set; }

        /// <summary>
        /// Collection of request processing errors
        /// </summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false, Order = 2)]
        public TError[] Errors { get; set; }

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
        internal ApiResponse(TData data, TError[] errors) : this()
        {
            Data = data;
            Errors = errors;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            if (Data != null)
            {
                builder.AppendLine("Data:");
                builder.AppendLine($"\t{Data.ToString()}");
            }

            if (Errors != null)
            {
                builder.AppendLine("Errors:");
                foreach (var error in Errors)
                    builder.AppendLine($"\t{error.ToString()}");
            }

            return builder.ToString();
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
        /// <typeparam name="TData">Type of the respone data</typeparam>
        /// <typeparam name="TError">Type of the response error </typeparam>
        /// <param name="data">Respones data</param>
        /// <param name="errors">Errors that happened during request processing</param>
        public static ApiResponse<TData, TError> Create<TData, TError>(TData data, TError[] errors)
        {
            return new ApiResponse<TData, TError>(data, errors);
        }

        /// <summary>
        /// Creates container for WebApi response without errors list
        /// </summary>
        /// <typeparam name="TData">Type of respones data</typeparam>
        /// <param name="data">Respones data</param>
        public static ApiResponse<TData, object> Success<TData>(TData data)
        {
            return new ApiResponse<TData, object>(data, null);
        }

        /// <summary>
        /// Creates container for WebApi response with only errors list
        /// </summary>
        /// <typeparam name="TError">Type of respones error</typeparam>
        /// <param name="errors">Errors that happened during request processing</param>
        public static ApiResponse<object, TError> Error<TError>(TError[] errors)
        {
            return new ApiResponse<object, TError>(null, errors);
        }
    }
}