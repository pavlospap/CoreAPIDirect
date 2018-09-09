using System;
using System.Linq.Expressions;

namespace CoreApiDirect.Routing
{
    internal interface IRouteFilterBuilder
    {
        Expression BuildFilter(Type controllerType);
    }
}
