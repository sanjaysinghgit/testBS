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
using System.Collections;

namespace MLM.Web.Controllers
{
    public class PayoutsController : ApiController
    {
        private MLMDbContext db;

        public PayoutsController()
        {
            db = new MLMDbContext("MLMCon");
        }


        // GET: api/Payouts
        [HttpGet]
        public IEnumerable Payouts()
        {

            IEnumerable q;

            return q = (from _pay in db.Payouts
                        join user in db.Users

                        on _pay.AgentCode
                          equals
                          user.UserName
                        //on ag.Id equals user.AgentInfo
                        orderby _pay.VoucherDate
                        select new
                        {
                            _pay.Id,
                            _pay.VoucherDate,
                            _pay.AgentCode,
                            user.Name,
                            user.FatherName,
                            _pay.TotalLeftPair,
                            _pay.TotalRightPair,
                            _pay.PairsInThisPayout,
                            _pay.TDS,
                            _pay.SaveIncome,
                            _pay.NetIncome,
                            _pay.DispatchedAmount

                        }).ToList();
        }

        // GET: api/Payouts/5
        [ResponseType(typeof(Payout))]
        public IHttpActionResult GetPayout(string id)
        {
            List<Payout> _payout = new List<Payout>();

            _payout = (from premalinktags in db.Payouts
                             where premalinktags.AgentCode == id
                             select premalinktags).ToList();


            if (_payout == null)
            {
                return NotFound();
            }

            return Ok(_payout);
        }

        // PUT: api/Payouts/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPayout(long id, Payout payout)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != payout.Id)
            {
                return BadRequest();
            }

            db.Entry(payout).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PayoutExists(id))
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

        // POST: api/Payouts
        [ResponseType(typeof(Payout))]
        public IHttpActionResult PostPayout(Payout payout)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Payouts.Add(payout);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = payout.Id }, payout);
        }

        // DELETE: api/Payouts/5
        [ResponseType(typeof(Payout))]
        public IHttpActionResult DeletePayout(long id)
        {
            Payout payout = db.Payouts.Find(id);
            if (payout == null)
            {
                return NotFound();
            }

            db.Payouts.Remove(payout);
            db.SaveChanges();

            return Ok(payout);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PayoutExists(long id)
        {
            return db.Payouts.Count(e => e.Id == id) > 0;
        }
    }
}