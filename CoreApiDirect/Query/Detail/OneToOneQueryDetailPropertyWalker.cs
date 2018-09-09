using System.Linq.Expressions;
using CoreApiDirect.Base;
using CoreApiDirect.Infrastructure;

namespace CoreApiDirect.Query.Detail
{
    internal class OneToOneQueryDetailPropertyWalker<TEntity> : PropertyWalker<Expression, QueryDetailWalkInfo>, IOneToOneQueryDetailPropertyWalker<TEntity>
    {
        public OneToOneQueryDetailPropertyWalker(IPropertyProvider propertyProvider)
            : base(propertyProvider)
        {
        }
    }
}
