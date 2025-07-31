using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.Enum
{
    public enum FuelType
    {
        [Description("No Aplica")] NoAplica = 1,
        [Description("DSL")] DSL = 2,
        [Description("ELT")] ELT = 3,
        [Description("GAS")] GAS = 4,
        [Description("GSL")] GSL = 5,


    }
        
}
