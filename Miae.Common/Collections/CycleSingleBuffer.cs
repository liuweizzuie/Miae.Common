using System;
using System.Collections.Generic;
using System.Text;

namespace Miae.Collections
{
    /// <summary>
    /// 圆形数据缓冲池,单缓冲
    /// 刘伟 liuweizzuie@qq.com
    /// 2011年2月24日
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CycleSingleBuffer<T> : DataBufferBase<T>
    {
        #region private properties
        private IList<T> buffer = new List<T>(MAX_COUNR_LIMIT);
        private object bufferLocker = new object();
        private int head = 0;
        private int tail = 0;
        private int count = 0;
        #endregion

        /// <summary>
        /// 对于引用类型的数据，如果不想添加相同的数据，则可以实现此事件。
        /// 如果返回0，则表明两者相等，Buffer不会将新的数据加入进去。
        /// </summary>
        public event Comparison<T> CompareIsEqual;

        protected bool OnCompare(T itemIn, T itemOut)
        {
            if (CompareIsEqual != null)
            {
                return CompareIsEqual(itemIn, itemOut) == 0;
            }
            return false;
        }

        public override int MaxCountLimit
        {
            get
            {
                return MAX_COUNR_LIMIT;
            }
        }
        public override int Count
        {
            get
            {
                return this.count;
            }
        }

        public CycleSingleBuffer()
        {
            for (int i = 0; i < MAX_COUNR_LIMIT; i++)
            {
                buffer.Add(default(T));
            }
        }

        public override T RetrieveOne()
        {
            if (count == 0)
            {
                return default(T);
            }

            lock (this.bufferLocker)
            {
                T t = this.buffer[tail];
                tail = (tail + 1) % MAX_COUNR_LIMIT;
                count--;
                return t;
            }
        }

        public override bool PutOne(T t)
        {
            if (count >= MAX_COUNR_LIMIT)
            {
                return false;
            }

            lock (this.bufferLocker)
            {
                int count_ = 0;
                if (this.CompareIsEqual != null)
                {
                    //执行MAX_COUNR_LIMIT次，太恐怖了。
                    //count_ = this.buffer.Count((tIn) =>
                    //{
                    //    bool result = OnCompare(tIn, t);
                    //    return result;
                    //});

                    for (int index = tail, c = 0; index != this.head && c < count; index = (index + 1) % MAX_COUNR_LIMIT, c++)
                    {
                        if (OnCompare(buffer[index], t))
                        {
                            count_++;
                            break;
                        }
                    }
                }

                if (count_ == 0)
                {
                    this.buffer[head] = t;
                    count++;
                    head = (head + 1) % MAX_COUNR_LIMIT;
                }
            }
            return true;
        }

        public override void Clear()
        {
            count = 0;
        }
    }
}
