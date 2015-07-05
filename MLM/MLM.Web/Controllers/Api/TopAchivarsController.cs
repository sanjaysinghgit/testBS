using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MLM.DB;
using MLM.Models;
using MLM.Controllers;

namespace MLM.Web.Controllers
{
    public class TopAchivarsController : BaseApiController
    {
        private readonly TopachiversRepository <TopAchivar> _TopachiversHandler;
        private readonly SPRepository<TopAchivar> _SPRepository;

        public TopAchivarsController()
        {
            _TopachiversHandler = new TopachiversRepository<TopAchivar>();
            _SPRepository = new SPRepository<TopAchivar>();
        }

      
        [HttpGet]
        public IEnumerable<TopAchivar> Topachivers()
        {
            try
            {
                return _TopachiversHandler.GetTopachivers().ToList();
            }
            catch (System.Exception ex)
            {
                Logger.LogDebug("Exception Inside TopAchivarController | Agents", ex);
                return Enumerable.Empty<TopAchivar>();
            }
        }


      
       
    }
}