using Sistran.Core.Application.AuditServices.Enums;
using Sistran.Core.Application.ModelServices.Enums;
using Sistran.Core.Application.ModelServices.Models.Audit;
using Sistran.Core.Application.ModelServices.Models.Param;
using Sistran.Core.Framework.UIF.Web.Areas.Audit.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Helpers.Enums;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using ENUMSM = Sistran.Core.Application.ModelServices.Enums;
namespace Sistran.Core.Framework.UIF.Web.Areas.Audit.Controllers
{
    public class AuditController : Controller
    {
        [NoDirectAccess]
        public ActionResult Audit()
        {
            return View();
        }
        #region Consultas
        public async Task<ActionResult> GetAudit(AuditModelView auditModelView)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var auditServiceModel = ModelAssembler.CreateAudit(auditModelView);
                    auditServiceModel.StatusTypeService = StatusTypeService.Original;
                    TimeSpan time = new TimeSpan(0, 23, 59, 0);
                    auditServiceModel.CurrentTo = auditServiceModel.CurrentTo.Add(time);
                    var data = await DelegateService.AuditServiceCore.GetAuditByObject(auditServiceModel);
                    data.AsParallel().ForAll(x => x.ActionTypeName = EnumsHelper.GetItemName<AudictTypeService>(x.ActionType));
                    data = data.OrderByDescending(x => x.RegisterDate).ToList();
                    return new UifJsonData(true, data);
                }
                else
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    return new UifJsonResult(false, errors.Select(x => x.ErrorMessage).ToList());
                }
            }
            catch (System.Exception ex)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.TitleAudit);
            }

        }

        private static async Task<List<AuditServiceModel>> GetAuditByObject(AuditServiceModel audit)
        {
            return await DelegateService.AuditServiceCore.GetAuditByObject(audit);
        }
        public ActionResult GetTypeTransaction()
        {
            try
            {
                var resul = EnumsHelper.GetItems<TransactionType>();
                return new UifJsonResult(true, resul);
            }
            catch (System.Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorDeleteDefault);
            }

        }

        public async Task<ActionResult> GetPackages()
        {
            try
            {
                ViewBag.SyncOrAsync = "Asynchronous";
                var result = await DelegateService.AuditServiceCore.GetPackages();
                return new UifJsonResult(true, result);
            }
            catch (System.Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.LabelPackages);
            }

        }
        public async Task<ActionResult> GetUserByAccountName(string userName)
        {
            try
            {
                ViewBag.SyncOrAsync = "Asynchronous";

                var userAccount = await DelegateService.uniqueUserService.GetUserByAccountName(userName, 0, 0, false);
                if (userAccount != null)
                {
                    var result = userAccount.Select(x => new { UserId = x.UserId, AccountName = x.AccountName }).ToList();
                    return Json(result.OrderBy(x => x.AccountName), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new List<UserServiceModel>(), JsonRequestBehavior.AllowGet);
                }
            }
            catch (System.Exception)
            {

                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }
        public async Task<ActionResult> GetEntityByQuery(int idPackage,string query)
        {
            try
            {
                ViewBag.SyncOrAsync = "Asynchronous"; 
                var entities = await DelegateService.AuditServiceCore.GetEntitiesByDescription(query, idPackage);
                var final = entities.Where(x => x.Description != string.Empty).OrderBy(x => x.Description).ToList();
                return Json(final, JsonRequestBehavior.AllowGet);
            }
            catch (System.Exception)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region entidades
        private async Task<List<PackageServiceModel>> GetPackage()
        {
            return await DelegateService.AuditServiceCore.GetPackages();
        }
        private static async Task<List<EntityServiceModel>> GetEntitiesByDescription(string description, int idPackage )
        {
            return await DelegateService.AuditServiceCore.GetEntitiesByDescription(description, idPackage);
        }
        #endregion
        #region Excel
        /// <summary>
        /// Genera archivo excel sucursales
        /// </summary>
        /// <returns></returns>
        public ActionResult GenerateFileToExport(AuditModelView auditModelView)
        {
            try
            {
                var auditServiceModel = ModelAssembler.CreateAudit(auditModelView);
                auditServiceModel.StatusTypeService = StatusTypeService.Original;
                ExcelFileServiceModel excelFileServiceModel = DelegateService.AuditServiceCore.GenerateFileToAudit(auditServiceModel, App_GlobalResources.Language.FileAudit);
                if (excelFileServiceModel.ErrorTypeService == ENUMSM.ErrorTypeService.Ok)
                {
                    var urlFile = excelFileServiceModel.FileData;
                    return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorFileNotFound);
                }
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
            }
        }
        #endregion

    }
}