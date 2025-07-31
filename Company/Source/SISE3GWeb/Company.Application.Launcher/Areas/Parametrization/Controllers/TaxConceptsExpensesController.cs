using Sistran.Core.Application.CommonService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Application.GeneralLedgerServices.DTOs.AccountingConcepts;
using Sistran.Core.Application.TaxServices.Models;
using Sistran.Core.Application.GeneralLedgerServices.DTOs;
using Sistran.Core.Application.TaxServices.DTOs;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    public class TaxConceptsExpensesController : Controller
    {
        // GET: Parametrization/TaxConceptsExpenses
        public ActionResult TaxConceptsExpenses()
        {
            return this.View();
        }

        public ActionResult GetBranchs()
        {
            try
            {
                List<Branch> branch = DelegateService.commonService.GetBranches();
                return new UifJsonResult(true, branch.OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetBranches);
            }
        }

        public ActionResult GetAccountingConceptsByBranchId(int branchId)
        {
            try
            {
                List<AccountingConceptDTO> accountingConcepts = DelegateService.glAccountingApplicationService.GetAccountingConceptsByBranchId(branchId);
                return new UifJsonResult(true, accountingConcepts);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetPaymentConcepts);
            }
        }

        public ActionResult GetTaxes()
        {
            try
            {
                List<Tax> taxes = DelegateService.taxService.GetTax();
                return new UifJsonResult(true, taxes);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGettingTaxes);
            }
        }

        public ActionResult GetTaxCategoriesByTaxId(int taxId)
        {
            try
            {
                List<TaxCategory> taxCategories = DelegateService.taxService.GetTaxCategoryByTaxId(taxId);
                return new UifJsonResult(true, taxCategories);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGettingTaxes);
            }
        }

        public ActionResult CreateAccountingConceptTaxes(List<AccountingConceptTaxDTO> accountingConceptTaxesDTO)
        {
            try
            {
                List<AccountingConceptTaxDTO> accountingConceptTaxDTO = DelegateService.taxService.CreateAccountingConceptTaxes(accountingConceptTaxesDTO);
                return new UifJsonResult(true, accountingConceptTaxDTO);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreattingAccountingConceptTaxes);
            }
        }

        public ActionResult GetAccountingConceptTaxesByAccountingConceptIdBranchId(int accountingConceptId, int branchId)
        {
            try
            {
                List<AccountingConceptTaxDTO> accountingConceptTaxDTOs = DelegateService.taxService.GetAccountingConceptTaxesByAccountingConceptIdBranchId(accountingConceptId, branchId);
                return new UifJsonResult(true, accountingConceptTaxDTOs);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGettingAccountingConceptTaxes);
            }
        }
    }
}