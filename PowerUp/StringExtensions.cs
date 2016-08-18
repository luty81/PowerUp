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
    }
}
