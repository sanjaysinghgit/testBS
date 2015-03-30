using MLM.API.DB;
using MLM.API.DB.Specifications;
using MLM.API.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MLM.API
{
    public class AgentHandler<T> : BaseHandler<T> where T : Agent
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

    }
        public class NotDeletedEntity<T> : SpecificationBase<T> where T : MLMBaseEntity
    {
        public NotDeletedEntity()
        {
            this.predicate = e => e.IsDeleted == false;
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