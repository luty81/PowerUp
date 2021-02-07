using System;
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
        public void GeneratesSqlUpdateFromObjectTest()
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
        public void CommanderMustConsiderateOnlyPropertiesWithNonDefaultValueOrNotNullOnes()
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
        public void WhenThereAreNoAssignedFields_NoUpdateSqlCommandShouldBeGenerated()
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
        public void GeneratesSqlInsertFromObjectTest()
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
                    "INSERT INTO IdentityUser ",
                    "(Id, Name, Email, PhoneNumber, Gender, Birthday, UserId)",
                    "VALUES",
                    "(@Id, @Name, @Email, @PhoneNumber, @Gender, @Birthday, @UserId)");
        }

        [Fact]
        public void DeleteCommandBuilderTest() =>
            DeleteCommandFor<SampleEntity>.Build().Lines()
                .ShouldBe("DELETE FROM SampleEntity", "WHERE Id = @Id");
        

        [Fact]
        public void DeleteCommandBuilderCustomTableNameTest() => 
            DeleteCommandFor<SampleEntity>.Build("sample_entity").Lines()
                .ShouldBe("DELETE FROM sample_entity", "WHERE Id = @Id");

        class SampleEntity { }

    }
}
