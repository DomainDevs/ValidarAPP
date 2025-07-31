// -----------------------------------------------------------------------
// <copyright file="AllianceController.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Juan Sebastián Cárdenas Leiva</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
    using Sistran.Core.Framework.UIF.Web.Models;
    using Sistran.Core.Framework.UIF.Web.Services;    
    using Sistran.Company.Application.UniquePersonParamService.Models;

    /// <summary>
    /// Controlador de la vista de creación y modificación de aliados.
    /// Módulo de parametrización.
    /// </summary>
    public class AllianceController : Controller
    {
        /// <summary>
        /// Contructor. Llamado de la vista inicial.
        /// </summary>
        /// <returns>Vista inicial de aliados</returns>
        public ActionResult Alliance()
        {
            return this.View();
        }

        /// <summary>
        /// Obtiene la lista de sucursales
        /// </summary>
        /// <returns>Lista de sucursales consultadas</returns>
        public ActionResult GetAllAlliances()
        {
            try
            {
                List<Alliance> alliance = DelegateService.companyUniquePersonParamService.GetAllAlliances();
                List<AllianceViewModel> aliancesView = ModelAssembler.CreateAlliances(alliance);
                return new UifJsonResult(true, aliancesView.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetBranchs);
            }
        }

        /// <summary>
        /// Busqueda de aliados por nombre
        /// </summary>
        /// <param name="description">Nombre del aliado</param>
        /// <returns>Aliado</returns>
        public ActionResult GetAllianceByDescription(string description)
        {
            try
            {
                List<Alliance> alliance = DelegateService.companyUniquePersonParamService.GetAllianceByDescription(description);
                List<AllianceViewModel> aliancesView = ModelAssembler.CreateAlliances(alliance);
                return new UifJsonResult(true, aliancesView.OrderBy(x => x.Description).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetBranchs);
            }
        }

        /// <summary>
        /// Guarda los cambio hechos en los aliados.
        /// </summary>
        /// <param name="lstAlliances">Liado de aliados (Modelo de vista)</param>
        /// <returns>Listado de aliados actualizado.</returns>
        public ActionResult SaveAlliances(List<AllianceViewModel> lstAlliances)
        {
            try
            {
                List<string> response = DelegateService.companyUniquePersonParamService.ExecuteOprationsAlliances(ModelAssembler.CreateAlliances(lstAlliances));

                List<Alliance> alliance = DelegateService.companyUniquePersonParamService.GetAllAlliances();
                List<AllianceViewModel> aliancesView = ModelAssembler.CreateAlliances(alliance);
                object[] result = new object[2];
                result[0] = response;
                result[1] = aliancesView.OrderBy(x => x.Description).ToList();
                return new UifJsonResult(true, result);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, "Error");
            }
        }

        /// <summary>
        /// Vista de busqueda
        /// </summary>
        /// <returns>Vista de busuqeda</returns>
        public ViewResult AllianceAdvancedSearch()
        {
            return this.View();
        }

        /// <summary>
        /// Metodo GenerateFileToExport que genera archivo excel y lo retorna
        /// </summary>
        /// <returns>Excel de aliados</returns>
        public ActionResult GenerateFileToExport()
        {
            try
            {
                List<Alliance> alliance = DelegateService.companyUniquePersonParamService.GetAllAlliances();
                string urlFile = DelegateService.companyUniquePersonParamService.GenerateFileToAlliance(alliance, App_GlobalResources.Language.FileNameAlliance);
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