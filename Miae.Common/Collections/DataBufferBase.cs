using System;
using System.Collections.Generic;
using System.Text;

namespace Miae.Collections
{
    /// <summary>
    /// 各种数据缓冲区的基础类
    /// 刘伟 liuweizzuie@qq.com
    /// 2011年2月24日
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class DataBufferBase<T> : IDataBuffer<T>
    {
        protected static readonly int MAX_COUNR_LIMIT = 1024;

        #region IDataBuffer 成员
        public abstract int MaxCountLimit { get; }
        public abstract int Count { get; }
        public abstract T RetrieveOne();
        public abstract bool PutOne(T t);
        public abstract void Clear();
        #endregion
    }
}
