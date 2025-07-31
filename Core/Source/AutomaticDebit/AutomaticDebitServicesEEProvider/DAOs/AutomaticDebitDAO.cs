//Sistran Core
using Sistran.Co.Application.Data;
using Sistran.Core.Application.AccountingServices;
using Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.AccountingServices.DTOs.BankAccounts;
using Sistran.Core.Application.AccountingServices.DTOs.Imputations;
using Sistran.Core.Application.AccountingServices.DTOs.Payments;
using Sistran.Core.Application.AccountingServices.DTOs.Search;
using Sistran.Core.Application.AccountingServices.Enums;
using AUTMOD = Sistran.Core.Application.AutomaticDebitServices.Models;
using Sistran.Core.Application.AutomaticDebitServices.EEProvider.Assemblers;
using Sistran.Core.Application.AutomaticDebitServices.Models;
using Sistran.Core.Application.CommonService;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniquePersonService.V1;
using Sistran.Core.Application.UniquePersonService.V1.Models;
//Sitran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.SAF;
using Sistran.Core.Framework.Views;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using PaymentModels = Sistran.Core.Application.AccountingServices.DTOs.Payments;
using Sistran.Core.Application.UniquePersonService.V1;

namespace Sistran.Core.Application.AutomaticDebitServices.EEProvider.DAOs
{
    public class AutomaticDebitDAO
    {
        #region Instance Variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;
        readonly IAccountingCollectService _collectService = ServiceProvider.Instance.getServiceManager().GetService<IAccountingCollectService>();
        readonly IAccountingCollectControlService _collectControlService = ServiceProvider.Instance.getServiceManager().GetService<IAccountingCollectControlService>();
        readonly ICommonServiceCore _commonService = ServiceProvider.Instance.getServiceManager().GetService<ICommonServiceCore>();
        readonly IAccountingParameterService _parameterService = ServiceProvider.Instance.getServiceManager().GetService<IAccountingParameterService>();
        readonly IAccountingPaymentService _paymentService = ServiceProvider.Instance.getServiceManager().GetService<IAccountingPaymentService>();
        readonly IUniquePersonServiceCore _uniquePersonService = ServiceProvider.Instance.getServiceManager().GetService<IUniquePersonServiceCore>();

        readonly AutomaticDebitStatusDAO _automaticDebitStatusDAO = new AutomaticDebitStatusDAO();
        readonly BankNetworkDAO _bankNetworkDAO = new BankNetworkDAO();

        #endregion

        #region Public Methods

        #region Save

