using CoreApiDirect.Controllers.Results;
using CoreApiDirect.Resources;
using CoreApiDirect.Response;
using CoreApiDirect.Url;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CoreApiDirect.Controllers.Filters
{
    internal class ValidateQueryStringFilter : IActionFilter
    {
        private readonly IResponseBuilder _responseBuilder;

        public ValidateQueryStringFilter(IResponseBuilder responseBuilder)
        {
            _responseBuilder = responseBuilder;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var queryString = context.ActionArguments["queryString"] as QueryString;

            if (queryString.ValidateQueryString)
            {
                queryString.Errors.ForEach(error => _responseBuilder.AddError(
                    string.Format(ApiResources.ResourceManager.GetString(error.Type.ToString()), error.ParameterName), error.Info));

                if (_responseBuilder.HasErrors())
                {
                    context.Result = new InvalidQueryStringResult(_responseBuilder.Build());
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
