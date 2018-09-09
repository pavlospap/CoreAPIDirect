using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreApiDirect.Controllers.Results
{
    internal class InvalidQueryStringResult : ObjectResult
    {
        public InvalidQueryStringResult(object value)
            : base(value)
        {
            StatusCode = StatusCodes.Status400BadRequest;
        }
    }
}
