using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.ProductParamService.Models
{
    using Sistran.Core.Application.ProductParamService.Models.Base;

    /// <summary>
    /// 
    /// </summary>
    public class CiaParamRiskType: BaseParamRiskType
    {
        public List<CiaParamCoverage> GroupCoverages { get; set; }
    }
}
