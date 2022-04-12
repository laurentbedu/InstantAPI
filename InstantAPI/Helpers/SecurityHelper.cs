using InstantAPI.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace InstantAPI.Helpers
{
    public class SecurityHelper
    {
        public static void HashAppUserPassword(ref AppUser appUser)
        {
            byte[] salt = new byte[256 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            string b64salt = Convert.ToBase64String(salt);
            string encryptedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: appUser.Password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            ));
            appUser.Password = b64salt + "$" + encryptedPassword;
        }

        public static bool VerifyAppUserPassword(AppUser storedUser, AppUser appUser)
        {
            string storedEncyptedPassword = storedUser.Password.Split('$')[1];
            string b64salt = storedUser.Password.Split('$')[0];
            byte[] salt = Convert.FromBase64String(b64salt);
            string inputEncryptedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: appUser.Password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            ));

            return storedEncyptedPassword == inputEncryptedPassword;
        }


    }
}
