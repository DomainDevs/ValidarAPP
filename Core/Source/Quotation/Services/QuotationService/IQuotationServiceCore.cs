using Sistran.Core.Application.ModelServices.Models.Param;
using Sistran.Core.Application.QuotationServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Core.Application.QuotationServices
{
    /// <summary>
    /// IQuotationServiceCore.
    /// </summary>
    [ServiceContract]
    public interface IQuotationServiceCore
    {
        /// <summary>
        /// Obtener Cotización
        /// </summary>
        /// <param name="quotationId">Id Cotización</param>
        /// <param name="version">Versión</param>
        /// <returns>Cotización</returns>
        [OperationContract]
        List<Policy> GetPoliciesByQuotationIdVersionPrefixId(int quotationId, int version, int prefixId, int branchId);

        /// <summary>
        /// Crear Nueva Versión De Una Cotización
        /// </summary>
        /// <param name="quotationId">Id Cotización</param>
        /// <returns>Cotización</returns>
        [OperationContract]
        Policy CreateNewVersionQuotation(int operationId);

        /// <summary>
        /// Convertir Cotización en Temporal
        /// </summary>
        /// <param name="quotationId">Id Cotización</param>
        /// <param name="version">Versión</param>
        /// <returns>Temporal</returns>
        [OperationContract]
        Policy CreateTemporalFromQuotation(int operationId);

        /// <summary>
        /// Obtener Cotización de busqueda avanzada
        /// </summary>
        /// <param name="policy"></param>
        /// <returns>Cotizaciones</returns>
        [OperationContract]
        List<Policy> GetPoliciesByPolicy(Policy policy);

        /// <summary>
        /// Obtener Cotizacion de la Poliza
        /// </summary>
        /// <param name="quotationId">Numero de Cotizacion</param>
        /// <returns></returns>        
        [OperationContract]
        Policy GetPolicyByQuotationId(int quotationId, int prefixCd, int branchCd);

        #region TextPrecatalogued
        
        [OperationContract]
        List<ConditionTextModel> GetconditionTextPrecatalogued();

        [OperationContract]
        ExcelFileServiceModel GenerateFileToText(string fileName);

        [OperationContract]
        List<ConditionTextModel> GetConditionTexts(string nameConditionText); 
        #endregion
    }
}