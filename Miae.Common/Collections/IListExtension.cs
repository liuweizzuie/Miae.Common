using System;
using System.Collections.Generic;

namespace Miae.Collections
{
    public static class IListExtension
    {
        public static IList<T> AddRange<T>(this IList<T> list, IEnumerable<T> items)
        {
            foreach (T t in items) { list.Add(t); }
            return list;
        }

        public static IList<T> RemoveRange<T>(this IList<T> list, IEnumerable<T> items)
        {
            foreach (T t in items) { list.Remove(t); }
            return list;
        }

        public static IList<T> SubList<T>(this IList<T> list, int startIndex, int count)
        {
            IList<T> result = new List<T>();
            for (int i = 0; i < count && startIndex + i < list.Count; i++)
            {
                result.Add(list[startIndex + i]);
            }

            return result;
        }

        public static IList<T> SubList<T>(this IList<T> list, int startIndex)
        {
            return list.SubList(startIndex, list.Count - startIndex);
        }

        public static IList<T> RemoveWhere<T>(this IList<T> list, Predicate<T> predicate)
        {
            for (int i = list.Count -1; i >= 0; i--)
            {
                if (predicate(list[i]))
                {
                    list.RemoveAt(i);
                }
            }
            return list;
        }
    }
}