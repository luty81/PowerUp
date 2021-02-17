using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FluentAssertions;
using PowerUp;
using PowerUp.SQL;
using Xunit;

namespace PowerUp.Tests.SQL
{
    public class SqlCommandBuilderTests
    {
        [Fact]
        public void CommandBuilderShouldHandleEnumsProperly()
        {
            var entity = new SampleEntity { EntityType = EntityTypes.Fake };
            SqlFor<SampleEntity>.GetInsert(entity).Lines().Join(" ")
                .Should().Be("INSERT INTO SampleEntity (EntityType) VALUES (@EntityType) ");
        }

        [Fact]
        public void CommandBuilderMustConsiderateOnlyPropertiesWithNonDefaultValueOrNotNullOnes()
        {
            var sampleObject = new
            {
                Id = Guid.NewGuid().ToString(),
                Name = "John Doe Castilho",
                Birthday = DateTime.Parse("1989-08-10T03:00:00.000Z"),
                UserId = 150,
                UserName = (string)null
            };

            new CommandBuilder(sampleObject)
                .AssignedProperties
                .Select(p => p.Name)
                .ShouldBe("Id", "Name", "Birthday", "UserId");
        }



        [Fact]
        public void InsertCommandGenerationFromObject()
        {
            var sampleObject = new
            {
                Id = Guid.NewGuid().ToString(),
                Name = "John Doe",
                Email = "john@doe.br",
                PhoneNumber = "+552199090912",
                Gender = 'M',
                Birthday = DateTime.Parse("1989-08-10T03:00:00.000Z"),
                UserId = 150,
                UserName = (string)null
            };


            new CommandBuilder(sampleObject)
                .For<InsertCommand>("IdentityUser").Trim().Lines()
                .ShouldBe(
                    "INSERT INTO IdentityUser",
                    "(Id, Name, Email, PhoneNumber, Gender, Birthday, UserId)",
                    "VALUES",
                    "(@Id, @Name, @Email, @PhoneNumber, @Gender, @Birthday, @UserId)");
        }

        [Fact]
        public void InsertCommandGenerationWithCustomIdParameter()
        {
            var sampleObject = new SampleEntity
            {
                Name = "John Doe",
            };

            var result = new CommandBuilder(sampleObject)
                .For<InsertCommand>("IdentityUser", ("Id", "UUID_SHORT()")).Trim().Lines();

            result.ShouldBe(
                "INSERT INTO IdentityUser",
                "(Id, Name)",
                "VALUES",
                "(UUID_SHORT(), @Name)");
        }

        [Fact]
        public void InsertCommandGeneration_CompositeKey()
        {
            var entity = new EntityWithCompositeKey
            {
                UserId = 1,
                ProfileId = Guid.NewGuid(),
                RoleId = "admin",
                Name = $"{Guid.NewGuid()}"
            };

            new CommandBuilder(entity)
                .For<InsertCommand>()
                .Trim().Lines()
                .ShouldBe(
                    "INSERT INTO EntityWithCompositeKey",
                    "(UserId, ProfileId, RoleId, Name)",
                    "VALUES",
                    "(@UserId, @ProfileId, @RoleId, @Name)");
        }



        [Fact]
        public void UpdateCommandGenerationFor_AnnotationTableAndKeyField()
        {
            var sampleObject = new EntityWithCustomKeyField { Oid = "1", Name = "" };

            new CommandBuilder(sampleObject)
                .For<UpdateCommand>().Trim().Lines()
                .ShouldBe(
                    "UPDATE entities",
                    "SET Name = @Name",
                    "WHERE Oid = @Oid");
        }

        [Fact]
        public void UpdateCommandGenerationFor_AdhocTableName()
        {
            var sampleObject = new
            {
                Id = 1,
                Name = "John Doe",
                Email = (string)null,
                PhoneNumber = "+55123123123"
            };

            new CommandBuilder(sampleObject)
                .For<UpdateCommand>("IdentityUser").Trim().Lines()
                .ShouldBe(
                    "UPDATE IdentityUser",
                    "SET Name = @Name, PhoneNumber = @PhoneNumber",
                    "WHERE Id = @Id");
        }

        [Fact]
        public void UpdateCommandGeneration_WhenThereAreNoAssignedFields()
        {
            var sampleObject = new object { };
            var sqlCommander = new CommandBuilder(sampleObject);

            sqlCommander
                .For<UpdateCommand>("IdentityUser")
                .Should().BeNull();

            sqlCommander
                .AssignedProperties
                .Should().BeNullOrEmpty();
        }

        [Fact]
        public void UpdateCommandGeneration_CompositeKey()
        {
            var entity = new EntityWithCompositeKey
            {
                UserId = 1,
                ProfileId = Guid.NewGuid(),
                RoleId = "admin",
                Name = $"{Guid.NewGuid()}"
            };

            new CommandBuilder(entity)
                .For<UpdateCommand>()
                .Trim().Lines()
                .ShouldBe(
                    "UPDATE EntityWithCompositeKey", 
                    "SET Name = @Name", 
                    "WHERE UserId = @UserId AND ProfileId = @ProfileId AND RoleId = @RoleId");
        }



        [Fact]
        public void DeleteCommandBuilderTest() =>
            DeleteCommandFor<SampleEntity>.Build().Trim().Lines()
                .ShouldBe("DELETE FROM SampleEntity", "WHERE Id = @Id;");
        
        [Fact]
        public void DeleteCommandBuilderCustomTableNameTest() => 
            DeleteCommandFor<SampleEntity>.Build("sample_entity").Trim().Lines()
                .ShouldBe("DELETE FROM sample_entity", "WHERE Id = @Id;");

        [Fact]
        public void DeleteCommandBuilderForCompositeKeyTable() =>
            DeleteCommandFor<EntityWithCompositeKey>.Build().Trim().Lines()
                .ShouldBe("DELETE FROM EntityWithCompositeKey", "WHERE UserId = @UserId AND ProfileId = @ProfileId AND RoleId = @RoleId;");



        class SampleEntity 
        {
            public ulong Id { get; set; }

            public string Name { get; set; }

            public EntityTypes EntityType { get; set; }
        }

        [Table("entities")]
        class EntityWithCustomKeyField
        {
            [Key]
            public string Oid { get; set; }

            public string Id { get; set; }

            public string Name { get; set; }

            public string Key { get; set; }
        }

        class EntityWithCompositeKey
        {
            [Key]
            public int UserId { get; set; }

            [Key]
            public Guid ProfileId { get; set; }

            [Key]
            public string RoleId { get; set; }

            public string Name { get; set; }
        }

        enum EntityTypes { Dummy = 0, Fake = 1 }
    }
}
