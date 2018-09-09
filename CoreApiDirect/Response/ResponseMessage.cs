using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CoreApiDirect.Response
{
    internal class ResponseMessage
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public MessageType MessageType { get; }

        public string Message { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string[] AdditionalInfo { get; }

        public ResponseMessage(
            MessageType messageType,
            string message,
            string[] additionalInfo)
        {
            MessageType = messageType;
            Message = message;
            if (additionalInfo.Any())
            {
                AdditionalInfo = additionalInfo;
            }
        }
    }
}
