using System;
using System.Collections.Generic;
using CoreApiDirect.Boot;
using CoreApiDirect.Boot.Generators;
using CoreApiDirect.Controllers;
using CoreApiDirect.Demo.Controllers.App;
using CoreApiDirect.Demo.Controllers.Logging;
using CoreApiDirect.Demo.Dto.In.App;
using CoreApiDirect.Demo.Entities.App;
using CoreApiDirect.Demo.Entities.Logging;
using CoreApiDirect.Demo.Flow;
using CoreApiDirect.Flow.Steps;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CoreApiDirect.Tests.Boot.Generators
{
    public class FlowStepServiceGeneratorTests : GeneratorsTestsBase
    {
        [Fact]
        public void Generate_ControllersExist_ServicesAdded()
        {
            var providedTypes = new List<Type>
            {
                typeof(LogEventsController),
                typeof(SystemInfoController),
                typeof(SchoolsController),
                typeof(StudentsController),
                typeof(FlowStepBeforeSaveBase<,>),
                typeof(FlowStepBeforeSaveBase<>),
                typeof(SystemInfoFlowStepBeforeSave),
                typeof(SchoolFlowStepBeforeSave)
            };

            var expectedServices = new Dictionary<Type, Type> {
                { typeof(IFlowStepBeforePost<LogEvent>), typeof(FlowStepBeforePost<LogEvent>) },
                { typeof(IFlowStepBeforePost<SystemInfo>), typeof(FlowStepBeforePost<SystemInfo>) },
                { typeof(IFlowStepBeforePost<SchoolInDto, School>), typeof(FlowStepBeforePost<SchoolInDto, School>) },
                { typeof(IFlowStepBeforePost<StudentInDto, Student>), typeof(FlowStepBeforePost<StudentInDto, Student>) },
                { typeof(IFlowStepBeforeUpdate<LogEvent>), typeof(FlowStepBeforeUpdate<LogEvent>) },
                { typeof(IFlowStepBeforeUpdate<SystemInfo>), typeof(FlowStepBeforeUpdate<SystemInfo>) },
                { typeof(IFlowStepBeforeUpdate<SchoolInDto, School>), typeof(FlowStepBeforeUpdate<SchoolInDto, School>) },
                { typeof(IFlowStepBeforeUpdate<StudentInDto, Student>), typeof(FlowStepBeforeUpdate<StudentInDto, Student>) },
                { typeof(IFlowStepBeforeSave<LogEvent>), typeof(FlowStepBeforeSaveBase<LogEvent>) },
                { typeof(IFlowStepBeforeSave<SystemInfo>), typeof(SystemInfoFlowStepBeforeSave) },
                { typeof(IFlowStepBeforeSave<SchoolInDto, School>), typeof(SchoolFlowStepBeforeSave) },
                { typeof(IFlowStepBeforeSave<StudentInDto, Student>), typeof(FlowStepBeforeSaveBase<StudentInDto, Student>) },
                { typeof(IFlowStepBeforeDelete<LogEvent>), typeof(FlowStepBeforeDelete<LogEvent>) },
                { typeof(IFlowStepBeforeDelete<SystemInfo>), typeof(FlowStepBeforeDelete<SystemInfo>) },
                { typeof(IFlowStepBeforeDelete<School>), typeof(FlowStepBeforeDelete<School>) },
                { typeof(IFlowStepBeforeDelete<Student>), typeof(FlowStepBeforeDelete<Student>) },
                { typeof(IFlowStepAfterPost<LogEvent>), typeof(FlowStepAfterPost<LogEvent>) },
                { typeof(IFlowStepAfterPost<SystemInfo>), typeof(FlowStepAfterPost<SystemInfo>) },
                { typeof(IFlowStepAfterPost<SchoolInDto, School>), typeof(FlowStepAfterPost<SchoolInDto, School>) },
                { typeof(IFlowStepAfterPost<StudentInDto, Student>), typeof(FlowStepAfterPost<StudentInDto, Student>) },
                { typeof(IFlowStepAfterUpdate<LogEvent>), typeof(FlowStepAfterUpdate<LogEvent>) },
                { typeof(IFlowStepAfterUpdate<SystemInfo>), typeof(FlowStepAfterUpdate<SystemInfo>) },
                { typeof(IFlowStepAfterUpdate<SchoolInDto, School>), typeof(FlowStepAfterUpdate<SchoolInDto, School>) },
                { typeof(IFlowStepAfterUpdate<StudentInDto, Student>), typeof(FlowStepAfterUpdate<StudentInDto, Student>) },
                { typeof(IFlowStepAfterSave<LogEvent>), typeof(FlowStepAfterSave<LogEvent>) },
                { typeof(IFlowStepAfterSave<SystemInfo>), typeof(FlowStepAfterSave<SystemInfo>) },
                { typeof(IFlowStepAfterSave<SchoolInDto, School>), typeof(FlowStepAfterSave<SchoolInDto, School>) },
                { typeof(IFlowStepAfterSave<StudentInDto, Student>), typeof(FlowStepAfterSave<StudentInDto, Student>) },
                { typeof(IFlowStepAfterDelete<LogEvent>), typeof(FlowStepAfterDelete<LogEvent>) },
                { typeof(IFlowStepAfterDelete<SystemInfo>), typeof(FlowStepAfterDelete<SystemInfo>) },
                { typeof(IFlowStepAfterDelete<School>), typeof(FlowStepAfterDelete<School>) },
                { typeof(IFlowStepAfterDelete<Student>), typeof(FlowStepAfterDelete<Student>)}
            };

            var generatedServices = new ServiceCollection();
            GenerateServices(generatedServices, providedTypes);

            ValidateServices(generatedServices, expectedServices);
        }

        private void GenerateServices(ServiceCollection generatedServices, List<Type> providedTypes)
        {
            GenerateServices(generatedServices, providedTypes, typeof(ControllerApi<,,,>), typeof(IFlowStepBeforePost<,>), typeof(FlowStepBeforePost<,>));
            GenerateServices(generatedServices, providedTypes, typeof(ControllerApi<,,,>), typeof(IFlowStepBeforeUpdate<,>), typeof(FlowStepBeforeUpdate<,>));
            GenerateServices(generatedServices, providedTypes, typeof(ControllerApi<,,,>), typeof(IFlowStepBeforeSave<,>), typeof(FlowStepBeforeSave<,>));
            GenerateServices(generatedServices, providedTypes, typeof(ControllerApi<,,,>), typeof(IFlowStepBeforeDelete<>), typeof(FlowStepBeforeDelete<>));
            GenerateServices(generatedServices, providedTypes, typeof(ControllerApi<,,,>), typeof(IFlowStepAfterPost<,>), typeof(FlowStepAfterPost<,>));
            GenerateServices(generatedServices, providedTypes, typeof(ControllerApi<,,,>), typeof(IFlowStepAfterUpdate<,>), typeof(FlowStepAfterUpdate<,>));
            GenerateServices(generatedServices, providedTypes, typeof(ControllerApi<,,,>), typeof(IFlowStepAfterSave<,>), typeof(FlowStepAfterSave<,>));
            GenerateServices(generatedServices, providedTypes, typeof(ControllerApi<,,,>), typeof(IFlowStepAfterDelete<>), typeof(FlowStepAfterDelete<>));
            GenerateServices(generatedServices, providedTypes, typeof(ControllerApi<,>), typeof(IFlowStepBeforePost<>), typeof(FlowStepBeforePost<>));
            GenerateServices(generatedServices, providedTypes, typeof(ControllerApi<,>), typeof(IFlowStepBeforeUpdate<>), typeof(FlowStepBeforeUpdate<>));
            GenerateServices(generatedServices, providedTypes, typeof(ControllerApi<,>), typeof(IFlowStepBeforeSave<>), typeof(FlowStepBeforeSave<>));
            GenerateServices(generatedServices, providedTypes, typeof(ControllerApi<,>), typeof(IFlowStepBeforeDelete<>), typeof(FlowStepBeforeDelete<>));
            GenerateServices(generatedServices, providedTypes, typeof(ControllerApi<,>), typeof(IFlowStepAfterPost<>), typeof(FlowStepAfterPost<>));
            GenerateServices(generatedServices, providedTypes, typeof(ControllerApi<,>), typeof(IFlowStepAfterUpdate<>), typeof(FlowStepAfterUpdate<>));
            GenerateServices(generatedServices, providedTypes, typeof(ControllerApi<,>), typeof(IFlowStepAfterSave<>), typeof(FlowStepAfterSave<>));
            GenerateServices(generatedServices, providedTypes, typeof(ControllerApi<,>), typeof(IFlowStepAfterDelete<>), typeof(FlowStepAfterDelete<>));
        }

        internal override IServiceGenerator CreateGenerator(ITypeProvider typeProvider, Type helperGenericDefinition, Type serviceGenericDefinition, Type implementationGenericDefinition, ServiceLifetime? serviceLifetime)
        {
            return new FlowStepServiceGenerator(typeProvider, helperGenericDefinition, serviceGenericDefinition, implementationGenericDefinition);
        }
    }
}
