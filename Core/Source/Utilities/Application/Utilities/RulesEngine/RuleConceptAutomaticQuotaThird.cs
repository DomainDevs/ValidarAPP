using Sistran.Core.Application.Utilities.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.Utilities.RulesEngine
{
    public class RuleConceptAutomaticQuotaThird
    {
        public static int Id = int.Parse(EnumHelper.GetEnumParameterValue(Enums.FacadeType.RULE_FACADE_THIRD_AUTOMATIC_QUOTA).ToString());
        public static KeyValuePair<string, int> QueryCIFIN => new KeyValuePair<string, int>("QueryCIFIN", Id);
        public static KeyValuePair<string, int> PrincipalDebtor => new KeyValuePair<string, int>("PrincipalDebtor", Id);
        public static KeyValuePair<string, int> Codebtor => new KeyValuePair<string, int>("Codebtor", Id);
        public static KeyValuePair<string, int> TotalThird => new KeyValuePair<string, int>("TotalThird", Id);
        public static KeyValuePair<string, int> RiskCenterListQuota => new KeyValuePair<string, int>("RiskCenterListQuota", Id);
        public static KeyValuePair<string, int> RestrictiveList => new KeyValuePair<string, int>("RestrictiveList", Id);
        public static KeyValuePair<string, int> ConsultPromissory => new KeyValuePair<string, int>("ConsultPromissory", Id);
        public static KeyValuePair<string, int> SISCONCReport => new KeyValuePair<string, int>("SISCONCReport", Id);

    }
}
