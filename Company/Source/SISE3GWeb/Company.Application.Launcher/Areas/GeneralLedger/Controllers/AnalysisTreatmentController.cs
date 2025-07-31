using System;
using System.Linq;
using System.Web.Mvc;
using Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models;
using Sistran.Core.Application.GeneralLedgerServices.DTOs;
using Sistran.Core.Framework.UIF.Web.Services;
namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Controllers
{
    [Authorize]
    [NoDirectAccess]
    public class AnalysisTreatmentController : Controller
    {
        #region View

        /// <summary>
        /// AnalysisTreatment
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult AnalysisTreatment()
        {

            //valida que el servicio este arriba
            var moduleDates = DelegateService.tempCommonService.GetModuleDates();

            return View();
        }

        #endregion View

        #region Actions

        /// <summary>
        /// GetAnalysisTreatment
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetAnalysisTreatment()
        {
            var analysisTreatments = DelegateService.glAccountingApplicationService.GetAnalysisTreatments();

            var jsonData = from analysisTreatment in analysisTreatments
                           select new
                           {
                               AnalysisTreatmentId = analysisTreatment.AnalysisTreatmentId,
                               Description = analysisTreatment.Description
                           };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// AnalysisTreatmentModal
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ActionResult</returns>
        public ActionResult AnalysisTreatmentModal(int? id)
        {
            AnalysisTreatmentModel analysisTreatmentModel = new AnalysisTreatmentModel();

            if (id.HasValue)
            {
                AnalysisTreatmentDTO analysisTreatment = DelegateService.glAccountingApplicationService.GetAnalysisTreatment(Convert.ToInt32(id));
                analysisTreatmentModel.AnalysisTreatmentId = analysisTreatment.AnalysisTreatmentId;
                analysisTreatmentModel.Description = analysisTreatment.Description;
            }
            return View(analysisTreatmentModel);
        }

        /// <summary>
        /// SaveAnalysisTreatment
        /// </summary>
        /// <param name="analysisTreatment"></param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult SaveAnalysisTreatment(AnalysisTreatmentDTO analysisTreatment)
        {
            bool isSucessfully = false;
            int saved = 0;

            try
            {
                if (analysisTreatment.AnalysisTreatmentId == 0)
                {
                    analysisTreatment = DelegateService.glAccountingApplicationService.SaveAnalysisTreatment(analysisTreatment);
                }
                else
                {
                    analysisTreatment = DelegateService.glAccountingApplicationService.UpdateAnalysisTreatment(analysisTreatment);
                }

                isSucessfully = true;
                saved = analysisTreatment.AnalysisTreatmentId;
            }
            catch (Exception)
            {
                isSucessfully = false;
                saved = 0;
            }

            return Json(new { success = isSucessfully, result = saved }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// DeleteAnalysisTreatment
        /// </summary>
        /// <param name="id"></param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult DeleteAnalysisTreatment(int id)
        {
            bool isSucessfully = false;
            int deleted = 0;

            try
            {
                DelegateService.glAccountingApplicationService.DeleteAnalysisTreatment(id);
                isSucessfully = true;
                deleted = 1;
            }
            catch (Exception)
            {
                isSucessfully = false;
                deleted = 0;
            }

            return Json(new { success = isSucessfully, result = deleted }, JsonRequestBehavior.AllowGet);
        }

        #endregion Actions

    }
}