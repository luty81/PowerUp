using System;
using System.Linq;
using FluentAssertions;
using PowerUp;
using PowerUp.SQL;
using PowerUp.Tests.SQL.FakeEntities;
using Xunit;

namespace PowerUp.Tests.SQL
{
    public class CommandBuilderTests
    {
        [Fact]
        public void CommandBuilderShouldHandleEnumsProperly()
        {
            var entity = new SampleEntity { EntityType = EntityTypes.Fake };
            SqlFor<SampleEntity>.GetInsert(entity, dontSetKeyFields: true).Lines().Join(" ")
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

    }

    public enum EntityTypes { Dummy = 0, Fake = 1 }
}
