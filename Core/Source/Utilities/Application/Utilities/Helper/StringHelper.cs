using System;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;

namespace Sistran.Core.Application.Utilities.Helper
{
    /// <summary>
    /// Ayudas y extenciones para las variables strings
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// Concatena una cadena de strings
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string ConcatenateString(params string[] values)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < values.Length; i++)
            {
                stringBuilder.Append(values[i]);
            }
            return stringBuilder.ToString();
        }

        public static bool ContainsEspecialCharacters(string value)
        {
            Regex req = new Regex("[_+-.,!@#$%^&*():;\\/|<>\"\' ]");
            return req.IsMatch(value);
        }

        public static string GetDefaultVale<T>()
        {
            string defaultValueLikeString = string.Empty;
            if (typeof(T) != typeof(string))
            {
                defaultValueLikeString = Convert.ToString(default(T));
            }
            return defaultValueLikeString;
        }

        public static string DateFormatCompatibleRulesR1()
        {
            string dateFormat = ConfigurationManager.AppSettings["DateFormatCompatibleRulesR1"];
            if (string.IsNullOrEmpty(dateFormat))
            {
                dateFormat = "MM/dd/yyyy HH:mm:ss";
            }
            return dateFormat;
        }

        public static string GetStringConceptDateCompatibleRulesR1(object dynamicConceptValue)
        {
            try
            {
                DateTime date = DateTime.Parse(dynamicConceptValue.ToString());
                return date.ToString(DateFormatCompatibleRulesR1());
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
