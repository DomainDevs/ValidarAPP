using System.Collections.Generic;
using Sistran.Company.Application.Marines.MarineApplicationService.DTOs;
using Sistran.Company.Application.Marines.MarineApplicationService.EEProvider.Business;
using Sistran.Company.Application.Marines.MarineApplicationService.EEProvider.Resources;
using Sistran.Company.Application.Marines.MarineApplicationService.EEProvider.Assemblers;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.Utilities.Managers;
using System;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Company.Application.Marines.MarineBusinessService.Models.Base;
using Sistran.Company.Application.UnderwritingServices.Models;

namespace Sistran.Company.Application.Marines.MarineApplicationService.EEProvider
{
    public class CompanyMarineApplicationServiceProvider : ICompanyMarineApplicationService
    {
        public List<CompanyBeneficiary> GetBeneficiariesByDescriptionInsuredSearchTypeCustomer(string description, InsuredSearchType insuredSearchType, CustomerType custormerType)
        {
            try
            {
                MarineBusiness MarineBusiness = new MarineBusiness();
                return MarineBusiness.GetBeneficiariesByDescriptionInsuredSearchTypeCustomer(description, insuredSearchType, custormerType);
            }
            catch (System.Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetBillingPeriods), ex);
            }
        }

        public List<BeneficiaryTypeDTO> GetBeneficiaryTypes()
        {
            try
            {
                return DTOAssembler.CreateBenefiarytypes(DelegateService.underwritingService.GetCompanyBeneficiaryTypes());
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetBeneficiaryTypes), ex);
            }
        }


        public List<SelectObjectDTO> GetCalculationTypes()
        {
            try
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                return coverageBusiness.GetCalculationTypes();
            }
            catch (System.Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCalculationTypes), ex);
            }
        }

        public List<CityDTO> GetCitiesByContryIdStateId(int countryId, int stateId)
        {
            try
            {
                MarineBusiness MarineBusiness = new MarineBusiness();
                return MarineBusiness.GetCitiesByContryIdStateId(countryId, stateId);
            }
            catch (System.Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCitiesByContryIdStateId), ex);
            }
        }

        public List<CountryDTO> GetCountries()
        {
            try
            {
                MarineBusiness MarineBusiness = new MarineBusiness();
                return MarineBusiness.GetCountries();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCountries), ex);
            }
        }

        public List<GroupCoverageDTO> GetGroupCoveragesByPrefixIdProductId(int prefixId, int productId)
        {
            try
            {
                MarineBusiness MarineBusiness = new MarineBusiness();
                return MarineBusiness.GetGroupCoveragesByPrefixIdProductId(prefixId, productId);
            }
            catch (System.Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCoverageGroupsByPrefixIdProductId), ex);
            }
        }

        public List<DeductibleDTO> GetDeductiblesByCoverageId(int coverageId)
        {
            try
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                return coverageBusiness.GetDeductiblesByCoverageId(coverageId);
            }
            catch (System.Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetDeductiblesByCoverageId), ex);
            }
        }

        public NotificationAddressDTO GetNotificationAddressesByIndividualId(int individualId)
        {
            try
            {
                MarineBusiness MarineBusiness = new MarineBusiness();
                return MarineBusiness.GetNotificationAddressesByIndividualId(individualId);
            }
            catch (System.Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetNotificationInfoByIndividualId), ex);
            }
        }

        public List<IndividualDetailsDTO> GetInsuredsByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType customerType)
        {
            try
            {
                MarineBusiness MarineBusiness = new MarineBusiness();
                return MarineBusiness.GetInsuredByDescriptionInsuredSearchTypeCustomer(description, insuredSearchType, customerType);
            }
            catch (System.Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetInsuredsByDescriptionInsuredSearchTypeCustomerType), ex);
            }
        }

        public List<SelectObjectDTO> GetRateTypes()
        {
            try
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                return coverageBusiness.GetRateTypes();
            }
            catch (System.Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetRateTypes), ex);
            }

        }

        public List<StateDTO> GetStatesByCountryId(int countryId)
        {
            try
            {
                MarineBusiness MarineBusiness = new MarineBusiness();
                return MarineBusiness.GetStatesByCountryId(countryId);
            }
            catch (System.Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetStatesByCountryId), ex);
            }
        }

        public List<MarineDTO> GetMarinesByTemporalId(int temporalId)
        {
            try
            {
                MarineBusiness MarineBusiness = new MarineBusiness();
                return MarineBusiness.GetMarinesByTemporalId(temporalId);
            }
            catch (System.Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetMarinesByTemporalId), ex);
            }
        }

        public List<CoverageDTO> QuotateCoverages(List<CoverageDTO> coverages, MarineDTO MarineDTO, bool runRulesPre, bool runRulesPost)
        {
            List<CoverageDTO> dto = new List<CoverageDTO>();
            foreach (var coverage in coverages)
            {

                dto.Add(new CoverageDTO
                {
                    RateTypeId = coverage.RateTypeId,
                    Rate = coverage.Rate,
                    CalculationTypeId = coverage.CalculationTypeId,
                    CoverageId = coverage.CoverageId,
                    CoverStatus = coverage.CoverStatus,
                    CoverStatusName = coverage.CoverStatusName,
                    CurrentFrom = coverage.CurrentFrom,
                    CurrentTo = coverage.CurrentTo,
                    DeclaredAmount = coverage.DeclaredAmount,
                    DeductibleId = coverage.DeductibleId,
                    DepositPremiumPercent = coverage.DepositPremiumPercent,
                    Description = coverage.Description,
                    Id = coverage.Id,
                    IsDeclarative = coverage.IsDeclarative,
                    IsMandatory = coverage.IsMandatory,
                    IsPrimary = coverage.IsPrimary,
                    IsSelected = coverage.IsSelected,
                    LimitAmount = coverage.LimitAmount,
                    LimitClaimantAmount = coverage.LimitClaimantAmount,
                    LimitOccurrenceAmount = coverage.LimitOccurrenceAmount,
                    MaxLiabilityAmount = coverage.MaxLiabilityAmount,
                    Number = coverage.Number,
                    InsuredObject = coverage.InsuredObject,
                    SubLimitAmount = coverage.SubLimitAmount,
                    SubLineBusiness = coverage.SubLineBusiness,
                    PremiumAmount = QuotateCoverage(coverage, MarineDTO, runRulesPre, runRulesPost).PremiumAmount
                });

            }
            return dto;
        }
        public CoverageDTO QuotateCoverage(CoverageDTO CoverageDTO, MarineDTO MarineDTO, bool runRulesPre, bool runRulesPost)
        {
            try
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                return coverageBusiness.QuotateCoverage(CoverageDTO, MarineDTO, runRulesPre, runRulesPost);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorQuotateCoverage), ex);
            }
        }

        public List<InsuredObjectDTO> GetInsuredObjectsByProductIdGroupCoverageId(int productId, int groupCoverageId, int prefixId)
        {
            try
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                return coverageBusiness.GetInsuredObjectsByProductIdGroupCoverageId(productId, groupCoverageId, prefixId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<CoverageDTO> GetCoveragesByProductIdGroupCoverageIdInsuredObjectId(int productId, int groupCoverageId, int insuredObjectId)
        {
            try
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                return coverageBusiness.GetCoveragesByProductIdGroupCoverageIdInsuredObjectId(productId, groupCoverageId, insuredObjectId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCoveragesByInsuredObjectIdGroupCoverageIdProductId), ex);
            }
        }

        public List<CoverageDTO> GetCoveragesByInsuredObjectId(int insuredObjectId)
        {
            try
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                return coverageBusiness.GetCoveragesByInsuredObjectId(insuredObjectId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCoveragesByInsuredObjectId), ex);
            }
        }

        public MarineDTO SaveMarine(MarineDTO MarineDTO)
        {
            try
            {
                MarineBusiness MarineBusiness = new MarineBusiness();
                return MarineBusiness.SaveMarine(MarineDTO);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateCompanyMarineTemporal), ex);
            }
        }

        public List<MarineDTO> GetMarinesByPolicyId(int policyId)
        {
            try
            {
                MarineBusiness MarineBusiness = new MarineBusiness();
                return MarineBusiness.GetMarinesByPolicyId(policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetMarinesByPolicyId), ex);
            }
        }

        public MarineDTO GetMarineByRiskId(int riskId)
        {
            try
            {
                MarineBusiness MarineBusiness = new MarineBusiness();
                return MarineBusiness.GetMarineByRiskId(riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCompanyMarineByRiskId), ex);
            }
        }

        public PolicyTypeDTO GetPolicyTypeByPolicyTypeIdPrefixId(int policyTypeId, int prefixId)
        {
            try
            {
                MarineBusiness MarineBusiness = new MarineBusiness();
                return MarineBusiness.GetPolicyTypeByPolicyTypeIdPrefixId(policyTypeId, prefixId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetPolicyTypeByPrefixIdPolicyTypeCode), ex);
            }
        }

        public MarineDTO QuotateMarine(MarineDTO MarineDTO, bool runRulesPre, bool runRulesPost)
        {
            try
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                return coverageBusiness.QuotateMarine(MarineDTO, runRulesPre, runRulesPost);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorQuotateCompanyMarine), ex);
            }
        }

        public MarineDTO ExcludeMarine(int temporalId, int riskId)
        {
            try
            {
                MarineBusiness MarineBusiness = new MarineBusiness();
                return MarineBusiness.ExcludeMarine(temporalId, riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorExcludeMarine), ex);
            }
        }

        public CoverageDTO ExcludeCoverage(int temporalId, int riskId, int coverageId)
        {
            try
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                return coverageBusiness.ExcludeCoverage(temporalId, riskId, coverageId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorExcludeCoverage));
            }
        }

        public CompanyBeneficiary SaveBeneficiary(CompanyBeneficiary beneficiaryDTO)
        {
            return null;
        }

        public TextDTO SaveText(int riskId, TextDTO textDTO)
        {
            try
            {
                MarineBusiness MarineBusiness = new MarineBusiness();
                return MarineBusiness.SaveText(riskId, textDTO);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorSaveText), ex); ;
            }
        }

        public List<ClauseDTO> SaveClauses(int temporalId, int riskId, List<ClauseDTO> clauseDTOs)
        {
            try
            {
                MarineBusiness MarineBusiness = new MarineBusiness();
                return MarineBusiness.SaveClauses(temporalId, riskId, clauseDTOs);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorSaveClauses), ex); ;
            }
        }

        public List<CompanyBeneficiary> SaveBeneficiaries(int temporalId, List<CompanyBeneficiary> beneficiariesDTOs, bool isColective)
        {
            try
            {

                MarineBusiness MarineBusiness = new MarineBusiness();
                return MarineBusiness.SaveBeneficiaries(temporalId, beneficiariesDTOs);

            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorSaveCompanyBeneficiary));
            }
        }

        public MarineDTO RunRulesRisk(MarineDTO MarineDTO, int ruleId)
        {
            try
            {
                MarineBusiness MarineBusiness = new MarineBusiness();
                return MarineBusiness.RunRulesRisk(ModelAssembler.CreateMarine(MarineDTO), ruleId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorRunRules));
            }
        }

        public MarineDTO SaveCoverages(int policyId, int riskId, List<CoverageDTO> coverages, int insuredObjectId)
        {
            try
            {
                MarineBusiness MarineBusiness = new MarineBusiness();
                return MarineBusiness.SaveCoverages(policyId, riskId, ModelAssembler.CreateCoverages(coverages), insuredObjectId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorSaveCoverages));
            }
        }

        public EndorsementDTO CreateEndorsement(int temporalId)
        {
            try
            {
                MarineBusiness MarineBusiness = new MarineBusiness();
                return MarineBusiness.CreateEndorsement(temporalId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorSaveCoverages), ex);
            }
        }

        public List<EndorsementDTO> GetEndorsementByEndorsementTypeIdPolicyId(int endorsementTypeId, int policyId)
        {
            try
            {
                MarineBusiness MarineBusiness = new MarineBusiness();
                return MarineBusiness.GetEndorsementByEndorsementTypeIdPolicyId(endorsementTypeId, policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetMarinesByPolicyId), ex);
            }
        }

        public List<MarineDTO> GetCompanyMarinesByPolicyIdEndorsementId(int policyId, int endorsementId)
        {
            try
            {
                MarineBusiness MarineBusiness = new MarineBusiness();
                return MarineBusiness.GetCompanyMarinesByPolicyIdEndorsementId(policyId, endorsementId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetMarinesByPolicyId), ex);
            }
        }

        public List<CoverageDTO> GetCoveragesByRiskId(int riskId, int temporalId)
        {
            List<CoverageDTO> coverageDTOs = new List<CoverageDTO>();
            return coverageDTOs = DTOAssembler.CreateCoverages(DelegateService.marineBusinessService.GetCoveragesByRiskId(riskId, temporalId));

        }


        public EndorsementDTO GetTemporalEndorsementByPolicyId(int policyId)
        {
            try
            {
                MarineBusiness MarineBusiness = new MarineBusiness();
                return MarineBusiness.GetTemporalEndorsementByPolicyId(policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetMarinesByPolicyId), ex);
            }
        }

        public List<CoverageDTO> GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(int policyId, int endorsementId, int riskId)
        {
            try
            {
                MarineBusiness MarineBusiness = new MarineBusiness();
                return MarineBusiness.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(policyId, endorsementId, riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetMarinesByPolicyId), ex);
            }
        }

        public TextDTO GetTextsByRiskId(int riskId)
        {
            try
            {
                MarineBusiness MarineBusiness = new MarineBusiness();
                return MarineBusiness.GetTextsByRiskId(riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetMarinesByTemporalId), ex);
            }
        }
        public bool GetLeapYear()
        {
            try
            {
                MarineBusiness MarineBusiness = new MarineBusiness();
                return MarineBusiness.GetLeapYear();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public bool DeleteCompanyRisk(int temporalId, int riskId)
        {
            MarineBusiness MarineBusiness = new MarineBusiness();
            return MarineBusiness.DeleteCompanyRisk(temporalId, riskId);
        }

        public List<CoverageDTO> GetTemporalCoveragesByRiskIdInsuredObjectId(int riskId, int insuredObjectId)
        {
            MarineBusiness MarineBusiness = new MarineBusiness();
            return MarineBusiness.GetTemporalCoveragesByRiskIdInsuredObjectId(riskId, insuredObjectId);
        }

        public bool SaveInsuredObject(int riskId, InsuredObjectDTO insuredObjectDTO, int tempId, int groupCoverageId)
        {
            MarineBusiness MarineBusiness = new MarineBusiness();
            return MarineBusiness.saveInsuredObject(riskId, insuredObjectDTO, tempId, groupCoverageId);
        }

        public List<UsePrefixDTO> GetUsesMarine(int prefixId)
        {
            try
            {
                MarineBusiness MarineBusiness = new MarineBusiness();
                List<UsePrefixDTO> usDTO = new List<UsePrefixDTO>();
                usDTO = MarineBusiness.GetMarineUsesByPrefixId(prefixId);
                return usDTO;

            }
            catch (Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetUsesByPrefixId), ex);
            }
        }
       
    }
}