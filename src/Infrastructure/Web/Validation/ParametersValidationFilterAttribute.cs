namespace Skeleton.Web.Validation
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;
    using Common.Extensions;
    using Conventions.Responses;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class ParametersValidationFilterAttribute : ActionFilterAttribute
    {
        private readonly ConcurrentDictionary<MethodInfo, (string ParameterName, ValidationAttribute[] ValidationAttributes)[]> _actionsParameters;

        public ParametersValidationFilterAttribute()
        {
            _actionsParameters = new ConcurrentDictionary<MethodInfo, (string ParameterName, ValidationAttribute[] ValidationAttributes)[]>();
        }

        private static (string ParameterName, ValidationAttribute[] ValidationAttributes)[]
            GetValidatingParameters(MethodInfo methodInfo)
        {
            return methodInfo
                .GetParameters()
                .Select(x => (ParameterName: x.Name, ValidationAttributes: x.GetCustomAttributes<ValidationAttribute>(true).AsArray()))
                .Where(x => x.ValidationAttributes.IsNotEmpty())
                .ToArray();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (actionDescriptor == null)
            {
                base.OnActionExecuting(context);
                return;
            }

            var parametersWithValidationAttributes = _actionsParameters.GetOrAdd(actionDescriptor.MethodInfo, GetValidatingParameters);
            if (parametersWithValidationAttributes.IsEmpty())
            {
                base.OnActionExecuting(context);
                return;
            }

            var errors = new List<ApiResponseError>();
            foreach (var (parameterName, validationAttributes) in parametersWithValidationAttributes)
            {
                context.ActionArguments.TryGetValue(parameterName, out var parameterValue);
                errors.AddRange(
                    validationAttributes
                        .Where(x => x.IsValid(parameterValue) == false)
                        .Select(x => new ApiResponseError {Title = x.FormatErrorMessage(parameterName)})
                );
            }

            if (errors.IsEmpty())
                base.OnActionExecuting(context);
            else
                context.Result = new BadRequestObjectResult(ApiResponse.Error(errors));
        }
    }
}