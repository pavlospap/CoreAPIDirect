using System.Collections.Generic;
using System.Linq;

namespace CoreApiDirect.Response
{
    internal class ResponseBuilder : IResponseBuilder
    {
        private readonly ResponseBucket _responseBucket;

        public ResponseBuilder()
        {
            _responseBucket = new ResponseBucket();
        }

        public IResponseBuilder AddInfo(string message, params string[] additionalInfo)
        {
            return AddMessage(MessageType.Info, message, additionalInfo);
        }

        public IResponseBuilder AddWarning(string message, params string[] additionalInfo)
        {
            return AddMessage(MessageType.Warning, message, additionalInfo);
        }

        public IResponseBuilder AddError(string message, params string[] additionalInfo)
        {
            return AddMessage(MessageType.Error, message, additionalInfo);
        }

        public IResponseBuilder AddMessage(MessageType messageType, string message, params string[] additionalInfo)
        {
            _responseBucket.Messages = _responseBucket.Messages ?? new List<ResponseMessage>();
            _responseBucket.Messages.Add(new ResponseMessage(messageType, message, additionalInfo));

            return this;
        }

        public IResponseBuilder AddData(object data)
        {
            _responseBucket.Data = data;
            return this;
        }

        public bool HasErrors()
        {
            return _responseBucket.Messages != null && _responseBucket.Messages.Any(p => p.MessageType == MessageType.Error);
        }

        public object Build()
        {
            return _responseBucket;
        }
    }
}
