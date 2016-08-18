using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerUp.EF6
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
        TEntity FindById(object entityId);
        void Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(object entityId);
        void Save();
        void SaveEntity(TEntity entity);
    }
}
