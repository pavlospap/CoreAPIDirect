using System.Linq.Expressions;

namespace CoreApiDirect.Query.Detail
{
    internal class OneToManyQueryDetailWalkInfo : QueryDetailWalkInfo
    {
        public Expression DetailMember { get; set; }
    }
}
