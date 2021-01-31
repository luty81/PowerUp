using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PowerUp.Tests
{
    public class LinqExtensionsTests
    {
        [Fact]
        public void GroupEveryTest()
        {
            var months = new[] { "", "jan", "feb", "mar", "apr", "may", "jun", "jul", "ago", "sep", "oct", "nov", "dec" };


            var quarters = months.Skip(1).GroupEvery(3);

            quarters.Count().Should().Be(4);
            quarters.Sum(x => x.Count()).Should().Be(12);
            quarters.ToList().TrueForAll(x => x.Count() == 3);

            VerifyQuarter(quarters.ElementAt(0).ToList(), months[1], months[2], months[3]);
            VerifyQuarter(quarters.ElementAt(1).ToList(), months[4], months[5], months[6]);
            VerifyQuarter(quarters.ElementAt(2).ToList(), months[7], months[8], months[9]);
            VerifyQuarter(quarters.ElementAt(3).ToList(), months[10], months[11], months[12]);
        }

        private void VerifyQuarter(IEnumerable<string> foundMonths, params object[] expectedMonths)
        {
            var index = 0;
            foreach(var expectedMonth in expectedMonths)
            {
                foundMonths.ElementAt(index).Should().Be(expectedMonth.ToString());
                index += 1;
            }
        }
    }
}