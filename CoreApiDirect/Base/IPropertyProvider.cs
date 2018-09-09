using System;
using System.Reflection;

namespace CoreApiDirect.Base
{
    internal interface IPropertyProvider
    {
        PropertyInfo[] GetProperties(Type type);
    }
}
