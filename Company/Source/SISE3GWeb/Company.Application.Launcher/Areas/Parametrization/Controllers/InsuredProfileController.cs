// -----------------------------------------------------------------------
// <copyright file="InsuredProfileController.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Eduardo Rodríguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;    
    using System.Web.Mvc;
    using Sistran.Core.Application.CommonService.Models;    
    using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;    
    using Sistran.Core.Framework.UIF.Web.Models;
    using Sistran.Core.Framework.UIF.Web.Services;
    using Sistran.Core.Services.UtilitiesServices.Models;
    using CoreMod = Sistran.Core.Application.UniquePersonService.V1.Models;
    using Sistran.Core.Services.UtilitiesServices.Models;

    /// <summary>
    /// Controlador de la vista Insured Profile.
    /// </summary>
    public class InsuredProfileController : Controller
    {
        /// <summary>
        /// Listado de Perfiles de Asegurado.
        /// </summary>
        private List<CoreMod.InsuredProfile> insuredProfiles = new List<CoreMod.InsuredProfile>();

        /// <summary>
        /// Método defecto de la vista.
        /// </summary>
        /// <returns>Retorna la vista Insured Profile.</returns>        
        [NoDirectAccess]
        public ActionResult InsuredProfile()
        {
            InsuredProfileViewModel model = new InsuredProfileViewModel();
            model.Id = 0;
            return this.View(model);
        }

        /// <summary>
        /// Retorna la vista para la búsqueda avanzada.
        /// </summary>
        /// <returns>Lista de perfiles de asegurados</returns>
        public ActionResult InsuredProfileAdvancedSearch()
        {
            return this.View();
        }

        /// <summary>
        /// Obtiene la lista de perfiles de asegurados
        /// </summary>
        /// <returns>Lista de perfiles de asegurados</returns>
        public ActionResult GetInsuredProfiles()
        {
            try
            {
                List<InsuredProfileViewModel> insuredProfileList = ModelAssembler.GetInsuredProfiles(DelegateService.uniquePersonServiceV1.GetInsuredProfiles());
                return new UifJsonResult(true, insuredProfileList.OrderBy(x => x.LongDescription).ToList());
            }
            catch (System.Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetInsuredProfiles);
            }
        }

        #region Formato del mensaje de operaciones realizadas
        /// <summary>
        /// Realiza los procesos del CRUD para los Perfiles de Asegurado
        /// </summary>
        /// <param name="listAdded"> Lista de insuredProfiles(perfiles de asegurados) para ser agregados</param>
        /// <param name="listModified">Lista de insuredProfiles(perfiles de asegurados) para ser modificados</param>
        /// <param name="listDeleted">Lista de insuredProfiles(perfiles de asegurados) para ser eliminados</param>
        /// <returns>Respuesta con el total de procesos realizados </returns>
        public ActionResult SaveInsuredProfiles(List<InsuredProfileViewModel> listAdded, List<InsuredProfileViewModel> listModified, List<InsuredProfileViewModel> listDeleted)
        {
            try
            {
                ParametrizationResponse<CoreMod.InsuredProfile> insuredProfileResponse = DelegateService.uniquePersonServiceV1.CreateInsuredProfiles(ModelAssembler.CreateInsuredProfiles(listAdded), ModelAssembler.CreateInsuredProfiles(listModified), ModelAssembler.CreateInsuredProfiles(listDeleted));                
                
                string added = string.Empty;
                string edited = string.Empty;
                string deleted = string.Empty;
                string message;

                if (!string.IsNullOrEmpty(insuredProfileResponse.ErrorAdded))
                { 
                    insuredProfileResponse.ErrorAdded = App_GlobalResources.Language.ResourceManager.GetString(insuredProfileResponse.ErrorAdded);
                }

                if (!string.IsNullOrEmpty(insuredProfileResponse.ErrorModify))
                { 
                    insuredProfileResponse.ErrorModify = App_GlobalResources.Language.ResourceManager.GetString(insuredProfileResponse.ErrorModify);
                }

                if (!string.IsNullOrEmpty(insuredProfileResponse.ErrorDeleted))
                { 
                    insuredProfileResponse.ErrorDeleted = App_GlobalResources.Language.ResourceManager.GetString(insuredProfileResponse.ErrorDeleted);
                }

                if (insuredProfileResponse.TotalAdded > 0)
                {
                    added = App_GlobalResources.Language.ReturnSaveAddedInsuredProfiles;
                }
                else
                {
                    insuredProfileResponse.TotalAdded = null;
                }

                if (insuredProfileResponse.TotalModify > 0)
                {
                    edited = App_GlobalResources.Language.ReturnSaveEditedInsuredProfiles;
                }
                else
                {
                    insuredProfileResponse.TotalModify = null;
                }

                if (insuredProfileResponse.TotalDeleted > 0)
                {
                    deleted = App_GlobalResources.Language.ReturnSaveDeletedInsuredProfiles;
                }
                else
                {
                    insuredProfileResponse.TotalDeleted = null;
                }

                message = string.Format(
                    added + edited + deleted + "{3}{4}{5}",
                    insuredProfileResponse.TotalAdded.ToString() ?? string.Empty,
                    insuredProfileResponse.TotalModify.ToString() ?? string.Empty,
                    insuredProfileResponse.TotalDeleted.ToString() ?? string.Empty,
                    insuredProfileResponse.ErrorAdded ?? string.Empty,
                    insuredProfileResponse.ErrorModify ?? string.Empty,
                    insuredProfileResponse.ErrorDeleted ?? string.Empty);
                var result = ModelAssembler.GetInsuredProfiles(insuredProfileResponse.ReturnedList.OrderBy(x => x.Description).ToList());
                
                return new UifJsonResult(true, new { message = message, data = result });
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveInsuredProfile);
            }
        }
        #endregion

        /// <summary>
        /// Obtiene perfiles de asegurado
        /// </summary>
        public void GetListInsuredProfiles()
        {
            if (this.insuredProfiles.Count == 0)
            {
                this.insuredProfiles = DelegateService.uniquePersonServiceV1.GetInsuredProfiles().ToList();
            }
        }

        /// <summary>
        /// Genera archivo excel de Perfiles de Asgurado.
        /// </summary>
        /// <returns>Objeto tipo ActionResult.</returns>
        public ActionResult GenerateFileToExportInsuredProfiles()
        {
            try
            {
                this.GetListInsuredProfiles();
                string urlFile = DelegateService.uniquePersonServiceV1.GenerateFileToInsuredProfile(this.insuredProfiles, App_GlobalResources.Language.FileNameInsuredProfile);
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