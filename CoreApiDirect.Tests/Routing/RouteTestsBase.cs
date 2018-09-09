using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace CoreApiDirect.Tests.Routing
{
    public abstract class RouteTestsBase
    {
        protected IActionContextAccessor GetActionContextAccessor(string routeEntityInfo, IServiceCollection services)
        {
            var mockActionContextAccessor = new Mock<IActionContextAccessor>();
            var routeData = GetRouteData(routeEntityInfo);

            mockActionContextAccessor.Setup(p => p.ActionContext).Returns(new ActionContext
            {
                HttpContext = new DefaultHttpContext
                {
                    RequestServices = services.BuildServiceProvider()
                },
                RouteData = routeData
            });

            return mockActionContextAccessor.Object;
        }

        private RouteData GetRouteData(string routeEntityInfo)
        {
            var routeData = new RouteData();

            var routeEntityInfoParts = routeEntityInfo.Split('#', StringSplitOptions.RemoveEmptyEntries);
            foreach (var routeEntityPart in routeEntityInfoParts)
            {
                var routeEntity = routeEntityPart.Split(':');
                routeData.Values[routeEntity[0]] = routeEntity[1];
            }

            return routeData;
        }
    }
}
