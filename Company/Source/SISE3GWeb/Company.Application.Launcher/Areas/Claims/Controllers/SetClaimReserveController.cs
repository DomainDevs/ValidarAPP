using Sistran.Core.Application.ClaimServices.DTOs.Claims;
using Sistran.Core.Application.ClaimServices.DTOs.PaymentRequest;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.UIF.Web.Areas.Claims.Business;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Claims.Controllers
{
    public class SetClaimReserveController : Controller
    {
        private static List<Sistran.Core.Application.CommonService.Models.Parameter> parameters = new List<Sistran.Core.Application.CommonService.Models.Parameter>();

        // GET: Claims/SetClaimReserve
        public ActionResult SetClaimReserve()
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

        public UifJsonResult GetSubClaimsByPrefixIdBranchIdPolicyDocumentNumberClaimNumber(int? prefixId, int? branchId, string policyDocumentNumber ,int claimNumber)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetClaimReserveByPrefixIdBranchIdPolicyDocumentNumberClaimNumber(prefixId, branchId, policyDocumentNumber , claimNumber));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetSubClaimsByPrefixIdBranchIdClaimNumber);
            }
        }

        public UifJsonResult ExecuteReserveOperations(ClaimReserveDTO claimReserveDTO)
        {
            try
            {
                claimReserveDTO.Modifications.First().UserId = SessionHelper.GetUserId();
                claimReserveDTO.Modifications.First().UserProfileId = SessionHelper.GetUserProfile();
                return new UifJsonResult(true, DelegateService.claimApplicationService.SetClaimReserve(claimReserveDTO));
            }
            catch (BusinessException ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorExecuteReserveOperations);
            }
        }

        public UifJsonResult GetInsuredAmount(int policyId, int riskNum, int coverageId, int coverNum, int claimId, int subClaimId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetInsuredAmount(policyId, riskNum, coverageId, coverNum, claimId, subClaimId));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetInsuredAmount);
            }
        }

        public UifJsonResult GetDeductiblesByPolicyIdRiskNumCoverageIdCoverageNumber(int policyId, int riskNum, int coverageId, int coverNum)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetDeductiblesByPolicyIdRiskNumCoverageIdCoverageNumber(policyId, riskNum, coverageId, coverNum));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetDeductiblesByPolicyIdRiskNumCoverageIdCoverageNumber);
            }
        }

        public UifJsonResult GetClaimModifiesByClaimId(int claimId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.claimApplicationService.GetClaimModifiesByClaimId(claimId));
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);

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

        private List<Sistran.Core.Application.CommonService.Models.Parameter> GetParameters()
        {
            parameters.Add(new Sistran.Core.Application.CommonService.Models.Parameter { Description = "Currency", Value = DelegateService.commonService.GetParameterByParameterId((int)ParametersTypes.Currency).NumberParameter.Value });

            return parameters;
        }
    }
}