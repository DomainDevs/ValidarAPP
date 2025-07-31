// -----------------------------------------------------------------------
// <copyright file="BillingPeriodController.cs" company="SISTRAN">
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
    using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
    using Sistran.Core.Framework.UIF.Web.Models;
    using Sistran.Core.Framework.UIF.Web.Services;
    using Sistran.Company.Application.Utilities.DTO;
    using Sistran.Company.Application.ParametrizationAplicationServices.DTO;
    using UtilEnum= Sistran.Company.Application.Utilities.Enums;
    using Sistran.Company.Application.ModelServices.Models.Param;

    /// <summary>
    /// BillingPeriodController. Clase Controlador de la vista de Load
    /// </summary>
    public class BillingPeriodController : Controller
    {
        /// <summary>
        /// Listado de .
        /// </summary>
        private List<BillingPeriodQueryDTO> billingPeriod = new List<BillingPeriodQueryDTO>(); // Cambiar

        /// <summary>
        /// Método defecto de la vista.
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult BillingPeriod()
        {            
            return this.View();
        }

        /// <summary>
        /// Gets todos los periodos de fcturación.
        /// </summary>
        /// <returns>Json<ModelViewDTOs></ModelViewDTOs></returns>
        public ActionResult GetBillingPeriod()
        {
            try
            {
                BillingPeriodQueryDTO BillingPeriodServiceModelList = DelegateService.parametrizationAplicationService.GetApplicationBillingPeriod();
                List<BillingPeriodViewModel> BillingPeriodList = ModelAssembler.GetBillingPeriod(BillingPeriodServiceModelList);

                if(BillingPeriodServiceModelList.Error.ErrorType == UtilEnum.ErrorType.Ok)
                   return new UifJsonResult(true, BillingPeriodList.OrderBy(x => x.DESCRIPTION).ToList());
                else if (BillingPeriodServiceModelList.Error.ErrorType == UtilEnum.ErrorType.NotFound)
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorDataNotFound);
                else
                return new UifJsonResult(false, this.ErrorMessages(App_GlobalResources.Language.ErrorGetBillingPeriods, BillingPeriodServiceModelList.Error.ErrorDescription));
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetBillingPeriods + " <br>" + ex.Message);

            }
        }


        /// <summary>
        /// Metodo para retornar los mensajes de error.
        /// </summary>
        /// <param name="errorList"></param>
        /// <returns>Mensajes de error.</returns>
        private string ErrorMessages(String Language , List<string> errorList)
        {
            string errorMessages = Language + " <br>";
            
            foreach (string errorMessageItem in errorList)
            {
                errorMessages = errorMessages + errorMessageItem + " <br>";
            }
            return errorMessages;
        }

        /// <summary>
        /// Genera archivo excel de periodos de facturación
        /// </summary>
        /// <returns>Objeto tipo Json<ExcelFileDTO>.</returns>
        public ActionResult GenerateFileToExportBillingPeriod()
        {
           try
            {
                ExcelFileDTO exportFileBillingPeriod =  DelegateService.parametrizationAplicationService.GenerateFileToApplicationBillingPeriod( App_GlobalResources.Language.FileNameBillingPeriod);
              
                if(exportFileBillingPeriod.ErrorType == UtilEnum.ErrorType.Ok )
                {
                    return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + exportFileBillingPeriod.File);
                }
                else
                {
                    return new UifJsonResult(false, new { App_GlobalResources.Language.FileNotFound, exportFileBillingPeriod.ErrorDescription });
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
            }
        }
    }
}