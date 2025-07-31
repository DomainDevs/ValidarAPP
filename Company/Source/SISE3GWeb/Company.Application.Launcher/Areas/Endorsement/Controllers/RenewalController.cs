using Sistran.Company.Application.UnderwritingServices;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Web.Mvc;
using MOS = Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    public class RenewalController : EndorsementBaseController
    {
        public ActionResult Renewal()
        {
            return PartialView();
        }

        public ActionResult GetTemporalPolicyByPolicyId(int policyId, int endorsementType)
        {
            try
            {
                var mapper = ModelAssembler.CreateMapCompanyEndorsement();
                CompanyEndorsement endorsement = mapper.Map<MOS.Endorsement, CompanyEndorsement>(DelegateService.endorsementBaseService.GetTemporalEndorsementByPolicyId(policyId));
                if (endorsement != null)
                {
                    if (endorsement.EndorsementType == (EndorsementType)endorsementType)
                    {
                        return GetTemporalById(endorsement.TemporalId);
                    }
                    else
                    {
                        return new UifJsonResult(false, App_GlobalResources.Language.PolicyFollowingTemporary + ": " + endorsement.TemporalId + ". " + App_GlobalResources.Language.NoCanGeneratedNewTemps);
                    }
                }
                else
                {
                    return new UifJsonResult(true, null);
                }
            }
            catch (Exception)
            {

                return new UifJsonResult(true, App_GlobalResources.Language.ErrorQueryEndorsement);
            }

        }
    }
}