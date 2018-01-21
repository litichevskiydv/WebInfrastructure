namespace Skeleton.Web.Conventions.Responses
{
    using System.Runtime.Serialization;
    using System.Text;

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

        public override string ToString()
        {
            var builder = new StringBuilder();

            if (string.IsNullOrWhiteSpace(Code) == false)
                builder.Append($"{nameof(Code)}: {Code}; ");
            if (string.IsNullOrWhiteSpace(Title) == false)
                builder.Append($"{nameof(Title)}: {Title}; ");
            if (string.IsNullOrWhiteSpace(Detail) == false)
                builder.Append($"{nameof(Detail)}: {Detail}; ");

            return builder.ToString();
        }
    }
}