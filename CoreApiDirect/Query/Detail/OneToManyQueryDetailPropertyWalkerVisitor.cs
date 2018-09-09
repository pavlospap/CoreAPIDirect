using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CoreApiDirect.Base;

namespace CoreApiDirect.Query.Detail
{
    internal class OneToManyQueryDetailPropertyWalkerVisitor<TEntity> : QueryDetailPropertyWalkerVisitorBase<TEntity, OneToManyQueryDetailWalkInfo>, IOneToManyQueryDetailPropertyWalkerVisitor<TEntity>
    {
        public OneToManyQueryDetailPropertyWalkerVisitor(
            IServiceProvider serviceProvider,
            IMethodProvider methodProvider)
            : base(serviceProvider, methodProvider)
        {
        }

        public override Expression Output(OneToManyQueryDetailWalkInfo walkInfo)
        {
            var expression = BuildWhereExpression(walkInfo);
            expression = BuildOrderExpression(expression, walkInfo);
            expression = BuildSelectExpression(expression, walkInfo);

            var toListMethod = MethodProvider.MakeGenericMethod(typeof(Enumerable), "ToList",
                new Type[] { typeof(IEnumerable<>) },
                new Type[] { walkInfo.Type });

            return Expression.Call(toListMethod, expression);
        }

        private Expression BuildWhereExpression(OneToManyQueryDetailWalkInfo walkInfo)
        {
            var filterExpression = BuildFilterExpression(walkInfo);

            if (filterExpression != null)
            {
                var whereMethod = MethodProvider.MakeGenericMethod(typeof(Enumerable), "Where",
                    new Type[] { typeof(IEnumerable<>), typeof(Func<,>) },
                    new Type[] { walkInfo.Type });

                return Expression.Call(
                        whereMethod,
                        walkInfo.DetailMember,
                        filterExpression);
            }

            return null;
        }

        private Expression BuildOrderExpression(Expression expression, OneToManyQueryDetailWalkInfo walkInfo)
        {
            var querySortList = GetSorts(walkInfo);

            for (int i = 0; i <= querySortList.Length - 1; i++)
            {
                var fieldName = querySortList[i].Field.Split('.')[querySortList[i].Field.Count(p => p == '.')];
                var property = walkInfo.Type.GetPropertyIgnoreCase(fieldName);

                expression = Expression.Call(
                    GetOrderMethod(i == 0, querySortList[i], typeof(Enumerable),
                        new Type[] { typeof(IEnumerable<>), typeof(Func<,>) },
                        new Type[] { typeof(IOrderedEnumerable<>), typeof(Func<,>) },
                        new Type[] { walkInfo.Type, property.PropertyType }),
                    expression ?? walkInfo.DetailMember,
                    GetOrderParameter(walkInfo.Type, property));
            }

            return expression;
        }

        private Expression BuildSelectExpression(Expression expression, OneToManyQueryDetailWalkInfo walkInfo)
        {
            var memberInitializationExpression = Expression.Lambda(
                typeof(Func<,>).MakeGenericType(walkInfo.Type, walkInfo.Type),
                BuildMemberInitExpression(),
                walkInfo.Member as ParameterExpression);

            var selectMethod = MethodProvider.MakeGenericMethod(typeof(Enumerable), "Select",
                new Type[] { typeof(IEnumerable<>), typeof(Func<,>) },
                new Type[] { walkInfo.Type, walkInfo.Type });

            return Expression.Call(
                selectMethod,
                expression ?? walkInfo.DetailMember,
                memberInitializationExpression);
        }
    }
}
