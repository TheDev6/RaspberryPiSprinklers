namespace Jobz.RootContracts.Security
{
    public interface IAuthenticator
    {
        ISignInResult SignIn(string username, string password);
    }
}
