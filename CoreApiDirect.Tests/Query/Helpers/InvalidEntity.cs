using CoreApiDirect.Entities;

namespace CoreApiDirect.Tests.Query.Helpers
{
    internal class InvalidEntity : Entity<int>
    {
        [SearchKey]
        public int IntegerProperty { get; set; }
    }
}
