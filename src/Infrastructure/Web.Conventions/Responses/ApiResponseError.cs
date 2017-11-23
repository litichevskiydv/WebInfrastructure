namespace Skeleton.Web.Conventions.Responses
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Contract for transmission single error what occurs during Web Api request processing
    /// </summary>
    [DataContract]
    public class ApiResponseError
    {
        /// <summary>
        /// Error code
        /// </summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false, Order = 1)]
        public string Code { get; set; }

        /// <summary>
        /// Short error description
        /// </summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false, Order = 2)]
        public string Title { get; set; }

        /// <summary>
        /// Full error description
        /// </summary>
        [DataMember(EmitDefaultValue = false, IsRequired = false, Order = 3)]
        public string Detail { get; set; }
    }
}