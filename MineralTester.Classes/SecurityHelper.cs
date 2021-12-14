using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MineralTester.Classes
{
    /// <summary>
    /// Class for generating salt and hashing passwords.
    /// Written by: Quinn Nimmer
    /// </summary>
    class SecurityHelper
    {
        public static string GenerateSalt()
        {
            var saltBytes = new byte[70];

            using (var provider = new RNGCryptoServiceProvider())
            {
                provider.GetNonZeroBytes(saltBytes);
            }

            return Convert.ToBase64String(saltBytes);
        }

        public static string HashPassword(string password, string salt)
        {
            var saltBytes = Convert.FromBase64String(salt);

            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 10101))
            {
                return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(70));
            }
        }

    }
}
