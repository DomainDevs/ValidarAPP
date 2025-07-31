using System.Collections.Generic;

namespace Sistran.Company.Application.UniquePersonListRiskBusinessService.Model
{
    public class CompanyListRiskModel
    {
        public List<CompanyListRisk> CompanyRiskListOwn { get; set; }
        public List<CompanyListRiskOfac> CompanyRiskListOfac { get; set; }

        public List<CompanyListRiskOnu> CompanyListRiskOnu { get; set; }
    }
}