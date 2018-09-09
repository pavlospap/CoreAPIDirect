using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CoreApiDirect.Base;
using CoreApiDirect.Demo.Controllers.App;
using CoreApiDirect.Demo.Entities.App;
using CoreApiDirect.Repositories;
using CoreApiDirect.Routing;
using CoreApiDirect.Tests.DataContext;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CoreApiDirect.Tests.Routing
{
    public class RouteFilterBuilderTests : RouteTestsBase
    {
        [Fact]
        public void BuildFilter_ValidData_EqualToExpected()
        {
            var repository = new Repository<Phone, int, AppDbContextTests>(AppDbContextTests.GetContextWithData());

            Expression<Func<Phone, bool>> filter = p => p.ContactInfoId == 1 && p.ContactInfo.StudentId == 1 && p.ContactInfo.Student.SchoolId == 1;
            var expectedData = GetData(repository, filter);

            var actionContextAccessor = GetActionContextAccessor("schoolid:1#studentid:1#contactinfoid:1", new ServiceCollection());
            var routeFilterBuilder = new RouteFilterBuilder(actionContextAccessor, new PropertyProvider(), new MethodProvider());
            var builtFilter = (Expression<Func<Phone, bool>>)routeFilterBuilder.BuildFilter(typeof(PhonesController));
            var builtData = GetData(repository, builtFilter);

            Assert.Equal(expectedData.ToJson(), builtData.ToJson());
        }

        private IEnumerable<Phone> GetData(IRepository<Phone, int> repository, Expression<Func<Phone, bool>> filter)
        {
            return repository.Query
                .Where(p => p.Id.Equals(1))
                .Where(filter)
                .ToList();
        }
    }
}
