using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM.Models
{
    public class AgentDetailsViewModel
    {
        /// <summary>
        /// Around 10 digit alphanumeric code
        /// </summary>
        /// 
        public string AgentCode { get; set; }
        public int TotalLeft { get; set; }
        public int TotalRight { get; set; }

    }

}
