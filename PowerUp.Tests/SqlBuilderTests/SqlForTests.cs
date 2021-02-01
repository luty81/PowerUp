using System;
using System.Linq;
using FluentAssertions;
using PowerUp.Sql;
using Xunit;

namespace PowerUp.Tests.Sql
{
    public class SqlForTests
    {
        [Fact]
        public void GeneratesSqlSelectAllFromTypeTest()
        {
            var sqlSelect = new SqlFor<SampleType>().Select;

            
            sqlSelect.Should().NotBeNullOrEmpty();
            var sqlSelectLines = sqlSelect.Split(Environment.NewLine);
            sqlSelectLines[0].Should().Be("SELECT * ");
            sqlSelectLines[1].Trim().Should().Be("FROM SampleType");
        }

        [Fact]
        public void GeneratesSqlSelectAllFromType_ResolveColumnNamesTest()
        {
            var sqlSelect = new SqlFor<SampleType>(ColumnsMode.ResolveNames).Select;

            sqlSelect.Should().NotBeNullOrEmpty();
            var sqlSelectLines = sqlSelect.Split(Environment.NewLine);
            sqlSelectLines[0].Should().Be("SELECT Id, Name, Active ");
            sqlSelectLines[1].Trim().Should().Be("FROM SampleType");
        }

        [Fact]
        public void GeneratesSqlSelectAllFromType_ResolveAliasesAndColumnNamesTest()
        {
            var sqlSelect = new SqlFor<SampleType>(ColumnsMode.ResolveAliasesAndNames).Select;

            sqlSelect.Should().NotBeNullOrEmpty();
            var sqlSelectLines = sqlSelect.Split(Environment.NewLine);
            sqlSelectLines[0].Should().Be("SELECT ST.Id, ST.Name, ST.Active ");
            sqlSelectLines[1].Trim().Should().Be("FROM SampleType ST");
        }

        [Fact]
        public void GeneratesSqlSelectWithWhereTest()
        {
            var sqlLines = 
                new SqlFor<SampleType>(ColumnsMode.ResolveAliasesAndNames)
                    .Where(x => x.Id)
                    .Done()
                    .Split(Environment.NewLine);

            sqlLines.Count().Should().Be(3);
            sqlLines.ElementAt(0).Should().Be("SELECT ST.Id, ST.Name, ST.Active ");
            sqlLines.ElementAt(1).Should().Be("FROM SampleType ST ");
            sqlLines.ElementAt(2).Should().Be("WHERE ST.Id = @Id ;");
        
        }

        class SampleType
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public bool Active { get; }
            private double Internal { get; set; }
        }
    }
}
