using Sistran.Core.Application.AccountingServices.DTOs;
using System.ServiceModel;

namespace Sistran.Core.Application.AccountingServices
{
    [ServiceContract]
    public interface IAccountingReversionService
    {
        [OperationContract]
        bool ReversionPremiumByFilterApp(ReversionFilterDTO reversionFilterDTO);
        [OperationContract]
        bool ReversionTempPremium(ReversionFilterDTO reversionFilterDTO);      
    }
}
