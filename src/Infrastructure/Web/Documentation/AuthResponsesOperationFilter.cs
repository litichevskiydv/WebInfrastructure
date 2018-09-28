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
            var authAttributes = context?.MethodInfo?.DeclaringType?.GetCustomAttributes(true)
                .Union(context.MethodInfo?.GetCustomAttributes(true))
                .OfType<AuthorizeAttribute>();

            if(authAttributes == null)
                return;
            
            if (authAttributes.Any())
                operation.Responses.Add("401", new Response { Description = "Unauthorized" });
        }
    }
}