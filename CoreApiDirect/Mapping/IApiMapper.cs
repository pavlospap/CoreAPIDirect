namespace CoreApiDirect.Mapping
{
    /// <summary>
    /// Provides functionality to map entities to DTOs and the other way around.
    /// </summary>
    public interface IApiMapper
    {
        /// <summary>
        /// Executes a mapping from a source object to a new destination object.
        /// </summary>
        /// <typeparam name="TDestination">The destination object type.</typeparam>
        /// <param name="source">The source object.</param>
        /// <returns>The new destination object.</returns>
        TDestination Map<TDestination>(object source);

        /// <summary>
        /// Executes a mapping from a source object to an existing destination object.
        /// </summary>
        /// <typeparam name="TSource">The source object type.</typeparam>
        /// <typeparam name="TDestination">The destination object type.</typeparam>
        /// <param name="source">The source object.</param>
        /// <param name="destination">The destination object.</param>
        void Map<TSource, TDestination>(TSource source, TDestination destination);
    }
}
