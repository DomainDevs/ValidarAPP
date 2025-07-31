//using System;
//using System.Collections.Generic;
//using System.Linq;

//using Sistran.Core.Application.AccountingServices.EEProvider.Models.Collect;
//using Sistran.Core.AccountingServices.DTOs.Payments;
//using Sistran.Core.AccountingServices.DTOs.Imputations;
//using Sistran.Core.AccountingServices.DTOs.BankAccounts;
//using Sistran.Core.Application.AccountingServices.EEProvider.Models.BankAccounts;
//using Sistran.Core.AccountingServices.DTOs.Search;
//using Sistran.Core.Application.AccountingServices.EEProvider.Models.Payments;
//using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
//using Sistran.Core.AccountingServices.DTOs;
//using Sistran.Core.AccountingServices.EEProvider.Enums;

//namespace Sistran.Core.Application.AccountingServices.EEProvider.Assemblers
//{
//    internal class OldDTOAssembler
//    {

//        #region CollectImputation   

//        /// <summary>
//        /// CreateCollectImputation
//        /// </summary>
//        /// <param name="collectImputation"></param>
//        /// <returns>CollectImputationDTO</returns>
//        public static CollectImputationDTO CreateCollectImputation(CollectImputation collectImputation)
//        {
//            CollectDTO collectDTO = new CollectDTO();

//            if (collectImputation.Collect != null)
//            {
//                collectDTO.CompanyIndividualId = collectImputation.Collect.AccountingCompany.IndividualId;
//                collectDTO.BranchId = collectImputation.Collect.Branch.Id;
//                collectDTO.CollectType = (int)collectImputation.Collect.CollectType;
//                collectDTO.Comments = collectImputation.Collect.Comments;

//                collectDTO.Concept.Id = collectImputation.Collect.Concept.Id;
//                collectDTO.Concept.Description = collectImputation.Collect.Concept.Description;
//                collectDTO.Date = collectImputation.Collect.Date;
//                collectDTO.Description = collectImputation.Collect.Description;
//                collectDTO.Id = collectImputation.Collect.Id;
//                collectDTO.IsTemporal = collectImputation.Collect.IsTemporal;
//                collectDTO.Number = collectImputation.Collect.Number;

//                PayerDTO payerDTO = new PayerDTO();

//                payerDTO.PayerId = collectImputation.Collect.Payer.IndividualId;
//                payerDTO.PayerName = collectImputation.Collect.Payer.Name;
//                payerDTO.PayerDocumentTypeId = collectImputation.Collect.Payer.IdentificationDocument.DocumentType.Id;
//                payerDTO.PayerDocumentNumber = collectImputation.Collect.Payer.IdentificationDocument.Number;
//                payerDTO.PayerTypeId = collectImputation.Collect.Payer.PersonType.PersonTypeCode;

//                collectDTO.Payer = payerDTO;

//                collectDTO.Payments = new List<PaymentDTO>();
//                collectDTO.PaymentsTotal = collectImputation.Collect.PaymentsTotal.Value;
//                collectDTO.PersonTypeId = collectImputation.Collect.PersonType.Id;
//                collectDTO.Status = collectImputation.Collect.Status;
//                collectDTO.UserId = collectImputation.Collect.UserId;

//            }

//            TransactionDTO transactionDTO = new TransactionDTO();

//            if (collectImputation.Transaction != null)
//            {
//                transactionDTO.AccountingTransaction = collectImputation.Transaction.AccountingTransaction;
//                transactionDTO.Id = collectImputation.Transaction.Id;
//                transactionDTO.TechnicalTransaction = collectImputation.Transaction.TechnicalTransaction;
//            }

//            ImputationDTO imputationDTO = new ImputationDTO();

