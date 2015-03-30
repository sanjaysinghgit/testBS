using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM.Util
{
    public class EnvironmentInfo
    {
        public string AssemblyVersion { get; set; }
        public string Environment { get; set; }
        public string TeamCityBuildId { get; set; }
        public string OSName { get; set; }
        public string MachineName { get; set; }
        public string Bitness { get; set; }
        public string Extra { get; set; }
    }
}
