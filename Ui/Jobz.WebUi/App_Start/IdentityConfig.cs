namespace Jobz.WebUi
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin;
    using Microsoft.Owin.Security;
    using Models;

//    public class EmailService : IIdentityMessageService
//    {
//        public Task SendAsync(IdentityMessage message)
//        {
//            // Plug in your email service here to send an email.
//            return Task.FromResult(0);
//        }
//    }
//
//    public class SmsService : IIdentityMessageService
//    {
//        public Task SendAsync(IdentityMessage message)
//        {
//            // Plug in your SMS service here to send a text message.
//            return Task.FromResult(0);
//        }
//    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    

    // Configure the application sign-in manager which is used in this application.
//    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
//    {
//        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
//            : base(userManager, authenticationManager)
//        {
//        }
//
//        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
//        {
//            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
//        }
//
//        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
//        {
//            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
//        }
//    }
}
