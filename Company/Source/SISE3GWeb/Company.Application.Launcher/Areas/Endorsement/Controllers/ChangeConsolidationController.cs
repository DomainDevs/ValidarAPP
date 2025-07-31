using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using Sistran.Core.Framework.UIF.Web.Services;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Endorsement.Controllers
{
    public class ChangeConsolidationController : EndorsementBaseController
    {
        public ActionResult ChangeConsolidation()
        {
            return PartialView();
        }

        public UifJsonResult GetHoldersByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType, TemporalType temporalType)
        {
            try
            {
                List<Holder> holders = DelegateService.underwritingService.GetHoldersByDocument(description, customerType);
                if (holders != null && holders.Any())
                {
                    return new UifJsonResult(true, holders);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorHolderNotFound);
                }


            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchPolicyholder);
            }
        }
    }
}
