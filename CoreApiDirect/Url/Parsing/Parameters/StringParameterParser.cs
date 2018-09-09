using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreApiDirect.Url.Parsing.Parameters
{
    internal class StringParameterParser : ParameterParser, IStringParameterParser
    {
        protected override object ParseValues(Type type, List<string> queryValues)
        {
            if (!queryValues.Any())
            {
                return null;
            }

            string value = queryValues.First();

            return !string.IsNullOrWhiteSpace(value) ? value : null;
        }
    }
}
