using System;

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

        private static void Fail(object expected, object found) => Fail($"Expected: {expected} Actual: {found}.");
        private static void Fail(string message) => throw new Exception($"{message}");

    }
}
