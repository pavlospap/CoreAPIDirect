using System.Collections.Generic;
using CoreApiDirect.Controllers.Filters;
using CoreApiDirect.Controllers.Results;
using CoreApiDirect.Options;
using CoreApiDirect.Response;
using CoreApiDirect.Url;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Xunit;

namespace CoreApiDirect.Tests.Controllers.Filters
{
    public class ValidateQueryStringFilterTests : FiltersTestsBase
    {
        [Fact]
        public void OnActionExecuting_NotValidate_ResultIsNull()
        {
            var actionArgs = GetActionArguments(validateQueryString: false);
            Assert.Null(GetActionResult(new ValidateQueryStringFilter(new ResponseBuilder()), new ModelStateDictionary(), actionArgs));
        }

        [Fact]
        public void OnActionExecuting_ValidateWithNoErrors_ResultIsNull()
        {
            var actionArgs = GetActionArguments(validateQueryString: true);
            Assert.Null(GetActionResult(new ValidateQueryStringFilter(new ResponseBuilder()), new ModelStateDictionary(), actionArgs));
        }

        [Fact]
        public void OnActionExecuting_ValidateWithErrors_InvalidQueryStringResult()
        {
            var actionArgs = GetActionArguments(validateQueryString: true);
            (actionArgs["queryString"] as QueryString).Errors = new List<QueryStringError>
            {
                new QueryStringError()
            };

            Assert.IsAssignableFrom<InvalidQueryStringResult>(
                GetActionResult(new ValidateQueryStringFilter(new ResponseBuilder()), new ModelStateDictionary(), actionArgs));
        }

        private Dictionary<string, object> GetActionArguments(bool validateQueryString)
        {
            return new Dictionary<string, object>
            {
                {
                    "queryString",
                    new QueryString(new CoreOptions())
                    {
                        ValidateQueryString = validateQueryString
                    }
                }
            };
        }
    }
}
