using Sistran.Core.Application.Utilities.Helper;
using System.Collections.Generic;

namespace Sistran.Core.Application.Utilities.RulesEngine
{
    public class RuleConceptGeneral
    {
        public static int Id = int.Parse(EnumHelper.GetEnumParameterValue(Enums.FacadeType.RULE_FACADE_GENERAL).ToString());

        public static KeyValuePair<string, int> PrefixCode => new KeyValuePair<string, int>("PrefixCode", Id);

        public static KeyValuePair<string, int> ExchangeRate => new KeyValuePair<string, int>("ExchangeRate", Id);

        public static KeyValuePair<string, int> BranchCode => new KeyValuePair<string, int>("BranchCode", Id);

        public static KeyValuePair<string, int> SalePointCode => new KeyValuePair<string, int>("SalePointCode", Id);

        public static KeyValuePair<string, int> IssueDate => new KeyValuePair<string, int>("IssueDate", Id);

        public static KeyValuePair<string, int> AggregateAnnualLimitAmount => new KeyValuePair<string, int>("AggregateAnnualLimitAmount", Id);

        public static KeyValuePair<string, int> CurrentToCurrentTo => new KeyValuePair<string, int>("CurrentTo", Id);

        public static KeyValuePair<string, int> CustomerTypeCode => new KeyValuePair<string, int>("CustomerTypeCode", Id);

        public static KeyValuePair<string, int> PrintedDate => new KeyValuePair<string, int>("PrintedDate", Id);

        public static KeyValuePair<string, int> IsPolicyHolderBill => new KeyValuePair<string, int>("IsPolicyHolderBill", Id);

        public static KeyValuePair<string, int> BillingGroupCode => new KeyValuePair<string, int>("BillingGroupCode", Id);

        public static KeyValuePair<string, int> BeginDate => new KeyValuePair<string, int>("BeginDate", Id);

        public static KeyValuePair<string, int> CurrencyCode => new KeyValuePair<string, int>("CurrencyCode", Id);

        public static KeyValuePair<string, int> UserId => new KeyValuePair<string, int>("UserId", Id);

        public static KeyValuePair<string, int> BillingPeriodCode => new KeyValuePair<string, int>("BillingPeriodCode", Id);

        public static KeyValuePair<string, int> ProductId => new KeyValuePair<string, int>("ProductId", Id);

        public static KeyValuePair<string, int> BillingDate => new KeyValuePair<string, int>("BillingDate", Id);

        public static KeyValuePair<string, int> CurrentFrom => new KeyValuePair<string, int>("CurrentFrom", Id);

        public static KeyValuePair<string, int> CurrentTo => new KeyValuePair<string, int>("CurrentTo", Id);

        public static KeyValuePair<string, int> HasDetailedCommission => new KeyValuePair<string, int>("HasDetailedCommission", Id);

        public static KeyValuePair<string, int> AddDiscountCommPercentage => new KeyValuePair<string, int>("AddDiscountCommPercentage", Id);

        public static KeyValuePair<string, int> SurchargeCommissionPercentage => new KeyValuePair<string, int>("SurchargeCommissionPercentage", Id);

        public static KeyValuePair<string, int> DecrementCommisionAdjustFactorPercentage => new KeyValuePair<string, int>("DecrementCommisionAdjustFactorPercentage", Id);

        public static KeyValuePair<string, int> IncrementCommisionAdjustFactorPercentage => new KeyValuePair<string, int>("IncrementCommisionAdjustFactorPercentage", Id);

        public static KeyValuePair<string, int> ScriptId => new KeyValuePair<string, int>("ScriptId", Id);

        public static KeyValuePair<string, int> AdditionalCommissionPercentage => new KeyValuePair<string, int>("AdditionalCommissionPercentage", Id);

        public static KeyValuePair<string, int> StdDiscountCommPercentage => new KeyValuePair<string, int>("StdDiscountCommPercentage", Id);

        public static KeyValuePair<string, int> RuleSetId => new KeyValuePair<string, int>("RuleSetId", Id);

        public static KeyValuePair<string, int> StandardCommissionPercentage => new KeyValuePair<string, int>("StandardCommissionPercentage", Id);

        public static KeyValuePair<string, int> IsGreen => new KeyValuePair<string, int>("IsGreen", Id);

        public static KeyValuePair<string, int> PrefixTypeCode => new KeyValuePair<string, int>("PrefixTypeCode", Id);

