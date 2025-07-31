using Sistran.Core.Application.OperationQuotaServices.DTOs;
using Sistran.Core.Application.OperationQuotaServices.DTOs.OperationQuota;
using System;
using System.Collections.Generic;
using System.ServiceModel;
namespace Sistran.Core.Application.OperationQuotaServices
{
    [ServiceContract]
    public interface IOperationQuotaService
    {
        /// <summary>
        /// Get Operating Quota Event By Individual Id By LineBusiness Id
        /// </summary>
        /// <param name="IndividualId"></param>
        /// <param name="LineBusinessId"></param>
        /// <returns></returns>
        [OperationContract]
        OperatingQuotaEventDTO GetOperatingQuotaEventByIndividualIdByLineBusinessId(int individualId, int lineBusinessId);

        /// <summary>
        /// Insert Operating Quota Event
        /// </summary>
        /// <param name="operatingQuotaEventDTOs"></param>
        /// <returns></returns>
        [OperationContract]
        List<OperatingQuotaEventDTO> InsertOperatingQuotaEvent(List<OperatingQuotaEventDTO> operatingQuotaEventDTOs);

        /// <summary>
        /// Get Cumulus Coverages By Individual
        /// </summary>
        /// <param name="filterOperationQuotaDTO"></param>
        /// <param name="enums"></param>
        /// <returns></returns>
        [OperationContract]
        List<OperatingQuotaEventDTO> GetCumulusCoveragesByIndividual(FilterOperationQuotaDTO filterOperationQuotaDTO, bool validatePriorityRetention = false);

        /// <summary>
        /// Obtiene la cantidad de decimales por producxto y moneda
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <param name="currencyId">The currency identifier.</param>
        /// <returns></returns>
        [OperationContract]
        int GetDecimalByProductIdCurrencyId(int productId, Int16 currencyId);

        /// <summary>
        /// Valida si se encuentra el afianzado en un Grupo Economico ,Consorcio o Individual
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        [OperationContract]
        TypeSecureDTO GetSecureType(int individualId, int lineBusinessId);

        /// <summary>
        /// Valida si se encuentra el afianzado con cupo
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        bool GetOperatingQuotaEventByIndividualId(int individualId);
    }
}
