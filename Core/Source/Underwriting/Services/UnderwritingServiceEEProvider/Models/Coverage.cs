using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Resources;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using Model = Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.BusinessModels
{
    public class Coverage
    {
        /// <summary>
        /// Cobertura
        /// </summary>
        protected Model.Coverage CoverageModel;

        /// <summary>
        /// Identificador de poliza 
        /// </summary>
        protected int PolicyId;

        /// <summary>
        /// Identificador de riesgo
        /// </summary>
        protected int RiskId;

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="coverage">Parametro de Busqueda</param>
        /// <param name="policyId">Identificador de poliza de endoso</param>
        /// <param name="riskId">Identificador del riesgo en endoso</param>
        public Coverage(Model.Coverage coverage, int policyId, int riskId)
        {
            CoverageModel = coverage;
            PolicyId = policyId;
            RiskId = riskId;
        }

        /// <summary>
        /// Tarifar
        /// </summary>
        public void Quotate(int decimalQuantity, int? prefixId = 0)
        {
            if (CoverageModel.EndorsementType != Enums.EndorsementType.Cancellation)
            {
                /*Valida suma asegurada para una cobertura adicional */
                if (CoverageModel.MainCoverageId != 0 && CoverageModel.MainCoverageId != null && (CoverageModel.AllyCoverageId == 0 || CoverageModel.AllyCoverageId == null))
                {
                    if (CoverageModel.SublimitPercentage != null && CoverageModel.SublimitPercentage != 0)
                    {
                        CoverageModel.SubLimitAmount = (decimal)(CoverageModel.DeclaredAmount * (int)CoverageModel.SublimitPercentage / 100);
                        CoverageModel.LimitOccurrenceAmount = CoverageModel.SubLimitAmount;
                        CoverageModel.LimitClaimantAmount = CoverageModel.SubLimitAmount;
                        CoverageModel.MaxLiabilityAmount = CoverageModel.SubLimitAmount;
                    }
                }

                switch (CoverageModel.EndorsementType)
                {
                    case Enums.EndorsementType.Modification:
                        if (CoverageModel.ModificationTypeId != 4)
                        {
                            CalculatePremiumModification(decimalQuantity);
                        }
                        else
                        {
                            CalculatePremium(decimalQuantity);
                        }
                        break;
                    case Enums.EndorsementType.EffectiveExtension:
                        CalculatePremiumEffectiveExtension(decimalQuantity);
                        break;
                    default:
                        CalculatePremium(decimalQuantity);
                        break;
                }

                if (CoverageModel.MinimumPremiumCoverage != null && CoverageModel.MinimumPremiumCoverage > 0 && CoverageModel.PremiumAmount > 0 && CoverageModel.PremiumAmount < CoverageModel.MinimumPremiumCoverage)
                {
                    CoverageModel.PremiumAmount = (decimal)CoverageModel.MinimumPremiumCoverage;
                }
                /*Valida suma asegurada para una cobertura aliadas */
                if (CoverageModel.AllyCoverageId != null)
                {
                    CoverageModel.SubLimitAmount = (decimal)(CoverageModel.DeclaredAmount * (int)CoverageModel.SublimitPercentage / 100);
                    CoverageModel.LimitClaimantAmount = CoverageModel.SubLimitAmount;
                    CoverageModel.LimitAmount = 0;
                    CoverageModel.PremiumAmount = 0;
                    CoverageModel.Rate = 0;
                }

                if (prefixId != 0 && (PrefixRc)prefixId == PrefixRc.Liability)
                {
                    CoverageModel.SubLimitAmount = (decimal)(CoverageModel.DeclaredAmount);
                    CoverageModel.LimitAmount = (decimal)(CoverageModel.DeclaredAmount);
                    if (CoverageModel.IsPrimary == false)
                    {
                        CoverageModel.LimitClaimantAmount = CoverageModel.LimitClaimantAmount;
                        CoverageModel.PremiumAmount = 0;
                        CoverageModel.Rate = 0;
                    }

                }
            }

            SetPremiumAmountZeroWhenClaimExlusion();
        }

        /// <summary>
        /// Calcular Prima Endoso Modificación
        /// </summary>
        /// <returns>Prima</returns>
        private void CalculatePremiumModification(int decimalQuantity)
        {
            decimal premiumBase = 0;
            if (CoverageModel.CoverStatus == CoverageStatusType.Excluded)
            {
                CalculateExcludedPremium(decimalQuantity);
            }
            else
            {
                if (CoverageModel.RateType != RateType.FixedValue && CoverageModel.CalculationType == Services.UtilitiesServices.Enums.CalculationType.Prorate)
                {
                    CoverageModel.EndorsementSublimitAmount = CoverageModel.SubLimitAmount - CoverageModel.OriginalSubLimitAmount;
                    if (CoverageModel.PremiumAmount != 0 || CoverageModel.EndorsementSublimitAmount != 0 || CoverageModel.CurrentTo != CoverageModel.CurrentToOriginal || CoverageModel.Rate != CoverageModel.OriginalRate)
                    {
                        CalculatePremium(decimalQuantity);
                    }
                }
                else
                {
                    CalculatePremium(decimalQuantity);
                }
            }
        }

        /// <summary>
        /// Calcular Prima Exclusión
        /// </summary>
        /// <returns>Prima</returns>
        private void CalculateExcludedPremium(int decimalQuantity)
        {
            DateTime currentFrom = CoverageModel.CurrentFrom;
            decimal premiumAmount = 0;

            CoverageDAO coverageDAO = new CoverageDAO();
            Model.Coverage coverageOriginal = coverageDAO.GetCoverageByRiskCoverageId(CoverageModel.RiskCoverageId);
            if (coverageOriginal != null)
            {
                if (currentFrom <= coverageOriginal.CurrentFrom)
                {
                    premiumAmount = coverageOriginal.PremiumAmount;
                }
                else if (currentFrom >= coverageOriginal.CurrentTo)
                {
                    premiumAmount = 0;
                }
                else
                {
                    int effectiveDays = CalculateEffectiveDays(coverageOriginal.CurrentFrom, coverageOriginal.CurrentTo);
                    int cancellationDays = CalculateEffectiveDays(currentFrom, CoverageModel.CurrentTo);
                    premiumAmount = decimal.Round(((coverageOriginal.PremiumAmount * cancellationDays) / effectiveDays), decimalQuantity);

                }
            }
            else
            {
                throw new Exception(String.Format("{0}: {1} - Id: {2}", Errors.CoverageNotFound, CoverageModel?.RiskCoverageId.ToString(), CoverageModel?.Id));
            }
            CoverageModel.PremiumAmount = premiumAmount * -1;
            CoverageModel.Rate = CoverageModel.Rate * -1;
            CoverageModel.EndorsementLimitAmount = CoverageModel.LimitAmount * -1;
            CoverageModel.EndorsementSublimitAmount = CoverageModel.SubLimitAmount * -1;
            CoverageModel.LimitAmount = 0;
            CoverageModel.SubLimitAmount = 0;
            CoverageModel.CoverStatus = CoverageStatusType.Excluded;
            CoverageModel.CoverStatusName = Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Excluded));
        }

        /// <summary>
        /// Calcular Prima Extensión
        /// </summary>
        /// <returns>Prima</returns>
        private void CalculatePremiumEffectiveExtension(int decimalQuantity)
        {
            CalculateProrate(decimalQuantity);
        }

        /// <summary>
        /// Calcular Prima
        /// </summary>
        /// <returns>Prima</returns>
        private void CalculatePremium(int decimalQuantity)
        {
            switch (CoverageModel.CalculationType)
            {
                case Services.UtilitiesServices.Enums.CalculationType.Direct:
                    CalculateDirect(decimalQuantity);
                    break;
                case Services.UtilitiesServices.Enums.CalculationType.Prorate:
                    CalculateProrate(decimalQuantity);
                    break;
                case Services.UtilitiesServices.Enums.CalculationType.ShortTerm:
                    CalculateShortTerm();
                    break;
                default:
                    CoverageModel.PremiumAmount = 0;
                    break;
            }
        }

        /// <summary>
        /// Calcular Prima Directa
        /// </summary>
        /// <returns>Prima</returns>
        private void CalculateDirect(int decimalQuantity, int? coveredRiskType = 0)
        {
            decimal premiumBase = 0;

            if (CoverageModel.RateType == RateType.FixedValue)
            {
                if (CoverageModel.CoveredRiskType == (int)CoveredRiskType.Surety)
                {
                    CoverageModel.PremiumAmount = CoverageModel.PremiumAmount;
                }
                else
                {
                    CoverageModel.PremiumAmount = CoverageModel.Rate.GetValueOrDefault();
                    if (CoverageModel.CoveredRiskType == (int)CoveredRiskType.Location) CalculatePremiumDifference(CoverageModel.PremiumAmount, decimalQuantity);
                }

            }
            else
            {
                decimal coeficientRate = CalculateCoeficientRate(CoverageModel.Rate.GetValueOrDefault(), CoverageModel.RateType);
                premiumBase = CoverageModel.SubLimitAmount * coeficientRate;
                CalculatePremiumDifference(premiumBase, decimalQuantity);
            }
        }

        /// <summary>
        /// Calcular Prima a Prorrata
        /// </summary>
        /// <returns>Prima</returns>
        private void CalculateProrate(int decimalQuantity)
        {
            decimal premiumBase = 0;
            decimal coeficientRate = CalculateCoeficientRate(CoverageModel.Rate.GetValueOrDefault(), CoverageModel.RateType);
            int effectiveDays = CalculateEffectiveDays(CoverageModel.CurrentFrom, CoverageModel.CurrentTo);

            if (CoverageModel.RateType == RateType.FixedValue)
            {
                premiumBase = decimal.Round((CoverageModel.Rate.GetValueOrDefault() * effectiveDays) / QuoteManager.AnnualDays, decimalQuantity);// QuoteManager.DecimalRound);
                if (CoverageModel.EndorsementType == Enums.EndorsementType.Modification)
                {
                    CalculatePremiumDifference(premiumBase, decimalQuantity);
                }
                else
                {
                    CoverageModel.PremiumAmount = premiumBase;
                }
            }
            else
            {

                premiumBase = decimal.Round((CoverageModel.SubLimitAmount * coeficientRate * effectiveDays) / QuoteManager.AnnualDays, decimalQuantity);//QuoteManager.DecimalRound);
                CalculatePremiumDifference(premiumBase, decimalQuantity);

            }

        }

        /// <summary>
        /// Calcular Prima a Corto Plazo
        /// </summary>
        /// <returns>Prima</returns>
        private void CalculateShortTerm()
        {
            CoverageModel.PremiumAmount = 0;
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
        /// Calcular Diferencia de Prima
        /// </summary>
        /// <param name="premiumBase">Prima Base</param>
        /// <param name="premiumRate">Tasa</param>
        /// <param name="prorateDays">Dias a Prorrata</param>
        /// <param name="effectiveDays">Dias Efectivos</param>
        /// <returns>Diferencia de Prima</returns>
        private void CalculatePremiumDifference(decimal premiumBase, int decimalQuantity)
        {
            decimal accumulatedPremium = 0;

            if (CoverageModel.EndorsementType == Enums.EndorsementType.Modification && CoverageModel.ModificationTypeId != 4 && CoverageModel.ModificationTypeId != 2 && CoverageModel.Rate > 0
                && CoverageModel.CalculationType != Services.UtilitiesServices.Enums.CalculationType.Direct && CoverageModel.RateType != Services.UtilitiesServices.Enums.RateType.FixedValue)
            {
                if (CoverageModel.CalculationType == Services.UtilitiesServices.Enums.CalculationType.Direct && CoverageModel.RateType == Services.UtilitiesServices.Enums.RateType.FixedValue)
                {
                    premiumBase = CoverageModel.Rate.GetValueOrDefault() - CoverageModel.OriginalRate.GetValueOrDefault();
                }
                else if (PolicyId != 0 && RiskId != 0)
                {
                    CoverageDAO coverageDAO = new CoverageDAO();
                    List<Models.Coverage> previousCoverages = coverageDAO.GetCoveragesByPolicyIdRiskIdCoverageId(PolicyId, RiskId, CoverageModel.Id);

                    if (previousCoverages != null)
                    {
                        if (CoverageModel.InsuredObject != null && CoverageModel.InsuredObject.IsDeclarative)
                        {
                            previousCoverages[0].PremiumAmount = decimal.Round(((previousCoverages[0].PremiumAmount / CoverageModel.DepositPremiumPercent) * 100), decimalQuantity); //2);
                        }
                        accumulatedPremium = CalculateAccumulatedPremium(previousCoverages);
                    }
                }
                else
                {
                    accumulatedPremium = 0;
                }
            }
            else
            {
                accumulatedPremium = 0;
            }

            decimal premiumDifference = premiumBase - accumulatedPremium;
            decimal delta = new decimal(0.01);

            if ((premiumDifference < delta) && (premiumDifference > (delta * (-1))))
            {
                premiumDifference = new decimal(0);
            }

            premiumDifference = decimal.Round(premiumDifference, decimalQuantity);//QuoteManager.DecimalRound);
            CoverageModel.PremiumAmount = premiumDifference;
        }

        /// <summary>
        /// Calcular Prima Acumulada
        /// </summary>
        /// <param name="coverages">Coberturas</param>
        /// <returns>Prima Acumulada</returns>
        private decimal CalculateAccumulatedPremium(List<Models.Coverage> coverages)
        {
            decimal premiumAmount = 0;

            foreach (Models.Coverage coverage in coverages)
            {
                DateTime maxFrom;
                DateTime minTo;

                if (coverage.CoverStatus != Enums.CoverageStatusType.NotModified)
                {
                    if (CoverageModel.CurrentFrom > coverage.CurrentTo.Value)
                    {
                        maxFrom = coverage.CurrentTo.Value;
                    }
                    else
                    {
                        if (CoverageModel.CurrentFrom < coverage.CurrentFrom)
                        {
                            maxFrom = coverage.CurrentFrom;
                        }
                        else
                        {
                            maxFrom = CoverageModel.CurrentFrom;
                        }
                    }

                    if (CoverageModel.CurrentTo < coverage.CurrentFrom)
                    {
                        minTo = coverage.CurrentFrom;
                    }
                    else
                    {
                        if (CoverageModel.CurrentTo > coverage.CurrentTo.Value)
                        {
                            minTo = coverage.CurrentTo.Value;
                        }
                        else
                        {
                            minTo = CoverageModel.CurrentTo != null ? CoverageModel.CurrentTo.Value : coverage.CurrentTo.Value;
                        }
                    }


                    int factorDays = CalculateEffectiveDays(maxFrom, minTo);
                    int days = CalculateEffectiveDays(coverage.CurrentFrom, coverage.CurrentTo);
                    premiumAmount += GetDifferenceAccumulatedPremium(coverage, days, factorDays);
                }
            }

            return premiumAmount;
        }

        /// <summary>
        /// Obtener Diferencia de Prima
        /// </summary>
        /// <param name="coverage">Cobertura</param>
        /// <param name="days">Dias a Prorrata</param>
        /// <param name="factorDays">Factor Dias</param>
        /// <returns>Prima</returns>
        private decimal GetDifferenceAccumulatedPremium(Models.Coverage coverage, int prorrateDays, int factorDays)
        {
            decimal diffMinPremiumAmount = coverage.DiffMinPremiumAmount.GetValueOrDefault();

            if (coverage.CalculationType == Services.UtilitiesServices.Enums.CalculationType.Direct && CoverageModel.CalculationType == coverage.CalculationType)
            {
                return coverage.PremiumAmount - diffMinPremiumAmount;
            }
            else if (coverage.CalculationType == Services.UtilitiesServices.Enums.CalculationType.Direct && CoverageModel.CalculationType != coverage.CalculationType)
            {
                return 0;
            }
            else
            {
                return Convert.ToDecimal((Convert.ToDouble(coverage.PremiumAmount - diffMinPremiumAmount) / prorrateDays) * factorDays);
            }
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
                if (!QuoteManager.LeapYear && CoverageModel.CoveredRiskType != (int)CoveredRiskType.Surety)
                {
                    int daysToRest = 0;

                    for (int year = currentFrom.Year; year <= currentTo.Value.Year; year++)
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
        /// Asignar Prima Cuando es Exclusión
        /// </summary>
        private void SetPremiumAmountZeroWhenClaimExlusion()
        {
            switch (CoverageModel.EndorsementType)
            {
                case EndorsementType.ClaimExclusion:
                    CoverageModel.PremiumAmount = 0;
                    break;
                case EndorsementType.DeclarationEndorsement:
                    if (CoverageModel.IsDeclarative && CoverageModel.IsMinPremiumDeposit)
                    {
                        CoverageModel.PremiumAmount = 0;
                    }

                    CoverageModel.EndorsementLimitAmount = CoverageModel.LimitAmount - CoverageModel.OriginalLimitAmount;
                    CoverageModel.EndorsementSublimitAmount = CoverageModel.SubLimitAmount - CoverageModel.OriginalSubLimitAmount;
                    break;
                case EndorsementType.Renewal:
                case EndorsementType.Emission:
                    if (CoverageModel.IsDeclarative && !CoverageModel.IsMinPremiumDeposit)
                    {
                        CoverageModel.PremiumAmount = 0;
                    }

                    CoverageModel.EndorsementLimitAmount = CoverageModel.LimitAmount;
                    CoverageModel.EndorsementSublimitAmount = CoverageModel.SubLimitAmount;
                    break;
                case EndorsementType.Modification:
                    if (CoverageModel.IsDeclarative && !CoverageModel.IsMinPremiumDeposit)
                    {
                        CoverageModel.PremiumAmount = 0;
                    }

                    if (CoverageModel.CoverStatus != CoverageStatusType.Excluded)
                    {
                        //if ((CoverageModel.LimitAmount - CoverageModel.OriginalLimitAmount) != 0) // Este cambio afecta el monto
                        //{
                        CoverageModel.EndorsementLimitAmount = CoverageModel.LimitAmount - CoverageModel.OriginalLimitAmount;
                        CoverageModel.EndorsementSublimitAmount = CoverageModel.SubLimitAmount - CoverageModel.OriginalSubLimitAmount;
                        //}
                        //else
                        //{
                        //    CoverageModel.EndorsementLimitAmount = CoverageModel.LimitAmount;
                        //    CoverageModel.EndorsementSublimitAmount = CoverageModel.SubLimitAmount;
                        //}

                        if ((CoverageModel.EndorsementSublimitAmount != 0 || CoverageModel.PremiumAmount != 0) && CoverageModel.CoverStatus != CoverageStatusType.Included)
                        {
                            CoverageModel.CoverStatus = CoverageStatusType.Modified;
                            CoverageModel.CoverStatusName = Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Modified));
                        }
                        //if (CoverageModel.PremiumAmount == 0)
                        //{
                        //    CoverageModel.EndorsementLimitAmount = CoverageModel.LimitAmount;
                        //    CoverageModel.EndorsementSublimitAmount = CoverageModel.SubLimitAmount;
                        //}
                    }
                    break;
            }
        }
    }
}