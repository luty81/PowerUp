using System;
using FluentAssertions;
using PowerUp.SQL;
using PowerUp.Tests.SQL.FakeEntities;
using Xunit;

namespace PowerUp.Tests.SQL
{
    public class CommandBuildingUpdateTests
    {

        [Fact]
        public void UpdateCommandGenerationFor_AnnotationTableAndKeyField()
        {
            var sampleObject =
                new EntityWithCustomKeyField { Oid = "1", Name = "" };

            new CommandBuilder(sampleObject)
                .For<UpdateCommand>().Trim().Lines()
                .ShouldBe(
                    "UPDATE entities",
                    "SET Name = @Name",
                    "WHERE Oid = @Oid");
        }

        [Fact]
        public void UpdateCommandUsingAnonymousType()
        {
            var @object = new { Id = 1, Enabled = (bool?)false, Active = false, Count = (int?)null };
            new CommandBuilder(@object)
                .For<UpdateCommand>("some_table").Trim().Lines()
                .ShouldBe(
                    "UPDATE some_table",
                    "SET Enabled = @Enabled, Active = @Active",
                    "WHERE Id = @Id");
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
    }
}
