using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM.Models
{
   public class TempAgTree : MLMBaseEntity
    {
        public string AgentCode { get; set; }
        public string SponsorCode{ get; set; }
        public string  IntroducerCode{ get; set; }
        public string Position{ get; set; }
        public int Level{ get; set; }
    }
}
