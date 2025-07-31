// -----------------------------------------------------------------------
// <copyright file="PaymentPlanController.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using AutoMapper;
    using Sistran.Core.Application.CommonService.Models;
    using Sistran.Core.Application.ModelServices.Models.Param;
    using Sistran.Core.Application.ModelServices.Models.UnderwritingParam;
    using Sistran.Core.Application.UnderwritingServices.Models;
    using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
    using Sistran.Core.Framework.UIF.Web.Helpers;
    using Sistran.Core.Framework.UIF.Web.Models;
    using Sistran.Core.Framework.UIF.Web.Services;
    using static Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Enums.ParametrizationTypes;
    using ENUMSM = Sistran.Core.Application.ModelServices.Enums;
    using ENUMISS = Sistran.Core.Application.UnderwritingServices.Enums;


    /// <summary>
    /// Controlador de plan de pago
    /// </summary>
    public class PaymentPlanController : Controller
    {
        /// <summary>
        /// Vista de plan de pago
        /// </summary>
        /// <returns>Retorna vista de plan de pago</returns>
        public ActionResult PaymentPlan()
        {
            return this.View();
        }

        /// <summary>
        /// Vista de dropdow de plan de pago
        /// </summary>
        /// <returns>Retorna vista de dropdown plan de pago</returns>
        public ActionResult AdvancedSearch()
        {
            return this.View();
        }

        /// <summary>
        /// Obtiene planes de pago
        /// </summary>
        /// <returns>Retorna listado de plane de pago</returns>
        public ActionResult GetParametrizationPaymentPlans()
        {
            PaymentPlansServiceModel paymentPlansServiceModel = DelegateService.UnderwritingParamServiceWeb.GetPaymentPlansServiceModel();
            if (paymentPlansServiceModel.ErrorTypeService == ENUMSM.ErrorTypeService.Ok)
            {
                List<PaymentPlanViewModel> parametrizationPaymentPlanVM = new List<PaymentPlanViewModel>();
                var imapperPaymentPlan = ModelAssembler.CreateMapPaymentPlan();
                foreach (var item in paymentPlansServiceModel.PaymentPlanServiceModels)
                {
                    parametrizationPaymentPlanVM.Add(imapperPaymentPlan.Map<PaymentPlanServiceModel, PaymentPlanViewModel>(item));
                }

                return new UifJsonResult(true, parametrizationPaymentPlanVM);
            }
            else
            {
                return new UifJsonResult(false, new { paymentPlansServiceModel.ErrorTypeService, paymentPlansServiceModel.ErrorDescription });
            }
        }

        /// <summary>
        /// Obtiene planes de pago
        /// </summary>
        /// <returns>Retorna listado de plane de pago</returns>
        public ActionResult GetPaymentPlansByDescription(string description)
        {
            PaymentPlansServiceModel paymentPlansServiceModel = DelegateService.UnderwritingParamServiceWeb.GetPaymentPlansByDescription(description);
            if (paymentPlansServiceModel.ErrorTypeService == ENUMSM.ErrorTypeService.Ok)
            {
                List<PaymentPlanViewModel> parametrizationPaymentPlanVM = new List<PaymentPlanViewModel>();
                var imapperPaymentPlan = ModelAssembler.CreateMapPaymentPlan();
                foreach (var item in paymentPlansServiceModel.PaymentPlanServiceModels)
                {
                    parametrizationPaymentPlanVM.Add(imapperPaymentPlan.Map<PaymentPlanServiceModel, PaymentPlanViewModel>(item));
                }

                return new UifJsonResult(true, parametrizationPaymentPlanVM);
            }
            else
            {
                return new UifJsonResult(false, new { paymentPlansServiceModel.ErrorTypeService, paymentPlansServiceModel.ErrorDescription });
            }
        }

        /// <summary>
        /// Obtiene tipo de cuotas
        /// </summary>
        /// <returns>Listado de los tipo de cuota</returns>
        public ActionResult GetQuotaTypes()
        {
            return new UifJsonResult(true, EnumsHelper.GetItems<QuotaType>());
        }

        /// <summary>
        /// Obtiene tipos de plan de pago
        /// </summary>
        /// <returns>Listado de tipos de planes de pago</returns>
        public ActionResult GetPaymentCalculationTypes()
        {
            return new UifJsonResult(true, EnumsHelper.GetItems<PaymentCalculationType>());
        }

        /// <summary>
        /// Obtiene cuotas de plan de pago por id de plan de pago
        /// </summary>
        /// <param name="paymentPlanId">id de plan de pago</param>
        /// <returns>Cuota asociada a plan de pago</returns>
        public ActionResult GetPaymentDistributionByPaymentPlanId(int paymentPlanId)
        {
            List<PaymentDistribution> paymentDistribution = DelegateService.underwritingService.GetPaymentDistributionByPaymentPlanId(paymentPlanId);
            return new UifJsonResult(true, paymentDistribution.Select(b => b.Percentage).ToList());
        }

        /// <summary>
        /// CRUD de plan de pago
        /// </summary>
        /// <param name="parametrizationPaymentPlanVM">Listado de Plan de pago VM</param>
        /// <returns>Conteo de CRUD de la operacion</returns>
        public ActionResult CreateParametrizationPaymentPlan(List<PaymentPlanViewModel> parametrizationPaymentPlanVM)
        {
            List<PaymentPlanServiceModel> paymentPlanServiceModel = new List<PaymentPlanServiceModel>();
            var imapperPaymentPlanView = ModelAssembler.CreateMapPaymentPlanView();
            for (int i = 0; i < parametrizationPaymentPlanVM.Count; i++)
            {
                paymentPlanServiceModel.Add(imapperPaymentPlanView.Map<PaymentPlanViewModel, PaymentPlanServiceModel>(parametrizationPaymentPlanVM[i]));
                if (parametrizationPaymentPlanVM[i].QuotasServiceModel != null)
                {
                    for (int j = 0; j < parametrizationPaymentPlanVM[i].QuotasServiceModel.Count; j++)
                    {
                        parametrizationPaymentPlanVM[i].QuotasServiceModel[j].StatusTypeService = ENUMSM.StatusTypeService.Original;
                    }
                }

                paymentPlanServiceModel[i].QuotasServiceModel = parametrizationPaymentPlanVM[i].QuotasServiceModel;
            }

            List<PaymentPlanServiceModel> paymentPlanServiceModels = DelegateService.UnderwritingParamServiceWeb.ExecuteOperationsPaymentPlanServiceModel(paymentPlanServiceModel);
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

        /// <summary>
        /// Genera archivo excel de plan de pago
        /// </summary>
        /// <returns>Arhivo de excel de plan de pago</returns>
        public ActionResult GenerateFileToExport()
        {
            try
            {
                PaymentPlansServiceModel paymentPlansServiceModel = DelegateService.UnderwritingParamServiceWeb.GetPaymentPlansServiceModel();
                ExcelFileServiceModel excelFileServiceModel = DelegateService.UnderwritingParamServiceWeb.GenerateFileToPaymentPlan(paymentPlansServiceModel.PaymentPlanServiceModels, App_GlobalResources.Language.LabelPaymentPlan);
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
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
            }
        }

        public ActionResult GetTypeComponent()
        {
            try
            {
                List<ComponentType> componentType = DelegateService.commonService.GetComponentType();
                foreach (var item in componentType)
                {

                    switch (item.ComponentTypeId)
                    {
                        case (int)ENUMISS.ComponentType.Discounts:
                            item.SmallDescription = "Descuentos";
                            break;
                        case (int)ENUMISS.ComponentType.Expenses:
                            item.SmallDescription = "Gastos";
                            break;
                        case (int)ENUMISS.ComponentType.Premium:
                            item.SmallDescription = "Prima";
                            break;
                        case (int)ENUMISS.ComponentType.Surcharges:
                            item.SmallDescription = "Surcharges";
                            break;
                        case (int)ENUMISS.ComponentType.Taxes:
                            item.SmallDescription = "Iva";
                            break;
                        default:
                            break;
                    }
                }
            

                return new UifJsonResult(true, componentType.OrderBy(b => b.SmallDescription).ToList());
        }
            catch (Exception ex)
            {

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
    }
}
    }
}