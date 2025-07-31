
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Assemblers;
using Sistran.Core.Integration.OperationQuotaServices;
using Sistran.Core.Integration.OperationQuotaServices.DTOs.Consortium;
using Sistran.Core.Integration.OperationQuotaServices.DTOs.OperationQuota;
using System.Collections.Generic;
namespace Sistran.Core.Application.OperationQuotaServices.EEProvider.IntegrationProviders
{
    public class ConsortiumIntegrationServiceEEProvider : IConsortiumIntegrationService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="consortiumId"></param>
        /// <param name="lineBusinessId"></param>
        /// <returns></returns>
        public OperatingQuotaEventDTO GetCumuloCupoConsortiumEventByConsortiumIdByLineBusinessId(int consortiumId, int lineBusinessId, bool? endorsement,int Id)
        {
            return DTOAssembler.CreateOperatingQuotaEventDTOs(DelegateService.consortiumService.GetCumuloCupoConsortiumEventByConsortiumIdByLineBusinessId(consortiumId, lineBusinessId, endorsement,Id));
        }
        /// <summary>
        /// Consulta el Consorciado Cupos y Cumulos por individualId Y LineBusinessId
        /// </summary>
        /// <param name="individualId"></param>
        /// <param name="lineBusinessId"></param>
        /// <returns></returns>
        public List<OperatingQuotaEventDTO> GetCumulusQuotaConsortiumByIndividualIdByLineBusinessId(int individualId, int lineBusinessId)
        {
            return DTOAssembler.CreateOperatingQuotaEventsDTOs(DelegateService.consortiumService.GetCumulusQuotaConsortiumByIndividualIdByLineBusinessId(individualId, lineBusinessId));
        }
        /// <summary>
        /// Valida los Consorciados Por Fecha y el Cupo Disponible VS el valor de la emision
        /// </summary>
        /// <param name="consortiumId"></param>
        /// <param name="AmountInsured"></param>
        /// <param name="lineBusinessId"></param>
        /// <returns></returns>
        public List<OperatingQuotaEventDTO> GetValidityParticipantCupoInConsortium(int consortiumId, long AmountInsured, int lineBusinessId)
        {
            return DTOAssembler.CreateOperatingQuotaEventsDTOs(DelegateService.consortiumService.GetValidityParticipantCupoInConsortium(consortiumId, AmountInsured, lineBusinessId));
        }
        /// <summary>
        /// Creacion del Consorcio
        /// </summary>
        /// <param name="consortiumEventDTO"></param>
        /// <returns></returns>
        public ConsortiumEventDTO CreateConsortiumEvent(ConsortiumEventDTO consortiumEventDTO)
        {
            return DTOAssembler.CreateConsortiumEventIntegrationDTO(DelegateService.consortiumService.CreateConsortiumEvent(DTOAssembler.CreateConsortiumEventDTO(consortiumEventDTO)));
        }

        public List<ConsortiumEventDTO> AssigendIndividualToConsotium(List<ConsortiumEventDTO> consortiumEventDTOs)
        {
            return DTOAssembler.CreateConsortiumsEventIntegrationDTO(DelegateService.consortiumService.AssigendIndividualToConsotium(DTOAssembler.CreateConsortiumsEventDTO(consortiumEventDTOs)));
        }
    }
}
