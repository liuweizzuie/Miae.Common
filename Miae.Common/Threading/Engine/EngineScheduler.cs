using System;

namespace Miae.Threading.Engine
{
    /// <summary>
    /// 引擎的周期性调度器。
    /// 在复杂的引擎中，有多个任务，这些任务需要执行的时间周期不同，所以需要判断是否到了某任务的执行时间。
    /// 用法： 在 BaseSingleThreadEngine 子类中：
    /// EngineScheduler scheduler = new EngineScheduler();
    /// protected override bool EngineTask()
    /// {
    ///    scheduler.ScheduleTaskEveryPeriod(SomeMethod);
    ///    scheduler.UpdateEngineTime();
    /// }
    /// </summary>
    public class EngineScheduler
    {
        /// <summary>
        /// 上次引擎运行的时间。用于计算两次引擎执行的时间差。
        /// </summary>
        private DateTime last_engine_time;
        private int period = 1000;

        public EngineScheduler(int periodMs = 1000)
        {
            this.period = periodMs;
        }

        /// <summary>
        /// 引擎定时任务：每秒调用一次
        /// </summary>
        /// <param name="actions"></param>
        public void ScheduleTaskEveryPeriod(params Action[] actions)
        {
            DosomethingEveryPeriod(() =>
            {
                foreach (Action action in actions)
                {
                    action();
                }
            });
        }


        /// <summary>
        /// 每秒更新一次 last_engine_time，用以定时任务的时间间隔检查。
        /// </summary>
        public void UpdateEngineTime()
        {
            DosomethingEveryPeriod(() => last_engine_time = DateTime.Now);
        }

        private void DosomethingEveryPeriod(Action action)
        {
            DateTime now = DateTime.Now;
            if ((now - last_engine_time).TotalMilliseconds > period)
            {
                action();
            }
        }

    }
}
