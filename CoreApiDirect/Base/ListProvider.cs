using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CoreApiDirect.Base
{
    internal class ListProvider : IListProvider
    {
        public object GetTypedList(IEnumerable<string> values, Type type, Type rawGenericListType)
        {
            var converter = TypeDescriptor.GetConverter(type);

            var objectArray = values
                .Select(p => converter.ConvertFromString(p.Trim()))
                .ToArray();

            var typedArray = Array.CreateInstance(type, objectArray.Length);
            objectArray.CopyTo(typedArray, 0);

            var listType = rawGenericListType.MakeGenericType(new Type[] { type });
            var typedList = Activator.CreateInstance(listType);

            var addRangeMethod = listType.GetMethod("AddRange");
            addRangeMethod.Invoke(typedList, new object[] { typedArray });

            return typedList;
        }
    }
}
