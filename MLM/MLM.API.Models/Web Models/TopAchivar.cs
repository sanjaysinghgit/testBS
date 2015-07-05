using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM.Models
{
    public class TopAchivar : MLMBaseEntity
    {
        public string name{ get; set; }
        public string AgencyCode{ get; set; }
        public string location{ get; set; }
        public string Achivarprizename { get; set; }

    }
}
