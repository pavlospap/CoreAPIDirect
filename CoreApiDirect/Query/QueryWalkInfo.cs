using System.Linq;

namespace CoreApiDirect.Query
{
    internal class QueryWalkInfo<TEntity> : QueryWalkInfoBase
    {
        public IQueryable<TEntity> Query { get; set; }
    }
}
