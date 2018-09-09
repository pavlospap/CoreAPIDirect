using System;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;

namespace CoreApiDirect.Url.Parsing
{
    internal interface IQueryStringParser
    {
        QueryString Parse(Type type, IEnumerable<KeyValuePair<string, StringValues>> parameters);
    }
}
