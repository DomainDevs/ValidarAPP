using Sistran.Core.Application.Utilities.Helper;
using System.Collections.Generic;

namespace Sistran.Core.Application.Utilities.RulesEngine
{
    public class RuleConceptGeneralSarlaft
    {
        public static int Id = int.Parse(EnumHelper.GetEnumParameterValue(Enums.FacadeType.RULE_FACADE_GENERAL_SARLAFT).ToString());

        public static KeyValuePair<string, int> IndividualIdSarlaft => new KeyValuePair<string, int>("IndividualIdSarlaft", Id);

        public static KeyValuePair<string, int> DocumentTypeSarlaft => new KeyValuePair<string, int>("DocumentTypeSarlaft", Id);

        public static KeyValuePair<string, int> DocumentNumberSarlaft => new KeyValuePair<string, int>("DocumentNumberSarlaft", Id);

        public static KeyValuePair<string, int> NamesBusinessNameSarlaft => new KeyValuePair<string, int>("NamesBusinessNameSarlaft", Id);

        public static KeyValuePair<string, int> CountrySarlaft => new KeyValuePair<string, int>("CountrySarlaft", Id);

        public static KeyValuePair<string, int> StateSarlaft => new KeyValuePair<string, int>("StateSarlaft", Id);

        public static KeyValuePair<string, int> CitySarlaft => new KeyValuePair<string, int>("CitySarlaft", Id);

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
