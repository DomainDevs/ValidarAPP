using Sistran.Core.Application.UniquePersonListRiskApplicationServices.DTO;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Core.Application.UniquePersonListRiskApplicationServices
{
    [ServiceContract]
    public interface IUniquePersonListRiskApplicationServices
    {

        [OperationContract]
        List<ListRiskMatchDTO> ValidateListRiskPerson(string documentNumber, string fullName, int? riskListType);
    }
}
