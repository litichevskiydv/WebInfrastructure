namespace Skeleton.Web.Integration
{
    using System.Text;
    using Conventions.Responses;

    public class ExceptionData : IExceptionData<ExceptionData>
    {
        public string ExceptionType { get; set; }
        public string ExceptionMessage { get; set; }
        public string StackTrace { get; set; }
        public ExceptionData InnerException { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder();

            var firstLine = $"{ExceptionType ?? ""} {ExceptionMessage ?? ""}".Trim();
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