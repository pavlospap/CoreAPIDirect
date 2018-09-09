using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CoreApiDirect.Base;
using CoreApiDirect.Entities;
using CoreApiDirect.Infrastructure;

namespace CoreApiDirect.Mapping.Configuration
{
    internal class EntityMapperConfigPropertyWalkerVisitor : PropertyWalkerVisitor<List<Type>, WalkInfo>, IEntityMapperConfigPropertyWalkerVisitor
    {
        private readonly List<Type> _types = new List<Type>();

        public EntityMapperConfigPropertyWalkerVisitor(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        public override List<Type> Output(WalkInfo walkInfo)
        {
            return _types;
        }

        public override void Visit(PropertyInfo property, WalkInfo walkInfo)
        {
            if (property.PropertyType.IsDetailOfRawGeneric(typeof(Entity<>)))
            {
                var type = property.PropertyType.IsListOfRawGeneric(typeof(Entity<>)) ?
                    property.PropertyType.BaseGenericType().GenericTypeArguments[0] :
                    property.PropertyType;

                NextWalk(type, walkInfo);
            }
        }

        private void NextWalk(Type type, WalkInfo walkInfo)
        {
            if (!_types.Contains(type))
            {
                _types.Add(type);
            }

            NextWalk<List<Type>, WalkInfo>(GeNextWalkInfo(type, walkInfo), typeof(IEntityMapperConfigPropertyWalker), typeof(IEntityMapperConfigPropertyWalkerVisitor));
        }

        private WalkInfo GeNextWalkInfo(Type type, WalkInfo walkInfo)
        {
            return new WalkInfo
            {
                Type = type,
                GenericDefinition = typeof(Entity<>),
                WalkedTypes = walkInfo.WalkedTypes.ToList(),
                Fields = walkInfo.Fields
            };
        }
    }
}
