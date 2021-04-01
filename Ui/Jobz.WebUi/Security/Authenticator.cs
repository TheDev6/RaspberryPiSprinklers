namespace Jobz.WebUi.Security
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using RootContracts.BehaviorContracts.Configuration;
    using RootContracts.Security;

    public class Authenticator : IAuthenticator
    {
        private readonly IHashUtility _hashUtil;
        private readonly IConfig _config;

        public Authenticator(
            IHashUtility hashUtil,
            IConfig config)
        {
            _hashUtil = hashUtil;
            _config = config;
        }

        public ISignInResult SignIn(string username, string password)
        {
            var result = new SignInResult { IsValidSignIn = false };

            var adminUser = _config.AdminUserName();
            var guestUser = _config.GuestUsername();
            var isAdmin = username == adminUser;
            var isGuest = username == guestUser;
            if ((isAdmin
                || isGuest)
                && !string.IsNullOrWhiteSpace(username)
                && !string.IsNullOrWhiteSpace(password))
            {
                string salt = isAdmin
                    ? _config.AdminUserSalt()
                    : _config.GuestUserSalt();
                var hash = _hashUtil.Hash(toBeHashed: password, salt: salt);
                var isPassMatch = isAdmin
                    ? String.Compare(hash, _config.AdminUserPassHash(), StringComparison.OrdinalIgnoreCase) == 0
                    : String.Compare(hash, _config.GuestUserPassHash(), StringComparison.OrdinalIgnoreCase) == 0;
                if (isPassMatch)
                {
                    result.IsValidSignIn = true;
                    result.ClaimsIdentity = BuildClaimsIdentity(username);
                }
            }
            return result;
        }

        internal ClaimsIdentity BuildClaimsIdentity(string name)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, name),
            };
            return new ClaimsIdentity(
                claims: claims,
                authenticationType: _config.ApplicationCookieName());
        }
    }
}