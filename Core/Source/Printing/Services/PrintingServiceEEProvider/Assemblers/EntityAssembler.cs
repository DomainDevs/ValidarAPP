using Sistran.Core.Application.PrintingServices.Models;
using Sistran.Core.Application.Report.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Sistran.Core.Application.PrintingServicesEEProvider.Assemblers
{
    public static class EntityAssembler
    {
        #region printDigitalFirm
        public static LogPrintStatus CreateLogPrintStatus(LogPrintStatusDTO logPrintStatusDTO)
        {
            return new LogPrintStatus
            {
                Id = logPrintStatusDTO.Id,
                PolicyId = logPrintStatusDTO.PolicyId,
                EndorsementId = logPrintStatusDTO.EndorsementId,
                Observacion = logPrintStatusDTO.Observacion,
                StatusId = logPrintStatusDTO.StatusId,
                Date = logPrintStatusDTO.Date,
                UserName = logPrintStatusDTO.UserName,
                Url= logPrintStatusDTO.Url

            };
        }

        
            public static LogErrorPrint CreateLogErrorPrint(LogErrorPrintDTO errorPrintDTO)
        {
            return new LogErrorPrint
            {
               // Id = errorPrintDTO.Id,
                Description = errorPrintDTO.Description,
                DateError = errorPrintDTO.DateError,
                EndorsementId = errorPrintDTO.EndorsementId,
                PolicyId = errorPrintDTO.PolicyId
                

            };
        }
        #endregion
    }
}
