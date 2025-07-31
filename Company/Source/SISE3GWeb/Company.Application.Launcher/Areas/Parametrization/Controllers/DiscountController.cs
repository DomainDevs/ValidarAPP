// -----------------------------------------------------------------------
// <copyright file="DiscountController.cs" company="SISTRAN">
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
    /// Controlador para descuentos
    /// </summary>
    public class DiscountController : Controller
    {
        /// <summary>
        /// Se llama la vista
        /// </summary>       
        /// <returns> Retorna View </returns>
        public ActionResult Discount()
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
        /// Lista los descuentos
        /// </summary>  
        /// <returns> retorna Lista de Tasas </returns>
        public ActionResult GetDiscount()
        {
            try
            {
                List<DiscountViewModel> discountViewModel = new List<DiscountViewModel>();
                discountViewModel = this.GetListDiscounts();
                return new UifJsonResult(true, discountViewModel.OrderBy(x => x.Id).ToList());
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetDiscount);
            }
        }

        [HttpPost]

        /// <summary>
        /// Guarda el modelo de descuentos
        /// </summary>  
        /// <param name="discount"> Modelo de descuentos </param>
        /// <returns> retorna si se guardo satisfactoriamente </returns>
        public ActionResult SaveDiscounts(List<DiscountViewModel> discount)
        {
            try
            {
                List<DiscountServiceModel> discountServiceModel = new List<DiscountServiceModel>();
                discountServiceModel = ModelAssembler.CreateDiscounts(discount);
                List<DiscountServiceModel> paymentPlanServiceModels = DelegateService.UnderwritingParamServiceWeb.ExecuteOperationsDiscountServiceModel(discountServiceModel);
                ParametrizationResult parametrizationResult = new ParametrizationResult();

                foreach (var item in paymentPlanServiceModels)
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
        /// Genera archivo excel de descuentos
        /// </summary>
        /// <returns> excel de descuentos </returns>
        public ActionResult GenerateFileToExport()
        {
            try
            {
                List<DiscountViewModel> discountViewModel = new List<DiscountViewModel>();
                discountViewModel = this.GetListDiscounts();
                if (discountViewModel.Count > 0)
                {
                    ExcelFileServiceModel excelFileServiceModel = DelegateService.UnderwritingParamServiceWeb.GenerateFileToDiscount(ModelAssembler.CreateDiscounts(discountViewModel), App_GlobalResources.Language.LabelDiscount);
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
        /// obtiene lista de descuentos
        /// </summary>
        /// <returns> retorna la vista de descuentos </returns>
        private List<DiscountViewModel> GetListDiscounts()
        {
            List<DiscountViewModel> discountViewModel = new List<DiscountViewModel>();
            if (discountViewModel.Count == 0)
            {
                DiscountsServiceModel discountsServiceModel = DelegateService.UnderwritingParamServiceWeb.GetDiscountServiceModel();

                discountViewModel = ModelAssembler.CreateDiscount(discountsServiceModel.DiscountServiceModel);
                return discountViewModel.OrderBy(x => x.Description).ToList();
            }

            return discountViewModel;
        }
    }
}