//            if (collectImputation.Imputation != null)
//            {
//                imputationDTO.Date = collectImputation.Imputation.Date;
//                imputationDTO.Id = collectImputation.Imputation.Id;
//                imputationDTO.ImputationType = (int)collectImputation.Imputation.ImputationType;
//                imputationDTO.IsTemporal = collectImputation.Imputation.IsTemporal;
//                imputationDTO.UserId = collectImputation.Imputation.UserId;
//                imputationDTO.VerificationValue = new AmountDTO()
//                {
//                    Value = collectImputation.Imputation.VerificationValue.Value
//                };
//            }

//            return new CollectImputationDTO()
//            {
//                Id = collectImputation.Id,
//                Collect = collectDTO,
//                Transaction = transactionDTO,
//                Imputation = imputationDTO

//            };
//        }

//        /// <summary>
//        /// CreateCollectImputations
//        /// </summary>
//        /// <param name="collectImputations"></param>
//        /// <returns>List<CollectImputationDTO></returns>
//        public static List<CollectImputationDTO> CreateCollectImputations(List<CollectImputation> collectImputations)
//        {
//            List<CollectImputationDTO> collectImputationDTOs = new List<CollectImputationDTO>();

//            foreach (CollectImputation collectImputation in collectImputations)
//            {
//                collectImputationDTOs.Add(CreateCollectImputation(collectImputation));
//            }
//            return collectImputationDTOs;
//        }


//        #endregion

//        #region Collect          

//        /// <summary>
//        /// CreateCollect
//        /// </summary>
//        /// <param name="collect"></param>
//        /// <returns>CollectDTO</returns>
//        public static CollectDTO CreateCollect(Collect collect)
//        {
//            CollectConceptDTO collectConceptDTO = new CollectConceptDTO();
//            collectConceptDTO.Id = collect.Concept.Id;
//            collectConceptDTO.Description = collect.Concept.Description;

//            PayerDTO payerDTO = new PayerDTO();
//            payerDTO.PayerId = collect.Payer.IndividualId;
//            payerDTO.PayerName = collect.Payer.Name;
//            payerDTO.PayerDocumentTypeId = collect.Payer.IdentificationDocument.DocumentType.Id;
//            payerDTO.PayerDocumentNumber = collect.Payer.IdentificationDocument.Number;
//            payerDTO.PayerTypeId = collect.Payer.PersonType.PersonTypeCode;

//            TransactionDTO transactionDTO = new TransactionDTO();
//            transactionDTO.Id = collect.Transaction.Id;
//            transactionDTO.TechnicalTransaction = collect.Transaction.TechnicalTransaction;
//            transactionDTO.AccountingTransaction = collect.Transaction.AccountingTransaction;

//            return new CollectDTO()
//            {
//                Id = collect.Id,
//                IsTemporal = Convert.ToBoolean(collect.IsTemporal),
//                Date = collect.Date,
//                Description = collect.Description,
//                Comments = collect.Comments,
//                Concept = collectConceptDTO,
//                PaymentsTotal = collect.PaymentsTotal.Value,
//                Payer = payerDTO,
//                Status = Convert.ToInt32(collect.Status),
//                UserId = Convert.ToInt32(collect.UserId),
//                Number = Convert.ToInt32(collect.Number),
//                CollectType = (int)collect.CollectType,
//                Transaction = transactionDTO,
//                CompanyIndividualId = collect.AccountingCompany.IndividualId
//            };
//        }

//        /// <summary>
//        /// CreateCollects
//        /// </summary>
//        /// <param name="collects"></param>
//        /// <returns>List<CollectDTO></returns>
//        internal static List<CollectDTO> CreateCollects(List<Collect> collects)
//        {
//            List<CollectDTO> collectsDTO = new List<CollectDTO>();

//            foreach (Collect collect in collects)
//            {
//                collectsDTO.Add(CreateCollect(collect));
//            }

//            return collectsDTO;
//        }

//        #endregion

//        #region CollectControl

