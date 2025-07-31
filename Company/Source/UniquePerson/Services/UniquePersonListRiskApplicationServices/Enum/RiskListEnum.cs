using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonListRiskApplicationServices.Enum
{
    public enum RiskListEventEnum
    {
        INCLUDED = 1,
        EXCLUDED = 2
    }
    public enum RiskListTypeEnum
    {
        OWN = 1,
        OFAC = 2,
        ONU = 3
    }
    
    public class RiskListConstants
    {
        public static readonly string Included  = "INCLUDED";
        public static readonly string Excluded  = "EXCLUDED";
        public static readonly string OWN       = "PROPIA";
        public static readonly string ONU       = "ONU";
        public static readonly string OFAC      = "OFAC";
        public static readonly string SI        = "SÍ";
        public static readonly string NO        = "NO";
    } 
}
