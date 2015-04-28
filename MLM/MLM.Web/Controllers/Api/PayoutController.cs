using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using MLM.Models;
using MLM.DB;
using System.Data.SqlClient;

namespace MLM.Controllers
{
    public class PayoutController : BaseApiController
    {

        private readonly PayoutRepository<Payout> _PayoutHandler;
        private readonly AgentRepository<Agent> _AgentHandler;
        private readonly SPRepository<Payout> _SPRepository;

        public PayoutController()
        {
            _AgentHandler = new AgentRepository<Agent>();
            _PayoutHandler = new PayoutRepository<Payout>();
            _SPRepository = new SPRepository<Payout>();
        }

        [HttpGet]
        public IEnumerable<Payout> Payouts(String agentCode, DateTime startDate, DateTime endDate)
        {
            try
            {
                return _PayoutHandler.GetPayouts(agentCode,startDate, endDate).ToList();
            }
            catch (System.Exception ex)
            {
                Logger.LogDebug("Exception Inside PayoutController | Payouts", ex);
                return Enumerable.Empty<Payout>();
            }
        }

    }
}