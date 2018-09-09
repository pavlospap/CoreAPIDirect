using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using CoreApiDirect.Base;
using CoreApiDirect.Entities;
using CoreApiDirect.Infrastructure;
using CoreApiDirect.Query.Detail;
using CoreApiDirect.Query.Filter;
using CoreApiDirect.Query.Operators;
using CoreApiDirect.Query.Parameters;
using Humanizer;
using Microsoft.Extensions.DependencyInjection;

namespace CoreApiDirect.Query
{
    internal abstract class QueryPropertyWalkerVisitorBase<TEntity, TResult, TWalkInfo> : PropertyWalkerVisitor<TResult, TWalkInfo>
        where TWalkInfo : QueryWalkInfoBase
    {
        protected readonly IMethodProvider MethodProvider;

        private List<MemberAssignment> _memberAssignments = new List<MemberAssignment>();

        public QueryPropertyWalkerVisitorBase(
            IServiceProvider serviceProvider,
            IMethodProvider methodProvider)
            : base(serviceProvider)
        {
            MethodProvider = methodProvider;
        }

        public override void Visit(PropertyInfo property, TWalkInfo walkInfo)
        {
            var member = Expression.Property(walkInfo.Member, property);

            if (property.PropertyType.IsListOfRawGeneric(typeof(Entity<>)))
            {
                var type = property.PropertyType.BaseGenericType().GetGenericArguments()[0];
                var parameter = Expression.Parameter(type, type.Name.Camelize());
                var nextWalkInfo = GetNextWalkInfo<OneToManyQueryDetailWalkInfo>(property, type, parameter, walkInfo);
                nextWalkInfo.DetailMember = member;
                AddMemberAssignment(property, NextWalk<Expression, OneToManyQueryDetailWalkInfo>(
                    nextWalkInfo,
                    typeof(IOneToManyQueryDetailPropertyWalker<>).MakeGenericType(type),
                    typeof(IOneToManyQueryDetailPropertyWalkerVisitor<>).MakeGenericType(type)) as Expression);
            }
            else if (property.PropertyType.IsSubclassOfRawGeneric(typeof(Entity<>)))
            {
                var type = property.PropertyType;
                var nextWalkInfo = GetNextWalkInfo<QueryDetailWalkInfo>(property, type, member, walkInfo);
                AddMemberAssignment(property, NextWalk<Expression, QueryDetailWalkInfo>(
                    nextWalkInfo,
                    typeof(IOneToOneQueryDetailPropertyWalker<>).MakeGenericType(type),
                    typeof(IOneToOneQueryDetailPropertyWalkerVisitor<>).MakeGenericType(type)) as Expression);
            }
            else
            {
                AddMemberAssignment(property, member);
            }
        }

        private TNextWalkInfo GetNextWalkInfo<TNextWalkInfo>(PropertyInfo property, Type type, Expression member, TWalkInfo walkInfo)
            where TNextWalkInfo : QueryDetailWalkInfo, new()
        {
            return new TNextWalkInfo
            {
                Type = type,
                GenericDefinition = type.BaseGenericType().GetGenericTypeDefinition(),
                WalkedTypes = walkInfo.WalkedTypes.ToList(),
                Fields = walkInfo.Fields,
                RelatedDataLevel = walkInfo.RelatedDataLevel,
                Member = member,
                QueryParams = walkInfo.QueryParams,
                FieldMemberChain = GetNextFieldMemberChain(property, walkInfo)
            };
        }

        protected abstract string GetNextFieldMemberChain(PropertyInfo property, TWalkInfo walkInfo);

        private void AddMemberAssignment(PropertyInfo property, Expression expression)
        {
            _memberAssignments.Add(Expression.Bind(property, expression));
        }

        protected Expression BuildMemberInitExpression()
        {
            return Expression.MemberInit(Expression.New(typeof(TEntity)), _memberAssignments);
        }

        protected Expression BuildFilterExpression(TWalkInfo walkInfo)
        {
            var type = typeof(TEntity);
            var parameter = Expression.Parameter(type, type.Name.Camelize());
            Expression expression = null;

            var logicalFilters = GetLogicalFilters(walkInfo);

            foreach (var logicalFilter in logicalFilters)
            {
                string filterPrefix = GetFilterFieldPrefix(walkInfo);
                string field = string.IsNullOrEmpty(filterPrefix) ? logicalFilter.Filter.Field : logicalFilter.Filter.Field.Replace(filterPrefix, "", StringComparison.OrdinalIgnoreCase);

                var walker = ServiceProvider.GetRequiredService<IQueryFilterPropertyWalker>();
                var visitor = ServiceProvider.GetRequiredService<IQueryFilterPropertyWalkerVisitor>();

                Expression comparisonExpression = walker.Accept(visitor, new QueryFilterWalkInfo
                {
                    Type = type,
                    GenericDefinition = typeof(Entity<>),
                    Fields = GenerateWalkInfoFields(field),
                    LogicalFilter = logicalFilter,
                    Member = parameter
                });

                expression = AppendExpression(logicalFilter, expression, comparisonExpression);
            }

            return expression != null ? Expression.Lambda(expression, parameter) : null;
        }

        private QueryLogicalFilter[] GetLogicalFilters(TWalkInfo walkInfo)
        {
            string fieldPrefix = GetFilterFieldPrefix(walkInfo);
            return walkInfo.QueryParams.Filter.Where(p => p.Filter.Field.StartsWith(fieldPrefix, StringComparison.OrdinalIgnoreCase) && !p.Filter.Field.Substring(fieldPrefix.Length).Contains("!")).ToArray();
        }

        protected abstract string GetFilterFieldPrefix(TWalkInfo walkInfo);

        private IEnumerable<string> GenerateWalkInfoFields(string field)
        {
            var fieldParts = field.Split('.').ToList();
            var fields = new List<string>();
            for (int i = 1; i <= fieldParts.Count; i++)
            {
                fields.Add(string.Join('.', fieldParts.GetRange(0, i)));
            }

            return fields;
        }

        private Expression AppendExpression(QueryLogicalFilter filter, Expression currentExpression, Expression newExpression)
        {
            return currentExpression != null ?
                (filter.Operator == LogicalOperator.And ? Expression.AndAlso(currentExpression, newExpression) : Expression.OrElse(currentExpression, newExpression)) :
                newExpression;
        }

        protected QuerySort[] GetSorts(TWalkInfo walkInfo)
        {
            string fieldPrefix = GetSortFieldPrefix(walkInfo);
            return walkInfo.QueryParams.Sort.Where(p => p.Field.StartsWith(fieldPrefix, StringComparison.OrdinalIgnoreCase) && !p.Field.Substring(fieldPrefix.Length).Contains(".")).ToArray();
        }

        protected abstract string GetSortFieldPrefix(TWalkInfo walkInfo);

        protected MethodInfo GetOrderMethod(bool isFirst, QuerySort querySort, Type type, Type[] parameterDefinitions, Type[] chainedOrderParameterDefinitions, Type[] typeArguments)
        {
            if (isFirst)
            {
                return querySort.Direction == SortDirection.Ascending ?
                    MethodProvider.MakeGenericMethod(type, "OrderBy", parameterDefinitions, typeArguments) :
                    MethodProvider.MakeGenericMethod(type, "OrderByDescending", parameterDefinitions, typeArguments);
            }

            return querySort.Direction == SortDirection.Ascending ?
                MethodProvider.MakeGenericMethod(type, "ThenBy", chainedOrderParameterDefinitions, typeArguments) :
                MethodProvider.MakeGenericMethod(type, "ThenByDescending", chainedOrderParameterDefinitions, typeArguments);
        }

        protected Expression GetOrderParameter(Type type, PropertyInfo property)
        {
            var parameter = Expression.Parameter(type, type.Name.Camelize());
            var member = Expression.Property(parameter, property);

            return Expression.Lambda(member, parameter);
        }
    }
}
