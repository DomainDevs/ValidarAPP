using Sistran.Company.Application.MassiveServices.EEProvider.DAOs;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using Sistran.Company.Application.MassiveServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Company.Application.Massive.EEProvider.DAOs;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Company.Application.MassiveServices.EEProvider.Resources;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.ProductServices.Models;
using Sistran.Core.Services.UtilitiesServices.Models;
using AutoMapper;
using UWMO = Sistran.Core.Application.UnderwritingServices.Models;
using System.Linq;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.MassiveServices.EEProvider.DAOs;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using EnumsCore = Sistran.Core.Application.UnderwritingServices.Enums;

namespace Sistran.Company.Application.MassiveServices.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class MassiveServiceEEProvider : Core.Application.MassiveServices.EEProvider.MassiveServiceEEProviderCore, IMassiveService
    {

        public MassiveServiceEEProvider()
        {
            Errors.Culture = KeySettings.ServiceResourceCulture;
        }

        /// <summary>
        /// Obtener Solicitudes Agrupadoras Por Grupo Facturación, Id O Descripción Solicitud
        /// </summary>
        /// <param name="billingGroup">Id Grupo Facturación</param>
        /// <param name="description">Id O Descripción Solicitud</param>
        /// <returns>Solicitudes Agrupadoras</returns>
        public List<CompanyRequest> GetCompanyRequestsByBillingGroupIdDescriptionRequestNumber(int billingGroupId, string description, int? requestNumber)
        {
            try
            {
                CompanyRequestDAO companyRequestDAO = new CompanyRequestDAO();
                return companyRequestDAO.GetCompanyRequestsByBillingGroupIdDescriptionRequestNumber(billingGroupId, description, requestNumber);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetRequests), ex);
            }
        }

        /// <summary>
        /// Obtener Ramos Comerciales Por Agente
        /// </summary>
        /// <param name="agentId">Id Agente</param>
        /// <returns>Ramos Comerciales</returns>
        public List<Prefix> GetPrefixesByAgentId(int agentId)
        {
            try
            {
                CompanyPrefixDAO companyPrefixDAO = new CompanyPrefixDAO();
                return companyPrefixDAO.GetPrefixesByAgentId(agentId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetPrefixes), ex);
            }
        }

        public List<Prefix> GetPrefixesToMassive()
        {
            try
            {
                CompanyPrefixDAO companyPrefixDAO = new CompanyPrefixDAO();
                return companyPrefixDAO.GetPrefixesToMassive();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetPrefixes), ex);
            }
        }

        /// <summary>
        /// Guarda en base de datos una nueva solicitud agrupadora
        /// </summary>
        /// <param name="request"> Modelo de solicitud agrupadora </param>
        /// <param name="userId"> Identificador del usuario </param>
        /// <returns> Modelo de la solicitud agrupadora </returns>
        public CompanyRequest CreateCompanyRequest(CompanyRequest companyRequest)
        {
            try
            {
                CompanyRequestDAO companyRequestDAO = new CompanyRequestDAO();
                return companyRequestDAO.CreateCompanyRequest(companyRequest);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetRequest), ex);
            }
        }

        /// <summary>
        /// Guarda en base de datos renovacion solicitud agrupadora
        /// </summary>
        /// <param name="request"> Modelo de solicitud agrupadora </param>
        /// <param name="userId"> Identificador del usuario </param>
        /// <returns> Modelo de la solicitud agrupadora </returns>
        public CompanyRequest SaveRenewalRequest(Models.CompanyRequest request, int userId)
        {
            try
            {
                CompanyRequestDAO coRequestDAO = new CompanyRequestDAO();
                return coRequestDAO.SaveRenewalRequest(request, userId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetRequest), ex);
            }
        }

        /// <summary>
        /// Buscar Solicitudes por código
        /// </summary>
        /// <param name="requestId">Codigo de solicitud</param>        
        /// <returns></returns>  
        public CompanyRequest GetCoRequestByRequestId(int requestId)
        {
            try
            {
                CompanyRequestDAO coRequestDAO = new CompanyRequestDAO();
                return coRequestDAO.GetCoRequestByRequestId(requestId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetRequest), ex);
            }
        }

        /// <summary>
        /// Buscar los productos para un agente habilitados para solicitud agrupadora
        /// </summary>
        /// <param name="agentId">Identificador del agente</param>
        /// <param name="prefixId">Identificador del ramo</param>
        /// <param name="isRequest">Esta habilitado para solicitud agrupadora</param>
        /// <returns>Lista Model.Product</returns>        
        public List<Product> GetProductsByAgentIdPrefixId(int agentId, int prefixId)
        {
            try
            {
                ProductDAO productDAO = new ProductDAO();
                return productDAO.GetProductsByAgentIdPrefixId(agentId, prefixId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetProducts), ex);
            }
        }

        /// <summary>
        /// Buscar los productos para un agente habilitados para colectivas
        /// </summary>
        /// <param name="agentId">Identificador del agente</param>
        /// <param name="prefixId">Identificador del ramo</param>
        /// <returns>Lista Model.Product</returns>        
        public List<Product> GetCollectiveProductsByAgentIdPrefixId(int agentId, int prefixId)
        {
            try
            {
                ProductDAO productDAO = new ProductDAO();
                return productDAO.GetCollectiveProductsByAgentIdPrefixId(agentId, prefixId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetProducts), ex);
            }
        }

        /// <summary>
        /// Buscar Solicitudes Por Id o Descripción
        /// </summary>
        /// <param name="description">Id o Descripción</param>
        /// <returns>Lista de Solicitudes</returns>
        public List<CompanyRequest> GetCoRequestByDescription(string description)
        {
            try
            {
                CompanyRequestDAO coRequestDAO = new CompanyRequestDAO();
                return coRequestDAO.GetCoRequestByDescription(description);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetRequests), ex);
            }
        }

        /// <summary>
        /// Busca los datos del typo de negocio
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="requestEndorsementId"></param>
        /// <param name="businessType"></param>
        /// <returns></returns>
        public List<CompanyIssuanceCoInsuranceCompany> GetCoRequestCoinsuranceByRequedIdByRequestEndorsementIdType(int requestId, int requestEndorsementId, BusinessType businessType)
        {
            try
            {
                CoRequestCoinsuranceDAO coRequestCoinsuranceDAO = new CoRequestCoinsuranceDAO();
                return coRequestCoinsuranceDAO.GetCoRequestCoinsuranceByRequedIdByRequestEndorsementIdType(requestId, requestEndorsementId, businessType);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCoInsurances), ex);
            }
        }

        /// <summary>
        /// Obtiene el valor deacuerdo a la fecha desde
        /// </summary>
        /// <param name="PolicyFrom"></param>
        /// <param name="companyRequest"></param>
        /// <returns></returns>
        public CompanyRequestEndorsement GetCompanyRequestEndorsmentPolicyWithRequest(DateTime PolicyFrom, CompanyRequest companyRequest)
        {
            try
            {
                CompanyRequestDAO companyRequestDAO = new CompanyRequestDAO();
                return companyRequestDAO.GetCompanyRequestEndorsmentPolicyWithRequest(PolicyFrom, companyRequest);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetRequest), ex);
            }
        }

        public List<FilterIndividual> GetFilterIndividuals(int userId, int branchId, List<File> files, string templatePropertyName)
        {
            try
            {
                FilterIndividualDAO filterIndividualDAO = new FilterIndividualDAO();
                return filterIndividualDAO.GetFilterIndividuals(userId, files, templatePropertyName, branchId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorFilterIndividual), ex);
            }
        }

        public Holder CreateHolder(Row row, List<FilterIndividual> filtersIndividuals)
        {
            try
            {
                IndividualDAO individualDAO = new IndividualDAO();
                return individualDAO.CreateHolder(row, filtersIndividuals);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateHolder), ex);
            }
        }

        public List<CompanyBeneficiary> CreateAdditionalBeneficiaries(Template beneficiariesTemplate, List<FilterIndividual> filtersIndividuals)
        {
            try
            {
                IndividualDAO individualDAO = new IndividualDAO();
                return individualDAO.CreateAdditionalBeneficiaries(beneficiariesTemplate, filtersIndividuals);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateBeneficiaries), ex);
            }
        }

        public CompanyIssuanceInsured CreateInsured(Row row, Holder holder, List<FilterIndividual> filtersIndividuals)
        {
            try
            {
                IndividualDAO individualDAO = new IndividualDAO();
                return individualDAO.CreateInsured(row, holder, filtersIndividuals);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateInsured), ex);
            }
        }

        public CompanyBeneficiary CreateBeneficiary(Row row, CompanyIssuanceInsured insured, List<FilterIndividual> filtersIndividuals)
        {
            try
            {
                IndividualDAO individualDAO = new IndividualDAO();
                return individualDAO.CreateBeneficiary(row, insured, filtersIndividuals);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateBeneficiary), ex);
            }
        }

        public List<CompanyCoverage> CreateAdditionalCoverages(List<CompanyCoverage> Allcoverages, List<CompanyCoverage> Actualcoverages, List<Row> rows)
        {
            try
            {
                MassiveTemplateDAO massiveTemplateDAO = new MassiveTemplateDAO();
                return massiveTemplateDAO.CreateAdditionalCoverages(Allcoverages, Actualcoverages, rows);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateDeductibles), ex);
            }
        }

        /// <summary>
        /// Crea Intermediarios adicionales
        /// </summary>
        /// <param name="template"></param>
        /// <returns>Lista de IssuanceAgency</returns>
        public List<IssuanceAgency> CreateAdditionalAgencies(Template template, ref string errorAgencies)
        {
            try
            {
                MassiveTemplateDAO massiveTemplateDAO = new MassiveTemplateDAO();
                return massiveTemplateDAO.CreateAdditionalAgencies(template, ref errorAgencies);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateAdditionalIntermediaries), ex);
            }
        }

        public List<CompanyCoverage> CreateDeductibles(List<CompanyCoverage> coverages, Template template)
        {
            try
            {
                MassiveTemplateDAO massiveTemplateDAO = new MassiveTemplateDAO();
                return massiveTemplateDAO.CreateDeductibles(coverages, template);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateDeductibles), ex);
            }
        }

        public List<CompanyIssuanceCoInsuranceCompany> CreateCoInsuranceAssigned(CompanyPolicy companyPolicy, Template template)
        {
            try
            {
                MassiveTemplateDAO massiveTemplateDAO = new MassiveTemplateDAO();
                return massiveTemplateDAO.CreateCoInsuranceAssigned(companyPolicy,template);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCoInsuranceAssigned), ex);
            }
        }

        public List<CompanyIssuanceCoInsuranceCompany> CreateCoInsuranceAccepted(CompanyPolicy companyPolicy, File file)
        {
            try
            {
                MassiveTemplateDAO massiveTemplateDAO = new MassiveTemplateDAO();
                return massiveTemplateDAO.CreateCoInsuranceAccepted(companyPolicy, file);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCoInsuranceAccepted), ex);
            }
        }

        public bool UpdateMassiveLoadStatusIfComplete(int massiveLoadId, bool changeStatus = true)
        {
            try
            {
                CompanyMassiveLoadDAO companyMassiveLoadDAO = new CompanyMassiveLoadDAO();
                return companyMassiveLoadDAO.UpdateMassiveLoadStatusIfComplete(massiveLoadId,changeStatus);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorUpdateMassiveLoad), ex);
            }
        }

        public int GetpendingOperationIdByMassiveLoadIdRowId(int massiveLoadId, int rowId)
        {
            try
            {
                CompanyMassiveLoadDAO companyMassiveLoadDAO = new CompanyMassiveLoadDAO();
                return companyMassiveLoadDAO.GetpendingOperationIdByMassiveLoadIdRowId(massiveLoadId, rowId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetPendingOperationId), ex);
            }
        }

        #region Cargue
        public override List<MassiveLoad> GetMassiveLoadsByDescription(string description)
        {
            try
            {
                CompanyMassiveLoadDAO massiveLoadDAO = new CompanyMassiveLoadDAO();
                return massiveLoadDAO.GetMassiveLoadsByDescription(description);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetMassiveLoads), ex);
            }
        }
        #endregion
        #region reportes
        public List<Field> GetFields(string serializeFields, CompanyPolicy companyPolicy)
        {
            try
            {
                ReportDAO massiveLoadPolicyDAO = new ReportDAO();
                return massiveLoadPolicyDAO.GetFields(serializeFields, companyPolicy);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetReportFields), ex);
            }
        }

        public string CreateBeneficiaries(List<CompanyBeneficiary> beneficiaries)
        {
            try
            {
                ReportDAO massiveLoadPolicyDAO = new ReportDAO();
                return massiveLoadPolicyDAO.CreateBeneficiaries(beneficiaries);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateBeneficiary), ex);
            }
        }

        public void LoadReportCacheList()
        {
            try
            {
                ReportDAO massiveReportDAO = new ReportDAO();
                massiveReportDAO.LoadReportCacheList();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorLoadReportCacheList), ex);
            }
        }

        public object GetCacheList(string key, string hash)
        {
            try
            {
                ReportDAO massiveReportDAO = new ReportDAO();
                return massiveReportDAO.GetCacheList(key, hash);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCacheList), ex);
            }
        }

        public void ClearCacheList()
        {
            try
            {
                ReportDAO massiveReportDAO = new ReportDAO();
                massiveReportDAO.ClearCacheList();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorClearCacheList), ex);
            }
        }

        public string CreateClauses(List<CompanyClause> clauses)
        {
            try
            {
                ReportDAO massiveLoadPolicyDAO = new ReportDAO();
                return massiveLoadPolicyDAO.CreateClauses(clauses);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateClauses), ex);
            }
        }

        public CompanyPolicy GetCompanyPolicyByMassiveLoadStatusPolicy(MassiveLoadStatus massiveLoadStatus, Policy policy)
        {

            try
            {
                ReportDAO massiveLoadPolicyDAO = new ReportDAO();
                return massiveLoadPolicyDAO.GetCompanyPolicyByMassiveLoadStatusPolicy(massiveLoadStatus, policy);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.GetCompanyPolicyByMassiveLoadStatusPolicy), ex);
            }
        }

        public List<Field> FillInsuredFields(List<Field> fields, CompanyIssuanceInsured mainInsured)
        {
            try
            {
                var reportDAO = new ReportDAO();
                return reportDAO.CreateInsured(fields, mainInsured);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorFillingInsuredFields), ex);
            }
        }

        public List<FilterIndividual> GetDataFilterIndividualRenewal(List<File> files, string templatePropertyName)
        {
            try
            {
                var reportDAO = new FilterIndividualDAO();
                return reportDAO.GetDataFilterIndividualRenewal(files, templatePropertyName);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetFilterIndividuals), ex);
            }
        }

        public List<FilterIndividual> GetDataFilterIndividualRenewalWithPropertyNames(List<File> files, string templatePropertyName, string policyNumberPropertyName, string prefixIdPropertyName)
        {
            try
            {
                var reportDAO = new FilterIndividualDAO();
                return reportDAO.GetDataFilterIndividualRenewal(files, templatePropertyName, policyNumberPropertyName, prefixIdPropertyName);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetFilterIndividuals), ex);
            }
        }

        #endregion

        public List<CompanyClause> GetClauses(Template templateClauses, EmissionLevel emissionLevel)
        {
            try
            {
                CompanyMassiveLoadDAO massiveLoadPolicyDAO = new CompanyMassiveLoadDAO();
                return massiveLoadPolicyDAO.GetClauses(templateClauses, emissionLevel);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetClauses), ex);
            }
        }

        public List<CompanyClause> GetClausesByCoverageId(Template templateClauses, int coverageId)
        {
            try
            {
                CompanyMassiveLoadDAO massiveLoadPolicyDAO = new CompanyMassiveLoadDAO();
                return massiveLoadPolicyDAO.GetClausesByCoverageId(templateClauses, EmissionLevel.Coverage, coverageId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetClauses), ex);
            }
        }

        public List<FilterIndividual> GetFilterIndividualsForCollective(Row policyRow, List<File> riskFiles, int userId, int branchId, string policyNumberPropertyName, string prefixIdPropertyname)
        {
            try
            {
                var filterIndividualDAO = new FilterIndividualDAO();
                return filterIndividualDAO.GetFilterIndividualsForCollective(policyRow, riskFiles, userId, policyNumberPropertyName, prefixIdPropertyname, branchId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetFilterIndividuals), ex);
            }
        }

        #region solicitud
        public CompanyPolicy SaveCompanyRequestTemporal(CompanyPolicy policy)
        {
            CompanyRequest model = GetCoRequestByRequestId(policy.Request.Id);
            CompanyRequestEndorsement endorsement = model.CompanyRequestEndorsements.First();

            if (endorsement.PaymentPlan != null)
            {
                List<UWMO.PaymentPlan> paymentPlans = DelegateService.underwritingService.GetPaymentPlansByProductId(policy.Product.Id);
                if (paymentPlans != null)
                {
                    policy.PaymentPlan = Mapper.Map<UWMO.PaymentPlan, CompanyPaymentPlan>(paymentPlans.First(p => p.Id == endorsement.PaymentPlan.Id));
                }
                else
                {
                    throw new BusinessException("Planes de Pago no Encontrados");
                }
            }

            foreach (IssuanceAgency issuanceAgency in endorsement.Agencies)
            {
                IssuanceAgency agency = DelegateService.underwritingService.GetAgencyByAgentIdAgentAgencyId(issuanceAgency.Agent.IndividualId, issuanceAgency.Id);

                issuanceAgency.FullName = agency.FullName;
                issuanceAgency.Agent.FullName = agency.Agent.FullName;
                issuanceAgency.Commissions = new List<IssuanceCommission>();

                ProductAgencyCommiss productAgencyCommiss = DelegateService.productService.GetCommissByAgentIdAgencyIdProductId(issuanceAgency.Agent.IndividualId, issuanceAgency.Id, policy.Product.Id);

                issuanceAgency.Commissions.Add(new IssuanceCommission
                {
                    Percentage = productAgencyCommiss.CommissPercentage
                });
            }

            policy.Agencies = endorsement.Agencies;
            return policy;
        }
        #endregion


        public List<DynamicConcept> GetDynamicConceptsByTemplate(int? scriptId,Template templateScripts,ref string error)
        {
            try
            {
                MassiveTemplateDAO massiveTemplateDAO = new MassiveTemplateDAO();
                return massiveTemplateDAO.GetDynamicConceptsByTemplate(scriptId,templateScripts, ref error);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetScripts), ex);
            }
        }

        // <summary>
        // Metodo que realiza la actualización de la fila del cargue desde politicas
        // </summary>
        // <param name="massiveLoadId">Id del cargue</param>
        // <param name="temporalId">Id del temporal</param>
        // <returns></returns>
        public string CompanyUpdateMassiveLoadAuthorization(string massiveLoadId, List<string> temporalId)
        {
            try
            {
                MassiveLoadDAO loadDao = new MassiveLoadDAO();
                loadDao.UpdateMassiveLoadAuthorization(massiveLoadId, temporalId);
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorUpdateMassiveLoad), ex);
            }
        }

        public List<CompanyAccessory> GetAccesorysByTemplate(Template templateScripts, CompanyPolicy policy, CompanyVehicle companyVehicle, int coverIdAccesoryNoORig, int coverIdAccesoryORig, ref string error)
        {
            try
            {
                MassiveTemplateDAO massiveTemplateDAO = new MassiveTemplateDAO();
                return massiveTemplateDAO.GetAccesorysByTemplate(templateScripts, policy,companyVehicle,coverIdAccesoryNoORig,coverIdAccesoryORig, ref error);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetAccesory), ex);
            }
        }

        public void GetClausesByTemplate(Template templateScripts, ref List<CompanyClause> companyClauses, ref List<CompanyCoverage> companyCoverages, List<CompanyClause> riskClauses, List<CompanyClause> coverageClause, ref string error)
        {
            try
            {
                MassiveTemplateDAO massiveTemplateDAO = new MassiveTemplateDAO();
                massiveTemplateDAO.GetClauseByTemplate(templateScripts, ref companyClauses,ref companyCoverages, riskClauses, coverageClause, ref error);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetClauses), ex);
            }
        }

        public List<CompanyBeneficiary> GetBeneficiariesAdditional(File file, Template template, List<FilterIndividual> filterIndividuals, List<CompanyBeneficiary> companyBeneficiaries, ref string error)
        {
            try
            {
                MassiveTemplateDAO massiveTemplateDAO = new MassiveTemplateDAO();
                return massiveTemplateDAO.GetBeficiaresAdditional(file, template, filterIndividuals, companyBeneficiaries,ref error);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetBeneficiary), ex);
            };
        }

        public List<Row> GetMassivePlatesValidation(List<Row> rows)
        {
            try
            {
                MassiveTemplateDAO massiveTemplateDAO = new MassiveTemplateDAO();
                return massiveTemplateDAO.GetMassivePlatesValidation(rows);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.DuplicatedPlate), ex);
            }
        }

        public List<IssuanceAgency> GetAgenciesValidation(File file, List<IssuanceAgency> issuanceAgency, ref string error)
        {
            try
            {
                MassiveTemplateDAO massiveTemplateDAO = new MassiveTemplateDAO();
                return massiveTemplateDAO.GetAgenciesValidation(file, issuanceAgency, ref error);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.DuplicatedPlate), ex);
            }
        }
        public List<CompanyClause> GetClausesObligatory(EnumsCore.EmissionLevel emissionLevel, int prefixId, int? conditionLevel)
        {
            ClauseMassiveDAO clauseMassiveDAO = new ClauseMassiveDAO();
            List<CompanyClause> companyClauses = clauseMassiveDAO.GetClauses(emissionLevel, prefixId, conditionLevel);
            return companyClauses;
        }
        public bool GetMassiveLoadErrorStatus(int massiveLoadId)
        {
            CompanyMassiveLoadDAO companyMassiveLoadDAO = new CompanyMassiveLoadDAO();
            try
            {
                return companyMassiveLoadDAO.GetMassiveLoadErrorStatus(massiveLoadId);
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }
    }
}