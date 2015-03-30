using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Http.Filters;
using System.Net.Http;
using MLM.Exception;

namespace MLM.Filters
{
    public class AntiForgeryValidatorFilterAttribute : ActionFilterAttribute
    {
        private IExceptionHandler ExceptionHandler { get; set; }
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            try
            {
                string cookieToken = string.Empty;
                string formToken = string.Empty;

                IEnumerable<string> tokenHeaders = actionContext.Request.Headers.GetValues("RequestVerificationToken");
                if (tokenHeaders != null && tokenHeaders.Count() > 0)
                {
                    string[] tokens = tokenHeaders.First().Split(':');
                    if (tokens.Length == 2)
                    {
                        cookieToken = tokens[0].Trim();
                        formToken = tokens[1].Trim();
                    }
                }
                AntiForgery.Validate(cookieToken, formToken);

                base.OnActionExecuting(actionContext);
            }
            catch (System.Web.Mvc.HttpAntiForgeryException afe)
            {
                ExceptionHandler = new BaseExceptionHandler(this.GetType());
                ExceptionHandler.HandleException(new MLMException("Exception Occured in the " + actionContext.ControllerContext.Controller + " Controller's Action: "
                    + actionContext.ControllerContext.RouteData.Values["action"], afe));
                //TODO: Check this
                //DotNetCasClient.CasAuthentication.RedirectToNotAuthorizedPage();
            }
            catch (System.Exception e)
            {
                ExceptionHandler = new BaseExceptionHandler(this.GetType());
                ExceptionHandler.HandleException(new MLMException("Exception Occured in the " + actionContext.ControllerContext.Controller + " Controller's Action: "
                    + actionContext.ControllerContext.RouteData.Values["action"], e));
                //TODO: Check this
                //DotNetCasClient.CasAuthentication.RedirectToNotAuthorizedPage();
            }
        }
    }
}
