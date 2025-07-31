using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Framework.Exceptions;
using Sistran.Core.Application.GeneralLedgerServices.DTOs.AccountingConcepts;
using Sistran.Core.Framework.UIF.Web.Services;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Controllers
{
    public class MovementTypeController : Controller
    {
        #region View

        /// <summary>
        /// MainMovementType
        /// LLamada a View 
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainMovementType()
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

        #endregion

        #region Public Methods

        /// <summary>
        /// GetConceptSources
        /// Llena Combo
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetConceptSources()
        {
            return new UifSelectResult(DelegateService.glAccountingApplicationService.GetConceptSources().OrderBy(cs => cs.Description));
        }

        /// <summary>
        /// GetMovementTypesByConceptSourceId
        /// Obtiene los tipos de moviemiento por el id concepto origen
        /// </summary>
        /// <param name="movementTypesId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetMovementTypesByConceptSourceId(int conceptSourceId)
        {
            ConceptSourceDTO conceptSource = new ConceptSourceDTO() { Id = conceptSourceId };

            List<MovementTypeDTO> movementTypes = DelegateService.glAccountingApplicationService.GetMovementTypesByConceptSource(conceptSource);
            var movementTypesResponse = (from itemsTypes in movementTypes
                                         select new
                                         {
                                             Id = itemsTypes.Id,
                                             ConceptSourceId = conceptSourceId,
                                             Description = itemsTypes.Description
                                         }).ToList();

            return Json(movementTypesResponse, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// SaveMovementType
        /// Guardar o Actualiza un registro de Tipo de Movimiento
        /// </summary>
        /// <param name="movementTypeModel">Modelo</param>
        /// <returns>JsonResult</returns>
        public JsonResult SaveMovementType(MovementTypeModel movementTypeModel)
        {

            bool isSucessfully = false;
            int saved = 0;
            string message = "";
            MovementTypeDTO movementType = new MovementTypeDTO();

            try
            {
                movementType.Id = movementTypeModel.Id;
                movementType.ConceptSource = new ConceptSourceDTO();
                movementType.ConceptSource.Id = movementTypeModel.ConceptSourceId;
                movementType.Description = movementTypeModel.Description;

                if (movementType.Id == 0)
                {
                    movementType = DelegateService.glAccountingApplicationService.SaveMovementType(movementType);
                }
                else
                {
                    movementType = DelegateService.glAccountingApplicationService.UpdateMovementType(movementType);
                }

                isSucessfully = true;
                saved = movementType.Id;
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
        /// DeleteMovementType
        /// </summary>
        /// <param name="movementTypeModel"></param>
        /// <returns>JsonResult</returns>
        public JsonResult DeleteMovementType(MovementTypeModel movementTypeModel)
        {
            bool isSucessfully = false;
            int deleted = 0;
            string message = "";

            try
            {
                MovementTypeDTO movementType = new MovementTypeDTO();
                movementType.Id = movementTypeModel.Id;
                movementType.ConceptSource = new ConceptSourceDTO();
                movementType.ConceptSource.Id = movementTypeModel.ConceptSourceId;

                isSucessfully = DelegateService.glAccountingApplicationService.DeleteMovementType(movementType);
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