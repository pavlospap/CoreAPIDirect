using System;
using Microsoft.AspNetCore.Mvc.Routing;

namespace CoreApiDirect.Controllers
{
    internal class BatchRouteAttribute : Attribute, IRouteTemplateProvider
    {
        public string Template
        {
            get
            {
                return "batch" + (!string.IsNullOrWhiteSpace(_routeParam) ? "/" + _routeParam : "");
            }
        }

        public int? Order => null;

        public string Name => null;

        private readonly string _routeParam;

        public BatchRouteAttribute()
        {
        }

        public BatchRouteAttribute(string routeParam)
        {
            _routeParam = routeParam;
        }
    }
}
