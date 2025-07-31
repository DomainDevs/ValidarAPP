using Sistran.Company.Application.ExternalProxyServices.Models;
using Sistran.Core.Application.ClaimServices.DTOs.Claims;
using Sistran.Core.Framework.UIF.Web.Services;
using System;

namespace Sistran.Core.Framework.UIF.Web.Areas.Claims.Business
{
    public static class ClaimBusiness
    {
        internal static bool GetPolicyReinsurance2G(PolicyDTO policyDTO)
        {
            try
            {
                ResponseReinsurance responseReinsurance = new ResponseReinsurance();
                RequestReinsurance requestReinsurance = new RequestReinsurance();
                requestReinsurance.DocumentNumber = Convert.ToInt32(policyDTO.DocumentNumber);
                requestReinsurance.EndorsementNumber = policyDTO.EndorsementDocumentNum;
                requestReinsurance.Prefix = policyDTO.PrefixId;
                requestReinsurance.Branch = policyDTO.BranchId;
                responseReinsurance = DelegateService.ExternalServiceWeb.GetReinsurancePolicy(requestReinsurance);

                return responseReinsurance.PolicyStatus == 1;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}