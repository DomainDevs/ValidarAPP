using System;
using System.Web.Mvc;
using Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    public class FidelityReversionController : ReversionController
    {

        public ActionResult CreateTemporal(ReversionViewModel reversionModel)
        {
            try
            {
                if (reversionModel == null)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
                }
                var CompanyEndorsement = ModelAssembler.CreateCompanyEndorsement(reversionModel);
                var policy = DelegateService.fidelityReversionService.CreateEndorsementReversion(CompanyEndorsement);
                return new UifJsonResult(true, policy);
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
            }

        }
    }
}