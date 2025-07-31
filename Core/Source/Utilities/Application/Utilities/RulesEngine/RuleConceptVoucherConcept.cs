using System.Collections.Generic;
using Sistran.Core.Application.Utilities.Helper;

namespace Sistran.Core.Application.Utilities.RulesEngine
{
    public class RuleConceptVoucherConcept
    {
        public static int Id = int.Parse(EnumHelper.GetEnumParameterValue(Enums.FacadeType.RULE_FACADE_VOUCHER_CONCEPT).ToString());

        public static KeyValuePair<string, int> PaymentConceptId => new KeyValuePair<string, int>("PaymentConceptId", Id);

        public static KeyValuePair<string, int> ConceptValue => new KeyValuePair<string, int>("ConceptValue", Id);

        public static KeyValuePair<string, int> ConceptTax => new KeyValuePair<string, int>("ConceptTax", Id);

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
