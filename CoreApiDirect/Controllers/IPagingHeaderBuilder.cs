using CoreApiDirect.Url;
using Microsoft.AspNetCore.Mvc;

namespace CoreApiDirect.Controllers
{
    internal interface IPagingHeaderBuilder
    {
        string Build<TEntity>(ControllerBase controller, QueryString queryString, PagedList<TEntity> entityList);
    }
}
