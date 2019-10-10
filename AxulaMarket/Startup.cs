using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(sellmarket.Startup))]
namespace sellmarket
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
