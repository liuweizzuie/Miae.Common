using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security;
using System.Text;
using System.Threading;

namespace Miae.Logger
{

    /// <summary>
    /// 单例模式的文本日志。操作最简化：提供一个写日志的方法，直接使用即可，而无须进行任何配置。
    /// </summary>
    public class TxtLogger : BaseLogger, ITxtLogger
    {
        #region SingleInstance
        private static ILogger singleInstance;
        private static object singleInstanceLocker = new object();
        public static new ILogger SingleInstance
        {
            get
            {
                if (singleInstance == null)
                {
                    lock (singleInstanceLocker)
                    {
                        if (singleInstance == null)
                        {
                            singleInstance = new TxtLogger();
                        }
                    }
                }
                return singleInstance;
            }
        }
        #endregion

        #region Fields
        private Queue<string> buffer = new Queue<string>();
        private ManualResetEvent bufferReadLocker = new ManualResetEvent(true);  //read-locker. (读锁)
        private Mutex bufferWriteLocker = new Mutex(false);  //write-locker.(写锁)
        #endregion

        #region Properties

        #region DicectoryPath
        private string directoryPath;
        public string DirectoryPath
        {
            get
            {
                return directoryPath;
            }
            set
            {
                try
                {
                    DirectoryInfo di = new DirectoryInfo(value);
                    this.directoryPath = value;
                }
                catch (ArgumentNullException)
                {
                    throw;
                }
                catch (SecurityException)
                {
                    throw;
                }
                catch (ArgumentException)
                {
                    throw;
                }
                catch (PathTooLongException)
                {
                    throw;
                }
            }
        }

        #region Path
        public override string FilePath { get { return Path.Combine(this.DirectoryPath, this.CurrentFileName); } }
        #endregion

        #endregion

        #region FileIntervalMin
        private uint fileIntervalMin;
        public uint FileIntervalMin
        {
            get { return this.fileIntervalMin; }
            set
            {
                if (value == 0)
                {
                    throw new ArgumentException("TxtLogger: FileIntervalMin can not be zero!");
                }
                this.fileIntervalMin = value;
            }
        }
        #endregion

        #region MaxBufferCount
        private uint maxBufferCount;
        public uint MaxBufferCount
        {
            get { return this.maxBufferCount; }
            set
            {
                if (value == 0)
                {
                    throw new ArgumentException("TxtLogger: MaxBufferCount can not be zero.");
                }
                maxBufferCount = value;
            }
        }
        #endregion

        #region CurrentFileName
        /// <summary>
        /// Name of the txt file that is in use.
        /// </summary>
        protected string CurrentFileName { get; set; }
        #endregion

        #endregion

        #region Constructor
        //Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)

        public TxtLogger() : this(Path.Combine(Directory.GetCurrentDirectory(), "log")) { }

        public TxtLogger(string directoryPath)
        {
            
            this.DirectoryPath = directoryPath;
            this.Initialize();
        }

        public TxtLogger(string directoryPath, uint fileIntervalMin) : this(directoryPath)
        {
            this.FileIntervalMin = fileIntervalMin;
        }

        private void Initialize()
        {
            this.FileIntervalMin = 1440;
            this.MaxBufferCount = uint.MaxValue;
        }
        #endregion

        /// <summary>
        /// Generate a new file name.
        /// </summary>
        /// <returns>a new file name.</returns>
        private string GetNewFileName(DateTime now)
        {
            TimeSpan ts = new TimeSpan(now.Hour, now.Minute, now.Second);
            uint fileTime_Minutes = ((uint)ts.TotalMinutes / this.FileIntervalMin) * this.FileIntervalMin;
            DateTime fileTime = new DateTime(now.Year, now.Month, now.Day).AddMinutes(fileTime_Minutes);

            StringBuilder sb = new StringBuilder();
            sb.Append(fileTime.ToString("yyyy-MM-dd-HH-mm"));
            sb.Append(".txt");
            return sb.ToString();
        }

        private bool EnsureDirectoryExists()
        {
            System.IO.DirectoryInfo info = new System.IO.DirectoryInfo(this.DirectoryPath);
            if (!info.Exists)
            {
                try
                {
                    info.Create();
                }
                catch (System.IO.IOException)
                {
                    return false;
                }
            }
            return true;
        }

        private bool EnsureFileExists(DateTime now)
        {
            string newFileName = GetNewFileName(now);
            if (!string.Equals(this.CurrentFileName, newFileName))
            {
                this.CurrentFileName = newFileName;
            }

            if (!File.Exists(this.FilePath))
            {
                lock (singleInstanceLocker)
                {
                    using (Stream stream = System.IO.File.Create(this.FilePath))
                    {
                        try
                        {
                            //stream.Close();
                            stream.Dispose();
                        }
                        catch (Exception)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 取出所有信息，并将它全部写进日志。
        /// </summary>
        protected virtual bool LogExistingMessage()
        {
            if (!EnsureDirectoryExists()) { return false; }
            if (!EnsureFileExists(DateTime.Now)) { return false; }

            StreamWriter writer = new StreamWriter(File.Open(this.FilePath, FileMode.Append)) { AutoFlush = true };
            while (this.buffer.Count > 0)
            {
                bufferReadLocker.WaitOne();
                if (!LogNextMessage(writer))
                {
                    //writer.Close();
                    writer.Dispose();
                    return false;
                }
            }
            //writer.Close();
            writer.Dispose();
            this.Stop();
            return true;
        }

        private bool LogNextMessage(StreamWriter writer)
        {
            if (this.buffer.Count == 0) { return true; }
            bufferReadLocker.WaitOne();

            string strMessage = this.buffer.Dequeue();
            if (!string.IsNullOrEmpty(strMessage))
            {
                if (!WriteLog(writer, strMessage))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 将单条指定的信息写入日志。
        /// </summary>
        /// <param name="message"></param>
        private bool WriteLog(StreamWriter writer, string message)
        {
            try
            {
                writer.Write(message);
                writer.WriteLine();
            }
            catch (Exception)
            {
                //RaiseOnLoggerStoppedEvent(exp);
                return false;
            }
            return true;
        }

        public override bool Log(string message)
        {
            if (!this.IsRunning)
            {
                this.Start();
            }
            this.bufferWriteLocker.WaitOne();
            this.bufferReadLocker.Reset();
            this.buffer.Enqueue(message);
            this.bufferReadLocker.Set();
            this.bufferWriteLocker.ReleaseMutex();
            return true;
        }

        public override bool LogWithTime(string message)
        {
            return Log(string.Concat(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"), " ", message));
        }

        protected override bool EngineTask()
        {
            return LogExistingMessage();
        }

        public override void StopLogService()
        {
            this.LogExistingMessage();
        }
    }
}
