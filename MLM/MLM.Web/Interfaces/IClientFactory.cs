using System.Net.Http;
using System.Web;

namespace MLM.Util
{
    public interface IClientFactory
    {
        HttpClient Create(HttpContextBase context);
        HttpResponseMessage Login(string username, string password);
        HttpResponseMessage Refresh(string refreshToken);
    }
}