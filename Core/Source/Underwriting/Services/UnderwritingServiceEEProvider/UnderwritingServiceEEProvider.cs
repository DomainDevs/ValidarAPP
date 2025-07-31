using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Newtonsoft.Json;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.ModelServices.Models.Param;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.underwritingService.Enums;
using Sistran.Core.Application.UnderwritingServices.DAOs;
using Sistran.Core.Application.UnderwritingServices.DTOs;
using Sistran.Core.Application.UnderwritingServices.DTOs.Filter;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Business;
using Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Resources;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models.Base;
using Sistran.Core.Application.UnderwritingServices.Models.Distribution;
using Sistran.Core.Application.UniquePersonService.DAO;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using CommonModel = Sistran.Core.Application.CommonService.Models;
using CORUT = Sistran.Core.Application.Utilities.Helper;
using EmAutPolicies = Sistran.Core.Application.AuthorizationPoliciesServices.Enums;
using Model = Sistran.Core.Application.UnderwritingServices.Models;
using MODPA = Sistran.Core.Application.ModelServices.Models.Param;
using PRODModel = Sistran.Core.Application.ProductServices.Models;
using TP = Sistran.Core.Application.Utilities.Utility;
using TR = System.Threading.Tasks;
using UNBM = Sistran.Core.Application.UnderwritingServices.EEProvider.BusinessModels;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class UnderwritingServiceEEProviderCore : IUnderwritingServiceCore
    {

        /// <summary>
        /// Obtener tomadores por Id, Documento o Descripción
        /// </summary>
        /// <param name="description">Parametro de Busqueda</param>
        /// <param name="insuredSearchType">Tipo de Busqueda</param>
        /// <param name="customerType">Tipo de Cliente</param>
        /// <returns>Tomadores</returns>
        public List<Model.Holder> GetHoldersByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType)
        {
            try
            {
                HolderDAO holderProvider = new HolderDAO();
                return holderProvider.GetHoldersByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener tomadores por Id, Documento o Descripción
        /// </summary>
        /// <param name="description">Parametro de Busqueda</param>
        /// <param name="insuredSearchType">Tipo de Busqueda</param>
        /// <param name="customerType">Tipo de Cliente</param>
        /// <returns>Tomadores</returns>
        public List<IssuanceInsured> GetInsuredsByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType)
        {
            try
            {
                InsuredDAO insuredDAO = new InsuredDAO();
                List<IssuanceInsured> insureds = insuredDAO.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType);
                return insureds;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener el asegurado por IndividualId
        /// </summary>
        /// <param name="description">Parametro de Busqueda</param>
        /// <param name="insuredSearchType">Tipo de Busqueda</param>
        /// <param name="customerType">Tipo de Cliente</param>
        /// <returns>Tomadores</returns>
        public IssuanceInsured GetInsuredByIndividualId(int insuredCode)
        {
            try
            {
                InsuredDAO insuredDAO = new InsuredDAO();
                var insured = insuredDAO.GetInsuredByIndividualId(insuredCode);
                return insured;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Clausulas
        /// </summary>
        /// <param name="emissionLevel">Nivel De Emisión</param>
        /// <param name="conditionLevelId">Id Condición De Nivel</param>
        /// <returns>Clausulas</returns>
        public List<Model.Clause> GetClausesByEmissionLevelConditionLevelId(UnderwritingServices.Enums.EmissionLevel emissionLevel, int conditionLevelId)
        {
            try
            {
                ClauseDAO clauseDAO = new ClauseDAO();
                return clauseDAO.GetClausesByEmissionLevelConditionLevelId(emissionLevel, conditionLevelId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Clausulas
        /// </summary>
        /// <param name="emissionLevel">Nivel De Emisión</param>
        /// <param name="conditionLevelId">Id Condición De Nivel</param>
        /// <returns>Clausulas</returns>
        public List<Model.Clause> GetClausesByEmissionPolicyId(int policyid)
        {
            try
            {
                ClauseDAO clauseDAO = new ClauseDAO();
                return clauseDAO.GetClausesByEmissionLevelConditionLevelId(policyid);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        /// <summary>
        /// Obtener Clausulas
        /// </summary>
        /// <param name="emissionLevel">Nivel De Emisión</param>
        /// <returns>Clausulas</returns>
        public List<Model.Clause> GetClausesByEmissionLevel(UnderwritingServices.Enums.EmissionLevel emissionLevel)
        {
            try
            {
                ClauseDAO clauseDAO = new ClauseDAO();
                return clauseDAO.GetClausesByEmissionLevel(emissionLevel);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener textos precatalogados
        /// </summary>
        /// <param name="name">nombre</param>
        /// <param name="levelId">Id nivel</param>
        /// <param name="conditionalLevelId">Id condición</param>
        /// <returns>Lista de textos</returns>
        public List<Model.Text> GetTextsByNameLevelIdConditionLevelId(string name, int levelId, int conditionLevelId)
        {
            try
            {
                TextDAO textProvider = new TextDAO();
                return textProvider.GetTextsByNameLevelIdConditionLevelId(name, levelId, conditionLevelId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener lista de beneficiarios
        /// </summary>
        /// <param name="description">Id o nombre o razón social</param>
        /// <returns></returns>
        public List<Model.Beneficiary> GetBeneficiariesByDescription(string description, InsuredSearchType insuredSearchType, CustomerType? customerType = CustomerType.Individual)
        {
            try
            {
                BeneficiaryDAO beneficiaryDAO = new BeneficiaryDAO();
                return beneficiaryDAO.GetBeneficiariesByDescription(description, insuredSearchType, customerType);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener lista de limites RC
        /// </summary>
        /// <param name="prefixId">Id ramo comercial</param>
        /// <param name="productId">Id producto</param>
        /// <param name="policyTypeId">Id tipo de poliza</param>
        /// <returns></returns>
        public List<Model.LimitRc> GetLimitsRcByPrefixIdProductIdPolicyTypeId(int prefixId, int productId, int policyTypeId)
        {
            try
            {
                CoLimitsRcDAO coLimitsRcDAO = new CoLimitsRcDAO();
                return coLimitsRcDAO.GetLimitsRcByPrefixIdProductIdPolicyTypeId(prefixId, productId, policyTypeId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener lista de grupo de coberturas
        /// </summary>
        /// <param name="productId">Id producto</param>
        /// <returns></returns>
        public List<Model.GroupCoverage> GetGroupCoverages(int productId)
        {
            try
            {
                GroupCoverageDAO groupCoverageDAO = new GroupCoverageDAO();
                return groupCoverageDAO.GetGroupCoveragesByProductId(productId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener lista de grupo de coberturas
        /// </summary>
        /// <param name="prefixCd">Id ramo</param>
        /// <returns></returns>
        public List<Model.GroupCoverage> GetGroupCoveragesByPrefixCd(int prefixCd)
        {
            try
            {
                GroupCoverageDAO groupCoverageDAO = new GroupCoverageDAO();
                return groupCoverageDAO.GetGroupCoveragesByPrefixCd(prefixCd);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }




        /// <summary>
        /// Obtener lista de plan de pagos
        /// </summary>
        /// <param name="productId">Id de producto</param>
        /// <returns></returns>
        public List<Model.PaymentPlan> GetPaymentPlansByProductId(int productId)
        {
            try
            {
                PaymentPlanDAO paymentPlanDAO = new PaymentPlanDAO();
                return paymentPlanDAO.GetPaymentPlansByProductId(productId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Calculo Quotas
        /// </summary>
        /// <param name="quotaFilterDTO"></param>
        /// <returns></returns>
        public List<Model.Quota> CalculateQuotas(QuotaFilterDTO quotaFilterDTO)
        {
            try
            {
                FinancialPaymentSchedule paymentSchedule = PaymentPlanDAO.GetPaymentsScheduleBySheduleId(new PaymentScheduleFilterDTO { Id = quotaFilterDTO.PlanId });
                List<PaymentDistributionPlan> distributions = PaymentPlanDAO.GetFinancialPaymentDistributionByPaymentPlanId(quotaFilterDTO.PlanId);
                List<PaymentDistribution> paymentDistribution = PaymentPlanDAO.GetPaymentDistributionByPaymentPlanId(quotaFilterDTO.PlanId);
                Validate(distributions);
                FinancialPaymentPlan paymentPlan = new FinancialPaymentPlan();
                paymentPlan = ModelAssembler.CreateFinancialPaymentPlan(paymentSchedule);
                paymentPlan.CurrentFrom = quotaFilterDTO.CurrentFrom;
                paymentPlan.IssueDate = quotaFilterDTO.IssueDate;
                paymentPlan.ComponentValue = ModelAssembler.CreateComponentValueDTO(quotaFilterDTO.ComponentValueDTO);
                paymentPlan.PaymentDistribution = distributions;
                return QuotasBusiness.CalculateQuotas(paymentPlan, paymentDistribution);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        private void Validate(List<PaymentDistributionPlan> paymentDistributions)
        {
            if (paymentDistributions?.Count < 1)
            {
                throw new BusinessException(string.Format(Resources.Errors.ErrorDristributionQuotas, "Component"));
            }
        }

        /// <summary>
        /// Obtener Coberturas por Producto, Grupo de Coberturas y Ramo
        /// </summary>
        /// <param name="productId">Id Producto</param>
        /// <param name="groupCoverageId">Id Grupo Cobertura</param>
        /// <param name="prefixId">Id Ramo Comercial</param>
        /// <returns>Coberturas</returns>
        public List<Model.Coverage> GetCoveragesByProductIdGroupCoverageIdPrefixId(int productId, int groupCoverageId, int prefixId)
        {
            try
            {
                CoverageDAO coverageDAO = new CoverageDAO();
                return coverageDAO.GetCoveragesByProductIdGroupCoverageIdPrefixId(productId, groupCoverageId, prefixId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener cobertura por Id
        /// </summary>
        /// <param name="coverageId">Id cobertura</param>
        /// <returns>Cobertura</returns>
        public Model.Coverage GetCoverageByCoverageIdProductIdGroupCoverageId(int coverageId, int productId, int groupCoverageId)
        {
            try
            {
                CoverageDAO coverageDAO = new CoverageDAO();
                return coverageDAO.GetCoverageByCoverageIdProductIdGroupCoverageId(coverageId, productId, groupCoverageId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener cobertura por Lista de Ids
        /// </summary>
        /// <param name="coverageId">Id cobertura</param>
        /// <returns>Cobertura</returns>
        public List<Model.Coverage> GetCoverageByListCoverageIdProductIdGroupCoverageId(List<int> coverageIds, int productId, int groupCoverageId)
        {
            try
            {
                CoverageDAO coverageDAO = new CoverageDAO();
                return coverageDAO.GetCoverageByCoverageIdProductIdGroupCoverageId(coverageIds, productId, groupCoverageId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }



        /// <summary>
        /// Obtene los deducibles de una cobertura si lo ubiese
        /// </summary>
        /// <param name="coverageId">Idenbtificador de cobertura</param>
        /// <returns>Deducible por defecto para covertura</returns>
        public List<Model.Deductible> GetDeductiblesByCoverageId(int coverageId)
        {
            try
            {
                DeductibleDAO deductibleDAO = new DeductibleDAO();
                return deductibleDAO.GetDeductiblesByCoverageId(coverageId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public Deductible GetCoverageDeductibleByCoverageId(int coverageId)
        {
            try
            {
                return DeductibleDAO.GetDeductibleDefaultByCoverageId(coverageId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Calcular Componentes De La Póliza
        /// </summary>
        /// <param name="policy">Póliza</param>
        /// <param name="risks">Riesgos</param>
        /// <returns>Componentes</returns>
        public List<Model.PayerComponent> CalculatePayerComponents(Model.Policy policy, List<Model.Risk> risks)
        {
            try
            {
                PayerComponentDAO payerComponentDAO = new PayerComponentDAO();
                return payerComponentDAO.CalculatePayerComponents(policy, risks);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Agregar Summary a laPóliza
        /// </summary>
        /// <param name="policy">Póliza</param>
        /// <param name="risks">Riesgos</param>
        /// <returns>Summary</returns>        
        public Model.Summary CalculateSummary(Model.Policy policy, List<Model.Risk> risks)
        {
            try
            {
                PolicyDAO policyDAO = new PolicyDAO();
                return policyDAO.CalculateSummary(policy, risks);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener limite RC por identificador
        /// </summary>
        /// <param name="id">Identificador</param>
        /// <returns>Limite RC</returns>
        public Model.LimitRc GetLimitRcById(int id)
        {
            try
            {
                CoLimitsRcDAO coLimitsRcDAO = new CoLimitsRcDAO();
                return coLimitsRcDAO.GetLimitRcById(id);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }



        /// <summary>
        /// Ejecutar Reglas Póliza
        /// </summary>
        /// <param name="policy">Póliza</param>
        /// <param name="ruleSetId">Id Regla</param>
        /// <returns>Póliza</returns>
        public Model.Policy RunRulesPolicy(Model.Policy policy, int ruleSetId)
        {
            try
            {
                BusinessModels.Policy businessPolicy = new BusinessModels.Policy(policy);
                return businessPolicy.RunRulesPolicy(ruleSetId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener objetos del seguro por producto y grupo
        /// </summary>
        /// <param name="productId">Id producto</param>
        /// <param name="groupCoverage">Id grupo cobertura</param>
        /// <returns>Lista de objetos de seguro</returns>
        public List<Model.InsuredObject> GetInsuredObjectsByProductIdGroupCoverageId(int productId, int groupCoverageId, int prefixId)
        {
            try
            {
                InsuredObjectDAO insuredObjectDAO = new InsuredObjectDAO();
                return insuredObjectDAO.GetInsuredObjectsByProductIdGroupCoverageId(productId, groupCoverageId, prefixId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener lista de coberturas por producto y grupo
        /// </summary>
        /// <param name="insuredObjectId">Id objeto del seguro</param>
        /// <returns>Lista de coberturas</returns>
        public List<Model.Coverage> GetCoveragesByInsuredObjectIdGroupCoverageIdProductId(int insuredObjectId, int groupCoverageId, int productId)
        {
            try
            {
                CoverageDAO coverageDAO = new CoverageDAO();
                return coverageDAO.GetCoveragesByInsuredObjectIdGroupCoverageIdProductId(insuredObjectId, groupCoverageId, productId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener la lista de coberturas por objetos asugurados, grupo de coberturas y producto
        /// </summary>
        /// <param name="insuredObjectsIds"></param>
        /// <param name="groupCoverageId"></param>
        /// <param name="productId"></param>
        /// <param name="filterSelected"></param>
        /// <returns>Lista de coberturas</returns>
        public List<Model.Coverage> GetCoveragesByInsuredObjectIdsGroupCoverageIdProductId(List<int> insuredObjectsIds, int groupCoverageId, int productId, bool filterSelected)
        {
            try
            {
                CoverageDAO coverageDAO = new CoverageDAO();
                return coverageDAO.GetCoveragesByInsuredObjectIdsGroupCoverageIdProductId(insuredObjectsIds, groupCoverageId, productId, filterSelected);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Año Bisiesto
        /// </summary>
        /// <returns>Año Bisiesto</returns>
        public Boolean GetLeapYear()
        {
            try
            {
                return QuoteManager.LeapYear;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener endosos de una poliza
        /// </summary>
        /// <param name="prefixId">Id ramo comercial</param>
        /// <param name="branchId">Id sucursal</param>
        /// <param name="policyNumber">Número de póliza</param>
        /// <param name="riskId">Id de riesgo</param>
        /// <returns>Lista de endosos</returns>
        public List<Model.Endorsement> GetEndorsementsByPrefixIdBranchIdPolicyNumber(int prefixId, int branchId, decimal policyNumber, int riskId = 0, bool isCurrent = false)
        {
            try
            {
                PolicyDAO policyDao = new PolicyDAO();
                return policyDao.GetEndorsementsByPrefixIdBranchIdPolicyNumber(prefixId, branchId, policyNumber, riskId, isCurrent);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener las coberturas de accesorios originales y no originales
        /// </summary>
        /// <param name="productId">Id producto</param>
        /// <param name="groupCoverageId">Id grupo de coberturas</param>
        /// <param name="prefixId">Id ramo comercial</param>
        /// <returns>Lista de coberturas</returns>
        public List<Model.Coverage> GetCoveragesAccessoriesByProductIdGroupCoverageIdPrefixId(int productId, int groupCoverageId, int prefixId)
        {
            try
            {
                CoverageDAO coverageDAO = new CoverageDAO();
                return coverageDAO.GetCoveragesAccessoriesByProductIdGroupCoverageIdPrefixId(productId, groupCoverageId, prefixId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener las coberturas aliadas
        /// </summary>
        /// <param name="productId">Id cobertura</param>
        /// <returns>Lista de coberturas</returns>
        public List<Model.Coverage> GetAllyCoveragesByCoverageIdProductIdGroupCoverageId(int coverageId, int productId, int groupCoverageId)
        {
            try
            {
                CoverageDAO coverageDAO = new CoverageDAO();
                return coverageDAO.GetAllyCoveragesByCoverageIdProductIdGroupCoverageId(coverageId, productId, groupCoverageId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener las coberturas adicionales 
        /// </summary>
        /// <param name="coverageId">Id QUOEN.Coverage</param>
        /// <param name="productId">Id QUOEN.Group</param>
        /// <param name="groupCoverageId">Id QUOEN.Coverage</param>
        /// <returns>Lista de coberturas</returns>
        public List<Model.Coverage> GetAddCoveragesByCoverageIdProductIdGroupCoverageId(int coverageId, int productId, int groupCoverageId)
        {
            try
            {
                CoverageDAO coverageDAO = new CoverageDAO();
                return coverageDAO.GetAddCoveragesByCoverageIdProductIdGroupCoverageId(coverageId, productId, groupCoverageId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public Model.PaymentPlan GetPaymentPlanByPolicyId(int policyd)
        {
            try
            {
                PaymentPlanDAO paymentPlanDAO = new PaymentPlanDAO();
                return paymentPlanDAO.GetPaymentPlanByPolicyId(policyd);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener objetos del seguro 
        /// </summary>
        /// <param name="RiskId">Id</param>
        /// <returns>Listado de objetos del seguro</returns>
        public List<Model.InsuredObject> GetInsuredObjectsByRiskId(int riskId)
        {
            try
            {
                InsuredObjectDAO insuredObjectDAO = new InsuredObjectDAO();
                return insuredObjectDAO.GetInsuredObjectByRiskId(riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }



        /// <summary>
        /// Obtener Grupos De Facturación
        /// </summary>
        /// <param name="description">Id o Descripción</param>
        /// <returns>Grupos De Facturación</returns>
        public List<Models.BillingGroup> GetBillingGroupsByDescription(string description)
        {
            try
            {
                BillingGroupDAO billingGroupDAO = new BillingGroupDAO();
                return billingGroupDAO.GetBillingGroupsByDescription(description);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Crea un grupo de facturación
        /// </summary>
        /// <param name="billingGroup">Modelo del grupo de facturación</param>
        /// <returns> Modelo de la solicitud agrupadora </returns>
        public Model.BillingGroup CreateBillingGroup(Model.BillingGroup billingGroup)
        {
            try
            {
                BillingGroupDAO billingGroupDAO = new BillingGroupDAO();
                return billingGroupDAO.CreateBillingGroup(billingGroup);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        /// <summary>
        /// Consultar los Objestos de Seguro
        /// </summary>
        /// <param name="prefixId">Id ramo comercial</param>        
        /// <returns>Lista de productos</returns>
        public List<Model.InsuredObject> GetInsuredObjectByPrefixIdList(int prefixId)
        {
            try
            {
                return InsuredObjectDAO.GetInsuredObjectByPrefixIdList(prefixId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Calcular Comisiones
        /// </summary>
        /// <param name="policy">Póliza</param>
        /// <param name="risks">Riesgos</param>
        /// <returns>Agencias</returns>
        public List<IssuanceAgency> CalculateCommissions(Model.Policy policy, List<Model.Risk> risks)
        {
            try
            {
                CommissionDAO commissionDAO = new CommissionDAO();
                return commissionDAO.CalculateCommissions(policy, risks);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Coberturas por Objecto del Seguro
        /// </summary>
        /// <param name="insuredObjectId">Id Objecto del Seguro</param>
        /// <returns>Coberturas</returns>
        public List<Model.Coverage> GetCoveragesByInsuredObjectId(int insuredObjectId)
        {
            try
            {
                CoverageDAO coverageDAO = new CoverageDAO();
                return coverageDAO.GetCoveragesByInsuredObjectId(insuredObjectId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Grupos de Coberturas
        /// </summary>
        /// <returns>Grupos de Coberturas</returns>
        public List<Model.GroupCoverage> GetAllGroupCoverages()
        {
            try
            {
                CoverGroupRiskTypeDAO coverGroupRiskTypeDAO = new CoverGroupRiskTypeDAO();
                return coverGroupRiskTypeDAO.GetAllGroupCoverages();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Listado de Temporales Por Filtro
        /// </summary>
        /// <param name="policy">Filtro</param>
        /// <returns>Temporales</returns>
        public List<Model.Policy> GetTemporalPoliciesByPolicy(Model.Policy policy)
        {
            try
            {
                PolicyDAO policyProvider = new PolicyDAO();
                return policyProvider.GetTemporalPoliciesByPolicy(policy);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Listado de Polizas Por Filtro
        /// </summary>
        /// <param name="policy">Filtro</param>
        /// <returns>Temporales</returns>
        public List<Model.Policy> GetPoliciesByPolicy(Model.Policy policy)
        {
            try
            {
                PolicyDAO policyProvider = new PolicyDAO();
                return policyProvider.GetPoliciesByPolicy(policy);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener plan de financion por moneda
        /// </summary>
        /// <param name="currencies">Monedas</param>
        /// <returns></returns>
        public List<Model.FinancialPlan> GetPaymentSchudeleByCurrencies(List<CommonModel.Currency> currencies)
        {
            try
            {
                FinancialPlanDAO productDAO = new FinancialPlanDAO();
                return productDAO.GetPaymentSchudeleByCurrencies(currencies);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Busca la informacion de la cobertura asociada al producto y asigna coberturas aliadas
        /// </summary>
        /// <param name="coverageId">Id cobertura</param>
        /// <returns>Datos de la cobertura</returns>
        public Model.Coverage GetCoverageProductByCoverageId(int coverageId)
        {
            try
            {
                CoverageDAO coverageDAO = new CoverageDAO();
                return coverageDAO.GetCoverageProductByCoverageId(coverageId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene todos los RiskCommercialClass
        /// </summary>
        /// <returns>Lista de RiskCommercialClass</returns>
        public List<Model.RiskCommercialClass> GetRiskCommercialClass()
        {
            try
            {
                RiskCommercialClassDAO riskCommercialClassDAO = new RiskCommercialClassDAO();
                return riskCommercialClassDAO.GetRiskCommercialClass();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener lista de limites RC
        /// </summary>
        /// <param name=""></param>
        /// <returns>Lista Limit RC</returns>
        public List<Model.LimitRc> GetLimitsRc()
        {
            try
            {
                CoLimitsRcDAO coLimitsRcDAO = new CoLimitsRcDAO();
                return coLimitsRcDAO.GetLimitsRc();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene el deducible por Ramo tecnico
        /// </summary>
        /// <param name="prefixCd">Codigo Ramo Comercial</param>
        /// <returns>Model de deducibles</returns>
        public List<Model.Deductible> GetDeductiblesByPrefixId(int prefixCd)
        {
            try
            {
                DeductibleDAO deductibleDAO = new DeductibleDAO();
                return deductibleDAO.GetDeductiblesByPrefixId(prefixCd);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene el deducible por Ramo tecnico
        /// </summary>
        /// <param name="lineBusinessCd">Codigo Ramo tecnico</param>
        /// <returns>Model de deducibles</returns>
        public List<Model.Deductible> GetDeductiblesByLineBusinessId(int lineBusinessCd)
        {
            try
            {
                DeductibleDAO deductibleDAO = new DeductibleDAO();
                return deductibleDAO.GetDeductiblesByLineBusinessId(lineBusinessCd);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene los deducibles 
        /// </summary>
        /// <returns>Model de deducibles</returns>
        public List<Model.Deductible> GetDeductiblesAll()
        {
            try
            {
                DeductibleDAO deductibleDAO = new DeductibleDAO();
                return deductibleDAO.GetDeductiblesAll();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Validar si la cobertura existe en un producto y si este ya fue usado en un temporal
        /// </summary>
        /// <param name="productId">Id producto</param>
        /// <param name="groupCoverageId">Id del grupo de cobertura</param>
        /// <param name="insuredObjectId">Id Objeto del seguro</param>
        /// <param name="coverageId">Id cobertura</param>        
        /// <returns>true o false</returns>        
        public Boolean ExistCoverageProductByProductIdGroupCoverageIdInsuredObjectId(int productId, int groupCoverageId, int insuredObjectId, int coverageId)
        {
            try
            {
                PolicyDAO productDAO = new PolicyDAO();
                return productDAO.ExistCoverageProductByProductIdGroupCoverageIdInsuredObjectId(productId, groupCoverageId, insuredObjectId, coverageId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Validar si el objeto del seguro existe en un producto y si este ya fue usado en un temporal
        /// </summary>
        /// <param name="productId">Id producto</param>
        /// <param name="groupCoverageId">Id del grupo de cobertura</param>
        /// <param name="insuredObjectId">Id Objeto del seguro</param>
        /// <param name="coverageId">Id cobertura</param>        
        /// <returns>true o false</returns>
        public Boolean ExistInsuredObjectProductByProductIdGroupCoverageIdInsuredObjectId(int productId, int groupCoverageId, int insuredObjectId)
        {
            try
            {

                PolicyDAO policyDAO = new PolicyDAO();
                return policyDAO.ExistInsuredObjectProductByProductIdGroupCoverageIdInsuredObjectId(productId, groupCoverageId, insuredObjectId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Coberturas por Riesgo
        /// </summary>
        /// <param name="policyId">Id Póliza</param>
        /// <param name="endorsementId">Id Endoso</param>
        /// <param name="riskId">Id Riesgo</param>
        /// <returns>Coberturas</returns>
        public List<Model.Coverage> GetCoveragesByPolicyIdEndorsementIdRiskId(int policyId, int endorsementId, int riskId)
        {
            try
            {
                CoverageDAO coverageDAO = new CoverageDAO();
                return coverageDAO.GetCoveragesByPolicyIdEndorsementIdRiskId(policyId, endorsementId, riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Cobertura Por Id
        /// </summary>
        /// <param name="riskCoverageId">Id Cobertura</param>
        /// <returns>Cobertura</returns>
        public Model.Coverage GetCoverageByRiskCoverageId(int riskCoverageId)
        {
            try
            {
                CoverageDAO coverageDAO = new CoverageDAO();
                return coverageDAO.GetCoverageByRiskCoverageId(riskCoverageId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public Model.Policy GetCurrentStatusPolicyByEndorsementIdIsCurrent(int endorsementId, bool isCurrent)
        {
            try
            {
                PolicyDAO policyDao = new PolicyDAO();
                return policyDao.GetCurrentStatusPolicyByEndorsementIdIsCurrent(endorsementId, isCurrent);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public Model.Policy GetStatusPolicyByEndorsementIdIsCurrent(int endorsementId, bool isCurrent)
        {
            try
            {
                PolicyDAO policyDao = new PolicyDAO();
                return policyDao.GetStatusPolicyByEndorsementIdIsCurrent(endorsementId, isCurrent);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener coberturas principales por objeto de seguro
        /// </summary>
        /// <param name="insuredObjectId">Id Objecto del Seguro</param>
        /// <returns>Coberturas Principales
        /// </returns>
        public List<Model.Coverage> GetCoveragesPrincipalByInsuredObjectId(int insuredObjectId)
        {
            try
            {
                CoverageDAO coverageDAO = new CoverageDAO();
                return coverageDAO.GetCoveragesPrincipalByInsuredObjectId(insuredObjectId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Planes Tecnicos por Tipo de Riesgo Cubierto
        /// </summary>
        /// <param name="coveredRiskTypeId">Tipo de Riesgo Cubierto</param>
        /// <param name="insuredObjectId">objeto del seguro</param>
        /// <returns></returns>
        public List<Model.TechnicalPlan> GetTechnicalPlanByCoveredRiskTypeIdInsuredObjectId(int coveredRiskTypeId, int insuredObjectId)
        {
            try
            {
                TechnicalPlanDAO technicalPlansDAO = new TechnicalPlanDAO();
                return technicalPlansDAO.GetTechnicalPlanByCoveredRiskTypeIdInsuredObjectId(coveredRiskTypeId, insuredObjectId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener coberturas por plan tecnico
        /// </summary>
        /// <param name="insuredObjectId">Id Plan Tecnico</param>
        /// <returns>Coberturas</returns>
        public List<Model.Coverage> GetCoveragesByTechnicalPlanId(int technicalPlanId)
        {
            try
            {
                CoverageDAO coverageDAO = new CoverageDAO();
                return coverageDAO.GetCoveragesByTechnicalPlanId(technicalPlanId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Grupos de Coberturas
        /// </summary>
        /// <returns>Grupos de Coberturas</returns>
        public List<Model.GroupCoverage> GetGroupCoveragesByRiskTypeId(int riskTypeId)
        {
            try
            {
                CoverGroupRiskTypeDAO coverGroupRiskTypeDAO = new CoverGroupRiskTypeDAO();
                return coverGroupRiskTypeDAO.GetGroupCoveragesByRiskTypeId(riskTypeId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Endoso actual por placa y póliza
        /// </summary>
        /// <param name="policyId">policyId</param>
        /// <param name="licensePlate">licensePlate</param>
        /// <returns>Endoso</returns>
        public Model.Endorsement GetCurrentEndorsementByPolicyIdLicensePlateId(int policyId, string licensePlate)
        {
            try
            {
                EndorsementDAO endorsementDAO = new EndorsementDAO();
                return endorsementDAO.GetCurrentEndorsementByPolicyIdLicensePlateId(policyId, licensePlate);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #region productos
        #endregion

        public IssuanceAgency GetAgencyByAgentIdAgentAgencyId(int agentId, int agentAgencyId)
        {
            try
            {
                AgencyDAO endorsementDAO = new AgencyDAO();
                return endorsementDAO.GetAgencyByAgentIdAgentAgencyId(agentId, agentAgencyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina toda la información del temporal
        /// </summary>
        /// <param name="operationId">id operation</param>
        /// <returns>bool</returns>
        public string DeleteTemporalByOperationId(int operationId, long documentNum, int prefixId, int branchId)
        {
            try
            {
                return PolicyDAO.DeleteTemporalByOperationId(operationId, documentNum, prefixId, branchId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        /// <summary>
        /// Elimina la informacion del temporal
        /// </summary>
        /// <param name="tempId"> id del Temporal</param>
        /// <returns></returns>
        public bool DeleteTemporalByTemporalId(int temporalId)
        {
            try
            {
                return PolicyDAO.DeleteTemporalByTemporalId(temporalId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina toda la información de los temporales (masivos).
        /// </summary>
        /// <param name="operationId">id operation</param>
        /// <exception cref="BusinessException"></exception>
        public void DeleteTemporalsByOperationId(int operationId)
        {
            try
            {
                PolicyDAO.DeleteTemporalsByOperationId(operationId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public Model.Coverage Quotate(Model.Coverage coverage, int policyId, int riskId, int decimalQuantity, int? CoveredRiskType = 0, int? prefixId = 0)
        {
            try
            {
                BusinessModels.Coverage coverageModel = new BusinessModels.Coverage(coverage, policyId, riskId);
                coverageModel.Quotate(decimalQuantity, prefixId);
                return coverage;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public Model.Clause GetClauseByClauseId(int clauseId)
        {
            try
            {
                ClauseDAO clause = new ClauseDAO();
                return clause.GetClauseByClauseId(clauseId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<Model.Clause> GetClausesByClauseIds(List<int> clauseIds)
        {
            try
            {
                ClauseDAO clause = new ClauseDAO();
                return clause.GetClausesByClauseIds(clauseIds);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Endosos Validos de Una Póliza
        /// </summary>
        /// <param name="policyId">Id Póliza</param>
        /// <returns>Endosos</returns>
        public List<Models.Endorsement> GetEffectiveEndorsementsByPolicyId(int policyId)
        {
            try
            {
                EndorsementDAO endorsement = new EndorsementDAO();
                return endorsement.GetEffectiveEndorsementsByPolicyId(policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener deducibles de las coberturas
        /// </summary>
        /// <param name="coverages">Lista de Coberturas</param>
        /// <returns>Coberturas</returns>
        public List<Models.Coverage> GetDeductiblesByCoverages(List<Models.Coverage> coverages)
        {
            try
            {
                return DeductibleDAO.GetDeductiblesByCoverages(coverages);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Calcular Prima Deducible
        /// </summary>
        public virtual void CalculatePremiumDeductible(Model.Coverage coverage)
        {
            try
            {
                Sistran.Core.Application.UnderwritingServices.EEProvider.BusinessModels.Deductible deductible = new BusinessModels.Deductible(coverage);
                deductible.CalculatePremiumDeductible();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<IssuanceAgency> GetAgentsByPolicyIdEndorsementId(int? policyId, int? endorsementId)
        {
            try
            {
                PolicyDAO policyDAO = new PolicyDAO();
                return policyDAO.GetAgentsByPolicyIdEndorsementId(policyId, endorsementId).Result;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene las politicas pendientes por autorizar
        /// </summary>
        /// <param name="temporalId">numero del temporal</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        protected List<PoliciesAut> GetPendingAuthorizationPolicies(int temporalId)
        {
            try
            {
                var commonService = DelegateService.utilitiesServiceCore;
                PendingOperation pendingOperation = commonService.GetPendingOperationById(temporalId);

                Model.Policy policy = CORUT.JsonHelper.DeserializeJson<Models.Policy>(pendingOperation.Operation);
                if (policy != null && policy.Product != null)
                {
                    policy.Id = pendingOperation.Id;
                    List<PendingOperation> pendingOperations = commonService.GetPendingOperationsByParentId(policy.Id);

                    foreach (var pendingOperationRisk in pendingOperations)
                    {
                        Model.Risk risk = JsonConvert.DeserializeObject<Model.Risk>(pendingOperationRisk.Operation);
                        if (risk.InfringementPolicies != null)
                        {
                            policy.InfringementPolicies.AddRange(risk.InfringementPolicies);
                        }
                    }

                    return policy.InfringementPolicies.Where(x => x.Type == EmAutPolicies.TypePolicies.Authorization || x.Type == EmAutPolicies.TypePolicies.Restrictive).ToList();
                }
                else
                {
                    return policy.InfringementPolicies = new List<PoliciesAut>();
                }
            }
            catch (BusinessException ex)
            {
                return new List<PoliciesAut>();
                throw new BusinessException(ex.Message, ex);
            }
        }

        ///// <summary>
        ///// Metodo que realiza la emision la poliza desde politicas
        ///// </summary>
        ///// <param name="temporalId">Id del temporal</param>
        ///// <returns></returns>
        //public string CreatePolicyAuthorization(string temporalId)
        //{
        //    try
        //    {
        //        PolicyDAO policyDao = new PolicyDAO();
        //        return policyDao.CreatePolicyAuthorization(temporalId);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new BusinessException(ex.Message, ex);
        //    }
        //}

        /// <summary>
        /// Consulta las reglas de negocio
        /// </summary>
        /// <returns>Lista de Reglas en DB</returns>
        public List<Models.BusinessRuleSet> GetRulesSet()
        {
            try
            {
                ExpenseDAO ExpenseProvider = new ExpenseDAO();
                return ExpenseProvider.GetRulesSet();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Consulta los gastos
        /// </summary>
        /// <returns>Lista de gastos en DB</returns>
        public List<Models.Expense> GetExpenses()
        {
            try
            {
                ExpenseDAO ExpenseProvider = new ExpenseDAO();
                return ExpenseProvider.GetExpenses();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        public Models.Holder GetHolderByInsuredCode(int insuredCode)
        {
            try
            {
                HolderDAO holderDao = new HolderDAO();
                return holderDao.GetHolderByInsuredCode(insuredCode);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefixId"></param>
        /// <param name="branchId"></param>
        /// <param name="policyNumber"></param>
        /// <returns></returns>
        public Model.Policy GetCurrentPolicyByPrefixIdBranchIdPolicyNumber(int prefixId, int branchId, decimal policyNumber)
        {
            try
            {
                var policyDAO = new PolicyDAO();
                return policyDAO.GetCurrentPolicyByPrefixIdBranchIdPolicyNumber(prefixId, branchId, policyNumber);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        /// <summary>
        /// Obtener datos de las cuotas
        /// </summary> 
        /// <param name="paymentPlanId">Identificador del plan de pago</param>
        /// <returns></returns>        
        public List<Model.PaymentDistribution> GetPaymentDistributionByPaymentPlanId(int paymentPlanId)
        {
            try
            {
                PaymentPlanDAO paymentPlanDAO = new PaymentPlanDAO();
                return GetPaymentDistributionByPaymentPlanId(paymentPlanId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }




        /// <summary>
        /// Obtener actividades del riesgo
        /// </summary>
        /// <param name="productId">identificador de producto</param>
        /// <param name="description">descripcion</param>
        /// <returns>Lista de actividades de riesgo</returns>
        public List<Models.RiskActivity> GetRiskActivitiesByProductIdDescription(int productId)
        {
            try
            {
                RiskActivityDAO riskActivityDAO = new RiskActivityDAO();
                return riskActivityDAO.GetRiskActivitiesByProductIdDescription(productId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener actividad por id
        /// </summary>
        /// <param name="productId">Id de actividad</param>
        /// <param name="description">descripcion</param>
        /// <returns>actividad</returns>
        public Models.RiskActivity GetRiskActivityByActivityId(int activityId)
        {
            try
            {
                RiskActivityDAO riskActivityDAO = new RiskActivityDAO();
                return riskActivityDAO.GetRiskActivityByActivityId(activityId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener actividad por tipo
        /// </summary>
        /// <param name="activityId">Id de actividad</param>
        /// <returns>actividad</returns>
        public List<Models.RiskActivity> GetRiskActivityTypeByActivityId(int activityId)
        {
            try
            {
                RiskActivityDAO riskActivityDAO = new RiskActivityDAO();
                return riskActivityDAO.GetRiskActivityTypeByActivityId(activityId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        /// <summary>
        /// Crea la entidad Tempsubscription
        /// </summary>
        /// <param name="policyModel">modelo policy</param>
        /// <returns>policy con los Ids TemporalId y QuotationId</returns>
        public Models.Risk RunRulesRisk(Models.Policy policy, Models.Risk risk, int rulsetId)
        {
            try
            {
                UNBM.Risk businessModelRisk = new UNBM.Risk(policy, risk);
                return businessModelRisk.RunRulesRisk(rulsetId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<Model.InsuredObject> GetInsuredObjects()
        {
            try
            {
                InsuredObjectDAO insuredObjectDAO = new InsuredObjectDAO();
                return insuredObjectDAO.GetInsuredObjects();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public Model.InsuredObject GetInsuredObjectByProductIdGroupCoverageIdInsuredObjectId(int productId, int groupCoverageId, int insuredObjectId)
        {
            try
            {
                InsuredObjectDAO insuredObjectDAO = new InsuredObjectDAO();
                return insuredObjectDAO.GetInsuredObjectByProductIdGroupCoverageIdInsuredObjectId(productId, groupCoverageId, insuredObjectId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public SubCoveredRiskType? GetSubcoverRiskTypeByPrefixIdBranchIdPolicyNumber(int prefixId, int branchId, decimal policyNumber)
        {
            try
            {
                var policyDAO = new PolicyDAO();
                return policyDAO.GetSubcoverRiskTypeByPrefixIdBranchIdPolicyNumber(prefixId, branchId, policyNumber);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Resumen de la prima
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <returns></returns>
        public Models.Summary GetSummaryByEndorsementId(int endorsementId)
        {
            try
            {
                var policyDAO = new PolicyDAO();
                return policyDAO.GetSummaryByEndorsementId(endorsementId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }



        /// <summary>
        /// Obtener Tipo Contrato
        /// </summary>
        /// <returns></returns>
        public List<Models.SuretyContractType> GetSuretyContractTypes()
        {
            try
            {
                SuretyContratTypeDAO suretyContratTypeDAO = new SuretyContratTypeDAO();
                return suretyContratTypeDAO.GetSuretyContractTypes();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorGetContractType), ex);
            }
        }

        public List<Models.SuretyContractCategories> GetSuretyContractCategories()
        {
            try
            {
                SuretyContractCategoriesDAO suretyContractCategoriesDAO = new SuretyContractCategoriesDAO();
                return suretyContractCategoriesDAO.GetSuretyContractCategories();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorGetContractCategories), ex);
            }
        }

        /// <summary>
        /// Obtener Tipo de Riesgo
        /// </summary>
        /// <param name="userId">identificador del Ramo</param>
        /// <returns></returns>
        public List<Models.RiskType> GetRiskTypeByPrefixId(int prefixId)
        {
            try
            {
                RiskTypeDAO riskTypeDao = new RiskTypeDAO();
                return riskTypeDao.GetRiskTypeByPrefixId(prefixId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorGetRiskType), ex);
            }
        }

        /// <summary>
		/// <summary>
        /// Obtener Tipo de Riesgo por placa
        /// </summary>
        /// <param name="description">
        /// <returns> Lista por Location </returns>
        public List<Models.RiskVehicle> GetRisksByPlate(string description)
        {
            try
            {
                RiskVehicleDAO riskVehicle = new RiskVehicleDAO();
                return riskVehicle.GetRisksByPlate(description);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorGetRiskByVehicle));
            }
        }
        /// Consulta los Tipos de endosos habilitados por ramo
        /// </summary>
        /// <param name="prefixId">Identificador del ramo</param>
        /// <param name="isEnabled">Enabled</param>
        /// <returns> Listado de Tipos de endoso </returns>
        public List<Models.PrefixEndoTypeEnabled> GetPrefixEndoEnabledByPrefixIdIsEnabled(int prefixId, bool isEnabled)
        {
            try
            {
                return PrefixEndoTypeEnabledDAO.GetPrefixEndoEnabledByPrefixIdIsEnabled(prefixId, isEnabled);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorGetTypesOfEndososEnabledByBranch), ex);
            }
        }

        /// <summary>
        /// Obtener lista de zonas de tarifación por ramo comercial
        /// </summary>
        /// <param name="prefixId">Id ramo comercial</param>
        /// <returns></returns>
        public List<Model.RatingZone> GetRatingZonesByPrefixId(int prefixId)
        {
            try
            {
                RatingZoneDAO ratingZoneProvider = new RatingZoneDAO();
                return ratingZoneProvider.GetRatingZonesByPrefixId(prefixId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorGetListTariffZonesByPrefix), ex);
            }
        }

        /// <summary>
        /// Obtener lista de zonas de tarifación 
        /// </summary>
        /// <returns></returns>
        public List<Model.RatingZone> GetRatingZones()
        {
            try
            {
                RatingZoneDAO ratingZoneProvider = new RatingZoneDAO();
                return ratingZoneProvider.GetRatingZones();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorGetListTariffZonesByPrefix), ex);
            }
        }

        /// <summary>
        /// Obtener zona de tarifacion 
        /// </summary>
        /// <param name="prefixId">Id ramo comercial</param>
        /// <param name="prefixId">Id pais</param>
        /// <param name="prefixId">Id departamento</param>
        /// <returns></returns>
        public Model.RatingZone GetRatingZonesByPrefixIdCountryIdStateId(int prefixId, int countryId, int stateId)
        {
            try
            {
                RatingZoneDAO ratingZoneProvider = new RatingZoneDAO();
                return ratingZoneProvider.GetRatingZonesByPrefixCodeCountryCodeStateCode(prefixId, countryId, stateId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorGetTariffZone), ex);
            }
        }

        public bool CreatePrefixByLineBusiness(Model.PrefixLineBusiness PrefixLineBusiness)
        {
            try
            {
                PrefixDAO PrefixByLineBusiness = new PrefixDAO();
                return PrefixByLineBusiness.CreatePrefixByLineBusiness(PrefixLineBusiness);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorCreatePrefixByLineBusiness), ex);
            }
        }

        public List<Models.BillingGroup> GetBillingGroup()
        {
            try
            {
                return new BillingGroupDAO().GetBillingGroup();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #region Adicionales

        /// <summary>
        /// Obtiene los planes de pago
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public List<Model.FinancialPlan> GetFinancialPlanByProductId(int productId)
        {
            try
            {
                FinancialPlanDAO financialPlanDAO = new FinancialPlanDAO();
                return financialPlanDAO.GetFinancialPlanByProductId(productId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<Model.GroupCoverage> GetProductCoverageGroupRiskByProductId(int productId)
        {
            try
            {
                return ProductCoverageGroupRiskDAO.GetProductCoverageGroupRiskByProductId(productId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene Riesgo de cobertura de acuerdo al Id del Producto
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public PRODModel.Product GetCoveredProductById(int productId)
        {
            try
            {
                PolicyDAO productDAO = new PolicyDAO();
                return productDAO.GetCoveredProductById(productId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Gets the default payment plan by product identifier.
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public Model.PaymentPlan GetDefaultPaymentPlanByProductId(int productId)
        {
            try
            {
                PaymentPlanDAO paymentPlanDAO = new PaymentPlanDAO();
                return paymentPlanDAO.GetDefaultPaymentPlanByProductId(productId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion

        public RatingZone RatingZoneByRatingZoneCode(int ratingZoneCode)
        {
            try
            {
                RatingZoneDAO ratingZoneDAO = new RatingZoneDAO();
                return ratingZoneDAO.RatingZoneByRatingZoneCode(ratingZoneCode);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetRatingZone), ex);
            }
        }

        /// <summary>
        /// Obtener Coaseguradora
        /// </summary>
        /// <param name="userId">identificador de Coaseguradora</param>
        /// <returns></returns>
        public Models.IssuanceCoInsuranceCompany GetCoInsuranceCompanyByCoinsuranceId(int coInsuranceId)
        {
            try
            {
                CoInsuranceCompanyDAO coInsuranceCompanyProvider = new CoInsuranceCompanyDAO();
                return coInsuranceCompanyProvider.GetCoInsuranceCompanyByCoinsuranceId(coInsuranceId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorGetCoInsurance), ex);
            }
        }

        #region VehicleType

        public List<VehicleType> ExecuteOperationsVehicleType(List<VehicleType> vehicleTypes)
        {
            List<VehicleType> listVehicleTypes = new List<VehicleType>();
            List<VehicleType> createVehicleTypes = new List<VehicleType>();
            List<VehicleType> updateVehicleTypes = new List<VehicleType>();
            List<VehicleType> deleteVehicleTypes = new List<VehicleType>();
            VehicleTypeDAO vehicleTypeDAO = new VehicleTypeDAO();

            createVehicleTypes = vehicleTypes.Where(x => x.State == (int)Status.Create).ToList();
            updateVehicleTypes = vehicleTypes.Where(x => x.State == (int)Status.Update).ToList();
            deleteVehicleTypes = vehicleTypes.Where(x => x.State == (int)Status.Delete).ToList();

            if (createVehicleTypes.Count > 0)
            {
                foreach (VehicleType item in createVehicleTypes)
                {
                    VehicleType result = new VehicleType();
                    result = vehicleTypeDAO.CreateVehicleType(item);
                    result.State = (int)Status.Create;
                    listVehicleTypes.Add(result);
                }
            }

            if (updateVehicleTypes.Count > 0)
            {
                foreach (VehicleType item in updateVehicleTypes)
                {
                    vehicleTypeDAO.UpdateVehicleType(item);
                    item.State = (int)Status.Update;
                    listVehicleTypes.Add(item);
                }
            }

            if (deleteVehicleTypes.Count > 0)
            {
                VehicleBodyDAO vehicleBodyDAO = new VehicleBodyDAO();
                foreach (VehicleType item in deleteVehicleTypes)
                {
                    vehicleBodyDAO.DeleteVehicleTypeBodies(item.Id);
                }

                foreach (VehicleType item in deleteVehicleTypes)
                {
                    vehicleTypeDAO.DeleteVehicleType(item);
                }
            }

            return listVehicleTypes;
        }

        public string GenerateFileToVehicleBody(VehicleType vehicleType, string fileName)
        {
            VehicleTypeDAO vehicleTypeDAO = new VehicleTypeDAO();
            VehicleBodyDAO vehicleBodyDAO = new VehicleBodyDAO();
            try
            {
                return vehicleBodyDAO.GenerateFileToExportVehicleBody(vehicleType, fileName);
            }
            catch (Exception ex)
            {

                throw new Exception("Error in GenerateFileToVehicleBody", ex);
            }
        }

        public string GenerateFileToVehicleType(string fileName)
        {
            VehicleTypeDAO vehicleTypeDAO = new VehicleTypeDAO();
            try
            {
                return vehicleTypeDAO.GenerateFileToVehicleType(fileName);
            }
            catch (Exception ex)
            {

                throw new Exception("Error in GenerateFileToVehicleType", ex);
            }
        }

        public List<VehicleType> GetVehicleTypes()
        {
            try
            {
                VehicleTypeDAO vehicleTypeDAO = new VehicleTypeDAO();
                VehicleBodyDAO vehicleBodyDAO = new VehicleBodyDAO();
                List<VehicleType> vehicleTypes = new List<VehicleType>();
                vehicleTypes = vehicleTypeDAO.GetVehicleTypes();

                foreach (VehicleType vehicleType in vehicleTypes)
                {
                    vehicleType.VehicleBodies = vehicleBodyDAO.GetVehicleTypeBodiesByVehicleType(vehicleType.Id);
                }

                return vehicleTypes;
            }
            catch (Exception ex)
            {

                throw new Exception("Error in GetVehicleTypes ", ex);
            }
        }


        #endregion

        #region CoCoverage
        /// <summary>
        /// CreateBusinessCoCoverage: metodo que inserta un registro en la tabla QUO.CO_COVERAGE_VALUE 
        /// </summary>
        /// <param name="paramCoCoverageValue"></param>
        /// <returns></returns>
        public ParamCoCoverageValue CreateCoCoverageValue(ParamCoCoverageValue paramCoCoverageValue)
        {
            try
            {
                CoCoverageValueBusiness coCoverageValueBusiness = new CoCoverageValueBusiness();
                return coCoverageValueBusiness.CreateBusinessCoCoverage(paramCoCoverageValue);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// GetApplicationBusinessCoverageValueByPrefixId: metodo que consulta listado de valores de cobertura a partir del id de prefix ingresada en la busqueda simple table: QUO.CO_COVERAGE_VALUE
        /// </summary>
        /// <param name="Description"></param>
        /// <returns></returns>
        public List<ParamCoCoverageValue> GetCoCoverageValueByPrefixId(int prefixId)
        {
            try
            {
                CoCoverageValueBusiness coCoverageValueBusiness = new CoCoverageValueBusiness();
                return coCoverageValueBusiness.GetApplicationBusinessCoverageValueByPrefixId(prefixId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// GetBusinessCoverageValueAdv: metodo que consulta el listado de coberturas a aprtir de los filtros ingresados en la busuqeda avanzada, QUO.CO_COVERAGE_VALUE
        /// </summary>
        /// <param name="paramCoCoverageValue"></param>
        /// <returns></returns>
        public List<ParamCoCoverageValue> GetCoCoverageValueAdv(ParamCoCoverageValue paramCoCoverageValue)
        {
            try
            {
                CoCoverageValueBusiness coCoverageValueBusiness = new CoCoverageValueBusiness();
                return coCoverageValueBusiness.GetBusinessCoverageValueAdv(paramCoCoverageValue);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// GenerateFileBusinessToCoverageValue: metodo que genera el archivo excel del listado de coberturas tabla QUO.CO_COVERAGE_VALUE
        /// </summary>
        /// <returns></returns>
        public string GenerateFileToCoCoverageValue(string fileName)
        {
            try
            {
                ExcelFileServiceModel excelFileServiceModel = new ExcelFileServiceModel();
                CoCoverageValueBusiness coCoverageValueBusiness = new CoCoverageValueBusiness();
                return excelFileServiceModel.FileData = coCoverageValueBusiness.GenerateFileBusinessToCoverageValue(fileName);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// GetBusinnesCoCoverageValue: metodo que consulta listado completo de coberturas  QUO.CO_COVERAGE_VALUE
        /// </summary>
        /// <returns></returns>
        public List<ParamCoCoverageValue> GetCoCoverageValue()
        {
            try
            {
                CoCoverageValueBusiness coCoverageValueBusiness = new CoCoverageValueBusiness();
                return coCoverageValueBusiness.GetAllCoCoverageValue();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// UpdateBusinessCoCoverageValue metodo que actualiza la informacion de una cobertura
        /// </summary>
        /// <param name="paramCoCoverageValue"></param>
        /// <returns></returns>
        public ParamCoCoverageValue UpdateCoCoverageValue(ParamCoCoverageValue paramCoCoverageValue)
        {
            try
            {
                CoCoverageValueBusiness coCoverageValueBusiness = new CoCoverageValueBusiness();
                return coCoverageValueBusiness.UpdateBusinessCoCoverageValue(paramCoCoverageValue);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// DeleteBusinessCoCoverageValue: metodo que elimina una cobertura
        /// </summary>
        /// <param name="paramCoCoverageValue"></param>
        /// <returns></returns>
        public string DeleteCoCoverageValue(ParamCoCoverageValue paramCoCoverageValue)
        {
            try
            {
                CoCoverageValueBusiness coCoverageValueBusiness = new CoCoverageValueBusiness();
                return coCoverageValueBusiness.DeleteBusinessCoCoverageValue(paramCoCoverageValue);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// GetCoverageByPrefixId: metodo que consulta el listado de coberturas a partir del prefix y linebusiness
        /// </summary>
        /// <param name="prefixId"></param>
        /// <returns></returns>
        public List<BaseParamCoverage> GetCoverageByPrefixId(int prefixId)
        {
            CoCoverageValueBusiness coCoverageValueBusiness = new CoCoverageValueBusiness();
            return coCoverageValueBusiness.GetCoverageByPrefixId(prefixId);
        }
        #endregion
        #region Conditional Text
        public ParamConditionText CreateBusinessConditiontext(ParamConditionText conditionText)
        {
            ConditionTextDAO ConditionTextDao = new ConditionTextDAO();
            return ConditionTextDao.CreateConditiontext(conditionText);
        }

        public ParamConditionText UpdateBusinessConditiontext(ParamConditionText conditionText)
        {
            ConditionTextDAO ConditionTextDao = new ConditionTextDAO();
            return ConditionTextDao.UpdateConditiontext(conditionText);
        }

        public string DeleteBusinessConditiontext(ParamConditionText conditionText)
        {
            ConditionTextDAO ConditionTextDao = new ConditionTextDAO();
            return ConditionTextDao.DeleteConditiontext(conditionText);
        }

        public List<ParamConditionText> GetBusinessConditiontext()
        {
            List<string> errorListDescription = new List<string>();

            ConditionTextDAO ConditionTextDao = new ConditionTextDAO();
            return ConditionTextDao.GetConditiontext();

        }

        public List<ParamConditionText> GetBusinessConditiontextByDescription(int integer = 0, string description = "")
        {
            ConditionTextDAO ConditionTextDao = new ConditionTextDAO();
            return ConditionTextDao.GetConditiontextByDescription(integer, description);

        }

        public string GenerateFileBusinessToConditiontext(string fileName)
        {
            var ConditionText = GetBusinessConditiontext();
            ConditionTextDAO ConditionTextDao = new ConditionTextDAO();
            return ConditionTextDao.GenerateFileToConditiontext(ConditionText, fileName);
        }
        #endregion

        #region Tax

        #region TaxInferface
        public ParamTax CreateTax(ParamTax paramTax)
        {
            try
            {
                TaxBusiness taxBusiness = new TaxBusiness();
                return taxBusiness.CreateTax(paramTax);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public ParamTax UpdateTax(ParamTax paramTax)
        {
            try
            {
                TaxBusiness taxBusiness = new TaxBusiness();
                return taxBusiness.UpdateTax(paramTax);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<ParamTax> GetAplicationTaxByDescription(string description)
        {
            try
            {
                TaxBusiness taxBusiness = new TaxBusiness();
                return taxBusiness.GetByDescriptionTax(description);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<ParamTax> GetTaxByIdAndDescription(int taxId, string taxDescription)
        {
            try
            {
                TaxBusiness taxBusiness = new TaxBusiness();
                return taxBusiness.GetByTaxIdAndDescription(taxId, taxDescription);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public MODPA.ExcelFileServiceModel GenerateTaxFileReport(int taxId)
        {
            try
            {
                TaxBusiness taxBusiness = new TaxBusiness();
                return taxBusiness.GenerateTaxFileReport(taxId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }
        #endregion

        #region TaxRateInterface

        public ParamTaxRate CreateTaxRate(ParamTaxRate paramTaxRate)
        {
            try
            {
                TaxBusiness taxBusiness = new TaxBusiness();
                return taxBusiness.CreateTaxRate(paramTaxRate);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public ParamTaxRate UpdateTaxRate(ParamTaxRate paramTaxRate)
        {
            try
            {
                TaxBusiness taxBusiness = new TaxBusiness();
                return taxBusiness.UpdateTaxRate(paramTaxRate);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<ParamTaxRate> GetTaxRatesByTaxId(int taxId)
        {
            try
            {
                TaxBusiness taxBusiness = new TaxBusiness();
                return taxBusiness.GetTaxRatesByTaxId(taxId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }

        public ParamTaxRate GetBusinessTaxRateByTaxIdbyAttributes(int taxId, int? taxConditionId, int? taxCategoryId, int? countryCode, int? stateCode, int? cityCode, int? economicActivityCode, int? prefixId, int? coverageId, int? technicalBranchId)
        {
            try
            {
                TaxBusiness taxBusiness = new TaxBusiness();
                return taxBusiness.GetBusinessTaxRateByTaxIdbyAttributes(taxId, taxConditionId, taxCategoryId, countryCode, stateCode, cityCode, economicActivityCode, prefixId, coverageId, technicalBranchId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public ParamTaxRate GetBusinessTaxRateById(int taxRateId)
        {
            try
            {
                TaxBusiness taxBusiness = new TaxBusiness();
                return taxBusiness.GetBusinessTaxRateById(taxRateId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

        /// <summary>
        /// Obtener los tipos de documentos
        /// </summary>
        /// <param name="typeDocument">tipo de documento
        /// 1. persona natural
        /// 2. persona juridica
        /// 3. todos</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public List<IssuanceDocumentType> GetDocumentTypes(int typeDocument)
        {
            try
            {
                DocumentTypeDAO documentypeDAO = new DocumentTypeDAO();
                return documentypeDAO.GetDocumentTypes(typeDocument);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        public List<IssuanceAgency> GetAgencyAll()
        {
            try
            {
                return new AgencyDAO().GetAgencyAll();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<Clause> GetClauseAll()
        {
            try
            {
                return new ClauseDAO().GetClauseAll();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public IssuanceAgency GetAgencyByAgentCodeAgentTypeCode(int agentCode, int agentTypeId)
        {
            try
            {
                AgencyDAO agencyDAO = new AgencyDAO();
                return agencyDAO.GetAgencyByAgentCodeAgentTypeCode(agentCode, agentTypeId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #region TaxCategoryInterface

        public ParamTaxCategory CreateTaxCategory(ParamTaxCategory paramTaxCategory)
        {
            try
            {
                TaxBusiness taxBusiness = new TaxBusiness();
                return taxBusiness.CreateTaxCategory(paramTaxCategory);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public ParamTaxCategory UpdateTaxCategory(ParamTaxCategory paramTaxCategory)
        {
            try
            {
                TaxBusiness taxBusiness = new TaxBusiness();
                return taxBusiness.UpdateTaxCategory(paramTaxCategory);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<ParamTaxCategory> GetTaxCategoriesByTaxId(int taxId)
        {
            try
            {
                TaxBusiness taxBusiness = new TaxBusiness();
                return taxBusiness.GetTaxCategoriesByTaxId(taxId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public bool DeleteTaxCategoriesByTaxId(int categoryId, int taxId)
        {
            try
            {
                TaxBusiness taxBusiness = new TaxBusiness();
                return taxBusiness.DeleteTaxCategoriesByTaxId(categoryId, taxId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

        #region TaxConditionInterface

        public ParamTaxCondition CreateTaxCondition(ParamTaxCondition paramTaxCondition)
        {
            try
            {
                TaxBusiness taxBusiness = new TaxBusiness();
                return taxBusiness.CreateTaxCondition(paramTaxCondition);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public ParamTaxCondition UpdateTaxCondition(ParamTaxCondition paramTaxCondition)
        {
            try
            {
                TaxBusiness taxBusiness = new TaxBusiness();
                return taxBusiness.UpdateTaxCondition(paramTaxCondition);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<ParamTaxCondition> GetTaxConditionsByTaxId(int taxId)
        {
            try
            {
                TaxBusiness taxBusiness = new TaxBusiness();
                return taxBusiness.GetTaxConditionsByTaxId(taxId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }
        public bool DeleteTaxConditionsByTaxId(int conditionId, int taxId)
        {
            try
            {
                TaxBusiness taxBusiness = new TaxBusiness();
                return taxBusiness.DeleteTaxConditionsByTaxId(conditionId, taxId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

        #endregion

        #region Guarantee
        public List<Models.IssuanceGuarantee> GetInsuredGuaranteesByIndividualId(int id)
        {
            try
            {
                GuaranteeDAO guarantee = new GuaranteeDAO();
                return guarantee.GetInsuredGuaranteesByIndividualId(id);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<IssuanceGuarantee> GetCounterGuaranteesByIndividualId(int individualId)
        {

            try
            {
                GuaranteeDAO guarantee = new GuaranteeDAO();
                return guarantee.GetCounterGuaranteesByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

        /// <summary>
        /// Grabado Integracion
        /// </summary>
        /// <param name="endorsemenId"></param>
        /// <param name="operationId"></param>
        /// <param name="isMassive"></param>
        public void SaveControlPolicy(int policyId, int endorsemenId, int operationId, int policyOrigin)
        {
            try
            {
                PolicyDAO policyDAO = new PolicyDAO();
                policyDAO.SaveControlPolicy(policyId, endorsemenId, operationId, policyOrigin);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<PolicyType> GetPolicyTypeAll()
        {
            try
            {
                return new PolicyTypeDAO().GetPolicyTypeAll();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #region Endorsement
        /// <summary>
        /// Obtener Endosos de una Póliza
        /// </summary>
        /// <param name="policyNumber">Número de Póliza</param>
        /// <returns>Endosos</returns>

        public List<Endorsement> GetEndorsementsAvaibleByPolicyId(int policyId)
        {
            try
            {
                PolicyDAO policyDAO = new PolicyDAO();
                return policyDAO.GetEndorsementsAvaibleByPolicyId(policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetClaimsPoliciesByPolicies), ex);
            }
        }

        #endregion

        #region LigthQuotation
        public IssuanceAgency GetIssuanceAgencyByUserId(int userId)
        {
            try
            {
                AgencyDAO agencyDAO = new AgencyDAO();
                return agencyDAO.GetAgencyByUserId(userId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetIssuanceAgencyByUserId), ex);
            }
        }
        #endregion

        public List<Endorsement> GetPolicyEndorsementsByPrefixIdBranchIdPolicyNumber(int prefixId, int branchId, decimal policyNumber)
        {
            try
            {
                return new PolicyDAO().GetPolicyEndorsementsByPrefixIdBranchIdPolicyNumber(prefixId, branchId, policyNumber);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<Endorsement> GetPolicyEndorsementsWithPremiumByPolicyId(int policyId)
        {
            try
            {
                return new PolicyDAO().GetPolicyEndorsementsWithPremiumByPolicyId(policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public InsuredObject GetInsuredObjectByInsuredObjectId(int insuredObjectId)
        {
            try
            {

                var insured = InsuredObjectDAO.GetInsuredObjectByInsuredObjectId(insuredObjectId);
                InsuredObject insuredObject = new InsuredObject
                {
                    Id = insured.InsuredObjectId,
                    Description = insured.Description,
                };
                return insuredObject;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #region Claim 

        public List<Policy> GetPoliciesByPolicyPersonTypeIdModuleType(Policy policy, int? personTypeId, ModuleType moduleType)
        {
            try
            {
                PolicyDAO policyDAO = new PolicyDAO();
                List<Policy> policies = new List<Policy>();
                switch (moduleType)
                {
                    case ModuleType.Emission:
                        policies = policyDAO.GetPoliciesByPolicy(policy);
                        break;
                    case ModuleType.Claim:
                        policies = policyDAO.GetClaimPoliciesByPolicyPersontypeId(policy, personTypeId);
                        break;
                }

                return policies;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetClaimsPoliciesByPolicies), ex);
            }
        }

        public Policy GetClaimPolicyByEndorsementId(int endorsementId)
        {
            try
            {
                PolicyDAO policyDAO = new PolicyDAO();
                return policyDAO.GetClaimPolicyByEndorsementid(endorsementId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetClaimsPoliciesByPolicies), ex);
            }
        }

        /// <summary>
        /// Consulta la información de las coberturas vigentes asociadas al riesgo
        /// </summary>
        /// <param name="riskId">Identificador del riesgo</param>
        /// <returns></returns>
        public List<Coverage> GetCoveragesByRiskId(int riskId)
        {
            try
            {
                CoverageDAO coverageDAO = new CoverageDAO();
                return coverageDAO.GetCoveragesByRiskId(riskId);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCoveragesByRiskId), ex);
            }
        }

        /// <summary>
        /// Consulta la información de las coberturas vigentes asociadas al riesgo según fecha de ocurrencia del siniestro y muestra las 
        /// sumas aseguradas según el porcentaje de participación de la compañía en el siniestro
        /// </summary>
        /// <param name="riskId"></param>
        /// <param name="occurrenceDate"></param>
        /// <param name="companyParticipationPercentage"></param>
        /// <returns></returns>
        public List<Coverage> GetCoveragesByRiskIdOccurrenceDateCompanyParticipationPercentage(int riskId, DateTime? occurrenceDate, decimal companyParticipationPercentage)
        {
            try
            {
                CoverageDAO coverageDAO = new CoverageDAO();
                return coverageDAO.GetCoveragesByRiskIdOccurrenceDateCompanyParticipationPercentage(riskId, occurrenceDate, companyParticipationPercentage);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCoveragesByRiskId), ex);
            }
        }

        public List<Coverage> GetCoveragesByLineBusinessId(int lineBusinessId)
        {
            try
            {
                CoverageDAO coverageDAO = new CoverageDAO();
                return coverageDAO.GetCoveragesByLineBusinessId(lineBusinessId);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCoveragesByRiskId), ex);
            }
        }

        public List<Coverage> GetCoveragesByLineBusinessIdSubLineBusinessId(int lineBusinessId, int subLineBusinessId)
        {
            try
            {
                CoverageDAO coverageDAO = new CoverageDAO();
                return coverageDAO.GetCoveragesByLineBusinessIdSubLineBusinessId(lineBusinessId, subLineBusinessId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCoverages), ex);
            }
        }

        public List<Deductible> GetDeductiblesByPolicyIdRiskNumCoverageIdCoverageNumber(int policyId, int riskNum, int coverageId, int coverNum)
        {
            try
            {
                DeductibleDAO deductibleDAO = new DeductibleDAO();
                return deductibleDAO.GetDeductiblesByPolicyIdRiskNumCoverageIdCoverageNumber(policyId, riskNum, coverageId, coverNum);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetDeductibles), ex);
            }
        }

        public List<CoInsuranceAssigned> GetCoInsuranceByPolicyIdByEndorsementId(int policyId, int endorsementId)
        {
            try
            {
                CoInsuranceDAO coInsuranceDAO = new CoInsuranceDAO();
                return coInsuranceDAO.GetCoInsuranceByPolicyIdByEndorsementId(policyId, endorsementId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetDeductibles), ex);
            }
        }
        public Policy GetPolicyByPolicyId(int policyId)
        {
            try
            {
                PolicyDAO policyDAO = new PolicyDAO();
                return policyDAO.GetPolicyByPolicyId(policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion
        #region Obtener Combos
        public ComboListDTO GetRiskListsByProductId(int productId)
        {
            List<TR.Task> task = new List<TR.Task>();
            TR.Task<List<SuretyContractType>> contractTypes = TP.Task.Run(() => GetSuretyContractTypes());
            TR.Task<List<SuretyContractCategories>> contractCategories = TP.Task.Run(() => GetSuretyContractCategories());
            TR.Task<List<GroupCoverage>> riskByProduct = null;
            if (productId > 0)
            {
                riskByProduct = TP.Task.Run(() => GetProductCoverageGroupRiskByProductId(productId));
            }
            TR.Task.WaitAll(contractTypes, contractCategories);
            ComboListDTO comboListDTO = new ComboListDTO();
            if (riskByProduct != null)
            {
                riskByProduct.Wait();
            }
            comboListDTO.ContractTypes = ModelAssembler.CreateSuretyContractTypes(contractTypes.Result);
            comboListDTO.ContractCategories = ModelAssembler.CreateContractCategories(contractCategories.Result);
            comboListDTO.GroupCoverages = ModelAssembler.CreateRiskGroupCoverages(riskByProduct?.Result);
            return comboListDTO;
        }
        #endregion

        public Summary CalculateSummaryPropertyDeclaration(Policy policy, List<Risk> risks, int riskId, int insuredObjectId)
        {
            try
            {
                PolicyDAO policyDAO = new PolicyDAO();
                return policyDAO.CalculateSummary(policy, risks, riskId, insuredObjectId);
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        #region 
        public List<DynamicConcept> GetDynamicConceptsByEndorsementIdRiskNumPolicyIdRiskId(int endorsementId, int riskNum, int policyId, int riskId)
        {
            try
            {
                DynamicConceptDAO dynamicConceptDAO = new DynamicConceptDAO();
                return dynamicConceptDAO.GetDynamicConceptsByEndorsementIdRiskNumPolicyIdRiskId(endorsementId, riskNum, policyId, riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion

        public List<Risk> GetRisksByPolicyIdEndorsmentId(int policyId, int endorsementId)
        {
            RiskDAO riskDAO = new RiskDAO();
            List<Risk> risks = new List<Risk>();
            List<Risk> riskSureties = new List<Risk>();
            return risks = riskDAO.GetRisksByPolicyIdEndorsmentId(policyId, endorsementId);
        }

        public Risk GetRiskSuretyByRiskId(int riskId)
        {
            RiskDAO riskDAO = new RiskDAO();
            Risk riskSurety = new Risk();
            riskSurety = riskDAO.GetRiskSuretyByRiskId(riskId);
            return riskSurety;
        }


        #region tomador
        public IssuanceInsured GetHolderValidateByIndividualId(int individualId)
        {
            try
            {
                return HolderBusiness.GetHolderByIndividualId(individualId);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }

        }

        #endregion tomador

        #region CoEquivalenceCoverage
        /// <summary>
        /// Obtener coberturas equivalentes
        /// </summary>
        /// <param name="policyId">Id Póliza</param>
        /// <returns>Endosos</returns>
        public List<IntCoEquivalenceCoverage> GetCoEquivalenceCoverage(int coverageId)
        {
            try
            {
                IntCoEquivalenceCoverageDAO intCoEquivalenceCoverageDAO = new IntCoEquivalenceCoverageDAO();
                return intCoEquivalenceCoverageDAO.GetCoEquivalenceCoverage(coverageId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion CoEquivalenceCoverage



        /// <summary>
        /// Obtener coberturas equivalentes
        /// </summary>
        /// <param name="policyId">Id Póliza</param>
        /// <returns>Endosos</returns>
        public List<Endorsement> GetPoliciesByGuaranteeId(int guaranteeId)
        {
            try
            {
                GuaranteeDAO guaranteeDAO = new GuaranteeDAO();
                return guaranteeDAO.GetPoliciesByGuaranteeId(guaranteeId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public int GetPolicyIdByEndormestId(int endormestId)
        {
            try
            {
                PolicyDAO policyDAO = new PolicyDAO();
                return policyDAO.GetPolicyIdByEndormestId(endormestId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public PortfolioPolicy GetPortfolioPolicyByPolicy(int branch, int prefix, string documentNumber)
        {
            try
            {
                PolicyDAO policyDAO = new PolicyDAO();
                return policyDAO.GetPortfolioPolicyByPolicy(branch, prefix, documentNumber);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<PortfolioPolicy> GetPortfolioPolicyByPerson(List<PortfolioPolicy> portfolioPolicies)
        {
            try
            {
                PolicyDAO policyDAO = new PolicyDAO();
                return policyDAO.GetPortfolioPolicyByPerson(portfolioPolicies);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}