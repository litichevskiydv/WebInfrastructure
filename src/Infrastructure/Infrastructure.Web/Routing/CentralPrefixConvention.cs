namespace Infrastructure.Web.Routing
{
    using Microsoft.AspNetCore.Mvc.ApplicationModels;
    using Microsoft.AspNetCore.Mvc.Routing;

    public class CentralPrefixConvention : IApplicationModelConvention
    {
        private readonly AttributeRouteModel _centralRoutePrefix;

        public CentralPrefixConvention(IRouteTemplateProvider routeTemplateProvider)
        {
            _centralRoutePrefix = new AttributeRouteModel(routeTemplateProvider);
        }

        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                foreach (var selector in controller.Selectors)
                {
                    if (selector.AttributeRouteModel != null)
                        selector.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(selector.AttributeRouteModel,
                            _centralRoutePrefix);
                    else
                        selector.AttributeRouteModel = _centralRoutePrefix;
                }
            }
        }
    }
}