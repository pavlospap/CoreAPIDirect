using System;
using System.Linq;
using System.Threading.Tasks;
using CoreApiDirect.Base;
using CoreApiDirect.Url.Parsing;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CoreApiDirect.Url
{
    internal class QueryStringModelBinder : IModelBinder
    {
        private readonly IQueryStringParser _queryStringParser;

        public QueryStringModelBinder(IQueryStringParser queryStringParser)
        {
            _queryStringParser = queryStringParser;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            bindingContext.Model = _queryStringParser.Parse(GetTypeFromController(bindingContext), bindingContext.HttpContext.Request.Query);
            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);

            return Task.CompletedTask;
        }

        private static Type GetTypeFromController(ModelBindingContext bindingContext)
        {
            var controllerType = (bindingContext.ActionContext.ActionDescriptor as ControllerActionDescriptor).ControllerTypeInfo;
            var controllerTypeBaseGenericTypeGenericArguments = controllerType.BaseGenericType().GetGenericTypeDefinition().GetGenericArguments();
            var genericArgument = controllerTypeBaseGenericTypeGenericArguments.First(p => p.Name == "TOutDto");
            var index = Array.IndexOf(controllerTypeBaseGenericTypeGenericArguments, genericArgument);

            return controllerType.BaseGenericType().GenericTypeArguments[index];
        }
    }
}
