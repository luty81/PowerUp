using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using static System.StringSplitOptions;

namespace PowerUp
{
    public static class StringExtensions
    {
        public static string ToDisplayName(this string name) => Regex.Replace(name, @"([A-Z])", " $1");
        public static string Minify(this string self) => self.Trim().ToLower();
        public static string FirstCharacter(this string self) => self.First().ToString();


        public static bool IsNotEmpty(this string self) => !self.IsEmpty();
        public static bool IsEmpty(this string self) => string.IsNullOrWhiteSpace(self);
        public static bool SameAs(this string self, string otherString)
        {
            if (self != null && otherString != null)
            {
                return self.Trim().ToLower() == otherString.Trim().ToLower();
            }

            return self == null && otherString == null;
        }


        public static string RegexReplace(this string self, string pattern, string replacement) =>
            Regex.Replace(self, pattern, replacement);

        public static string RegexRemove(this string self, string pattern) => 
            self.RegexReplace(pattern, string.Empty);
        
        public static string RemoveAll(this string self, params string[] chunksToBeRemoved) =>
            self.ReplaceAll(chunksToBeRemoved, string.Empty);
        public static string ReplaceAll(this string self, string[] toBeReplaced, string newValue)
        {
            toBeReplaced.ForEach(str => { self = self.Replace(str, newValue); });
            return self;
        }

        public static string RemoveDiacritics(this string self)
        {
            var resultChars = 
                self.Normalize(NormalizationForm.FormD)
                    .Where(IsNonSpacingMarkch)
                    .ToArray();

            return new string(resultChars).Normalize(NormalizationForm.FormC);

            bool IsNonSpacingMarkch(char ch) => CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark;
        }

        /// <summary>
        /// Splits the given string by line breaks
        /// </summary>
        public static IEnumerable<string> Lines(this string self) =>
            self.Split(Environment.NewLine);

        public static IEnumerable<string> Split(this string self) =>
            self.SplitBy(' ').Select(x => x.Trim());

        public static IEnumerable<string> SplitBy(this string self, char @char, StringSplitOptions options = RemoveEmptyEntries) =>
            self.Split(new[] { @char }, options);

        public static IEnumerable<string> SplitByPascalCase(this string self) =>
            Regex.Replace(self, @"([A-Z])", " $1").SplitBy(' ');

        public static string JoinByComma(this IEnumerable<string> self) =>
            string.Join(", ", self);

        public static string Join(this IEnumerable<string> self, string separator = null) => 
            string.Join(separator, self);

        public static string GroupByFormat(this string self, int groupSize, string separator = "") => 
            self.GroupEvery(groupSize)
                .Select(x => new string(x.ToArray()))
                .Join(separator);
    }
}
