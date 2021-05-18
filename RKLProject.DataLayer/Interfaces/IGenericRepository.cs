using RKLProject.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RKLProject.DataLayer.Interfaces
{
    public interface IGenericRepository<TEntity> : IDisposable where TEntity : BaseEntity
    {
        IQueryable<TEntity> GetEntitiesQuery();

        Task<TEntity> GetEntityById(long EntityId);

        Task AddEntity(TEntity entity);
        Task AddRenge(List<TEntity> entities);
        void UpdateRange(List<TEntity> entities);

        void UpdateEntity(TEntity entity);

        void RemoveEntity(TEntity entity);

        Task RemoveEntity(long id);
        void DeleteEntity(TEntity entity);
        Task DeleteEntity(long id);
    }
}