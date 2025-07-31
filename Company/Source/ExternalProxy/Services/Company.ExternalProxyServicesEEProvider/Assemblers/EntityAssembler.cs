using Sistran.Company.Application.Common.Entities;
using Sistran.Company.Application.ExternalProxyServices.Models;

namespace Sistran.Company.Application.ExternalProxyServicesEEProvider.Assemblers
{
    public static class EntityAssembler
    {
        #region ExternalInformationLog
        public static ExternalInformationLog CreateExternalInformationLog(ExternalInformationLogDTO externalInformationLogDTO)
        {
            return new ExternalInformationLog
            {
                Id = externalInformationLogDTO.Id,
                GuidProcess = externalInformationLogDTO.GuidProcess,
                SuccessInvoke = externalInformationLogDTO.SuccessInvoke,
                ServerClient = externalInformationLogDTO.ServerClient,
                ServiceMethod = externalInformationLogDTO.ServiceMethod,
                JsonRequestParams = externalInformationLogDTO.JsonRequestParams,
                ErrorMessage = externalInformationLogDTO.ErrorMessage,
                TotalTimeResponse = externalInformationLogDTO.TotalTimeResponse,
                LocalRequestDate = externalInformationLogDTO.LocalRequestDate
            };
        }
        #endregion
    }
}
