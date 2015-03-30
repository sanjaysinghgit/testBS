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
    public class ProductKitController : BaseApiController
    {
        private MLMDbContext db = null;

        // GET api/ProductKit
        public IEnumerable<ProductKit> GetProductKits()
        {
            return db.ProductKits.AsEnumerable();
        }

        // GET api/ProductKit/5
        public ProductKit GetProductKit(int id)
        {
            ProductKit productkit = db.ProductKits.Find(id);
            if (productkit == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return productkit;
        }

        // PUT api/ProductKit/5
        public HttpResponseMessage PutProductKit(int id, ProductKit productkit)
        {
            if (ModelState.IsValid && id == productkit.Id)
            {
                db.Entry(productkit).State = EntityState.Modified;

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

        // POST api/ProductKit
        public HttpResponseMessage PostProductKit(ProductKit productkit)
        {
            if (ModelState.IsValid)
            {
                db.ProductKits.Add(productkit);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, productkit);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = productkit.Id }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/ProductKit/5
        public HttpResponseMessage DeleteProductKit(int id)
        {
            ProductKit productkit = db.ProductKits.Find(id);
            if (productkit == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.ProductKits.Remove(productkit);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, productkit);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}