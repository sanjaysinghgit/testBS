using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using EPS.IoC.ServiceLocation;
using MLM.Logging;
using MLM.Configuration;

namespace MLM.Filters
{
    public class ModuleContextFilterAttribute : ActionFilterAttribute
    {
        private readonly ILog _logger;

        public ModuleContextFilterAttribute()
        {
            _logger = new Logger(typeof(ModuleContextFilterAttribute));
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);

            //var currentModuleName = actionContext.ControllerContext.RouteData.Values[Constants.ModulePattern];

            //if (currentModuleName == null)
            //{
            //    if (_logger.IsDebugEnabled)
            //        _logger.LogDebug(string.Format("Could not set module context for Controller: {0}", actionContext.Request.RequestUri));
            //    return;
            //}

            //if(_logger.IsDebugEnabled)
            //    _logger.LogDebug(string.Format("Setting Module Context for Controller: {0}", currentModuleName));

            // TODO: Performance Issue?
            //var contextProvider = CommonBootStrapper.Locator.GetInstance<IApplicationContextProvider>();
            //contextProvider.SetContextObject(Constants.ContextModuleKey, currentModuleName);
        }
    }
}