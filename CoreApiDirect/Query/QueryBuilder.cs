using System;
using System.Linq;
using System.Linq.Expressions;
using CoreApiDirect.Base;
using CoreApiDirect.Entities;
using CoreApiDirect.Mapping;
using CoreApiDirect.Query.Parameters;
using CoreApiDirect.Url;
using Humanizer;

namespace CoreApiDirect.Query
{
    internal class QueryBuilder<TEntity> : IQueryBuilder<TEntity>
    {
        private readonly IQueryPropertyWalker<TEntity> _walker;
        private readonly IQueryPropertyWalkerVisitor<TEntity> _visitor;
        private readonly IQueryParamsMapper _queryParamsMapper;

        public QueryBuilder(
            IQueryPropertyWalker<TEntity> walker,
            IQueryPropertyWalkerVisitor<TEntity> visitor)
        {
            _walker = walker;
            _visitor = visitor;
        }

        public QueryBuilder(
            IQueryPropertyWalker<TEntity> walker,
            IQueryPropertyWalkerVisitor<TEntity> visitor,
            IQueryParamsMapper queryParamsMapper)
            : this(walker, visitor)
        {
            _queryParamsMapper = queryParamsMapper;
        }

        public IQueryable<TEntity> Build(IQueryable<TEntity> query, QueryString queryString)
        {
            var type = typeof(TEntity);
            var queryParams = GetMappedQueryParams(queryString, type);

            return _walker.Accept(_visitor, new QueryWalkInfo<TEntity>
            {
                Type = type,
                GenericDefinition = typeof(Entity<>),
                Fields = queryParams.Fields,
                RelatedDataLevel = queryString.RelatedDataLevel,
                Query = query,
                QueryParams = queryParams,
                Member = Expression.Parameter(type, type.Name.Camelize())
            });
        }

        private QueryParams GetMappedQueryParams(QueryString queryString, Type entityType)
        {
            return _queryParamsMapper == null ?
                queryString.QueryParams :
                _queryParamsMapper.MapQueryParams(entityType, queryString.QueryParams.Copy());
        }
    }
}
