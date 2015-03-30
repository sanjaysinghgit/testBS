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
    public class CouponTypeController : BaseApiController
    {
        private MLMDbContext db = null;

        // GET api/CouponType
        public IEnumerable<CouponType> GetCouponTypes()
        {
            return db.CouponTypes.AsEnumerable();
        }

        // GET api/CouponType/5
        public CouponType GetCouponType(int id)
        {
            CouponType coupontype = db.CouponTypes.Find(id);
            if (coupontype == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return coupontype;
        }

        // PUT api/CouponType/5
        public HttpResponseMessage PutCouponType(int id, CouponType coupontype)
        {
            if (ModelState.IsValid && id == coupontype.Id)
            {
                db.Entry(coupontype).State = EntityState.Modified;

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

        // POST api/CouponType
        public HttpResponseMessage PostCouponType(CouponType coupontype)
        {
            if (ModelState.IsValid)
            {
                db.CouponTypes.Add(coupontype);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, coupontype);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = coupontype.Id }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/CouponType/5
        public HttpResponseMessage DeleteCouponType(int id)
        {
            CouponType coupontype = db.CouponTypes.Find(id);
            if (coupontype == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.CouponTypes.Remove(coupontype);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, coupontype);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}