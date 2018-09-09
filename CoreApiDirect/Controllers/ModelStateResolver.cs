using System.Collections.Generic;
using System.Linq;
using CoreApiDirect.Base;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CoreApiDirect.Controllers
{
    internal class ModelStateResolver : IModelStateResolver
    {
        private readonly IFieldNameResolver _fieldNameResolver;

        public ModelStateResolver(IFieldNameResolver fieldNameResolver)
        {
            _fieldNameResolver = fieldNameResolver;
        }

        public List<ModelStateError> GetModelErrors(ModelStateDictionary modelState)
        {
            var errors = new List<ModelStateError>();

            foreach (string key in modelState.Keys)
            {
                var field = _fieldNameResolver.GetFieldName(key);
                errors.AddRange(GetMessages(field, modelState[key].Errors));
                errors.AddRange(GetExceptions(field, modelState[key].Errors));
            }

            return errors;
        }

        private IEnumerable<ModelStateError> GetMessages(string field, ModelErrorCollection modelErrors)
        {
            return modelErrors
                .Where(p => !string.IsNullOrWhiteSpace(p.ErrorMessage))
                .Select(p => new ModelStateError(field, p.ErrorMessage));
        }

        private IEnumerable<ModelStateError> GetExceptions(string field, ModelErrorCollection modelErrors)
        {
            return modelErrors
                .Where(p => p.Exception != null)
                .Select(p => new ModelStateError(field, p.Exception.MostInnerMessage()));
        }
    }
}
