using System;
using System.Collections.Generic;
using CoreApiDirect.Base;
using CoreApiDirect.Controllers;
using CoreApiDirect.Demo.Dto.Out.App;
using CoreApiDirect.Tests.Controllers.Helpers;
using CoreApiDirect.Tests.Options;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Xunit;

namespace CoreApiDirect.Tests.Controllers
{
    public class ModelStateResolverTests
    {
        [Fact]
        public void GetModelErrors_ModelStateWithErrors_ErrorList()
        {
            var modelState = new ModelStateDictionary();
            modelState.AddModelError("field1", "field1Message");
            modelState.AddModelError("field2", "field2Message");
            modelState.AddModelError("field1", new Exception("field1Exception"), ModelMetadataInitializer.ForType(typeof(SchoolOutDto)));
            modelState.AddModelError("field2", new Exception("field2Exception"), ModelMetadataInitializer.ForType(typeof(SchoolOutDto)));

            var expectedErrors = new List<ModelStateError>
            {
                new ModelStateError("field1", "field1Message"),
                new ModelStateError("field1", "field1Exception"),
                new ModelStateError("field2", "field2Message"),
                new ModelStateError("field2", "field2Exception"),
            };

            var modelStateResolver = new ModelStateResolver(new FieldNameResolver(new MvcJsonOptionsTests()));

            Assert.Equal(expectedErrors.ToJson(), modelStateResolver.GetModelErrors(modelState).ToJson());
        }
    }
}
