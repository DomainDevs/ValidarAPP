using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListRiskMatchingProcess.Model
{
    public class CompanyListRiskModel
    {
        public List<CompanyListRisk> CompanyRiskListOwn { get; set; }
        public List<CompanyListRiskOfac> CompanyRiskListOfac { get; set; }
        public List<OnuPerson> CompanyRiskListOnu { get; set; }
    }
}
