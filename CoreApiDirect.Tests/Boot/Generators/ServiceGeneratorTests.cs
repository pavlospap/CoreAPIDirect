using System;
using System.Collections.Generic;
using System.Linq;
using CoreApiDirect.Boot;
using CoreApiDirect.Boot.Generators;
using CoreApiDirect.Controllers;
using CoreApiDirect.Demo.Controllers.App;
using CoreApiDirect.Demo.Controllers.Logging;
using CoreApiDirect.Demo.Dto.In.App;
using CoreApiDirect.Demo.Entities.App;
using CoreApiDirect.Demo.Entities.Logging;
using CoreApiDirect.Flow;
using CoreApiDirect.Mapping;
using CoreApiDirect.Query;
using CoreApiDirect.Query.Detail;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CoreApiDirect.Tests.Boot.Generators
{
    public class ServiceGeneratorTests : GeneratorsTestsBase
    {
        [Fact]
        public void Generate_NoControllersExist_NoServicesAdded()
        {
            var generatedServices = new ServiceCollection();
            GenerateServices(generatedServices, new List<Type>());
            Assert.False(generatedServices.Any());
        }

        [Fact]
        public void Generate_ControllersExist_ServicesAdded()
        {
            var providedTypes = new List<Type>
            {
                typeof(LogEventsController),
                typeof(SchoolsController),
                typeof(StudentsController)
            };

            var expectedServices = new Dictionary<Type, Type> {
                { typeof(IFlow<LogEvent>), typeof(Flow<LogEvent>) },
                { typeof(IFlow<School, SchoolInDto>), typeof(Flow<School, SchoolInDto>) },
                { typeof(IFlow<Student, StudentInDto>), typeof(Flow<Student, StudentInDto>) },
                { typeof(IQueryBuilder<LogEvent>), typeof(QueryBuilder<LogEvent>) },
                { typeof(IQueryBuilder<School>), typeof(QueryBuilder<School>) },
                { typeof(IQueryBuilder<Student>), typeof(QueryBuilder<Student>) },
                { typeof(IQueryPropertyWalker<LogEvent>), typeof(QueryPropertyWalker<LogEvent>) },
                { typeof(IQueryPropertyWalker<School>), typeof(QueryPropertyWalker<School>) },
                { typeof(IQueryPropertyWalker<Student>), typeof(QueryPropertyWalker<Student>) },
                { typeof(IQueryPropertyWalkerVisitor<LogEvent>), typeof(QueryPropertyWalkerVisitor<LogEvent>) },
                { typeof(IQueryPropertyWalkerVisitor<School>), typeof(QueryPropertyWalkerVisitor<School>) },
                { typeof(IQueryPropertyWalkerVisitor<Student>), typeof(QueryPropertyWalkerVisitor<Student>) },
                { typeof(IOneToManyQueryDetailPropertyWalker<LogEvent>), typeof(OneToManyQueryDetailPropertyWalker<LogEvent>) },
                { typeof(IOneToManyQueryDetailPropertyWalker<School>), typeof(OneToManyQueryDetailPropertyWalker<School>) },
                { typeof(IOneToManyQueryDetailPropertyWalker<Student>), typeof(OneToManyQueryDetailPropertyWalker<Student>) },
                { typeof(IOneToManyQueryDetailPropertyWalkerVisitor<LogEvent>), typeof(OneToManyQueryDetailPropertyWalkerVisitor<LogEvent>) },
                { typeof(IOneToManyQueryDetailPropertyWalkerVisitor<School>), typeof(OneToManyQueryDetailPropertyWalkerVisitor<School>) },
                { typeof(IOneToManyQueryDetailPropertyWalkerVisitor<Student>), typeof(OneToManyQueryDetailPropertyWalkerVisitor<Student>) },
                { typeof(IOneToOneQueryDetailPropertyWalker<LogEvent>), typeof(OneToOneQueryDetailPropertyWalker<LogEvent>) },
                { typeof(IOneToOneQueryDetailPropertyWalker<School>), typeof(OneToOneQueryDetailPropertyWalker<School>) },
                { typeof(IOneToOneQueryDetailPropertyWalker<Student>), typeof(OneToOneQueryDetailPropertyWalker<Student>) },
                { typeof(IOneToOneQueryDetailPropertyWalkerVisitor<LogEvent>), typeof(OneToOneQueryDetailPropertyWalkerVisitor<LogEvent>) },
                { typeof(IOneToOneQueryDetailPropertyWalkerVisitor<School>), typeof(OneToOneQueryDetailPropertyWalkerVisitor<School>) },
                { typeof(IOneToOneQueryDetailPropertyWalkerVisitor<Student>), typeof(OneToOneQueryDetailPropertyWalkerVisitor<Student>) },
                { typeof(IEntityMapper<LogEvent>), typeof(EntityMapper<LogEvent>) },
                { typeof(IEntityMapper<School>), typeof(EntityMapper<School>) },
                { typeof(IEntityMapper<Student>), typeof(EntityMapper<Student>) }
            };

            var generatedServices = new ServiceCollection();
            GenerateServices(generatedServices, providedTypes);

            ValidateServices(generatedServices, expectedServices);
        }

        private void GenerateServices(ServiceCollection generatedServices, List<Type> providedTypes)
        {
            GenerateServices(generatedServices, providedTypes, typeof(ControllerApi<,,,>), typeof(IFlow<,>), typeof(Flow<,>), ServiceLifetime.Scoped);
            GenerateServices(generatedServices, providedTypes, typeof(ControllerApi<,,,>), typeof(IQueryBuilder<>), typeof(QueryBuilder<>), ServiceLifetime.Scoped);
            GenerateServices(generatedServices, providedTypes, typeof(ControllerApi<,,,>), typeof(IQueryPropertyWalker<>), typeof(QueryPropertyWalker<>), ServiceLifetime.Transient);
            GenerateServices(generatedServices, providedTypes, typeof(ControllerApi<,,,>), typeof(IQueryPropertyWalkerVisitor<>), typeof(QueryPropertyWalkerVisitor<>), ServiceLifetime.Transient);
            GenerateServices(generatedServices, providedTypes, typeof(ControllerApi<,,,>), typeof(IOneToManyQueryDetailPropertyWalker<>), typeof(OneToManyQueryDetailPropertyWalker<>), ServiceLifetime.Transient);
            GenerateServices(generatedServices, providedTypes, typeof(ControllerApi<,,,>), typeof(IOneToManyQueryDetailPropertyWalkerVisitor<>), typeof(OneToManyQueryDetailPropertyWalkerVisitor<>), ServiceLifetime.Transient);
            GenerateServices(generatedServices, providedTypes, typeof(ControllerApi<,,,>), typeof(IOneToOneQueryDetailPropertyWalker<>), typeof(OneToOneQueryDetailPropertyWalker<>), ServiceLifetime.Transient);
            GenerateServices(generatedServices, providedTypes, typeof(ControllerApi<,,,>), typeof(IOneToOneQueryDetailPropertyWalkerVisitor<>), typeof(OneToOneQueryDetailPropertyWalkerVisitor<>), ServiceLifetime.Transient);
            GenerateServices(generatedServices, providedTypes, typeof(ControllerApi<,,,>), typeof(IEntityMapper<>), typeof(EntityMapper<>), ServiceLifetime.Scoped);
            GenerateServices(generatedServices, providedTypes, typeof(ControllerApi<,>), typeof(IFlow<>), typeof(Flow<>), ServiceLifetime.Scoped);
            GenerateServices(generatedServices, providedTypes, typeof(ControllerApi<,>), typeof(IQueryBuilder<>), typeof(QueryBuilder<>), ServiceLifetime.Scoped);
            GenerateServices(generatedServices, providedTypes, typeof(ControllerApi<,>), typeof(IQueryPropertyWalker<>), typeof(QueryPropertyWalker<>), ServiceLifetime.Transient);
            GenerateServices(generatedServices, providedTypes, typeof(ControllerApi<,>), typeof(IQueryPropertyWalkerVisitor<>), typeof(QueryPropertyWalkerVisitor<>), ServiceLifetime.Transient);
            GenerateServices(generatedServices, providedTypes, typeof(ControllerApi<,>), typeof(IOneToManyQueryDetailPropertyWalker<>), typeof(OneToManyQueryDetailPropertyWalker<>), ServiceLifetime.Transient);
            GenerateServices(generatedServices, providedTypes, typeof(ControllerApi<,>), typeof(IOneToManyQueryDetailPropertyWalkerVisitor<>), typeof(OneToManyQueryDetailPropertyWalkerVisitor<>), ServiceLifetime.Transient);
            GenerateServices(generatedServices, providedTypes, typeof(ControllerApi<,>), typeof(IOneToOneQueryDetailPropertyWalker<>), typeof(OneToOneQueryDetailPropertyWalker<>), ServiceLifetime.Transient);
            GenerateServices(generatedServices, providedTypes, typeof(ControllerApi<,>), typeof(IOneToOneQueryDetailPropertyWalkerVisitor<>), typeof(OneToOneQueryDetailPropertyWalkerVisitor<>), ServiceLifetime.Transient);
            GenerateServices(generatedServices, providedTypes, typeof(ControllerApi<,>), typeof(IEntityMapper<>), typeof(EntityMapper<>), ServiceLifetime.Scoped);
        }

        internal override IServiceGenerator CreateGenerator(ITypeProvider typeProvider, Type helperGenericDefinition, Type serviceGenericDefinition, Type implementationGenericDefinition, ServiceLifetime? serviceLifeTime)
        {
            return new ServiceGenerator(typeProvider, helperGenericDefinition, serviceGenericDefinition, implementationGenericDefinition, (ServiceLifetime)serviceLifeTime);
        }
    }
}
