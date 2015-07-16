using System;
using System.Collections.Generic;
using System.Collections;
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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using MLM.Web.Models;

namespace MLM.Controllers
{
    public class AgentController : BaseApiController
    {

         private MLMDbContext db ;
         public UserManager<ApplicationUser> UserManager { get; private set; }




        private readonly AgentRepository<Agent> _AgentHandler;
        private readonly SPRepository<Agent> _SPRepository;

        public AgentController()
        {
            db = new MLMDbContext("MLMCon");
            _AgentHandler = new AgentRepository<Agent>();
            _SPRepository = new SPRepository<Agent>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="providedSponsorCode"></param>
        /// <param name="agentPosition"></param>
        /// <returns>AgentApplicableSponsorCode</returns>
        [HttpGet]
        public string GetAgentApplicableSponsorCode(string providedSponsorCode, AgentPosition agentPosition)
        {
            return _SPRepository.GetAgentApplicableSponsorCode( providedSponsorCode, agentPosition);

        }

        [HttpGet]
        public void AgentSponsorRegistrationConditions(string agentcode, string sponsorCode, string introducerCode, AgentPosition position)
        {
            _SPRepository.AgentSponsorRegistrationConditions(agentcode, sponsorCode, introducerCode, position);

        }

        [HttpGet]
        public IEnumerable<Agent> Agents()
        {
            try
            {
                return _AgentHandler.GetAgents().ToList();
            }
            catch (System.Exception ex)
            {
                Logger.LogDebug("Exception Inside AgentController | Agents", ex);
                return Enumerable.Empty<Agent>();
            }
        }

        [HttpGet]
        [Route("api/Agent/Tree/{agentcode}")]
        public IEnumerable<AgentTreeViewModel> GetAgentTree(string agentcode)
        {
            try
            {
                return _SPRepository.AgentTree( agentcode).ToList();
            }
            catch (System.Exception ex)
            {
                Logger.LogDebug("Exception Inside AgentController | GetAgentTree", ex);
                return Enumerable.Empty<AgentTreeViewModel>();
            }
        }

        [HttpGet]
        [Route("api/Agent/Details/{agentcode}")]
        public AgentDetailsViewModel GetAgentDetails(string agentcode)
        {
            try
            {
                return _SPRepository.GetAgentDetails(agentcode);
            }
            catch (System.Exception ex)
            {
                Logger.LogDebug("Exception Inside AgentController | GetAgentDetails", ex);
                return null;
            }
        }

        [HttpGet]
        public string LatestAgentCode()
        {
            try
            {
                return _AgentHandler.GetLatestAgentCode();
            }
            catch (System.Exception ex)
            {
                Logger.LogDebug("Exception Inside AgentController | LatestAgentCode", ex);
                return String.Empty;
            }
        }


        //Get All Agents With Name (Some Data From Application User)




        [HttpGet]
        public IEnumerable AllAgents()
        {

            IEnumerable q;
            

             
             return    q = (from ag in db.Agents
                          join user in db.Users

                          on ag.Code
                            equals
                            user.UserName


                          //on ag.Id equals user.AgentInfo
                          orderby ag.ActivationDate

                          select new
                          {
                              ag.Code,
                              user.Name,
                              user.FatherName,
                              user.Address,
                              user.PhoneNumber,
                              user.UserName,
                              ag.SponsorCode,
                              ag.ActivationDate,
                              ag.IntroducerCode,
                              ag.Position,
                              ag.LeftAgent,
                              ag.RightAgent,
                              ag.SaveIncomeStatus,
                              ag.Status,
                              ag.VoucherStatus

                          }).ToList();
              

           


      }
        




















        // //GET api/Agent/5
        //public Agent GetAgent(int id)
        //{
        //    Agent agent = db.Agents.Find(id);
        //    if (agent == null)
        //    {
        //        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
        //    }

        //    return agent;
        //}

        //// PUT api/Agent/5
        //public HttpResponseMessage PutAgent(int id, Agent agent)
        //{
        //    if (ModelState.IsValid && id == agent.Id)
        //    {
        //        db.Entry(agent).State = EntityState.Modified;

        //        try
        //        {
        //            db.SaveChanges();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.NotFound);
        //        }

        //        return Request.CreateResponse(HttpStatusCode.OK);
        //    }
        //    else
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest);
        //    }
        //}

        //// POST api/Agent
        //public HttpResponseMessage PostAgent(Agent agent)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Agents.Add(agent);
        //        db.SaveChanges();

        //        HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, agent);
        //        response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = agent.Id }));
        //        return response;
        //    }
        //    else
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest);
        //    }
        //}

        //// DELETE api/Agent/5
        //public HttpResponseMessage DeleteAgent(int id)
        //{
        //    Agent agent = db.Agents.Find(id);
        //    if (agent == null)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.NotFound);
        //    }

        //    db.Agents.Remove(agent);

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.NotFound);
        //    }

        //    return Request.CreateResponse(HttpStatusCode.OK, agent);
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    db.Dispose();
        //    base.Dispose(disposing);
        //}


        //public OutputModel GetReport(InputModel inputModel)
        //{
        //    //can create view model based on entity and can use it.
        //    //var entity = new AgentViewModal(inputModel);

        //    var sqlParameters = new List<SqlParameter>
        //        {
        //            new SqlParameter("@ids", inputModel.propone),
        //            new SqlParameter("@searchstring", inputModel.proptwo),
        //        };

        //    var parameter = new SqlParameter("@TotalRows", SqlDbType.BigInt, 4)
        //    {
        //        Direction = ParameterDirection.Output
        //    };
        //    sqlParameters.Add(parameter);

        //    var outputModel = new OutputModel()
        //    {
        //        TempModel = new List<TempModel>()
        //    };

        //    using (var reader = SQLRepository.ExecuteProcedure("GetAgentTree", sqlParameters.ToArray()))
        //    {
        //        while (reader.Read())
        //        {
        //            var tempModel = new TempModel
        //            {
        //                proone = Convert.ToInt64(reader["proone"]),
        //                proptwo = reader["proptwo"].ToString(),
        //                propthree = Convert.ToDateTime(reader["propthree"]).ToString("MM/dd/yyyy"),
        //                propfour = null,
        //                propfive = null,
        //            };

        //            if (reader["propsix"] != DBNull.Value)
        //            {
        //                tempModel.propsix = reader["propsix"].ToString();
        //            }

        //            outputModel.TempModel.Add(tempModel);
        //        }
        //    }

        //    outputModel.TotalItems = Convert.ToInt64(parameter.Value);
        //    return outputModel;
        //}
    }
}