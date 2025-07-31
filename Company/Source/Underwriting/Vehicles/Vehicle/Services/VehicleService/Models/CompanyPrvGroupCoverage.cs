using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.Vehicles.VehicleServices.Models
{
    public class CompanyCiaGroupCoverage
    {

    
        public int CoverageId { get; set; }
     
        public int ProductId { get; set; }
        
        public int CoverGroupId { get; set; }
    
        public int IsPremiumMin { get; set; }
 
        public int NoCalculate { get; set; }
    }
}
