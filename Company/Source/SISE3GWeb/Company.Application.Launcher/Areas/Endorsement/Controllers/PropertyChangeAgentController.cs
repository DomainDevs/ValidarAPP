using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    public class PropertyChangeAgentController : ChangeAgentController
    {
        public ActionResult CreateTemporal(ChangeAgentViewModel changeAgentModel)
        {
            try
            {
                var CompanyPolicy = ModelAssembler.CreateCompanyEndorsement(changeAgentModel);
                var policy = DelegateService.propertyChangeAgentServiceCia.CreateTemporal(CompanyPolicy, false);
                if (policy != null)
                {
                    
                    return new UifJsonResult(true, policy);
                }
                else
                {
                    return new UifJsonResult(false, "No hay Cambios en los Agentes");
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
            }
        }
        public ActionResult CreateEndorsementChangeAgent(ChangeAgentViewModel changeAgentModel)
        {
            if (changeAgentModel != null)
            {
                var companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(changeAgentModel.TemporalId.Value, false);
                companyPolicy.UserId = SessionHelper.GetUserId();
                companyPolicy.User = new CompanyPolicyUser { UserId = SessionHelper.GetUserId() };
                companyPolicy = DelegateService.underwritingService.CreatePolicyTemporal(companyPolicy, false);

                companyPolicy = ModelAssembler.CreateCompanyEndorsement(changeAgentModel);
                var policy = DelegateService.propertyChangeAgentServiceCia.CreateEndorsementChangeAgent(companyPolicy.Endorsement);
                DelegateService.underwritingService.SaveTextLarge(policy.FirstOrDefault().Endorsement.PolicyId, policy.FirstOrDefault().Endorsement.Id);
                return new UifJsonResult(true, policy);
            }
            else
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
            }
        }

    }
}