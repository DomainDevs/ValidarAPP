using Sistran.Company.Application.OperationQuotaServices.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AUTOQUOTA = Sistran.Company.Application.OperationQuotaServices.EEProvider.Models.OperationQuota;
using AQMOD = Sistran.Company.Application.OperationQuotaServices.EEProvider.Models.OperationQuota;

namespace Sistran.Company.Application.OperationQuotaServices.EEProvider.Utilities
{
    public class GetDatatables
    {
        public CommonDataTables GetcommonDataTables(AQMOD.AutomaticQuota automaticQuota)
        {
            CommonDataTables datatables = new CommonDataTables();

            datatables.dtAutomaticQuota = ModelAssemblers.GetDataTableAutomatic_Quota(automaticQuota);
            datatables.dtUtility = ModelAssemblers.GetDataTableUtility(automaticQuota);
            datatables.dtSummaryUtility = ModelAssemblers.GetDataTableUtilitySummary(automaticQuota);
            datatables.dtIndicator = ModelAssemblers.GetDataTableIndicator(automaticQuota);
            datatables.dtThird = ModelAssemblers.GetDataTableThird(automaticQuota);
            return datatables;

        }
    }
}
