using Owin;

[assembly: Microsoft.Owin.OwinStartup(typeof(Sistran.Core.Framework.UIF.Web.Startup))]
namespace Sistran.Core.Framework.UIF.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}