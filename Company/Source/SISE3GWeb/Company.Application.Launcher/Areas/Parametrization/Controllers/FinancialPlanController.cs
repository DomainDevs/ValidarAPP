// -----------------------------------------------------------------------
// <copyright file="FinancialPlanController.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara Leiva</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Sistran.Core.Application.ModelServices.Models.Param;
    using Sistran.Core.Application.ModelServices.Models.UnderwritingParam;
    using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
    using Sistran.Core.Framework.UIF.Web.Models;
    using Sistran.Core.Framework.UIF.Web.Services;
    using ENUMSM = Sistran.Core.Application.ModelServices.Enums;

    public class FinancialPlanController : Controller
    {
        // GET: Parametrization/FinancialPlan
        public ActionResult FinancialPlan()
        {
            return this.View();
        }

        /// <summary>
        /// Consulta planes de pago
        /// </summary>
        /// <returns>Retorna lista planes de pago</returns>
        public ActionResult GetPaymentPlans()
        {
            PaymentPlansServiceModel paymentPlansService = DelegateService.UnderwritingParamServiceWeb.GetPaymentPlansServiceModel();
            if (paymentPlansService.ErrorTypeService == ENUMSM.ErrorTypeService.Ok)
            {
                return new UifJsonResult(true, paymentPlansService.PaymentPlanServiceModels.OrderBy(x => x.Description).ToList());
            }
            else
            {
                return new UifJsonResult(false, new { paymentPlansService.ErrorTypeService, paymentPlansService.ErrorDescription });
            }
        }

        /// <summary>
        /// Consulta metodo de pago
        /// </summary>
        /// <returns>Retorna lista metodo de pago</returns>
        public ActionResult GetPaymentMethods()
        {
            PaymentMethodsServiceQueryModel paymentMethodService = DelegateService.UnderwritingParamServiceWeb.GetMethodPaymentServiceModel();
            if (paymentMethodService.ErrorTypeService == ENUMSM.ErrorTypeService.Ok)
            {
                return new UifJsonResult(true, paymentMethodService.PaymentMethodServiceModels.OrderBy(x => x.Description).ToList());
            }
            else
            {
                return new UifJsonResult(false, new { paymentMethodService.ErrorTypeService, paymentMethodService.ErrorDescription });
            }
        }

        /// <summary>
        /// Consulta tipo de moneda
        /// </summary>
        /// <returns>Retorna lista monedas</returns>
        public ActionResult GetCurrencies() 
        {
            CurrenciesServiceQueryModel paymentMethodService = DelegateService.UnderwritingParamServiceWeb.GetCurrencies();
            if (paymentMethodService.ErrorTypeService == ENUMSM.ErrorTypeService.Ok)
            {
                return new UifJsonResult(true, paymentMethodService.CurrencyServiceModel.OrderBy(x => x.Description).ToList());
            }
            else
            {
                return new UifJsonResult(false, new { paymentMethodService.ErrorTypeService, paymentMethodService.ErrorDescription });
            }
        }

        /// <summary>
        /// Consulta de componentes
        /// </summary>
        /// <returns>listado de componentes</returns>
        public ActionResult GetComponents()
        {
            ComponentRelationsServiceModel component = DelegateService.UnderwritingParamServiceWeb.GetComponentRelationServiceModel();
            if (component.ErrorTypeService == ENUMSM.ErrorTypeService.Ok)
            {
                return new UifJsonResult(true, component.ComponentRelationServiceModels.OrderBy(x => x.Description).ToList());
            }
            else
            {
                return new UifJsonResult(false, new { component.ErrorTypeService, component.ErrorDescription });
            }
        }
        /// <summary>
        /// Metodo permite consulta por plan de pago y llena los campos siguientes
        /// </summary>
        /// <param name="description">Parametro descripcion</param>
        /// <returns>Retorna lista de coberturas por descripcion</returns>
        public ActionResult GetFinancialPlanForItems(int idPaymentPlan, int idPaymentMethod, int idCurrency)
        {
            try
            {
                FinancialPlansServiceModel financialPlanParametrization = DelegateService.UnderwritingParamServiceWeb.GetFinancialPlanForItems(idPaymentPlan, idPaymentMethod, idCurrency);

                if (financialPlanParametrization.FinancialPlanServiceModels.Count != 0)
                {
                    return new UifJsonResult(true, financialPlanParametrization);
                }

                return new UifJsonResult(false, financialPlanParametrization);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorConsultingFinancialPlan);
            }
        }

        #region exportar excel plan financiero
        /// <summary>
        /// Genera archivo excel de plan de pago
        /// </summary>
        /// <returns>Arhivo de excel de plan de pago</returns>
        public ActionResult GenerateFileToExport()
        {
            try
            {
                ExcelFileServiceModel excelFileServiceModel = DelegateService.UnderwritingParamServiceWeb.GenerateFileToFinancialPlan();
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
        #endregion

        #region CRUD plan financiero
        /// <summary>
        /// Crud de plan financiero
        /// </summary>
        /// <param name="coverage">plan financiero a crear</param>
        /// <returns>plan financiero a afectar</returns>
        public ActionResult ExecuteOperations(FinancialPlanViewModel financialPlan)
        {


            FinancialPlanServiceModel coverageServiceModel = ModelAssembler.CreateFinancialPlan(financialPlan);
            FinancialPlanServiceModel result = DelegateService.UnderwritingParamServiceWeb.ExecuteOperationFinancialPlan(coverageServiceModel);
            if (result.ErrorServiceModel.ErrorTypeService == ENUMSM.ErrorTypeService.Ok)
            {
                return new UifJsonResult(true, financialPlan.StatusTypeService);
            }
            else
            {
                return new UifJsonResult(false, result.ErrorServiceModel.ErrorDescription);
            }
        }
        #endregion
    }
}