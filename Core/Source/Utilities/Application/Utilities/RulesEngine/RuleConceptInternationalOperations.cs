using Sistran.Core.Application.Utilities.Helper;
using System.Collections.Generic;

namespace Sistran.Core.Application.Utilities.RulesEngine
{
    public class RuleConceptInternationalOperations
    {
        public static int Id = int.Parse(EnumHelper.GetEnumParameterValue(Enums.FacadeType.RULE_FACADE_INTERNATIONAL_OPERATIONS).ToString());

        public static KeyValuePair<string, int> CityInternationalOperations => new KeyValuePair<string, int>("CityInternationalOperations", Id);

        public static KeyValuePair<string, int> CountryInternationalOperations => new KeyValuePair<string, int>("CountryInternationalOperations", Id);

        public static KeyValuePair<string, int> StateInternationalOperations => new KeyValuePair<string, int>("StateInternationalOperations", Id);

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
