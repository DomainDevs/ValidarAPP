using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using ENUMPARAM = Sistran.Core.Application.UnderwritingParamService.Enums;
using Sistran.Core.Framework.UIF.Web.Helpers;
using MODEL = Sistran.Core.Application.ModelServices.Models.Underwriting;
using Sistran.Core.Application.ModelServices.Enums;
using Sistran.Core.Application.ModelServices.Models.Underwriting;
using Sistran.Core.Application.ModelServices.Models.Param;
using Sistran.Core.Services.UtilitiesServices.Models;
using modelServicesParam =  Sistran.Company.Application.ModelServices.Models.Param;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    public class ExpenseController : Controller
    {
        // GET: Parametrization/Expense
        [HttpGet]
        public ActionResult Expense()
        {            
            return View();
        }
        
        // Recibe listado de elementos a modificar.
        public ActionResult SaveExpense(List<ExpenseViewModel> lstExpense)
        {
            List<ExpenseServiceModel> expense = new List<ExpenseServiceModel>();
            expense = ModelAssembler.CreateExpensesServiceModel(lstExpense);
            expense = DelegateService.UnderwritingParamServiceWeb.ExecuteOperationsExpense(expense);
            modelServicesParam.ParametrizationResult parametrizationResult = new modelServicesParam.ParametrizationResult();
            foreach (var item in expense)
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


        private static List<MODEL.ExpenseServiceModel> listExpense;

        // GET: Parametrization/GetDataSectionExpenses
        [HttpGet]
        public UifJsonResult GetDataSectionExpenses()
        {
            MODEL.ExpensesServiceModel expenseViewModel = DelegateService.UnderwritingParamServiceWeb.GetExpenseServiceModel();
            if (expenseViewModel.ErrorTypeService == ErrorTypeService.Ok)
            {
                listExpense = expenseViewModel.ComponentServiceModel;
                return new UifJsonResult(true, expenseViewModel.ComponentServiceModel);
            }
            else
            {
                return new UifJsonResult(false, new { expenseViewModel.ErrorTypeService, expenseViewModel.ErrorDescription });
            }
        }

        // GET: Parametrization/GetRateTypes
        [HttpGet]
        public UifJsonResult GetRateTypes()
        {
            List<MODEL.RateTypeServiceQueryModel> rateTypeServiceQueryModel = new List<MODEL.RateTypeServiceQueryModel>();
            MODEL.RateTypeServicesQueryModel rateTypeServicesQueryModel = DelegateService.UnderwritingParamServiceWeb.GetRateType();
            return new UifJsonResult(true, rateTypeServicesQueryModel);
        }

        // GET: Parametrization/GetExecutionTypes
        [HttpGet]
        public UifJsonResult GetExecutionTypes()
        {
            try
            {
                return new UifJsonResult(true, EnumsHelper.GetItems<ENUMPARAM.RuteSet>());
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorRateType);
            }
        }


        [HttpGet]
        public UifJsonResult GetRules()
        {
            List<MODEL.RuleSetServiceQueryModel> ruleSetServiceQueryModel = new List<MODEL.RuleSetServiceQueryModel>();
            MODEL.RulesSetServiceQueryModel rulesSetServiceQueryModel = DelegateService.UnderwritingParamServiceWeb.GetRuleSet();
            return new UifJsonResult(true, rulesSetServiceQueryModel);
        }

        /// <summary>
        /// Modelo punto de venta
        /// </summary>
        private List<ExpenseViewModel> Exponses = new List<ExpenseViewModel>();

        [HttpPost]

        /// <summary>
        /// Lista los punto de venta
        /// </summary>  
        /// <param name="description">descripcion de punto de venta</param>
        /// <returns> retorna Lista de punto de venta </returns>
        public ActionResult GetExpenseByDescription(string description)
        {
                this.GetListExpenseDescription(description);
                return new UifJsonResult(true, this.Exponses.
                    Select(x => new
                    {
                        Id = x.id,
                        SmallDescription = x.Description,
                        TinyDescripcion = x.Abbreviation,
                        IsMandatory = x.Mandatory,
                        IsInitially = x.InitiallyIncluded,
                        Rate = x.Rate,
                        RateTypeServiceQueryModel = x.RateType,
                        RuleSetName = x.RuleSetName
                    }).OrderBy(x => x.SmallDescription).ToList());
            
        }

        /// <summary>
        /// obtiene lista de punto de venta
        /// </summary>
        /// <param name="description">descripcion de punto de venta</param>
        /// <returns> retorna la vista de punto de venta </returns>
        private List<ExpenseViewModel> GetListExpenseDescription(string description)
        {
            if (this.Exponses.Count == 0)
            {
                ExpensesServiceModel expensesServiceModel = DelegateService.UnderwritingParamServiceWeb.GetExpenseByDescription(description);

                this.Exponses = ModelAssembler.CreateExponseViewModel(expensesServiceModel);
                return this.Exponses.OrderBy(x => x.Description).ToList();
            }

            return this.Exponses;
        }

        public ActionResult ExpenseSearch()
        {
            return this.View();
        }

        /// <summary>
        /// Metodo GenerateFileToExport que genera archivo excel y lo retorna
        /// </summary>
        /// <returns>Excel de aliados</returns>
        public ActionResult GenerateFileToExport()
        {
            ExcelFileServiceModel urlFile = DelegateService.UnderwritingParamServiceWeb.GenerateFileToExpense(App_GlobalResources.Language.IssuanceExpenses);
            if (urlFile.ErrorTypeService == ErrorTypeService.Ok)
            {
                return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile.FileData);
            }
            else
            {
                return new UifJsonResult(false, string.Join("<br />", urlFile.ErrorDescription));
            }

        }
    }
}