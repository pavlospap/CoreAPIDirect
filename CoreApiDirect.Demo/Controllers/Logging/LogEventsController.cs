﻿using CoreApiDirect.Controllers;
using CoreApiDirect.Demo.Entities.Logging;

namespace CoreApiDirect.Demo.Controllers.Logging
{
    [ApiRoute(typeof(LogEvent))]
    public class LogEventsController : ControllerApi<int, LogEvent>
    {
    }
}
