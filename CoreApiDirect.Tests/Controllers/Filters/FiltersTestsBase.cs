using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Moq;

namespace CoreApiDirect.Tests.Controllers.Filters
{
    public abstract class FiltersTestsBase
    {
        protected IActionResult GetActionResult(IActionFilter filter, ModelStateDictionary modelState, IDictionary<string, object> actionArguments)
        {
            var actionContext = new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor(), modelState);
            var mockFilters = new Mock<IList<IFilterMetadata>>();
            var actionExecutingContext = new ActionExecutingContext(actionContext, mockFilters.Object, actionArguments, null);
            filter.OnActionExecuting(actionExecutingContext);

            return actionExecutingContext.Result;
        }
    }
}
