using System;
using System.Collections.Generic;

namespace CoreApiDirect.Boot
{
    internal interface ITypeProvider
    {
        List<Type> Types { get; }
    }
}
