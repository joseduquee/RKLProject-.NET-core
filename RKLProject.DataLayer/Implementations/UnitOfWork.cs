using System.Threading.Tasks;
using RKLProject.DataLayer.Interfaces;

namespace RKLProject.DataLayer.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Constructor

        private RKLDatabaseContext db;

        public UnitOfWork(RKLDatabaseContext db)
        {
            this.db = db;
        }

        #endregion

        #region Implementations

        public async Task<int> SaveChanges()
        {
            return await db.SaveChangesAsync();
        }

        async Task<TRepository> IUnitOfWork.GetRepository<TRepository, TEntity>()
        {
            await Task.CompletedTask;
            return new GenericRepository<TEntity>(db) as TRepository;
        }

        #endregion

        #region Dispose

        public void Dispose()
        {
            db?.Dispose();
        }

        #endregion
    }
}
