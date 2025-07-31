using System.Collections.Generic;
using Sistran.Core.Application.Utilities.Helper;

namespace Sistran.Core.Application.Utilities.RulesEngine
{
    public class RuleConceptEstimation
    {
        public static int Id = int.Parse(EnumHelper.GetEnumParameterValue(Enums.FacadeType.RULE_FACADE_ESTIMATION).ToString());

        public static KeyValuePair<string, int> ClaimCoverageId => new KeyValuePair<string, int>("ClaimCoverageId", Id);
        public static KeyValuePair<string, int> SubClaimId => new KeyValuePair<string, int>("SubClaimId", Id);
        public static KeyValuePair<string, int> RiskNumber => new KeyValuePair<string, int>("RiskNumber", Id);
        public static KeyValuePair<string, int> CoverageNumber => new KeyValuePair<string, int>("CoverageNumber", Id);
        public static KeyValuePair<string, int> CoverageInsuredAmount => new KeyValuePair<string, int>("CoverageInsuredAmount", Id);
        public static KeyValuePair<string, int> RiskId => new KeyValuePair<string, int>("RiskId", Id);
        public static KeyValuePair<string, int> CoverageId => new KeyValuePair<string, int>("CoverageId", Id);
        public static KeyValuePair<string, int> AffectedIsInsured => new KeyValuePair<string, int>("AffectedIsInsured", Id);
        public static KeyValuePair<string, int> AffectedIsProspect => new KeyValuePair<string, int>("AffectedIsProspect", Id);
        public static KeyValuePair<string, int> EstimationTypeId => new KeyValuePair<string, int>("EstimationTypeId", Id);
        public static KeyValuePair<string, int> EstimationStatusId => new KeyValuePair<string, int>("EstimationStatusId", Id);
        public static KeyValuePair<string, int> EstimationReasonId => new KeyValuePair<string, int>("EstimationReasonId", Id);
        public static KeyValuePair<string, int> EstimationAmount => new KeyValuePair<string, int>("EstimationAmount", Id);
        public static KeyValuePair<string, int> DeductibleAmount => new KeyValuePair<string, int>("DeductibleAmount", Id);
        public static KeyValuePair<string, int> EstimationDate => new KeyValuePair<string, int>("EstimationDate", Id);
        public static KeyValuePair<string, int> CurrencyId => new KeyValuePair<string, int>("CurrencyId", Id);
        public static KeyValuePair<string, int> EstimationAmountAccumulated => new KeyValuePair<string, int>("EstimationAmountAccumulated", Id);
        public static KeyValuePair<string, int> AffectedId => new KeyValuePair<string, int>("AffectedId", Id);
        public static KeyValuePair<string, int> AffectedName => new KeyValuePair<string, int>("AffectedName", Id);
        public static KeyValuePair<string, int> AffectedDocumentNumber => new KeyValuePair<string, int>("AffectedDocumentNumber", Id);
        public static KeyValuePair<string, int> AffectedDocumentType => new KeyValuePair<string, int>("AffectedDocumentType", Id);
        public static KeyValuePair<string, int> ClaimedAmount => new KeyValuePair<string, int>("ClaimedAmount", Id);
        public static KeyValuePair<string, int> IsClaimedAmount => new KeyValuePair<string, int>("IsClaimedAmount", Id);

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
