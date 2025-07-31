//System
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Threading;
using System.Globalization;

//Sitran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Framework.UIF2.Services;
using Sistran.Core.Framework.UIF.Web.Resources;

//Sistran Core
using Sistran.Core.Application.AccountingClosingServices;
using Sistran.Core.Application.TempCommonServices;
using Sistran.Core.Application.TempCommonServices.Models;

//Sistran Company
using Sistran.Company.Application.CommonServices;
using Sistran.Company.Application.UniqueUserServices;
using Sistran.Core.Framework.UIF.Web.Services;

namespace Sistran.Core.Framework.UIF.Web.Areas.AccountingClosing.Controllers
{
    public class BaseController : Controller
    {
        #region Instance Variables
        
        readonly IUniqueUserService _uniqueUserService = ServiceManager.Instance.GetService<IUniqueUserService>();
        readonly ITempCommonService _tempCommonService = ServiceManager.Instance.GetService<ITempCommonService>();

        #endregion

        #region Months

        /// <summary>
        /// GetMonthsList
        /// Carga los meses en list
        /// </summary>
        /// <returns>Year and Months</returns>
        public static List<object> GetMonthsList()
        {
            var yearMonths = new List<object>();
            int monthNumber = 0;
            string monthDescription = "";

            monthNumber++;
            monthDescription = (@Global.January);
            yearMonths.Add(new { Id = monthNumber, Description = monthDescription });

            monthNumber++;
            monthDescription = (@Global.February);
            yearMonths.Add(new { Id = monthNumber, Description = monthDescription });

            monthNumber++;
            monthDescription = (@Global.March);
            yearMonths.Add(new { Id = monthNumber, Description = monthDescription });

            monthNumber++;
            monthDescription = (@Global.April);
            yearMonths.Add(new { Id = monthNumber, Description = monthDescription });

            monthNumber++;
            monthDescription = (@Global.May);
            yearMonths.Add(new { Id = monthNumber, Description = monthDescription });

            monthNumber++;
            monthDescription = (@Global.June);
            yearMonths.Add(new { Id = monthNumber, Description = monthDescription });

            monthNumber++;
            monthDescription = (@Global.July);
            yearMonths.Add(new { Id = monthNumber, Description = monthDescription });

            monthNumber++;
            monthDescription = (@Global.August);
            yearMonths.Add(new { Id = monthNumber, Description = monthDescription });

            monthNumber++;
            monthDescription = (@Global.September);
            yearMonths.Add(new { Id = monthNumber, Description = monthDescription });

            monthNumber++;
            monthDescription = (@Global.October);
            yearMonths.Add(new { Id = monthNumber, Description = monthDescription });

            monthNumber++;
            monthDescription = (@Global.November);
            yearMonths.Add(new { Id = monthNumber, Description = monthDescription });

            monthNumber++;
            monthDescription = (@Global.December);
            yearMonths.Add(new { Id = monthNumber, Description = monthDescription });

            return yearMonths;
        }


        /// <summary>
        /// GetMonths
        /// Carga los meses en DropDownlist
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetMonths()
        {
            string urlReferrer = Request.UrlReferrer.ToString();

            if (urlReferrer.IndexOf("en") > 0)
            {
                Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-us");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture("es-GT");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-GT");
            }

            var yearMonths = new List<object>();
            int monthNumber = 0;
            string monthDescription = "";

            monthNumber++;
            monthDescription = (@Global.January);
            yearMonths.Add(new { Id = monthNumber, Description = monthDescription });

            monthNumber++;
            monthDescription = (@Global.February);
            yearMonths.Add(new { Id = monthNumber, Description = monthDescription });

            monthNumber++;
            monthDescription = (@Global.March);
            yearMonths.Add(new { Id = monthNumber, Description = monthDescription });

            monthNumber++;
            monthDescription = (@Global.April);
            yearMonths.Add(new { Id = monthNumber, Description = monthDescription });

            monthNumber++;
            monthDescription = (@Global.May);
            yearMonths.Add(new { Id = monthNumber, Description = monthDescription });

            monthNumber++;
            monthDescription = (@Global.June);
            yearMonths.Add(new { Id = monthNumber, Description = monthDescription });

            monthNumber++;
            monthDescription = (@Global.July);
            yearMonths.Add(new { Id = monthNumber, Description = monthDescription });

            monthNumber++;
            monthDescription = (@Global.August);
            yearMonths.Add(new { Id = monthNumber, Description = monthDescription });

