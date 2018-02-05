using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Miae.Text
{
    /// <summary>
    /// 在二进制与十进制数字之间进行转换。
    /// </summary>
    public static class BinaryDecimal
    {
        /// <summary>
        /// 把一个符合IEEE-754标准的64位二进制字节组转化为双精度浮点数。
        /// </summary>
        /// <param name="bytes">要转换的二进制字节组，长度必须是8位。</param>
        /// <returns>此二进制字节组表示的双精度浮点数。</returns>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static double ToDouble(this byte[] bytes)
        {
            Debug.Assert(bytes.Length == 8, "The Length of byts must be 8.");

            if (bytes.Length != 8) { throw new ArgumentOutOfRangeException("The Length of byts must be 8."); }

            double d = BitConverter.ToDouble(bytes, 0);
            return d;
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

        /// <summary>
        /// 转为十进制字符串。一个典型的应用场景就是不规则点分十进制，如5位，6位，不能用 IPAddress 。
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="spliter">如果指定为 char.MinValue，则会忽略。</param>
        /// <returns></returns>
        public static string ToDecimalString(this byte[] bytes, char spliter)
        {
            string returnStr = string.Empty;
            for (int i = 0; i < bytes.Length; i++)
            {
                returnStr += bytes[i].ToString();

                if (spliter != char.MinValue && i != bytes.Length - 1)
                {
                    returnStr += spliter;
                }
            }
            return returnStr;
        }

        /// <summary>
        /// 从数组中返回唯一的 byte 。用以屏蔽 [0] 。
        /// 为什么要屏蔽 [0] ？ 因为它看起来让人不舒服。
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte ToByte(this byte[] bytes)
        {
            Debug.Assert(bytes.Length == 1, "the bytes array must contains just one element");
            return bytes[0];
        }

        #region ToBytes
        /// <summary>
        /// 把 Int16 类型的数字转换成为 两个Byte。
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static byte[] ToBytes(this Int16 target)
        {
            byte[] result = new byte[2];
            result[0] = (byte)(target & 0xFF);
            result[1] = (byte)((target & 0xFF00) >> 8);
            return result;
        }

        /// <summary>
        /// 把 UInt16 类型的数字转换成为 两个Byte。
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static byte[] ToBytes(this UInt16 target)
        {
            byte[] result = new byte[2];

            result[0] = (byte)(target & 0xFF);

            result[1] = (byte)((target & 0xFF00) >> 8);

            return result;
        }

        public static byte[] ToBytes(this Int32 target)
        {
            byte[] result = new byte[4];

            result[0] = (byte)(target & 0xFF);
            result[1] = (byte)(target & 0xFF00 >> 8);
            result[2] = (byte)(target & 0xFF0000 >> 16);
            result[3] = (byte)(target & 0xFF000000 >> 24);

            return result;
        }
        #endregion 


    }
}
