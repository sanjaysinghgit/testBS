using System.Web.Http.Filters;
using MLM.Exception;

namespace MLM.API.Filters
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private IExceptionHandler ExceptionHandler { get; set; }

        public ApiExceptionFilterAttribute()
        {
            ExceptionHandler = new BaseExceptionHandler(typeof(ApiExceptionFilterAttribute));
        }

        public override void OnException(HttpActionExecutedContext filterContext)
        {
            ExceptionHandler.HandleException(
                new MLMException(
                    string.Format("Exception occured in Controller: {0} | Action: {1}", filterContext.ActionContext.ControllerContext.RouteData.Values["controller"],
                        filterContext.ActionContext.ControllerContext.RouteData.Values["action"]), filterContext.Exception));
        }
    }
}
