namespace Jobz.WebUi.Security
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using RootContracts.Security;

    public class HashUtility : IHashUtility
    {
        private byte[] HashPasswordWithSalt(byte[] toBeHashed, byte[] salt)
        {
            using (var sha = SHA512.Create())
            {
                return sha.ComputeHash(Combine(toBeHashed, salt));
            }
        }
        private byte[] Combine(byte[] first, byte[] second)
        {
            var ret = new byte[first.Length + second.Length];

            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);

            return ret;
        }

        private byte[] GenerateSaltBytes(int saltLength = 32)
        {
            using (var randomNumberGenerator = new RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[saltLength];
                randomNumberGenerator.GetBytes(randomNumber);

                return randomNumber;
            }
        }

        public string Hash(string toBeHashed, string salt)
        {
            var toBeHashedBytes = Encoding.UTF8.GetBytes(toBeHashed);
            var saltBytes = Convert.FromBase64String(salt);
            var hashBytes = this.HashPasswordWithSalt(toBeHashed: toBeHashedBytes, salt: saltBytes);
            var hashString = Convert.ToBase64String(hashBytes);
            return hashString;
        }

        

        public string GenerateSalt()
        {
            var saltBytes = this.GenerateSaltBytes();
            var saltStr = Convert.ToBase64String(saltBytes);
            return saltStr;
        }
    }
}