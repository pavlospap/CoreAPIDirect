namespace CoreApiDirect.Entities
{
    /// <summary>
    /// Provides functionality to get localized entity names.
    /// </summary>
    public interface IEntityLocalizer
    {
        /// <summary>
        /// Gets a localized entity name.
        /// </summary>
        /// <param name="entityName">The entity name.</param>
        /// <returns>The localized entity name.</returns>
        string GetLocalizedEntityName(string entityName);
    }
}
