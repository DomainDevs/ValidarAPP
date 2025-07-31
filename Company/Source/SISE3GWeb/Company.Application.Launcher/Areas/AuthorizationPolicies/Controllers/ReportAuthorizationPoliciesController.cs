// -----------------------------------------------------------------------
// <copyright file="ReportAuthorizationPoliciesController.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Stiveen Niño Gutierrez</author>
// -----------------------------------------------------------------------

using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Web.Mvc;
using MODUD = Sistran.Company.Application.ModelServices.Models.AuthorizationPolicies;

namespace Sistran.Core.Framework.UIF.Web.Areas.AuthorizationPolicies.Controllers
{
    using Application.AuthorizationPoliciesServices.Models;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    [Authorize]
    public class ReportAuthorizationPoliciesController : Controller
    {
        #region Vistas

        /// <summary>
        /// retorna la vista principal
        /// </summary>
        /// <returns>Json</returns>
        public ActionResult ReportAuthorizationPolicies()
        {
            return this.View();
        }
        #endregion


        /// <summary>
        /// retorna todos los grupos de politicas de la BD
        /// </summary>
        /// <returns>Json</returns>
        public ActionResult GetAllStatus()
        {
            MODUD.StatusServicesModel statusServicesModel = DelegateService.AuthorizationPoliciesService.GetAllStates();
            return new UifJsonResult(true, statusServicesModel.StatusServicemodel.OrderBy(b => b.Description));
        }

        /// <summary>
        /// Consulta las solicitudes de autorizacion por el usuario que solicita, fecha inicio/fin y los estados
        /// </summary>
        /// <param name="status">lista de estados</param>
        /// <param name="strDateInit">fecha inicial</param>
        /// <param name="strDateEnd">fecha final</param>
        /// <returns></returns>
        public ActionResult GetAuthorizationRequestGroups(List<int> status, string strDateInit, string strDateEnd)
        {
            try
            {
                int idUser = Helpers.SessionHelper.GetUserId();
                DateTime dateInit = DateTime.ParseExact(strDateInit, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime dateEnd = DateTime.ParseExact(strDateEnd, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                List<AuthorizationRequestGroup> requestGroups = DelegateService.AuthorizationPoliciesService.GetAuthorizationRequestGroups(idUser, status, dateInit, dateEnd);
                return new UifJsonResult(true, requestGroups);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        public ActionResult GetDetailsAuthorizationRequestGroups(string key, int policiesId)
        {
            try
            {
                int idUser = Helpers.SessionHelper.GetUserId();

                List<AuthorizationRequestGroup> requestGroups = DelegateService.AuthorizationPoliciesService.GetDetailsAuthorizationRequestGroups(idUser, key, policiesId);
                return new UifJsonResult(true, requestGroups);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        public ActionResult GetAuthorizationAnswersByRequestId(int requestId)
        {
            try
            {
                List<AuthorizationAnswer> answers = DelegateService.AuthorizationPoliciesService.GetAuthorizationAnswersByRequestId(requestId);
                return new UifJsonResult(true, answers);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }


        ///// <summary>
            ///// ExportFileCities: Consulta listado de ciudades y genera el excel a descargar
            ///// </summary>
            ///// <returns></returns>
            //public JsonResult ExportFileReportAuthorizationPolicies(CompanyPolicyValid parampolicies)
            //{
            //    try
            //    {
            //        ExcelFileDTO Result = DelegateService.AuthorizationPoliciesService.GenerateFileToReportAuthorizationPolicies(App_GlobalResources.Language.FileNameReportAuthorizationPolicies, parampolicies);
            //        if (Result.File == "Error al consultar")
            //        {
            //            return new UifJsonResult(false, App_GlobalResources.Language.ErrorThereIsNoDataToExport);
            //        }
            //        else if (Result.File == "")
            //        {
            //            return new UifJsonResult(false, App_GlobalResources.Language.ErrorExportExcel);
            //        }
            //        else
            //        {
            //            return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + Result.File);
            //        }

            //    }
            //    catch (Exception)
            //    {
            //        return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
            //    }
            //}
        }
}