using System.Collections.Generic;
using FluentAssertions;
using PowerUp.SQL;
using PowerUp.Tests.SQL.FakeEntities;
using Xunit;

namespace PowerUp.Tests.SQL
{
    public class CommandBuildingDeleteTests
    {
        [Fact]
        public void DeleteCommandBuilderReturnsTheSameForBothInstanceOrTypeInput()
        {
            var expected = "DELETE FROM SampleEntity\r\nWHERE Id = @Id;";

            DeleteCommand.For<SampleEntity>()
                .Should().Be(expected);

            DeleteCommand.For(new SampleEntity())
                .Should().Be(expected);
        }

        [Fact]
        public void DeleteCommandBuilderCustomTableNameTest() =>
            Get(DeleteCommand.For<SampleEntity>("sample_entity"))
                .ShouldBe("DELETE FROM sample_entity", "WHERE Id = @Id;");

        [Fact]
        public void DeleteCommandBuilderForCompositeKeyTable() =>
            Get(DeleteCommand.For<EntityWithCompositeKey>())
                .ShouldBe(
                    "DELETE FROM EntityWithCompositeKey", 
                    "WHERE UserId = @UserId AND ProfileId = @ProfileId AND RoleId = @RoleId;");

        private static IEnumerable<string> Get(string rawCommand) => rawCommand.Trim().Lines();
    }
}
