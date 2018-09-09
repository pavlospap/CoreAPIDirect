using System.Linq.Expressions;
using CoreApiDirect.Base;
using CoreApiDirect.Infrastructure;

namespace CoreApiDirect.Query.Filter
{
    internal class QueryFilterPropertyWalker : PropertyWalker<Expression, QueryFilterWalkInfo>, IQueryFilterPropertyWalker
    {
        public QueryFilterPropertyWalker(IPropertyProvider propertyProvider)
            : base(propertyProvider)
        {
        }
    }
}
