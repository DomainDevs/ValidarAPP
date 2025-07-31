using Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Linq;
using System.Web.Mvc;
using Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Controllers;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Company.Application.UnderwritingServices.Models;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    public class LiabilityChangeTermController : ChangeTermController
    {
        public ActionResult CreateTemporal(ChangeTermViewModel changeTermModel)
        {
            UnderwritingController underwritingController = new UnderwritingController();
            try
            {
                var CompanyPolicy = ModelAssembler.CreateCompanyEndorsement(changeTermModel);
                //if (!string.IsNullOrEmpty(CompanyPolicy.Text.TextBody))
                if (CompanyPolicy.Text != null)
                {
                    CompanyPolicy.Text.TextBody = underwritingController.unicode_iso8859(CompanyPolicy.Text.TextBody);
                }
                var policy = DelegateService.LiabilityChangeTermServiceCompany.CreateTemporal(CompanyPolicy, false);
                if (policy != null)
                {
                    return new UifJsonResult(true, policy);
                }
                else
                {
                    return new UifJsonResult(false, string.Format(App_GlobalResources.Language.EndorrsementNotChange, App_GlobalResources.Language.ChangeTermEndorsement));
                }
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
            }

        }


        public ActionResult CreateEndorsementChangeTerm(ChangeTermViewModel changeTermModel)
        {
            try
            {
                if (changeTermModel != null)
                {
                    var companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(changeTermModel.TemporalId.Value, false);
                    companyPolicy.UserId = SessionHelper.GetUserId();
                    companyPolicy.User = new CompanyPolicyUser { UserId = SessionHelper.GetUserId() };
                    companyPolicy = DelegateService.underwritingService.CreatePolicyTemporal(companyPolicy, false);

                    companyPolicy = ModelAssembler.CreateCompanyEndorsement(changeTermModel);
                    var policy = DelegateService.LiabilityChangeTermServiceCompany.CreateEndorsementChangeTerm(companyPolicy.Endorsement);
                    if (policy.FirstOrDefault().Errors != null && policy.FirstOrDefault().Errors.Any())
                    {
                        return new UifJsonResult(policy.FirstOrDefault().Errors.First().StateData, policy.FirstOrDefault().Errors.First().Error);
                    }
                    return new UifJsonResult(true, policy);
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