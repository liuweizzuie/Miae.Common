using System;
using System.Collections.Generic;
using System.Text;

namespace Miae.Text
{
    class BinaryHelper
    {
        /// <summary>
        /// 在data中搜索pattern，返回data中pattern的位置。
        /// </summary>
        /// <param name="data">目标字节。</param>
        /// <param name="pattern">匹配字节。</param>
        /// <returns>如果有多条符合，则有多个位置；如果没有，则返回一个空的List</returns>
        public static List<int> IndexOf(byte[] data, byte[] pattern)
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
    }
}
