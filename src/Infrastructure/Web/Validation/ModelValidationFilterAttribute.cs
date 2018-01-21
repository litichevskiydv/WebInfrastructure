namespace Skeleton.Web.Validation
{
    using System.Linq;
    using Conventions.Responses;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class ModelValidationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
            {
                base.OnActionExecuting(context);
                return;
            }

            var modelErrors = context.ModelState.Values
                .SelectMany(x => x.Errors)
                .Select(x => new ApiResponseError {Title = x.ErrorMessage, Detail = x.Exception?.Message})
                .ToArray();
            context.Result = new BadRequestObjectResult(ApiResponse.Error(modelErrors));
        }
    }
}