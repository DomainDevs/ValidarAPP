using Sistran.Company.Application.Common.Entities;
using Sistran.Company.Application.ExternalProxyServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;

namespace Sistran.Company.Application.ExternalProxyServicesEEProvider.DAOs
{
    public class ExternalInformationLogDAO
    {
        public void RegisterExternalInformationLog(ExternalInformationLogDTO externalInformationLogDTO)
        {
            ExternalInformationLog externalInformationLog = Assemblers.EntityAssembler.CreateExternalInformationLog(externalInformationLogDTO);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(externalInformationLog);
        }
    }
}