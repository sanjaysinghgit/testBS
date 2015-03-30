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

namespace MLM.Controllers
{
    public class CouponController : BaseApiController
    {
        private MLMDbContext db = null;

        // GET api/Coupon
        public IEnumerable<Coupon> GetCoupons()
        {
            return db.Coupons.AsEnumerable();
        }

        // GET api/Coupon/5
        public Coupon GetCoupon(int id)
        {
            Coupon coupon = db.Coupons.Find(id);
            if (coupon == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return coupon;
        }

        // PUT api/Coupon/5
        public HttpResponseMessage PutCoupon(int id, Coupon coupon)
        {
            if (ModelState.IsValid && id == coupon.Id)
            {
                db.Entry(coupon).State = EntityState.Modified;

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

        // POST api/Coupon
        public HttpResponseMessage PostCoupon(Coupon coupon)
        {
            if (ModelState.IsValid)
            {
                db.Coupons.Add(coupon);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, coupon);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = coupon.Id }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Coupon/5
        public HttpResponseMessage DeleteCoupon(int id)
        {
            Coupon coupon = db.Coupons.Find(id);
            if (coupon == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Coupons.Remove(coupon);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, coupon);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}