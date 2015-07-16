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

namespace MLM.Web.Controllers
{
    public class NewsModelsController : ApiController
    {
        private MLMDbContext db ;

        public NewsModelsController ()
        {
            db = new MLMDbContext("MLMCon");
        }

        // GET: api/NewsModels
        public IQueryable<NewsModel> GetNewsModels()
        {
            return db.NewsModels;
        }

        // GET: api/NewsModels/5
        [ResponseType(typeof(NewsModel))]
        public IHttpActionResult GetNewsModel(long id)
        {
            NewsModel newsModel = db.NewsModels.Find(id);
            if (newsModel == null)
            {
                return NotFound();
            }

            return Ok(newsModel);
        }

        // PUT: api/NewsModels/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutNewsModel(long id, NewsModel newsModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != newsModel.Id)
            {
                return BadRequest();
            }

            db.Entry(newsModel).State = EntityState.Modified;

            try
            {
                newsModel.CreatedDate = DateTime.UtcNow;
                newsModel.UpdateDate = DateTime.UtcNow;
                newsModel.IsDeleted = false;
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewsModelExists(id))
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

        // POST: api/NewsModels
        [ResponseType(typeof(NewsModel))]
        public IHttpActionResult PostNewsModel(NewsModel newsModel)
        {
          //  NewsModel newsModel = new NewsModel();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //newsModel.Id = (db.NewsModels.Max(m=>m.Id)+1);
            newsModel.CreatedDate = DateTime.UtcNow;
            newsModel.UpdateDate = DateTime.UtcNow;
            newsModel.IsDeleted = false;
            db.NewsModels.Add(newsModel);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = newsModel.Id }, newsModel);
        }



        // DELETE: api/NewsModels/5
        [ResponseType(typeof(NewsModel))]
        public IHttpActionResult DeleteNewsModel(long id)
        {
            NewsModel newsModel = db.NewsModels.Find(id);
            if (newsModel == null)
            {
                return NotFound();
            }

            db.NewsModels.Remove(newsModel);
            db.SaveChanges();

            return Ok(newsModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NewsModelExists(long id)
        {
            return db.NewsModels.Count(e => e.Id == id) > 0;
        }
    }
}