using MLM.Filters;
using MLM.Exception;
using MLM.Logging;
using System;
using System.Web;
using System.Web.Http;

namespace MLM.Controllers
{
    //[PrincipalPermission(SecurityAction.Demand, Role = "*")]
    [ApiExceptionFilter]
    [InstrumentationFilter]
    [ModuleContextFilter]
    public class BaseApiController : ApiController
    {
        public ILog Logger { protected get; set; }
        public IExceptionHandler ExceptionHandler { protected get; set; }

        public BaseApiController()
        {
            Logger = new Logger(this.GetType());
            ExceptionHandler = new BaseExceptionHandler(this.GetType());
        }

       

        public Uri RequestUrl
        {
            get
            {
                return HttpContext.Current.Request.Url;
            }
        }

        //public IIdentityExtension IdentityExtension
        //{
        //    get
        //    {
        //        //If we have personated user available then we need to run this call under personated user context.
        //        if (HttpContext.Current != null && HttpContext.Current.Items[AuthenticationConstants.PersonatedContextItem] != null && (IsInRole(AuthenticationConstants.ImpersonatorRole) || IsInRole(AuthenticationConstants.ImpersonatorFacultyRole)))
        //        {
        //            var personatedUser = HttpContext.Current.Items[AuthenticationConstants.PersonatedContextItem] as IIdentityExtension;
        //            return personatedUser;
        //        }
        //        var customPrincipal = this.User as CustomPrincipal;
        //        if (customPrincipal == null)
        //            return null;
        //        return customPrincipal.IdentityExtension;
        //    }
        //}



        //private bool IsInRole(string role)
        //{
        //    var customPrincipal = this.User as CustomPrincipal;
        //    if (customPrincipal == null)
        //        return false;
        //    if (customPrincipal.IdentityExtension == null)
        //        return false;
        //    if (customPrincipal.IdentityExtension.Associations != null && customPrincipal.IdentityExtension.Associations.ContainsRole(role))
        //        return true;
           
        //    return false;
        //}

        /// <summary>
        /// Generate UniqueId
        /// </summary>
        /// <returns></returns>
        public string GenerateUniqueId()
        {
            long ticks = DateTime.Now.Ticks;
            byte[] bytes = BitConverter.GetBytes(ticks);
            string id = Convert.ToBase64String(bytes)
                                    .Replace('+', '_')
                                    .Replace('/', '-')
                                    .TrimEnd('=');

            return id;
        }       
    }    
}