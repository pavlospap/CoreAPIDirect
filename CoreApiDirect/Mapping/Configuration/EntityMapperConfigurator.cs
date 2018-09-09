using System;
using System.Collections.Generic;
using AutoMapper;
using CoreApiDirect.Entities;
using CoreApiDirect.Infrastructure;

namespace CoreApiDirect.Mapping.Configuration
{
    internal class EntityMapperConfigurator : IEntityMapperConfigurator
    {
        private readonly IEntityMapperConfigPropertyWalker _walker;
        private readonly IEntityMapperConfigPropertyWalkerVisitor _visitor;

        public EntityMapperConfigurator(
            IEntityMapperConfigPropertyWalker walker,
            IEntityMapperConfigPropertyWalkerVisitor visitor)
        {
            _walker = walker;
            _visitor = visitor;
        }

        public void Configure<TEntity>(IMapperConfigurationExpression config)
        {
            var entityTypes = new List<Type>() { typeof(TEntity) };
            entityTypes.AddRange(_walker.Accept(_visitor, new WalkInfo
            {
                Type = typeof(TEntity),
                GenericDefinition = typeof(Entity<>),
                Fields = new List<string> { "*" }
            }));

            entityTypes.ForEach(type => config.CreateMap(type, type));
        }
    }
}
