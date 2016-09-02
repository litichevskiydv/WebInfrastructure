namespace Infrastructure.Web.Routing
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Routing;

    public static class MvcOptionsExtensions
    {
        public static MvcOptions UseCentralRoutePrefix(this MvcOptions mvcOptions, IRouteTemplateProvider routeTemplateProvider)
        {
            if(mvcOptions == null)
                throw new ArgumentNullException(nameof(mvcOptions));
            if(routeTemplateProvider == null)
                throw new ArgumentNullException(nameof(routeTemplateProvider));

            mvcOptions.Conventions.Insert(0, new CentralPrefixConvention(routeTemplateProvider));
            return mvcOptions;
        }

        public static MvcOptions UseCentralRoutePrefix(this MvcOptions mvcOptions, string routeTemplate)
        {
            return UseCentralRoutePrefix(mvcOptions, new RouteAttribute(routeTemplate));
        }
    }
}