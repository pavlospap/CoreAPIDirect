using System;
using System.Linq;
using CoreApiDirect.Base;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace CoreApiDirect.Routing
{
    internal static class IActionContextAccessorExtensions
    {
        public static bool HasRouteParamIgnoreCase(this IActionContextAccessor actionContextAccessor, string paramName)
        {
            return actionContextAccessor.ActionContext.RouteData.Values.Keys.Contains(paramName, StringComparer.OrdinalIgnoreCase);
        }

        public static object GetRouteParamIgnoreCase(this IActionContextAccessor actionContextAccessor, string paramName)
        {
            return actionContextAccessor.ActionContext.RouteData.Values.GetValueIgnoreCase(paramName);
        }
    }
}
