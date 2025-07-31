// -----------------------------------------------------------------------
// <copyright file="SurchargeController.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Oscar Camacho</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Application.ModelServices.Models.Param;
    using Models;
    using Sistran.Core.Application.ModelServices.Models.UnderwritingParam;
    using Sistran.Core.Services.UtilitiesServices.Enums;
    using Sistran.Core.Framework.UIF.Web.Helpers;
    using Sistran.Core.Framework.UIF.Web.Models;
    using Sistran.Core.Framework.UIF.Web.Services;
    using ENUMSM = Sistran.Core.Application.ModelServices.Enums;

    /// <summary>
    /// Controlador para recargos
    /// </summary>
    public class SurchargeController : Controller
    {
        /// <summary>
        /// Se llama la vista
        /// </summary>       
        /// <returns> Retorna View </returns>
        public ActionResult Surcharge()
        {
            return this.View();
        }

        [HttpGet]

        /// <summary>
        /// Hace llamado al tipo de tasas 
        /// </summary>  
        /// <returns> retorna Lista de Tasas </returns>
        public ActionResult GetRateTypes()
        {
            try
            {
                var rateTypes = EnumsHelper.GetItems<RateType>();
                return new UifJsonResult(true, rateTypes);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryRateTypes);
            }
        }

        [HttpGet]

        /// <summary>
        /// Lista los recargos
        /// </summary>  
        /// <returns> retorna Lista de Tasas </returns>
        public ActionResult GetSurcharge()
        {
            try
            {
                List<SurchargeViewModel> surchargeViewModel = new List<SurchargeViewModel>();
                surchargeViewModel = this.GetListSurcharges();
                return new UifJsonResult(true, surchargeViewModel.OrderBy(x => x.Id).ToList());
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetSurcharge);
            }
        }

        [HttpPost]

        /// <summary>
        /// Guarda el modelo de recargos
        /// </summary>  
        /// <param name="surcharge"> Modelo de recargos </param>
        /// <returns> retorna si se guardo satisfactoriamente </returns>
        public ActionResult SaveSurcharges(List<SurchargeViewModel> surcharge)
        {
            try
            {
                List<SurchargeServiceModel> surchargeServiceModel = new List<SurchargeServiceModel>();
                surchargeServiceModel = ModelAssembler.CreateSurcharges(surcharge);
                List<SurchargeServiceModel> surchargeServiceModels = DelegateService.UnderwritingParamServiceWeb.ExecuteOperationsSurchargeServiceModel(surchargeServiceModel);
                ParametrizationResult parametrizationResult = new ParametrizationResult();

                foreach (var item in surchargeServiceModels)
                {
                    if (item.ErrorServiceModel.ErrorTypeService != ENUMSM.ErrorTypeService.Ok)
                    {
                        string errores = string.Empty;
                        foreach (var itemError in item.ErrorServiceModel.ErrorDescription)
                        {
                            errores += itemError;
                        }

                        parametrizationResult.Message += errores + "</br>";
                    }
                    else
                    {
                        switch (item.StatusTypeService)
                        {
                            case ENUMSM.StatusTypeService.Create:
                                parametrizationResult.TotalAdded++;
                                break;
                            case ENUMSM.StatusTypeService.Update:
                                parametrizationResult.TotalModified++;
                                break;
                            case ENUMSM.StatusTypeService.Delete:
                                parametrizationResult.TotalDeleted++;
                                break;
                            default:
                                break;
                        }
                    }
                }

                return new UifJsonResult(true, parametrizationResult);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSave);
            }
        }

        [HttpGet]

        /// <summary>
        /// Genera archivo excel de recargos
        /// </summary>
        /// <returns> excel de recargos </returns>
        public ActionResult GenerateFileToExport()
        {
            try
            {
                List<SurchargeViewModel> surchargeViewModel = new List<SurchargeViewModel>();
                surchargeViewModel = this.GetListSurcharges();
                if (surchargeViewModel.Count > 0)
                {
                    ExcelFileServiceModel excelFileServiceModel = DelegateService.UnderwritingParamServiceWeb.GenerateFileToSurcharge(ModelAssembler.CreateSurcharges(surchargeViewModel), App_GlobalResources.Language.LabelSurcharge);
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
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorThereIsNoDataToExport);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
            }
        }

        /// <summary>
        /// obtiene lista de recargos
        /// </summary>
        /// <returns> retorna la vista de recargos </returns>
        private List<SurchargeViewModel> GetListSurcharges()
        {
            List<SurchargeViewModel> surchargeViewModel = new List<SurchargeViewModel>();
            if (surchargeViewModel.Count == 0)
            {
                SurchargesServiceModel surchargeServiceModel = DelegateService.UnderwritingParamServiceWeb.GetSurchargeServiceModel();

                surchargeViewModel = ModelAssembler.CreateSurcharge(surchargeServiceModel.SurchargeServiceModel);
                return surchargeViewModel.OrderBy(x => x.Description).ToList();
            }

            return surchargeViewModel;
        }

        /// <summary>
        /// obtiene lista de recargos
        /// </summary>
        /// <returns> retorna la vista de recargos </returns>
        [HttpGet]
        public ActionResult GetListQuoSurcharges()
        {
            try
            {
                List<SurchargeViewModel> surchargesViewModel = new List<SurchargeViewModel>();
                if (surchargesViewModel.Count == 0)
                {
                    SurchargesServiceModel surchargeServiceModel = DelegateService.UnderwritingParamServiceWeb.GetSurchargeServiceModel();
                    surchargesViewModel = ModelAssembler.CreateSurcharge(surchargeServiceModel.SurchargeServiceModel);
                    return new UifJsonResult(true, surchargesViewModel.OrderBy(p => p.Description));
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }
    }
}