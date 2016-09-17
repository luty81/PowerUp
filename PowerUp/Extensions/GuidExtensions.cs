using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerUp.Extensions
{
    public static class GuidExtensions
    {
        public static bool IsNullOrZero(this Guid? self)
        {
            return self.HasValue ? self.Value.IsZero() : true;
        }

        public static bool IsZero(this Guid self)
        {
            return self == new Guid(new String('0', 32));
        }
    }
}
