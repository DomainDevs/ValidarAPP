
using Sistran.Company.Application.ProductServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.CommonService.Models.Base;
using Sistran.Core.Application.UniqueUserServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Models
{
    public class PolicyCombosDTO
    {
        public List<UserAgency> UserAgencies { get; set; }
        public List<Branch> Branches { get; set; }
        public List<SalePoint> SalePoints { get; set; }
        public List<BasePrefix> BasePrefixes { get; set; }
        public List<CompanyProduct> CompanyProducts { get; set; }
        public List<PolicyType> PolicyTypes { get; set; }
        public List<Currency> Currencies { get; set; }
        public List<CompanyJustificationSarlaft> CompanyJustificationSarlafts { get; set; }

    }
}