using System;
using System.Web.Mvc;

//Sistran FWK
using Sistran.Core.Framework.UIF.Web.Resources;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF2.Services;
using Sistran.Core.Framework.Exceptions;

//Sistran Core
using Sistran.Core.Application.GeneralLedgerServices;
using Sistran.Core.Application.TempCommonServices;
using Sistran.Core.Application.TempCommonServices.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF.Web.Helpers;

namespace Sistran.Core.Framework.UIF.Web.Areas.GeneralLedger.Controllers
{
    [Authorize]
    public class ProcessController : Controller
    {
        #region View

        /// <summary>
        /// Posting
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult Posting()
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

        #region EntryPosting

        /// <summary>
        /// LoadProcessDate
        /// </summary>
        /// <param name="moduleDateId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult LoadProcessDate(int moduleDateId)
        {
            ModuleDate moduleDate = new ModuleDate();
            moduleDate.Id = Convert.ToInt32(moduleDateId);
            moduleDate = DelegateService.tempCommonService.GetModuleDate(moduleDate);

            int year = 0;
            int month = 0;

            if (moduleDate.LastClosingMm == 12)
            {
                year = moduleDate.LastClosingYyyy + 1;
                month = 1;
            }
            else
            {
                year = moduleDate.LastClosingYyyy;
                month = moduleDate.LastClosingMm + 1;
            }

            int numberOfDays = DateTime.DaysInMonth(year, month);
            DateTime date = new DateTime(year, month, numberOfDays);

            return new UifJsonResult(true, date);
        }

        /// <summary>
        /// Método para validar la fecha de cierre del mes en la mayorización de asientos
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ValidateClosureDate(int year, int month)
        {
            bool isValidated = false;

            int closureNumberOfDays = DateTime.DaysInMonth(year, month);
            string date = Convert.ToString(closureNumberOfDays) + "/" + Convert.ToString(month) + "/" + Convert.ToString(year) + " 23:59:59";
            DateTime closureDate = Convert.ToDateTime(date);

            DateTime currenDate = DateTime.Now;

            if (closureDate > currenDate)
            {
                isValidated = false;
            }

            if (closureDate < currenDate)
            {
                isValidated = true;
            }

            return Json(isValidated, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// SaveProcessEntries
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="isClosure"></param>
        /// <returns>ActionResult</returns>
        public ActionResult SaveProcessEntries(int year, int month, int isClosure)
        {
            int saved = 0;

            try
            {
                int userId = SessionHelper.GetUserId();

                DateTime currentTime = DateTime.Now;
                int numberOfDays = DateTime.DaysInMonth(year, month);
                DateTime requestDate = new DateTime(year, month, numberOfDays);
                DateTime resultDate;

                if (currentTime > requestDate)
                {
                    resultDate = requestDate;
                }
                else
                {
                    resultDate = currentTime;
                }

                saved = DelegateService.glAccountingApplicationService.SaveProcessEntries(year, month, userId, resultDate, isClosure);

                // Se actualiza el moduleDate
                if (saved > 0 && isClosure == 1)
                {
                    ModuleDate module = new ModuleDate()
                    {
                        Id = Convert.ToInt32(Global.BalanceClousingModuleDateId) //Cierre de Balance
                    };
                    module = DelegateService.tempCommonService.GetModuleDate(module);

                    module.CeilingYyyy = year;
                    module.CeilingMm = month;
                    module.LastClosingMm = month;
                    module.LastClosingYyyy = year;
                    module.OfflineCeilingMm = month;
                    module.OfflineCeilingYyyy = year;

                    DelegateService.tempCommonService.UpdateModuleDate(module);
                }

                return new UifJsonResult(true, saved);
            }
            catch
            {
                return new UifJsonResult(false, saved);
            }
        }

        /// <summary>
        /// SaveAutomaticLedgerEntry
        /// </summary>
        /// <param name="module"></param>
        /// <param name="moduleDescription"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="isClosure"></param>
        /// <returns>ActionResult</returns>
        public ActionResult SaveAutomaticLedgerEntry(int moduleId, string moduleDescription, int year, int month, int day, int isClosure)
        {
            int saved = 0;

            try
            {
                int userId = SessionHelper.GetUserId();
                DateTime closingDate = new DateTime(year, month, day);

                saved = DelegateService.glAccountingApplicationService.SaveAutomaticLedgerEntry(moduleId, closingDate, userId);

                // Se actualiza el moduleDate
                if (saved > 0 && isClosure == 1)
                {
                    ModuleDate module = new ModuleDate()
                    {
                        Id = moduleId
                    };

                    module = DelegateService.tempCommonService.GetModuleDate(module);

                    module.CeilingYyyy = year;
                    module.CeilingMm = month;
                    module.LastClosingMm = month;
                    module.LastClosingYyyy = year;
                    module.OfflineCeilingMm = month;
                    module.OfflineCeilingYyyy = year;

                    DelegateService.tempCommonService.UpdateModuleDate(module);
                }

                return new UifJsonResult(true, saved);
            }
            catch
            {
                return new UifJsonResult(false, saved);
            }
        }

        #endregion EntryPosting
    }
}