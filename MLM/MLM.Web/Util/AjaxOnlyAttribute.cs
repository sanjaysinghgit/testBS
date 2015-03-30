using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MLM.Util
{
    public class AjaxOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
                filterContext.HttpContext.Response.Redirect("/error/404");
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }
    }
}