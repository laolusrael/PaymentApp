using PaymentApp.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentApp.Persistence
{
    public class Repository<TEntity, TContext> : IRepository<TEntity> 
        where TEntity : class, new() 
        where TContext: DbContext
    {
        protected TContext DatabaseContext;

        public Repository(TContext dataContext)
        {
            DatabaseContext = dataContext;

        }

        public static IRepository<TEntity> GetInstance(TContext entityContextInstance)
        {
            return new Repository<TEntity, TContext>(entityContextInstance);
        }

        /// <summary>
        /// Provides direct access to underlying DBContext Database Object for advanced operations
        /// </summary>

        public DatabaseFacade UnderlyDatabase
        {
            get
            {
                return DatabaseContext.Database;
            }
        }

        public virtual Task<TEntity> GetByIdAsync(int id)
        {
            return DatabaseContext.Set<TEntity>().FindAsync(id);
        }
        /// <summary>
        /// Removes the entity with the specified Id from the database
        /// </summary>
        /// <param name="id">Id of the entity to delete</param>
        /// <returns></returns>
        public virtual async Task<TEntity> DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new Exception("Id of an entity cannot be 0");
            }

            var e = DatabaseContext.Set<TEntity>();

            var et = await e.FindAsync(id);

            if (et == null)
            {
                return null;
            }

            e.Remove(et);

            if (await DatabaseContext.SaveChangesAsync() > 0)
            {
                return et;
            }

            throw new Exception("Unable to delete the requested item");
        }
        /// <summary>
        /// Deactivates the entity instead of deleting it by turning its IsActive field to false.
        /// It is unlikely that every entity object will support this feature
        /// </summary>
        /// <param name="id">Id of the entity to delete</param>
        /// <returns></returns>
        public virtual async Task<TEntity> SoftDelete(int id)
        {
            if (id <= 0)
            {
                throw new Exception("Id of an entity cannot be 0");
            }

            var entity = await GetByIdAsync(id);

            if (entity != null)
            {
                var tt = entity.GetType().GetProperty("IsActive");
                if (tt != null)
                {
                    tt.SetValue(entity, false);
                }

                await UpdateChangesAsync();
                return entity;
            }

            throw new Exception("Unable to delete the requested item");
        }
        /// <summary>
        /// Deactivates the entity instead of deleting it by turning its IsActive field to false.
        /// It is unlikely that every entity object will support this feature
        /// </summary>
        /// <param name="id">Guid of the item to soft delete</param>
        /// <returns></returns>
        public virtual async Task<TEntity> SoftDeleteAsync(int id)
        {
            if (id  <= 0)
            {
                throw new Exception("Guid of an entity cannot be null");
            }

            var entity = await GetByIdAsync(id);

            if (entity != null)
            {
                var tt = entity.GetType().GetProperty("IsActive");
                if (tt != null)
                {
                    tt.SetValue(entity, false);
                }

                await UpdateChangesAsync();
                return entity;
            }

            throw new Exception("Unable to delete the requested item");

        }
        public virtual Task<IEnumerable<TEntity>> FindAsync(Func<TEntity, bool> expression = null)
        {
            if (expression != null)
            {
                return Task.Run(() => DatabaseContext.Set<TEntity>().Where(expression));
            }

            return Task.Run(() => DatabaseContext.Set<TEntity>().AsEnumerable());
        }
        /// <summary>
        /// Updates the underlying data context against the data souce.
        /// Normally, would update all attached entity changes, but will skip any entity that is not attached already
        /// </summary>
        /// <param name="updatedEntity">Provide an individual entity to attach it to the context before running update on the context</param>
        /// <returns></returns>
        public virtual Task<int> UpdateChangesAsync(TEntity updatedEntity = null)
        {
            if (updatedEntity != null)
            {
                DatabaseContext.Entry(updatedEntity).State = EntityState.Modified;
            }
            return DatabaseContext.SaveChangesAsync();
        }

        /// <summary>
        /// Adds the entity to the DataContext without updating the underlying data source
        /// You will need to call UpdateChanges() to update the underlying data source
        /// </summary>
        /// <param name="entity">The entity to be added</param>
        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException();
            }

            var entry = await DatabaseContext.Set<TEntity>().AddAsync(entity);

            return entry.Entity;
        }
    }
}
