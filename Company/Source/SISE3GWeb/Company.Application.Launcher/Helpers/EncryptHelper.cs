// -----------------------------------------------------------------------
// <copyright file="EncryptHelper.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Robinson Castro Londoño</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Helpers
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public static class EncryptHelper
    {
        private static readonly string PasswordHash = "fs14ff!#@@s48";
        private static readonly string SaltKey = "$@w%48gp";
        private static readonly string VIKey = "@@lsd45f1hya8gg5df";

        /// <summary>
        /// Permite encripriptar una cadena de caracteres
        /// </summary>
        /// <param name="text">texto a encriptar</param>
        /// <returns>texto encriptado</returns>
        public static string EncryptKey(string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));

            byte[] cipherTextBytes;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    cryptoStream.Close();
                }
                memoryStream.Close();
            }
            return Convert.ToBase64String(cipherTextBytes);
        }

        /// <summary>
        /// Permite desencripriptar una cadena de caracteres
        /// </summary>
        /// <param name="text">texto a desencriptar</param>
        /// <returns>texto desencriptado</returns>
        public static string DecryptKey(string encryptedText)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());

        }

        public static string EncryptSupLogin(string value)
        {
            string secretKey = "Ña25%fy&";
            string p = "", b = "", S = "";
            int J = 0;
            int A1 = 0, A2 = 0, A3 = 0;

            for (int i = 0; i < secretKey.Length; i++)
            {
                p = p + System.Text.Encoding.GetEncoding("iso-8859-1").GetBytes(secretKey.Substring(i, 1))[0];
            }

            for (int i = 0; i < value.Length; i++)
            {
                A1 = System.Text.Encoding.GetEncoding("iso-8859-1").GetBytes(p.Substring(J, 1))[0];
                J = (J > p.Length ? 1 : J + 1);
                A2 = System.Text.Encoding.GetEncoding("iso-8859-1").GetBytes(value.Substring(i, 1))[0];
                A3 = A1 ^ A2;
                b = String.Format("{0:X}", A3);
                if (b.Length < 2) { b = "0" + b; }
                S = S + b;
            }
            return S;
        }
    }
}