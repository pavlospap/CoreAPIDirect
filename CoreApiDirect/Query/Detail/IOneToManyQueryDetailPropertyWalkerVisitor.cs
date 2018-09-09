using System.Linq.Expressions;
using CoreApiDirect.Infrastructure;

namespace CoreApiDirect.Query.Detail
{
    internal interface IOneToManyQueryDetailPropertyWalkerVisitor<TEntity> : IPropertyWalkerVisitor<Expression, OneToManyQueryDetailWalkInfo>
    {
    }
}
