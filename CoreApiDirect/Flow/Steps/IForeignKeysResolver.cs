namespace CoreApiDirect.Flow.Steps
{
    /// <summary>
    /// Provides functionality to resolve entity's foreign keys.
    /// </summary>
    public interface IForeignKeysResolver
    {
        /// <summary>
        /// Resolves entity's foreign keys by retrieving them from route parameters.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="entity">The specified entity.</param>
        void FillForeignKeysFromRoute<TEntity>(TEntity entity);
    }
}
