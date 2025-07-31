using Sistran.Core.Application.UniquePersonListRiskBusinessService.Model;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Core.Application.UniquePersonListRiskBusinessService
{
    [ServiceContract]
    public interface IUniquePersonListRiskBusinessService
    {
        [OperationContract]
        void LoadOnMemoryListRisks(string userName);

        [OperationContract]
        void SendToRefreshOnMemoryListRisks(string userName);

        [OperationContract]
        List<RiskListMatch> ValidateListRiskPerson(string documentNumber, string fullName, int? riskListType);
    }
}
