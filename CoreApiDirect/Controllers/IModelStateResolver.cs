using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CoreApiDirect.Controllers
{
    internal interface IModelStateResolver
    {
        List<ModelStateError> GetModelErrors(ModelStateDictionary modelState);
    }
}
