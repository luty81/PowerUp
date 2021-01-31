using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PowerUp.Tests
{
    public class EnumExtensionsTests
    {
        [Fact]
        public void ShouldBePossibleToSplitCamelCaseInBlankSpaces()
        {
            EnumSample.SomeEnumElement.Display().Should().Be("Some Enum Element");
        }

        [Fact]
        public void ShouldBePossibleToGetEnumElementDescription()
        {
            EnumSample.SomeDecoratedEnumElement.Display().Should().Be("My custom display");
        }
    }

    public enum EnumSample
    {
        SomeEnumElement,
        [DisplayAs("My custom display")]
        SomeDecoratedEnumElement
    }
}