using System;
using System.Collections.Generic;
using System.Text;

namespace Miae
{
    /// <summary>
    /// 空异常，即没有异常的异常。
    /// </summary>
    public class EmptyException : Exception
    {
        public EmptyException() : base(string.Empty) { }
    }
}
