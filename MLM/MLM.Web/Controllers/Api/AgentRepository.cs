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
    public class AgentRepository<T> : BaseRepository<T> where T : Agent
    {
        public virtual T GetById(long id, bool includeDeleted = false)
        {
            ISpecification<T> spec = new GetAgentsById<T>(id);
            if (includeDeleted == false)
            {
                spec.And(new NotDeletedEntity<T>());
            }

            return EntityRepository
                .GetMany(spec)
                .FirstOrDefault();
        }

        public virtual IEnumerable<T> GetAgents()
        {
            return EntityRepository
                .GetMany(new AgentsList<T>());
        }

        public virtual String GetLatestAgentCode()
        {
            ISpecification<T> spec = new GetLatestAgentCode<T>();

            string agentCode = EntityRepository
                .GetMany(spec)
                .OrderByDescending(a => a.Code)
                .FirstOrDefault().Code;

            string todaysCode = DateTime.UtcNow.Year.ToString("0000") + DateTime.UtcNow.Month.ToString("00") + DateTime.UtcNow.Day.ToString("00");
            if (todaysCode.Equals(agentCode.Substring(0, 8)))
            {
                Int32 newAgentSerial = Convert.ToInt32(agentCode.Substring(8, 2)) + 1;
                return todaysCode + newAgentSerial.ToString("00");
            }
            else
            {
                return todaysCode + "01";
            }

        }

    }
        public class NotDeletedEntity<T> : SpecificationBase<T> where T : MLMBaseEntity
    {
        public NotDeletedEntity()
        {
            this.predicate = e => e.IsDeleted == false;
        }
    }
        public class GetLatestAgentCode<T> : SpecificationBase<T> where T : Agent
        {
            public GetLatestAgentCode()
            {
                this.predicate = tc => tc.IsDeleted == false;
            }
        }

    public class GetAgentsById<T> : SpecificationBase<T> where T : Agent
    {
        public GetAgentsById(long id)
        {
            this.predicate = ac => ac.Id == id;
        }
    }

    public class AgentsList<T> : SpecificationBase<T> where T : Agent
    {
        public AgentsList()
        {
            this.predicate = tc => tc.IsDeleted == false;
        }
    }
    
}