//        /// <summary>
//        /// CreateCollectControl
//        /// </summary>
//        /// <param name="collectControl"></param>
//        /// <returns>CollectControlDTO</returns>
//        public static CollectControlDTO CreateCollectControl(CollectControl collectControl)
//        {
//            return new CollectControlDTO()
//            {
//                Id = collectControl.Id,
//                AccountingDate = collectControl.AccountingDate,
//                CloseDate = collectControl.CloseDate,
//                OpenDate = collectControl.OpenDate,
//                UserId = collectControl.UserId,
//                Status = collectControl.Status,
//                BranchId = collectControl.Branch.Id,
//                Collects = CreateCollects(collectControl.Collects),
//                CollectControlPayments = CreateCollectControlPaymentsDTO(collectControl.CollectControlPayments)
//            };
//        }

//        #endregion

//        #region CollectControlPayment

//        /// <summary>
//        /// CreateCollectControlPaymentDTO
//        /// </summary>
//        /// <param name="collectControlPayment"></param>
//        /// <returns>CollectControlPaymentDTO</returns>
//        internal static CollectControlPaymentDTO CreateCollectControlPayment(CollectControlPayment collectControlPayment)
//        {
//            return new CollectControlPaymentDTO
//            {
//                Id = collectControlPayment.Id,
//                PaymentMethodId = collectControlPayment.PaymentMethod.Id,
//                PaymentsTotalDifference = collectControlPayment.PaymentsTotalDifference.Value,
//                PaymentsTotalReceived = collectControlPayment.PaymentsTotalReceived.Value,
//                PaymentTotalAdmitted = collectControlPayment.PaymentTotalAdmitted.Value,
//            };
//        }


//        /// <summary>
//        /// CreateCollectControlPaymentsDTO
//        /// </summary>
//        /// <param name="collectControlPayments"></param>
//        /// <returns>List<CollectControlPaymentDTO></returns>
//        internal static List<CollectControlPaymentDTO> CreateCollectControlPaymentsDTO(List<CollectControlPayment> collectControlPayments)
//        {
//            List<CollectControlPaymentDTO> collectControlPaymentsDTO = new List<CollectControlPaymentDTO>();

//            foreach (CollectControlPayment collectControlPaymentDTO in collectControlPayments)
//            {
//                collectControlPaymentsDTO.Add(CreateCollectControlPayment(collectControlPaymentDTO));
//            }

//            return collectControlPaymentsDTO;
//        }


//        #endregion

//        #region JournalEntry

//        /// <summary>
//        /// CreateJournalEntry
//        /// </summary>
//        /// <param name="journalEntry"></param>
//        /// <returns>JournalEntryDTO</returns>
//        public static JournalEntryDTO CreateJournalEntry(Models.Imputations.JournalEntry journalEntry)
//        {

//            ImputationDTO imputation = new ImputationDTO();
//            imputation.Id = journalEntry.Imputation.Id;

//            TransactionDTO transaction = new TransactionDTO();
//            transaction.Id = journalEntry.Transaction.Id;

//            return new JournalEntryDTO()
//            {
//                Id = journalEntry.Id,
//                AccountingDate = journalEntry.AccountingDate,
//                Branch = new BranchDTO()
//                {
//                    Id = journalEntry.Branch.Id
//                },
//                SalePoint = new SalePointDTO()
//                {
//                    Id = journalEntry.SalePoint.Id
//                },
//                Company = new DTOs.Imputations.CompanyDTO()
//                {
//                    CompanyId = journalEntry.Company.IndividualId
//                },
//                Payer = new IndividualDTO()
//                {
//                    IndividualId = journalEntry.Company.IndividualId
//                },
//                PersonType = new PersonTypeDTO()
//                {
//                    Id = journalEntry.PersonType.Id
//                },
//                Description = journalEntry.Description,
//                Comments = journalEntry.Comments,
//                Status = journalEntry.Status,
//                IsTemporal = journalEntry.IsTemporal,
//                Imputation = imputation,
//                Transaction = transaction
//            };
//        }

//        #endregion


//        #region Imputation

//        /// <summary>
//        /// CreateImputation
//        /// </summary>
//        /// <param name="imputation"></param>
//        /// <returns>ImputationDTO</returns>
//        public static ImputationDTO CreateImputation(Imputation imputation)
//        {

