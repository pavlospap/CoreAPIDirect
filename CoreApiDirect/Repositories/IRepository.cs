using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CoreApiDirect.Repositories
{
    /// <summary>
    /// Provides functionality to work with an abstraction of data.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The entity ID type.</typeparam>
    public interface IRepository<TEntity, TKey>
    {
        /// <summary>
        /// Returns a query for the current repository.
        /// </summary>
        IQueryable<TEntity> Query { get; }

        /// <summary>
        /// Asynchronously gets an entity by the specified ID.
        /// </summary>
        /// <param name="id">The entity ID.</param>
        /// <returns>The entity with the specified ID. If no entity found, returns null.</returns>
        Task<TEntity> FindByIdAsync(TKey id);

        /// <summary>
        /// Asynchronously gets an entity using an expression filter without tracking any changes.
        /// </summary>
        /// <param name="filter">The expression filter.</param>
        /// <returns>The entity found by the given expression filter. If no entity found, returns null.</returns>
        Task<TEntity> FindNoTrackingAsync(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// Adds an entity to the repository.
        /// </summary>
        /// <param name="entity">The entity to add to the repository.</param>
        void Add(TEntity entity);

        /// <summary>
        /// Adds an enumerable of entities to the repository.
        /// </summary>
        /// <param name="entityList">An enumerable of entities to add to the repository.</param>
        void AddRange(IEnumerable<TEntity> entityList);

        /// <summary>
        /// Removes an entity from the repository.
        /// </summary>
        /// <param name="entity">The entity to remove from the repository.</param>
        void Remove(TEntity entity);

        /// <summary>
        /// Removes an enumerable of entities from the repository.
        /// </summary>
        /// <param name="entityList">An enumerable of entities to remove from the repository.</param>
        void RemoveRange(IEnumerable<TEntity> entityList);

        /// <summary>
        /// Asynchronously saves all the repository changes.
        /// </summary>
        /// <returns>True if the changes have successfully been saved. Otherwise, false.</returns>
        Task<bool> SaveAsync();
    }
}
