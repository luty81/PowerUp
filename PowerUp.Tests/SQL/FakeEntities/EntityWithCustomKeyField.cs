using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerUp.Tests.SQL.FakeEntities
{
    [Table("entities")]
    public class EntityWithCustomKeyField
    {
        [Key]
        public string Oid { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Key { get; set; }
    }
}
