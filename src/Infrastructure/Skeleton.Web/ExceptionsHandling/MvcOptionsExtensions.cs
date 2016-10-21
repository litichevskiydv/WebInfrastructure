namespace Skeleton.Web.ExceptionsHandling
{
    using Microsoft.AspNetCore.Mvc;

    public static class MvcOptionsExtensions
    {
        public static MvcOptions UseUnhandledExceptionFilter(this MvcOptions mvcOptions)
        {
            mvcOptions.Filters.Add(typeof(UnhandledExceptionsFilterAttribute));
            return mvcOptions;
        }
    }
}