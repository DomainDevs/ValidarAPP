using Sistran.Core.Application.Common.Entities;
using System;
using System.Configuration;
using Sistran.Core.Application.Utilities.Enums;

namespace Sistran.Core.Application.Utilities.Managers
{
    public class QuoteManager
    {
        /// <summary>
        /// Dias Año
        /// </summary>
        public const int AnnualDays = 365;

        /// <summary>
        /// Decimales a Redondear
        /// </summary>
        public const int DecimalRound = 2;

        /// <summary>
        /// Es Año Bisiesto
        /// </summary>
        private static bool? leapYear = null;

        /// <summary>
        /// Valor de redondeo
        /// </summary>
        /// <value>
        /// Valor de redondeo
        /// </value>
        public static int RoundValue { get; private set; }

        /// <summary>
        /// Valor de redondeo Prima
        /// </summary>
        /// <value>
        /// Valor de redondeo Prima
        /// </value>
        public static int PremiumRoundValue { get; private set; }

        static QuoteManager()
        {
            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["RoundValue"]))
            {
                RoundValue = 2;
            }
            else
            {
                RoundValue = Convert.ToInt32(ConfigurationManager.AppSettings["RoundValue"]);
            }
            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["PremiumRoundValue"]))
            {
                PremiumRoundValue = 6;
            }
            else
            {
                PremiumRoundValue = Convert.ToInt32(ConfigurationManager.AppSettings["PremiumRoundValue"]);
            }


        }
        /// <summary>
        /// Es Año Bisiesto
        /// </summary>
        public static bool LeapYear
        {
            get
            {
                if (leapYear == null)
                {
                    Parameter parameter = ManagerDAO.GetParameterByParameterId(10000);

                    if (parameter != null)
                    {
                        leapYear = parameter.BoolParameter;
                    }
                    else
                    {
                        leapYear = false;
                    }
                }

                return leapYear.Value;
            }
        }

        public static decimal[] RoundCollection(decimal[] collection, decimal totalToRound, int decimalPrecision)
        {
            int top = collection.GetUpperBound(0);
            int bottom = collection.GetLowerBound(0);
            decimal[] resultList = new decimal[top - bottom + 1];
            decimal roundTotal = 0;

            for (int index = bottom; index <= top; index++)
            {
                decimal roundValue = decimal.Round(collection[index], decimalPrecision);
                roundTotal += roundValue;
                resultList[index] = roundValue;
            }

            resultList[top] += decimal.Round(totalToRound - roundTotal, decimalPrecision);

            return resultList;
        }

        /// <summary>
        /// Calcular Dias de Vigencia
        /// </summary>
        /// <param name="currentFrom">Fecha Inicial</param>
        /// <param name="currentTo">Fecha Final</param>
        /// <returns>Dias de Vigencia</returns>
        public static int CalculateEffectiveDays(DateTime currentFrom, DateTime? currentTo)
        {
            int effectiveDays = AnnualDays;

            if (currentTo.HasValue)
            {
                effectiveDays = (currentTo.Value - currentFrom).Days;
                if (!LeapYear && effectiveDays == (AnnualDays + 1))
                {
                    int daysToRest = CalculateLeapDays(currentFrom, currentTo.Value);
                    effectiveDays -= daysToRest;
                }
            }

            return effectiveDays;
        }

        /// <summary>
        /// devuelve el valor para calcular la tasa
        /// </summary>
        /// <param name="rateType">Tipo tasa</param>
        /// <returns>Dias de Vigencia</returns>
        public static decimal GetFactor(TaxType rateType)
        {
            decimal numerator = 1;
            decimal divider = 1;

            switch (rateType)
            {
                case TaxType.FixedValue:
                    divider = 1;
                    break;
                case TaxType.Percentage:
                    divider = 100;
                    break;
                case TaxType.Permilage:
                    divider = 1000;
                    break;
            }

            return (numerator / divider);
        }

        private static int CalculateLeapDays(DateTime currentFrom, DateTime currentTo)
        {
            int leapDays = 0;
            if (currentTo < currentFrom)
            {
                return leapDays;
            }
            for (int year = currentFrom.Year; year <= currentTo.Year; year++)
            {
                if (DateTime.IsLeapYear(year) && (new DateTime(year, 2, 29) >= currentFrom && new DateTime(year, 2, 29) <= currentTo))
                {
                    leapDays++;
                }
            }
            return leapDays;
        }
    }
}