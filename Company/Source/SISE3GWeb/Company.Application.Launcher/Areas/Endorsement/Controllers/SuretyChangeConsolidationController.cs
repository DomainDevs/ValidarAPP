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
using System.Linq;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    public class SuretyChangeConsolidationController : ChangeConsolidationController
    {
        [Authorize]
        public ActionResult CreateTemporal(ChangeConsolidationViewModel changeConsolidationViewModel)
        {
            UnderwritingController underwritingController = new UnderwritingController();
            try
            {
                CompanyChangeConsolidation companyChangeConsolidation = new CompanyChangeConsolidation();
                companyChangeConsolidation = ModelAssembler.CreateCompanyEndorsement(changeConsolidationViewModel);
                if (!string.IsNullOrEmpty(companyChangeConsolidation.Text.TextBody))
                    companyChangeConsolidation.Text.TextBody = underwritingController.unicode_iso8859(companyChangeConsolidation.Text.TextBody);
                var changeConsolidation = DelegateService.ciaSuretyChangeConsolidationService.CreateTemporal(companyChangeConsolidation, false);
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
        public ActionResult CreateEndorsementChangeConsolidation(ChangeConsolidationViewModel changeConsolidationViewModel)
        {
            try
            {
                if (changeConsolidationViewModel != null)
                {

                    var companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(changeConsolidationViewModel.TemporalId.Value, false);
                    companyPolicy.UserId = SessionHelper.GetUserId();
                    companyPolicy.User = new CompanyPolicyUser { UserId = SessionHelper.GetUserId() };
                    DelegateService.underwritingService.CreatePolicyTemporal(companyPolicy, false);
                    
                    var companyChangeConsolidation = ModelAssembler.CreateCompanyEndorsement(changeConsolidationViewModel);
                    var policy = DelegateService.ciaSuretyChangeConsolidationService.CreateEndorsementChangeConsolidation(companyChangeConsolidation);
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

        public ActionResult GetRiskTemporalById(int id)
        {
            try
            {
                CompanyContract companyContract = DelegateService.ciaSuretyChangeConsolidationService.GetCompanyContractByTemporalId(id, false);

                return new UifJsonResult(true, companyContract);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchTemp);
            }
        }
    }
}