//            List<TransactionTypeDTO> transactionTypes = new List<TransactionTypeDTO>();

//            foreach (TransactionType transactionType in imputation.ImputationItems)
//            {
//                TransactionTypeDTO transactionTypeDTO = new TransactionTypeDTO();
//                transactionTypeDTO.Id = transactionType.Id;
//                transactionTypeDTO.Description = transactionType.Description;
//                transactionTypeDTO.TotalCredit = new AmountDTO()
//                {
//                    Value = transactionType.TotalCredit.Value
//                };
//                transactionTypeDTO.TotalDebit = new AmountDTO()
//                {
//                    Value = transactionType.TotalDebit.Value
//                };
//                transactionTypes.Add(transactionTypeDTO);
//            }

//            ImputationTypes imputationType = new ImputationTypes();

//            switch ((int)imputation.ImputationType)
//            {
//                case 1:
//                    imputationType = ImputationTypes.Collect;
//                    break;
//                case 2:
//                    imputationType = ImputationTypes.JournalEntry;
//                    break;
//                case 3:
//                    imputationType = ImputationTypes.PreLiquidation;
//                    break;
//                case 4:
//                    imputationType = ImputationTypes.PaymentOrder;
//                    break;
//            }

//            return new ImputationDTO()
//            {
//                Id = imputation.Id,
//                UserId = imputation.UserId,
//                Date = imputation.Date,
//                IsTemporal = imputation.IsTemporal,
//                ImputationItems = transactionTypes,
//                ImputationType = (int)imputationType
//            };
//        }

//        /// <summary>
//        /// CreateImputations
//        /// </summary>
//        /// <param name="imputations"></param>
//        /// <returns>List<ImputationDTO></returns>
//        public static List<ImputationDTO> CreateImputations(List<Imputation> imputations)
//        {
//            List<ImputationDTO> imputationsDTO = new List<ImputationDTO>();

//            foreach (Imputation imputation in imputations)
//            {
//                imputationsDTO.Add(CreateImputation(imputation));
//            }
//            return imputationsDTO;
//        }

//        #endregion

//        #region AccountCompany


//        public static BankAccountCompanyDTO CreateBankAccountCompanyDTO(Models.BankAccounts.BankAccountCompany bankAccountCompany)
//        {
//            return new BankAccountCompanyDTO
//            {
//                Id = bankAccountCompany.Id,
//                BankAccountType = BankAccountTypeDTO(bankAccountCompany.BankAccountType),
//                Branch = new BranchDTO()
//                {
//                    Id = bankAccountCompany.Bank.Id,
//                    Description = bankAccountCompany.Branch.Description
//                },
//                Bank = new BankDTO()
//                {
//                    Id = bankAccountCompany.Bank.Id,
//                    Description = bankAccountCompany.Bank.Description
//                },
//                Currency = new CurrencyDTO()
//                {
//                    Id = bankAccountCompany.Currency.Id,
//                    Description = bankAccountCompany.Currency.Description
//                },
//                DisableDate = bankAccountCompany.DisableDate,
//                IsDefault = bankAccountCompany.IsDefault,
//                IsEnabled = bankAccountCompany.IsEnabled,
//                Number = bankAccountCompany.Number,
//                AccountingAccount = new AccountingAccountDTO()
//                {
//                    AccountingAccountId = bankAccountCompany.AccountingAccount.AccountingAccountId
//                }
//            };
//        }

//        internal static BankAccountTypeDTO BankAccountTypeDTO(BankAccountType bankAccountType)
//        {
//            return new BankAccountTypeDTO
//            {
//                Id = bankAccountType.Id,
//                Description = bankAccountType.Description,
//                IsEnabled = bankAccountType.IsEnabled
//            };
//        }

//        internal static List<BankAccountCompanyDTO> BankAccountCompanyDTOs(List<BankAccountCompany> status)
//        {
//            List<BankAccountCompanyDTO> bankAccountCompanyDTOs = new List<BankAccountCompanyDTO>();

