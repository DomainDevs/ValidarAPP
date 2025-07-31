using Sistran.Core.Application.Utilities.Helper;
using System.Collections.Generic;

namespace Sistran.Core.Application.Utilities.RulesEngine
{
    public class RuleConceptClaim
    {
        public static int Id = int.Parse(EnumHelper.GetEnumParameterValue(Enums.FacadeType.RULE_FACADE_CLAIM).ToString());

        public static KeyValuePair<string, int> ClaimId => new KeyValuePair<string, int>("ClaimId", Id);
        public static KeyValuePair<string, int> ClaimDate => new KeyValuePair<string, int>("ClaimDate", Id);
        public static KeyValuePair<string, int> JudicialDecisionDate => new KeyValuePair<string, int>("JudicialDecisionDate", Id);
        public static KeyValuePair<string, int> ClaimNoticeDate => new KeyValuePair<string, int>("ClaimNoticeDate", Id);
        public static KeyValuePair<string, int> RegistrationDate => new KeyValuePair<string, int>("RegistrationDate", Id);
        public static KeyValuePair<string, int> NoticeId => new KeyValuePair<string, int>("NoticeId", Id);
        public static KeyValuePair<string, int> PolicyId => new KeyValuePair<string, int>("PolicyId", Id);
        public static KeyValuePair<string, int> EndorsementId => new KeyValuePair<string, int>("EndorsementId", Id);
        public static KeyValuePair<string, int> ClaimEndorsementNumber => new KeyValuePair<string, int>("ClaimEndorsementNumber", Id);
        public static KeyValuePair<string, int> ClaimPolicyBusinessTypeId => new KeyValuePair<string, int>("ClaimPolicyBusinessTypeId", Id);
        public static KeyValuePair<string, int> ClaimPolicyTypeId => new KeyValuePair<string, int>("ClaimPolicyTypeId", Id);
        public static KeyValuePair<string, int> ClaimPolicyProductId => new KeyValuePair<string, int>("ClaimPolicyProductId", Id);
        public static KeyValuePair<string, int> ClaimNumber => new KeyValuePair<string, int>("ClaimNumber", Id);
        public static KeyValuePair<string, int> PrefixId => new KeyValuePair<string, int>("PrefixId", Id);
        public static KeyValuePair<string, int> ClaimBranchId => new KeyValuePair<string, int>("ClaimBranchId", Id);
        public static KeyValuePair<string, int> ClaimUserId => new KeyValuePair<string, int>("ClaimUserId", Id);
        public static KeyValuePair<string, int> ClaimUserProfile => new KeyValuePair<string, int>("ClaimUserProfile", Id);
        public static KeyValuePair<string, int> PolicyNumber => new KeyValuePair<string, int>("PolicyNumber", Id);
        public static KeyValuePair<string, int> ClaimCauseId => new KeyValuePair<string, int>("ClaimCauseId", Id);
        public static KeyValuePair<string, int> ClaimDetail => new KeyValuePair<string, int>("ClaimDetail", Id);
        public static KeyValuePair<string, int> ClaimDamageTypeId => new KeyValuePair<string, int>("ClaimDamageTypeId", Id);
        public static KeyValuePair<string, int> ClaimDamageResponsibilityId => new KeyValuePair<string, int>("ClaimDamageResponsibilityId", Id);
        public static KeyValuePair<string, int> ClaimOccurenceLocation => new KeyValuePair<string, int>("ClaimOccurenceLocation", Id);
        public static KeyValuePair<string, int> CountryId => new KeyValuePair<string, int>("CountryId", Id);
        public static KeyValuePair<string, int> StateId => new KeyValuePair<string, int>("StateId", Id);
        public static KeyValuePair<string, int> CityId => new KeyValuePair<string, int>("CityId", Id);
        public static KeyValuePair<string, int> InspectionDate => new KeyValuePair<string, int>("InspectionDate", Id);
        public static KeyValuePair<string, int> Analyst => new KeyValuePair<string, int>("Analyst", Id);
        public static KeyValuePair<string, int> Investigator => new KeyValuePair<string, int>("Investigator", Id);
        public static KeyValuePair<string, int> Ajuster => new KeyValuePair<string, int>("Ajuster", Id);
        public static KeyValuePair<string, int> AddressOfSinister => new KeyValuePair<string, int>("AddressOfSinister", Id);
        public static KeyValuePair<string, int> DescriptionOfSinister => new KeyValuePair<string, int>("DescriptionOfSinister", Id);
        public static KeyValuePair<string, int> CatastropheCurrentTo => new KeyValuePair<string, int>("CatastropheCurrentTo", Id);
        public static KeyValuePair<string, int> CatastropheCurrentFrom => new KeyValuePair<string, int>("CatastropheCurrentFrom", Id);
        public static KeyValuePair<string, int> Catastrophe => new KeyValuePair<string, int>("Catastrophe", Id);
        public static KeyValuePair<string, int> CatastropheCityId => new KeyValuePair<string, int>("CatastropheCityId", Id);
        public static KeyValuePair<string, int> CatastropheStateId => new KeyValuePair<string, int>("CatastropheStateId", Id);
        public static KeyValuePair<string, int> CatastropheCountryId => new KeyValuePair<string, int>("CatastropheCountryId", Id);
        public static KeyValuePair<string, int> EstimationWithDifferentCurrency => new KeyValuePair<string, int>("EstimationWithDifferentCurrency", Id);

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
