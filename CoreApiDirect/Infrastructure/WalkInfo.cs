using System;
using System.Collections.Generic;

namespace CoreApiDirect.Infrastructure
{
    internal class WalkInfo
    {
        public Type Type { get; set; }
        public Type GenericDefinition { get; set; }
        public List<Type> WalkedTypes { get; set; } = new List<Type>();
        public IEnumerable<string> Fields { get; set; } = new List<string>();
        public int RelatedDataLevel { get; set; }
    }
}
