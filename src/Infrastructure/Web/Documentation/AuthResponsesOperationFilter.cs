namespace Skeleton.Web.Documentation
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Swashbuckle.AspNetCore.Swagger;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public class AuthResponsesOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.ActionDescriptor.FilterDescriptors.Any(
                    x => x.Filter.GetType().IsAssignableFrom(typeof(AuthorizeFilter))
                )
            )
                operation.Responses.Add("401", new Response {Description = "Unauthorized"});
        }
    }
}