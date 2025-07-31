using Sistran.Core.Application.PrintingServices.Models;
using Sistran.Core.Application.Report.Entities;
using Sistran.Core.Framework.DAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.PrintingServicesEEProvider.Assemblers
{
    public class ModelAssembler
    {
        public static LogPrintStatusDTO CreateLogStatus(LogPrintStatus entityPrintStatus)
        {
            return new LogPrintStatusDTO
            {
                Id = entityPrintStatus.Id,
                PolicyId = entityPrintStatus.PolicyId,
                EndorsementId = entityPrintStatus.EndorsementId,
                Observacion = entityPrintStatus.Observacion,
                StatusId = entityPrintStatus.StatusId,
                Date = entityPrintStatus.Date,
                UserName = entityPrintStatus.UserName,
                Url = entityPrintStatus.Url
            };
        }

        public static List<LogPrintStatusDTO> CreateLogsStatus(BusinessCollection businessCollection)
        {
            List<LogPrintStatusDTO> listPrintStatus = new List<LogPrintStatusDTO>();

            foreach (LogPrintStatus entity in businessCollection)
            {
                listPrintStatus.Add(ModelAssembler.CreateLogStatus(entity));
            }

            return listPrintStatus;
        }
    }
}
