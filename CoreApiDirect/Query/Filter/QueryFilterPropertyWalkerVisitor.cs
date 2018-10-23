using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using CoreApiDirect.Base;
using CoreApiDirect.Entities;
using CoreApiDirect.Infrastructure;
using CoreApiDirect.Query.Operators;
using CoreApiDirect.Query.Parameters;
using Humanizer;

namespace CoreApiDirect.Query.Filter
{
    internal class QueryFilterPropertyWalkerVisitor : PropertyWalkerVisitor<Expression, QueryFilterWalkInfo>, IQueryFilterPropertyWalkerVisitor
    {
        private readonly IMethodProvider _methodProvider;
        private readonly IListProvider _listProvider;

        private Expression _expression;

        public QueryFilterPropertyWalkerVisitor(
            IServiceProvider serviceProvider,
            IMethodProvider methodProvider,
            IListProvider listProvider)
            : base(serviceProvider)
        {
            _methodProvider = methodProvider;
            _listProvider = listProvider;
        }

        public override Expression Output(QueryFilterWalkInfo walkInfo)
        {
            return _expression;
        }

        public override void Visit(PropertyInfo property, QueryFilterWalkInfo walkInfo)
        {
            var member = Expression.Property(walkInfo.Member, property);

            if (property.PropertyType.IsListOfRawGeneric(typeof(Entity<>)))
            {
                var type = property.PropertyType.BaseGenericType().GetGenericArguments()[0];

                var whereMethod = _methodProvider.MakeGenericMethod(typeof(Enumerable), "Any",
                    new Type[] { typeof(IEnumerable<>), typeof(Func<,>) },
                    new Type[] { type });

                var parameter = Expression.Parameter(type, type.Name.Camelize());
                var lambda = Expression.Lambda(NextWalk(type, walkInfo, parameter), parameter);

                _expression = Expression.Call(whereMethod, member, lambda);
            }
            else if (property.PropertyType.IsSubclassOfRawGeneric(typeof(Entity<>)))
            {
                _expression = NextWalk(property.PropertyType, walkInfo, member);
            }
            else
            {
                _expression = GetPropertyComparisonExpression(member, walkInfo.LogicalFilter);
            }
        }

        private Expression NextWalk(Type type, QueryFilterWalkInfo walkInfo, Expression member)
        {
            return NextWalk<Expression, QueryFilterWalkInfo>(new QueryFilterWalkInfo
            {
                Type = type,
                GenericDefinition = typeof(Entity<>),
                Fields = walkInfo.Fields.ToList(),
                LogicalFilter = walkInfo.LogicalFilter,
                Member = member
            }, typeof(IQueryFilterPropertyWalker), typeof(IQueryFilterPropertyWalkerVisitor));
        }

        private Expression GetPropertyComparisonExpression(Expression member, QueryLogicalFilter logicalFilter)
        {
            object value = ConvertValue(member.Type, logicalFilter.Filter.Values.FirstOrDefault());

            switch (logicalFilter.Filter.Operator)
            {
                case ComparisonOperator.Equal:
                    return Expression.Equal(member, Expression.Convert(Expression.Constant(value), member.Type));
                case ComparisonOperator.NotEqual:
                    return Expression.NotEqual(member, Expression.Convert(Expression.Constant(value), member.Type));
                case ComparisonOperator.Greater:
                    return Expression.GreaterThan(member, Expression.Convert(Expression.Constant(value), member.Type));
                case ComparisonOperator.GreaterOrEqual:
                    return Expression.GreaterThanOrEqual(member, Expression.Convert(Expression.Constant(value), member.Type));
                case ComparisonOperator.Less:
                    return Expression.LessThan(member, Expression.Convert(Expression.Constant(value), member.Type));
                case ComparisonOperator.LessOrEqual:
                    return Expression.LessThanOrEqual(member, Expression.Convert(Expression.Constant(value), member.Type));
                case ComparisonOperator.In:
                    return BuildInExpression(member, logicalFilter, isIn: true);
                case ComparisonOperator.NotIn:
                    return BuildInExpression(member, logicalFilter, isIn: false);
                case ComparisonOperator.Like:
                    return BuildLikeExpression(member, value, isLike: true);
                case ComparisonOperator.NotLike:
                    return BuildLikeExpression(member, value, isLike: false);
                case ComparisonOperator.Null:
                    return Expression.Equal(member, Expression.Constant(null));
                case ComparisonOperator.NotNull:
                    return Expression.NotEqual(member, Expression.Constant(null));
            }

            throw new NotImplementedException(nameof(ComparisonOperator));
        }

        private object ConvertValue(Type memberType, string value)
        {
            if (Nullable.GetUnderlyingType(memberType) != null)
            {
                var getNullableValueMethod = GetType().GetMethod(nameof(GetNullableValue))
                    .MakeGenericMethod(memberType.GenericTypeArguments[0]);
                return getNullableValueMethod.Invoke(this, new object[] { value });
            }
            else
            {
                return Convert.ChangeType(value, memberType);
            }
        }

        public T? GetNullableValue<T>(string value)
            where T : struct
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            var converter = TypeDescriptor.GetConverter(typeof(T?));

            return (T?)converter.ConvertFrom(value);
        }

        private Expression BuildInExpression(Expression member, QueryLogicalFilter logicalFilter, bool isIn)
        {
            var typedList = _listProvider.GetTypedList(logicalFilter.Filter.Values, member.Type, typeof(List<>));
            var listExpression = Expression.Constant(typedList);
            var containsMethod = typedList.GetType().GetMethod("Contains", new Type[] { member.Type });

            return Expression.Equal(Expression.Call(listExpression, containsMethod, member), Expression.Constant(isIn));
        }

        private Expression BuildLikeExpression(Expression member, object value, bool isLike)
        {
            var valueExpression = Expression.Constant(value);
            var containsMethod = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });

            return Expression.Equal(Expression.Call(member, containsMethod, valueExpression), Expression.Constant(isLike));
        }
    }
}
