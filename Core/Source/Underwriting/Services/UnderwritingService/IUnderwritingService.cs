using System;
using System.Collections.Generic;
using System.ServiceModel;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.DTOs;
using Sistran.Core.Application.UnderwritingServices.DTOs.Filter;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models.Base;
using Sistran.Core.Services.UtilitiesServices.Enums;
using CommonModel = Sistran.Core.Application.CommonService.Models;
using Model = Sistran.Core.Application.UnderwritingServices.Models;
using MODPA = Sistran.Core.Application.ModelServices.Models.Param;
using ProductModel = Sistran.Core.Application.ProductServices.Models;

namespace Sistran.Core.Application.UnderwritingServices
{
    [ServiceContract]
    public interface IUnderwritingServiceCore
    {

        /// <summary>
        /// Obtener tomadores por Id, Documento o Descripción
        /// </summary>
        /// <param name="description">Parametro de Busqueda</param>
        /// <param name="insuredSearchType">Tipo de Busqueda</param>
        /// <param name="customerType">Tipo de Cliente</param>
        /// <returns>Tomadores</returns>
        [OperationContract]
        List<Model.Holder> GetHoldersByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType);

        /// <summary>
        /// Obtener tomadores por Id, Documento o Descripción
        /// </summary>
        /// <param name="description">Parametro de Busqueda</param>
        /// <param name="insuredSearchType">Tipo de Busqueda</param>
        /// <param name="customerType">Tipo de Cliente</param>
        /// <returns>Tomadores</returns>
        [OperationContract]
        List<IssuanceInsured> GetInsuredsByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType);
        /// <summary>
        /// Obtener Clausulas
        /// </summary>
        /// <param name="emissionLevel">Nivel De Emisión</param>
        /// <param name="conditionLevelId">Id Condición De Nivel</param>
        /// <returns>Clausulas</returns>
        [OperationContract]
        List<Clause> GetClausesByEmissionLevelConditionLevelId(UnderwritingServices.Enums.EmissionLevel emissionLevel, int conditionLevelId);

        /// <summary>
        /// Obtener Clausulas
        /// </summary>
        /// <param name="emissionLevel">Nivel De Emisión</param>
        /// <returns>Clausulas</returns>
        [OperationContract]
        List<Model.Clause> GetClausesByEmissionLevel(UnderwritingServices.Enums.EmissionLevel emissionLevel);

        /// <summary>
        /// Obtener Clausulas
        /// </summary>
        /// <param name="policyid">Nivel De Emisión</param>
        /// <returns>Clausulas</returns>
        [OperationContract]
        List<Model.Clause> GetClausesByEmissionPolicyId(int policyid);

        /// <summary>
        /// Obtener textos precatalogados
        /// </summary>
        /// <param name="name">nombre</param>
        /// <param name="levelId">Id nivel</param>
        /// <param name="conditionalLevelId">Id condición</param>
        /// <returns>Lista de textos</returns>
        [OperationContract]
        List<Model.Text> GetTextsByNameLevelIdConditionLevelId(string name, int levelId, int conditionLevelId);

        /// <summary>
        /// Obtener lista de beneficiarios
        /// </summary>
        /// <param name="description">Id o nombre o razón social</param>
        /// <returns></returns>
        [OperationContract]
        List<Beneficiary> GetBeneficiariesByDescription(string description, InsuredSearchType insuredSearchType, CustomerType? customerType = CustomerType.Individual);

        /// <summary>
        /// Obtener lista de limites RC
        /// </summary>
        /// <param name="prefixId">Id ramo comercial</param>
        /// <param name="productId">Id producto</param>
        /// <param name="policyTypeId">Id tipo de poliza</param>
        /// <returns></returns>
        [OperationContract]
        List<LimitRc> GetLimitsRcByPrefixIdProductIdPolicyTypeId(int prefixId, int productId, int policyTypeId);

        /// <summary>
        /// Obtener lista de grupo de coberturas
        /// </summary>
        /// <param name="productId">Id producto</param>
        /// <returns></returns>
        [OperationContract]
        List<GroupCoverage> GetGroupCoverages(int productId);

        /// <summary>
        /// Obtener lista de planes de pago
        /// </summary>
        /// <param name="description">Identificador producto</param>
        /// <returns></returns>
        [OperationContract]
        List<PaymentPlan> GetPaymentPlansByProductId(int productId);

        /// <summary>
        /// Obtener Coberturas por Producto, Grupo de Coberturas y Ramo
        /// </summary>
        /// <param name="productId">Id Producto</param>
        /// <param name="groupCoverageId">Id Grupo Cobertura</param>
        /// <param name="prefixId">Id Ramo Comercial</param>
        /// <returns>Coberturas</returns>
        [OperationContract]
        List<Coverage> GetCoveragesByProductIdGroupCoverageIdPrefixId(int productId, int groupCoverageId, int prefixId);

        /// <summary>
        /// Obtener cobertura por Id
        /// </summary>
        /// <param name="coverageId">Id cobertura</param>
        /// <returns>Cobertura</returns>
        [OperationContract]
        Coverage GetCoverageByCoverageIdProductIdGroupCoverageId(int coverageId, int productId, int groupCoverageId);

        /// <summary>
        /// Obtener cobertura por lista de Ids
        /// </summary>
        /// <param name="coverageId">Id cobertura</param>
        /// <returns>Cobertura</returns>
        [OperationContract]
        List<Coverage> GetCoverageByListCoverageIdProductIdGroupCoverageId(List<int> coverageIds, int productId, int groupCoverageId);



        /// <summary>
        /// Obtener lista de coberturas por producto y grupo
        /// </summary>
        /// <param name="productId">Id producto</param>
        /// <param name="groupCoverage">Id grupo cobertura</param>
        /// <returns>Lista de coberturas</returns>
        [OperationContract]
        List<Models.Deductible> GetDeductiblesByCoverageId(int coverageId);

        /// <summary>
        /// Calcular Cuotas
        /// </summary>
        /// <param name="policy">Póliza</param>
        /// <returns>Cuotas</returns>
        [OperationContract]
        //List<Model.Quota> CalculateQuotas(Model.Policy policy);
        List<Model.Quota> CalculateQuotas(QuotaFilterDTO quotaFilterDTO);
        /// <summary>
        /// Obtener limite RC por identificador
        /// </summary>
        /// <param name="id">Identificador</param>
        /// <returns>Limite RC</returns>
        [OperationContract]
        LimitRc GetLimitRcById(int id);

        /// <summary>
        /// Ejecutar Reglas Póliza
        /// </summary>
        /// <param name="policy">Póliza</param>
        /// <param name="ruleSetId">Id Regla</param>
        /// <returns>Póliza</returns>
        [OperationContract]
        Policy RunRulesPolicy(Model.Policy policy, int ruleSetId);
        /// <summary>
        /// Retorna todos los valores del objeto del seguro, dependiendo el Id
        /// </summary>
        /// <param name="insuredObjectId"></param>
        /// <returns></returns>
        [OperationContract]
        InsuredObject GetInsuredObjectByInsuredObjectId(int insuredObjectId);

        /// <summary>
        /// Obtener lista de objetos de seguro  por producto y grupo
        /// </summary>
        /// <param name="productId">Id producto</param>
        /// <param name="groupCoverage">Id grupo cobertura</param>
        /// <returns>Lista de objetos de seguros</returns>
        [OperationContract]
        List<InsuredObject> GetInsuredObjectsByProductIdGroupCoverageId(int productId, int groupCoverageId, int prefixId);

        /// <summary>
        /// Obtener lista de coberturas por producto y grupo
        /// </summary>
        /// <param name="insuredObjectId">Id objeto del seguro</param>        
        /// <returns>Lista de coberturas</returns>
        [OperationContract]
        List<Model.Coverage> GetCoveragesByInsuredObjectIdGroupCoverageIdProductId(int insuredObjectId, int groupCoverageId, int productId);

        /// <summary>
        /// Obtener la lista de coberturas por objetos asugurados, grupo de coberturas y producto
        /// </summary>
        /// <param name="insuredObjectsIds"></param>
        /// <param name="groupCoverageId"></param>
        /// <param name="productId"></param>
        /// <param name="filterSelected"></param>
        /// <returns>Lista de coberturas</returns>
        List<Model.Coverage> GetCoveragesByInsuredObjectIdsGroupCoverageIdProductId(List<int> insuredObjectsIds, int groupCoverageId, int productId, bool filterSelected);

        /// <summary>
        /// Obtener Si aplica o no año bisisesto
        /// </summary>
        /// <param name="insuredObjectId">año bisisesto</param>        
        /// <returns>año bisisesto</returns>
        [OperationContract]
        Boolean GetLeapYear();

        /// <summary>
        ///  Obtener lista de endosos
        /// </summary>
        /// <returns>Colección de tipo Endorsement</returns>
        [OperationContract]
        List<Endorsement> GetEndorsementsByPrefixIdBranchIdPolicyNumber(int prefixId, int branchId, decimal policyNumber, int riskId = 0, bool isCurrent = false);

        /// <summary>
        /// Obtener las coberturas de accesorios originales y no originales
        /// </summary>
        /// <param name="productId">Id producto</param>
        /// <param name="groupCoverageId">Id grupo de coberturas</param>
        /// <param name="prefixId">Id ramo comercial</param>
        /// <returns>Lista de coberturas</returns>
        [OperationContract]
        List<Coverage> GetCoveragesAccessoriesByProductIdGroupCoverageIdPrefixId(int productId, int groupCoverageId, int prefixId);

        /// <summary>
        /// Obtener las coberturas aliadas
        /// </summary>
        /// <param name="coverageId">Id cobertura</param>
        /// <returns>Lista de coberturas</returns>
        [OperationContract]
        List<Coverage> GetAllyCoveragesByCoverageIdProductIdGroupCoverageId(int coverageId, int productId, int groupCoverageId);

        ///// <summary>
        ///// Obtener Plan De Pago Por Identificador
        ///// </summary>
        ///// <param name="paymentPlanId">Identificador</param>
        ///// <returns>Plan De Pago</returns>
        //[OperationContract]
        //Model.PaymentPlan GetPaymentPlanByPaymentPlanId(int paymentPlanId);

        /// <summary>
        /// Obtener objetos del seguro de esquema real
        /// </summary>
        /// <param name="id">Id plan de pago</param>
        /// <returns>Plan de pago</returns>
        [OperationContract]
        List<Model.InsuredObject> GetInsuredObjectsByRiskId(int riskId);



        /// <summary>
        /// Obtener Grupos De Facturación
        /// </summary>
        /// <param name="description">Id o Descripción</param>
        /// <returns>Grupos De Facturación</returns>
        [OperationContract]
        List<Models.BillingGroup> GetBillingGroupsByDescription(string description);

        /// <summary>
        /// Crea un grupo de facturación
        /// </summary>
        /// <param name="billingGroup">Modelo del grupo de facturación</param>
        /// <returns> Modelo de la solicitud agrupadora </returns>
        [OperationContract]
        Model.BillingGroup CreateBillingGroup(Model.BillingGroup billingGroup);


        [OperationContract]
        List<Models.BillingGroup> GetBillingGroup();

        /// <summary>
        /// Consulta los Objetos de Seguro 
        /// </summary>
        /// <param name="product">Modelo de Producto</param>
        /// <returns></returns>
        [OperationContract]
        List<Model.InsuredObject> GetInsuredObjectByPrefixIdList(int prefixId);

        /// <summary>
        /// Calcular Comisiones
        /// </summary>
        /// <param name="policy">Póliza</param>
        /// <param name="risks">Riesgos</param>
        /// <returns>Agencias</returns>
        [OperationContract]
        List<IssuanceAgency> CalculateCommissions(Model.Policy policy, List<Risk> risks);

        /// <summary>
        /// Obtener Coberturas por Objecto del Seguro
        /// </summary>
        /// <param name="insuredObjectId">Id Objecto del Seguro</param>
        /// <returns>Coberturas</returns>
        [OperationContract]
        List<Model.Coverage> GetCoveragesByInsuredObjectId(int insuredObjectId);

        /// <summary>
        /// Obtener Grupos de Coberturas
        /// </summary>
        /// <returns>Grupos de Coberturas</returns>
        [OperationContract]
        List<Model.GroupCoverage> GetAllGroupCoverages();

        /// <summary>
        /// Listado de Temporales Por Filtro
        /// </summary>
        /// <param name="policy">Filtro</param>
        /// <returns>Temporales</returns>
        [OperationContract]
        List<Policy> GetTemporalPoliciesByPolicy(Policy policy);

        [OperationContract]
        List<Policy> GetPoliciesByPolicy(Policy policy);

        /// <summary>
        /// Obtener plan de financion por moneda
        /// </summary>
        /// <param name="currencies">Monedas</param>
        /// <returns></returns>
        [OperationContract]
        List<Model.FinancialPlan> GetPaymentSchudeleByCurrencies(List<CommonModel.Currency> currencies);

        /// <summary>
        /// Busca la informacion de la cobertura asociada al producto y asigna coberturas aliadas
        /// </summary>
        /// <param name="coverageId">Id cobertura</param>
        /// <returns>Datos de la cobertura</returns>
        [OperationContract]
        Model.Coverage GetCoverageProductByCoverageId(int coverageId);

        /// <summary>
        /// Obtiene todos los RiskCommercialClass
        /// </summary>
        /// <returns>Lista de RiskCommercialClass</returns>
        [OperationContract]
        List<Model.RiskCommercialClass> GetRiskCommercialClass();

        /// <summary>
        /// Obtener lista de limites RC
        /// </summary>
        /// <param name=""></param>
        /// <returns>Lista Limit RC</returns>
        [OperationContract]
        List<Model.LimitRc> GetLimitsRc();

        /// <summary>
        /// Obtiene el deducible por Ramo tecnico
        /// </summary>
        /// <param name="prefixCd">Codigo Ramo Comercial</param>
        /// <returns>Model de deducibles</returns>
        [OperationContract]
        List<Model.Deductible> GetDeductiblesByPrefixId(int prefixCd);

        /// <summary>
        /// Obtiene el deducible por Ramo tecnico
        /// </summary>
        /// <param name="lineBusinessCd">Codigo Ramo tecnico</param>
        /// <returns>Model de deducibles</returns>
        [OperationContract]
        List<Model.Deductible> GetDeductiblesByLineBusinessId(int lineBusinessCd);

        /// <summary>
        /// Obtiene los deducibles
        /// </summary>
        /// <returns>Model de deducibles</returns>
        [OperationContract]
        List<Model.Deductible> GetDeductiblesAll();

        /// <summary>
        /// Validar si la cobertura existe en un producto y si este ya fue usado en un temporal
        /// </summary>
        /// <param name="productId">Id producto</param>
        /// <param name="groupCoverageId">Id del grupo de cobertura</param>
        /// <param name="insuredObjectId">Id Objeto del seguro</param>
        /// <param name="coverageId">Id cobertura</param>        
        /// <returns>true o false</returns>        
        [OperationContract]
        Boolean ExistCoverageProductByProductIdGroupCoverageIdInsuredObjectId(int productId, int groupCoverageId, int insuredObjectId, int coverageId);

        /// <summary>
        /// Validar si el objeto del seguro existe en un producto y si este ya fue usado en un temporal
        /// </summary>
        /// <param name="productId">Id producto</param>
        /// <param name="groupCoverageId">Id del grupo de cobertura</param>
        /// <param name="insuredObjectId">Id Objeto del seguro</param>
        /// <param name="coverageId">Id cobertura</param>        
        /// <returns>true o false</returns>
        [OperationContract]
        Boolean ExistInsuredObjectProductByProductIdGroupCoverageIdInsuredObjectId(int productId, int groupCoverageId, int insuredObjectId);

        /// <summary>
        /// Obtener Coberturas por Riesgo
        /// </summary>
        /// <param name="policyId">Id Póliza</param>
        /// <param name="endorsementId">Id Endoso</param>
        /// <param name="riskId">Id Riesgo</param>
        /// <returns>Coberturas</returns>
        [OperationContract]
        List<Model.Coverage> GetCoveragesByPolicyIdEndorsementIdRiskId(int policyId, int endorsementId, int riskId);

        /// <summary>
        /// Obtener Cobertura Por Id
        /// </summary>
        /// <param name="riskCoverageId">Id Cobertura</param>
        /// <returns>Cobertura</returns>
        [OperationContract]
        Model.Coverage GetCoverageByRiskCoverageId(int riskCoverageId);
        /// <summary>
        /// Obtener estado actual de la poliza de un endoso
        /// </summary>
        /// <param name="prefixId">Id ramo comercial</param>
        /// <param name="endorsementId">Id endoso</param>
        /// <returns>Modelo summary</returns>
        //[OperationContract]
        //Policy GetCurrentStatusPolicyByEndorsementId(int endorsementId);

        [OperationContract]
        Policy GetCurrentStatusPolicyByEndorsementIdIsCurrent(int endorsementId, bool isCurrent);


        /// <summary>
        /// Obtener estado de la poliza de un endoso
        /// </summary>
        /// <param name="prefixId">Id ramo comercial</param>
        /// <param name="endorsementId">Id endoso</param>
        /// <returns>Modelo summary</returns>
        //[OperationContract]
        //Policy GetCurrentStatusPolicyByEndorsementId(int endorsementId);

        [OperationContract]
        Policy GetStatusPolicyByEndorsementIdIsCurrent(int endorsementId, bool isCurrent);

        /// <summary>
        /// Obtener coberturas principales por objeto de seguro
        /// </summary>
        /// <param name="insuredObjectId">Id Objecto del Seguro</param>
        /// <returns>Coberturas Principales
        /// </returns>
        [OperationContract]
        List<Model.Coverage> GetCoveragesPrincipalByInsuredObjectId(int insuredObjectId);

        /// <summary>
        /// Obtener Planes Tecnicos por Tipo de Riesgo Cubierto
        /// </summary>
        /// <param name="coveredRiskTypeId">Tipo de Riesgo Cubierto</param>
        /// <param name="insuredObjectId">objeto del seguro</param>
        /// <returns></returns>
        [OperationContract]
        List<Model.TechnicalPlan> GetTechnicalPlanByCoveredRiskTypeIdInsuredObjectId(int coveredRiskTypeId, int insuredObjectId);


        /// <summary>
        /// Obtener coberturas por plan tecnico
        /// </summary>
        /// <param name="insuredObjectId">Id Plan Tecnico</param>
        /// <returns>Coberturas</returns>
        [OperationContract]
        List<Model.Coverage> GetCoveragesByTechnicalPlanId(int technicalPlanId);

        /// <summary>
        /// Obtener Grupos de Coberturas por tipo de riesgo
        /// </summary>
        /// <returns>Grupos de Coberturas</returns>
        [OperationContract]
        List<Model.GroupCoverage> GetGroupCoveragesByRiskTypeId(int riskTypeId);


        /// <summary>
        /// Obtener Endoso actual por placa y póliza
        /// </summary>
        /// <param name="policyId">policyId</param>
        /// <param name="licensePlate">licensePlate</param>
        /// <returns>Endoso</returns>
        [OperationContract]
        Models.Endorsement GetCurrentEndorsementByPolicyIdLicensePlateId(int policyId, string licensePlate);


        /// <summary>
        /// Elimina toda la información del temporal
        /// </summary>
        /// <param name="operationId">id operation</param>
        /// <returns>bool</returns>
        [OperationContract]
        string DeleteTemporalByOperationId(int operationId, long documentNum, int prefixId, int branchId);

        /// <summary>
        /// Elimina toda la información de los temporales (masivos).
        /// </summary>
        /// <param name="operationId">id operation</param>
        [OperationContract]
        void DeleteTemporalsByOperationId(int operationId);

        /// <summary>
        /// Elimina la informacion del temporal
        /// </summary>
        /// <param name="tempId">Id del temporal</param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteTemporalByTemporalId(int temporalId);

        /// <summary>
        /// Obtener lista de grupo de coberturas, Descripcion COVER_GROUP_RISK_TYPE
        /// </summary>
        /// <param name="productId">Id producto</param>
        /// <returns></returns>
        [OperationContract]
        List<GroupCoverage> GetProductCoverageGroupRiskByProductId(int productId);

        /// <summary>
        /// Obtiene los planes de pago
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [OperationContract]
        List<Model.FinancialPlan> GetFinancialPlanByProductId(int productId);

        [OperationContract]
        Model.Coverage Quotate(Model.Coverage coverage, int policyId, int riskId, int decimalQuantity, int? CoveredRiskType = 0, int? prefixId = 0);

        [OperationContract]
        Model.Clause GetClauseByClauseId(int clauseId);

        [OperationContract]
        List<Model.Clause> GetClausesByClauseIds(List<int> clauseIds);

        /// <summary>
        /// Obtener Endosos Validos de Una Póliza
        /// </summary>
        /// <param name="policyId">Id Póliza</param>
        /// <returns>Endosos</returns>
        [OperationContract]
        List<Models.Endorsement> GetEffectiveEndorsementsByPolicyId(int policyId);

        /// <summary>
        /// Obtener deducibles de las coberturas
        /// </summary>
        /// <param name="coverages">Lista de Coberturas</param>
        /// <returns>Coberturas</returns>
        [OperationContract]
        List<Models.Coverage> GetDeductiblesByCoverages(List<Models.Coverage> coverages);

        /// <summary>
        /// Calcular Prima Deducible
        /// </summary>
        [OperationContract]
        void CalculatePremiumDeductible(Model.Coverage coverage);

        [OperationContract]
        IssuanceAgency GetAgencyByAgentIdAgentAgencyId(int agentId, int agentAgencyId);

        /// <summary>
        /// Calcular Componentes De La Póliza
        /// </summary>
        /// <param name="policy">Póliza</param>
        /// <param name="risks">Riesgos</param>
        /// <returns>Componentes</returns>
        [OperationContract]
        List<Model.PayerComponent> CalculatePayerComponents(Model.Policy policy, List<Model.Risk> risks);

        /// <summary>
        /// Agregar Summary a laPóliza
        /// </summary>
        /// <param name="policy">Póliza</param>
        /// <param name="risks">Riesgos</param>
        /// <returns>Summary</returns>
        [OperationContract]
        Model.Summary CalculateSummary(Policy policy, List<Risk> risks);

        /// <summary>
        /// Consulta Agentes asociados a la poliza y endoso
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <param name="policyId"></param>
        /// <returns></returns>
        [OperationContract]
        List<IssuanceAgency> GetAgentsByPolicyIdEndorsementId(int? policyId, int? endorsementId);

        [OperationContract]
        Holder GetHolderByInsuredCode(int insuredCode);

        /// <summary>
        /// Obtener los Gastos
        /// </summary>
        /// <returns>Retorna la lista de Expenses</returns>
        [OperationContract]
        List<Expense> GetExpenses();

        /// Consulta las reglas de negocio
        /// </summary>
        /// <returns>Lista de Reglas en DB</returns>
        [OperationContract]
        List<BusinessRuleSet> GetRulesSet();
        /// <summary>
        /// Obtiene el id de la última póliza no anulada del ramo, sucursal y número de póliza relacionado
        /// </summary>
        /// <param name="prefixId"></param>
        /// <param name="branchId"></param>
        /// <param name="policyNumber"></param>
        /// <returns></returns>
        [OperationContract]
        Model.Policy GetCurrentPolicyByPrefixIdBranchIdPolicyNumber(int prefixId, int branchId, decimal policyNumber);





        /// <summary>
        /// Obtener datos de las cuotas
        /// </summary> 
        /// <param name="paymentPlanId">Identificador del plan de pago</param>
        /// <returns></returns>
        [OperationContract]
        List<Model.PaymentDistribution> GetPaymentDistributionByPaymentPlanId(int paymentPlanId);




        /// <summary>
        ///  actividad del riesgo
        /// </summary>
        [OperationContract]
        List<RiskActivity> GetRiskActivitiesByProductIdDescription(int productId);

        /// <summary>
        /// Obtener actividad por id
        /// </summary>
        /// <param name="productId">Id de actividad</param>
        /// <param name="description">descripcion</param>
        /// <returns>actividad</returns>
        [OperationContract]
        RiskActivity GetRiskActivityByActivityId(int activityId);

        /// <summary>
        /// Obtener actividad por tipo
        /// </summary>
        /// <param name="activityId">Id de actividad</param>
        /// <returns>actividad</returns>
        [OperationContract]
        List<RiskActivity> GetRiskActivityTypeByActivityId(int activityId);


        /// <summary>
        /// Agregar Summary a laPóliza
        /// </summary>
        /// <param name="policy">Póliza</param>
        /// <param name="risk">Riesgo</param>
        /// <returns>Riesgo</returns>
        [OperationContract]
        Model.Risk RunRulesRisk(Policy policy, Risk risk, int rulsetId);



        [OperationContract]
        List<Models.InsuredObject> GetInsuredObjects();

        [OperationContract]
        Model.InsuredObject GetInsuredObjectByProductIdGroupCoverageIdInsuredObjectId(int productId, int groupCoverageId, int insuredObjectId);

        /// <summary>
        /// Obtener SubcoveredRiskType por datos de poliza
        /// </summary>
        /// <param name="prefixId"></param>
        /// <param name="branchId"></param>
        /// <param name="policyNumber"></param>
        /// <returns></returns>
        SubCoveredRiskType? GetSubcoverRiskTypeByPrefixIdBranchIdPolicyNumber(int prefixId, int branchId, decimal policyNumber);

        /// <summary>
        /// Resumen de la prima
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <returns></returns>
        [OperationContract]
        Models.Summary GetSummaryByEndorsementId(int endorsementId);



        /// <summary>
        /// Obtener Tipo Contrato
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<SuretyContractType> GetSuretyContractTypes();

        ///<summary>
        /// Obtener Clase de Contrato
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<SuretyContractCategories> GetSuretyContractCategories();

        /// <summary>
        /// Obtener Coaseguradora
        /// </summary>
        /// <param name="userId">identificador de Coaseguradora</param>
        /// <returns></returns>
        [OperationContract]
        List<RiskType> GetRiskTypeByPrefixId(int prefixId);

        /// <summary>
        /// Consulta los Tipos de endosos habilitados por ramo
        /// </summary>
        /// <param name="prefixId">Identificador del ramo</param>
        /// <param name="isEnabled">Enabled</param>
        /// <returns> Listado de Tipos de endoso </returns>
        [OperationContract]
        List<PrefixEndoTypeEnabled> GetPrefixEndoEnabledByPrefixIdIsEnabled(int prefixId, bool isEnabled);

        /// <summary>
        /// Obtener lista de zonas de tarifación por ramo comercial
        /// </summary>
        /// <param name="prefixId">Id ramo comercial</param>
        /// <returns></returns>
        [OperationContract]
        List<RatingZone> GetRatingZonesByPrefixId(int prefixId);

        /// <summary>
        /// Obtener lista de grupo de coberturas
        /// </summary>
        /// <param name="prefixCd">Id ramo</param>
        /// <returns></returns>
        [OperationContract]
        List<Model.GroupCoverage> GetGroupCoveragesByPrefixCd(int prefixCd);

        /// <summary>
        /// Obtener lista de zonas de tarifación 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<RatingZone> GetRatingZones();

        /// <summary>
        /// Obtener zona de tarifacion
        /// </summary>
        /// <param name="parameter">Id ramo</param>
        /// <param name="parameter">Id ciudad</param>
        /// <param name="parameter">id departamento</param>
        /// <returns>list<Parameter Model></Parameter></returns>
        [OperationContract]
        RatingZone GetRatingZonesByPrefixIdCountryIdStateId(int prefixId, int countryId, int stateId);

        /// <summary>
        /// guarda la relacion de ramo comercial y ramo tecnico
        /// </summary>
        /// <param name="PrefixLineBusiness">Modelo donde se cargan las ralaciones que se van a guardar</param>
        /// <returns></returns>
        bool CreatePrefixByLineBusiness(PrefixLineBusiness PrefixLineBusiness);

        /// <summary>
        /// Obtiene Riesgo de cobertura de acuerdo al Id del Producto
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [OperationContract]
        ProductModel.Product GetCoveredProductById(int productId);

        /// <summary>
        /// Ratings the zone by rating zone code.
        /// </summary>
        /// <param name="ratingZoneCode">The rating zone code.</param>
        /// <returns></returns>
        [OperationContract]
        RatingZone RatingZoneByRatingZoneCode(int ratingZoneCode);


        #region adiciona producto
        [OperationContract]
        PaymentPlan GetDefaultPaymentPlanByProductId(int productId);
        #endregion

        /// <summary>
        /// Obtener Coaseguradora
        /// </summary>
        /// <param name="userId">identificador de Coaseguradora</param>
        /// <returns></returns>
        [OperationContract]
        IssuanceCoInsuranceCompany GetCoInsuranceCompanyByCoinsuranceId(int coInsuranceId);

        #region VehicleType

        /// <summary>
        /// ExecuteOperationsVehicleType
        /// </summary>
        /// <param name="vehicleTypes"></param>
        /// <returns>List<VehicleType></returns>
        [OperationContract]
        List<VehicleType> ExecuteOperationsVehicleType(List<VehicleType> vehicleTypes);

        /// <summary>
        /// GenerateFileToVehicleBody
        /// </summary>
        /// <param name="vehicleType"></param>
        /// <param name="fileName"></param>
        /// <returns>string</returns>
        [OperationContract]
        string GenerateFileToVehicleBody(VehicleType vehicleType, string fileName);

        /// <summary>
        /// GenerateFileToVehicleType
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>string</returns>
        [OperationContract]
        string GenerateFileToVehicleType(string fileName);

        /// <summary>
        /// GetVehicleTypes
        /// </summary>
        /// <returns>List<VehicleType></returns>
        [OperationContract]
        List<VehicleType> GetVehicleTypes();

        #endregion

        #region AllyCoverage
        ///// <summary>
        ///// Listado de coberturas aliadas, carga inicial
        ///// </summary>
        ///// <returns>AllyCoverageQueryDTO. Objeto Cobertura aliada DTO</returns>
        //[OperationContract]
        //UnderwritingParamService.DTO.AllyCoverageQueryDTO GetBusinessAllyCoverage();

        ///// <summary>
        ///// Obtiene resultados de una busqueda avanzada de coberturas
        ///// </summary>
        ///// <param name="AllyCoverageDTO">Objeto Cobertura Aliada DTO</param>
        ///// <returns>AllyCoverageQueryDTO. Objeto Cobertura Aliada DTO</returns>
        //[OperationContract]
        //UnderwritingParamService.DTO.AllyCoverageQueryDTO GetBusinessAllyCoverageAdv(UnderwritingParamService.DTO.AllyCoverageDTO allyCoverageQueryDTO);
        #endregion

        #region Coverage

        /// <summary>
        /// CreateBusinessCoCoverage: metodo que inserta una cobertura
        /// </summary>
        /// <param name="paramCoCoverageValue"></param>
        /// <returns></returns>
        [OperationContract]
        ParamCoCoverageValue CreateCoCoverageValue(ParamCoCoverageValue paramCoCoverageValue);

        /// <summary>
        /// GetApplicationBusinessCoverageValueByDescription: metodo que consulta listado de las coberturas a partir de una descripcion ingresada en la busqueda simple
        /// </summary>
        /// <param name="Description"></param>
        /// <returns></returns>
        [OperationContract]
        List<ParamCoCoverageValue> GetCoCoverageValueByPrefixId(int prefixId);

        /// <summary>
        /// GetBusinessCoverageValueAdv: metodo que consulta listado de coberturas  a partir de los filtros ingresados en la busuqeda avanzzada
        /// </summary>
        /// <param name="paramCoCoverageValue"></param>
        /// <returns></returns>
        [OperationContract]
        List<ParamCoCoverageValue> GetCoCoverageValueAdv(ParamCoCoverageValue paramCoCoverageValue);

        /// <summary>
        /// GenerateFileBusinessToCoverageValue: metodo que genera el archivo excel del listado completo de coberturas
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        string GenerateFileToCoCoverageValue(string fileName);
        /// <summary>
        /// Obtener Tipo de Riesgo por placa
        /// </summary>
        /// <param name="description">
        /// <returns> Lista por placa </returns>
        [OperationContract]
        List<Model.RiskVehicle> GetRisksByPlate(string description);

        /// <summary>
        /// GetBusinnesCoCoverageValue: metodo que obtiene el listado completo de las coberturas
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<ParamCoCoverageValue> GetCoCoverageValue();

        /// <summary>
        /// UpdateBusinessCoCoverageValue: metodo que actualiza la informacion de una cobertura
        /// </summary>
        /// <param name="paramCoCoverageValue"></param>
        /// <returns></returns>
        [OperationContract]
        ParamCoCoverageValue UpdateCoCoverageValue(ParamCoCoverageValue paramCoCoverageValue);

        /// <summary>
        /// DeleteBusinessCoCoverageValue: metodo que elimina una cobertura
        /// </summary>
        /// <param name="paramCoCoverageValue"></param>
        /// <returns></returns>
        [OperationContract]
        string DeleteCoCoverageValue(ParamCoCoverageValue paramCoCoverageValue);

        /// <summary>
        /// GetCoverageByPrefixId: metodo que consulta listado de las coberturaspor prefixId para carga de combo dependiente
        /// </summary>
        /// <param name="prefixId"></param>
        /// <returns></returns>
        [OperationContract]
        List<BaseParamCoverage> GetCoverageByPrefixId(int prefixId);
        #endregion

        #region conditionText
        /// <summary>
        /// CreateApplicationConditiontext. Crea un nuevo Texto Precatalogado
        /// </summary>
        /// <param name="conditionText">Modelo Company Conditional Text.</param>
        /// <returns>CompanyParamConditionText. Modelo Company Conditional Text.</returns>
        [OperationContract]
        ParamConditionText CreateBusinessConditiontext(ParamConditionText conditionText);

        /// <summary>
        /// UpdateBusinessConditiontext. Edita un nuevo Texto Precatalogado.
        /// </summary>
        /// <param name="conditionText">Modelo Company Conditional Text.</param>
        /// <returns>ParamConditionText. Modelo Company Conditional Text.</returns>

        [OperationContract]
        ParamConditionText UpdateBusinessConditiontext(ParamConditionText conditionText);

        /// <summary>
        /// DeleteBusinessConditiontext. Elimina un nuevo Texto Precatalogado.
        /// </summary>
        /// <param name="conditionText">Modelo Company Conditional Text.</param>
        /// <returns>ConditionTextDTO. Modelo Company Conditional Text.</returns>
        [OperationContract]
        string DeleteBusinessConditiontext(ParamConditionText conditionText);

        /// <summary>
        /// GetBusinessConditiontext. Retorna Lista de textos precatalogados
        /// </summary>
        /// <returns>List<CompanyParamConditionText>. Lista Modelos Company Conditional Text.</returns>
        [OperationContract]
        List<ParamConditionText> GetBusinessConditiontext();

        /// <summary>
        /// GetBusinessConditiontextByDescription. Retorna Lista de textos precatalogados a partir de Id o descripción.
        /// </summary>
        /// <param name="integer">Id Texto Precatalogado</param>
        /// <param name="description">Descripción Texto Precatalogado</param>
        /// <returns>List<ParamConditionText>. Lista Modelos Core Conditional Text.</returns>
        [OperationContract]
        List<ParamConditionText> GetBusinessConditiontextByDescription(int integer = 0, string description = "");

        /// <summary>
        /// ExcelFileDTO. Retorna objeto Excel DTO.        
        /// </summary>
        /// <returns>ExcelFileDTO. Objeto Excel DTO</returns>
        [OperationContract]
        string GenerateFileBusinessToConditiontext(string fileName);
        #endregion

        #region Tax

        #region TaxInferface

        /// <summary>
        /// CreateTax: metodo que inserta un impuesto nuevo
        /// </summary>
        /// <param name="paramTax"></param>
        /// <returns>mappedParamTax</returns>
        [OperationContract]
        ParamTax CreateTax(ParamTax paramTax);

        /// <summary>
        /// UpdateTax: metodo que actualiza un impuesto creado
        /// </summary>
        /// <param name="paramTax"></param>
        /// <returns>mappedParamTax</returns>
        [OperationContract]
        ParamTax UpdateTax(ParamTax paramTax);


        /// <summary>
        /// GetAplicationTaxByDescription: metodo que consulta un impuesto creado por descripcion
        /// </summary>
        /// <param name="TaxDescription"></param>
        /// <returns>mappedParamTaxList</returns>
        [OperationContract]
        List<ParamTax> GetAplicationTaxByDescription(string description);


        /// <summary>
        /// GetAplicationTaxById: metodo que consulta un impuesto creado por id y descripcion
        /// </summary>
        /// <param name="taxId"></param>
        /// <param name="taxDescription"></param>
        /// <returns>mappedParamTaxList</returns>
        [OperationContract]
        List<ParamTax> GetTaxByIdAndDescription(int taxId, string taxDescription);


        /// <summary>
        /// Genera el reporte de impuestos
        /// </summary>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>Ruta del archivo</returns>
        [OperationContract]
        MODPA.ExcelFileServiceModel GenerateTaxFileReport(int taxId);

        #endregion


        #region TaxRateInterface

        /// <summary>
        /// CreateTaxRate: metodo que inserta una tasa de impuesto nuevo
        /// </summary>
        /// <param name="paramTaxRate"></param>
        /// <returns>mappedParamTaxRate</returns>
        [OperationContract]
        ParamTaxRate CreateTaxRate(ParamTaxRate paramTaxRate);


        /// <summary>
        /// UpdateTaxRate: metodo que actualiza una tasa de impuesto creado
        /// </summary>
        /// <param name="paramTaxRate"></param>
        /// <returns>mappedParamTaxRate</returns>
        [OperationContract]
        ParamTaxRate UpdateTaxRate(ParamTaxRate paramTaxRate);

        [OperationContract]
        Deductible GetCoverageDeductibleByCoverageId(int CoverageId);


        /// <summary>
        /// GetTaxRatesByTaxId: metodo que consulta una lista tasas de impuesto por id de impuesto
        /// </summary>
        /// <param name="taxId"></param>
        /// <returns>mappedParamTaxRateList</returns>
        [OperationContract]
        List<ParamTaxRate> GetTaxRatesByTaxId(int taxId);

        [OperationContract]
        ParamTaxRate GetBusinessTaxRateByTaxIdbyAttributes(int taxId, int? taxConditionId, int? taxCategoryId, int? countryCode, int? stateCode, int? cityCode, int? economicActivityCode, int? prefixId, int? coverageId, int? technicalBranchId);

        [OperationContract]
        ParamTaxRate GetBusinessTaxRateById(int taxRateId);

        #endregion


        #region TaxCategoryInterface

        /// <summary>
        /// CreateTaxCategory: metodo que inserta una categoria de impuesto nuevo
        /// </summary>
        /// <param name="ParamTaxCategory"></param>
        /// <returns>mappedParamTaxCategory</returns>
        [OperationContract]
        ParamTaxCategory CreateTaxCategory(ParamTaxCategory paramTaxCategory);


        /// <summary>
        /// UpdateTaxCategory: metodo que actualiza una categoria de impuesto creado
        /// </summary>
        /// <param name="paramTaxCategory"></param>
        /// <returns>mappedParamTaxCategory</returns>
        [OperationContract]
        ParamTaxCategory UpdateTaxCategory(ParamTaxCategory paramTaxCategory);


        /// <summary>
        /// GetTaxCategoriesByTaxId: metodo que consulta una lista de categorias por id de impuesto
        /// </summary>
        /// <param name="taxId"></param>
        /// <returns>mappedParamTaxCategoryList</returns>
        [OperationContract]
        List<ParamTaxCategory> GetTaxCategoriesByTaxId(int taxId);


        /// <summary>
        /// DeleteAllTaxCategoriesByTaxId: metodo que borra todas las categorias por id de impuesto
        /// </summary>
        /// <param name="taxId"></param>
        /// <returns>bool Confirmation of Delete TaxCategory</returns>
        [OperationContract]
        bool DeleteTaxCategoriesByTaxId(int categoryId, int taxId);
        #endregion


        #region TaxConditionInterface

        /// <summary>
        /// CreateTaxCondition: metodo que inserta una condicion de impuesto nuevo
        /// </summary>
        /// <param name="ParamTaxCondition"></param>
        /// <returns>mappedParamTaxCondition</returns>
        [OperationContract]
        ParamTaxCondition CreateTaxCondition(ParamTaxCondition paramTaxCondition);


        /// <summary>
        /// UpdateTaxCategory: metodo que actualiza una categoria de impuesto creado
        /// </summary>
        /// <param name="ParamTaxCondition"></param>
        /// <returns>mappedParamTaxCategory</returns>
        [OperationContract]
        ParamTaxCondition UpdateTaxCondition(ParamTaxCondition paramTaxCondition);


        /// <summary>
        /// GetTaxConditionsByTaxId: metodo que consulta una lista de condiciones por id de impuesto
        /// </summary>
        /// <param name="taxId"></param>
        /// <returns>mappedParamTaxCoditionList</returns>
        [OperationContract]
        List<ParamTaxCondition> GetTaxConditionsByTaxId(int taxId);


        /// <summary>
        /// DeleteAllTaxConditionsByTaxId: metodo que borra todas las condiciones por id de impuesto
        /// </summary>
        /// <param name="taxId"></param>
        /// <returns>bool Confirmation of Delete TaxCondition</returns>
        [OperationContract]
        bool DeleteTaxConditionsByTaxId(int conditionId, int taxId);
        #endregion


        #endregion

        #region Guarantee
        [OperationContract]
        List<Models.IssuanceGuarantee> GetInsuredGuaranteesByIndividualId(int id);
        [OperationContract]
        List<Models.IssuanceGuarantee> GetCounterGuaranteesByIndividualId(int individualId);
        #endregion

        [OperationContract]
        IssuanceAgency GetAgencyByAgentCodeAgentTypeCode(int agentCode, int agentTypeId);

        /// <summary>
        /// Obtener los tipos de documentos
        /// </summary>
        /// <param name="typeDocument">tipo de documento
        /// 1. persona natural
        /// 2. persona juridica
        /// 3. todos</param>
        /// <returns></returns>
        [OperationContract]
        List<IssuanceDocumentType> GetDocumentTypes(int typeDocument);

        /// <summary>
        /// Obtiene todas las agencias
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<IssuanceAgency> GetAgencyAll();

        /// <summary>
        /// Obtiene todas las clausulas
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Clause> GetClauseAll();

        /// <summary>
        /// Obtiene los tipos de póliza
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<PolicyType> GetPolicyTypeAll();

        /// <summary>
        /// Obtiene el asegurado por insuredCode
        /// </summary>
        /// <param name="insuredCode"></param>
        /// <returns></returns>
        [OperationContract]
        IssuanceInsured GetInsuredByIndividualId(int insuredCode);

        /// <summary>
        /// Obtener Endosos de una Póliza
        /// </summary>
        /// <param name="policyNumber">Número de Póliza</param>
        /// <returns>Endosos</returns>        
        [OperationContract]
        List<Endorsement> GetEndorsementsAvaibleByPolicyId(int policyId);

        #region LightQuotation
        /// <summary>
        /// Retorna una agencia asociada a un usuario
        /// </summary>
        /// <param name="userId">Identificador del usuario</param>
        /// <returns>Agencia</returns>
        [OperationContract]
        IssuanceAgency GetIssuanceAgencyByUserId(int userId);
        #endregion
        /// <summary>
        /// Consulta los endosos que se han ejecutad contra una póliza
        /// </summary>
        /// <param name="prefixId">Identificador del ramo</param>
        /// <param name="branchId">Identificador de la sucursal</param>
        /// <param name="policyNumber">Número de póliza</param>
        /// <returns>Listado de endosos</returns>
        [OperationContract]
        List<Endorsement> GetPolicyEndorsementsByPrefixIdBranchIdPolicyNumber(int prefixId, int branchId, decimal policyNumber);

        /// <summary>
        /// Consulta los endosos que han cobrado prima
        /// </summary>
        /// <param name="policyId">Identificador de la póliza</param>
        /// <returns>Listado de endosos</returns>
        [OperationContract]
        List<Endorsement> GetPolicyEndorsementsWithPremiumByPolicyId(int policyId);

        /// <summary>
        /// Obtener estado de la poliza de un endoso
        /// </summary>
        /// <param name="prefixId">Id ramo comercial</param>
        /// <param name="endorsementId">Id endoso</param>
        /// <returns>Modelo summary</returns>
        //[OperationContract]
        //Policy GetCurrentStatusPolicyByEndorsementId(int endorsementId);

        /// <summary>
        /// Obtener las coberturas adicionales 
        /// </summary>
        /// <param name="coverageId">Id QUOEN.Coverage</param>
        /// <param name="productId">Id QUOEN.Group</param>
        /// <param name="groupCoverageId">Id QUOEN.Coverage</param>
        /// <returns>Lista de coberturas</returns>
        [OperationContract]
        List<Model.Coverage> GetAddCoveragesByCoverageIdProductIdGroupCoverageId(int coverageId, int productId, int groupCoverageId);

        /// <summary>
        /// Grabado Integracion
        /// </summary>
        /// <param name="endorsemenId"></param>
        /// <param name="operationId"></param>
        /// <param name="isMassive"></param>
        [OperationContract]
        void SaveControlPolicy(int policyId, int endorsemenId, int operationId, int policyOrigin);

        #region Claim

        /// Obtiene las pólizas por póliza y tipo de módulo
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="moduleType"></param>
        /// <returns>Pólizas</returns>
        [OperationContract]
        List<Policy> GetPoliciesByPolicyPersonTypeIdModuleType(Policy policy, int? personType, ModuleType moduleType);

        [OperationContract]
        Policy GetClaimPolicyByEndorsementId(int endorsementId);


        /// <summary>
        /// Consulta la información de las coberturas vigentes asociadas al riesgo
        /// </summary>
        /// <param name="riskId">Identificador del riesgo</param>
        /// <returns></returns>
        [OperationContract]
        List<Coverage> GetCoveragesByRiskId(int riskId);

        /// <summary>
        /// Consulta la información de las coberturas vigentes asociadas al riesgo según fecha de ocurrencia del siniestro y muestra las 
        /// sumas aseguradas según el porcentaje de participación de la compañía en el siniestro
        /// </summary>
        /// <param name="riskId"></param>
        /// <param name="occurrenceDate"></param>
        /// <param name="companyParticipationPercentage"></param>
        /// <returns></returns>
        [OperationContract]
        List<Coverage> GetCoveragesByRiskIdOccurrenceDateCompanyParticipationPercentage(int riskId, DateTime? occurrenceDate, decimal companyParticipationPercentage);

        [OperationContract]
        List<Coverage> GetCoveragesByLineBusinessId(int lineBusinessId);

        [OperationContract]
        List<Coverage> GetCoveragesByLineBusinessIdSubLineBusinessId(int lineBusinessId, int subLineBusinessId);

        [OperationContract]
        List<Deductible> GetDeductiblesByPolicyIdRiskNumCoverageIdCoverageNumber(int policyId, int riskNum, int coverageId, int coverNum);

        [OperationContract]
        List<CoInsuranceAssigned> GetCoInsuranceByPolicyIdByEndorsementId(int policyId, int endorsementId);

        #endregion
        /// <summary>
        /// Resumen para el endoso de declaracion para los ramos de ubicacion
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="risks"></param>
        /// <returns></returns>
        [OperationContract]
        Model.Summary CalculateSummaryPropertyDeclaration(Policy policy, List<Risk> risks, int riskId, int insuredObjectId);

        #region Combos
        [OperationContract]
        ComboListDTO GetRiskListsByProductId(int productId);
        #endregion

        /// <summary>
        /// GetDynamicConceptsB
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <param name="riskNum"></param>
        /// <param name="policyId"></param>
        /// <param name="riskId"></param>
        /// <returns></returns>
        [OperationContract]
        List<DynamicConcept> GetDynamicConceptsByEndorsementIdRiskNumPolicyIdRiskId(int endorsementId, int riskNum, int policyId, int riskId);

        /// <summary>
        /// GetRiskSuretyByPolicyIdEndorsmentId
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="endorsementId"></param>
        /// <returns></returns>
        [OperationContract]
        List<Risk> GetRisksByPolicyIdEndorsmentId(int policyId, int endorsementId);
        /// <summary>
        /// Obtiene las coberturas equivalentes de 3g a 2g
        /// </summary>
        /// <param name="coverageId"></param>
        /// <returns></returns>
        [OperationContract]
        List<IntCoEquivalenceCoverage> GetCoEquivalenceCoverage(int coverageId);

        /// <summary>
        /// Obtiene endosos por contragarantia
        /// </summary>
        /// <param name="guaranteeId"></param>
        /// <returns></returns>
        [OperationContract]
        List<Endorsement> GetPoliciesByGuaranteeId(int guaranteeId);


        /// <summary>
        /// GetRiskSuretyByRiskId
        /// </summary>
        /// <param name="riskId"></param>
        /// <returns></returns>
        [OperationContract]
        Risk GetRiskSuretyByRiskId(int riskId);

        /// <summary>
        /// Obtern datos validacion Tomador
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        [OperationContract]
        IssuanceInsured GetHolderValidateByIndividualId(int individualId);
        [OperationContract]
        int GetPolicyIdByEndormestId(int endormestId);
        [OperationContract]
        Policy GetPolicyByPolicyId(int policyId);

        [OperationContract]
        PortfolioPolicy GetPortfolioPolicyByPolicy(int branch, int prefix, string documentNumber);

        [OperationContract]
        List<PortfolioPolicy> GetPortfolioPolicyByPerson(List<PortfolioPolicy> portfolioPolicies);
    }
}