using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreApiDirect.Controllers;

namespace CoreApiDirect.Routing
{
    internal interface IRouteValidator
    {
        Task<RecordError> ValidateRoute(Type controllerType, IEnumerable<object> ids);
    }
}
