using FluentAssertions;
using PowerUp.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PowerUp.Tests.Database
{
    public class CommandBatchingTests
    {
        [Fact]
        public void InsertBatchCommandBuilding_UUID()
        {
            var result = new CommandBatching()
                .InsertUUID(
                    new FakeDeviceModel { SerialNumber = "A1B2" });

            SqlLines(result.Operations)
                 .ShouldBe(
                    "INSERT INTO FakeDeviceModel", "(Id, SerialNumber)", "VALUES", "(UUID_SHORT(), @SerialNumber)");
        }

        [Fact]
        public void InsertBatchCommandBuilding_NoDefinedKey()
        {
            var result = new CommandBatching()
                .Insert(
                    new FakeEntityModel { Name = "C1" });

            SqlLines(result.Operations)
                .ShouldBe("INSERT INTO FakeEntityModel", "(Name)", "VALUES", "(@Name)");
        }

        [Fact]
        public void InsertBatchCommandBuilding_EntityList()
        {
            var sample = new[] { "C1", "C2", "C3" }
                .Select(c => new FakeEntityModel { Name = c });

            var result = new CommandBatching()
                .Insert(sample)
                .Operations;

            result.Count().Should().Be(3);
            result.ForEach(VerifyBatchOperationItem);
            result.Select(x => x.Entity).Cast<FakeEntityModel>()
                .Select(x => x.Name)
                .ShouldBe("C1", "C2", "C3");

            static void VerifyBatchOperationItem(BatchOperation item)
            {
                item.CommandSQL.Lines().ShouldBe("INSERT INTO FakeEntityModel", "(Name)", "VALUES", "(@Name)", "");
                item.Entity.Should().BeOfType<FakeEntityModel>();
            }
        }

        [Fact]
        public void UpdateBatchCommandBuilding()
        {
            var deviceToUpdate =
                new FakeDeviceModel { Id = 1, NetworkAddress = "10.0.0.1" };

            var result = new CommandBatching()
                .Update(deviceToUpdate);

            SqlLines(result.Operations)
                .ShouldBe("UPDATE FakeDeviceModel",
                          "SET NetworkAddress = @NetworkAddress",
                          "WHERE Id = @Id", ";");
        }

        private static IEnumerable<string> SqlLines(IEnumerable<BatchOperation> operations)
        {
            Assert.True(operations.Count() == 1);
            Assert.NotNull(operations.First().CommandSQL);
            return operations.First().CommandSQL.Trim().Lines();
        }
    }

    class FakeDeviceModel
    {
        public ulong Id { get; set; }
        public string NetworkAddress { get; set; }
        public string SerialNumber { get; set; }
    }

    class FakeEntityModel
    {
        public string Name { get; set; }
    }

}
