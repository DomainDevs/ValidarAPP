using System;
using System.Linq;
using System.Web.Mvc;
using Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models;
using Sistran.Core.Framework.UIF2.Controls.UifTable;
using Sistran.Core.Framework.Exceptions;
using Sistran.Core.Application.GeneralLedgerServices.DTOs;
using Sistran.Core.Framework.UIF.Web.Services;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Controllers
{
    [Authorize]
    [NoDirectAccess]
    public class AnalysisConceptController : Controller
    {
            #region View

        /// <summary>
        /// AnalysisConcept
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult AnalysisConcept()
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
        /// AnalysisConceptModal
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ActionResult</returns>
        public ActionResult AnalysisConceptModal(int? id)
        {
            try
            {

                AnalysisConceptModel analysisConceptModel = new AnalysisConceptModel();
                if (id.HasValue)
                {
                    AnalysisConceptDTO analysisConcept = DelegateService.glAccountingApplicationService.GetAnalysisConcept(Convert.ToInt32(id));
                    analysisConceptModel.AnalysisConceptId = analysisConcept.AnalysisConceptId;
                    analysisConceptModel.AnalysisTreatmentId = analysisConcept.AnalysisTreatment.AnalysisTreatmentId;
                    analysisConceptModel.Description = analysisConcept.Description;
                }

                return View(analysisConceptModel);

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        /// <summary>
        /// MainAnalysisConceptKey
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainAnalysisConceptKey()
        {
            try
            {
                //valida que el servicio este arriba
                var moduleDates = DelegateService.tempCommonService.GetModuleDates();

                return View("~/Areas/GeneralLedger/Views/AnalysisConceptKey/MainAnalysisConceptKey.cshtml");
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }            
        }

        #endregion View

        #region Actions

        /// <summary>
        /// GetAnalysisConcept
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetAnalysisConcept()
        {
            var analysisConcepts = DelegateService.glAccountingApplicationService.GetAnalysisConcepts();

            var jsonData = from analysisConcept in analysisConcepts
                           select new
                           {
                               AnalysisConceptId = analysisConcept.AnalysisConceptId,
                               Description = analysisConcept.Description,
                               AnalysisTreatmentId = analysisConcept.AnalysisTreatment.AnalysisTreatmentId,
                               AnalysisTreatmentDescription = DelegateService.glAccountingApplicationService.GetAnalysisTreatment
                               (analysisConcept.AnalysisTreatment.AnalysisTreatmentId).Description
                           };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// SaveAnalysisConcept
        /// </summary>
        /// <param name="analysisConceptModel"></param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult SaveAnalysisConcept(AnalysisConceptModel analysisConceptModel)
        {
            bool isSucessfully = false;
            int saved = 0;

            AnalysisConceptDTO analysisConcept = new AnalysisConceptDTO();

            try
            {
                analysisConcept.AnalysisConceptId = analysisConceptModel.AnalysisConceptId;
                analysisConcept.Description = analysisConceptModel.Description;
                analysisConcept.AnalysisTreatment = new AnalysisTreatmentDTO();
                analysisConcept.AnalysisTreatment.AnalysisTreatmentId = analysisConceptModel.AnalysisTreatmentId;
                analysisConcept.AnalysisCode = new AnalysisCodeDTO();

                if (analysisConcept.AnalysisConceptId == 0)
                {
                    analysisConcept = DelegateService.glAccountingApplicationService.SaveAnalysisConcept(analysisConcept);
                }
                else
                {
                    analysisConcept = DelegateService.glAccountingApplicationService.UpdateAnalysisConcept(analysisConcept);
                }

                isSucessfully = true;
                saved = analysisConcept.AnalysisConceptId;
            }
            catch (Exception)
            {
                isSucessfully = false;
                saved = 0;
            }

            return Json(new { success = isSucessfully, result = saved }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// DeleteAnalysisConcept
        /// </summary>
        /// <param name="id"></param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult DeleteAnalysisConcept(int id)
        {
            bool isSucessfully = false;
            int deleted = 0;

            try
            {
                DelegateService.glAccountingApplicationService.DeleteAnalysisConcept(id);
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


        #region ActionAnalysisConceptKeys
        /// <summary>
        /// GetAnalysisConceptKeys
        /// </summary>
        /// <param name="analysisConceptId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetAnalysisConceptKeys(int analysisConceptId)
        {
            AnalysisConceptDTO analysisConcept = new AnalysisConceptDTO();
            analysisConcept.AnalysisConceptId = analysisConceptId;

            var analysisConceptKeys = DelegateService.glAccountingApplicationService.GetAnalysisConceptKeysByAnalysisConcept(analysisConcept);

            var jsonData = from analysisConceptKey in analysisConceptKeys
                           select new
                           {
                               Id = analysisConceptKey.Id,
                               AnalysisConceptId = analysisConceptId,
                               TableName = analysisConceptKey.TableName,
                               ColumnName = analysisConceptKey.ColumnName,
                               Description = analysisConceptKey.ColumnDescription
                           };
            return new UifTableResult(jsonData);
        }

        /// <summary>
        /// SaveAnalysisConceptKey
        /// </summary>
        /// <param name="analysisConceptModel"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveAnalysisConceptKey(AnalysisConceptKeyModel analysisConceptKeyModel)
        {
            bool isSucessfully = false;
            int saved = 0;
            AnalysisConceptKeyDTO analysisConceptKey = new AnalysisConceptKeyDTO();
            try
            {
                analysisConceptKey.Id = analysisConceptKeyModel.Id;
                analysisConceptKey.AnalysisConcept = new AnalysisConceptDTO();
                analysisConceptKey.AnalysisConcept.AnalysisConceptId = analysisConceptKeyModel.AnalysisConceptId;
                analysisConceptKey.TableName = analysisConceptKeyModel.TableName;
                analysisConceptKey.ColumnName = analysisConceptKeyModel.ColumnName;
                analysisConceptKey.ColumnDescription = analysisConceptKeyModel.Description;

                if (analysisConceptKey.Id == 0)
                {
                    analysisConceptKey = DelegateService.glAccountingApplicationService.SaveAnalysisConceptKey(analysisConceptKey);
                }
                else
                {
                    analysisConceptKey = DelegateService.glAccountingApplicationService.UpdateAnalysisConceptKey(analysisConceptKey);
                }

                isSucessfully = true;
                saved = analysisConceptKey.Id;
            }
            catch (Exception)
            {
                isSucessfully = false;
                saved = 0;
            }

            return Json(new { success = isSucessfully, result = saved}, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// DeleteAnalysisConceptKey
        /// </summary>
        /// <param name="analysisConceptKeyId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult DeleteAnalysisConceptKey(int analysisConceptKeyId, int analysisConceptId)
        {
            bool isSucessfully = false;
            int deleted = 0;

            try
            {
                AnalysisConceptKeyDTO analysisConceptKey = new AnalysisConceptKeyDTO();
                analysisConceptKey.Id = analysisConceptKeyId;

                AnalysisConceptDTO analysisConcept = new AnalysisConceptDTO();
                analysisConcept.AnalysisConceptId = analysisConceptId;
                analysisConceptKey.AnalysisConcept = analysisConcept;
                //   var analysisConceptKeys = DelegateService.glAccountingApplicationService.GetAnalysisConceptKey(analysisConceptKey);
                DelegateService.glAccountingApplicationService.DeleteAnalysisConceptKey(analysisConceptKey);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="analysisConceptKeyModel"></param>
        /// <returns>JsonResult</returns>        
        public JsonResult GetAnalysisConceptKey(AnalysisConceptKeyModel analysisConceptKeyModel)
        {

            AnalysisConceptKeyDTO analysisConceptKey = new AnalysisConceptKeyDTO();
            analysisConceptKey.Id = analysisConceptKeyModel.Id;
            analysisConceptKey.AnalysisConcept.AnalysisConceptId = analysisConceptKeyModel.AnalysisConceptId;
            analysisConceptKey.TableName = analysisConceptKeyModel.TableName;
            analysisConceptKey.ColumnName = analysisConceptKeyModel.ColumnName;
            analysisConceptKey.ColumnDescription = analysisConceptKeyModel.Description;

            var resultModel = DelegateService.glAccountingApplicationService.GetAnalysisConceptKey(analysisConceptKey);
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetAnalysisConceptByCodeId
        /// </summary>
        /// <param name="analysisConceptModel"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetAnalysisConceptByCodeId(AnalysisConceptDTO analysisConceptModel)
        {
            var analysisConceptKeys = (from items in DelegateService.glAccountingApplicationService.GetAnalysisConceptKeysByAnalysisConcept(analysisConceptModel)
                                       select new
                                       {
                                           Id = items.Id,
                                           AnalysisConceptId = items.AnalysisConcept.AnalysisConceptId,
                                           TableName = items.TableName,
                                           ColumnName = items.ColumnName,
                                           ColumnDescription = items.ColumnDescription
                                       }).ToList();

            return Json(analysisConceptKeys, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion Actions
    }
}