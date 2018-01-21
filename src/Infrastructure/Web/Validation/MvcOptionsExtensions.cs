namespace Skeleton.Web.Validation
{
    using Microsoft.AspNetCore.Mvc;

    public static class MvcOptionsExtensions
    {
        public static MvcOptions UseModelValidationFilter(this MvcOptions mvcOptions)
        {
            mvcOptions.Filters.Add(typeof(ModelValidationFilterAttribute));
            return mvcOptions;
        }
    }
}