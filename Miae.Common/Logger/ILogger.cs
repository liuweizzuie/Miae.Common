using System;
using System.Collections.Generic;
using System.Text;

namespace Miae.Logger   
{
    /// <summary>
    /// 当缓冲区满，或者其他原因造成日志停止工作时，将会触发此事件类型。
    /// 刘伟 liuweizzuie@qq.com
    /// 2011年2月24日
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void LoggerStopWorkingEventHandler(object sender, LoggerStopWorkingEventArgs e);

    /// <summary>
    /// 日志停止工作时，可以得到日志服务的开始和停止时刻。
    /// 刘伟 liuweizzuie@qq.com
    /// 2011年2月24日
    /// </summary>
    public class LoggerStopWorkingEventArgs : ExceptionEventArgs
    {
        public DateTime LoggerStartTime { get; protected set; }
        public DateTime LoggerStopTime { get; protected set; }

        public LoggerStopWorkingEventArgs(
            DateTime loggerStartTime,
            DateTime loggerStopTime,
            Exception exception) : base(exception)
        {
            this.LoggerStartTime = loggerStartTime;
            this.LoggerStopTime = loggerStopTime;
        }

        public LoggerStopWorkingEventArgs(
            DateTime loggerStartTime,
            DateTime loggerStopTime)
            : this(loggerStartTime, loggerStopTime, new EmptyException()) { }
    }

    public interface ILogger
    {
        bool Log(string message);
        bool LogWithTime(string message);
        bool IsRunning { get; }
        DateTime StartTime { get; }
        event LoggerStopWorkingEventHandler OnLoggerStopped;
        void StartLogService();
        void StopLogService();
    }
}