//            foreach (BankAccountCompany statusModel in status)
//            {
//                bankAccountCompanyDTOs.Add(CreateBankAccountCompanyDTO(statusModel));
//            }

//            return bankAccountCompanyDTOs;
//        }
//        #endregion AccountCompany

//        #region PaymentTicket

//        public static DTOs.PaymentTicketDTO CreatePaymentTicket(PaymentTicket paymentTicket)
//        {
//            return new DTOs.PaymentTicketDTO
//            {
//                Id = paymentTicket.Id,
//                AccountNumber = paymentTicket.AccountNumber,
//                Amount = new AmountDTO()
//                {
//                    Value = paymentTicket.Amount.Value
//                },
//                Branch = new BranchDTO()
//                {
//                    Id = paymentTicket.Bank.Id,
//                    Description = paymentTicket.Branch.Description
//                },
//                Bank = new BankDTO()
//                {
//                    Id = paymentTicket.Bank.Id,
//                    Description = paymentTicket.Bank.Description
//                },
//                CashAmount = new AmountDTO()
//                {
//                    Value = paymentTicket.CashAmount.Value
//                },
//                Commission = new AmountDTO()
//                {
//                    Value = paymentTicket.Commission.Value
//                },
//                Currency = new CurrencyDTO()
//                {
//                    Id = paymentTicket.Currency.Id
//                },
//                DisabledDate = paymentTicket.DisabledDate,
//                DisabledUser = paymentTicket.DisabledUser,
//                PaymentMethod = paymentTicket.PaymentMethod,
//                Status = paymentTicket.Status,
//                Payments = PaymentDTOs(paymentTicket.Payments)
//            };
//        }

//        internal static PaymentDTO PaymentDTO(Payment payment)
//        {
//            return new DTOs.Payments.PaymentDTO
//            {
//                Id = payment.Id,
//                Amount = new AmountDTO()
//                {
//                    Value = payment.Amount.Value
//                },
//                //ExchangeRateSellAmount = payment.ExchangeRate.SellAmount,
//                //ExchangeRateBuyAmount = payment.ExchangeRate.BuyAmount,
//                LocalAmount = new AmountDTO()
//                {
//                    Value = payment.LocalAmount.Value
//                },
//                PaymentMethod = new PaymentMethodDTO
//                {
//                    Id = payment.PaymentMethod.Id,
//                    Description = payment.PaymentMethod.Description,
//                },
//                Retention = payment.Retention,
//                Status = payment.Status,
//                Tax = payment.Tax,
//                Taxes = PaymentTaxDTOs(payment.Taxes)
//            };
//        }

//        internal static PaymentTaxDTO PaymentTaxDTOs(PaymentTax paymentTax)
//        {
//            return new PaymentTaxDTO
//            {
//                Id = paymentTax.Id,
//                Branch = new BranchDTO()
//                {
//                    Id = paymentTax.Branch.Id
//                },
//                Rate = paymentTax.Rate,
//                TaxBase = new AmountDTO()
//                {
//                    Value = paymentTax.TaxBase.Value
//                },
//                Tax = new TaxDTO()
//                {
//                    Id = paymentTax.Tax.Id
//                }
//            };
//        }

//        internal static List<PaymentTaxDTO> PaymentTaxDTOs(List<PaymentTax> paymentTaxes)
//        {
//            List<PaymentTaxDTO> paymentTaxDTOs = new List<PaymentTaxDTO>();

//            foreach (PaymentTax PaymentTaxModel in paymentTaxes)
//            {
//                paymentTaxDTOs.Add(PaymentTaxDTOs(PaymentTaxModel));
//            }

//            return paymentTaxDTOs;
//        }

//        internal static List<PaymentDTO> PaymentDTOs(List<Payment> payment)
//        {
//            List<PaymentDTO> paymentDTOs = new List<PaymentDTO>();

//            foreach (Payment PaymentModel in payment)
//            {
//                paymentDTOs.Add(PaymentDTO(PaymentModel));
//            }

//            return paymentDTOs;
//        }
//        #endregion PaymentTicket


//    }
//}
