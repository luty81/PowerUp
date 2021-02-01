﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using static System.StringSplitOptions;

namespace PowerUp
{
    public static class StringExtensions
    {
        public static string ToDisplayName(this string name) => 
            Regex.Replace(name, @"([A-Z])", " $1");
        
        public static IEnumerable<string> SplitBy(this string self, char @char, StringSplitOptions options = RemoveEmptyEntries) =>
            self.Split(new[] { @char }, options);
        
        public static string Minify(this string self) => 
            self.Trim().ToLower();

        public static IEnumerable<string> SplitByPascalCase(this string self) => 
            Regex.Replace(self, @"([A-Z])", " $1").SplitBy(' ');

        public static IEnumerable<string> Split(this string self) => 
            self.SplitBy(' ').Select(x => x.Trim());

        public static string Join(this IEnumerable<string> self, string separator = null) => 
            string.Join(separator, self);

        public static bool IsNotEmpty(this string self) => 
            !self.IsEmpty();
        public static bool IsEmpty(this string self) => 
            string.IsNullOrWhiteSpace(self);

        public static bool SameAs(this string self, string otherString)
        {
            if (self != null && otherString != null)
            {
                return self.Trim().ToLower() == otherString.Trim().ToLower();
            }

            return self == null && otherString == null;
        }
    }
}
