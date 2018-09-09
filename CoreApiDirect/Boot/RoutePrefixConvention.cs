using System;
using System.Linq;
using CoreApiDirect.Base;
using CoreApiDirect.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace CoreApiDirect.Boot
{
    internal class RoutePrefixConvention : IApplicationModelConvention
    {
        private readonly AttributeRouteModel _routePrefix;

        public RoutePrefixConvention(string prefix)
        {
            _routePrefix = new AttributeRouteModel(new RouteAttribute(prefix + "/"));
        }

        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers.Where(p => IsCoreApiController(p.ControllerType)))
            {
                foreach (var selector in controller.Selectors.Where(x => x.AttributeRouteModel != null))
                {
                    AddRoutePrefixToExistingRoute(selector);
                }
            }
        }

        private bool IsCoreApiController(Type controllerType)
        {
            return controllerType.IsSubclassOfRawGeneric(typeof(ControllerApi<,,,>)) || controllerType.IsSubclassOfRawGeneric(typeof(ControllerApi<,>));
        }

        private void AddRoutePrefixToExistingRoute(SelectorModel selector)
        {
            selector.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(_routePrefix, selector.AttributeRouteModel);
        }
    }
}
