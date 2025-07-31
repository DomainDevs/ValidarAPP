using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Sistran.Core.Application.ModelServices.Enums;
using Sistran.Core.Application.ModelServices.Models.Param;
using Sistran.Core.Application.ModelServices.Models.Underwriting;
using Sistran.Core.Application.ModelServices.Models.UnderwritingParam;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    public class PaymentMethodController : Controller
    {
        // GET: Parametrization/PaymentMethod

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult PaymentMethod()
        {
            return View();
        }

        /// <summary>
        /// Obtiene planes de pago
        /// </summary>
        /// <returns>Retorna listado de Medios de pago</returns>
        public ActionResult GetPaymentMethods()
        {
            PaymentMethodsServiceModel paymentMethodsServiceModel = DelegateService.UnderwritingParamServiceWeb.GetPaymentMethod();
            List<PaymentMethodTypeServiceQueryModel> paymentMethodTipesServiceModel = new List<PaymentMethodTypeServiceQueryModel>();
            paymentMethodTipesServiceModel = DelegateService.UnderwritingParamServiceWeb.GetPaymentMethodType();

            if (paymentMethodsServiceModel.ErrorTypeService == ErrorTypeService.Ok)
            {
                List<PaymentMethodViewModel> parametrizationPaymentMethodVM = new List<PaymentMethodViewModel>();
                foreach (var item in paymentMethodsServiceModel.PaymentMethodServiceModel)
                {
                    PaymentMethodViewModel model = new PaymentMethodViewModel();
                    model.Description = item.Description;
                    model.Id = item.Id;
                    model.TinyDescription = item.TinyDescription;
                    model.SmallDescription = item.SmallDescription;
                    model.PaymentMethodType = new PaymentMethodTypeViewModel()
                    {
                        Id = item.PaymentMethodTypeServiceQueryModel.Id,
                        Description = (from m in paymentMethodTipesServiceModel
                                       where m.Id == item.PaymentMethodTypeServiceQueryModel.Id
                                       select m.Description).FirstOrDefault().ToString()
                    };
                    parametrizationPaymentMethodVM.Add(model);
                }

                return new UifJsonResult(true, parametrizationPaymentMethodVM);
            }
            else
            {
                return new UifJsonResult(false, new { paymentMethodsServiceModel.ErrorTypeService, paymentMethodsServiceModel.ErrorDescription });
            }
        }

        /// <summary>
        /// Lista los metodos de pago filtrados por descripcion
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public ActionResult GetPaymentMethodsByDescription(string description)
        {
            PaymentMethodsServiceModel paymentMethodsServiceModel = DelegateService.UnderwritingParamServiceWeb.GetPaymentMethodByDescription(description);
            List<PaymentMethodTypeServiceQueryModel> paymentMethodTipesServiceModel = new List<PaymentMethodTypeServiceQueryModel>();
            paymentMethodTipesServiceModel = DelegateService.UnderwritingParamServiceWeb.GetPaymentMethodType();

            if (paymentMethodsServiceModel.ErrorTypeService == ErrorTypeService.Ok)
            {
                List<PaymentMethodViewModel> parametrizationPaymentMethodVM = new List<PaymentMethodViewModel>();
                foreach (var item in paymentMethodsServiceModel.PaymentMethodServiceModel)
                {                    
                    PaymentMethodViewModel model = new PaymentMethodViewModel();
                    model.Description = item.Description;
                    model.Id = item.Id;
                    model.TinyDescription = item.TinyDescription;
                    model.SmallDescription = item.SmallDescription;
                    model.PaymentMethodType = new PaymentMethodTypeViewModel()
                    {
                        Id = item.PaymentMethodTypeServiceQueryModel.Id,
                        Description = (from m in paymentMethodTipesServiceModel
                                       where m.Id == item.PaymentMethodTypeServiceQueryModel.Id
                                       select m.Description).FirstOrDefault().ToString()
                    };
                    parametrizationPaymentMethodVM.Add(model);
                }

                return new UifJsonResult(true, parametrizationPaymentMethodVM);
            }
            else
            {
                return new UifJsonResult(false, new { paymentMethodsServiceModel.ErrorTypeService, paymentMethodsServiceModel.ErrorDescription });
            }
        }

        /// <summary>
        /// Listado de los tipos de metodo de pago
        /// </summary>
        /// <returns></returns>
        public ActionResult GetPaymentMethodTypes()
        {
            List<PaymentMethodTypeServiceQueryModel> paymentMethodTypesServiceModel = DelegateService.UnderwritingParamServiceWeb.GetPaymentMethodType();
            var imapper = ModelAssembler.CreateMapPaymentMethodType();
            List<PaymentMethodTypeViewModel> parametrizationPaymentMethodTypeVM = imapper.Map<List<PaymentMethodTypeServiceQueryModel>, List<PaymentMethodTypeViewModel>>(paymentMethodTypesServiceModel);
            return new UifSelectResult(parametrizationPaymentMethodTypeVM.OrderBy(x => x.Description));
        }


        /// <summary>
        /// CRUD de Metodos de pago
        /// </summary>
        /// <param name="parametrizationPaymentPlanVM">Listado de Plan de pago VM</param>
        /// <returns>Conteo de CRUD de la operacion</returns>
        public ActionResult SavePaymentMethod(List<PaymentMethodViewModel> paymentMethodVM)
        {
            List<PaymentMethodTypeServiceQueryModel> paymentMethodTipesServiceModel = new List<PaymentMethodTypeServiceQueryModel>();
            paymentMethodTipesServiceModel = DelegateService.UnderwritingParamServiceWeb.GetPaymentMethodType();
            List<PaymentMethodServiceModel> paymentMethodServiceModel = new List<PaymentMethodServiceModel>();
            for (int i = 0; i < paymentMethodVM.Count; i++)
            {
                PaymentMethodServiceModel model = new PaymentMethodServiceModel();
                model.Id = paymentMethodVM[i].Id;
                model.Description = paymentMethodVM[i].Description;
                model.SmallDescription = paymentMethodVM[i].SmallDescription;
                model.TinyDescription = paymentMethodVM[i].Description.Substring(0,3);
                model.StatusTypeService = (StatusTypeService)paymentMethodVM[i].StatusTypeService;
                model.PaymentMethodTypeServiceQueryModel = new PaymentMethodTypeServiceQueryModel()
                {
                    Id = paymentMethodVM[i].PaymentMethodType.Id,
                    Description = (from m in paymentMethodTipesServiceModel
                                   where m.Id == paymentMethodVM[i].PaymentMethodType.Id
                                   select m.Description).FirstOrDefault().ToString(),
                    ErrorTypeService = ErrorTypeService.Ok,

                    ErrorDescription =new List<string>()
                };

                paymentMethodServiceModel.Add(model);
            }

            List<PaymentMethodServiceModel> paymentMethodServiceModels = new List<PaymentMethodServiceModel>();
            paymentMethodServiceModels = DelegateService.UnderwritingParamServiceWeb.ExecuteOperationPaymentMethod(paymentMethodServiceModel);
            ParametrizationResult parametrizationResult = new ParametrizationResult();

            foreach (var item in paymentMethodServiceModels)
            {
                if (item.ErrorServiceModel.ErrorTypeService != ErrorTypeService.Ok)
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
                        case StatusTypeService.Create:
                            parametrizationResult.TotalAdded++;
                            break;
                        case StatusTypeService.Update:
                            parametrizationResult.TotalModified++;
                            break;
                        case StatusTypeService.Delete:
                            parametrizationResult.TotalDeleted++;
                            break;
                        default:
                            break;
                    }
                }
            }

            return new UifJsonResult(true, parametrizationResult);
        }

        /// <summary>
        /// Genera archivo excel de plan de pago
        /// </summary>
        /// <returns>Arhivo de excel de plan de pago</returns>
        public ActionResult GenerateFileToPaymentMethod()
        {
            try
            {
                PaymentMethodsServiceModel paymentMethodServiceModel = DelegateService.UnderwritingParamServiceWeb.GetPaymentMethod();
                ExcelFileServiceModel excelFileServiceModel = DelegateService.UnderwritingParamServiceWeb.GenerateFileToPaymentMethod(paymentMethodServiceModel.PaymentMethodServiceModel, App_GlobalResources.Language.LabelPaymentMethod);
                if (excelFileServiceModel.ErrorTypeService == ErrorTypeService.Ok)
                {
                    var urlFile = excelFileServiceModel.FileData;
                    return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile);
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