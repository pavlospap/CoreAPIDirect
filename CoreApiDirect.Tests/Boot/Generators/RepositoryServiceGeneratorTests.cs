using System;
using System.Collections.Generic;
using System.Linq;
using CoreApiDirect.Boot;
using CoreApiDirect.Boot.Generators;
using CoreApiDirect.Demo.Entities.App;
using CoreApiDirect.Demo.Entities.Logging;
using CoreApiDirect.Entities;
using CoreApiDirect.Repositories;
using CoreApiDirect.Tests.Boot.Generators.Helpers;
using CoreApiDirect.Tests.DataContext;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CoreApiDirect.Tests.Boot.Generators
{
    public class RepositoryServiceGeneratorTests : GeneratorsTestsBase
    {
        [Fact]
        public void Generate_NoDbContextExists_ExceptionThrown()
        {
            Assert.Throws<InvalidOperationException>(
                () => GenerateServices(new ServiceCollection(), new List<Type>(), typeof(Entity<>), typeof(IRepository<,>), typeof(Repository<,,>)));
        }

        [Fact]
        public void Generate_EntityNotBelongsToDbContext_ExceptionThrown()
        {
            Assert.Throws<InvalidOperationException>(
                () => GenerateServices(new ServiceCollection(), new List<Type> { typeof(EntityNotInDbContext), typeof(AppDbContextTests), typeof(LogDbContextTests) }, typeof(Entity<>), typeof(IRepository<,>), typeof(Repository<,,>)));
        }

        [Fact]
        public void Generate_NoEntitiesExist_NoServicesAdded()
        {
            var generatedServices = new ServiceCollection();
            GenerateServices(generatedServices, new List<Type> { typeof(AppDbContextTests), typeof(LogDbContextTests) }, typeof(Entity<>), typeof(IRepository<,>), typeof(Repository<,,>));
            Assert.False(generatedServices.Any());
        }

        [Fact]
        public void Generate_EntitiesAndDbContextExist_ServicesAdded()
        {
            var providedTypes = new List<Type>
            {
                typeof(LogEvent),
                typeof(School),
                typeof(Student),
                typeof(AppDbContextTests),
                typeof(LogDbContextTests)
            };

            var expextedServices = new Dictionary<Type, Type> {
                { typeof(IRepository<LogEvent, int>), typeof(Repository<LogEvent, int, LogDbContextTests>) },
                { typeof(IRepository<School, int>), typeof(Repository<School, int, AppDbContextTests>) },
                { typeof(IRepository<Student, int>), typeof(Repository<Student, int, AppDbContextTests>) },
            };

            var generatedServices = new ServiceCollection();
            GenerateServices(generatedServices, providedTypes, typeof(Entity<>), typeof(IRepository<,>), typeof(Repository<,,>));

            ValidateServices(generatedServices, expextedServices);
        }

        internal override IServiceGenerator CreateGenerator(ITypeProvider typeProvider, Type helperGenericDefinition, Type serviceGenericDefinition, Type implementationGenericDefinition, ServiceLifetime? serviceLifetime)
        {
            return new RepositoryServiceGenerator(typeProvider, helperGenericDefinition, serviceGenericDefinition, implementationGenericDefinition);
        }
    }
}
