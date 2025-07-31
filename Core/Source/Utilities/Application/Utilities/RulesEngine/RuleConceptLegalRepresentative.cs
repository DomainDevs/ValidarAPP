using System.Collections.Generic;
using Sistran.Core.Application.Utilities.Helper;

namespace Sistran.Core.Application.Utilities.RulesEngine
{
    public class RuleConceptLegalRepresentative
    {
        public static int Id = int.Parse(EnumHelper.GetEnumParameterValue(Enums.FacadeType.RULE_FACADE_LEGAL_REPRESENTATIVE).ToString());

        public static KeyValuePair<string, int> CountryLegalRepresentative => new KeyValuePair<string, int>("CountryLegalRepresentative", Id);

        public static KeyValuePair<string, int> StateLegalRepresentative => new KeyValuePair<string, int>("StateLegalRepresentative", Id);

        public static KeyValuePair<string, int> CityLegalRepresentative => new KeyValuePair<string, int>("CityLegalRepresentative", Id);

        public static KeyValuePair<string, int> DocumentTypeLegal => new KeyValuePair<string, int>("DocumentTypeLegal", Id);

        public static KeyValuePair<string, int> NameLegal => new KeyValuePair<string, int>("NameLegal", Id);

        public static KeyValuePair<string, int> DocumentNumberLegal => new KeyValuePair<string, int>("DocumentNumberLegal", Id);
        
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
