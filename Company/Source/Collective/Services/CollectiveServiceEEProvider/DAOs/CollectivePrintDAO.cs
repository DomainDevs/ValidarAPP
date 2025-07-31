using Sistran.Company.Application.PrintingServices.Models;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniqueUserServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Company.Application.CollectiveServices.EEProvider.DAOs
{
    public class CollectivePrintDAO
    {

        public string PrintCollectiveLoad(int massiveLoadId, int rangeFrom, int rangeTo, User user, bool checkIssuedDetail)
        {
            CollectiveEmission collectiveEmission = DelegateService.collectiveService.GetCollectiveEmissionByMassiveLoadId(massiveLoadId, true, false, null);

            collectiveEmission.Rows = collectiveEmission.Rows.Where(x => x.HasError == false || x.HasEvents == false).ToList();
            collectiveEmission.Rows = collectiveEmission.Rows.GetRange(rangeFrom - 1, (rangeTo - rangeFrom) + 1);

            CompanyFilterReport companyFilterReport = new CompanyFilterReport
            {
                User = user,
                TemporalId = collectiveEmission.TemporalId,
                Risks = collectiveEmission.Rows.Select(x => x.Risk).ToList(),
                EndorsementId = collectiveEmission.EndorsementId.GetValueOrDefault()
            };
            List<CompanyFilterReport> companyFilterReports = new List<CompanyFilterReport>();
            companyFilterReports.Add(companyFilterReport);
            companyFilterReport.Risks[0].Policy = new Core.Application.UnderwritingServices.Models.Policy
            {
                DocumentNumber = collectiveEmission.DocumentNumber.GetValueOrDefault()
            };

            //if (collectiveEmission.Status == MassiveLoadStatus.Issued)
            //{
            //    companyFilterReport.Risks = GetRisksByEndorsementIdRangeFromRangeTo(companyFilterReport.EndorsementId, rangeFrom, rangeTo);
            //}
            //else
            //{
            //    companyFilterReport.Risks = collectiveEmission.Rows.Select(x => x.Risk).ToList();
            //}
            //validacion de impresion massiva
            //return DelegateService.printingService.GenerateReportCollective(companyFilterReport, massiveLoadId/*, checkIssuedDetail*/);
            return DelegateService.printingService.GenerateReportMassive(companyFilterReports, massiveLoadId);
        }

        private List<Risk> GetRisksByEndorsementIdRangeFromRangeTo(int endorsementId, int rangeFrom, int rangeTo)
        {
            throw new NotImplementedException();
        }
    }
}