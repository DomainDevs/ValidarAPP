using System.Collections.Generic;
using Sistran.Core.Application.Utilities.Helper;

namespace Sistran.Core.Application.Utilities.RulesEngine
{
    public class RuleConceptBeneficiaries
    {
        public static int Id = int.Parse(EnumHelper.GetEnumParameterValue(Enums.FacadeType.RULE_FACADE_BENEFICIARIES).ToString());

        public static KeyValuePair<string, int> NameBeneficiary => new KeyValuePair<string, int>("NameBeneficiary", Id);

        public static KeyValuePair<string, int> DocumentTypeBeneficiary => new KeyValuePair<string, int>("DocumentTypeBeneficiary", Id);

        public static KeyValuePair<string, int> DocumentNumberBeneficiary => new KeyValuePair<string, int>("DocumentNumberBeneficiary", Id);

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
