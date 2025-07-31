using Sistran.Core.Application.CommonService.Models;
using System.Collections.Generic;
using System.ServiceModel;
using Sistran.Company.Application.MassiveServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Services.UtilitiesServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using System;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using EnumsCore = Sistran.Core.Application.UnderwritingServices.Enums;

namespace Sistran.Company.Application.MassiveServices
{
    [ServiceContract]
    [XmlSerializerFormat]
    public interface IMassiveService : Sistran.Core.Application.MassiveServices.IMassiveServiceCore
    {
        /// <summary>
        /// Guarda en base de datos una nueva solicitud agrupadora
        /// </summary>
        /// <param name="request"> Modelo de solicitud agrupadora </param>
        /// <param name="userId"> Identificador del usuario </param>
        /// <returns> Modelo de la solicitud agrupadora </returns>
        [OperationContract]
        CompanyRequest CreateCompanyRequest(CompanyRequest companyRequest);

        /// <summary>
        /// Guarda en base de datos una nueva solicitud agrupadora
        /// </summary>
        /// <param name="request"> Modelo de solicitud agrupadora </param>
        /// <param name="userId"> Identificador del usuario </param>
        /// <returns> Modelo de la solicitud agrupadora </returns>
        [OperationContract]
        Models.CompanyRequest SaveRenewalRequest(Models.CompanyRequest request, int userId);

        /// <summary>
        /// Buscar Solicitudes por código
        /// </summary>
        /// <param name="requestId">Codigo de solicitud</param>        
        /// <returns></returns>  
        [OperationContract]
        Models.CompanyRequest GetCoRequestByRequestId(int requestId);

        /// <summary>
        /// Obtener Ramos Comerciales Por Agente
        /// </summary>
        /// <param name="agentId">Id Agente</param>
        /// <returns>Ramos Comerciales</returns>
        [OperationContract]
        List<Prefix> GetPrefixesByAgentId(int agentId);

        /// <summary>
        /// Obtener los ramos comerciales activos para Masivos
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Prefix> GetPrefixesToMassive();

        /// <summary>
        /// Buscar los productos para un agente habilitados para solicitud agrupadora
        /// </summary>
        /// <param name="agentId">Identificador del agente</param>
        /// <param name="prefixId">Identificador del ramo</param>
        /// <param name="isRequest">Esta habilitado para solicitud agrupadora</param>
        /// <returns>Lista Model.Product</returns>        
        [OperationContract]
        List<Core.Application.ProductServices.Models.Product> GetProductsByAgentIdPrefixId(int agentId, int prefixId);

        /// <summary>
        /// Buscar los productos para un agente habilitados para colectivas
        /// </summary>
        /// <param name="agentId">Identificador del agente</param>
        /// <param name="prefixId">Identificador del ramo</param>
        /// <returns>Lista Model.Product</returns>        
        [OperationContract]
        List<Core.Application.ProductServices.Models.Product> GetCollectiveProductsByAgentIdPrefixId(int agentId, int prefixId);

        /// <summary>
        /// Obtener Solicitudes Agrupadoras Por Grupo Facturación, Id O Descripción Solicitud
        /// </summary>
        /// <param name="billingGroup">Id Grupo Facturación</param>
        /// <param name="description">Id O Descripción Solicitud</param>
        /// <returns>Solicitudes Agrupadoras</returns>
        [OperationContract]
        List<Models.CompanyRequest> GetCompanyRequestsByBillingGroupIdDescriptionRequestNumber(int billingGroup, string description, int? requestNumber);

        /// <summary>
        /// Buscar Solicitudes Por Id o Descripción
        /// </summary>
        /// <param name="description">Id o Descripción</param>
        /// <returns>Lista de Solicitudes</returns>
        [OperationContract]
        List<Models.CompanyRequest> GetCoRequestByDescription(string description);

        [OperationContract]
        List<CompanyIssuanceCoInsuranceCompany> GetCoRequestCoinsuranceByRequedIdByRequestEndorsementIdType(int requestId, int requestEndorsementId, BusinessType businessType);


        [OperationContract]
        CompanyRequestEndorsement GetCompanyRequestEndorsmentPolicyWithRequest(System.DateTime PolicyFrom, CompanyRequest companyRequest);

        [OperationContract]
        Holder CreateHolder(Row row, List<FilterIndividual> filtersIndividuals);

        [OperationContract]
        CompanyIssuanceInsured CreateInsured(Row row, Holder holder, List<FilterIndividual> filtersIndividuals);

        [OperationContract]
        CompanyBeneficiary CreateBeneficiary(Row row, CompanyIssuanceInsured insured, List<FilterIndividual> filtersIndividuals);

        [OperationContract]
        List<CompanyBeneficiary> CreateAdditionalBeneficiaries(Template beneficiariesTemplate, List<FilterIndividual> filtersIndividuals);

        [OperationContract]
        List<FilterIndividual> GetFilterIndividuals(int userId, int branchId, List<File> files, string templatePropertyName);

        [OperationContract]
        List<FilterIndividual> GetFilterIndividualsForCollective(Row policyRow, List<File> riskFiles, int userId, int branchId, string policyNumberPropertyName, string prefixIdPropertyname);

        [OperationContract]
        List<FilterIndividual> GetDataFilterIndividualRenewal(List<File> files, string templatePropertyName);

