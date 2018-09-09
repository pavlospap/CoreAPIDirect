using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreApiDirect.Url.Parsing.Parameters
{
    internal class BooleanParameterParser : ParameterParser, IBooleanParameterParser
    {
        protected override object ParseValues(Type type, List<string> queryValues)
        {
            if (!queryValues.Any())
            {
                return null;
            }

            if (bool.TryParse(queryValues.First(), out bool result))
            {
                return result;
            }

            return null;
        }
    }
}
