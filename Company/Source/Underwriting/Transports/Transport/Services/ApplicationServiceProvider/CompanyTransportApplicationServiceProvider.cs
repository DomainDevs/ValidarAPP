using System.Collections.Generic;
using Sistran.Company.Application.Transports.TransportApplicationService.DTOs;
using Sistran.Company.Application.Transports.TransportApplicationService.EEProvider.Business;
using Sistran.Company.Application.Transports.TransportApplicationService.EEProvider.Resources;
using Sistran.Company.Application.Transports.TransportApplicationService.EEProvider.Assemblers;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.Utilities.Managers;
using System;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Company.Application.Transports.TransportBusinessService.Models.Base;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Transports.TransportBusinessService.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;

namespace Sistran.Company.Application.Transports.TransportApplicationService.EEProvider
{
    public class CompanyTransportApplicationServiceProvider : ICompanyTransportApplicationService
    {
        public List<CompanyBeneficiary> GetBeneficiariesByDescriptionInsuredSearchTypeCustomer(string description, InsuredSearchType insuredSearchType, CustomerType custormerType)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.GetBeneficiariesByDescriptionInsuredSearchTypeCustomer(description, insuredSearchType, custormerType);
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

        public List<BillingPeriodDTO> GetBillingPeriods()
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.GetBillingPeriods();
            }
            catch (System.Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetBillingPeriods), ex);
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

        public List<CargoTypeDTO> GetCargoTypes()
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.GetCargoTypes();
            }
            catch (System.Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCargoTypes), ex);
            }
        }

        public List<CityDTO> GetCitiesByContryIdStateId(int countryId, int stateId)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.GetCitiesByContryIdStateId(countryId, stateId);
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
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.GetCountries();
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
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.GetGroupCoveragesByPrefixIdProductId(prefixId, productId);
            }
            catch (System.Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCoverageGroupsByPrefixIdProductId), ex);
            }
        }

        public List<DeclarationPeriodDTO> GetDeclarationPeriods()
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.GetDeclarationPeriods();
            }
            catch (System.Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetDeclarationPeriods), ex);
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
                TransportBusiness TransportBusiness = new TransportBusiness();
                return TransportBusiness.GetNotificationAddressesByIndividualId(individualId);
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
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.GetInsuredByDescriptionInsuredSearchTypeCustomer(description, insuredSearchType, customerType);
            }
            catch (System.Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetInsuredsByDescriptionInsuredSearchTypeCustomerType), ex);
            }
        }

        public List<PackagingTypeDTO> GetPackagingTypes()
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.GetPackagingTypes();
            }
            catch (System.Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetPackagingTypes), ex);
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
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.GetStatesByCountryId(countryId);
            }
            catch (System.Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetStatesByCountryId), ex);
            }
        }

        public List<TransportDTO> GetTransportsByTemporalId(int temporalId)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.GetTransportsByTemporalId(temporalId);
            }
            catch (System.Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetTransportsByTemporalId), ex);
            }
        }

        public List<TransportTypeDTO> GetTransportTypes()
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.GetTransportTypes();
            }
            catch (System.Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetTransportTypes), ex);
            }
        }

        public List<ViaTypeDTO> GetViaTypes()
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.GetViaTypes();
            }
            catch (System.Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetTransportTypes), ex);
            }
        }
        public List<CoverageDTO> QuotateCoverages(List<CoverageDTO> coverages, TransportDTO TransportDTO, bool runRulesPre, bool runRulesPost)
        {
            List<CoverageDTO> dto = new List<CoverageDTO>();
            foreach (var coverage in coverages)
            {
                CoverageDTO coverageDTO = new CoverageDTO();
                coverageDTO = QuotateCoverage(coverage, TransportDTO, runRulesPre, runRulesPost);
                coverageDTO.CoverageId = coverage.CoverageId;
                coverageDTO.Id = coverage.Id;
                dto.Add(coverageDTO);
            }
            return dto;
        }
        public CoverageDTO QuotateCoverage(CoverageDTO CoverageDTO, TransportDTO TransportDTO, bool runRulesPre, bool runRulesPost)
        {
            try
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                return coverageBusiness.QuotateCoverage(CoverageDTO, TransportDTO, runRulesPre, runRulesPost);
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

        public TransportDTO SaveTransport(int temporalId, TransportDTO transportDTO)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.SaveTransport(temporalId,transportDTO);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateCompanyTransportTemporal), ex);
            }
        }

        public List<TransportDTO> GetTransportsByPolicyId(int policyId)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.GetTransportsByPolicyId(policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetTransportsByPolicyId), ex);
            }
        }

        public TransportDTO GetTransportByRiskId(int riskId)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.GetTransportByRiskId(riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCompanyTransportByRiskId), ex);
            }
        }

        public PolicyTypeDTO GetPolicyTypeByPolicyTypeIdPrefixId(int policyTypeId, int prefixId)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.GetPolicyTypeByPolicyTypeIdPrefixId(policyTypeId, prefixId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetPolicyTypeByPrefixIdPolicyTypeCode), ex);
            }
        }

        public TransportDTO QuotateTransport(TransportDTO TransportDTO, bool runRulesPre, bool runRulesPost)
        {
            try
            {
                CoverageBusiness coverageBusiness = new CoverageBusiness();
                return coverageBusiness.QuotateTransport(TransportDTO, runRulesPre, runRulesPost);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorQuotateCompanyTransport), ex);
            }
        }

        public TransportDTO ExcludeTransport(int temporalId, int riskId)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.ExcludeTransport(temporalId, riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorExcludeTransport), ex);
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
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.SaveText(riskId, textDTO);
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
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.SaveClauses(temporalId, riskId, clauseDTOs);
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

                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.SaveBeneficiaries(temporalId, beneficiariesDTOs);

            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorSaveCompanyBeneficiary));
            }
        }

        public TransportDTO RunRulesRisk(TransportDTO transportDTO, int ruleId)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.RunRulesRisk(ModelAssembler.CreateTransport(transportDTO, new CompanyTransport()), ruleId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorRunRules));
            }
        }

        public TransportDTO SaveCoverages(int policyId, int riskId, List<CoverageDTO> coverages, int insuredObjectId)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.SaveCoverages(policyId, riskId, ModelAssembler.CreateCompanyCoverages(coverages), insuredObjectId);
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
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.CreateEndorsement(temporalId);
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
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.GetEndorsementByEndorsementTypeIdPolicyId(endorsementTypeId, policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetTransportsByPolicyId), ex);
            }
        }

        public List<TransportDTO> GetCompanyTransportsByPolicyIdEndorsementId(int policyId, int endorsementId)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.GetCompanyTransportsByPolicyIdEndorsementId(policyId, endorsementId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetTransportsByPolicyId), ex);
            }
        }

        public List<CoverageDTO> GetCoveragesByRiskId(int riskId, int temporalId)
        {
            List<CoverageDTO> coverageDTOs = new List<CoverageDTO>();
            return coverageDTOs = DTOAssembler.CreateCoveragesDtos(DelegateService.transportBusinessService.GetCoveragesByRiskId(riskId, temporalId));

        }


        public EndorsementDTO GetTemporalEndorsementByPolicyId(int policyId)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.GetTemporalEndorsementByPolicyId(policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetTransportsByPolicyId), ex);
            }
        }

        public List<CoverageDTO> GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(int policyId, int endorsementId, int riskId)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(policyId, endorsementId, riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetTransportsByPolicyId), ex);
            }
        }

        public TextDTO GetTextsByRiskId(int riskId)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.GetTextsByRiskId(riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetTransportsByTemporalId), ex);
            }
        }
        public bool GetLeapYear()
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.GetLeapYear();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public bool DeleteCompanyRisk(int temporalId, int riskId)
        {
            TransportBusiness transportBusiness = new TransportBusiness();
            return transportBusiness.DeleteCompanyRisk(temporalId, riskId);
        }

        public List<CoverageDTO> GetTemporalCoveragesByRiskIdInsuredObjectId(int riskId, int insuredObjectId)
        {
            TransportBusiness transportBusiness = new TransportBusiness();
            return transportBusiness.GetTemporalCoveragesByRiskIdInsuredObjectId(riskId, insuredObjectId);
        }

        public bool SaveInsuredObject(int riskId, InsuredObjectDTO insuredObjectDTO, int tempId, int groupCoverageId)
        {
            TransportBusiness transportBusiness = new TransportBusiness();
            return transportBusiness.saveInsuredObject(riskId, insuredObjectDTO, tempId, groupCoverageId);
        }

        public List<RiskCommercialClassDTO> GetRiskCommercialClasses(string description)
        {
            TransportBusiness transportBusiness = new TransportBusiness();
            return transportBusiness.GetRiskCommercialClasses(description);
        }

        public List<HolderTypeDTO> GetHolderTypes()
        {
            TransportBusiness transportBusiness = new TransportBusiness();
            return transportBusiness.GetHolderTypes();
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
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.CanMakeDeclarationEndorsement(policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }

        /// <summary>
        /// Indica si es posible hacer un endoso de Ajuste
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        public bool CanMakeAdjustmentEndorsement(int policyId)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.CanMakeAdjustmentEndorsement(policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public EndorsementDTO GetNextAdjustmentEndorsementByPolicyId(int policyId)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.GetNextAdjustmentEndorsementByPolicyId(policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetNextAdjustmentEndorsementByPolicyId), ex);
            }
        }

        public EndorsementDTO GetNextDeclarationEndorsementByPolicyId(int policyId)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.GetNextDeclarationEndorsementByPolicyId(policyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetNextDeclarationEndorsementByPolicyId), ex);
            }
        }

        public CoverageDTO CreateCoverageDTO(CompanyCoverage companyCoverage)
        {
            try
            {
                return DTOAssembler.CreateCoverageDTO(companyCoverage);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyCoverage CreateCompanyCoverage(CoverageDTO coverageDTO)
        {
            try
            {
                return ModelAssembler.CreateCompanyCoverage(coverageDTO);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public bool CanMakeEndorsement(int policyId, out Dictionary<string, object> endorsementValidate)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.CanMakeEndorsement(policyId, out endorsementValidate);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyCoverage> GetCoveragesByCoveragesAdd(int productId, int coverageGroupId, int prefixId, string coveragesAdd, int insuredObjectId)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.GetCoveragesByCoveragesAdd(productId, coverageGroupId, prefixId, coveragesAdd, insuredObjectId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public EndorsementPeriod SaveCompanyEndorsementPeriod(EndorsementPeriod companyEndorsementPeriod)
        {
            return DTOAssembler.CreateEndorsementPeriod(DelegateService.transportBusinessService.SaveCompanyEndorsementPeriod(ModelAssembler.CreateCompanyEndorsementPeriod(companyEndorsementPeriod)));
        }

        public bool CanMakeEndorsementByRiskByInsuredObjectId(decimal policyId, decimal riskId, decimal insuredObjectId, EndorsementType endorsementType)
        {

            return DelegateService.transportBusinessService.CanMakeEndorsementByRiskByInsuredObjectId(policyId, riskId, insuredObjectId, endorsementType);
        }
        public CompanyCoverage GetCoverageByCoverageId(int coverageId, int temporalId, int groupCoverageId)
        {
            try
            {
                TransportBusiness transportBusiness = new TransportBusiness();
                return transportBusiness.GetCoverageByCoverageId(coverageId, temporalId, groupCoverageId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }

}
