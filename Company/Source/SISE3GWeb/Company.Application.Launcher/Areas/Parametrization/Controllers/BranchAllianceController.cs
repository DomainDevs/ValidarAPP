// -----------------------------------------------------------------------
// <copyright file="BranchAllianceController.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Juan Sebastián Cárdenas Leiva</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    using Sistran.Company.Application.UniquePersonParamService.Models;
    using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
    using Sistran.Core.Framework.UIF.Web.Models;
    using Sistran.Core.Framework.UIF.Web.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    /// <summary>
    /// Controlador de la vista de creación y modificación de oficinas de aliados.
    /// Módulo de parametrización.
    /// </summary>
    public class BranchAllianceController : Controller
    {
        /// <summary>
        /// Contructor. Llamado de la vista inicial.
        /// </summary>
        /// <returns>Vista inicial de sucursal aliados</returns>
        public ActionResult BranchAlliance()
        {
            return this.View();
        }

        /// <summary>
        /// Obtiene la lista de sucursales
        /// </summary>
        /// <returns>Lista de sucursales consultadas</returns>
        public ActionResult GetBranchsByAlliancesId(int allianceId)
        {
            try
            {
                List<BranchAlliance> branchAlliance = DelegateService.companyUniquePersonParamService.GetAllBranchAlliancesByAlliancedId(allianceId);
                List<BranchAllianceViewModel> brachAliancesView = ModelAssembler.CreateBrachAlliances(branchAlliance);
                return new UifJsonResult(true, brachAliancesView.OrderBy(x => x.StateName).OrderBy(x => x.CityName).OrderBy(x => x.BranchDescription).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetBranchs);
            }
        }

        /// <summary>
        /// Obtiene la lista de sucursales
        /// </summary>
        /// <returns>Lista de sucursales consultadas</returns>
        public ActionResult GetSalesPointsByBranchId(int branchId, int allianceId)
        {
            try
            {
                List<AllianceBranchSalePonit> salesPoints = DelegateService.companyUniquePersonParamService.GetAllSalesPointsByBranchId(branchId, allianceId);
                List<AllianceSalesPointsViewModel> salesPointsView = ModelAssembler.CreateSalesPoints(salesPoints);
                return new UifJsonResult(true, salesPointsView.OrderBy(x => x.SalePointId).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetBranchs);
            }
        }

        /// <summary>
        /// Guarda los cambios hecho a las sucursales
        /// </summary>
        /// <param name="lstBranchAlliances"></param>
        /// <returns>Lista de sucursales actualizada</returns>
        public ActionResult SaveBranchAlliances(List<BranchAllianceViewModel> lstBranchAlliances)
        {
            try
            {
                List<BranchAlliance> response = DelegateService.companyUniquePersonParamService.ExecuteOprationsBranchAlliances(ModelAssembler.CreateBrachAlliances(lstBranchAlliances));
                List<BranchAllianceViewModel> branchAliancesView = ModelAssembler.CreateBrachAlliances(response);
                return new UifJsonResult(true, branchAliancesView.OrderBy(x => x.BranchId).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, "Error"/*App_GlobalResources.Language.ErrorSaveInsurencesObjects*/);
            }
        }

        /// <summary>
        /// Busqueda de aliados por nombre
        /// </summary>
        /// <param name="description">Nombre del aliado</param>
        /// <returns>Aliado</returns>
        public ActionResult GetBranchAllianceByDescription(string description)
        {
            try
            {
                List<BranchAlliance> branchAlliance = DelegateService.companyUniquePersonParamService.GetBranchAllianceByDescription(description);
                List<BranchAllianceViewModel> aliancesView = ModelAssembler.CreateBrachAlliances(branchAlliance);
                return new UifJsonResult(true, aliancesView.OrderBy(x => x.BranchId).ToList());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetBranchs);
            }
        }

        /// <summary>
        /// Vista de busqueda
        /// </summary>
        /// <returns>Vista de busuqeda</returns>
        public ViewResult BranchAllianceSearch()
        {
            return this.View();
        }

        /// <summary>
        /// Metodo GenerateFileToExport que genera archivo excel y lo retorna
        /// </summary>
        /// <returns>Excel de sucursales de aliado</returns>
        public ActionResult GenerateFileBranchesToExport(int allianceId)
        {
            try
            {
                List<BranchAlliance> branchAlliance = DelegateService.companyUniquePersonParamService.GetAllBranchAlliancesByAlliancedId(allianceId);
                string urlFile = DelegateService.companyUniquePersonParamService.GenerateFileToBranchAlliance(branchAlliance.OrderBy(b => b.BranchDescription).ToList(), App_GlobalResources.Language.FileNameBranchAlliance);
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

        /// <summary>
        /// Metodo GenerateFileToExport que genera archivo excel y lo retorna
        /// </summary>
        /// <returns>Excel de puntos de venta aliados</returns>
        public ActionResult GenerateFileSalesPointsToExport()
        {
            try
            {
                List<AllianceBranchSalePonit> salesPointsAlliance = DelegateService.companyUniquePersonParamService.GetAllSalesPointsAlliance();
                string urlFile = DelegateService.companyUniquePersonParamService.GenerateFileToSalePointsAlliance(salesPointsAlliance, App_GlobalResources.Language.FileNameSalePointsAlliance);
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