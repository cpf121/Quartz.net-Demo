using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(QuartzWeb.Startup))]
namespace QuartzWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
