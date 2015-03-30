using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MLM.Startup))]
namespace MLM
{
    public partial class Startup 
    {
        public void Configuration(IAppBuilder app) 
        {
            ConfigureAuth(app);
        }
    }
}
