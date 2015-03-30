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

namespace MLM.API.Controllers
{
    public class EPinController : BaseApiController
    {
        private MLMDbContext db = null;

        // GET api/EPin
        public IEnumerable<EPin> GetEPins()
        {
            return db.EPins.AsEnumerable();
        }

        // GET api/EPin/5
        public EPin GetEPin(int id)
        {
            EPin epin = db.EPins.Find(id);
            if (epin == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return epin;
        }

        // PUT api/EPin/5
        public HttpResponseMessage PutEPin(int id, EPin epin)
        {
            if (ModelState.IsValid && id == epin.Id)
            {
                db.Entry(epin).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // POST api/EPin
        public HttpResponseMessage PostEPin(EPin epin)
        {
            if (ModelState.IsValid)
            {
                db.EPins.Add(epin);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, epin);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = epin.Id }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/EPin/5
        public HttpResponseMessage DeleteEPin(int id)
        {
            EPin epin = db.EPins.Find(id);
            if (epin == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.EPins.Remove(epin);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, epin);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}