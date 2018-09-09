using System.Linq.Expressions;
using CoreApiDirect.Infrastructure;

namespace CoreApiDirect.Query.Detail
{
    internal interface IOneToOneQueryDetailPropertyWalkerVisitor<TEntity> : IPropertyWalkerVisitor<Expression, QueryDetailWalkInfo>
    {
    }
}
