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
            var expected = new[] { "DELETE FROM SampleEntity", "WHERE Id = @Id;" };

            Get(DeleteCommand.For<SampleEntity>())
                .ShouldBe(expected);

            Get(DeleteCommand.For(new SampleEntity()))
                .ShouldBe(expected);
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
