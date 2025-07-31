using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.OperationQuotaServices.EEProvider.Utilities
{
    public class CommonDataTables
    {
        public DataTable dtAutomaticQuota { get; set; }
        public DataTable dtUtility { get; set; }

        public DataTable dtSummaryUtility { get; set; }
        public DataTable dtIndicator { get; set; }
        public DataTable dtThird { get; set; }
    }
}
