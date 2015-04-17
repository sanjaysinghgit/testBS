using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM.Models
{
   public class PairPriceMaster : MLMBaseEntity
    {
        public int pairFrom { get; set; }
        public int pairTo { get; set; }
        public int TimeinDaysFrom { get; set; }
        public int TimeinDaysTo { get; set; }
        public string Price { get; set; }
    }
}
