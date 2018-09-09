using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace CoreApiDirect.Base
{
    internal static class TypeExtensions
    {
        public static bool IsSubclassOfRawGeneric(this Type type, Type genericDefinition)
        {
            genericDefinition.ValidateNull(nameof(genericDefinition));
            genericDefinition.ValidateGenericClass();

            return type.DeepCheck(genericDefinition, (t, gd) =>
                t != gd &&
                t.IsGenericType &&
                t.GetGenericTypeDefinition() == gd);
        }

        private static void ValidateGenericClass(this Type genericDefinition)
        {
            if (!genericDefinition.IsGenericType || genericDefinition.IsInterface)
            {
                throw new ArgumentException($"'{genericDefinition.Name}' is not a generic class.");
            }
        }

        private static bool DeepCheck(this Type type, Type genericDefinition, Func<Type, Type, bool> checkFunc)
        {
            do
            {
                if (checkFunc(type, genericDefinition))
                {
                    return true;
                }
            }
            while ((type = type.BaseType) != null);

            return false;
        }

        public static bool ImplementsRawGenericInterface(this Type type, Type genericDefinition)
        {
            genericDefinition.ValidateNull(nameof(genericDefinition));
            genericDefinition.ValidateGenericInterface();

            return type.GetInterfaces().Any(p => p.IsGenericType &&
                                                 p.GetGenericTypeDefinition() == genericDefinition);
        }

        private static void ValidateGenericInterface(this Type genericDefinition)
        {
            if (!genericDefinition.IsGenericType || !genericDefinition.IsInterface)
            {
                throw new ArgumentException($"'{genericDefinition.Name}' is not a generic interface.");
            }
        }

        public static bool IsListOfRawGeneric(this Type type, Type genericDefinition)
        {
            genericDefinition.ValidateNull(nameof(genericDefinition));
            genericDefinition.ValidateGenericClass();

            return type.DeepCheck(genericDefinition, (t, gd) =>
                t.IsGenericType &&
                typeof(IEnumerable).IsAssignableFrom(t) &&
                t.GenericTypeArguments[0].IsSubclassOfRawGeneric(gd));
        }

        public static bool IsDetailOfRawGeneric(this Type type, Type genericDefinition)
        {
            return type.IsListOfRawGeneric(genericDefinition) || type.IsSubclassOfRawGeneric(genericDefinition);
        }

        public static bool IsListOfType(this Type type, Type elementType)
        {
            elementType.ValidateNull(nameof(elementType));

            return type.DeepCheck(elementType, (t, et) =>
                t.IsGenericType &&
                typeof(IEnumerable).IsAssignableFrom(t) &&
                t.GenericTypeArguments[0] == et);
        }

        public static Type BaseGenericType(this Type type)
        {
            Type baseGenericType = null;

            do
            {
                if (type.IsGenericType)
                {
                    baseGenericType = type;
                }
            }
            while ((type = type.BaseType) != null);

            return baseGenericType;
        }

        public static PropertyInfo GetPropertyIgnoreCase(this Type type, string propertyName)
        {
            propertyName.ValidateNull(nameof(propertyName));
            return type.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        }
    }
}
