using Sistran.Company.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sistran.Core.Application.UnderwritingServices.Models;
namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models
{
    public class ChangeCoinsuranceViewModel : EndorsementViewModel
    {
        public int Id { get; set; }

        public List<IssuanceAgency> Agencies { get; set; }

        public List<CompanyIssuanceCoInsuranceCompany> companyIssuanceCoInsuranceCompanies { get; set; }

        public decimal Premium { get; set; }

        public DateTime ChangeCoinsuranceFrom { get; set; }

        public int ProductId { get; set; }
    }
}