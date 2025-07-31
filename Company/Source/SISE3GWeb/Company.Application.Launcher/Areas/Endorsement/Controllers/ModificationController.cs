using Sistran.Core.Application.BaseEndorsementService.DTOs;
using Sistran.Core.Application.EndorsementBaseService.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    public class ModificationController : EndorsementBaseController
    {
        public ActionResult Modification()
        {
            return PartialView();

        }

        public ActionResult GetModificationReasons()
        {
            try
            {
                List<EndorsementReason> endorsementReasons = DelegateService.endorsementBaseService.GetEndorsementReasonsByEndorsementType(EndorsementType.Modification);
                return new UifJsonResult(true, endorsementReasons.OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryEndorsementReasons);
            }
        }

        /// <summary>
        /// Gets the type of the modification.
        /// </summary>
        /// <returns></returns>
        public ActionResult GetModificationType()
        {
            try
            {
                List<EndorsementTypeDTO> endorsementTypes = DelegateService.endorsementBaseService.GetModificationType();
                Parallel.ForEach(endorsementTypes, item =>
                 {
                     item.Description = EnumsHelper.GetName(item.Description);
                 });
                return new UifJsonResult(true, endorsementTypes.OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryEndorsementReasons);
            }
        }

        public ActionResult Texts()
        {
            return PartialView();

        }

        public ActionResult PaymentPlan()
        {
            return PartialView();
        }

        public ActionResult Clauses()
        {
            return PartialView();

        }

        #region optimizacion
        #endregion
    }
}