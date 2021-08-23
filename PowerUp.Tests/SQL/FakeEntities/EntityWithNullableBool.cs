using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerUp.Tests.SQL.FakeEntities
{
    public class EntityWithNullableBool
    {
        public int Id { get; set; }
        public bool? MyProperty { get; set; }
    }
}
