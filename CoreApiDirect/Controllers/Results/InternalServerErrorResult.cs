using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreApiDirect.Controllers.Results
{
    internal class InternalServerErrorResult : ObjectResult
    {
        public InternalServerErrorResult(object value)
            : base(value)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}
