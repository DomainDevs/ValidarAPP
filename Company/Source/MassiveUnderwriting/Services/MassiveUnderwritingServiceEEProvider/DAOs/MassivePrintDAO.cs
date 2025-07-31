using Sistran.Company.Application.PrintingServices.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveUnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniqueUserServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.MassiveUnderwritingServices.EEProvider.DAOs
{
    public class MassivePrintDAO
    {

        public string PrintMassiveLoad(int massiveLoadId, int rangeFrom, int rangeTo, User user, bool checkIssuedDetail)
        {
            List<CompanyFilterReport> companyFilterReports = new List<CompanyFilterReport>();
            string fileBytes = string.Empty;
            MassiveEmission massiveEmission = DelegateService.massiveUnderwritingService.GetMassiveEmissionByMassiveLoadId(massiveLoadId);
            massiveEmission.Rows = DelegateService.massiveUnderwritingService.GetMassiveEmissionRowsByMassiveLoadIdMassiveLoadProcessStatus(massiveLoadId, null, false, null);

            List<MassiveEmissionRow> LstMassiveRow = new List<MassiveEmissionRow>();
            if (massiveEmission.Rows.Count > 1)
            {
                for (int i = 0; i < massiveEmission.Rows.Count; i++)
                {
                    if (i >= (rangeFrom - 1) && i <= (rangeTo - 1))
                    {
                        LstMassiveRow.Add(massiveEmission.Rows[i]);
                    }
                }
                //LstMassiveRow = massiveEmission.Rows.Where(x => x.RowNumber >= rangeFrom && x.RowNumber <= rangeTo).ToList();
            }
            else
            {
                LstMassiveRow = massiveEmission.Rows;
            }
            for (int i = 0; i < LstMassiveRow.Count; i++)
            {
                CompanyFilterReport companyFilterReport = new CompanyFilterReport();
                companyFilterReport.Risks = new List<Risk>();
                companyFilterReport.Risks.Add(massiveEmission.Rows[i].Risk);
                companyFilterReport.Risks[0].Number = i;
                companyFilterReport.EndorsementId = massiveEmission.Rows[i].Risk.Policy.Endorsement.Id;
                companyFilterReport.User = new User();
                companyFilterReport.User.Name = user.Name;
                companyFilterReport.User.UserId = user.UserId;
                companyFilterReports.Add(companyFilterReport);
            }

            return DelegateService.printingService.GenerateReportMassive(companyFilterReports, massiveLoadId); //companyFilterReports, massiveLoadId, checkIssuedDetail

        }

    }
}
