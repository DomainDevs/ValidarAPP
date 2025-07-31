using Sistran.Core.Application.CancellationEndorsement.Enums;
using Sistran.Core.Application.EndorsementBaseService.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    public class CancellationController : EndorsementBaseController
    {
        public ActionResult Cancellation()
        {
            return PartialView();
        }

        public ActionResult GetCancellationTypes()
        {
            try
            {
                return new UifSelectResult(EnumsHelper.GetItems<CancellationType>());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryCancellationTypes);
            }
        }

        public JsonResult GetEndorsementReasonsByCancellationType(CancellationType cancellationType)
        {
            try
            {
                List<EndorsementReason> endorsementReasons = new List<EndorsementReason>();

                if (cancellationType == CancellationType.Nominative)
                {
                    endorsementReasons = DelegateService.endorsementBaseService.GetEndorsementReasonsByEndorsementType(EndorsementType.Nominative_cancellation);
                }
                else
                {
                    endorsementReasons = DelegateService.endorsementBaseService.GetEndorsementReasonsByEndorsementType(EndorsementType.Cancellation);
                }

                return new UifJsonResult(true, endorsementReasons.OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryEndorsementReasons);
            }
        }
    }
}