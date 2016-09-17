namespace Infrastructure.Integrations.WebApiClient
{
    using System.Text;
    using Domain.Models.WebApiExceptionsContract;

    public class ExceptionData : ExceptionDataBase<ExceptionData>
    {
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