        public static KeyValuePair<string, int> HolderBirthDate => new KeyValuePair<string, int>("HolderBirthDate", Id);

        public static KeyValuePair<string, int> HolderAge => new KeyValuePair<string, int>("HolderAge", Id);

        public static KeyValuePair<string, int> HolderGender => new KeyValuePair<string, int>("HolderGender", Id);

        public static KeyValuePair<string, int> ConditionText => new KeyValuePair<string, int>("ConditionText", Id);

        public static KeyValuePair<string, int> EndorsementId => new KeyValuePair<string, int>("EndorsementId", Id);

        public static KeyValuePair<string, int> DocumentNumber => new KeyValuePair<string, int>("DocumentNumber", Id);

        public static KeyValuePair<string, int> PolicyId => new KeyValuePair<string, int>("PolicyId", Id);

        public static KeyValuePair<string, int> QuotationVersion => new KeyValuePair<string, int>("QuotationVersion", Id);

        public static KeyValuePair<string, int> MailAddressId => new KeyValuePair<string, int>("MailAddressId", Id);

        public static KeyValuePair<string, int> QuotationId => new KeyValuePair<string, int>("QuotationId", Id);

        public static KeyValuePair<string, int> NextPolicyId => new KeyValuePair<string, int>("NextPolicyId", Id);

        public static KeyValuePair<string, int> PreviousPolicyId => new KeyValuePair<string, int>("PreviousPolicyId", Id);

        public static KeyValuePair<string, int> TempId => new KeyValuePair<string, int>("TempId", Id);

        public static KeyValuePair<string, int> EndorsementTypeCode => new KeyValuePair<string, int>("EndorsementTypeCode", Id);

        public static KeyValuePair<string, int> PrimnaryAgentAgencyId => new KeyValuePair<string, int>("PrimnaryAgentAgencyId", Id);

        public static KeyValuePair<string, int> PrimaryAgentId => new KeyValuePair<string, int>("PrimaryAgentId", Id);

        public static KeyValuePair<string, int> PrimaryAgentAnnotations => new KeyValuePair<string, int>("PrimaryAgentAnnotations", Id);

        public static KeyValuePair<string, int> PrimaryAgentCode => new KeyValuePair<string, int>("PrimaryAgentCode", Id);

        public static KeyValuePair<string, int> PrimaryAgentLicenseNumber => new KeyValuePair<string, int>("PrimaryAgentLicenseNumber", Id);

        public static KeyValuePair<string, int> PrimaryAgentLocker => new KeyValuePair<string, int>("PrimaryAgentLocker", Id);

        public static KeyValuePair<string, int> PrimaryAgentSalesChannelCode => new KeyValuePair<string, int>("PrimaryAgentSalesChannelCode", Id);

        public static KeyValuePair<string, int> PrimaryAgentLicenseDate => new KeyValuePair<string, int>("PrimaryAgentLicenseDate", Id);

        public static KeyValuePair<string, int> PrimaryAgentReferredBy => new KeyValuePair<string, int>("PrimaryAgentReferredBy", Id);

        public static KeyValuePair<string, int> PrimaryAgentCheckPayableTo => new KeyValuePair<string, int>("PrimaryAgentCheckPayableTo", Id);

        public static KeyValuePair<string, int> PrimaryAgentDeclinedDate => new KeyValuePair<string, int>("PrimaryAgentDeclinedDate", Id);

        public static KeyValuePair<string, int> PrimaryAgentTypeCode => new KeyValuePair<string, int>("PrimaryAgentTypeCode", Id);

        public static KeyValuePair<string, int> PrimaryAgentAccExecutiveIndId => new KeyValuePair<string, int>("PrimaryAgentAccExecutiveIndId", Id);

        public static KeyValuePair<string, int> PrimaryAgentGroupCode => new KeyValuePair<string, int>("PrimaryAgentGroupCode", Id);

        public static KeyValuePair<string, int> PrimaryAgentEnteredDate => new KeyValuePair<string, int>("PrimaryAgentEnteredDate", Id);

        public static KeyValuePair<string, int> PrimaryAgentDeclinedTypeCode => new KeyValuePair<string, int>("PrimaryAgentDeclinedTypeCode", Id);

        public static KeyValuePair<string, int> IsPrimary => new KeyValuePair<string, int>("IsPrimary", Id);

        public static KeyValuePair<string, int> IsOrganizerAgent => new KeyValuePair<string, int>("IsOrganizerAgent", Id);

