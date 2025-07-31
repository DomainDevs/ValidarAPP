using System;
using System.Configuration;

namespace Sistran.Core.Application.CommonService.Helper
{
    /// <summary>
    /// Datos tipo fechas
    /// </summary>
    public static class HelperDate
    {
        /// <summary>
        /// Obtiene la fecha Actual
        /// </summary>
        /// <value>
        /// The now.
        /// </value>
        public static DateTime Now { get { return DateTime.Now; } }

        /// <summary>
        /// Obtiene el formato de fecha Corta
        /// </summary>
        /// <value>
        /// The short date pattern.
        /// </value>
        public static string ShortDatePattern { get; private set; }

        /// <summary>
        /// Obtiene el separador de fechas
        /// </summary>
        /// <value>
        /// The short date pattern.
        /// </value>
        public static string ShortDateSeparator { get; private set; }

        static HelperDate()
        {
            ShortDatePattern = ConfigurationManager.AppSettings.Get("ShortDatePattern");
            ShortDateSeparator = ConfigurationManager.AppSettings.Get("ShortDateSeparator");
            if (ShortDatePattern == null)
            {
                ShortDatePattern = "dd/MM/yyyy";
            }
            if (ShortDateSeparator == null)
            {
                ShortDateSeparator = "/";
            }
        }
    }
}
