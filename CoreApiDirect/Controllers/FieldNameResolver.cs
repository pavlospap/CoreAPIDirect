using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;

namespace CoreApiDirect.Controllers
{
    internal class FieldNameResolver : IFieldNameResolver
    {
        private readonly MvcJsonOptions _mvcJsonOptions;

        public FieldNameResolver(IOptions<MvcJsonOptions> mvcJsonOptions)
        {
            _mvcJsonOptions = mvcJsonOptions.Value;
        }

        public string GetFieldName(string propertyName)
        {
            return _mvcJsonOptions.SerializerSettings.ContractResolver is DefaultContractResolver jsonResolver ?
                jsonResolver.GetResolvedPropertyName(propertyName) :
                propertyName;
        }
    }
}
