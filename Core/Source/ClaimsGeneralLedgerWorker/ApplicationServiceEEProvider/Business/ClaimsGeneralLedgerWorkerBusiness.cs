using System;
using System.Collections.Generic;
using GLMO = Sistran.Core.Application.GeneralLedgerServices.DTOs;
using Sistran.Core.Application.GeneralLedgerServices.DTOs.AccountingRules;
using System.Linq;
using Sistran.Core.Integration.ClaimsGeneralLedgerWorkerServices.DTOs;
using Sistran.Core.Application.GeneralLedgerServices.Enums;
using Sistran.Core.Application.ClaimsGeneralLedgerWorkerServices.EEProvider.Assemblers;
using System.Diagnostics;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.Utilities.Managers;

namespace Sistran.Core.Application.ClaimsGeneralLedgerWorkerServices.EEProvider.Business
{
    public class ClaimsGeneralLedgerWorkerBusiness
    {
        public List<GLMO.JournalEntryItemDTO> CalculateDifferenceIntoDebitsAndCredits(List<GLMO.JournalEntryItemDTO> journalEntryItemDTOs)
        {
            decimal debits = journalEntryItemDTOs.Where(x => x.AccountingNature == (int)AccountingNatures.Debit).Sum(x => x.Amount.Value);
            decimal credits = journalEntryItemDTOs.Where(x => x.AccountingNature == (int)AccountingNatures.Credit).Sum(x => x.Amount.Value);

            if (Math.Abs(debits - credits) > 0)
            {
                journalEntryItemDTOs.LastOrDefault().Amount.Value += (debits - credits);
                journalEntryItemDTOs.LastOrDefault().LocalAmount.Value += (debits - credits) * journalEntryItemDTOs.LastOrDefault().ExchangeRate.SellAmount;
            }

            return journalEntryItemDTOs;
        }

