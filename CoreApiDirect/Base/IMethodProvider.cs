using System;
using System.Reflection;

namespace CoreApiDirect.Base
{
    internal interface IMethodProvider
    {
        MethodInfo MakeGenericMethod(Type type, string name, Type[] parameterDefinitions, Type[] typeArguments);
    }
}
