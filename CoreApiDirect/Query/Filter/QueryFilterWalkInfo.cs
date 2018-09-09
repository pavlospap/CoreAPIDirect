using System.Linq.Expressions;
using CoreApiDirect.Infrastructure;
using CoreApiDirect.Query.Parameters;

namespace CoreApiDirect.Query.Filter
{
    internal class QueryFilterWalkInfo : WalkInfo
    {
        public QueryLogicalFilter LogicalFilter { get; set; }
        public Expression Member { get; set; }
    }
}
