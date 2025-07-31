//TODO CÓDIGO/PANTALLAS DE RETENCIONES FUE MIGRADO DESDE BE ESTÀ PENDIENTE EN EE IMPLEMENTAR ESTA FUNCIONALIDAD
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

// Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Framework.UIF2.Controls.UifTable;
using Sistran.Core.Framework.UIF2.Services;
using Sistran.Core.Framework.Exceptions;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Application.AccountingServices.DTOs.Retentions;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers.Parameters
{
    [Authorize]
    [HandleError]
    [NoDirectAccess]
    public class RetentionController : Controller
    {
        #region View

        /// <summary>
        /// MainRetentionConcept
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainRetentionConcept()
        {
            try
            {
                return View("~/Areas/Accounting/Views/Parameters/Retention/MainRetentionConcept.cshtml");
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }           
        }

        /// <summary>
        /// GetRetentionConcepts
        /// Obtiene los conceptos de retención
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetRetentionConcepts()
        {
            List<object> retentionConceptsResponses = new List<object>();
            var retentionConcepts = DelegateService.accountingRetentionService.GetRetentionConcepts();
            
            foreach (RetentionConceptDTO retentionConcept in retentionConcepts)
            {
                retentionConceptsResponses.Add(new
                {
                    Id = retentionConcept.Id,
                    Description = retentionConcept.Description,
                    RetentionBase = DelegateService.accountingRetentionService.GetRetentionBases().Where(r => (r.Id.Equals(retentionConcept.RetentionBase.Id))).ToList()[0].Description,
                    RetentionBaseId = retentionConcept.RetentionBase.Id,
                    Status = retentionConcept.Status==1 ? "SI":"NO",
                    StatusId = retentionConcept.Status==0? 0:1,
                    DifferenceAmount = retentionConcept.DifferenceAmount
                });
            }

            return new UifTableResult(retentionConceptsResponses);
        }

        /// <summary>
        /// GetRetentionConceptPercentages
        /// Obtiene los porcentajes de los conceptos de retención
        /// </summary>
        /// <param name="retentionConceptId"></param>
        /// <returns>ActionResult<RetentionConceptPercentage/></returns>
        public ActionResult GetRetentionConceptPercentages(int retentionConceptId)
        {
            RetentionConceptDTO retentionConcept = new RetentionConceptDTO();

            retentionConcept.Id = retentionConceptId;

            List<object> retentionConceptPercentageResponses = new List<object>();

            var retentionConceptPercentages = DelegateService.accountingRetentionService.GetRetentionConceptPercentages(retentionConcept).Where(rp => rp.RetentionConcept.Id.Equals(retentionConceptId)).ToList();

            foreach (RetentionConceptPercentageDTO retentionConceptPercentage in retentionConceptPercentages)
            {
                retentionConceptPercentageResponses.Add(new
                {
                    DateFrom = retentionConceptPercentage.DateFrom.ToShortDateString(),
                    DateTo = retentionConceptPercentage.DateTo.ToShortDateString(),
                    Id = retentionConceptPercentage.Id,
                    Percentage = retentionConceptPercentage.Percentage,
                    RetentionConcept = retentionConceptPercentage.RetentionConcept.Id,
                    ExternalCode = retentionConceptPercentage.ExternalCode
                });
            }

            return new UifTableResult(retentionConceptPercentageResponses);
        }

        #endregion

        #region RetentionBase

        /// <summary>
        /// RetentionBase
        /// Muestra la pantalla principal de Base de retenciones
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult RetentionBase()
        {
            try
            {
                return View("~/Areas/Accounting/Views/Parameters/Retention/RetentionBase.cshtml");
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }            
        }

        ///// <summary>
        ///// GetRetentionBases
        ///// Obtiene los conceptos de ingreso
        ///// </summary>
        ///// <returns>Json</returns>
        //public List<RetentionBase> GetRetentionBases()
        //{
        //    List<object> incomeBases = new List<object>();
        //    var retentionBases = DelegateService.accountingRetentionService.GetRetentionBases();

        //    foreach (AccountingModels.RetentionBase retentionBase in retentionBases)
        //    {
        //        incomeBases.Add(new
        //        {
        //            Id = retentionBase.Id,
        //            Description = retentionBase.Description.ToUpper()

        //        });
        //    }

        //    return Json(incomeBases, JsonRequestBehavior.AllowGet);
        //}

        /// <summary>
        /// RetentionBase
        /// Guarda un nuevo registro base de retenciones
        /// </summary>
        /// <param name="retentionBase"></param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult RetentionBase(RetentionBaseDTO retentionBase)
        {
            try
            {
                var isSavedRetentionBase = DelegateService.accountingRetentionService.SaveRetentionBase(retentionBase);

                return Json(new { success = true, isSavedRetentionBase }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// UpdateRetentionBase
        /// Actualiza un registro de base de retenciones
        /// </summary>
        /// <param name="retentionBase"></param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult UpdateRetentionBase(RetentionBaseDTO retentionBase)
        {
            try
            {
                var isUpdateRetentionBase = DelegateService.accountingRetentionService.UpdateRetentionBase(retentionBase);
                return Json(new { success = true, result = isUpdateRetentionBase }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// DeleteRetentionBase
        /// Elimina un registro de base de retenciones
        /// </summary>
        /// <param name="retentionBase"></param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult DeleteRetentionBase(RetentionBaseDTO retentionBase)
        {
            try
            {
                DelegateService.accountingRetentionService.DeleteRetentionBase(retentionBase);
                return Json(new { success = true, result = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// GetRetentionBase
        /// Obtiene la base de retenciones
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetRetentionBase()
        {
            var retentionBases = DelegateService.accountingRetentionService.GetRetentionBases();

            var retentionBasesResponse = from retentionBase in retentionBases
                           select new
                           {
                               Id = retentionBase.Id,
                               Description = retentionBase.Description.ToUpper()
                           };

            return Json(retentionBasesResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion RetentionBase

        #region RetentionConcept

        /// <summary>
        /// GetTaxBase
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetTaxBase()
        {
            List<RetentionBaseDTO> retentionBases = DelegateService.accountingRetentionService.GetRetentionBases();

            var retentionBasesResponse = (from retentionBase in retentionBases
                        select new
                        {
                            retentionBase.Id,
                            retentionBase.Description
                        }).OrderBy(x => x.Id).ToList();

            return new UifSelectResult(retentionBasesResponse);
        }

        /// <summary>
        /// GetRetentionConcept
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetRetentionConcept()
        {
            List<RetentionConceptDTO> retentionConcepts = DelegateService.accountingRetentionService.GetRetentionConcepts();

            var retentionConceptsResponse = (from retentionConcept in retentionConcepts
                        select new
                        {
                            retentionConcept.Id,
                            retentionConcept.Description
                        }).OrderBy(x => x.Description).ToList();

            return new UifSelectResult(retentionConceptsResponse);
        }

        /// <summary>
        /// GetActive
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetActive()
        {
            List<object> actives = new List<object>();

            actives.Add(new
            {
                Id = 1,
                Description = "SI"
            });
            actives.Add(new
            {
                Id = 0,
                Description = "NO"
            });

            return new UifSelectResult(actives);
        }

        /// <summary>
        /// SaveRetentionConcept
        /// Graba un nuevo registro en concepto de retenciones
        /// </summary>
        /// <param name="retentionConcept"></param>
        /// <param name="operationType"></param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult SaveRetentionConcept(RetentionConceptDTO retentionConcept, string operationType)
        {
            try
            {
                var isSavedRetentionConcept = DelegateService.accountingRetentionService.SaveRetentionConcept(retentionConcept);

                return Json(new { success = true, result = isSavedRetentionConcept }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException businessException)
            {
                return Json(new { success = false, result = businessException.ExceptionMessages }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// UpdateRetentionConcept
        /// Actualiza un registro de concepto de retenciones
        /// </summary>
        /// <param name="retentionConcept"></param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult UpdateRetentionConcept(RetentionConceptDTO retentionConcept)
        {
            try
            {
                var isUpdateRetentionConcept = DelegateService.accountingRetentionService.UpdateRetentionConcept(retentionConcept);

                return Json(new { success = true, result = isUpdateRetentionConcept }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// DeleteRetentionConcept
        /// Elimina un registro de concepto de retenciones
        /// </summary>
        /// <param name="rententionConcept"></param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult DeleteRetentionConcept(RetentionConceptDTO rententionConcept)
        {
            try
            {
                DelegateService.accountingRetentionService.DeleteRetentionConcept(rententionConcept);
                return Json(new { success = true, result = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// SaveRetentionConceptPercentage
        /// Graba un nuevo porcenatje de un concepto de retención
        /// </summary>
        /// <param name="retentionConceptPercentage"></param>
        /// <param name="operationType"></param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult SaveRetentionConceptPercentage(RetentionConceptPercentageDTO retentionConceptPercentage, string operationType)
        {
            try
            {
                var isSavedRetentionConceptPercentage = DelegateService.accountingRetentionService.SaveRetentionConceptPercentage(retentionConceptPercentage);
                return Json(new {success = true, result = isSavedRetentionConceptPercentage }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// UpdateRetentionConceptPercentage
        /// Actualiza un porcentaje de un concepto de retención
        /// </summary>
        /// <param name="retentionConceptPercentage"></param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult UpdateRetentionConceptPercentage(RetentionConceptPercentageDTO retentionConceptPercentage)
        {
            try
            {
                var isUpdatedRetentionConceptPercentage = DelegateService.accountingRetentionService.UpdateRetentionConceptPercentage(retentionConceptPercentage);
                return Json(new { success = true, isUpdatedRetentionConceptPercentage }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion RetentionConcept

    }

}