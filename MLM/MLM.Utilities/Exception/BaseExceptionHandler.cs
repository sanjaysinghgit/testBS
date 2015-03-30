using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MLM.Logging;

namespace MLM.Exception
{
    public class BaseExceptionHandler : IExceptionHandler
    {
        public ILog Logger { protected get; set; }

        public BaseExceptionHandler(Type T)
        {
            Logger = new Logger(T);
            return;
        }

        public IDictionary<string, object> HandlerPropertyBag
        {
            get;
            set;
        }

        public bool HandleException(MLMException exToHandle)
        {
            Logger.LogError(String.Empty, exToHandle);
            return true;
        }

        public bool HandleException(MLMException exToHandle, string message)
        {
            Logger.LogError(message, exToHandle);
            return true;
        }
    }
}
