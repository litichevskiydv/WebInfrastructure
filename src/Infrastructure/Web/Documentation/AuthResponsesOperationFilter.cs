namespace Skeleton.Web.Documentation
{
    using System.Linq;
    using Microsoft.AspNetCore.Authorization;
    using Swashbuckle.Swagger.Model;
    using Swashbuckle.SwaggerGen.Generator;

    public class AuthResponsesOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var authAttributes = context.ApiDescription
                .GetControllerAttributes()
                .Union(context.ApiDescription.GetActionAttributes())
                .OfType<AuthorizeAttribute>();

            if (authAttributes.Any())
                operation.Responses.Add("401", new Response { Description = "Unauthorized" });
        }
    }
}