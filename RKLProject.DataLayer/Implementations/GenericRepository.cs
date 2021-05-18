using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RKLProject.DataLayer.Interfaces;
using RKLProject.Entities.Common;

namespace RKLProject.DataLayer.Implementations
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {

        #region Constructor

        private RKLDatabaseContext context;
        private DbSet<TEntity> dbset;

        public GenericRepository(RKLDatabaseContext context)
        {
            this.context = context;

            this.dbset = this.context.Set<TEntity>();
        }

        #endregion

        #region Properties

        public async Task AddEntity(TEntity entity)
        {
            entity.CreateDate = DateTime.Now;
            entity.UpdateDate = DateTime.Now;
            entity.IsDelete = false;
            await dbset.AddAsync(entity);
        }
        public async Task AddRenge(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.CreateDate = DateTime.Now;
                entity.UpdateDate = DateTime.Now;
                entity.IsDelete = false;
            }

            await dbset.AddRangeAsync(entities);
        }

        public void UpdateRange(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.UpdateDate = DateTime.Now;
            }

            dbset.UpdateRange(entities);
        }

        public System.Linq.IQueryable<TEntity> GetEntitiesQuery()
        {
            return dbset.AsQueryable();
        }

        public async Task<TEntity> GetEntityById(long EntityId)
        {
            return await dbset.SingleOrDefaultAsync(e => e.Id == EntityId);
        }

        public void RemoveEntity(TEntity entity)
        {
            entity.IsDelete = true;
            UpdateEntity(entity);
        }

        public async Task RemoveEntity(long id)
        {
            var entity = await GetEntityById(id);
            RemoveEntity(entity);
        }

        public void DeleteEntity(TEntity entity)
        {
            dbset.Remove(entity);
        }

        public async Task DeleteEntity(long id)
        {
            var entity = await GetEntityById(id);
            dbset.Remove(entity);
        }

        public void UpdateEntity(TEntity entity)
        {
            dbset.Update(entity);
        }


        #endregion

        #region Dispose

        public void Dispose()
        {
            context?.Dispose();
        }


        #endregion
    }
}