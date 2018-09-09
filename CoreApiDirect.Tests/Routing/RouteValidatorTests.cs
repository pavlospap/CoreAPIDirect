using System;
using CoreApiDirect.Base;
using CoreApiDirect.Controllers;
using CoreApiDirect.Demo.Controllers.App;
using CoreApiDirect.Demo.DataContext;
using CoreApiDirect.Demo.Entities.App;
using CoreApiDirect.Repositories;
using CoreApiDirect.Routing;
using CoreApiDirect.Tests.DataContext;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CoreApiDirect.Tests.Routing
{
    public class RouteValidatorTests : RouteTestsBase
    {
        private const string RANDOM_GUID = "F5829391-6226-4F16-A1E3-282D3D17E2D0";

        [Theory]
        [InlineData(typeof(SchoolsController), "", "")]
        [InlineData(typeof(SchoolsController), "", "1")]
        [InlineData(typeof(StudentsController), "schoolid:1", "")]
        [InlineData(typeof(StudentsController), "schoolid:1", "1")]
        [InlineData(typeof(LessonsController), "schoolid:1", "")]
        [InlineData(typeof(LessonsController), "schoolid:1", InitialData.LESSON01_ID)]
        [InlineData(typeof(BooksController), "schoolid:1#lessonid:" + InitialData.LESSON02_ID, "")]
        [InlineData(typeof(BooksController), "schoolid:1#lessonid:" + InitialData.LESSON02_ID, "2")]
        [InlineData(typeof(ContactInfoController), "schoolid:1#studentid:1", "")]
        [InlineData(typeof(ContactInfoController), "schoolid:1#studentid:1", "1")]
        [InlineData(typeof(PhonesController), "schoolid:1#studentid:1#contactinfoid:1", "")]
        [InlineData(typeof(PhonesController), "schoolid:1#studentid:1#contactinfoid:1", "1")]
        public void ValidateRoute_ValidData_Null(Type controller, string routeEntityInfo, string ids)
        {
            Assert.Null(GetValidationResult(controller, routeEntityInfo, ids));
        }

        [Theory]
        [InlineData(typeof(SchoolsController), "", "999", nameof(RecordErrorType.RecordNotExist), typeof(School), 999, null, null)]
        [InlineData(typeof(LessonsController), "schoolid:999", InitialData.LESSON01_ID, nameof(RecordErrorType.RecordNotExist), typeof(School), 999, null, null)]
        [InlineData(typeof(LessonsController), "schoolid:1", RANDOM_GUID, nameof(RecordErrorType.RecordNotExist), typeof(Lesson), RANDOM_GUID, null, null)]
        [InlineData(typeof(BooksController), "schoolid:999#lessonid:" + InitialData.LESSON02_ID, "2", nameof(RecordErrorType.RecordNotExist), typeof(School), 999, null, null)]
        [InlineData(typeof(BooksController), "schoolid:1#lessonid:" + RANDOM_GUID, "2", nameof(RecordErrorType.RecordNotExist), typeof(Lesson), RANDOM_GUID, null, null)]
        [InlineData(typeof(BooksController), "schoolid:1#lessonid:" + InitialData.LESSON02_ID, "999", nameof(RecordErrorType.RecordNotExist), typeof(Book), 999, null, null)]
        [InlineData(typeof(StudentsController), "schoolid:999", "1", nameof(RecordErrorType.RecordNotExist), typeof(School), 999, null, null)]
        [InlineData(typeof(StudentsController), "schoolid:1", "999", nameof(RecordErrorType.RecordNotExist), typeof(Student), 999, null, null)]
        [InlineData(typeof(ContactInfoController), "schoolid:999#studentid:1", "1", nameof(RecordErrorType.RecordNotExist), typeof(School), 999, null, null)]
        [InlineData(typeof(ContactInfoController), "schoolid:1#studentid:999", "1", nameof(RecordErrorType.RecordNotExist), typeof(Student), 999, null, null)]
        [InlineData(typeof(ContactInfoController), "schoolid:1#studentid:1", "999", nameof(RecordErrorType.RecordNotExist), typeof(ContactInfo), 999, null, null)]
        [InlineData(typeof(PhonesController), "schoolid:999#studentid:1#contactinfoid:1", "1", nameof(RecordErrorType.RecordNotExist), typeof(School), 999, null, null)]
        [InlineData(typeof(PhonesController), "schoolid:1#studentid:999#contactinfoid:1", "1", nameof(RecordErrorType.RecordNotExist), typeof(Student), 999, null, null)]
        [InlineData(typeof(PhonesController), "schoolid:1#studentid:1#contactinfoid:999", "1", nameof(RecordErrorType.RecordNotExist), typeof(ContactInfo), 999, null, null)]
        [InlineData(typeof(PhonesController), "schoolid:1#studentid:1#contactinfoid:1", "999", nameof(RecordErrorType.RecordNotExist), typeof(Phone), 999, null, null)]
        [InlineData(typeof(LessonsController), "schoolid:1", InitialData.LESSON06_ID, nameof(RecordErrorType.RecordRelationNotValid), typeof(Lesson), InitialData.LESSON06_ID, typeof(School), 1)]
        [InlineData(typeof(BooksController), "schoolid:1#lessonid:" + InitialData.LESSON06_ID, "8", nameof(RecordErrorType.RecordRelationNotValid), typeof(Lesson), InitialData.LESSON06_ID, typeof(School), 1)]
        [InlineData(typeof(BooksController), "schoolid:1#lessonid:" + InitialData.LESSON02_ID, "1", nameof(RecordErrorType.RecordRelationNotValid), typeof(Book), 1, typeof(Lesson), InitialData.LESSON02_ID)]
        [InlineData(typeof(StudentsController), "schoolid:1", "17", nameof(RecordErrorType.RecordRelationNotValid), typeof(Student), 17, typeof(School), 1)]
        [InlineData(typeof(ContactInfoController), "schoolid:1#studentid:17", "17", nameof(RecordErrorType.RecordRelationNotValid), typeof(Student), 17, typeof(School), 1)]
        [InlineData(typeof(ContactInfoController), "schoolid:1#studentid:16", "17", nameof(RecordErrorType.RecordRelationNotValid), typeof(ContactInfo), 17, typeof(Student), 16)]
        [InlineData(typeof(PhonesController), "schoolid:1#studentid:17#contactinfoid:17", "25", nameof(RecordErrorType.RecordRelationNotValid), typeof(Student), 17, typeof(School), 1)]
        [InlineData(typeof(PhonesController), "schoolid:1#studentid:1#contactinfoid:2", "3", nameof(RecordErrorType.RecordRelationNotValid), typeof(ContactInfo), 2, typeof(Student), 1)]
        [InlineData(typeof(PhonesController), "schoolid:1#studentid:1#contactinfoid:1", "3", nameof(RecordErrorType.RecordRelationNotValid), typeof(Phone), 3, typeof(ContactInfo), 1)]
        public void ValidateRoute_InvalidData_Error(Type controller, string routeEntityInfo, string ids, string errorType, Type entityType, object entityId, Type parentEntityType, object parentEntityId)
        {
            var result = GetValidationResult(controller, routeEntityInfo, ids);
            Assert.True(
                result.ErrorType == Enum.Parse<RecordErrorType>(errorType) &&
                result.EntityType == entityType &&
                Convert.ToString(result.EntityId) == Convert.ToString(entityId) &&
                result.ParentEntityType == parentEntityType &&
                Convert.ToString(result.ParentEntityId) == Convert.ToString(parentEntityId));
        }

        private RecordError GetValidationResult(Type controller, string routeEntityInfo, string ids)
        {
            return new RouteValidator(GetActionContextAccessor(routeEntityInfo, GetRepositoryServices()), new PropertyProvider(), new MethodProvider()).ValidateRoute(controller, ids.Split(',', StringSplitOptions.RemoveEmptyEntries)).Result;
        }

        private IServiceCollection GetRepositoryServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IRepository<Book, int>>(new Repository<Book, int, AppDbContextTests>(AppDbContextTests.GetContextWithData()));
            services.AddSingleton<IRepository<ContactInfo, int>>(new Repository<ContactInfo, int, AppDbContextTests>(AppDbContextTests.GetContextWithData()));
            services.AddSingleton<IRepository<Lesson, string>>(new Repository<Lesson, string, AppDbContextTests>(AppDbContextTests.GetContextWithData()));
            services.AddSingleton<IRepository<Phone, int>>(new Repository<Phone, int, AppDbContextTests>(AppDbContextTests.GetContextWithData()));
            services.AddSingleton<IRepository<School, int>>(new Repository<School, int, AppDbContextTests>(AppDbContextTests.GetContextWithData()));
            services.AddSingleton<IRepository<Student, int>>(new Repository<Student, int, AppDbContextTests>(AppDbContextTests.GetContextWithData()));

            return services;
        }
    }
}
