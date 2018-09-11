using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using CoreApiDirect.Base;
using CoreApiDirect.Entities;
using Humanizer;

namespace CoreApiDirect.Query
{
    internal class QueryPropertyWalkerVisitor<TEntity> : QueryPropertyWalkerVisitorBase<TEntity, IQueryable<TEntity>, QueryWalkInfo<TEntity>>, IQueryPropertyWalkerVisitor<TEntity>
    {
        private readonly IPropertyProvider _propertyProvider;

        public QueryPropertyWalkerVisitor(
            IServiceProvider serviceProvider,
            IPropertyProvider propertyProvider,
            IMethodProvider methodProvider)
            : base(serviceProvider, methodProvider)
        {
            _propertyProvider = propertyProvider;
        }

        public override IQueryable<TEntity> Output(QueryWalkInfo<TEntity> walkInfo)
        {
            var query = ApplySearch(walkInfo);
            query = ApplyWhere(query, walkInfo);
            query = ApplyOrder(query, walkInfo);
            query = ApplySelect(query, walkInfo);

            return query;
        }

        private IQueryable<TEntity> ApplySearch(QueryWalkInfo<TEntity> walkInfo)
        {
            if (string.IsNullOrWhiteSpace(walkInfo.QueryParams.Search))
            {
                return walkInfo.Query;
            }

            var searchKeyProperties = _propertyProvider.GetProperties(typeof(TEntity)).Where(p => p.GetCustomAttribute<SearchKeyAttribute>() != null);

            if (searchKeyProperties.Where(p => p.PropertyType != typeof(string)).Any())
            {
                throw new InvalidOperationException("CoreApiDirect.Entities.SearchKeyAttribute is added to a non string property.");
            }

            if (!searchKeyProperties.Any())
            {
                return walkInfo.Query;
            }

            var type = typeof(TEntity);
            var parameter = Expression.Parameter(type, type.Name.Camelize());
            Expression expressionOr = null;

            foreach (var searchKeyProperty in searchKeyProperties)
            {
                var property = Expression.Property(parameter, searchKeyProperty.Name);
                var value = Expression.Constant(walkInfo.QueryParams.Search);
                var containsMethod = typeof(String).GetMethod("Contains", new Type[] { typeof(String) });
                var expression = Expression.Equal(Expression.Call(property, containsMethod, value), Expression.Constant(true));
                expressionOr = expressionOr == null ? expression : Expression.OrElse(expressionOr, expression);
            }

            return walkInfo.Query.Where(Expression.Lambda<Func<TEntity, bool>>(expressionOr, parameter));
        }

        private IQueryable<TEntity> ApplyWhere(IQueryable<TEntity> query, QueryWalkInfo<TEntity> walkInfo)
        {
            var expression = BuildFilterExpression(walkInfo);

            if (expression != null)
            {
                query = query.Where((Expression<Func<TEntity, bool>>)expression);
            }

            return query;
        }

        private IQueryable<TEntity> ApplyOrder(IQueryable<TEntity> query, QueryWalkInfo<TEntity> walkInfo)
        {
            var querySortList = GetSorts(walkInfo);
            var type = typeof(TEntity);

            for (int i = 0; i <= querySortList.Length - 1; i++)
            {
                var property = type.GetPropertyIgnoreCase(querySortList[i].Field);

                query = GetOrderMethod(i == 0, querySortList[i], typeof(Queryable),
                    new Type[] { typeof(IQueryable<>), typeof(Expression<>) },
                    new Type[] { typeof(IOrderedQueryable<>), typeof(Expression<>) },
                    new Type[] { type, property.PropertyType }).Invoke(query, new object[]
                {
                    query,
                    GetOrderParameter(type, property)
                }) as IQueryable<TEntity>;
            }

            return query;
        }

        private IQueryable<TEntity> ApplySelect(IQueryable<TEntity> query, QueryWalkInfo<TEntity> walkInfo)
        {
            return query.Select(Expression.Lambda<Func<TEntity, TEntity>>(BuildMemberInitExpression(), walkInfo.Member as ParameterExpression));
        }

        protected override string GetNextFieldMemberChain(PropertyInfo property, QueryWalkInfo<TEntity> walkInfo)
        {
            return property.Name;
        }

        protected override string GetFilterFieldPrefix(QueryWalkInfo<TEntity> walkInfo)
        {
            return "";
        }

        protected override string GetSortFieldPrefix(QueryWalkInfo<TEntity> walkInfo)
        {
            return "";
        }
    }
}
