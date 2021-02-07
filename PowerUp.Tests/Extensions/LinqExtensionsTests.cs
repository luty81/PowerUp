using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PowerUp.Tests.Extensions
{
    public class LinqExtensionsTests
    {
        [Fact]
        public void GroupEveryTest()
        {
            var months = new[] { "jan", "feb", "mar", "apr", "may", "jun", "jul", "ago", "sep", "oct", "nov", "dec" };


            var quarters = months.GroupEvery(3);

            quarters.Count().Should().Be(4);
            quarters.Sum(x => x.Count()).Should().Be(12);
            quarters.ToList().TrueForAll(x => x.Count() == 3);

            Verify(Quarter(1), "jan", "feb", "mar");
            Verify(Quarter(2), "apr", "may", "jun");
            Verify(Quarter(3), "jul", "ago", "sep");
            Verify(Quarter(4), "oct", "nov", "dec");

            string[] Quarter(int i) => quarters.ElementAt(i - 1).ToArray();
        }

        private void Verify(IEnumerable<string> foundMonths, params object[] expectedMonths) =>
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
