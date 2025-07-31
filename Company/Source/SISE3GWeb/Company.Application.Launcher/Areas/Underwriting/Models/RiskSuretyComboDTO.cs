using Sistran.Core.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Models
{
    public class RiskSuretyComboDTO
    {
        public List<SuretyContractType> SuretyContractTypes { get; set; }

        public List<SuretyContractCategories> SuretyContractCategories { get; set; }

        public List<GroupCoverage> GroupCoverages { get; set; }
    }
}