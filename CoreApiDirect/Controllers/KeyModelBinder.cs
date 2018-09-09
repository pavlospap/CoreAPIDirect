using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CoreApiDirect.Controllers
{
    internal class KeyModelBinder : IModelBinder
    {
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

            var typedArray = GetTypedArray(routeParam, type);
            var typedList = GetTypedList(type, typedArray);

            bindingContext.Model = typedList;
            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);

            return Task.CompletedTask;
        }

        private Array GetTypedArray(string routeParam, Type type)
        {
            var converter = TypeDescriptor.GetConverter(type);

            var ids = routeParam.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(p => converter.ConvertFromString(p.Trim()))
                .ToArray();

            var typedArray = Array.CreateInstance(type, ids.Length);
            ids.CopyTo(typedArray, 0);

            return typedArray;
        }

        private object GetTypedList(Type type, Array typedArray)
        {
            var listType = typeof(KeyList<>).MakeGenericType(new Type[] { type });
            var typedList = Activator.CreateInstance(listType);

            var addRangeMethod = listType.GetMethod("AddRange");
            addRangeMethod.Invoke(typedList, new object[] { typedArray });

            return typedList;
        }
    }
}