        [OperationContract]
        List<FilterIndividual> GetDataFilterIndividualRenewalWithPropertyNames(List<File> files, string templatePropertyName, string policyNumberPropertyName, string prefixIdPropertyName);

        [OperationContract]
        List<CompanyCoverage> CreateAdditionalCoverages(List<CompanyCoverage> Allcoverages, List<CompanyCoverage> Actualcoverages, List<Row> rows);

        [OperationContract]
        List<IssuanceAgency> CreateAdditionalAgencies(Template template, ref string errorAgencies);

        [OperationContract]
        List<CompanyCoverage> CreateDeductibles(List<CompanyCoverage> coverages, Template template);

        [OperationContract]
        List<CompanyIssuanceCoInsuranceCompany> CreateCoInsuranceAssigned(CompanyPolicy companyPolicy, Template template);

        [OperationContract]
        List<CompanyIssuanceCoInsuranceCompany> CreateCoInsuranceAccepted(CompanyPolicy companyPolicy, File file);

        [OperationContract]
        bool UpdateMassiveLoadStatusIfComplete(int massiveLoadId, bool changeStatus = true);

        [OperationContract]
        int GetpendingOperationIdByMassiveLoadIdRowId(int massiveLoadId, int rowId);

        #region reportes
        /// <summary>
        /// Obtiene la lista de campos de policy
        /// </summary>
        /// <param name="file"></param>
        /// <param name="companyPolicy"></param>
        /// <returns></returns>
        [OperationContract]
        List<Field> GetFields(string serializeFields, CompanyPolicy companyPolicy);

        [OperationContract]
        string CreateBeneficiaries(List<CompanyBeneficiary> beneficiaries);

        /// <summary>
        /// Cargar en cache listas comunes para reportes
        /// </summary>
        [OperationContract]
        void LoadReportCacheList();
        /// <summary>
        /// Obtener registros desde la cache
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hash"></param>
        /// <returns></returns>

        [OperationContract]
        object GetCacheList(string key, string hash);

        /// <summary>
        /// Liberar lista cache
        /// </summary>
        [OperationContract]
        void ClearCacheList();

        /// <summary>
        /// Crear clausulas
        /// </summary>
        /// <param name="clauses"></param>
        /// <returns></returns>
        [OperationContract]
        string CreateClauses(List<CompanyClause> clauses);

        /// <summary>
        /// Obtener company policy reportes
        /// </summary>
        /// <param name="massiveLoadStatus"></param>
        /// <param name="policy"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyPolicy GetCompanyPolicyByMassiveLoadStatusPolicy(MassiveLoadStatus massiveLoadStatus, Policy policy);

        /// <summary>
        /// Crear asegurado
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="mainInsured"></param>
        /// <returns></returns>
        [OperationContract]
        List<Field> FillInsuredFields(List<Field> fields, CompanyIssuanceInsured mainInsured);
        #endregion

        #region Cargue
        /// <summary>
        /// Obtener los datos del cargue por descripcion
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        [OperationContract(Name = "CompanyGetMassiveLoadsByDescription")]
        new List<MassiveLoad> GetMassiveLoadsByDescription(string description);

        #endregion


        [OperationContract]
        List<CompanyClause> GetClauses(Template templateClauses, EmissionLevel emissionLevel);

        [OperationContract]
        List<CompanyClause> GetClausesByCoverageId(Template templateClauses, int coverageId);

        #region solicitud
        /// <summary>
        /// Saves the company request temporal.
        /// </summary>
        /// <param name="policy">The policy.</param>
        /// <returns></returns>
        [OperationContract]
        CompanyPolicy SaveCompanyRequestTemporal(CompanyPolicy policy);
        #endregion

        [OperationContract]
        List<DynamicConcept> GetDynamicConceptsByTemplate(int? scriptId, Template templateScripts, ref string error);

        [OperationContract]
        string CompanyUpdateMassiveLoadAuthorization(string massiveLoadId, List<string> temporalId);
        [OperationContract]
        List<CompanyAccessory> GetAccesorysByTemplate(Template templateScripts, CompanyPolicy policy, CompanyVehicle companyVehicle, int coverIdAccesoryNoORig, int coverIdAccesoryORig, ref string error);
        [OperationContract]
        void GetClausesByTemplate(Template templateScripts, ref List<CompanyClause> companyClauses, ref List<CompanyCoverage> companyCoverages, List<CompanyClause> riskClauses, List<CompanyClause> coverageClause, ref string error);

        [OperationContract]
        List<CompanyBeneficiary> GetBeneficiariesAdditional(File file, Template template, List<FilterIndividual> filterIndividuals, List<CompanyBeneficiary> companyBeneficiarie, ref string error);

        [OperationContract]
        List<Row> GetMassivePlatesValidation(List<Row> rows);

        [OperationContract]
        List<IssuanceAgency> GetAgenciesValidation(File file, List<IssuanceAgency> issuanceAgency, ref string error);
        [OperationContract]
        List<CompanyClause> GetClausesObligatory(EnumsCore.EmissionLevel emissionLevel, int prefixId, int? conditionLevel);

        [OperationContract]
        bool GetMassiveLoadErrorStatus(int massiveLoadId);
    }
}