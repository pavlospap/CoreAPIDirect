using CoreApiDirect.Controllers;
using CoreApiDirect.Demo.Entities.Logging;

namespace CoreApiDirect.Demo.Controllers.Logging
{
    [ApiRoute(typeof(LogEvent), typeof(SystemInfo))]
    public class SystemInfoController : ControllerBase<SystemInfo>
    {
    }
}
