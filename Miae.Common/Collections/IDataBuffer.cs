using System;
using System.Collections.Generic;
using System.Text;

namespace Miae.Collections
{
    /// <summary>
    /// 数据缓冲池接口，实现本接口的类必须保证线程安全。
    /// 一般用于线程协作：一个线程放入数据，另一个取出数据。
    /// 刘伟 liuweizzuie@qq.com
    /// 2011年2月24日
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDataBuffer<T>
    {
        int MaxCountLimit { get; }
        int Count { get; }
        T RetrieveOne();
        bool PutOne(T t);
        void Clear();
    }
}
