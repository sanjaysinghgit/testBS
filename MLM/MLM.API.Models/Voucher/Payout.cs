using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM.Models
{
    public class Payout : MLMBaseEntity
    {
        
        public DateTime VoucherDate { get; set; }

        public string Agentcode { get; set; }
            
        public string PAN { get; set; }

        public int TotalLeftPair { get; set; }

        public int TotalRightPair { get; set; }

        public int  TotalLeftPairPV { get; set; }
       // public System.Nullable<int> TotalLeftPairPV { get; set; }

        public int  TotalRightPairPV { get; set; }
        //public System.Nullable<int> TotalRightPairPV { get; set; }

        public int PairsInThisPayout { get; set; }
        public int PairsPVInThisPayout { get; set; }
        //public System.Nullable<int> PairsPVInThisPayout { get; set; }
        
        public decimal SaveIncome { get; set; } 

        public Decimal PairIncome { get; set; }

        public Decimal NetIncome { get; set; }

        public Decimal TDS { get; set; }

        public Decimal  ProcessingCharges{ get; set; }

        public decimal DispatchedAmount { get; set; }

        

       // public bool IsActive { get; set; }
    }
}