        /// <summary>
        /// SaveAutomaticDebit
        /// </summary>
        /// <param name="automaticDebit"></param>
        /// <returns></returns>
        public AutomaticDebit SaveAutomaticDebit(AutomaticDebit automaticDebit)
        {
            AutomaticDebit newAutomaticDebit = new AutomaticDebit();

            var parameters = new NameValue[10];

            if (automaticDebit.Coupons[0].Policy.Id > 0)
            {
                DateTime accountingDate = _commonService.GetModuleDateIssue(automaticDebit.Id, DateTime.Now).Date;

                string lotNumber = Convert.ToString(automaticDebit.BankNetwork.Id) + automaticDebit.Date.ToString("yyyyMMddHHmm");

                parameters[0] = new NameValue("BRANCH_CD", automaticDebit.Coupons[0].Policy.Branch.Id);
                parameters[1] = new NameValue("BANK_NETWORK_ID", automaticDebit.BankNetwork.Id);
                parameters[2] = new NameValue("SHIPPING_DATE", automaticDebit.Date);
                parameters[3] = new NameValue("PROCESS_PREPARE", -1);
                parameters[4] = new NameValue("USER_CD", automaticDebit.Coupons[0].Policy.UserId);
                if (automaticDebit.RetriesNumber == -999)
                {
                    parameters[5] = new NameValue("ATTEMPTS_NUMBER", null);
                }
                else
                {
                    parameters[5] = new NameValue("ATTEMPTS_NUMBER", automaticDebit.RetriesNumber);
                }
                parameters[6] = new NameValue("PREFIX_CD", automaticDebit.Coupons[0].Policy.Prefix.Id);
                parameters[7] = new NameValue("POLICY_NUMBER", automaticDebit.Coupons[0].Policy.DocumentNumber);
                parameters[8] = new NameValue("LOT_NUMBER", lotNumber);
                parameters[9] = new NameValue("DATE_ACCOUNTING", accountingDate);

                try
                {
                    using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                    {
                        dynamicDataAccess.ExecuteSPDataTable("ACC.GENERATE_COUPONS", parameters);
                    }

                    parameters = new NameValue[10];

                    parameters[0] = new NameValue("BRANCH_CD", automaticDebit.Coupons[0].Policy.Branch.Id);
                    parameters[1] = new NameValue("BANK_NETWORK_ID", automaticDebit.BankNetwork.Id);
                    parameters[2] = new NameValue("SHIPPING_DATE", automaticDebit.Date);
                    parameters[3] = new NameValue("PROCESS_PREPARE", 0);
                    parameters[4] = new NameValue("USER_CD", automaticDebit.Coupons[0].Policy.UserId);

                    if (automaticDebit.RetriesNumber == -999)
                    {
                        parameters[5] = new NameValue("ATTEMPTS_NUMBER", null);
                    }
                    else
                    {
                        parameters[5] = new NameValue("ATTEMPTS_NUMBER", automaticDebit.RetriesNumber);
                    }
                    parameters[6] = new NameValue("PREFIX_CD", automaticDebit.Coupons[0].Policy.Prefix.Id);
                    parameters[7] = new NameValue("POLICY_NUMBER", automaticDebit.Coupons[0].Policy.DocumentNumber);
                    parameters[8] = new NameValue("LOT_NUMBER", lotNumber);
                    parameters[9] = new NameValue("DATE_ACCOUNTING", accountingDate);

                    using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                    {
                        dynamicDataAccess.ExecuteSPDataTable("ACC.GENERATE_COUPONS", parameters);
                    }
                    newAutomaticDebit.Id = 0;
                    newAutomaticDebit.Description = "";
                }
                catch (BusinessException ex)
                {
                    newAutomaticDebit.Id = -1;
                    newAutomaticDebit.Description = ex.Message;
                }
            }
            else if (automaticDebit.Coupons[0].Policy.Id == 0)
            {
                String[] logBankResponse = new String[15];
                logBankResponse[0] = "0";
                logBankResponse[1] = automaticDebit.BankNetwork.Id.ToString();
                logBankResponse[2] = (automaticDebit.BankNetwork.Description == "") ? "" : automaticDebit.BankNetwork.Description;
                logBankResponse[3] = automaticDebit.Coupons[0].Policy.Holder.IndividualId.ToString();
                logBankResponse[4] = (automaticDebit.Description == null) ? "" : automaticDebit.Description;
                logBankResponse[5] = (automaticDebit.Coupons[0].Policy.Branch.Description == "") ? "" : automaticDebit.Coupons[0].Policy.Branch.Description;
                logBankResponse[6] = (automaticDebit.Coupons[0].Policy.ExchangeRate.Currency.Description == "") ? "" : automaticDebit.Coupons[0].Policy.ExchangeRate.Currency.Description;
                logBankResponse[7] = automaticDebit.Date.ToString("dd/MM/yyyy");
                logBankResponse[8] = (automaticDebit.Coupons[0].Policy.BillingGroup.Description == "") ? "0" : automaticDebit.Coupons[0].Policy.BillingGroup.Description;
                logBankResponse[9] = automaticDebit.Date.ToString("dd/MM/yyyy");
                logBankResponse[10] = (automaticDebit.Coupons[0].Policy.Prefix.Description == "") ? "" : automaticDebit.Coupons[0].Policy.Prefix.Description;
                logBankResponse[11] = (automaticDebit.Coupons[0].Policy.PolicyType.Description == "") ? "" : automaticDebit.Coupons[0].Policy.PolicyType.Description;
                logBankResponse[12] = automaticDebit.Coupons[0].Policy.Prefix.Id.ToString() == "0" ? "false" : "true";
                logBankResponse[13] = "";
                logBankResponse[14] = automaticDebit.Coupons[0].Policy.Holder.IndividualId.ToString();

                var automaticDebitList = SaveLogBankResponse(logBankResponse);

                newAutomaticDebit.Id = Convert.ToInt32(automaticDebitList.GetValue(0));
                newAutomaticDebit.Description = automaticDebitList.GetValue(1).ToString();
            }
            else if (automaticDebit.Coupons[0].Policy.Id == -1)
            {
                int rows;

                CollectDTO collect = new CollectDTO();
                CollectConceptDTO collectConcept = new CollectConceptDTO();

                AmountDTO paymentsTotal = new AmountDTO();
                PersonDTO payer = new PersonDTO();
                BranchDTO branch = new BranchDTO();
                int number = 0;
                int parameterId = 6016;

                Parameter parameter = new Parameter();

                CollectControlDTO collectControls = _collectControlService.NeedCloseCollect(automaticDebit.UserId, branch.Id, DateTime.Now.Date, 1);

                ObjectCriteriaBuilder filterShipmentNetwork = new ObjectCriteriaBuilder();
                filterShipmentNetwork.PropertyEquals(Entities.AppliedCouponsView.Properties.BankNetworkId, automaticDebit.Id);
                filterShipmentNetwork.And();
                filterShipmentNetwork.Property(Entities.AppliedCouponsView.Properties.StatusCode);
                filterShipmentNetwork.Distinct();
                filterShipmentNetwork.Constant(6);   //ya aplicados

                UIView shipmentCollection = _dataFacadeManager.GetDataFacade().GetView("AppliedCouponView", filterShipmentNetwork.GetPredicate(), null, 0, -1, null, false, out rows);

                number = (int)_commonService.GetParameterByParameterId(parameterId).NumberParameter;

                foreach (DataRow shipmentCoupon in shipmentCollection)
                {
                    // Se actualiza parámetro de número de carátula.
                    parameter.Id = parameterId;
                    parameter.NumberParameter = number;
                    parameter.Description = "Numero de Caratula de Recibo";
                    parameter.NumberParameter = parameter.NumberParameter + 1;
                    _commonService.UpdateParameter(parameter);

                    collectConcept.Id = 1;  //REC.PRIMAS                
                    paymentsTotal.Value = Convert.ToDecimal(shipmentCoupon["CouponEndorsementAmount"]);
                    payer.IndividualId = Convert.ToInt32(shipmentCoupon["IndividualId"]);
                    payer.IdentificationDocument = new IdentificationDocumentDTO()
                    {
                        DocumentType = new DocumentTypeDTO() { Id = 0 }
                    };
                    branch.Id = Convert.ToInt32(shipmentCoupon["BranchCode"]);
                    CompanyDTO accountingCompany = new CompanyDTO();

                    collect.Description = "Debitos automaticos envio del " + automaticDebit.Date.ToString();
                    collect.Date = _commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateAccounting"]), DateTime.Now);
                    collect.Concept = collectConcept;
                    collect.PaymentsTotal = paymentsTotal;
                    collect.Payer = payer;
                    collect.Status = Convert.ToInt16(CollectStatus.Applied);
                    collect.Number = number;
                    collect.CollectType = (int)CollectTypes.Incoming;
                    collect.UserId = automaticDebit.UserId;
                    collect.AccountingCompany = accountingCompany;
                    collect.Branch = branch;
                    collect.Payments = new List<PaymentDTO>();

                    #region Payment

                    PaymentMethodDTO paymentMethod = new PaymentMethodDTO() { Id = Convert.ToInt32(shipmentCoupon["PaymentMethodCode"]) };

                    #region PaymentMethodType

                    #region Cash

                    if (paymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodCash"]))
                    {

                        AmountDTO amount = new AmountDTO()
                        {
                            Currency = new CurrencyDTO() { Id = Convert.ToInt32(shipmentCoupon["CurrencyCode"]) },
                            Value = Convert.ToDecimal(shipmentCoupon["CouponEndorsementAmount"]) * Convert.ToDecimal(shipmentCoupon["ExchangeRate"])
                        };

                        ExchangeRateDTO exchangeRate = new ExchangeRateDTO() { BuyAmount = Convert.ToDecimal(shipmentCoupon["ExchangeRate"]) };
                        AmountDTO localAmount = new AmountDTO() { Value = Convert.ToDecimal(shipmentCoupon["CouponEndorsementAmount"]) };


                        collect.Payments.Add(new CashDTO()
                        {
                            PaymentMethod = paymentMethod,
                            Amount = amount,
                            ExchangeRate = exchangeRate,
                            Id = 0,
                            LocalAmount = localAmount,
                            Status = Convert.ToInt16(PaymentStatus.Active)
                        });
                    }

                    #endregion Cash

                    #region Check

                    if (paymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodPostdatedCheck"]) ||
                        paymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodCurrentCheck"]))
                    {
                        AmountDTO amount = new AmountDTO()
                        {
                            Currency = new CurrencyDTO() { Id = Convert.ToInt32(shipmentCoupon["CurrencyCode"]) },
                            Value = Convert.ToDecimal(shipmentCoupon["CouponEndorsementAmount"]) * Convert.ToDecimal(shipmentCoupon["ExchangeRate"])
                        };
                        BankDTO issuingBank = new BankDTO() { Id = Convert.ToInt32(shipmentCoupon["IssuingCardBankCode"]) };

                        AmountDTO localAmount = new AmountDTO() { Value = Convert.ToDecimal(shipmentCoupon["CouponEndorsementAmount"]) };
                        ExchangeRateDTO exchangeRate = new ExchangeRateDTO() { BuyAmount = Convert.ToDecimal(shipmentCoupon["ExchangeRate"]) };

                        collect.Payments.Add(new CheckDTO()
                        {
                            PaymentMethod = paymentMethod,
                            Amount = amount,
                            ExchangeRate = exchangeRate,
                            Date = automaticDebit.Date.Date,
                            DocumentNumber = shipmentCoupon["SourceCode"].ToString(),
                            Id = 0,
                            LocalAmount = localAmount,
                            IssuerName = GetPersonByIndividualId(Convert.ToInt32(shipmentCoupon["IndividualId"])).Name,  //revisar emisor
                            IssuingAccountNumber = shipmentCoupon["AccountBankCode"].ToString(),
                            IssuingBank = issuingBank,
                            Status = Convert.ToInt16(PaymentStatus.Active)
                        });
                    }

                    #endregion Check

                    #region CreditCard

                    if (paymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodCreditableCreditCard"]) ||
                        paymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodUncreditableCreditCard"]) ||
                        paymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodDataphone"]))
                    {

                        decimal taxBase = Convert.ToDecimal(shipmentCoupon["Tax"]);
                        AmountDTO amount = new AmountDTO()
                        {
                            Currency = new CurrencyDTO() { Id = Convert.ToInt32(shipmentCoupon["CurrencyCode"]) },
                            Value = Convert.ToDecimal(shipmentCoupon["CouponEndorsementAmount"])
                        };

                        ExchangeRateDTO exchangeRate = new ExchangeRateDTO() { BuyAmount = Convert.ToDecimal(shipmentCoupon["ExchangeRate"]) };

                        AmountDTO localAmount = new AmountDTO();
                        localAmount.Value = Convert.ToDecimal(shipmentCoupon["CouponEndorsementAmount"]);

                        BankDTO issuingBank = new BankDTO();
                        issuingBank.Id = Convert.ToInt32(shipmentCoupon["IssuingCardBankCode"]);

                        CreditCardTypeDTO creditCardType = new CreditCardTypeDTO();
                        creditCardType.Id = Convert.ToInt32(shipmentCoupon["CreditCardTypeCode"]);

                        CreditCardValidThruDTO creditCardValidThru = new CreditCardValidThruDTO();
                        creditCardValidThru.Year = DateTime.Now.Year;
                        creditCardValidThru.Month = 1;

                        List<PaymentTaxDTO> paymentTaxes = _paymentService.GetTaxCreditCard(creditCardType.Id, Convert.ToInt32(shipmentCoupon["TaxBranch"])).Taxes;

                        decimal ivaCardAmount = 0;
                        decimal tax = 0;
                        decimal retention = 0;

                        if (paymentTaxes.Count > 0)
                        {
                            for (int i = 0; i < paymentTaxes.Count; i++)
                            {
                                if (paymentTaxes[i].Tax.Id == Convert.ToInt32(shipmentCoupon["CreditCardTypeTaxCode"]))
                                {
                                    ivaCardAmount = taxBase * paymentTaxes[i].Rate / 100;
                                }
                            }

                            // Calcula la comisión
                            if (creditCardType.Id > 0)
                            {
                                creditCardType.Commission = _parameterService.GetCreditCardType(Convert.ToInt32(shipmentCoupon["CreditCardTypeCode"])).Commission * (localAmount.Value - ivaCardAmount) / 100;

                            }
                            else
                            {
                                creditCardType.Commission = 0;
                            }

                            // Asigna las base del impuesto
                            for (int f = 0; f < paymentTaxes.Count; f++)
                            {
                                paymentTaxes[f].TaxBase = new AmountDTO();
                                if (paymentTaxes[f].Tax.Id == Convert.ToInt32(shipmentCoupon["CreditCardTypeTaxCode"]))
                                {
                                    paymentTaxes[f].TaxBase.Value = taxBase;
                                }

                                if (paymentTaxes[f].Tax.Id == Convert.ToInt32(10))    //TaxRetentionCardIcaId
                                {
                                    paymentTaxes[f].TaxBase.Value = Convert.ToDecimal(shipmentCoupon["CouponEndorsementAmount"]) - ivaCardAmount;
                                }

                                if (paymentTaxes[f].Tax.Id == Convert.ToInt32(15))    //TaxRetentionCardIvaId
                                {
                                    paymentTaxes[f].TaxBase.Value = ivaCardAmount;
                                }

                                if (paymentTaxes[f].Tax.Id == Convert.ToInt32(16))    //TaxRetentionCardSourceId
                                {
                                    paymentTaxes[f].TaxBase.Value = Convert.ToDecimal(shipmentCoupon["CouponEndorsementAmount"]) - ivaCardAmount;
                                }
                            }

                            // Calcula el valor del impuesto total
                            for (int f = 0; f < paymentTaxes.Count; f++)
                            {
                                /*TODO LFREIRE hasta implementar el TaxServices
                                if (paymentTaxes[f].Tax.IsRetention == false)
                                {
                                    tax = tax + (paymentTaxes[f].TaxBase.Value * paymentTaxes[f].Rate / 100);
                                }
                                else
                                {
                                    retention = retention + (paymentTaxes[f].TaxBase.Value * paymentTaxes[f].Rate / 100);
                                }
                                */
                            }
                        }

                        collect.Payments.Add(new CreditCardDTO()
                        {
                            Amount = amount,
                            AuthorizationNumber = shipmentCoupon["AuthorizationNumber"].ToString(),
                            CardNumber = shipmentCoupon["CreditCardTypeTaxCode"].ToString(),
                            Holder = shipmentCoupon["IndividualId"].ToString(),
                            ExchangeRate = exchangeRate,
                            Id = 0,
                            IssuingBank = issuingBank,
                            LocalAmount = localAmount,
                            PaymentMethod = paymentMethod,
                            Type = creditCardType,
                            ValidThru = creditCardValidThru,
                            Voucher = shipmentCoupon["VoucherNumber"].ToString(),
                            Status = Convert.ToInt16(PaymentStatus.Active),
                            Taxes = paymentTaxes,
                            Tax = tax,
                            Retention = retention
                        });
                    }

                    #endregion CreditCard

                    #region Transfer

                    if (paymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodDirectConection"]) ||
                        paymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodTransfer"]))
                    {

                        AmountDTO amount = new AmountDTO()
                        {
                            Currency = new CurrencyDTO() { Id = Convert.ToInt32(shipmentCoupon["CurrencyCode"]) },
                            Value = Convert.ToDecimal(shipmentCoupon["CouponEndorsementAmount"]) * Convert.ToDecimal(shipmentCoupon["ExchangeRate"])
                        };

                        ExchangeRateDTO exchangeRate = new ExchangeRateDTO() { BuyAmount = Convert.ToDecimal(shipmentCoupon["ExchangeRate"]) };
                        AmountDTO localAmount = new AmountDTO() { Value = Convert.ToDecimal(shipmentCoupon["CouponEndorsementAmount"]) };
                        BankDTO issuingBank = new BankDTO() { Id = Convert.ToInt32(shipmentCoupon["IssuingCardBankCode"]) };
                        BankAccountPersonDTO recievingAccount = new BankAccountPersonDTO()
                        {
                            Bank = new BankDTO() { Id = Convert.ToInt32(shipmentCoupon["BankCode"]) },
                            Number = shipmentCoupon["AccountBankCode"].ToString()
                        };

                        BankAccountCompanyDTO bankAccountCompany = new BankAccountCompanyDTO();
                        bankAccountCompany.Id = Convert.ToInt32(shipmentCoupon["AccountBankCode"]);

                        collect.Payments.Add(new PaymentModels.TransferDTO()
                        {
                            Amount = amount,
                            Date = automaticDebit.Date.Date,
                            DocumentNumber = GetPersonByIndividualId(Convert.ToInt32(shipmentCoupon["IndividualId"])).IdentificationDocument.Number,
                            ExchangeRate = exchangeRate,
                            Id = 0,
                            IssuerName = GetPersonByIndividualId(Convert.ToInt32(shipmentCoupon["IndividualId"])).FullName,
                            IssuingAccountNumber = _parameterService.GetBankAccountCompany(bankAccountCompany).Number,
                            IssuingBank = issuingBank,
                            LocalAmount = localAmount,
                            PaymentMethod = paymentMethod,
                            ReceivingAccount = recievingAccount,
                            Status = Convert.ToInt16(PaymentStatus.Active)
                        });
                    }

                    #endregion Transfer

                    #region Deposit

                    if (paymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodDepositVoucher"]))
                    {
                        AmountDTO amount = new AmountDTO()
                        {
                            Currency = new CurrencyDTO() { Id = Convert.ToInt32(shipmentCoupon["CurrencyCode"]) },
                            Value = Convert.ToDecimal(shipmentCoupon["CouponEndorsementAmount"]) * Convert.ToDecimal(shipmentCoupon["ExchangeRate"])
                        };

                        ExchangeRateDTO exchangeRate = new ExchangeRateDTO() { BuyAmount = Convert.ToDecimal(shipmentCoupon["ExchangeRate"]) };
                        AmountDTO localAmount = new AmountDTO() { Value = Convert.ToDecimal(shipmentCoupon["CouponEndorsementAmount"]) };

                        BankAccountCompanyDTO recievingAccount = new BankAccountCompanyDTO()
                        {
                            Bank = new BankDTO() { Id = Convert.ToInt32(shipmentCoupon["BankCode"]) },
                            Number = shipmentCoupon["AccountBankCode"].ToString()
                        };

                        collect.Payments.Add(new DepositVoucherDTO()
                        {
                            Amount = amount,
                            Date = automaticDebit.Date.Date,
                            DepositorName = GetPersonByIndividualId(Convert.ToInt32(shipmentCoupon["IndividualId"])).FullName,
                            ExchangeRate = exchangeRate,
                            Id = 0,
                            LocalAmount = localAmount,
                            PaymentMethod = paymentMethod,
                            ReceivingAccount = recievingAccount,
                            VoucherNumber = shipmentCoupon["VoucherNumber"].ToString(),
                            Status = Convert.ToInt16(PaymentStatus.Active)
                        });
                    }

                    #endregion Deposit

                    #region Retention

                    if (paymentMethod.Id == Convert.ToInt32(ConfigurationManager.AppSettings["ParamPaymentMethodRetentionReceipt"]))
                    {

                        AmountDTO amount = new AmountDTO()
                        {
                            Currency = new CurrencyDTO() { Id = Convert.ToInt32(shipmentCoupon["CurrencyCode"]) },
                            Value = Convert.ToDecimal(shipmentCoupon["CouponEndorsementAmount"]) * Convert.ToDecimal(shipmentCoupon["ExchangeRate"])
                        };
                        ExchangeRateDTO exchangeRate = new ExchangeRateDTO() { BuyAmount = Convert.ToDecimal(shipmentCoupon["ExchangeRate"]) };
                        AmountDTO localAmount = new AmountDTO() { Value = Convert.ToDecimal(shipmentCoupon["CouponEndorsementAmount"]) };


                        collect.Payments.Add(new RetentionReceiptDTO()
                        {
                            Amount = amount,
                            AuthorizationNumber = shipmentCoupon["AuthorizationNumber"].ToString(),
                            BillNumber = shipmentCoupon["CollectCode"].ToString(),
                            Date = automaticDebit.Date.Date,
                            ExchangeRate = exchangeRate,
                            Id = 0,
                            LocalAmount = localAmount,
                            PaymentMethod = paymentMethod,
                            SerialNumber = shipmentCoupon["SerialNumber"].ToString(),
                            SerialVoucherNumber = shipmentCoupon["VoucherNumber"].ToString(),
                            VoucherNumber = shipmentCoupon["VoucherNumber"].ToString(),
                            Status = Convert.ToInt16(PaymentStatus.Active)
                        });
                    }

                    #endregion Retention

                    #endregion PaymentMethodType

                    #endregion Payment

                    #region Imputation

                    ApplicationDTO imputation = new ApplicationDTO()
                    {
                        RegisterDate = DateTime.Now,
                        ModuleId = (int)ApplicationTypes.Collect,
                        Id = 0,
                        UserId = automaticDebit.UserId
                    };
                    imputation.ApplicationItems = new List<TransactionTypeDTO>();

                    PremiumReceivableTransactionDTO premiumReceivableTransaction = new PremiumReceivableTransactionDTO();
                    premiumReceivableTransaction.Id = 0;
                    premiumReceivableTransaction.PremiumReceivableItems = new List<PremiumReceivableTransactionItemDTO>();

                    PremiumReceivableTransactionItemDTO premiumReceivableTransactionItem = new PremiumReceivableTransactionItemDTO();
                    premiumReceivableTransactionItem.Policy = new PolicyDTO();
                    premiumReceivableTransactionItem.Policy.Id = Convert.ToInt32(shipmentCoupon["PolicyId"]);
                    premiumReceivableTransactionItem.Policy.Endorsement = new EndorsementDTO()
                    {
                        Id = Convert.ToInt32(shipmentCoupon["EndorsementId"]),
                    };
                    premiumReceivableTransactionItem.Policy.DefaultBeneficiaries = new List<BeneficiaryDTO>()
                    {
                        new BeneficiaryDTO() { IndividualId = Convert.ToInt32(shipmentCoupon["IndividualId"]) }
                    };

                    premiumReceivableTransactionItem.Policy.ExchangeRate = new ExchangeRateDTO()
                    {
                        Currency = new CurrencyDTO() { Id = Convert.ToInt32(shipmentCoupon["CurrencyCode"]) },
                        SellAmount = Convert.ToDecimal(shipmentCoupon["ExchangeRate"])
                    };
                    premiumReceivableTransactionItem.Policy.PayerComponents = new List<PayerComponentDTO>()
                    {
                        new PayerComponentDTO()
                        {
                            Amount = Convert.ToDecimal(shipmentCoupon["CouponEndorsementAmount"]),
                            BaseAmount = Convert.ToDecimal(shipmentCoupon["PrimeAmount"])
                        }
                    };
                    premiumReceivableTransactionItem.Policy.PaymentPlan = new PaymentPlanDTO()
                    {
                        Quotas = new List<QuotaDTO>()
                        {
                            new QuotaDTO() { Number = Convert.ToInt32(shipmentCoupon["SerialNumber"]) }
                        }
                    };


                    premiumReceivableTransactionItem.DeductCommission = new AmountDTO() { Value = 0 }; //no se graba comisiones

                    premiumReceivableTransaction.PremiumReceivableItems.Add(premiumReceivableTransactionItem);
                    imputation.ApplicationItems.Add(premiumReceivableTransaction);

                    #endregion Imputation

                    int technicalTransaction = (int)_commonService.GetParameterByParameterId(Convert.ToInt32(ConfigurationManager.AppSettings["TransactionNumber"])).NumberParameter;

                    TransactionDTO transaction = new TransactionDTO() { Id = 0, TechnicalTransaction = technicalTransaction };
                    collect.Transaction = transaction;

                    CollectApplicationDTO collectImputation = new CollectApplicationDTO()
                    {
                        Collect = collect,
                        Application = imputation
                    };

                    // Se graba y aplica recibo
                    collectImputation = _collectService.SaveCollectImputation(collectImputation, collectControls.Id != 0 ? collectControls.Id : 0, true);

                    // Actualización de datos de cupon
                    using (Context context = new Context())
                    {
                        UpdateShipmentCoupon(Convert.ToString(shipmentCoupon["ShipmentCode"]), Convert.ToInt32(shipmentCoupon["PolicyId"]), Convert.ToInt32(shipmentCoupon["EndorsementId"]), collectImputation.Collect.Id);
                        UpdateShipment(Convert.ToString(shipmentCoupon["ShipmentCode"]), Convert.ToDecimal(shipmentCoupon["CouponEndorsementAmount"]));
                    }
                }

                newAutomaticDebit.Id = 0;
                newAutomaticDebit.Description = "";
            }

            return newAutomaticDebit;
        }

        #endregion

        #region Update

        /// <summary>
        /// UpdateAutomaticDebit
        /// </summary>
        /// <param name="automaticDebit"></param>
        /// <returns></returns>
        public AutomaticDebit UpdateAutomaticDebit(AutomaticDebit automaticDebit)
        {
            AutomaticDebit newAutomaticDebit = new AutomaticDebit();
            var parameters = new NameValue[12];

            if (automaticDebit.Coupons[0].Policy.Id == 0)
            {
                parameters[0] = new NameValue("SHIPMENT_ID", (automaticDebit.BankNetwork.Description == "") ? null : automaticDebit.BankNetwork.Description);
                parameters[1] = new NameValue("VALIDATE", 0);
                parameters[2] = new NameValue("OUTPUT_MESSAGE", null);
                parameters[3] = new NameValue("CARD_ACCOUNT_NUMBER", (automaticDebit.Description == "") ? null : automaticDebit.Description);
                parameters[4] = new NameValue("RECEIPT_ACCOUNT_NUMBER", (automaticDebit.Coupons[0].Policy.Branch.Description == "") ? null : automaticDebit.Coupons[0].Policy.Branch.Description);
                parameters[5] = new NameValue("REJECTION_CODE", (automaticDebit.Coupons[0].Policy.ExchangeRate.Currency.Description == "") ? null : automaticDebit.Coupons[0].Policy.ExchangeRate.Currency.Description);
                parameters[6] = new NameValue("APPLICATION_DATE", (automaticDebit.Date == Convert.ToDateTime("01/01/0001 0:00:00")) ? null : automaticDebit.Date.ToString("dd/MM/yyyy"));
                parameters[7] = new NameValue("BANK_PREMIUM_LOCAL_AMOUNT", (automaticDebit.Coupons[0].Policy.BillingGroup.Description == "") ? null : automaticDebit.Coupons[0].Policy.BillingGroup.Description);
                parameters[8] = new NameValue("DEFAULT_APPLICATION_DATE", (automaticDebit.Date == Convert.ToDateTime("01/01/0001 0:00:00")) ? null : automaticDebit.Date.ToString("dd/MM/yyyy"));
                parameters[9] = new NameValue("AUTHORIZATION_NUMBER", (automaticDebit.Coupons[0].Policy.Prefix.Description == "") ? null : automaticDebit.Coupons[0].Policy.Prefix.Description);
                parameters[10] = new NameValue("DOCUMENT_NUMBER", (automaticDebit.Coupons[0].Policy.PolicyType.Description == "") ? null : automaticDebit.Coupons[0].Policy.PolicyType.Description);
                parameters[11] = new NameValue("PRENOTIFICATION", automaticDebit.Coupons[0].Policy.Prefix.Id);

                try
                {
                    DataTable result;
                    using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                    {
                        result = dynamicDataAccess.ExecuteSPDataTable("ACC.COLLECT_COUPONS", parameters);
                    }

                    if (result != null && result.Rows.Count > 0)
                    {
                        foreach (DataRow arrayItem in result.Rows)
                        {
                            newAutomaticDebit.Id = 0;
                            newAutomaticDebit.Description = arrayItem[0].ToString();
                        }
                    }
                    else
                    {
                        newAutomaticDebit.Id = -1;
                        newAutomaticDebit.Description = "No existe datos para procesar";
                    }
                }
                catch (BusinessException ex)
                {
                    newAutomaticDebit.Id = -1;
                    newAutomaticDebit.Description = ex.Message;
                }
            }
            else
            {
                parameters = new NameValue[3];

                parameters[0] = new NameValue("BANK_NETWORK_ID", automaticDebit.BankNetwork.Id);
                parameters[1] = new NameValue("LOT_NUMBER", (automaticDebit.BankNetwork.Description == "") ? null : automaticDebit.BankNetwork.Description);
                parameters[2] = new NameValue("USER", automaticDebit.Coupons[0].Policy.Branch.Id);

                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    dynamicDataAccess.ExecuteSPDataTable("ACC.BANK_RESPONSE_PROCESS", parameters);
                }

                newAutomaticDebit.Id = 0;
                newAutomaticDebit.Description = "";
            }

            return newAutomaticDebit;
        }

        #endregion

        #region Delete

        /// <summary>
        /// DeleteAutomaticDebit
        /// </summary>
        /// <param name="automaticDebit"></param>
        /// <returns></returns>
        public bool DeleteAutomaticDebit(AutomaticDebit automaticDebit)
        {
            return true;
        }

        #endregion

        #region Get

        /// <summary>
        /// GetAutomaticDebit
        /// </summary>
        /// <param name="automaticDebit"></param>
        /// <returns></returns>
        public AutomaticDebit GetAutomaticDebit(AutomaticDebit automaticDebit)
        {
            return new AutomaticDebit();
        }

        /// <summary>
        /// GetAutomaticDebits
        /// </summary>
        /// <param name="automaticDebit"></param>
        /// <returns></returns>
        public List<AutomaticDebit> GetAutomaticDebits(AutomaticDebit automaticDebit)
        {
            int rows;
            List<AutomaticDebit> automaticDebitList = new List<AutomaticDebit>();

            try
            {
                string lotNumber = automaticDebit.BankNetwork.Id.ToString() + automaticDebit.Date.ToString("yyyyMMddHHmm");
                var parameters = new NameValue[7];

                // 3. Opción Prenotificación
                if (automaticDebit.BankNetwork.RetriesNumber == 3)
                {
                    parameters[0] = new NameValue("BRANCH_CD_ENTRY", null);
                    parameters[1] = new NameValue("NETWORK_ID_ENTRY", automaticDebit.BankNetwork.Id);
                    parameters[2] = new NameValue("SHIPPING_DATE", automaticDebit.Date);
                    parameters[3] = new NameValue("SHIPPING_DATE_BANK", Convert.ToDateTime(automaticDebit.AutomaticDebitStatus.Description));
                    parameters[4] = new NameValue("USER_NAME", automaticDebit.UserId);
                    parameters[5] = new NameValue("PROCESS_TYPE_ID", automaticDebit.AutomaticDebitStatus.Id);
                    parameters[6] = new NameValue("SHIPMENT_CD", lotNumber);

                    if (automaticDebit.Id == 0)
                    {
                        using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                        {
                            dynamicDataAccess.ExecuteSPDataTable("ACC.GEN_SHIPMENT", parameters);
                        }
                    }
                    else if (automaticDebit.Id == 1)
                    {
                        ObjectCriteriaBuilder shipmentFilter = new ObjectCriteriaBuilder();
                        shipmentFilter.PropertyEquals(Entities.ValidatedCoupon.Properties.ShipmentCode, Convert.ToDecimal(automaticDebit.Description));
                        BusinessCollection shipmentCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(Entities.ValidatedCoupon), shipmentFilter.GetPredicate()));

                        if (shipmentCollection.Count > 0)
                        {
                            int rowNumber = 0;
                            foreach (Entities.ValidatedCoupon validatedCoupon in shipmentCollection.OfType<Entities.ValidatedCoupon>())
                            {
                                rowNumber++;
                                AutomaticDebit automaticDebits = new AutomaticDebit();
                                automaticDebits.Date = Convert.ToDateTime(validatedCoupon.ExpirationDate);                      //fec_venc

                                automaticDebits.BankNetwork = new BankNetwork()
                                {
                                    Description = validatedCoupon.AccountNumberSource.ToString()                               //nombre_ori
                                };
                                automaticDebits.Description = validatedCoupon.DocumentType.ToString();                          //tipo_doc

                                Policy policy = new Policy();
                                policy.Prefix = new Prefix()
                                {
                                    Id = Convert.ToInt32(validatedCoupon.PaymentId),                                            //ind_conducto
                                    Description = validatedCoupon.PaymentNum.ToString()                                         //nro_comprobante
                                };

                                policy.Holder = new Holder()
                                {
                                    IndividualId = Convert.ToInt32(validatedCoupon.IndividualId),                               //cod_aseg
                                    Name = validatedCoupon.DocumentNum.ToString(),                                              //nro_doc	varchar

                                    IndividualType = IndividualType.Company,
                                    CustomerType = CustomerType.Individual,
                                };

                                policy.DefaultBeneficiaries = new List<Beneficiary>()
                                {
                                    new Beneficiary()
                                    {
                                        IndividualId = Convert.ToInt32(validatedCoupon.PayerIndCode),                           //cod_ind_pagador
                                        Name = validatedCoupon.HolderNameService.ToString(),                                    //nom_titular_servicio
                                        BeneficiaryTypeDescription = "0",     
                                        CustomerType = CustomerType.Individual,
                                    }
                                };

                                policy.PaymentPlan = new PaymentPlan()
                                {
                                    Id = Convert.ToInt32(validatedCoupon.IdCardNo),                                             //nro_lote
                                    Description = validatedCoupon.AccountCardNumber.ToString(),                                 //nro_cta_tarjeta
                                    Quotas = new List<Quota>()
                                    {
                                        new Quota()
                                        {
                                            Amount = Convert.ToDecimal(validatedCoupon.PrimeAmount),                            //imp_premio_me
                                            Percentage = Convert.ToDecimal(validatedCoupon.BaseAmountTax)                       //imp_base_iva 
                                        }
                                    }
                                };

                                policy.CurrentFrom = Convert.ToDateTime(validatedCoupon.PresentationDate);                      //fec_presentacion
                                policy.CurrentTo = Convert.ToDateTime(validatedCoupon.ShippingDate);                            //fec_envio

                                policy.BillingGroup = new BillingGroup()
                                {
                                    Description = validatedCoupon.SourceName.ToString(),                                        //nombre_ori
                                    Id = Convert.ToInt32(validatedCoupon.VoucherNumber)                                         //nro_cuota
                                };


                                policy.PayerComponents = new List<PayerComponent>()
                                {
                                    new PayerComponent()
                                    {
                                        Amount = Convert.ToDecimal(validatedCoupon.ShipmentNumber),                               //id_envio 
                                        BaseAmount = Convert.ToDecimal(validatedCoupon.AmountTax),                                //imp_iva_me
                                        Coverage = new Coverage()
                                        {
                                            AccumulatedDeductAmount = Convert.ToDecimal(validatedCoupon.AmountBank),              //imp_premio_bco_me	numeric
                                            CurrentFrom = Convert.ToDateTime(validatedCoupon.PresentationDateDetail),             //fec_presentacion_det 
                                            CurrentTo = Convert.ToDateTime(validatedCoupon.ApplicationDate),                      //fec_aplicacion
                                            CoverNum = rowNumber,                                                                 //Records
                                            CoverStatusName = validatedCoupon.ToString(),                                         //nro_cuenta_ori
                                            Id = Convert.ToInt32(validatedCoupon.AccountTypeReceivingCode),                       //tipo_cuenta_rec
                                            
                                            EndorsementType = UnderwritingServices.Enums.EndorsementType.Emission,
                                            CoverStatus = UnderwritingServices.Enums.CoverageStatusType.Original,
                                            CoverageOriginalStatus = UnderwritingServices.Enums.CoverageStatusType.Original,
                                            FirstRiskType = UnderwritingServices.Enums.FirstRiskType.None,
                                            RateType = Services.UtilitiesServices.Enums.RateType.Percentage,
                                            CalculationType =  Services.UtilitiesServices.Enums.CalculationType.Prorate,
                                            InsuredObject = new InsuredObject()
                                            {
                                                ParametrizationStatus = Services.UtilitiesServices.Enums.ParametrizationStatus.Original,
                                            },

                                        },
                                        Component = new Component()
                                        {
                                            ComponentType = UnderwritingServices.Enums.ComponentType.Premium,
                                            ComponentClass = UnderwritingServices.Enums.ComponentClassType.HardComponent,
                                        },

                                        Id = Convert.ToInt32(validatedCoupon.SummaryShippment),                                   //nro_lote_sumario

                                    }
                                };

                                policy.PolicyType = new PolicyType()
                                {
                                    Description = validatedCoupon.ReceivingName.ToString(),                                       //nombre_rec
                                    Id = Convert.ToInt32(validatedCoupon.SummarySequenceNumber),                                  //nro_secuencia_sumario
                                };

                                policy.Branch = new Branch()
                                {
                                    Description = "0",                                                                             //rfc_curp_rec
                                    Id = Convert.ToInt32(validatedCoupon.AccountType),                                             //tipo_cta	
                                    SalePoints = new List<SalePoint>()
                                    {
                                        new SalePoint() { Description = validatedCoupon.LegendOrigin.ToString() }                 //ref_leyenda_ori 
                                    },
                                    SmallDescription = validatedCoupon.IssuingService.ToString(),                                 //ref_servicio_emisor
                                };

                                policy.ExchangeRate = new ExchangeRate()
                                {
                                    Currency = new Currency()
                                    {
                                        Description = validatedCoupon.SerialNumber.ToString(),                                    //nro_serie	varchar
                                        Id = Convert.ToInt32(validatedCoupon.NumberSource)                                        //ref_num_ori
                                    }
                                };

                                List<Coupon> couponList = new List<Coupon>();
                                Coupon coupon = new Coupon();
                                coupon.Id = Convert.ToInt32(validatedCoupon.CouponNumber);                                        //nro_cupon
                                coupon.CouponStatus = new CouponStatus()
                                {
                                    Id = Convert.ToInt32(validatedCoupon.CouponIndex),                                            //ind_cupon
                                    Description = validatedCoupon.CouponStatusCode.ToString(),                                    //cod_rechazo
                                    CouponStatusType = AUTMOD.CouponStatusTypes.Rejected,
                                    SmallDescription = validatedCoupon.DocumentNum.ToString()                                     //nro_pol 
                                };

                                coupon.Policy = policy;
                                couponList.Add(coupon);
                                automaticDebits.Coupons = couponList;
                                automaticDebitList.Add(automaticDebits);

                            }
                        }
                    }
                }
                // 1. Opción generar txt y carga dropdown: Nro. de Lote
                if (automaticDebit.BankNetwork.RetriesNumber == 1)
                {
                    automaticDebitList = new List<AutomaticDebit>();
                    AutomaticDebit AutomaticDebit = new AutomaticDebit();

                    /*
                    1. consulta de cabecera de débitos automáticos
                    2. consulta de estados (cabecera de débitos automáticos) 
                    3. consulta la prenotificacion de la red
                    */

                    if (automaticDebit.Id == 3)
                    {
                        AutomaticDebit.Description = _bankNetworkDAO.GetBankNetworkById(automaticDebit.BankNetwork.Id).RequiresNotification ? "-1" : "0";
                        automaticDebitList.Add(AutomaticDebit);
                    }
                    if (automaticDebit.Id == 2)
                    {
                        List<Array> shipmentStatus = _automaticDebitStatusDAO.GetAutomaticDebitStatus();

                        if (shipmentStatus.Count > 0)
                        {
                            foreach (Array item in shipmentStatus)
                            {
                                AutomaticDebit = new AutomaticDebit();
                                AutomaticDebit.Id = Convert.ToInt32(item.GetValue(0));
                                AutomaticDebit.Description = item.GetValue(1).ToString();
                                AutomaticDebit.AutomaticDebitStatus = new AutomaticDebitStatus();
                                AutomaticDebit.AutomaticDebitStatus.Id = Convert.ToInt32(0);

                                AutomaticDebit.Date = Convert.ToDateTime(DateTime.Now);
                                AutomaticDebit.BankNetwork = new BankNetwork()
                                {
                                    Id = 0,
                                    Description = ""
                                };
                                AutomaticDebit.Coupons = new List<Coupon>();
                                automaticDebitList.Add(AutomaticDebit);
                            }
                        }
                    }
                    if (automaticDebit.Id == 1 || automaticDebit.Id == 3)
                    {
                        ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                        criteriaBuilder.PropertyEquals(Entities.BankNetwork.Properties.BankNetworkId, automaticDebit.BankNetwork.Id);
                        criteriaBuilder.And();
                        criteriaBuilder.PropertyEquals(Entities.Shipment.Properties.ProcessDate, FormatDateTime(automaticDebit.Date.ToShortDateString()));

                        UIView lotNumbersView = _dataFacadeManager.GetDataFacade().GetView("ShipmentAutomaticDebitStatusView", criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out rows);

                        // Add New row for return total records
                        if (lotNumbersView.Rows.Count > 0)
                        {
                            lotNumbersView.Columns.Add("rows", typeof(int));
                            lotNumbersView.Rows[0]["rows"] = lotNumbersView.Rows.Count;
                        }

                        foreach (DataRow dataRow in lotNumbersView)
                        {
                            AutomaticDebit = new AutomaticDebit();
                            if (automaticDebit.Id == 3)
                            {
                                AutomaticDebit.Description = dataRow["AutomaticDebitStatusCode"].ToString();
                                automaticDebitList.Add(AutomaticDebit);
                            }
                            else
                            {
                                AutomaticDebit.Id = Convert.ToInt32(dataRow["AutomaticDebitStatusCode"]);
                                AutomaticDebit.Description = dataRow["ShipmentId"].ToString();
                                AutomaticDebit.AutomaticDebitStatus = new AutomaticDebitStatus();
                                AutomaticDebit.AutomaticDebitStatus.Id = Convert.ToInt32(dataRow["AutomaticDebitStatusCode"]);

                                AutomaticDebit.Date = Convert.ToDateTime(dataRow["ProcessDate"]);
                                AutomaticDebit.BankNetwork = new BankNetwork()
                                {
                                    Id = 0,
                                    Description = ""
                                };
                                AutomaticDebit.Coupons = new List<Coupon>();
                                automaticDebitList.Add(AutomaticDebit);
                            }
                        }
                    }
                }

                // 2. Generación de archivo excel
                if (automaticDebit.BankNetwork.RetriesNumber == 2)
                {
                    //EntUnder.
                    string reasonRejection = "";

                    automaticDebitList = new List<AutomaticDebit>();

                    ObjectCriteriaBuilder filterCouponExcel = new ObjectCriteriaBuilder();
                    filterCouponExcel.PropertyEquals(Entities.ShipmentCoupon.Properties.ShipmentCode, Convert.ToDecimal(lotNumber));
                    filterCouponExcel.And();
                    filterCouponExcel.Property(Entities.ShipmentCoupon.Properties.EndorsementId);
                    filterCouponExcel.Distinct();
                    filterCouponExcel.Constant(0);
                    filterCouponExcel.And();
                    filterCouponExcel.OpenParenthesis();
                    filterCouponExcel.PropertyEquals(Entities.ShipmentCoupon.Properties.StatusCode, 1);
                    filterCouponExcel.Or();
                    filterCouponExcel.PropertyEquals(Entities.ShipmentCoupon.Properties.StatusCode, 3);
                    filterCouponExcel.CloseParenthesis();
                    filterCouponExcel.And();
                    filterCouponExcel.Property(Entities.CouponStatus.Properties.CouponStatusBankCode);
                    filterCouponExcel.Distinct();
                    filterCouponExcel.Constant("PC");
                    filterCouponExcel.And();
                    filterCouponExcel.PropertyEquals(Entities.BankNetworkStatus.Properties.BankNetworkCode, automaticDebit.BankNetwork.Id);

                    UIView getCollectedCuponsExcel = _dataFacadeManager.GetDataFacade().GetView("ShipmentCouponPrefixView", filterCouponExcel.GetPredicate(), null, 0, -1, null, false, out rows);
                    int total = 1;

                    if (getCollectedCuponsExcel.Count > 0)
                    {
                        foreach (DataRow dataRow in getCollectedCuponsExcel)
                        {
                            AutomaticDebit automaticDebits = new AutomaticDebit();
                            List<Risk> risks = new List<Risk>();
                            Risk risk = new Risk();
                            risk.Id = total;

                            automaticDebits.BankNetwork = new BankNetwork()
                            {
                                Id = Convert.ToInt32(dataRow["PrefixCode"].ToString()),                            //Sucursal
                                Description = dataRow["PrefixDescription"].ToString(),                             //Ramo
                            };

                            List<Coupon> couponList = new List<Coupon>();
                            Coupon coupon = new Coupon();
                            coupon.CouponStatus = new CouponStatus()
                            {
                                Id = 0,                                                                             //Sufijo / aaaa_endoso
                                RetriesNumber = Convert.ToInt32(dataRow["BranchCode"]),                             //cod.ramo
                                Description = dataRow["EndorsementId"].ToString(),                                  //Id. SISE
                                CouponStatusType = AUTMOD.CouponStatusTypes.Rejected,
                                SmallDescription = Convert.ToInt32(dataRow["EndorsementId"]) == 1 ? "SI" : "NO",    //Temporario
                                ExternalDescription = dataRow["CouponStatusCode"].ToString()                        //Cod Rechazo
                            };


                            if (dataRow["CouponStatusBankCode"].ToString() == "PC")
                            {
                                reasonRejection = "Pol Cancelada";
                            }
                            else if (dataRow["CouponStatusBankCode"].ToString() == "PI")
                            {
                                reasonRejection = "Pol. Inconsistente";
                            }
                            else if (dataRow["CouponStatusBankCode"].ToString() == "NN")
                            {
                                reasonRejection = "Pol. Sin Nro de Cuotas***";
                            }
                            else if (dataRow["CouponStatusBankCode"].ToString() == "IM")
                            {
                                reasonRejection = "Premio_me > Tope_envio";
                            }
                            else if (dataRow["CouponStatusBankCode"].ToString() == "LE")
                            {
                                reasonRejection = "Premio_me > Tope_envio";
                            }
                            else if (dataRow["CouponStatusBankCode"].ToString() == "ET")
                            {
                                reasonRejection = "|Error al Trasladar Poliza a Emision";
                            }
                            else if (dataRow["CouponStatusBankCode"].ToString() == "RT")
                            {
                                reasonRejection = "|Error al Trasladar Poliza a Emision - Reintento";
                            }
                            else if (dataRow["CouponStatusBankCode"].ToString() == "PE")
                            {
                                reasonRejection = "Sucusal Ramo Poliza Endoso ya existe";
                            }

                            Policy policy = new Policy()
                            {
                                DocumentNumber = Convert.ToDecimal(dataRow["PolicyDocumentNumber"]),        //Póliza
                                Id = Convert.ToInt32(dataRow["StatusCode"]),                                //Cod.Estado
                                Endorsement = new Endorsement()
                                {
                                    Number = Convert.ToInt32(dataRow["DocumentNumber"]),                    //N° Endoso
                                },
                                PaymentPlan = new PaymentPlan()
                                {
                                    Description = reasonRejection == "" ? (dataRow["CouponStatusDescription"] == null ? "" : dataRow["CouponStatusDescription"].ToString()) : reasonRejection, //Motivo Rechazo
                                    Id = total
                                },
                                PayerComponents = new List<PayerComponent>()
                                {
                                    new PayerComponent()
                                    {
                                        Amount = Convert.ToDecimal(dataRow["TotalAmount"]),                    //Total 
                                    }
                                }
                            };

                            coupon.Policy = policy;
                            couponList.Add(coupon);
                            automaticDebits.Coupons = couponList;
                            automaticDebitList.Add(automaticDebits);
                            total++;
                        }
                    }
                }
                // 4. Obtención errores carga respuesta banco
                if (automaticDebit.BankNetwork.RetriesNumber == 4)
                {
                    automaticDebitList = new List<AutomaticDebit>();

                    List<Array> pendingProcessList = GetPendingProcessCollection(automaticDebit);

                    if (pendingProcessList.Count > 0)
                    {
                        foreach (Array item in pendingProcessList)
                        {
                            AutomaticDebit newAutomaticDebit = new AutomaticDebit();
                            List<Risk> risks = new List<Risk>();
                            Risk risk = new Risk();

                            newAutomaticDebit.BankNetwork = new BankNetwork()
                            {
                                Id = Convert.ToInt32(item.GetValue(1)),                                         //BANK_NETWORK_ID
                                Description = item.GetValue(2).ToString(),                                      //LOT_NUMBER
                            };

                            List<Coupon> couponList = new List<Coupon>();
                            Coupon coupon = new Coupon();
                            coupon.CouponStatus = new CouponStatus()
                            {
                                Id = Convert.ToInt32(item.GetValue(3)),                                          //LINE_NUMBER
                                Description = item.GetValue(13).ToString(),                                      //DESCRIPTION_ERROR
                                CouponStatusType = AUTMOD.CouponStatusTypes.Rejected,
                                SmallDescription = item.GetValue(14).ToString()                                  //PROCESS_STATUS
                            };

                            Policy policy = new Policy()
                            {
                                Id = 0                                                                          //IS_PRENOTIFICACION
                            };

                            coupon.Policy = policy;
                            couponList.Add(coupon);
                            newAutomaticDebit.Coupons = couponList;
                            newAutomaticDebit.Id = Convert.ToInt32(item.GetValue(0));      //LOG_BANK_RESPONSE_ID 
                            newAutomaticDebit.Description = item.GetValue(13).ToString();  //DESCRIPTION_ERROR
                            automaticDebitList.Add(newAutomaticDebit);
                        }
                    }
                }
                // 5. Obtención procesos pendientes débito
                if (automaticDebit.BankNetwork.RetriesNumber == 5)
                {
                    automaticDebitList = new List<AutomaticDebit>();

                    ObjectCriteriaBuilder pendingProcessFilter = new ObjectCriteriaBuilder();

                    pendingProcessFilter.PropertyEquals(Entities.LogAutomaticDebitView.Properties.BankNetworkId, automaticDebit.BankNetwork.Id);
                    pendingProcessFilter.And();
                    pendingProcessFilter.PropertyEquals(Entities.LogAutomaticDebitView.Properties.UserCode, automaticDebit.UserId);

                    if (automaticDebit.Id == 0)
                    {
                        pendingProcessFilter.And();
                        pendingProcessFilter.Property(Entities.LogAutomaticDebitView.Properties.ProcessTypeId);
                        pendingProcessFilter.Distinct();
                        pendingProcessFilter.Constant(Convert.ToInt32(ProcessType.GenerationCoupon));
                    }

                    if (automaticDebit.Id == 1 || automaticDebit.Id == 6)
                    {
                        pendingProcessFilter.And();
                        pendingProcessFilter.PropertyEquals(Entities.LogAutomaticDebitView.Properties.ProcessTypeId, automaticDebit.Id);
                    }

                    UIView logAutomaticDebits = _dataFacadeManager.GetDataFacade().GetView("LogAutomaticDebitView",
                                                pendingProcessFilter.GetPredicate(), null, 0, -1, null, false, out rows);

                    if (logAutomaticDebits.Count > 0)
                    {
                        int rowNumber = 0;

                        foreach (DataRow dataRow in logAutomaticDebits)
                        {
                            rowNumber++;
                            AutomaticDebit newAutomaticDebit = new AutomaticDebit();

                            newAutomaticDebit.BankNetwork = new BankNetwork()
                            {
                                Id = Convert.ToInt32(dataRow["BankNetworkId"]),                                     //BANK_NETWORK_ID
                                Description = dataRow["LotNumber"].ToString(),                                      //LOT_NUMBER
                            };

                            List<Coupon> couponList = new List<Coupon>();
                            Coupon coupon = new Coupon();
                            coupon.CouponStatus = new CouponStatus()
                            {
                                Id = Convert.ToInt32(dataRow["ProcessTypeId"]),                                     //PROCESS_TYPE_ID
                                RetriesNumber = Convert.ToInt32(dataRow["RecordsNumber"]),                          //RECORDS_NUMBER
                                Description = dataRow["RecordsProcessed"].ToString(),                               //RECORDS_PROCESSED
                                CouponStatusType = AUTMOD.CouponStatusTypes.Rejected,
                                SmallDescription = dataRow["RecordsFailed"].ToString()                              //RECORDS_FAILED
                            };

                            Policy policy = new Policy()
                            {
                                CurrentFrom = Convert.ToDateTime(dataRow["StartDate"].ToString()),    //START_DATE
                                CurrentTo = DBNull.ReferenceEquals(dataRow["EndDate"], DBNull.Value) ? Convert.ToDateTime("01/01/0001 0:00:00") :
                                           Convert.ToDateTime(dataRow["EndDate"].ToString()),        //END_DATE
                                DocumentNumber = rowNumber,                                          //Records
                                Id = DBNull.ReferenceEquals(dataRow["Status"], DBNull.Value) ? 1 : Convert.ToInt32(dataRow["Status"]), //STATUS_SHIPMENT
                            };

                            coupon.Policy = policy;
                            couponList.Add(coupon);
                            newAutomaticDebit.Coupons = couponList;
                            newAutomaticDebit.Id = Convert.ToInt32(dataRow["LogAutomaticDebitId"]);     //LOG_AUTOMATIC_DEBIT_ID 
                            newAutomaticDebit.Description = dataRow["Description"].ToString();          //DESCRIPTION
                            automaticDebitList.Add(newAutomaticDebit);
                        }
                    }
                }
            }
            catch (BusinessException Exception)
            {
                AutomaticDebit newAutomaticDebit = new AutomaticDebit();
                newAutomaticDebit.Id = -4;
                newAutomaticDebit.Description = Exception.Message;
                automaticDebitList.Add(newAutomaticDebit);
            }

            return automaticDebitList;
        }

        /// <summary>
        /// GetAutomaticDebitSummary
        /// </summary>
        /// <param name="automaticDebit"></param>
        /// <returns></returns>
        public List<DTOs.AutomaticDebitSummaryDTO> GetAutomaticDebitSummary(AutomaticDebit automaticDebit)
        {
            int rows;
            List<DTOs.AutomaticDebitSummaryDTO> automaticDebitSummaries = new List<DTOs.AutomaticDebitSummaryDTO>();

            ObjectCriteriaBuilder filterCoupon = new ObjectCriteriaBuilder();
            filterCoupon.PropertyEquals(Entities.GeneratedCouponView.Properties.ShipmentCode, automaticDebit.Description);

            UIView couponSummaries = _dataFacadeManager.GetDataFacade().GetView("SummaryCouponView", filterCoupon.GetPredicate(), null, 0, -1, null, false, out rows);

            foreach (DataRow dataRow in couponSummaries)
            {
                automaticDebitSummaries.Add(new DTOs.AutomaticDebitSummaryDTO()
                {
                    Prefix = dataRow["SmallDescription"].ToString(),
                    NetId = automaticDebit.BankNetwork.Id,
                    TotalPremium = Convert.ToDecimal(dataRow["TotalAmount"]),
                    ValueAddedTax = Convert.ToDecimal(dataRow["ValueAddedTax"]),
                    CouponsNumber = Convert.ToInt32(dataRow["CouponNumber"]),
                    AcceptedNumber = Convert.ToInt32(dataRow["Accepted"]),
                    ErrorAcceptedNumber = Convert.ToInt32(dataRow["TaxAccepted"]),
                    ErrorAccepted = Convert.ToDecimal(dataRow["TaxAccepted"]),
                    Rejections = Convert.ToDecimal(dataRow["ErrorRejection"]),
                });
            }

            return automaticDebitSummaries;
        }

        /// <summary>
        /// GetCouponStatusDetail
        /// </summary>
        /// <param name="dateTo"></param>
        /// <param name="dateFrom"></param>
        /// <param name="prefix"></param>
        /// <param name="agent"></param>
        /// <param name="couponStatus"></param>
        /// <returns></returns>
        public List<DTOs.CouponStatusDTO> GetCouponStatusDetail(DateTime dateTo, DateTime dateFrom, Prefix prefix, Agent agent, CouponStatus couponStatus)
        {
            int rows;
            int shipmentStatusId = 0;
            List<DTOs.CouponStatusDTO> couponStatusDTOs = new List<DTOs.CouponStatusDTO>();

            ObjectCriteriaBuilder filterShipmentStatus = new ObjectCriteriaBuilder();
            filterShipmentStatus.Property(Entities.AutomaticDebitStatus.Properties.AutomaticDebitStatusId);
            filterShipmentStatus.GreaterEqual();
            filterShipmentStatus.Constant(0);

            BusinessCollection formatDesignCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(Entities.AutomaticDebitStatus), filterShipmentStatus.GetPredicate()));
            List<DTOs.CouponStatusDTO> CouponStatusDTO = new List<DTOs.CouponStatusDTO>();

            foreach (Entities.AutomaticDebitStatus automaticDebitStatus in formatDesignCollection.OfType<Entities.AutomaticDebitStatus>())
            {
                CouponStatusDTO.Add(new DTOs.CouponStatusDTO()
                {
                    StatusId = automaticDebitStatus.AutomaticDebitStatusId,
                    StatusDescription = automaticDebitStatus.Description,
                });
            }

            shipmentStatusId = CouponStatusDTO.Where(sl => sl.StatusDescription == "APLICADO-FINALIZADO").Select(sl2 => sl2.StatusId).First();
            dateFrom = dateFrom.AddDays(1);

            ObjectCriteriaBuilder filterShipment = new ObjectCriteriaBuilder();
            filterShipment.Property(Entities.Shipment.Properties.ProcessDate);
            filterShipment.GreaterEqual();
            filterShipment.Constant(FormatDateTime(dateTo.ToString("dd/MM/yyyy")));
            filterShipment.And();
            filterShipment.Property(Entities.Shipment.Properties.ProcessDate);
            filterShipment.LessEqual();
            filterShipment.Constant(FormatDateTime(dateFrom.ToString("dd/MM/yyyy")));
            filterShipment.And();
            filterShipment.OpenParenthesis();
            filterShipment.Property(Entities.ShipmentCoupon.Properties.PrefixCode);
            filterShipment.IsNotNull();
            filterShipment.Or();
            filterShipment.Property(Entities.ShipmentCoupon.Properties.PrefixCode);
            filterShipment.Equal();
            filterShipment.Constant(prefix.Id);
            filterShipment.CloseParenthesis();
            filterShipment.And();
            filterShipment.OpenParenthesis();
            filterShipment.Property(Entities.ShipmentCoupon.Properties.IndividualId);
            filterShipment.IsNotNull();
            filterShipment.Or();
            filterShipment.Property(Entities.ShipmentCoupon.Properties.IndividualId);
            filterShipment.Equal();
            filterShipment.Constant(agent.IndividualId);
            filterShipment.CloseParenthesis();

            if (couponStatus.SmallDescription == "C")
            {
                filterShipment.And();
                filterShipment.PropertyEquals(Entities.Shipment.Properties.AutomaticDebitStatusCode, shipmentStatusId);
            }
            else if (couponStatus.SmallDescription == "U")
            {
                filterShipment.And();
                filterShipment.Property(Entities.Shipment.Properties.AutomaticDebitStatusCode);
                filterShipment.Distinct();
                filterShipment.Constant(shipmentStatusId);
            }

            UIView getCollectedCuponsView = _dataFacadeManager.GetDataFacade().GetView("CollectedCouponsView", filterShipment.GetPredicate(), null, 0, -1, null, false, out rows);

            int rowCount = 0;

            foreach (DataRow dataRow in getCollectedCuponsView)
            {
                DTOs.CouponStatusDTO couponsStatusDTO = new DTOs.CouponStatusDTO();

                couponsStatusDTO.Amount = Convert.ToDecimal(0);
                couponsStatusDTO.LocalAmount = Convert.ToDecimal(dataRow["TotalAmount"].ToString());
                couponsStatusDTO.AuthorizationNumber = DBNull.ReferenceEquals(dataRow["AuthorizationNumber"].ToString(), DBNull.Value) ? "" : Convert.ToString(dataRow["AuthorizationNumber"].ToString());
                couponsStatusDTO.BankNetworkId = Convert.ToInt32(dataRow["BankNetworkCode"]);
                couponsStatusDTO.BankNetworkDescription = dataRow["BankNetworkDescription"].ToString();
                couponsStatusDTO.Id = dataRow["ShipmentId"].ToString();
                couponsStatusDTO.InsuredId = Convert.ToInt32(dataRow["IndividualId"]);
                couponsStatusDTO.InsuredDocumentNumber = dataRow["DocumentNumber"].ToString();
                couponsStatusDTO.InsuredName = dataRow["Name"].ToString();
                couponsStatusDTO.PolicyId = Convert.ToInt32(dataRow["PolicyId"]);
                couponsStatusDTO.ReceiptNumber = ReferenceEquals(dataRow["CollectCode"], DBNull.Value) ? "" : Convert.ToString(dataRow["CollectCode"]);
                couponsStatusDTO.StatusId = 0;
                couponsStatusDTO.StatusDescription = ReferenceEquals(dataRow["PolicyId"], DBNull.Value) ? "EMITIDA" : "EN TEMPORALES";
                couponsStatusDTO.StatusResponseId = ReferenceEquals(dataRow["CouponStatusCode"], DBNull.Value) ? "" : Convert.ToString(dataRow["CouponStatusCode"]);
                couponsStatusDTO.StatusResponse = ReferenceEquals(dataRow["CouponStatusDescription"], DBNull.Value) ? "" : Convert.ToString(dataRow["CouponStatusDescription"]);

                couponStatusDTOs.Add(couponsStatusDTO);
                rowCount++;
            }

            return couponStatusDTOs;
        }

