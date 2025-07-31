/*****************************************************************************
 * Auttor : Marcelo A. Charytoniuk
//  {05/07/2004, mcharytoniuk, MODIFICACIÓN, Agregado de partícula DC=local 
//  en la determinación del dominio de DS services.}
 * ***************************************************************************/
using Sistran.Core.Framework;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
namespace Sistran.Core.Application.EEProvider.Helper
{
    public class CommonUtilitiesUser
    {

        public byte[] Clave = Encoding.ASCII.GetBytes("SistranKey");
        public byte[] IV = Encoding.ASCII.GetBytes("Devjoker7.37hAES");

        /// <summary>
        /// Return or set if PasswordExpirationDate property value is null.
        /// </summary>
        /// <value>true if PasswordExpirationDate property value is null, false
        /// otherwise.</value>
        public static int CalculateExpirationDays(DateTime dateToCalculate)
        {
            TimeSpan diffDays = dateToCalculate.Subtract(
                BusinessServices.GetDate());
            return Convert.ToInt32(diffDays.Days);
        }

        /// <summary>
        /// Return or set if PasswordExpirationDate property value is null.
        /// </summary>
        /// <value>true if PasswordExpirationDate property value is null, 
        /// false otherwise.</value>
        public static DateTime CalculateExpirationDate(int diasAlVencimiento)
        {
            return BusinessServices.GetDate().AddDays(diasAlVencimiento);
        }

        /// <summary>
        /// Extrae el Domain del Login Name
        /// </summary>
        /// <param name="loginName">
        /// LoginName del que se desea extraer el Domain.
        /// </param>
        /// <returns>
        /// LoginName domain (formato xx.xx.xx).
        /// </returns>
        public static string GetDomainFromLoginName(string loginName)
        {
            int indice;
            string resultado = "";
            if ((indice = loginName.IndexOfAny(new char[] { '\\' })) > 0)
            {
                resultado = loginName.Split(new char[] { '\\' })[0];
            }
            return resultado;
        }

        /// <summary>
        /// Extrae el Nick del Login Name
        /// </summary>
        /// <param name="loginName">
        /// LoginName del que se desea extraer el NickName.
        /// </param>
        /// <returns>
        /// LoginName Nickname
        /// </returns>
        public static string GetNickFromLoginName(string loginName)
        {
            int indice;
            string resultado = "";
            if ((indice = loginName.IndexOfAny(new char[] { '\\' })) > 0)
            {
                resultado = loginName.Split(new char[] { '\\' })[1];
            }
            else
            {
                resultado = loginName;
            }

            return resultado;
        }

        public string Encrypt(string Cadena)
        {

            byte[] inputBytes = Encoding.ASCII.GetBytes(Cadena);
            byte[] encripted;
            RijndaelManaged cripto = new RijndaelManaged();
            using (MemoryStream ms = new MemoryStream(inputBytes.Length))
            {
                using (CryptoStream objCryptoStream = new CryptoStream(ms, cripto.CreateEncryptor(Clave, IV), CryptoStreamMode.Write))
                {
                    objCryptoStream.Write(inputBytes, 0, inputBytes.Length);
                    objCryptoStream.FlushFinalBlock();
                    objCryptoStream.Close();
                }
                encripted = ms.ToArray();
            }
            return Convert.ToBase64String(encripted);
        }



        public string Decrypt(string Cadena)
        {
            byte[] inputBytes = Convert.FromBase64String(Cadena);
            byte[] resultBytes = new byte[inputBytes.Length];
            string textoLimpio = String.Empty;
            RijndaelManaged cripto = new RijndaelManaged();
            using (MemoryStream ms = new MemoryStream(inputBytes))
            {
                using (CryptoStream objCryptoStream = new CryptoStream(ms, cripto.CreateDecryptor(Clave, IV), CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(objCryptoStream, true))
                    {
                        textoLimpio = sr.ReadToEnd();
                    }
                }
            }
            return textoLimpio;
        }
    }
}
