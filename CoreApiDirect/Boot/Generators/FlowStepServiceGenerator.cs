using System;
using System.Collections.Generic;
using System.Linq;
using CoreApiDirect.Base;
using Microsoft.Extensions.DependencyInjection;

namespace CoreApiDirect.Boot.Generators
{
    internal class FlowStepServiceGenerator : ServiceGenerator
    {
        private readonly List<Type> _specificImplementationTypes = new List<Type>();

        public FlowStepServiceGenerator(
            ITypeProvider typeProvider,
            Type helperGenericDefinition,
            Type serviceGenericDefinition,
            Type implementationGenericDefinitione)
            : base(typeProvider, helperGenericDefinition, serviceGenericDefinition, implementationGenericDefinitione, ServiceLifetime.Scoped)
        {
        }

        protected override Type MakeImplementationType(Type helperType)
        {
            return GetExistingImplementationType(helperType) ?? base.MakeImplementationType(helperType);
        }

        private Type GetExistingImplementationType(Type helperType)
        {
            var specificImplementationType = GetSpecificImplementationType(helperType);

            if (specificImplementationType != null)
            {
                _specificImplementationTypes.Add(specificImplementationType);
                return specificImplementationType;
            }

            return GetGenericImplementationType(helperType);
        }

        private Type GetSpecificImplementationType(Type helperType)
        {
            return (from specificImplementationType in TypeProvider.Types
                    where !specificImplementationType.IsGenericType &&
                          !specificImplementationType.IsAbstract &&
                          specificImplementationType.ImplementsRawGenericInterface(ServiceGenericDefinition) &&
                          MatchGenericArguments(specificImplementationType, helperType)
                    select specificImplementationType).FirstOrDefault();
        }

        private bool MatchGenericArguments(Type specificImplementationType, Type helperType)
        {
            var implementationGenericDefinitionArguments = ImplementationGenericDefinition.GetGenericArguments();
            var helperGenericDefinitionArguments = HelperGenericDefinition.GetGenericArguments();
            var specificImplementationTypeBaseGenericTypeArguments = specificImplementationType.BaseGenericType().GetGenericArguments();
            var helperTypeBaseGenericTypeArguments = helperType.BaseGenericType().GetGenericArguments();

            for (int i = 0; i < implementationGenericDefinitionArguments.Length; i++)
            {
                var helperGenericDefinitionArgument = helperGenericDefinitionArguments.FirstOrDefault(p => implementationGenericDefinitionArguments[i].Name == p.Name);
                var index = Array.IndexOf(helperGenericDefinitionArguments, helperGenericDefinitionArgument);
                if (specificImplementationTypeBaseGenericTypeArguments[i] != helperTypeBaseGenericTypeArguments[index])
                {
                    return false;
                }
            }

            return true;
        }

        private Type GetGenericImplementationType(Type helperType)
        {
            var implementationType = GetGenericImplementationType();

            if (implementationType != null)
            {
                return MakeGenericImplementationType(implementationType, helperType);
            }

            return null;
        }

        private Type GetGenericImplementationType()
        {
            return (from genericImplementationType in TypeProvider.Types
                    where genericImplementationType.IsGenericType &&
                          !genericImplementationType.IsAbstract &&
                          genericImplementationType.IsSubclassOfRawGeneric(ImplementationGenericDefinition) &&
                          !HasSubclass(genericImplementationType)
                    select genericImplementationType).FirstOrDefault();
        }

        private bool HasSubclass(Type genericImplementationType)
        {
            return (from subclassType in TypeProvider.Types
                    where !_specificImplementationTypes.Contains(subclassType) &&
                          subclassType.IsSubclassOfRawGeneric(genericImplementationType)
                    select subclassType).Any();
        }

        private Type MakeGenericImplementationType(Type implementationType, Type helperType)
        {
            var genericArguments = GetGenericArguments(implementationType, helperType);
            return implementationType.MakeGenericType(genericArguments);
        }
    }
}
