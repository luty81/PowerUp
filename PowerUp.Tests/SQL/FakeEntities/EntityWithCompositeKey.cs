using System;
using System.ComponentModel.DataAnnotations;

namespace PowerUp.Tests.SQL.FakeEntities
{
    public class EntityWithCompositeKey
    {
        [Key]
        public int UserId { get; set; }

        [Key]
        public Guid ProfileId { get; set; }

        [Key]
        public string RoleId { get; set; }

        public string Name { get; set; }
    }
}
