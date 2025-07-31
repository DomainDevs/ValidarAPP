using Sistran.Company.Application.WrapperServices.Models;
using Sistran.Core.Application.Cache.CacheBusinessService.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.WrapperServices
{
    [ServiceContract]
    [XmlSerializerFormat]
    public interface IWrapperService
    {
        /// <summary>
        /// Cotizar Póliza Autos
        /// </summary>
        /// <param name="requestQuoteVehicle">Datos Cotización</param>
        /// <returns>Cotización</returns>
        [OperationContract]
        QuoteVehicleResponse QuoteVehicle(QuoteVehicleRequest requestQuoteVehicle);

        /// <summary>
        /// Tarifar Póliza Autos
        /// </summary>
        /// <param name="quoteVehiclePolicyRequest">Datos temporal</param>
        /// <returns>Tarifación</returns>        
        [OperationContract]
        QuoteVehiclePolicyResponse Quotate(QuoteVehiclePolicyRequest quoteVehiclePolicyRequest);


        /// <summary>
        /// Persiste en temporales de cotización
        /// </summary>
        /// <param name="quoteVehiclePolicyRequest">lista de cotizaciones</param>
        /// <returns>bool: estado de la transacción</returns>
        [OperationContract]
        bool CreateQuotation(QuoteVehiclePolicyRequest quoteVehiclePolicyRequest);
      
        /// <summary>
        /// Emite poliza
        /// </summary>
        /// <param name="temporalId">id temporal json</param>
        /// <returns>documentNumber:numero de poliza</returns>
        [OperationContract]
        Policy CreatePolicy(int  temporalId);

        /// <summary>
        /// Tarifa cargue masivos
        /// </summary>
        /// <param name="massiveLoadId">id de cargue masivo</param>
        /// <returns>documentNumber:numero de poliza</returns>
        [OperationContract]
        string QuotateMassive(int massiveLoadId);

        /// <summary>
        /// Listado de maquinas con la lista de reglas actuales
        /// </summary>
        /// <returns>NodeRulessetEsatus</returns>
        [OperationContract]
        List<NodeRulesetStatus> GetNodeRulSet();

        /// <summary>
        /// Obtiene la lista las ultimas versiones Publicadas
        /// </summary>
        /// <returns>NodeRulessetEsatus</returns>
        [OperationContract]
        List<VersionHistory> GetVersionHistory(int count);

        /// <summary>
        /// Validación Estado Caché
        /// </summary>
        /// <returns>CacheStatus</returns>
        [OperationContract]
        CacheStatus GetCacheStatus();
        /// <summary>
        /// Ejecutar Recargar caché por Json
        /// </summary>
        /// <param name="HistoryVersionJson">Datos Version publicada</param>
        /// <returns></returns>
        [OperationContract]
        void loadCacheByJson(string HistoryVersionJson);

        /// <summary>
        /// Audits the data.
        /// </summary>
        /// <param name="body">The body.</param>
        [OperationContract]
        void AuditData(object body);

        [OperationContract]
        void ReloadListRisksCache(string userName);
    }
}