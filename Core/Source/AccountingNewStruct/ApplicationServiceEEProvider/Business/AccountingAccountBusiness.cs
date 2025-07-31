using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using Sistran.Core.Application.AccountingServices.Assemblers;
using Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.AccountingServices.DTOs.Payments;
using Sistran.Core.Application.AccountingServices.DTOs.Search;
using Sistran.Core.Application.AccountingServices.EEProvider.DAOs;
using Sistran.Core.Application.AccountingServices.EEProvider.Enums;
using Sistran.Core.Application.AccountingServices.Enums;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using UTILHELPER = Sistran.Core.Application.Utilities.Helper;
using System.Configuration;
using Sistran.Core.Application.AccountingServices.EEProvider.DAOs.Accounting;
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Framework.Transactions;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Business
{
    public class AccountingAccountBusiness
    {
        public MessageSuccessDTO SaveCollectGeneralLedgerPayment(CollectGeneralLedgerDTO collectGeneralLedgerDTO)
        {
            int branchId = collectGeneralLedgerDTO.CollectImputation.Collect.Branch.Id;
            DateTime accountingDate = collectGeneralLedgerDTO.CollectImputation.Collect.Date;//accounting date de la collect control
            #region SaveCollect
            collectGeneralLedgerDTO.CollectImputation = DelegateService.accountingCollectService.SaveCollectImputation(collectGeneralLedgerDTO.CollectImputation, collectGeneralLedgerDTO.BillControlId, true);
            collectGeneralLedgerDTO.CollectImputation.Collect.Date = accountingDate;//se re asigna la fecha contable 
            #endregion SaveCollect

            #region APLICACIÓN PRELIQUIDACIÓN EE

            // Para aplicación de preliquidación. 
            if (collectGeneralLedgerDTO.CollectImputation.Collect.SourcePaymentId > 0)
            {
                PreliquidationsDTO preliquidationsDTO = new PreliquidationsDTO()
                {
                    BranchId = Convert.ToInt32(collectGeneralLedgerDTO.PreliquidationBranch),
                    PreliquidationId = Convert.ToInt32(collectGeneralLedgerDTO.CollectImputation.Collect.SourcePaymentId)
                };

                List<PreliquidationsDTO> preLiquidations = DelegateService.accountingApplicationService.GetPreliquidations(preliquidationsDTO);

                if (preLiquidations.Count > 0)
                {
                    DelegateService.accountingApplicationService.SaveApplicationRequest(collectGeneralLedgerDTO.CollectImputation.Collect.Id,
                                              Convert.ToInt32(preLiquidations[0].TempImputationId),
                                              Convert.ToInt32(ApplicationTypes.PreLiquidation), collectGeneralLedgerDTO.CollectImputation.Collect.Description,
                                              collectGeneralLedgerDTO.StatusId, collectGeneralLedgerDTO.UserId, collectGeneralLedgerDTO.CollectImputation.Collect.SourcePaymentId, collectGeneralLedgerDTO.CollectImputation.Transaction.Id, accountingDate);
                }
            }
            #endregion

            #region Accounting

            // Ejecuto método para iniciar contabilización
            string recordCollectMessage = "";
            string imputationMessage = "";
            bool showMessage = true;
            bool generalLedgerSuccess = false;

            if ((string)UTILHELPER.EnumHelper.GetEnumParameterValue<AccountingKeys>(AccountingKeys.ACC_ENABLED_GENERAL_LEGDER) == "true")
            {
                SaveBillParametersDTO saveBillParametersModel = new SaveBillParametersDTO()
                {
                    Collect = collectGeneralLedgerDTO.CollectImputation.Collect,
                    TypeId = 1,
                    UserId = collectGeneralLedgerDTO.UserId,
                };

                if (saveBillParametersModel.Collect.Branch == null)
                {
                    saveBillParametersModel.Collect.Branch = new BranchDTO()
                    {
                        Id = branchId
                    };
                }

                string integrationMessage = "";
                int entryNumber = 0;
                //using (Context.Current) {
                //    using (Transaction transaction = new Transaction())
                //    {
                //        try
                //        {
                            entryNumber = DelegateService.accountingAccountService.AccountingParametersRequest(saveBillParametersModel);
                //            transaction.Complete();
                //        } catch (Exception ex)
                //        {
                //            transaction.Dispose();
                //        }
                //    }
                //}

                if (entryNumber > 0)
                {
                    generalLedgerSuccess = true;
                    integrationMessage = Resources.Resources.IntegrationSuccessMessage + " " + entryNumber;
                }
                if (entryNumber == 0)
                {
                    integrationMessage = Resources.Resources.AccountingIntegrationUnbalanceEntry;
                }
                if (entryNumber == -2)
                {
                    integrationMessage = Resources.Resources.EntryRecordingError;
                }

                recordCollectMessage = integrationMessage;
            }
            else
            {
                recordCollectMessage = Convert.ToString(Resources.Resources.IntegrationServiceDisabledLabel);
                showMessage = false;
                generalLedgerSuccess = true;
            }

            // Se obtiene el asiento generado desde GL
            if (collectGeneralLedgerDTO.CollectImputation.Collect.Transaction.TechnicalTransaction > 0)
            {
                recordCollectMessage = Convert.ToString(Resources.Resources.IntegrationSuccessMessage) + " " + collectGeneralLedgerDTO.CollectImputation.Collect.Transaction.TechnicalTransaction;
            }

            #endregion Accounting

            #region PaymentTicket
            //Grabación boleta interna (únicamente aplica para consignación de cheques)

            DTOs.PaymentTicketDTO paymentTicket = new DTOs.PaymentTicketDTO();
            List<PaymentDTO> payments = new List<PaymentDTO>();

            decimal total = 0;
            decimal commission = 0;
            // TODO: Validar que los movimientos correspondan al resultado
            if (collectGeneralLedgerDTO.CollectImputation.Collect.Payments.Count == collectGeneralLedgerDTO.Bill.PaymentSummary.Count
                && collectGeneralLedgerDTO.CollectImputation.Collect.Payments.Count(p => p.PaymentMethod.Id == Convert.ToInt32(PaymentMethods.DepositVoucher)
                    || p.PaymentMethod.Id == Convert.ToInt32(PaymentMethods.ConsignmentByCheck)) > 0)
            {
                PaymentDAO paymentDAO = new PaymentDAO();
                List<Models.Payments.Payment> collectPayments = paymentDAO.GetCollectPaymentsByCollectId(collectGeneralLedgerDTO.CollectImputation.Collect.Id);

                if (collectPayments != null && collectPayments.Count > 0)
                {
                    collectPayments.ForEach(p =>
                    {
                        if (p.PaymentMethod.Id == Convert.ToInt32(PaymentMethods.ConsignmentByCheck)
                            || p.PaymentMethod.Id == Convert.ToInt32(PaymentMethods.DepositVoucher))
                        {
                            commission = 0;
                            total = p.Amount.Value;
                            //Objeto PaymentTicket
                            paymentTicket = new DTOs.PaymentTicketDTO();
                            payments = new List<PaymentDTO>();
                            paymentTicket.Amount = new AmountDTO();
                            paymentTicket.Commission = new AmountDTO();
                            paymentTicket.Commission.Value = commission;
                            paymentTicket.Branch = new BranchDTO();
                            paymentTicket.Branch.Id = branchId;
                            paymentTicket.Bank = new BankDTO();
                            paymentTicket.CashAmount = new AmountDTO();
                            paymentTicket.CashAmount.Value = 0;
                            paymentTicket.Currency = new CurrencyDTO();
                            paymentTicket.Currency.Id = p.Amount.Currency.Id;
                            paymentTicket.PaymentMethod = p.PaymentMethod.Id;
                            paymentTicket.Bank.Id = p.IssuingBankCode;
                            paymentTicket.AccountNumber = p.IssuingAccountNumber;
                            if (p.PaymentMethod.Id == Convert.ToInt32(PaymentMethods.DepositVoucher))
                            {
                                paymentTicket.CashAmount.Value = total;
                            }
                            else if (p.PaymentMethod.Id == Convert.ToInt32(PaymentMethods.ConsignmentByCheck))
                            {
                                paymentTicket.Amount.Value = total;
                            }

                            payments.Add(new PaymentDTO()
                            {
                                Id = p.Id
                            });


                            paymentTicket.Payments = payments;

                            paymentTicket = DelegateService.accountingPaymentTicketService.SaveInternalBallot(paymentTicket, collectGeneralLedgerDTO.UserId);

                            #region PaymentBallot

                            PaymentBallotAccountingDTO paymentBallotAccounting = new PaymentBallotAccountingDTO();
                            PaymentBallotParametersDTO PaymentBallotParameters = new PaymentBallotParametersDTO();

                            PaymentBallotParameters.PaymentAccountNumber = p.IssuingAccountNumber;
                            PaymentBallotParameters.PaymentBallotAmount = total;
                            PaymentBallotParameters.PaymentBallotBankAmount = total;
                            PaymentBallotParameters.PaymentBallotBankId = paymentTicket.Bank.Id;
                            PaymentBallotParameters.PaymentBallotId = 0;
                            PaymentBallotParameters.PaymentBankDate = Convert.ToDateTime(p.DatePayment);
                            PaymentBallotParameters.PaymentCurrency = p.Amount.Currency.Id;
                            PaymentBallotParameters.PaymentStatus = 1;//frmPaymentBallot.PaymentStatus;
                            PaymentBallotParameters.PaymentBallotNumber = p.Voucher;

                            List<PaymentTicketBallotDTO> PaymentTicketBallotModels = new List<PaymentTicketBallotDTO>();

                            PaymentTicketBallotModels.Add(new PaymentTicketBallotDTO()
                            {
                                PaymentTicketBallotId = paymentTicket.Id
                            });

                            PaymentBallotParameters.PaymentTicketBallotModels = PaymentTicketBallotModels;
                            paymentBallotAccounting.PaymentBallotParameters = PaymentBallotParameters;
                            paymentBallotAccounting.UserId = collectGeneralLedgerDTO.UserId;
                            paymentBallotAccounting.TypeId = 1;
                            paymentBallotAccounting.AccountingDate = collectGeneralLedgerDTO.CollectImputation.Collect.Date;

                            List<PaymentBallotResponsesDTO> paymentBallotResponses = DelegateService.accountingPaymentBallotService.SaveAccountingPaymentBallot(paymentBallotAccounting);

                            #endregion PaymentBallot
                        }
                    });
                }
            }

            #endregion PaymentTicket

            MessageSuccessDTO messageSucess = new MessageSuccessDTO
            {
                ImputationMessage = imputationMessage,
                RecordCollectMessage = recordCollectMessage,
                ShowMessage = showMessage,
                TechnicalTransaction = collectGeneralLedgerDTO.CollectImputation.Collect.Transaction.TechnicalTransaction,
                BillId = collectGeneralLedgerDTO.CollectImputation.Collect.Id,
                GeneralLedgerSuccess = generalLedgerSuccess
            };
            //TODO: SE GRABA TABLA DE CONTROL DESPUES DE QUE SE GUARDE LA DATA TECNICA Y CONTABLE(ESTA PUEDE NO QUEDAR GRABADA/ERROR DE ASIENTO)
            Models.Collect.Collect movementToControl = new Models.Collect.Collect()
            {
                Id = collectGeneralLedgerDTO.CollectImputation.Collect.Id
            };
            Integration2GBusiness integration2GBusiness = new Integration2GBusiness();
            integration2GBusiness.Save(movementToControl.ToModelIntegration(1));
            return messageSucess;
        }

        public int AccountingParametersRequest(SaveBillParametersDTO saveBillParametersDTO)
        {
            decimal billIncomeAmount = 0;
            decimal billAmount = 0;
            string description = null;

            //Get transaction number
            CollectDTO collect = new CollectDTO();
            CollectApplicationDTO collectApplicationDTO = new CollectApplicationDTO();
            collect.Id = saveBillParametersDTO.Collect.Id;
            DTOs.Imputations.ApplicationDTO applicationDTO = new DTOs.Imputations.ApplicationDTO();
            //Se obtienen los parámetros para ejecutar 
            List<AccountingParameterDTO> accountingParameter = GetAccountingParameters(saveBillParametersDTO.Collect.Id);

            //Se generan los parámetro para la cabecera del recibo.
            foreach (var accountingItem in accountingParameter)
            {
                billIncomeAmount = billIncomeAmount + accountingItem.IncomeAmount;
                billAmount = billAmount + accountingItem.Amount;
            }

            if (saveBillParametersDTO.TypeId == 1) //ingreso de caja 
            {
                //Valida si tiene pago con retención
                if (ValidatePaymentRetention(saveBillParametersDTO.Collect.Id))
                {
                    description = GetDescriptionRetentionByCollectId(saveBillParametersDTO.Collect.Id);
                }
            }

            AccountingParametersDTO accountingParameters = new AccountingParametersDTO();
            accountingParameters.JournalEntryListParameters = new List<AccountingListParametersDTO>();
            foreach (AccountingParameterDTO accountingParam in accountingParameter)
            {
                AccountingListParametersDTO accountingListParametersDTO = new AccountingListParametersDTO();
                accountingListParametersDTO.CurrencyCode = accountingParam.CurrencyCode;
                accountingListParametersDTO.BranchCode = accountingParam.BranchCode;
                accountingListParametersDTO.Amount = accountingParam.IncomeAmount;
                accountingListParametersDTO.LocalAmount = accountingParam.Amount;
                accountingListParametersDTO.ExchangeRate = accountingParam.ExchangeRate;
                accountingListParametersDTO.PayerId = accountingParam.PayerId;
                accountingListParametersDTO.PaymentCode = accountingParam.PaymentCode;
                accountingListParametersDTO.PaymentMethodTypeCode = accountingParam.PaymentMethodTypeCode;

                if (accountingParam.PaymentMethodTypeCode == Convert.ToInt32(PaymentMethods.DepositVoucher)
                    || accountingParam.PaymentMethodTypeCode == Convert.ToInt32(PaymentMethods.ConsignmentByCheck))
                {
                    accountingListParametersDTO.AccountingNature = (int)AccountingNature.Debit;

                    if (accountingParam.PaymentMethodTypeCode == Convert.ToInt32(PaymentMethods.DepositVoucher))
                    {
                        accountingListParametersDTO.PaymentMethodTypeCode = Convert.ToInt32(PaymentMethods.Cash);
                    }
                    else
                        accountingListParametersDTO.PaymentMethodTypeCode = Convert.ToInt32(PaymentMethods.CurrentCheck);

                }
                accountingParameters.JournalEntryListParameters.Add(accountingListParametersDTO);
            }

            CollectControlBusiness collectControlBusiness = new CollectControlBusiness();
            accountingParameters.BranchId = saveBillParametersDTO.Collect.Branch.Id;
            accountingParameters.BillId = saveBillParametersDTO.Collect.Id;
            accountingParameters.Description = description;
            accountingParameters.TypeId = saveBillParametersDTO.TypeId;
            accountingParameters.UserId = saveBillParametersDTO.UserId;
            accountingParameters.TechnicalTransaction = saveBillParametersDTO.Collect.Transaction.TechnicalTransaction;
            accountingParameters.AccountingDate = collectControlBusiness.GetAccountingDateByCollectId(saveBillParametersDTO.Collect.Id);
            accountingParameters.BridgeAccoutingId = CommonBusiness.GetIntParameter(AccountingKeys.ACC_BRIDGE_COLLECT);
            collectApplicationDTO = DelegateService.accountingApplicationService.GetApplicationByTechnicalTransaction(saveBillParametersDTO.Collect.Transaction.TechnicalTransaction);

            if (collectApplicationDTO.Application.Id > 0)
            {
                Business.ApplicationBusiness applicationBusiness = new Business.ApplicationBusiness();

                accountingParameters.Application = collectApplicationDTO.Application.ToLedgerDTO();
                // Se usa la cuenta puente de collect, ya que es la que sirve para balancear el asiento
                accountingParameters.Application.BridgeAccountingId = CommonBusiness.GetIntParameter(AccountingKeys.ACC_BRIDGE_COLLECT);
                accountingParameters.ApplicationItems = new List<DTOs.Imputations.ApplicationJournalEntryDTO>();
                // Consultar los movimentos contables
                var premium = applicationBusiness.GetApplicationPremiumsByApplicationId(accountingParameters.Application.Id).ToDTOs();
                accountingParameters.ApplicationItems.AddRange(DelegateService.accountingApplicationService.GetApplicationAccountsByApplicationId(accountingParameters.Application.Id).ToJournalDTOs());

                accountingParameters.ApplicationItems.AddRange(premium.ToJournalDTOs(accountingParameters.Application.ModuleId));
                foreach (DTOs.Imputations.ApplicationPremiumDTO applicationPremiumDTO in premium)
                {
                    accountingParameters.ApplicationItems.AddRange(DelegateService.accountingApplicationService.
                        GetApplicationPremiumCommisionsByApplicationPremiumId(applicationPremiumDTO.Id).ToJournalDTOs(accountingParameters.Application.ModuleId));
                }
            }
            string parameters = Newtonsoft.Json.JsonConvert.SerializeObject(accountingParameters);
            int entryNumber = 0;
            using (Context.Current)
            {
                //using (Transaction transaction = new Transaction())
                //{
                    //try
                    //{
                        entryNumber = DelegateService.accountingGeneralLedgerApplicationService.SaveCollectJournalEntry(parameters);
                    //    transaction.Complete();
                    //}
                    //catch (Exception ex)
                    //{
                     //   transaction.Dispose();
                    //}
                //}
            }


            return entryNumber;
        }

        /// <summary>
        /// GetAccountingParameters
        /// Metodo para obtener los parámetros para armar la contabilización de un ingreso de caja
        /// </summary>
        /// <param name="collectId"></param>
        /// <returns>List<AccountingParameterDTO/></returns>
        public List<AccountingParameterDTO> GetAccountingParameters(int collectId)
        {
            List<AccountingParameterDTO> accountingParameters = new List<AccountingParameterDTO>();

            int rows;
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.Collect.Properties.CollectId, collectId);

            Framework.DAF.Engine.IDataFacadeManager _dataFacadeManager = Framework.DAF.Engine.Factories.AppConfigDataFacadeManagerFactory.SingletonInstance;
            List<ACCOUNTINGEN.PaymentV> dataParameters = _dataFacadeManager.GetDataFacade().List(
                typeof(ACCOUNTINGEN.PaymentV), criteriaBuilder.GetPredicate()).Cast<ACCOUNTINGEN.PaymentV>().ToList();
            int decimalPlaces = 2;

            if (dataParameters.Count > 0)
            {
                foreach (ACCOUNTINGEN.PaymentV payment in dataParameters)

                {
                    AccountingParameterDTO accountingParameterDTO = new AccountingParameterDTO();
                    accountingParameterDTO.CollectCode = payment.CollectId;
                    accountingParameterDTO.CollectConceptCode = Convert.ToInt32(payment.CollectConceptCode);
                    accountingParameterDTO.RegisterDate = Convert.ToDateTime(payment.RegisterDate);
                    accountingParameterDTO.PaymentCode = payment.PaymentCode;
                    accountingParameterDTO.PaymentMethodTypeCode = Convert.ToInt32(payment.PaymentMethodTypeCode);
                    accountingParameterDTO.IssuingBankCode = Convert.ToInt32(payment.IssuingBankCode);
                    accountingParameterDTO.CurrencyCode = Convert.ToInt32(payment.CurrencyCode);
                    accountingParameterDTO.ExchangeRate = Math.Round(Convert.ToDecimal(payment.ExchangeRate), decimalPlaces);
                    accountingParameterDTO.IncomeAmount = Math.Round(Convert.ToDecimal(payment.IncomeAmount), decimalPlaces);
                    accountingParameterDTO.Amount = Math.Round(Convert.ToDecimal(payment.Amount), decimalPlaces);
                    accountingParameterDTO.PayerId = Convert.ToInt32(payment.IndividualId);
                    accountingParameterDTO.PayerDocumentNumber = payment.PayerDocumentNumber;
                    accountingParameterDTO.CollectControlCode = payment.CollectControlId;
                    accountingParameterDTO.BranchCode = Convert.ToInt32(payment.BranchCode);
                    accountingParameterDTO.UserId = Convert.ToInt32(payment.UserId);
                    accountingParameterDTO.PaymentsTotal = Math.Round(Convert.ToDecimal(payment.PaymentsTotal), decimalPlaces);
                    accountingParameterDTO.ReceivingBankCode = Convert.ToInt32(payment.ReceivingBankCode);
                    accountingParameterDTO.ReceivingAccountingNumber = payment.ReceivingAccountNumber;
                    accountingParameterDTO.IssuingAccountingNumber = payment.IssuingAccountNumber;
                    accountingParameterDTO.DocumentNumber = payment.DocumentNumber;
                    accountingParameters.Add(accountingParameterDTO);
                }
            }

            return accountingParameters;

        }
        /// <summary>
        /// ValidatePaymentRetention
        /// </summary>
        /// <param name="collectId"></param>
        /// <returns>bool</returns>
        private bool ValidatePaymentRetention(int collectId)
        {
            foreach (PaymentDTO payment in DelegateService.accountingApplicationService.GetPaymentsByCollectId(collectId))
            {
                if (payment.PaymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodRetentionReceipt"]))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// GetDescriptionRetentionByRetentionConceptId
        /// </summary>
        /// <param name="retentionConceptId"></param>
        /// <returns>string</returns>
        private string GetDescriptionRetentionByCollectId(int collectId)
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder(Resources.Resources.By);

            foreach (PaymentDTO payment in DelegateService.accountingApplicationService.GetPaymentsByCollectId(collectId))
            {
                if (payment.PaymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodRetentionReceipt"]))
                {
                    stringBuilder.Append(DelegateService.accountingRetentionService.GetRetentionConcepts().Where(rc => rc.Id.Equals(Convert.ToInt32(payment.PaymentMethod.Description))).ToList()[0].Description);
                    stringBuilder.Append(", ");
                }
            }

            return stringBuilder.ToString() + Resources.Resources.AccordingNumberReceipt;
        }

        public int GetAccountingAccountIdByConceptId(int conceptId)
        {
            AccountingAccountDAO accountingAccountDAO = new AccountingAccountDAO();
            return accountingAccountDAO.GetAccountingAccountIdByConceptId(conceptId);
        }

        public int GetConceptIdByAccoutingAccountId(int accountingAccountId)
        {
            AccountingAccountDAO accountingAccountDAO = new AccountingAccountDAO();
            return accountingAccountDAO.GetConceptIdByAccoutingAccountId(accountingAccountId);
        }
    }
}
