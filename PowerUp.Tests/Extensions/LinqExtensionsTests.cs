using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PowerUp.Tests.Extensions
{
    public class LinqExtensionsTests
    {
        private readonly string[] Months = 
            new[] { "jan", "feb", "mar", "apr", "may", "jun", "jul", "ago", "sep", "oct", "nov", "dec" };

        [Fact]
        public void GroupEveryTest()
        {
            var quarters = Months.GroupEvery(3);

            quarters.Count().Should().Be(4);
            quarters.Sum(x => x.Count()).Should().Be(12);
            quarters.ToList().TrueForAll(x => x.Count() == 3);

            Verify(Quarter(1), "jan", "feb", "mar");
            Verify(Quarter(2), "apr", "may", "jun");
            Verify(Quarter(3), "jul", "ago", "sep");
            Verify(Quarter(4), "oct", "nov", "dec");

            string[] Quarter(int i) => quarters.ElementAt(i - 1).ToArray();
        }

        [Fact]
        public void NotContainsTest()
        {
            Months.NotContains("wednesday").Should().Be(true);
            new[] { 1, 3, 5 }.NotContains(2).Should().Be(true);

            Months.NotContains("dec").Should().Be(false);
            new[] { 1, 3, 5 }.NotContains(3).Should().Be(false);
        }

        [Fact]
        public void HasNoTest()
        {
            Months.HasNo("wednesday").Should().Be(true);
            new[] { 1, 3, 5 }.HasNo(2).Should().Be(true);

            Months.HasNo("dec").Should().Be(false);
            new[] { 1, 3, 5 }.HasNo(3).Should().Be(false);
        }

        private static void Verify(IEnumerable<string> foundMonths, params object[] expectedMonths) =>
            foundMonths
                .Select((item, index) => new { index, item })
                .ForEach(found => expectedMonths.ElementAt(found.index).Should().Be(found.item));
    }
}

//var index = 0;
//foreach(var expectedMonth in expectedMonths)
//{
//    foundMonths.ElementAt(index).Should().Be(expectedMonth.ToString());
//    index += 1;
//}
