// -----------------------------------------------------------------------
// <copyright file="InsuredSegmentController.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Eduardo Rodríguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
    using Sistran.Core.Framework.UIF.Web.Models;
    using Sistran.Core.Framework.UIF.Web.Services;
    using Sistran.Core.Services.UtilitiesServices.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using CoreModV1 = Sistran.Core.Application.UniquePersonService.V1.Models;

    /// <summary>
    /// Controlador de la vista Insured Profile.
    /// </summary>
    public class InsuredSegmentController : Controller
    {
        /// <summary>
        /// Listado de Perfiles de Asegurado.
        /// </summary>
        private List<CoreModV1.InsuredSegmentV1> insuredSegments = new List<CoreModV1.InsuredSegmentV1>();
        /// <summary>
        /// Método defecto de la vista.
        /// </summary>
        /// <returns>Retorna la vista Insured Profile.</returns>        
        public ActionResult InsuredSegment()
        {
            InsuredSegmentViewModel model = new InsuredSegmentViewModel();
            model.Id = 0;
            return this.View(model);
        }

        /// <summary>
        /// Retorna la vista para la búsqueda avanzada.
        /// </summary>
        /// <returns>Lista de perfiles de asegurados</returns>
        public ActionResult InsuredSegmentAdvancedSearch()
        {
            return this.View();
        }

        /// <summary>
        /// Obtiene la lista de perfiles de asegurados
        /// </summary>
        /// <returns>Lista de perfiles de asegurados</returns>
        public ActionResult GetInsuredSegments()
        {
            try
            {
                List<InsuredSegmentViewModel> insuredSegmentList = ModelAssembler.GetInsuredSegments(DelegateService.uniquePersonServiceV1.GetInsuredSegments());
                return new UifJsonResult(true, insuredSegmentList.OrderBy(x => x.LongDescription).ToList());
            }
            catch (System.Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetInsuredSegments);
            }
        }

        #region Formato del mensaje de operaciones realizadas
        /// <summary>
        /// Realiza los procesos del CRUD para los Perfiles de Asegurado
        /// </summary>
        /// <param name="listAdded"> Lista de insuredSegments(perfiles de asegurados) para ser agregados</param>
        /// <param name="listModified">Lista de insuredSegments(perfiles de asegurados) para ser modificados</param>
        /// <param name="listDeleted">Lista de insuredSegments(perfiles de asegurados) para ser eliminados</param>
        /// <returns>Respuesta con el total de procesos realizados </returns>
        public ActionResult SaveInsuredSegments(List<InsuredSegmentViewModel> listAdded, List<InsuredSegmentViewModel> listModified, List<InsuredSegmentViewModel> listDeleted)
        {
            try
            {
                ParametrizationResponse<CoreModV1.InsuredSegmentV1> insuredSegmentResponse = DelegateService.uniquePersonServiceV1.CreateInsuredSegments(ModelAssembler.CreateInsuredSegments(listAdded), ModelAssembler.CreateInsuredSegments(listModified), ModelAssembler.CreateInsuredSegments(listDeleted));                
                
                string added = string.Empty;
                string edited = string.Empty;
                string deleted = string.Empty;
                string message;

                if (!string.IsNullOrEmpty(insuredSegmentResponse.ErrorAdded))
                { 
                    insuredSegmentResponse.ErrorAdded = App_GlobalResources.Language.ResourceManager.GetString(insuredSegmentResponse.ErrorAdded);
                }

                if (!string.IsNullOrEmpty(insuredSegmentResponse.ErrorModify))
                { 
                    insuredSegmentResponse.ErrorModify = App_GlobalResources.Language.ResourceManager.GetString(insuredSegmentResponse.ErrorModify);
                }

                if (!string.IsNullOrEmpty(insuredSegmentResponse.ErrorDeleted))
                { 
                    insuredSegmentResponse.ErrorDeleted = App_GlobalResources.Language.ResourceManager.GetString(insuredSegmentResponse.ErrorDeleted);
                }

                if (insuredSegmentResponse.TotalAdded > 0)
                {
                    added = App_GlobalResources.Language.ReturnSaveAddedInsuredSegments;
                }
                else
                {
                    insuredSegmentResponse.TotalAdded = null;
                }

                if (insuredSegmentResponse.TotalModify > 0)
                {
                    edited = App_GlobalResources.Language.ReturnSaveEditedInsuredSegments;
                }
                else
                {
                    insuredSegmentResponse.TotalModify = null;
                }

                if (insuredSegmentResponse.TotalDeleted > 0)
                {
                    deleted = App_GlobalResources.Language.ReturnSaveDeletedInsuredSegments;
                }
                else
                {
                    insuredSegmentResponse.TotalDeleted = null;
                }

                message = string.Format(
                    added + edited + deleted + "{3}{4}{5}",
                    insuredSegmentResponse.TotalAdded.ToString() ?? string.Empty,
                    insuredSegmentResponse.TotalModify.ToString() ?? string.Empty,
                    insuredSegmentResponse.TotalDeleted.ToString() ?? string.Empty,
                    insuredSegmentResponse.ErrorAdded ?? string.Empty,
                    insuredSegmentResponse.ErrorModify ?? string.Empty,
                    insuredSegmentResponse.ErrorDeleted ?? string.Empty);
                var result = ModelAssembler.GetInsuredSegments(insuredSegmentResponse.ReturnedList.OrderBy(x => x.LongDescription).ToList());
                
                return new UifJsonResult(true, new { message = message, data = result });
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveInsuredSegment);
            }
        }
        #endregion

        /// <summary>
        /// Obtiene perfiles de asegurado
        /// </summary>
        public void GetListInsuredSegments()
        {
            if (this.insuredSegments.Count == 0)
            {
                this.insuredSegments = DelegateService.uniquePersonServiceV1.GetInsuredSegments().ToList();
            }
        }

        /// <summary>
        /// Genera archivo excel de Perfiles de Asgurado.
        /// </summary>
        /// <returns>Objeto tipo ActionResult.</returns>
        public ActionResult GenerateFileToExportInsuredSegments()
        {
            try
            {
                this.GetListInsuredSegments();
                string urlFile = DelegateService.uniquePersonServiceV1.GenerateFileToInsuredSegment(this.insuredSegments, App_GlobalResources.Language.FileNameInsuredSegment);
                if (string.IsNullOrEmpty(urlFile))
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorFileNotFound);
                }
                else
                {
                    return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
            }
        }
    }
}