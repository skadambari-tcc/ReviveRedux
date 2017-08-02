using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Revive.Redux.Startup))]
namespace Revive.Redux
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
