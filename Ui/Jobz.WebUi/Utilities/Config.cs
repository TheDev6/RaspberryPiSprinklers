namespace Jobz.WebUi.Utilities
{
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using RootContracts.BehaviorContracts.Configuration;

    public class Config : IConfig
    {
        public string AiInstrumentKey() => ConfigurationManager.AppSettings.Get("AiInstrumentKey");
        public string ApplicationCookieName() => "ApplicationCookie";
        public string AdminUserPassHash() => ConfigurationManager.AppSettings.Get("AdminUserPassHash");
        public string AdminUserName() => ConfigurationManager.AppSettings.Get("AdminUserName");
        public string AdminUserSalt() => ConfigurationManager.AppSettings.Get("AdminUserSalt");
        public string GuestUsername() => ConfigurationManager.AppSettings.Get("GuestUsername");
        public string GuestUserPassHash() => ConfigurationManager.AppSettings.Get("GuestUserPassHash");
        public string GuestUserSalt() => ConfigurationManager.AppSettings.Get("GuestUserSalt");
        
        public string SprinklerApiUrl() => ConfigurationManager.AppSettings.Get("SprinklerApiUrl");
        public string SprinklerApiCredentials() => ConfigurationManager.AppSettings.Get("SprinklerApiCredentials");

        public bool SecureControllerActions() => Convert.ToBoolean(ConfigurationManager.AppSettings.Get("SecureControllerActions"));
        public string AppVersion()
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var fileVersion = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fileVersion.FileVersion;
        }
        public string BlobConnectionString() => ConfigurationManager.AppSettings.Get("BlobConnectionString");
        public string BlobContainerName() => ConfigurationManager.AppSettings.Get("BlobContainerName");
    }
}