using System.Diagnostics;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using MLM.Logging;
using MLM.Configuration;

namespace MLM.API.Filters
{
    /// <summary>
    /// MVC Action Filter for capturing execution time of Controller Actions. Failsafe allows controller execution to continue even if instrumentation logging fails.
    /// </summary>
    public class InstrumentationFilterAttribute : ActionFilterAttribute
    {
        private readonly ILog _logger;

        public InstrumentationFilterAttribute()
        {
            _logger = new Logger(typeof(InstrumentationFilterAttribute));
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);

            try
            {
                if (_logger.IsDebugEnabled)
                {
                    _logger.LogDebug(string.Format("Instrumented Method Invocation for Controller: {0} | Action: {1}",
                        actionContext.ControllerContext.RouteData.Values["controller"],
                        actionContext.ControllerContext.RouteData.Values["action"]
                        ));
                }

                actionContext.Request.Properties[Constants.ContextStopwatch] = Stopwatch.StartNew();
            }
            catch (System.Exception exception)
            {
                _logger.LogInfo("Instrumentation Logging Failed for Request.", exception);
            }
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionContext)
        {
            base.OnActionExecuted(actionContext);

            try
            {
                var stopwatch = (Stopwatch)actionContext.Request.Properties[Constants.ContextStopwatch];

                if (_logger.IsDebugEnabled)
                {
                    _logger.LogDebug(
                        string.Format(
                            "Instrumented Method Invocation for Controller: {0} | Action: {1} | Duration: {2}",
                            actionContext.ActionContext.ControllerContext.RouteData.Values["controller"],
                            actionContext.ActionContext.ControllerContext.RouteData.Values["action"],
                            stopwatch.ElapsedMilliseconds
                            ));
                }
            }
            catch (System.Exception exception)
            {
                _logger.LogInfo("Instrumentation Logging Failed for Request.", exception);
            }
        }
    }
}
