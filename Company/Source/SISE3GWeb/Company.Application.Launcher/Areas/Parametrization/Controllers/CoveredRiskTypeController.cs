// -----------------------------------------------------------------------
// <copyright file="CoveredRiskTypeController.cs" company="SISTRAN">
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
    using Sistran.Core.Application.ModelServices.Enums;
    using Sistran.Core.Application.ModelServices.Models.Param;
    using Sistran.Core.Application.ModelServices.Models.Underwriting;
    using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
    using Sistran.Core.Framework.UIF.Web.Models;
    using Sistran.Core.Framework.UIF.Web.Services;
    

    /// <summary>
    /// Controlador de la vista de Tipos de riesgo cubierto.
    /// </summary>
    public class CoveredRiskTypeController : Controller
    {
        /// <summary>
        /// Listado de Tipos de riesgo cubierto.
        /// </summary>
        private List<CoveredRiskTypeServiceModel> coveredRiskTypes = new List<CoveredRiskTypeServiceModel>();

        /// <summary>
        /// Método defecto de la vista.
        /// </summary>
        /// <returns>Retorna la vista Covered Risk Type.</returns>        

        [NoDirectAccess]
        public ActionResult CoveredRiskType()
        {            
            return this.View();
        }

        /// <summary>
        /// Obtiene la lista de Tipos de riesgo cubierto.
        /// </summary>
        /// <returns>Lista de Tipos de riesgo cubierto.</returns>
        public ActionResult GetCoveredRiskTypes()
        {    
            CoveredRiskTypesServiceModel coveredRiskTypeServiceModelList = DelegateService.UnderwritingParamServiceWeb.GetCoveredRiskTypes();
            ErrorTypeService errorTypeProcess = coveredRiskTypeServiceModelList.ErrorTypeService;
            if (errorTypeProcess == ErrorTypeService.Ok)
            {
                List<CoveredRiskTypeViewModel> coveredRiskTypeList = ModelAssembler.GetCoveredRiskTypes(coveredRiskTypeServiceModelList);
                return new UifJsonResult(true, coveredRiskTypeList.OrderBy(x => x.ShortDescription).ToList());
            }
            else if (errorTypeProcess == ErrorTypeService.NotFound || errorTypeProcess == ErrorTypeService.BusinessFault || errorTypeProcess == ErrorTypeService.TechnicalFault)
            {
                return new UifJsonResult(false, this.ErrorMessages(coveredRiskTypeServiceModelList.ErrorDescription));
            }
            else
            {
                return null;                
            }
        }


        /// <summary>
        /// Metodo para retornar los mensajes de error.
        /// </summary>
        /// <param name="errorList"></param>
        /// <returns>Mensajes de error.</returns>
        private string ErrorMessages(List<string> errorList)
        {
            string errorMessages = string.Empty;
            foreach (string errorMessageItem in errorList)
            {
                errorMessages = errorMessages + errorMessageItem + " <br>";
            }
            return errorMessages;
        }



        /// <summary>
        /// Obtiene la lista de Tipos de riesgo cubierto.
        /// </summary>
        /// <returns>Tipo de error de servicio.</returns>
        public ErrorTypeService GetListCoveredRiskTypes()
        {            
            CoveredRiskTypesServiceModel coveredRiskTypeServiceModelList = DelegateService.UnderwritingParamServiceWeb.GetCoveredRiskTypes();
            ErrorTypeService errorTypeProcess = coveredRiskTypeServiceModelList.ErrorTypeService;
            if (errorTypeProcess == ErrorTypeService.Ok)
            {
                this.coveredRiskTypes = coveredRiskTypeServiceModelList.CoveredRiskTypeServiceModel.ToList();                
            }

            return coveredRiskTypeServiceModelList.ErrorTypeService;
        }

        /// <summary>
        /// Genera archivo excel de Tipos de riesgo cubiertos.
        /// </summary>
        /// <returns>Objeto tipo ActionResult.</returns>
        public ActionResult GenerateFileToExportCoveredRiskTypes()
        {
            try
            {
                ErrorTypeService errorTypeGetListCoveredRiskTypes = this.GetListCoveredRiskTypes();
                if (errorTypeGetListCoveredRiskTypes == ErrorTypeService.Ok)
                {
                    ExcelFileServiceModel exportFile = DelegateService.UnderwritingParamServiceWeb.GenerateFileToCoveredRiskTypes(this.coveredRiskTypes, App_GlobalResources.Language.FileNameCoveredRiskType);
                    if (exportFile.ErrorTypeService == ErrorTypeService.Ok)
                    {
                        return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + exportFile.FileData);
                        
                    }
                    else
                    {
                        return new UifJsonResult(false, ErrorMessages(exportFile.ErrorDescription));
                    }
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorFileNotFound);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
            }
        }
    }
}