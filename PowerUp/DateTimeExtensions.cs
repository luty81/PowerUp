using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerUp
{
    public static class DateTimeExtensions
    {
        public static DateTime SetTime(this DateTime self, int hours, int minutes, int seconds = 0)
        {
            return new DateTime(self.Year, self.Month, self.Day, hours, minutes, 0);
        }

        public static DateTime SetTime(this DateTime self, string onlyTimeAsString)
        {
            var timeParts = onlyTimeAsString.Split(':').Select(x => int.Parse(x.Trim())).ToArray();
            var seconds = timeParts.Count() > 2 ? timeParts[2] : 0;
            return new DateTime(self.Year, self.Month, self.Day, timeParts[0], timeParts[1], seconds);
        }

        public static string OnlyDate(this DateTime? self, string dateFormat = "dd/MM/yyyy")
        {
            return self.HasValue ? OnlyDate(self.Value, dateFormat) : string.Empty;
        }

        public static string OnlyDate(this DateTime self, string dateFormat = "dd/MM/yyyy")
        {
            return self.ToString(dateFormat);
        }

        public static string OnlyTime(this DateTime? self, string timeFormat = "HH:mm")
        {
            return self.HasValue ? OnlyTime(self.Value) : string.Empty;

        }

        public static string OnlyTime(this DateTime self, string timeFormat = "HH:mm")
        {
            return self.ToString(timeFormat);
        }

        public static bool IsNotNullOrZero(this DateTime? self)
        {
            return self.HasValue && self.Value > DateTime.MinValue;
        }

        public static bool IsNullOrZero(this DateTime? self)
        {
            return !self.HasValue || self.Value == DateTime.MinValue;
        }

        public static bool TheSameAs(this DateTime self, DateTime dateToCompare)
        {
            return self.Year == dateToCompare.Year
                && self.Month == dateToCompare.Month
                && self.Day == dateToCompare.Day
                && self.Hour == dateToCompare.Hour
                && self.Minute == dateToCompare.Minute
                && self.Second == dateToCompare.Second;
        }

    }
}
