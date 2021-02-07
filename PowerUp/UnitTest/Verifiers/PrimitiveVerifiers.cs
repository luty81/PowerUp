using System;
using System.Collections.Generic;
using System.Linq;

namespace PowerUp
{
    public static class PrimitiveVerifiers
    {
        public static void ShouldBeToday(this DateTime self)
        {
            if (self.Date != DateTime.Today)
                Fail(DateTime.Today, self.Date);
        }

        public static void ShouldBeNull<T>(this T? self) where T : struct
        {
            if (self.HasValue)
                Fail($"A null reference was expected, but an instance of {self.Value.GetType().Name} was found.");
        }

        public static void ShouldBeNotNull<T>(this T? self) where T : struct
        {
            if (self.HasValue)
                return;

            Fail($"An instance of {typeof(T).Name} was expected, but a null reference was found.");
        }

        public static void ShouldBe<T>(this IEnumerable<T> foundList, params T[] expectedItems) where T : class
        {
            if (foundList.Count() != expectedItems.Length)
                Fail($"A mismatch list size was found. Expected size: {expectedItems.Length}. Found: {foundList.Count()}");

            expectedItems
                .Select((expected, itemIndex) => new { expected, itemIndex })
                .ForEach(x =>
                {
                    var found = foundList.ElementAt(x.itemIndex);
                    if (! x.expected.Equals(found))
                        Fail($"{DefaultFailMessage(x.expected, found)} at {x.itemIndex} position");
                });
        }

        private static void Fail(object expected, object found) => Fail(DefaultFailMessage(expected, found));
        private static void Fail(string message) => throw new Exception($"{message}");

        private static string DefaultFailMessage(object expected, object found) => $"Expected: {expected} Actual: {found}";

    }
}
