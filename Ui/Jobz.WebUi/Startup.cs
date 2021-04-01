using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Jobz.WebUi.Startup))]
namespace Jobz.WebUi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
