using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MLM.Web.Controllers
{
    public class ProductsController : Controller
    {
        //
        // GET: /Products/

        public ActionResult Index()
        {
            return View();
        }



        public ActionResult Productpage()
        {
            return View("Products");
        }

        public ActionResult SingleProduct()
        {
            return View("SingleProduct");
        }

    }
}
