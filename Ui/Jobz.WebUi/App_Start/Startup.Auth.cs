namespace Jobz.WebUi
{
    using Microsoft.Owin;
    using Microsoft.Owin.Security.Cookies;
    using Owin;
    using Utilities;

    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationType = new Config().ApplicationCookieName(),
                CookieHttpOnly = true,//there is a setting in web.config as well, didn't seem to work?
                CookieSecure = CookieSecureOption.Always,
                LoginPath = new PathString("/Account/login")
            });

            //https://stackoverflow.com/questions/30976980/mvc-5-owin-login-with-claims-and-antiforgerytoken-do-i-miss-a-claimsidentity-pr
            //tell the dumb system how to do every last thing like which cliam to use for antiforgery.
            System.Web.Helpers.AntiForgeryConfig.UniqueClaimTypeIdentifier = System.Security.Claims.ClaimTypes.NameIdentifier;
        }
    }
}