using System;
using System.Collections.Generic;
using System.Text;

namespace Miae.Text
{
    public class BinaryHex
    {
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

        /// <summary> 
        /// 中间带有空格的16进制字符串转字节数组
        /// </summary>
        /// <param name="hexString"></param> 
        /// <returns></returns> 
        private static byte[] HexWithSpaces2Byte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }


        /// <summary> 
        /// 字节数组转16进制字符串 
        /// </summary> 
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string Byte2Hex(byte[] bytes, bool withSpace = false)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                    if (withSpace)
                    {
                        returnStr += " ";
                    }
                }
            }
            return returnStr;
        }

        /// <summary> 
        /// 从汉字转换到16进制 
        /// </summary> 
        /// <param name="s"></param> 
        /// <param name="charset">编码,如"utf-8","gb2312"</param> 
        /// <param name="isSplitWithSemicolon">是否每字符用逗号分隔</param> 
        /// <returns></returns> 
        public static string ChineseCharacter2Hex(string s, string charset, bool isSplitWithSemicolon)
        {
            if ((s.Length % 2) != 0)
            {
                s += " ";//空格 
                //throw new ArgumentException("s is not valid chinese string!"); 
            }
            Encoding chs = Encoding.GetEncoding(charset);
            byte[] bytes = chs.GetBytes(s);
            string str = string.Empty;
            for (int i = 0; i < bytes.Length; i++)
            {
                str += string.Format("{0:X}", bytes[i]);
                if (isSplitWithSemicolon && (i != bytes.Length - 1))
                {
                    str += string.Format("{0}", ",");
                }
            }
            return str.ToLower();
        }

        ///<summary> 
        /// 从16进制转换成汉字 
        /// </summary> 
        /// <param name="hex"></param> 
        /// <param name="charset">编码,如"utf-8","gb2312"</param> 
        /// <returns></returns> 
        public static string Hex2ChineseCharacter(string hex, string charset)
        {
            if (hex == null)
                throw new ArgumentNullException("hex");
            hex = hex.Replace(",", "");
            hex = hex.Replace("\n", "");
            hex = hex.Replace("\\", "");
            hex = hex.Replace(" ", "");
            if (hex.Length % 2 != 0)
            {
                hex += "20";//空格 
            }
            // 需要将 hex 转换成 byte 数组。 
            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                try
                {
                    // 每两个字符是一个 byte。 
                    bytes[i] = byte.Parse(hex.Substring(i * 2, 2),
                    System.Globalization.NumberStyles.HexNumber);
                }
                catch
                {
                    // Rethrow an exception with custom message. 
                    throw new ArgumentException("hex is not a valid hex number!", "hex");
                }
            }
            Encoding chs = Encoding.GetEncoding(charset);
            return chs.GetString(bytes);
        }
    }
}
