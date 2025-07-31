using Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    public class MarineReversionController : Controller
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
                var policy = DelegateService.marineReversionService.CreateEndorsementReversion(CompanyEndorsement);
                return new UifJsonResult(true, policy);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateTemporary);
            }

        }
    }
}