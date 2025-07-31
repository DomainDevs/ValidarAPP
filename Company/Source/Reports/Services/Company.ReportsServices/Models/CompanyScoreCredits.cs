using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.ReportsServices.Models
{
    public class CompanyScoreCredits
    {
        public List<CompanyScoreCredit> companyScoreCredit { get; set; }
        public CompanyScoreCreditValid companyScoreCreditValid { get; set; }
    }
}
