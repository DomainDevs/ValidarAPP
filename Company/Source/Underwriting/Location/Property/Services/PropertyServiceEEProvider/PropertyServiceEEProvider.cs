using AutoMapper;
using Sistran.Company.Application.Location.PropertyServices.EEProvider.Assemblers;
using Sistran.Company.Application.Location.PropertyServices.EEProvider.Business;
using Sistran.Company.Application.Location.PropertyServices.EEProvider.DAOs;
using Sistran.Company.Application.Location.PropertyServices.EEProvider.Resources;
using Sistran.Company.Application.Location.PropertyServices.Models;
using Sistran.Company.Application.Location.PropertyServices.Enum;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using CiaPersonModel = Sistran.Company.Application.UniquePersonServices.V1.Models;
using CiaUnderwritingModel = Sistran.Company.Application.UnderwritingServices.Models;
using Rules = Sistran.Core.Framework.Rules;
using Sistran.Core.Framework.Rules.Engine;
using Sistran.Core.Framework.Rules.Integration;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Application.Utilities.DataFacade;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;
using Sistran.Company.Application.Location.PropertyServices.DTO;
using TP = Sistran.Core.Application.Utilities.Utility;
using Sistran.Co.Application.Data;
using System.Data;
using System.Diagnostics;
using Newtonsoft.Json;
using Sistran.Company.Application.UnderwritingServices;

