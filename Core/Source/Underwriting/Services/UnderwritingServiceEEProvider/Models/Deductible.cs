using Sistran.Core.Application.TaxServices;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using Model = Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.BusinessModels
{
    public class Deductible
    {
        /// <summary>
        /// Cobertura
        /// </summary>
        protected Model.Coverage CoverageModel;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="coverage">Cobertura</param>
        public Deductible(Model.Coverage coverage)
        {
            CoverageModel = coverage;
        }

        /// <summary>
        /// Calcular Prima Deducible
        /// </summary>
        public void CalculatePremiumDeductible()
        {
            int effectiveDays = CalculateEffectiveDays(CoverageModel.CurrentFrom, CoverageModel.CurrentTo);
            decimal coeficientRate = CalculateCoeficientRate(CoverageModel.Rate.GetValueOrDefault(), CoverageModel.RateType);
            decimal coeficientRateDeductible = CalculateCoeficientRateDeductible(CoverageModel.Deductible.Rate.GetValueOrDefault(), CoverageModel.Deductible.RateType);
            decimal premiumAmount = 0;

            if (CoverageModel.Deductible.RateType == RateType.FixedValue)
            {
                if (CoverageModel.Deductible.Rate.HasValue)
                {
                    premiumAmount = (CoverageModel.Deductible.Rate.Value / QuoteManager.AnnualDays) * effectiveDays;
                }
            }
            else
            {
                decimal annualPremium = (CoverageModel.SubLimitAmount * coeficientRate) - CoverageModel.AccumulatedPremiumAmount;
                premiumAmount = (annualPremium + CoverageModel.AccumulatedPremiumAmount) * coeficientRateDeductible;
            }

            decimal acumulatedPremiumAmountProrrated = (CoverageModel.Deductible.AccDeductAmt / QuoteManager.AnnualDays) * effectiveDays;
            decimal premiumDifference = premiumAmount - acumulatedPremiumAmountProrrated;
            decimal delta = new decimal(0.01);

            if ((premiumDifference < delta) && (premiumDifference > (delta * (-1))))
            {
                premiumDifference = new decimal(0);
            }

            decimal proratePremium = decimal.Round(premiumDifference, QuoteManager.DecimalRound);

            CoverageModel.Deductible.DeductPremiumAmount = proratePremium;
        }

        /// <summary>
        /// Calcular Dias de Vigencia
        /// </summary>
        /// <param name="currentFrom">Fecha Inicial</param>
        /// <param name="currentTo">Fecha Final</param>
        /// <returns>Dias de Vigencia</returns>
        private int CalculateEffectiveDays(DateTime currentFrom, DateTime? currentTo)
        {
            int effectiveDays = QuoteManager.AnnualDays;

            if (currentTo.HasValue)
            {
                if (QuoteManager.LeapYear)
                {
                    int daysToRest = 0;

                    for (int year = currentFrom.Year; year < currentTo.Value.Year; year++)
                    {
                        if (DateTime.IsLeapYear(year) && (new DateTime(year, 2, 29) >= currentFrom && new DateTime(year, 2, 29) <= currentTo.Value))
                        {
                            daysToRest++;
                        }
                    }

                    effectiveDays = (currentTo.Value - currentFrom).Days - daysToRest;
                }
                else
                {
                    effectiveDays = (currentTo.Value - currentFrom).Days;
                }
            }

            return effectiveDays;
        }

        /// <summary>
        /// Calcular Tasa
        /// </summary>
        /// <param name="rate">Tasa</param>
        /// <param name="rateType">Tipo de Tasa</param>
        /// <returns>Tasa</returns>
        private decimal CalculateCoeficientRate(decimal rate, RateType? rateType)
        {
            decimal coeficientPremiumRate = 0;

            if (rate > 0)
            {
                decimal factor = Convert.ToDecimal(GetFactor(rateType));

                if (rateType != RateType.FixedValue)
                {
                    coeficientPremiumRate = rate * factor;
                }
                else
                {
                    coeficientPremiumRate = 1;
                }
            }

            return coeficientPremiumRate;
        }

        /// <summary>
        /// Obtener Factor por Tipo de Tasa
        /// </summary>
        /// <param name="rateType">Tipo de Tasa</param>
        /// <returns>Factor</returns>
        private decimal GetFactor(RateType? rateType)
        {
            decimal numerator = 1;
            decimal divider = 1;

            switch (rateType)
            {
                case RateType.FixedValue:
                    divider = 1;
                    break;
                case RateType.Percentage:
                    divider = 100;
                    break;
                case RateType.Permilage:
                    divider = 1000;
                    break;
            }

            return (numerator / divider);
        }

        /// <summary>
        /// Calcular Tasa
        /// </summary>
        /// <param name="rate">Tasa</param>
        /// <param name="rateType">Tipo de Tasa</param>
        /// <returns>Tasa</returns>
        private decimal CalculateCoeficientRateDeductible(decimal rate, RateType? rateType)
        {
            decimal coeficientPremiumRate = 0;

            if (rate > 0)
            {
                decimal factor = Convert.ToDecimal(GetFactor(rateType));

                if (rateType != RateType.FixedValue)
                {
                    coeficientPremiumRate = rate * factor;
                }
                else
                {
                    coeficientPremiumRate = 1;
                }
            }
            else
            {
                coeficientPremiumRate = 1;
            }

            return coeficientPremiumRate;
        }
    }
}