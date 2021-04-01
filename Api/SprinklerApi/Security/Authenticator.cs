namespace SprinklerApi.Security
{
    using System.Threading.Tasks;
    using Data;
    using Microsoft.EntityFrameworkCore;

    public class Authenticator
    {
        private readonly HashUtility _hashUtility;
        private readonly ISprinklerDataClient _dataClient;

        public Authenticator(
            HashUtility hashUtility,
            ISprinklerDataClient dataClient)
        {
            _hashUtility = hashUtility;
            _dataClient = dataClient;
        }

        public async Task<bool> IsValidUser(string username, string password) {
            var result = false;

            if (!string.IsNullOrWhiteSpace(username)
                && !string.IsNullOrWhiteSpace(password))
            {
                var user = await _dataClient.CallAsync(db => db.AppUsers.SingleOrDefaultAsync(u => u.Username == username));
                if (user != null)
                {
                    var hashPass = _hashUtility.Hash(toBeHashed: password, salt: user.Salt);
                    result = string.CompareOrdinal(hashPass, user.PasswordBase64Hash) == 0;
                }
            }

            return result;
        }
    }
}