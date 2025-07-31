using Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Controllers;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using System;
using Sistran.Core.Framework.UIF.Web.Services;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers;
using Sistran.Company.Application.UnderwritingServices.Models;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
	public class LiabilityChangeCoinsuranceController :ChangeCoinsuranceController
	{
        [Authorize]
        public ActionResult CreateTemporal(ChangeCoinsuranceViewModel changeCoinsuranceViewModel)
        {
            UnderwritingController underwritingController = new UnderwritingController();
            try
            {
                var CompanyPolicy = ModelAssembler.CreateCompanyEndorsement(changeCoinsuranceViewModel);
                if (!string.IsNullOrEmpty(CompanyPolicy.Text.TextBody))
                    CompanyPolicy.Text.TextBody = underwritingController.unicode_iso8859(CompanyPolicy.Text.TextBody);
                var policy = DelegateService.liabilityChangeCoinsuranceServiceCia.CreateTemporal(CompanyPolicy, false);
                if (policy != null)
                {
                    return new UifJsonResult(true, policy);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
                }
            }
            catch (Exception ex)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
            }

        }

        /// <summary>
        /// Cambio de Intermediario. persistencia del Endoso doble.
        /// </summary>
        /// <param name="changeCoinsuredModel"></param>
        /// <returns></returns>
        public ActionResult CreateEndorsementChangeCoinsurance(ChangeCoinsuranceViewModel changeCoinsuranceViewModel)
        {
            try
            {
                if (changeCoinsuranceViewModel != null)
                {
                    var companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(changeCoinsuranceViewModel.TemporalId.Value, false);
                    companyPolicy.UserId = SessionHelper.GetUserId();
                    companyPolicy.User = new CompanyPolicyUser { UserId = SessionHelper.GetUserId() };
                    companyPolicy = DelegateService.underwritingService.CreatePolicyTemporal(companyPolicy, false);
                    
                    companyPolicy = ModelAssembler.CreateCompanyEndorsement(changeCoinsuranceViewModel);
                    var policy = DelegateService.liabilityChangeCoinsuranceServiceCia.CreateEndorsementChangeCoinsurance(companyPolicy.Endorsement);
                    
                    if (policy.FirstOrDefault().InfringementPolicies != null && policy.FirstOrDefault().InfringementPolicies.Count > 0)
                    {
                        return new UifJsonResult(true, policy);
                    }
                    else
                    {
                        DelegateService.underwritingService.SaveTextLarge(policy.FirstOrDefault().Endorsement.PolicyId, policy.FirstOrDefault().Endorsement.Id);
                        return new UifJsonResult(true, policy);
                    }
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
                }
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
            }

        }
    }
}