namespace Jobz.WebUi.Security
{
    using System.Security.Claims;
    using RootContracts.Security;

    public class SignInResult : ISignInResult
    {
        public bool IsValidSignIn { get; set; }
        public ClaimsIdentity ClaimsIdentity { get; set; }
    }
}