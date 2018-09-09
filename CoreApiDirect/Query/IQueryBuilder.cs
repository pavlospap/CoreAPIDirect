using System.Linq;
using CoreApiDirect.Url;

namespace CoreApiDirect.Query
{
    internal interface IQueryBuilder<TEntity>
    {
        IQueryable<TEntity> Build(IQueryable<TEntity> query, QueryString queryString);
    }
}
