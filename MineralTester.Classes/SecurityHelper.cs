using System;
using System.Security.Cryptography;

/// <summary>
/// Written by: Quinn Nimmer
/// </summary>
namespace MineralTester.Classes
{
    /// <summary>
    /// Class for generating salt and hashing passwords.
    /// </summary>
    class SecurityHelper
    {
        /// <summary>
        /// Generates a salt.
        /// </summary>
        /// <returns> A salt string. </returns>
        public static string GenerateSalt()
        {
            var saltBytes = new byte[70];
            using (var provider = new RNGCryptoServiceProvider())
            {
                provider.GetNonZeroBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        /// <summary>
        /// Hashes a password.
        /// </summary>
        /// <param name="password"> The password to hash. </param>
        /// <param name="salt"> The salt for the password. </param>
        /// <returns> A string representation of the salted and hashed password. </returns>
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
