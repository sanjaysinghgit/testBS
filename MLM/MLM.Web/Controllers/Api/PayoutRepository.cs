using MLM.DB;
using MLM.DB.Specifications;
using MLM.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MLM
{
    public class PayoutRepository<T> : BaseRepository<T> where T : Payout
    {
        public virtual IEnumerable<T> GetPayouts(String agentCode, DateTime startDate, DateTime endDate)
        {
           
            return EntityRepository
                .GetMany(new PayoutsList<T>(agentCode, startDate, endDate));
        }
    }

    public class PayoutsList<T> : SpecificationBase<T> where T : Payout
    {
        public PayoutsList(String agentCode, DateTime startDate, DateTime endDate)
        {
            if (!string.IsNullOrEmpty(agentCode))
            {
                this.predicate = tc => tc.AgentCode == agentCode && tc.CreatedDate >= startDate && tc.CreatedDate <= endDate && tc.IsDeleted == false;
            }
            else
            {
                this.predicate = tc => tc.CreatedDate >= startDate && tc.CreatedDate <= endDate && tc.IsDeleted == false;
            }
            
        }
    }
    
}