using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerUp
{
    public static class LinqExtensions
    {
        public static IEnumerable<IGrouping<int, T>> GroupEvery<T>(this IEnumerable<T> self, int everyN)
        {
            return
                self
                    .Select((x, index) => new { groupN = index + 1 - (index % everyN), element = x })
                    .GroupBy(x => x.groupN, y => y.element);
        }

        public static Dictionary<TKey, IEnumerable<T>> GroupEveryAsDictionary<T, TKey>(this IEnumerable<T> self, 
            int everyN, Func<T, TKey> keySelector)
        {
            return
                self
                    .Select((x, index) => new { groupN = index + 1 - (index % everyN), element = x })
                    .GroupBy(x => x.groupN, y => y.element)
                    .ToDictionary(x => keySelector.Invoke(x.First()), x => x.AsEnumerable());
        }

    }
}
