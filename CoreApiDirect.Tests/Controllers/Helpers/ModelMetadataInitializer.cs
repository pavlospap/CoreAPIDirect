using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Moq;

namespace CoreApiDirect.Tests.Controllers.Helpers
{
    internal class ModelMetadataInitializer
    {
        public static ModelMetadata ForType(Type modelType)
        {
            var mockModelMetadataProvider = new Mock<IModelMetadataProvider>();
            var mockCompositeMetadataDetailsProvider = new Mock<ICompositeMetadataDetailsProvider>();
            var defaultMetadataDetails = new DefaultMetadataDetails(ModelMetadataIdentity.ForType(modelType), ModelAttributes.GetAttributesForType(typeof(List<object>)));

            return new DefaultModelMetadata(mockModelMetadataProvider.Object, mockCompositeMetadataDetailsProvider.Object, defaultMetadataDetails);
        }
    }
}
