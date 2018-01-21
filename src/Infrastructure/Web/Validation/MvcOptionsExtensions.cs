namespace Skeleton.Web.Validation
{
    using Microsoft.AspNetCore.Mvc;

    public static class MvcOptionsExtensions
    {
        public static MvcOptions UseModelValidationFilter(this MvcOptions mvcOptions)
        {
            mvcOptions.Filters.Add<ModelValidationFilterAttribute>();
            return mvcOptions;
        }

        public static MvcOptions UseParametersValidationFilter(this MvcOptions mvcOptions)
        {
            mvcOptions.Filters.Add(new ParametersValidationFilterAttribute());
            return mvcOptions;
        }
    }
}