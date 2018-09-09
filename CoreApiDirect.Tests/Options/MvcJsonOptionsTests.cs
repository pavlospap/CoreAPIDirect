using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;

namespace CoreApiDirect.Tests.Options
{
    internal class MvcJsonOptionsTests : IOptions<MvcJsonOptions>
    {
        public MvcJsonOptions Value { get; }

        public MvcJsonOptionsTests()
        {
            Value = new MvcJsonOptions();
            Value.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}
