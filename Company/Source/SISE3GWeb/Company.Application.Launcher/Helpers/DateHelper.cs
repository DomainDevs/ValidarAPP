using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Configuration;
using System.Globalization;

namespace Sistran.Core.Framework.UIF.Web.Helpers
{
    /// <summary>
    /// Formato Fechas
    /// </summary>
    public class DateHelper
    {
        /// <summary>
        /// Si aplica  Bisiesto
        /// </summary>
        private static Boolean _leapYear;

        /// <summary>
        /// The is active
        /// </summary>
        private static Boolean _isActive;

        /// <summary>
        /// The format date
        /// </summary>
        public static string FormatDate = "dd/MM/yyyy";

        public static string FormatFullDate = "dd/MM/yyyy HH:mm:ss";
        /// <summary>
        /// The mindate date
        /// </summary>
        public static readonly string MindateDate = "01/01/1900";


        public static Boolean LeapYear
        {
            get
            {
                if (_isActive == false)
                {
                    _leapYear = DelegateService.underwritingService.GetLeapYear();
                    _isActive = true;
                }
                return _leapYear;
            }
            private set { _leapYear = value; }
        }

        static DateHelper()
        {
            if (_isActive == false)
            {
                _leapYear = DelegateService.underwritingService.GetLeapYear();
                _isActive = true;
            }
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["FormatDate"]))
            {
                FormatDate = ConfigurationManager.AppSettings["FormatDate"];
            }
        }

        /// <summary>
        /// Obtener la menor Fecha .net 01/01/0001 base 01/01/1900
        /// </summary>
        /// <returns></returns>
        public static DateTime GetMinDate() => DateTime.Parse(MindateDate);

    }

    public static class Extensions
    {
        /// <summary>
        /// To the date.
        /// </summary>
        /// <param name="dateTimeStr">The date time string.</param>
        /// <param name="dateFmt">The date FMT.</param>
        /// <returns></returns>
        public static DateTime ToDate(this string dateTimeStr, params string[] dateFmt)
        {
            if (!string.IsNullOrEmpty(dateTimeStr))
            {
                const DateTimeStyles style = DateTimeStyles.AllowWhiteSpaces;
                if (dateFmt == null || dateFmt.Length < 1)
                {
                    var dateInfo = System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat;
                    dateFmt = dateInfo.GetAllDateTimePatterns();
                }
                DateTime result = DateTime.MinValue;
                DateTime.TryParseExact(dateTimeStr, dateFmt, CultureInfo.InvariantCulture, style, out result);
                if (result == DateTime.MinValue)
                {
                    DateTime.TryParse(dateTimeStr, null, style, out result);
                }
                return result;
            }
            else
            {
                return DateTime.MinValue;
            }

        }
    }
}