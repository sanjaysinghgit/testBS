using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM.Models
{
    public class Coupon : MLMBaseEntity
    {
        public int SerialNumber { get; set; }
        
        public Guid RandomNumber { get; set; }

        public decimal Amount { get; set; }
        
        public DateTime Expiry { get; set; }

        public CouponType Type { get; set; }

    }
}
