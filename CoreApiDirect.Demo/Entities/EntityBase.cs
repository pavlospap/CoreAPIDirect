using CoreApiDirect.Entities;

namespace CoreApiDirect.Demo.Entities
{
    public abstract class EntityBase<TKey> : Entity<TKey>
    {
        public string Notes { get; set; }
    }

    public abstract class EntityBase : EntityBase<int>
    {
    }
}
