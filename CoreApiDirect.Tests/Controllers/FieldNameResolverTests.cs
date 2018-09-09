using CoreApiDirect.Controllers;
using CoreApiDirect.Tests.Controllers.Helpers;
using CoreApiDirect.Tests.Options;
using Humanizer;
using Newtonsoft.Json.Serialization;
using Xunit;

namespace CoreApiDirect.Tests.Controllers
{
    public class FieldNameResolverTests
    {
        private const string PROPERTY_NAME = "PropertyName";

        private readonly MvcJsonOptionsTests _mvcJsonOptions = new MvcJsonOptionsTests();
        private readonly FieldNameResolver _fieldNameResolver;

        public FieldNameResolverTests()
        {
            _fieldNameResolver = new FieldNameResolver(_mvcJsonOptions);
        }

        [Fact]
        public void GetFieldName_DefaultResolver_PropertyName()
        {
            _mvcJsonOptions.Value.SerializerSettings.ContractResolver = new DefaultContractResolver();
            Assert.Equal(PROPERTY_NAME, _fieldNameResolver.GetFieldName(PROPERTY_NAME));
        }

        [Fact]
        public void GetFieldName_CamelCaseResolver_PropertyName()
        {
            Assert.Equal(PROPERTY_NAME.Camelize(), _fieldNameResolver.GetFieldName(PROPERTY_NAME));
        }

        [Fact]
        public void GetFieldName_CustomResolver_PropertyName()
        {
            _mvcJsonOptions.Value.SerializerSettings.ContractResolver = new CustomContractResolver();
            Assert.Equal(PROPERTY_NAME, _fieldNameResolver.GetFieldName(PROPERTY_NAME));
        }
    }
}
