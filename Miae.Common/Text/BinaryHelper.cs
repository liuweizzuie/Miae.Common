using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

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

        public static void SetZero(this byte[] bytes)
        {
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = 0x00;
            }
        }

        /// <summary>
        /// 16进制字符串转字节数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] Hex2Byte(string hexString)
        {
            if (hexString.Length % 2 == 1) { throw new InvalidOperationException("长度不对！"); }
            int len = hexString.Length / 2;
            byte[] arr = new byte[len];
            for (int i = 0; i < len; i++)
            {
                arr[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
            return arr;
        }

        public static string ToHexString(this byte[] bytes, char spliter)
        {
            string returnStr = string.Empty;
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");

                    if (spliter != char.MinValue)
                    {
                        returnStr += spliter;
                    }
                }
            }
            return returnStr;
        }

        public static string ToHexString(this byte[] bytes)
        {
            return ToHexString(bytes, char.MinValue);
        }

        public static short ToShort(this byte[] bytes, bool bigEndian)
        {
            Debug.Assert(bytes.Length == 2);

            if (bigEndian)
            {
                return (short)((bytes[1] << 8) + bytes[0]);
            }
            else
            {
                return (short)((bytes[0] << 8) + bytes[1]);
            }
        }

        public static byte ToBcdValue(this byte @byte)
        {
            return (byte)((@byte / 0x10) * 10 + (@byte % 0x10));
        }
    }
}
