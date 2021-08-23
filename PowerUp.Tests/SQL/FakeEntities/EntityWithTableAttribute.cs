using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerUp.Tests.SQL.FakeEntities
{

    [Table("custom_table_name")]
    public class EntityWithTableAttribute
    {
        public int Id { get; set; }
    }
}
