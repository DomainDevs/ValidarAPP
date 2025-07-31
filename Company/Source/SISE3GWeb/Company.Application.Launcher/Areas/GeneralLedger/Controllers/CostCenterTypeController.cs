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
    public class CostCenterTypeController : Controller
    {
        #region CostCenterType

        /// <summary>
        /// CostCenterType
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult CostCenterType()
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
        /// GetCostCentersTypes
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetCostCentersTypes()
        {
            var costCenterTypes = DelegateService.glAccountingApplicationService.GetCostCenterTypes();

            var costCentersTypes = from costCenterType in costCenterTypes
                                   select new
                                   {
                                       CostCenterTypeId = costCenterType.CostCenterTypeId,
                                       Description = costCenterType.Description
                                   };

            return Json(costCentersTypes, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// CostCenterTypeModal
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ActionResult</returns>
        public ActionResult CostCenterTypeModal(int? id)
        {
            CostCenterTypeModel costCenterTypeModel = new CostCenterTypeModel();
            if (id.HasValue)
            {
                CostCenterTypeDTO costCenterType = DelegateService.glAccountingApplicationService.GetCostCenterTypeById(Convert.ToInt32(id));
                costCenterTypeModel.CostCenterTypeId = costCenterType.CostCenterTypeId;
                costCenterTypeModel.Description = costCenterType.Description;
            }

            return View(costCenterTypeModel);
        }

        /// <summary>
        /// SaveCostCenterType
        /// </summary>
        /// <param name="costCenterType"></param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult SaveCostCenterType(CostCenterTypeDTO costCenterType)
        {
            try
            {
                if (costCenterType.CostCenterTypeId == 0)
                {
                    DelegateService.glAccountingApplicationService.SaveCostCenterType(costCenterType);
                }
                else
                {
                    DelegateService.glAccountingApplicationService.UpdateCostCenterType(costCenterType);
                }
                return Json(new { success = true, result = costCenterType.CostCenterTypeId }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { success = false, result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// DeleteCostCenterType
        /// </summary>
        /// <param name="id"></param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult DeleteCostCenterType(int id)
        {
            bool isSucessfully = false;
            bool isDeleted = false;

            try
            {
                isDeleted = DelegateService.glAccountingApplicationService.DeleteCostCenterType(id);
                isSucessfully = true;
            }
            catch (Exception)
            {
                isDeleted = false;
                isSucessfully = false;
            }

            return Json(new { success = isSucessfully, result = isDeleted }, JsonRequestBehavior.AllowGet);
        }

        #endregion CostCenterType
    }
}