using Sistran.Company.Application.Location.PropertyServices.Models;
using Sistran.Company.Application.QuotationServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Application.QuotationServices;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Services.UtilitiesServices.Models;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.QuotationServices
{
    /// <summary>
    /// 
    /// </summary>
    [ServiceContract]
    public interface IQuotationService : IQuotationServiceCore
    {
        /// <summary>
        /// Cotizar póliza autos
        /// </summary>
        /// <param name="Vehicle">Datos vehiculo</param>
        /// <returns>Cotización</returns>
        [OperationContract]
        CompanyVehicle QuoteVehicle(CompanyVehicle companyVehicle);

        /// <summary>
        /// Obtener cotización por identificador
        /// </summary>
        /// <param name="quotationId">Identificador</param>
        /// <returns>Cotización</returns>
        [OperationContract]
        CompanyVehicle GetCompanyVehicleByQuotationId(int quotationId);

        /// <summary>
        /// Obtiene la lista de Amparos
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Peril> GetPerils();
        /// <summary>
        /// Ontiene la lista de amparos de un ramo técnico
        /// </summary>
        /// <param name="lineBusinessId"></param>
        /// <returns></returns>
        [OperationContract]
        List<Peril> GetPerilsByLineBusinessId(int lineBusinessId);

        /// <summary>
        /// Obtiene los Amparos por descripcion larga
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        [OperationContract]
        List<Peril> GetPerilsByDescription(string description);


        [OperationContract]
        PerilLineBusiness GetPerilsByLineBusinessAssigned(int lineBusinessId);

        /// <summary>
        /// Genera archivo excel amparos
        /// </summary>
        /// <param name="peril"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [OperationContract]
        string GenerateFileToPeril(List<Peril> peril, string fileName);

        [OperationContract]
        List<CompanyPolicy> GetCompanyPoliciesByQuotationIdVersionPrefixId(int quotationId, int version, int prefixId, int branchId);

        /// <summary>
        /// Cotizar Riesgo De Hogar
        /// </summary>
        /// <param name="companyProperty">Riesgo</param>
        /// <returns>Riesgo</returns>
        [OperationContract]
        CompanyPropertyRisk QuoteProperty(CompanyPropertyRisk companyProperty);

        /// <summary>
        /// Obtener Riesgo De Hogar
        /// </summary>
        /// <param name="temporalId">Id Temporal</param>
        /// <returns>Riesgo</returns>
        [OperationContract]
        CompanyPropertyRisk GetCompanyPropertyRiskByTemporalId(int temporalId);

        [OperationContract]
        CompanyPropertyRisk GetCompanyPropertyByQuotationId(int quotationId);
        [OperationContract]
        List<CompanyPropertyRisk> GetCompanyPropertyByCompanyPolicy(CompanyPropertyRisk temporal);

        [OperationContract]
        List<TextPretacalogued> GetTextPretacaloguedDto();
       
        [OperationContract]
        void ExecuteOpertaionTextPrecatalogued(List<TextPretacalogued> textPretacalogueds);
        [OperationContract]
        CompanyPolicy CreateCompanyTemporalFromQuotation(int operationId);
        /// <summary>
        /// Consultar cotizador por persona (identificador)
        /// </summary>
        /// <param name="tempId"></param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyQuotationVehicleSearch> CompanyQuotationVehicleSearches(int tempId);
        /// <summary>
        /// Actualiza la fecha de impresión (Printed_Date)
        /// </summary>
        /// <param name="tempId"></param>
        [OperationContract]
        void UpdateCompanyPrintedDateByTempId(int tempId);
        UserAgency GetDefaultAgentByUserId(int idUser);

        [OperationContract]
        List<TextPretacalogued> GetTextPretacaloguedByDescription(string textPre);
    }
}