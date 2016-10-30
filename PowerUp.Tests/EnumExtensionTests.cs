using Microsoft.VisualStudio.TestTools.UnitTesting;
using PowerUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpTestsEx;
using System.ComponentModel.DataAnnotations;

namespace PowerUp.Tests
{
    [TestClass]
    public class EnumExtensionsTests
    {
        [TestMethod]
        public void ShouldBePossibleToSplitCamelCaseInBlankSpaces()
        {
            EnumSample.SomeEnumElement.Display().Should().Be("Some Enum Element");
        }

        [TestMethod]
        public void ShouldBePossibleToGetEnumElementDescription()
        {
            EnumSample.SomeDecoratedEnumElement.Display().Should().Be("My custom display");
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

    public enum EnumSample
    {
        SomeEnumElement,
        [DisplayAs("My custom display")]
        SomeDecoratedEnumElement
    }
}