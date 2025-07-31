using System.Collections.Generic;
using Sistran.Core.Application.Utilities.Helper;

namespace Sistran.Core.Application.Utilities.RulesEngine
{
    public class RuleConceptPartners
    {
        public static int Id = int.Parse(EnumHelper.GetEnumParameterValue(Enums.FacadeType.RULE_FACADE_PARTNERS).ToString());

        public static KeyValuePair<string, int> IndividualIdShareholders => new KeyValuePair<string, int>("IndividualIdShareholders", Id);

        public static KeyValuePair<string, int> DocumentNumberShareholders => new KeyValuePair<string, int>("DocumentNumberShareholders", Id);

        public static KeyValuePair<string, int> NameShareholders => new KeyValuePair<string, int>("NameShareholders", Id);

        public static KeyValuePair<string, int> DocumentTypePartner => new KeyValuePair<string, int>("DocumentTypePartner", Id);

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
