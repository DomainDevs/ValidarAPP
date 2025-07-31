using Sistran.Company.Application.ExternalProxyServices.Models;
using Sistran.Core.Application.ClaimServices.DTOs.Recovery;
using Sistran.Core.Framework.UIF.Web.Areas.Claims.Business;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Application.ClaimServices.DTOs.Claims;
using System;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Claims.Controllers
{
    public class RecoveryController : Controller
    {
        // GET: Claims/Recovery
        public ActionResult Recovery()
        {
            return View();
        }

        public UifJsonResult GetBranches()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetBranches());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetBranches);
            }
        }

        public UifJsonResult GetPrefixes()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetPrefixes());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPrefixes);
            }
        }

        public UifJsonResult GetCancellationReasons()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetCancellationReasons());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetCancellationReasons);
            }
        }

        public UifJsonResult GetCurrencies()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetCurrencies());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetCurrencies);
            }
        }

        public UifJsonResult GetPaymentClasses()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.salvageApplicationService.GetPaymentClasses());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPaymentClasses);
            }
        }

        public UifJsonResult CalculateRecoveryAgreedPlan(DateTime dateStart, decimal totalSale, int payments, string currencyDescription)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.salvageApplicationService.CalculateSaleAgreedPlan(dateStart, totalSale, payments, currencyDescription));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCalculateRecoveryAgreedPlan);
            }
        }

        public UifJsonResult GetSubClaimsByPrefixIdBranchIdPolicyDocumentNumberClaimNumber(int? prefixId, int? branchId, string policyDocumentNumber, int claimNumber)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetSubClaimsByPrefixIdBranchIdPolicyDocumentNumberClaimNumber(prefixId, branchId, policyDocumentNumber, claimNumber));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetSubClaimsByPrefixIdBranchIdClaimNumber);
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

        public JsonResult GetDebtorsByNameDocumentNumber(string query)
        {
            try
            {
                return Json(DelegateService.claimApplicationService.GetDebtorsByDescriptionInsuredSearchTypeCustomerType(query, InsuredSearchType.DocumentNumber, CustomerType.Individual), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(App_GlobalResources.Language.ErrorGetDebtorsByNameDocumentNumber, JsonRequestBehavior.DenyGet);
            }
        }

        public JsonResult GetRecuperatorsByNameDocumentNumber(string query)
        {
            try
            {
                return Json(DelegateService.claimApplicationService.GetRecuperatorsByDescriptionInsuredSearchTypeCustomerType(query, InsuredSearchType.DocumentNumber, CustomerType.Individual), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(App_GlobalResources.Language.ErrorGetRecuperatorsByNameDocumentNumber, JsonRequestBehavior.DenyGet);
            }
        }

        public JsonResult GetRecoveriesByClaimIdSubClaimId(int claimId, int subClaimId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.recoveryApplicationService.GetRecoveriesByClaimIdSubClaimId(claimId, subClaimId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetRecoveriesByClaimIdSubClaimId);
            }
        }

        public JsonResult ExecuteRecoveryOperations(RecoveryDTO recovery)
        {
            recovery.Documentation = 1;
            try
            {
                if (recovery.Id == 0)
                {
                    return new UifJsonResult(true, DelegateService.recoveryApplicationService.CreateRecovery(recovery));
                }
                else
                {
                    return new UifJsonResult(true, DelegateService.recoveryApplicationService.UpdateRecovery(recovery));
                }

            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorExecuteRecoveryOperations);
            }
        }

        public UifJsonResult GetRecoveryTypes()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.recoveryApplicationService.GetRecoveryTypes());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetRecoveryTypes);
            }
        }

        public UifJsonResult GetRecoveryTypesById(int recoveryType)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.recoveryApplicationService.GetRecoveryTypesById(recoveryType));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetRecoveryTypesById);
            }
        }

        public UifJsonResult GetRecoveryByRecoveryId(int recoveryId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.recoveryApplicationService.GetRecoveryByRecoveryId(recoveryId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetRecoveryByRecoveryId);
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

    }
}