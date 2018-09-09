using System;
using System.Linq.Expressions;
using System.Reflection;
using CoreApiDirect.Base;

namespace CoreApiDirect.Query.Detail
{
    internal abstract class QueryDetailPropertyWalkerVisitorBase<TEntity, TWalkInfo> : QueryPropertyWalkerVisitorBase<TEntity, Expression, TWalkInfo>
        where TWalkInfo : QueryDetailWalkInfo
    {
        public QueryDetailPropertyWalkerVisitorBase(
            IServiceProvider serviceProvider,
            IMethodProvider methodProvider)
            : base(serviceProvider, methodProvider)
        {
        }

        protected override string GetNextFieldMemberChain(PropertyInfo property, TWalkInfo walkInfo)
        {
            return walkInfo.FieldMemberChain != null ? walkInfo.FieldMemberChain + "." + property.Name : property.Name;
        }

        protected override string GetFilterFieldPrefix(TWalkInfo walkInfo)
        {
            return walkInfo.FieldMemberChain + "!.";
        }

        protected override string GetSortFieldPrefix(TWalkInfo walkInfo)
        {
            return walkInfo.FieldMemberChain + ".";
        }
    }
}
