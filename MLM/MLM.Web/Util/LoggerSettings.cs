using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM.Util
{
    public class LoggerSettings
    {
        public bool IsTraceEnabled { get; set; }
        public bool IsDebugEnabled { get; set; }
        public bool IsInfoEnabled { get; set; }
        public bool IsWarnEnabled { get; set; }
        public bool IsErrorEnabled { get; set; }
        public bool IsFatalEnabled { get; set; }
        public int RefreshPeriod { get; set; }
    }
}
