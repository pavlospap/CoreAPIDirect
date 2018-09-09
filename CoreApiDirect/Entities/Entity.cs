using CoreApiDirect.Controllers;

namespace CoreApiDirect.Entities
{
    /// <summary>
    /// The base class for every API entity.
    /// </summary>
    /// <typeparam name="TKey">The type of the entity ID.</typeparam>
    public abstract class Entity<TKey> : IKey<TKey>
    {
        /// <summary>
        /// The entity ID.
        /// </summary>
        public TKey Id { get; set; }
    }
}
