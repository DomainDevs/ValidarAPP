using System;
using Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System.Web.Mvc;
using Sistran.Company.Application.UnderwritingServices.Models;
using System.Collections.Generic;
using System.Linq;
using Sistran.Core.Framework.UIF.Web.Helpers;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    public class LiabilityChangeAgentController : ChangeAgentController
    {
        public ActionResult CreateTemporal(ChangeAgentViewModel changeAgentModel)
        {
            try
            {
                CompanyPolicy CompanyPolicy = ModelAssembler.CreateCompanyEndorsement(changeAgentModel);
                var policy = DelegateService.liabilityChangeAgentServiceCia.CreateTemporal(CompanyPolicy, false);
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
            try
            {
                if (changeAgentModel != null)
                {
                    var companyPolicy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(changeAgentModel.TemporalId.Value, false);
                    companyPolicy.UserId = SessionHelper.GetUserId();
                    companyPolicy.User = new CompanyPolicyUser { UserId = SessionHelper.GetUserId() };
                    companyPolicy = DelegateService.underwritingService.CreatePolicyTemporal(companyPolicy, false);

                    companyPolicy = ModelAssembler.CreateCompanyEndorsement(changeAgentModel);
                    List<CompanyPolicy> policy = DelegateService.liabilityChangeAgentServiceCia.CreateEndorsementChangeAgent(companyPolicy.Endorsement);
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