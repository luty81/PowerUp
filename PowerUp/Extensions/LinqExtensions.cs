using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerUp
{
    public static class LinqExtensions
    {
        public static IEnumerable<IGrouping<int, T>> GroupEvery<T>(
            this IEnumerable<T> self, int everyN) => self
                .Select((item, i) => new NGroupedItem<T>(item, i, everyN))
                .GroupBy(x => x.GroupKey, y => y.Item);

        public static Dictionary<TKey, IEnumerable<T>> GroupEveryAsDictionary<T, TKey>(
            this IEnumerable<T> self, int everyN, Func<T, TKey> keySelector) => self
                .GroupEvery<T>(everyN)
                .ToDictionary(
                    x => keySelector.Invoke(x.First()), 
                    x => x.AsEnumerable());

        public static void ForEach<T>(this IEnumerable<T> self, Action<T> action) =>
            self.ToList().ForEach(action);
    }

    class NGroupedItem<T>
    {
        public T Item { get; }
        public int N { get; }
        public int Index { get; }
        public int GroupKey { get => Index + 1 - (Index % N); }

        public NGroupedItem(T item, int index, int n)
        {
            Item = item;
            Index = index;
            N = n;
        }
    }
}