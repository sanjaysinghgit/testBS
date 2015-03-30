using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM.Models
{
    public class EPin : MLMBaseEntity
    {
        public int SerialNumber { get; set; }
        public Guid Pin { get; set; }

    }
}
