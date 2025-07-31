using Sistran.Company.Application.Location.PropertyServices.Models;
using Sistran.Company.Application.ProductServices.DTOs;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.DTOs;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.UniquePersonAplicationServices.DTOs;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Enums;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Framework.UIF.Web.App_GlobalResources;
using Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using static Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Enums.Enums;
using CTPLM = Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.Models;
using MOS = Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    using UNSE = Sistran.Company.Application.UnderwritingServices.Enums;
    public class SearchController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [NoDirectAccess]
        public ActionResult Search()
        {

            return PartialView();
        }

        public PartialViewResult AdvancedSearch()
        {
            return PartialView();
        }

        public ActionResult GetBranches()
        {
            try
            {
                List<Branch> branches = DelegateService.uniqueUserService.GetBranchesByUserId(SessionHelper.GetUserId());
                return new UifJsonResult(true, branches.OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryBranches);
            }
        }
        public ActionResult GetPrefixes()
        {
            try
            {
                List<Prefix> prefixes = DelegateService.commonService.GetPrefixes();
                return new UifJsonResult(true, prefixes.OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryPrefix);
            }
        }

        /// <summary>
        /// Buscar Policy
        /// </summary>
        /// <param name="prefixId"></param>
        /// <param name="branchId"></param>
        /// <param name="policyNumber"></param>
        /// <param name="current"></param>
        /// <returns></returns>
        public ActionResult GetEndorsementsByPrefixIdBranchIdPolicyNumber(int prefixId, int branchId, decimal policyNumber, bool current)
        {
            try
            {
                List<CompanyEndorsement> endorsements = DelegateService.underwritingService.GetCiaEndorsementsByFilterPolicy(branchId, prefixId, policyNumber, current);
                var selectedId = 0;

                if (endorsements != null)
                {
                    endorsements.Where(x => x.IsCurrent).AsParallel().ForAll(endorsement =>
                    {
                        endorsement.Description = App_GlobalResources.Language.ActualState;
                        selectedId = endorsement.Id;
                    });

                    return new UifJsonResult(true, new { endorsements, selectedId });
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.PolicyNotFound);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryEndorsement);
            }
        }

        public ActionResult GetListEndorsement(int branchId, int prefixId, decimal policyNumber)
        {
            try
            {
                List<MOS.Endorsement> endorsements = DelegateService.underwritingService.GetEndorsementsByPrefixIdBranchIdPolicyNumber(prefixId, branchId, policyNumber);
                if (endorsements != null)
                {
                    return new UifJsonResult(true, endorsements);
                }
                else
                {
                    return new UifJsonResult(false, ViewModelError.GetMessages(ModelState.Values));
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorPolicyNoExist);
            }
        }


        public ActionResult GetEndorsementsByPrefixIdBranchIdPolicyNumberCompany(int branchId, int prefixId, decimal policyNumber)
        {
            try
            {
                List<EndorsementCompanyDTO> endorsements = DelegateService.underwritingService.GetEndorsementsByPrefixIdBranchIdPolicyNumberCompany(branchId, prefixId, policyNumber);
                if (endorsements != null)
                {
                    return new UifJsonResult(true, endorsements);
                }
                else
                {
                    return new UifJsonResult(false, ViewModelError.GetMessages(ModelState.Values));
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorPolicyNoExist);
            }
        }

        public ActionResult GetEndorsementPolicyInformation(int endorsementId, bool isCurrent = false)
        {
            try
            {
                CompanyPolicy companyPolicy = DelegateService.underwritingService.GetEndorsementInformation(endorsementId, isCurrent);
                companyPolicy.Endorsement.EndorsementTypeDescription = EnumsHelper.GetItemName<EndorsementType>(companyPolicy.Endorsement.EndorsementType);

                return new UifJsonResult(true, companyPolicy);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorInformationEndorsement);
            }
        }

        public ActionResult GetCurrentStatusPolicyByEndorsementIdIsCurrent(int endorsementId, bool isCurrent)
        {
            try
            {
                CompanyPolicy policy = DelegateService.underwritingService.GetCiaCurrentStatusPolicyByEndorsementIdIsCurrentCompany(endorsementId, isCurrent);

                if (policy != null)
                {
                    policy = GetDataPolicy(policy);
                    return new UifJsonResult(true, policy);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.NoInformationEndorsement);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorInformationEndorsement);
            }
        }

        /// <summary>
        /// Gets the data policy.
        /// </summary>
        /// <param name="policy">The policy.</param>
        /// <returns></returns>
        private CompanyPolicy GetDataPolicy(CompanyPolicy policy)
        {
            policy.BusinessTypeDescription = App_GlobalResources.Language.ResourceManager.GetString(EnumsHelper.GetItemName<BusinessType>(policy.BusinessType));
            if (policy.BusinessTypeDescription == null)
            {
                policy.BusinessTypeDescription = EnumsHelper.GetItemName<BusinessType>(policy.BusinessType);
            }
            policy.Endorsement.EndorsementTypeDescription = App_GlobalResources.Language.ResourceManager.GetString(EnumsHelper.GetItemName<EndorsementType>(policy.Endorsement.EndorsementType));
            if (policy.Endorsement.EndorsementTypeDescription == null)
            {
                policy.Endorsement.EndorsementTypeDescription = EnumsHelper.GetItemName<EndorsementType>(policy.Endorsement.EndorsementType);
            }
            if (policy.Endorsement.EndorsementType == EndorsementType.Cancellation || policy.Endorsement.EndorsementType == EndorsementType.Nominative_cancellation)
            {
                policy.Endorsement.Description = App_GlobalResources.Language.LabelEndorsementCanceled;
            }
            else
            {
                if (policy.Endorsement.CurrentTo < DateTime.Now.Date)
                    policy.Endorsement.Description = App_GlobalResources.Language.LabelEndorsementExpired;
                else
                    policy.Endorsement.Description = App_GlobalResources.Language.LabelEndorsementValid;
            }
            
            List<Email> emails = DelegateService.uniquePersonServiceCore.GetEmailsByIndividualId(policy.Holder.IndividualId);
            if (emails.Count > 0)
            {
                foreach (Email email in emails)
                {
                    if (email.IsPrincipal == false)
                    {
                        policy.Holder.Email = email.Description;
                    }
                }
            }

            List<InsuredFiscalResponsibility> fiscal = DelegateService.uniquePersonServiceCore.GetFiscalResponsibilityByIndividualId(policy.Holder.IndividualId);
            if (fiscal.Count > 0)
            {
                policy.Holder.FiscalResponsibility = fiscal;
            }

            InsuredDTO insured = DelegateService.uniquePersonAplicationService.GetAplicationInsuredByIndividualId(policy.Holder.IndividualId);
            if (insured != null)
            {
                policy.Holder.RegimeType = insured.RegimeType;
            }


            return policy;
        }

        public ActionResult GetEndorsementControllerByPrefixIdProductIdEndorsementType(int prefixId, int productId, string endorsementType)
        {
            try
            {
                SubCoverageDTO subCoverageDTO = DelegateService.productService.GetCompanySubCoverageRiskTypeByProductIdPrefixId(productId, prefixId);
                return new UifJsonResult(true, subCoverageDTO.Id);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryEndorserDriver);
            }
        }

        public ActionResult GetRiskByPolicyIdByRiskDescription(int policyId, int endorsementId, string riskDescription)
        {
            try
            {

                CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByEndorsementId(endorsementId);
                companyPolicy.Product = DelegateService.productService.GetCompanyProductByProductIdPrefixId(companyPolicy.Product.Id, companyPolicy.Prefix.Id);
                switch (companyPolicy.Product.CoveredRisk.SubCoveredRiskType.Value)
                {
                    case SubCoveredRiskType.ThirdPartyLiability:
                        List<CTPLM.CompanyTplRisk> companyTplRisks = DelegateService.thirdPartyLiabilityService.GetThirdPartyLiabilityPolicyByPolicyIdEndorsementIdlicensePlate(policyId, 0, riskDescription);
                        if (companyTplRisks != null && companyTplRisks.Count > 0)
                        {
                            return new UifJsonResult(true, companyTplRisks[0]);
                        }
                        break;
                    case SubCoveredRiskType.Vehicle:
                        List<CompanyVehicle> companyVehicles = DelegateService.vehicleService.GetVehiclesByPolicyIdEndorsementIdLicensePlate(policyId, 0, riskDescription);
                        if (companyVehicles != null && companyVehicles.Count > 0)
                        {
                            return new UifJsonResult(true, companyVehicles[0]);
                        }
                        break;
                    case SubCoveredRiskType.Property:
                        int riskNumber = 0;
                        bool isConvert = Int32.TryParse(riskDescription, out riskNumber);
                        if (isConvert)
                        {
                            List<CompanyPropertyRisk> companyPropertyRisk = DelegateService.propertyService.GetPropertiesByPolicyIdByRiskIdList(policyId, new List<int>() { riskNumber });
                            if (companyPropertyRisk != null && companyPropertyRisk.Count > 0)
                            {
                                return new UifJsonResult(true, companyPropertyRisk[0]);
                            }
                        }
                        else
                        {
                            return new UifJsonResult(false, "Debe Ingresar el numero de riesgo");
                        }
                        break;
                }

                return new UifJsonResult(false, App_GlobalResources.Language.PlateNotExistEndorsementOrPolicy);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryPlate);

            }
        }

        public ActionResult GetPolicyRenewal(RenewalViewModel renewalModel, int temporalId = 0)
        {
            try
            {
                var mapper = ModelAssembler.CreateMapCompanyEndorsement();
                CompanyEndorsement endorsement = mapper.Map<MOS.Endorsement, CompanyEndorsement>(DelegateService.endorsementBaseService.GetTemporalEndorsementByPolicyId(renewalModel.PolicyId.Value));
                if (endorsement == null)
                {
                    renewalModel.IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Today).ToString(DateHelper.FormatFullDate);
                    renewalModel.ModifyFrom = Convert.ToDateTime(renewalModel.CurrentTo);
                    renewalModel.ModifyTo = Convert.ToDateTime(renewalModel.CurrentTo).AddYears(1);
                    renewalModel.TemporalId = temporalId;
                    return new UifJsonResult(true, renewalModel);
                }
                else if (endorsement.EndorsementType == EndorsementType.Renewal)
                {
                    renewalModel.IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Today).ToString(DateHelper.FormatFullDate);
                    renewalModel.TemporalId = endorsement.TemporalId;
                    return new UifJsonResult(true, renewalModel);
                }
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryRenewal);
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryRenewal);
            }

        }

        public ActionResult GetPrefixEndoEnabledByPrefixId(int prefixId)
        {
            try
            {
                List<PrefixEndoTypeEnabled> prefixEndoTypesEnabled = new List<PrefixEndoTypeEnabled>();
                prefixEndoTypesEnabled = DelegateService.underwritingService.GetPrefixEndoEnabledByPrefixIdIsEnabled(prefixId, true);

                if (prefixEndoTypesEnabled != null)
                {
                    return new UifJsonResult(true, prefixEndoTypesEnabled);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.NoFoundInformationEndorsementBranch);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryInformationBranch);
            }
        }
        public ActionResult GetHoldersByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType customerType)
        {
            try
            {
                List<Holder> holders = DelegateService.underwritingService.GetHoldersByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType);

                if (holders.Count == 0)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.MessageSearchHolders);
                }
                else if (holders.Count == 1)
                {
                    if (holders.Exists(x => x.DeclinedDate > DateTime.MinValue))
                    {
                        return new UifJsonResult(false, App_GlobalResources.Language.ErrorPolicyholderDisabled);
                    }
                    else
                    {
                        return new UifJsonResult(true, holders);
                    }
                }
                else
                {
                    return new UifJsonResult(true, holders);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchHolders);
            }
        }
        /// <summary>
        /// Busqueda avanzada
        /// </summary>
        /// <param name="policy"></param>
        /// <returns></returns>
        public ActionResult GetPoliciesByPolicy(CompanyPolicy policy)
        {
            try
            {
                List<CompanyPolicy> policies = DelegateService.underwritingService.GetCiaPoliciesByPolicy(policy);
                if (policies != null && policies.Any())
                {
                    policies.Where(x => x.Endorsement != null).AsParallel().ForAll(policie =>
                    {
                        policie.Endorsement.EndorsementTypeDescription = EnumsHelper.GetItemName<EndorsementType>(policie.Endorsement.EndorsementType);
                    });

                    return new UifJsonResult(true, policies.OrderBy(b => b.Endorsement.CurrentFrom).ToList());
                }
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorTempNoExist);

            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchQuotation);
            }
        }

        

        /// <summary>
        /// Validar Coverturas Con Post Contractuales
        /// </summary>
        /// <param name="Policyid"></param>
        /// <returns></returns>
        public UifJsonResult ValidateCoveragePostContractual(int Policyid)
        {
            System.Collections.ArrayList coverPost;
            try
            {
                coverPost = DelegateService.underwritingService.ValidateCoveragePostContractual(Policyid);
                return new UifJsonResult(true, coverPost);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, "nO SE PUEDE CONSULTAR LAS Coberturas Post Contractuales");
            }
        }

        public ActionResult GetParameterIvaById()
        {
            try
            {
                Parameter parameter = DelegateService.commonService.GetParameterByParameterId((int)ParameterEnum.IvaParameter);
                return new UifJsonResult(true, parameter);
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchParameter);
            }

        }

        public ActionResult GetParameterUniquePolicy()
        {
            try
            {
                Parameter parameter = DelegateService.commonService.GetParameterByParameterId((int)ParameterEnum.UniquePolicy);
                return new UifJsonResult(true, parameter);
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchParameter);
            }

        }

        public ActionResult CanMakeDeclarationEndorsement(int policyId)
        {
            try
            {
                bool canMakeDeclarationEndorsement = DelegateService.transportApplicationService.CanMakeDeclarationEndorsement(policyId);
                return new UifJsonResult(true, canMakeDeclarationEndorsement);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, false);
            }
        }

        public ActionResult CanMakeAdjustmentEndorsement(int policyId)
        {
            try
            {
                bool canMakeAdjustmentEndorsement = DelegateService.transportApplicationService.CanMakeAdjustmentEndorsement(policyId);
                return new UifJsonResult(true, canMakeAdjustmentEndorsement);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, false);
            }
        }

        public ActionResult DeleteEndorsementControl(int EndorsementId)
        {
            try
            {
                if (EndorsementId != 0)
                {
                    DelegateService.utilitiesServiceCore.DeleteEndorsementControl(EndorsementId);
                }

                return new UifJsonResult(true, true);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, false);
            }
        }

        public ActionResult ValidateDeleteTemporal(int EndorsementId)
        {
            try
            {
                int UserId = SessionHelper.GetUserId();
                if (DelegateService.utilitiesServiceCore.GetEndorsementControlById(EndorsementId, UserId))
                {
                    DelegateService.utilitiesServiceCore.DeleteEndorsementControl(EndorsementId);
                    return new UifJsonResult(true, App_GlobalResources.Language.PolicyInEdition);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.PolicyInEdition);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.ToString());
            }
        }

        public ActionResult GetValidateOriginPolicy(decimal documentNumber, int prefixId, int branchId)
        {
            try
            {
                TemporalDTO temporalDTO = new TemporalDTO { Id = 1 };
                temporalDTO = DelegateService.underwritingService.GetTemporalByDocumentNumberPrefixIdBrachId(documentNumber, prefixId, branchId);
                if (temporalDTO.Id == 0)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ValidateOriginPolicy);
                }
                else if (temporalDTO.Id == 1)
                {
                    temporalDTO.Id = 0;
                }

                return new UifJsonResult(true, temporalDTO);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.ToString());
            }
        }

        #region optimizacion
        public ActionResult GetTemporalByPolicyIdEndorsementId(int policyId, int endorsementId)
        {
            try
            {
                TemporalDTO temporalDTO = new TemporalDTO { Id = 1 };
                temporalDTO = DelegateService.underwritingService.GetTemporalByPolicyIdEndorsementId(policyId, endorsementId);
                if (temporalDTO.Id == 0)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ValidateOriginPolicy);
                }
                else if (temporalDTO.Id == 1)
                {
                    temporalDTO.Id = 0;
                }

                return new UifJsonResult(true, temporalDTO);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.ToString());
            }
        }

        public ActionResult GetCompanyEndorsementsByFilterPolicy(int prefixId, int branchId, decimal policyNumber, bool current = true)
        {
            try
            {
                List<Sistran.Company.Application.UnderwritingServices.DTOs.EndorsementDTO> endorsements = DelegateService.underwritingService.GetCompanyEndorsementsByFilterPolicy(branchId, prefixId, policyNumber, current);
                if (endorsements != null)
                {
                    if (current)
                    {
                        endorsements.First().Description = App_GlobalResources.Language.ActualState;
                    }
                    return new UifJsonResult(true, new { endorsements, endorsements.FirstOrDefault().Id });
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.PolicyNotFound);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryEndorsement);
            }
        }
        public ActionResult GetCurrentPolicyByEndorsementId(int endorsementId, bool isCurrent)
        {
            try
            {
                CompanyPolicy policy = DelegateService.underwritingService.GetCurrentPolicyByEndorsementId(endorsementId, isCurrent);

                if (policy != null)
                {
                    policy = GetDataPolicy(policy);
                    return new UifJsonResult(true, policy);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.NoInformationEndorsement);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorInformationEndorsement);
            }
        }
        #region optimizacion
        public ActionResult ValidateEndorsement(PolicyBaseViewModel policyBaseModel)
        {
            try
            {
                PolicyResultViewModel policyResultViewModel = new PolicyResultViewModel();
                policyResultViewModel.Id = 0;
                policyResultViewModel.Message = "";
                if (policyBaseModel != null && policyBaseModel.PolicyId > 0)
                {
                    if ((EndorsementType)policyBaseModel.EndorsementType == EndorsementType.LastEndorsementCancellation)
                    {
                        var result = ValidateDataEndorsement(policyBaseModel);
                        return new UifJsonResult(true, result);
                    }
                    else if ((EndorsementType)policyBaseModel.EndorsementTypeOriginal != EndorsementType.Cancellation && (EndorsementType)policyBaseModel.EndorsementType != EndorsementType.AutomaticCancellation)
                    {
                        if (policyBaseModel?.Id > 0)
                        {
                            var result = ValidateDataEndorsement(policyBaseModel);
                            return new UifJsonResult(true, result);
                        }
                        return new UifJsonResult(true, policyResultViewModel);
                    }
                    else
                    {
                        policyResultViewModel.Message = App_GlobalResources.Language.NoCanCancellationModified;
                    }
                    return new UifJsonResult(false, policyResultViewModel.Message);

                }
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryEndorsementReasons);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryEndorsementReasons);
            }
        }
        private PolicyResultViewModel ValidateDataEndorsement(PolicyBaseViewModel policyBaseModel)
        {
            PolicyResultViewModel policyResultViewModel = new PolicyResultViewModel();
            policyResultViewModel.Id = 0;
            policyResultViewModel.Message = "";
            policyResultViewModel.HasEvent = false;
            if (policyBaseModel?.Id > 0)
            {
                policyResultViewModel.Id = policyBaseModel.Id;
                if (!DelegateService.utilitiesServiceCore.GetEndorsementControlById(policyBaseModel.EndorsementId, SessionHelper.GetUserId()))
                {
                    policyResultViewModel.Message = App_GlobalResources.Language.PolicyInEdition;
                }
                else if (policyBaseModel.EndorsementTemporal == (int)policyBaseModel.EndorsementType)
                {
                    var authorizationRequests = DelegateService.AuthorizationPoliciesService.GetAuthorizationRequestsByKey(policyBaseModel.Id.ToString());

                    if (authorizationRequests.Any(x => x.Status == TypeStatus.Authorized) && authorizationRequests.All(x => x.Status != TypeStatus.Rejected))
                    {
                        if (authorizationRequests.Count(x => x.Status == TypeStatus.Authorized) == authorizationRequests.Count)
                        {
                            policyResultViewModel.HasEvent = true;
                            policyResultViewModel.Message = string.Format(Language.MessageModifyAcceptPolicies, authorizationRequests.Count);
                        }
                        else
                        {
                            var countAuthorized = authorizationRequests.Count(x => x.Status == TypeStatus.Authorized);
                            var countPending = authorizationRequests.Count(x => x.Status == TypeStatus.Pending);
                            policyResultViewModel.HasEvent = true;
                            policyResultViewModel.Message = string.Format(Language.MessageModifyPendingAcceptPolicies, countPending, countAuthorized);
                        }
                    }
                    else
                    {
                        if (authorizationRequests.Any(x => x.Status == TypeStatus.Rejected))
                        {
                            policyResultViewModel.HasEvent = true;
                            policyResultViewModel.Message = string.Format(Language.MessageModifyRejectedPolicies, authorizationRequests.Count(x => x.Status == TypeStatus.Rejected));
                        }
                        else if (authorizationRequests.Any(x => x.Status == TypeStatus.Pending))
                        {
                            policyResultViewModel.HasEvent = true;
                            policyResultViewModel.Message = string.Format(Language.MessageModifyPendingPolicies, authorizationRequests.Count);
                        }
                        policyResultViewModel.IssueDate = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Today);
                    }
                    if (policyBaseModel.ModificationEndorsementType == "ModificationText" || policyBaseModel.ModificationEndorsementType == "ModificationClauses")
                    {
                        var endorsemenType = EnumsHelper.GetItemName<EndorsementType>(policyBaseModel.EndorsementType);
                        if (endorsemenType == null)
                        {
                            endorsemenType = "";
                        }
                        if (policyBaseModel.EndorsementType == (byte)EndorsementType.Modification)
                        {
                            if (policyResultViewModel.Message.Length > 0)
                            {
                                policyResultViewModel.Message += "\n\n" + $"{App_GlobalResources.Language.PolicyTemporal} : {policyBaseModel?.Id} - {EnumsHelper.GetItemName<EndorsementType>(policyBaseModel.EndorsementTemporal)}  {App_GlobalResources.Language.NotIs} {endorsemenType }";
                            }
                            else
                            {
                                policyResultViewModel.Message = $"{App_GlobalResources.Language.PolicyTemporal} : {policyBaseModel?.Id} - {EnumsHelper.GetItemName<EndorsementType>(policyBaseModel.EndorsementTemporal)}  {App_GlobalResources.Language.NotIs} {endorsemenType }";
                            }
                        }
                    }
                }
                else
                {
                    var endorsemenType = EnumsHelper.GetItemName<EndorsementType>(policyBaseModel.EndorsementType);
                    if (endorsemenType == null)
                    {
                        endorsemenType = "";
                    }
                    policyResultViewModel.Message = $"{App_GlobalResources.Language.PolicyTemporal} : {policyBaseModel?.Id} - {EnumsHelper.GetItemName<EndorsementType>(policyBaseModel.EndorsementTemporal)}  {App_GlobalResources.Language.NotIs} {endorsemenType }";
                }
            }
            return policyResultViewModel;
        }
        #endregion
        #endregion optimizacion

    }
}