using System;
using System.Collections.Generic;
using System.Text;

namespace Miae.Threading.Engine
{
    public class DelegateSingleThreadEngine : BaseSingleThreadEngine
    {
        private Func<bool> task;

        /// <summary>
        /// 获取 DelegateSingleThreadEngine 的新实例。
        /// </summary>
        /// <param name="task">委托的任务。</param>
        public DelegateSingleThreadEngine(Func<bool> task)
        {
            this.task = task;
        }

        protected override bool EngineTask()
        {
            return task();
        }
    }
}
