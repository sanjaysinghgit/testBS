using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using MLM.DB;
using MLM.Models;



namespace MLM.Controllers
{
    public class ProductCategoryController : BaseApiController
    {
        private MLMDbContext db = null;



        //Get Products Categorys
        public IEnumerable<ProductCategory>GetProductCategorys()
        {
            return db.ProductCategorys.AsEnumerable();

        }



        //Get api/GetProductCategary/5
        public ProductCategory GetProductCategory(int id)
        {
            ProductCategory productCategary = db.ProductCategorys.Find(id);
            if (productCategary==null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));

            }
            return productCategary;

        }

        //Post api/ProductCategory
        public HttpResponseMessage PostProductsCategory(ProductCategory productcategory)
        {
            if (ModelState.IsValid)
            {
                productcategory.IsDeleted = false;
                productcategory.CreatedDate = DateTime.Now;
                productcategory.UpdateDate = DateTime.Now;
                db.ProductCategorys.Add(productcategory);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, productcategory);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = productcategory.Id }));
                return response;

            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // Put api/Product Category/5
        public HttpResponseMessage PutProductCategory(int id, ProductCategory productcategory)
        {
            if (ModelState.IsValid && productcategory.Id == id)
            {
                db.Entry(productcategory).State = EntityState.Modified;
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

        //Delete api/Product Category/5
        public HttpResponseMessage DeleteProductCategory(int id)
        {
            ProductCategory productcategory = db.ProductCategorys.Find(id);
            if (productcategory == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.ProductCategorys.Remove(productcategory);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, productcategory);

        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}
