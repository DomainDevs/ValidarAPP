using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Reinsurance.Helpers
{
    public class Formats
    {
        /// <summary>
        /// Formatea  un valor decimal  a moneda
        /// </summary>
        /// <param name="value"
        /// <returns>string</returns>
        public static string ConvertDecimalToCurrency(decimal value)
        {
           
            string money = ("$ " + String.Format("{0:n}", value)).ToString();
            int d = money.Length - 3;
            string valor = money.Substring(0, d).Replace(".", ",");
            string decim = money.Substring(d,3).Replace(",", ".");

            return valor + decim;

        }

        /// <summary>
        /// Elimina tildes, acentos, signos especiales de un string
        /// </summary>
        /// <param name="value"
        /// <returns>string</returns>
        public string RemoverSignosAcentos(string texto)
        {
            string ConSignos = "áàäéèëíìïóòöúùuñÁÀÄÉÈËÍÌÏÓÒÖÚÙÜçÇÑ";
            string SinSignos = "aaaeeeiiiooouuunAAAEEEIIIOOOUUUcCN";

            var textoSinAcentos = string.Empty;

            foreach (var caracter in texto)
            {
                var indexConAcento = ConSignos.IndexOf(caracter);
                if (indexConAcento > -1)
                    textoSinAcentos = textoSinAcentos + (SinSignos.Substring(indexConAcento, 1));
                else
                    textoSinAcentos = textoSinAcentos + (caracter);
            }
            return textoSinAcentos;
        }

    }
}