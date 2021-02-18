using Xunit;
using System.Linq;
using FluentAssertions;

namespace PowerUp.Tests.Extensions
{
    public class StringExtensionsTests
    {
        [Fact]
        public void Split_to_PascalCaseTest()
        {
            var result = "CamelCase".SplitByPascalCase();

            result.Count().Should().Be(2);
            result.ElementAt(0).Should().Be("Camel");
            result.ElementAt(1).Should().Be("Case");
        }

        [Fact]
        public void GroupByFormat()
        {
            "825FD9F98A000000"
                .GroupByFormat(4, "-")
            .Should().Be("825F-D9F9-8A00-0000");
        }
    }
}
