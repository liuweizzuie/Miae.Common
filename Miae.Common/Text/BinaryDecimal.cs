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
        public static double BinaryToDouble(byte[] bytes)
        {
            Debug.Assert(bytes != null, "bytes can not be null.");
            Debug.Assert(bytes.Length == 8, "The Length of byts must be 8.");

            if (bytes == null) { throw new ArgumentNullException("bytes can not be null."); }
            else if (bytes.Length != 8) { throw new ArgumentOutOfRangeException("The Length of byts must be 8."); }

            double d = BitConverter.ToDouble(bytes, 0);
            return d;
        }

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

    }
}
