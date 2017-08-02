using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Revive.Redux.UI.Startup))]
namespace Revive.Redux.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
