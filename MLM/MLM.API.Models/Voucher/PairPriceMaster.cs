using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM.Models
{
   public class PairPriceMaster : MLMBaseEntity
    {
        public int pair { get; set; }

        public int TimeinDays { get; set; }

        public string Price { get; set; }
    }
}
