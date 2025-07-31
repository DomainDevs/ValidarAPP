using Sistran.Core.Application.Utilities.Helper;
using System.Collections.Generic;

namespace Sistran.Core.Application.Utilities.RulesEngine
{
    public class RuleConceptCoverage
    {
        public static int Id = int.Parse(EnumHelper.GetEnumParameterValue(Enums.FacadeType.RULE_FACADE_COVERAGE).ToString());

        public static KeyValuePair<string, int> LineBusinessCode => new KeyValuePair<string, int>("LineBusinessCode", Id);

        public static KeyValuePair<string, int> DeclaredAmount => new KeyValuePair<string, int>("DeclaredAmount", Id);

        public static KeyValuePair<string, int> LimitOccurrenceAmount => new KeyValuePair<string, int>("LimitOccurrenceAmount", Id);

        public static KeyValuePair<string, int> CurrentTo => new KeyValuePair<string, int>("CurrentTo", Id);

        public static KeyValuePair<string, int> CoverageId => new KeyValuePair<string, int>("CoverageId", Id);

        public static KeyValuePair<string, int> PremiumAmount => new KeyValuePair<string, int>("PremiumAmount", Id);

        public static KeyValuePair<string, int> AccumulatedSubLimitAmount => new KeyValuePair<string, int>("AccumulatedSubLimitAmount", Id);

        public static KeyValuePair<string, int> LimitAmount => new KeyValuePair<string, int>("LimitAmount", Id);

        public static KeyValuePair<string, int> EndorsementLimitAmount => new KeyValuePair<string, int>("EndorsementLimitAmount", Id);

        public static KeyValuePair<string, int> EndorsementSubLimitAmount => new KeyValuePair<string, int>("EndorsementSubLimitAmount", Id);

        public static KeyValuePair<string, int> MainCoverageId => new KeyValuePair<string, int>("MainCoverageId", Id);

        public static KeyValuePair<string, int> IsDeclarative => new KeyValuePair<string, int>("IsDeclarative", Id);

        public static KeyValuePair<string, int> Rate => new KeyValuePair<string, int>("Rate", Id);

        public static KeyValuePair<string, int> SubLimitAmount => new KeyValuePair<string, int>("SubLimitAmount", Id);

        public static KeyValuePair<string, int> ShortTermPercentage => new KeyValuePair<string, int>("ShortTermPercentage", Id);

        public static KeyValuePair<string, int> RateTypeCode => new KeyValuePair<string, int>("RateTypeCode", Id);

        public static KeyValuePair<string, int> MainCoveragePercentage => new KeyValuePair<string, int>("MainCoveragePercentage", Id);

        public static KeyValuePair<string, int> FirstRiskTypeCode => new KeyValuePair<string, int>("FirstRiskTypeCode", Id);

        public static KeyValuePair<string, int> ShortTermCode => new KeyValuePair<string, int>("ShortTermCode", Id);

        public static KeyValuePair<string, int> CalculationTypeCode => new KeyValuePair<string, int>("CalculationTypeCode", Id);

        public static KeyValuePair<string, int> LimitClaimantAmount => new KeyValuePair<string, int>("LimitClaimantAmount", Id);

        public static KeyValuePair<string, int> IsMinimumPremiumDeposit => new KeyValuePair<string, int>("IsMinimumPremiumDeposit", Id);

        public static KeyValuePair<string, int> LimitInExcess => new KeyValuePair<string, int>("LimitInExcess", Id);

        public static KeyValuePair<string, int> AccumulatedLimitAmount => new KeyValuePair<string, int>("AccumulatedLimitAmount", Id);

        public static KeyValuePair<string, int> CurrentFrom => new KeyValuePair<string, int>("CurrentFrom", Id);

        public static KeyValuePair<string, int> SubLineBusinessCode => new KeyValuePair<string, int>("SubLineBusinessCode", Id);

        public static KeyValuePair<string, int> IsPrimary => new KeyValuePair<string, int>("IsPrimary", Id);

        public static KeyValuePair<string, int> PerilCode => new KeyValuePair<string, int>("PerilCode", Id);

        public static KeyValuePair<string, int> ExpirationDate => new KeyValuePair<string, int>("ExpirationDate", Id);

        public static KeyValuePair<string, int> InsuredObjectId => new KeyValuePair<string, int>("InsuredObjectId", Id);

        public static KeyValuePair<string, int> SurchargeAmount => new KeyValuePair<string, int>("SurchargeAmount", Id);

        public static KeyValuePair<string, int> SurchargeRate => new KeyValuePair<string, int>("SurchargeRate", Id);

        public static KeyValuePair<string, int> SurchargeRateTypeCode => new KeyValuePair<string, int>("SurchargeRateTypeCode", Id);

        public static KeyValuePair<string, int> DiscountAmount => new KeyValuePair<string, int>("DiscountAmount", Id);

