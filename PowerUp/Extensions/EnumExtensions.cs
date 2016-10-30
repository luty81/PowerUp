using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;

namespace PowerUp
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Get enum element display.
        /// </summary>
        /// <param name="self">Enum element</param>
        /// <returns>
        /// If DisplayAsAttribute is present returns custom display of enum element, 
        /// otherwise returns enum element name with blank space before every capital letter.
        /// </returns>
        public static string Display(this Enum self)
        {
            var enumValue = self.ToString();
            var displayAs = self.GetType().GetMember(enumValue).First().GetCustomAttribute<DisplayAsAttribute>();
            return displayAs != null ? displayAs.Value : Regex.Replace(enumValue, @"([A-Z])", " $1").Trim();
        }

        public static string ToString(this Enum self) 
        {
            return Display(self);
        }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class DisplayAsAttribute: Attribute
    {
        public string Value { get; set; }

        public DisplayAsAttribute(string display)
        {
            Value = display;
        }
    }
}
