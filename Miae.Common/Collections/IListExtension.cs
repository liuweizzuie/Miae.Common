using System;
using System.Collections.Generic;
using System.Linq;

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

        #region Minus
        /// <summary>
        /// 集合相减，如 A - B， 得到 A 中存在，而 B 中不存在的元素的集合，结果元素为A中元素的引用。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static IEnumerable<T> Minus<T>(this IEnumerable<T> list, IEnumerable<T> other) where T : class
        {
            IList<T> result = new List<T>();

            foreach (T item in list)
            {
                //string 到底还是引用类型，使用 == 来比较，并不靠谱。
                Func<T, bool> func = item is string ? 
                    new Func<T, bool>(o => object.Equals(o, item)) : 
                    new Func<T, bool>(o => o == item); 

                if (!other.Any(func))
                {
                    result.Add(item);
                }
            }
            return result;
        }
        #endregion
    }
}