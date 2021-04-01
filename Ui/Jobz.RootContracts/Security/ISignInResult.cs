namespace Jobz.RootContracts.Security
{
    using System.Security.Claims;

    public interface ISignInResult
    {
        bool IsValidSignIn { get; set; }
        ClaimsIdentity ClaimsIdentity { get; set; }
    }
}