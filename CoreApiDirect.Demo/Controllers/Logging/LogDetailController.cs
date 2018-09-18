using CoreApiDirect.Controllers;
using CoreApiDirect.Demo.Entities.Logging;

namespace CoreApiDirect.Demo.Controllers.Logging
{
    [ApiRoute(typeof(LogEvent), typeof(LogDetail))]
    public class LogDetailController : ControllerBase<int, LogDetail>
    {
    }
}
