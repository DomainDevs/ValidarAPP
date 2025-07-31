using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models;
using Sistran.Core.Framework.Exceptions;
using Sistran.Core.Application.GeneralLedgerServices.DTOs.AccountingConcepts;
using Sistran.Core.Framework.UIF.Web.Services;
namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Controllers
{
    public class ConceptSourceController : Controller
    {
        #region View

        /// <summary>
        /// MainConceptSource
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainConceptSource()
        {
            try
            {
                //valida que el servicio este arriba
                var moduleDates = DelegateService.tempCommonService.GetModuleDates();

                return View("~/Areas/GeneralLedger/Views/ConceptSource/MainConceptSource.cshtml");
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// GetConceptSources
        /// Obtiene los origenes de conceptos
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetConceptSources()
        {
            List<object> conceptSourcesResponse = new List<object>();

            List<ConceptSourceDTO> conceptSources = DelegateService.glAccountingApplicationService.GetConceptSources();

            foreach (ConceptSourceDTO items in conceptSources)
            {
                conceptSourcesResponse.Add(new
                {
                    Id = items.Id,
                    Description = items.Description
                });
            }

            return Json(conceptSourcesResponse, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// SaveConceptSource
        /// Guardar o Actualiza un registro de Origen de  concepto
        /// </summary>
        /// <param name="conceptSourceModel"></param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveConceptSource(ConceptSourceModel conceptSourceModel)
        {

            bool isSucessfully = false;
            int saved = 0;
            var message = "";
            ConceptSourceDTO conceptSource = new ConceptSourceDTO();

            try
            {
                conceptSource.Id = conceptSourceModel.Id;
                conceptSource.Description = conceptSourceModel.Description;

                if (conceptSource.Id == 0)
                {
                    conceptSource = DelegateService.glAccountingApplicationService.SaveConceptSource(conceptSource);
                }
                else
                {
                    conceptSource = DelegateService.glAccountingApplicationService.UpdateConceptSource(conceptSource);
                }

                isSucessfully = true;
                saved = conceptSource.Id;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                isSucessfully = false;
                saved = 0;
            }
            return Json(new { success = isSucessfully, result = saved, msg = message }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// DeleteConceptSource
        /// Elimina un registro de Origen de concepto
        /// </summary>
        /// <param name="conceptSourceId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult DeleteConceptSource(ConceptSourceModel conceptSourceModel)
        {
            bool isSucessfully = false;
            int deleted = 0;
            var message = "";

            try
            {
                ConceptSourceDTO conceptSource = new ConceptSourceDTO() { Id = conceptSourceModel.Id };
                isSucessfully = DelegateService.glAccountingApplicationService.DeleteConceptSource(conceptSource);
                deleted = 1;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                isSucessfully = false;
                deleted = 0;
            }

            return Json(new { success = isSucessfully, result = deleted, msg = message }, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}