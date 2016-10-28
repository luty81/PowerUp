using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpTestsEx;

namespace PowerUp
{
    public static class PrimitiveVerifiers
    {
        public static void ShouldBeToday(this DateTime self)
        {
            self.Date.Should().Be(DateTime.Today);
        }

        public static void ShouldBeNull<T>(this T? self) where T : struct
        {
            self.HasValue.Should().Be.False();
        }

        public static void ShouldBeNotNull<T>(this T? self) where T : struct
        {
            self.HasValue.Should().Be.True();
        }

    }
}
