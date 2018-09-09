using System;
using System.Linq.Expressions;
using CoreApiDirect.Base;

namespace CoreApiDirect.Query.Detail
{
    internal class OneToOneQueryDetailPropertyWalkerVisitor<TEntity> : QueryDetailPropertyWalkerVisitorBase<TEntity, QueryDetailWalkInfo>, IOneToOneQueryDetailPropertyWalkerVisitor<TEntity>
    {
        public OneToOneQueryDetailPropertyWalkerVisitor(
            IServiceProvider serviceProvider,
            IMethodProvider methodProvider)
            : base(serviceProvider, methodProvider)
        {
        }

        public override Expression Output(QueryDetailWalkInfo walkInfo)
        {
            return Expression.Condition(
                Expression.NotEqual(walkInfo.Member, Expression.Default(walkInfo.Type)),
                BuildMemberInitExpression(),
                Expression.Default(walkInfo.Type));
        }
    }
}
