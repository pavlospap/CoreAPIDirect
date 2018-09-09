namespace CoreApiDirect.Controllers
{
    /// <summary>
    /// Provides a typed ID property.
    /// </summary>
    /// <typeparam name="TKey">The type of the ID.</typeparam>
    public interface IKey<TKey>
    {
        /// <summary>
        /// The typed ID.
        /// </summary>
        TKey Id { get; set; }
    }
}
