using System;
using System.Collections.Generic;
using CoreApiDirect.Infrastructure;

namespace CoreApiDirect.Mapping.Configuration
{
    internal interface IEntityMapperConfigPropertyWalker : IPropertyWalker<List<Type>, WalkInfo>
    {
    }
}
