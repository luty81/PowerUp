using AutoFixture.Xunit2;
using FluentAssertions;
using PowerUp.SQL;
using PowerUp.Tests.SQL.FakeEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PowerUp.Tests.SqlBuilderTests
{
    public class SqlForTests
    {
        [Fact]
        public void SqlForInsertReturnsACommandWithOnlyAssignedProperties()
        {
            var sample = new SampleType() { Name = "Test" };
            SqlFor<SampleType>.GetInsert(sample, dontSetKeyFields: true).Trim().Lines()
                .ShouldBe("INSERT INTO SampleType", "(Name)", "VALUES", "(@Name)");
        }

        [Theory, AutoData]
        public void SqlForInsertTakesIntoAccountAllAssignedPropertiesEvenTheKeyOnes(
            SampleType entitySample, 
            EntityWithCustomKeyField entityWithCustomKey, 
            EntityWithCompositeKey entityWithCompositeKey)
        {
            GetInsertFor(entitySample)
                .ShouldBe("INSERT INTO SampleType", "(Id, Name)", "VALUES", "(@Id, @Name)");

            GetInsertFor(entityWithCompositeKey)
                .ShouldBe(
                    "INSERT INTO EntityWithCompositeKey",
                    "(UserId, ProfileId, RoleId, Name)",
                    "VALUES",
                    "(@UserId, @ProfileId, @RoleId, @Name)");

            entityWithCustomKey.Id = entityWithCustomKey.Key = null;
            GetInsertFor(entityWithCustomKey)
                .ShouldBe("INSERT INTO entities", "(Oid, Name)", "VALUES", "(@Oid, @Name)");

            static IEnumerable<string> GetInsertFor<T>(T fakeEntity) 
                where T : class, new() => 
                    SqlFor<T>.GetInsert(fakeEntity, dontSetKeyFields: false).Trim().Lines();
        }

        [Theory, AutoData]
        public void ShouldBePossibleToCloseGeneratedCommand(SampleType fakeEntity)
        {
            var rawCommand = SqlFor<SampleType>.GetInsert(fakeEntity, dontSetKeyFields: false).Close();

            rawCommand.EndsWith(";")
                .Should().BeTrue();

            rawCommand.Lines()
                .ShouldBe("INSERT INTO SampleType", "(Id, Name)", "VALUES", "(@Id, @Name)", ";");
        }
    }
}
