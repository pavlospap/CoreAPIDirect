using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreApiDirect.Url.Parsing.Parameters
{
    internal class IntegerParameterParser : ParameterParser, IIntegerParameterParser
    {
        protected override object ParseValues(Type type, List<string> queryValues)
        {
            if (!queryValues.Any())
            {
                return null;
            }

            if (int.TryParse(queryValues.First(), out int result))
            {
                return result;
            }

            return null;
        }
    }
}
