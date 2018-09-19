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
            var nullExpression = Expression.Constant(null, walkInfo.Type);

            return Expression.Condition(
                Expression.NotEqual(walkInfo.Member, nullExpression),
                BuildMemberInitExpression(),
                nullExpression);
        }
    }
}
