using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreApiDirect.Controllers.Results
{
    internal class ApiUnprocessableEntityResult : ObjectResult
    {
        public ApiUnprocessableEntityResult(object value)
            : base(value)
        {
            StatusCode = StatusCodes.Status422UnprocessableEntity;
        }
    }
}
