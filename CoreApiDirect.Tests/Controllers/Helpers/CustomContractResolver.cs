using System;
using Newtonsoft.Json.Serialization;

namespace CoreApiDirect.Tests.Controllers.Helpers
{
    internal class CustomContractResolver : IContractResolver
    {
        public JsonContract ResolveContract(Type type)
        {
            return null;
        }
    }
}
