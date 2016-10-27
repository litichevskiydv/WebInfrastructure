namespace Skeleton.Integrations.WebApiClient
{
    using System.Text;
    using Web.Conventions.Responses;

    public class ApiErrorResponse : ExceptionData, IApiErrorResponse<ExceptionData>
    {
        public string Message { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder();

            if (string.IsNullOrWhiteSpace(Message) == false)
                builder.AppendLine(Message);

            var baseInformation = base.ToString();
            if (string.IsNullOrWhiteSpace(baseInformation) == false)
                builder.AppendLine(baseInformation);

            return builder.ToString();
        }
    }
}