using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM.Models
{
    public class ProductGrade : MLMBaseEntity
    {
        public string Title { get; set; }
        public decimal BusinessShare { get; set; }
        public Decimal CommissionShare { get; set; }
    }
}
