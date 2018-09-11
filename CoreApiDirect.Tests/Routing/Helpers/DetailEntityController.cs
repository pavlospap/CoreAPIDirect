using CoreApiDirect.Controllers;

namespace CoreApiDirect.Tests.Routing.Helpers
{
    [ApiRoute(typeof(MasterEntity), typeof(DetailEntity))]
    internal class DetailEntityController : ControllerApi<int, MasterEntity>
    {
    }
}
