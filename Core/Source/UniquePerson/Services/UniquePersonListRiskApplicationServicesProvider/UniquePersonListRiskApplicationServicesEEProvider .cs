using Sistran.Core.Application.UniquePersonListRiskApplicationServices;
using Sistran.Core.Application.UniquePersonListRiskApplicationServices.DTO;
using Sistran.Core.Application.UniquePersonListRiskApplicationServicesProvider.Assembler;
using System.Collections.Generic;

namespace Sistran.Core.Application.UniquePersonListRiskApplicationServicesProvider
{
    public class UniquePersonListRiskApplicationServicesEEProvider : IUniquePersonListRiskApplicationServices
    {
        public List<ListRiskMatchDTO> ValidateListRiskPerson(string documentNumber, string fullName, int? riskListType)
        {
            return ModelAssembler.CreateListRiskMatchDTOList(Delegate.uniquePersonListRiskBusinessService.ValidateListRiskPerson(documentNumber, fullName, riskListType));
        }
    }
}