namespace Sistran.Company.Application.Location.PropertyServices.EEProvider
{
    /// <summary>
    /// Implementacion de metodos
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class PropertyServiceEEProvider : Sistran.Core.Application.Location.PropertyServices.EEProvider.PropertyServiceEEProvider, IPropertyService
    {

        /// <summary>
        /// Tarifar Poliza
        /// </summary>
        /// <param name="PropertyPolicy">Modelo de Poliza hogar</param>
        /// <param name="runRulesPre">Ejecutar Reglas Pre</param>
        /// <param name="runRulesPost">Ejecutar Reglas Post</param>
        /// <returns>PropertyPolicy</returns>
        public CompanyPropertyRisk QuotateProperty(CompanyPropertyRisk propertyPolicy, bool runRulesPre, bool runRulesPost)
        {
            try
            {
                PropertyRiskBusiness propertyRiskBusiness = new PropertyRiskBusiness();
                return propertyRiskBusiness.QuotateProperty(propertyPolicy, runRulesPre, runRulesPost);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Ejecutar Reglas de Riesgo
        /// </summary>
        /// <param name="propertyRisk">propertyRisk</param>
        /// <param name="ruleSetId">Id Regla</param>
        /// <returns></returns>
        public CompanyPropertyRisk RunRulesRisk(CompanyPropertyRisk propertyRisk, int ruleSetId)
        {
            try
            {
                PropertyRiskBusiness propertyRiskBusiness = new PropertyRiskBusiness();
                return propertyRiskBusiness.RunRulesRisk(propertyRisk, ruleSetId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Tarifar Cobertura
        /// </summary>
        /// <param name="coverage">Cobertura</param>
        /// <param name="runRulesPre">Ejecutar Reglas Pre</param>
        /// <param name="runRulesPost">Ejecutar Reglas Post</param>
        /// <returns>Cobertura</returns>
        public CompanyCoverage QuotateCoverage(CompanyPropertyRisk propertyRisk, CompanyCoverage coverage, bool runRulesPre, bool runRulesPost)
        {
            try
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                return coverageBusiness.Quotate(propertyRisk, coverage, runRulesPre, runRulesPost);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Ejecutar reglas pre cobertura.
        /// </summary>
        /// <param name="coverage">Cobertura</param>
        /// <returns>Cobertura</returns>
        public CompanyCoverage RunRulesCoverage(CompanyPropertyRisk propertyRisk, CompanyCoverage companyCoverage, int ruleSetId)
        {
            try
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                return coverageBusiness.RunRulesCoverage(propertyRisk, companyCoverage, ruleSetId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Tarifar Riesgo Hogar
        /// </summary>
        /// <param name="propertyRisk">Riesgo Hogar</param>
        /// <param name="runRulesPre">Ejecutar Reglas Pre</param>
        /// <param name="runRulesPost">Ejecutar Reglas Post</param>
        /// <returns>Modelo PropertyRisk</returns>
        public List<CompanyPropertyRisk> QuotateProperties(CompanyPolicy companyPolicy, List<CompanyPropertyRisk> companyProperties, bool runRulesPre, bool runRulesPost)
        {
            try
            {
                PropertyRiskBusiness propertyRiskBusiness = new PropertyRiskBusiness();
                return propertyRiskBusiness.QuotateProperties(companyPolicy, companyProperties, runRulesPre, runRulesPost);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Insertar en tablas temporales desde el JSON
        /// </summary>
        /// <param name="propertyRisk">Modelo propertyRisk</param>
        public CompanyPropertyRisk CreatePropertyTemporal(CompanyPropertyRisk propertyRisk, bool isMassive)
        {
            try
            {
                PropertyDAO propertyDAO = new PropertyDAO();
                propertyDAO.CreatePropertyTemporal(propertyRisk, isMassive);
                if (propertyRisk.Risk.Policy != null && propertyRisk.Risk.Policy.TemporalType != TemporalType.TempQuotation)
                {
                    propertyRisk.Risk.LimitRc = new CompanyLimitRc
                    {
                        Id = DelegateService.commonService.GetParameterByParameterId(3000).NumberParameter.Value,
                        LimitSum = DelegateService.commonService.GetParameterByParameterId(3001).NumberParameter.Value,
                    };
                    propertyRisk = propertyDAO.SaveCompanyPropertyTemporalTables(propertyRisk);
                }
                return propertyRisk;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Poliza de hogar
        /// </summary>
        /// <param name="policyId">Id policy</param>
        /// <param name="temporalId">Id temporal</param>
        /// <returns>propertyPolicy</returns>
        public List<CompanyPropertyRisk> GetCompanyPropertiesByEndorsementId(int endorsementId)
        {
            try
            {
                PropertyDAO propertyDAO = new PropertyDAO();
                return propertyDAO.GetCompanyPropertiesByEndorsementId(endorsementId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Poliza de hogar
        /// </summary>
        /// <param name="policyId">Id policy</param>
        /// <param name="temporalId">Id temporal</param>
        /// <param name="RistIdList">Id temporal</param>
        /// <returns>propertyPolicy</returns>
        public List<CompanyPropertyRisk> GetPropertiesByPolicyIdEndorsementIdRiskIdList(int policyId, int endorsementId, List<int> RiskIdList)
        {
            try
            {
                PropertyDAO propertyDAO = new PropertyDAO();
                return propertyDAO.GetPropertiesByPolicyIdEndorsementIdRiskIdList(policyId, endorsementId, RiskIdList);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorGetPropertiesByPolicy), ex);
            }
        }

        public List<CompanyPropertyRisk> GetCompanyPropertyByPrefixBranchDocumentNumberEndorsementType(int prefixId, int branchId, decimal documentNumber, EndorsementType endorsementType)
        {
            try
            {
                PropertyDAO propertyDAO = new PropertyDAO();
                return propertyDAO.GetCompanyPropertyByPrefixBranchDocumentNumberEndorsementType(prefixId, branchId, documentNumber, endorsementType);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.GetBaseException().Message, ex); throw;
            }
        }

        //public List<CompanyPropertyRisk> GetCompanyPropertyRiskByEndorsementId(int endorsementId)
        //{
        //    PropertyDAO propertyDAO = new PropertyDAO();
        //    return propertyDAO.GetCompanyPropertyRiskByEndorsementId(endorsementId);
        //}

        public List<CompanyPropertyRisk> GetCompanyPropertiesByPolicyId(int policyId)
        {
            try
            {
                PropertyDAO propertyDAO = new PropertyDAO();
                return propertyDAO.GetCompanyPropertiesByPolicyId(policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.GetBaseException().Message, ex); throw;
            }
        }

        public List<CompanyPropertyRisk> GetCompanyPropertiesByTemporalId(int temporalId)
        {
            try
            {
                PropertyDAO propertyDAO = new PropertyDAO();
                return propertyDAO.GetCompanyPropertiesByTemporalId(temporalId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.GetBaseException().Message, ex); throw;
            }
        }

        /// <summary>
        /// Gets the properties by policy identifier by risk identifier list.
        /// </summary>
        /// <param name="policyId">The policy identifier.</param>
        /// <param name="RiskIdList">The risk identifier list.</param>
        /// <returns></returns>
        public List<CompanyPropertyRisk> GetPropertiesByPolicyIdByRiskIdList(int policyId, List<int> RiskIdList, int riskId = 0)
        {
            try
            {
                PropertyDAO propertyDAO = new PropertyDAO();
                return propertyDAO.GetPropertiesByPolicyIdByRiskIdList(policyId, RiskIdList, riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.GetBaseException().Message, ex); throw;
            }
        }

        public CompanyPolicy CreateEndorsement(CompanyPolicy companyPolicy, List<CompanyPropertyRisk> companyPropertyRisks)
        {
            try
            {
                PropertyDAO propertyDAO = new PropertyDAO();
                return propertyDAO.CreateEndorsement(companyPolicy, companyPropertyRisks);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateEndorsement), ex);
            }

        }

        /// <summary>
        /// Polizas asociadas a individual
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public List<CompanyPropertyRisk> GetPropertiesByIndividualId(int individualId)
        {
            try
            {
                PropertyDAO propertyDAO = new PropertyDAO();
                return propertyDAO.GetPropertiesByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public void CreateRisk(CompanyPropertyRisk propertyRisk)
        {
            try
            {
                new PropertyDAO().CreateRisk(propertyRisk);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreatePropertyRisk), ex);
            }
        }

        /// <summary>
        /// Obtener Riesgo
        /// </summary>
        /// <param name="riskId">Id Riesgo</param>
        /// <returns>Riesgo</returns>
        public CompanyPropertyRisk GetCompanyPropertyRiskByRiskId(int riskId)
        {
            try
            {
                PropertyDAO propertyDAO = new PropertyDAO();
                return propertyDAO.GetCompanyPropertyRiskByRiskId(riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public CompanyRiskLocation GetCompanyPropertyRiskByRiskIdModuleType(int riskId, ModuleType moduleType)
        {
            try
            {
                PropertyDAO propertyDAO = new PropertyDAO();
                return propertyDAO.GetCompanyPropertyRiskByRiskIdModuleType(riskId, moduleType);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Obtiene las subactividades
        /// </summary>
        /// <param name="riskActivity"></param>
        /// <returns></returns>
        public List<CompanyRiskSubActivity> GetSubActivities(int riskActivity)
        {
            try
            {
                List<CompanyRiskSubActivity> riskSubActivities = new List<CompanyRiskSubActivity>();
                PropertyDAO propertyDAO = new PropertyDAO();
                return propertyDAO.GetRiskSubActivitiesByActivity(riskActivity);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene el modo de aseguramiento
        /// </summary>
        /// <returns></returns>
        public List<CompanyAssuranceMode> GetAssuranceMode()
        {
            List<CompanyAssuranceMode> riskSubActivities = new List<CompanyAssuranceMode>();
            PropertyDAO propertyDAO = new PropertyDAO();

            return propertyDAO.GetRiskAssuranceMode();

        }

        #region RiskLocation
        /// <summary>
        /// 
        /// </summary>
        public List<CompanyRiskLocation> GetRiskLocationsByEndorsementIdModuleType(int endorsementId, ModuleType moduleType)
        {
            try
            {
                PropertyDAO propertyDAO = new PropertyDAO();
                return propertyDAO.GetRiskLocationsByEndorsementIdModuleType(endorsementId, moduleType);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }

        public List<CompanyRiskLocation> GetCompanyRisksLocationByInsuredId(int insuredId)
        {
            try
            {
                PropertyDAO propertyDAO = new PropertyDAO();
                return propertyDAO.GetCompanyRisksLocationByInsuredId(insuredId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyRiskLocation> GetRisksLocationByAddress(string address)
        {
            try
            {
                PropertyDAO propertyDAO = new PropertyDAO();
                return propertyDAO.GetRisksLocationByAddress(address);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, ex.Message));
            }
        }
        #endregion

        #region emision
        /// <summary>
        /// 
        /// </summary>
        public CompanyPropertyRisk GetCompanyRiskByRiskId(int temporalId, int id)
        {

            CompanyPropertyRisk risk = GetCompanyPropertyRiskByRiskId(id);

            if (risk != null)
            {
                risk = GetRiskDescriptions(risk, temporalId);
                return risk;
            }
            else
            {
                throw new BusinessException(Errors.NoRiskWasFound);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private CompanyPropertyRisk GetRiskDescriptions(CompanyPropertyRisk risk, int temporalId)
        {
            CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
            switch (companyPolicy.Endorsement.EndorsementType)
            {
                case EndorsementType.Emission:
                    risk = GetDataEmission(risk, companyPolicy);
                    break;
                default:
                    break;
            }

            return risk;
        }

        /// <summary>
        /// 
        /// </summary>
        private CompanyPropertyRisk GetDataEmission(CompanyPropertyRisk risk, CompanyPolicy policy)
        {
            if (risk.Risk != null && risk.Risk.Beneficiaries == null)
            {
                risk.Risk.Beneficiaries = policy.DefaultBeneficiaries;
            }

            if (risk.Risk != null && risk.Risk.MainInsured?.IdentificationDocument == null)
            {
                var companyInsured = DelegateService.underwritingService.GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(risk.Risk.MainInsured.IndividualId.ToString(), InsuredSearchType.IndividualId, risk.Risk.MainInsured.CustomerType);
                if (companyInsured != null)
                {
                    risk.Risk.MainInsured = companyInsured;
                    risk.Risk.MainInsured.Name = risk.Risk.MainInsured.Surname + " " + (string.IsNullOrEmpty(risk.Risk.MainInsured.SecondSurname) ? "" : risk.Risk.MainInsured.SecondSurname + " ") + risk.Risk.MainInsured.Name;
                }

            }

            if (risk.Risk != null && risk.Risk.Beneficiaries?[0].IdentificationDocument == null)
            {
                ConcurrentBag<CompanyBeneficiary> beneficiaries = new ConcurrentBag<CompanyBeneficiary>();

                if (risk.Risk.Beneficiaries != null)
                {
                    TP.Parallel.For(0, risk.Risk.Beneficiaries.Count, itemRow =>
                     {
                         var beneficiary = DelegateService.underwritingService.GetBeneficiariesByDescription(risk.Risk.Beneficiaries[itemRow].IndividualId.ToString(), InsuredSearchType.IndividualId).FirstOrDefault();
                         if (beneficiary != null)
                         {
                             var imapper = ModelAssembler.CreateMapBeneficiary();
                             var benef = imapper.Map<Beneficiary, CompanyBeneficiary>(beneficiary);
                             beneficiaries.Add(benef);
                         }
                     });
                }

                risk.Risk.Beneficiaries = beneficiaries.ToList();
            }
            if (risk?.Risk?.RiskActivity != null)
            {
                var riskActivity = DelegateService.underwritingService.GetRiskActivityByActivityId(risk.Risk.RiskActivity.Id);
                var imapper = ModelAssembler.CreateMapRiskActivity();
                risk.Risk.RiskActivity = imapper.Map<RiskActivity, CompanyRiskActivity>(riskActivity);


            }
            if (risk.Risk.Coverages != null && risk.Risk.Coverages.Any())
            {
                risk.Risk.Coverages.AsParallel().ForAll(item =>
                {
                    if (item.CoverStatusName == null)
                    {
                        item.CoverStatusName = Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Original));
                    }
                });
            }
            return risk;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<CompanyInsuredObject> GetCompanyInsuredObjectsByTemporalIdRiskId(int temporalId, int riskId)
        {

            List<CompanyPropertyRisk> companyPropertyRisks = GetCompanyPropertiesByTemporalId(temporalId);
            if (companyPropertyRisks != null && companyPropertyRisks.Any())
            {
                CompanyPropertyRisk risk = companyPropertyRisks.FirstOrDefault(x => x.Risk.Id == riskId);
                if (risk != null)
                {
                    List<CompanyInsuredObject> insuredObjects = risk.Risk.Coverages.GroupBy(i => i.InsuredObject.Id,
                                       (key, group) => group.First().InsuredObject).ToList();
                    if (insuredObjects != null && insuredObjects.Any())
                    {
                        object objlock = new object();
                        insuredObjects.AsParallel().ForAll(obj =>
                            {
                                lock (objlock)
                                {
                                    insuredObjects.FirstOrDefault(u => u.Id == obj.Id).Amount = risk.Risk.Coverages.Where(u => u.InsuredObject.Id == obj.Id).Sum(u => u.LimitAmount);
                                    insuredObjects.FirstOrDefault(u => u.Id == obj.Id).Premium = risk.Risk.Coverages.Where(u => u.InsuredObject.Id == obj.Id).Sum(u => u.PremiumAmount);
                                }
                            });
                        insuredObjects = insuredObjects.Where(x => x.Description != null).OrderBy(u => u.Description).ToList();

                        return insuredObjects;
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
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public CompanyInsuredObject GetCompanyInsuredObjectByTemporalIdRiskIdInsuredObjectId(int temporalId, int riskId, int insuredObjectId)
        {
            try
            {
                List<CompanyPropertyRisk> companyPropertyRisks = GetCompanyPropertiesByTemporalId(temporalId);
                if (companyPropertyRisks != null && companyPropertyRisks.Any())
                {
                    CompanyPropertyRisk risk = companyPropertyRisks.FirstOrDefault(x => x.Risk.Id == riskId);
                    if (risk != null && risk.Risk != null && risk.Risk.Coverages != null && risk.Risk.Coverages.Any())
                    {
                        CompanyInsuredObject insuredObjects = risk.Risk.Coverages.GroupBy(i => i.InsuredObject.Id,
                                           (key, group) => group.First().InsuredObject).FirstOrDefault(x => x.Id == insuredObjectId);

                        return insuredObjects;
                    }
                    else
                    {
                        return risk.InsuredObjects.Where(x => x.Id == insuredObjectId).FirstOrDefault();
                    }
                }
                else
                {
                    throw new BusinessException(Errors.ErrorSearchRisk);
                }
            }
            catch
            {
                throw new BusinessException(Errors.ErrorSearchRisk);

            }

        }

        /// <summary>
        /// 
        /// </summary>
        public bool DeleteCompanyRisk(int temporalId, int riskId)
        {
            try
            {
                bool result = false;
                CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);

                if (companyPolicy != null)
                {
                    CompanyPropertyRisk propertyRisk = GetCompanyPropertyRiskByRiskId(riskId);
                    if (propertyRisk != null)
                    {
                        if (companyPolicy.Endorsement.EndorsementType == EndorsementType.Emission || companyPolicy.Endorsement.EndorsementType == EndorsementType.Renewal || propertyRisk.Risk.Status == RiskStatusType.Included || propertyRisk.Risk.Status == null || propertyRisk.Risk.Status == RiskStatusType.Original)
                        {
                            result = DelegateService.underwritingService.DeleteCompanyRisksByRiskId(riskId, false);
                        }
                        else
                        {
                            propertyRisk.Risk.Status = RiskStatusType.Excluded;
                            propertyRisk.Risk.Description = propertyRisk.FullAddress + " (" + EnumHelper.GetItemName<RiskStatusType>(RiskStatusType.Excluded) + ")";
                            propertyRisk.Risk.IsPersisted = true;
                            propertyRisk.Risk.Policy = companyPolicy;
                            propertyRisk = QuotateProperty(propertyRisk, false, false);
                            propertyRisk?.Risk?.Coverages.AsParallel().ForAll(x =>
                            {
                                if (Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(x?.CoverStatus.Value)) == null)
                                {

                                    x.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(x?.CoverStatus.Value);
                                }
                                else
                                {
                                    x.CoverStatusName = Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(x?.CoverStatus.Value));
                                }
                                x.CurrentFrom = companyPolicy.CurrentFrom;
                                x.CurrentTo = companyPolicy.CurrentTo;
                            });
                            CreatePropertyTemporal(propertyRisk, false);
                            result = true;
                        }

                        if (result)
                        {
                            return true;

                        }
                        else
                        {
                            throw new BusinessException(Errors.ErrorDeleteRisk);
                        }
                    }
                    else
                    {
                        throw new BusinessException(Errors.ErrorSearchRisk);
                    }
                }
                else
                {
                    throw new BusinessException(Errors.ErrorSearchRisk);

                }
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorSearchRisk);

            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<CompanyInsuredObject> GetCompanyInsuredObjectsByProductIdGroupCoverageId(CompanyPropertyRisk risk, Boolean allInsuredObject, Boolean isSelected = false)
        {

            if (risk == null || risk.Risk == null)
            {
                throw new Exception("Riesgo vacio");
            }
            List<CompanyInsuredObject> companyInsuredObjects = DelegateService.underwritingService.GetCompanyInsuredObjectsByProductIdGroupCoverageId(risk.Risk.Policy.Product.Id, risk.Risk.GroupCoverage.Id, risk.Risk.Policy.Prefix.Id);
            CompanyPropertyRisk companyPropertyRisk = new CompanyPropertyRisk();
            if (companyInsuredObjects != null && companyInsuredObjects.Any())
            {
                if (allInsuredObject)
                {
                    companyInsuredObjects = companyInsuredObjects.Where(x => x.IsSelected == true).ToList();
                }
                if (isSelected)
                {
                    if (risk.Risk.Id != 0)
                    {
                        companyPropertyRisk = GetCompanyPropertyRiskByRiskId(risk.Risk.Id);
                        companyPropertyRisk.Risk.GroupCoverage.Id = risk.Risk.GroupCoverage.Id;
                    }
                    else
                    {
                        companyPropertyRisk = risk;
                    }
                    companyInsuredObjects = Runrules(risk.Risk.GroupCoverage.Id, companyInsuredObjects, risk.Risk.Policy.Id, companyPropertyRisk);
                }
                if (companyInsuredObjects != null && companyInsuredObjects.Any())
                {
                    return companyInsuredObjects.OrderBy(x => x.Description).ToList();
                }
                else
                {
                    throw new BusinessException(Errors.NoInsuranceObjects);
                }
            }
            else
            {
                throw new BusinessException(Errors.NoInsuranceObjects);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public List<CompanyInsuredObject> Runrules(int groupCoverageId, List<CompanyInsuredObject> companyInsuredObjects, int temporalId, CompanyPropertyRisk companyPropertyRisk)
        {
            CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
            companyPropertyRisk.Risk.CoveredRiskType = companyPolicy.Product.CoveredRisk.CoveredRiskType;

            if (companyInsuredObjects != null && companyInsuredObjects.Any())
            {
                companyPropertyRisk.Risk.Coverages = new List<CompanyCoverage>();

                foreach (CompanyInsuredObject companyInsuredObject in companyInsuredObjects)
                {
                    companyPropertyRisk.Risk.Coverages.AddRange(DelegateService.underwritingService.GetCompanyCoveragesByInsuredObjectIdGroupCoverageIdProductId(companyInsuredObject.Id, companyPropertyRisk.Risk.GroupCoverage.Id, companyPolicy.Product.Id).Where(x => x.IsSelected == true));
                }

                if (companyPropertyRisk.Risk.Coverages.Count > 0)
                {
                    foreach (CompanyCoverage companyCoverage in companyPropertyRisk.Risk.Coverages)
                    {
                        companyCoverage.CurrentFrom = companyPolicy.CurrentFrom;
                        companyCoverage.CurrentTo = companyPolicy.CurrentTo;
                        companyCoverage.InsuredObject = companyInsuredObjects.FirstOrDefault(u => u.Id == companyCoverage.InsuredObject.Id);
                        companyCoverage.EndorsementType = companyPolicy.Endorsement.EndorsementType;
                        companyCoverage.CoverStatus = CoverageStatusType.Original;
                        companyCoverage.FirstRiskType = FirstRiskType.None;
                    }
                }

                companyPropertyRisk.Risk.IsPersisted = true;
                companyPropertyRisk.Risk.Policy = companyPolicy;
                companyPropertyRisk = QuotateProperty(companyPropertyRisk, true, true);

                //Objetos del seguros
                companyInsuredObjects = new List<CompanyInsuredObject>();
                companyInsuredObjects = companyPropertyRisk.Risk.Coverages.GroupBy(i => i.InsuredObject.Id, (key, group) => group.First().InsuredObject).ToList();

                return companyInsuredObjects.OrderBy(u => u.Description).ToList();
            }


            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<CompanyInsuredObject> GetCompanyRunrules(int groupCoverageId, List<CompanyInsuredObject> companyInsuredObjects, int temporalId, CompanyPropertyRisk companyPropertyRisk)
        {
            CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
            companyPropertyRisk.Risk.CoveredRiskType = companyPolicy.Product.CoveredRisk.CoveredRiskType;
            if (companyInsuredObjects != null && companyInsuredObjects.Any())
            {
                companyPropertyRisk.Risk.Coverages = new List<CompanyCoverage>();

                foreach (CompanyInsuredObject companyInsuredObject in companyInsuredObjects)
                {
                    companyPropertyRisk.Risk.Coverages.AddRange(DelegateService.underwritingService.GetCompanyCoveragesByInsuredObjectIdGroupCoverageIdProductId(companyInsuredObject.Id, companyPropertyRisk.Risk.GroupCoverage.Id, companyPolicy.Product.Id).Where(x => x.IsSelected == true));
                }

                if (companyPropertyRisk.Risk.Coverages.Count > 0)
                {
                    foreach (CompanyCoverage companyCoverage in companyPropertyRisk.Risk.Coverages)
                    {
                        companyCoverage.CurrentFrom = companyPolicy.CurrentFrom;
                        companyCoverage.CurrentTo = companyPolicy.CurrentTo;
                        companyCoverage.InsuredObject = companyInsuredObjects.FirstOrDefault(u => u.Id == companyCoverage.InsuredObject.Id);
                        companyCoverage.EndorsementType = companyPolicy.Endorsement.EndorsementType;
                        companyCoverage.CoverStatus = CoverageStatusType.Original;
                        companyCoverage.FirstRiskType = FirstRiskType.None;
                    }
                }

                companyPropertyRisk.Risk.IsPersisted = true;
                companyPropertyRisk.Risk.Policy = companyPolicy;
                companyPropertyRisk = QuotateProperty(companyPropertyRisk, true, true);

                //Objetos del seguros
                companyInsuredObjects = new List<CompanyInsuredObject>();
                companyInsuredObjects = companyPropertyRisk.Risk.Coverages.GroupBy(i => i.InsuredObject.Id, (key, group) => group.First().InsuredObject).ToList();

                return companyInsuredObjects.OrderBy(u => u.Description).ToList();
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        public CompanyPropertyRisk SaveCompanyRisk(int temporalId, CompanyPropertyRisk propertyRisk)
        {
            try
            {
                if (propertyRisk == null || propertyRisk.Risk == null || propertyRisk.InsuredObjects == null)
                {
                    throw new Exception("propertyRisk");
                }
                CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                if (companyPolicy == null)
                {
                    throw new Exception(Errors.ErrorPolicyNotFound);
                }

                propertyRisk.Risk.Policy = companyPolicy;
                propertyRisk.Risk.CoveredRiskType = companyPolicy.Product?.CoveredRisk?.CoveredRiskType;
                List<CompanyInsuredObject> insuredObjects = propertyRisk.InsuredObjects.GroupBy(i => i.Id, (key, group) => group.First()).ToList();
                if (propertyRisk.Risk?.Id != 0)
                {
                    switch (companyPolicy.Endorsement.EndorsementType.Value)
                    {
                        case EndorsementType.Emission:
                        case EndorsementType.Renewal:
                            propertyRisk = SetDataEmission(propertyRisk);
                            break;
                        case EndorsementType.Modification:
                            propertyRisk = SetDataModification(propertyRisk, insuredObjects, companyPolicy);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    List<CompanyPropertyRisk> companyPropertyRisks = GetCompanyPropertiesByTemporalId(temporalId);
                    if (companyPropertyRisks.Count < companyPolicy.Product.CoveredRisk.MaxRiskQuantity)
                    {
                        if (companyPolicy.Endorsement.EndorsementType == EndorsementType.Modification)
                        {
                            propertyRisk.Risk.Status = RiskStatusType.Included;
                        }
                        propertyRisk.Risk.Beneficiaries = new List<CompanyBeneficiary>();
                        if (companyPolicy.DefaultBeneficiaries != null && companyPolicy.DefaultBeneficiaries.Count > 0)
                        {
                            propertyRisk.Risk.Beneficiaries = companyPolicy.DefaultBeneficiaries;
                        }
                        else
                        {
                            if (companyPropertyRisks.Count > 0)
                            {
                                propertyRisk.Risk.Number = companyPropertyRisks.Where(x => x.Risk.Number != 0).Count() + 1;
                            }
                            else
                            {
                                propertyRisk.Risk.Number = 1;
                            }
                            ModelAssembler.CreateMapCompanyBeneficiary();
                            propertyRisk.Risk.Beneficiaries.Add(ModelAssembler.CreateBeneficiaryFromInsured(propertyRisk.Risk.MainInsured));
                        }
                    }
                    else
                    {
                        throw new BusinessException(Errors.SelectedProductNotAllowMoreRisks);
                    }
                }
                propertyRisk = CreatePropertyTemporal(propertyRisk, false);
                return propertyRisk;

            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message);
            }


        }

        /// <summary>
        /// 
        /// </summary>
        private CompanyPropertyRisk SetDataEmission(CompanyPropertyRisk property)
        {
            try
            {
                if (property != null)
                {
                    CompanyPropertyRisk propertyOld = GetCompanyPropertyRiskByRiskId(property.Risk.Id);
                    property.Risk.Coverages = propertyOld.Risk.Coverages;
                    if (property.Risk.Policy.Endorsement.EndorsementType.Value == EndorsementType.Renewal)
                    {
                        property.Risk.Coverages.ForEach(x => { x.CurrentFrom = property.Risk.Policy.CurrentFrom; x.CurrentTo = property.Risk.Policy.CurrentTo; });
                    }
                    property.Risk.Beneficiaries = propertyOld.Risk.Beneficiaries;
                    if (property.Risk.Beneficiaries.Count == 1)
                    {
                        property.Risk.Beneficiaries[0].Participation = 100;
                    }
                    //property.Risk.Text = propertyOld.Risk.Text;
                    //property.Risk.Clauses = propertyOld.Risk.Clauses;
                    property.BillingPeriodDepositPremium = propertyOld.BillingPeriodDepositPremium;
                    property.DeclarationPeriodCode = propertyOld.DeclarationPeriodCode;
                    property.AdjustPeriod = propertyOld.AdjustPeriod;
                    property.DeclarationPeriod = propertyOld.DeclarationPeriod;
                    property.Risk.Number = propertyOld.Risk.Number;
                    return property;
                }
                else
                {
                    throw new Exception("riesgo vacio");
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private CompanyPropertyRisk SetDataModification(CompanyPropertyRisk propertyRisk, List<CompanyInsuredObject> insuredObjects, CompanyPolicy companyPolicy)
        {
            try
            {
                var riskOld = GetCompanyPropertyRiskByRiskId(propertyRisk.Risk.Id);
                propertyRisk.Risk.RiskId = riskOld.Risk.RiskId;
                propertyRisk.Risk.Status = riskOld.Risk.Status;
                propertyRisk.Risk.Beneficiaries = riskOld.Risk.Beneficiaries;
                propertyRisk.Risk.Text = riskOld.Risk.Text;
                propertyRisk.Risk.Clauses = riskOld.Risk.Clauses;
                propertyRisk.ConstructionYear = riskOld.ConstructionYear;
                propertyRisk.ConstructionType = riskOld.ConstructionType;
                propertyRisk.FloorNumber = riskOld.FloorNumber;
                propertyRisk.RiskType = riskOld.RiskType;
                propertyRisk.RiskUse = riskOld.RiskUse;
                propertyRisk.Longitude = riskOld.Longitude;
                propertyRisk.Latitude = riskOld.Latitude;
                propertyRisk.RiskAge = riskOld.RiskAge;
                propertyRisk.PML = riskOld.PML;
                propertyRisk.Square = riskOld.Square;
                propertyRisk.Risk.SecondInsured = riskOld.Risk.SecondInsured;
                propertyRisk.Risk.OriginalStatus = riskOld.Risk.OriginalStatus;
                propertyRisk.Risk.Number = riskOld.Risk.Number;
                propertyRisk.InsuredObjects = riskOld.InsuredObjects;
                if (insuredObjects.Count > 0)
                {
                    if (propertyRisk.Risk.GroupCoverage.Id == riskOld.Risk.GroupCoverage.Id)
                    {
                        riskOld.Risk.Coverages.RemoveAll(u => !insuredObjects.Select(z => z.Id).Contains(u.InsuredObject.Id));
                        propertyRisk.Risk.Coverages = riskOld.Risk.Coverages;
                    }
                    else
                    {
                        propertyRisk.Risk.Coverages = GetCoveragesByPolicyInsuredObjectsGroupCoverageId(companyPolicy, insuredObjects, propertyRisk.Risk.GroupCoverage.Id);
                    }
                }
                if (propertyRisk.Risk.Status != RiskStatusType.Included && propertyRisk.Risk.Status != RiskStatusType.Excluded)
                {
                    propertyRisk.Risk.Status = RiskStatusType.Modified;
                }
                return propertyRisk;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private List<CompanyCoverage> GetCoveragesByPolicyInsuredObjectsGroupCoverageId(CompanyPolicy policy, List<CompanyInsuredObject> insuredObjects, int groupCoverageId)
        {
            try
            {
                List<CompanyCoverage> coverages = new List<CompanyCoverage>();
                if (insuredObjects != null)
                {
                    foreach (var item in insuredObjects)
                    {
                        coverages.AddRange(DelegateService.underwritingService.GetCompanyCoveragesByInsuredObjectIdGroupCoverageIdProductId(item.Id, groupCoverageId, policy.Product.Id)
                            .Where(x => x.IsSelected == true)
                            .OrderBy(x => x.MainCoverageId));
                    }
                }
                coverages.Where(x => x.AllyCoverageId != 0).Select(item =>
                {
                    item.MainCoverageId = null;
                    item.CurrentFrom = policy.CurrentFrom;
                    item.CurrentTo = policy.CurrentTo;
                    item.InsuredObject = insuredObjects.FirstOrDefault(u => u.Id == item.InsuredObject.Id);
                    item.Rate = 0;
                    if (policy.Endorsement.EndorsementType == EndorsementType.Emission || policy.Endorsement.EndorsementType == EndorsementType.Renewal)
                    {
                        item.CoverStatus = CoverageStatusType.Original;
                        item.EndorsementType = policy.Endorsement.EndorsementType;
                        item.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Original);
                    }
                    else
                    {
                        if (policy.Endorsement.EndorsementType == EndorsementType.Modification)
                        {
                            item.CoverStatus = CoverageStatusType.Included;
                            item.EndorsementType = policy.Endorsement.EndorsementType;
                            item.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Included);
                        }

                    }
                    return item;
                }).ToList();
                /*                if (coverages != null)
                                {
                                    foreach (var item in coverages)
                                    {
                                        item.CurrentFrom = policy.CurrentFrom;
                                        item.CurrentTo = policy.CurrentTo;
                                        item.InsuredObject = insuredObjects.FirstOrDefault(u => u.Id == item.InsuredObject.Id);
                                        item.Rate = 0;
                                        if (policy.Endorsement.EndorsementType == EndorsementType.Emission || policy.Endorsement.EndorsementType == EndorsementType.Renewal)
                                        {
                                            item.CoverStatus = CoverageStatusType.Original;
                                            item.EndorsementType = policy.Endorsement.EndorsementType;
                                            item.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Original);
                                        }
                                        else
                                        {
                                            if (policy.Endorsement.EndorsementType == EndorsementType.Modification)
                                            {
                                                item.CoverStatus = CoverageStatusType.Included;
                                                item.EndorsementType = policy.Endorsement.EndorsementType;
                                                item.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Included);
                                            }

                                        }

                                    }
                                }*/
                return coverages;
            }
            catch (Exception)
            {

                return new List<CompanyCoverage>();
            }

        }

        /// <summary>
        /// Obtención de objetos asegurados por ID de producto Cobertura del grupo
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="groupCoverageId"></param>
        /// <param name="prefixId"></param>
        /// <returns></returns>
        public List<CompanyInsuredObject> GetInsuredsObjectsByProductIdGroupCoverageId(int productId, int groupCoverageId, int prefixId)
        {
            try
            {
                List<CompanyInsuredObject> companyInsuredObjects = DelegateService.underwritingService.GetCompanyInsuredObjectsByProductIdGroupCoverageId(productId, groupCoverageId, prefixId);
                companyInsuredObjects.Where(x => x.IsSelected == true).OrderBy(x => x.Description).ToList();
                return companyInsuredObjects;
            }
            catch (Exception)
            {
                throw new Exception(Errors.ErrorConsultingInsuranceObjects);
            }
        }

        /// <summary>
        /// Obtener grupo de coberturas
        /// </summary>
        /// <param name="allInsuredObject"></param>
        /// <param name="companyInsuredObject"></param>
        /// <param name="isSelected"></param>
        /// <returns></returns>
        public List<CompanyInsuredObject> GetInsuredObjectsByProductIdGroupCoverageId(Boolean allInsuredObject, CompanyPropertyRisk companyPropertyRisk, Boolean isSelected = false)
        {
            try
            {
                CompanyPropertyRisk risk;
                List<CompanyInsuredObject> companyInsuredObjects = DelegateService.underwritingService.GetCompanyInsuredObjectsByProductIdGroupCoverageId(companyPropertyRisk.Risk.Policy.Product.Id, companyPropertyRisk.Risk.GroupCoverage.Id, companyPropertyRisk.Risk.Policy.Prefix.Id);
                if (allInsuredObject)
                {
                    companyInsuredObjects = companyInsuredObjects.Where(x => x.IsSelected == true).OrderBy(x => x.Id).ToList();
                }
                if (isSelected)
                {
                    if (companyPropertyRisk.Risk.Id != 0)
                    {
                        risk = GetCompanyPropertyRiskByRiskId(companyPropertyRisk.Risk.Id);
                        risk.Risk.GroupCoverage.Id = companyPropertyRisk.Risk.GroupCoverage.Id;
                    }
                    else
                    {
                        risk = companyPropertyRisk;
                    }
                    //companyInsuredObjects = Runrules(companyPropertyRisk.Risk.GroupCoverage.Id, companyInsuredObjects, companyPropertyRisk.Risk.Policy.Id, risk);
                }
                if (companyInsuredObjects == null || companyInsuredObjects?.Count == 0)
                {
                    throw new BusinessException(Errors.NoInsuranceObjects);
                }
                return companyInsuredObjects;

            }
            catch (Exception ex)
            {
                if (ex.GetType().Name == "BusinessException")
                {
                    throw new BusinessException(Errors.NoInsuranceObjects);
                }
                else
                {
                    throw new Exception(Errors.ErrorQueryCoverageGroups);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public InsuredObject DeleteInsuredObjectByRiskIdInsuredObjectId(int temporalId, int riskId, int insuredObjectId)
        {
            try
            {
                CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                companyPolicy.IsPersisted = true;

                CompanyPropertyRisk propertyRisk = GetCompanyPropertyRiskByRiskId(riskId);
                propertyRisk.Risk.IsPersisted = true;

                if (companyPolicy.Endorsement.EndorsementType == EndorsementType.Emission || companyPolicy.Endorsement.EndorsementType == EndorsementType.Renewal)
                {
                    propertyRisk.Risk.Coverages.RemoveAll(u => u.InsuredObject.Id == insuredObjectId);
                    CompanyInsuredObject companyInsuredObject = propertyRisk.InsuredObjects.Where(x => x.Id == insuredObjectId).FirstOrDefault();
                    if (companyInsuredObject.IsDeclarative)
                    {
                        propertyRisk.AdjustPeriod = null;
                        propertyRisk.DeclarationPeriod = null;
                        propertyRisk.BillingPeriodDepositPremium = 0;
                        propertyRisk.DeclarationPeriodCode = 0;
                    }
                    propertyRisk.InsuredObjects.Remove(companyInsuredObject);
                    propertyRisk.Risk.Policy = companyPolicy;
                    propertyRisk = QuotateProperty(propertyRisk, false, false);
                    propertyRisk = CreatePropertyTemporal(propertyRisk, false);

                    InsuredObject insuredObject = new InsuredObject
                    {
                        Id = insuredObjectId
                    };

                    return insuredObject;
                }
                else
                {
                    propertyRisk.Risk.Coverages.RemoveAll(u => u.InsuredObject.Id == insuredObjectId && u.CoverStatus == CoverageStatusType.Included);

                    foreach (CompanyCoverage item in propertyRisk.Risk.Coverages.Where(u => u.InsuredObject.Id == insuredObjectId))
                    {
                        item.CoverStatus = CoverageStatusType.Excluded;
                        item.IsVisible = false;
                        item.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(item.CoverStatus);
                    }

                    propertyRisk.Risk.Policy = companyPolicy;
                    propertyRisk = QuotateProperty(propertyRisk, false, false);
                    propertyRisk = CreatePropertyTemporal(propertyRisk, false);

                    InsuredObject insuredObject = new InsuredObject
                    {
                        Id = insuredObjectId,
                        Amount = propertyRisk.Risk.Coverages.Where(u => u.InsuredObject.Id == insuredObjectId).Sum(u => u.LimitAmount),
                        Premium = propertyRisk.Risk.Coverages.Where(u => u.InsuredObject.Id == insuredObjectId).Sum(u => u.PremiumAmount)
                    };
                    return insuredObject;
                }
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorRemovingObjectInsurance);//Errors.ErrorRemovingObjectInsurance
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Boolean ConvertProspectToInsured(int temporalId, int individualId, string documentNumber)
        {
            try
            {
                DelegateService.underwritingService.ConvertProspectToHolder(temporalId, individualId, documentNumber);
                List<CompanyPropertyRisk> companyPropertyRisks = GetCompanyPropertiesByTemporalId(temporalId);
                if (companyPropertyRisks != null && companyPropertyRisks.Count > 0)
                {
                    foreach (CompanyPropertyRisk propertyRisk in companyPropertyRisks)
                    {
                        CompanyRisk risk = DelegateService.underwritingService.ConvertProspectToInsured(propertyRisk.Risk, individualId, documentNumber);
                        propertyRisk.Risk.Beneficiaries = risk.Beneficiaries;
                        CreatePropertyTemporal(propertyRisk, false);
                    }
                }

                return true;
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorConvertingProspectIntoIndividual);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public CompanyText SaveTexts(int riskId, CompanyText textModel)
        {
            try
            {
                CompanyPropertyRisk risk = GetCompanyPropertyRiskByRiskId(riskId);
                if (risk.Risk.Id > 0)
                {


                    if (risk != null)
                    {
                        risk.Risk.Text = textModel;
                        CreatePropertyTemporal(risk, false);
                        return risk.Risk.Text;
                    }
                    else
                    {
                        throw new BusinessException(Errors.ErrorSaveText);//Errors.ErrorErrorSaveText
                    }
                }
                else
                {

                    throw new BusinessException(Errors.NoExistRisk);//Errors.ErrorNoExistRisk
                }

            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorSaveText);//Errors.ErrorErrorSaveText
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<CompanyClause> SaveClauses(int riskId, List<CompanyClause> clauses)
        {
            try
            {
                if (clauses != null)
                {
                    CompanyPropertyRisk risk = GetCompanyPropertyRiskByRiskId(riskId);
                    if (risk.Risk.Id > 0)
                    {
                        risk.Risk.Clauses = clauses;
                        CreatePropertyTemporal(risk, false);
                        return clauses;
                    }
                    else
                    {
                        throw new BusinessException(Errors.ErrorSaveClauses);

                    }
                }
                else
                {
                    throw new BusinessException(Errors.NoExistRisk);
                }

            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorSaveClauses);//Errors.ErrorSaveClauses
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<CompanyClause> SaveClausesByCoverageId(int riskId, int coverageId, List<CompanyClause> clauses)
        {
            try
            {
                if (clauses != null)
                {
                    CompanyPropertyRisk risk = GetCompanyPropertyRiskByRiskId(riskId);

                    if (risk.Risk.Id > 0)
                    {
                        CompanyCoverage coverageRisk = risk.Risk.Coverages.Where(x => x.Id == coverageId).FirstOrDefault();

                        if (coverageRisk.Clauses != null)
                        {
                            coverageRisk.Clauses.Clear();
                        }
                        coverageRisk.Clauses = clauses;

                        CreatePropertyTemporal(risk, false);
                        return clauses;
                    }
                    //if (risk != null)
                    //{
                    //    return new clauses;
                    //}
                    else
                    {
                        throw new BusinessException(Errors.ErrorSaveClauses);
                    }
                }
                else
                {
                    throw new BusinessException(Errors.NoExistCoverage);
                }

            }

            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorSaveClauses);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool SaveInsuredObject(int riskId, CompanyInsuredObject objectModel, int tempId, int groupCoverageId)
        {
            try
            {
                if (objectModel == null)
                {
                    throw new BusinessException(Errors.ErrorMinimumCoverage);
                }
                CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(tempId, false);
                CompanyPropertyRisk propertyRisk = GetCompanyPropertyRiskByRiskId(riskId);

                if (propertyRisk == null)
                {
                    return false;
                }

                List<CompanyCoverage> coverages = propertyRisk.Risk?.Coverages?.Where(u => u.InsuredObject.Id == objectModel.Id)?.ToList();

                if (coverages != null && coverages.Any())
                {
                    coverages.ForEach(u => u.InsuredObject.Amount = objectModel.Amount);

                    if (companyPolicy.Endorsement.EndorsementType == EndorsementType.Modification)
                    {
                        coverages.Where(x => (x.CoverStatus == CoverageStatusType.NotModified)).Select(item =>
                        {
                            item.CoverStatus = CoverageStatusType.Modified;
                            item.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Modified);
                            item.CurrentFrom = companyPolicy.CurrentFrom;
                            item.CurrentTo = companyPolicy.CurrentTo;
                            item.InsuredObject = objectModel;
                            item.EndorsementType = companyPolicy.Endorsement.EndorsementType;
                            return item;
                        });
                    }
                }
                else
                {
                    coverages = DelegateService.underwritingService.GetCompanyCoveragesByInsuredObjectIdGroupCoverageIdProductId(
                        objectModel.Id, propertyRisk.Risk.GroupCoverage.Id, companyPolicy.Product.Id);
                    coverages.RemoveAll(x => !x.IsSelected);


                    coverages.ForEachParallel(item =>
                    {
                        item.CurrentFrom = companyPolicy.CurrentFrom;
                        item.CurrentTo = companyPolicy.CurrentTo;
                        item.InsuredObject = objectModel;
                        item.DepositPremiumPercent = (decimal)objectModel.DepositPremiunPercent;
                        item.EndorsementType = companyPolicy.Endorsement.EndorsementType;

                        if (companyPolicy.Endorsement.EndorsementType == EndorsementType.Emission || companyPolicy.Endorsement.EndorsementType == EndorsementType.Renewal)
                        {
                            item.CoverStatus = CoverageStatusType.Original;
                            item.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Original);
                        }
                        else
                        {
                            if (companyPolicy.Endorsement.EndorsementType == EndorsementType.Modification)
                            {
                                item.CoverStatus = CoverageStatusType.Included;
                                item.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Included);
                            }
                        }
                    });

                    if (propertyRisk.Risk.Coverages == null)
                    {
                        propertyRisk.Risk.Coverages = new List<CompanyCoverage>();
                    }
                    propertyRisk.Risk.Coverages.AddRange(coverages);
                }

                propertyRisk.Risk.Policy = companyPolicy;
                propertyRisk.Risk.CoveredRiskType = companyPolicy.Product?.CoveredRisk?.CoveredRiskType;
                propertyRisk.Risk.Policy.IsPersisted = false;
                if (objectModel.IsDeclarative)
                {
                    propertyRisk.BillingPeriodDepositPremium = objectModel.BillingPeriodDepositPremium;
                    propertyRisk.DeclarationPeriodCode = Convert.ToInt32(objectModel.DeclarationPeriod);
                    propertyRisk.AdjustPeriod = new Core.Application.Location.PropertyServices.Models.AdjustPeriod
                    {
                        Id = objectModel.BillingPeriodDepositPremium
                    };
                    propertyRisk.DeclarationPeriod = new Core.Application.Location.PropertyServices.Models.DeclarationPeriod
                    {
                        Id = (int)objectModel.DeclarationPeriod
                    };
                }


                if (propertyRisk.InsuredObjects.Any(x => x.Id == objectModel.Id))
                {
                    propertyRisk.InsuredObjects.Where(x => x.Id == objectModel.Id).Select(x =>
                    {
                        x.Amount = objectModel.Amount;
                        x.DepositPremiunPercent = Convert.ToDecimal(objectModel.DepositPremiunPercent);
                        x.Rate = objectModel.Rate;
                        x.IsDeclarative = objectModel.IsDeclarative;
                        return x;
                    }).ToList();
                }
                else
                {
                    propertyRisk.InsuredObjects.Add(objectModel);
                }

                propertyRisk.Risk.Coverages.Where(x => (x.IsPrimary == false && x.SublimitPercentage == null
                    && (x.MainCoverageId == 0 || x.MainCoverageId == null) && (x.AllyCoverageId == 0 || x.AllyCoverageId == null))).Select(item =>
                {
                    item.PremiumAmount = 0;
                    return item;
                });
                CreatePropertyTemporal(propertyRisk, false);
                return true;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, "Error al Guardar Objeto Asegurado"), ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public CompanyPolicy UpdateRisks(int temporalId)
        {
            bool RiskInZero = false;
            try
            {
                CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                if (companyPolicy != null)
                {
                    companyPolicy.IsPersisted = true;
                    List<CompanyPropertyRisk> companyPropertyRisks = GetTemporalById(temporalId);
                    if (companyPropertyRisks != null && companyPropertyRisks.Any())
                    {
                        foreach (var riesgo in companyPropertyRisks)
                        {
                            if (riesgo.Risk.Premium < 1)
                            {
                                RiskInZero = true;
                            }
                            List<CompanyInsuredObject> objetos = riesgo.InsuredObjects.ToList();
                            if (objetos != null && objetos.Count() == 1 && objetos[0].IsDeclarative)
                            {
                                if (objetos[0].Premium < 1)
                                {
                                    RiskInZero = false;
                                }
                            }
                            if (RiskInZero)
                            {
                                throw new BusinessException(Errors.RiskInZero);
                            }
                        }
                        TP.Parallel.ForEach(companyPropertyRisks, companyLiabilityRisk =>
                       {
                           companyLiabilityRisk.Risk.Policy = companyPolicy;
                           companyLiabilityRisk.Risk.IsPersisted = true;
                           companyLiabilityRisk?.Risk.Coverages.AsParallel().ForAll(x =>
                           {
                               x.CurrentTo = companyPolicy.CurrentTo;
                               x.CurrentFrom = companyPolicy.CurrentFrom;
                           });
                           var companyLiabilityRiskData = QuotateProperty(companyLiabilityRisk, true, true);
                           CreatePropertyTemporal(companyLiabilityRiskData, false);
                       });
                        companyPolicy = DelegateService.underwritingService.UpdatePolicyComponents(companyPolicy.Id);
                        List<CompanyRiskInsured> risks = new List<CompanyRiskInsured>();
                        foreach (CompanyPropertyRisk item in companyPropertyRisks)
                        {
                            CompanyRiskInsured risk = new CompanyRiskInsured();
                            risk.Beneficiaries = new List<CompanyBeneficiary>();
                            foreach (var beneficiary in item.Risk.Beneficiaries)
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
                                    risk.Beneficiaries.Add(companyBeneficiary);
                                }
                                else
                                {
                                    risk.Beneficiaries.Add(beneficiary);
                                }
                            }

                            if (companyPolicy.TemporalType == TemporalType.Policy && item.Risk.MainInsured.CustomerType == CustomerType.Prospect)
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
                                risk.Insured = Insured;
                                risks.Add(risk);
                            }
                            else
                            {
                                risk.Insured = item.Risk.MainInsured;
                                risks.Add(risk);
                            }
                        }
                        companyPolicy.Summary.RisksInsured = risks;
                        return companyPolicy;
                    }
                    else
                    {
                        return companyPolicy;
                    }
                }
                else
                {
                    throw new Exception(Errors.ErrorTemporalNotFound);
                }
            }
            catch (Exception)
            {
                if (RiskInZero)
                {
                    throw new BusinessException(Errors.RiskInZero);
                }
                else
                {
                    throw new BusinessException(Errors.ErrorUpdatePolicy);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public CompanyPropertyRisk SaveAdditionalData(int riskId, CompanyPropertyRisk companyPropertyRisk)
        {
            try
            {
                CompanyPropertyRisk risk = GetCompanyPropertyRiskByRiskId(riskId);
                var riskData = CreateAdditionalData(risk, companyPropertyRisk);
                if (risk != null && risk.Risk != null && risk.Risk.Id > 0)
                {
                    risk = CreatePropertyTemporal(riskData, false);
                    if (risk != null && risk.Risk != null)
                    {
                        return risk;
                    }
                    else
                    {
                        throw new Exception(Errors.ErrorSaveAdditionalData);
                    }
                }
                else
                {
                    throw new Exception(Errors.NoExistRisk);
                }

            }
            catch (Exception)
            {
                throw new Exception(Errors.ErrorSaveAdditionalData);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public CompanyPropertyRisk CreateAdditionalData(CompanyPropertyRisk companyPropertyRisk, CompanyPropertyRisk companyPropertyRiskNew)
        {
            if (companyPropertyRiskNew.Risk?.SecondInsured != null && companyPropertyRiskNew.Risk?.SecondInsured.InsuredId != 0)
            {
                companyPropertyRisk.Risk.SecondInsured = companyPropertyRiskNew.Risk.SecondInsured;
            }

            companyPropertyRisk.ConstructionYear = companyPropertyRiskNew.ConstructionYear;
            companyPropertyRisk.ConstructionType = companyPropertyRiskNew.ConstructionType;
            companyPropertyRisk.FloorNumber = companyPropertyRiskNew.FloorNumber;
            companyPropertyRisk.RiskType = companyPropertyRiskNew.RiskType;
            companyPropertyRisk.RiskUse = companyPropertyRiskNew.RiskUse;
            companyPropertyRisk.Longitude = companyPropertyRiskNew.Longitude;
            companyPropertyRisk.Latitude = companyPropertyRiskNew.Latitude;
            companyPropertyRisk.PML = companyPropertyRiskNew.PML;
            companyPropertyRisk.Square = companyPropertyRiskNew.Square;
            companyPropertyRisk.RiskAge = companyPropertyRiskNew.RiskAge;
            companyPropertyRisk.ReinsuranceObservations = companyPropertyRiskNew.ReinsuranceObservations;
            companyPropertyRisk.PrincipalRisk = companyPropertyRiskNew.PrincipalRisk;

            return companyPropertyRisk;

        }

        /// <summary>
        /// 
        /// </summary>
        public List<CompanyPropertyRisk> GetTemporalById(int id)
        {
            CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(id, false);
            if (companyPolicy != null)
            {
                List<CompanyPropertyRisk> companyPropertyRisks = GetCompanyPropertiesByTemporalId(companyPolicy.Id);

                if (companyPropertyRisks != null && companyPropertyRisks.Any())
                {
                    TP.Parallel.For(0, companyPropertyRisks.Count, row =>
                    {
                        var companyPropertyRisk = companyPropertyRisks[row];
                        companyPropertyRisk.Risk.AmountInsured = companyPropertyRisk.Risk.Coverages?.Sum(x => x.LimitAmount) ?? 0;
                        companyPropertyRisk.Risk.Premium = companyPropertyRisk.Risk.Coverages?.Sum(x => x.PremiumAmount) ?? 0;
                    });
                }
                return companyPropertyRisks;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private bool ExistsRisk(int temporalId, int? riskId, string fullAddress)
        {
            bool exists = false;
            List<CompanyPropertyRisk> companyPropertyRisks = GetCompanyPropertiesByTemporalId(temporalId);

            if (companyPropertyRisks != null)
            {
                foreach (CompanyPropertyRisk risk in companyPropertyRisks)
                {
                    if (risk.FullAddress == fullAddress)
                    {
                        if (riskId.HasValue)
                        {
                            if (risk.Risk.Id != riskId.Value)
                            {
                                exists = true;
                            }
                        }
                        else
                        {
                            exists = true;
                        }
                    }
                }
            }
            return exists;
        }

        /// <summary>
        /// 
        /// </summary>
        public CompanyCoverage QuotationCoverageByRiskId(CompanyPropertyRisk companyPropertyRisk)
        {
            try
            {
                CompanyCoverage coverage = QuotateCoverage(companyPropertyRisk, companyPropertyRisk.Risk.Coverages.FirstOrDefault(), false, true);
                if (coverage.CoverStatus != null)
                {
                    coverage.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(coverage.CoverStatus);
                }
                return coverage;
            }
            catch (Exception)
            {
                throw new Exception("Errors.ErrorQuotationCoverage");
            }
        }

        /// <summary>
        /// Creates the company policy.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="temporalType">Type of the temporal.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="BusinessException"></exception>
        public CompanyPolicyResult CreateCompanyPolicy(int temporalId, int temporalType, bool clearPolicies)
        {
            try
            {
                PropertyDAO propertyDAO = new PropertyDAO();
                CompanyPolicyResult companyPolicyResult = new CompanyPolicyResult();
                companyPolicyResult.IsError = false;
                companyPolicyResult.Errors = new List<ErrorBase>();
                List<CompanyEndorsement> companyEndorsements = new List<CompanyEndorsement>();
                string message = string.Empty;
                CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                policy.Errors = new List<ErrorBase>();
                List<CompanyPropertyRisk> companyPropertyRisk = GetCompanyPropertiesByTemporalId(policy.Id);
                if (policy == null)
                {
                    companyPolicyResult.IsError = true;
                    companyPolicyResult.Errors.Add(new ErrorBase { StateData = false, Error = Errors.ErrorTemporalNotFound });
                }
                else
                {
                    if (temporalType != TempType.Quotation)
                    {
                        ValidateHolder(ref policy);
                    }
                    if (policy.Errors != null && !policy.Errors.Any() && policy.Product.CoveredRisk != null)
                    {
                        //List<CompanyPropertyRisk> companyPropertyRisk = GetCompanyPropertiesByTemporalId(policy.Id);

                        if (companyPropertyRisk != null && companyPropertyRisk.Any())
                        {
                            if (clearPolicies)
                            {
                                policy.InfringementPolicies.Clear();
                                companyPropertyRisk.ForEach(x => x.Risk.InfringementPolicies.Clear());
                            }

                            policy = CreateEndorsement(policy, companyPropertyRisk);
                            //se agrega la validación para el caso en que tenga un evento de autorización
                            if (policy?.InfringementPolicies?.Count == 0)
                            {
                                //if (policy.Endorsement.PolicyId != 0 && policy.Endorsement.Id != 0)

                                DelegateService.underwritingService.SaveTextLarge(policy.Endorsement.PolicyId, policy.Endorsement.Id);
                            }
                        }
                        else
                        {
                            throw new ArgumentException(Errors.NoExistRisk);
                        }

                        if (temporalType != TempType.Quotation)
                        {
                            if (policy.InfringementPolicies.Any())
                            {
                                companyPolicyResult.TemporalId = policy.Id;
                                companyPolicyResult.InfringementPolicies = policy.InfringementPolicies;
                            }
                            else
                            {
                                if (policy.Endorsement.EndorsementType == EndorsementType.Emission)
                                {
                                    if (policy.PaymentPlan.PremiumFinance == null)
                                    {
                                        companyPolicyResult.Message = string.Format(Errors.PolicyNumber, policy.DocumentNumber);
                                    }
                                    else
                                    {
                                        companyPolicyResult.Message = string.Format(Errors.PolicyNumber, policy.DocumentNumber +
                                       " \n\r " + Errors.labelPay + policy.PaymentPlan.PremiumFinance.PromissoryNoteNumCode +
                                       " \n\r " + Errors.labelUser + policy.UserId.ToString());
                                    }
                                }
                                else
                                {
                                    string additionalFinancing = "";

                                    if (policy.PaymentPlan.PremiumFinance != null)
                                    {
                                        companyPolicyResult.PromissoryNoteNumCode = policy.PaymentPlan.PremiumFinance.PromissoryNoteNumCode;
                                        additionalFinancing = string.Format(Errors.PromissoryNote, policy.PaymentPlan.PremiumFinance.PromissoryNoteNumCode, policy.User.UserId);
                                    }

                                    companyPolicyResult.Message = string.Format(Errors.EndorsementNumber, policy.DocumentNumber, policy.Endorsement.Number, policy.Endorsement.Id, additionalFinancing);
                                }

                                companyPolicyResult.DocumentNumber = policy.DocumentNumber;
                                companyPolicyResult.EndorsementId = policy.Endorsement.Id;
                                companyPolicyResult.EndorsementNumber = policy.Endorsement.Number;
                            }
                        }
                        else
                        {
                            companyPolicyResult.Message = string.Format(Errors.QuotationNumber, policy.Endorsement.QuotationId.ToString());
                            companyPolicyResult.DocumentNumber = Convert.ToDecimal(policy.Endorsement.QuotationId);
                        }
                    }
                    else
                    {
                        companyPolicyResult.IsError = true;
                        companyPolicyResult.Errors.Add(new ErrorBase { StateData = false, Error = string.Join(" - ", policy.Errors) });
                    }

                }
                List<CompanyEndorsementDetail> companyEndorsementDetails = new List<CompanyEndorsementDetail>();
                foreach (var item in companyPropertyRisk)
                {
                    var companyinsuredobject = item.InsuredObjects.Where(x => x.IsDeclarative == true).FirstOrDefault();
                    if (companyinsuredobject != null)
                    {
                        if (companyinsuredobject.IsDeclarative && temporalType == TempType.Policy)
                        {
                            propertyDAO.SaveCompanyEndorsementPeriod(ModelAssembler.CreateCompanyEndorsementPeriod(policy, companyPropertyRisk.FirstOrDefault(), companyPolicyResult.DocumentNumber));
                        }

                        else if (temporalType == TempType.Endorsement)
                        {
                            if (policy.Endorsement.EndorsementType == EndorsementType.Renewal || policy.Endorsement.EndorsementType == EndorsementType.EffectiveExtension)
                            {
                                propertyDAO.SaveCompanyEndorsementPeriod(ModelAssembler.CreateCompanyEndorsementPeriod(policy, companyPropertyRisk.FirstOrDefault(), companyPolicyResult.DocumentNumber));
                            }
                            else
                            {
                                CompanyEndorsementPeriod endorsementPeriod = propertyDAO.GetEndorsementPeriodByPolicyId(companyPolicyResult.DocumentNumber);
                                companyEndorsementDetails = propertyDAO.SaveEndorsementDetailS(ModelAssembler.CreateCompanyEndorsementDetails(companyEndorsements, companyPropertyRisk, companyPolicyResult.DocumentNumber, endorsementPeriod));
                            }

                        }
                    }

                }

                return companyPolicyResult;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.GetBaseException().Message);
            }
        }

        public List<CompanyEndorsementDetail> CreateCompanyEndorsementDetails(List<CompanyEndorsement> companyEndorsements, List<CompanyPropertyRisk> companyPropertyRisks, decimal documentNumber, CompanyEndorsementPeriod endorsementPeriod)
        {
            List<CompanyEndorsementDetail> endorsementDetails = new List<CompanyEndorsementDetail>();
            foreach (CompanyEndorsement item in companyEndorsements)
            {
                CompanyPropertyRisk propertyRisk = companyPropertyRisks.Where(x => x.Risk.RiskId == item.RiskId).FirstOrDefault();
                if (propertyRisk == null)
                {
                    propertyRisk = companyPropertyRisks.Where(x => x.Risk.Number == item.RiskId).FirstOrDefault();
                }
                CompanyInsuredObject insured = propertyRisk.InsuredObjects.Where(x => x.Id == item.InsuredObjectId).FirstOrDefault();
                item.Number = propertyRisk.Risk.Number;
                endorsementDetails.Add(CreateCompanyEndorsementDetail(propertyRisk, item, insured, endorsementPeriod, documentNumber));
            }
            return endorsementDetails;
        }
        public CompanyEndorsementDetail CreateCompanyEndorsementDetail(CompanyPropertyRisk propertyRisk, CompanyEndorsement item, CompanyInsuredObject insured, CompanyEndorsementPeriod endorsementPeriod, decimal documentNumber)
        {
            switch (item.EndorsementType)
            {
                case EndorsementType.AdjustmentEndorsement:
                    CompanyEndorsementDetail endorsementDetail = new CompanyEndorsementDetail();
                    List<CompanyEndorsementDetail> detailsList = GetEndorsementDetailsListByPolicyId(documentNumber, endorsementPeriod.Version);
                    decimal endorsementSum = (decimal)detailsList.Where(x => x.PolicyId == endorsementPeriod.PolicyId && x.RiskNum == item.Number && x.InsuredObjectId == insured.Id && x.Version == endorsementPeriod.Version && x.EndorsementType == (int)EndorsementType.DeclarationEndorsement).Sum(n => n.DeclarationValue);
                    if (insured.DepositPremiunPercent > 0)
                    {
                        endorsementDetail.PremiumAmount = (endorsementSum * insured.Rate) - ((insured.Amount * insured.DepositPremiunPercent) * insured.Rate);
                    }
                    else
                    {
                        endorsementDetail.PremiumAmount = 0;
                    }
                    endorsementDetail.DeclarationValue = 0; // item.DeclaredValue;
                    endorsementDetail.RiskNum = item.Number;
                    endorsementDetail.EndorsementType = (int)item.EndorsementType;
                    endorsementDetail.PolicyId = documentNumber;
                    endorsementDetail.InsuredObjectId = (int)item.InsuredObjectId;
                    endorsementDetail.EndorsementDate = DateTime.Now;
                    endorsementDetail.Version = endorsementPeriod.Version;
                    return endorsementDetail;
                    break;
                case EndorsementType.DeclarationEndorsement:

                    return new CompanyEndorsementDetail()
                    {
                        DeclarationValue = item.DeclaredValue,
                        RiskNum = item.Number,//(int)item.RiskId,
                        EndorsementType = (int)item.EndorsementType,
                        PolicyId = documentNumber,
                        InsuredObjectId = (int)item.InsuredObjectId,
                        //PremiumAmmount = (insuredObject.DepositPremiunPercent != 0) ? (item.DeclaredValue * (insuredObject.DepositPremiunPercent/100) * (insuredObject.Rate/100)) : (insuredObject.BillingPeriodDepositPremium * (insuredObject.Rate/100)),
                        PremiumAmount = (item.DeclaredValue * (insured.Rate / 100)),
                        EndorsementDate = DateTime.Now,
                        Version = endorsementPeriod.Version
                    };
                    break;
            }
            return new CompanyEndorsementDetail();
        }

        

        /// <summary>
        /// Validates the holder.
        /// </summary>
        /// <param name="policy">The policy.</param>
        public void ValidateHolder(ref CompanyPolicy policy)
        {
            if (policy.Holder != null)
            {
                if (policy.Holder.CustomerType == CustomerType.Prospect)
                {
                    policy.Errors.Add(new ErrorBase { StateData = false, Error = Errors.ErrorHolderNoInsuredRole });
                }
                else
                {
                    List<Holder> holders = DelegateService.underwritingService.GetHoldersByDescriptionInsuredSearchTypeCustomerType(policy.Holder.IndividualId.ToString(), InsuredSearchType.IndividualId, policy.Holder.CustomerType);

                    if (holders != null && holders.Count == 1)
                    {
                        if (holders[0].InsuredId == 0)
                        {
                            policy.Errors.Add(new ErrorBase { StateData = false, Error = Errors.ErrorPolicyholderWithoutRol });
                        }
                        else if (holders[0]?.DeclinedDate > DateTime.MinValue)
                        {
                            policy.Errors.Add(new ErrorBase { StateData = false, Error = Errors.ErrorPolicyholderDisabled });
                        }
                    }
                    else
                    {
                        policy.Errors.Add(new ErrorBase { StateData = false, Error = Errors.ErrorConsultPolicyholder });
                    }

                    if (policy.Holder.PaymentMethod != null)
                    {
                        if (policy.Holder.PaymentMethod.Id == 0)
                        {
                            policy.Errors.Add(new ErrorBase { StateData = false, Error = Errors.ErrorPolicyholderDefaultPaymentPlan });
                        }
                    }

                    //Validación asegurado principal como prospecto
                    switch (policy.Product.CoveredRisk.CoveredRiskType)
                    {
                        case CoveredRiskType.Location:
                            List<CompanyPropertyRisk> properties = GetCompanyPropertiesByTemporalId(policy.Id);
                            if (properties != null && properties.Any())
                            {
                                var result = properties.Select(x => x.Risk).Where(z => z.MainInsured?.CustomerType == CustomerType.Prospect).Count();
                                if (result > 0)
                                {
                                    policy.Errors.Add(new ErrorBase { StateData = false, Error = Errors.ErrorInsuredNoInsuredRole });
                                }
                            }
                            break;
                    }
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public CompanyCoverage GetCoverageToAddByRiskId(int tempId, int riskId, int coverageId, int insuredObjectId)
        {

            CompanyPolicy policy;
            CompanyPropertyRisk risk;
            CompanyInsuredObject insuredObjects;
            List<CompanyCoverage> coverages = new List<CompanyCoverage>();
            CompanyCoverage coverage;
            try
            {
                policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(tempId, false);
                if (policy != null)
                {
                    risk = GetCompanyPropertyRiskByRiskId(riskId);
                    if (risk != null)
                    {
                        insuredObjects = risk.Risk.Coverages.GroupBy(i => i.InsuredObject.Id, (key, group) => group.First().InsuredObject).FirstOrDefault(x => x.Id == insuredObjectId);
                        coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(policy.Product.Id, risk.Risk.GroupCoverage.Id, policy.Prefix.Id);
                        coverages = coverages.Where(c => c.InsuredObject.Id == insuredObjectId).ToList();
                        coverage = coverages.FirstOrDefault(x => x.Id == coverageId);
                        if (coverage.EndorsementType == EndorsementType.Modification)
                        {
                            coverage.CoverStatus = CoverageStatusType.Included;
                        }
                        coverage.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(coverage.CoverStatus.Value);
                        if (coverage.RuleSetId.GetValueOrDefault() > 0)
                        {
                            risk.Risk.Coverages = new List<CompanyCoverage> { coverage };
                            List<CompanyPropertyRisk> CompanyPropertyRisks = new List<CompanyPropertyRisk> { risk };
                            coverage = RunRulesCoverage(risk, coverage, coverage.RuleSetId.Value);
                        }
                        return coverage;
                    }
                    else
                    {
                        throw new Exception(Errors.NoExistRisk);
                    }
                }
                else
                {
                    throw new Exception(Errors.ErrorGetPropertiesByPolicy);
                }

            }
            catch (Exception)
            {

                throw new Exception(Errors.ErrorCoverages);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<CompanyCoverage> GetAllyCoverageByCoverage(int tempId, int riskId, int groupCoverageId, CompanyCoverage coverage)
        {
            List<CompanyCoverage> coverages = new List<CompanyCoverage>();
            List<CompanyCoverage> allyCoverages = new List<CompanyCoverage>();
            CompanyPolicy policy;
            try
            {
                policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(tempId, false);
                if (policy != null)
                {
                    coverages = DelegateService.underwritingService.GetAllyCompanyCoveragesByCoverageIdProductIdGroupCoverageId(coverage.Id, policy.Product.Id, groupCoverageId);
                    if (coverages != null && coverages.Any())
                    {
                        CompanyPropertyRisk propertyRisk = new CompanyPropertyRisk
                        {
                            Risk = new CompanyRisk
                            {
                                Id = riskId

                            }
                        };
                        foreach (CompanyCoverage item in coverages)
                        {
                            if (policy.Endorsement.EndorsementType == EndorsementType.Emission || policy.Endorsement.EndorsementType == EndorsementType.Renewal)
                            {
                                item.CoverStatus = CoverageStatusType.Original;
                                item.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(item.CoverStatus.Value);
                            }
                            else
                            {
                                if (item.Id == 0)
                                {
                                    item.CoverStatus = CoverageStatusType.Included;
                                    item.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(item.CoverStatus.Value);
                                }
                                else
                                {
                                    item.CoverStatus = CoverageStatusType.Modified;
                                    item.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(item.CoverStatus.Value);
                                }
                            }

                            item.SubLimitAmount = (coverage.SubLimitAmount * (item.SublimitPercentage ?? 0)) / 100;
                            propertyRisk.Risk.Coverages = new List<CompanyCoverage> { coverage };
                            List<CompanyPropertyRisk> CompanyPropertyRisks = new List<CompanyPropertyRisk> { propertyRisk };
                            propertyRisk.Risk.Policy = policy;
                            allyCoverages.Add(QuotateCoverage(propertyRisk, coverage, true, true));
                        }
                    }
                    return allyCoverages;
                }
                else
                {
                    throw new Exception(Errors.ErrorGetPropertiesByPolicy);
                }

            }
            catch (Exception)
            {
                throw new Exception(Errors.ErrorCoverages);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public CompanyCoverage ExcludeCoverage(int temporalId, int riskId, int riskCoverageId, string description)
        {
            CompanyCoverage coverage;
            try
            {
                coverage = DelegateService.underwritingService.GetCompanyCoverageByRiskCoverageId(riskCoverageId);
                if (coverage != null)
                {
                    CompanyPolicy propertyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                    CompanyPropertyRisk risk = GetCompanyPropertyRiskByRiskId(riskId);
                    coverage.Description = description;
                    coverage.MainCoverageId = 0;
                    coverage.SubLineBusiness = risk.Risk.Coverages.First(x => x.RiskCoverageId == riskCoverageId).SubLineBusiness;
                    coverage.CoverStatus = CoverageStatusType.Excluded;
                    coverage.IsVisible = false;
                    coverage.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Excluded);
                    coverage.EndorsementType = propertyPolicy.Endorsement.EndorsementType;
                    coverage.CurrentFrom = propertyPolicy.CurrentFrom;

                    risk.Risk.Coverages = new List<CompanyCoverage>();
                    risk.Risk.Coverages.Add(coverage);
                    List<CompanyPropertyRisk> CompanyPropertyRisks = new List<CompanyPropertyRisk> { risk };
                    coverage = QuotateCoverage(risk, coverage, false, false);

                }
            }
            catch (Exception)
            {
                throw new Exception(Errors.ErrorCoverages);
            }

            return coverage;
        }

        /// <summary>
        /// 
        /// </summary>
        public CompanyPropertyRisk GetPremium(CompanyPropertyRisk riskModel)
        {

            CompanyPolicy policy;
            try
            {
                policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(riskModel.Risk.Policy.Id, false);
                if (policy != null)
                {
                    policy.IsPersisted = true;
                    CompanyPropertyRisk risk = GetCompanyPropertyRiskByRiskId(riskModel.Risk.Id);
                    riskModel = QuotateProperty(riskModel, false, true);
                    return riskModel;
                }
                else
                {
                    throw new Exception(Errors.ErrorGetPropertiesByPolicy);
                }
            }
            catch (Exception)
            {

                throw new Exception("Error obteniendo prima");
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public List<CompanyCoverage> GetCoveragesByCoveragesAdd(int productId, int coverageGroupId, int prefixId, string coveragesAdd, int insuredObjectId)
        {
            List<CompanyCoverage> coverages = new List<CompanyCoverage>();

            if (!string.IsNullOrEmpty(coveragesAdd))
            {
                string[] idCoverages = coveragesAdd.Split(',');

                try
                {
                    coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(productId, coverageGroupId, prefixId);
                    coverages = coverages.Where(c => c.InsuredObject.Id == insuredObjectId).ToList();
                    coverages = coverages.Where(c => (!idCoverages.Any(x => Convert.ToInt32(x) == c.Id)) && c.IsVisible == true && c.InsuredObject.Id == insuredObjectId).ToList();
                    coverages.RemoveAll(u => u.MainCoverageId != 0);
                    if (coverages != null)
                    {
                        coverages.OrderBy(x => x.Description);
                        return coverages;
                    }
                    else
                    {
                        throw new Exception(Errors.ErrorCoverages);
                    }
                }
                catch (Exception)
                {

                    throw new Exception(Errors.ErrorCoverages);
                }

            }
            else
            {
                throw new Exception(Errors.ErrorCoverages);
            }


        }

        /// <summary>
        /// 
        /// </summary>
        public List<CompanyCoverage> GetCoveragesByInsuredObjectId(int riskId, int insuredObjectId)
        {
            CompanyPropertyRisk companyPropertyRisk = GetCompanyPropertyRiskByRiskId(riskId);

            if (companyPropertyRisk != null && companyPropertyRisk.Risk != null)
            {
                List<CompanyCoverage> coverages = new List<CompanyCoverage>();
                if (companyPropertyRisk.Risk.Coverages != null && companyPropertyRisk.Risk.Coverages.Any())
                {
                    if (insuredObjectId != 0)
                    {
                        coverages = companyPropertyRisk.Risk.Coverages.Where(x => x.InsuredObject.Id == insuredObjectId)?.ToList();
                    }
                    else
                    {
                        coverages = companyPropertyRisk.Risk.Coverages;
                    }
                    if (coverages != null && coverages.Any())
                    {
                        return coverages;
                    }
                    else
                    {
                        throw new Exception(Errors.ErrorCoverages);
                    }
                }
                else
                {
                    return coverages;
                }

            }
            else
            {
                throw new Exception(Errors.ErrorCoverages);
            }
        }

        #endregion emision

        /// <summary>
        /// 
        /// </summary>
        public CompanyCoverage GetCoverageByCoverageId(int coverageId, int riskId, int temporalId, int groupCoverageId)
        {
            try
            {
                CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);


                CompanyPropertyRisk propertyRisk = GetCompanyPropertyRiskByRiskId(riskId);

                CompanyCoverage coverage = DelegateService.underwritingService.GetCompanyCoverageByCoverageIdProductIdGroupCoverageId(coverageId, policy.Product.Id, groupCoverageId);

                coverage.EndorsementType = policy.Endorsement.EndorsementType;
                coverage.CurrentFrom = policy.CurrentFrom;
                coverage.CurrentTo = policy.CurrentTo;
                coverage.Days = Convert.ToInt32((coverage.CurrentTo.Value - coverage.CurrentFrom).TotalDays);
                coverage.FirstRiskType = FirstRiskType.None;

                if (coverage.EndorsementType == EndorsementType.Modification)
                {
                    coverage.CoverStatus = CoverageStatusType.Included;
                }

                coverage.CoverStatusName = EnumHelper.GetItemName<CoverageStatusType>(coverage.CoverStatus);
                if (coverage.RuleSetId.HasValue)
                {
                    coverage = RunRulesCoverage(propertyRisk, coverage, coverage.RuleSetId.Value);
                }

                return coverage;
            }
            catch (Exception)
            {
                throw new Exception(Errors.ErrorCoverages);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<CompanyDeclarationPeriod> GetCompanyDeclarationPeriods()
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(PARAMEN.DeclarationPeriod.Properties.IsEnabled, typeof(PARAMEN.DeclarationPeriod).Name);
                filter.Equal();
                filter.Constant(Status.Enabled);

                BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(PARAMEN.DeclarationPeriod), filter.GetPredicate());
                return ModelAssembler.CreateDeclarationPeriodTypes(businessCollection);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, ""/*Errors.ErrorDeclarationPeriodTypes*/), ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<CompanyAdjustPeriod> GetCompayAdjustPeriods()
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(PARAMEN.BillingPeriod.Properties.IsEnabled, typeof(PARAMEN.BillingPeriod).Name);
                filter.Equal();
                filter.Constant(Status.Enabled);

                BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(PARAMEN.BillingPeriod), filter.GetPredicate());

                return ModelAssembler.CreateAdjustPeriods(businessCollection);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, ""/*Errors.ErrorGetAdjustPeriods*/), ex);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public List<EndorsementDTO> GetEndorsementByEndorsementTypeIdPolicyId(int endorsementTypeId, int policyId)
        {
            PropertyRiskBusiness propertyRiskBusiness = new PropertyRiskBusiness();
            return DTOAssembler.CreateEndorsements(propertyRiskBusiness.GetCompanyEndorsementByEndorsementTypeIdPolicyId(endorsementTypeId, policyId));
        }

        /// <summary>
        /// 
        /// </summary>
        public EndorsementDTO GetTemporalEndorsementByPolicyId(int policyId)
        {
            try
            {
                PropertyRiskBusiness propertyRiskBusiness = new PropertyRiskBusiness();
                return propertyRiskBusiness.GetTemporalEndorsementByPolicyId(policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetPropertyRiskByPolicyId), ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public EndorsementDTO GetNextAdjustmentEndorsementByPolicyId(int policyId)
        {
            try
            {
                PropertyRiskBusiness propertyRiskBusiness = new PropertyRiskBusiness();
                return DTOAssembler.CreateEndorsementDTO(propertyRiskBusiness.GetNextAdjustmentEndorsementByPolicyId(policyId));
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetNextAdjustmentEndorsementByPolicyId), ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public EndorsementDTO GetNextDeclarationEndorsementByPolicyId(int policyId)
        {
            try
            {
                PropertyRiskBusiness propertyRiskBusiness = new PropertyRiskBusiness();
                return DTOAssembler.CreateEndorsementDTO(propertyRiskBusiness.GetNextDeclarationEndorsementByPolicyId(policyId));
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetNextDeclarationEndorsementByPolicyId), ex);
            }
        }

        /// <summary>
        /// Indica si es posible hacer un endoso de declaración
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        public bool CanMakeDeclarationEndorsement(int policyId)
        {
            try
            {
                PropertyRiskBusiness propertyRiskBusiness = new PropertyRiskBusiness();
                return propertyRiskBusiness.CanMakeDeclarationEndorsement(policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }


        }

        /// <summary>
        /// Valida si en la poliza actual se pueden realizar endosos de ajuste 
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        public bool CanMakeAdjustmentEndorsement(int policyId)
        {
            try
            {
                PropertyRiskBusiness propertyRiskBusiness = new PropertyRiskBusiness();
                return propertyRiskBusiness.CanMakeAdjustmentEndorsement(policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public bool CanMakeEndorsement(int policyId, out Dictionary<string, object> validateEndorsement)
        {
            try
            {
                PropertyRiskBusiness propertyRiskBusiness = new PropertyRiskBusiness();
                return propertyRiskBusiness.CanMakeEndorsement(policyId, out validateEndorsement);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Realiza el llamado los objetos del seguro asociados al riesgo
        /// </summary>
        /// <param name="liabilityRisk"></param>
        /// <returns>CompanyLiabilityRisk</returns>
        public List<CompanyCoverage> GetTemporalCoveragesByRiskIdInsuredObjectId(int riskId, int insuredObjectId)
        {
            try
            {
                PropertyRiskBusiness property = new PropertyRiskBusiness();
                return property.GetTemporalCoveragesByRiskIdInsuredObjectId(riskId, insuredObjectId);
            }
            catch (Exception)
            {
                throw new Exception(Errors.ErrorGetInsuredObject);

            }
        }

        public List<InsuredObjectDTO> GetInsuredObjectsByRiskId(int riskId)
        {
            try
            {
                PropertyRiskBusiness property = new PropertyRiskBusiness();
                return property.GetInsuredObjectsByRiskId(riskId);
            }
            catch (Exception)
            {
                throw new Exception(Errors.ErrorGetInsuredObject);

            }
        }


        /// <summary>
        /// Realiza el calculo de las coberturas asoociadas al objeto del seguro
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        public List<CompanyCoverage> GetCalculateCoveragesByInsuredObjectId(int riskId, CompanyInsuredObject insuredObject,
            decimal depositPremiumPercent, decimal rate, DateTime currentFrom, DateTime currentTo, decimal insuredLimit, bool runRulesPre, bool runRulesPost)
        {
            CompanyPropertyRisk companyPropertyRisk = GetCompanyPropertyRiskByRiskId(riskId);
            List<CompanyCoverage> coverages = GetTemporalCoveragesByRiskIdInsuredObjectId(riskId, insuredObject.Id);
            companyPropertyRisk.Risk.Coverages = coverages.Where(x => x.IsSelected).Select(item =>
            {
                item.InsuredObject = insuredObject;
                /*if (!item.IsPrimary)
                    item.Rate = 0;*/
                item.CurrentFrom = currentFrom;
                item.CurrentTo = currentTo;
                item.DeclaredAmount = insuredLimit;
                item.LimitAmount = insuredLimit;
                item.SubLimitAmount = insuredLimit;
                item.MaxLiabilityAmount = insuredLimit;
                item.LimitOccurrenceAmount = insuredLimit;
                item.LimitClaimantAmount = insuredLimit;
                item.DepositPremiumPercent = depositPremiumPercent;
                item.Rate = rate;
                item = QuotateCoverage(companyPropertyRisk, item, true, true);
                if (item.IsPrimary == false && item.SublimitPercentage == null && (item.MainCoverageId == 0 || item.MainCoverageId == null) && (item.AllyCoverageId == 0 || item.AllyCoverageId == null))
                {
                    item.PremiumAmount = 0;
                }
                return item;
            }).ToList();

            PropertyRiskBusiness propertyRiskBusiness = new PropertyRiskBusiness();
            companyPropertyRisk = QuotateProperty(companyPropertyRisk, runRulesPre, runRulesPost);
            return companyPropertyRisk.Risk.Coverages.Where(x => x.IsSelected).ToList();
        }
        #region Persistencia Emision (Ajuste/Declaracion)
        private DatatableToList DatatableToList = new DatatableToList();
        private bool tryAgain = true;
        public CompanyEndorsementPeriod SaveCompanyEndorsementPeriod(CompanyEndorsementPeriod companyEndorsementPeriod)
        {
            try
            {
                PropertyRiskBusiness propertyRiskBusiness = new PropertyRiskBusiness();
                NameValue[] parameters = new NameValue[8];
                CompanyEndorsementPeriod resultEndorsementPeriod = new CompanyEndorsementPeriod();
                decimal monthsVigency = propertyRiskBusiness.GetMonthsByVigency(companyEndorsementPeriod.CurrentFrom, companyEndorsementPeriod.CurrentTo);
                companyEndorsementPeriod.DeclarationPeriod = propertyRiskBusiness.GetMothsByDeclarationPeriod(companyEndorsementPeriod.DeclarationPeriod);
                companyEndorsementPeriod.AdjustPeriod = propertyRiskBusiness.GetMothsByAdjustmentPeriod(companyEndorsementPeriod.AdjustPeriod);
                companyEndorsementPeriod.TotalAdjustment = (int)Math.Floor(monthsVigency / companyEndorsementPeriod.AdjustPeriod);
                companyEndorsementPeriod.TotalDeclarations = (int)Math.Ceiling(monthsVigency / companyEndorsementPeriod.DeclarationPeriod);
                if (monthsVigency < 12 && companyEndorsementPeriod.TotalAdjustment == 0)
                {
                    companyEndorsementPeriod.TotalAdjustment = 1;
                }
                //int rowcount = 0;
                parameters[0] = new NameValue("@CURRENT_FROM", companyEndorsementPeriod.CurrentFrom);
                parameters[1] = new NameValue("@CURRENT_TO", companyEndorsementPeriod.CurrentTo);
                parameters[2] = new NameValue("@ADJUST_PERIOD", companyEndorsementPeriod.AdjustPeriod);
                parameters[3] = new NameValue("@DECLARATION_PERIOD", companyEndorsementPeriod.DeclarationPeriod);
                parameters[4] = new NameValue("@POLICY_ID", companyEndorsementPeriod.PolicyId);
                parameters[5] = new NameValue("@TOTAL_DECLARATIONS", companyEndorsementPeriod.TotalDeclarations);
                parameters[6] = new NameValue("@TOTAL_ADJUSTMENT", companyEndorsementPeriod.TotalAdjustment);
                parameters[7] = new NameValue("@VERSION", companyEndorsementPeriod.Version);
                DataSet result;
                using (DynamicDataAccess pdb = new DynamicDataAccess())
                {
                    result = pdb.ExecuteSPDataSet("ISS.SAVE_ENDORSEMENT_COUNT_PERIOD", parameters);
                }
                if (result != null)
                {
                    return resultEndorsementPeriod;
                    //resultEndorsementPeriod = DatatableToList.ConvertTo<CompanyEndorsementPeriod>(result.Tables[0]).FirstOrDefault();
                }
                return new CompanyEndorsementPeriod();
            }
            catch (Exception ex)
            {

                EventLog.WriteEntry("SaveCompanyEndorsementPeriod", String.Format("Error Persistiendo Datos de la poliza en ISS.ENDORSEMENT_COUNT_PERIOD DETALLES {0} : {1}", ex.Message, JsonConvert.SerializeObject(companyEndorsementPeriod)));
                if (tryAgain)
                {
                    tryAgain = false;
                    SaveCompanyEndorsementPeriod(companyEndorsementPeriod);

                }
                throw new Exception(String.Format("Error Persistiendo Datos de la poliza en ISS.ENDORSEMENT_COUNT_PERIOD DETALLES {0}", ex.Message));
            }
        }
        public List<CompanyEndorsementDetail> SaveEndorsementDetailS(List<CompanyEndorsementDetail> endorsementDetails)
        {

            List<CompanyEndorsementDetail> ValidEndtrsementDetails = new List<CompanyEndorsementDetail>();
            try
            {
                foreach (CompanyEndorsementDetail item in endorsementDetails)
                {
                    ValidEndtrsementDetails.Add(SaveEndorsementDetail(item));
                }

                return ValidEndtrsementDetails;
            }
            catch (Exception ex)
            {

                throw;
            }

        }


        public CompanyEndorsementDetail SaveEndorsementDetail(CompanyEndorsementDetail model)
        {

            CompanyEndorsementDetail resultEndorsementPeriod = new CompanyEndorsementDetail();
            NameValue[] parameters = new NameValue[12];
            parameters[0] = new NameValue("@POLICY_ID", model.PolicyId);
            parameters[1] = new NameValue("@ENDORSEMENT_TYPE", model.EndorsementType);
            parameters[2] = new NameValue("@RISK_NUM", model.RiskNum);
            parameters[3] = new NameValue("@INSURED_OBJECT_ID", model.InsuredObjectId);
            parameters[4] = new NameValue("@VERSION", model.Version);
            parameters[5] = new NameValue("@ENDORSEMENT_DATE", model.EndorsementDate);
            if (model.DeclarationValue != null)
            {
                parameters[6] = new NameValue("@DECLARATION_VALUE", model.DeclarationValue);
            }
            else
            {
                parameters[6] = new NameValue("@DECLARATION_VALUE", DBNull.Value, DbType.Decimal);
            }
            parameters[7] = new NameValue("@PREMIUM_AMOUNT", model.PremiumAmount);
            if (model.DeductibleAmmount != null)
            {
                parameters[8] = new NameValue("@DEDUCTIBLE_AMOUNT", model.DeductibleAmmount);
            }
            else
            {
                parameters[8] = new NameValue("@DEDUCTIBLE_AMOUNT", DBNull.Value, DbType.Int32);
            }
            if (model.Taxes != null)
            {
                parameters[9] = new NameValue("@TAXES", model.Taxes);
            }
            else
            {
                parameters[9] = new NameValue("@TAXES", DBNull.Value, DbType.Int32);
            }
            if (model.Surchanges != null)
            {
                parameters[10] = new NameValue("@SURCHANGE", model.Surchanges);
            }
            else
            {
                parameters[10] = new NameValue("@SURCHANGE", DBNull.Value, DbType.Int32);
            }
            if (model.Expenses != null)
            {
                parameters[11] = new NameValue("@EXPENSES", model.Expenses);
            }
            else
            {
                parameters[11] = new NameValue("@EXPENSES", DBNull.Value, DbType.Int32);
            }



            DataSet result;
            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                result = pdb.ExecuteSPDataSet("ISS.SAVE_ENDORSEMENT_COUNT_DETAIL", parameters);
            }
            if (result != null)
            {
                //resultEndorsementPeriod;
                return resultEndorsementPeriod = DatatableToList.ConvertTo<CompanyEndorsementDetail>(result.Tables[0]).FirstOrDefault();
            }

            return new CompanyEndorsementDetail();
        }

        public CompanyEndorsementPeriod GetEndorsementPeriodByPolicyId(decimal policyId)
        {
            CompanyEndorsementPeriod endorsementPeriod = new CompanyEndorsementPeriod();
            NameValue[] parameters = new NameValue[1];
            parameters[0] = new NameValue("@POLICY_ID", policyId);
            DataSet result;
            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                result = pdb.ExecuteSPDataSet("ISS.GET_ENDORSEMENT_COUNT_PERIOD", parameters);
            }
            if (result != null)
            {

                endorsementPeriod = DatatableToList.ConvertTo<CompanyEndorsementPeriod>(result.Tables[0]).FirstOrDefault();
            }
            return endorsementPeriod;
        }
        public List<CompanyEndorsementDetail> GetEndorsementDetailsListByPolicyId(decimal policyId, decimal version)
        {
            try
            {
                List<CompanyEndorsementDetail> endorsementPeriod = new List<CompanyEndorsementDetail>();
                NameValue[] parameters = new NameValue[2];
                parameters[0] = new NameValue("@POLICY_ID", policyId);
                parameters[1] = new NameValue("@VERSION", version);
                DataSet result;
                using (DynamicDataAccess pdb = new DynamicDataAccess())
                {
                    result = pdb.ExecuteSPDataSet("ISS.GET_ENDORSEMENT_COUNT_DETAIL", parameters);
                }
                if (result != null)
                {
                    endorsementPeriod = DatatableToList.ConvertTo<CompanyEndorsementDetail>(result.Tables[0]);
                }
                return endorsementPeriod;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool CanMakeEndorsementByRiskByInsuredObjectId(decimal policyId, decimal riskId, decimal insuredObjectId, EndorsementType endorsementType)
        {
            try
            {
                CompanyEndorsementPeriod period = GetEndorsementPeriodByPolicyId(policyId);
                List<CompanyEndorsementDetail> detailsList = GetEndorsementDetailsListByPolicyId(policyId, period.Version);
                int endorsementCount = detailsList.Where(x => x.PolicyId == policyId && x.RiskNum == riskId && x.InsuredObjectId == insuredObjectId && x.Version == period.Version && x.EndorsementType == (int)endorsementType).Count();
                switch (endorsementType)
                {
                    case EndorsementType.DeclarationEndorsement:
                        return (endorsementCount < period.TotalDeclarations) ? true : false;
                    case EndorsementType.AdjustmentEndorsement:
                        return (endorsementCount < period.TotalAdjustment) ? true : false;
                    default:
                        return false;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }


        #endregion

    }

}









