using Sistran.Company.Application.PrintingServices.Models;
using Sistran.Core.Application.MassiveRenewalServices.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveUnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniqueUserServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.MassiveRenewalServices.EEProvider.DAOs
{
    public class RenewalPrintDAO
    {

        public string PrintRenewalLoad(int massiveLoadId, int rangeFrom, int rangeTo, User user, bool checkIssuedDetail)
        {
            int number = 0;
            List<CompanyFilterReport> companyFilterReports = new List<CompanyFilterReport>();
            MassiveRenewal massiveRenewal = DelegateService.massiveRenewalService.GetMassiveRenewalByMassiveRenewalId(massiveLoadId, true, false, null);

            massiveRenewal.Rows = massiveRenewal.Rows.GetRange(rangeFrom - 1, (rangeTo - rangeFrom) + 1);

            foreach (MassiveRenewalRow massiveRenewalRow in massiveRenewal.Rows)
            {
                CompanyFilterReport companyFilterReport = new CompanyFilterReport();
                companyFilterReport.Risks = new List<Risk>();

                massiveRenewalRow.Risk.Number = ++number;
                companyFilterReport.Risks.Add(massiveRenewalRow.Risk);

                companyFilterReport.TemporalId = massiveRenewalRow.TemporalId.Value;
                companyFilterReport.EndorsementId = massiveRenewalRow.Risk.Policy.Endorsement.Id;
                companyFilterReport.User = user;

                companyFilterReports.Add(companyFilterReport);
            }
            
            return DelegateService.printingService.GenerateReportMassive(companyFilterReports, massiveLoadId);
        }
    }
}