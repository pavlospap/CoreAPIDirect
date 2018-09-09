using System.Linq;
using CoreApiDirect.Infrastructure;

namespace CoreApiDirect.Query
{
    internal interface IQueryPropertyWalker<TEntity> : IPropertyWalker<IQueryable<TEntity>, QueryWalkInfo<TEntity>>
    {
    }
}
