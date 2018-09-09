using System;
using System.Linq;
using CoreApiDirect.Base;
using CoreApiDirect.Demo.Entities.App;
using Xunit;

namespace CoreApiDirect.Tests.Base
{
    public class PropertyProviderTests
    {
        private readonly PropertyProvider _propertyProvider = new PropertyProvider();

        [Fact]
        public void GetProperties_TypeIsNull_ExceptionThrown()
        {
            Assert.Throws<ArgumentNullException>(() => _propertyProvider.GetProperties(null));
        }

        [Fact]
        public void GetProperties_TypeIsValid_PropertyArray()
        {
            Assert.True(Enumerable.SequenceEqual(typeof(School).GetProperties().OrderBy(p => p.Name), _propertyProvider.GetProperties(typeof(School)).OrderBy(p => p.Name)));
        }
    }
}
