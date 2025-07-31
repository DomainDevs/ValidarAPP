using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.Utilities.Helper;

namespace Sistran.Core.Framework.UIF.Web.Areas.Massive.Controllers
{
    [Authorize]
    public class MassiveProcessController : Controller
    {
        public ActionResult MassiveProcess()
        {
            return View();
        }

        public PartialViewResult SearchAdvancedMassiveProcess()
        {
            return PartialView();
        }

        private List<MassiveLoad> GetMassiveLoadsByRange(string rangeFrom, string rangeTo, MassiveLoad massiveLoad)
        {
            DateTime dateRangeFrom = new DateTime();
            DateTime dateRangeTo = new DateTime();

            if (string.IsNullOrEmpty(rangeFrom) && string.IsNullOrEmpty(rangeTo))
            {
                dateRangeFrom = DateTime.Now;
                dateRangeTo = DateTime.Now.AddDays(1);
            }
            else if (!string.IsNullOrEmpty(rangeFrom) && string.IsNullOrEmpty(rangeTo))
            {
                dateRangeFrom = Convert.ToDateTime(rangeFrom);
                dateRangeTo = dateRangeFrom.AddDays(1);
            }
            else
            {
                dateRangeFrom = Convert.ToDateTime(rangeFrom);
                dateRangeTo = Convert.ToDateTime(rangeTo).AddDays(1);
            }
            return DelegateService.massiveService.GetMassiveLoadsByRangeFromRangeToMassiveLoad(dateRangeFrom, dateRangeTo, massiveLoad).OrderBy(x => x.Id).ToList();

        }

        /// <summary>
        /// Búsqueda avanzada, filtra por fecha 
        /// </summary>
        /// <param name="fromDateView"></param>
        /// <param name="toDateView"></param>
        /// <param name="massiveLoad"></param>
        /// <returns></returns>
        public ActionResult GetMassiveProcessesAdvancedSearch(string rangeFrom, string rangeTo, MassiveLoad massiveLoad)
        {
            try
            {
                List<MassiveLoad> massiveLoads = GetMassiveLoadsByRange(rangeFrom, rangeTo, massiveLoad);
                if (massiveLoads.Count > 0)
                {
                    foreach (MassiveLoad item in massiveLoads)
                    {
                        item.LoadType.ProcessTypeDescription = EnumsHelper.GetItemName<MassiveProcessType>(item.LoadType.ProcessType);
                    }

                    return new UifJsonResult(true, massiveLoads);
                }
                else
                {
                    return new UifJsonResult(false, "no se encontraron cargues");
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetMassiveProcesses);
            }
        }

        /// <summary>
        /// Obtiene los tipos de Procesos
        /// </summary>
        /// <returns>Lista de tipos de campo del enum MassiveProcessType</returns>
        public ActionResult GetProcessType()
        {
            try
            {
                return new UifJsonResult(true, EnumsHelper.GetItems<MassiveProcessType>());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetProcessType);
            }
        }

        /// <summary>
        /// Obtiene los tipos de Estados
        /// </summary>
        /// <returns>Lista de tipos de campo del enum MassiveLoadStatus</returns>
        public ActionResult GetStatusProcess()
        {
            try
            {
                return new UifJsonResult(true, EnumsHelper.GetItems<MassiveLoadStatus>());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetLoadStatus);
            }
        }

        /// <summary>
        /// Obtiene una lista de usuarios 
        /// </summary>
        /// <returns>Lista de Usuarios</returns>
        public ActionResult GetUsers()
        {
            try
            {
                User user = new User();
                user.CreatedUserId = 2;
                List<User> usersList = DelegateService.uniqueUserService.GetUsersByUser(user);
                return new UifJsonResult(true, usersList);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetUsers);
            }
        }

        public ActionResult GenerateMassiveProcessReport(string rangeFrom, string rangeTo, MassiveLoad massiveLoad)
        {
            try
            {
                List<MassiveLoad> massiveLoads = GetMassiveLoadsByRange(rangeFrom, rangeTo, massiveLoad);

                if (massiveLoads.Count > 0)
                {
                    foreach (MassiveLoad item in massiveLoads)
                    {
                        item.LoadType.ProcessTypeDescription = EnumsHelper.GetItemName<MassiveProcessType>(item.LoadType.ProcessType);
                    }
                }

                string urlFile = "";

                urlFile = DelegateService.massiveService.GenerateMassiveProcessReport(massiveLoads);

                if (string.IsNullOrEmpty(urlFile))
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorFileNotFound);
                }
                return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile);

            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
            }
        }

    }
}