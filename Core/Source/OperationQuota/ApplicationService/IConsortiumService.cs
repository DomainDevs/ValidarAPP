using Sistran.Core.Application.OperationQuotaServices.DTOs.Consortium;
using Sistran.Core.Application.OperationQuotaServices.DTOs.OperationQuota;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Core.Application.OperationQuotaServices
{
    [ServiceContract]
    public interface IConsortiumService
    {
        /// <summary>
        /// Consulta el Consorcio Cupos y Cumulos por ConsorcioId Y LineBusinessId
        /// </summary>
        /// <param name="consortiumId"></param>
        /// <param name="lineBusinessId"></param>
        /// <returns></returns>
        [OperationContract]
        OperatingQuotaEventDTO GetCumuloCupoConsortiumEventByConsortiumIdByLineBusinessId(int consortiumId, int lineBusinessId, bool? endorsement,int Id);
        /// <summary>
        /// Consulta el Consorciado Cupos y Cumulos por individualId Y LineBusinessId
        /// </summary>
        /// <param name="individualId"></param>
        /// <param name="lineBusinessId"></param>
        /// <returns></returns>
        [OperationContract]
        List<OperatingQuotaEventDTO> GetCumulusQuotaConsortiumByIndividualIdByLineBusinessId(int individualId, int lineBusinessId);

        [OperationContract]
        ConsortiumEventDTO CreateConsortiumEvent(ConsortiumEventDTO consortiumEventDTOs);

        [OperationContract]
        List<ConsortiumEventDTO> AssigendIndividualToConsotium(List<ConsortiumEventDTO> consortiumEventDTOs);

        [OperationContract]
        List<ConsortiumEventDTO> GetConsortiumEventByIndividualId(int individualId);

        [OperationContract]
        List<ConsortiumEventDTO> GetConsortiumExistsByConsortiumId(int consortiumId);

        [OperationContract]
        List<OperatingQuotaEventDTO> GetValidityParticipantCupoInConsortium(int consortiumId, long AmountInsured, int lineBusinessId);
    }
}
