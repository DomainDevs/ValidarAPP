// -----------------------------------------------------------------------
// <copyright file="BusinessTypeController.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>@ETriana</author>
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
    using Sistran.Company.Application.Utilities.DTO;
    using Sistran.Company.Application.ParametrizationAplicationServices.DTO;
    using UtilEnum = Sistran.Company.Application.Utilities.Enums;

    /// <summary>
    /// BusinessTypeController. Clase Controlador de la vista de Load
    /// </summary>
    public class BusinessTypeController : Controller
    {
        /// <summary>
        /// Listado de .
        /// </summary>
        private List<BusinessTypeQueryDTO> businessType = new List<BusinessTypeQueryDTO>(); // Cambiar
                     
        /// <summary>
        /// Método defecto de la vista.
        /// </summary>
        /// <returns>Retorna la vista   .</returns>        
        public ActionResult BusinessType()
        {            
            return this.View();
        }

        /// <summary>
        /// Gets todos los tipos de negocios.
        /// </summary>
        /// <returns>Lista de </returns>
        public ActionResult GetBusinnesTypes()
        {
            try
            {
                BusinessTypeQueryDTO BusinessTypeServiceModelList = DelegateService.parametrizationAplicationService.GetApplicationBusinessType();
                List<BusinessTypeViewModel> BusinessTypeList = ModelAssembler.GetBusinessType(BusinessTypeServiceModelList);
                

                if (BusinessTypeServiceModelList.Error.ErrorType == UtilEnum.ErrorType.Ok)
                    return new UifJsonResult(true, BusinessTypeList.OrderBy(x => x.SMALL_DESCRIPTION).ToList());
                else if (BusinessTypeServiceModelList.Error.ErrorType == UtilEnum.ErrorType.NotFound)
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorDataNotFound);
                else
                    return new UifJsonResult(false, this.ErrorMessages(App_GlobalResources.Language.ErrorGetBusinessTypes, BusinessTypeServiceModelList.Error.ErrorDescription));
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetBusinessTypes + " <br>" + ex.Message);

            }
        }


        /// <summary>
        /// Metodo para retornar los mensajes de error.
        /// </summary>
        /// <param name="errorList"></param>
        /// <returns>Mensajes de error.</returns>
        private string ErrorMessages(String Language, List<string> errorList)
        {
            string errorMessages = Language + " <br>";

            foreach (string errorMessageItem in errorList)
            {
                errorMessages = errorMessages + errorMessageItem + " <br>";
            }
            return errorMessages;
        }

        /// <summary>
        /// Genera archivo excel de tipos de negocios.
        /// </summary>
        /// <returns>Objeto tipo Json<ExcelFileDTO>.</returns>
        public ActionResult GenerateFileToExportBusinessTypes()
        {
            try
            {
                ExcelFileDTO exportFileBusinessTypes = DelegateService.parametrizationAplicationService.GenerateFileToApplicationBusinessType(App_GlobalResources.Language.FileNameBusinessType);

                if (exportFileBusinessTypes.ErrorType == UtilEnum.ErrorType.Ok)
                {
                    return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + exportFileBusinessTypes.File);
                }
                else
                {
                    return new UifJsonResult(false, new { App_GlobalResources.Language.FileNotFound, exportFileBusinessTypes.ErrorDescription });
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
            }
        }
    }
}