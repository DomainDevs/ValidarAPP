using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Sistran.Core.Application.UniqueUserServices.EEProvider.Helper
{
    public class SecurityHelper
    {
        /// <summary>
        /// Generate sha256
        /// </summary> 
        /// <param name="password">clear password</param>
        /// <param name="salt">random number</param>
        /// <returns>password hashed</returns>
        public static string GetHashSha256(string password, string salt)
        {
            StringBuilder stringBuilder = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(salt + password));
                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                foreach (Byte byteResult in result)
                    stringBuilder.Append(byteResult.ToString("x2"));
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Generate random number
        /// </summary> 
        /// <returns>string salt</returns>
        public static string CreateSalt()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            int saltLength = Convert.ToByte(ConfigurationManager.AppSettings["MaximumSaltLength"]);
            byte[] salt = new byte[saltLength];
            rng.GetBytes(salt);
            return Convert.ToBase64String(salt);
        }

    }
}
