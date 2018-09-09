using System;
using System.Collections.Generic;
using CoreApiDirect.Infrastructure;

namespace CoreApiDirect.Mapping.Configuration
{
    internal interface IEntityMapperConfigPropertyWalkerVisitor : IPropertyWalkerVisitor<List<Type>, WalkInfo>
    {
    }
}
