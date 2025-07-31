using System.Collections.Generic;
using Sistran.Core.Application.Utilities.Helper;

namespace Sistran.Core.Application.Utilities.RulesEngine
{
    public class RuleConceptPaymentRequest
    {
        public static int Id = int.Parse(EnumHelper.GetEnumParameterValue(Enums.FacadeType.RULE_FACADE_PAYMENT_REQUEST).ToString());

        public static KeyValuePair<string, int> TotalAmount => new KeyValuePair<string, int>("TotalAmount", Id);
        public static KeyValuePair<string, int> PaymentBranchId => new KeyValuePair<string, int>("PaymentBranchId", Id);
        public static KeyValuePair<string, int> EstimatedDate => new KeyValuePair<string, int>("EstimatedDate", Id);
        public static KeyValuePair<string, int> PaymentSourceId => new KeyValuePair<string, int>("PaymentSourceId", Id);
        public static KeyValuePair<string, int> PaymentCreatedDate => new KeyValuePair<string, int>("PaymentCreatedDate", Id);
        public static KeyValuePair<string, int> PaymentDate => new KeyValuePair<string, int>("PaymentDate", Id);
        public static KeyValuePair<string, int> PaymentMethodId => new KeyValuePair<string, int>("PaymentMethodId", Id);
        public static KeyValuePair<string, int> PaymentCurrencyId => new KeyValuePair<string, int>("PaymentCurrencyId", Id);
        public static KeyValuePair<string, int> PaymentMovementTypeId => new KeyValuePair<string, int>("PaymentMovementTypeId", Id);
        public static KeyValuePair<string, int> PaymentClaimPrefixId => new KeyValuePair<string, int>("PaymentClaimPrefixId", Id);
        public static KeyValuePair<string, int> PaymentTypeId => new KeyValuePair<string, int>("PaymentTypeId", Id);
        public static KeyValuePair<string, int> DescriptionPaymentRequest => new KeyValuePair<string, int>("DescriptionPaymentRequest", Id);
        public static KeyValuePair<string, int> PayTo => new KeyValuePair<string, int>("PayTo", Id);
        public static KeyValuePair<string, int> PaymentIndividualId => new KeyValuePair<string, int>("PaymentIndividualId", Id);
        public static KeyValuePair<string, int> PaymentIndividualName => new KeyValuePair<string, int>("PaymentIndividualName", Id);
        public static KeyValuePair<string, int> PaymentIndividualDocumentNumber => new KeyValuePair<string, int>("PaymentIndividualDocumentNumber", Id);
        public static KeyValuePair<string, int> PaymentIndividualDocumentType => new KeyValuePair<string, int>("PaymentIndividualDocumentType", Id);
        public static KeyValuePair<string, int> ClaimCurrency => new KeyValuePair<string, int>("ClaimCurrency", Id);
        public static KeyValuePair<string, int> EstimationDate => new KeyValuePair<string, int>("EstimationDate", Id);
        public static KeyValuePair<string, int> EstimationAmount => new KeyValuePair<string, int>("EstimationAmount", Id);
        public static KeyValuePair<string, int> EstimationReason => new KeyValuePair<string, int>("EstimationReason", Id);
        public static KeyValuePair<string, int> EstimationStatus => new KeyValuePair<string, int>("EstimationStatus", Id);
        public static KeyValuePair<string, int> EstimationType => new KeyValuePair<string, int>("EstimationType", Id);
        public static KeyValuePair<string, int> Coverage => new KeyValuePair<string, int>("Coverage", Id);
        public static KeyValuePair<string, int> PolicyId => new KeyValuePair<string, int>("PolicyId", Id);
        public static KeyValuePair<string, int> PolicyNumber => new KeyValuePair<string, int>("PolicyNumber", Id);
        public static KeyValuePair<string, int> CreationDate => new KeyValuePair<string, int>("CreationDate", Id);
        public static KeyValuePair<string, int> ClaimNumber => new KeyValuePair<string, int>("ClaimNumber", Id);
        public static KeyValuePair<string, int> DaysAfterPayAndOccurrence => new KeyValuePair<string, int>("DaysAfterPayAndOccurrence", Id);
        public static KeyValuePair<string, int> PaymentRequestPolicyProductId => new KeyValuePair<string, int>("PaymentRequestPolicyProductId", Id);
        public static KeyValuePair<string, int> RiskId => new KeyValuePair<string, int>("RiskId", Id);
        public static KeyValuePair<string, int> PaymentIndividualAssociationType => new KeyValuePair<string, int>("PaymentIndividualAssociationType", Id);
        public static KeyValuePair<string, int> JudicialDecisionDate => new KeyValuePair<string, int>("JudicialDecisionDate", Id);
        public static KeyValuePair<string, int> PaymentClaimLineBusinessId => new KeyValuePair<string, int>("PaymentClaimLineBusinessId", Id);

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
