using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreApiDirect.Controllers;

namespace CoreApiDirect.Routing
{
    internal interface IRouteValidator
    {
        Task<RecordError> ValidateRouteAsync(Type controllerType, IEnumerable<object> ids);
    }
}
