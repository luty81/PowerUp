using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerUp.EF6
{
    public class EntityNotFoundException<TEntity>: Exception
    {
        public EntityNotFoundException(object entityId)
            : base(String.Format("An object of type {0} and id {1} could not be deleted, because it was not found in database.", typeof(TEntity).Name, entityId))
        {

        }
    }
}
