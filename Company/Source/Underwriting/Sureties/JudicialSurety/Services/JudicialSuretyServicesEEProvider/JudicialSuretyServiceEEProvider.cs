using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using JudicialSuretyServicesEEProvider;
using Sistran.Company.Application.Sureties.JudicialSuretyServices.EEProvider.Assemblers;
using Sistran.Company.Application.Sureties.JudicialSuretyServices.EEProvider.Business;
using Sistran.Company.Application.Sureties.JudicialSuretyServices.EEProvider.DAOs;
using Sistran.Company.Application.Sureties.JudicialSuretyServices.EEProvider.Resources;
using Sistran.Company.Application.Sureties.JudicialSuretyServices.Models;
using Sistran.Company.Application.Sureties.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.Sureties.JudicialSuretyServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Services.UtilitiesServices.Enums;
using JSEEPROVIDER = Sistran.Core.Application.Sureties.JudicialSuretyServices.EEProvider;
using TP = Sistran.Core.Application.Utilities.Utility;
using UNMO = Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Company.Application.Sureties.JudicialSuretyServices.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class JudicialSuretyServiceEEProvider : JSEEPROVIDER.JudicialSuretyServiceEEProviderCore, IJudicialSuretyService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="endorsementId"></param>
        /// <returns></returns>
        //public List<CompanyJudgement> GetJudicialSuretyPolicyByPolicyIdEndorsementId(int policyId, int endorsementId)
        //{
        //    try
        //    {
        //        JudicialSuretyDAO judicialSuretyDAO = new JudicialSuretyDAO();
        //        return judicialSuretyDAO.GetJudicialSuretyPolicyByPolicyIdEndorsementId(policyId, endorsementId);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new BusinessException(ex.Message, ex);
        //    }
        //}

        public List<CompanyJudgement> GetCompanyJudicialSuretyByPolicyId(int policyId)
        {
            try
            {
                JudicialSuretyDAO judicialSuretyDao = new JudicialSuretyDAO();
                return judicialSuretyDao.GetCompanyJudicialSuretyByPolicyId(policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyJudgement CreateJudgementTemporal(CompanyJudgement judicialSurety, bool isMassive)
        {
            try
            {
                JudicialSuretyDAO judicialSuretyDAO = new JudicialSuretyDAO();
                return judicialSuretyDAO.CreateJudgementTemporal(judicialSurety, isMassive);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        /// <summary>
        /// Tarifar Vehiculo
        /// </summary>
        /// <param name="CompanyJudgement">Vehiculo</param>
        /// <param name="runRulesPre">Ejecutar Reglas Pre?</param>
        /// <param name="runRulesPost">Ejecutar Reglas Post?</param>
        /// <returns>Vehiculo</returns>
        public CompanyJudgement QuotateCompanyJudgement(CompanyJudgement companyJudgement, bool runRulesPre, bool runRulesPost)
        {
            try
            {
                JudgementBusiness judgementBusiness = new JudgementBusiness();
                return judgementBusiness.QuotateJudgement(companyJudgement, runRulesPre, runRulesPost);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Tarifar Vehiculos
        /// </summary>
        /// <param name="CompanyJudgements">Vehiculos</param>
        /// <param name="runRulesPre">Ejecutar Reglas Pre?</param>
        /// <param name="runRulesPost">Ejecutar Reglas Post?</param>
        /// <returns>Vehiculos</returns>
        public List<CompanyJudgement> QuotateJudgements(CompanyPolicy companyPolicy, List<CompanyJudgement> companyJudgements, bool runRulesPre, bool runRulesPost)
        {
            try
            {
                JudgementBusiness judgementBusiness = new JudgementBusiness();
                return judgementBusiness.QuotateJudgements(companyPolicy, companyJudgements, runRulesPre, runRulesPost);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Tarifar Cobertura
        /// </summary>
        /// <param name="CompanyJudgement">Vehiculo</param>
        /// <param name="coverage">Cobertura</param>
        /// <param name="runRulesPre">Ejecutar Reglas Pre?</param>
        /// <param name="runRulesPost">Ejecutar Reglas Post?</param>
        /// <returns>Cobertura</returns>
        public CompanyCoverage QuotateCoverage(CompanyJudgement companyJudgement, CompanyCoverage coverage, bool runRulesPre, bool runRulesPost)
        {
            try
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();

                if (runRulesPost)
                {
                    coverage = coverageBusiness.Quotate(companyJudgement, coverage, runRulesPre, !runRulesPost);
                }
                return coverageBusiness.Quotate(companyJudgement, coverage, runRulesPre, runRulesPost);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyJudgement RunRulesRisk(CompanyJudgement companyJudgement, int ruleId)
        {
            try
            {
                JudgementBusiness judgementBusiness = new JudgementBusiness();
                return judgementBusiness.RunRulesRisk(companyJudgement, ruleId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Riesgos
        /// </summary>
        /// <param name="temporalId">Id Temporal</param>
        /// <returns>Riesgos</returns>
        public List<CompanyJudgement> GetCompanyJudgementsByTemporalId(int temporalId)
        {
            try
            {
                JudicialSuretyDAO judgementDAO = new JudicialSuretyDAO();
                return judgementDAO.GetCompanyJudgementsByTemporalId(temporalId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Riesgos
        /// </summary>
        /// <param name="endorsementId">Id Endoso</param>
        /// <returns>Riesgos</returns>
        public List<CompanyJudgement> GetCompanyJudgementsByEndorsementId(int endorsementId)
        {
            try
            {
                JudicialSuretyDAO judgementDAO = new JudicialSuretyDAO();
                return judgementDAO.GetCompanyJudgementsByEndorsementId(endorsementId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Riesgo
        /// </summary>
        /// <param name="riskId">Id Riesgo</param>
        /// <returns>Riesgo</returns>
        public CompanyJudgement GetCompanyJudgementByRiskId(int riskId)
        {
            try
            {
                JudicialSuretyDAO DAO = new JudicialSuretyDAO();
                return DAO.GetCompanyJudgementByRiskId(riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyPolicy CreateEndorsement(CompanyPolicy companyPolicy, List<CompanyJudgement> companyJudgement)
        {
            try
            {
                JudicialSuretyDAO judicialSuretyDAO = new JudicialSuretyDAO();
                return judicialSuretyDAO.CreateEndorsement(companyPolicy, companyJudgement);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error creando endoso", ex);
            }
        }

        public List<CompanyJudgement> GetRisksByTemporalId(int temporalId)
        {
            try
            {
                List<CompanyJudgement> companyJudgements = GetCompanyJudgementsByTemporalId(temporalId);
                object obj = new object();
                if (companyJudgements != null)
                {
                    var ListArticle = GetCompanyArticles();
                    List<CompanyJudgement> risks = new List<CompanyJudgement>();
                    TP.Parallel.For(0, companyJudgements.Count, judgementRow =>
                    {
                        var CompanyJudgement = companyJudgements[judgementRow];
                        if (CompanyJudgement != null)
                        {
                            CompanyJudgement.Risk.Description = ListArticle.Where(x => x.Id == companyJudgements[judgementRow].Article.Id).FirstOrDefault().SmallDescription;
                            lock (obj)
                            {
                                risks.Add(CompanyJudgement);
                            }
                        }
                    });

                    return risks;
                }
                else
                {
                    return companyJudgements;
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorSearchRisk, ex);
            }
        }

        public List<CompanyArticle> GetCompanyArticles()
        {
            try
            {
                var imapper = ModelAssembler.CreateMapCompanyArticle();
                List<CompanyArticle> ListArticle = imapper.Map<List<Article>, List<CompanyArticle>>(DelegateService.judicialSuretyServiceCore.GetArticles());
                return ListArticle;

            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorGetBranchs, ex);
            }
        }
        public List<CompanyProductArticle> GetCompanyProductArticles()
        {
            try
            {
                var imapper = ModelAssembler.CreateMapCompanyArticle();
                List<CompanyProductArticle> ListProductArticles = imapper.Map<List<ProductArticle>, List<CompanyProductArticle>>(DelegateService.judicialSuretyServiceCore.GetProductArticles());
                return ListProductArticles;

            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorGetBranchs, ex);
            }
        }

        public List<CompanyProductArticle> GetCompanyProductArticlesByDescription(string smallDescription)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapCompanyArticle();
                List<CompanyProductArticle> ListProductArticles = imapper.Map<List<ProductArticle>, List<CompanyProductArticle>>(DelegateService.judicialSuretyServiceCore.GetProductArticlesByDescription(smallDescription));
                return ListProductArticles;

            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorGetBranchs, ex);
            }
        }

        public CompanyJudgement GetRiskById(EndorsementType endorsementType, int temporalId, int id)
        {
            try
            {
                CompanyJudgement risk = GetCompanyJudgementByRiskId(id);
                if (risk != null)
                {
                    return risk = GetRiskDescriptions(endorsementType, risk);
                }
                else
                {
                    return risk;
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorSearchRisk, ex);
            }
        }

        public CompanyJudgement GetRiskDescriptions(EndorsementType endorsementType, CompanyJudgement judgement)
        {
            switch (endorsementType)
            {
                case EndorsementType.Emission:
                    judgement = GetDataEmission(judgement);
                    break;
                default:
                    break;
            }

            return judgement;
        }

        public CompanyJudgement GetDataEmission(CompanyJudgement risk)
        {
            if (risk.Risk.MainInsured.IdentificationDocument == null)
            {
                ModelAssembler.CreatetMapCompanyInsured();
                risk.Risk.MainInsured = DelegateService.underwritingService.GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(risk.Risk.MainInsured.IndividualId.ToString(), InsuredSearchType.IndividualId, risk.Risk.MainInsured.CustomerType);
                risk.Risk.MainInsured = DelegateService.underwritingService.GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(risk.Risk.MainInsured.IndividualId.ToString(), InsuredSearchType.IndividualId, risk.Risk.MainInsured.CustomerType);
                risk.Risk.MainInsured.Name = risk.Risk.MainInsured.Surname + " " + (string.IsNullOrEmpty(risk.Risk.MainInsured.SecondSurname) ? "" : risk.Risk.MainInsured.SecondSurname + " ") + risk.Risk.MainInsured.Name;
            }

            if (risk.Risk.Beneficiaries == null && risk.Risk.Beneficiaries[0].IdentificationDocument == null)
            {
                List<Beneficiary> beneficiaries = new List<Beneficiary>();

                foreach (CompanyBeneficiary item in risk.Risk.Beneficiaries)
                {
                    //beneficiaries.Add(DelegateService.underwritingService.GetBeneficiariesByDescription(item.IndividualId.ToString(), InsuredSearchType.IndividualId).FirstOrDefault());
                    UNMO.Beneficiary beneficiary = new UNMO.Beneficiary();
                    beneficiary = DelegateService.underwritingService.GetBeneficiariesByDescription(item.IndividualId.ToString(), InsuredSearchType.IndividualId).FirstOrDefault();
                    item.IdentificationDocument = beneficiary.IdentificationDocument;
                    item.Name = beneficiary.Name;
                }
                // risk.Beneficiaries = beneficiaries;
            }

            foreach (CompanyCoverage item in risk.Risk.Coverages)
            {
                if (item.CoverStatusName == null)
                {
                    item.CoverStatusName = Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Original));
                }
            }

            return risk;
        }

        ///// <summary>
        ///// FUncion para obtener el listado de coberturas por Id
        ///// </summary>
        ///// <param name="policyId"></param>
        ///// <param name="groupCoverageId"></param>
        ///// <returns></returns>
        public List<CompanyCoverage> GetCoveragesByProductIdGroupCoverageId(int policyId, int groupCoverageId)
        {
            try
            {
                CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(policyId, false);

                List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(policy.Product.Id, groupCoverageId, policy.Prefix.Id);

                coverages.ForEach(x => x.EndorsementType = policy.Endorsement.EndorsementType);
                coverages.ForEach(x => x.CurrentFrom = Convert.ToDateTime(policy.CurrentFrom));
                coverages.ForEach(x => x.CurrentTo = Convert.ToDateTime(policy.CurrentTo));
                if (policy.Endorsement.EndorsementType == EndorsementType.Modification)
                {
                    coverages.ForEach(x => x.CoverStatusName = Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Included)));
                    coverages.ForEach(x => x.CoverStatus = CoverageStatusType.Included);
                }
                else
                {
                    coverages.ForEach(x => x.CoverStatusName = Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Original)));
                }

                coverages = coverages.Where(x => x.IsSelected == true).ToList();

                return coverages;
            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorSearchCoverages, ex);
            }
        }

        public CompanyCoverage GetCoverageByCoverageId(int coverageId, int groupCoverageId, int policyId)
        {
            try
            {
                CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(policyId, false);
                CompanyCoverage coverage = DelegateService.underwritingService.GetCompanyCoverageByCoverageIdProductIdGroupCoverageId(coverageId, policy.Product.Id, groupCoverageId);

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
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorSearchCoverage, ex);
            }
        }

        /// <summary>
        /// Funcion para guardar el riesgo del Ramo Judicial
        /// </summary>
        /// <param name="CompanyJudgement"></param>
        /// <param name="riskModel"></param>        
        /// <returns></returns>
        public CompanyJudgement SaveCompanyRisk(CompanyJudgement companyJudgement, int temporalId)
        {
            try
            {
                CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                if (policy != null)
                {
                    List<CompanyJudgement> companyJudgements = GetCompanyJudgementsByTemporalId(temporalId);
                    bool existRisk = ExistsRisk(companyJudgement, companyJudgements);
                    if (!existRisk)
                    {
                        if (companyJudgement != null && companyJudgement.Risk != null)
                        {
                            companyJudgement.Risk.Beneficiaries = new List<CompanyBeneficiary>();

                            companyJudgement.Risk.CoveredRiskType = policy.Product.CoveredRisk.CoveredRiskType;
                            companyJudgement.Risk.Policy = policy;
                            if (companyJudgement.Risk.MainInsured != null)
                            {
                                companyJudgement.Risk.Description = companyJudgement.Risk.MainInsured.Name;

                            }
                            if (policy.Endorsement.EndorsementType == EndorsementType.Modification)
                            {
                                companyJudgement.Risk.Status = RiskStatusType.Included;
                            }

                            if (companyJudgement.Risk?.Id == 0)
                            {
                                if (companyJudgements.Count < policy.Product.CoveredRisk.MaxRiskQuantity)
                                {
                                    if (policy.DefaultBeneficiaries != null && policy.DefaultBeneficiaries.Count > 0)
                                    {
                                        companyJudgement.Risk.Beneficiaries = policy.DefaultBeneficiaries;
                                    }
                                    else
                                    {
                                        companyJudgement.Risk.Beneficiaries.Add(ModelAssembler.CreateBeneficiaryFromInsured(companyJudgement.Risk.MainInsured));
                                    }
                                }
                                else
                                {
                                    throw new BusinessException(Errors.ProductNotAddingMoreRisks);
                                }
                            }
                            else
                            {
                                if (policy.Endorsement.EndorsementType != null)
                                {
                                    switch (policy.Endorsement.EndorsementType.Value)
                                    {
                                        case EndorsementType.Emission:
                                            companyJudgement = SetDataEmission(companyJudgement);
                                            break;
                                        case EndorsementType.Renewal:
                                            companyJudgement = SetDataEmission(companyJudgement);
                                            break;
                                        case EndorsementType.Modification:
                                            companyJudgement = SetDataModification(companyJudgement);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                else
                                {
                                    throw new BusinessException(Errors.ErrorEndorsementTypeEmpty);
                                }

                            }
                            companyJudgement = CreateJudgementTemporal(companyJudgement, false);
                            return companyJudgement;
                        }
                        else
                        {
                            throw new BusinessException(Errors.ErrorSaveJudgement);
                        }
                    }
                    else
                    {
                        throw new BusinessException(Errors.ExistRisk);
                    }
                }
                else
                {
                    throw new BusinessException(Errors.ErrorTemporalNotFound);
                }

            }
            catch (Exception ex)
            {
                if (ex.GetType().Name == "BusinessException")
                {
                    throw new BusinessException(ex.Message);
                }
                else
                {
                    throw new Exception(Errors.ErrorSaveJudgement);
                }
            }

        }

        /// <summary>
        /// Sets the data emission.
        /// </summary>
        /// <param name="companyJudgement">The surety.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        private CompanyJudgement SetDataEmission(CompanyJudgement companyJudgement)
        {
            try
            {
                CompanyJudgement companyJudgementOld = GetCompanyJudgementByRiskId(companyJudgement.Risk.Id);

                companyJudgement.Risk.Description = companyJudgementOld.Risk.Description;
                companyJudgement.Risk.Beneficiaries = companyJudgementOld.Risk.Beneficiaries;
                companyJudgement.Risk.Text = companyJudgementOld.Risk.Text;
                companyJudgement.Risk.Clauses = companyJudgementOld.Risk.Clauses;
                companyJudgement.Guarantees = companyJudgementOld.Guarantees;
                companyJudgement.Risk.SecondInsured = companyJudgementOld.Risk.SecondInsured;
                companyJudgement.Attorney = companyJudgementOld.Attorney;
                return companyJudgement;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }

        }

        /// <summary>
        /// Sets the data modification.
        /// </summary>
        /// <param name="companyJudgement">The surety.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        private CompanyJudgement SetDataModification(CompanyJudgement companyJudgement)
        {
            try
            {
                CompanyJudgement companyJudgementOld = GetCompanyJudgementByRiskId(companyJudgement.Risk.Id);

                companyJudgement.Risk.RiskId = companyJudgementOld.Risk.RiskId;
                companyJudgement.Risk.Description = companyJudgementOld.Risk.Description;
                companyJudgement.Risk.RatingZone = companyJudgementOld.Risk.RatingZone;
                companyJudgement.Risk.MainInsured = companyJudgementOld.Risk.MainInsured;
                companyJudgement.Risk.Beneficiaries = companyJudgementOld.Risk.Beneficiaries;
                companyJudgement.Risk.Text = companyJudgementOld.Risk.Text;
                companyJudgement.Risk.Clauses = companyJudgementOld.Risk.Clauses;
                companyJudgement.Risk.Status = companyJudgementOld.Risk.Status;
                companyJudgement.Risk.OriginalStatus = companyJudgementOld.Risk.OriginalStatus;
                companyJudgement.Risk.SecondInsured = companyJudgementOld.Risk.SecondInsured;
                if (companyJudgementOld?.Guarantees != null && companyJudgementOld.Guarantees.Count > 0)
                {
                    companyJudgement.Guarantees = companyJudgementOld.Guarantees;
                }
                companyJudgement.Risk.Number = companyJudgementOld.Risk.Number;
                companyJudgement.Risk.CoveredRiskType = companyJudgementOld.Risk.CoveredRiskType;

                if (companyJudgement.Risk.Status != RiskStatusType.Included && companyJudgement.Risk.Status != RiskStatusType.Excluded)
                {
                    companyJudgement.Risk.Status = RiskStatusType.Modified;
                }
                if (companyJudgementOld.Attorney != null)
                {
                    companyJudgement.Attorney = companyJudgementOld.Attorney;
                }


                return companyJudgement;
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message);
            }

        }


        public bool ExistsRisk(CompanyJudgement companyJudgement, List<CompanyJudgement> companyJudgements)
        {
            bool exists = false;
            if (companyJudgements != null && companyJudgements.Any())
            {
                object lockobj = new object();
                Parallel.For(0, companyJudgements.Count, (i, loopState) =>
                 {
                     var judgement = companyJudgements[i];
                     if (companyJudgement.Risk.Id != judgement.Risk.Id)
                     {
                         if (companyJudgement.Risk.MainInsured == judgement.Risk.MainInsured)
                         {
                             lock (lockobj)
                             {
                                 exists = true;
                             }
                             loopState.Break();
                         }
                     }
                 });
            }
            return exists;
        }


        public List<CiaRiskSuretyGuarantee> SaveGuarantees(int riskId, List<CiaRiskSuretyGuarantee> guarantees)
        {
            try
            {
                CompanyJudgement riskCompanyJudgement = GetCompanyJudgementByRiskId(riskId);
                List<InsuredGuaranteeLog> guaranteesLog = new List<InsuredGuaranteeLog>();

                if (riskCompanyJudgement != null)
                {
                    foreach (CiaRiskSuretyGuarantee guarantee in guarantees)
                    {
                        if (guarantee.InsuredGuarantee.InsuredGuaranteeLog == null)
                        {
                            guaranteesLog = DelegateService.uniquePersonServiceCore.GetInsuredGuaranteeLogs(riskCompanyJudgement.Risk.SecondInsured.IndividualId, guarantee.Id);
                            guarantee.InsuredGuarantee.InsuredGuaranteeLog = guaranteesLog;
                        }
                    }

                    riskCompanyJudgement.Guarantees = guarantees;

                    riskCompanyJudgement = CreateJudgementTemporal(riskCompanyJudgement, false);

                    if (riskCompanyJudgement != null)
                    {
                        return guarantees;
                    }
                    else
                    {
                        throw new BusinessException(Errors.ErrorSaveGuarantees);
                    }
                }
                else
                {
                    throw new BusinessException(Errors.ErrorTempNoExist);
                }

            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorSaveRisk, ex);
            }
        }


        public bool DeleteRisk(int policyId, int id)
        {
            try
            {
                CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(policyId, false);
                bool result = false;

                if (policy != null)
                {
                    CompanyJudgement judgement = GetCompanyJudgementByRiskId(id);

                    if (judgement.Risk.Status == RiskStatusType.Original || judgement.Risk.Status == RiskStatusType.Included)
                    {
                        result = DelegateService.underwritingService.DeleteCompanyRisksByRiskId(judgement.Risk.Id, false);
                    }
                    else
                    {
                        judgement.Risk.Status = RiskStatusType.Excluded;
                        judgement.Risk.Coverages.ForEach(x => x.CurrentFrom = policy.CurrentFrom);
                        judgement.Risk.Coverages.ForEach(x => x.CurrentTo = policy.CurrentTo);
                        judgement.Risk.IsPersisted = true;
                        judgement.Risk.Policy = policy;

                        judgement = QuotateCompanyJudgement(judgement, false, false);
                        judgement.Risk.Coverages.ForEach(x => x.CoverStatusName = Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(x.CoverStatus)));

                        judgement = CreateJudgementTemporal(judgement, false);
                        result = true;
                    }

                    return result;
                }
                else
                {
                    throw new BusinessException(Errors.ErrorTempNoExist);
                }
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorDeleteRisk);
            }
        }


        public CompanyJudgement RunRules(CompanyJudgement companyJudgement, int? ruleSetId)
        {
            try
            {

                if (ruleSetId.GetValueOrDefault() > 0)
                {
                    companyJudgement = RunRulesRisk(companyJudgement, ruleSetId.Value);
                }
                return companyJudgement;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Metodo para actualizacion de poliza, se utiliza si el sistema detecta cambios en la pantalla principal de poliza para que
        /// actualice los riesgos y coberturas con los nuevos parametros. El metodo debe estar en todos los ramos haciendo
        /// la adecuacion al modelo correspondiente
        /// </summary>
        /// <param name="tempId">temporal de la poliza</param>
        /// <returns></returns>
        public CompanyPolicy UpdateRisks(int temporalId)
        {
            CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
            List<CompanyJudgement> judgements = GetCompanyJudgementsByTemporalId(temporalId);

            if (judgements.Count > 0 && judgements != null)
            {
                foreach (CompanyJudgement judgement in judgements)
                {
                    judgement.Risk.Policy = companyPolicy;
                    judgement.Risk.Coverages.ForEach(x => x.CurrentTo = companyPolicy.CurrentTo);
                    judgement.Risk.Coverages.ForEach(x => x.CurrentFrom = companyPolicy.CurrentFrom);

                    QuotateCompanyJudgement(judgement, true, true);
                    CreateJudgementTemporal(judgement, false);
                }

                companyPolicy = DelegateService.underwritingService.UpdatePolicyComponents(temporalId);

                return companyPolicy;

            }
            else
            {
                throw new BusinessException(Errors.ErrorUpdatePolicy);
            }
        }

        public void ConvertProspectToInsured(int temporalId, int individualId, string documentNumber)
        {
            try
            {
                DelegateService.underwritingService.ConvertProspectToHolder(temporalId, individualId, documentNumber);
                List<CompanyJudgement> companyJudgement = GetCompanyJudgementsByTemporalId(temporalId);

                if (companyJudgement.Count > 0)
                {
                    foreach (CompanyJudgement judgement in companyJudgement)
                    {
                        CompanyRisk risk = DelegateService.underwritingService.ConvertProspectToInsured(judgement.Risk, individualId, documentNumber);
                        judgement.Risk.Beneficiaries = risk.Beneficiaries;

                        CreateJudgementTemporal(judgement, false);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException("", ex);
            }
        }

        public CompanySummary ConvertProspectToInsuredRisk(CompanyPolicy companyPolicy, int individualId)
        {
            try
            {
                List<CompanyJudgement> companyJudgement = GetCompanyJudgementsByTemporalId(companyPolicy.Id);

                if (companyJudgement.Count > 0)
                {
                    foreach (CompanyJudgement judgement in companyJudgement)
                    {
                        judgement.Risk.Policy = companyPolicy;
                        if (judgement.Risk.MainInsured.CustomerType == CustomerType.Prospect)
                        {

                            CompanyRisk risk = DelegateService.underwritingService.ConvertProspectToInsuredRisk(judgement.Risk, individualId);
                            judgement.Risk.Beneficiaries = risk.Beneficiaries;
                        }
                        List<CompanyBeneficiary> listBeneficiary = new List<CompanyBeneficiary>();
                        judgement.Risk.Beneficiaries.ToList().ForEach(x =>
                        {
                            if (x.CustomerType == CustomerType.Prospect)
                            {
                                CompanyBeneficiary result = DelegateService.underwritingService.ConvertProspectToBeneficiary(x, individualId);
                                listBeneficiary.Add(result);
                            }
                            else
                            {
                                listBeneficiary.Add(x);
                            }
                        });
                        judgement.Risk.Beneficiaries = listBeneficiary;
                        judgement.Risk.Description = judgement.Risk.MainInsured.Name;
                        CreateJudgementTemporal(judgement, false);
                        CompanyRiskInsured companyRiskInsureds = new CompanyRiskInsured
                        {
                            Insured = judgement.Risk.MainInsured,
                            Beneficiaries = judgement.Risk.Beneficiaries
                        };
                        companyPolicy.Summary.RisksInsured[0] = companyRiskInsureds;
                        companyPolicy = DelegateService.underwritingService.CompanySavePolicyTemporal(companyPolicy, false);
                    }
                    return companyPolicy.Summary;

                }
                else
                {
                    return companyPolicy.Summary;
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorConvertingProspectIntoIndividual, ex);
            }
        }

        public List<CompanyBeneficiary> SaveBeneficiaries(int riskId, List<CompanyBeneficiary> beneficiaries)
        {
            try
            {
                CompanyJudgement risk = GetCompanyJudgementByRiskId(riskId);

                if (risk.Risk.Id > 0)
                {
                    risk.Risk.Beneficiaries = beneficiaries;

                    risk = CreateJudgementTemporal(risk, false);

                    if (risk != null)
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
                    throw new BusinessException(Errors.ErrorTempNoExist);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorSaveBeneficiaries, ex);
            }
        }

        public CompanyText SaveTexts(int riskId, CompanyText companyText)
        {
            try
            {
                CompanyJudgement riskCompanyJudgement = GetCompanyJudgementByRiskId(riskId);

                if (riskCompanyJudgement.Risk.Id > 0)
                {

                    riskCompanyJudgement.Risk.Text = companyText;

                    riskCompanyJudgement = CreateJudgementTemporal(riskCompanyJudgement, false);

                    if (riskCompanyJudgement != null)
                    {
                        return riskCompanyJudgement.Risk.Text;
                    }
                    else
                    {
                        throw new BusinessException(Errors.ErrorSaveText);
                    }
                }
                else
                {
                    throw new BusinessException(Errors.NoExistRisk);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorSaveText, ex);
            }
        }

        public List<CompanyClause> SaveClauses(int riskId, List<CompanyClause> clauses)
        {
            try
            {
                if (clauses != null)
                {
                    CompanyJudgement risk = GetCompanyJudgementByRiskId(riskId);

                    if (risk.Risk.Id > 0)
                    {
                        risk.Risk.Clauses = clauses;

                        risk = CreateJudgementTemporal(risk, false);

                        if (risk != null)
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
                        throw new BusinessException(Errors.NoExistRisk);

                    }
                }
                else
                {
                    throw new BusinessException(Errors.ErrorSelectedClauses);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorSaveClauses, ex);
            }
        }

        public void SaveCoverages(int policyId, int riskId, List<CompanyCoverage> coverages)
        {
            try
            {
                CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(policyId, false);
                policy.IsPersisted = true;

                CompanyJudgement judgement = GetCompanyJudgementByRiskId(riskId);

                if (policy != null && judgement != null && coverages != null)
                {
                    judgement.Risk.IsPersisted = true;
                    judgement.Risk.Coverages = coverages;
                    judgement = QuotateCompanyJudgement(judgement, false, true);
                    judgement = CreateJudgementTemporal(judgement, false);
                }
                else
                {
                    throw new BusinessException(Errors.NoExistTemporaryNoHaveCoverages);
                }
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorSaveCoverages);
            }
        }

        public CompanyCoverage ExcludeCoverage(int temporalId, int riskId, int riskCoverageId, string description)
        {
            try
            {
                CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                CompanyCoverage coverage;
                if (companyPolicy != null)
                {
                    CompanyJudgement judgement = GetCompanyJudgementByRiskId(riskId);

                    coverage = DelegateService.underwritingService.GetCompanyCoverageByRiskCoverageId(riskCoverageId);
                    coverage.Description = description;
                    coverage.SubLineBusiness = judgement.Risk.Coverages.First(x => x.RiskCoverageId == riskCoverageId).SubLineBusiness;
                    coverage.Rate = coverage.Rate * -1;
                    coverage.AccumulatedPremiumAmount = 0;
                    coverage.EndorsementType = companyPolicy.Endorsement.EndorsementType;
                    coverage.CurrentFrom = companyPolicy.CurrentFrom;
                    coverage.CoverStatus = CoverageStatusType.Excluded;
                    coverage = QuotateCoverage(judgement, coverage, false, false);
                    coverage.CoverStatusName = Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(CoverageStatusType.Excluded));
                    coverage.LimitAmount = 0;
                    coverage.SubLimitAmount = 0;
                    coverage.EndorsementLimitAmount = coverage.EndorsementLimitAmount * -1;
                    coverage.EndorsementSublimitAmount = coverage.EndorsementSublimitAmount * -1;
                }
                else
                {
                    throw new BusinessException(Errors.ErrorTempNoExist);
                }
                return coverage;

            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorEcludeCoverage, ex);
            }

        }

        public CompanyJudgement GetPremium(int policyId, CompanyJudgement riskCompanyJudgement)
        {
            try
            {
                if (riskCompanyJudgement.Risk?.Coverages?.Count > 0)
                {
                    CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(policyId, false);

                    if (companyPolicy != null)
                    {
                        companyPolicy.IsPersisted = true;

                        if (riskCompanyJudgement.Risk.Coverages != null)
                        {
                            foreach (CompanyCoverage item in riskCompanyJudgement.Risk.Coverages)
                            {
                                item.CurrentFrom = companyPolicy.CurrentFrom;
                                item.CurrentTo = companyPolicy.CurrentTo;
                            }
                        }
                        if (companyPolicy.Endorsement.EndorsementType == EndorsementType.Emission || companyPolicy.Endorsement.EndorsementType == EndorsementType.Renewal)
                        {
                            riskCompanyJudgement.Risk.Status = RiskStatusType.Original;
                        }
                        else if (companyPolicy.Endorsement.EndorsementType == EndorsementType.Modification && riskCompanyJudgement.Risk.RiskId == 0)
                        {
                            riskCompanyJudgement.Risk.Status = RiskStatusType.Included;
                        }
                        else
                        {
                            riskCompanyJudgement.Risk.Status = RiskStatusType.Modified;
                        }

                        riskCompanyJudgement.Risk.Policy = companyPolicy;
                        riskCompanyJudgement = QuotateCompanyJudgement(riskCompanyJudgement, true, true);

                        if (riskCompanyJudgement.Risk.Coverages != null)
                        {
                            foreach (CompanyCoverage item in riskCompanyJudgement.Risk.Coverages)
                            {
                                if (item.CoverStatus == CoverageStatusType.NotModified)
                                {
                                    if (item.EndorsementSublimitAmount > 0)
                                    {
                                        item.CoverStatus = CoverageStatusType.Modified;
                                    }
                                }

                                item.CoverStatusName = Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(item.CoverStatus.Value));
                            }
                        }


                    }
                    return riskCompanyJudgement;
                }
                else
                {
                    throw new BusinessException(Errors.EnterInsuredSelectCoverages);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.GetBaseException().Message, ex);
            }
        }

        #region emision
        public CompanyJudgement SaveAdditionalData(CompanyJudgement companyJudgement)
        {
            try
            {

                CompanyJudgement risk = GetCompanyJudgementByRiskId(companyJudgement.Risk.Id);
                var judgement = setDataAdditional(risk, companyJudgement);
                if (judgement != null)
                {
                    judgement = CreateJudgementTemporal(judgement, false);
                    return judgement;
                }
                else
                {
                    throw new Exception(Errors.NoExistRisk);
                }

            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorSaveAdditionalData);
            }
        }
        private CompanyJudgement setDataAdditional(CompanyJudgement companyJudgement, CompanyJudgement companyJudgementOld)
        {
            companyJudgement.Attorney = companyJudgementOld.Attorney;
            companyJudgement.Risk.SecondInsured = companyJudgementOld.Risk.SecondInsured;

            return companyJudgement;
        }
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
                            policy.Errors.Add(new ErrorBase { StateData = false, Error = Errors.ErrorHolderNoInsuredRole });
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
                        case CoveredRiskType.Surety:
                            List<CompanyJudgement> judicialSurety = GetCompanyJudgementsByTemporalId(policy.Id);

                            var result = judicialSurety.Select(x => x.Risk).Where(z => z.MainInsured?.CustomerType == CustomerType.Prospect).Count();
                            if (result > 0)
                            {
                                policy.Errors.Add(new ErrorBase { StateData = false, Error = Errors.ErrorInsuredNoInsuredRole });
                            }
                            break;
                    }
                }
            }

        }

        /// <summary>
        /// Creates the company policy.
        /// </summary>
        /// <param name="temporalId">The temporal identifier.</param>
        /// <param name="temporalType">Type of the temporal.</param>
        /// <param name="companyModification"></param>
        /// <param name="clearPolicies"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public CompanyPolicyResult CreateCompanyPolicy(int temporalId, int temporalType, CompanyModification companyModification, bool clearPolicies = false)
        {
            try
            {
                CompanyPolicyResult companyPolicyResult = new CompanyPolicyResult();
                companyPolicyResult.IsError = false;
                companyPolicyResult.Errors = new List<ErrorBase>();
                string message = string.Empty;
                CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                if (policy == null)
                {
                    companyPolicyResult.IsError = true;
                    companyPolicyResult.Errors.Add(new ErrorBase { StateData = false, Error = Errors.ErrorTemporalNotFound });
                }
                else
                {
                    if (EndorsementType.Modification == policy.Endorsement.EndorsementType && companyModification != null)
                    {
                        if (policy.Endorsement.Text != null)
                        {
                            policy.Endorsement.Text.TextBody = companyModification.Text;
                            policy.Endorsement.Text.Observations = companyModification.Observations;
                            policy.Endorsement.TicketDate = companyModification.RegistrationDate;
                            policy.Endorsement.TicketNumber = companyModification.RegistrationNumber;
                        }
                    }
                    policy.Errors = new List<ErrorBase>();
                    if (policy.Summary == null || ((policy.Endorsement?.EndorsementType != EndorsementType.Cancellation && policy.Endorsement?.EndorsementType != EndorsementType.Modification) && policy.Summary.Premium == 0))
                    {
                        companyPolicyResult.IsError = true;
                        companyPolicyResult.Errors.Add(new ErrorBase { StateData = false, Error = Errors.ErrorTempPremiumZero });
                    }
                    else
                    {
                        if (temporalType != TempType.Quotation)
                        {
                            ValidateHolder(ref policy);
                        }
                        if (!policy.Errors.Any())
                        {
                            switch (policy.Product.CoveredRisk.SubCoveredRiskType)
                            {
                                case SubCoveredRiskType.JudicialSurety:
                                    List<CompanyJudgement> companyJudgements = GetCompanyJudgementsByTemporalId(policy.Id);
                                    if (companyJudgements != null && companyJudgements.Any())
                                    {
                                        if (clearPolicies)
                                        {
                                            policy.InfringementPolicies.Clear();
                                            companyJudgements.ForEach(x => x.Risk.InfringementPolicies.Clear());
                                        }

                                        policy = CreateEndorsement(policy, companyJudgements);
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

                                        companyPolicyResult.Message = string.Format(Errors.PolicyNumber, policy.DocumentNumber);
                                        companyPolicyResult.DocumentNumber = policy.DocumentNumber;
                                        companyPolicyResult.EndorsementId = policy.Endorsement.Id;
                                        companyPolicyResult.EndorsementNumber = policy.Endorsement.Number;
                                        companyPolicyResult.IsReinsured = policy.IsReinsured;
                                    }
                                    else
                                    {
                                        companyPolicyResult.Message = string.Format(Errors.QuotationNumber, policy.Endorsement.QuotationId.ToString());
                                        companyPolicyResult.DocumentNumber = Convert.ToDecimal(policy.Endorsement.QuotationId);
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            companyPolicyResult.IsError = true;
                            companyPolicyResult.Errors.Add(new ErrorBase { StateData = false, Error = string.Join(" - ", policy.Errors) });
                        }


                    }
                }
                return companyPolicyResult;
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorCreatePolicy);
            }

        }
        #endregion emision


        public List<CompanyJudgement> GetCompanyJudicialSuretiesByEndorsementIdModuleType(int endorsementId, ModuleType moduleType)
        {
            try
            {
                JudgementBusiness judgementBusiness = new JudgementBusiness();
                List<CompanyJudgement> companyJudgements = new List<CompanyJudgement>();
                companyJudgements = judgementBusiness.GetCompanyJudicialSuretiesByEndorsementIdModuleType(endorsementId, moduleType);

                return companyJudgements;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyJudgement GetCompanyJudicialSuretyByRiskIdModuleType(int riskId, ModuleType moduleType)
        {
            try
            {
                JudgementBusiness judgementBusiness = new JudgementBusiness();
                return judgementBusiness.GetCompanyJudicialSuretyByRiskIdModuleType(riskId, moduleType);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Gets the company premium.
        /// </summary>
        /// <param name="policyId">The policy identifier.</param>
        /// <param name="vehicle">The vehicle.</param>
        /// <returns></returns>
        public CompanyJudgement GetCompanyPremium(int policyId, CompanyJudgement companyJudgement)
        {
            try
            {
                CompanyPolicy policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(policyId, false);
                policy.IsPersisted = true;
                companyJudgement.Risk.Policy = policy;
                companyJudgement.Risk.Description = companyJudgement.Risk.Description;
                companyJudgement = QuotateCompanyJudgement(companyJudgement, true, true);
                companyJudgement?.Risk?.Coverages.AsParallel().ForAll(x =>
                {
                    x.CoverStatusName = Errors.ResourceManager.GetString(EnumHelper.GetItemName<CoverageStatusType>(x?.CoverStatus.Value));
                });
                return companyJudgement;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.GetBaseException().Message);
            }

        }
        public List<CompanyProductArticle> DeleteCompanyProductArticle(List<CompanyProductArticle> productArticleDelete)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapCompanyArticle();

                var productArticles = imapper.Map<List<CompanyProductArticle>, List<ProductArticle>>(productArticleDelete);
                List<CompanyProductArticle> ListProductArticles = imapper.Map<List<ProductArticle>, List<CompanyProductArticle>>(DelegateService.judicialSuretyServiceCore.DeleteProductArticle(productArticles));
                return ListProductArticles;

            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorGetBranchs, ex);
            }
        }
        public List<CompanyProductArticle> UpdateCompanyProductArticle(List<CompanyProductArticle> productArticleUpdate)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapCompanyArticle();
                var productArticles = imapper.Map<List<CompanyProductArticle>, List<ProductArticle>>(productArticleUpdate);
                List<CompanyProductArticle> ListProductArticles = imapper.Map<List<ProductArticle>, List<CompanyProductArticle>>(DelegateService.judicialSuretyServiceCore.UpdateProductArticle(productArticles));
                return ListProductArticles;

            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorGetBranchs, ex);
            }
        }
        public List<CompanyProductArticle> InsertCompanyProductArticle(List<CompanyProductArticle> productArticleInsert)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapCompanyArticle();
                var productArticles = imapper.Map<List<CompanyProductArticle>, List<ProductArticle>>(productArticleInsert);
                List<CompanyProductArticle> ListProductArticles = imapper.Map<List<ProductArticle>, List<CompanyProductArticle>>(DelegateService.judicialSuretyServiceCore.InsertProductArticle(productArticles));
                return ListProductArticles;

            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorGetBranchs, ex);
            }
        }

        public List<CompanyArticleLine> getCompanyArticleLines()
        {
            try
            {
                var imapper = ModelAssembler.CreateMapCompanyArticle();
                List<CompanyArticleLine> articleLines = imapper.Map<List<ArticleLine>, List<CompanyArticleLine>>(DelegateService.judicialSuretyServiceCore.getArticleLines());
                return articleLines;

            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorGetBranchs, ex);
            }
        }
        public List<CompanyArticleLine> GetCompanyArticleLineByDescription(string smallDescription)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapCompanyArticle();
                List<CompanyArticleLine> articleLines = imapper.Map<List<ArticleLine>, List<CompanyArticleLine>>(DelegateService.judicialSuretyServiceCore.GetArticleLineByDescription(smallDescription));
                return articleLines;

            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorGetBranchs, ex);
            }
        }

        public List<CompanyArticleLine> CompanyArticleLineDelete(List<CompanyArticleLine> articleLineDelete)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapCompanyArticle();
                var articleLine = imapper.Map<List<CompanyArticleLine>, List<ArticleLine>>(articleLineDelete);
                List<CompanyArticleLine> ListArticleLine = imapper.Map<List<ArticleLine>, List<CompanyArticleLine>>(DelegateService.judicialSuretyServiceCore.ArticleLineDelete(articleLine));
                return ListArticleLine;
            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorGetBranchs, ex);
            }

        }
        public List<CompanyArticleLine> CompanyArticleLineUpdate(List<CompanyArticleLine> articleLineUpdate)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapCompanyArticle();
                var articleLine = imapper.Map<List<CompanyArticleLine>, List<ArticleLine>>(articleLineUpdate);
                List<CompanyArticleLine> ListArticleLine = imapper.Map<List<ArticleLine>, List<CompanyArticleLine>>(DelegateService.judicialSuretyServiceCore.ArticleLineUpdate(articleLine));
                return ListArticleLine;
            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorGetBranchs, ex);
            }

        }

        public List<CompanyArticleLine> CompanyArticleLineInsert(List<CompanyArticleLine> articleLineInsert)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapCompanyArticle();
                var articleLine = imapper.Map<List<CompanyArticleLine>, List<ArticleLine>>(articleLineInsert);
                List<CompanyArticleLine> ListArticleLine = imapper.Map<List<ArticleLine>, List<CompanyArticleLine>>(DelegateService.judicialSuretyServiceCore.ArticleLineInsert(articleLine));
                return ListArticleLine;
            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorGetBranchs, ex);
            }

        }

        public List<CompanyCourt> GetCompanyCourtsTypeByDescription(string smallDescription)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapCompanyArticle();
                List<CompanyCourt> companyCourt = imapper.Map<List<Court>, List<CompanyCourt>>(DelegateService.judicialSuretyServiceCore.GetCourtsTypeByDescription(smallDescription));
                return companyCourt;
            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorGetBranchs, ex);
            }

        }
        public List<CompanyCourt> GetCompanyCourtsType()
        {
            try
            {
                var imapper = ModelAssembler.CreateMapCompanyArticle();
                List<CompanyCourt> companyCourt = imapper.Map<List<Court>, List<CompanyCourt>>(DelegateService.judicialSuretyServiceCore.GetCourts());
                return companyCourt;

            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorGetBranchs, ex);
            }
        }

        public List<CompanyCourt> CompanyCourtTypeDelete(List<CompanyCourt> courtTypeDelete)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapCompanyArticle();
                var courts = imapper.Map<List<CompanyCourt>, List<Court>>(courtTypeDelete);
                List<CompanyCourt> ListCourtType = imapper.Map<List<Court>, List<CompanyCourt>>(DelegateService.judicialSuretyServiceCore.CourtTypeDelete(courts));
                return ListCourtType;
            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorGetBranchs, ex);
            }
        }
        public List<CompanyCourt> CompanyCourtTypeUpdate(List<CompanyCourt> courtTypeUpdate)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapCompanyArticle();
                var courts = imapper.Map<List<CompanyCourt>, List<Court>>(courtTypeUpdate);
                List<CompanyCourt> ListCourtType = imapper.Map<List<Court>, List<CompanyCourt>>(DelegateService.judicialSuretyServiceCore.CourtTypeUpdate(courts));
                return ListCourtType;
            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorGetBranchs, ex);
            }
        }
        public List<CompanyCourt> CompanyCourtTypeInsert(List<CompanyCourt> courtTypeInsert)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapCompanyArticle();
                var courts = imapper.Map<List<CompanyCourt>, List<Court>>(courtTypeInsert);
                List<CompanyCourt> ListCourtType = imapper.Map<List<Court>, List<CompanyCourt>>(DelegateService.judicialSuretyServiceCore.CourtTypeInsert(courts));
                return ListCourtType;
            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorGetBranchs, ex);
            }
        }

        public bool GetInsuredGuaranteeRelationPolicy(int guaranteeId)
        {
            try
            {
                JudicialSuretyDAO judicialSuretyDAO = new JudicialSuretyDAO();
                return judicialSuretyDAO.GetInsuredGuaranteeRelationPolicy(guaranteeId);
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorSearchGuarantees);
            }
        }
    }

}