using System;
using System.Collections.Generic;
using System.Linq;
using CoreApiDirect.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CoreApiDirect.Boot.Generators
{
    internal class RepositoryServiceGenerator : ServiceGenerator
    {
        private readonly IEnumerable<Type> _dbContextTypes;

        public RepositoryServiceGenerator(
            ITypeProvider typeProvider,
            Type helperGenericDefinition,
            Type serviceGenericDefinition,
            Type implementationGenericDefinition)
            : base(typeProvider, helperGenericDefinition, serviceGenericDefinition, implementationGenericDefinition, ServiceLifetime.Scoped)
        {
            _dbContextTypes = GetDbContextTypes();

            if (!_dbContextTypes.Any())
            {
                throw new InvalidOperationException("No database context found.");
            }
        }

        private IEnumerable<Type> GetDbContextTypes()
        {
            return TypeProvider.Types.Where(p => p.IsSubclassOf(typeof(DbContext)) && !p.IsGenericType);
        }

        protected override Type[] GetServiceGenericArguments(Type helperType)
        {
            return new Type[] { helperType, GetEntityKeyType(helperType) };
        }

        private Type GetEntityKeyType(Type helperType)
        {
            return helperType.BaseGenericType().GenericTypeArguments[0];
        }

        protected override Type[] GetImplementationGenericArguments(Type helperType)
        {
            var dbContextType = GetEntityDbContextType(helperType);

            if (dbContextType == null)
            {
                throw new InvalidOperationException($"'{helperType.Name}' does not belong to a database context.");
            }

            return new Type[] { helperType, GetEntityKeyType(helperType), dbContextType };
        }

        private Type GetEntityDbContextType(Type helperType)
        {
            return _dbContextTypes.FirstOrDefault(dbc => dbc.GetProperties().Where(p => p.PropertyType.IsListOfType(helperType)).Any());
        }
    }
}
