using CoreApiDirect.Entities;

namespace CoreApiDirect.Tests.Routing.Helpers
{
    internal class DetailEntity : Entity<int>
    {
        public int MasterEntityId { get; set; }
    }
}