            monthNumber++;
            monthDescription = (@Global.September);
            yearMonths.Add(new { Id = monthNumber, Description = monthDescription });

            monthNumber++;
            monthDescription = (@Global.October);
            yearMonths.Add(new { Id = monthNumber, Description = monthDescription });

            monthNumber++;
            monthDescription = (@Global.November);
            yearMonths.Add(new { Id = monthNumber, Description = monthDescription });

            monthNumber++;
            monthDescription = (@Global.December);
            yearMonths.Add(new { Id = monthNumber, Description = monthDescription });

            return new UifSelectResult(yearMonths);
        }

        #endregion Month

        #region ModuleDates

        /// <summary>
        /// GetModuleDates
        /// Obtiene el listado de ModuleDate
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetModuleDates()
        {
            try
            {
                // Filtrado para mostrar en pantalla
                string[] enableModules = ConfigurationManager.AppSettings["AccountingClosureEnableModules"].Split(';');
                List<int> ids = new List<int>();

                foreach (var module in enableModules)
                {
                    int id = Convert.ToInt32(module);
                    ids.Add(id);
                }

                // Ordenado por descripción
                var moduleDates = (from ModuleDate moduleDate in _tempCommonService.GetModuleDates() where ids.Contains(Convert.ToInt32(moduleDate.Id)) select moduleDate).OrderBy(c => c.Id);

                return new UifSelectResult(moduleDates);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// GetModuleDays
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns>days</returns>
        public ActionResult GetModuleDays(int moduleId)
        {
            List<object> days = new List<object>();

            try
            {
                DateTime closingDate = DelegateService.accountingClosingApplicationService.GetClosingDate(moduleId);
                days.Add(new { Id = closingDate.Day, Description = closingDate.Day });

                return new UifSelectResult(days);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// GetModuleDateDescriptionByModuleId
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public string GetModuleDateDescriptionByModuleId(int moduleId)
        {
            return _tempCommonService.GetModuleDates().FirstOrDefault(i => i.Id == moduleId).Description;
        }

        #endregion ModulesDates

        #region Date

        /// <summary>
        /// GetDate
        /// </summary>
        /// <returns>JsonResult</returns>
        public JsonResult GetDate()
        {
            string dateToday = String.Format("{0:dd/MM/yyyy}", DateTime.Today);

            string[] dateSplit = dateToday.Split();

            return Json(dateSplit[0], JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetAccountingDate
        /// Obtiene la fecha contable
        /// </summary>
        /// <param name="idModuleDate"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetAccountingDate(int idModuleDate)
        {
            return Json(Convert.ToString(DelegateService.accountingClosingApplicationService.GetClosingDate(idModuleDate)).Split()[0], JsonRequestBehavior.AllowGet);
        }

        #endregion Date      

        #region Branch

        /// <summary>
        /// GetBranchDescriptionByBranchId
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns>string</returns>
        public string GetBranchDescriptionByBranchId(int branchId)
        {
            return DelegateService.commonService.GetBranchById(branchId).Description;
        }

        /// <summary>
        /// GetBranchDefaultByUserId
        /// Obtiene la sucursal por defecto del usuario
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>int</returns>
        public int GetBranchDefaultByUserId(int userId)
        {
            return _uniqueUserService.GetBranchesByUserId(userId).Where(br => br.IsDefault).ToList()[0].Id;
        }

        #endregion

        #region Currency

        /// <summary>
        /// GetCurrencyDescriptionbyCurrencyId
        /// </summary>
        /// <param name="currencyId"></param>
        /// <returns></returns>
        public string GetCurrencyDescriptionByCurrencyId(int currencyId)
        {
            var currencies = DelegateService.commonService.GetCurrencies();
            var currency = currencies.Where(sl => sl.Id == currencyId).ToList();

            return currency[0].Description;
        }

        /// <summary>
        /// GetCurrencies
        /// Obtiene las descripciones de las monedas
        /// </summary>
        /// <returns>List<Currency/></returns>
        public ActionResult GetCurrencies()
        {
            return new UifSelectResult(DelegateService.commonService.GetCurrencies());
        }

        #endregion

        #region User

        /// <summary>
        /// GetUserIdByName
        /// </summary>
        /// <param name="name"></param>
        /// <returns>int</returns>
        public int GetUserIdByName(string name)
        {
            return _uniqueUserService.GetUserByName(name)[0].UserId;
        }

        #endregion User

    }
}