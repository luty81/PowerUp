using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerUp.EF6
{
    public sealed class RepositoryFor<TEntity>: IRepository<TEntity>, IDisposable 
        where TEntity : class
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _table;

        public RepositoryFor(DbContext dbContext)
        {
            _context = dbContext;
            _table = _context.Set<TEntity>();

        }

        public IEnumerable<TEntity> GetAll()
        {
            return _table.ToList();
        }

        public TEntity FindById(object entityId)
        {
            return _table.Find(entityId);
        }

        public void Insert(TEntity entity)
        {
            _table.Add(entity);
        }

        public void Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(object entityId)
        {
            var deletingEntity = _table.Find(entityId);
            if(deletingEntity == null)
            {
                throw new EntityNotFoundException<TEntity>(entityId);
            }

            _table.Remove(deletingEntity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void SaveEntity(TEntity entity)
        {
            Update(entity);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
