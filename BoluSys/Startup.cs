using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BoluSys.Startup))]
namespace BoluSys
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