        public static KeyValuePair<string, int> OrganizerAgentAgencyId => new KeyValuePair<string, int>("OrganizerAgentAgencyId", Id);

        public static KeyValuePair<string, int> OrganizerAgentId => new KeyValuePair<string, int>("OrganizerAgentId", Id);

        public static KeyValuePair<string, int> OrganizerAgentAnnotations => new KeyValuePair<string, int>("OrganizerAgentAnnotations", Id);

        public static KeyValuePair<string, int> OrganizerAgentAgentCode => new KeyValuePair<string, int>("OrganizerAgentAgentCode", Id);

        public static KeyValuePair<string, int> OrganizerAgentLicenseNumber => new KeyValuePair<string, int>("OrganizerAgentLicenseNumber", Id);

        public static KeyValuePair<string, int> OrganizerAgentLocker => new KeyValuePair<string, int>("OrganizerAgentLocker", Id);

        public static KeyValuePair<string, int> PrimaryAgentAgencyZoneCode => new KeyValuePair<string, int>("PrimaryAgentAgencyZoneCode", Id);

        public static KeyValuePair<string, int> DefaultPaymentScheduleId => new KeyValuePair<string, int>("DefaultPaymentScheduleId", Id);

        public static KeyValuePair<string, int> DefaultPaymentMethodCode => new KeyValuePair<string, int>("DefaultPaymentMethodCode", Id);

        public static KeyValuePair<string, int> PolicyNum1G => new KeyValuePair<string, int>("PolicyNum1G", Id);

        public static KeyValuePair<string, int> MainAgentSurchargeCommisionPercentage => new KeyValuePair<string, int>("MainAgentSurchargeCommisionPercentage", Id);

        public static KeyValuePair<string, int> IssueMonth => new KeyValuePair<string, int>("IssueMonth", Id);

        public static KeyValuePair<string, int> IssueYear => new KeyValuePair<string, int>("IssueYear", Id);

        public static KeyValuePair<string, int> DaysVigency => new KeyValuePair<string, int>("DaysVigency", Id);

        public static KeyValuePair<string, int> OrganizerAgentSalesChannelCode => new KeyValuePair<string, int>("OrganizerAgentSalesChannelCode", Id);

        public static KeyValuePair<string, int> OrganizerAgentLicenseDate => new KeyValuePair<string, int>("OrganizerAgentLicenseDate", Id);

        public static KeyValuePair<string, int> OrganizerAgentReferredBy => new KeyValuePair<string, int>("OrganizerAgentReferredBy", Id);

        public static KeyValuePair<string, int> OrganizerAgentCheckPayableTo => new KeyValuePair<string, int>("OrganizerAgentCheckPayableTo", Id);

        public static KeyValuePair<string, int> OrganizerAgentDeclinedDate => new KeyValuePair<string, int>("OrganizerAgentDeclinedDate", Id);

        public static KeyValuePair<string, int> OrganizerAgentTypeCode => new KeyValuePair<string, int>("OrganizerAgentTypeCode", Id);

        public static KeyValuePair<string, int> OrganizerAgentAccExecutiveIndId => new KeyValuePair<string, int>("OrganizerAgentAccExecutiveIndId", Id);

        public static KeyValuePair<string, int> OrganizerAgentGroupCode => new KeyValuePair<string, int>("OrganizerAgentGroupCode", Id);

        public static KeyValuePair<string, int> OrganizerAgentEnteredDate => new KeyValuePair<string, int>("OrganizerAgentEnteredDate", Id);

        public static KeyValuePair<string, int> OrganizerAgentDeclinedTypeCode => new KeyValuePair<string, int>("OrganizerAgentDeclinedTypeCode", Id);

        public static KeyValuePair<string, int> OrganizerAgentAgencyZoneCode => new KeyValuePair<string, int>("OrganizerAgentAgencyZoneCode", Id);

        public static KeyValuePair<string, int> PolicyTypeCode => new KeyValuePair<string, int>("PolicyTypeCode", Id);

        public static KeyValuePair<string, int> PaymentScheduleId => new KeyValuePair<string, int>("PaymentScheduleId", Id);

        public static KeyValuePair<string, int> BusinessTypeCode => new KeyValuePair<string, int>("BusinessTypeCode", Id);

        public static KeyValuePair<string, int> RequestId => new KeyValuePair<string, int>("RequestId", Id);

        public static KeyValuePair<string, int> RequestEndorsementId => new KeyValuePair<string, int>("RequestEndorsementId", Id);

