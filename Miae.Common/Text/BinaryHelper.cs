using Miae.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Miae.Text
{
    public static class BinaryHelper
    {
        /// <summary>
        /// 在data中搜索pattern，返回data中pattern的位置。
        /// </summary>
        /// <param name="data">目标字节。</param>
        /// <param name="pattern">匹配字节。</param>
        /// <returns>如果有多条符合，则有多个位置；如果没有，则返回一个空的List</returns>
        public static List<int> IndexOf(this byte[] data, byte[] pattern)
        {
            List<int> matchedPos = new List<int>();

            if (data.Length == 0 || data.Length < pattern.Length) return matchedPos;

            int end = data.Length - pattern.Length;
            bool matched = false;

            for (int i = 0; i <= end; i++)
            {
                for (int j = 0; j < pattern.Length || !(matched = (j == pattern.Length)); j++)
                {
                    if (data[i + j] != pattern[j]) break;
                }
                if (matched)
                {
                    matched = false;
                    matchedPos.Add(i);
                }
            }
            return matchedPos;
        }

        /// <summary>
        /// 将一个二进制串按指定的分割标志进行分割。
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="spliter"></param>
        /// <returns></returns>
        public static IList<byte[]> Split(this byte[] bytes, byte[] spliter)
        {
            IList<byte[]> result = new List<byte[]>();

            IList<int> indexes = bytes.IndexOf(spliter);

            if (indexes.Count == 0)
            {
                result.Add(bytes);
            }
            else
            {
                for (int i = 0; i < indexes.Count; i++)
                {
                    if (i == indexes.Count - 1)
                    {
                        byte[] lastBytes = bytes.SubList(indexes[i], bytes.Length - indexes[i]).ToArray();
                        result.Add(lastBytes);
                    }
                    else
                    {
                        byte[] msgBinary = bytes.SubList(indexes[i], indexes[i + 1] - indexes[i]).ToArray();
                        result.Add(msgBinary);
                    }
                }
            }

            return result;
        }

        public static void SetZero(this byte[] bytes)
        {
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = 0x00;
            }
        }
    }
}
