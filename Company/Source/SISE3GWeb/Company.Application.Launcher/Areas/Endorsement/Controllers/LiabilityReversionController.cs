using Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    public class LiabilityReversionController : ReversionController
    {
        /// <summary>
        /// Crea un nuevo temporal
        /// </summary>
        /// <param name="reversionViewModel"></param>
        /// <returns> Identificador del temporal creado </returns>
        public ActionResult CreateTemporal(ReversionViewModel reversionModel)
        {
            if (reversionModel != null)
            {
                try
                {
                    var CompanyEndorsement = ModelAssembler.CreateCompanyEndorsement(reversionModel);
                    if (CompanyEndorsement != null)
                    {
                        var policy = DelegateService.liabilityReversionServiceCia.CreateEndorsementReversion(CompanyEndorsement, false);
                        if (policy != null)
                        {
                            var policyWF = DelegateService.ReversionEndorsementService.CreateEndorsementWorkFlow(reversionModel.PolicyId, policy.Endorsement.Id, reversionModel.TicketNumber.ToString(), Convert.ToDateTime(reversionModel.TicketDate));
                            
                            // DelegateService.underwritingService.SaveTextLarge(policy.Endorsement.PolicyId, policy.Endorsement.Id);
                            return new UifJsonResult(true, policy);
                        }
                        else
                        {
                            return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
                        }
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
            else
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
            }

        }

    }
}