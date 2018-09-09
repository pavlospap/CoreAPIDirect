using System.Collections.Generic;
using CoreApiDirect.Controllers;
using CoreApiDirect.Controllers.Filters;
using CoreApiDirect.Controllers.Results;
using CoreApiDirect.Demo.Dto.In.App;
using CoreApiDirect.Response;
using CoreApiDirect.Tests.Options;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Xunit;

namespace CoreApiDirect.Tests.Controllers.Filters
{
    public class ValidateDtoFilterTests : FiltersTestsBase
    {
        private readonly ModelStateResolver _modelStateResolver = new ModelStateResolver(new FieldNameResolver(new MvcJsonOptionsTests()));

        [Fact]
        public void OnActionExecuting_DtoIsMissing_UnprocessableEntityResult()
        {
            Assert.IsAssignableFrom<ApiUnprocessableEntityResult>(
                GetActionResult(new ValidateDtoFilter(new ResponseBuilder(), _modelStateResolver), new ModelStateDictionary(), new Dictionary<string, object>()));
        }

        [Fact]
        public void OnActionExecuting_DtoIsNull_UnprocessableEntityResult()
        {
            Assert.IsAssignableFrom<ApiUnprocessableEntityResult>(
                GetActionResult(new ValidateDtoFilter(new ResponseBuilder(), _modelStateResolver), new ModelStateDictionary(), new Dictionary<string, object>() { { "dto", null } }));
        }

        [Fact]
        public void OnActionExecuting_InvalidModelState_UnprocessableEntityResult()
        {
            var modelState = new ModelStateDictionary();
            modelState.AddModelError("errorKey", "errorMessage");
            Assert.IsAssignableFrom<ApiUnprocessableEntityResult>(
                GetActionResult(new ValidateDtoFilter(new ResponseBuilder(), _modelStateResolver), modelState, new Dictionary<string, object>() { { "dto", new SchoolInDto() } }));
        }

        [Fact]
        public void OnActionExecuting_ValidDto_ResultIsNull()
        {
            Assert.Null(GetActionResult(new ValidateDtoFilter(new ResponseBuilder(), _modelStateResolver), new ModelStateDictionary(), new Dictionary<string, object>() { { "dto", new SchoolInDto() } }));
        }
    }
}
