using System;
using System.Collections.Generic;
using System.Linq;
using CoreApiDirect.Boot;
using CoreApiDirect.Boot.Generators;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace CoreApiDirect.Tests.Boot.Generators
{
    public abstract class GeneratorsTestsBase
    {
        protected void GenerateServices(IServiceCollection services, List<Type> providedTypes, Type helperGenericDefinition, Type serviceGenericDefinition, Type implementationGenericDefinition, ServiceLifetime? serviceLifetime = null)
        {
            var mockTypeProvider = new Mock<ITypeProvider>();
            mockTypeProvider.Setup(p => p.Types).Returns(providedTypes);
            var generator = CreateGenerator(mockTypeProvider.Object, helperGenericDefinition, serviceGenericDefinition, implementationGenericDefinition, serviceLifetime);
            generator.Generate(services);
        }

        internal abstract IServiceGenerator CreateGenerator(ITypeProvider typeProvider, Type helperGenericDefinition, Type serviceGenericDefinition, Type implementationGenericDefinition, ServiceLifetime? serviceLifetime);

        protected void ValidateServices(IServiceCollection generatedServices, IDictionary<Type, Type> expectedServices)
        {
            var orderedGeneratedServices = generatedServices.OrderBy(p => p.ServiceType.FullName);
            var orderedExpectedServices = expectedServices.OrderBy(p => p.Key.FullName);

            Assert.True(Enumerable.SequenceEqual(orderedGeneratedServices.Select(p => p.ServiceType), orderedExpectedServices.Select(p => p.Key)));
            Assert.True(Enumerable.SequenceEqual(orderedGeneratedServices.Select(p => p.ImplementationType), orderedExpectedServices.Select(p => p.Value)));
        }
    }
}
