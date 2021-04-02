using System;
using PowerUp.SQL;
using PowerUp.Tests.SQL.FakeEntities;
using Xunit;

namespace PowerUp.Tests.SQL
{
    public class CommandBuildingInsertTests
    {

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
    }
}
