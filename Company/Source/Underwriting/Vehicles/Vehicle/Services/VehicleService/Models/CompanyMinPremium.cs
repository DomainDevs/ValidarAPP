using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.Vehicles.VehicleServices.Models
{
    public class CompanyMinPremium
    {
      
        public decimal? SubsMinPremium { get; set; }
       
        public bool? CalculateProrate { get; set; }
        
        public int EndoTypeCode { get; set; }
    
        public decimal? GroupCoverage { get; set; }
        
        public int PrefixCode { get; set; }
        
        public int? ValidityType { get; set; }

        public int MinPremiumRelId { get; set; }

        public int? BranchCode { get; set; }

        public decimal? RiskMinPremium { get; set; }

        public decimal? ProductId { get; set; }

        public int CurrencyCode { get; set; }
    }
}
