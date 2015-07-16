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
using System.Web;

namespace MLM.Web.Controllers
{
    public class TopAchivarsController : ApiController
    {
        private MLMDbContext db ;
        public TopAchivarsController()
        {
            db = new MLMDbContext("MLMCon");
        }



        // GET: api/TopAchivars
        public IQueryable<TopAchivar> GetTopAchivars()
        {
            return db.TopAchivars;
        }

        // GET: api/TopAchivars/5
        [ResponseType(typeof(TopAchivar))]
        public IHttpActionResult GetTopAchivar(long id)
        {
            TopAchivar topAchivar = db.TopAchivars.Find(id);
            if (topAchivar == null)
            {
                return NotFound();
            }

            return Ok(topAchivar);
        }

        // PUT: api/TopAchivars/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTopAchivar(long id, TopAchivar topAchivar)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != topAchivar.Id)
            {
                return BadRequest();
            }

            db.Entry(topAchivar).State = EntityState.Modified;

            try
            {
                topAchivar.CreatedDate = DateTime.UtcNow;
                topAchivar.UpdateDate = DateTime.UtcNow;
                topAchivar.IsDeleted = false;
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TopAchivarExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/TopAchivars
        [HttpPost]
        [ResponseType(typeof(TopAchivar))]
        public HttpResponseMessage PostTopAchivar(TopAchivar topAchivar)
        {
           // TopAchivar topAchivar = new TopAchivar();


            if (ModelState.IsValid)
            {
                topAchivar.CreatedDate = DateTime.UtcNow;
                topAchivar.UpdateDate = DateTime.UtcNow;
                topAchivar.IsDeleted = false;

                db.TopAchivars.Add(topAchivar);
                db.SaveChanges();
               // IEnumerable<TopAchivar> TopAc = db.TopAchivars;

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, topAchivar);
                //HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, TopAc);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = topAchivar.Id }));                
                response.Content = new StringContent(topAchivar.Id.ToString());
                
                return response;
                //return Request.CreateResponse(HttpStatusCode.Created, topAchivar);
            }

          
            else
            {
                //return Request.CreateResponse(HttpStatusCode.BadRequest);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }





        }

        // DELETE: api/TopAchivars/5
        [ResponseType(typeof(TopAchivar))]
        public IHttpActionResult DeleteTopAchivar(long id)
        {
            TopAchivar topAchivar = db.TopAchivars.Find(id);
            if (topAchivar == null)
            {
                return NotFound();
            }

            db.TopAchivars.Remove(topAchivar);
            db.SaveChanges();

            return Ok(topAchivar);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TopAchivarExists(long id)
        {
            return db.TopAchivars.Count(e => e.Id == id) > 0;
        }
    }
}