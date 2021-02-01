using System;
using System.Linq;
using FluentAssertions;
using PowerUp.Sql;
using Xunit;

namespace PowerUp.Tests.Sql
{
    public class ObjectToSqlCommanderTests
    {
        [Fact]
        public void GeneratesSqlUpdateFromObjectTest()
        {
            var sampleObject = new { Id = 1, Name = "John Doe", Email = (string)null, PhoneNumber = "+55123123123" };

            var updateCommand = 
                new ObjectToSqlCommander(sampleObject)
                    .GetUpdateCommandFor("IdentityUser");

            var commandLines = updateCommand.Trim().Split(Environment.NewLine);
            commandLines.Count().Should().Be(3);
            commandLines[0].Should().Be("UPDATE IdentityUser");
            commandLines[1].Should().Be("SET Name = @Name, PhoneNumber = @PhoneNumber");
            commandLines[2].Should().Be("WHERE Id = @Id");
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

            var commandColumns = 
                new ObjectToSqlCommander(sampleObject)
                    .AssignedProperties;
            
            commandColumns.Count().Should().Be(4);
            commandColumns.Should().Contain("Id");
            commandColumns.Should().Contain("Name");
            commandColumns.Should().Contain("Birthday");
            commandColumns.Should().Contain("UserId");
        }


        [Fact]
        public void WhenThereAreNoAssignedFields_NoUpdateSqlCommandShouldBeGenerated()
        {
            var sampleObject = new object { };
            var sqlCommander = new ObjectToSqlCommander(sampleObject);

            var command = sqlCommander.GetUpdateCommandFor("IdentityUser");

            command.Should().BeNull();
            sqlCommander.AssignedProperties.Should().BeNullOrEmpty();
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

            var insertCommand =
                new ObjectToSqlCommander(sampleObject)
                    .GetInsertCommandFor("IdentityUser");

            var commandLines = insertCommand.Trim().Split(Environment.NewLine);
            commandLines.Count().Should().Be(4);
            commandLines[0].Should().Be("INSERT INTO IdentityUser ");
            commandLines[1].Should().Be("(Id, Name, Email, PhoneNumber, Gender, Birthday, UserId)", commandLines[1]);
            commandLines[2].Should().Be("VALUES");
            commandLines[3].Should().Be("(@Id, @Name, @Email, @PhoneNumber, @Gender, @Birthday, @UserId)");
        }
    }
}
