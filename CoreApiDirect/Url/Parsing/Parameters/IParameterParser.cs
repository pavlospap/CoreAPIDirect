using System;
using Microsoft.Extensions.Primitives;

namespace CoreApiDirect.Url.Parsing.Parameters
{
    internal interface IParameterParser
    {
        ParameterParsingResult Parse(Type type, string parameterName, StringValues queryValues);
    }
}
