using System.Collections.Generic;

namespace CoreApiDirect.Url.Parsing.Parameters
{
    internal class ParameterParsingResult
    {
        public object Value { get; set; }
        public IEnumerable<QueryStringError> Errors { get; set; }
    }
}
