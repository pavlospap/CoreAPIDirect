using System;
using System.Linq;
using System.Reflection;

namespace CoreApiDirect.Base
{
    internal class MethodProvider : IMethodProvider
    {
        public MethodInfo MakeGenericMethod(Type type, string name, Type[] parameterDefinitions, Type[] typeArguments)
        {
            return type.GetMethods(BindingFlags.Static | BindingFlags.Public)
                .First(m => m.Name == name &&
                            Enumerable.SequenceEqual(m.GetParameters().Select(p => p.ParameterType.GetGenericTypeDefinition()), parameterDefinitions))
                .MakeGenericMethod(typeArguments);
        }
    }
}
