using System;
using System.Linq;
using CoreApiDirect.Base;
using CoreApiDirect.Infrastructure;

namespace CoreApiDirect.Query
{
    internal class QueryPropertyWalker<TEntity> : PropertyWalker<IQueryable<TEntity>, QueryWalkInfo<TEntity>>, IQueryPropertyWalker<TEntity>
    {
        public QueryPropertyWalker(IPropertyProvider propertyProvider)
            : base(propertyProvider)
        {
        }

        protected override Type GetWalkInfoTypeForProperty(QueryWalkInfo<TEntity> walkInfo)
        {
            return typeof(QueryWalkInfo<>).MakeGenericType(walkInfo.Type);
        }
    }
}
