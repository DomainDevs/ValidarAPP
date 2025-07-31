using System.Collections.Generic;
using Sistran.Company.Application.Aircrafts.AircraftApplicationService.DTOs;
using Sistran.Company.Application.Aircrafts.AircraftApplicationService.EEProvider.Business;
using Sistran.Company.Application.Aircrafts.AircraftApplicationService.EEProvider.Resources;
using Sistran.Company.Application.Aircrafts.AircraftApplicationService.EEProvider.Assemblers;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.Utilities.Managers;
using System;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Company.Application.Aircrafts.AircraftBusinessService.Models.Base;
using Sistran.Company.Application.UnderwritingServices.Models;

namespace Sistran.Company.Application.Aircrafts.AircraftApplicationService.EEProvider
{
    public class CompanyAircraftApplicationServiceProvider : ICompanyAircraftApplicationService
    {
        public List<CompanyBeneficiary> GetBeneficiariesByDescriptionInsuredSearchTypeCustomer(string description, InsuredSearchType insuredSearchType, CustomerType custormerType)
        {
            try
            {
                AircraftBusiness AircraftBusiness = new AircraftBusiness();
                return AircraftBusiness.GetBeneficiariesByDescriptionInsuredSearchTypeCustomer(description, insuredSearchType, custormerType);
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
                AircraftBusiness AircraftBusiness = new AircraftBusiness();
                return AircraftBusiness.GetCitiesByContryIdStateId(countryId, stateId);
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
                AircraftBusiness AircraftBusiness = new AircraftBusiness();
                return AircraftBusiness.GetCountries();
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
                AircraftBusiness AircraftBusiness = new AircraftBusiness();
                return AircraftBusiness.GetGroupCoveragesByPrefixIdProductId(prefixId, productId);
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
                AircraftBusiness AircraftBusiness = new AircraftBusiness();
                return AircraftBusiness.GetNotificationAddressesByIndividualId(individualId);
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
                AircraftBusiness AircraftBusiness = new AircraftBusiness();
                return AircraftBusiness.GetInsuredByDescriptionInsuredSearchTypeCustomer(description, insuredSearchType, customerType);
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
                AircraftBusiness AircraftBusiness = new AircraftBusiness();
                return AircraftBusiness.GetStatesByCountryId(countryId);
            }
            catch (System.Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetStatesByCountryId), ex);
            }
        }

        public List<AircraftDTO> GetAircraftsByTemporalId(int temporalId)
        {
            try
            {
                AircraftBusiness AircraftBusiness = new AircraftBusiness();
                return AircraftBusiness.GetAircraftsByTemporalId(temporalId);
            }
            catch (System.Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetAircraftsByTemporalId), ex);
            }
        }


        public List<CoverageDTO> QuotateCoverages(List<CoverageDTO> coverages, AircraftDTO AircraftDTO, bool runRulesPre, bool runRulesPost)
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
                    PremiumAmount = QuotateCoverage(coverage, AircraftDTO, runRulesPre, runRulesPost).PremiumAmount
                });

            }
            return dto;
        }
        public CoverageDTO QuotateCoverage(CoverageDTO CoverageDTO, AircraftDTO AircraftDTO, bool runRulesPre, bool runRulesPost)
        {
            try
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                return coverageBusiness.QuotateCoverage(CoverageDTO, AircraftDTO, runRulesPre, runRulesPost);
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

        public AircraftDTO SaveAircraft(AircraftDTO AircraftDTO)
        {
            try
            {
                AircraftBusiness AircraftBusiness = new AircraftBusiness();
                return AircraftBusiness.SaveAircraft(AircraftDTO);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateCompanyAircraftTemporal), ex);
            }
        }

        public List<AircraftDTO> GetAircraftsByPolicyId(int policyId)
        {
            try
            {
                AircraftBusiness AircraftBusiness = new AircraftBusiness();
                return AircraftBusiness.GetAircraftsByPolicyId(policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetAircraftsByPolicyId), ex);
            }
        }

        public AircraftDTO GetAircraftByRiskId(int riskId)
        {
            try
            {
                AircraftBusiness AircraftBusiness = new AircraftBusiness();
                return AircraftBusiness.GetAircraftByRiskId(riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCompanyAircraftByRiskId), ex);
            }
        }

        public PolicyTypeDTO GetPolicyTypeByPolicyTypeIdPrefixId(int policyTypeId, int prefixId)
        {
            try
            {
                AircraftBusiness AircraftBusiness = new AircraftBusiness();
                return AircraftBusiness.GetPolicyTypeByPolicyTypeIdPrefixId(policyTypeId, prefixId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetPolicyTypeByPrefixIdPolicyTypeCode), ex);
            }
        }

        public AircraftDTO QuotateAircraft(AircraftDTO AircraftDTO, bool runRulesPre, bool runRulesPost)
        {
            try
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                return coverageBusiness.QuotateAircraft(AircraftDTO, runRulesPre, runRulesPost);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorQuotateCompanyAircraft), ex);
            }
        }

        public AircraftDTO ExcludeAircraft(int temporalId, int riskId)
        {
            try
            {
                AircraftBusiness AircraftBusiness = new AircraftBusiness();
                return AircraftBusiness.ExcludeAircraft(temporalId, riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorExcludeAircraft), ex);
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
                AircraftBusiness AircraftBusiness = new AircraftBusiness();
                return AircraftBusiness.SaveText(riskId, textDTO);
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
                AircraftBusiness AircraftBusiness = new AircraftBusiness();
                return AircraftBusiness.SaveClauses(temporalId, riskId, clauseDTOs);
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

                AircraftBusiness AircraftBusiness = new AircraftBusiness();
                return AircraftBusiness.SaveBeneficiaries(temporalId, beneficiariesDTOs);

            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorSaveCompanyBeneficiary));
            }
        }

        public AircraftDTO RunRulesRisk(AircraftDTO aircraftDTO, int ruleId)
        {
            try
            {
                AircraftBusiness AircraftBusiness = new AircraftBusiness();
                return AircraftBusiness.RunRulesRisk(ModelAssembler.CreateAircraft(aircraftDTO), ruleId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorRunRules));
            }
        }

        public AircraftDTO SaveCoverages(int policyId, int riskId, List<CoverageDTO> coverages, int insuredObjectId)
        {
            try
            {
                AircraftBusiness AircraftBusiness = new AircraftBusiness();
                return AircraftBusiness.SaveCoverages(policyId, riskId, ModelAssembler.CreateCoverages(coverages), insuredObjectId);
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
                AircraftBusiness AircraftBusiness = new AircraftBusiness();
                return AircraftBusiness.CreateEndorsement(temporalId);
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
                AircraftBusiness AircraftBusiness = new AircraftBusiness();
                return AircraftBusiness.GetEndorsementByEndorsementTypeIdPolicyId(endorsementTypeId, policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetAircraftsByPolicyId), ex);
            }
        }

        public List<AircraftDTO> GetCompanyAircraftsByPolicyIdEndorsementId(int policyId, int endorsementId)
        {
            try
            {
                AircraftBusiness AircraftBusiness = new AircraftBusiness();
                return AircraftBusiness.GetCompanyAircraftsByPolicyIdEndorsementId(policyId, endorsementId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetAircraftsByPolicyId), ex);
            }
        }

        public List<CoverageDTO> GetCoveragesByRiskId(int riskId, int temporalId)
        {
            List<CoverageDTO> coverageDTOs = new List<CoverageDTO>();
            return coverageDTOs = DTOAssembler.CreateCoverages(DelegateService.AircraftBusinessService.GetCoveragesByRiskId(riskId, temporalId));

        }


        public EndorsementDTO GetTemporalEndorsementByPolicyId(int policyId)
        {
            try
            {
                AircraftBusiness AircraftBusiness = new AircraftBusiness();
                return AircraftBusiness.GetTemporalEndorsementByPolicyId(policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetAircraftsByPolicyId), ex);
            }
        }

        public List<CoverageDTO> GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(int policyId, int endorsementId, int riskId)
        {
            try
            {
                AircraftBusiness AircraftBusiness = new AircraftBusiness();
                return AircraftBusiness.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(policyId, endorsementId, riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetAircraftsByPolicyId), ex);
            }
        }

        public TextDTO GetTextsByRiskId(int riskId)
        {
            try
            {
                AircraftBusiness AircraftBusiness = new AircraftBusiness();
                return AircraftBusiness.GetTextsByRiskId(riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetAircraftsByTemporalId), ex);
            }
        }
     
        public bool DeleteCompanyRisk(int temporalId, int riskId)
        {
            AircraftBusiness AircraftBusiness = new AircraftBusiness();
            return AircraftBusiness.DeleteCompanyRisk(temporalId, riskId);
        }

        public List<CoverageDTO> GetTemporalCoveragesByRiskIdInsuredObjectId(int riskId, int insuredObjectId)
        {
            AircraftBusiness AircraftBusiness = new AircraftBusiness();
            return AircraftBusiness.GetTemporalCoveragesByRiskIdInsuredObjectId(riskId, insuredObjectId);
        }

        public bool SaveInsuredObject(int riskId, InsuredObjectDTO insuredObjectDTO, int tempId, int groupCoverageId)
        {
            AircraftBusiness AircraftBusiness = new AircraftBusiness();
            return AircraftBusiness.saveInsuredObject(riskId, insuredObjectDTO, tempId, groupCoverageId);
        }


        public List<CoverageDTO> GetCoveragesByInsuredObjectIdGroupCoverageIdProductId(int insuredObjectId, int groupCoverageId, int productId)
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

        public List<InsuredDTO> GetInsuredsByName(string name)
        {
            throw new System.NotImplementedException();
        }

        public List<MakeDTO> GetMakes()
        {
            try
            {
                AircraftBusiness aircraftBusiness = new AircraftBusiness();
                return aircraftBusiness.GetMakes();
            }
            catch (System.Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetMakes), ex);
            }
        }

        public List<ModelDTO> GetModelsByMakeId(int makeId)
        {
            try
            {
                AircraftBusiness aircraftBusiness = new AircraftBusiness();
                return aircraftBusiness.GetModelsByMakeId(makeId);

            }
            catch (Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetModelsByMakeId), ex);
            }
        }

        public List<OperatorDTO> GetOperators()
        {
            try
            {
                AircraftBusiness aircraftBusiness = new AircraftBusiness();
                return aircraftBusiness.GetOperators();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetOperators), ex);
            }
        }


        public List<RegisterDTO> GetRegisters()
        {
            try
            {
                AircraftBusiness aircraftBusiness = new AircraftBusiness();
                return aircraftBusiness.GetRegisters();

            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetRegisters), ex);
            }
        }

     
        public List<UseDTO> GetUsesByPrefixId(int prefixId)
        {
            try
            {
                AircraftBusiness aircraftBusiness = new AircraftBusiness();
                return aircraftBusiness.GetUsesByPrefixId(prefixId);

            }
            catch (Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetUsesByPrefixId), ex);
            }
        }

        public CombosRiskDTO GetCombosRisk(int prefixId)
        {
            try
            {
                AircraftBusiness aircraftBusiness = new AircraftBusiness();
                CombosRiskDTO combosRiskDTO = new CombosRiskDTO();
                combosRiskDTO.Makes = aircraftBusiness.GetMakes();
                combosRiskDTO.Operators = aircraftBusiness.GetOperators();
                combosRiskDTO.Registers = aircraftBusiness.GetRegisters();
                combosRiskDTO.Types = aircraftBusiness.GetTybeByTypesByPrefixId(prefixId);
                combosRiskDTO.Uses = aircraftBusiness.GetUseByusessByPrefixId(prefixId);
                return combosRiskDTO;

            }
            catch (Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetUsesByPrefixId), ex);
            }
        }
    }
}