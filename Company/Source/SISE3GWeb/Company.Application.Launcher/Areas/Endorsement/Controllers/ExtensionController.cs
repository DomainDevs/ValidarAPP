using Sistran.Core.Application.EndorsementBaseService.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    public class ExtensionController : EndorsementBaseController
    {
        public ActionResult Extension()
        {
            return PartialView();
        }

        public ActionResult GetExtensionReasons()
        {
            try
            {
                List<EndorsementReason> endorsementReasons = DelegateService.endorsementBaseService.GetEndorsementReasonsByEndorsementType(EndorsementType.EffectiveExtension);
                if (endorsementReasons?.Count > 0)
                {
                    return new UifJsonResult(true, endorsementReasons.OrderBy(x => x.Description).ToList());
                }
                else
                {
                    return new UifJsonResult(true, null);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryingReasons);
            }
        }
    }
}