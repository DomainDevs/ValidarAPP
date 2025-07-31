using System;
using System.Linq;
using System.Web.Mvc;
using Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models;
using Sistran.Core.Framework.Exceptions;
using Sistran.Core.Application.GeneralLedgerServices.DTOs;
using Sistran.Core.Framework.UIF.Web.Services;
namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Controllers
{
    [Authorize]
    [NoDirectAccess]
    public class DestinationController : Controller
    {
        #region View

        /// <summary>
        /// Destination
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult Destination()
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

        #region Methods

        /// <summary>
        /// GetDestinations
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetDestinations()
        {
            var destinations = DelegateService.glAccountingApplicationService.GetEntryDestinations();

            var jsonData = from destination in destinations
                           select new
                           {
                               DestinationId = destination.DestinationId,
                               Description = destination.Description
                           };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// DestinationModal
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ActionResult</returns>
        public ActionResult DestinationModal(int? id)
        {
            EntryDestinationDTO destination = new EntryDestinationDTO();
            DestinationModel destinationModel = new DestinationModel();

            if (id.HasValue)
            {
                destination.DestinationId = Convert.ToInt32(id);
                destination = DelegateService.glAccountingApplicationService.GetDestination(destination);

                destinationModel.DestinationId = destination.DestinationId;
                destinationModel.Description = destination.Description;
            }

            return View(destinationModel);
        }

        /// <summary>
        /// SaveDestination
        /// </summary>
        /// <param name="model"></param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult SaveDestination(EntryDestinationDTO destination)
        {
            try
            {
                if (destination.DestinationId == 0)
                {
                    destination = DelegateService.glAccountingApplicationService.SaveEntryDestination(destination);
                }
                else
                {
                    destination = DelegateService.glAccountingApplicationService.UpdateEntryDestination(destination);
                }

                return Json(new { success = true, result = destination.DestinationId }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// DeleteDestination
        /// </summary>
        /// <param name="id"></param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult DeleteDestination(int id)
        {
            try
            {
                DelegateService.glAccountingApplicationService.DeleteEntryDestination(id);
                return Json(new { success = true, result = id }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion Methods

    }
}