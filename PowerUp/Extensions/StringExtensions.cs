using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PowerUp
{
    public static class StringExtensions
    {
        public static string ToDisplayName(this string name)
        {
            return Regex.Replace(name, @"([A-Z])", " $1");
        }

        public static IEnumerable<string> SplitBy(this string self, char @char, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
        {
            return self.Split(new[] { @char }, options);
        }

        public static bool SameAs(this string self, string otherString)
        {
            if(self != null && otherString != null)
            {
                return self.Trim().ToLower() == otherString.Trim().ToLower();
            }

            return self == null && otherString == null;
        }

        public static string Minify(this string self)
        {
            return self.Trim().ToLower();
        }

    }
}
