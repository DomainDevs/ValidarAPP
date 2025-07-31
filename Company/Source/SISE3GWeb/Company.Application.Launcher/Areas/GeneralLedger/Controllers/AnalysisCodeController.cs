using Sistran.Core.Application.GeneralLedgerServices.DTOs;
using Sistran.Core.Framework.UIF.Web.Resources;
using Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models;
using Sistran.Core.Framework.Exceptions;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Sistran.Core.Application.GeneralLedgerServices.Enums;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Controllers
{
    [Authorize]
    [NoDirectAccess]
    public class AnalysisCodeController : Controller
    {
        #region View

        /// <summary>
        /// AnalysisCode
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult AnalysisCode()
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

        /// <summary>
        /// AnalysisCodeModal
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ActionResult</returns>
        public ActionResult AnalysisCodeModal(int? id)
        {
            var analysisCodesModel = new AnalysisCodesModel();

            try
            {

                if (id.HasValue)
                {
                    var analysis = DelegateService.glAccountingApplicationService.GetAnalysisCode(id.Value);

                    analysisCodesModel.Id = analysis.AnalysisCodeId;
                    analysisCodesModel.Description = analysis.Description;
                    analysisCodesModel.CheckBalance = analysis.CheckBalance;
                    analysisCodesModel.CheckModule = analysis.CheckModuleType;
                    analysisCodesModel.AccountingNatureId = (int)analysis.AccountingNature;
                }

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }


            return View(analysisCodesModel);
        }

        #endregion View

        #region Actions

        /// <summary>
        /// GetAnalysisCodes
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetAnalysisCodes()
        {
            var analysisCodes = DelegateService.glAccountingApplicationService.GetAnalysisCodes();

            var jsonData = from items in analysisCodes
                           select new
                           {
                               Id = items.AnalysisCodeId,
                               Description = items.Description,
                               AccountingNature = (int)items.AccountingNature == 1 ? Global.Credits : Global.Debits,
                               CheckBalance = items.CheckBalance ? Global.Yes : Global.No,
                               CheckModuleType = items.CheckModuleType ? Global.Yes : Global.No
                           };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// SaveAnalysisCodes
        /// </summary>
        /// <param name="analysisCodesModel"></param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult SaveAnalysisCodes(AnalysisCodesModel analysisCodesModel)
        {
            var analysisCode = new AnalysisCodeDTO()
            {
                AnalysisCodeId = analysisCodesModel.Id,
                Description = analysisCodesModel.Description,
                AccountingNature = analysisCodesModel.AccountingNatureId,
                CheckModuleType = Convert.ToBoolean(analysisCodesModel.CheckModule),
                CheckBalance = Convert.ToBoolean(analysisCodesModel.CheckBalance),
                AnalisisConcepts = new List<AnalysisConceptDTO>()
            };

            if (analysisCode.AnalysisCodeId == 0)
            {
                analysisCode = DelegateService.glAccountingApplicationService.SaveAnalysisCode(analysisCode);
            }
            else
            {
                analysisCode = DelegateService.glAccountingApplicationService.UpdateAnalysisCode(analysisCode);
            }

            return Json(analysisCode.AnalysisCodeId, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// DeleteAnalysisCodes
        /// </summary>
        /// <param name="id"></param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult DeleteAnalysisCodes(int id)
        {
            try
            {
                DelegateService.glAccountingApplicationService.DeleteAnalysisCode(id);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion Actions
    }
}