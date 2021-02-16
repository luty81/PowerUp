using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FluentAssertions;
using PowerUp.SQL;
using Xunit;

namespace PowerUp.Tests.SQL
{
    public class SqlBuilderTests
    {
        [Fact]
        public void GeneratesSqlSelectAllFromTypeTest()
        {
            var sqlSelect = new SelectBuilder<SampleType>().Done();
            
            sqlSelect.Should().NotBeNullOrEmpty();
            var sqlSelectLines = sqlSelect.Split(Environment.NewLine);
            sqlSelectLines[0].Should().Be("SELECT * ");
            sqlSelectLines[1].Trim().Should().Be("FROM SampleType");
        }

        [Fact]
        public void GeneratesSqlSelectAllFromType_ResolveColumnNamesTest()
        {
            var sqlSelect = new SelectBuilder<SampleType>(ColumnsMode.ResolveNames).SelectAll;

            sqlSelect.Should().NotBeNullOrEmpty();
            var sqlSelectLines = sqlSelect.Split(Environment.NewLine);
            sqlSelectLines[0].Should().Be("SELECT Id, Name, Active ");
            sqlSelectLines[1].Trim().Should().Be("FROM SampleType");
        }

        [Fact]
        public void GeneratesSqlSelectAllFromType_ResolveAliasesAndColumnNamesTest()
        {
            var sqlSelect = new SelectBuilder<SampleType>(ColumnsMode.ResolveAliasesAndNames).SelectAll;

            sqlSelect.Should().NotBeNullOrEmpty();
            var sqlSelectLines = sqlSelect.Split(Environment.NewLine);
            sqlSelectLines[0].Should().Be("SELECT ST.Id, ST.Name, ST.Active ");
            sqlSelectLines[1].Trim().Should().Be("FROM SampleType ST");
        }

        [Fact]
        public void GeneratesSqlSelectWithWhereTest()
        {
            var sqlLines = 
                new SelectBuilder<SampleType>(ColumnsMode.ResolveAliasesAndNames)
                    .Where(x => x.Id)
                    .Done()
                    .Split(Environment.NewLine);

            sqlLines.Count().Should().Be(3);
            sqlLines.ElementAt(0).Should().Be("SELECT ST.Id, ST.Name, ST.Active ");
            sqlLines.ElementAt(1).Should().Be("FROM SampleType ST ");
            sqlLines.ElementAt(2).Should().Be("WHERE ST.Id = @Id ;");
        }

        [Fact]
        public void GeneratesSqlSelectWithBinaryExpressionWhereTest()
        {
            var sql =
                new SelectBuilder<SampleType>(ColumnsMode.ResolveAliasesAndNames)
                    .Where(x => x.Id)
                        .And(x => x.Name)
                        .Done();

            var sqlLines = sql.Split(Environment.NewLine).Select(x => x.Trim());

            sqlLines.Count().Should().Be(4);
            sqlLines.ElementAt(0).Should().Be("SELECT ST.Id, ST.Name, ST.Active");
            sqlLines.ElementAt(1).Should().Be("FROM SampleType ST");
            sqlLines.ElementAt(2).Should().Be("WHERE ST.Id = @Id");
            sqlLines.ElementAt(3).Should().Be("AND ST.Name = @Name ;");

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
