using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Helpers
{
    public class CurrencyHelper
    {
        public static T RoundDecimal<T>(T roundValue, int decimals) where T : struct
        {
            T roundValeNew = default(T);

            if (roundValue is decimal)
            {
                return (T)Convert.ChangeType(System.Math.Round(Convert.ToDecimal(roundValue), decimals), roundValeNew.GetType());
            }
            else if (roundValue is float)
            {
                return (T)Convert.ChangeType(System.Math.Round(Convert.ToDouble(roundValue), decimals), roundValeNew.GetType());
            }
            else if (roundValue is double)
            {
                return (T)Convert.ChangeType(System.Math.Round(Convert.ToDouble(roundValue), decimals), roundValeNew.GetType());
            }
            else
            {
                throw new Exception("Tipo de Dato no Valido");
            }
        }

    }
}