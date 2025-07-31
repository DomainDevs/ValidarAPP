using Sistran.Core.Application.Utilities.Helper;
using System.Collections.Generic;

namespace Sistran.Core.Application.Utilities.RulesEngine
{
    public class RuleConceptGeneralPerson
    {
        public static int Id = int.Parse(EnumHelper.GetEnumParameterValue(Enums.FacadeType.RULE_FACADE_GENERAL_PERSON).ToString());

        public static KeyValuePair<string, int> IndividualId => new KeyValuePair<string, int>("IndividualId", Id);

        public static KeyValuePair<string, int> Surname => new KeyValuePair<string, int>("Surname", Id);

        public static KeyValuePair<string, int> SecondSurname => new KeyValuePair<string, int>("SecondSurname", Id);

        public static KeyValuePair<string, int> IndividualType => new KeyValuePair<string, int>("IndividualType", Id);

        public static KeyValuePair<string, int> Names => new KeyValuePair<string, int>("Names", Id);

        public static KeyValuePair<string, int> BusinessName => new KeyValuePair<string, int>("BusinessName", Id);

        public static KeyValuePair<string, int> DocumentNumber => new KeyValuePair<string, int>("DocumentNumber", Id);

        public static KeyValuePair<string, int> DocumentType => new KeyValuePair<string, int>("DocumentType", Id);

        public static KeyValuePair<string, int> StatePerson => new KeyValuePair<string, int>("StatePerson", Id);

        public static KeyValuePair<string, int> CountryPerson => new KeyValuePair<string, int>("CountryPerson", Id);

        public static KeyValuePair<string, int> CityPerson => new KeyValuePair<string, int>("CityPerson", Id);

        public static KeyValuePair<string, int> AssociationTypePerson => new KeyValuePair<string, int>("AssociationTypePerson", Id);
        
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
