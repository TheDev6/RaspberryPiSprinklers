namespace Jobz.WebUi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using Models;
    using RootContracts.BehaviorContracts.Utilities;
    using RootContracts.Security;

    [RequireHttps]
    public class AccountController : Controller
    {
        private readonly IAuthenticator auth;
        private readonly ILogger logger;

        public AccountController(IAuthenticator auth, ILogger logger)
        {
            this.auth = auth;
            this.logger = logger;
        }

        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            ActionResult result = View(model);

            if (ModelState.IsValid)
            {
                var signInResult = this.auth.SignIn(username: model.UserName, password: model.Password);
                if (signInResult.IsValidSignIn)
                {
                    //actual signin code
                    Request.GetOwinContext().Authentication.SignIn(signInResult.ClaimsIdentity);

                    //track the sign in event
                    this.logger.TrackEvent(
                        name: CustomEventNames.LoginSuccess,
                        properties: new Dictionary<string, string>
                        {
                            {"AppUser", model.UserName},
                            {"IpAddress", Request.UserHostAddress}
                        });

                    if (!String.IsNullOrWhiteSpace(returnUrl))
                    {
                        result = Redirect(returnUrl);
                    }
                    else
                    {
                        result = RedirectToAction(actionName: "Schedule", controllerName: "Home");//TODO
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Login.");
                    this.logger.TrackEvent(
                        name: CustomEventNames.LoginFail,
                        properties: new Dictionary<string, string>
                        {
                            {"AppUser", model.UserName},
                            {"IpAddress", Request.UserHostAddress}
                        });
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid Login.");
            }
            return result;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> LogOff()
        {
            Request.GetOwinContext().Authentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}