using System.Linq.Expressions;
using CoreApiDirect.Infrastructure;

namespace CoreApiDirect.Query.Detail
{
    internal interface IOneToManyQueryDetailPropertyWalker<TEntity> : IPropertyWalker<Expression, OneToManyQueryDetailWalkInfo>
    {
    }
}
