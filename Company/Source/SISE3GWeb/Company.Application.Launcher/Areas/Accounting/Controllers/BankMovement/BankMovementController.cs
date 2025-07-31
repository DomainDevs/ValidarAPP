using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

//Sistran FWK
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.BankReconciliation;
using Sistran.Core.Framework.UIF.Web.Resources;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Framework.UIF2.Controls.UifTable;
using Sistran.Core.Framework.UIF2.Services;
using Sistran.Core.Framework.Exceptions;

//Sistran Core
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.ReconciliationServices;
using Sistran.Core.Application.ReconciliationServices.Models;
using Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Application.GeneralLedgerServices.DTOs;
using Sistran.Core.Application.GeneralLedgerServices.Enums;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers.BankMovement
{
    public class BankMovementController : Controller
    {
        #region Public Methods

        #region Views
        /// <summary>
        /// MainBankMovement
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult MainBankMovement()
        {
            try
            {
                return View();
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }        
        }

        /// <summary>
        /// BankCodeEquivalence
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult BankCodeEquivalence()
        {
            try
            {

                return View();
            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }
        }

        #endregion Views

        #region BankMovement

        /// <summary>
        /// GetBankMovements 
        /// Obtiene los movimientos bancarios
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetBankMovements()
        {
            
            List<ReconciliationMovementTypeDTO> reconciliationMovementTypes = DelegateService.glAccountingApplicationService.GetReconciliationMovementTypes();

            var bankMovements = from items in reconciliationMovementTypes
                                select new
                                {
                                    Id = items.Id,
                                    Description = items.Description,
                                    ShortDescription = items.SmallDescription,
                                    AccountingNatureId = (AccountingNatures)items.AccountingNature == AccountingNatures.Credit ? 1 : 2,
                                    AccountingNatureCompany = (AccountingNatures)items.AccountingNature == AccountingNatures.Credit ? "Crédito" : "Débito",
                                };

            return new UifTableResult(bankMovements);
        }

        /// <summary>
        /// SaveBankMovement
        /// Graba y actualiza los movimientos bancarios
        /// </summary>
        /// <param name="bankMovementModel">BankMovementModel</param>
        /// <param name="operationType">string</param>
        /// <returns>ActionResult</returns>
        public ActionResult SaveBankMovement(BankMovementModel bankMovementModel, string operationType)
        {
            List<object> bankMovements = new List<object>();

            ReconciliationMovementTypeDTO reconciliationMovementType = new ReconciliationMovementTypeDTO();

            reconciliationMovementType.Id = bankMovementModel.Id;
            reconciliationMovementType.Description = bankMovementModel.Description;
            reconciliationMovementType.SmallDescription = bankMovementModel.ShortDescription;
            reconciliationMovementType.AccountingNature = bankMovementModel.AccountingNatureCompany == 1 ? (int)AccountingNatures.Credit : (int)AccountingNatures.Debit;

            if (operationType.Equals("I"))
            {
                try
                {
                    DelegateService.glAccountingApplicationService.SaveReconciliationMovementType(reconciliationMovementType);
                    bankMovements.Add(new
                    {
                        BankMovementCode = 0,
                    });
                }
                catch (Exception ex)
                {
                    bankMovements.Add(new
                    {
                        BankMovementCode = -1,
                        MessageError = Global.MessageErrorSaveReconciliation + ex.Message
                    });
                }
            }
            if (operationType.Equals("U"))
            {
                try
                {
                    DelegateService.glAccountingApplicationService.UpdateReconciliationMovementType(reconciliationMovementType);
                    bankMovements.Add(new
                    {
                        BankMovementCode = 0,
                    });
                }
                catch (Exception ex)
                {
                    bankMovements.Add(new
                    {
                        BankMovementCode = -1,
                        MessageError = Global.MessageErrorUpdateReconciliation + ex.Message
                    });
                }
            }

            return Json(bankMovements, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// DeleteBankMovement
        /// Borra los movimientos bancarios
        /// </summary>
        /// <param name="bankMovementModel">BankMovementModel</param>
        /// <param name="operationType">string</param>
        /// <returns>ActionResult</returns>
        public ActionResult DeleteBankMovement(BankMovementModel bankMovementModel, string operationType)
        {
            bool isDeleted = false;
            List<object> bankMovements = new List<object>();

            ReconciliationMovementTypeDTO reconciliationMovementType = new ReconciliationMovementTypeDTO();

            reconciliationMovementType.Id = bankMovementModel.Id;

            if (operationType.Equals("D"))
            {
                isDeleted = DelegateService.glAccountingApplicationService.DeleteReconciliationMovementType(reconciliationMovementType);
                if (isDeleted)
                {
                    bankMovements.Add(new
                    {
                        BankMovementCode = 0,
                    });
                }
                else
                {
                    bankMovements.Add(new
                    {
                        BankMovementCode = -1,
                    });
                }
            }

            return Json(bankMovements, JsonRequestBehavior.AllowGet);
        }

        #endregion BankMovement

        #region BankCodeEquivalence

        /// <summary>
        /// GetBankCodesByBankId
        /// </summary>
        /// <param name="bankId">int</param>
        /// <returns>ActionResult</returns>
        public ActionResult GetBankCodesByBankId(int bankId)
        {
            Bank bank = new Bank();
            bank.Id = bankId;

             List<BankReconciliationMovementType> bankReconciliationMovementType = DelegateService.reconciliationService.GetBankReconciliationMovementTypes(bank);

            var bankCodesReconciliation = from items in bankReconciliationMovementType
                                          select new
                                          {
                                              Id = items.ReconciliationMovementType.Id,
                                              Description = items.ReconciliationMovementType.Description,
                                              BankMovementCode = items.SmallDescription,
                                              HasVoucher = items.VoucherNumber,
                                              BankId = items.Bank.Id,
                                              BankDescription = items.Bank.Description
                                          };

            return new UifTableResult(bankCodesReconciliation);
        }

        /// <summary>
        /// SaveBankCodeEquivalence
        /// </summary>
        /// <param name="bankCodeEquivalenceModel">BankCodeEquivalenceModel</param>
        /// <param name="operationType">string</param>
        /// <returns>ActionResult</returns>
        public ActionResult SaveBankCodeEquivalence(BankCodeEquivalenceModel bankCodeEquivalenceModel, string operationType)
        {
            List<object> bankCodeEquivalences = new List<object>();

            BankReconciliationMovementType bankReconciliationMovementType = new BankReconciliationMovementType();            
            bankReconciliationMovementType.Id = bankCodeEquivalenceModel.Id;
            bankReconciliationMovementType.SmallDescription = bankCodeEquivalenceModel.BankCode;
            bankReconciliationMovementType.ReconciliationMovementType = new ReconciliationMovementTypeDTO()
            {
                Id = Convert.ToInt32(bankCodeEquivalenceModel.BankMovementId)
            };
            
            bankReconciliationMovementType.Bank = new Bank() { Id = bankCodeEquivalenceModel.BankId };            
            bankReconciliationMovementType.VoucherNumber = bankCodeEquivalenceModel.HasVoucher;            
            

            if (operationType == "I")
            {
                try
                {
                    DelegateService.reconciliationService.SaveBankReconciliationMovementType(bankReconciliationMovementType);
                    bankCodeEquivalences.Add(new
                    {
                        ResultEquivalenceCode = 0,
                    });
                }
                catch (Exception ex)
                {
                    bankCodeEquivalences.Add(new { ResultEquivalenceCode = -1,
                        MessageError = Global.MessageErrorSaveReconciliation + ex.Message
                    });
                }
            }
            else if (operationType == "U")
            {
                try
                {

                    DelegateService.reconciliationService.UpdateBankReconciliationMovementType(bankReconciliationMovementType);
                    bankCodeEquivalences.Add(new
                    {
                        ResultEquivalenceCode = 0,
                    });
                }
                catch (Exception ex)
                {
                    bankCodeEquivalences.Add(new
                    {
                        ResultEquivalenceCode = -1,
                        MessageError = Global.MessageErrorUpdateReconciliation + ex.Message
                    });
                }
            }


            return Json(bankCodeEquivalences, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// DeleteBankCodeEquivalence
        /// </summary>
        /// <param name="bankCodeEquivalenceModel">BankCodeEquivalenceModel</param>
        /// <param name="operationType">string</param>
        /// <returns>ActionResult</returns>
        public ActionResult DeleteBankCodeEquivalence(BankCodeEquivalenceModel bankCodeEquivalenceModel, string operationType)
        {
            List<object> bankCodeEquivalences = new List<object>();
            BankReconciliationMovementType bankReconciliationMovementType = new BankReconciliationMovementType();
            
            bankReconciliationMovementType.SmallDescription = bankCodeEquivalenceModel.BankCode;
            bankReconciliationMovementType.ReconciliationMovementType = new ReconciliationMovementTypeDTO()
            {
                Id = Convert.ToInt32(bankCodeEquivalenceModel.BankMovementId)
            };

            bankReconciliationMovementType.Bank = new Bank() { Id = bankCodeEquivalenceModel.BankId };            

            if (operationType.Equals("D"))
            {                

                try
                {
                
                    DelegateService.reconciliationService.DeleteBankReconciliationMovementType(bankReconciliationMovementType);
                    bankCodeEquivalences.Add(new
                    {
                        ResultEquivalenceCode = 0,
                    });
                }
                catch (Exception ex)
                {
                    bankCodeEquivalences.Add(new
                    {
                        ResultEquivalenceCode = -1,
                        MessageError = ex.Message
                    });
                }
            }
           
            return Json(bankCodeEquivalences, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetSiseMovements
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetSiseMovements()
        {     
            List<ReconciliationMovementTypeDTO> reconciliationMovementTypes = DelegateService.glAccountingApplicationService.GetReconciliationMovementTypes();
            
            return new UifSelectResult(reconciliationMovementTypes);
        }

        #endregion BankCodeEquivalence

        #endregion
    }
}