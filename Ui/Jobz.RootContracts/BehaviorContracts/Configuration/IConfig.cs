namespace Jobz.RootContracts.BehaviorContracts.Configuration
{
    public interface IConfig
    {
        string AiInstrumentKey();
        string ApplicationCookieName();
        string AppVersion();
        string AdminUserPassHash();
        string AdminUserName();
        string AdminUserSalt();
        string GuestUsername();
        string GuestUserPassHash();
        string GuestUserSalt();

        string SprinklerApiUrl();
        string SprinklerApiCredentials();

        bool SecureControllerActions();
        string BlobConnectionString();
        string BlobContainerName();
    }
}
