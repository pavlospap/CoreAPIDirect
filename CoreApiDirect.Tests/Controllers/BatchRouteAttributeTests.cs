using CoreApiDirect.Controllers;
using Xunit;

namespace CoreApiDirect.Tests.Controllers
{
    public class BatchRouteAttributeTests
    {
        [Fact]
        public void Template_NoRouteParamProvided_TemplateWithNoParam()
        {
            var route = new BatchRouteAttribute();
            Assert.Equal("batch", route.Template);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Template_RouteParamIsNullOrWhiteSpace_TemplateWithNoParam(string routeParam)
        {
            var route = new BatchRouteAttribute(routeParam);
            Assert.Equal("batch", route.Template);
        }

        [Fact]
        public void Template_RouteParamProvided_TemplateWithParam()
        {
            string routeParam = "({ids})";
            var route = new BatchRouteAttribute(routeParam);
            Assert.Equal("batch/" + routeParam, route.Template);
        }
    }
}
