using CoreApiDirect.Controllers;

namespace CoreApiDirect.Dto
{
    /// <summary>
    /// The base class for every API DTO used in an HTTP GET request.
    /// </summary>
    /// <typeparam name="TKey">The type of the DTO ID.</typeparam>
    public abstract class OutDto<TKey> : IKey<TKey>
    {
        /// <summary>
        /// The DTO ID.
        /// </summary>
        public TKey Id { get; set; }
    }
}
