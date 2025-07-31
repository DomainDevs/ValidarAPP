using Sistran.Core.Application.ClaimServices.DTOs.Salvage;
using Sistran.Core.Framework.UIF.Web.Areas.Claims.Business;
using Sistran.Core.Application.ClaimServices.DTOs.Claims;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Claims.Controllers
{
    public class SalvageController : Controller
    {
        public ActionResult Salvage()
        {
            return View();
        }

        public UifJsonResult GetPrefixes()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.salvageApplicationService.GetPrefixesSalvage());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPrefixes);
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
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetBranches);
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

        public UifJsonResult GetSalvagesByClaimIdSubClaimId(int claimId, int subClaimId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.salvageApplicationService.GetSalvagesByClaimIdSubClaimId(claimId, subClaimId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetSalvagesByClaimIdSubClaimId);
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

        public UifJsonResult GetSubClaimsByPrefixIdBranchIdPolicyDocumentNumberClaimNumber(int? prefixId, int? branchId, string policyDocumentNumber, int claimNumber)
        {
            try
            {
                List<Application.ClaimServices.DTOs.Claims.PrefixDTO> prefixes = DelegateService.salvageApplicationService.GetPrefixesSalvage();
                List<Application.ClaimServices.DTOs.Claims.SubClaimDTO> subClaims = DelegateService.claimApplicationService.GetSubClaimsByPrefixIdBranchIdPolicyDocumentNumberClaimNumber(prefixId, branchId, policyDocumentNumber, claimNumber);
                
                if (!subClaims.Any() || prefixes.Exists(x => subClaims.Any(y => y.PrefixId == x.Id)))
                {
                    return new UifJsonResult(true, subClaims);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.CannotSalvageClaimPrefix);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetSubClaimsByPrefixIdBranchIdClaimNumber);
            }
        }

        public UifJsonResult GetSalvageBySalvageId(int salvageId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.salvageApplicationService.GetSalvageBySalvageId(salvageId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetSalvageBySalvageId);
            }
        }

        public UifJsonResult GetSalesBySalvageId(int salvageId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.salvageApplicationService.GetSalesBySalvageId(salvageId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetSalesBySalvageId);
            }
        }

        public JsonResult GetBuyersByNameDocumentNumber(string query)
        {
            try
            {
                return Json(DelegateService.salvageApplicationService.GetBuyersByDescriptionInsuredSearchTypeCustomerType(query, InsuredSearchType.DocumentNumber, CustomerType.Individual), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(App_GlobalResources.Language.ErrorGetBuyersByNameDocumentNumber, JsonRequestBehavior.DenyGet);
            }
        }

        public UifJsonResult CalculateSaleAgreedPlan(DateTime dateStart, decimal totalSale, int payments, string currencyDescription)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.salvageApplicationService.CalculateSaleAgreedPlan(dateStart, totalSale, payments, currencyDescription));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCalculateSaleAgreedPlan);
            }
        }

        public UifJsonResult GetSaleBySaleId(int saleId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.salvageApplicationService.GetSaleBySaleId(saleId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetSaleBySaleId);
            }
        }

        public UifJsonResult ExecuteSalvageOperations(SalvageDTO salvage)
        {
            try
            {
                if (salvage.Id == 0)
                {
                    return new UifJsonResult(true, DelegateService.salvageApplicationService.CreateSalvage(salvage));
                }
                else
                {
                    return new UifJsonResult(true, DelegateService.salvageApplicationService.UpdateSalvage(salvage));
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorExecuteSalvageOperations);
            }
        }

        public UifJsonResult ExecuteSaleOperations(SaleDTO sale, int salvageId)
        {
            try
            {
                if (sale.Id == 0)
                {
                    return new UifJsonResult(true, DelegateService.salvageApplicationService.CreateSale(sale, salvageId));
                }
                else
                {
                    return new UifJsonResult(true, DelegateService.salvageApplicationService.UpdateSale(sale, salvageId));
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorExecuteSaleOperations);
            }
        }

        public UifJsonResult DeleteSalvage(int salvageId)
        {
            try
            {

                if (DelegateService.salvageApplicationService.GetSalesBySalvageId(salvageId).Count > 0)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.CannotDeleteSalvage);
                }
                else
                {
                    DelegateService.salvageApplicationService.DeleteSalvage(salvageId);
                }

                return new UifJsonResult(true, "");
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorDeleteSalvage);
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
    }
}