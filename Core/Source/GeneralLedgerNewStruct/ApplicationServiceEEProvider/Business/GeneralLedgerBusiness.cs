using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.GeneralLedgerServices.Assemblers;
using Sistran.Core.Application.GeneralLedgerServices.DTOs;
using Sistran.Core.Application.GeneralLedgerServices.DTOs.AccountingRules;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Assemblers;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.DAOs;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.DAOs.Integration2G;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Transactions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using GENERALLEDGEREN = Sistran.Core.Application.GeneralLedger.Entities;

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.Business
{
    public class GeneralLedgerBusiness
    {
        public int SaveGenericJournalEntry(JournalParameter journalParameter)
        {
            int journalEntryId = 0;
            //using (Context.Current)
            //{
                //Listado de movimientos que se armarán con los valores obtenidos de la ejecución de reglas.
                List<JournalEntryItem> newJournalEntryItems = new List<JournalEntryItem>();
                List<JournalEntryItem> entryItems;

                //Transaction transaction = new Transaction();
                try
                {
                    int bridgeAccount = journalParameter.BridgeAccounting;
                    int moduleId = journalParameter.JournalEntry.ModuleDateId;
                    string codeRulePackage = journalParameter.CodeRulePackage;
                    if (journalParameter.OriginalGeneralLedger != null)
                    {
                        bridgeAccount = journalParameter.OriginalGeneralLedger.BridgeAccountingId;
                        moduleId = journalParameter.OriginalGeneralLedger.ModuleId;
                    }

                    //la longitud de la lista de parámetros tiene que ser la misma de la longitud de detalles del asiento.
                    for (int i = 0; i < journalParameter.JournalEntry.JournalEntryItems.Count; i++)
                    {
                        if (journalParameter.Parameters[i].Count > 0)
                        {
                            if (journalParameter.JournalEntry.JournalEntryItems[i].AccountingConcept > 0)
                            {
                                //se realiza el cálculo de los movimientos.
                                entryItems = AssembleAccountingJournalEntryItems(journalParameter.JournalEntry.JournalEntryItems[i], journalParameter.JournalEntry.JournalEntryItems[i].AccountingConcept);

                                if (entryItems.Count > 0)
                                {
                                    foreach (JournalEntryItem entryItem in entryItems)
                                    {
                                        newJournalEntryItems.Add(entryItem);
                                    }
                                }
                            }
                            else
                            {
                                //se realiza el cálculo de los movimientos.
                                entryItems = AssembleAccountingJournalEntryItems(journalParameter.JournalEntry.JournalEntryItems[i], moduleId, journalParameter.Parameters[i], codeRulePackage);

                                if (entryItems.Count > 0)
                                {
                                    foreach (JournalEntryItem entryItem in entryItems)
                                    {
                                        newJournalEntryItems.Add(entryItem);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (journalParameter.BridgeAccounts[i] > 0)
                            {
                                List<Models.AccountingRules.Parameter> itemsParameters = new List<Models.AccountingRules.Parameter>();
                                itemsParameters.Add(new Models.AccountingRules.Parameter() { Value = Convert.ToString(journalParameter.BridgeAccounts[i]) }); //tipo de pago crédito
                                itemsParameters.Add(new Models.AccountingRules.Parameter() { Value = Convert.ToString(journalParameter.JournalEntry.JournalEntryItems[i].Currency.Id, CultureInfo.InvariantCulture) }); //moneda
                                itemsParameters.Add(new Models.AccountingRules.Parameter() { Value = Convert.ToString(journalParameter.JournalEntry.JournalEntryItems[i].Amount.Value, CultureInfo.InvariantCulture) }); //valor
                                itemsParameters.Add(new Models.AccountingRules.Parameter() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                                itemsParameters.Add(new Models.AccountingRules.Parameter() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                                itemsParameters.Add(new Models.AccountingRules.Parameter() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                                itemsParameters.Add(new Models.AccountingRules.Parameter() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                                itemsParameters.Add(new Models.AccountingRules.Parameter() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                                itemsParameters.Add(new Models.AccountingRules.Parameter() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                                itemsParameters.Add(new Models.AccountingRules.Parameter() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                                itemsParameters.Add(new Models.AccountingRules.Parameter() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos

                                //se realiza el cálculo de los movimientos.
                                entryItems = AssembleAccountingJournalEntryItems(journalParameter.JournalEntry.JournalEntryItems[i],
                                    moduleId, itemsParameters,
                                    Convert.ToString(journalParameter.CodeRulePackage));

                                if (entryItems.Count > 0)
                                {
                                    foreach (JournalEntryItem entryItem in entryItems)
                                    {
                                        newJournalEntryItems.Add(entryItem);
                                    }
                                }
                            }
                            else
                            {
                                newJournalEntryItems.Add(journalParameter.JournalEntry.JournalEntryItems[i]);
                            }
                        }
                    }

                    // Se asigna los nuevos detalles generados al asiento.
                    journalParameter.JournalEntry.JournalEntryItems = newJournalEntryItems;
                    // Si los movimientos no están balanceados, genera un movimiento por cuenta puenta
                    if (!ValidateJournalEntryDebitsAndCredits(journalParameter.JournalEntry.JournalEntryItems))
                    {
                        decimal creditLocalAmount = newJournalEntryItems.Where(x => x.AccountingNature == (Enums.AccountingNatures.Credit)).Sum(x => x.LocalAmount.Value);
                        decimal debitLocalAmount = newJournalEntryItems.Where(x => x.AccountingNature == (Enums.AccountingNatures.Debit)).Sum(x => x.LocalAmount.Value);

                        decimal total = debitLocalAmount - creditLocalAmount;

                        JournalEntryItem journalEntryItem = new JournalEntryItem();
                        journalEntryItem.AccountingAccount = new AccountingAccount();
                        journalEntryItem.Amount = new Amount()
                        {
                            Currency = new Currency() { Id = 0 }
                        };
                        journalEntryItem.LocalAmount = new Amount()
                        {
                            Value = total
                        };
                        journalEntryItem.ExchangeRate = new ExchangeRate() { SellAmount = 1 };
                        journalEntryItem.Analysis = new List<Analysis>();
                        journalEntryItem.CostCenters = new List<CostCenter>();
                        journalEntryItem.Currency = new Currency() { Id = 0 };
                        journalEntryItem.Description = journalParameter.JournalEntry.Description;
                        journalEntryItem.EntryType = new EntryType();
                        journalEntryItem.Id = 0;
                        journalEntryItem.Individual = new UniquePersonService.V1.Models.Individual() { IndividualId = journalParameter.JournalEntry.JournalEntryItems[0].Individual.IndividualId };
                        journalEntryItem.PostDated = new List<PostDated>();
                        if (journalParameter.Receipt != null)
                        {
                            journalEntryItem.Receipt = new Receipt() { Number = journalParameter.Receipt.Number, Date = journalParameter.Receipt.Date };
                        }
                        else
                        {
                            journalEntryItem.Receipt = new Receipt() { Number = 0, Date = null };
                        }
                        if (journalParameter.ReconciliationMovementType != null)
                        {
                            journalEntryItem.ReconciliationMovementType = new ReconciliationMovementType() { Id = journalParameter.ReconciliationMovementType.Id };
                        }
                        else
                        {
                            journalEntryItem.ReconciliationMovementType = new ReconciliationMovementType() { Id = 0 };
                        }

                        journalEntryItem.SourceCode = 0;
                        journalEntryItem.Branch = new Branch() { Id = journalParameter.JournalEntry.Branch.Id };
                        journalEntryItem.SalePoint = new SalePoint() { Id = journalParameter.JournalEntry.SalePoint.Id };

                        if (bridgeAccount > 0)
                        {
                            // Movimientos con cuenta puente parametrizada
                            // Cálculo de la cuenta contable y la naturaleza y se arma la estructura de parámetros para su evaluación.
                            List<Models.AccountingRules.Parameter> itemsParameters = new List<Models.AccountingRules.Parameter>();
                            itemsParameters.Add(new Models.AccountingRules.Parameter() { Value = Convert.ToString(bridgeAccount) }); //tipo de pago crédito
                                                                                                                                     // Currency Code: Local currency
                            itemsParameters.Add(new Models.AccountingRules.Parameter() { Value = Convert.ToString(0, CultureInfo.InvariantCulture) }); //moneda
                            itemsParameters.Add(new Models.AccountingRules.Parameter() { Value = Convert.ToString(total, CultureInfo.InvariantCulture) }); //valor
                            itemsParameters.Add(new Models.AccountingRules.Parameter() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                            itemsParameters.Add(new Models.AccountingRules.Parameter() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                            itemsParameters.Add(new Models.AccountingRules.Parameter() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                            itemsParameters.Add(new Models.AccountingRules.Parameter() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                            itemsParameters.Add(new Models.AccountingRules.Parameter() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                            itemsParameters.Add(new Models.AccountingRules.Parameter() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                            itemsParameters.Add(new Models.AccountingRules.Parameter() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                            itemsParameters.Add(new Models.AccountingRules.Parameter() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
                                                                                                                                                        //Detalle con parámetros fijo
                                                                                                                                                        //se realiza el cálculo de los movimientos.
                            entryItems = AssembleAccountingJournalEntryItems(journalEntryItem, moduleId, itemsParameters, journalParameter.CodeRulePackage);

                            if (entryItems.Count > 0)
                            {
                                foreach (JournalEntryItem entryItem in entryItems)
                                {
                                    newJournalEntryItems.Add(entryItem);
                                }
                            }
                        }
                        else if (journalParameter.AccountingAccountId > 0)
                        {
                            // Boletas de depósito
                            decimal creditAmount = newJournalEntryItems.Where(x => x.AccountingNature == (Enums.AccountingNatures.Credit)).Sum(x => x.Amount.Value);
                            decimal debitAmount = newJournalEntryItems.Where(x => x.AccountingNature == (Enums.AccountingNatures.Debit)).Sum(x => x.Amount.Value);
                            if (newJournalEntryItems.FirstOrDefault() != null)
                            {
                                journalEntryItem.Amount.Currency.Id = newJournalEntryItems.FirstOrDefault().Currency.Id;
                                journalEntryItem.ExchangeRate.SellAmount = newJournalEntryItems.FirstOrDefault().ExchangeRate.SellAmount;
                                journalEntryItem.Currency = journalEntryItem.Amount.Currency;
                            }
                            journalEntryItem.Amount.Value = Math.Abs(debitAmount - creditAmount);

                            //se realiza el cálculo de los movimientos.
                            entryItems = AssembleAccountingJournalEntryItems(journalEntryItem, journalParameter.AccountingAccountId);

                            if (entryItems.Count > 0)
                            {
                                foreach (JournalEntryItem entryItem in entryItems)
                                {
                                    // La naturaleza depende del asiento
                                    if (creditAmount > debitAmount)
                                        entryItem.AccountingNature = (Enums.AccountingNatures.Debit);
                                    else
                                        entryItem.AccountingNature = (Enums.AccountingNatures.Credit);
                                    newJournalEntryItems.Add(entryItem);
                                }
                            }
                        }

                        //se asigna los nuevos detalles generados al asiento.
                        journalParameter.JournalEntry.JournalEntryItems = newJournalEntryItems;
                    }


                    //Valida débitos y créditos
                    if (ValidateJournalEntryDebitsAndCredits(journalParameter.JournalEntry.JournalEntryItems))
                    {
                        journalEntryId = SaveJournalEntryWithoutTransaction(journalParameter.JournalEntry);
                        if (journalEntryId > 0)
                        {
                            int technicalTransaction = journalParameter.JournalEntry.TechnicalTransaction;
                            if (technicalTransaction == 0)
                            {
                                var newJournalEntry = new JournalEntryDAO().GetJournalEntryByJournaEntryId(journalEntryId);
                                if (newJournalEntry != null)
                                    technicalTransaction = newJournalEntry.TechnicalTransaction;
                            }
                            CollectApplicationControlDAO integration2G = new CollectApplicationControlDAO();
                            integration2G.InsertOnce(ModelDTOAssembler.ToModelIntegrationOnce(technicalTransaction));
                            //transaction.Complete();
                        }
                    }
                    else
                    {
                        //asiento descuadrado
                        journalEntryId = 0;
                        //transaction.Dispose();
                    }
                }
                catch (BusinessException exception)
                {
                    var message = exception.Message; //mensaje para revisión de errores

                    //error en grabación de asiento.
                    journalEntryId = -2;
                    //transaction.Dispose();
                }
            //}
            return journalEntryId;
        }

        private List<JournalEntryItem> AssembleAccountingJournalEntryItems(JournalEntryItem journalEntryItem, int moduleDateId, List<Models.AccountingRules.Parameter> parameters, string codeRulePackage = "")
        {
            List<JournalEntryItem> newJournalEntryItems = new List<JournalEntryItem>();
            try
            {
                List<ResultDTO> results = DelegateService.entryParameterApplicationService.ExecuteAccountingRulePackage(moduleDateId, parameters.ToDTOs().ToList(), codeRulePackage);

                if (results.Count > 0)
                {
                    int decimalPlaces = 2;
                    foreach (ResultDTO result in results)
                    {
                        //Detalle
                        JournalEntryItem newJournalEntryItem = new JournalEntryItem();
                        newJournalEntryItem.AccountingAccount = new AccountingAccount();
                        newJournalEntryItem.AccountingAccount.Number = result.AccountingAccount;
                        var accountings = GetAccountingAccountsByNumberDescription(newJournalEntryItem.AccountingAccount);
                        newJournalEntryItem.AccountingAccount = accountings.Any() ? accountings.First() : new AccountingAccount();
                        newJournalEntryItem.AccountingNature = (Enums.AccountingNatures)Convert.ToInt32(result.AccountingNature);

                        if (!accountings.Any())
                        {
                            new LogJournalEntryItemDAO().SaveJournalEntryItem(new LogJournalEntryItem()
                            {
                                AccountNature = Convert.ToString(result.AccountingNature),
                                AccountNumber = Convert.ToString(result.AccountingAccount),
                                Amount = Convert.ToString(result.Parameter.Value),
                                CodePackagesRules = codeRulePackage,
                                CurrencyId = Convert.ToString(journalEntryItem.Currency.Id),
                                ModuleId = Convert.ToString(moduleDateId),
                                JeiJson = Newtonsoft.Json.JsonConvert.SerializeObject(journalEntryItem)
                            });
                        }

                        newJournalEntryItem.ExchangeRate = journalEntryItem.ExchangeRate;
                        newJournalEntryItem.ExchangeRate.SellAmount = journalEntryItem.ExchangeRate.SellAmount;

                        newJournalEntryItem.Amount = new Amount();
                        newJournalEntryItem.LocalAmount = new Amount();
                        newJournalEntryItem.Amount.Value = Math.Abs(Convert.ToDecimal(result.Parameter.Value, CultureInfo.InvariantCulture));
                        if (journalEntryItem.LocalAmount == null)
                        {
                            newJournalEntryItem.LocalAmount.Value = Math.Round(newJournalEntryItem.Amount.Value * journalEntryItem.ExchangeRate.SellAmount, decimalPlaces);
                        }
                        else
                        {
                            newJournalEntryItem.LocalAmount.Value = Math.Abs(journalEntryItem.LocalAmount.Value);
                        }

                        newJournalEntryItem.Analysis = new List<Analysis>();
                        newJournalEntryItem.ReconciliationMovementType = journalEntryItem.ReconciliationMovementType;
                        newJournalEntryItem.CostCenters = new List<CostCenter>();
                        newJournalEntryItem.Currency = journalEntryItem.Currency;
                        newJournalEntryItem.Description = journalEntryItem.Description;
                        newJournalEntryItem.EntryType = new EntryType();
                        newJournalEntryItem.Id = 0;
                        newJournalEntryItem.Individual = journalEntryItem.Individual;
                        newJournalEntryItem.PostDated = new List<PostDated>();
                        newJournalEntryItem.Receipt = journalEntryItem.Receipt;
                        newJournalEntryItem.SourceCode = journalEntryItem.SourceCode;
                        newJournalEntryItems.Add(newJournalEntryItem);
                    }
                }

                return newJournalEntryItems;
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        private bool ValidateJournalEntryDebitsAndCredits(List<JournalEntryItem> journalEntryItems)
        {
            bool isValid = false;

            try
            {
                decimal debits = 0;
                decimal credits = 0;

                foreach (JournalEntryItem journalEntryItem in journalEntryItems)
                {
                    if (journalEntryItem.AccountingNature == Enums.AccountingNatures.Debit)
                    {
                        debits = debits + journalEntryItem.LocalAmount.Value;
                    }
                    else
                    {
                        credits = credits + journalEntryItem.LocalAmount.Value;
                    }
                }

                if ((System.Math.Abs(debits) > 0) && (System.Math.Abs(credits) > 0))
                {
                    isValid = (System.Math.Abs(debits) == System.Math.Abs(credits));
                }
            }
            catch (BusinessException)
            {
                isValid = false;
            }

            return isValid;
        }

        private List<JournalEntryItem> AssembleAccountingJournalEntryItems(JournalEntryItem journalEntryItem, int accountingAccountId)
        {
            List<JournalEntryItem> newJournalEntryItems = new List<JournalEntryItem>();

            try
            {
                AccountingAccountBusiness accountingAccountBusiness = new AccountingAccountBusiness();
                var result = accountingAccountBusiness.GetAccountingAccountByAccountingAccountId(accountingAccountId);

                if (result != null && result.Number != "")
                {
                    //Detalle
                    JournalEntryItem newJournalEntryItem = new JournalEntryItem();
                    newJournalEntryItem.AccountingAccount = new AccountingAccount();
                    newJournalEntryItem.AccountingAccount.Number = result.Number;
                    newJournalEntryItem.AccountingAccount = result;
                    newJournalEntryItem.AccountingNature = result.AccountingNature;

                    newJournalEntryItem.ExchangeRate = journalEntryItem.ExchangeRate;
                    newJournalEntryItem.ExchangeRate.SellAmount = journalEntryItem.ExchangeRate.SellAmount;

                    newJournalEntryItem.Amount = new Amount();
                    newJournalEntryItem.Amount.Currency = journalEntryItem.Amount.Currency;
                    newJournalEntryItem.LocalAmount = new Amount();
                    newJournalEntryItem.Amount.Value = Math.Abs(journalEntryItem.Amount.Value);
                    newJournalEntryItem.LocalAmount.Value = Math.Abs(journalEntryItem.LocalAmount.Value);

                    newJournalEntryItem.Analysis = new List<Analysis>();
                    newJournalEntryItem.ReconciliationMovementType = journalEntryItem.ReconciliationMovementType;
                    newJournalEntryItem.CostCenters = new List<CostCenter>();
                    newJournalEntryItem.Currency = journalEntryItem.Currency;
                    newJournalEntryItem.Description = journalEntryItem.Description;
                    newJournalEntryItem.EntryType = new EntryType();
                    newJournalEntryItem.Id = 0;
                    newJournalEntryItem.Individual = journalEntryItem.Individual;
                    newJournalEntryItem.PostDated = new List<PostDated>();
                    newJournalEntryItem.Receipt = journalEntryItem.Receipt;
                    newJournalEntryItem.SourceCode = journalEntryItem.SourceCode;
                    newJournalEntryItems.Add(newJournalEntryItem);
                }

                return newJournalEntryItems;
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }



        /// <summary>
        /// SaveJournalEntry
        /// </summary>
        /// <param name="journalEntry"></param>
        /// <returns>int</returns>
        public int SaveJournalEntryWithoutTransaction(JournalEntry journalEntry)
        {
            int technicalTransaction = journalEntry.TechnicalTransaction;
            int journalEntryId = 0;

            try
            {
                JournalEntryDAO journalEntryDAO = new JournalEntryDAO();
                JournalEntryItemDAO journalEntryItemDAO = new JournalEntryItemDAO();
                JournalEntry newJournalEntry = new JournalEntry();
                if (technicalTransaction > 0)
                    newJournalEntry = journalEntryDAO.GetJournalEntryByTechnicalTransaction(technicalTransaction);
                else
                    newJournalEntry = journalEntryDAO.GetJournalEntryByJournaEntryId(journalEntry.Id);

                if (newJournalEntry.Id == 0)
                    newJournalEntry = journalEntryDAO.SaveJournalEntry(journalEntry);

                //si se graba la cabecera, se procede a grabar los movimientos.
                if (newJournalEntry.Id > 0)
                {
                    if (journalEntry.JournalEntryItems.Any())
                    {
                        bool validateIsNull = (journalEntry.ModuleDateId == 2 && journalEntry.JournalEntryItems.Count == journalEntry.JournalEntryItems.Count(x => x.AccountingAccount == null || x.AccountingAccount?.AccountingAccountId == 0));
                        if (validateIsNull)
                        {
                            int cashAccount = CommonBusiness.GetIntParameter(Enums.GeneralLederKeys.GL_ACCOUNTING_ACCOUNT_CASH);
                            int bridgeAccount = CommonBusiness.GetIntParameter(Enums.GeneralLederKeys.GL_ACCOUNTING_ACCOUNT_BRIDGE);

                            if (cashAccount > 0)
                            {
                                journalEntry.JournalEntryItems.ForEach(x => {
                                    if (x.AccountingNature == Enums.AccountingNatures.Debit)
                                    {
                                        x.AccountingAccount = new AccountingAccount()
                                        {
                                            AccountingAccountId = cashAccount
                                        };
                                    }
                                });
                            }
                            if (bridgeAccount > 0)
                            {
                                journalEntry.JournalEntryItems.ForEach(x => {
                                    if (x.AccountingNature == Enums.AccountingNatures.Credit)
                                    {
                                        x.AccountingAccount = new AccountingAccount()
                                        {
                                            AccountingAccountId = bridgeAccount
                                        };
                                    }
                                });
                            }
                        }

                        JournalEntryItem newJournalEntryItem;
                        foreach (JournalEntryItem journalEntryItem in journalEntry.JournalEntryItems)
                        {
                            newJournalEntryItem = journalEntryItemDAO.SaveJournalEntryItem(journalEntryItem, newJournalEntry.Id);
                            SaveItemGroup(journalEntryItem, newJournalEntryItem);
                        }
                        journalEntryId = newJournalEntry.Id;
                    }
                    else
                    {
                        //Si no existen items en el asiento elimina la cabecera y envía el código de mensaje de error de grabación de asiento.
                        journalEntryDAO.DeleteJournalEntry(journalEntry);
                        journalEntryId = -2;
                    }

                }
            }
            catch (BusinessException exception)
            {
                var message = exception.Message; //mensaje para revisión de errores
                journalEntryId = 0;
            }
            return journalEntryId;
        }

        public List<AccountingAccount> GetAccountingAccountsByNumberDescription(AccountingAccount accountingAccount)
        {
            List<AccountingAccount> filteredAccountingAccounts;
            try
            {

                // Creación del filtro 
                var criteriaBuilder = new ObjectCriteriaBuilder();
                if (!string.IsNullOrEmpty(accountingAccount.Number))
                {
                    criteriaBuilder.Property(GENERALLEDGEREN.AccountingAccount.Properties.AccountNumber);
                    criteriaBuilder.Like();
                    criteriaBuilder.Constant(accountingAccount.Number + "%");
                }

                if (!string.IsNullOrEmpty(accountingAccount.Description))
                {
                    criteriaBuilder.Or();
                    criteriaBuilder.Property(GENERALLEDGEREN.AccountingAccount.Properties.AccountName);
                    criteriaBuilder.Like();
                    criteriaBuilder.Constant(accountingAccount.Description);
                }

                // Asignamos BusinessCollection a una Lista
                BusinessCollection businessCollection = DataFacadeManager.GetObjects(
                    typeof(GENERALLEDGEREN.AccountingAccount), criteriaBuilder.GetPredicate(), null, 0, 10, false);

                List<AccountingAccount> accountingAccounts = new List<AccountingAccount>();

                foreach (GENERALLEDGEREN.AccountingAccount accountingAccountEntity in businessCollection.OfType<GENERALLEDGEREN.AccountingAccount>())
                {
                    Branch branch = new Branch();
                    branch.Id = Convert.ToInt32(accountingAccountEntity.DefaultBranchCode);

                    Currency currency = new Currency();
                    currency.Id = Convert.ToInt32(accountingAccountEntity.DefaultCurrencyCode);

                    Analysis analysis = new Analysis();
                    analysis.AnalysisId = Convert.ToInt32(accountingAccountEntity.AnalysisId);

                    accountingAccounts.Add(new AccountingAccount
                    {
                        AccountingAccountId = accountingAccountEntity.AccountingAccountId,
                        Number = accountingAccountEntity.AccountNumber,
                        Description = accountingAccountEntity.AccountName,
                        Branch = branch,
                        Currency = currency,
                        AccountingNature = (Enums.AccountingNatures)Convert.ToInt32(accountingAccountEntity.AccountingNature),
                        AccountingAccountParentId = Convert.ToInt32(accountingAccountEntity.AccountingAccountParentId)
                    });
                }

                filteredAccountingAccounts = accountingAccounts;
            }
            catch
            {
                filteredAccountingAccounts = new List<AccountingAccount>();
            }
            return filteredAccountingAccounts;
        }

        private void SaveItemGroup(JournalEntryItem journalEntryItem, JournalEntryItem newJournalEntryItem)
        {
            AnalysisDAO analysisDAO = new AnalysisDAO();
            PostDatedDAO postDatedDAO = new PostDatedDAO();
            CostCenterEntryDAO costCenterEntryDAO = new CostCenterEntryDAO();
            if (journalEntryItem.CostCenters.Any())
            {
                foreach (CostCenter costCenter in journalEntryItem.CostCenters)
                {
                    costCenterEntryDAO.SaveCostCenterEntry(costCenter, newJournalEntryItem.Id, true);
                }
            }

            if (journalEntryItem.Analysis.Any())
            {
                foreach (Analysis analysis in journalEntryItem.Analysis)
                {
                    int correlativeNumber = GetCorrelativeNumber(analysis.AnalysisId, analysis.AnalysisConcept.AnalysisConceptId, analysis.Key) + 1;
                    analysisDAO.SaveAnalysis(analysis, newJournalEntryItem.Id, correlativeNumber, true);
                }
            }

            if (journalEntryItem.PostDated.Any())
            {
                foreach (PostDated postDated in journalEntryItem.PostDated)
                {
                    postDatedDAO.SavePostDated(postDated, newJournalEntryItem.Id, true);
                }
            }
        }

        public int GetCorrelativeNumber(int analysisCodeId, int analysisConceptId, string key)
        {
            int correlativeNumber = 0;

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

            criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AnalysisEntryItem.Properties.AnalysisId, analysisCodeId);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AnalysisEntryItem.Properties.AnalysisConceptId, analysisConceptId);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(GENERALLEDGEREN.AnalysisEntryItem.Properties.ConceptKey, key);

            BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(GENERALLEDGEREN.AnalysisEntryItem), criteriaBuilder.GetPredicate());

            if (businessCollection.Count > 0)
            {
                var maxNumber = (from GENERALLEDGEREN.AnalysisEntryItem entryAnalisis in businessCollection select entryAnalisis.CorrelativeNumber).Max();
                correlativeNumber = Convert.ToInt32(maxNumber);
            }
            else
            {
                correlativeNumber = 1;
            }

            return correlativeNumber;
        }

        public int ReverseJournalEntry(JournalEntryReversionParameters reversionParameters)
        {
            JournalEntry newJournalEntry = null;
            JournalEntry journalEntry = GetJournalEntryByTechnicalTransaction(reversionParameters.TechnicalTransaction);

            using (Context.Current)
            {
                Transaction transaction = new Transaction();

                try
                {
                    if (journalEntry != null && journalEntry.Id > 0)
                    {
                        int journalEntryId = journalEntry.Id;

                        JournalEntryDAO journalEntryDAO = new JournalEntryDAO();
                        journalEntry.Id = 0;
                        journalEntry.RegisterDate = DateTime.Now;
                        journalEntry.AccountingDate = reversionParameters.AccountingDate;
                        journalEntry.TechnicalTransaction = reversionParameters.NewTechnicalTransaction;
                        journalEntry.UserId = reversionParameters.UserId;
                        journalEntry.Description = String.Format(Resources.Resources.ReversionMessage, reversionParameters.TechnicalTransaction);
                        newJournalEntry = journalEntryDAO.SaveJournalEntry(journalEntry);

                        if (newJournalEntry != null && newJournalEntry.Id > 0)
                        {
                            if (journalEntry.JournalEntryItems.Any())
                            {
                                foreach (JournalEntryItem journalEntryItem in journalEntry.JournalEntryItems)
                                {
                                    if (journalEntryItem.AccountingNature == (Enums.AccountingNatures.Credit))
                                        journalEntryItem.AccountingNature = (Enums.AccountingNatures.Debit);
                                    else
                                        journalEntryItem.AccountingNature = (Enums.AccountingNatures.Credit);

                                    journalEntryItem.Id = 0;
                                    if (journalEntryItem.Analysis.Any())
                                        journalEntryItem.Analysis.ForEach(analysis =>
                                        {
                                            analysis.Id = 0;
                                            analysis.EntryItemId = 0;
                                        });
                                    if (journalEntryItem.CostCenters.Any())
                                        journalEntryItem.CostCenters.ForEach(costCenter =>
                                        {
                                            costCenter.Id = 0;
                                            costCenter.EntryItemId = 0;
                                        });
                                }

                                AnalysisDAO analysisDAO = new AnalysisDAO();
                                CostCenterEntryDAO costCenterEntryDAO = new CostCenterEntryDAO();
                                JournalEntryItemDAO journalEntryItemDAO = new JournalEntryItemDAO();

                                journalEntry.JournalEntryItems.ForEach(item =>
                                {
                                    var newItem = journalEntryItemDAO.SaveJournalEntryItem(item, newJournalEntry.Id);
                                    if (newItem != null && newItem.Id > 0)
                                    {
                                        if (item.Analysis.Any())
                                        {
                                            item.Analysis.ForEach(analysis =>
                                            {
                                                analysisDAO.SaveAnalysis(analysis, newItem.Id, 0, true);
                                            });
                                        }
                                        if (item.CostCenters.Any())
                                        {
                                            item.CostCenters.ForEach(costCenter =>
                                            {
                                                costCenterEntryDAO.SaveCostCenterEntry(costCenter, newItem.Id, true);
                                            });
                                        }
                                    }
                                });
                            }
                            CollectApplicationControlDAO integration2G = new CollectApplicationControlDAO();
                            integration2G.InsertOnce(ModelDTOAssembler.ToModelIntegrationOnce(newJournalEntry.TechnicalTransaction));

                            EntryRevertionDAO entryRevertionDAO = new EntryRevertionDAO();
                            entryRevertionDAO.SaveEntryRevertion(0, journalEntryId, newJournalEntry.Id, journalEntry.UserId, DateTime.Now, true);
                        }
                    }

                    transaction.Complete();
                }
                catch (BusinessException ex)
                {
                    transaction.Dispose();
                }
            }
            if (newJournalEntry != null && newJournalEntry.Id > 0)
                return newJournalEntry.Id;
            return -1;
        }

        public JournalEntry GetJournalEntryByTechnicalTransaction(int technicalTransaction)
        {
            JournalEntryItemDAO journalEntryItemDAO = new JournalEntryItemDAO();
            JournalEntryDAO journalEntryDAO = new JournalEntryDAO();
            JournalEntry journalEntry = journalEntryDAO.GetJournalEntryByTechnicalTransaction(technicalTransaction);
            journalEntry.JournalEntryItems = new List<JournalEntryItem>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(GENERALLEDGEREN.JournalEntryItem.Properties.JournalEntryId, typeof(GENERALLEDGEREN.JournalEntryItem).Name, journalEntry.Id);

            BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(GENERALLEDGEREN.JournalEntryItem), filter.GetPredicate());
            //se llena los movimientos del asiento de diario
            if (businessCollection.Any())
            {
                foreach (GENERALLEDGEREN.JournalEntryItem journalEntryItemEntity in businessCollection.OfType<GENERALLEDGEREN.JournalEntryItem>())
                {
                    JournalEntryItem journalEntryItem = ModelAssembler.CreateJournalEntryItem(journalEntryItemEntity);
                    journalEntryItem.Analysis = new List<Analysis>();
                    journalEntryItem.Analysis = GetAnalysesByEntryId(journalEntryItemEntity.JournalEntryItemId, true);
                    journalEntryItem.CostCenters = new List<CostCenter>();
                    journalEntryItem.CostCenters = GetCostCentersByEntryId(journalEntryItemEntity.JournalEntryItemId, true);

                    journalEntry.JournalEntryItems.Add(journalEntryItem);
                }
            }
            return journalEntry;
        }

        public List<CostCenter> GetCostCentersByEntryId(int entryItemId, bool isJournalEntry)
        {
            List<CostCenter> costCenters = new List<CostCenter>();

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

            criteriaBuilder.PropertyEquals(GENERALLEDGEREN.CostCenterEntryItem.Properties.EntryItemId, entryItemId);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(GENERALLEDGEREN.CostCenterEntryItem.Properties.IsJournalEntry, isJournalEntry);

            BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(GENERALLEDGEREN.CostCenterEntryItem), criteriaBuilder.GetPredicate());

            if (businessCollection.Any())
            {
                foreach (GENERALLEDGEREN.CostCenterEntryItem entityCostCenterEntryItem in businessCollection.OfType<GENERALLEDGEREN.CostCenterEntryItem>())
                {
                    costCenters.Add(new CostCenter()
                    {
                        CostCenterId = Convert.ToInt32(entityCostCenterEntryItem.CostCenterId),
                        EntryItemId = Convert.ToInt32(entityCostCenterEntryItem.EntryItemId),
                        Id = Convert.ToInt32(entityCostCenterEntryItem.CostCenterEntryItemId),
                        PercentageAmount = Convert.ToDecimal(entityCostCenterEntryItem.CostCenterPercentage),
                        IsJournalEntry = Convert.ToBoolean(entityCostCenterEntryItem.IsJournalEntry)
                    });
                }
            }
            return costCenters;
        }

        private List<Analysis> GetAnalysesByEntryId(int entryItemId, bool isJournalEntry)
        {
            AnalysisDAO analysisDAO = new AnalysisDAO();
            List<Analysis> analyses = new List<Analysis>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(GENERALLEDGEREN.AnalysisEntryItem.Properties.EntryItemId, entryItemId);
            filter.And();
            filter.PropertyEquals(GENERALLEDGEREN.AnalysisEntryItem.Properties.IsJournalEntry, isJournalEntry);

            BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(GENERALLEDGEREN.AnalysisEntryItem), filter.GetPredicate());

            if (businessCollection.Any())
            {
                foreach (GENERALLEDGEREN.AnalysisEntryItem entityAnalysisEntryItem in businessCollection.OfType<GENERALLEDGEREN.AnalysisEntryItem>())
                {
                    analyses.Add(ModelAssembler.CreateAnalysis(entityAnalysisEntryItem));
                }
            }
            return analyses;
        }



        public bool UpdateJournalEntryStatusByTechnicalTransaction(int technicalTransaction, int status)
        {
            JournalEntryDAO journalEntryDAO = new JournalEntryDAO();
            return journalEntryDAO.UpdateJournalEntryStatusByTechnicalTransction(technicalTransaction, Convert.ToInt32(AccountingEntryStatus.Reverted));
        }
    }
}
