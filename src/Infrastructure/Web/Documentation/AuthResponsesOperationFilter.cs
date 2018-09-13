namespace Skeleton.Web.Documentation
{
    using System.Linq;
    using Microsoft.AspNetCore.Authorization;
    using Swashbuckle.AspNetCore.Swagger;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public class AuthResponsesOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var authAttributes = context.ControllerActionDescriptor
                .GetControllerAndActionAttributes(true)
                .OfType<AuthorizeAttribute>();

            if (authAttributes.Any())
                operation.Responses.Add("401", new Response { Description = "Unauthorized" });
        }
    }
}