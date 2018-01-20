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
            if(context.ModelState.IsValid)
                return;

            var modelErrors = context.ModelState.Values
                .SelectMany(x => x.Errors)
                .Select(x =>
                            new ApiResponseError
                            {
                                Detail = string.IsNullOrWhiteSpace(x.Exception?.Message) == false
                                    ? x.Exception.Message
                                    : x.ErrorMessage
                            }
                )
                .ToArray();
            context.Result = new BadRequestObjectResult(ApiResponse.Error(modelErrors));
        }
    }
}