using Sistran.Co.Application.Data;
using Sistran.Core.Application.AccountingServices.Assemblers;
using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using Sistran.Core.Application.AccountingServices.EEProvider.DAOs;
using Sistran.Core.Application.AccountingServices.EEProvider.DAOs.Accounting;
using Sistran.Core.Application.AccountingServices.EEProvider.Models;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Collect;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Application;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
using Sistran.Core.Application.AccountingServices.Enums;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using UTILHELPER = Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.AccountingServices.EEProvider.DAOs.Accounting.Reversion;
using Sistran.Core.Integration.UndewritingIntegrationServices.DTOs;
using Sistran.Core.Application.AccountingServices.DTOs.Imputations;
using Sistran.Core.Application.AccountingServices.DTOs.Application;
using Sistran.Core.Application.AccountingServices.DTOs.AccountsPayables;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Payments;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Business
{
    public class TempApplicationBusiness
    {
        public List<ApplicationPremiumCommision> GetApplicationPremiumCommisionsByTempAppPremiumId(int tempAppPremiumId)
        {
            ApplicationPremiumCommisionDAO applicationPremiumCommisionDAO = new ApplicationPremiumCommisionDAO();
            return applicationPremiumCommisionDAO.GetTempApplicationPremiumCommissByTempAppId(tempAppPremiumId);
        }

        public Models.Imputations.TempApplication GetTempApplicationByTempApplicationId(int tempApplicationId)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplication.Properties.TempAppCode, tempApplicationId);

            List<ACCOUNTINGEN.TempApplication> entityTempApplications = DataFacadeManager.GetObjects(
                        typeof(ACCOUNTINGEN.TempApplication), criteriaBuilder.GetPredicate()).
                        Cast<ACCOUNTINGEN.TempApplication>().ToList();

            Models.Imputations.TempApplication application = null;

            if (entityTempApplications != null && entityTempApplications.Count > 0)
            {
                application = ModelAssembler.CreateTempApplication(entityTempApplications[0]);
            }
            return application;
        }

        public Message SaveApplicationGeneralLedger(int tmpAccountintId, int userId)
        {
            Message message = new Message()
            {
                Success = false,
                GeneralLedgerSuccess = false
            };
            int idToControl = 0;
            int origin = 0;
            TempApplication tempApplication = GetTempApplicationByTempApplicationId(tmpAccountintId);

            if (tempApplication != null)
            {
                tempApplication.TempApplicationPremiums = GetTempApplicationPremiumsByTempApplicationId(tmpAccountintId);
                tempApplication.TempApplicationAccountings = GetTempApplicationAccountingByTempApplicationId(tmpAccountintId);
                ApplicationPremiumTransaction tempApplicationPremiumRev = TempApplicationPremiumReversionDAO.GetTempApplicationPremiumByTempApplicationId(tempApplication.Id);

                int branchId = 0;
                int sourceCode = 0;
                int salePointId = 0;
                int applicationId = 0;
                int technicalTransaction = 0;

                bool saveJournalEntry = false;

                if (tempApplication.ModuleId == (int)ApplicationTypes.Collect)
                {
                    CollectBusiness collectBusiness = new CollectBusiness();
                    CollectControlBusiness collectControlBusiness = new CollectControlBusiness();
                    Collect collect = collectBusiness.GetCollectByCollectId(tempApplication.SourceId);
                    CollectControl collectControl = collectControlBusiness.GetCollectControlById(collect.CollectControlId);
                    DateTime accountingDate = DateTime.Now;
                    if (collectControl != null)
                    {
                        accountingDate = collectControl.AccountingDate;
                        if (collectControl.Branch != null)
                        {
                            branchId = collectControl.Branch.Id;
                        }
                    }
                    else if (collect != null)
                    {
                        accountingDate = collect.Date;
                        if (collect.Branch != null)
                        {
                            branchId = collect.Branch.Id;
                        }
                    }

                    if (collect != null)
                    {
                        if (collect.PaymentsTotal.Value == GetTotal(tempApplication, tempApplicationPremiumRev))
                        {
                            sourceCode = collect.Id;
                            technicalTransaction = collect.Transaction.Id;
                            tempApplication.IndividualId = collect.Payer.IndividualId;
                            applicationId = SaveTempApplicationToReal(tempApplication, userId, collect.Transaction.Id, accountingDate, branchId);
                            idToControl = applicationId;
                            origin = 2;
                            if (applicationId > 0)
                            {
                                DeleteTempApplication deleteTempApplication = new DeleteTempApplication()
                                {
                                    TempApplicationId = tmpAccountintId,
                                    ApplicationId = applicationId,
                                    ApplicationType = ApplicationTypes.Collect,
                                    SourceId = tempApplication.SourceId,
                                    UserId = userId
                                };

                                if (ConvertTempApplicationtoReal(deleteTempApplication, accountingDate))
                                    saveJournalEntry = true;
                            }
                            else
                                message.Info = Resources.Resources.ErrorSavingApplication;
                        }
                        else
                            message.Info = Resources.Resources.ErrorApplicationNotBalanced;
                    }
                    else
                        message.Info = Resources.Resources.ErrorCollectNotFound;
                }
                else if (tempApplication.ModuleId == (int)ApplicationTypes.JournalEntry)
                {

                    if (0 == GetTotal(tempApplication, tempApplicationPremiumRev))
                    {
                        TempJournalEntryDAO tmpJournalEntryDAO = new TempJournalEntryDAO();
                        JournalEntryDAO journalEntryDAO = new JournalEntryDAO();
                        JournalEntry journalEntry = tmpJournalEntryDAO.GetTempJournalEntry(tempApplication.SourceId);
                        int lastId = 0;
                        if (journalEntry != null)
                        {
                            lastId = journalEntry.Id;
                            journalEntry.Id = 0;
                            journalEntry = journalEntryDAO.SaveJournalEntry(journalEntry);
                            if (journalEntry != null)
                            {
                                tempApplication.SourceId = journalEntry.Id;
                                tmpJournalEntryDAO.DeleteTempJournalEntry(lastId);
                            }
                        }
                        else
                            journalEntry = journalEntryDAO.GetJournalEntryById(tempApplication.SourceId);

                        if (journalEntry != null)
                        {
                            sourceCode = journalEntry.Id;
                            branchId = journalEntry.Branch.Id;
                            tempApplication.IndividualId = journalEntry.Payer.IndividualId;
                            TechnicalTransactionBusiness technicalTransactionBusiness = new TechnicalTransactionBusiness();
                            technicalTransaction = technicalTransactionBusiness.GetTechnicalTransaction(journalEntry.Branch.Id);
                            idToControl = technicalTransaction;
                            origin = 3;
                            applicationId = SaveTempApplicationToReal(tempApplication, userId, technicalTransaction, journalEntry.AccountingDate, branchId);
                            if (applicationId > 0)
                            {
                                branchId = journalEntry.Branch.Id;
                                salePointId = journalEntry.SalePoint.Id;
                                DeleteTempApplication deleteTempApplication = new DeleteTempApplication()
                                {
                                    TempApplicationId = tmpAccountintId,
                                    ApplicationId = applicationId,
                                    ApplicationType = ApplicationTypes.JournalEntry,
                                    SourceId = (lastId > 0) ? lastId : tempApplication.SourceId,
                                    UserId = userId
                                };

                                if (ConvertTempApplicationtoReal(deleteTempApplication, journalEntry.AccountingDate))
                                    saveJournalEntry = true;
                            }
                            else
                                message.Info = Resources.Resources.ErrorSavingApplication;
                        }
                        else
                            message.Info = Resources.Resources.ErrorTemporalNotFound;
                    }
                    else
                        message.Info = Resources.Resources.ErrorApplicationNotBalanced;
                }
                else
                {
                    message.Info = Resources.Resources.ErrorModuleNotSupported;
                }

                // Saves journal Entry
                if (saveJournalEntry)
                {
                    message.Code = SaveApplicationJournalEntry(applicationId, branchId, salePointId, userId);
                    // Validacion de integración a 2G Recibos Acontreras
                    if (message.Code > 0)
                    {
                       AccountingAccountDAO accountingAccountDAO = new AccountingAccountDAO();
                        message.Code = accountingAccountDAO.ValidateApplicationJournalEntry(technicalTransaction, message.Code);
                    }


                    if (sourceCode > 0)
                        message.SourceCode = sourceCode;

                    if (message.Code > 0)
                    {
                        message.Code = technicalTransaction;
                        message.Success = true;
                        message.GeneralLedgerSuccess = true;
                        message.Info = Resources.Resources.IntegrationSuccessMessage + " " + applicationId;

                    }
                    else if (message.Code == 0)
                    {
                        message.Success = true;
                        message.Info = Resources.Resources.AccountingIntegrationUnbalanceEntry;
                    }
                    else if (message.Code == -1)
                    {
                        message.Info = Resources.Resources.ErrorApplicationNotFound;
                    }
                    else if (message.Code == -2)
                    {
                        message.Info = Resources.Resources.EntryRecordingError;
                    }
                }
            }
            else
            {
                message.Info = Resources.Resources.ErrorTemporalNotFound;
            }
            //TODO: SE GRABA TABLA DE CONTROL DESPUES DE QUE SE GUARDE LA DATA TECNICA Y CONTABLE(ESTA PUEDE NO QUEDAR GRABADA/ERROR DE ASIENTO)
            Models.Imputations.Application movementToControl = new Models.Imputations.Application()
            {
                Id = idToControl
            };
            Integration2GBusiness integration2GBusiness = new Integration2GBusiness();
            integration2GBusiness.Save(movementToControl.ToModelIntegration(origin));
            return message;
        }

        private int SaveApplicationJournalEntry(int applicationId, int brachId, int salePointId, int userId)
        {
            ApplicationBusiness applicationBusiness = new ApplicationBusiness();
            Models.Imputations.Application application = applicationBusiness.GetApplicationByApplicationId(applicationId);
            if (application != null)
            {
                DTOs.AccountingParametersDTO accountingParameters = new DTOs.AccountingParametersDTO();
                accountingParameters.AccountingDate = application.AccountingDate;
                accountingParameters.BranchId = brachId;
                accountingParameters.SalePointId = salePointId;
                accountingParameters.Application = application.ToDTO().ToLedgerDTO();
                accountingParameters.UserId = userId;
                accountingParameters.Application.BridgeAccountingId =
                    CommonBusiness.GetIntParameter(Enums.AccountingKeys.ACC_BRIDGE_APPLICATION);

                if (application.ModuleId == Convert.ToInt32(ApplicationTypes.Collect))
                    accountingParameters.Application.BridgeAccountingId =
                        CommonBusiness.GetIntParameter(Enums.AccountingKeys.ACC_BRIDGE_COLLECT);

                accountingParameters.ApplicationItems = new List<DTOs.Imputations.ApplicationJournalEntryDTO>();
                // Consultar los movimentos contables
                var premium = applicationBusiness.GetApplicationPremiumsByApplicationId(accountingParameters.Application.Id).ToDTOs();
                accountingParameters.ApplicationItems.AddRange(DelegateService.accountingApplicationService.GetApplicationAccountsByApplicationId(accountingParameters.Application.Id).ToJournalDTOs());

                accountingParameters.ApplicationItems.AddRange(premium.ToJournalDTOs(application.ModuleId));
                foreach (DTOs.Imputations.ApplicationPremiumDTO applicationPremiumDTO in premium)
                {
                    accountingParameters.ApplicationItems.AddRange(DelegateService.accountingApplicationService.
                        GetApplicationPremiumCommisionsByApplicationPremiumId(applicationPremiumDTO.Id).ToJournalDTOs(accountingParameters.Application.ModuleId));
                }
                string parameters = Newtonsoft.Json.JsonConvert.SerializeObject(accountingParameters);
                return DelegateService.accountingGeneralLedgerApplicationService.SaveApplicationJournalEntry(parameters);
            }
            return -1;
        }

        


        private int SaveTempApplicationToReal(TempApplication tempApplication, int userId, int transactionId, DateTime accountingDate, int branchId)
        {
            ApplicationDAO applicationDAO = new ApplicationDAO();
            Models.Imputations.Application application = new Models.Imputations.Application();

            application.RegisterDate = DateTime.Now;
            application.ModuleId = tempApplication.ModuleId;
            application.UserId = userId;
            application.AccountingDate = accountingDate;
            application.BranchId = branchId;
            application.Description = tempApplication.Description;
            application.IndividualId = tempApplication.IndividualId;
            // Graba la cabecera de imputación
            application = applicationDAO.SaveImputation(application, tempApplication.SourceId, transactionId);

            return application.Id;
        }

        private decimal GetTotal(TempApplication tempApplication, ApplicationPremiumTransaction tempApplicationPremiumRev = null)
        {
            decimal total = 0;

            total += tempApplication.TempApplicationPremiums.Sum(x => x.LocalAmount);
            total -= tempApplication.TempApplicationPremiums.Sum(x => x.DiscountedCommission);
            if (tempApplicationPremiumRev != null && tempApplicationPremiumRev.PremiumReceivableItems.Any())
            {
                //fgomez  total -= Math.Abs(tempApplicationPremiumRev.PremiumReceivableItems.Sum(x => x.Amount.Value));
                total = tempApplicationPremiumRev.PremiumReceivableItems.Sum(x => x.Amount.Value);
            }
            total += tempApplication.TempApplicationAccountings.Where(x => x.AccountingNature == (int)AccountingNature.Credit).Sum(x => x.LocalAmount.Value);
            total -= tempApplication.TempApplicationAccountings.Where(x => x.AccountingNature == (int)AccountingNature.Debit).Sum(x => x.LocalAmount.Value);

            return total;
        }

        public List<TempApplicationPremium> GetTempApplicationPremiumsByTempApplicationId(int tempApplicationId)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplicationPremium.Properties.TempAppCode, tempApplicationId);

            List<ACCOUNTINGEN.TempApplicationPremium> entityTempApplicationPremiums = DataFacadeManager.GetObjects(
                        typeof(ACCOUNTINGEN.TempApplicationPremium), criteriaBuilder.GetPredicate()).
                        Cast<ACCOUNTINGEN.TempApplicationPremium>().ToList();

            List<TempApplicationPremium> tempApplicationPremiums = ModelAssembler.CreateTemporalApplicationPremiums(entityTempApplicationPremiums);
            return tempApplicationPremiums;
        }

        public List<ApplicationAccounting> GetTempApplicationAccountingByTempApplicationId(int tempApplicationId)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplicationAccounting.Properties.TempAppCode, tempApplicationId);

            List<ACCOUNTINGEN.TempApplicationAccounting> entityTempApplicationAccounts = DataFacadeManager.GetObjects(
                        typeof(ACCOUNTINGEN.TempApplicationAccounting), criteriaBuilder.GetPredicate()).
                        Cast<ACCOUNTINGEN.TempApplicationAccounting>().ToList();

            return ModelAssembler.CreateTemporalApplicationAccountings(entityTempApplicationAccounts);
        }

        private bool ConvertTempApplicationtoReal(DeleteTempApplication deleteTempApplication, DateTime accountingDate)
        {
            IncomeBusiness incomeBusiness = new IncomeBusiness();
            bool convert = incomeBusiness.ConvertTempApplicationtoRealApplication(deleteTempApplication.SourceId,
                Convert.ToInt32(deleteTempApplication.ApplicationType),
                deleteTempApplication.ApplicationId, accountingDate,
                deleteTempApplication.UserId);

            if (convert)
            {
                //Elimina Temporales
                incomeBusiness.DeleteTempApplicationByTempAplicationIdModuleId(deleteTempApplication.TempApplicationId,
                    Convert.ToInt32(deleteTempApplication.ApplicationType));

                // Actualiza estatus en Solicitud de pago a pagado
                incomeBusiness.UpdatePaymentRequestToStatusPayed(deleteTempApplication.ApplicationId,
                    deleteTempApplication.UserId, DateTime.Now);

                if (deleteTempApplication.ApplicationType == ApplicationTypes.Collect)
                {
                    Collect collect = new Collect();
                    // ACTUALIZA ESTADO DE BILL
                    collect.Id = deleteTempApplication.SourceId;
                    collect.Date = DelegateService.accountingParameterService.GetAccountingDate(Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<Enums.AccountingKeys>(Enums.AccountingKeys.ACC_MODULE_DATE_ACCOUNTING))); ;
                    collect.UserId = deleteTempApplication.UserId;
                    collect.Status = Convert.ToInt32(CollectStatus.Applied);

                    CollectDAO collectDAO = new CollectDAO();
                    collectDAO.UpdateCollect(collect, -1);
                }
            }
            return convert;
        }

        public List<TempApplicationPremiumComponent> UpdTempApplicationPremiumComponents(UpdTempApplicationPremiumComponent updTempApplicationPremiumComponentDTO)
        {
            ApplicationBusiness applicationBusiness = new ApplicationBusiness();
            IntegrationBusiness integrationBusiness = new IntegrationBusiness();
            TempApplicationPremiumDAO tempApplicationPremiumDAO = new TempApplicationPremiumDAO();
            TempApplicationPremiumComponentDAO tempApplicationPremiumComponentDAO = new TempApplicationPremiumComponentDAO();

            int decimalPlaces = 2;
            var components = tempApplicationPremiumComponentDAO.GetTempApplicationPremiumComponentsByTemAppPremium(updTempApplicationPremiumComponentDTO.TempApplicationPremiumCode).ToDTOs();
            var tempAppPremium = tempApplicationPremiumDAO.GetTempApplicationPremiumByTempApplicationPremiumId(updTempApplicationPremiumComponentDTO.TempApplicationPremiumCode);
            var payerPayments = applicationBusiness.GetPayerPaymentComponentsByEndorsementIdQuotaNumber(tempAppPremium.EndorsementId, tempAppPremium.PaymentNumber);

            decimal ApplicationExpensesAmount = 0;
            decimal participation = integrationBusiness.GetParticipationPercentageByEndorsementId(tempAppPremium.EndorsementId) / 100;
            var exchangeRate = updTempApplicationPremiumComponentDTO.ExchangeRate;
            var originalExchangeRate = exchangeRate;

            decimal OriginalTaxAmount = payerPayments.Where(x => x.TinyDescription == "I").Sum(x => x.Amount);
            var defObject = payerPayments.Where(x => x.TinyDescription == "I" || x.TinyDescription == "G").FirstOrDefault();

            if (defObject != null)
                originalExchangeRate = defObject.ExchangeRate;

            decimal ApplicationTaxesAmount = Math.Round(updTempApplicationPremiumComponentDTO.TaxLocalAmount / originalExchangeRate, decimalPlaces);
            if (OriginalTaxAmount >= 0)
            {
                if (ApplicationTaxesAmount > OriginalTaxAmount)
                {
                    ApplicationTaxesAmount = OriginalTaxAmount;
                    updTempApplicationPremiumComponentDTO.TaxLocalAmount = Math.Round(ApplicationTaxesAmount * originalExchangeRate, decimalPlaces);
                }
            }
            else
            {
                if (ApplicationTaxesAmount < OriginalTaxAmount)
                {
                    ApplicationTaxesAmount = OriginalTaxAmount;
                    updTempApplicationPremiumComponentDTO.TaxLocalAmount = Math.Round(ApplicationTaxesAmount * originalExchangeRate, decimalPlaces);
                }
            }

            if (updTempApplicationPremiumComponentDTO.ExpensesLocalAmount != 0)
            {
                updTempApplicationPremiumComponentDTO.ExpensesLocalAmount = payerPayments.Where(x => x.TinyDescription == "G").Sum(x => x.LocalAmount);
                ApplicationExpensesAmount = Math.Round(updTempApplicationPremiumComponentDTO.ExpensesLocalAmount / originalExchangeRate, decimalPlaces);
            }

            var ApplicationPremiumAmount = updTempApplicationPremiumComponentDTO.PremiumAmount;
            var ApplicationPremiumLocalAmount = Math.Round(updTempApplicationPremiumComponentDTO.PremiumAmount * exchangeRate, decimalPlaces);
            var ApplicationPremiumMainAmount = Math.Round(ApplicationPremiumAmount * participation, decimalPlaces);
            var ApplicationPremiumMainLocalAmount = Math.Round(ApplicationPremiumLocalAmount * participation, decimalPlaces);

            var ApplicationTotalAmount = ApplicationTaxesAmount + ApplicationExpensesAmount + ApplicationPremiumAmount;
            var ApplicationTotalLocalAmount = ApplicationPremiumLocalAmount + updTempApplicationPremiumComponentDTO.TaxLocalAmount
                + updTempApplicationPremiumComponentDTO.ExpensesLocalAmount;
            var ApplicationTotalMainAmount = ApplicationTaxesAmount + ApplicationExpensesAmount + ApplicationPremiumMainAmount;
            var ApplicationTotalMainLocalAmount = updTempApplicationPremiumComponentDTO.TaxLocalAmount
                + updTempApplicationPremiumComponentDTO.ExpensesLocalAmount + ApplicationPremiumMainLocalAmount;

            var tempApplicationPremium = new TempApplicationPremium()
            {
                Id = updTempApplicationPremiumComponentDTO.TempApplicationPremiumCode,
                ExchangeRate = exchangeRate,
                Amount = ApplicationTotalAmount,
                LocalAmount = ApplicationTotalLocalAmount,
                MainAmount = ApplicationTotalMainAmount,
                MainLocalAmount = ApplicationTotalMainLocalAmount
            };

            tempApplicationPremiumDAO.UpdateTempApplicationPremiumAmounts(tempApplicationPremium);

            components.ForEach(item =>
            {
                switch (item.ComponentTinyDescription)
                {
                    case "I":
                        item.Amount = ApplicationTaxesAmount;
                        item.LocalAmount = updTempApplicationPremiumComponentDTO.TaxLocalAmount;
                        item.ExchangeRate = originalExchangeRate;
                        item.MainAmount = item.Amount;
                        item.MainLocalAmount = item.LocalAmount;
                        break;
                    case "G":
                        item.Amount = ApplicationExpensesAmount;
                        item.LocalAmount = updTempApplicationPremiumComponentDTO.ExpensesLocalAmount;
                        item.ExchangeRate = originalExchangeRate;
                        item.MainAmount = item.Amount;
                        item.MainLocalAmount = item.LocalAmount;
                        break;
                    case "P":
                        item.Amount = ApplicationPremiumAmount;
                        item.LocalAmount = ApplicationPremiumLocalAmount;
                        item.ExchangeRate = exchangeRate;
                        item.MainAmount = ApplicationPremiumMainAmount;
                        item.MainLocalAmount = ApplicationPremiumMainLocalAmount;
                        break;
                }
                tempApplicationPremiumComponentDAO.UpdateTempApplicationPremiumComponent(item.ToModel());
            });
            return components.ToModels();
        }

        public bool SaveTempApplicationPremiumComponents(TempApplicationPremium tempApplicationPremium, decimal taxLocalAmount, bool noExpenses)
        {
            if (tempApplicationPremium.Id == 0)
            {
                int decimalPlaces = 2;
                ApplicationBusiness applicationBusiness = new ApplicationBusiness();
                IntegrationBusiness integrationBusiness = new IntegrationBusiness();
                TempApplicationPremiumItemDAO tempApplicationPremiumItemDAO = new TempApplicationPremiumItemDAO();
                ApplicationPremiumCommisionDAO applicationPremiumCommisionDAO = new ApplicationPremiumCommisionDAO();
                TempApplicationPremiumComponentDAO tempApplicationPremiumComponentDAO = new TempApplicationPremiumComponentDAO();

                decimal participation = integrationBusiness.GetParticipationPercentageByEndorsementId(tempApplicationPremium.EndorsementId) / 100;

                //se consulta los componentes de la poliza que se va a agregar
                var payerPaymentComponentDTOs = applicationBusiness.GetPayerPaymentComponentsByEndorsementIdQuotaNumber(
                    tempApplicationPremium.EndorsementId, tempApplicationPremium.PaymentNumber)
                    .Where(x => x.TinyDescription == "P" || x.TinyDescription == "I" || x.TinyDescription == "G").ToList();

                var exchangeRate = DelegateService.commonService.GetExchangeRateByCurrencyId(tempApplicationPremium.Currencyid).SellAmount;

                if (DelegateService.commonService.CalculateExchangeRateTolerance(tempApplicationPremium.ExchangeRate, tempApplicationPremium.Currencyid))
                {
                    exchangeRate = tempApplicationPremium.ExchangeRate;
                }

                var defaultComp = payerPaymentComponentDTOs.Where(x => x.TinyDescription == "I" || x.TinyDescription == "G").FirstOrDefault();
                var originalExchangeRate = exchangeRate;
                if (defaultComp != null)
                    originalExchangeRate = defaultComp.ExchangeRate;

                var originalPremium = payerPaymentComponentDTOs.Where(x => x.TinyDescription == "P").Sum(x => x.LocalAmount);
                var originalTax = payerPaymentComponentDTOs.Where(x => x.TinyDescription == "I").Sum(x => x.LocalAmount);
                decimal expensesLocalAmount = 0;
                if (!noExpenses)
                {
                    expensesLocalAmount = payerPaymentComponentDTOs.Where(x => x.TinyDescription == "G").Sum(x => x.LocalAmount);
                }
                if (originalTax >= 0)
                    taxLocalAmount = (taxLocalAmount > originalTax) ? originalTax : taxLocalAmount;
                else
                    taxLocalAmount = (taxLocalAmount < originalTax) ? originalTax : taxLocalAmount;

                var premiumLocalAmount = tempApplicationPremium.LocalAmount;
                var premiumAmount = Math.Round(premiumLocalAmount / exchangeRate, decimalPlaces);
                var premiumMainAmount = Math.Round(premiumAmount * participation, decimalPlaces);
                var premiumLocalMainAmount = Math.Round(premiumLocalAmount * participation, decimalPlaces);

                var taxAmount = Math.Round(taxLocalAmount / originalExchangeRate, decimalPlaces);
                var expensesAmount = Math.Round(expensesLocalAmount / originalExchangeRate, decimalPlaces);

                tempApplicationPremium.Amount = premiumAmount + taxAmount + expensesAmount;
                tempApplicationPremium.LocalAmount = premiumLocalAmount + taxLocalAmount + expensesLocalAmount;
                tempApplicationPremium.MainAmount = premiumMainAmount + taxAmount + expensesAmount;
                tempApplicationPremium.MainLocalAmount = premiumLocalMainAmount + taxLocalAmount + expensesLocalAmount;
                tempApplicationPremium.ExchangeRate = exchangeRate;
                tempApplicationPremium.AccountingDate = tempApplicationPremium.AccountingDate.Date;
                tempApplicationPremium.DiscountedCommission = tempApplicationPremium.Commissions == null ? 0 : Math.Round(tempApplicationPremium.Commissions.Sum(x => x.LocalAmount), decimalPlaces);

                var savedTempApplicationPremium = tempApplicationPremiumItemDAO.SaveTempApplicationPremium(tempApplicationPremium);

                if (tempApplicationPremium.Commissions != null)
                {
                    tempApplicationPremium.Commissions.ForEach(x =>
                    {
                        x.ApplicationPremiumId = savedTempApplicationPremium.Id;
                        applicationPremiumCommisionDAO.CreateTempApplicationPremiumCommisses(x);
                    });
                }

                decimal amount, localAmount, mainAmount, mainLocalAmount, exchange;

                foreach (Sistran.Core.Integration.UndewritingIntegrationServices.DTOs.PayerPaymentComponentDTO payerPaymentComponentDTO in payerPaymentComponentDTOs)
                {
                    if (payerPaymentComponentDTO.TinyDescription == "G"
                        || payerPaymentComponentDTO.TinyDescription == "I")
                    {
                        exchange = payerPaymentComponentDTO.ExchangeRate;

                        if (payerPaymentComponentDTO.TinyDescription == "G")
                        {
                            amount = expensesAmount;
                            localAmount = expensesLocalAmount;
                        }
                        else
                        {
                            amount = taxAmount;
                            localAmount = taxLocalAmount;
                        }
                        mainAmount = amount;
                        mainLocalAmount = localAmount;
                    }
                    else
                    {
                        localAmount = premiumLocalAmount;
                        exchange = exchangeRate;
                        amount = premiumAmount;
                        mainAmount = premiumMainAmount;
                        mainLocalAmount = premiumLocalMainAmount;
                    }

                    var tempApplicationPremiumComponent = new TempApplicationPremiumComponent
                    {
                        TempApplicationPremiumCode = savedTempApplicationPremium.Id,
                        ComponentCode = payerPaymentComponentDTO.ComponentId,
                        ComponentCurrencyCode = savedTempApplicationPremium.Currencyid,
                        ExchangeRate = exchange,
                        Amount = amount,
                        LocalAmount = localAmount,
                        MainAmount = mainAmount,
                        MainLocalAmount = mainLocalAmount
                    };
                    tempApplicationPremiumComponentDAO.SaveTempApplicationPremiumComponent(tempApplicationPremiumComponent);
                }
                return true;
            }
            return false;
        }

        public List<PremiumSearchPolicyDTO> GetTempApplicationsPremiumByTempApplicationId(int tempApplicationId)
        {
            SearchPolicyPaymentDTO searchPolicyPayment = new SearchPolicyPaymentDTO()
            {
                BranchId = "",
                PrefixId = "",
                InsuredId = "",
                AgentId = "",
                GroupId = "",
                PolicyId = "",
                PolicyDocumentNumber = "",
                SalesTicket = "",
                EndorsementId = "",
                DateFrom = "",
                DateTo = "",
                PageSize = "",
                PageIndex = "",
                PayerId = "",
                InsuredDocumentNumber = "",
                EndorsementDocumentNumber = ""
            };
            List<TempApplicationPremium> appplicationPremiums = GetTempApplicationPremiumByTempApplicationId(tempApplicationId);
            List<PremiumSearchPolicyDTO> premiumsResult = new List<PremiumSearchPolicyDTO>();
            List<PremiumSearchPolicyDTO> premiums = new List<PremiumSearchPolicyDTO>();
            appplicationPremiums.ForEach(item =>
            {
                searchPolicyPayment.EndorsementId = item.EndorsementId.ToString();
                premiums = DelegateService.underwritingIntegrationService.GetPremiuPaymSearchPolicies(searchPolicyPayment);
                if (premiums != null && premiums.Count > 0)
                {
                    premiums.ForEach(x =>
                    {
                        var appPremiums = appplicationPremiums.Where(y => y.EndorsementId == x.EndorsementId && x.PaymentNumber == y.PaymentNumber);
                        x.PaidAmount = appPremiums.Sum(z => z.Amount);
                        x.Tax = appPremiums.Sum(z => z.Tax);
                        x.DiscountedCommission = appPremiums.Sum(z => z.DiscountedCommission);
                        premiumsResult.Add(x);
                    });
                }
            });

            return premiumsResult;
        }
        private List<ApplicationPremium> GetTempApplicationPremiums(List<PremiumSearchPolicyDTO> premiums)
        {
            TempApplicationPremiumItemDAO applicationPremiumItemDAO = new TempApplicationPremiumItemDAO();
            return applicationPremiumItemDAO.GetTempApplicationsByFilter(premiums);
        }

        private List<TempApplicationPremium> GetTempApplicationPremiumByTempApplicationId(int tempApplicationId)
        {
            TempApplicationPremiumItemDAO tempApplicationPremiumItemDAO = new TempApplicationPremiumItemDAO();
            TempApplicationPremiumComponentDAO tempApplicationPremiumComponentDAO = new TempApplicationPremiumComponentDAO();
            List<TempApplicationPremium> tempAppPremiums = tempApplicationPremiumItemDAO.GetTempApplicationPremiumsByTempApplicationId(tempApplicationId);
            tempAppPremiums.ForEach(item =>
            {
                tempApplicationPremiumComponentDAO.GetTempApplicationPremiumComponentsByTemAppPremium(item.Id).ToDTOs().ForEach(itemComp =>
                {
                    switch (itemComp.ComponentTinyDescription)
                    {
                        case "I":
                            item.Tax = itemComp.LocalAmount;
                            break;
                    }
                });
            });

            return tempAppPremiums;
        }

        public string GetJournalEntryItemDescription(int endorsementId, int quota)
        {
            try
            {
                var policy = DelegateService.underwritingIntegrationService.GetPaymentQuotaDescription(new FilterBaseDTO { Id = endorsementId, Quota = quota });
                if (policy != null)
                {
                    return String.Format(Resources.Resources.PaymentDescription, policy.PrefixDescription, policy.DocumentNumber, policy.EndorsementDocumentNum);
                }
            }
            catch (Exception)
            {

            }
            return "";
        }

        public bool UpdateTempApplicationIndividualId(int tempApplicationId, int individualId)
        {
            TempApplicationDAO tempApplicationDAO = new TempApplicationDAO();
            return tempApplicationDAO.UpdateTempApplicationIndividualId(tempApplicationId, individualId);
        }

        public int GetTempApplicationBySourceId(int sourceId, int moduleId)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplication.Properties.SourceCode, sourceId);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.TempApplication.Properties.ModuleCode, moduleId);

            SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCOUNTINGEN.TempApplication.Properties.TempAppCode, typeof(ACCOUNTINGEN.TempApplication).Name), ACCOUNTINGEN.TempApplication.Properties.TempAppCode));
            selectQuery.Table = new ClassNameTable(typeof(ACCOUNTINGEN.TempApplication), typeof(ACCOUNTINGEN.TempApplication).Name);
            selectQuery.Where = criteriaBuilder.GetPredicate();

            int tempAppCode = 0;
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    tempAppCode = Convert.ToInt32(reader[ACCOUNTINGEN.TempApplication.Properties.TempAppCode].ToString());
                }
            }
            return tempAppCode;
        }

        /// <summary>
        /// saveTempApplication
        /// </summary>
        /// <param name="applicationRequest"></param>
        /// <returns></returns>
        public PaymentOrderDTO SaveTempApplication(PremiumRequestDTO applicationRequest)
        {
            if (applicationRequest == null)
            {
                return null;
            }
            PaymentOrderDTO application = new PaymentOrderDTO();
            application.Imputation = new ApplicationDTO();
            ApplicationBusiness applicationBusiness = new ApplicationBusiness();

            application.Imputation.Id = 0;
            application.Imputation.RegisterDate = DateTime.Now;
            application.Imputation.ModuleId = (int)ApplicationTypes.Collect;
            application.Imputation.IndividualId = -1;
            application.Imputation.UserId = applicationRequest.UserId;
            application.Imputation.AccountingDate = applicationRequest.AccountingDate;
            application.Imputation = DelegateService.accountingApplicationService.SaveTempApplication(application.Imputation, 0);

            PremiumReceivableTransactionDTO premiumRecievableTransaction = new PremiumReceivableTransactionDTO();
            premiumRecievableTransaction.PremiumReceivableItems = new List<PremiumReceivableTransactionItemDTO>();

            var isSave = SavetmpApplicationPremium(new PremiumRequestDTO { PremiumReceivableTransaction = applicationRequest.PremiumReceivableTransaction, ApplicationId = application.Imputation.Id, UserId = applicationRequest.UserId, RegisterDate = DateTime.Now, AccountingDate = applicationRequest.AccountingDate, ExchangeRate = applicationRequest.ExchangeRate });
            if (isSave.Success == true)
            {
                return application;
            }
            else
            {
                application.Imputation = null;
                application.Observation = isSave.Info;
                return application;
            }
        }

        public Message SavetmpApplicationPremium(PremiumRequestDTO premiumRequestDTO)
        {

            TempApplicationPremiumItemDAO tempApplicationPremiumItemDAO = new TempApplicationPremiumItemDAO();
            TempApplicationPremiumComponentDAO tempApplicationPremiumComponentDAO = new TempApplicationPremiumComponentDAO();
            Message recorded = new Message() { Success = false, Info = "" };
            if (premiumRequestDTO.PremiumReceivableTransaction != null && premiumRequestDTO.PremiumReceivableTransaction.PremiumReceivableItems.Any())
            {
                try
                {
                    PremiumReceivableTransactionItemDTO tempApplicationPremium = null;
                    ApplicationBusiness applicationBusiness = new ApplicationBusiness();
                    IntegrationBusiness integrationBusiness = new IntegrationBusiness();

                    TempApplicationPremiumDAO tempApplicationPremiumDAO = new TempApplicationPremiumDAO();
                    for (int i = 0; i < premiumRequestDTO.PremiumReceivableTransaction.PremiumReceivableItems.Count; i++)
                    {
                        PremiumReceivableTransactionItem modelPremium = premiumRequestDTO.PremiumReceivableTransaction.PremiumReceivableItems[i].ToModel();
                        if (modelPremium.DeductCommission != null)
                        {
                            modelPremium.DeductCommission.Value = premiumRequestDTO.PremiumReceivableTransaction.CommissionsDiscounted?.Sum(x => x.LocalAmount) ?? decimal.Zero;
                        }
                        int decimalPlaces = 2;
                        if (modelPremium.Id == 0)
                        {
                            tempApplicationPremium = tempApplicationPremiumItemDAO.SaveTempPremiumRecievableTransactionItem(modelPremium, premiumRequestDTO.ApplicationId, premiumRequestDTO.ExchangeRate, premiumRequestDTO.UserId, premiumRequestDTO.RegisterDate, premiumRequestDTO.AccountingDate).ToDTO();
                            
                            var exchangeRate = modelPremium.Policy.ExchangeRate.SellAmount;
                            var payerPaymentComponentDTOs = applicationBusiness.GetPayerPaymentComponentsByEndorsementIdQuotaNumber(modelPremium.Policy.Endorsement.Id,
                                modelPremium.Policy.PaymentPlan.Quotas[0].Number);

                            payerPaymentComponentDTOs = payerPaymentComponentDTOs.Where(x => x.TinyDescription == ComponentTypes.I.ToString()
                                            || x.TinyDescription == ComponentTypes.P.ToString() || x.TinyDescription == ComponentTypes.G.ToString()).ToList();
                            var incomeLocalAmount = modelPremium.Policy.PayerComponents.FirstOrDefault().Amount;//saldo a pagar monto ingresado
                            var totalPremium = Math.Round(payerPaymentComponentDTOs.Where(x => x.TinyDescription == ComponentTypes.G.ToString()
                                            || x.TinyDescription == ComponentTypes.I.ToString()).Sum(x => x.LocalAmount) + (payerPaymentComponentDTOs.Where(x => x.TinyDescription == ComponentTypes.P.ToString()).Sum(x=>x.Amount) * exchangeRate), decimalPlaces);
                            if (incomeLocalAmount > totalPremium)//si monto ingresado mayor a suma de componentes
                            {
                                recorded.Success = false;
                                recorded.Info = Resources.Resources.ValueToCollectGreater;
                                return recorded;
                            }

                            decimal participation = integrationBusiness.GetParticipationPercentageByEndorsementId(modelPremium.Policy.Endorsement.Id);

                            var defaultValue = payerPaymentComponentDTOs.FirstOrDefault();
                            var originalExchangeRate = (defaultValue != null) ? defaultValue.ExchangeRate : exchangeRate;

                            decimal OriginalTaxAmount = Math.Round(payerPaymentComponentDTOs.Where(x => x.TinyDescription == ComponentTypes.I.ToString()).Sum(x => x.Amount), decimalPlaces);
                            decimal OriginalExpensesAmount = Math.Round(payerPaymentComponentDTOs.Where(x => x.TinyDescription == ComponentTypes.G.ToString()).Sum(x => x.Amount), decimalPlaces);

                            decimal OriginalTaxLocalAmount = Math.Round(payerPaymentComponentDTOs.Where(x => x.TinyDescription == ComponentTypes.I.ToString()).Sum(x => x.LocalAmount), decimalPlaces);
                            decimal OriginalExpensesLocalAmount = Math.Round(payerPaymentComponentDTOs.Where(x => x.TinyDescription == ComponentTypes.G.ToString()).Sum(x => x.LocalAmount), decimalPlaces);
                            decimal OriginalPremiumLocalAmount = Math.Round(payerPaymentComponentDTOs.Where(x => x.TinyDescription == ComponentTypes.P.ToString()).Sum(x => x.Amount) * exchangeRate, decimalPlaces);

                            decimal taxLocalAmount = 0;
                            decimal expensesLocalAmount = 0;
                            decimal premiumLocalAmount = 0;


                            if (OriginalTaxLocalAmount < incomeLocalAmount)
                            {
                                incomeLocalAmount = incomeLocalAmount - OriginalTaxLocalAmount;
                                taxLocalAmount = OriginalTaxLocalAmount;
                            }
                            else
                            {
                                taxLocalAmount = incomeLocalAmount;
                                incomeLocalAmount = 0;
                            }


                            if (OriginalExpensesLocalAmount < incomeLocalAmount)
                            {
                                incomeLocalAmount = incomeLocalAmount - OriginalExpensesLocalAmount;
                                expensesLocalAmount = OriginalExpensesLocalAmount;
                            }
                            else
                            {
                                expensesLocalAmount = incomeLocalAmount;
                                incomeLocalAmount = 0;
                            }

                            if (incomeLocalAmount > 0)
                            {
                                if (OriginalPremiumLocalAmount < incomeLocalAmount)
                                {
                                    premiumLocalAmount = OriginalPremiumLocalAmount;
                                    incomeLocalAmount = incomeLocalAmount - OriginalPremiumLocalAmount;
                                }
                                else
                                {
                                    premiumLocalAmount = incomeLocalAmount;
                                    incomeLocalAmount = 0;
                                }
                            }

                            payerPaymentComponentDTOs.ForEach(item =>
                            {
                                switch (item.TinyDescription)
                                {
                                    case "I":
                                        item.LocalAmount = taxLocalAmount;
                                        item.ExchangeRate = originalExchangeRate;
                                        break;
                                    case "G":
                                        item.LocalAmount = expensesLocalAmount;
                                        item.ExchangeRate = originalExchangeRate;
                                        break;
                                    case "P":
                                        item.LocalAmount = premiumLocalAmount;
                                        item.ExchangeRate = exchangeRate;
                                        break;
                                }
                            });

                            PaymentComponentModel PaymentComponentModel = new PaymentComponentModel { AppId = tempApplicationPremium.Id, CurrencyId = tempApplicationPremium.Policy.ExchangeRate.Currency.Id, ExchangeRate = exchangeRate, payerPaymentComponentDTOs = payerPaymentComponentDTOs, PercentageCoinsurance = participation };
                            List<TempApplicationPremiumComponent> tempApplicationPremiumComponents = ApplicationPremiumBusiness.CreatePremiumComponentForLocalAmount(PaymentComponentModel);
                            foreach (TempApplicationPremiumComponent tempApplicationPremiumComponent in tempApplicationPremiumComponents)
                            {
                                tempApplicationPremiumComponentDAO.SaveTempApplicationPremiumComponent(tempApplicationPremiumComponent);
                            }
                            
                            TempApplicationPremium tempApplicationUpd = new TempApplicationPremium()
                            {
                                Amount = Math.Round(tempApplicationPremiumComponents.Sum(x => Math.Round(x.Amount, decimalPlaces)), decimalPlaces),
                                LocalAmount = Math.Round(tempApplicationPremiumComponents.Sum(x => Math.Round(x.LocalAmount, decimalPlaces)), decimalPlaces),
                                MainLocalAmount = Math.Round(tempApplicationPremiumComponents.Sum(x => Math.Round(x.MainLocalAmount, decimalPlaces)), decimalPlaces),
                                MainAmount = Math.Round(tempApplicationPremiumComponents.Sum(x => Math.Round(x.MainAmount, decimalPlaces)), decimalPlaces),
                                ExchangeRate = exchangeRate,
                                Id = tempApplicationPremium.Id
                            };
                            tempApplicationPremiumDAO.UpdateTempApplicationPremiumAmounts(tempApplicationUpd);
                            
                            recorded.Success = true;
                        }
                    }
                    return recorded;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return recorded;
        }
        
        public bool SaveLogMassiveDataPolicy(LogMassiveDataPolicyDTO logMassiveDataPolicy)
        {
            try
            {
                ApplicationDAO applicationDAO = new ApplicationDAO();
                return applicationDAO.SaveLogMassiveDataPolicy(logMassiveDataPolicy.ToModel());
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
