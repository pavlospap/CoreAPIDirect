using System;
using System.Collections.Generic;

namespace CoreApiDirect.Base
{
    internal interface IListProvider
    {
        object GetTypedList(IEnumerable<string> values, Type type, Type rawGenericListType);
    }
}
