using System;
using System.Collections.Generic;
using System.Linq;
using CoreApiDirect.Base;
using Microsoft.Extensions.DependencyInjection;

namespace CoreApiDirect.Boot.Generators
{
    internal class ServiceGenerator : IServiceGenerator
    {
        protected readonly ITypeProvider TypeProvider;
        protected readonly Type HelperGenericDefinition;
        protected readonly Type ServiceGenericDefinition;
        protected readonly Type ImplementationGenericDefinition;

        private readonly ServiceLifetime _serviceLifetime;

        public ServiceGenerator(
            ITypeProvider typeProvider,
            Type helperGenericDefinition,
            Type serviceGenericDefinition,
            Type implementationGenericDefinition,
            ServiceLifetime serviceLifetime)
        {
            TypeProvider = typeProvider;
            HelperGenericDefinition = helperGenericDefinition;
            ServiceGenericDefinition = serviceGenericDefinition;
            ImplementationGenericDefinition = implementationGenericDefinition;
            _serviceLifetime = serviceLifetime;
        }

        public IServiceCollection Generate(IServiceCollection services)
        {
            foreach (var helperType in GetHelperTypes())
            {
                var serviceType = MakeServiceType(helperType);
                var implementationType = MakeImplementationType(helperType);
                AddService(services, serviceType, implementationType);
            }

            return services;
        }

        private IEnumerable<Type> GetHelperTypes()
        {
            return from helperType in TypeProvider.Types
                   where !helperType.IsAbstract &&
                         !helperType.IsGenericType &&
                         helperType.IsSubclassOfRawGeneric(HelperGenericDefinition)
                   select helperType;
        }

        private Type MakeServiceType(Type helperType)
        {
            var genericArguments = GetServiceGenericArguments(helperType);
            return ServiceGenericDefinition.MakeGenericType(genericArguments);
        }

        protected virtual Type[] GetServiceGenericArguments(Type helperType)
        {
            return GetGenericArguments(ServiceGenericDefinition, helperType);
        }

        protected Type[] GetGenericArguments(Type genericDefinition, Type helperType)
        {
            var genericArguments = new List<Type>();
            var helperGenericDefinitionArguments = HelperGenericDefinition.GetGenericArguments();
            var helperTypeBaseGenericTypeArguments = helperType.BaseGenericType().GetGenericArguments();

            foreach (var genericDefinitionArgument in genericDefinition.GetGenericArguments())
            {
                var helperGenericDefinitionArgument = helperGenericDefinitionArguments.FirstOrDefault(p => p.Name == genericDefinitionArgument.Name);
                int index = Array.IndexOf(helperGenericDefinitionArguments, helperGenericDefinitionArgument);
                genericArguments.Add(helperTypeBaseGenericTypeArguments[index]);
            }

            return genericArguments.ToArray();
        }

        protected virtual Type MakeImplementationType(Type helperType)
        {
            var genericArguments = GetImplementationGenericArguments(helperType);
            return ImplementationGenericDefinition.MakeGenericType(genericArguments);
        }

        protected virtual Type[] GetImplementationGenericArguments(Type helperType)
        {
            return GetGenericArguments(ImplementationGenericDefinition, helperType);
        }

        private void AddService(IServiceCollection services, Type serviceType, Type implementationType)
        {
            switch (_serviceLifetime)
            {
                case ServiceLifetime.Transient:
                    services.AddTransient(serviceType, implementationType);
                    break;
                case ServiceLifetime.Scoped:
                    services.AddScoped(serviceType, implementationType);
                    break;
                case ServiceLifetime.Singleton:
                    services.AddSingleton(serviceType, implementationType);
                    break;
            }
        }
    }
}
