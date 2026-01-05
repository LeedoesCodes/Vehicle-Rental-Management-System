using System;
using System.Security.Cryptography;

namespace Vehicle_Rental_Management_System.Helpers
{
    public static class SecurityHelper
    {
        public static string HashPassword(string password)
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] salt = new byte[16];
                rng.GetBytes(salt);

                using (var pbkdf2 = new Rfc2898DeriveBytes(
                           password,
                           salt,
                           10000,
                           HashAlgorithmName.SHA256))
                {
                    byte[] hash = pbkdf2.GetBytes(32);

                    byte[] result = new byte[48];
                    Buffer.BlockCopy(salt, 0, result, 0, 16);
                    Buffer.BlockCopy(hash, 0, result, 16, 32);

                    return Convert.ToBase64String(result);
                }
            }
        }

        public static bool VerifyPassword(string password, string storedHash)
        {
            byte[] data = Convert.FromBase64String(storedHash);

            byte[] salt = new byte[16];
            byte[] hash = new byte[32];

            Buffer.BlockCopy(data, 0, salt, 0, 16);
            Buffer.BlockCopy(data, 16, hash, 0, 32);

            using (var pbkdf2 = new Rfc2898DeriveBytes(
                       password,
                       salt,
                       10000,
                       HashAlgorithmName.SHA256))
            {
                byte[] testHash = pbkdf2.GetBytes(32);

                int diff = hash.Length ^ testHash.Length;
                for (int i = 0; i < hash.Length && i < testHash.Length; i++)
                {
                    diff |= hash[i] ^ testHash[i];
                }

                return diff == 0;
            }
        }
    }
}