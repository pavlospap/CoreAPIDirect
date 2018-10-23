using System;
using System.Threading.Tasks;
using CoreApiDirect.Base;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CoreApiDirect.Controllers
{
    internal class KeyModelBinder : IModelBinder
    {
        private readonly IListProvider _listProvider;

        public KeyModelBinder(IListProvider listProvider)
        {
            _listProvider = listProvider;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (!bindingContext.ModelMetadata.IsEnumerableType)
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }

            string routeParam = Convert.ToString(bindingContext.ValueProvider.GetValue(bindingContext.ModelName));

            if (string.IsNullOrWhiteSpace(routeParam))
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }

            var type = bindingContext.ModelType.GenericTypeArguments[0];

            var typedList = _listProvider.GetTypedList(
                routeParam.Split(',', StringSplitOptions.RemoveEmptyEntries), type, typeof(KeyList<>));

            bindingContext.Model = typedList;
            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);

            return Task.CompletedTask;
        }
    }
}
