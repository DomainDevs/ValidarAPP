using Newtonsoft.Json;
using Sistran.Company.Application.ProductServices.Models;
using Sistran.Company.Application.UnderwritingBusinessServiceProvider.Business;
using Sistran.Company.Application.UnderwritingBusinessServiceProvider.DAO;
using Sistran.Company.Application.UnderwritingServices.DTOs;
using Sistran.Company.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Company.Application.UnderwritingServices.EEProvider.Business;
using Sistran.Company.Application.UnderwritingServices.EEProvider.DAO;
using Sistran.Company.Application.UnderwritingServices.EEProvider.DAOs;
using Sistran.Company.Application.UnderwritingServices.EEProvider.Resources;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Enums;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.ModelServices.Enums;
using Sistran.Core.Application.ModelServices.Models.Param;
using Sistran.Core.Application.ProductServices.Models;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.EEProvider;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using COMUT = Sistran.Company.Application.Utilities.Helpers;
using ENUM = Sistran.Company.Application.UnderwritingServices.Enums;
using EnumsCore = Sistran.Core.Application.UnderwritingServices.Enums;
using MODSM = Sistran.Core.Application.ModelServices.Models.Param;
using UTIMO = Sistran.Core.Application.Utilities.Error;
using TP = Sistran.Core.Application.Utilities.Utility;
using Sistran.Core.Application.UnderwritingServices.DTOs.Filter;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class UnderwritingServiceEEProvider : UnderwritingServiceEEProviderCore, IUnderwritingService
    {
        /// <summary>
        /// Insertar en tablas temporales desde el JSON
        /// </summary>
        /// <param name="policy">Modelo policy</param>
        public CompanyPolicy CreatePolicyTemporal(CompanyPolicy policy, bool isMasive)
        {
            try
            {
                PolicyBusiness policyBusiness = new PolicyBusiness();
                return policyBusiness.CreatePolicyTemporal(policy, isMasive);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Eliminar riesgo
        /// </summary>
        /// <param name="operationId">Id operacion</param>
        /// <returns>Resultado</returns>
        public bool DeleteRisk(int operationId)
        {
            try
            {
                RiskDAO riskDAO = new RiskDAO();
                return riskDAO.DeleteRisk(operationId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Eliminar riesgo
        /// </summary>
        /// <param name="operationId">Id operacion</param>
        /// <returns>Resultado</returns>
        public bool DeleteCompanyRisksByRiskId(int riskId, bool isMasive)
        {
            try
            {
                RiskDAO riskDAO = new RiskDAO();
                return riskDAO.DeleteCompanyRisksByRiskId(riskId, isMasive);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Póliza
        /// </summary>
        /// <param name="temporalId">Id Temporal</param>
        /// <returns>Póliza</returns>
        public CompanyPolicy GetCompanyPolicyByTemporalId(int temporalId, bool isMasive)
        {
            try
            {
                PolicyDAO policyDAO = new PolicyDAO();
                return policyDAO.GetCompanyPolicyByTemporalId(temporalId, isMasive);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<EndorsementCompanyDTO> GetEndorsementsByPrefixIdBranchIdPolicyNumberCompany(int branchId, int prefixId, decimal policyNumber)
        {
            try
            {
                PolicyDAO policyDAO = new PolicyDAO();
                return policyDAO.GetEndorsementsByPrefixIdBranchIdPolicyNumberCompany(branchId, prefixId, policyNumber);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener riesgos
        /// </summary>
        /// <param name="temporalId">Id Temporal</param>
        /// <returns>Póliza</returns>
        public List<CompanyRisk> GetCompanyRisksByTemporalId(int temporalId, bool isMasive)
        {
            try
            {
                RiskDAO riskDAO = new RiskDAO();
                return riskDAO.GetCompanyRisksByTemporalId(temporalId, isMasive);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        /// <summary>
        /// Obtener Riesgos
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="isMasive">if set to <c>true</c> [is masive].</param>
        /// <returns></returns>
        public List<CompanyRisk> GetCiaRiskByTemporalId(int temporalId, bool isMasive)
        {
            try
            {
                RiskDAO riskDAO = new RiskDAO();
                return riskDAO.GetCiaRiskByTemporalId(temporalId, isMasive);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener lista de clausulas por cobertura
        /// </summary>
        /// <param name="CoverageId">cobertura</param>
        /// <returns></returns>
        public List<CompanyClause> GetClausesByCoverageId(int CoverageId)
        {
            try
            {
                var clauseDAO = new ClauseDAO();
                return clauseDAO.GetClausesByCoverageId(CoverageId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener lista de tipos de detalle por cobertura
        /// </summary>
        /// <param name="CoverageId">cobertura</param>
        /// <returns></returns>
        public List<CoverDetailType> GetCoverDetailTypesByCoverageId(int CoverageId)
        {
            try
            {
                var detailTypeDAO = new CoverDetailTypeDAO(DataFacadeManager.Instance.GetDataFacade());
                return detailTypeDAO.GetCoverDetailTypesByCoverageId(CoverageId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public string GenerateFileToCompanyInsuredObject(List<CompanyInsuredObject> companyInsuredObjects, string fileName)
        {
            try
            {
                InsuredObjectBusiness insuredObjectBusiness = new InsuredObjectBusiness();
                return insuredObjectBusiness.GenerateFileToCompanyInsuredObject(companyInsuredObjects, fileName);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyPolicy RunRulesCompanyPolicy(CompanyPolicy companyPolicy, int ruleId)
        {
            try
            {
                PolicyBusiness businessPolicy = new PolicyBusiness();
                return businessPolicy.RunRulesCompanyPolicy(companyPolicy, ruleId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public decimal GetMinimumPremiumAmountByModelDynamicConcepts(List<DynamicConcept> modelDynamicConcepts)
        {
            try
            {
                var coverageBusinessModel = new CoverageBusiness();
                return coverageBusinessModel.GetMinimumPremiumAmount(modelDynamicConcepts);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public bool GetProrateMinimumPremiumByModelDynamicConcepts(List<DynamicConcept> modelDynamicConcepts)
        {
            try
            {
                var coverageBusinessModel = new CoverageBusiness();
                return coverageBusinessModel.GetProrateMinimumPremium(modelDynamicConcepts);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyCoverage> GetCompanyCoveragesByInsuredObjectIdsGroupCoverageIdProductId(List<int> insuredObjectsIds, int groupCoverageId, int productId, bool filterSelected)
        {
            try
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                return coverageBusiness.GetCompanyCoveragesByInsuredObjectIdsGroupCoverageIdProductId(insuredObjectsIds, groupCoverageId, productId, filterSelected);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyCoverage> GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(int policyId, int endorsementId, int riskId)
        {
            try
            {
                CoverageDAO coverageDAO = new CoverageDAO();
                List<CompanyCoverage> listCoverageRisk = coverageDAO.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(policyId, endorsementId, riskId);
                List<CompanyCoverage> listCoveragePost = coverageDAO.getCoberturaPostContractualPrv();
                for (int i = 0; i < listCoverageRisk.Count(); i++)
                {
                    if (listCoveragePost.Any(x => x.Id == listCoverageRisk[i].Id))
                    {
                        listCoverageRisk[i].IsPostcontractual = true;
                    }
                }
                return listCoverageRisk;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Póliza Por Identificador
        /// </summary>
        /// <param name="endorsementId">Id Endoso</param>
        /// <returns>Póliza</returns>
        public CompanyPolicy GetCompanyPolicyByEndorsementId(int endorsementId)
        {
            try
            {
                PolicyDAO policyDAO = new PolicyDAO();
                return policyDAO.GetCompanyPolicyByEndorsementId(endorsementId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyCoverage> GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(int productId, int groupCoverageId, int prefixId)
        {
            try
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                CoverageDAO coverageDAO = new CoverageDAO();
                List<CompanyCoverage> listCoverages = coverageBusiness.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(productId, groupCoverageId, prefixId);
                if (prefixId == (int)Enums.PrefixType.Surety)
                {
                    List<CompanyCoverage> listCoveragePost = coverageDAO.getCoberturaPostContractualPrv();
                    foreach (CompanyCoverage companyCoverage in listCoverages)
                    {
                        companyCoverage.IsPostcontractual = listCoveragePost.Any(x => x.Id == companyCoverage.Id);
                    }
                }
                return listCoverages;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyCoverage> GetDeductiblesByCompanyCoverages(List<CompanyCoverage> companyCoverage)
        {
            try
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                return coverageBusiness.GetDeductiblesByCompanyCoverages(companyCoverage);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public void CalculateCompanyPremiumDeductible(CompanyCoverage companyCoverage)
        {
            try
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                coverageBusiness.CalculateCompanyPremiumDeductible(companyCoverage);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyCoverage GetCompanyCoverageByCoverageIdProductIdGroupCoverageId(int coverageId, int productId, int groupCoverageId)
        {
            try
            {
                CoverageDAO coverageDAO = new CoverageDAO();
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                CompanyCoverage companyCoverage = coverageBusiness.GetCompanyCoverageByCoverageIdProductIdGroupCoverageId(coverageId, productId, groupCoverageId);
                if (companyCoverage != null)
                {
                    List<CompanyCoverage> listCoveragePost = coverageDAO.getCoberturaPostContractualPrv();
                    if (listCoveragePost.Any(x => x.Id == companyCoverage.Id))
                    {
                        companyCoverage.IsPostcontractual = true;
                    }
                }
                return companyCoverage;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyCoverage> GetCompanyCoveragesAccessoriesByProductIdGroupCoverageIdPrefixId(int productId, int groupCoverageId, int prefixId)
        {
            try
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                return coverageBusiness.GetCompanyCoveragesAccessoriesByProductIdGroupCoverageIdPrefixId(productId, groupCoverageId, prefixId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyCoverage QuotateCompanyCoverage(CompanyCoverage companyCoverage, int policyId, int riskId, int decimalQuantity, int? CoveredRiskType, int? prefixId)
        {
            try
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                return coverageBusiness.QuotateCompanyCoverage(companyCoverage, policyId, riskId, decimalQuantity, CoveredRiskType, prefixId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyCoverage> GetCompanyCoveragesByInsuredObjectIdGroupCoverageIdProductId(int insuredObjectId, int groupCoverageId, int productId)
        {
            try
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                return coverageBusiness.GetCompanyCoveragesByInsuredObjectIdGroupCoverageIdProductId(insuredObjectId, groupCoverageId, productId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyCoverage> GetCompanyCoverageByCoverageIdsProductIdGroupCoverageId(List<int> coverageIds, int productId, int groupCoverageId)
        {
            try
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                return coverageBusiness.GetCompanyCoverageByCoverageIdsProductIdGroupCoverageId(coverageIds, productId, groupCoverageId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyCoverage GetCompanyCoverageByRiskCoverageId(int riskCoverageId)
        {
            try
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                return coverageBusiness.GetCompanyCoverageByRiskCoverageId(riskCoverageId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyInsuredObject> GetCompanyInsuredObjects()
        {
            try
            {
                InsuredObjectBusiness insuredObjectBusiness = new InsuredObjectBusiness();
                return insuredObjectBusiness.GetCompanyInsuredObjects();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyInsuredObject> GetCompanyInsuredObjectsByProductIdGroupCoverageId(int productId, int groupCoverageId, int prefixId)
        {
            try
            {
                InsuredObjectBusiness insuredObjectBusiness = new InsuredObjectBusiness();
                return insuredObjectBusiness.GetCompanyInsuredObjectsByProductIdGroupCoverageId(productId, groupCoverageId, prefixId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyCoverage> GetAllyCompanyCoveragesByCoverageIdProductIdGroupCoverageId(int coverageId, int productId, int groupCoverageId)
        {
            try
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                return coverageBusiness.GetAllyCompanyCoveragesByCoverageIdProductIdGroupCoverageId(coverageId, productId, groupCoverageId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyCoverage> GetAddCompanyCoveragesByCoverageIdProductIdGroupCoverageId(int coverageId, int productId, int groupCoverageId)
        {
            try
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                return coverageBusiness.GetAddCompanyCoveragesByCoverageIdProductIdGroupCoverageId(coverageId, productId, groupCoverageId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyInsuredObject> GetCompanyInsuredObjectByPrefixIdList(int prefixId)
        {
            try
            {
                InsuredObjectBusiness insuredObjectBusiness = new InsuredObjectBusiness();
                return insuredObjectBusiness.GetCompanyInsuredObjectByPrefixIdList(prefixId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyCoverage> GetCompanyCoveragesByTechnicalPlanId(int technicalPlanId)
        {
            try
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                return coverageBusiness.GetCompanyCoveragesByTechnicalPlanId(technicalPlanId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyCoverage> GetCompanyCoveragesPrincipalByInsuredObjectId(int insuredObjectId)
        {
            try
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                return coverageBusiness.GetCompanyCoveragesPrincipalByInsuredObjectId(insuredObjectId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyCoverage GetCompanyCoverageProductByCoverageId(int coverageId)
        {
            try
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                return coverageBusiness.GetCompanyCoverageProductByCoverageId(coverageId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyInsuredObject> GetCompanyInsuredObjectsByRiskId(int riskId)
        {
            try
            {
                InsuredObjectBusiness insuredObjectBusiness = new InsuredObjectBusiness();
                return insuredObjectBusiness.GetCompanyInsuredObjectsByRiskId(riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyCoverage> GetCompanyCoveragesByInsuredObjectId(int insuredObjectId)
        {
            try
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                return coverageBusiness.GetCompanyCoveragesByInsuredObjectId(insuredObjectId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyCoverage> CalculateMinimumPremiumRatePerCoverage(List<CompanyCoverage> companyCoverages, decimal minimumPremiumAmount, bool prorate, bool assistance)
        {
            try
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                return coverageBusiness.CalculateMinimumPremiumRatePerCoverage(companyCoverages, minimumPremiumAmount, prorate, assistance);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyInsuredObject> GetCompanyInsuredObjectsByLineBusinessId(int lineBusinessId)
        {
            try
            {
                InsuredObjectDAO insuredObjectDAO = new InsuredObjectDAO();
                return insuredObjectDAO.GetCompanyInsuredObjectsByLineBusinessId(lineBusinessId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #region CompanyPolicy Extendidos

        /// <summary>
        /// Calcular Cuotas
        /// </summary>
        /// <param name="policy">Póliza</param>
        /// <returns>Cuotas</returns>
        //public List<Quota> CalculateQuotasByCompanyPolicy(CompanyPolicy policy)
        //{
        //    try
        //    {
        //        PolicyBusiness bis = new PolicyBusiness();
        //        return bis.CalculateQuotasByCompanyPolicy(policy);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new BusinessException(ex.Message, ex);
        //    }
        //}

        /// <summary>
        /// Calcular Componentes De La Póliza
        /// </summary>
        /// <param name="policy">Póliza</param>
        /// <param name="risks">Riesgos</param>
        /// <returns>Componentes</returns>
        public List<CompanyPayerComponent> CalculatePayerComponentsByCompanyPolicy(CompanyPolicy policy, List<CompanyRisk> risks)
        {
            try
            {
                PolicyBusiness bis = new PolicyBusiness();
                return bis.CalculatePayerComponentsByCompanyPolicy(policy, risks);
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
        public CompanySummary CalculateSummaryByCompanyPolicy(CompanyPolicy policy, List<CompanyRisk> risks)
        {
            try
            {
                PolicyBusiness bis = new PolicyBusiness();
                return bis.CalculateSummaryByCompanyPolicy(policy, risks);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Crear Póliza
        /// </summary>
        /// <param name="temporalId">Id Temporal</param>
        /// <returns>Póliza</returns>
        public CompanyPolicy CreateCompanyPolicy(CompanyPolicy companyPolicy)
        {
            try
            {
                companyPolicy.IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Now);
                companyPolicy.IssueDate = Convert.ToDateTime(companyPolicy.IssueDate.ToString("dd-MM-yyyy"));
                companyPolicy.BeginDate = DateTime.Now;
                PolicyDAO policyDAO = new PolicyDAO();
                var policy = policyDAO.CreateCompanyPolicy(companyPolicy);

                if (policy.PaymentPlan.PremiumFinance != null && policy.PaymentPlan.PremiumFinance.Insured.IndividualId > 0)
                {
                    if (companyPolicy.Endorsement.EndorsementType != EndorsementType.Cancellation && companyPolicy.Endorsement.EndorsementType != EndorsementType.LastEndorsementCancellation)
                    {
                        if (companyPolicy.Endorsement.EndorsementType == EndorsementType.Renewal)
                        { // Cuando es renovacion actualizar las fechas, para que no duplique el pagare
                            policy.PaymentPlan.PremiumFinance.CurrentFrom = policy.Endorsement.CurrentFrom;
                            policy.PaymentPlan.PremiumFinance.CurrentTo = policy.Endorsement.CurrentTo;
                        }

                        policy.PaymentPlan.PremiumFinance = SaveCompanyPremiumFinance(policy);
                    }

                }

                return policy;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Metodo que realiza la emision la poliza desde politicas
        /// </summary>
        /// <param name="temporalId">Id del temporal</param>
        /// <returns></returns>
        public string CreatePolicyAuthorization(string temporalId)
        {
            try
            {
                PolicyDAO policyDao = new PolicyDAO();
                return policyDao.CreatePolicyAuthorization(temporalId);
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
        public List<IssuanceAgency> CalculateCommissionsByCompanyPolicy(CompanyPolicy policy, List<CompanyRisk> risks)
        {
            try
            {
                PolicyBusiness bis = new PolicyBusiness();
                return bis.CalculateCommissionsByCompanyPolicy(policy, risks);
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
        public List<CompanyPolicy> GetCompanyTemporalPoliciesByCompanyPolicy(CompanyPolicy policy)
        {
            try
            {
                PolicyBusiness bis = new PolicyBusiness();
                return bis.GetCompanyTemporalPoliciesByCompanyPolicy(policy);
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
        public List<CompanyPolicy> GetCompanyPoliciesByCompanyPolicy(CompanyPolicy policy)
        {
            try
            {
                PolicyBusiness bis = new PolicyBusiness();
                return bis.GetCompanyPoliciesByCompanyPolicy(policy);
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
        public CompanyPolicy GetCurrentCompanyPolicyByPrefixIdBranchIdPolicyNumber(int prefixId, int branchId, decimal policyNumber)
        {
            try
            {
                PolicyBusiness bis = new PolicyBusiness();
                return bis.GetCurrentCompanyPolicyByPrefixIdBranchIdPolicyNumber(prefixId, branchId, policyNumber);
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
        public CompanyRisk RunRulesCompanyRisk(CompanyPolicy policy, CompanyRisk risk, int rulsetId)
        {
            try
            {
                RiskBusiness bis = new RiskBusiness();
                risk.Policy = policy;
                return bis.RunRulesRisk(risk, rulsetId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion

        public List<CompanyInsuredObject> GetCompanyInsuredObjectsByDescription(string description)
        {
            try
            {
                InsuredObjectDAO insuredObjectDAO = new InsuredObjectDAO();
                return insuredObjectDAO.GetCompanyInsuredObjectsByDescription(description);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyInsuredObject> CreateCompanyInsuredObjects(List<CompanyInsuredObject> companyInsuredObjects)
        {
            try
            {
                InsuredObjectDAO insuredObjectDAO = new InsuredObjectDAO();
                return insuredObjectDAO.CreateCompanyInsuredObjects(companyInsuredObjects);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        public CompanyInsuredObject GetCompanyInsuredObjectByProductIdGroupCoverageIdInsuredObjectId(int productId, int groupCoverageId, int insuredObjectId)
        {
            try
            {
                InsuredObjectBusiness insuredObjectBusiness = new InsuredObjectBusiness();
                return insuredObjectBusiness.GetCompanyInsuredObjectByProductIdGroupCoverageIdInsuredObjectId(productId, groupCoverageId, insuredObjectId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<String> GetRiskByEndorsementDocumentNumber(int endorsementId)
        {
            try
            {
                RiskDAO riskDAO = new RiskDAO();
                return riskDAO.GetRiskByEndorsementDocumentNumber(endorsementId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetPolicyByPolicyIdEndorsementId), ex);
            }
        }

        public List<PoliciesAut> ValidateAuthorizationPolicies(CompanyPolicy policy)
        {
            try
            {
                PolicyDAO policyDAO = new PolicyDAO();
                return policyDAO.ValidateAuthorizationPolicies(policy);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public bool GetAssistanceInPremiumMin(List<DynamicConcept> modelDynamicProperties)
        {
            try
            {
                CoverageBusiness businessCoverage = new CoverageBusiness();
                return businessCoverage.GetAssistanceInPremiumMin(modelDynamicProperties);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Poliza por 
        /// </summary>
        /// <param name="policyId">Id poliza</param>
        /// <param name="policyId">Id endoso</param>
        /// <returns>Objeto policy</returns>
        public CompanyPolicy GetTemporalPolicyByPolicyIdEndorsementId(int policyId, int endorsementId)
        {
            try
            {
                PolicyDAO policy = new PolicyDAO();
                return policy.GetTemporalPolicyByPolicyIdEndorsementId(policyId, endorsementId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCompanyPolicy));
            }
        }

        public CompanyLimitRc GetCompanyLimitRcById(int id)
        {
            try
            {
                LimitRcDAO limitRcDAO = new LimitRcDAO();
                return limitRcDAO.GetCompanyLimitRcById(id);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetLimitRc));
            }
        }

        public CompanyRatingZone GetCompanyRatingZoneByRatingZoneId(int ratingZoneId)
        {
            try
            {
                CompanyRatingZoneDAO companyRatingZoneDAO = new CompanyRatingZoneDAO();
                return companyRatingZoneDAO.GetCompanyRatingZoneByRatingZoneId(ratingZoneId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetRatingZone));
            }
        }

        /// <summary>
        /// Gets the company beneficiary types.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public List<CompanyBeneficiaryType> GetCompanyBeneficiaryTypes()
        {
            try
            {
                return new List<CompanyBeneficiaryType> {
                    new CompanyBeneficiaryType { Id = KeySettings.OnerousBeneficiaryTypeId, SmallDescription = Errors.BeneficiaryOnerous },
                    new CompanyBeneficiaryType { Id = KeySettings.LeasingBeneficiaryTypeId, SmallDescription = Errors.BeneficiaryLeasing },
                    new CompanyBeneficiaryType { Id = KeySettings.NotApplyBeneficiaryTypeId, SmallDescription = Errors.BeneficiaryNotApply  } };
            }
            catch (Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.BeneficiaryOnerous));
            }

            BeneficiaryTypeDAO beneficiaryTypeDAO = new BeneficiaryTypeDAO();
            return beneficiaryTypeDAO.GetCompanyBeneficiaryTypes();
        }

        #region Migracion Company
        #region comisiones
        public List<IssuanceAgency> GetCompanyCommissionsByTempId(int temporalId, IssuanceAgency agency, List<IssuanceAgency> agencies)
        {
            CompanyPolicy policy = GetCompanyPolicyByTemporalId(temporalId, false);
            if (policy?.Id > 0)
            {
                if (policy.PolicyOrigin == PolicyOrigin.Collective)
                {
                    policy.Agencies = agencies;
                }
                else
                {
                    if (policy.Summary != null && policy.Summary.Premium > 0)
                    {
                        List<CompanyRisk> risks = GetCompanyRisksByTemporalId(temporalId, false);

                        if (agency != null)
                        {
                            agencies.AsParallel().ForAll(x => x.Commissions.AsParallel().ForAll(y => { y.Percentage = agency.Commissions[0].Percentage; y.PercentageAdditional = agency.Commissions[0].PercentageAdditional; }));
                        }

                        policy.Agencies = agencies;

                        policy.Agencies = DelegateService.underwritingService.CalculateCommissionsByCompanyPolicy(policy, risks);
                    }
                    else
                    {
                        if (policy.Prefix.Id == (int)ENUM.PrefixType.Transportes && policy.Summary != null && policy.Summary.Premium == 0)
                        {
                            policy.Agencies = agencies;
                        }
                    }
                }
                return policy.Agencies;
            }
            else
            {
                throw new BusinessException(Errors.ErrorTemporalNotFound);
            }

        }
        public List<IssuanceAgency> SaveCompanyCommissions(int temporalId, List<IssuanceAgency> agencies)
        {
            CompanyPolicy policy = GetCompanyPolicyByTemporalId(temporalId, false);
            if (policy?.Id > 0)
            {
                if (policy.Endorsement?.EndorsementType == EndorsementType.Emission || policy.Endorsement.EndorsementType == EndorsementType.Renewal)
                {
                    if (agencies?.Sum(x => x.Participation) == 100)
                    {
                        if (agencies.Exists(x => x.IsPrincipal == true))
                        {
                            policy.Id = temporalId;
                            policy.Agencies = agencies;

                            policy = CreatePolicyTemporal(policy, false);

                            if (policy != null)
                            {
                                return policy.Agencies;
                            }
                            else
                            {
                                throw new BusinessException(Errors.ErrorSaveIntermediaries);
                            }
                        }
                        else
                        {
                            throw new BusinessException(Errors.ErrorPrincipalIntermediary);

                        }
                    }
                    else
                    {
                        throw new BusinessException(Errors.ErrorTotalParticipation);
                    }
                }
                else
                {
                    return policy.Agencies;
                }
            }
            else
            {
                throw new BusinessException(Errors.ErrorTemporalNotFound);
            }
        }
        #endregion comisiones
        #region beneficiarios
        /// <summary>
        /// Saves the company beneficiary.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="beneficiaries">The beneficiaries.</param>
        /// <returns></returns>
        /// <exception cref="Exception">
        /// </exception>
        public List<CompanyBeneficiary> SaveCompanyBeneficiary(int temporalId, List<CompanyBeneficiary> beneficiaries)
        {
            CompanyPolicy policy = GetCompanyPolicyByTemporalId(temporalId, false);

            if (policy.Id > 0)
            {

                if (policy.Endorsement.EndorsementType == EndorsementType.Emission)
                {
                    policy.DefaultBeneficiaries = beneficiaries;

                    policy = CreatePolicyTemporal(policy, false);

                    if (policy != null)
                    {
                        return beneficiaries;
                    }
                    else
                    {
                        throw new BusinessException(Errors.ErrorSaveBeneficiaries);
                    }
                }
                else
                {
                    return beneficiaries;
                }
            }
            else
            {
                throw new Exception(Errors.ErrorTemporalNotFound);
            }
        }

        public CompanyBeneficiary GetBeneficiaryByPrefixId(int prefixId)
        {
            try
            {
                Sistran.Core.Application.CommonService.Models.Parameter parameter = DelegateService.commonService.GetParameterByParameterId((int)ParametersTypes.ThirdPartiesAffected);

                if (parameter != null && parameter.TextParameter != null && parameter.TextParameter.Contains("RC")) //prefixId.ToString()
                {
                    var companyBeneficiaries = GetBeneficiariesByDescription(parameter.NumberParameter.ToString(), InsuredSearchType.IndividualId);
                    if (companyBeneficiaries != null)
                    {
                        var config = ModelAssembler.CreateMapCompanyBeneficiary();
                        List<CompanyBeneficiary> beneficiaries = config.Map<List<Beneficiary>, List<CompanyBeneficiary>>(companyBeneficiaries);
                        if (beneficiaries != null && beneficiaries.Count > 0)
                        {
                            CompanyBeneficiaryType BeneficiaryType = GetCompanyBeneficiaryTypes().Where(x => x.Id == beneficiaries[0].BeneficiaryType.Id).First();
                            beneficiaries[0].BeneficiaryTypeDescription = BeneficiaryType.SmallDescription;
                            beneficiaries[0].Participation = 100;

                            var companyName = DelegateService.uniquePersonServiceV1.GetNotificationAddressesByIndividualId(beneficiaries[0].IndividualId, (CustomerType)beneficiaries[0].CustomerType).FirstOrDefault();

                            beneficiaries[0].CompanyName = new IssuanceCompanyName
                            {
                                NameNum = companyName.NameNum,
                                TradeName = companyName.TradeName,
                                Address = companyName.Address != null ? new IssuanceAddress
                                {
                                    Id = companyName.Address.Id,
                                    Description = companyName.Address.Description,
                                    City = companyName.Address.City
                                } : null,
                                Phone = companyName.Phone != null ? new IssuancePhone
                                {
                                    Id = companyName.Phone.Id,
                                    Description = companyName.Phone.Description
                                } : null,
                                Email = companyName.Email != null ? new IssuanceEmail
                                {
                                    Id = companyName.Email.Id,
                                    Description = companyName.Email.Description
                                } : null
                            };
                            return beneficiaries[0];
                        }

                        return null;
                    }
                    return null;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
        #region Poliza
        public CompanyPolicy GetCompanyTemporalByIdTemporalType(int id, TemporalType temporalType)
        {
            CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(id, false);
            if (policy != null && policy.Id > 0)
            {
                var settings = new JsonSerializerSettings();

                if (policy.TemporalType == temporalType)
                {
                    policy = GetPolicyDescriptions(policy);

                    var authorizationRequests = DelegateService.authorizationPoliciesService.GetAuthorizationRequestsByKey(policy.Id.ToString());

                    if (authorizationRequests.Any(x => x.Status == TypeStatus.Rejected))
                    {
                        throw new Exception(Errors.ErrorTempEventReject);
                    }
                    if (authorizationRequests.Count > 0)
                    {
                        throw new Exception(Errors.ErrorTempEventAutorize);
                    }

                    return policy;
                }
                else
                {
                    throw new BusinessException(string.Format(Errors.ErrorTemporalType, policy.TemporalTypeDescription));
                }
            }
            else
            {
                throw new BusinessException(Errors.ErrorTemporalNotFound);
            }
        }

        private CompanyPolicy GetPolicyDescriptions(CompanyPolicy policy)
        {
            if (policy.Holder.IdentificationDocument == null)
            {
                policy.Holder = DelegateService.underwritingService.GetHoldersByDescriptionInsuredSearchTypeCustomerType(policy.Holder.IndividualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();
            }

            if (policy.Agencies[0].Branch == null || string.IsNullOrEmpty(policy.Agencies[0].FullName))
            {
                TP.Parallel.ForEach(policy?.Agencies, item =>
                {
                    var agent = DelegateService.uniquePersonServiceV1.GetAgentByIndividualId(item.Agent.IndividualId);
                    item.Agent = new IssuanceAgent
                    {
                        IndividualId = agent.IndividualId,
                        FullName = agent.FullName,
                        AgentType = new IssuanceAgentType
                        {
                            Id = agent.AgentType.Id,
                            Description = agent.AgentType.Description
                        },
                    };
                    item.FullName = item.FullName;
                });
            }

            if (string.IsNullOrEmpty(policy.PaymentPlan.Description))
            {
                policy.PaymentPlan.Description = DelegateService.underwritingService.GetPaymentPlanByPaymentPlanId(policy.PaymentPlan.Id).Description;
            }
            if (policy.CoInsuranceCompanies != null && policy.BusinessType != BusinessType.CompanyPercentage)
            {
                var insuranceCompanies = policy?.CoInsuranceCompanies.Where(x => string.IsNullOrEmpty(x.Description)).ToList();
                var insuranceCompaniesData = policy.CoInsuranceCompanies.Where(x => !string.IsNullOrEmpty(x.Description)).ToList();
                insuranceCompanies.AsParallel().ForAll(x => x.Description = DelegateService.underwritingService.GetCoInsuranceCompanyByCoinsuranceId((int)x.Id).Description);
                insuranceCompanies.AddRange(insuranceCompaniesData);
                policy.CoInsuranceCompanies = insuranceCompanies;
            }

            // policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);

            return policy;
        }
        public CompanyPaymentPlan GetPaymentPlanByPaymentPlanId(int paymentPlanId)
        {
            try
            {
                PaymentPlanDAO paymentPlanDAO = new PaymentPlanDAO();
                return paymentPlanDAO.GetPaymentPlanByPaymentPlanId(paymentPlanId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyPolicy SaveCompanyTemporal(CompanyPolicy policy, bool isMasive)
        {
            PendingOperation pendingOperation = new PendingOperation();

            if (policy.Id == 0)
            {
                pendingOperation.OperationName = "Temporal";
                policy.Endorsement.EndorsementTypeDescription = Errors.ResourceManager.GetString(EnumHelper.GetItemName<EndorsementType>(policy.Endorsement.EndorsementType));
                policy.BusinessType = EnumsCore.BusinessType.CompanyPercentage;
                policy.CoInsuranceCompanies = new List<CompanyIssuanceCoInsuranceCompany>
                {
                    new CompanyIssuanceCoInsuranceCompany
                    {
                        ParticipationPercentageOwn = 100
                    }
                };
                if (policy.Request == null)
                {
                    var imapper = ModelAssembler.CreateMapCiaPaymentPlan();
                    policy.PaymentPlan = imapper.Map<PaymentPlan, CompanyPaymentPlan>(DelegateService.underwritingService.GetDefaultPaymentPlanByProductId(policy.Product.Id));
                    policy.Agencies[0].Participation = 100;

                    ProductAgencyCommiss productAgencyCommiss = DelegateService.productService.GetCommissByAgentIdAgencyIdProductId(policy.Agencies[0].Agent.IndividualId, policy.Agencies[0].Id, policy.Product.Id);
                    policy.Agencies[0].Commissions.Add(new IssuanceCommission
                    {
                        Percentage = productAgencyCommiss.CommissPercentage,
                        PercentageAdditional = productAgencyCommiss.AdditionalCommissionPercentage.GetValueOrDefault(0)
                    });
                }
                var config = ModelAssembler.CreateMapCompanyClause();
                List<CompanyClause> clauses = config.Map<List<Clause>, List<CompanyClause>>(DelegateService.underwritingService.GetClausesByEmissionLevelConditionLevelId(Core.Application.UnderwritingServices.Enums.EmissionLevel.General, policy.Prefix.Id));

                if (policy.Clauses != null)
                {
                    policy.Clauses = policy.Clauses.Where(x => x.IsMandatory == false).ToList();
                }
                else
                {
                    policy.Clauses = new List<CompanyClause>();
                }

                if (clauses.Count > 0)
                {
                    policy.Clauses.AddRange(clauses.Where(x => x.IsMandatory == true).ToList());
                }

                policy.BeginDate = DateTime.Now;
                policy.EffectPeriod = DelegateService.commonService.GetExtendedParameterByParameterId(1027).NumberParameter.Value;
                policy.Product = DelegateService.productService.GetCompanyProductByProductIdPrefixId(policy.Product.Id, policy.Prefix.Id);
                if (policy?.Product != null)
                {
                    policy.InfringementPolicies = ValidateAuthorizationPolicies(policy);
                    pendingOperation.Operation = COMUT.JsonHelper.SerializeObjectToJson(policy);
                    pendingOperation = DelegateService.utilitiesServiceCore.CreatePendingOperation(pendingOperation);
                    policy = COMUT.JsonHelper.DeserializeJson<CompanyPolicy>(pendingOperation.Operation);
                    policy.Id = pendingOperation.Id;
                    if (policy.TemporalType == TemporalType.Quotation)
                    {
                        var task = TP.Task.Run<CompanyPolicy>(() =>
                        {
                            PolicyDAO policyDAO = new PolicyDAO();
                            // Cuando guarda el temporal de cotización solo guarda en la cabecera de la temporal de cotización.
                            var policyData = policyDAO.SaveTemporalPolicy(policy);
                            DataFacadeManager.Dispose();
                            var ResultPolicy = CreatePolicyTemporal(policyData, false);
                            return ResultPolicy;
                        }
                       );
                        task.Wait();
                        return task.Result;
                    }
                    else
                    {
                        if (policy.Endorsement.EndorsementType != EndorsementType.Emission)
                        {
                            TP.Task.Run(() =>
                            {
                                PolicyDAO policyDAO = new PolicyDAO();
                                policyDAO.CreateTemporalCompanyPolicy(policy);
                                DataFacadeManager.Dispose();

                            }
                         );
                        }
                        return policy;
                    }


                }
                else
                {
                    throw new BusinessException(Errors.ErrorGetCptProductById);
                }
            }
            else
            {
                pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(policy.Id);
                if (pendingOperation != null && pendingOperation.Operation != null)
                {
                    CompanyPolicy companyPolicy = COMUT.JsonHelper.DeserializeJson<CompanyPolicy>(pendingOperation.Operation);
                    companyPolicy.Id = pendingOperation.Id;

                    switch (companyPolicy.Endorsement.EndorsementType)
                    {
                        case EnumsCore.EndorsementType.Emission:
                        case EnumsCore.EndorsementType.Renewal:
                            policy = SetDataEmission(policy, companyPolicy);
                            break;
                        case EnumsCore.EndorsementType.Modification:
                            policy = SetDataModification(policy, companyPolicy);
                            break;
                        default:
                            break;
                    }
                    policy.InfringementPolicies = ValidateAuthorizationPolicies(policy);
                    pendingOperation.Operation = COMUT.JsonHelper.SerializeObjectToJson(policy);
                    try
                    {
                        TP.Task.Run(() =>
                        {
                            PolicyDAO policyDAO = new PolicyDAO();
                            policyDAO.CreateTemporalCompanyPolicy(policy);
                            DataFacadeManager.Dispose();
                        }
                        );
                        DelegateService.utilitiesServiceCore.UpdatePendingOperation(pendingOperation);
                        policy = COMUT.JsonHelper.DeserializeJson<CompanyPolicy>(pendingOperation.Operation);
                        policy.Id = pendingOperation.Id;
                        return policy;
                    }
                    catch (Exception ex)
                    {
                        throw new BusinessException(Errors.ErrorSaveTemp);
                    }
                }
                else
                {
                    throw new BusinessException(Errors.ErrorTempNoExist);
                }
            }
        }
        /// <summary>
        /// Sets the data emission.
        /// </summary>
        /// <param name="policy">The policy.</param>
        /// <param name="policyOld">The policy old.</param>
        /// <returns></returns>
        private CompanyPolicy SetDataEmission(CompanyPolicy policy, CompanyPolicy policyOld)
        {
            var TicketDate = policy.Endorsement.TicketDate;
            var TicketNumber = policy.Endorsement.TicketNumber;
            var Justification = policy.JustificationSarlaft;
            policy.TicketNumber = TicketNumber.Value;
            policy.Endorsement = policyOld.Endorsement;
            policy.DocumentNumber = policyOld.DocumentNumber;
            policy.TemporalType = policyOld.TemporalType;
            policy.TemporalTypeDescription = policyOld.TemporalTypeDescription;
            policy.BusinessType = policyOld.BusinessType;
            policy.CoInsuranceCompanies = policyOld.CoInsuranceCompanies;
            if (policy.Agencies != null)
            {
                IssuanceAgency agency = policy.Agencies.First(x => x.IsPrincipal);
                policy.Agencies = policyOld.Agencies;

                if (policy.Agencies.Exists(x => x.Agent.IndividualId == agency.Agent.IndividualId && x.Id == agency.Id))
                {
                    policy.Agencies.AsParallel().ForAll(x => x.IsPrincipal = false);
                    policy.Agencies.First(x => x.Agent.IndividualId == agency.Agent.IndividualId && x.Id == agency.Id).IsPrincipal = true;

                    var oAgencies = policy.Agencies.First(x => x.Agent.IndividualId == agency.Agent.IndividualId && x.Id == agency.Id);

                    if (oAgencies.Branch == null || (oAgencies.Branch != null && (oAgencies.Branch.SalePoints == null || oAgencies.Branch.SalePoints.Count == 0)))
                    {
                        policy.Agencies.First(x => x.Agent.IndividualId == agency.Agent.IndividualId && x.Id == agency.Id).Branch = agency.Branch;
                    }
                }
                else
                {
                    policy.Agencies.First(x => x.IsPrincipal == true).Id = agency.Id;
                    policy.Agencies.First(x => x.IsPrincipal == true).Code = agency.Code;
                    policy.Agencies.First(x => x.IsPrincipal == true).FullName = agency.FullName;
                    policy.Agencies.First(x => x.IsPrincipal == true).Agent = agency.Agent;
                    policy.Agencies.First(x => x.IsPrincipal == true).Branch = agency.Branch;

                    ProductAgencyCommiss productAgencyCommiss = DelegateService.productService.GetCommissByAgentIdAgencyIdProductId(agency.Agent.IndividualId, agency.Id, policy.Product.Id);
                    policy.Agencies.AsParallel().ForAll(x => x.Commissions.AsParallel().ForAll(y => { y.Percentage = productAgencyCommiss.CommissPercentage; y.Percentage = productAgencyCommiss.CommissPercentage; }));
                }
            }

            policy.PaymentPlan = policyOld.PaymentPlan;
            policy.PayerComponents = policyOld.PayerComponents;
            policy.Text = policyOld.Text;
            policy.Clauses = policyOld.Clauses;
            policy.DefaultBeneficiaries = policyOld.DefaultBeneficiaries;
            policy.BeginDate = policyOld.BeginDate;
            policy.EffectPeriod = DelegateService.commonService.GetExtendedParameterByParameterId(1027).NumberParameter.Value;
            policy.Product = DelegateService.productService.GetCompanyProductByProductIdPrefixId(policy.Product.Id, policy.Prefix.Id);

            if (policyOld.Summary?.RisksInsured != null && policyOld.Summary?.RisksInsured.Count > 0 && policyOld.Summary?.RisksInsured[0]?.Insured?.CustomerType != CustomerType.Prospect && policyOld.Summary?.RisksInsured[0]?.Beneficiaries[0].CustomerType != CustomerType.Prospect)
            {
                policy.Summary = policyOld.Summary;
            }
            else
            {
                policy.Summary.Premium = policyOld.Summary.Premium;
                policy.Summary.AmountInsured = policyOld.Summary.AmountInsured;
                policy.Summary.CoveredRiskType = policyOld.Summary.CoveredRiskType;
                policy.Summary.FullPremium = policyOld.Summary.FullPremium;
                policy.Summary.Taxes = policyOld.Summary.Taxes;
                policy.Summary.RiskCount = policyOld.Summary.RiskCount;
                policy.Summary.Premium = policyOld.Summary.Premium;
                policy.Summary.Risks = policyOld.Summary.Risks;
            }
            policy.Endorsement.TicketDate = TicketDate;
            policy.Endorsement.TicketNumber = TicketNumber;
            policy.JustificationSarlaft = Justification;

            var fiscalResponsibility = DelegateService.uniquePersonServiceCore.GetFiscalResponsibilityByIndividualId(policy.Holder.IndividualId);
            if (fiscalResponsibility.Count > 0)
            {
                policy.Holder.FiscalResponsibility = fiscalResponsibility;
            }
            var EmailElectronic = DelegateService.uniquePersonServiceCore.GetEmailsByIndividualId(policy.Holder.IndividualId);
            if (EmailElectronic.Count > 0)
            {
                foreach (Email email in EmailElectronic)
                {
                    if (email.Description != null && email.EmailType.Id == 23)
                    {
                        policy.Holder.Email = email.Description;
                    }
                }
            }
            var insured = DelegateService.uniquePersonServiceCore.GetInsuredByIndividualId(policy.Holder.IndividualId);
            if (insured != null)
            {
                policy.Holder.RegimeType = insured.RegimeType;
            }


            return policy;
        }

        /// <summary>
        /// Sets the data emission.
        /// </summary>
        /// <param name="policy">The policy.</param>
        /// <param name="policyOld">The policy old.</param>
        /// <returns></returns>
        private CompanyPolicy SetDataQuatation(CompanyPolicy policy, CompanyPolicy policyOld)
        {
            QuotationDAO QuotationDAO = new QuotationDAO();
            var TicketDate = policy.Endorsement.TicketDate;
            var TicketNumber = policy.Endorsement.TicketNumber;
            var Justification = policy.JustificationSarlaft;
            policy.DocumentNumber = policyOld.DocumentNumber;
            policy.Endorsement.QuotationVersion = QuotationDAO.GetVersionQuotation(policyOld.Id, policy.Endorsement.QuotationId);
            policy.BusinessType = policyOld.BusinessType;
            policy.CoInsuranceCompanies = policyOld.CoInsuranceCompanies;
            if (policy.Agencies != null)
            {
                IssuanceAgency agency = policy.Agencies.First(x => x.IsPrincipal);
                policy.Agencies = policyOld.Agencies;

                if (policy.Agencies.Exists(x => x.Agent.IndividualId == agency.Agent.IndividualId && x.Id == agency.Id))
                {
                    policy.Agencies.AsParallel().ForAll(x => x.IsPrincipal = false);
                    policy.Agencies.First(x => x.Agent.IndividualId == agency.Agent.IndividualId && x.Id == agency.Id).IsPrincipal = true;

                    var oAgencies = policy.Agencies.First(x => x.Agent.IndividualId == agency.Agent.IndividualId && x.Id == agency.Id);

                    if (oAgencies.Branch == null || (oAgencies.Branch != null && (oAgencies.Branch.SalePoints == null || oAgencies.Branch.SalePoints.Count == 0)))
                    {
                        policy.Agencies.First(x => x.Agent.IndividualId == agency.Agent.IndividualId && x.Id == agency.Id).Branch = agency.Branch;
                    }
                }
                else
                {
                    policy.Agencies.First(x => x.IsPrincipal == true).Id = agency.Id;
                    policy.Agencies.First(x => x.IsPrincipal == true).Code = agency.Code;
                    policy.Agencies.First(x => x.IsPrincipal == true).FullName = agency.FullName;
                    policy.Agencies.First(x => x.IsPrincipal == true).Agent = agency.Agent;
                    policy.Agencies.First(x => x.IsPrincipal == true).Branch = agency.Branch;

                    ProductAgencyCommiss productAgencyCommiss = DelegateService.productService.GetCommissByAgentIdAgencyIdProductId(agency.Agent.IndividualId, agency.Id, policy.Product.Id);
                    policy.Agencies.AsParallel().ForAll(x => x.Commissions.AsParallel().ForAll(y => { y.Percentage = productAgencyCommiss.CommissPercentage; y.Percentage = productAgencyCommiss.CommissPercentage; }));
                }
            }

            policy.PaymentPlan = policyOld.PaymentPlan;
            policy.PayerComponents = policyOld.PayerComponents;
            policy.Text = policyOld.Text;
            policy.Clauses = policyOld.Clauses;
            policy.DefaultBeneficiaries = policyOld.DefaultBeneficiaries;
            policy.BeginDate = policyOld.BeginDate;
            policy.EffectPeriod = DelegateService.commonService.GetExtendedParameterByParameterId(1027).NumberParameter.Value;
            policy.Product = DelegateService.productService.GetCompanyProductByProductIdPrefixId(policy.Product.Id, policy.Prefix.Id);

            if (policyOld.Summary?.RisksInsured == null)
            {
                //consultar riesgo para no entrar a la pantalla del riesgo.
                List<CompanyRisk> risks = DelegateService.underwritingService.GetCompanyRisksByTemporalId(policyOld.Id, false);
                List<CompanyRiskInsured> companyRisksInsured = new List<CompanyRiskInsured>();
                foreach (var companyRisk in risks)
                {
                    CompanyRiskInsured companyRiskInsured = new CompanyRiskInsured();
                    companyRiskInsured.Beneficiaries = new List<CompanyBeneficiary>();

                    foreach (var beneficiary in companyRisk.Beneficiaries)
                    {
                        companyRiskInsured.Beneficiaries.Add(beneficiary);
                    }
                    companyRiskInsured.Insured = companyRisk.MainInsured;
                    companyRisksInsured.Add(companyRiskInsured);
                }
                policyOld.Summary.RisksInsured = companyRisksInsured;
            }


            if (policyOld.Summary?.RisksInsured != null && policyOld.Summary?.RisksInsured.Count > 0 && policyOld.Summary?.RisksInsured[0]?.Insured?.CustomerType != CustomerType.Prospect && policyOld.Summary?.RisksInsured[0]?.Beneficiaries[0].CustomerType != CustomerType.Prospect)
            {
                policy.Summary = policyOld.Summary;
            }
            else
            {
                policy.Summary.Premium = policyOld.Summary.Premium;
                policy.Summary.AmountInsured = policyOld.Summary.AmountInsured;
                policy.Summary.CoveredRiskType = policyOld.Summary.CoveredRiskType;
                policy.Summary.FullPremium = policyOld.Summary.FullPremium;
                policy.Summary.Taxes = policyOld.Summary.Taxes;
                policy.Summary.RiskCount = policyOld.Summary.RiskCount;
                policy.Summary.Premium = policyOld.Summary.Premium;
                policy.Summary.Risks = policyOld.Summary.Risks;
            }
            policy.Endorsement.TicketDate = TicketDate;
            policy.Endorsement.TicketNumber = TicketNumber;
            policy.JustificationSarlaft = Justification;
            return policy;
        }

        private CompanyPolicy SetDataModification(CompanyPolicy policy, CompanyPolicy policyOld)
        {
            policy.CurrentFrom = policyOld.CurrentFrom;
            var fiscalResponsibility = DelegateService.uniquePersonServiceCore.GetFiscalResponsibilityByIndividualId(policy.Holder.IndividualId);
            if (fiscalResponsibility.Count > 0)
            {
                policy.Holder.FiscalResponsibility = fiscalResponsibility;
            }
            var EmailElectronic = DelegateService.uniquePersonServiceCore.GetEmailsByIndividualId(policy.Holder.IndividualId);
            if (EmailElectronic.Count > 0)
            {
                foreach (Email email in EmailElectronic)
                {
                    if (email.Description != null && email.EmailType.Id == 23)
                    {
                        policy.Holder.Email = email.Description;
                    }
                }
            }
            var insured = DelegateService.uniquePersonServiceCore.GetInsuredByIndividualId(policy.Holder.IndividualId);
            if (insured != null)
            {
                policy.Holder.RegimeType = insured.RegimeType;
            }

            return policy;
        }
        #endregion
        #region pagadores
        public CompanyPolicy UpdatePolicyComponents(int policyId)
        {
            try
            {
                CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(policyId, false);
                if (companyPolicy.Id > 0)
                {
                    if (companyPolicy != null)
                    {
                        List<CompanyRisk> risks = DelegateService.underwritingService.GetCompanyRisksByTemporalId(companyPolicy.Id, false);
                        companyPolicy = CalculatePolicyAmounts(companyPolicy, risks);
                        List<CompanyRiskInsured> companyRisksInsured = new List<CompanyRiskInsured>();
                        foreach (var companyRisk in risks)
                        {
                            CompanyRiskInsured companyRiskInsured = new CompanyRiskInsured();
                            companyRiskInsured.Beneficiaries = new List<CompanyBeneficiary>();
                            foreach (var beneficiary in companyRisk.Beneficiaries)
                            {
                                if (companyPolicy.TemporalType == TemporalType.Policy && beneficiary.CustomerType == CustomerType.Prospect)
                                {
                                    CompanyBeneficiary companyBeneficiary = new CompanyBeneficiary()
                                    {
                                        BeneficiaryType = beneficiary.BeneficiaryType,
                                        CompanyName = companyPolicy.Holder.CompanyName,
                                        DeclinedDate = companyPolicy.Holder.DeclinedDate,
                                        ExtendedProperties = companyPolicy.Holder.ExtendedProperties,
                                        IdentificationDocument = companyPolicy.Holder.IdentificationDocument,
                                        CodeBeneficiary = beneficiary.CodeBeneficiary,
                                        CustomerType = companyPolicy.Holder.CustomerType,
                                        IndividualId = companyPolicy.Holder.IndividualId,
                                        Name = companyPolicy.Holder.Name,
                                        BeneficiaryTypeDescription = beneficiary.BeneficiaryTypeDescription,
                                        Participation = beneficiary.Participation


                                    };
                                    companyRiskInsured.Beneficiaries.Add(companyBeneficiary);
                                }
                                else
                                {
                                    companyRiskInsured.Beneficiaries.Add(beneficiary);
                                }
                            }

                            if (companyPolicy.TemporalType == TemporalType.Policy && companyRisk.MainInsured.CustomerType == CustomerType.Prospect)
                            {
                                CompanyIssuanceInsured
                                      Insured = new CompanyIssuanceInsured()
                                      {
                                          BirthDate = companyPolicy.Holder.BirthDate,
                                          CompanyName = companyPolicy.Holder.CompanyName,
                                          CustomerType = companyPolicy.Holder.CustomerType,
                                          CustomerTypeDescription = companyPolicy.Holder.CustomerTypeDescription,
                                          DeclinedDate = companyPolicy.Holder.DeclinedDate,
                                          EconomicActivity = companyPolicy.Holder.EconomicActivity,
                                          ExtendedProperties = companyPolicy.ExtendedProperties,
                                          IdentificationDocument = companyPolicy.Holder.IdentificationDocument,
                                          PaymentMethod = companyPolicy.Holder.PaymentMethod,
                                          Name = companyPolicy.Holder.Name,
                                          IndividualId = companyPolicy.Holder.IndividualId,
                                          IndividualType = companyPolicy.Holder.IndividualType,
                                          InsuredId = companyPolicy.Holder.InsuredId,
                                          Gender = companyPolicy.Holder.Gender,
                                          SecondSurname = companyPolicy.Holder.SecondSurname,
                                          OwnerRoleCode = companyPolicy.Holder.OwnerRoleCode,
                                          Surname = companyPolicy.Holder.Surname
                                      };
                                companyRiskInsured.Insured = Insured;
                                companyRisksInsured.Add(companyRiskInsured);
                            }
                            else
                            {
                                companyRiskInsured.Insured = companyRisk.MainInsured;
                                companyRisksInsured.Add(companyRiskInsured);
                            }

                        }
                        companyPolicy.Summary.RisksInsured = companyRisksInsured;
                        CreatePolicyTemporal(companyPolicy, false);
                        return companyPolicy;
                    }
                    else
                    {
                        throw new BusinessException(Errors.ErrorUpdatePolicy);
                    }
                }
                else
                {
                    throw new BusinessException(Errors.ErrorTemporalNotFound);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// Calculates the policy amounts.
        /// </summary>
        /// <param name="policy">The policy.</param>
        /// <param name="risks">The risks.</param>
        /// <returns></returns>
        public CompanyPolicy CalculatePolicyAmounts(CompanyPolicy policy, List<CompanyRisk> risks)
        {
            policy.Summary = new CompanySummary();
            if (risks != null && risks.Count > 0)
            {
                policy.PayerComponents = CalculatePayerComponentsByCompanyPolicy(policy, risks);
                policy.Summary = CalculateSummaryByCompanyPolicy(policy, risks);

                if (policy.Summary != null)
                {
                    policy.PaymentPlan.Quotas = DelegateService.underwritingService.CalculateQuotas(new QuotaFilterDTO
                    {
                        PlanId = policy.PaymentPlan.Id,
                        CurrentFrom = policy.CurrentFrom,
                        IssueDate = policy.IssueDate,
                        ComponentValueDTO = ModelAssembler.CreateCompanyComponentValueDTO
                        (policy.Summary)
                    });
                    policy.Agencies = CalculateCommissionsByCompanyPolicy(policy, risks);
                }
                else
                {
                    policy.PaymentPlan.Quotas = new List<Quota>();
                    policy.Agencies.SelectMany(x => x.Commissions).AsParallel().ForAll(item =>
                    {
                        item.CalculateBase = 0;
                        item.Amount = 0;
                    }
                        );

                }
            }
            return policy;
        }
        #endregion

        #region Personas
        public CompanyRisk ConvertProspectToInsured(CompanyRisk risk, int individualId, string documentNumber)
        {
            if (risk.MainInsured.IdentificationDocument.Number == documentNumber)
            {
                CompanyIssuanceInsured insured = null;
                var companyInsured = DelegateService.underwritingService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(individualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).First();

                if (companyInsured != null)
                {
                    var imapper = ModelAssembler.CreateMapInsured();
                    insured = new CompanyIssuanceInsured();
                    insured.Surname = companyInsured.Surname;
                    insured.SecondSurname = companyInsured.SecondSurname;
                    insured.Name = companyInsured.Name;
                    insured.IndividualId = companyInsured.IndividualId;
                    insured.IdentificationDocument = companyInsured.IdentificationDocument;
                    insured.IndividualType = companyInsured.IndividualType;
                    insured.InsuredId = companyInsured.InsuredId;
                    insured.PaymentMethod = companyInsured.PaymentMethod;
                    insured.Surname = companyInsured.Surname;
                    insured.SecondSurname = companyInsured.SecondSurname;
                    insured.OwnerRoleCode = companyInsured.OwnerRoleCode;
                    insured.Gender = companyInsured.Gender;
                    insured.BirthDate = companyInsured.BirthDate;
                    insured.CustomerTypeDescription = companyInsured.CustomerTypeDescription;
                    insured.CompanyName = companyInsured.CompanyName;
                    insured.DeclinedDate = companyInsured.DeclinedDate;
                    insured.EconomicActivity = companyInsured.EconomicActivity;
                    insured.ExtendedProperties = companyInsured.ExtendedProperties;
                    insured.CustomerType = companyInsured.CustomerType;

                    //insured = imapper.Map<IssuanceInsured, CompanyIssuanceInsured>(companyInsured);
                }
                risk.MainInsured = insured;
                risk.MainInsured.Name = risk.MainInsured.Surname + " " + (string.IsNullOrEmpty(risk.MainInsured.SecondSurname) ? "" : risk.MainInsured.SecondSurname + " ") + risk.MainInsured.Name;
                CompanyName companyName = DelegateService.uniquePersonServiceV1.GetNotificationAddressesByIndividualId(risk.MainInsured.IndividualId, CustomerType.Individual).FirstOrDefault();
                risk.MainInsured.CompanyName = new IssuanceCompanyName
                {
                    NameNum = companyName.NameNum,
                    TradeName = companyName.TradeName,
                    Address = new IssuanceAddress
                    {
                        Id = companyName.Address.Id,
                        Description = companyName.Address.Description,
                        City = companyName.Address.City
                    },
                    Phone = new IssuancePhone
                    {
                        Id = companyName.Phone.Id,
                        Description = companyName.Phone.Description
                    },
                    Email = new IssuanceEmail
                    {
                        Id = companyName?.Email?.Id ?? 0,
                        Description = companyName?.Email?.Description ?? ""
                    }
                };
            }

            if (risk.Beneficiaries != null && risk.Beneficiaries.Count > 0)
            {
                List<CompanyBeneficiary> beneficiaries = new List<CompanyBeneficiary>();
                var companyBeneficiaryBase = risk.Beneficiaries.Where(x => x?.IdentificationDocument.Number == documentNumber).ToList();
                var companyBeneficiary = risk.Beneficiaries.Where(x => x?.IdentificationDocument.Number != documentNumber).ToList();
                PersonBusiness policyBusiness = new PersonBusiness();
                var beneficiary = policyBusiness.GetCompanyBeneficiariesByDescription(individualId.ToString(), InsuredSearchType.IndividualId).First();
                beneficiary.CompanyName = risk.MainInsured.CompanyName;
                companyBeneficiaryBase.AsParallel().ForAll(x =>
                {
                    x = beneficiary;
                }
                );
                companyBeneficiaryBase.AddRange(companyBeneficiary);
                risk.Beneficiaries = companyBeneficiaryBase;
            }

            return risk;
        }

        public CompanyRisk ConvertProspectToInsuredRisk(CompanyRisk risk, int individualId)
        {

            CompanyIssuanceInsured insured = null;
            var companyInsured = DelegateService.underwritingService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(individualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).First();

            if (companyInsured != null)
            {
                var imapper = ModelAssembler.CreateMapInsured();
                insured = new CompanyIssuanceInsured();
                insured.Surname = companyInsured.Surname;
                insured.SecondSurname = companyInsured.SecondSurname;
                insured.Name = companyInsured.Name;
                insured.IndividualId = companyInsured.IndividualId;
                insured.IdentificationDocument = companyInsured.IdentificationDocument;
                insured.IndividualType = companyInsured.IndividualType;
                insured.InsuredId = companyInsured.InsuredId;
                insured.PaymentMethod = companyInsured.PaymentMethod;
                insured.Surname = companyInsured.Surname;
                insured.SecondSurname = companyInsured.SecondSurname;
                insured.OwnerRoleCode = companyInsured.OwnerRoleCode;
                insured.Gender = companyInsured.Gender;
                insured.BirthDate = companyInsured.BirthDate;
                insured.CustomerTypeDescription = companyInsured.CustomerTypeDescription;
                insured.CompanyName = companyInsured.CompanyName;
                insured.DeclinedDate = companyInsured.DeclinedDate;
                insured.EconomicActivity = companyInsured.EconomicActivity;
                insured.ExtendedProperties = companyInsured.ExtendedProperties;
                insured.CustomerType = companyInsured.CustomerType;
                //insured = imapper.Map<IssuanceInsured, CompanyIssuanceInsured>(companyInsured);
            }
            risk.MainInsured = insured;
            risk.MainInsured.Name = risk.MainInsured.Surname + " " + (string.IsNullOrEmpty(risk.MainInsured.SecondSurname) ? "" : risk.MainInsured.SecondSurname + " ") + risk.MainInsured.Name;
            CompanyName companyName = DelegateService.uniquePersonServiceV1.GetNotificationAddressesByIndividualId(risk.MainInsured.IndividualId, CustomerType.Individual).FirstOrDefault();
            risk.MainInsured.CompanyName = new IssuanceCompanyName
            {
                NameNum = companyName.NameNum,
                TradeName = companyName.TradeName,
                Address = new IssuanceAddress
                {
                    Id = companyName.Address.Id,
                    Description = companyName.Address.Description,
                    City = companyName.Address.City
                },
                Phone = new IssuancePhone
                {
                    Id = companyName.Phone.Id,
                    Description = companyName.Phone.Description
                },
                Email = new IssuanceEmail
                {
                    Id = companyName?.Email?.Id ?? 0,
                    Description = companyName?.Email?.Description ?? ""
                }
            };


            return risk;
        }

        public CompanyBeneficiary ConvertProspectToBeneficiary(CompanyBeneficiary beneficiary, int individualId)
        {
            PersonBusiness policyBusiness = new PersonBusiness();
            CompanyBeneficiary newBeneficiary = policyBusiness.GetCompanyBeneficiariesByDescription(individualId.ToString(), InsuredSearchType.IndividualId).First();
            List<CompanyBeneficiary> listBeneficiary = new List<CompanyBeneficiary>();
            newBeneficiary.Participation = beneficiary.Participation;
            newBeneficiary.CodeBeneficiary = beneficiary.CodeBeneficiary;
            return newBeneficiary;
        }



        public Boolean ConvertProspectToHolder(int temporalId, int individualId, string documentNumber)

        {
            try
            {
                CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                if (policy.Id > 0)
                {
                    if (policy.Holder != null && policy.Holder.IdentificationDocument.Number == documentNumber)
                    {
                        policy.Holder = DelegateService.underwritingService.GetHoldersByDescriptionInsuredSearchTypeCustomerType(individualId.ToString(), InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault();
                        if (policy.Holder.CompanyName == null)
                        {
                            var companyName = DelegateService.uniquePersonServiceV1.GetNotificationAddressesByIndividualId(individualId, (CustomerType)policy.Holder.CustomerType).FirstOrDefault();
                            policy.Holder.CompanyName = new IssuanceCompanyName
                            {
                                NameNum = companyName.NameNum,
                                TradeName = companyName.TradeName,
                                Address = new IssuanceAddress
                                {
                                    Id = companyName.Address.Id,
                                    Description = companyName.Address.Description,
                                    City = companyName.Address.City
                                },
                                Phone = new IssuancePhone
                                {
                                    Id = companyName.Phone.Id,
                                    Description = companyName.Phone.Description
                                },
                                Email = new IssuanceEmail
                                {
                                    Id = companyName.Email.Id,
                                    Description = companyName.Email.Description
                                }
                            };
                            if (policy.Holder.CompanyName.NameNum == 0 && policy.Holder.CustomerType == CustomerType.Individual)
                            {
                                policy.Holder.CompanyName.NameNum = 1;
                                var companyNamePolicy = new CompanyName
                                {
                                    NameNum = policy.Holder.CompanyName.NameNum,
                                    TradeName = policy.Holder.CompanyName.TradeName,
                                    Address = new Address
                                    {
                                        Id = policy.Holder.CompanyName.Address.Id,
                                        Description = policy.Holder.CompanyName.Address.Description,
                                        City = policy.Holder.CompanyName.Address.City
                                    },
                                    Phone = new Phone
                                    {
                                        Id = policy.Holder.CompanyName.Phone.Id,
                                        Description = policy.Holder.CompanyName.Phone.Description
                                    },
                                    Email = new Email
                                    {
                                        Id = policy.Holder.CompanyName.Email.Id,
                                        Description = policy.Holder.CompanyName.Email.Description
                                    }
                                };
                                DelegateService.uniquePersonServiceV1.CreateCompaniesName(companyNamePolicy, policy.Holder.IndividualId);
                            }
                        }
                        policy = CreatePolicyTemporal(policy, false);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorConvertingProspectIntoIndividual);
            }
        }
        #endregion


        public List<CompanyClause> SaveCompanyClauses(int temporalId, List<CompanyClause> clauses)
        {
            if (clauses != null)
            {
                CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                if (policy.Id > 0)
                {

                    policy.Clauses = clauses;
                    policy = CreatePolicyTemporal(policy, false);

                    if (policy != null)
                    {
                        return clauses;
                    }
                    else
                    {
                        throw new BusinessException(Errors.ErrorSaveClauses);
                    }
                }
                else
                {
                    throw new BusinessException(Errors.ErrorTempNoExist);
                }
            }
            else
            {
                throw new BusinessException(Errors.ErrorSelectedClauses);
            }

        }

        public CompanyPolicy SaveCompanyCoinsurance(CompanyIssuanceCoInsuranceCompany coInsuranceCompany, List<CompanyIssuanceCoInsuranceCompany> assignedCompanies, BusinessType businessType, int temporalId)
        {
            CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
            if (policy.Id > 0)
            {

                if (policy.Endorsement.EndorsementType == EndorsementType.Emission || policy.Endorsement.EndorsementType == EndorsementType.Renewal)
                {
                    policy.BusinessType = businessType;

                    switch (policy.BusinessType)
                    {
                        case BusinessType.Accepted:
                            //assignedCompanies = new List<CompanyIssuanceCoInsuranceCompany>();
                            //assignedCompanies.Add(assignedCompanies[0]);
                            //assignedCompanies.Add(coInsuranceCompany);
                            break;
                        case BusinessType.CompanyPercentage:
                            assignedCompanies = new List<CompanyIssuanceCoInsuranceCompany>();
                            assignedCompanies.Add(new CompanyIssuanceCoInsuranceCompany { ParticipationPercentageOwn = 100 });
                            break;
                        default:
                            break;
                    }

                    policy.CoInsuranceCompanies = assignedCompanies;

                    if (policy.Summary != null)
                    {
                        List<CompanyRisk> risks = DelegateService.underwritingService.GetCompanyRisksByTemporalId(policy.Id, false);
                        policy.Summary = DelegateService.underwritingService.CalculateSummaryByCompanyPolicy(policy, risks);

                    }


                    policy = CreatePolicyTemporal(policy, false);

                    if (policy != null)
                    {
                        return policy;
                    }
                    else
                    {
                        throw new Exception(Errors.ErrorSaveBusinessType);
                    }
                }
                else
                {
                    return policy;
                }
            }
            else
            {
                throw new BusinessException(Errors.ErrorTempNoExist);
            }
        }

        public CompanyPaymentPlan SaveCompanyPaymentPlan(int temporalId, CompanyPaymentPlan paymentPlan, List<Quota> quotas)
        {

            CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
            if (policy.Id > 0)
            {

                if (policy.Endorsement.EndorsementType == EndorsementType.Emission || policy.Endorsement.EndorsementType == EndorsementType.Renewal)
                {

                    policy.PaymentPlan = paymentPlan;
                    if (quotas != null)
                    {
                        policy.PaymentPlan.Quotas = quotas;
                    }

                    policy = CreatePolicyTemporal(policy, false);
                    if (policy != null)
                    {
                        return policy.PaymentPlan;
                    }
                    else
                    {
                        throw new BusinessException(Errors.ErrorSavePaymentPlan);
                    }
                }
                else
                {
                    return policy.PaymentPlan;
                }
            }
            else
            {
                throw new BusinessException(Errors.ErrorTempNoExist);
            }

        }

        public CompanyPaymentPlan SaveCompanyPremiumFinance(int temporalId, CompanyPaymentPlan premiumFinance)
        {

            CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
            if (policy.Id > 0)
            {

                if (policy.Endorsement.EndorsementType == EndorsementType.Emission || policy.Endorsement.EndorsementType == EndorsementType.Renewal)
                {

                    policy.PaymentPlan = premiumFinance;
                    policy = CreatePolicyTemporal(policy, false);
                    if (policy != null)
                    {
                        return policy.PaymentPlan;
                    }
                    else
                    {
                        throw new BusinessException(Errors.ErrorSavePaymentPlan);
                    }
                }
                else
                {
                    return policy.PaymentPlan;
                }
            }
            else
            {
                throw new BusinessException(Errors.ErrorTempNoExist);
            }

        }

        public CompanyText SaveCompanyTexts(int temporalId, CompanyText companyText)
        {
            CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
            if (policy.Id > 0)
            {
                policy.Text = companyText;
                policy = CreatePolicyTemporal(policy, false);

                if (policy != null)
                {
                    return policy.Text;
                }
                else
                {
                    throw new BusinessException(Errors.ErrorSaveText);
                }
            }
            else
            {
                throw new BusinessException(Errors.ErrorTempNoExist);
            }
        }

        public CompanyPremiumFinance SaveCompanyPremiumFinance(CompanyPolicy policy)
        {
            if (policy.Id > 0)
            {
                PolicyDAO policyDAO = new PolicyDAO();
                policy = policyDAO.CreateCompanyPremiumFinance(policy);

                if (policy != null)
                {
                    bool isMassive = policy.PolicyOrigin != PolicyOrigin.Individual ? true : false;
                    policy = policyDAO.CreatePolicyTemporal(policy, isMassive);
                    return policy.PaymentPlan.PremiumFinance;
                }
                else
                {
                    throw new BusinessException(Errors.ErrorSaveText);
                }
            }
            else
            {
                throw new BusinessException(Errors.ErrorTempNoExist);
            }
        }

        public CompanyPremiumFinance GetCompanyNumberFinalcialPremium(int policyId)
        {
            PolicyDAO policyDAO = new PolicyDAO();
            return policyDAO.GetCompanyNumberFinalcialPremium(policyId);
        }

        public List<Holder> GetCompanyHoldersByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType, TemporalType temporalType)
        {
            List<Holder> holders = DelegateService.underwritingService.GetHoldersByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType);
            if (holders != null)
            {
                if (holders.Count > 1)
                {
                    holders.RemoveAll(x => x.CustomerType == CustomerType.Prospect);
                }
            }

            if (holders != null && holders.Any())
            {
                if (holders.Count == 1 && customerType == CustomerType.Individual && temporalType != TemporalType.Quotation)
                {
                    if (holders[0].InsuredId == 0)
                    {
                        throw new BusinessException(Errors.ErrorPolicyholderWithoutRol);
                    }
                    else if (holders[0].DeclinedDate > DateTime.MinValue)
                    {
                        throw new BusinessException(Errors.ErrorPolicyholderDisabled);
                    }
                    else
                    {
                        holders.Where(x => x.CustomerType == CustomerType.Individual).ToList().AsParallel().ForAll(x => x.CustomerTypeDescription = Errors.Individual);
                        holders.Where(x => x.CustomerType == CustomerType.Prospect).ToList().AsParallel().ForAll(x => x.CustomerTypeDescription = Errors.Prospect);

                        return holders;
                    }
                }
                else
                {
                    holders.Where(x => x.CustomerType == CustomerType.Individual).ToList().AsParallel().ForAll(x => x.CustomerTypeDescription = Errors.Individual);
                    holders.Where(x => x.CustomerType == CustomerType.Prospect).ToList().AsParallel().ForAll(x => x.CustomerTypeDescription = Errors.Prospect);
                    return holders;
                }
            }
            else
            {
                return null;
            }
        }

        public List<CompanyProduct> GetCompanyProductsByAgentIdPrefixId(int agentId, int prefixId, bool isCollective)
        {
            List<CompanyProduct> products = DelegateService.productService.GetCompanyProductsByAgentIdPrefixId(agentId, prefixId);
            if (products.Count > 0)
            {
                if (isCollective)
                {
                    products = products.Where(x => x.IsCollective == true).ToList();
                }
                else
                {
                    products = products.Where(x => x.IsInteractive == true).ToList();
                }
                return products.OrderBy(x => x.Description).ToList();
            }
            else
            {
                throw new BusinessException(Errors.MessageAgentWithoutProduct);
            }
        }

        public bool? SaveCompanyAdditionalDAta(int temporalId, bool calculateMinimumPremium)
        {
            try
            {
                CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                if (policy.Id > 0)
                {
                    policy.CalculateMinPremium = calculateMinimumPremium;
                    policy = CreatePolicyTemporal(policy, false);
                    if (policy != null)
                    {
                        return policy.CalculateMinPremium;
                    }
                    else
                    {
                        throw new BusinessException(Errors.ErrorSaveAdditionalData);
                    }
                }
                else
                {
                    throw new BusinessException(Errors.ErrorTempNoExist);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public int? SaveCompanyCorrelativePolicy(int temporalId, int? correlativePolicyNumber)
        {
            try
            {
                CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                if (policy.Id > 0)
                {
                    policy.CorrelativePolicyNumber = correlativePolicyNumber;
                    policy = CreatePolicyTemporal(policy, false);
                    if (policy != null)
                    {
                        return (int)policy.CorrelativePolicyNumber;
                    }
                    else
                    {
                        throw new BusinessException(Errors.ErrorSaveAdditionalData);
                    }
                }
                else
                {
                    throw new BusinessException(Errors.ErrorTempNoExist);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }


        public string ValidateCompanySurety(int temporalId, CompanyPolicy policy)
        {
            string msj = string.Empty;
            try
            {
                List<CompanyRisk> risks = DelegateService.underwritingService.GetCompanyRisksByTemporalId(temporalId, false);
                if (risks != null && risks.Any())
                {
                    Parallel.ForEach(risks, (risk, state) =>
                    {
                        if (risk?.Coverages == null || risk.Coverages.Count < 1)
                        {
                            msj = Errors.ErrorMinimumCoverage;
                            state.Stop();
                        }
                        var CurrentTo = risk.Coverages.Where(i => i.CoverStatus != CoverageStatusType.NotModified && i.CoverStatus != CoverageStatusType.Excluded).Max(i => i.CurrentTo);
                        var CurrentFrom = risk.Coverages.Where(i => i.CoverStatus != CoverageStatusType.NotModified && i.CoverStatus != CoverageStatusType.Excluded).Max(i => i.CurrentFrom);
                        if (policy.CurrentTo < CurrentTo || CurrentFrom < policy.CurrentFrom)
                        {
                            msj = Errors.ErrorDatesCoveragesPolicy;
                            state.Stop();
                        }

                    });
                }
                else
                {
                    msj = Errors.ErrorRiskNoExist;
                }
            }
            catch (Exception ex)
            {
                msj = ex.GetBaseException().ToString();
            }
            return msj;
        }
        /// <summary>
        /// Gets the status risk policy by temporal identifier.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="coveredRiskType">Type of the covered risk.</param>
        /// <returns></returns>
        public bool GetStatusRiskPolicyByTemporalId(int temporalId, CoveredRiskType coveredRiskType)
        {
            try
            {
                if (coveredRiskType == CoveredRiskType.Location)
                {
                    List<CompanyRisk> risks = DelegateService.underwritingService.GetCompanyRisksByTemporalId(temporalId, false);
                    if (risks != null && risks.Any())
                    {
                        var count = risks.SelectMany(x => x.Coverages).Where(u => u.InsuredObject.Amount == 0 && u.InsuredObject.IsMandatory == true).Count();
                        if (count > 0)
                        {
                            return false;
                        }
                        return true;
                    }
                    else
                    {

                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
        #region Coberuras
        /// <summary>
        /// Gets the company covered risk by temporal identifier.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="isMasive">if set to <c>true</c> [is masive].</param>
        /// <returns></returns>
        public CompanyCoveredRisk GetCompanyCoveredRiskByTemporalId(int temporalId, bool isMasive)
        {
            PolicyDAO policyDAO = new PolicyDAO();
            return policyDAO.GetCompanyCoveredRiskByTemporalId(temporalId, isMasive);
        }
        /// <summary>
        /// Gets the coverage by coverage identifier.
        /// </summary>
        /// <param name="coverageId">The coverage identifier.</param>
        /// <param name="groupCoverageId">The group coverage identifier.</param>
        /// <param name="policyId">The policy identifier.</param>
        /// <returns></returns>
        public CompanyCoverage GetCompanyCoverageByCoverageId(int coverageId, int groupCoverageId, int policyId)
        {
            try
            {
                CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(policyId, false);
                if (policy != null && policy.Product != null)
                {
                    CompanyCoverage coverage = DelegateService.underwritingService.GetCompanyCoverageByCoverageIdProductIdGroupCoverageId(coverageId, policy.Product.Id, groupCoverageId);
                    if (coverage != null)
                    {
                        coverage.EndorsementType = policy.Endorsement.EndorsementType;
                        coverage.CurrentFrom = policy.CurrentFrom;
                        coverage.CurrentTo = policy.CurrentTo;
                        coverage.Days = Convert.ToInt32((coverage.CurrentTo.Value - coverage.CurrentFrom).TotalDays);

                        if (coverage.EndorsementType == EndorsementType.Modification)
                        {
                            coverage.CoverStatus = CoverageStatusType.Included;
                        }

                        coverage.CoverStatusName = Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(coverage.CoverStatus));

                        return coverage;
                    }
                    else
                    {

                        return null;
                    }
                }
                else
                {

                    return null;
                }
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorSearchCoverage);
            }
        }
        #endregion
        #region reglas
        /// <summary>
        /// Runs the rules company policy pre.
        /// </summary>
        /// <param name="policy">The policy.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public CompanyPolicy RunRulesCompanyPolicyPre(CompanyPolicy policy)
        {
            if (policy == null)
            {
                throw new ArgumentException(Errors.ErrorGetCompanyPolicy);
            }
            policy.Product = DelegateService.productService.GetCompanyProductById(policy.Product.Id);
            policy.ExchangeRate.Currency = DelegateService.productService.GetCurrenciesByProductId(policy.Product.Id).FirstOrDefault();
            if (policy != null && policy.Product != null && policy.Product.PreRuleSetId.HasValue)
            {
                policy = DelegateService.underwritingService.RunRulesCompanyPolicy(policy, policy.Product.PreRuleSetId.Value);
            }


            return policy;
        }
        #endregion
        #region endosos
        public List<CompanyEndorsement> GetCiaEndorsementsByFilterPolicy(int branchId, int prefixId, decimal policyNumber, bool isCurrent = false, bool? isExchange = false)
        {
            try
            {
                PolicyBusiness policyBusiness = new PolicyBusiness();
                return policyBusiness.GetCiaEndorsementsByFilterPolicy(branchId, prefixId, policyNumber, isCurrent, isExchange);
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorGetPolicyByPolicyIdEndorsementId);
            }

        }
        public CompanyPolicy GetCiaCurrentStatusPolicyByEndorsementIdIsCurrent(int endorsementId, bool isCurrent)
        {
            try
            {
                PolicyBusiness policyBusiness = new PolicyBusiness();
                return policyBusiness.GetCiaCurrentStatusPolicyByEndorsementIdIsCurrent(endorsementId, isCurrent);
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorGetPolicyByPolicyIdEndorsementId);
            }
        }

        public CompanyPolicy GetEndorsementInformation(int endorsementId, bool isCurrent)
        {
            try
            {
                PolicyBusiness policyBusiness = new PolicyBusiness();
                return policyBusiness.GetEndorsementInformation(endorsementId, isCurrent);
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorGetPolicyByPolicyIdEndorsementId);
            }
        }
        public bool DeleteEndorsementByPolicyIdEndorsementIdEndorsementType(int policyId, int endorsementId, EndorsementType endorsementType)
        {
            try
            {
                PolicyDAO policyDAO = new PolicyDAO();
                return policyDAO.DeleteEndorsementByPolicyIdEndorsementIdEndorsementType(policyId, endorsementId, endorsementType);
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorGetPolicyByPolicyIdEndorsementId);
            }
        }
        #endregion endosos

        public void CreateCompanyPolicyPayer(CompanyPolicy companyPolicy)
        {
            try
            {
                PayerPaymentDAO payerPaymentDAO = new PayerPaymentDAO();
                payerPaymentDAO.CreateCompanyPolicyPayer(companyPolicy);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }
        }

        public int GetPaymentPlanScheduleByPolicyId(int policyId)
        {
            try
            {
                PaymentPlanDAO paymentPlanDAO = new PaymentPlanDAO();
                return paymentPlanDAO.GetPaymentPlanScheduleByPolicyId(policyId);

            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }
        }

        public int GetPaymentPlanScheduleByPolicyEndorsementId(int policyId, int endorsementId)
        {
            try
            {
                PaymentPlanDAO paymentPlanDAO = new PaymentPlanDAO();
                return paymentPlanDAO.GetPaymentPlanScheduleByPolicyEndorsementId(policyId, endorsementId);

            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }
        }


        #region findig polices


        public List<CompanyPolicy> GetCiaPoliciesByPolicy(CompanyPolicy companyPolicy)
        {
            try
            {
                PolicyDAO policyDAO = new PolicyDAO();
                return policyDAO.GetCiaPoliciesByPolicy(companyPolicy);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion  findig polices
        public List<CompanyCoverage> GetCompanyCoveragesByLineBusinessIdSubLineBusinessId(int lineBusinessId, int subLineBusinessId)
        {
            try
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                return coverageBusiness.GetCompanyCoveragesByLineBusinessIdSubLineBusinessId(lineBusinessId, subLineBusinessId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyClause> RemoveClauses(List<CompanyClause> companyClauses, List<int> clauseIds)
        {
            try
            {
                ClauseDAO clauseDAO = new ClauseDAO();
                return clauseDAO.RemoveClauses(companyClauses, clauseIds);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyClause> AddClauses(List<CompanyClause> companyClauses, List<int> clauseIds)
        {
            try
            {
                ClauseDAO clauseDAO = new ClauseDAO();
                return clauseDAO.AddClauses(companyClauses, clauseIds);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<ConditionLevel> GetConditionLevels()
        {
            try
            {
                ClauseDAO clauseDAO = new ClauseDAO();
                return clauseDAO.GetConditionLevels();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyCoverage> RemoveCoverages(List<CompanyCoverage> companyCoverages, List<int> coverageIds)
        {
            try
            {
                CoverageDAO coverageDAO = new CoverageDAO();
                return coverageDAO.RemoveCoverages(companyCoverages, coverageIds);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyIssuanceInsured GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType)
        {
            CompanyIssuanceInsured companyIssuanceInsured = null;
            var insured = DelegateService.underwritingService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType).First();

            if (insured != null)
            {
                var imapper = ModelAssembler.CreateMapIssueCompanyInsured();
                companyIssuanceInsured = imapper.Map<IssuanceInsured, CompanyIssuanceInsured>(insured);
                companyIssuanceInsured.Name = (insured.Surname + " " + (string.IsNullOrEmpty(insured.SecondSurname) ? "" : insured.SecondSurname + " ") + insured.Name);
                companyIssuanceInsured.IdentificationDocument.DocumentType.Description = EnumHelper.GetItemName<DocumentTypes>(insured.IdentificationDocument.DocumentType.Id);
                return companyIssuanceInsured;
            }
            else
            {
                return null;
            }
        }

        public List<CompanyIssuanceInsured> GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType, TemporalType temporalType)
        {
            List<CompanyIssuanceInsured> insureds = null;
            var companyInsureds = DelegateService.underwritingService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType);

            if (companyInsureds != null)
            {
                var config = MapperCache.GetMapper<IssuanceInsured, CompanyIssuanceInsured>(cfg =>
                {
                    cfg.CreateMap<IssuanceInsured, CompanyIssuanceInsured>();
                });
                insureds = config.Map<List<IssuanceInsured>, List<CompanyIssuanceInsured>>(companyInsureds);
            }

            insureds.ForEach(x => x.Name = (x.Surname + " " + (string.IsNullOrEmpty(x.SecondSurname) ? "" : x.SecondSurname + " ") + x.Name));

            if (insureds.Count == 1 && customerType == CustomerType.Individual && temporalType != TemporalType.Quotation)
            {
                if (insureds[0].InsuredId == 0)
                {
                    throw new BusinessException(Errors.ErrorInsuredWithoutRol);
                }
                else if (insureds[0].DeclinedDate > DateTime.MinValue)
                {
                    throw new BusinessException(Errors.ErrorInsuredDisabled);
                }
                else if (insureds[0].CompanyName == null && temporalType != TemporalType.TempQuotation && temporalType != TemporalType.Quotation)
                {
                    throw new BusinessException(Errors.ErrorInsuredWithoutAddress);
                }
                else
                {
                    return insureds;
                }
            }
            else
            {
                insureds.Where(x => x.CustomerType == CustomerType.Individual).ToList().ForEach(x => x.CustomerTypeDescription = Errors.Individual);
                insureds.Where(x => x.CustomerType == CustomerType.Prospect).ToList().ForEach(x => x.CustomerTypeDescription = Errors.Prospect);

                return insureds;
            }

        }

        public List<CompanyIssuanceInsured> GetListCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType)
        {
            List<CompanyIssuanceInsured> insureds = null;
            var companyInsured = DelegateService.underwritingService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType);

            if (companyInsured != null)
            {
                var imapper = ModelAssembler.CreateMapInsuredListType();
                insureds = imapper.Map<List<IssuanceInsured>, List<CompanyIssuanceInsured>>(companyInsured);
            }
            return insureds;
        }

        /// <summary>
        /// Metodo para devolver poliza del esquema report
        /// </summary>
        /// <param name="prefixId">ramo </param>
        /// <param name="branchId">sucursal</param>
        /// <param name="documentNumber">numero de poliza</param>
        /// <param name="endorsementType"> tipo de endos</param>
        /// <returns>modelo company policy</returns>
        public String GetPolicyByEndorsementDocumentNumber(int endorsementId, decimal documentNumber)
        {
            try
            {
                PolicyDAO policyDAO = new PolicyDAO();
                return policyDAO.GetPolicyByEndorsementDocumentNumber(endorsementId, documentNumber);

            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, "GetPolicyByEndorsementDocumentNumber"));
            }
        }

        #region VehicleType

        public List<CompanyVehicleType> ExecuteOperationsCompanyVehicleType(List<CompanyVehicleType> vehicleTypes)
        {
            try
            {
                return ModelAssembler.CreateCompanyVehicleTypes(DelegateService.underwritingServiceCore.ExecuteOperationsVehicleType(ModelAssembler.CreateVehicleTypes(vehicleTypes)));
            }
            catch (Exception ex)
            {
                throw new Exception("Error in ExecuteOperationsCompanyVehicleType", ex);
            }
        }

        public List<CompanyVehicleType> GetCompanyVehicleTypes()
        {
            try
            {
                return ModelAssembler.CreateCompanyVehicleTypes(DelegateService.underwritingService.GetVehicleTypes());
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetCompanyVehicleTypes", ex);
            }
        }

        public string GenerateFileToCompanyVehicleType(string fileName)
        {
            try
            {
                return DelegateService.underwritingServiceCore.GenerateFileToVehicleType(fileName);
            }
            catch (Exception ex)
            {

                throw new Exception("Error in GenerateFileToCompanyVehicleType", ex);
            }
        }

        public string GenerateFileToCompanyVehicleBody(CompanyVehicleType vehicleTypeDTO, string fileName)
        {
            try
            {
                return DelegateService.underwritingServiceCore.GenerateFileToVehicleBody(ModelAssembler.CreateVehicleType(vehicleTypeDTO), fileName);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GenerateFileToCompanyVehicleBody", ex);
            }
        }




        #endregion

        #region Previsora


        #region Crear Poliza Previsora


        /// <summary>
        /// Guardar Temporal de la Poliza
        /// </summary>
        /// <param name="policy">Modelo policy</param>
        /// 


        public CompanyPolicy CompanySavePolicyTemporal(CompanyPolicy policy, bool isMasive,bool polities=false)
        {
            policy.Endorsement.IsMassive = isMasive;
            if (policy.Id == 0)
            {
                return CreatePolicy(ref policy);
            }
            else
            {
                return UpdatePolicy(ref policy,polities);
            }
        }

        private CompanyPolicy CreatePolicy(ref CompanyPolicy policy)
        {
            policy.Endorsement.EndorsementTypeDescription = Errors.ResourceManager.GetString(Core.Application.Utilities.Helper.EnumHelper.GetItemName<Core.Application.UnderwritingServices.Enums.EndorsementType>(policy.Endorsement.EndorsementType));
            policy.BusinessType = Core.Application.UnderwritingServices.Enums.BusinessType.CompanyPercentage;
            var fiscalResponsibility = DelegateService.uniquePersonServiceCore.GetFiscalResponsibilityByIndividualId(policy.Holder.IndividualId);
            if (fiscalResponsibility.Count > 0)
            {
                policy.Holder.FiscalResponsibility = fiscalResponsibility;
            }
            var EmailElectronic = DelegateService.uniquePersonServiceCore.GetEmailsByIndividualId(policy.Holder.IndividualId);
            if (EmailElectronic.Count > 0)
            {
                foreach (Email email in EmailElectronic)
                {
                    if (email.Description != null && email.EmailType.Id == 23)
                    {
                        policy.Holder.Email = email.Description;
                    }
                }
            }
            var insured = DelegateService.uniquePersonServiceCore.GetInsuredByIndividualId(policy.Holder.IndividualId);
            if (insured != null)
            {
                policy.Holder.RegimeType = insured.RegimeType;
            }
            policy.CoInsuranceCompanies = new List<CompanyIssuanceCoInsuranceCompany>
                {
                    new CompanyIssuanceCoInsuranceCompany
                    {
                        ParticipationPercentageOwn = 100
                    }
                };
            if (policy.Request == null && policy.Agencies != null && policy.Agencies.Count == 1)
            {
                PaymentPlanActions(policy);
            }
            else
            {
                GetCommissByAgentIdAgency(policy);
            }
            GetClauses(policy);

            SetDataPolicy(policy);
            policy = CompanySavePolicyTemporal(policy);
            return policy;
        }


        private CompanyPolicy UpdatePolicy(ref CompanyPolicy policy,bool polities)
        {
            PendingOperation pendingOperation = new PendingOperation();
            pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(policy.Id);

            if (pendingOperation != null && pendingOperation.Operation != null)
            {
                CompanyPolicy companyPolicy = COMUT.JsonHelper.DeserializeJson<CompanyPolicy>(pendingOperation.Operation);//JsonConvert.DeserializeObject<CompanyPolicy>(pendingOperation.Operation);
                companyPolicy.Id = pendingOperation.Id;
                bool Cotexist;
                if ((TemporalType)policy.TemporalType == TemporalType.Quotation)
                {
                    Cotexist = (policy.Endorsement.QuotationVersion != 0);
                    SetDataPolicy(policy);
                    policy = SetDataQuatation(policy, companyPolicy);
                    if (Cotexist)
                    {
                        return policy;
                    }
                }
                else
                {
                    switch (companyPolicy.Endorsement.EndorsementType)
                    {
                        case EnumsCore.EndorsementType.Emission:
                        case EnumsCore.EndorsementType.Renewal:
                            policy = SetDataEmission(policy, companyPolicy);
                            break;

                        case EnumsCore.EndorsementType.Modification:
                            policy = SetDataModification(policy, companyPolicy);
                            break;

                        default:
                            break;
                    }
                }
                if (!polities) { 
                policy.InfringementPolicies = ValidateAuthorizationPolicies(policy);
                }
                try
                {
                    policy = SaveTableTemporal(policy);
                    pendingOperation.Operation = JsonConvert.SerializeObject(policy);
                    DelegateService.utilitiesServiceCore.UpdatePendingOperation(pendingOperation);
                    policy = JsonConvert.DeserializeObject<CompanyPolicy>(pendingOperation.Operation);
                    policy.Id = pendingOperation.Id;
                    return policy;
                }
                catch (Exception)
                {
                    throw new BusinessException(Errors.ErrorRecordTemporal);
                }
            }
            else
            {
                throw new BusinessException(Errors.ErrorTempNoExist);
            }
        }

        public CompanyPaymentPlan GetPaymentPlanByPolicyId(int policyd)
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

        public CompanyPolicy SaveTableTemporal(CompanyPolicy companyPolicy)
        {
            PolicyDAO policyDAO = new PolicyDAO();
            companyPolicy = policyDAO.SaveTemporalPolicy(companyPolicy);
            return companyPolicy;
        }

        /// <summary>
        /// Reserva consecutivos de cotizacion
        /// </summary>
        /// <param name="countQuotation">cantidad de cotizaciones</param>
        /// <param name="branchCode">sucursal</param>
        /// <param name="prefixCode">ramo</param>
        /// <returns>lista de reserva numeración cotizaciones</returns>
        private List<int> GetReserveListQuotes(int countQuotation, int branchCode, int prefixCode)
        {
            BusinessQuotationNumber businessQuotationNumber = new BusinessQuotationNumber();
            businessQuotationNumber.ValidateArguments(countQuotation, branchCode, prefixCode);
            List<int> quotationNumber = new List<int>();

            try
            {
                QuotationNumberDAO quotationNumberDAO = new QuotationNumberDAO();
                quotationNumber = quotationNumberDAO.GetReserveListQuotes(countQuotation, branchCode, prefixCode);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }

            return quotationNumber;
        }


        /// <summary>
        /// Se graba Json y Tablas Temporales
        /// </summary>
        /// <param name="companyPolicy"></param>
        /// <returns></returns>
        private CompanyPolicy CompanySavePolicyTemporal(CompanyPolicy companyPolicy)
        {
            try
            {
                PendingOperation pendingOperation = new PendingOperation
                {
                    OperationName = companyPolicy.TemporalTypeDescription,
                    UserId = companyPolicy.UserId,
                    UserName = companyPolicy.User?.AccountName,
                    Operation = JsonConvert.SerializeObject(companyPolicy)
                };
                pendingOperation = DelegateService.utilitiesServiceCore.CreatePendingOperation(pendingOperation);
                companyPolicy.Id = pendingOperation.Id;
                PolicyDAO policyDAO = new PolicyDAO();
                companyPolicy = SaveTableTemporal(companyPolicy);
                pendingOperation.Operation = JsonConvert.SerializeObject(companyPolicy);
                pendingOperation.AdditionalInformation = companyPolicy.Endorsement.TemporalId.ToString();
                DelegateService.utilitiesServiceCore.UpdatePendingOperation(pendingOperation);

                return companyPolicy;
            }
            catch (Exception ex)
            {
                if (ex is BusinessException || ex is ValidationException)
                {
                    throw;
                }
                throw new BusinessException(ex.Message);
            }
        }


        private void PaymentPlanActions(CompanyPolicy policy)
        {
            var mapper = ModelAssembler.CreateMapCiaPaymentPlan();
            policy.PaymentPlan = mapper.Map<Core.Application.UnderwritingServices.Models.PaymentPlan, CompanyPaymentPlan>(GetDefaultPaymentPlanByProductId(policy.Product.Id));
            policy.Agencies[0].Participation = 100;

            if (policy.Agencies[0].Agent.IndividualId == 1 && policy.Agencies[0].Id == 1)
            {
                policy.Agencies[0].Commissions.Add(new IssuanceCommission
                {
                    Percentage = 0,
                    PercentageAdditional = 0,
                    SubLineBusiness = new SubLineBusiness
                    {
                        LineBusiness = new LineBusiness
                        {
                            Id = policy.Prefix.Id
                        },
                    },
                });
            }
            else
            {
                Core.Application.ProductServices.Models.ProductAgencyCommiss productAgencyCommiss = DelegateService.productService.GetCommissByAgentIdAgencyIdProductId(policy.Agencies[0].Agent.IndividualId, policy.Agencies[0].Id, policy.Product.Id);
                policy.Agencies[0].Commissions.Add(new IssuanceCommission
                {
                    Percentage = productAgencyCommiss.CommissPercentage,
                    PercentageAdditional = productAgencyCommiss.AdditionalCommissionPercentage.GetValueOrDefault(0),
                    SubLineBusiness = new SubLineBusiness
                    {
                        LineBusiness = new LineBusiness
                        {
                            Id = policy.Prefix.Id
                        },
                    },
                });
            }


        }


        private static void GetCommissByAgentIdAgency(CompanyPolicy policy)
        {
            foreach (var item in policy.Agencies)
            {
                if (item.Code == 1 && item.Id == 1)
                {
                    item.Commissions.Add(new IssuanceCommission
                    {
                        Percentage = 0,
                        PercentageAdditional = 0,
                        SubLineBusiness = new SubLineBusiness
                        {
                            LineBusiness = new LineBusiness
                            {
                                Id = policy.Prefix.Id
                            },
                        }
                    });
                }
                else
                {
                    Core.Application.ProductServices.Models.ProductAgencyCommiss productAgencyCommiss = DelegateService.productService.GetCommissByAgentIdAgencyIdProductId(item.Code, item.Id, policy.Product.Id);
                    item.Commissions.Add(new IssuanceCommission
                    {
                        Percentage = productAgencyCommiss.CommissPercentage,
                        PercentageAdditional = productAgencyCommiss.AdditionalCommissionPercentage.GetValueOrDefault(0),
                        SubLineBusiness = new SubLineBusiness
                        {
                            LineBusiness = new LineBusiness
                            {
                                Id = policy.Prefix.Id
                            },
                        }
                    });
                }
            }
        }

        private void GetClauses(CompanyPolicy policy)
        {
            var mapper = ModelAssembler.CreateMapCompanyClause();
            List<CompanyClause> clauses = mapper.Map<List<Core.Application.UnderwritingServices.Models.Clause>, List<CompanyClause>>(GetClausesByEmissionLevelConditionLevelId(EnumsCore.EmissionLevel.General, policy.Prefix.Id));

            if (policy.Clauses != null)
            {
                policy.Clauses = policy.Clauses.Where(x => x.IsMandatory == false).ToList();
            }
            else
            {
                policy.Clauses = new List<CompanyClause>();
            }

            if (clauses.Count > 0)
            {
                policy.Clauses.AddRange(clauses.Where(x => x.IsMandatory == true).ToList());
            }
        }

        private void SetDataPolicy(CompanyPolicy policy)
        {
            policy.BeginDate = DateTime.Now;
            policy.EffectPeriod = policy.EffectPeriod == 0 ? DelegateService.commonService.GetExtendedParameterByParameterId((int)ParametersTypes.DaysValidity).NumberParameter.Value : policy.EffectPeriod;
            policy.Product = DelegateService.productService.GetCompanyProductByProductIdPrefixId(policy.Product.Id, policy.Prefix.Id);
            policy.CalculateMinPremium = DelegateService.productService.GetCalculateMinPremiumByProductId(policy.Product.Id);
            policy.InfringementPolicies = ValidateAuthorizationPolicies(policy);

            if (policy.TemporalType == Core.Application.UnderwritingServices.Enums.TemporalType.Quotation)
            {
                if (policy.Endorsement.QuotationId == 0)
                {
                    policy.Endorsement.QuotationId = GetReserveListQuotes(1, policy.Branch.Id, policy.Prefix.Id).FirstOrDefault();
                    policy.Endorsement.QuotationVersion = 0;
                }

            }
        }




        #endregion

        #region CompanyAcceptCoInsurance

        public CompanyAcceptCoInsurance CompanySaveDiscounts(int temporalId, CompanyAcceptCoInsurance acceptCoInsurance)
        {
            if (acceptCoInsurance != null)
            {
                CompanyPolicy policy = CompanyGetPolicyByTemporalId(temporalId, false);
                if (policy.Id > 0)
                {

                    policy.AcceptCoInsurance = new CompanyAcceptCoInsurance();
                    policy.AcceptCoInsurance = acceptCoInsurance;

                    policy = CreatePolicyTemporal(policy, false);

                    if (policy != null)
                    {
                        return acceptCoInsurance;
                    }
                    else
                    {
                        throw new BusinessException(Errors.ErrorSaveAcceptCoInsurance);
                    }
                }
                else
                {
                    throw new BusinessException(Errors.ErrorTempNoExist);
                }
            }
            else
            {
                throw new BusinessException(Errors.ErrorSelectedAcceptCoInsurance);
            }

        }



        #endregion

        #region CompanyPremiumFinance

        public CompanyPaymentPlan CompanySavePremiumFinance(int temporalId, CompanyPaymentPlan companyPaymentPlan)
        {
            if (companyPaymentPlan != null)
            {
                CompanyPolicy policy = CompanyGetPolicyByTemporalId(temporalId, false);
                if (policy.Id > 0)
                {

                    if (policy.PaymentPlan != null)
                    {
                        policy.PaymentPlan.PremiumFinance = new CompanyPremiumFinance();
                        policy.PaymentPlan = companyPaymentPlan;
                    }
                    else
                    {
                        policy.PaymentPlan = new CompanyPaymentPlan();
                        policy.PaymentPlan = companyPaymentPlan;

                    }

                    policy = CreatePolicyTemporal(policy, false);

                    if (policy != null)
                    {
                        return companyPaymentPlan;
                    }
                    else
                    {
                        throw new BusinessException(Errors.ErrorSavePremiumFinance);
                    }
                }
                else
                {
                    throw new BusinessException(Errors.ErrorTempNoExist);
                }
            }
            else
            {
                throw new BusinessException(Errors.ErrorSelectedPremiumFinance);
            }

        }



        #endregion


        #region CompanyDiscounts


        public CompanySummaryComponent CompanySaveDiscounts(int temporalId, CompanySummaryComponent discounts)
        {
            if (discounts != null)
            {
                CompanyPolicy policy = CompanyGetPolicyByTemporalId(temporalId, false);
                if (policy.Id > 0)
                {

                    policy.SummaryComponent = new CompanySummaryComponent();
                    policy.SummaryComponent.Discount = discounts.Discount;
                    policy.SummaryComponent.TotalDiscount = discounts.TotalDiscount;

                    policy = CreatePolicyTemporal(policy, false);

                    if (policy != null)
                    {
                        return discounts;
                    }
                    else
                    {
                        throw new BusinessException(Errors.ErrorSaveDiscounts);
                    }
                }
                else
                {
                    throw new BusinessException(Errors.ErrorTempNoExist);
                }
            }
            else
            {
                throw new BusinessException(Errors.ErrorSelectedDiscounts);
            }

        }


        #endregion

        #region CompanySurcharge
        public List<CompanySurchargeComponent> GetCompanySurcharges()
        {
            try
            {
                SurchargeDAO surchargeDAO = new SurchargeDAO();
                return surchargeDAO.GetSurcharges();
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message);
            }
        }


        public CompanySummaryComponent CompanySaveSurcharge(int temporalId, CompanySummaryComponent surcharge)
        {
            if (surcharge != null)
            {
                CompanyPolicy policy = CompanyGetPolicyByTemporalId(temporalId, false);
                if (policy.Id > 0)
                {

                    policy.SummaryComponent = new CompanySummaryComponent();
                    policy.SummaryComponent.Surcharge = surcharge.Surcharge;
                    policy.SummaryComponent.TotalSurcharge = surcharge.TotalSurcharge;
                    policy = CreatePolicyTemporal(policy, false);

                    if (policy != null)
                    {
                        return surcharge;
                    }
                    else
                    {
                        throw new BusinessException(Errors.ErrorSaveSurcharge);
                    }
                }
                else
                {
                    throw new BusinessException(Errors.ErrorTempNoExist);
                }
            }
            else
            {
                throw new BusinessException(Errors.ErrorSelectedSurcharge);
            }

        }
        #endregion

        #region  Consultar CompanyPolicy Previsora

        public CompanyPolicy CompanyGetTemporalByIdTemporalType(int id, EnumsCore.TemporalType temporalType)
        {
            CompanyPolicy policy = CompanyGetPolicyByTemporalId(id, false);
            if (policy != null && policy.Id > 0)
            {
                var settings = new JsonSerializerSettings();
                //&&  policy.Endorsement.EndorsementType = EndorsementType.Renewal 
                if (policy.TemporalType == temporalType || (policy.TemporalType == TemporalType.Policy && policy.Endorsement.EndorsementType == EndorsementType.Renewal))
                {
                    policy = GetPolicyDescriptions(policy);
                    return policy;
                }
                else if (policy.TemporalType == TemporalType.TempQuotation)
                {
                    throw new BusinessException(string.Format(Errors.ErrorTemporalType, "Cotización"));
                }
                else
                {
                    throw new BusinessException(string.Format(Errors.ErrorTemporalType, GetMessajes<EndorsementType>(policy.Endorsement.EndorsementType)));
                }
            }
            else
            {
                throw new BusinessException(Errors.ErrorTempNoExist);
            }
        }

        private string GetMessajes<T>(object value)
        {
            string result = EnumHelper.GetItemName<T>(value);
            if (!string.IsNullOrEmpty(result))
            {
                result = Errors.ResourceManager.GetString(result);
                if (string.IsNullOrEmpty(result))
                {
                    result = EnumHelper.GetItemName<T>(value);
                }

            }
            return result;
        }
        /// <summary>
        /// Obtener Póliza
        /// </summary>
        /// <param name="temporalId">Id Temporal</param>
        /// <returns>Póliza</returns>
        public CompanyPolicy CompanyGetPolicyByTemporalId(int temporalId, bool isMasive)
        {
            try
            {
                PolicyDAO policyDAO = new PolicyDAO();
                return policyDAO.GetCompanyPolicyByTemporalId(temporalId, isMasive);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }



        #endregion


        #region  Consultar Cotización Previsora

        public CompanyPolicy GetPolicyByPendingOperation(int operationId)
        {

            QuotationDAO quotationDAO = new QuotationDAO();
            return quotationDAO.GetPolicyByPendingOperation(operationId);
        }

        public List<CompanyPolicy> GetCompanyPoliciesByQuotationIdVersionPrefixId(int quotationId, int version, int prefixId, int branchId)
        {
            QuotationBusiness bis = new QuotationBusiness();
            return bis.GetCompanyPoliciesByQuotationIdVersionPrefixId(quotationId, version, prefixId, branchId);
        }

        /// <summary>
        /// Obtener Cotización
        /// </summary>
        /// <param name="quotationId">Id Cotización</param>
        /// <param name="version">Versión</param>
        /// <returns>Cotización</returns>
        public List<CompanyPolicy> GetPoliciesByQuotationIdVersionPrefixId(int quotationId, int version, int prefixId, int branchId)
        {
            try
            {
                QuotationDAO quotationDAO = new QuotationDAO();
                return quotationDAO.GetPoliciesByQuotationIdVersionPrefixId(quotationId, version, prefixId, branchId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion


        #region Reglas
        /// <summary>
        /// Crea FacadeGeneral
        /// </summary>
        /// <param name="companyPolicy"></param>
        /// <returns></returns>
        public Core.Framework.Rules.Facade CreateFacadeGeneral(CompanyPolicy companyPolicy)
        {
            try
            {
                Core.Framework.Rules.Facade facade = new Core.Framework.Rules.Facade();
                EntityAssembler.CreateFacadeGeneral(companyPolicy, facade);
                return facade;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Report
        public ExcelFileServiceModel GeneratePoductionReportServiceModel(CompanyProductionReport CompanyProductionReport)
        {
            ExcelFileServiceModel excelFileServiceModel = new ExcelFileServiceModel();
            CompanyProductionReportDAO companyProductionReportDAO = new CompanyProductionReportDAO();
            UTIMO.Result<string, UTIMO.ErrorModel> result = companyProductionReportDAO.GenerateProductionReport(CompanyProductionReport);

            if (result is UTIMO.ResultError<string, UTIMO.ErrorModel>)
            {
                UTIMO.ErrorModel errorModelResult = (result as UTIMO.ResultError<string, UTIMO.ErrorModel>).Message;
                excelFileServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
                excelFileServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIMO.ResultValue<string, UTIMO.ErrorModel>)
            {
                excelFileServiceModel.FileData = (result as UTIMO.ResultValue<string, UTIMO.ErrorModel>).Value;
            }


            return excelFileServiceModel;
        }

        #endregion



        #endregion

        public List<CompanyJustificationSarlaft> GetJustificationSarlaft()
        {
            PolicyDAO policyDao = new PolicyDAO();
            List<CompanyJustificationSarlaft> listJustification = policyDao.GetJustificationSarlaft();
            return listJustification;
        }




        public List<CompanyPolicyAgent> GetAgenciesByDesciption(string agentId = "", string description = "", string productId = "", string userId = "")
        {
            try
            {
                PolicyDAO policyDao = new PolicyDAO();
                return policyDao.GetAgenciesByDesciption(agentId, description, productId, userId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public List<CompanyCoverage> GetCompanyCoveragesByRiskId(int riskId)
        {

            try
            {
                return ModelAssembler.CreateCompanyCoverages(DelegateService.underwritingServiceCore.GetCoveragesByRiskId(riskId));
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }

        public List<CompanyRiskVehicle> GetCompanyRiskByPlate(string description)
        {
            try
            {
                return ModelAssembler.CreateRiskVehicles(DelegateService.underwritingServiceCore.GetRisksByPlate(description));
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message);
            }
        }

        public CompanyCoverageDeductible GetCompanyCoverageDeductibleByCoverageId(int CoverageId)
        {
            try
            {
                return ModelAssembler.CreateCompanyCoverageDeductible(DelegateService.underwritingServiceCore.GetCoverageDeductibleByCoverageId(CoverageId));
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyPolicy"></param>
        /// <returns></returns>
        public CompanyPolicy CreateNewVersionQuotation(int quotationId)
        {
            try
            {
                QuotationDAO quotationDAO = new QuotationDAO();
                return quotationDAO.CreateNewVersionQuotation(quotationId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #region SubscriptionSearch

        public List<CompanyPolicy> GetQuotationById(int quotationId, int IndividualId, int UserId, DateTime CurrentFrom, DateTime CurrentTo)
        {
            try
            {
                #region mapper
                Policy policy = new Policy();
                policy.Endorsement = new Endorsement();
                policy.Endorsement.QuotationId = quotationId;
                policy.Holder = new Holder();
                policy.Holder.IndividualId = IndividualId;
                policy.UserId = UserId;
                policy.CurrentFrom = CurrentFrom;
                policy.CurrentTo = CurrentTo;
                #endregion
                QuotationDAO quotationDAO = new QuotationDAO();
                return quotationDAO.GetPoliciesByPolicy(policy);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        public List<CompanyQuotationSearch> SearchQuotations(CompanySubscriptionSearch companySubscriptionSearch)
        {
            try
            {
                SubscriptionSearchDAO subscriptionSearchDAO = new SubscriptionSearchDAO();
                return subscriptionSearchDAO.SearchQuotations(companySubscriptionSearch);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<CompanyPolicySearch> SearchPolicies(CompanySubscriptionSearch companySubscriptionSearch)
        {
            try
            {
                SubscriptionSearchDAO subscriptionSearchDAO = new SubscriptionSearchDAO();
                return subscriptionSearchDAO.SearchPolicies(companySubscriptionSearch);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<CompanyTemporalSearch> SearchTemporals(CompanySubscriptionSearch companySubscriptionSearch)
        {
            try
            {
                SubscriptionSearchDAO subscriptionSearchDAO = new SubscriptionSearchDAO();
                return subscriptionSearchDAO.SearchTemporals(companySubscriptionSearch);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Genera el reporte al buscar cotizaciones
        /// </summary>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>Ruta del archivo</returns>
        public MODSM.ExcelFileServiceModel GenerateQuotations(string fileName, CompanySubscriptionSearch companySubscriptionSearch)
        {
            SubscriptionSearchDAO subscriptionSearchDAO = new SubscriptionSearchDAO();
            MODSM.ExcelFileServiceModel excelFileServiceModel = new MODSM.ExcelFileServiceModel();
            UTIMO.Result<string, UTIMO.ErrorModel> result = subscriptionSearchDAO.GenerateQuotation(fileName, companySubscriptionSearch);
            if (result is UTIMO.ResultError<string, UTIMO.ErrorModel>)
            {
                UTIMO.ErrorModel errorModelResult = (result as UTIMO.ResultError<string, UTIMO.ErrorModel>).Message;
                excelFileServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
                excelFileServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIMO.ResultValue<string, UTIMO.ErrorModel>)
            {
                excelFileServiceModel.FileData = (result as UTIMO.ResultValue<string, UTIMO.ErrorModel>).Value;
            }
            return excelFileServiceModel;
        }

        /// <summary>
        /// Genera el reporte al buscar polizas
        /// </summary>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>Ruta del archivo</returns>
        public MODSM.ExcelFileServiceModel GeneratePolicies(string fileName, CompanySubscriptionSearch companySubscriptionSearch)
        {
            SubscriptionSearchDAO subscriptionSearchDAO = new SubscriptionSearchDAO();
            MODSM.ExcelFileServiceModel excelFileServiceModel = new MODSM.ExcelFileServiceModel();
            UTIMO.Result<string, UTIMO.ErrorModel> result = subscriptionSearchDAO.GeneratePolicy(fileName, companySubscriptionSearch);
            if (result is UTIMO.ResultError<string, UTIMO.ErrorModel>)
            {
                UTIMO.ErrorModel errorModelResult = (result as UTIMO.ResultError<string, UTIMO.ErrorModel>).Message;
                excelFileServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
                excelFileServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTIMO.ResultValue<string, UTIMO.ErrorModel>)
            {
                excelFileServiceModel.FileData = (result as UTIMO.ResultValue<string, UTIMO.ErrorModel>).Value;
            }

            return excelFileServiceModel;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PolicyId"></param>
        /// <param name="EndorsementId"></param>
        public void SaveTextLarge(int PolicyId, int EndorsementId)
        {
            PolicyDAO obj = new PolicyDAO();
            obj.SaveTextLarge(PolicyId, EndorsementId);

        }

        /// <summary>
        /// Validar Coverturas Con Post Contractuales
        /// </summary>
        /// <param name="Policyid"></param>
        /// <returns></returns>
        public System.Collections.ArrayList ValidateCoveragePostContractual(int Policyid)
        {

            try
            {

                CoverageDAO coverage = new CoverageDAO();
                return coverage.ValidateCoveragePostContractual(Policyid);


            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyPolicy GetCiaCurrentStatusPolicyByEndorsementIdIsCurrentCompany(int endorsementId, bool isCurrent, bool fromPrinting = false)
        {
            try
            {
                PolicyDAO policyDAO = new PolicyDAO();
                return policyDAO.GetCurrentStatusPolicyByEndorsementIdIsCurrentCompany(endorsementId, isCurrent, fromPrinting);
            }
            catch (Exception)
            {

                throw new BusinessException(Errors.ErrorGetPolicyByPolicyIdEndorsementId);
            }
        }

        public int GetCurrentRiskNumByPolicyId(int policyId)
        {
            try
            {
                PolicyDAO policyDAO = new PolicyDAO();
                return policyDAO.GetCurrentRiskNumByPolicyId(policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorRiskNotFound), ex);
            }
        }

        public void RecordEndorsementOperation(int endorsementId, int pendingOperationId)
        {
            try
            {
                PolicyDAO policyDAO = new PolicyDAO();
                policyDAO.RecordEndorsementOperation(endorsementId, pendingOperationId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorRecordEndorsement), ex);
            }
        }


        public CompanyPrvCoverage GetPrvCoverageByIdAndNum(int coverageId, int coverageNum)
        {
            try
            {
                CoverageDAO coverageDAO = new CoverageDAO();
                var coverage = coverageDAO.GetPrvCoverageByIdAndNum(coverageId, coverageNum);
                if (coverage == null)
                {
                    return null;
                }
                return ModelAssembler.CreatePrvCoverageToCompany(coverage);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Get Prv_Coverage");
            }
        }

        /// <summary>
        /// Obtener la fecha de cotización.
        /// </summary>
        /// <param name="moduleCode"></param>
        /// <param name="issueDate"></param>
        /// <returns>fecha de cotización</returns>
        public DateTime GetQuotationDate(int moduleCode, DateTime issueDate)
        {
            try
            {
                QuotationDAO quotationDAO = new QuotationDAO();
                return quotationDAO.GetQuotationDate(moduleCode, issueDate);
            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorGetQuotationDate, ex);
            }
        }

        public List<PayerPayment> CalculatePayerPayment(CompanyPolicy companyPolicy, bool RequestIsOpen, DateTime RequestFrom, DateTime RequestTo)
        {
            try
            {
                PayerPaymentDAO payerPaymentDAO = new PayerPaymentDAO();
                return payerPaymentDAO.Calculate(companyPolicy, RequestIsOpen, RequestFrom, RequestTo);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorInCalculatePayerPayment));
            }
        }

        public CompanyPrvCoverage CreatePrvCoverage(CompanyPrvCoverage prvCoverage)
        {
            try
            {
                CoverageDAO coverageDAO = new CoverageDAO();
                var prvCoverageEntity = ModelAssembler.CreateCompanyToPrvCoverage(prvCoverage);
                coverageDAO.CreatePrvCoverage(prvCoverageEntity);
                return prvCoverage;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Create Prv_Coverage");
            }
        }

        public List<Quota> CalculateQuotasWithrequestGroupig(List<PayerPayment> payerPayments)
        {
            try
            {
                PayerPaymentDAO payerPaymentDAO = new PayerPaymentDAO();
                return payerPaymentDAO.CalculateQuotasWithrequestGroupig(payerPayments);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorInCalculatePayerPayment));
            }
        }

        /// <summary>
        /// Obtener un riesgo especifico
        /// </summary>
        /// <param name="policyId">The policy identifier.</param>
        /// <param name="riskId">The risk identifier.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public List<CompanyCoverage> GetCompanyCoveragesByPolicyIdByRiskId(int policyId, int riskId)
        {
            try
            {
                CoverageDAO coverageDAO = new CoverageDAO();
                return coverageDAO.GetCompanyCoveragesByPolicyIdByRiskId(policyId, riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<Endorsement> GetEndorsementsContainByPolicyId(int policyId)
        {
            try
            {
                return DelegateService.underwritingServiceCore.GetEndorsementsAvaibleByPolicyId(policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public CiaRatingZoneBranch CreateCiaRatingZoneBranch(CiaRatingZoneBranch ciaRatingZoneBranch)
        {
            try
            {
                CiaRatingZoneBranchBusiness ciaRatingZoneBranchBusiness = new CiaRatingZoneBranchBusiness();
                return ciaRatingZoneBranchBusiness.CreateCiaRatingZoneBranch(ciaRatingZoneBranch);
            }
            catch (System.Exception ex)
            {
                throw new BusinessException("Error in CreateCiaRatingZoneBranch", ex);
            }
        }

        public void DeleteCiaBranchRatingZone(int ratingZoneCode, int branchCode)
        {
            try
            {
                CiaRatingZoneBranchBusiness ciaRatingZoneBranchBusiness = new CiaRatingZoneBranchBusiness();
                ciaRatingZoneBranchBusiness.DeleteCiaBranchRatingZone(ratingZoneCode, branchCode);
            }
            catch (System.Exception ex)
            {
                throw new BusinessException("Error in DeleteCiaBranchRatingZone", ex);
            }
        }

        public CiaRatingZoneBranch GetRatingZoneBranch(int ratingZoneCode, int branchCode)
        {
            try
            {
                CiaRatingZoneBranchBusiness ciaRatingZoneBranchBusiness = new CiaRatingZoneBranchBusiness();
                return ciaRatingZoneBranchBusiness.GetRatingZoneBranch(ratingZoneCode, branchCode);
            }
            catch (System.Exception ex)
            {
                throw new BusinessException("Error in GetRatingZoneBranch", ex);
            }
        }

        public List<CiaRatingZoneBranch> GetRatingZonesBranchs()
        {
            try
            {
                CiaRatingZoneBranchBusiness ciaRatingZoneBranchBusiness = new CiaRatingZoneBranchBusiness();
                return ciaRatingZoneBranchBusiness.GetRatingZonesBranchs();
            }
            catch (System.Exception ex)
            {
                throw new BusinessException("Error in GetRatingZonesBranchs", ex);
            }
        }

        public List<CompanyRatingZone> GetRatingZonesByPrefixIdAndBranchId(int prefixId, int branchId)
        {
            try
            {
                CiaRatingZoneBranchBusiness ciaRatingZoneBranchBusiness = new CiaRatingZoneBranchBusiness();
                return ciaRatingZoneBranchBusiness.GetRatingZonesByPrefixIdAndBranchId(prefixId, branchId);
            }
            catch (System.Exception ex)
            {
                throw new BusinessException("Error in GetRatingZonesByPrefixIdAndBranchId", ex);
            }

        }

        public List<CompanyRatingZone> GetCompanyRatingZonesByPrefixId(int prefixId)
        {
            try
            {
                return ModelAssembler.CreateCompanyRatingZones(DelegateService.underwritingService.GetRatingZonesByPrefixId(prefixId));
            }
            catch (System.Exception ex)
            {
                throw new BusinessException("Error in GetCompanyRatingZonesByPrefixId", ex);
            }

        }

        public List<CompanyProduct> GetCompanyProductsByAgentIdPrefixIdIsGreen(int agentId, int prefixId, bool isGreen)
        {
            List<CompanyProduct> products = DelegateService.productService.GetCompanyProductsByAgentIdPrefixIdIsGreen(agentId, prefixId, isGreen);
            if (products.Count > 0)
            {
                return products.OrderBy(x => x.Description).ToList();
            }
            else
            {
                throw new BusinessException(Errors.ErrorGetCompanyProductsByAgentIdPrefixIdIsGreen);
            }
        }

        public List<CiaRatingZoneBranch> SaveCiaRatingZoneBranch(List<CompanyRatingZone> companyRatingZones, int branchId)
        {
            try
            {
                CiaRatingZoneBranchBusiness ciaRatingZoneBranchBusiness = new CiaRatingZoneBranchBusiness();
                return ciaRatingZoneBranchBusiness.SaveCiaRatingZoneBranch(companyRatingZones, branchId);
            }
            catch (System.Exception ex)
            {
                throw new BusinessException("Error in SaveCiaRatingZoneBranch", ex);
            }
        }
        public List<CompanyRatingZone> GetRatingZonesAndPrefixAndBranch()
        {
            try
            {
                CiaRatingZoneBranchBusiness ciaRatingZoneBranchBusiness = new CiaRatingZoneBranchBusiness();
                return ciaRatingZoneBranchBusiness.GetRatingZonesAndPrefixAndBranch();
            }
            catch (System.Exception ex)
            {
                throw new BusinessException("Error in GetRatingZonesAndPrefixAndBranch", ex);
            }
        }

        public string GenerateFileToCiaRatingZone(List<CompanyRatingZone> companyRatingZones, string fileName)
        {
            try
            {
                CiaRatingZoneBranchBusiness ciaRatingZoneBranchBusiness = new CiaRatingZoneBranchBusiness();
                return ciaRatingZoneBranchBusiness.GenerateFileToCiaRatingZone(companyRatingZones, fileName);
            }
            catch (System.Exception ex)
            {
                throw new BusinessException("Error in GenerateFileToCiaRatingZone", ex);
            }
        }

        public List<CompanyEndorsement> GetCoPolicyEndorsementsByPrefixIdBranchIdPolicyNumber(int prefixId, int branchId, decimal policyNumber)
        {
            try
            {
                return ModelAssembler.CreateMapCompanyEndorsement(
                    DelegateService.underwritingServiceCore.GetPolicyEndorsementsByPrefixIdBranchIdPolicyNumber(prefixId, branchId, policyNumber));
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyRisk> GetRiskByPolicyIdEndorsmentId(int policyId, int endorsementId)
        {
            try
            {
                RiskDAO riskDAO = new RiskDAO();
                return riskDAO.GetRiskByPolicyIdEndorsmentId(policyId, endorsementId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyEndorsement> GetCoPolicyEndorsementsWithPremiumByPolicyId(int policyId)
        {
            try
            {
                return new PolicyBusiness().GetCoPolicyEndorsementsWithPremiumByPolicyId(policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public CompanyPrvCoverage UpdatePrvCoverage(CompanyPrvCoverage prvCoverage)
        {
            try
            {
                CoverageDAO coverageDAO = new CoverageDAO();
                var prvCoverageEntity = ModelAssembler.CreateCompanyToPrvCoverage(prvCoverage);
                coverageDAO.UpdatePrvCoverage(prvCoverageEntity);
                return prvCoverage;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// Actualiza el numero de documento de la poliza al final de emitir
        /// </summary>
        /// <param name="companyPolicy"></param>
        /// <returns></returns>
        public CompanyPolicy UpdateCompanyPolicyDocumentNumber(CompanyPolicy companyPolicy)
        {
            PolicyDAO policyDAO = new PolicyDAO();
            try
            {
                return companyPolicy = policyDAO.UpdateCompanyPolicyDocumentNumber(companyPolicy);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public int? GetOperationIdTemSubscription(int temporalId)
        {
            try
            {
                PolicyDAO policyDAO = new PolicyDAO();
                return policyDAO.GetOperationIdTemSubscription(temporalId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public int GetEndorsementRiskCount(int policyId, EndorsementType endorsementType)
        {
            try
            {
                PolicyDAO policyDAO = new PolicyDAO();
                return policyDAO.GetEndorsementRiskCount(policyId, endorsementType);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public TemporalDTO GetTemporalByDocumentNumberPrefixIdBrachId(decimal documentNumber, int prefixId, int branchId)
        {
            try
            {
                PolicyDAO policyDAO = new PolicyDAO();
                return policyDAO.GetTemporalByDocumentNumberPrefixIdBrachId(documentNumber, prefixId, branchId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #region optimizacion
        public List<EndorsementDTO> GetCompanyEndorsementsByFilterPolicy(int branchId, int prefixId, decimal policyNumber, bool isCurrent = true)
        {
            try
            {
                PolicyDAO policyDAO = new PolicyDAO();
                return policyDAO.GetCompanyEndorsementsByFilterPolicy(branchId, prefixId, policyNumber);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Gets the current policy by endorsement identifier.
        /// </summary>
        /// <param name="endorsementId">The endorsement identifier.</param>
        /// <param name="isCurrent">if set to <c>true</c> [is current].</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public CompanyPolicy GetCurrentPolicyByEndorsementId(int endorsementId, bool isCurrent = true)
        {
            try
            {
                PolicyDAO policyDAO = new PolicyDAO();
                return policyDAO.GetCurrentPolicyByEndorsementId(endorsementId, isCurrent);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Gets the temporal by policy identifier endorsement identifier.
        /// </summary>
        /// <param name="policyId">The policy identifier.</param>
        /// <param name="endorsementId">The endorsement identifier.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public TemporalDTO GetTemporalByPolicyIdEndorsementId(int policyId, int endorsementId)
        {
            try
            {
                PolicyDAO policyDAO = new PolicyDAO();
                return policyDAO.GetTemporalByPolicyIdEndorsementId(policyId, endorsementId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion optimizacion

        public Tuple<Holder, List<IssuanceCompanyName>> GetHolderByIndividualId(string individual, CustomerType? customerType)
        {
            try
            {
                HolderCompDAO holderDAO = new HolderCompDAO();

                return holderDAO.GetHolderByIndividualId(individual, customerType);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<Holder> GetHoldersByDocument(string document, CustomerType? customerType)
        {
            try
            {
                HolderCompDAO holderDAO = new HolderCompDAO();

                return holderDAO.GetHoldersByDocument(document, customerType);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public List<Holder> GetPersonOrCompanyByDescription(string description, CustomerType? customerType)
        {
            try
            {
                HolderCompDAO holderDAO = new HolderCompDAO();

                return holderDAO.GetPersonOrCompanyByDescription(description, customerType);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyPaymentPlan GetDefaultPaymentPlan(int productId)
        {
            var imapper = ModelAssembler.CreateMapCiaPaymentPlan();
            return imapper.Map<PaymentPlan, CompanyPaymentPlan>(DelegateService.underwritingService.GetDefaultPaymentPlanByProductId(productId));
        }

        public CompanyPolicy ValidateApplyPremiumFinance(CompanyPolicy companyPolicy, CompanyIssuanceInsured companyIssuanceInsured)
        {
            try
            {
                PaymentPlanDAO paymentPlanDAO = new PaymentPlanDAO();
                return paymentPlanDAO.ValidateApplyPremiumFinance(companyPolicy, companyIssuanceInsured);
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorValidateApplyPremiumFinance);
            }
        }

        public List<DynamicConcept> LoadDynamicPropertiesRiskConcepts(int policyId, int endorsementId, int riskId, int riskNum)
        {
            try
            {
                DynamicPropertiesCollectionDAO dynamicPropertiesCollectionDAO = new DynamicPropertiesCollectionDAO();
                return dynamicPropertiesCollectionDAO.LoadDynamicPropertiesRiskConcepts(policyId, endorsementId, riskId, riskNum);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetPolicyByPolicyIdEndorsementId), ex);
            }
        }

        public Endorsement SaveContractObjectPolicyId(int endorsementId, int riskId, string textRisk, string textPolicy)
        {
            try
            {
                PolicyDAO policyDAO = new PolicyDAO();
                return policyDAO.SaveContractObjectPolicyId(endorsementId, riskId, textRisk, textPolicy);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public EndoChangeText SaveLog(EndoChangeText endoChangeText)
        {
            try
            {
                PolicyDAO policyDAO = new PolicyDAO();
                return policyDAO.SaveLog(endoChangeText);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public long GetRateCoveragesByCoverageIdPolicyId(int policyId, int coverageId)
        {
            try
            {
                CoverageDAO coverageDAO = new CoverageDAO();
                return coverageDAO.GetRateCoveragesByCoverageIdPolicyId(policyId, coverageId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public decimal GetCumulusQSise(int individualId)
        {
            try
            {
                PolicyDAO policyDao = new PolicyDAO();
                return policyDao.GetCumulusQSise(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}