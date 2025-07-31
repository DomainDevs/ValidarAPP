using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Helpers
{
    public static class Helper
    {
        internal static string ToCurrency(this decimal property)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.CurrencySymbol = "$";
            nfi.CurrencyPositivePattern = 0;
            nfi.CurrencyGroupSeparator = ".";
            nfi.CurrencyDecimalSeparator = ",";
            nfi.CurrencyDecimalDigits = 2;
            return string.Format(nfi, "{0:C2}", property);
        }

        internal static string ToExchangeRate(this decimal property)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.CurrencySymbol = "$";
            nfi.CurrencyPositivePattern = 0;
            nfi.CurrencyGroupSeparator = ".";
            nfi.CurrencyDecimalSeparator = ",";
            nfi.CurrencyDecimalDigits = 6;
            return string.Format(nfi, "{0:C6}", property);
        }
    }
}
