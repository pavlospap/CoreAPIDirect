using System.Linq.Expressions;
using CoreApiDirect.Infrastructure;
using CoreApiDirect.Query.Parameters;

namespace CoreApiDirect.Query
{
    internal abstract class QueryWalkInfoBase : WalkInfo
    {
        public QueryParams QueryParams { get; set; }
        public Expression Member { get; set; }
    }
}
