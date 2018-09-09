using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreApiDirect.Controllers.Results
{
    internal class NoChangesToSaveResult : ObjectResult
    {
        public NoChangesToSaveResult(object value)
            : base(value)
        {
            StatusCode = StatusCodes.Status204NoContent;
        }
    }
}
