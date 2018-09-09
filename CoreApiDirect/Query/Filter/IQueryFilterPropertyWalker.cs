using System.Linq.Expressions;
using CoreApiDirect.Infrastructure;

namespace CoreApiDirect.Query.Filter
{
    internal interface IQueryFilterPropertyWalker : IPropertyWalker<Expression, QueryFilterWalkInfo>
    {
    }
}