        public static KeyValuePair<string, int> DiscountRate => new KeyValuePair<string, int>("DiscountRate", Id);

        public static KeyValuePair<string, int> DiscountRateTypeCode => new KeyValuePair<string, int>("DiscountRateTypeCode", Id);

        public static KeyValuePair<string, int> ConditionText => new KeyValuePair<string, int>("ConditionText", Id);

        public static KeyValuePair<string, int> RiskCoverageId => new KeyValuePair<string, int>("RiskCoverageId", Id);

        public static KeyValuePair<string, int> CoverageNumber => new KeyValuePair<string, int>("CoverageNumber", Id);

        public static KeyValuePair<string, int> CoverageOriginalStatusCode => new KeyValuePair<string, int>("CoverageOriginalStatusCode", Id);

        public static KeyValuePair<string, int> RiskId => new KeyValuePair<string, int>("RiskId", Id);

        public static KeyValuePair<string, int> QuotationId => new KeyValuePair<string, int>("QuotationId", Id);

        public static KeyValuePair<string, int> EndorsementId => new KeyValuePair<string, int>("EndorsementId", Id);

        public static KeyValuePair<string, int> CoverageStatusCode => new KeyValuePair<string, int>("CoverageStatusCode", Id);

        public static KeyValuePair<string, int> DaysVigencyCoverage => new KeyValuePair<string, int>("DaysVigencyCoverage", Id);

        public static KeyValuePair<string, int> DeductRateTypeCode => new KeyValuePair<string, int>("DeductRateTypeCode", Id);

        public static KeyValuePair<string, int> DeductRate => new KeyValuePair<string, int>("DeductRate", Id);

        public static KeyValuePair<string, int> DeductPremiumAmount => new KeyValuePair<string, int>("DeductPremiumAmount", Id);

        public static KeyValuePair<string, int> DeductValue => new KeyValuePair<string, int>("DeductValue", Id);

        public static KeyValuePair<string, int> DeductUnitCode => new KeyValuePair<string, int>("DeductUnitCode", Id);

        public static KeyValuePair<string, int> DeductSubjectCode => new KeyValuePair<string, int>("DeductSubjectCode", Id);

        public static KeyValuePair<string, int> MinDeductValue => new KeyValuePair<string, int>("MinDeductValue", Id);

        public static KeyValuePair<string, int> MinDeductUnitCode => new KeyValuePair<string, int>("MinDeductUnitCode", Id);

        public static KeyValuePair<string, int> MinDeductSubjectCode => new KeyValuePair<string, int>("MinDeductSubjectCode", Id);

        public static KeyValuePair<string, int> MaxDeductValue => new KeyValuePair<string, int>("MaxDeductValue", Id);

        public static KeyValuePair<string, int> MaxDeductUnitCode => new KeyValuePair<string, int>("MaxDeductUnitCode", Id);

        public static KeyValuePair<string, int> MaxDeductSubjectCode => new KeyValuePair<string, int>("MaxDeductSubjectCode", Id);

        public static KeyValuePair<string, int> CurrencyCode => new KeyValuePair<string, int>("CurrencyCode", Id);

        public static KeyValuePair<string, int> AccDeductAmt => new KeyValuePair<string, int>("AccDeductAmt", Id);

        public static KeyValuePair<string, int> IsEnabledMinimumPremium => new KeyValuePair<string, int>("IsEnabledMinimumPremium", Id);

        public static KeyValuePair<string, int> EnabledMinimumPremiumAmount => new KeyValuePair<string, int>("EnabledMinimumPremiumAmount", Id);

        public static KeyValuePair<string, int> MaxLiabilityAmount => new KeyValuePair<string, int>("MaxLiabilityAmount", Id);

        public static KeyValuePair<string, int> ConditionTextId => new KeyValuePair<string, int>("ConditionTextId", Id);

        public static KeyValuePair<string, int> MinimumPremiumCoverage => new KeyValuePair<string, int>("MinimumPremiumCoverage", Id);

        public static KeyValuePair<string, int> ClausesAdd => new KeyValuePair<string, int>("ClausesAdd", Id);

        public static KeyValuePair<string, int> ClausesRemove => new KeyValuePair<string, int>("ClausesRemove", Id);

        public static KeyValuePair<string, int> DeductId => new KeyValuePair<string, int>("DeductId", Id);

        public static KeyValuePair<string, int> InsuredObjectAmount => new KeyValuePair<string, int>("InsuredObjectAmount", Id);

        public static KeyValuePair<int, int> DynamicConcept(int id)
        {
            return new KeyValuePair<int, int>(id, Id);
        }
        public static KeyValuePair<int, int> DynamicConcept(int id, int entityId)
        {
            return new KeyValuePair<int, int>(id, entityId);
        }
    }
}