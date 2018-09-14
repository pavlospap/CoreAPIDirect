using System;
using CoreApiDirect.Boot;
using CoreApiDirect.Options;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CoreApiDirect.Tests.Boot
{
    public class IServiceCollectionExtentionsTests
    {
        [Fact]
        public void AddApiServices_ResponseBuilderTypeMissing_ExceptionThrown()
        {
            Assert.Throws<InvalidOperationException>(() =>
            IServiceCollectionExtentions.AddApiServices(new ServiceCollection(), new CoreOptions
            {
                ResponseBuilderType = null
            }));
        }
    }
}
