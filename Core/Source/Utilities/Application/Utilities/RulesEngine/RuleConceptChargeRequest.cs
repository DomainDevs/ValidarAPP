using System;
using System.Collections.Generic;
using Sistran.Core.Application.Utilities.Helper;

namespace Sistran.Core.Application.Utilities.RulesEngine
{
    public class RuleConceptChargeRequest
    {
        public static int Id = int.Parse(EnumHelper.GetEnumParameterValue(Enums.FacadeType.RULE_FACADE_CHARGE_REQUEST).ToString());

        public static KeyValuePair<string, int> TotalAmount => new KeyValuePair<string, int>("TotalAmount", Id);
        public static KeyValuePair<string, int> ChargeBranchId => new KeyValuePair<string, int>("ChargeBranchId", Id);
        public static KeyValuePair<string, int> PaymentSourceId => new KeyValuePair<string, int>("PaymentSourceId", Id);
        public static KeyValuePair<string, int> ChargeCreatedDate => new KeyValuePair<string, int>("ChargeCreatedDate", Id);
        public static KeyValuePair<string, int> AccountingDate => new KeyValuePair<string, int>("AccountingDate", Id);        
        public static KeyValuePair<string, int> ChargeMovementTypeId => new KeyValuePair<string, int>("ChargeMovementTypeId", Id);
        public static KeyValuePair<string, int> ChargeClaimPrefixId => new KeyValuePair<string, int>("ChargeClaimPrefixId", Id);                
        public static KeyValuePair<string, int> ChargeTo => new KeyValuePair<string, int>("ChargeTo", Id);
        public static KeyValuePair<string, int> ChargeIndividualId => new KeyValuePair<string, int>("ChargeIndividualId", Id);
        public static KeyValuePair<string, int> ChargeIndividualName => new KeyValuePair<string, int>("ChargeIndividualName", Id);
        public static KeyValuePair<string, int> ChargeIndividualDocumentNumber => new KeyValuePair<string, int>("ChargeIndividualDocumentNumber", Id);
        public static KeyValuePair<string, int> ChargeIndividualDocumentType => new KeyValuePair<string, int>("ChargeIndividualDocumentType", Id);
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
