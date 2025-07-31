using Sistran.Core.Framework.UIF.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Company.Application.UniquePersonAplicationServices.DTOs;
using Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Controllers;
using Sistran.Company.Application.Sureties.SuretyServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    public class LiabilityChangePolicyHolderController : ChangePolicyHolderController
    {
        [Authorize]
        public ActionResult CreateTemporal(ChangePolicyHolderViewModel changePolicyHolderViewModel)
        {
            UnderwritingController underwritingController = new UnderwritingController();
            try
            {
                CompanyChangePolicyHolder companyChangePolicyHolder = new CompanyChangePolicyHolder();
                companyChangePolicyHolder = ModelAssembler.CreateCompanyEndorsement(changePolicyHolderViewModel);
                if (!string.IsNullOrEmpty(companyChangePolicyHolder.Text.TextBody))
                    companyChangePolicyHolder.Text.TextBody = underwritingController.unicode_iso8859(companyChangePolicyHolder.Text.TextBody);
                var changeConsolidation = DelegateService.ciaLiabilityChangePolicyHolderService.CreateTemporal(companyChangePolicyHolder, false);
                if (changeConsolidation != null)
                {
                    return new UifJsonResult(true, changeConsolidation);
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
        public ActionResult CreateEndorsementChangePolicyHolder(ChangePolicyHolderViewModel changePolicyHolderViewModel)
        {
            try
            {
                if (changePolicyHolderViewModel != null)
                {
                    var companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(changePolicyHolderViewModel.TemporalId.Value, false);
                    companyPolicy.UserId = SessionHelper.GetUserId();
                    companyPolicy.User = new CompanyPolicyUser { UserId = SessionHelper.GetUserId() };
                    DelegateService.underwritingService.CreatePolicyTemporal(companyPolicy, false);

                    var companyChangePolicyHolder = ModelAssembler.CreateCompanyEndorsement(changePolicyHolderViewModel);
                    var policy = DelegateService.ciaLiabilityChangePolicyHolderService.CreateEndorsementChangePolicyHolder(companyChangePolicyHolder);
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
