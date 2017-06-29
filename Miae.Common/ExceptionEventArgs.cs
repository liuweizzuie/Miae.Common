using System;
using System.Collections.Generic;
using System.Text;

namespace Miae
{
    /// <summary>
    /// 为 Exception 事件提供数据。
    /// </summary>
    public class ExceptionEventArgs : EventArgs
    {
        /// <summary>
        /// 获取Exception。
        /// </summary>
        public Exception Exception { get; protected set; }

        /// <summary>
        /// 获取 ExceptionEventArgs 的新实例。
        /// </summary>
        public ExceptionEventArgs(Exception exception)
        {
            this.Exception = exception;
        }

        public ExceptionEventArgs()
        {
            this.Exception = new EmptyException();
        }
    }
}
