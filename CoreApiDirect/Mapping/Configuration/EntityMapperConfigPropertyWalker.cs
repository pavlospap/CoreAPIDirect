using System;
using System.Collections.Generic;
using CoreApiDirect.Base;
using CoreApiDirect.Infrastructure;

namespace CoreApiDirect.Mapping.Configuration
{
    internal class EntityMapperConfigPropertyWalker : PropertyWalker<List<Type>, WalkInfo>, IEntityMapperConfigPropertyWalker
    {
        public EntityMapperConfigPropertyWalker(IPropertyProvider propertyProvider)
            : base(propertyProvider)
        {
        }
    }
}
