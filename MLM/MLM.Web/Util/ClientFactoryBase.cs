using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLM.Util
{
    public class ClientFactoryBase : IClientFactory
    {
        public virtual System.Net.Http.HttpClient Create(HttpContextBase context)
        {
            throw new NotImplementedException();
        }

        public virtual System.Net.Http.HttpResponseMessage Login(string username, string password)
        {
            throw new NotImplementedException();
        }

        public virtual System.Net.Http.HttpResponseMessage Refresh(string refreshToken)
        {
            throw new NotImplementedException();
        }

        public static string GetHeaderFromClientRequest(HttpContextBase context, string header)
        {
            var value = context.Request.Headers[header];
            if (string.IsNullOrWhiteSpace(value)
                && context.Request.Form[header] != null
                && !string.IsNullOrWhiteSpace(context.Request.Form[header]))
            {
                value = context.Request.Form[header];
            }
            return value;
        }
    }
}