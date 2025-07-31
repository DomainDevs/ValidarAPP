using Sistran.Core.Integration.OperationQuotaServices.DTOs.Consortium;
using Sistran.Core.Integration.OperationQuotaServices.DTOs.OperationQuota;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.OperationQuotaServices
{
    [ServiceContract]
    public interface IConsortiumIntegrationService
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
        /// <summary>
        /// Valida los Consorciados Por Fecha y el Cupo Disponible VS el valor de la emision
        /// </summary>
        /// <param name="consortiumId"></param>
        /// <param name="AmountInsured"></param>
        /// <param name="lineBusinessId"></param>
        /// <returns></returns>
        [OperationContract]
        List<OperatingQuotaEventDTO> GetValidityParticipantCupoInConsortium(int consortiumId, long AmountInsured, int lineBusinessId);
        /// <summary>
        /// Creacion de Consorcio
        /// </summary>
        /// <param name="consortiumEventDTO"></param>
        /// <returns></returns>
        [OperationContract]
        ConsortiumEventDTO CreateConsortiumEvent(ConsortiumEventDTO consortiumEventDTO);
        /// <summary>
        /// Asignacion de consorciado a consorcio
        /// </summary>
        /// <param name="consortiumEventDTOs"></param>
        /// <returns></returns>
        [OperationContract]
        List<ConsortiumEventDTO> AssigendIndividualToConsotium(List<ConsortiumEventDTO> consortiumEventDTOs);
    }
}
