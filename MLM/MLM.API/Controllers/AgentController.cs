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
using MLM.API.Models;
using MLM.API.DB;
using System.Data.SqlClient;

namespace MLM.API.Controllers
{
    public class AgentController : BaseApiController
    {

        private readonly AgentHandler<Agent> _AgentHandler;
        private readonly SPHandler<MLMBaseEntity> _SPHandler;

        public AgentController(AgentHandler<Agent> AgentHandler, SPHandler<MLMBaseEntity> spHandler)
        {
            _AgentHandler = AgentHandler;
            _SPHandler = spHandler;
        }

        public int GetAgentsT(string a)
        {
            return _SPHandler.getAgentsTree(a);

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