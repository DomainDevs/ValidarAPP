using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.UnderwritingUtilities
{
    public class CommonDataTables
    {
        public DataTable dtTempRisk { get; set; }
        public DataTable dtCOTempRisk { get; set; }
        public DataTable dtBeneficary { get; set; }
        public DataTable dtRiskPayer { get; set; }
        public DataTable dtRiskCoverage { get; set; }
        public DataTable dtRiskClause { get; set; }
        public DataTable dtClause { get; set; }
        public DataTable dtDeduct { get; set; }
        public DataTable dtCoverClause { get; set; }
        public DataTable dtDynamic { get; set; }
        public DataTable dtDynamicCoverage { get; set; }

    }
}
