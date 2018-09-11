using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using CoreApiDirect.Base;
using CoreApiDirect.Controllers;
using CoreApiDirect.Entities;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace CoreApiDirect.Routing
{
    internal class RouteFilterBuilder : IRouteFilterBuilder
    {
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly IPropertyProvider _propertyProvider;
        private readonly IMethodProvider _methodProvider;

        public RouteFilterBuilder(
            IActionContextAccessor actionContextAccessor,
            IPropertyProvider propertyProvider,
            IMethodProvider methodProvider)
        {
            _actionContextAccessor = actionContextAccessor;
            _propertyProvider = propertyProvider;
            _methodProvider = methodProvider;
        }

        public Expression BuildFilter(Type controllerType)
        {
            var routeEntityTypes = controllerType.GetCustomAttribute<ApiRouteAttribute>().RouteEntityTypes;
            return BuildFilter(routeEntityTypes);
        }

        private Expression BuildFilter(IEnumerable<Type> routeEntityTypes)
        {
            var parameter = Expression.Parameter(routeEntityTypes.Last());

            Expression expression = null;
            Expression memberChain = parameter;

            for (int i = routeEntityTypes.Count() - 1; i >= 1; i--)
            {
                var masterType = routeEntityTypes.ElementAt(i - 1);
                var detailType = routeEntityTypes.ElementAt(i);

                bool isManyToMany = false;
                var relationExpression = BuildMasterDetailRelationExpression(ref isManyToMany, ref memberChain, masterType, detailType, routeEntityTypes, i);

                AppendExpression(ref expression, relationExpression);

                if (isManyToMany)
                {
                    break;
                }
            }

            return BuildFinalExpression(parameter, expression);
        }

        private Expression BuildMasterDetailRelationExpression(ref bool isManyToMany, ref Expression memberChain, Type masterType, Type detailType, IEnumerable<Type> routeEntityTypes, int currentEntityIndex)
        {
            var masterIdProperty = detailType.GetPropertyIgnoreCase(masterType.Name + "id");

            if (masterIdProperty != null)
            {
                var relation = Expression.Equal(
                    Expression.Property(memberChain, masterIdProperty),
                    Expression.Constant(Convert.ChangeType(_actionContextAccessor.GetRouteParamIgnoreCase(masterIdProperty.Name), masterIdProperty.PropertyType)));

                var masterObjectProperty = detailType.GetProperty(masterType.Name);

                if (masterObjectProperty == null)
                {
                    throw new InvalidOperationException($"Type '{detailType.Name}' does not contain a '{masterType.Name}' property.");
                }

                AppendMemberChain(ref memberChain, masterObjectProperty);

                return relation;
            }

            isManyToMany = true;

            return BuildMasterDetailManyToManyRelationExpression(memberChain, masterType, detailType, routeEntityTypes, currentEntityIndex);
        }

        private void AppendMemberChain(ref Expression memberChain, PropertyInfo property)
        {
            memberChain = Expression.Property(memberChain, property);
        }

        private Expression BuildMasterDetailManyToManyRelationExpression(Expression memberChain, Type masterType, Type detailType, IEnumerable<Type> routeEntityTypes, int currentRouteEntityTypeIndex)
        {
            var listProperties = _propertyProvider.GetProperties(detailType).Where(p => p.PropertyType.IsListOfRawGeneric(typeof(Entity<>)));

            foreach (var detailProperty in listProperties)
            {
                var manyToManyType = detailProperty.PropertyType.BaseGenericType().GenericTypeArguments[0];
                var masterIdProperty = manyToManyType.GetPropertyIgnoreCase(masterType.Name + "id");

                if (masterIdProperty != null)
                {
                    var unprocessedRouteEntityTypes = GetUnprocessedRouteEntityTypes(routeEntityTypes, currentRouteEntityTypeIndex);
                    unprocessedRouteEntityTypes.Add(manyToManyType);

                    return Expression.Call(
                        _methodProvider.MakeGenericMethod(typeof(Enumerable), "Any",
                            new Type[] { typeof(IEnumerable<>), typeof(Func<,>) },
                            new Type[] { manyToManyType }),
                        Expression.Property(memberChain, detailProperty),
                        BuildFilter(unprocessedRouteEntityTypes));
                }
            }

            return null;
        }

        private List<Type> GetUnprocessedRouteEntityTypes(IEnumerable<Type> routeEntityTypes, int currentRouteEntityTypeIndex)
        {
            return new List<Type>(routeEntityTypes.Take(currentRouteEntityTypeIndex));
        }

        private void AppendExpression(ref Expression expression, Expression newExpression)
        {
            expression = expression != null ? Expression.AndAlso(expression, newExpression) : newExpression;
        }

        private LambdaExpression BuildFinalExpression(ParameterExpression parameter, Expression expression)
        {
            return expression != null ? Expression.Lambda(expression, parameter) : null;
        }
    }
}
