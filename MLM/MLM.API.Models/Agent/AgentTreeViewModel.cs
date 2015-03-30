using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM.Models
{
    public class AgentTreeViewModel
    {
        /// <summary>
        /// Around 10 digit alphanumeric code
        /// </summary>
        /// 
        public int SrNo { get; set; }
        public string AgentCode { get; set; }
        public string SponsorCode { get; set; }
        public string IntroducerCode { get; set; }
        public AgentPosition Position { get; set; }
        public AgentStatus Status { get; set; }
    }

}
