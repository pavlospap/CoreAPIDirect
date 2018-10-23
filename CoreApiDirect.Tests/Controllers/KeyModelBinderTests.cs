using System;
using System.Linq;
using CoreApiDirect.Base;
using CoreApiDirect.Controllers;
using CoreApiDirect.Tests.Controllers.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using Moq;
using Xunit;

namespace CoreApiDirect.Tests.Controllers
{
    public class KeyModelBinderTests
    {
        [Fact]
        public void BindModelAsync_ModelIsNotEnumerableType_Failed()
        {
            Assert.False(GetBindingResult(typeof(int), null).IsModelSet);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void BindModelAsync_RouteParamIsNullOrWhiteSpace_Failed(string routeParam)
        {
            Assert.False(GetBindingResult(typeof(KeyList<int>), routeParam).IsModelSet);
        }

        [Fact]
        public void BindModelAsync_RouteParamHasEmptyValues_EmptyList()
        {
            Assert.False((GetBindingResult(typeof(KeyList<int>), ",,").Model as KeyList<int>).Any());
        }

        [Fact]
        public void BindModelAsync_RouteParamHasInvalidValues_ExceptionThrown()
        {
            Assert.Throws<ArgumentException>(() => GetBindingResult(typeof(KeyList<int>), "nonInteger,nonInteger"));
        }

        [Fact]
        public void BindModelAsync_RouteParamHasValidValues_KeyList()
        {
            Assert.True(Enumerable.SequenceEqual(new KeyList<int> { 1, 2, 3 }, GetBindingResult(typeof(KeyList<int>), "1,2,3").Model as KeyList<int>));
        }

        private ModelBindingResult GetBindingResult(Type modelType, string routeParam)
        {
            var modelMetadata = ModelMetadataInitializer.ForType(modelType);

            var mockValueProvider = new Mock<IValueProvider>();
            mockValueProvider.Setup(p => p.GetValue(It.IsAny<string>())).Returns(new ValueProviderResult(new StringValues(routeParam)));

            var modelBindingContext = new DefaultModelBindingContext
            {
                ModelMetadata = modelMetadata,
                ValueProvider = mockValueProvider.Object
            };

            var binder = new KeyModelBinder(new ListProvider());
            binder.BindModelAsync(modelBindingContext);

            return modelBindingContext.Result;
        }
    }
}
