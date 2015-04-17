using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM.Models
{
    public class PairPrizeWinner
    {
        public long Id { get; set; }
        public long voucherid { get; set; }
        public DateTime voucherdate { get; set; }
        public string Agentcode { get; set; }
        public int TotPair { get; set; }
        public int days { get; set; }
        public int Prize{ get; set; }

    }
}
