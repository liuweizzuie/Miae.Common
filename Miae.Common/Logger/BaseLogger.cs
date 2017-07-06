using Miae.Threading.Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Miae.Logger
{
    public abstract class BaseLogger : BaseSingleThreadEngine, ILogger, IDisposable
    {
        public static ILogger SingleInstance { get { return null; } }

        protected BaseLogger()
        {
            this.StartTime = DateTime.Now;
            this.OnEngineStopped += (s, e) => { RaiseOnLoggerStoppedEvent(e.Exception); };
            this.Start();
        }

        #region ILogger 成员
        public abstract bool Log(string message);
        public abstract bool LogWithTime(string message);
        public abstract string FilePath { get; }
        public DateTime StartTime { get; protected set; }
        public event LoggerStopWorkingEventHandler OnLoggerStopped;
        protected void RaiseOnLoggerStoppedEvent()
        {
            if (this.OnLoggerStopped != null) { OnLoggerStopped(this, new LoggerStopWorkingEventArgs(this.StartTime, DateTime.Now)); }
        }
        protected void RaiseOnLoggerStoppedEvent(Exception exception)
        {
            if (this.OnLoggerStopped != null) { OnLoggerStopped(this, new LoggerStopWorkingEventArgs(this.StartTime, DateTime.Now, exception)); }
        }
        #endregion

        public override void Dispose()
        {
            if (this.IsRunning)
            {
                this.Stop();
            }
            base.Dispose();
        }

        public virtual void StartLogService()
        {
            this.Start();
        }

        public virtual void StopLogService()
        {
            this.Stop();
        }
    }
}
