using System.Linq.Expressions;
using CoreApiDirect.Infrastructure;

namespace CoreApiDirect.Query.Detail
{
    internal interface IOneToOneQueryDetailPropertyWalker<TEntity> : IPropertyWalker<Expression, QueryDetailWalkInfo>
    {
    }
}
