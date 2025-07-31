using System.Collections.Generic;
using Sistran.Core.Application.Utilities.Helper;

namespace Sistran.Core.Application.Utilities.RulesEngine
{
    public class RuleConceptVoucher
    {
        public static int Id = int.Parse(EnumHelper.GetEnumParameterValue<Enums.FacadeType>(Enums.FacadeType.RULE_FACADE_VOUCHER).ToString());

        public static KeyValuePair<string, int> VoucherTypeId => new KeyValuePair<string, int>("VoucherTypeId", Id);
        public static KeyValuePair<string, int> VoucherDate => new KeyValuePair<string, int>("VoucherDate", Id);
        public static KeyValuePair<string, int> VoucherValue => new KeyValuePair<string, int>("VoucherValue", Id);
        public static KeyValuePair<string, int> VoucherCurrencyId => new KeyValuePair<string, int>("VoucherCurrencyId", Id);

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