        public static void GetAccountingAccounts(AccountingPaymentRequestDTO accountingPaymentRequest, int moduleId)
        {
            try
            {
                accountingPaymentRequest.Vouchers.ForEach(voucher =>
                {
                    voucher.Concepts.ForEach(concept =>
                    {
                        GLMO.AccountingAccountDTO conceptsAccountingAccountDTO = new GLMO.AccountingAccountDTO();

                        conceptsAccountingAccountDTO = DelegateService.generalLedgerService.GetAccountingNumberByAccountingConcept(new GLMO.AccountingParameterDTO { BranchId = accountingPaymentRequest.BranchId, PrefixId = accountingPaymentRequest.PrefixId, AccountingConceptId = concept.Id.ToString() }).ValidateTextReplaceInAccountingAccountNumber(accountingPaymentRequest.PrefixId);

                        if (conceptsAccountingAccountDTO == null || conceptsAccountingAccountDTO.AccountingAccountId == 0)
                        {
                            throw new BusinessException(Resources.Resources.ErrorAccountingAccount);
                        }

                        concept.AccountingAccountId = conceptsAccountingAccountDTO.AccountingAccountId;
                        concept.AccountingAccountNumber = conceptsAccountingAccountDTO.Number;
                        concept.AccountingNatureId = conceptsAccountingAccountDTO.AccountingNature;

                        concept.Taxes.ForEach(tax =>
                        {
                            GLMO.AccountingAccountDTO taxesAccountingAccountDTO = new GLMO.AccountingAccountDTO();

                            taxesAccountingAccountDTO = DelegateService.generalLedgerService.GetAccountingNumberByAccountingConcept(new GLMO.AccountingParameterDTO { BranchId = accountingPaymentRequest.BranchId, PrefixId = accountingPaymentRequest.PrefixId, AccountingConceptId = tax.AccountingConceptId.ToString() }).ValidateTextReplaceInAccountingAccountNumber(accountingPaymentRequest.PrefixId);

                            if (taxesAccountingAccountDTO == null || taxesAccountingAccountDTO.AccountingAccountId == 0)
                            {
                                throw new BusinessException(Resources.Resources.ErrorAccountingAccount);
                            }

                            tax.AccountingAccountId = taxesAccountingAccountDTO.AccountingAccountId;
                            tax.AccountingAccountNumber = taxesAccountingAccountDTO.Number;
                            tax.AccountingNatureId = taxesAccountingAccountDTO.AccountingNature;
                        });
                    });
                });

                accountingPaymentRequest.CoInsuranceAssigneds.ForEach(coinsurance =>
                {
                    GLMO.AccountingAccountDTO coinsurancesAccountingAccountDTO = new GLMO.AccountingAccountDTO();

                    ResultDTO accountingAccountResult = DelegateService.entryParameterApplicationService.ExecuteAccountingRulePackage(moduleId, DTOAssembler.CreateCoinsuranceRuleParameters(accountingPaymentRequest, coinsurance), "201").FirstOrDefault();
                    if (accountingAccountResult != null)
                    {
                        coinsurancesAccountingAccountDTO.Number = accountingAccountResult.AccountingAccount;
                        coinsurancesAccountingAccountDTO = DelegateService.generalLedgerService.GetAccountingAccountsByNumberDescription(coinsurancesAccountingAccountDTO).First().ValidateTextReplaceInAccountingAccountNumber(accountingPaymentRequest.PrefixId);
                        coinsurancesAccountingAccountDTO.AccountingNature = accountingAccountResult.AccountingNature;

                        if (coinsurancesAccountingAccountDTO.AccountingAccountId == 0)
                        {
                            throw new BusinessException(Resources.Resources.ErrorAccountingAccount);
                        }

                        coinsurance.AccountingAccountId = coinsurancesAccountingAccountDTO.AccountingAccountId;
                        coinsurance.AccountingAccountNumber = coinsurancesAccountingAccountDTO.Number;
                        coinsurance.AccountingNatureId = coinsurancesAccountingAccountDTO.AccountingNature;
                    }
                });

                GLMO.AccountingAccountDTO paymentRequestAccountingAccountDTO = new GLMO.AccountingAccountDTO();

                if (accountingPaymentRequest.SalvageId > 0 || accountingPaymentRequest.RecoveryId > 0) //Solicitud de cobro
                {
                    ResultDTO accountingAccountResult = DelegateService.entryParameterApplicationService.ExecuteAccountingRulePackage(moduleId, DTOAssembler.CreateChargeRequestRuleParameters(accountingPaymentRequest), "202").FirstOrDefault();
                    if (accountingAccountResult != null)
                    {
                        paymentRequestAccountingAccountDTO.Number = accountingAccountResult.AccountingAccount;
                        paymentRequestAccountingAccountDTO = DelegateService.generalLedgerService.GetAccountingAccountsByNumberDescription(paymentRequestAccountingAccountDTO).First().ValidateTextReplaceInAccountingAccountNumber(accountingPaymentRequest.PrefixId);
                        paymentRequestAccountingAccountDTO.AccountingNature = accountingAccountResult.AccountingNature;
                    }
                }
                else
                {
                    ResultDTO accountingAccountResult = DelegateService.entryParameterApplicationService.ExecuteAccountingRulePackage(moduleId, DTOAssembler.CreatePaymentRequestRuleParameters(accountingPaymentRequest), "201").FirstOrDefault();
                    if (accountingAccountResult != null)
                    {
                        paymentRequestAccountingAccountDTO.Number = accountingAccountResult.AccountingAccount;
                        paymentRequestAccountingAccountDTO = DelegateService.generalLedgerService.GetAccountingAccountsByNumberDescription(paymentRequestAccountingAccountDTO).First().ValidateTextReplaceInAccountingAccountNumber(accountingPaymentRequest.PrefixId);
                        paymentRequestAccountingAccountDTO.AccountingNature = accountingAccountResult.AccountingNature;
                    }
                }

                if (paymentRequestAccountingAccountDTO.AccountingAccountId > 0)
                {
                    accountingPaymentRequest.AccountingAccountId = paymentRequestAccountingAccountDTO.AccountingAccountId;
                    accountingPaymentRequest.AccountingAccountNumber = paymentRequestAccountingAccountDTO.Number;
                    accountingPaymentRequest.AccountingNatureId = paymentRequestAccountingAccountDTO.AccountingNature;
                }
                else
                {
                    throw new BusinessException(Resources.Resources.ErrorAccountingAccount);
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("ErrorAccounting", "Error:" + ExceptionManager.GetMessage(ex, ex.GetBaseException().Message));
                                
                throw new BusinessException(ex.GetBaseException().Message);
            }
        }
    }
}
