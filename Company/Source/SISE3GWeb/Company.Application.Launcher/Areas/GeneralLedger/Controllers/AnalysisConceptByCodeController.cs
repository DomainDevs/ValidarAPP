using System.Linq;
using System.Web.Mvc;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Framework.Exceptions;
using Sistran.Core.Framework.UIF.Web.Services;
namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Controllers
{
    [Authorize]
    [NoDirectAccess]
    public class AnalysisConceptByCodeController : Controller
    {
       

        #region View

        /// <summary>
        /// AnalysisConceptByCode
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult AnalysisConceptByCode()
        {
            try
            {
                //valida que el servicio este arriba
                var moduleDates = DelegateService.tempCommonService.GetModuleDates();

                return View();
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            } 
        }

        #endregion View

        #region Actions

        /// <summary>
        /// GetAnalysisCodes
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetAnalysisCodes()
        {
            var analysisCodes = (from items in DelegateService.glAccountingApplicationService.GetAnalysisCodes()
                            select new
                            {
                                Id = items.AnalysisCodeId,
                                Description = items.Description
                            }).ToList();

            return new UifSelectResult(analysisCodes);
        }

        /// <summary>
        /// GetAnalysisConceptByCodeId
        /// </summary>
        /// <param name="id"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetAnalysisConceptByCodeId(int id)
        {
            var analysisConcepts = (from items in DelegateService.glAccountingApplicationService.GetPaymentConceptsByAnalysisCode(id)
                            select new
                            {
                                AnalysisConceptDescription = items.AnalysisConceptDescription,
                                AnalysisConceptAnalysisId = items.AnalysisConceptAnalysisId,
                                AnalysisConceptId = items.AnalysisConceptId,
                                AnalysisId = items.AnalysisId

                            }).ToList();

            return Json(analysisConcepts, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// DeleteAnalysisConceptAnalysis
        /// </summary>
        /// <param name="id"></param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult DeleteAnalysisConceptAnalysis(int id)
        {
            try
            {
                DelegateService.glAccountingApplicationService.DeleteAnalysisConceptAnalysis(id);
                return Json(new { success = true, result = id }, JsonRequestBehavior.AllowGet);
            }
            catch (System.Exception)
            {
                return Json(new { success = false, result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// GetCountRemainingAnalysisConcepts
        /// </summary>
        /// <param name="id"></param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult GetCountRemainingAnalysisConcepts(int id)
        {
            var analysisConcepts = DelegateService.glAccountingApplicationService.GetRemainingAnalysisConcepts(id).Count;
            return Json(analysisConcepts, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetRemainingAnalysisConcepts
        /// </summary>
        /// <param name="id"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetRemainingAnalysisConcepts(int id)
        {
            var remainingAnalysisConcepts = (from rows in DelegateService.glAccountingApplicationService.GetRemainingAnalysisConcepts(id)
                            select new
                            {
                                AnalysisConceptId = rows.AnalysisConceptId,
                                Description = rows.Description,
                                AnalysisTreatment = DelegateService.glAccountingApplicationService.GetAnalysisTreatment
                                (rows.AnalysisTreatment.AnalysisTreatmentId).Description
                            }).ToList();

            return Json(new { aaData = remainingAnalysisConcepts }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// SaveAnalysisConceptAnalysis
        /// </summary>
        /// <param name="analysisId"></param>
        /// <param name="analysisConceptId"></param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult SaveAnalysisConceptAnalysis(int analysisId, int analysisConceptId)
        {
            DelegateService.glAccountingApplicationService.SaveAnalysisConceptAnalysis(analysisId, analysisConceptId);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        #endregion Actions

    }
}