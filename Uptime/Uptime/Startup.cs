using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Uptime.Startup))]
namespace Uptime
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