        public static KeyValuePair<string, int> PolicyTypeViewCode => new KeyValuePair<string, int>("PolicyTypeViewCode", Id);

        public static KeyValuePair<string, int> HolderIdentificationDocument => new KeyValuePair<string, int>("HolderIdentificationDocument", Id);

        public static KeyValuePair<string, int> YearsVigency => new KeyValuePair<string, int>("YearsVigency", Id);

        public static KeyValuePair<string, int> DaysRetroactivityPosterity => new KeyValuePair<string, int>("DaysRetroactivityPosterity", Id);

        public static KeyValuePair<string, int> Risks => new KeyValuePair<string, int>("Risks", Id);

        public static KeyValuePair<string, int> RisksQuantity => new KeyValuePair<string, int>("RisksQuantity", Id);

        public static KeyValuePair<string, int> YearCurrentFrom => new KeyValuePair<string, int>("YearCurrentFrom", Id);

        public static KeyValuePair<string, int> TimeHour => new KeyValuePair<string, int>("TimeHour", Id);

        public static KeyValuePair<string, int> OperationId => new KeyValuePair<string, int>("OperationId", Id);

        public static KeyValuePair<string, int> AmountInsured => new KeyValuePair<string, int>("AmountInsured", Id);

        public static KeyValuePair<string, int> Premium => new KeyValuePair<string, int>("Premium", Id);

        public static KeyValuePair<string, int> Expenses => new KeyValuePair<string, int>("Expenses", Id);

        public static KeyValuePair<string, int> Taxes => new KeyValuePair<string, int>("TaxAmount", Id);

        public static KeyValuePair<string, int> FullPremium => new KeyValuePair<string, int>("FullPremium", Id);

        public static KeyValuePair<string, int> ConditionTextId => new KeyValuePair<string, int>("ConditionTextId", Id);

        public static KeyValuePair<string, int> ConditionTextObservations => new KeyValuePair<string, int>("ConditionTextObservations", Id);

        public static KeyValuePair<string, int> CalculateMinPremium => new KeyValuePair<string, int>("CalculateMinPremium", Id);

        public static KeyValuePair<string, int> PolicyHolderId => new KeyValuePair<string, int>("PolicyHolderId", Id);

        public static KeyValuePair<string, int> ClausesAdd => new KeyValuePair<string, int>("ClausesAdd", Id);

        public static KeyValuePair<string, int> ClausesRemove => new KeyValuePair<string, int>("ClausesRemove", Id);

        public static KeyValuePair<string, int> RiskStatusCount => new KeyValuePair<string, int>("RiskStatusCount", Id);
        public static KeyValuePair<string, int> TemporalTypeCode => new KeyValuePair<string, int>("TemporalTypeCode", Id);

        public static KeyValuePair<string, int> ProcessType => new KeyValuePair<string, int>("ProcessType", Id);

        public static KeyValuePair<string, int> TotalRisk => new KeyValuePair<string, int>("TotalRisk", Id);

        public static KeyValuePair<string, int> InsuredDocumentNumberOfTheBond => new KeyValuePair<string, int>("InsuredDocumentNumberOfTheBond", Id);
        public static KeyValuePair<string, int> InsuredNameOfTheBond => new KeyValuePair<string, int>("InsuredNameOfTheBond", Id);

        public static KeyValuePair<string, int> DocumentoNumberHolder => new KeyValuePair<string, int>("DocumentoNumberHolder", Id);
        public static KeyValuePair<string, int> NameHolder => new KeyValuePair<string, int>("NameHolder", Id);

        public static KeyValuePair<string, int> CoinsuranceExpensesCeded => new KeyValuePair<string, int>("CoinsuranceExpensesCeded", Id);

        public static KeyValuePair<string, int> CoinsuranceExpensesAccepted => new KeyValuePair<string, int>("CoinsuranceExpensesAccepted", Id);
        
        public static KeyValuePair<string, int> AgentType => new KeyValuePair<string, int>("AgentType", Id);
        public static KeyValuePair<string, int> AssociationType => new KeyValuePair<string, int>("AssociationType", Id);
        public static KeyValuePair<string, int> IndividualNumberHolder => new KeyValuePair<string, int>("IndividualNumberHolder", Id);

        public static KeyValuePair<string, int> TypeOfHolderDocument => new KeyValuePair<string, int>("TypeOfHolderDocument", Id);



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