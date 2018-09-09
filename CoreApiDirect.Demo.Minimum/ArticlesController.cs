using CoreApiDirect.Controllers;

namespace CoreApiDirect.Demo.Minimum
{
    [ApiRoute(typeof(Article))]
    public class ArticlesController : ControllerApi<int, Article>
    {
    }
}
