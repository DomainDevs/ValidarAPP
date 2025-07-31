using System;
using System.Linq;
using System.Web.Mvc;
using Sistran.Core.Framework.UIF.Web.Resources;
using Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Models;
using Sistran.Core.Framework.Exceptions;
using Sistran.Core.Application.GeneralLedgerServices.DTOs;
using Sistran.Core.Framework.UIF.Web.Services;
namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Controllers
{
    [Authorize]
    [NoDirectAccess]
    public class CostCenterController : Controller
    {
        #region CostCenter

        /// <summary>
        /// CostCenter
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult CostCenter()
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
        /// GetCostCenters
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetCostCenters()
        {
            var costCenters = DelegateService.glAccountingApplicationService.GetCostCenters();

            var jsonData = from costCenter in costCenters
                           select new
                           {
                               Id = costCenter.CostCenterId,
                               Description = costCenter.Description,
                               IsProrated = costCenter.IsProrated,
                               IsProratedDescription = costCenter.IsProrated ? Global.Yes : Global.No,
                               CostCenterTypeId = costCenter.CostCenterType.CostCenterTypeId,
                               CostCenterTypeDescription = costCenter.CostCenterType.Description
                           };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// CostCenterModal
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ActionResult</returns>
        public ActionResult CostCenterModal(int? id)
        {
            var costCenterModel = new CostCenterModel();

            if (id.HasValue)
            {
                ViewBag.CostCenterTypeId = new SelectList(DelegateService.glAccountingApplicationService.GetCostCenterTypes(), "CostCenterTypeId", "Description", costCenterModel.CostCenterTypeId);

                var costCenter = new CostCenterDTO();
                costCenter.CostCenterId = id.Value;
                costCenter = DelegateService.glAccountingApplicationService.GetCostCenter(costCenter);

                costCenterModel.Id = costCenter.CostCenterId;
                costCenterModel.Description = costCenter.Description;
                costCenterModel.IsProrated = costCenter.IsProrated;
                costCenterModel.CostCenterTypeId = costCenter.CostCenterType.CostCenterTypeId;
            }
            else
            {
                ViewBag.CostCenterTypeId = new SelectList(DelegateService.glAccountingApplicationService.GetCostCenterTypes(), "CostCenterTypeId", "Description");
            }

            return View(costCenterModel);
        }

        /// <summary>
        /// SaveCostCenter
        /// </summary>
        /// <param name="costCenterModel"></param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult SaveCostCenter(CostCenterModel costCenterModel)
        {
            var costCenter = new CostCenterDTO()
            {
                CostCenterId = costCenterModel.Id,
                Description = costCenterModel.Description,
                IsProrated = Convert.ToBoolean(costCenterModel.IsProrated),
                CostCenterType = new CostCenterTypeDTO()
                {
                    CostCenterTypeId = costCenterModel.CostCenterTypeId
                },
            };

            if (costCenter.CostCenterId == 0)
            {
                costCenter = DelegateService.glAccountingApplicationService.SaveCostCenter(costCenter);
            }
            else
            {
                costCenter = DelegateService.glAccountingApplicationService.UpdateCostCenter(costCenter);
            }

            return Json(costCenter.CostCenterId, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// DeleteCostCenter
        /// </summary>
        /// <param name="id"></param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult DeleteCostCenter(int id)
        {
            try
            {
                return Json(DelegateService.glAccountingApplicationService.DeleteCostCenter(id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion CostCenter
    }
}