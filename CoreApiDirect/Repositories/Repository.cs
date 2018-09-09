using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreApiDirect.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoreApiDirect.Repositories
{
    /// <summary>
    /// Implements the functionality to work with an abstraction of data.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The entity ID type.</typeparam>
    /// <typeparam name="TDbContext">The database context type.</typeparam>
    public class Repository<TEntity, TKey, TDbContext> : IRepository<TEntity, TKey>
        where TEntity : Entity<TKey>
        where TDbContext : DbContext
    {
        /// <summary>
        /// Returns a query for the current repository.
        /// </summary>
        public IQueryable<TEntity> Query
        {
            get
            {
                return _dbSet;
            }
        }

        private readonly TDbContext _dbContext;

        private DbSet<TEntity> _dbSet
        {
            get
            {
                return _dbContext.Set<TEntity>();
            }
        }

        /// <summary>
        /// Initializes a new instance of the CoreApiDirect.Repositories.Repository class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public Repository(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Asynchronously gets an entity by the specified ID.
        /// </summary>
        /// <param name="id">The entity ID.</param>
        /// <returns>The entity with the specified ID. If no entity found, returns null.</returns>
        public async Task<TEntity> FindByIdAsync(TKey id)
        {
            return await Query.Where(p => p.Id.Equals(id)).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Asynchronously gets an entity using an expression filter without tracking any changes.
        /// </summary>
        /// <param name="filter">The expression filter.</param>
        /// <returns>The entity found by the given expression filter. If no entity found, returns null.</returns>
        public async Task<TEntity> FindNoTrackingAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await Query.Where(filter).AsNoTracking().FirstOrDefaultAsync();
        }

        /// <summary>
        /// Adds an entity to the repository.
        /// </summary>
        /// <param name="entity">The entity to add to the repository.</param>
        public void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        /// <summary>
        /// Adds an enumerable of entities to the repository.
        /// </summary>
        /// <param name="entityList">An enumerable of entities to add to the repository.</param>
        public void AddRange(IEnumerable<TEntity> entityList)
        {
            _dbSet.AddRange(entityList);
        }

        /// <summary>
        /// Removes an entity from the repository.
        /// </summary>
        /// <param name="entity">The entity to remove from the repository.</param>
        public void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        /// <summary>
        /// Removes an enumerable of entities from the repository.
        /// </summary>
        /// <param name="entityList">An enumerable of entities to remove from the repository.</param>
        public void RemoveRange(IEnumerable<TEntity> entityList)
        {
            _dbSet.RemoveRange(entityList);
        }

        /// <summary>
        /// Gets a value that indicates whether the repository has changes.
        /// </summary>
        /// <returns>True if the repository has changes. Otherwise, false.</returns>
        public bool HasChanges()
        {
            return _dbContext.ChangeTracker.HasChanges();
        }

        /// <summary>
        /// Asynchronously saves all the repository changes.
        /// </summary>
        /// <returns>True if the changes have successfully been saved. Otherwise, false.</returns>
        public async Task<bool> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
