using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerUp
{
    public static class GuidExtensions
    {
        public static Guid NullGuid => new Guid(new string('0', 32)); 
        
        public static bool IsNullOrZero(this Guid? self) => 
            self == null || self.IsZero();
        public static bool IsZero(this Guid? self) => 
            self.HasValue && self == NullGuid;

        public static Guid ToGuid(this byte[] self) => new Guid(self);

    }

}
