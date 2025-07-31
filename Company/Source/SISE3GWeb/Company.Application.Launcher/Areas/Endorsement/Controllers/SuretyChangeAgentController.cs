using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Controllers;
using Sistran.Core.Framework.UIF.Web.Controllers;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    public class SuretyChangeAgentController : ChangeAgentController
    {
        public ActionResult CreateTemporal(ChangeAgentViewModel changeAgentModel)
        {
            UnderwritingController underwritingController = new UnderwritingController();
            try
            {
                var CompanyPolicy = ModelAssembler.CreateCompanyEndorsement(changeAgentModel);
                if (!string.IsNullOrEmpty(CompanyPolicy.Text.TextBody))
                    CompanyPolicy.Text.TextBody = underwritingController.unicode_iso8859(CompanyPolicy.Text.TextBody);
                var policy = DelegateService.suretyChangeAgentServiceCia.CreateTemporal(CompanyPolicy, false);
                if (policy != null)
                {
                    return new UifJsonResult(true, policy);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.NochangesInTheAgents);
                }
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
            }

        }

        /// <summary>
        /// Cambio de Intermediario. persistencia del Endoso doble.
        /// </summary>
        /// <param name="changeAgentModel"></param>
        /// <returns></returns>
        public ActionResult CreateEndorsementChangeAgent(ChangeAgentViewModel changeAgentModel)
        {
            try
            {
                if (changeAgentModel != null)
                {
                    var companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(changeAgentModel.TemporalId.Value, false);
                    companyPolicy.UserId = SessionHelper.GetUserId();
                    companyPolicy.User = new CompanyPolicyUser { UserId = SessionHelper.GetUserId() };
                    companyPolicy = DelegateService.underwritingService.CreatePolicyTemporal(companyPolicy, false);

                    companyPolicy = ModelAssembler.CreateCompanyEndorsement(changeAgentModel);
                    var policy = DelegateService.suretyChangeAgentServiceCia.CreateEndorsementChangeAgent(companyPolicy.Endorsement);
                    if (policy.Exists(x=> x.InfringementPolicies != null && x.InfringementPolicies.Count > 0))
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