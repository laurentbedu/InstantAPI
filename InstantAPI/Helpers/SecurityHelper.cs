using InstantAPI.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

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

        private static IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();



        public static string CreateJWt(AppUser appUser)
        {
            var test = configuration["Jwt:SigningKey"];

            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("Id", appUser.Id.ToString()),
                        new Claim("Login", appUser.Login),
                        new Claim("IdRole", appUser.IdRole.ToString())
                    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SigningKey"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(1),
                signingCredentials: signIn);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
