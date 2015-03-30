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
    public class ProductGradeController : BaseApiController
    {
        private MLMDbContext db = null;

        // GET api/ProductGrade
        public IEnumerable<ProductGrade> GetProductGrades()
        {
            return db.ProductGrades.AsEnumerable();
        }

        // GET api/ProductGrade/5
        public ProductGrade GetProductGrade(int id)
        {
            ProductGrade productgrade = db.ProductGrades.Find(id);
            if (productgrade == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return productgrade;
        }

        // PUT api/ProductGrade/5
        public HttpResponseMessage PutProductGrade(int id, ProductGrade productgrade)
        {
            if (ModelState.IsValid && id == productgrade.Id)
            {
                db.Entry(productgrade).State = EntityState.Modified;

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

        // POST api/ProductGrade
        public HttpResponseMessage PostProductGrade(ProductGrade productgrade)
        {
            if (ModelState.IsValid)
            {
                db.ProductGrades.Add(productgrade);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, productgrade);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = productgrade.Id }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/ProductGrade/5
        public HttpResponseMessage DeleteProductGrade(int id)
        {
            ProductGrade productgrade = db.ProductGrades.Find(id);
            if (productgrade == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.ProductGrades.Remove(productgrade);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, productgrade);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}