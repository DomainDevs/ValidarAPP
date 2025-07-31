using Sistran.Core.Application.ClaimServices.DTOs.Claims;
using Sistran.Core.Application.ClaimServices.DTOs.PaymentRequest;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.UIF.Web.Areas.Claims.Business;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace Sistran.Core.Framework.UIF.Web.Areas.Claims.Controllers
{
    public class ClaimController : Controller
    {
        private static List<Sistran.Core.Application.CommonService.Models.Parameter> parameters = new List<Sistran.Core.Application.CommonService.Models.Parameter>();

        public ActionResult MasterClaim()
        {
            return View();
        }

        public UifJsonResult GetClaimByClaimId(int claimId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetClaimByClaimId(claimId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetClaimByClaimId);
            }
        }

        public UifJsonResult GetBranches()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetBranches());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetBranchs);
            }
        }

        public UifJsonResult GetCausesByPrefixId(int prefixId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetCausesByPrefixId(prefixId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetCausesByPrefixId);
            }
        }

        public UifJsonResult GetEndorsementByPrefixIdBranchIdCoveredRiskTypeIdPolicyNumber(int? prefixId, int? branchId, CoveredRiskType coveredRiskTypeId, decimal policyNumber, DateTime claimDate)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetEndorsementByPrefixIdBranchIdCoveredRiskTypeIdDocumentNumber(prefixId, branchId, coveredRiskTypeId, policyNumber, claimDate));
            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetEndorsementByPrefixIdBranchIdPolicyNumber);
            }
        }

        public UifJsonResult GetPolicyByEndorsementIdModuleType(int endorsementId)
        {
            try
            {
                PolicyDTO policyDTO = DelegateService.claimApplicationService.GetPolicyByEndorsementIdModuleType(endorsementId);
                if (!DelegateService.commonService.GetParameterByParameterId(12191).BoolParameter.GetValueOrDefault())
                {
                    policyDTO.IsReinsurance = ClaimBusiness.GetPolicyReinsurance2G(policyDTO);
                }

                return new UifJsonResult(true, policyDTO);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPolicyByEndorsementIdModuleType);
            }
        }

        public UifJsonResult GetDamageTypes()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetDamageTypes());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetDamageTypes);
            }
        }

        public UifJsonResult GetDamageResponsibilities()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetDamageResponsibilities());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetDamageResponsibilities);
            }
        }

        public UifJsonResult GetAnalyst()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetAnalizers());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetAnalyst);
            }
        }

        public UifJsonResult GetAdjuster()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetAdjusters());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetAdjuster);
            }
        }

        public UifJsonResult GetInvestigator()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetResearchers());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetInvestigator);
            }
        }

        public UifJsonResult GetHourInspection()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetResearchers());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetHourInspection);
            }
        }

        public UifJsonResult GetCountries()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetCountries());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetCountries);
            }
        }

        public UifJsonResult GetStatesByCountryId(int countryId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetStatesByCountryId(countryId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetStatesByCountryId);
            }
        }

        public UifJsonResult GetCitiesByCountryIdStateId(int countryId, int stateId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetCitiesByCountryIdStateId(countryId, stateId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetCitiesByCountryIdStateId);
            }
        }

        public JsonResult GetCatastrophesByDescription(string query)
        {
            try
            {
                return Json(DelegateService.claimApplicationService.GetCatastrophesByDescription(query), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(App_GlobalResources.Language.ErrorGetCatastrophesByDescription, JsonRequestBehavior.DenyGet);
            }
        }

        public UifJsonResult GetAccountingDate()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetModuleDateByModuleTypeMovementDate(ModuleType.Claim, DateTime.Now));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetAccountingDate);
            }
        }

        public UifJsonResult GetCurrency()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.paymentRequestApplicationService.GetCurrencies());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetCurrency);
            }
        }

        public UifJsonResult GetDocumentTypeByIndividualType(int individualType)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetDocumentTypesByIndividualType(individualType));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetDocumentTypeByIndividualType);
            }
        }

        public UifJsonResult GetGender()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetGenders());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetGender);
            }
        }

        public UifJsonResult GetMaritalStatus()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetMaritalStatus());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetMaritalStatus);
            }
        }

        public JsonResult GetUserByFullName(string query)
        {
            try
            {
                return Json(DelegateService.claimApplicationService.GetUserByName(query), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(App_GlobalResources.Language.ErrorGetUserByFullName, JsonRequestBehavior.DenyGet);
            }
        }

        public JsonResult GetInsuredsByDescriptionInsuredSearchTypeCustomerType(string query)
        {
            try
            {
                return Json(DelegateService.claimApplicationService.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(query, Core.Services.UtilitiesServices.Enums.InsuredSearchType.DocumentNumber, Core.Services.UtilitiesServices.Enums.CustomerType.Individual), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(App_GlobalResources.Language.ErrorGetInsuredsByDescriptionInsuredSearchTypeCustomerType, JsonRequestBehavior.DenyGet);
            }
        }

        public JsonResult GetHoldersByDescriptionInsuredSearchTypeCustomerType(string query)
        {
            try
            {
                return Json(DelegateService.claimApplicationService.GetHoldersByDescriptionInsuredSearchTypeCustomerType(query, InsuredSearchType.DocumentNumber, CustomerType.Individual), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(App_GlobalResources.Language.ErrorGetHoldersByName, JsonRequestBehavior.DenyGet);
            }
        }

        public UifJsonResult GetClaimSupplierByClaimId(int claimId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetClaimSupplierByClaimId(claimId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetClaimSupplierByClaimId);
            }
        }

        public UifJsonResult GetEstimationTypesByPrefixId(int prefixId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetEstimationsByPrefixId(prefixId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetEstimationTypesByPrefixId);
            }
        }

        public UifJsonResult GetClaimCatastrophicInformationByClaimId(int claimId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetClaimCatastrophicInformationByClaimId(claimId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetClaimCatastrophicInformationByClaimId);
            }
        }

        public UifJsonResult GetStatusesByEstimationTypeId(int estimationTypeId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetStatusesByEstimationTypeId(estimationTypeId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetStatusesByEstimationTypeId);
            }
        }

        public UifJsonResult GetReasonsByStatusIdPrefixId(int statusId, int prefixId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetReasonsByStatusIdPrefixId(statusId, prefixId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetReasonsByStatusId);
            }
        }

        public UifJsonResult GetCoveragesByRiskIdOccurrenceDateCompanyParticipationPercentage(int riskId, DateTime occurrenceDate, decimal companyParticipationPercentage)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetCoveragesByRiskIdOccurrenceDateCompanyParticipationPercentage(riskId, occurrenceDate, companyParticipationPercentage));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetCompanyCoveragesByRiskId);
            }
        }

        public UifJsonResult GetActivePanelsByCoverageId(int coverageId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetActivePanelsByCoverageId(coverageId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetActivePanelsByCoverageId);
            }
        }

        public UifJsonResult GetCoverageDeductibleByCoverageId(int coverageId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetCoverageDeductibleByCoverageId(coverageId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetCoverageDeductibleByCoverageId);
            }
        }

        public UifJsonResult GetEstimationByClaimId(int claimId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetEstimationByClaimId(claimId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetEstimationByClaimId);
            }
        }

        public UifJsonResult GetEstimationTypesByClaimModifyIdPrefixIdCoverageIdIndividualId(int claimModifyId, int prefixId, int coverageId, int individualId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetEstimationTypesByClaimModifyIdPrefixIdCoverageIdIndividualId(claimModifyId, prefixId, coverageId, individualId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetEstimationTypesByClaimModifyIdPrefixIdCoverageId);
            }
        }

        public UifJsonResult GetClaimsByPolicyId(int policyId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetClaimsByPolicyId(policyId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetClaimsByPolicyId);
            }
        }


        public UifJsonResult GetClaimsByPolicyIdOccurrenceDate(int policyId, DateTime occurrenceDate)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetClaimsByPolicyIdOccurrenceDate(policyId, occurrenceDate));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetClaimsByPolicyId);
            }
        }

        public UifJsonResult GetThirdAffectedByClaimCoverageId(int claimCoverageId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetThirdAffectedByClaimCoverageId(claimCoverageId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetClaimCoverageThirdAffectedByCoverage);
            }
        }


        public JsonResult GetThirdPartyByDescriptionInsuredSearchType(string query)
        {
            try
            {
                return Json(DelegateService.claimApplicationService.GetThirdPartyByDescriptionInsuredSearchType(query, InsuredSearchType.DocumentNumber), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(App_GlobalResources.Language.ErrorGetThirdPartyVehicle, JsonRequestBehavior.DenyGet);
            }
        }

        public UifJsonResult GetThirdPartyByIndividualId(string individualId)
        {
            try
            {
                //TODO: Ajuste agregado por que en NASE los terceros son creados como proveedores
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetSuppliersByDescriptionInsuredSearchTypeCustomerType(individualId, InsuredSearchType.IndividualId, CustomerType.Individual).FirstOrDefault());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetThirdPartyVehicle);
            }
        }

        public JsonResult GetParticipantsByDescriptionInsuredSearchTypeCustomerType(string query)
        {
            try
            {
                return Json(DelegateService.claimApplicationService.GetParticipantsByDescriptionInsuredSearchTypeCustomerType(query, InsuredSearchType.DocumentNumber, CustomerType.Individual), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(App_GlobalResources.Language.ErrorGetInsuredByNameDocumentNumber, JsonRequestBehavior.DenyGet);
            }
        }

        public UifJsonResult CreateParticipant(ClaimParticipantDTO claimParticipantDTO)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.CreateParticipant(claimParticipantDTO));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetClaimCoverageThirdAffectedByCoverage);
            }
        }

        public UifJsonResult GetParticipantByParticipantId(int participantId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetParticipantByParticipantId(participantId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorDeleteCauseCoverage);

            }
        }

        /// <summary>
        /// GetStatuses
        /// Carga la tabla Listado de Estados
        /// </summary>
        /// <returns>JsonResult</returns>
        public ActionResult GetStatuses()
        {
            List<StatusDTO> listStatus = DelegateService.claimApplicationService.GetStatuses();

            return new UifJsonData(true, listStatus);
        }

        public ActionResult GetReasonsByPrefixId(int prefixId)
        {
            try
            {
                List<ReasonDTO> reasonDTOs = DelegateService.claimApplicationService.GetReasonsByPrefixId(prefixId);
                return new UifJsonData(true, reasonDTOs);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorDeleteCauseCoverage);

            }
        }

        public UifJsonResult GetClaimDriverInformationByClaimCoverageId(int claimCoverageId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetClaimDriverInformationByClaimCoverageId(claimCoverageId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetClaimDriverInformationByClaimIdCoverageId);
            }
        }

        public UifJsonResult GetClaimThirdPartyVehicleByClaimCoverageId(int claimCoverageId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetClaimThirdPartyVehicleByClaimCoverageId(claimCoverageId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetClaimThirdPartyVehicleByClaimCoverageId);
            }
        }

        public UifJsonResult GetAffectedPropertyByClaimCoverageId(int claimCoverageId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetAffectedPropertyByClaimCoverageId(claimCoverageId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetClaimCoverageByClaimModifyIdCoverageId);
            }
        }

        public UifJsonResult GetClaimedAmountByClaimCoverageId(int claimCoverageId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetClaimedAmountByClaimCoverageId(claimCoverageId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetClaimCoverageByClaimModifyIdCoverageId);
            }
        }

        public UifJsonResult GetAmountType()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetAmountType());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetAmountType);
            }
        }

        public UifJsonResult GetMinimumSalaryByYear(int year)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetMinimumSalaryByYear(year));
            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetAmountType);
            }
        }

        public UifJsonResult GetJudicialDecisionDateIsActiveByPrefixId(int prefixId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetJudicialDecisionDateIsActiveByPrefixId(prefixId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetJudicialDecisionDateIsActiveByPrefixId);
            }
        }

        public JsonResult GetPaymentBeneficiariesByDescription(string query)
        {
            try
            {
                return Json(DelegateService.claimApplicationService.GetPaymentBeneficiariesByDescription(query), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(App_GlobalResources.Language.ErrorGetPaymentBeneficiaries, JsonRequestBehavior.DenyGet);
            }
        }

        public UifJsonResult GetPaymentBeneficiaryByBeneficiaryId(int beneficiaryId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetPaymentBeneficiaryByBeneficiaryId(beneficiaryId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPaymentBeneficiaries);
            }
        }

        public UifJsonResult GetEstimationTypesSalariesEstimation()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetEstimationTypesSalariesEstimation());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetEstimationTypesSalariesEstimation);
            }
        }

        public UifJsonResult GetInsuredsByIndividualId(string individualId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetAffectedByDescriptionInsuredSearchTypeCustomerType(individualId, Core.Services.UtilitiesServices.Enums.InsuredSearchType.IndividualId, Core.Services.UtilitiesServices.Enums.CustomerType.Individual));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetInsuredsByIndividualId);
            }
        }

        public UifJsonResult GetDefaultCountry()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.commonService.GetParameterByDescription("DefaultCountryId").NumberParameter);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSetDefaultCountry);
            }

        }

        public ActionResult GetExchangeRate(int currencyId)
        {
            try
            {
                if (!parameters.Any())
                {
                    parameters = this.GetParameters();
                }

                if (currencyId != (int)parameters.First(x => x.Description == "Currency").Value)
                {
                    return new UifJsonResult(true, DelegateService.paymentRequestApplicationService.GetExchangeRateByRateDateCurrencyId(DateTime.Now, currencyId));
                }
                else
                {
                    return new UifJsonResult(true, new ExchangeRateDTO
                    {
                        Currency = new CurrencyDTO
                        {
                            Id = currencyId
                        },
                        RateDate = DateTime.Now,
                        SellAmount = 1
                    });
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetExchangeRate);
            }
        }

        private List<Sistran.Core.Application.CommonService.Models.Parameter> GetParameters()
        {
            parameters.Add(new Sistran.Core.Application.CommonService.Models.Parameter { Description = "Currency", Value = DelegateService.commonService.GetParameterByParameterId((int)ParametersTypes.Currency).NumberParameter.Value });

            return parameters;
        }
    }
}