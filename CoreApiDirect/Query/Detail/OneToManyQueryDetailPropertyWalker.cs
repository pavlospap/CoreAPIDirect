using System.Linq.Expressions;
using CoreApiDirect.Base;
using CoreApiDirect.Infrastructure;

namespace CoreApiDirect.Query.Detail
{
    internal class OneToManyQueryDetailPropertyWalker<TEntity> : PropertyWalker<Expression, OneToManyQueryDetailWalkInfo>, IOneToManyQueryDetailPropertyWalker<TEntity>
    {
        public OneToManyQueryDetailPropertyWalker(IPropertyProvider propertyProvider)
            : base(propertyProvider)
        {
        }
    }
}
