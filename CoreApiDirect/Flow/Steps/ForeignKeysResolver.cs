using System;
using System.Linq;
using CoreApiDirect.Base;
using CoreApiDirect.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace CoreApiDirect.Flow.Steps
{
    internal class ForeignKeysResolver : IForeignKeysResolver
    {
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly IPropertyProvider _propertyProvider;

        public ForeignKeysResolver(
            IActionContextAccessor actionContextAccessor,
            IPropertyProvider propertyProvider)
        {
            _actionContextAccessor = actionContextAccessor;
            _propertyProvider = propertyProvider;
        }

        public void FillForeignKeysFromRoute<TEntity>(TEntity entity)
        {
            foreach (var property in _propertyProvider.GetProperties(entity.GetType())
                .Where(p => _actionContextAccessor.HasRouteParamIgnoreCase(p.Name)))
            {
                entity.SetPropertyValue(property.Name, Convert.ChangeType(_actionContextAccessor.GetRouteParamIgnoreCase(property.Name), property.PropertyType));
            }
        }
    }
}
