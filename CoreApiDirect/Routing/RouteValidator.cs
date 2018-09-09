using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using CoreApiDirect.Base;
using CoreApiDirect.Controllers;
using CoreApiDirect.Entities;
using CoreApiDirect.Repositories;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace CoreApiDirect.Routing
{
    internal class RouteValidator : IRouteValidator
    {
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly IPropertyProvider _propertyProvider;
        private readonly IMethodProvider _methodProvider;

        private IEnumerable<Type> _routeEntityTypes;
        private IEnumerable<object> _ids;

        public RouteValidator(
            IActionContextAccessor actionContextAccessor,
            IPropertyProvider propertyProvider,
            IMethodProvider methodProvider)
        {
            _actionContextAccessor = actionContextAccessor;
            _propertyProvider = propertyProvider;
            _methodProvider = methodProvider;
        }

        public async Task<RecordError> ValidateRoute(Type controllerType, IEnumerable<object> ids)
        {
            _routeEntityTypes = controllerType.GetCustomAttribute<ApiRouteAttribute>().RouteEntityTypes;
            _ids = ids;

            foreach (var validation in new List<Func<Task<RecordError>>>()
            {
                ValidateParentEntitiesExistence,
                ValidateEntityExistence,
                ValidateParentEntitiesRelations,
                ValidateEntityRelations
            })
            {
                var result = await validation();
                if (result != null)
                {
                    return result;
                }
            }

            return await Task.FromResult<RecordError>(null);
        }

        private async Task<RecordError> ValidateParentEntitiesExistence()
        {
            for (int i = 0; i <= _routeEntityTypes.Count() - 2; i++)
            {
                var entityType = _routeEntityTypes.ElementAt(i);
                var id = Convert.ChangeType(_actionContextAccessor.GetRouteParamIgnoreCase(entityType.Name + "id"), entityType.BaseGenericType().GenericTypeArguments[0]);

                var result = await ValidateEntityExistence(entityType, id);
                if (result != null)
                {
                    return result;
                }
            }

            return await Task.FromResult<RecordError>(null);
        }

        private async Task<RecordError> ValidateEntityExistence()
        {
            var entityType = _routeEntityTypes.Last();

            foreach (var id in _ids)
            {
                var result = await ValidateEntityExistence(entityType, id);
                if (result != null)
                {
                    return result;
                }
            }

            return await Task.FromResult<RecordError>(null);
        }

        private async Task<RecordError> ValidateEntityExistence(Type entityType, object id)
        {
            var expression = BuildEntityExistenceExpression(entityType, out ParameterExpression parameter, id);
            var lambda = Expression.Lambda(expression, parameter);

            if (!await EntityExists(entityType, lambda))
            {
                return new RecordError
                {
                    ErrorType = RecordErrorType.RecordNotExist,
                    EntityType = entityType,
                    EntityId = id
                };
            }

            return await Task.FromResult<RecordError>(null);
        }

        private Expression BuildEntityExistenceExpression(Type entityType, out ParameterExpression parameter, object id)
        {
            parameter = Expression.Parameter(entityType);
            var idProperty = entityType.GetPropertyIgnoreCase("id");

            return Expression.Equal(
                Expression.Property(parameter, idProperty),
                Expression.Constant(Convert.ChangeType(id, idProperty.PropertyType)));
        }

        private async Task<bool> EntityExists(Type entityType, LambdaExpression filter)
        {
            var genericExpression = MakeGenericExpression(entityType);
            var repository = _actionContextAccessor.ActionContext.HttpContext.RequestServices.GetService(typeof(IRepository<,>).MakeGenericType(new Type[] { entityType, entityType.BaseGenericType().GenericTypeArguments[0] }));
            var findAsyncMethod = repository.GetType().GetMethod("FindNoTrackingAsync", new Type[] { genericExpression });
            var convertReturnTypeMethod = GetType().GetMethod(nameof(ConvertReturnType), BindingFlags.Instance | BindingFlags.NonPublic).MakeGenericMethod(entityType);
            var entity = await (Task<object>)convertReturnTypeMethod.Invoke(this, new object[] { findAsyncMethod, repository, filter });

            return entity != null;
        }

        private Type MakeGenericExpression(Type entityType)
        {
            var funcType = typeof(Func<,>).MakeGenericType(new Type[] { entityType, typeof(bool) });
            return typeof(Expression<>).MakeGenericType(new Type[] { funcType });
        }

        private async Task<object> ConvertReturnType<TEntity>(MethodInfo findAsyncMethod, object repository, LambdaExpression filter)
        {
            return await (Task<TEntity>)findAsyncMethod.Invoke(repository, new object[] { filter });
        }

        private async Task<RecordError> ValidateParentEntitiesRelations()
        {
            for (int i = 0; i <= _routeEntityTypes.Count() - 3; i++)
            {
                var masterType = _routeEntityTypes.ElementAt(i);
                var detailType = _routeEntityTypes.ElementAt(i + 1);

                var detailId = Convert.ChangeType(_actionContextAccessor.GetRouteParamIgnoreCase(detailType.Name + "id"), detailType.BaseGenericType().GenericTypeArguments[0]);

                var result = await ValidateEntitiesRelation(masterType, detailType, detailId);
                if (result != null)
                {
                    return result;
                }
            }

            return await Task.FromResult<RecordError>(null);
        }

        private async Task<RecordError> ValidateEntityRelations()
        {
            if (_routeEntityTypes.Count() < 2)
            {
                return await Task.FromResult<RecordError>(null);
            }

            var masterType = _routeEntityTypes.ElementAt(_routeEntityTypes.Count() - 2);
            var detailType = _routeEntityTypes.Last();

            foreach (var id in _ids)
            {
                var result = await ValidateEntitiesRelation(masterType, detailType, id);
                if (result != null)
                {
                    return result;
                }
            }

            return await Task.FromResult<RecordError>(null);
        }

        private async Task<RecordError> ValidateEntitiesRelation(Type masterType, Type detailType, object detailId)
        {
            var masterId = Convert.ChangeType(_actionContextAccessor.GetRouteParamIgnoreCase(masterType.Name + "id"), masterType.BaseGenericType().GenericTypeArguments[0]);
            var expression = BuildMasterDetailRelationExpression(masterType, detailType, masterId, detailId);

            if (!await EntityExists(detailType, expression))
            {
                return new RecordError
                {
                    ErrorType = RecordErrorType.RecordRelationNotValid,
                    EntityType = detailType,
                    EntityId = detailId,
                    ParentEntityType = masterType,
                    ParentEntityId = masterId
                };
            }

            return await Task.FromResult<RecordError>(null);
        }

        private LambdaExpression BuildMasterDetailRelationExpression(Type masterType, Type detailType, object masterId, object detailId)
        {
            var existenceExpression = BuildEntityExistenceExpression(detailType, out ParameterExpression parameter, detailId);
            var masterIdProperty = detailType.GetPropertyIgnoreCase(masterType.Name + "id");

            var relationExpression = masterIdProperty != null ?
                Expression.Equal(
                    Expression.Property(parameter, masterIdProperty),
                    Expression.Constant(Convert.ChangeType(masterId, masterIdProperty.PropertyType))) :
                BuildMasterDetailManyToManyRelationExpression(parameter, masterType, detailType, masterId);

            return Expression.Lambda(Expression.AndAlso(existenceExpression, relationExpression), parameter);
        }

        private Expression BuildMasterDetailManyToManyRelationExpression(ParameterExpression parameter, Type masterType, Type detailType, object masterId)
        {
            var properties = _propertyProvider.GetProperties(detailType).Where(p => p.PropertyType.IsListOfRawGeneric(typeof(Entity<>)));

            foreach (var property in properties)
            {
                var propertyGenericArgument = property.PropertyType.BaseGenericType().GenericTypeArguments[0];
                var masterIdProperty = propertyGenericArgument.GetPropertyIgnoreCase(masterType.Name + "id");

                if (masterIdProperty != null)
                {
                    return Expression.Call(
                        _methodProvider.MakeGenericMethod(typeof(Enumerable), "Any",
                            new Type[] { typeof(IEnumerable<>), typeof(Func<,>) },
                            new Type[] { propertyGenericArgument }),
                        Expression.Property(parameter, property),
                        BuildAnyExpresion(propertyGenericArgument, masterIdProperty, masterId));
                }
            }

            return null;
        }

        private LambdaExpression BuildAnyExpresion(Type collectionPropertyGenericType, PropertyInfo masterIdProperty, object masterId)
        {
            var parameter = Expression.Parameter(collectionPropertyGenericType);
            var expression = Expression.Equal(
                Expression.Property(parameter, masterIdProperty),
                Expression.Constant(Convert.ChangeType(masterId, masterIdProperty.PropertyType)));

            return Expression.Lambda(expression, parameter);
        }
    }
}
