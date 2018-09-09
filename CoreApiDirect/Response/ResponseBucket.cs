using System.Collections.Generic;
using Newtonsoft.Json;

namespace CoreApiDirect.Response
{
    internal class ResponseBucket
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ResponseMessage> Messages { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object Data { get; set; }
    }
}
