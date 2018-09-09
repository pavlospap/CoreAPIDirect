using System;
using CoreApiDirect.Controllers;
using CoreApiDirect.Demo.Entities.App;
using Xunit;

namespace CoreApiDirect.Tests.Controllers
{
    public class ApiRouteAttributeTests
    {
        [Fact]
        public void Template_RouteEntityTypesMissing_ExceptionTrown()
        {
            Assert.Throws<ArgumentException>(() => new ApiRouteAttribute());
        }

        [Fact]
        public void Template_RouteEntityTypesPresent_TemplateCreated()
        {
            var route = new ApiRouteAttribute(typeof(School), typeof(Student), typeof(ContactInfo));
            Assert.Equal("schools/{schoolid}/students/{studentid}/contactinfo", route.Template);
        }
    }
}
