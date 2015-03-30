using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace MLM.Logging
{
    public class Logger : ILog
    {
        public log4net.ILog Log;
        public Logger(Type T)
        {
            if (!log4net.LogManager.GetRepository().Configured)
                log4net.Config.XmlConfigurator.Configure();

            var name = String.IsNullOrWhiteSpace(T.Name) ? String.Empty : T.Name.Split('`').Length > 0 ? T.Name.Split('`')[0] : String.Empty;
            Log = LogManager.GetLogger(T.Namespace + '.' + name);
        }
        
        public bool IsDebugEnabled
        {
            get { return Log.IsDebugEnabled; }
        }

        public bool IsInfoEnabled
        {
            get { return Log.IsInfoEnabled; }
        }

        public void ClearNDC()
        {
            log4net.NDC.Clear();
        }

        public void LogDebug(object Message)
        {
            if (Log.IsDebugEnabled)
            {
                Log.Debug(Message);
            }
        }

        public void LogDebug(string Message)
        {
            if (Log.IsDebugEnabled)
            {
                Log.Debug(Message);
            }
        }

        public void LogDebug(object Message, System.Exception ex)
        {
            if (Log.IsDebugEnabled)
            {
                Log.Debug(Message,ex);
            }
        }

        public void LogError(object Message)
        {
            Log.Error(Message);
        }

        public void LogError(object Message, System.Exception ex)
        {
            Log.Error(Message,ex);
        }

        public void LogFatal(object Message)
        {
            Log.Fatal(Message);
        }

        public void LogFatal(object Message, System.Exception ex)
        {
            Log.Fatal(Message,ex);
        }

        public void LogInfo(object Message)
        {
            Log.Info(Message);
        }

        public void LogInfo(string Message)
        {
            Log.Info(Message);
        }

        public void LogInfo(object Message, System.Exception ex)
        {
            Log.Info(Message, ex);
        }

        public void LogInfoWithoutSession(object Message)
        {
            Log.Info(Message);
        }

        public void LogTrack(string PageName, string EventName)
        {
            throw new NotImplementedException();
        }

        public void LogTrack(string Message, string PageName, string EventName)
        {
            throw new NotImplementedException();
        }

        public void LogWarn(object Message)
        {
            Log.Warn(Message);
        }

        public void LogWarn(object Message, System.Exception ex)
        {
            Log.Info(Message,ex);
        }
    }
}
