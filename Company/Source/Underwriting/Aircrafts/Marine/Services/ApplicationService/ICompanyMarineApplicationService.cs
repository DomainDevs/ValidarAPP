using Sistran.Company.Application.Marines.MarineApplicationService.DTOs;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.Marines.MarineApplicationService
{
    [ServiceContract]
    public interface ICompanyMarineApplicationService
    {
        /// <summary>
        /// Guardar Riesgo
        /// </summary>
        /// <param name="MarineDTO">Riesgo</param>
        /// <returns>Riesgo</returns>
        [OperationContract]
        MarineDTO SaveMarine(MarineDTO MarineDTO);
        
        /// <summary>
        /// Excluir coberturas de la poliza
        /// </summary>
        /// <param name="temporalId">Identificador temporal de la póliza</param>
        /// <param name="riskId">Identificador del riesgo</param>
        /// <param name="coverageId">Identificador del la cobertura</param>
        /// <returns>Modelo de cobertura</returns>
        [OperationContract]
        CoverageDTO ExcludeCoverage(int temporalId, int riskId, int coverageId);

        /// <summary>
        /// Obtiene la lista de beneficiarios, los busca por el nombre
        /// </summary>
        /// <param name="description">Texto a buscar</param>
        /// <param name="insuredSearchType">Tipo de búsqueda</param>
        /// <param name="custormerType">Tipo de cliente</param>
        /// <returns>Lista de beneficiarios</returns>
        [OperationContract] 
		List<CompanyBeneficiary> GetBeneficiariesByDescriptionInsuredSearchTypeCustomer(string description, InsuredSearchType insuredSearchType, CustomerType custormerType);
  
        /// <summary>
        /// Obtiene la lista de tipos de beneficiarios
        /// </summary>
        /// <returns>Listado de tipos de beneficiarios</returns>
        [OperationContract] 
		List<BeneficiaryTypeDTO> GetBeneficiaryTypes();

       

        /// <summary>
        /// Obtiene un listado de tipos de cálculo
        /// </summary>
        /// <returns>Listado de tipos de cálculo</returns>
        [OperationContract] 
		List<SelectObjectDTO> GetCalculationTypes();


        /// <summary>
        /// Obtiene un listado de ciudades a partir del identificador
        ///         del país y el identificador del estado
        /// </summary>
        /// <param name="countryId">Identificador de país</param>
        /// <param name="stateId">Identificador de estado</param>
        /// <returns>Listado de ciudades</returns>
        [OperationContract] 
		List<CityDTO> GetCitiesByContryIdStateId(int countryId, int stateId);

        /// <summary>
        /// Retorna un objeto de Marinee a partir del identificador del riesgo
        /// </summary>
        /// <param name="riskId">Identificador del riesgo</param>
        /// <returns>Un objeto de Marinee</returns>
        [OperationContract]
        MarineDTO GetMarineByRiskId(int riskId);

        /// <summary>
        /// Obtiene un listado de países
        /// </summary>
        /// <returns>Listado de países</returns>
        [OperationContract] 
		List<CountryDTO> GetCountries();
        
        /// <summary>
        /// Retorna el listado de coberturar a partir del objeto de seguro
        /// </summary>
        /// <param name="insuredObjectId">Identificador del objeto del seguro</param>
        /// <returns>Listado de coberturas</returns>
        [OperationContract]
        List<CoverageDTO> GetCoveragesByInsuredObjectId(int insuredObjectId);

        /// <summary>
        /// Obtiene una lista de coberturas a partir de un objeto de negocio, grupo de cobertura e
        ///         identificador del producto
        /// </summary>
        /// <param name="insuredObjectId">Identificador del objeto de seguro</param>
        /// <param name="groupCoverageId">Identificador del grupo de cobertura</param>
        /// <param name="productId">Identificador del producto</param>
        /// <returns>Cobertura</returns>
        [OperationContract]
        List<CoverageDTO> GetCoveragesByProductIdGroupCoverageIdInsuredObjectId(int productId, int groupCoverageId, int insuredObjectId);

           /// <summary>
        /// Obtiene una lista de deduibles a partir del identificador de la cobertura
        /// </summary>
        /// <param name="coverageId">Identificador de la cobertura</param>
        /// <returns>Listado de decibles</returns>
        [OperationContract] 
		List<DeductibleDTO> GetDeductiblesByCoverageId(int coverageId);

        /// <summary>
        /// Obtiene los grupos de cobertura a apartir del identificador
        ///         del producto
        /// </summary>
        /// <param name="prefixId"></param>
        /// <param name="productId">Identificador del producto</param>
        /// <returns>Listado de grupos de cobertura</returns>
        [OperationContract]
        List<GroupCoverageDTO> GetGroupCoveragesByPrefixIdProductId(int prefixId, int productId);

        /// <summary>
        /// Obtiene el listado de teléfonos, correos y direcciones asociadas
        ///         a un beneficiario a apartir de su identificador
        /// </summary>
        /// <param name="individualId">Identificador de la persona</param>
        /// <returns>Objeto que contiene teléfonos, correos y direcciones</returns>
        [OperationContract]
        NotificationAddressDTO GetNotificationAddressesByIndividualId(int individualId);

        /// <summary>
        /// Obtiene un listado de beneficiarios a partir de un texto de búsqueda
        /// </summary>
        /// <param name="description">Texto de búsqueda</param>
        /// <param name="insuredSearchType"></param>
        /// <param name="customerType"></param>
        /// <returns>Listado de beneficiaros</returns>
        [OperationContract] 
		List<IndividualDetailsDTO> GetInsuredsByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType customerType);

        /// <summary>
        /// Obtiene el listado de objetos de seguro
        /// </summary>
        /// <param name="productId">Identificador del producto</param>
        /// <param name="groupCoverageId">Identificador del grupo de cobertura</param>
        /// <returns>Listado de objetos de seguro</returns>
        [OperationContract]
        List<InsuredObjectDTO> GetInsuredObjectsByProductIdGroupCoverageId(int productId, int groupCoverageId, int prefixId);

        /// <summary>
        /// Obtener Tipo de poliza por ramo y código de tipo de póliza
        /// </summary>
        /// <param name="prefixId">Identificador del ramo</param>
        /// <param name="policyTypeCode">Identificador del tipo de poliza</param>
        /// <returns>Tipo de póliza</returns>
        [OperationContract]
        PolicyTypeDTO GetPolicyTypeByPolicyTypeIdPrefixId(int policyTypeId, int prefixId);

        /// <summary>
        /// Obtiene el listado de tipos de tasa
        /// </summary>
        /// <returns>Listado de tipos de tasa</returns>
        [OperationContract] 
		List<SelectObjectDTO> GetRateTypes();

        /// <summary>
        /// Obtiene el listado de departamentos a partir del identificador
        ///         del país
        /// </summary>
        /// <param name="countryId">Identificador del país</param>
        /// <returns>Listado de departamentos</returns>
        [OperationContract] 
		List<StateDTO> GetStatesByCountryId(int countryId);

        /// <summary>
        /// Obtieene el temporal para una póliza de tipo Marinees
        /// </summary>
        /// <param name="temporalId">Identificador del temporal</param>
        /// <returns>Póliza tipo Marinee</returns>
        [OperationContract] 
		List<MarineDTO> GetMarinesByTemporalId(int temporalId);
        
        /// <summary>
        /// Obtiene un listado de tipos de Marinee
        /// </summary>
        /// <returns>Listado de tipos de Marinee</returns>
        //[OperationContract]
        //List<MarineTypeDTO> GetMarineTypes();

        /// Calcula riesgo del Marinee
        /// </summary>
        /// <param name="MarineDTO">Marine</param>
        /// <param name="runRulesPre">reglas pre</param>
        /// <param name="runRulesPost">reglas pos</param>
        /// <returns>MarineDTO</returns>
        [OperationContract]
        MarineDTO QuotateMarine(MarineDTO MarineDTO, bool runRulesPre, bool runRulesPost);

        /// <summary>
        /// Tarifación de la cobertura
        /// </summary>
        /// <param name="coverage">Cobertura</param>
        /// <param name="policyId">Identificador de la póliza</param>
        /// <param name="riskId">Identificador del riesgo</param>
        /// <returns>Cobertura</returns>
        [OperationContract]
        CoverageDTO QuotateCoverage(CoverageDTO CoverageDTO, MarineDTO MarineDTO, bool runRulesPre, bool runRulesPost);
        /// <summary>
        /// Tarifacion de multiples coberturas
        /// </summary>
        /// <param name="coverages"></param>
        /// <param name="policyId"></param>
        /// <param name="riskId"></param>
        /// <returns></returns>
        [OperationContract]
        List<CoverageDTO> QuotateCoverages(List <CoverageDTO> CoverageDTO, MarineDTO MarineDTO, bool runRulesPre, bool runRulesPost);

        /// <summary>
        /// Excluir objetos en tranportes
        /// </summary>
        /// <param name="temporalId"></param>
        /// <param name="riskId"></param>
        /// <param name="riskMarineId"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        [OperationContract]
        MarineDTO ExcludeMarine(int temporalId, int riskId);

        /// <summary>
        /// Guarda los datos de los beneficiarios
        /// </summary>
        /// <param name="riskId"></param>
        /// <param name="beneficiariesDTOs"></param>
        /// <param name="isMasive"></param>
        /// <returns></returns>

        [OperationContract]
        List<CompanyBeneficiary> SaveBeneficiaries(int riskId, List<CompanyBeneficiary> beneficiariesDTOs, bool isMasive = true);

        /// <summary>
        /// Guarda los datos de los beneficiarios
        /// </summary>
        /// <param name="riskId"></param>
        /// <param name="beneficiariesDTO"></param>
        /// <returns></returns>
        //[OperationContract]
        //List<BeneficiaryDTO> SaveBeneficiaries(int riskId, List<BeneficiaryDTO> beneficiariesDTO);

        /// <summary>
        /// crea Texto del DTO
        /// </summary>
        /// <param name="TextDTO">Textos</param>
        /// <returns>TextDTO</returns>
        [OperationContract]
        TextDTO SaveText(int riskId, TextDTO textDTO);

        /// <summary>
        /// crea Clausulas del DTO
        /// </summary>
        /// <param name="ClauseDTO">Clausulas</param>
        /// <returns>ClauseDTO</returns>
        [OperationContract]
        List<ClauseDTO> SaveClauses(int temporalId,int riskId, List<ClauseDTO> clauseDTOs);

        /// <summary>
        /// Ejecuta las relgas de riego
        /// </summary>
        /// <param name="Marine">Objeto de riego-Marinee</param>
        /// <param name="ruleId">Identificador de la regla</param>
        /// <returns>Modelo riesgo-Marinee</returns>
        [OperationContract]
        MarineDTO RunRulesRisk(MarineDTO Marine, int ruleId);

        /// <summary>
        /// Guardar las coberturas 
        /// </summary>
        /// <param name="policyId">Póliza</param>
        /// <param name="riskId">Riesgo</param>
        /// <param name="coverages">Coberturas asociadas al riesgo</param>
        /// <param name="insuredObjectId">Id del objeto del seguro</param>
        /// <returns>Guarda las coberturas para el riesgo</returns>
        [OperationContract]
        MarineDTO SaveCoverages(int policyId, int riskId, List<CoverageDTO> coverages, int insuredObjectId);

        [OperationContract]
        EndorsementDTO CreateEndorsement(int temporalId);

        /// <summary>
        /// Lista los riesgos por póliza
        /// </summary>
        /// <param name="policyId">Póliza</param>
        [OperationContract]
        List<MarineDTO> GetMarinesByPolicyId(int policyId);
        /// <summary>
        /// listado de endosos por tipo de endoso
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="endorsementId"></param>
        /// <returns></returns>
        [OperationContract]
        List<EndorsementDTO> GetEndorsementByEndorsementTypeIdPolicyId(int endorsementTypeId, int policyId);
        [OperationContract]
        List<MarineDTO> GetCompanyMarinesByPolicyIdEndorsementId(int policyId, int endorsementId);
        [OperationContract]
        List<CoverageDTO> GetCoveragesByRiskId(int riskId, int temporalId);
        [OperationContract]
        EndorsementDTO GetTemporalEndorsementByPolicyId(int policyId);
        
        [OperationContract]
        List<CoverageDTO> GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(int policyId, int endorsementId, int riskId);

        [OperationContract]
        TextDTO GetTextsByRiskId(int riskId);

        /// <summary>
        /// Verifica si es año bisiesto
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        bool GetLeapYear();

        /// <summary>
        /// Eliminar el riesgo en la emision
        /// </summary>
        /// <param name="temporalId"></param>
        /// <param name="riskId"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteCompanyRisk(int temporalId, int riskId);
        /// <summary>
        /// Coberturas asociadas a un objeto del seguro perteneciente a un riesgo
        /// </summary>
        /// <param name="riskId"></param>
        /// <param name="insuredObjectId"></param>
        /// <returns></returns>
        [OperationContract]
        List<CoverageDTO> GetTemporalCoveragesByRiskIdInsuredObjectId(int riskId, int insuredObjectId);

        /// <summary>
        /// Se guarda un objeto del seguro nuevo, asociado a un riesgo
        /// </summary>
        /// <param name="riskId"></param>
        /// <param name="insuredObjectDTO"></param>
        /// <param name="tempId"></param>
        /// <param name="groupCoverageId"></param>
        /// <returns></returns>
        [OperationContract]
        bool SaveInsuredObject(int riskId, InsuredObjectDTO insuredObjectDTO, int tempId, int groupCoverageId);

        [OperationContract]
        List<UsePrefixDTO> GetUsesMarine(int prefixId);

    }
}