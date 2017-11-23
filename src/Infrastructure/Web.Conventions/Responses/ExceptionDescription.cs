namespace Skeleton.Web.Conventions.Responses
{
    using System;
    using System.Runtime.Serialization;
    using System.Text;

    [DataContract]
    public class ExceptionDescription
    {
        [DataMember(EmitDefaultValue = false, IsRequired = false, Order = 1)]
        public string Type { get; set; }

        [DataMember(EmitDefaultValue = false, IsRequired = false, Order = 2)]
        public string Message { get; set; }

        [DataMember(EmitDefaultValue = false, IsRequired = false, Order = 3)]
        public string StackTrace { get; set; }

        [DataMember(EmitDefaultValue = false, IsRequired = false, Order = 4)]
        public ExceptionDescription InnerException { get; set; }

        public ExceptionDescription()
        {
        }

        public ExceptionDescription(Exception exception) : this()
        {
            Type = exception.GetType().ToString();
            Message = exception.Message;
            StackTrace = exception.StackTrace;

            if (exception.InnerException != null)
                InnerException = new ExceptionDescription(exception.InnerException);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            var firstLine = $"{Type ?? ""} {Message ?? ""}".Trim();
            if (string.IsNullOrWhiteSpace(firstLine) == false)
                builder.AppendLine(firstLine);

            if (InnerException != null)
            {
                builder.AppendLine("InnerException:");
                builder.AppendLine(InnerException.ToString());
            }

            if (string.IsNullOrWhiteSpace(StackTrace) == false)
                builder.AppendLine(StackTrace);

            return builder.ToString();
        }
    }
}