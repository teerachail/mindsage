using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MindsageWeb.Startup))]
namespace MindsageWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
