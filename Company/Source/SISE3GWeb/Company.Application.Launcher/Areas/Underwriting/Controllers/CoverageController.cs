using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Controllers
{
    public class CoverageController : Controller
    {

        public ActionResult GetCoverageByCoverageId(int coverageId, int riskId, int temporalId, int groupCoverageId)
        {
            try
            {
                var companyCoverage = DelegateService.underwritingService.GetCompanyCoverageByCoverageId(coverageId, groupCoverageId, temporalId);
                return new UifJsonResult(true, companyCoverage);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchCoverage);
            }
        }

        public ActionResult GetCoveragesByProductIdGroupCoverageIdPrefixId(int productId, int groupCoverageId, int prefixId, string coveragesAdd)
        {
            try
            {
                string[] idCoverages = coveragesAdd.Split(',');
                List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(productId, groupCoverageId, prefixId);
                if (coveragesAdd != "" && coverages != null && coverages.Any())
                {
                    coverages = coverages.Where(c => (!idCoverages.Any(x => Convert.ToInt32(x) == c.Id)) && c.IsVisible == true).ToList();
                }
                if (coverages != null && coverages.Count > 0)
                {
                    return new UifJsonResult(true,coverages.OrderBy(x => x.Description));
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.NoCoverageToAdd);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchCoverages);
            }
        }

        public ActionResult GetCalculeTypes()
        {
            try
            {
                return new UifJsonResult(true, EnumsHelper.GetItems<Core.Services.UtilitiesServices.Enums.CalculationType>());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryCalculationTypes);
            }
        }

        public ActionResult GetRateTypes()
        {
            try
            {
                return new UifJsonResult(true, EnumsHelper.GetItems<RateType>());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryRateTypes);
            }
        }

        public ActionResult GetFirstRiskTypes()
        {
            try
            {
                return new UifSelectResult(EnumsHelper.GetItems<FirstRiskType>());
            }
            catch (Exception)
            {
                throw;
            }

        }

        public ActionResult GetCoveragesByProductIdGroupCoverageId(int productId, int groupCoverageId, int prefixId)
        {
            try
            {
                List<CompanyCoverage> coveragesList = DelegateService.underwritingService.GetCompanyCoveragesByProductIdGroupCoverageIdPrefixId(productId, groupCoverageId, prefixId);
                coveragesList = coveragesList.Where(x => x.IsMandatory == true).ToList();
                return Json(coveragesList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryCoverageGroups);
            }
        }

        public ActionResult GetDeductiblesByCoverageId(int coverageId)
        {
            try
            {
                List<Deductible> deductibles = DelegateService.underwritingService.GetDeductiblesByCoverageId(coverageId);
                return new UifJsonResult(true, deductibles.OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorConsultingDeductibles);
            }
        }
    }
}