using System.Linq;
using CoreApiDirect.Infrastructure;

namespace CoreApiDirect.Query
{
    internal interface IQueryPropertyWalkerVisitor<TEntity> : IPropertyWalkerVisitor<IQueryable<TEntity>, QueryWalkInfo<TEntity>>
    {
    }
}