        /// <summary>
        /// GetAutomaticDebitsDetail
        /// </summary>
        /// <param name="dateTo"></param>
        /// <param name="dateFrom"></param>
        /// <returns></returns>
        public List<DTOs.AutomaticDebitsDTO> GetAutomaticDebitsDetail(DateTime dateTo, DateTime dateFrom)
        {
            List<DTOs.AutomaticDebitsDTO> automaticDebits = new List<DTOs.AutomaticDebitsDTO>();
            ObjectCriteriaBuilder automaticDebitsFilter = new ObjectCriteriaBuilder();

            try
            {
                automaticDebitsFilter.Property(Entities.CouponsStatusView.Properties.ProcessDate);
                automaticDebitsFilter.GreaterEqual();
                automaticDebitsFilter.Constant(dateFrom);
                automaticDebitsFilter.And();
                automaticDebitsFilter.Property(Entities.CouponsStatusView.Properties.ProcessDate);
                automaticDebitsFilter.LessEqual();
                automaticDebitsFilter.Constant(dateTo);

                BusinessCollection automaticDebitsCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(Entities.CouponsStatusView), automaticDebitsFilter.GetPredicate()));

                if (automaticDebitsCollection.Count > 0)
                {
                    foreach (Entities.CouponsStatusView couponStatus in automaticDebitsCollection.OfType<Entities.CouponsStatusView>())
                    {
                        DTOs.AutomaticDebitsDTO automaticDebitsDTO = new DTOs.AutomaticDebitsDTO();

                        automaticDebitsDTO.BankNetworkId = Convert.ToInt32(couponStatus.BankNetworkCode);
                        automaticDebitsDTO.BankNetworkDescription = couponStatus.Description == null ? "" : couponStatus.Description.ToString();
                        automaticDebitsDTO.BranchId = Convert.ToInt32(couponStatus.BranchCode);
                        automaticDebitsDTO.BranchDescription = couponStatus.BranchDescription == null ? " " : couponStatus.BranchDescription.ToString();
                        automaticDebitsDTO.Id = couponStatus.ShipmentId.ToString();
                        automaticDebitsDTO.ProcessDate = Convert.ToDateTime(couponStatus.ProcessDate).ToString("dd/MM/yyyy HH:mm");
                        automaticDebitsDTO.StatusId = Convert.ToInt32(couponStatus.AutomaticDebitStatusCode);
                        automaticDebitsDTO.StatusDescription = String.IsNullOrEmpty(couponStatus.DebitStatusDescription.ToString()) ? "" : couponStatus.DebitStatusDescription.ToString();
                        automaticDebitsDTO.UserId = Convert.ToInt32(couponStatus.UserCode);
                        automaticDebitsDTO.UserNick = couponStatus.AccountName.ToString();

                        automaticDebits.Add(automaticDebitsDTO);
                    }
                }
            }
            catch (BusinessException Exception)
            {
                throw new BusinessException(Exception.Message);
            }
            return automaticDebits;
        }

        /// <summary>
        /// GetAutomaticDebitStatus
        /// </summary>
        /// <returns></returns>
        public List<AutomaticDebitStatus> GetAutomaticDebitStatus()
        {
            List<AutomaticDebitStatus> automaticDebitStatuses = new List<AutomaticDebitStatus>();
            List<Array> shipmentStatuses = _automaticDebitStatusDAO.GetAutomaticDebitStatus();

            foreach (Array shipmentStatus in shipmentStatuses)
            {
                AutomaticDebitStatus status = new AutomaticDebitStatus();
                status.Id = Convert.ToInt32(shipmentStatus.GetValue(0));
                status.Description = shipmentStatus.GetValue(1).ToString();
                automaticDebitStatuses.Add(status);
            }

            return automaticDebitStatuses;
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// SaveLogBankResponse
        /// Guarda un nuevo rejistro en la tabla.
        /// </summary>
        /// <param name="logBankResponse"></param>
        /// <returns>Array</returns>
        private Array SaveLogBankResponse(Array logBankResponse)
        {
            try
            {
                // Convertir de model a entity
                Entities.LogBankResponse logBankResponseEntity = EntityAssembler.CreateLogBankResponse(logBankResponse);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(logBankResponseEntity);

                return ModelAssembler.CreateLogBankResponse(logBankResponseEntity);
            }
            catch (BusinessException Exception)
            {
                throw new BusinessException(Exception.Message);
            }
        }

        /// <summary>
        /// UpdateShipmentCoupon
        /// </summary>
        /// <param name="shipmentCode"></param>
        /// <param name="policyId"></param>
        /// <param name="endorsementId"></param>
        /// <param name="collectId"></param>
        private void UpdateShipmentCoupon(string shipmentCode, int policyId, int endorsementId, int collectId)
        {
            try
            {
                ObjectCriteriaBuilder filterShipmentNetwork = new ObjectCriteriaBuilder();

                filterShipmentNetwork.PropertyEquals(Entities.ShipmentCoupon.Properties.ShipmentCode, shipmentCode);
                filterShipmentNetwork.And();
                filterShipmentNetwork.PropertyEquals(Entities.ShipmentCoupon.Properties.PolicyId, policyId);
                filterShipmentNetwork.And();
                filterShipmentNetwork.PropertyEquals(Entities.ShipmentCoupon.Properties.EndorsementId, endorsementId);

                BusinessCollection shipmentCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(Entities.ShipmentCoupon), filterShipmentNetwork.GetPredicate()));

                foreach (Entities.ShipmentCoupon shipmentCouponCollection in shipmentCollection.OfType<Entities.ShipmentCoupon>())
                {
                    PrimaryKey primaryKey = Entities.ShipmentCoupon.CreatePrimaryKey(shipmentCouponCollection.CouponNumber, shipmentCouponCollection.CouponIndex);

                    // Realizar las operaciones con los entities utilizando DAF
                    Entities.ShipmentCoupon shipmentCouponEntity = (Entities.ShipmentCoupon)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                    shipmentCouponEntity.StatusCode = 6;        //Aplicado en COUPON_STATUS
                    shipmentCouponEntity.CollectCode = collectId;
                    shipmentCouponEntity.IsTemporal = false;

                    _dataFacadeManager.GetDataFacade().UpdateObject(shipmentCouponEntity);
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateShipment
        /// </summary>
        /// <param name="shipmentId"></param>
        /// <param name="total"></param>
        private void UpdateShipment(string shipmentId, decimal total)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = Entities.Shipment.CreatePrimaryKey(shipmentId);

                // Realizar las operaciones con los entities utilizando DAF
                Entities.Shipment shipmentEntity = (Entities.Shipment)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                shipmentEntity.ImputationDate = DateTime.Now.Date;
                shipmentEntity.AutomaticDebitStatusCode = 6;      //Aplicado
                //shipmentEntity.TotalAmount = total; //Se comento porque actualiza el total de la cabezera con el ultimo registro del valor del detalle shipment_coupon

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(shipmentEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GetPendingProcessCollection
        /// Obtiene procesos pendientes de débito y errores carga respuesta banco
        /// ACC.GET_PENDING_PROCESSES_AUTOMATIC_DEBIT
        /// </summary>
        /// <param name="automaticDebit"></param>
        /// <returns>List<Array/></returns>
        private List<Array> GetPendingProcessCollection(AutomaticDebit automaticDebit)
        {
            List<Array> pendingProcesses = new List<Array>();
            ObjectCriteriaBuilder pendingProcessesFilter = new ObjectCriteriaBuilder();
            BusinessCollection pendingProcessesCollection;

            if (automaticDebit.Id != 7)
            {
                pendingProcessesFilter.PropertyEquals(Entities.LogAutomaticDebit.Properties.BankNetworkId, automaticDebit.BankNetwork.Id);
                pendingProcessesFilter.And();
                pendingProcessesFilter.PropertyEquals(Entities.LogAutomaticDebit.Properties.UserCode, automaticDebit.UserId);
                if (automaticDebit.Id == 1 || automaticDebit.Id == 6)
                {
                    pendingProcessesFilter.And();
                    pendingProcessesFilter.PropertyEquals(Entities.LogAutomaticDebit.Properties.ProcessTypeId, automaticDebit.Id);
                }
                if (automaticDebit.Id == 0)
                {
                    pendingProcessesFilter.And();
                    pendingProcessesFilter.Property(Entities.LogAutomaticDebit.Properties.ProcessTypeId);
                    pendingProcessesFilter.GreaterEqual();
                    pendingProcessesFilter.Constant(2);
                    pendingProcessesFilter.And();
                    pendingProcessesFilter.Property(Entities.LogAutomaticDebit.Properties.ProcessTypeId);
                    pendingProcessesFilter.LessEqual();
                    pendingProcessesFilter.Constant(5);
                }

                pendingProcessesCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(Entities.LogAutomaticDebit), pendingProcessesFilter.GetPredicate()));

                if (pendingProcessesCollection.Count > 0)
                {
                    foreach (Entities.LogAutomaticDebit pendingProcess in pendingProcessesCollection.OfType<Entities.LogAutomaticDebit>())
                    {
                        string[] newPendingProcess = new string[15];
                        newPendingProcess[2] = pendingProcess.BankNetworkId.ToString();
                        newPendingProcess[1] = pendingProcess.LotNumber.ToString();
                        newPendingProcess[3] = pendingProcess.ProcessTypeId.ToString();  //
                        newPendingProcess[13] = "";
                        newPendingProcess[14] = "";
                        newPendingProcess[5] = "";
                        newPendingProcess[12] = "";
                        newPendingProcess[0] = pendingProcess.LogAutomaticDebitId.ToString();
                        newPendingProcess[7] = pendingProcess.RecordsNumber.ToString();
                        newPendingProcess[8] = pendingProcess.RecordsProcessed.ToString();
                        newPendingProcess[4] = pendingProcess.Description.ToString();
                        newPendingProcess[9] = pendingProcess.RecordsFailed.ToString();
                        newPendingProcess[11] = Convert.ToInt32(pendingProcess.Status).ToString();
                        newPendingProcess[6] = pendingProcess.StartDate.ToString();
                        newPendingProcess[10] = Convert.IsDBNull(pendingProcess.EndDate) ? "01/01/0001 0:00:00" : pendingProcess.EndDate.ToString();

                        pendingProcesses.Add(newPendingProcess);
                    }
                }
            }
            else
            {
                pendingProcessesFilter.PropertyEquals(Entities.LogBankResponse.Properties.BankNetworkId, automaticDebit.BankNetwork.Id);
                pendingProcessesFilter.And();
                pendingProcessesFilter.PropertyEquals(Entities.LogBankResponse.Properties.LotNumber, automaticDebit.BankNetwork.Description);
                pendingProcessesFilter.And();
                pendingProcessesFilter.PropertyEquals(Entities.LogBankResponse.Properties.UserCode, automaticDebit.UserId);

                pendingProcessesCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(Entities.LogBankResponse), pendingProcessesFilter.GetPredicate()));

                if (pendingProcessesCollection.Count > 0)
                {
                    foreach (Entities.LogBankResponse pendingProcess in pendingProcessesCollection.OfType<Entities.LogBankResponse>())
                    {
                        String[] newPendingProcess = new String[15];
                        newPendingProcess[2] = pendingProcess.BankNetworkId.ToString();
                        newPendingProcess[1] = pendingProcess.LotNumber.ToString();
                        newPendingProcess[3] = pendingProcess.LineNumber.ToString();  //
                        newPendingProcess[13] = pendingProcess.DescriptionError.ToString();
                        newPendingProcess[14] = pendingProcess.RejectionCode.ToString();  //PROCESS_STATUS
                        newPendingProcess[0] = "";
                        newPendingProcess[7] = "";
                        newPendingProcess[8] = "";
                        newPendingProcess[4] = "";
                        newPendingProcess[9] = "";
                        newPendingProcess[11] = "";
                        newPendingProcess[6] = "";
                        newPendingProcess[10] = "";
                        newPendingProcess[5] = "";
                        newPendingProcess[12] = "";

                        pendingProcesses.Add(newPendingProcess);
                    }
                }
            }

            return pendingProcesses;
        }

        /// <summary>
        /// GetPersonDocumentNumberByIndividualId
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        private Person GetPersonByIndividualId(int individualId)
        {
            return _uniquePersonService.GetPersonByIndividualId(individualId);
        }

        /// <summary>
        /// FormatDateTime
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>DateTime</returns>
        private DateTime FormatDateTime(string dateTime)
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("es-ES");
                IFormatProvider culture = new CultureInfo("es-EC", true);

                return Convert.ToDateTime(dateTime, culture);

            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion
    }
}
