using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Sistran.Core.Application.AccountingGeneralLedgerServices.EEProvider.Enums;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.AccountingGeneralLedgerServices.DTOs;

namespace Sistran.Core.Application.AccountingGeneralLedgerServices.EEProvider.Business
{
    public class AccountingGeneralLedgerBusiness
    {
        public int GeneralLedgerforCollect(string accountigParameters)
        {
            #region Parameters
            JournalEntryParametersDTO journalEntryParametersDTO = new JournalEntryParametersDTO();
            journalEntryParametersDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<JournalEntryParametersDTO>(accountigParameters);

            if (journalEntryParametersDTO.Description == null)
            {
                journalEntryParametersDTO.Description = Resources.Resources.GeneralLedgerCashIncomeDescription + " ";
            }

            int moduleId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingGeneralLedgerKey>(AccountingGeneralLedgerKey.ACC_MODULE_DATE_ACCOUNTING).ToString());

            ReconciliationMovementTypeDTO bankReconciliation = new ReconciliationMovementTypeDTO();
            ReceiptDTO receipt = new ReceiptDTO();

            if (journalEntryParametersDTO.TypeId == 2) //rechazo de cheques
            {
                journalEntryParametersDTO.Description = Resources.Resources.GeneralLedgerCheckRejectionDescription + " ";
                bankReconciliation.Id = Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingGeneralLedgerKey>(AccountingGeneralLedgerKey.ACC_BANK_RECONCILIATION_RETURNED_CHECK));
                receipt.Date = DelegateService.commonServiceCore.GetModuleDateIssue(Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingGeneralLedgerKey>(AccountingGeneralLedgerKey.ACC_MODULE_DATE_ACCOUNTING).ToString()), DateTime.Now);
                receipt.Number = journalEntryParametersDTO.BillId;
            }
            if (journalEntryParametersDTO.TypeId == 3) //canje de cheques
            {
                journalEntryParametersDTO.Description = Resources.Resources.GeneralLedgerCheckExchangeDescription + " ";
            }
            if (journalEntryParametersDTO.TypeId == 4) //RECHAZO DE TARJETA
            {
                journalEntryParametersDTO.Description = Resources.Resources.GeneralLedgerCardRejectionDescription + " ";
                bankReconciliation.Id = Convert.ToInt32(Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingGeneralLedgerKey>(AccountingGeneralLedgerKey.ACC_BANK_RECONCILIATION_RETURNED_CARD)));
                receipt.Date = DelegateService.commonServiceCore.GetModuleDateIssue(Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingGeneralLedgerKey>(AccountingGeneralLedgerKey.ACC_MODULE_DATE_ACCOUNTING).ToString()), DateTime.Now);
                receipt.Number = journalEntryParametersDTO.BillId;
            }
            if (journalEntryParametersDTO.TypeId == 5) //CANJE DE TARJETA
            {
                journalEntryParametersDTO.Description = Resources.Resources.GeneralLedgerCardExchangeDescription + " ";
            }

            //Listado en donde se llevaran los grupos de parametros al servicio
            List<List<ParameterDTO>> parametersCollection = new List<List<ParameterDTO>>();

            #endregion Parameters

            #region JournalEntryHeader

            //Se genera la cabecera del asiento.
            JournalEntryDTO journalEntry = new JournalEntryDTO();

            int accountingCompanyId = (from item in DelegateService.accountingApplicationService.GetAccountingCompanies() where item.Default select item).ToList()[0].AccountingCompanyId;

            journalEntry.AccountingCompany = new AccountingCompanyDTO { AccountingCompanyId = accountingCompanyId }; //este dato se graba con este valor en billing                
                                                                                                                     //journalEntry.AccountingDate = DelegateService.commonServiceCore.GetModuleDateIssue(Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingGeneralLedgerKey>(AccountingGeneralLedgerKey.ACC_MODULE_DATE_ACCOUNTING).ToString()), DateTime.Now);
            journalEntry.AccountingDate = journalEntryParametersDTO.AccountingDate;
            journalEntry.AccountingMovementType = new AccountingMovementTypeDTO(); //este dato solo pertenece a asientos de mayor
            journalEntry.Branch = new BranchDTO() { Id = journalEntryParametersDTO.BranchId };
            journalEntry.Description = journalEntryParametersDTO.Description + Convert.ToString(journalEntryParametersDTO.TechnicalTransaction);
            journalEntry.EntryNumber = 0; //pendiente definición.
            journalEntry.Id = 0; //id autonumérico
            journalEntry.ModuleDateId = moduleId;
            journalEntry.RegisterDate = DateTime.Now;
            journalEntry.SalePoint = new SalePointDTO() { Id = journalEntryParametersDTO.SalePointId }; //no existe este dato en ingreso de caja
            journalEntry.Status = 1; //activo
            journalEntry.TechnicalTransaction = journalEntryParametersDTO.TechnicalTransaction;
            journalEntry.UserId = journalEntryParametersDTO.UserId;

            journalEntry.JournalEntryItems = new List<JournalEntryItemDTO>();

            #endregion JournalEntryHeader

            #region JournalEntryItem

            string description = journalEntryParametersDTO.Description;
            journalEntry.TechnicalTransaction = journalEntryParametersDTO.TechnicalTransaction;

            // Se arma los movimientos para los tipos de pago del recibo
            foreach (JournalEntryListParametersDTO accountingParameterDTO in journalEntryParametersDTO.JournalEntryListParameters)
            {
                // Cálculo de la cuenta contable y la naturaleza y se arma la estructura de parámetros para su evaluación.
                List<ParameterDTO> parameters = new List<ParameterDTO>();
                journalEntry.JournalEntryItems.Add(CreateJournalEntryItemforFixAccounting(accountingParameterDTO, description));

                if (accountingParameterDTO.PaymentMethodTypeCode != 6
                    && accountingParameterDTO.PaymentMethodTypeCode != 25)
                {
                    parameters = CreateParametersforCollectPayment(accountingParameterDTO);
                }
                parametersCollection.Add(parameters);
            }
            #endregion
            AccountingParameterDTO accountingParametersDTO = new AccountingParameterDTO();

            accountingParametersDTO.JournalEntry = journalEntry;
            accountingParametersDTO.Parameters = parametersCollection;
            accountingParametersDTO.ModuleId = moduleId;
            accountingParametersDTO.BridgeAccounting = journalEntryParametersDTO.BridgeAccoutingId;

            string accountingJournalEntryParametersCollection = Newtonsoft.Json.JsonConvert.SerializeObject(accountingParametersDTO);

            int entryNumber = DelegateService.generalLedgerService.AccountingWithoutTransactional(accountingJournalEntryParametersCollection);

            if (journalEntryParametersDTO.Application != null)
            {
                journalEntryParametersDTO.TypeId = moduleId;
                journalEntryParametersDTO.Description = Resources.Resources.ApplicationCollectDescription
                    + " " + journalEntryParametersDTO.TechnicalTransaction;
                return SaveJournalEntryforApplication(journalEntryParametersDTO, entryNumber);
            }
            return entryNumber;
        }

        private int SaveJournalEntryforApplication(JournalEntryParametersDTO journalEntryParametersDTO, int journalEntryId)
        {
            if (journalEntryParametersDTO.Application != null)
            {
                if (journalEntryParametersDTO.ApplicationItems != null
                    && journalEntryParametersDTO.ApplicationItems.Count > 0)
                {
                    JournalEntryDTO journalEntry = new JournalEntryDTO();
                    journalEntry.AccountingDate = journalEntryParametersDTO.AccountingDate;
                    journalEntry.AccountingMovementType = new AccountingMovementTypeDTO(); //este dato solo pertenece a asientos de mayor
                    journalEntry.Branch = new BranchDTO() { Id = journalEntryParametersDTO.BranchId };
                    journalEntry.Description = journalEntryParametersDTO.Description + Convert.ToString(journalEntryParametersDTO.TechnicalTransaction);
                    journalEntry.Id = journalEntryId; //id autonumérico
                    journalEntry.ModuleDateId = journalEntryParametersDTO.Application.ModuleId;
                    journalEntry.RegisterDate = DateTime.Now;
                    journalEntry.SalePoint = new SalePointDTO() { Id = journalEntryParametersDTO.SalePointId }; //no existe este dato en ingreso de caja
                    journalEntry.Status = 1; //activo
					journalEntry.TechnicalTransaction = journalEntryParametersDTO.Application.TechnicalTransaction;					
                    journalEntry.UserId = journalEntryParametersDTO.UserId;
                    journalEntry.JournalEntryItems = new List<JournalEntryItemDTO>();

                    string description = Resources.Resources.ApplicationDescription;
                    List<List<ParameterDTO>> parametersCollection = new List<List<ParameterDTO>>();
                    journalEntry.JournalEntryItems = new List<JournalEntryItemDTO>();
                    foreach (ApplicationJournalEntryDTO item in journalEntryParametersDTO.ApplicationItems)
                    {
                        JournalEntryItemDTO journalEntryItem = CreateJournalEntryItemforFixAccounting(item, description);
                        journalEntry.JournalEntryItems.Add(journalEntryItem);
                        parametersCollection.Add(new List<ParameterDTO>());
                    }
                    journalEntry.Description = journalEntryParametersDTO.Description;
                    AccountingParameterDTO accountingParametersDTO = new AccountingParameterDTO();
                    accountingParametersDTO.JournalEntry = journalEntry;
                    accountingParametersDTO.Parameters = parametersCollection;
                    accountingParametersDTO.ModuleId = journalEntryParametersDTO.Application.ModuleId;
                    accountingParametersDTO.BridgeAccounting = journalEntryParametersDTO.Application.BridgeAccountingId;
                    accountingParametersDTO.CodeRulePackage = journalEntryParametersDTO.CodeRulePackage;

                    if (journalEntryParametersDTO.OriginalGeneralLedger != null)
                    {
                        accountingParametersDTO.OriginalGeneralLedger = journalEntryParametersDTO.OriginalGeneralLedger;
                        accountingParametersDTO.CodeRulePackage = Convert.ToString(journalEntryParametersDTO.OriginalGeneralLedger.CodePackageRule);
                    }

                    string accountingJournalEntryParametersCollection = Newtonsoft.Json.JsonConvert.SerializeObject(accountingParametersDTO);
                    return DelegateService.generalLedgerService.SaveBasicJournalEntry(accountingJournalEntryParametersCollection);
                }
            }
            return journalEntryId;
        }

        private List<ParameterDTO> CreateParametersforCollectPayment(JournalEntryListParametersDTO parameter)
        {
            List<ParameterDTO> parameters = new List<ParameterDTO>();
            parameters.Add(new ParameterDTO() { Value = Convert.ToString(parameter.PaymentMethodTypeCode, CultureInfo.InvariantCulture) }); //tipo de pago
            parameters.Add(new ParameterDTO() { Value = Convert.ToString(parameter.CurrencyCode, CultureInfo.InvariantCulture) }); //moneda
            parameters.Add(new ParameterDTO() { Value = Convert.ToString(parameter.Amount, CultureInfo.InvariantCulture) }); //valor
            parameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
            parameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
            parameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
            parameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
            parameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
            parameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
            parameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos
            parameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); //parametro para uso de boleta de depósitos

            return parameters;
        }

        private JournalEntryItemDTO CreateJournalEntryItemforFixAccounting(JournalEntryListParametersDTO parameter, string description)
        {
            JournalEntryItemDTO journalEntryItem = new JournalEntryItemDTO()
            {
                Description = (parameter.Description == null) ? description : parameter.Description,
                EntryType = new EntryTypeDTO(),
                Id = 0,
                PostDated = new List<PostDatedDTO>(),
                Receipt = new ReceiptDTO() { Number = parameter.RecieptNumber, Date = parameter.RecieptDate },
                ReconciliationMovementType = new ReconciliationMovementTypeDTO() { Id = parameter.BankReconciliationId ?? 0 },
                SourceCode = parameter.PaymentCode,
                AccountingConcept = 0,
                Branch = new BranchDTO(),
                SalePoint = new SalePointDTO(),
                Analysis = new List<AnalysisDTO>(),
                CostCenters = new List<CostCenterDTO>(),
                Individual = new IndividualDTO()
                {
                    IndividualId = parameter.PayerId
                },
                AccountingAccount = new AccountingAccountDTO()
                {
                    AccountingAccountId = parameter.AccountingAccountId
                },
                ExchangeRate = new ExchangeRateDTO()
                {
                    SellAmount = parameter.ExchangeRate
                },
                Currency = new CurrencyDTO()
                {
                    Id = parameter.CurrencyCode
                },
                Amount = new AmountDTO()
                {
                    Currency = new CurrencyDTO() { Id = parameter.CurrencyCode },
                    Value = parameter.Amount
                },
                LocalAmount = new AmountDTO()
                {
                    Value = parameter.LocalAmount
                }
            };

            // Detalle con parámetros fijos.
            if (parameter.AccountingAccountId > 0)
            {
                journalEntryItem.AccountingNature = parameter.AccountingNature;
            }
            return journalEntryItem;
        }

        private JournalEntryItemDTO CreateJournalEntryItemforFixAccounting(ApplicationJournalEntryDTO parameter, string description)
        {
            JournalEntryItemDTO journalEntryItem = new JournalEntryItemDTO()
            {
                Description = (parameter.Description == null) ? description : parameter.Description,
                EntryType = new EntryTypeDTO(),
                Id = 0,
                PostDated = new List<PostDatedDTO>(),
                Receipt = new ReceiptDTO() { Number = 0 },
                ReconciliationMovementType = new ReconciliationMovementTypeDTO() { Id = 0 },
                SourceCode = parameter.Id,
                AccountingConcept = 0,
                PackageRuleCodeId = parameter.PackageRuleCodeId,
                BridgeAccountId = Convert.ToInt32(parameter.BridgeAccountId),
                Branch = new BranchDTO()
                {
                    Id = Convert.ToInt32(parameter.BranchId)
                },
                SalePoint = new SalePointDTO()
                {
                    Id = Convert.ToInt32(parameter.SalePointId)
                },
                Analysis = new List<AnalysisDTO>(),
                CostCenters = new List<CostCenterDTO>(),
                Individual = new IndividualDTO()
                {
                    IndividualId = Convert.ToInt32(parameter.IndividualId)
                },
                AccountingAccount = new AccountingAccountDTO()
                {
                    AccountingAccountId = parameter.AccountAccountingId
                },
                ExchangeRate = new ExchangeRateDTO()
                {
                    SellAmount = parameter.ExchangeRate
                },
                Currency = new CurrencyDTO()
                {
                    Id = parameter.CurrencyId
                },
                Amount = new AmountDTO()
                {
                    Currency = new CurrencyDTO() { Id = parameter.CurrencyId },
                    Value = parameter.Amount
                },
                LocalAmount = new AmountDTO()
                {
                    Value = parameter.LocalAmount
                }
            };

            //Detalle con parámetros fijos.
            if ((Convert.ToInt32(parameter.AccountAccountingId) > 0)
                || (Convert.ToInt32(parameter.BridgeAccountId) > 0))
            {
                journalEntryItem.AccountingNature = parameter.AccountingNature;
            }
            if (parameter.AccountingAnalysisCodes != null && parameter.AccountingAnalysisCodes.Count > 0)
            {
                journalEntryItem.Analysis = parameter.AccountingAnalysisCodes;
            }
            if(parameter.AccountingCostCenters != null && parameter.AccountingCostCenters.Count > 0) {
                journalEntryItem.CostCenters = parameter.AccountingCostCenters;
            }
            return journalEntryItem;
        }

        private JournalEntryListParametersDTO CreateJournalEntryItemforFixAccounting(GeneralLedgerServices.DTOs.JournalEntryItemDTO parameter)
        {
            JournalEntryListParametersDTO journalEntryItem = new JournalEntryListParametersDTO()
            {
                AccountingAccountId = parameter.AccountingAccount.AccountingAccountId,
                PaymentCode = parameter.SourceCode,
                AccountingNature = parameter.AccountingNature,
                BankReconciliationId = parameter.ReconciliationMovementType.Id,
                Amount = parameter.Amount.Value,
                LocalAmount = parameter.LocalAmount.Value,
                BranchCode = parameter.Branch.Id,
                CurrencyCode = parameter.Amount.Currency.Id,
                ExchangeRate = parameter.ExchangeRate.SellAmount,
                PayerId = parameter.Individual.IndividualId,
                RecieptDate = parameter.Receipt.Date,
                RecieptNumber = parameter.Receipt.Number
            };
            return journalEntryItem;
        }

        public int SaveCollectJournalEntry(string collectParamters)
        {
            #region Parameters
            JournalEntryParametersDTO journalEntryParametersDTO = new JournalEntryParametersDTO();
            journalEntryParametersDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<JournalEntryParametersDTO>(collectParamters);

            if (journalEntryParametersDTO.Description == null)
            {
                journalEntryParametersDTO.Description = Resources.Resources.GeneralLedgerCashIncomeDescription + " ";
            }

            int moduleId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingGeneralLedgerKey>(AccountingGeneralLedgerKey.ACC_MODULE_DATE_ACCOUNTING).ToString());

            ReconciliationMovementTypeDTO bankReconciliation = new ReconciliationMovementTypeDTO();
            ReceiptDTO receipt = new ReceiptDTO();

            if (journalEntryParametersDTO.TypeId == 2) //rechazo de cheques
            {
                journalEntryParametersDTO.Description = Resources.Resources.GeneralLedgerCheckRejectionDescription + " ";
                bankReconciliation.Id = Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingGeneralLedgerKey>(AccountingGeneralLedgerKey.ACC_BANK_RECONCILIATION_RETURNED_CHECK));
                receipt.Date = DelegateService.commonServiceCore.GetModuleDateIssue(Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingGeneralLedgerKey>(AccountingGeneralLedgerKey.ACC_MODULE_DATE_ACCOUNTING).ToString()), DateTime.Now);
                receipt.Number = journalEntryParametersDTO.BillId;
            }
            if (journalEntryParametersDTO.TypeId == 3) //canje de cheques
            {
                journalEntryParametersDTO.Description = Resources.Resources.GeneralLedgerCheckExchangeDescription + " ";
            }
            if (journalEntryParametersDTO.TypeId == 4) //RECHAZO DE TARJETA
            {
                journalEntryParametersDTO.Description = Resources.Resources.GeneralLedgerCardRejectionDescription + " ";
                bankReconciliation.Id = Convert.ToInt32(Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingGeneralLedgerKey>(AccountingGeneralLedgerKey.ACC_BANK_RECONCILIATION_RETURNED_CARD)));
                receipt.Date = DelegateService.commonServiceCore.GetModuleDateIssue(Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingGeneralLedgerKey>(AccountingGeneralLedgerKey.ACC_MODULE_DATE_ACCOUNTING).ToString()), DateTime.Now);
                receipt.Number = journalEntryParametersDTO.BillId;
            }
            if (journalEntryParametersDTO.TypeId == 5) //CANJE DE TARJETA
            {
                journalEntryParametersDTO.Description = Resources.Resources.GeneralLedgerCardExchangeDescription + " ";
            }

            //Listado en donde se llevaran los grupos de parametros al servicio
            List<List<ParameterDTO>> parametersCollection = new List<List<ParameterDTO>>();

            #endregion Parameters

            #region JournalEntryHeader

            //Se genera la cabecera del asiento.
            JournalEntryDTO journalEntry = new JournalEntryDTO();

            int accountingCompanyId = (from item in DelegateService.accountingApplicationService.GetAccountingCompanies() where item.Default select item).ToList()[0].AccountingCompanyId;

            journalEntry.AccountingCompany = new AccountingCompanyDTO { AccountingCompanyId = accountingCompanyId }; //este dato se graba con este valor en billing                
            journalEntry.AccountingDate = journalEntryParametersDTO.AccountingDate;
            journalEntry.AccountingMovementType = new AccountingMovementTypeDTO(); //este dato solo pertenece a asientos de mayor
            journalEntry.Branch = new BranchDTO() { Id = journalEntryParametersDTO.JournalEntryListParameters[0].BranchCode };
            journalEntry.Description = journalEntryParametersDTO.Description + Convert.ToString(journalEntryParametersDTO.TechnicalTransaction);
            journalEntry.EntryNumber = 0; //pendiente definición.
            journalEntry.Id = 0; //id autonumérico
            journalEntry.ModuleDateId = moduleId;
            journalEntry.RegisterDate = DateTime.Now;
            journalEntry.SalePoint = new SalePointDTO() { Id = 0 }; //no existe este dato en ingreso de caja
            journalEntry.Status = 1; //activo
            journalEntry.TechnicalTransaction = journalEntryParametersDTO.TechnicalTransaction;
            journalEntry.UserId = journalEntryParametersDTO.UserId;

            journalEntry.JournalEntryItems = new List<JournalEntryItemDTO>();

            #endregion JournalEntryHeader

            #region JournalEntryItem

            string description = journalEntryParametersDTO.Description + Convert.ToString(journalEntryParametersDTO.TechnicalTransaction);
            journalEntry.TechnicalTransaction = journalEntryParametersDTO.TechnicalTransaction;

            // Se arma los movimientos para los tipos de pago del recibo
            foreach (JournalEntryListParametersDTO accountingParameterDTO in journalEntryParametersDTO.JournalEntryListParameters)
            {
                // Cálculo de la cuenta contable y la naturaleza y se arma la estructura de parámetros para su evaluación.
                List<ParameterDTO> parameters = new List<ParameterDTO>();
                journalEntry.JournalEntryItems.Add(CreateJournalEntryItemforFixAccounting(accountingParameterDTO, description));               
                parameters = CreateParametersforCollectPayment(accountingParameterDTO);
                parametersCollection.Add(parameters);
            }
            AccountingParameterDTO accountingParametersDTO = new AccountingParameterDTO();

            accountingParametersDTO.JournalEntry = journalEntry;
            accountingParametersDTO.Parameters = parametersCollection;
            accountingParametersDTO.ModuleId = moduleId;
            accountingParametersDTO.BridgeAccounting = journalEntryParametersDTO.BridgeAccoutingId;

            string accountingJournalEntryParametersCollection = Newtonsoft.Json.JsonConvert.SerializeObject(accountingParametersDTO);
            
            if (journalEntryParametersDTO.Application == null)
            {
                return DelegateService.generalLedgerService.SaveBasicJournalEntry(accountingJournalEntryParametersCollection);
            }
            else
            {
                int entryNumber = DelegateService.generalLedgerService.AccountingWithoutTransactional(accountingJournalEntryParametersCollection);
                journalEntryParametersDTO.TypeId = moduleId;
                journalEntryParametersDTO.Description = Resources.Resources.ApplicationCollectDescription
                    + " " + journalEntryParametersDTO.TechnicalTransaction;
                return SaveJournalEntryforApplication(journalEntryParametersDTO, entryNumber);
            }
            #endregion
        }


        public int SaveChecksJournalEntry(string collectParamters)
        {
            #region Parameters
            JournalEntryParametersDTO journalEntryParametersDTO = new JournalEntryParametersDTO();
            journalEntryParametersDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<JournalEntryParametersDTO>(collectParamters);

            int moduleId = Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingGeneralLedgerKey>(AccountingGeneralLedgerKey.ACC_MODULE_DATE_ACCOUNTING).ToString());

            ReconciliationMovementTypeDTO bankReconciliation = new ReconciliationMovementTypeDTO();
            ReceiptDTO receipt = new ReceiptDTO();

            if (journalEntryParametersDTO.TypeId == 4) //rechazo de cheques
            {

                journalEntryParametersDTO.Description = Resources.Resources.GeneralLedgerCheckRejectionDescription + " ";
                bankReconciliation.Id = Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingGeneralLedgerKey>(AccountingGeneralLedgerKey.ACC_BANK_RECONCILIATION_RETURNED_CHECK));
                receipt.Date = DelegateService.commonServiceCore.GetModuleDateIssue(Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingGeneralLedgerKey>(AccountingGeneralLedgerKey.ACC_MODULE_DATE_ACCOUNTING).ToString()), DateTime.Now);
                receipt.Number = journalEntryParametersDTO.BillId;

                GeneralLedgerServices.DTOs.JournalEntryDTO journalEntryDTO = DelegateService.generalLedgerService.GetJournalEntryItemsByTechnicalTransaction(journalEntryParametersDTO.TechnicalTransaction);
                journalEntryParametersDTO.JournalEntryListParameters = new List<JournalEntryListParametersDTO>();
                if (journalEntryDTO != null && journalEntryDTO.Id > 0)
                {
                    foreach (GeneralLedgerServices.DTOs.JournalEntryItemDTO journalEntryItem in journalEntryDTO.JournalEntryItems.Where(x => x.SourceCode == journalEntryParametersDTO.PaymentCode))
                    {
                        journalEntryItem.AccountingNature = Convert.ToInt32(GeneralLedgerServices.Enums.AccountingNatures.Credit);
                        journalEntryItem.Branch.Id = journalEntryDTO.Branch.Id;
                        journalEntryItem.SourceCode = journalEntryParametersDTO.CollectPaymentCode;
                        journalEntryParametersDTO.JournalEntryListParameters.Add(CreateJournalEntryItemforFixAccounting(journalEntryItem));
                    }
                }
            }
            if (journalEntryParametersDTO.TypeId == 5) //Legalizacion de cheques
            {
                journalEntryParametersDTO.Description = Resources.Resources.GeneralLedgerCheckLegalizeDescription + " ";
                bankReconciliation.Id = Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingGeneralLedgerKey>(AccountingGeneralLedgerKey.ACC_BANK_RECONCILIATION_RETURNED_CHECK));
                receipt.Date = DelegateService.commonServiceCore.GetModuleDateIssue(Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingGeneralLedgerKey>(AccountingGeneralLedgerKey.ACC_MODULE_DATE_ACCOUNTING).ToString()), DateTime.Now);
                receipt.Number = journalEntryParametersDTO.BillId;

                List<GeneralLedgerServices.DTOs.JournalEntryItemDTO> journalEntryItemsDTO = DelegateService.generalLedgerService.GetJournalEntryItemsBySourceCode(journalEntryParametersDTO.PaymentCode);
                int branchCode = journalEntryParametersDTO.JournalEntryListParameters[0].BranchCode;
                journalEntryParametersDTO.JournalEntryListParameters = new List<JournalEntryListParametersDTO>();
                if (journalEntryItemsDTO != null && journalEntryItemsDTO.Count > 0)
                {
                    foreach (GeneralLedgerServices.DTOs.JournalEntryItemDTO journalEntryItem in journalEntryItemsDTO.Where(x => x.AccountingNature == (int)GeneralLedgerServices.Enums.AccountingNatures.Debit && x.SourceCode == journalEntryParametersDTO.PaymentCode && x.ReconciliationMovementType.Id == 0))
                    {
                        journalEntryItem.AccountingNature = journalEntryItem.AccountingNature == 1 ? (int)GeneralLedgerServices.Enums.AccountingNatures.Debit : (int)GeneralLedgerServices.Enums.AccountingNatures.Credit;
                        journalEntryItem.Branch.Id = branchCode;
                        journalEntryItem.SourceCode = journalEntryParametersDTO.CollectPaymentCode;
                        journalEntryParametersDTO.JournalEntryListParameters.Add(CreateJournalEntryItemforFixAccounting(journalEntryItem));
                        break;
                    }
                }

            }
            if (journalEntryParametersDTO.TypeId == 7)//canje De cheques
            {
                journalEntryParametersDTO.Description = Resources.Resources.GeneralLedgerCheckExchangeDescription + " ";
                receipt.Date = DelegateService.commonServiceCore.GetModuleDateIssue(Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingGeneralLedgerKey>(AccountingGeneralLedgerKey.ACC_MODULE_DATE_ACCOUNTING).ToString()), DateTime.Now);
                receipt.Number = journalEntryParametersDTO.BillId;

                GeneralLedgerServices.DTOs.JournalEntryDTO journalEntryDTO = DelegateService.generalLedgerService.GetJournalEntryItemsByTechnicalTransaction(journalEntryParametersDTO.TechnicalTransaction);
                journalEntryParametersDTO.JournalEntryListParameters = new List<JournalEntryListParametersDTO>();
                if (journalEntryDTO != null && journalEntryDTO.Id > 0)
                {
                    foreach (GeneralLedgerServices.DTOs.JournalEntryItemDTO journalEntryItem in journalEntryDTO.JournalEntryItems.Where(x => x.SourceCode == journalEntryParametersDTO.PaymentCode))
                    {
                        journalEntryItem.SourceCode = journalEntryParametersDTO.CollectPaymentCode;
                        journalEntryItem.AccountingNature = journalEntryItem.AccountingNature == 1 ? (int)GeneralLedgerServices.Enums.AccountingNatures.Debit : (int)GeneralLedgerServices.Enums.AccountingNatures.Credit;
                        journalEntryItem.Branch.Id = journalEntryDTO.Branch.Id;
                        journalEntryParametersDTO.JournalEntryListParameters.Add(CreateJournalEntryItemforFixAccounting(journalEntryItem));
                    }

                }
            }
            if (journalEntryParametersDTO.TypeId == 6)//regularizacion De cheques
            {
                journalEntryParametersDTO.Description = Resources.Resources.GeneralLedgerCheckRegularizedDescription + " ";
                receipt.Date = DelegateService.commonServiceCore.GetModuleDateIssue(Convert.ToInt32(EnumHelper.GetEnumParameterValue<AccountingGeneralLedgerKey>(AccountingGeneralLedgerKey.ACC_MODULE_DATE_ACCOUNTING).ToString()), DateTime.Now);
                receipt.Number = journalEntryParametersDTO.BillId;

                GeneralLedgerServices.DTOs.JournalEntryDTO journalEntryDTO = DelegateService.generalLedgerService.GetJournalEntryItemsByTechnicalTransaction(journalEntryParametersDTO.TechnicalTransaction);
                journalEntryParametersDTO.JournalEntryListParameters = new List<JournalEntryListParametersDTO>();
                if (journalEntryDTO != null && journalEntryDTO.Id > 0)
                {
                    foreach (GeneralLedgerServices.DTOs.JournalEntryItemDTO journalEntryItem in journalEntryDTO.JournalEntryItems.Where(x => x.SourceCode == journalEntryParametersDTO.PaymentCode))
                    {
                        journalEntryItem.SourceCode = journalEntryParametersDTO.CollectPaymentCode;
                        journalEntryItem.AccountingNature = journalEntryItem.AccountingNature == 1 ? (int)GeneralLedgerServices.Enums.AccountingNatures.Credit : (int)GeneralLedgerServices.Enums.AccountingNatures.Debit;
                        journalEntryItem.Branch.Id = journalEntryDTO.Branch.Id;
                        //journalEntryItem.Amount.Value = journalEntryItem.Amount.Value * -1;
                        //journalEntryItem.LocalAmount.Value = journalEntryItem.LocalAmount.Value * -1;
                        journalEntryParametersDTO.JournalEntryListParameters.Add(CreateJournalEntryItemforFixAccounting(journalEntryItem));
                    }

                }
            }

            //Listado en donde se llevaran los grupos de parametros al servicio
            List<List<ParameterDTO>> parametersCollection = new List<List<ParameterDTO>>();

            #endregion Parameters

            #region JournalEntryHeader

            //Se genera la cabecera del asiento.
            JournalEntryDTO journalEntry = new JournalEntryDTO();

            int accountingCompanyId = (from item in DelegateService.accountingApplicationService.GetAccountingCompanies() where item.Default select item).ToList()[0].AccountingCompanyId;

            journalEntry.AccountingCompany = new AccountingCompanyDTO { AccountingCompanyId = accountingCompanyId };
            journalEntry.AccountingDate = journalEntryParametersDTO.AccountingDate;
            journalEntry.AccountingMovementType = new AccountingMovementTypeDTO(); //este dato solo pertenece a asientos de mayor
            journalEntry.Branch = new BranchDTO() { Id = journalEntryParametersDTO.JournalEntryListParameters[0].BranchCode };
            journalEntry.Description = journalEntryParametersDTO.Description + Convert.ToString(journalEntryParametersDTO.CollectTechnicalTransaction);
            journalEntry.EntryNumber = 0; //pendiente definición.
            journalEntry.Id = 0; //id autonumérico
            journalEntry.ModuleDateId = moduleId;
            journalEntry.RegisterDate = DateTime.Now;
            journalEntry.SalePoint = new SalePointDTO() { Id = 0 }; //no existe este dato en ingreso de caja
            journalEntry.Status = 1; //activo
            journalEntry.TechnicalTransaction = journalEntryParametersDTO.CollectTechnicalTransaction;
            journalEntry.UserId = journalEntryParametersDTO.UserId;

            journalEntry.JournalEntryItems = new List<JournalEntryItemDTO>();

            #endregion JournalEntryHeader

            #region JournalEntryItem

            string description = journalEntryParametersDTO.Description + Convert.ToString(journalEntryParametersDTO.CollectTechnicalTransaction);
            journalEntry.TechnicalTransaction = journalEntryParametersDTO.CollectTechnicalTransaction;

            // Se arma los movimientos para los tipos de pago del recibo
            foreach (JournalEntryListParametersDTO accountingParameterDTO in journalEntryParametersDTO.JournalEntryListParameters)
            {
                // Cálculo de la cuenta contable y la naturaleza y se arma la estructura de parámetros para su evaluación.
                List<ParameterDTO> parameters = new List<ParameterDTO>();
                ParameterDTO parameter = new ParameterDTO();
                journalEntry.JournalEntryItems.Add(CreateJournalEntryItemforFixAccounting(accountingParameterDTO, description));
                if (journalEntryParametersDTO.TypeId == 6)//regularizacion De cheques
                {

                    parameters.Add(new ParameterDTO() { Value = Convert.ToString(EnumHelper.GetEnumParameterValue(AccountingGeneralLedgerKey.ACC_PARAM_PAYMENT_METHOD_CURRENTCHECK), CultureInfo.InvariantCulture) }); //tipo de pago Cheque
                    parameters.Add(new ParameterDTO() { Value = Convert.ToString(accountingParameterDTO.CurrencyCode, CultureInfo.InvariantCulture) }); //moneda
                    parameters.Add(new ParameterDTO() { Value = Convert.ToString(accountingParameterDTO.Amount, CultureInfo.InvariantCulture) }); //valor
                }                
                parametersCollection.Add(parameters);
            }
            AccountingParameterDTO accountingParametersDTO = new AccountingParameterDTO();

            accountingParametersDTO.JournalEntry = journalEntry;
            accountingParametersDTO.Parameters = parametersCollection;
            accountingParametersDTO.ModuleId = moduleId;
            accountingParametersDTO.TypeId = journalEntryParametersDTO.TypeId;
            accountingParametersDTO.BridgeAccounting = journalEntryParametersDTO.BridgeAccoutingId;
            accountingParametersDTO.CodeRulePackage = journalEntryParametersDTO.BridgePackageCode;
            accountingParametersDTO.SourceCode = journalEntryParametersDTO.PaymentCode;
            string accountingJournalEntryParametersCollection = Newtonsoft.Json.JsonConvert.SerializeObject(accountingParametersDTO);

            int entryNumber = DelegateService.generalLedgerService.AccountingChecking(accountingJournalEntryParametersCollection);
            return entryNumber;
            #endregion
        }

        public int SaveApplicationJournalEntry(string applicationParameters, int entryNumberTransaction)
        {
            JournalEntryParametersDTO journalEntryParametersDTO = new JournalEntryParametersDTO();
            journalEntryParametersDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<JournalEntryParametersDTO>(applicationParameters);

            return SaveJournalEntryforApplication(journalEntryParametersDTO, entryNumberTransaction);
        }

        public int SaveJournalEntry(string journalEntryParameters)
        {
            GenericJournalEntryDTO genericJournalEntry = new GenericJournalEntryDTO();
            genericJournalEntry = Newtonsoft.Json.JsonConvert.DeserializeObject<GenericJournalEntryDTO>(journalEntryParameters);
            
            JournalEntryDTO journalEntry = new JournalEntryDTO();
            journalEntry.AccountingDate = genericJournalEntry.AccountingDate;
            journalEntry.AccountingMovementType = new AccountingMovementTypeDTO(); //este dato solo pertenece a asientos de mayor
            journalEntry.Branch = new BranchDTO() { Id = genericJournalEntry.BranchId };
            journalEntry.Description = genericJournalEntry.Description + Convert.ToString(genericJournalEntry.TechnicalTransaction);
            journalEntry.Id = 0; //id autonumérico
            journalEntry.ModuleDateId = genericJournalEntry.ModuleId;
            journalEntry.RegisterDate = DateTime.Now;
            journalEntry.SalePoint = new SalePointDTO() { Id = genericJournalEntry.SalePointId };
            journalEntry.Status = 1; //activo
            journalEntry.TechnicalTransaction = genericJournalEntry.TechnicalTransaction;
            journalEntry.UserId = genericJournalEntry.UserId;
            journalEntry.JournalEntryItems = new List<JournalEntryItemDTO>();

            List<List<ParameterDTO>> parametersCollection = new List<List<ParameterDTO>>();
            journalEntry.JournalEntryItems = new List<JournalEntryItemDTO>();
            if (genericJournalEntry.JournalEntryItems != null &&
                genericJournalEntry.JournalEntryItems.Count > 0) {

                genericJournalEntry.JournalEntryItems.ForEach(item =>
                {
                    JournalEntryItemDTO journalEntryItem = CreateJournalEntryItemforFixAccounting(item, journalEntry.Description);
                    journalEntry.JournalEntryItems.Add(journalEntryItem);

                    if (genericJournalEntry.AccountingTypeId == Convert.ToInt32(AccountingType.Ballot))
                        parametersCollection.Add(CreateParametersforBallot(item, genericJournalEntry.BankId, genericJournalEntry.AccountingNumber));
                    else if (genericJournalEntry.AccountingTypeId != Convert.ToInt32(AccountingType.Ballot))
                        parametersCollection.Add(new List<ParameterDTO>());
                });
            }

            AccountingParameterDTO accountingParameters = new AccountingParameterDTO();
            accountingParameters.JournalEntry = journalEntry;
            accountingParameters.Parameters = parametersCollection;
            accountingParameters.ModuleId = genericJournalEntry.ModuleId;
            accountingParameters.AccountingAccountId = genericJournalEntry.AccountingAccountId;
            if (genericJournalEntry.BridgeAccountId > 0)
                accountingParameters.BridgeAccounting = Convert.ToInt32(genericJournalEntry.BridgeAccountId);
            if (genericJournalEntry.PackageRuleCode > 0)
                accountingParameters.CodeRulePackage = Convert.ToString(genericJournalEntry.PackageRuleCode);
            if (genericJournalEntry.OriginalGeneralLedger != null)
            {
                accountingParameters.OriginalGeneralLedger = genericJournalEntry.OriginalGeneralLedger;
                accountingParameters.CodeRulePackage = Convert.ToString(genericJournalEntry.OriginalGeneralLedger.CodePackageRule);
            }
            if (genericJournalEntry.RecieptNumber != null)
            {
                accountingParameters.Receipt = new ReceiptDTO()
                {
                    Number = Convert.ToInt32(genericJournalEntry.RecieptNumber),
                    Date = Convert.ToDateTime(genericJournalEntry.RecieptDate)
                };
            }
            if (genericJournalEntry.BankReconciliationId != null)
            {
                accountingParameters.ReconciliationMovementType = new ReconciliationMovementTypeDTO() { Id = genericJournalEntry.BankReconciliationId ?? 0 };
            }

            string parameters = Newtonsoft.Json.JsonConvert.SerializeObject(accountingParameters);
            return DelegateService.generalLedgerService.SaveBasicJournalEntry(parameters);
        }

        private List<ParameterDTO> CreateParametersforBallot(JournalEntryListParametersDTO parameter, int bankId, string accountNumber)
        {
            List<ParameterDTO> parameters = new List<ParameterDTO>();
            parameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); // Parámetro para uso de ingreso de caja
            parameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); // Parámetro para uso de ingreso de caja
            parameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); // Parámetro para uso de ingreso de caja
            parameters.Add(new ParameterDTO() { Value = Convert.ToString(parameter.CurrencyCode, CultureInfo.InvariantCulture) }); // Moneda
            parameters.Add(new ParameterDTO() { Value = Convert.ToString(parameter.PaymentMethodTypeCode, CultureInfo.InvariantCulture) }); // Tipo de pago
            parameters.Add(new ParameterDTO() { Value = Convert.ToString(bankId, CultureInfo.InvariantCulture) }); // Id del banco
            parameters.Add(new ParameterDTO() { Value = Convert.ToString(accountNumber, CultureInfo.InvariantCulture) }); // Número de la cuenta
            parameters.Add(new ParameterDTO() { Value = Convert.ToString(0, CultureInfo.InvariantCulture) }); // Valor total de la boleta
            if (parameter.PaymentMethodTypeCode == Convert.ToInt32(PaymentMethods.Cash))
            {
                parameters.Add(new ParameterDTO() { Value = Convert.ToString(parameter.Amount, CultureInfo.InvariantCulture) }); // Valor en efectivo
                parameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); // Valor de la comisión
                parameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); // Valor de los cheques / tarjetas
            }
            else
            {
                parameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); // Valor en efectivo
                parameters.Add(new ParameterDTO() { Value = Convert.ToString(-1, CultureInfo.InvariantCulture) }); // Valor de la comisión
                parameters.Add(new ParameterDTO() { Value = Convert.ToString(parameter.Amount, CultureInfo.InvariantCulture) }); // Valor de los cheques / tarjetas
            }

            return parameters;
        }

        public int ReverseJournalEntry(string parameters)
        {
            return DelegateService.generalLedgerService.ReverseBasicJournalEntry(parameters);
        }

    }
}