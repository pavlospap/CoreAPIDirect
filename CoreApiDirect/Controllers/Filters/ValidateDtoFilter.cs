using CoreApiDirect.Controllers.Results;
using CoreApiDirect.Resources;
using CoreApiDirect.Response;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CoreApiDirect.Controllers.Filters
{
    internal class ValidateDtoFilter : IActionFilter
    {
        protected virtual string DtoVariableName => "dto";

        private readonly IResponseBuilder _responseBuilder;
        private readonly IModelStateResolver _modelStateResolver;

        public ValidateDtoFilter(
            IResponseBuilder responseBuilder,
            IModelStateResolver modelStateResolver)
        {
            _responseBuilder = responseBuilder;
            _modelStateResolver = modelStateResolver;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var dto = context.ActionArguments.ContainsKey(DtoVariableName) ? context.ActionArguments[DtoVariableName] : null;

            if (dto == null)
            {
                context.Result = new ApiUnprocessableEntityResult(_responseBuilder.AddError(ApiResources.ModelDataMissing).Build());
            }
            else if (!context.ModelState.IsValid)
            {
                _modelStateResolver.GetModelErrors(context.ModelState).ForEach(error => _responseBuilder.AddError(error.Message, error.Field));
                context.Result = new ApiUnprocessableEntityResult(_responseBuilder.Build());
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
