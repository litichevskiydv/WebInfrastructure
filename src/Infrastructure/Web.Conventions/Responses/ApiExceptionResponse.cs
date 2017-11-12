namespace Skeleton.Web.Conventions.Responses
{
    using System;
    using System.Runtime.Serialization;
    using System.Text;

    [DataContract]
    public class ApiExceptionResponse
    {
        [DataMember(EmitDefaultValue = false, IsRequired = false, Order = 1)]
        public string Message { get; set; }

        [DataMember(EmitDefaultValue = false, IsRequired = false, Order = 2)]
        public ExceptionDescription ExceptionDescription { get; set; }

        public ApiExceptionResponse()
        {
        }

        public ApiExceptionResponse(string message, Exception exception) : this()
        {
            Message = message;
            if(exception != null)
                ExceptionDescription = new ExceptionDescription(exception);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            if (string.IsNullOrWhiteSpace(Message) == false)
                builder.AppendLine(Message);

            var exceptionDescriptionString = ExceptionDescription?.ToString();
            if (string.IsNullOrWhiteSpace(exceptionDescriptionString) == false)
                builder.AppendLine(exceptionDescriptionString);

            return builder.ToString();
        }
    }
}