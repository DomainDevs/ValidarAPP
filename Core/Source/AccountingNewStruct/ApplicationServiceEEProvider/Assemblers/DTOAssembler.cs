using AutoMapper;
using Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.AccountingServices.DTOs.AccountsPayables;
using Sistran.Core.Application.AccountingServices.DTOs.Amortizations;
using Sistran.Core.Application.AccountingServices.DTOs.BankAccounts;
using Sistran.Core.Application.AccountingServices.DTOs.CancellationPolicies;
using Sistran.Core.Application.AccountingServices.DTOs.CreditNotes;
using Sistran.Core.Application.AccountingServices.DTOs.Imputations;
using Sistran.Core.Application.AccountingServices.DTOs.Payments;
using Sistran.Core.Application.AccountingServices.DTOs.Retentions;
using Sistran.Core.Application.AccountingServices.DTOs.Search;
using Sistran.Core.Application.AccountingServices.EEProvider.Models;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.AccountsPayables;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Amortizations;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.BankAccounts;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.CancellationPolicies;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Collect;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.CreditNotes;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Payments;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Retentions;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Underwriting;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.Cache;
using System.Collections.Generic;
using System.Linq;
using PaymentsModels = Sistran.Core.Application.AccountingServices.EEProvider.Models.Payments;
using Sistran.Core.Services.UtilitiesServices.Enums;
using PRCLAIM = Sistran.Core.Application.AccountingServices.EEProvider.Models.Claims.PaymentRequest;
using CLAIMODL = Sistran.Core.Application.AccountingServices.EEProvider.Models.Claims;
using ACCINTDTO = Sistran.Core.Integration.AccountingServices.DTOs.Accounting;
using Sistran.Core.Application.AccountingServices.Enums;
using System.Threading;
using System;
using UTILHELPER = Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.AccountingServices.EEProvider.Enums;
using Sistran.Core.Application.AccountingServices.EEProvider;
using Sistran.Core.Application.AccountingServices.EEProvider.Business;

namespace Sistran.Core.Application.AccountingServices.Assemblers
{
    internal static class DTOAssembler
    {
        #region ActionType       
        public static IMapper CreateMapActionTypes()
        {
            var config = MapperCache.GetMapper<ActionType, ActionTypeDTO>(cfg =>
            {
                cfg.CreateMap<ActionType, ActionTypeDTO>();
            });
            return config;
        }

        public static ActionTypeDTO ToDTO(this ActionType actionType)
        {
            var config = CreateMapActionTypes();
            return config.Map<ActionType, ActionTypeDTO>(actionType);
        }

        public static IEnumerable<ActionTypeDTO> ToDTOs(this IEnumerable<ActionType> actionType)
        {
            return actionType.Select(ToDTO);
        }
        #endregion

        #region Collects
        public static IMapper CreateMapCollects()
        {
            var config = MapperCache.GetMapper<Collect, CollectDTO>(cfg =>
            {
                cfg.CreateMap<Collect, CollectDTO>()
                        .ForMember(dest => dest.CollectType, opt => opt.MapFrom(src => (int)src.CollectType))
                        .ForMember(dest => dest.AccountingCompany, opt => opt.MapFrom(src => new CompanyDTO { IndividualId = src.AccountingCompany.IndividualId, Name = src.AccountingCompany.FullName }));
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
                cfg.CreateMap<CollectConcept, CollectConceptDTO>();
                cfg.CreateMap<PaymentsModels.Payment, PaymentDTO>();
                cfg.CreateMap<PaymentsModels.PaymentMethod, PaymentMethodDTO>();
                cfg.CreateMap<ExchangeRate, ExchangeRateDTO>();
                cfg.CreateMap<PaymentTax, PaymentTaxDTO>();
                cfg.CreateMap<Tax, TaxDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<Person, PersonDTO>()
                .ForMember(dest => dest.IndividualType, opt => opt.MapFrom(src => (int)src.IndividualType))
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (int)src.CustomerType));
                cfg.CreateMap<DocumentType, DocumentTypeDTO>();
                cfg.CreateMap<IdentificationDocument, DTOs.IdentificationDocumentDTO>();
                cfg.CreateMap<Company, DTOs.CompanyDTO>()
                .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName));
                cfg.CreateMap<Transaction, TransactionDTO>();
                cfg.CreateMap<PersonType, PersonTypeDTO>();
            });
            return config;
        }

        public static CollectDTO ToDTO(this Collect collect)
        {
            var config = CreateMapCollects();
            return config.Map<Collect, CollectDTO>(collect);
        }

        public static IEnumerable<CollectDTO> ToDTOs(this IEnumerable<Collect> collect)
        {
            return collect.Select(ToDTO);
        }

        public static IMapper CreateMapCollectConcepts()
        {
            var config = MapperCache.GetMapper<CollectConcept, CollectConceptDTO>(cfg =>
            {
                cfg.CreateMap<CollectConcept, CollectConceptDTO>();
            });
            return config;
        }

        public static CollectConceptDTO ToDTO(this CollectConcept collectConcept)
        {
            var config = CreateMapCollectConcepts();
            return config.Map<CollectConcept, CollectConceptDTO>(collectConcept);
        }

        public static IEnumerable<CollectConceptDTO> ToDTOs(this IEnumerable<CollectConcept> collectConcept)
        {
            return collectConcept.Select(ToDTO);
        }

        public static IMapper CreateMapCollectControls()
        {
            var config = MapperCache.GetMapper<CollectControl, CollectControlDTO>(cfg =>
            {
                cfg.CreateMap<CollectControl, CollectControlDTO>();
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<Collect, CollectDTO>()
                        .ForMember(dest => dest.CollectType, opt => opt.MapFrom(src => (int)src.CollectType))
                        .ForMember(dest => dest.AccountingCompany, opt => opt.MapFrom(src => new CompanyDTO { IndividualId = src.AccountingCompany.IndividualId, Name = src.AccountingCompany.FullName }));
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
                cfg.CreateMap<CollectConcept, CollectConceptDTO>();
                cfg.CreateMap<PaymentsModels.Payment, PaymentDTO>();
                cfg.CreateMap<PaymentsModels.PaymentMethod, PaymentMethodDTO>();
                cfg.CreateMap<ExchangeRate, ExchangeRateDTO>();
                cfg.CreateMap<PaymentTax, PaymentTaxDTO>();
                cfg.CreateMap<Tax, TaxDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<Person, PersonDTO>()
                .ForMember(dest => dest.IndividualType, opt => opt.MapFrom(src => (int)src.IndividualType))
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (int)src.CustomerType));
                cfg.CreateMap<DocumentType, DocumentTypeDTO>();
                cfg.CreateMap<IdentificationDocument, DTOs.IdentificationDocumentDTO>();
                cfg.CreateMap<Company, DTOs.CompanyDTO>()
                .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName));
                cfg.CreateMap<Transaction, TransactionDTO>();
                cfg.CreateMap<PersonType, PersonTypeDTO>();
                cfg.CreateMap<CollectControlPayment, CollectControlPaymentDTO>();
            });
            return config;
        }

        public static CollectControlDTO ToDTO(this CollectControl collectControl)
        {
            var config = CreateMapCollectControls();
            return config.Map<CollectControl, CollectControlDTO>(collectControl);
        }

        public static IEnumerable<CollectControlDTO> ToDTOs(this IEnumerable<CollectControl> collectControl)
        {
            return collectControl.Select(ToDTO);
        }

        public static IMapper CreateMapCollectControlPayments()
        {
            var config = MapperCache.GetMapper<CollectControlPayment, CollectControlPaymentDTO>(cfg =>
            {
                cfg.CreateMap<CollectControlPayment, CollectControlPaymentDTO>();
                cfg.CreateMap<PaymentsModels.PaymentMethod, PaymentMethodDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
            });
            return config;
        }

        public static CollectControlPaymentDTO ToDTO(this CollectControlPayment collectControlPayment)
        {
            var config = CreateMapCollectControlPayments();
            return config.Map<CollectControlPayment, CollectControlPaymentDTO>(collectControlPayment);
        }

        public static IEnumerable<CollectControlPaymentDTO> ToDTOs(this IEnumerable<CollectControlPayment> collectControlPayment)
        {
            return collectControlPayment.Select(ToDTO);
        }
        #endregion

        #region Payments
        public static IMapper CreateMapPaymentBallots()
        {
            var config = MapperCache.GetMapper<PaymentBallot, DTOs.PaymentBallotDTO>(cfg =>
            {
                cfg.CreateMap<PaymentBallot, DTOs.PaymentBallotDTO>();
                cfg.CreateMap<Bank, BankDTO>();
                cfg.CreateMap<PaymentTicket, DTOs.PaymentTicketDTO>();
                cfg.CreateMap<PaymentsModels.Payment, PaymentDTO>();
                cfg.CreateMap<PaymentsModels.PaymentMethod, PaymentMethodDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<ExchangeRate, ExchangeRateDTO>();
                cfg.CreateMap<PaymentTax, PaymentTaxDTO>();
                cfg.CreateMap<Tax, TaxDTO>();
                cfg.CreateMap<Transaction, TransactionDTO>();
            });
            return config;
        }

        public static DTOs.PaymentBallotDTO ToDTO(this PaymentBallot paymentBallot)
        {
            var config = CreateMapPaymentBallots();
            return config.Map<PaymentBallot, DTOs.PaymentBallotDTO>(paymentBallot);
        }

        public static IEnumerable<DTOs.PaymentBallotDTO> ToDTOs(this IEnumerable<PaymentBallot> paymentBallot)
        {
            return paymentBallot.Select(ToDTO);
        }

        public static IMapper CreateMapPaymentBallotsNew()
        {
            var config = MapperCache.GetMapper<PaymentBallot, DTOs.Search.PaymentBallotDTO>(cfg =>
            {
                cfg.CreateMap<PaymentBallot, DTOs.Search.PaymentBallotDTO>()
                .ForMember(dest => dest.DepositBallotId, opt => opt.MapFrom(src => (int)src.Id))
                .ForMember(dest => dest.DepositBallotRegisterDate, opt => opt.MapFrom(src => (DateTime)src.BankDate))
                .ForMember(dest => dest.DepositBallotNumber, opt => opt.MapFrom(src => src.BallotNumber))
                .ForMember(dest => dest.DepositBallotBankId, opt => opt.MapFrom(src => src.Bank.Id))
                .ForMember(dest => dest.DepositBallotBankDescription, opt => opt.MapFrom(src => src.Bank.Description))
                .ForMember(dest => dest.DepositBallotAccountNumber, opt => opt.MapFrom(src => src.AccountNumber));
                cfg.CreateMap<Bank, BankDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<PaymentTicket, DTOs.PaymentTicketDTO>();
                cfg.CreateMap<Transaction, TransactionDTO>();
            });
            return config;
        }

        public static DTOs.Search.PaymentBallotDTO ToDTOPayment(this PaymentBallot paymentBallot)
        {
            var config = CreateMapPaymentBallotsNew();
            return config.Map<PaymentBallot, DTOs.Search.PaymentBallotDTO>(paymentBallot);
        }

        public static IEnumerable<DTOs.Search.PaymentBallotDTO> ToDTOsPayment(this IEnumerable<PaymentBallot> paymentBallot)
        {
            return paymentBallot.Select(ToDTOPayment);
        }

        public static IMapper CreateMapPaymentTickets()
        {
            var config = MapperCache.GetMapper<PaymentTicket, DTOs.PaymentTicketDTO>(cfg =>
            {
                cfg.CreateMap<PaymentTicket, DTOs.PaymentTicketDTO>();
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
                cfg.CreateMap<Bank, BankDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<PaymentsModels.Payment, PaymentDTO>();
                cfg.CreateMap<PaymentsModels.PaymentMethod, PaymentMethodDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<ExchangeRate, ExchangeRateDTO>();
                cfg.CreateMap<PaymentTax, PaymentTaxDTO>();
                cfg.CreateMap<Tax, TaxDTO>();
            });
            return config;
        }

        public static DTOs.PaymentTicketDTO ToDTO(this PaymentTicket paymentTicket)
        {
            var config = CreateMapPaymentTickets();
            return config.Map<PaymentTicket, DTOs.PaymentTicketDTO>(paymentTicket);
        }

        public static IEnumerable<DTOs.PaymentTicketDTO> ToDTOs(this IEnumerable<PaymentTicket> paymentTicket)
        {
            return paymentTicket.Select(ToDTO);
        }

        public static IMapper CreateMapRejectedPayments()
        {
            var config = MapperCache.GetMapper<RejectedPayment, DTOs.RejectedPaymentDTO>(cfg =>
            {
                cfg.CreateMap<RejectedPayment, DTOs.RejectedPaymentDTO>();
                cfg.CreateMap<PaymentsModels.Payment, PaymentDTO>();
                cfg.CreateMap<PaymentsModels.PaymentMethod, PaymentMethodDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<ExchangeRate, ExchangeRateDTO>();
                cfg.CreateMap<PaymentTax, PaymentTaxDTO>();
                cfg.CreateMap<Tax, TaxDTO>();
                cfg.CreateMap<Rejection, RejectionDTO>();
            });
            return config;
        }

        public static DTOs.RejectedPaymentDTO ToDTO(this RejectedPayment rejectedPayment)
        {
            var config = CreateMapRejectedPayments();
            return config.Map<RejectedPayment, DTOs.RejectedPaymentDTO>(rejectedPayment);
        }

        public static IEnumerable<DTOs.RejectedPaymentDTO> ToDTOs(this IEnumerable<RejectedPayment> rejectedPayment)
        {
            return rejectedPayment.Select(ToDTO);
        }

        #endregion



        public static IMapper CreateMapRanges()
        {
            var config = MapperCache.GetMapper<Range, RangeDTO>(cfg =>
            {
                cfg.CreateMap<Range, RangeDTO>();
                cfg.CreateMap<RangeItem, RangeItemDTO>();
            });
            return config;
        }

        public static RangeDTO ToDTO(this Range range)
        {
            var config = CreateMapRanges();
            return config.Map<Range, RangeDTO>(range);
        }

        public static IEnumerable<RangeDTO> ToDTOs(this IEnumerable<Range> range)
        {
            return range.Select(ToDTO);
        }

        public static IMapper CreateMapRejections()
        {
            var config = MapperCache.GetMapper<Rejection, RejectionDTO>(cfg =>
            {
                cfg.CreateMap<Rejection, RejectionDTO>();
            });
            return config;
        }

        public static RejectionDTO ToDTO(this Rejection rejection)
        {
            var config = CreateMapRejections();
            return config.Map<Rejection, RejectionDTO>(rejection);
        }

        public static IEnumerable<RejectionDTO> ToDTOs(this IEnumerable<Rejection> rejection)
        {
            return rejection.Select(ToDTO);
        }

        public static IMapper CreateMapTransactions()
        {
            var config = MapperCache.GetMapper<Transaction, TransactionDTO>(cfg =>
            {
                cfg.CreateMap<Transaction, TransactionDTO>();
            });
            return config;
        }

        public static TransactionDTO ToDTO(this Transaction transaction)
        {
            var config = CreateMapTransactions();
            return config.Map<Transaction, TransactionDTO>(transaction);
        }

        public static IEnumerable<TransactionDTO> ToDTOs(this IEnumerable<Transaction> transaction)
        {
            return transaction.Select(ToDTO);
        }

        public static IMapper CreateMapRetentionBases()
        {
            var config = MapperCache.GetMapper<RetentionBase, RetentionBaseDTO>(cfg =>
            {
                cfg.CreateMap<RetentionBase, RetentionBaseDTO>();
            });
            return config;
        }

        public static RetentionBaseDTO ToDTO(this RetentionBase retentionBase)
        {
            var config = CreateMapRetentionBases();
            return config.Map<RetentionBase, RetentionBaseDTO>(retentionBase);
        }

        public static IEnumerable<RetentionBaseDTO> ToDTOs(this IEnumerable<RetentionBase> retentionBase)
        {
            return retentionBase.Select(ToDTO);
        }

        public static IMapper CreateMapRetentionConcepts()
        {
            var config = MapperCache.GetMapper<RetentionConcept, RetentionConceptDTO>(cfg =>
            {
                cfg.CreateMap<RetentionConcept, RetentionConceptDTO>();
                cfg.CreateMap<RetentionBase, RetentionBaseDTO>();
            });
            return config;
        }

        public static RetentionConceptDTO ToDTO(this RetentionConcept retentionConcept)
        {
            var config = CreateMapRetentionConcepts();
            return config.Map<RetentionConcept, RetentionConceptDTO>(retentionConcept);
        }

        public static IEnumerable<RetentionConceptDTO> ToDTOs(this IEnumerable<RetentionConcept> retentionConcept)
        {
            return retentionConcept.Select(ToDTO);
        }

        public static IMapper CreateMapRetentionConceptPercentages()
        {
            var config = MapperCache.GetMapper<RetentionConceptPercentage, RetentionConceptPercentageDTO>(cfg =>
            {
                cfg.CreateMap<RetentionConceptPercentage, RetentionConceptPercentageDTO>();
                cfg.CreateMap<RetentionConcept, RetentionConceptDTO>();
                cfg.CreateMap<RetentionBase, RetentionBaseDTO>();
            });
            return config;
        }

        public static RetentionConceptPercentageDTO ToDTO(this RetentionConceptPercentage retentionConceptPercentage)
        {
            var config = CreateMapRetentionConceptPercentages();
            return config.Map<RetentionConceptPercentage, RetentionConceptPercentageDTO>(retentionConceptPercentage);
        }

        public static IEnumerable<RetentionConceptPercentageDTO> ToDTOs(this IEnumerable<RetentionConceptPercentage> retentionConceptPercentage)
        {
            return retentionConceptPercentage.Select(ToDTO);
        }

        public static IMapper CreateMapCashs()
        {
            var config = MapperCache.GetMapper<Cash, CashDTO>(cfg =>
            {
                cfg.CreateMap<Cash, CashDTO>();
                cfg.CreateMap<PaymentsModels.Payment, PaymentDTO>();
                cfg.CreateMap<PaymentsModels.PaymentMethod, PaymentMethodDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<ExchangeRate, ExchangeRateDTO>();
                cfg.CreateMap<PaymentTax, PaymentTaxDTO>();
                cfg.CreateMap<Tax, TaxDTO>();
            });
            return config;
        }

        public static CashDTO ToDTO(this Cash cash)
        {
            var config = CreateMapCashs();
            return config.Map<Cash, CashDTO>(cash);
        }

        public static IEnumerable<CashDTO> ToDTOs(this IEnumerable<Cash> cash)
        {
            return cash.Select(ToDTO);
        }

        public static IMapper CreateMapChecks()
        {
            var config = MapperCache.GetMapper<Check, CheckDTO>(cfg =>
            {
                cfg.CreateMap<Check, CheckDTO>();
                cfg.CreateMap<Bank, BankDTO>();
                cfg.CreateMap<PaymentsModels.Payment, PaymentDTO>();
                cfg.CreateMap<PaymentsModels.PaymentMethod, PaymentMethodDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<ExchangeRate, ExchangeRateDTO>();
                cfg.CreateMap<PaymentTax, PaymentTaxDTO>();
                cfg.CreateMap<Tax, TaxDTO>();
            });
            return config;
        }

        public static CheckDTO ToDTO(this Check check)
        {
            var config = CreateMapChecks();
            return config.Map<Check, CheckDTO>(check);
        }

        public static IEnumerable<CheckDTO> ToDTOs(this IEnumerable<Check> check)
        {
            return check.Select(ToDTO);
        }

        public static IMapper CreateMapCreditCards()
        {
            var config = MapperCache.GetMapper<CreditCard, CreditCardDTO>(cfg =>
            {
                cfg.CreateMap<CreditCard, CreditCardDTO>();
                cfg.CreateMap<CreditCardType, CreditCardTypeDTO>();
                cfg.CreateMap<Bank, BankDTO>();
                cfg.CreateMap<CreditCardValidThru, CreditCardValidThruDTO>();
                cfg.CreateMap<PaymentsModels.Payment, PaymentDTO>();
                cfg.CreateMap<PaymentsModels.PaymentMethod, PaymentMethodDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<ExchangeRate, ExchangeRateDTO>();
                cfg.CreateMap<PaymentTax, PaymentTaxDTO>();
                cfg.CreateMap<Tax, TaxDTO>();
            });
            return config;
        }

        public static CreditCardDTO ToDTO(this CreditCard creditCard)
        {
            var config = CreateMapCreditCards();
            return config.Map<CreditCard, CreditCardDTO>(creditCard);
        }

        public static IEnumerable<CreditCardDTO> ToDTOs(this IEnumerable<CreditCard> creditCard)
        {
            return creditCard.Select(ToDTO);
        }

        public static IMapper CreateMapCreditCardTypes()
        {
            var config = MapperCache.GetMapper<CreditCardType, CreditCardTypeDTO>(cfg =>
            {
                cfg.CreateMap<CreditCardType, CreditCardTypeDTO>();
            });
            return config;
        }

        public static CreditCardTypeDTO ToDTO(this CreditCardType creditCardType)
        {
            var config = CreateMapCreditCardTypes();
            return config.Map<CreditCardType, CreditCardTypeDTO>(creditCardType);
        }

        public static IEnumerable<CreditCardTypeDTO> ToDTOs(this IEnumerable<CreditCardType> creditCardType)
        {
            return creditCardType.Select(ToDTO);
        }

        public static IMapper CreateMapCreditCardValidThrus()
        {
            var config = MapperCache.GetMapper<CreditCardValidThru, CreditCardValidThruDTO>(cfg =>
            {
                cfg.CreateMap<CreditCardValidThru, CreditCardValidThruDTO>();
            });
            return config;
        }

        public static CreditCardValidThruDTO ToDTO(this CreditCardValidThru creditCardValidThru)
        {
            var config = CreateMapCreditCardValidThrus();
            return config.Map<CreditCardValidThru, CreditCardValidThruDTO>(creditCardValidThru);
        }

        public static IEnumerable<CreditCardValidThruDTO> ToDTOs(this IEnumerable<CreditCardValidThru> creditCardValidThru)
        {
            return creditCardValidThru.Select(ToDTO);
        }

        public static IMapper CreateMapDepositVouchers()
        {
            var config = MapperCache.GetMapper<DepositVoucher, DepositVoucherDTO>(cfg =>
            {
                cfg.CreateMap<DepositVoucher, DepositVoucherDTO>();
                cfg.CreateMap<BankAccountCompany, BankAccountCompanyDTO>();
                cfg.CreateMap<Bank, BankDTO>();
                cfg.CreateMap<BankAccountType, BankAccountTypeDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
                cfg.CreateMap<GeneralLedgerServices.EEProvider.Models.AccountingAccount, AccountingAccountDTO>();
                cfg.CreateMap<PaymentsModels.Payment, PaymentDTO>();
                cfg.CreateMap<PaymentsModels.PaymentMethod, PaymentMethodDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<ExchangeRate, ExchangeRateDTO>();
                cfg.CreateMap<PaymentTax, PaymentTaxDTO>();
                cfg.CreateMap<Tax, TaxDTO>();
            });
            return config;
        }

        public static DepositVoucherDTO ToDTO(this DepositVoucher depositVoucher)
        {
            var config = CreateMapDepositVouchers();
            return config.Map<DepositVoucher, DepositVoucherDTO>(depositVoucher);
        }

        public static IEnumerable<DepositVoucherDTO> ToDTOs(this IEnumerable<DepositVoucher> depositVoucher)
        {
            return depositVoucher.Select(ToDTO);
        }

        public static IMapper CreateMapPayments()
        {
            var config = MapperCache.GetMapper<PaymentsModels.Payment, PaymentDTO>(cfg =>
            {
                cfg.CreateMap<PaymentsModels.Payment, PaymentDTO>();
                cfg.CreateMap<PaymentsModels.PaymentMethod, PaymentMethodDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<ExchangeRate, ExchangeRateDTO>();
                cfg.CreateMap<PaymentTax, PaymentTaxDTO>();
                cfg.CreateMap<Tax, TaxDTO>();
            });
            return config;
        }

        public static PaymentDTO ToDTO(this PaymentsModels.Payment payment)
        {
            var config = CreateMapPayments();
            return config.Map<PaymentsModels.Payment, PaymentDTO>(payment);
        }

        public static IEnumerable<PaymentDTO> ToDTOs(this IEnumerable<PaymentsModels.Payment> payment)
        {
            return payment.Select(ToDTO);
        }

        public static IMapper CreateMapPaymentMethods()
        {
            var config = MapperCache.GetMapper<Core.Application.AccountingServices.EEProvider.Models.Payments.PaymentMethod, PaymentMethodDTO>(cfg =>
            {
                cfg.CreateMap<PaymentsModels.PaymentMethod, PaymentMethodDTO>();
            });
            return config;
        }

        public static PaymentMethodDTO ToDTO(this Core.Application.AccountingServices.EEProvider.Models.Payments.PaymentMethod paymentMethod)
        {
            var config = CreateMapPaymentMethods();
            return config.Map<Core.Application.AccountingServices.EEProvider.Models.Payments.PaymentMethod, PaymentMethodDTO>(paymentMethod);
        }

        public static IEnumerable<PaymentMethodDTO> ToDTOs(this IEnumerable<Core.Application.AccountingServices.EEProvider.Models.Payments.PaymentMethod> paymentMethod)
        {
            return paymentMethod.Select(ToDTO);
        }

        public static IMapper CreateMapPaymentTaxs()
        {
            var config = MapperCache.GetMapper<PaymentTax, PaymentTaxDTO>(cfg =>
            {
                cfg.CreateMap<PaymentTax, PaymentTaxDTO>();
                cfg.CreateMap<TaxServices.Models.Tax, TaxDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
            });
            return config;
        }

        public static PaymentTaxDTO ToDTO(this PaymentTax paymentTax)
        {
            var config = CreateMapPaymentTaxs();
            return config.Map<PaymentTax, PaymentTaxDTO>(paymentTax);
        }

        public static IEnumerable<PaymentTaxDTO> ToDTOs(this IEnumerable<PaymentTax> paymentTax)
        {
            return paymentTax.Select(ToDTO);
        }

        public static IMapper CreateMapRetentionReceipts()
        {
            var config = MapperCache.GetMapper<RetentionReceipt, RetentionReceiptDTO>(cfg =>
            {
                cfg.CreateMap<RetentionReceipt, RetentionReceiptDTO>();
                cfg.CreateMap<Policy, PolicyDTO>();
                cfg.CreateMap<Beneficiary, BeneficiaryDTO>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.BeneficiaryTypeDescription))
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (int)src.CustomerType));
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<Endorsement, EndorsementDTO>();
                cfg.CreateMap<PayerComponent, PayerComponentDTO>();
                cfg.CreateMap<PaymentPlan, PaymentPlanDTO>();
                cfg.CreateMap<Quota, QuotaDTO>();
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
                cfg.CreateMap<Prefix, PrefixDTO>();
                cfg.CreateMap<IssuanceAgency, IssuanceAgencyDTO>();
                cfg.CreateMap<IssuanceAgent, IssuanceAgentDTO>();
                cfg.CreateMap<IssuanceAgentType, IssuanceAgentTypeDTO>();
                cfg.CreateMap<IssuanceCommission, IssuanceCommissionDTO>();
                cfg.CreateMap<SubLineBusiness, SubLineBusinessDTO>();
                cfg.CreateMap<LineBusiness, LineBusinessDTO>();
                cfg.CreateMap<Holder, HolderDTO>()
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (int)src.CustomerType))
                .ForMember(dest => dest.IndividualType, opt => opt.MapFrom(src => (int)src.IndividualType));
                cfg.CreateMap<BillingGroup, BillingGroupDTO>();
                cfg.CreateMap<PolicyType, PolicyTypeDTO>();
                cfg.CreateMap<Prefix, PrefixDTO>();

                cfg.CreateMap<RetentionConcept, RetentionConceptDTO>();
                cfg.CreateMap<RetentionBase, RetentionBaseDTO>();
                cfg.CreateMap<PaymentsModels.PaymentMethod, PaymentMethodDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<ExchangeRate, ExchangeRateDTO>();
                cfg.CreateMap<PaymentTax, PaymentTaxDTO>();
                cfg.CreateMap<Tax, TaxDTO>();
            });
            return config;
        }

        public static RetentionReceiptDTO ToDTO(this RetentionReceipt retentionReceipt)
        {
            var config = CreateMapRetentionReceipts();
            return config.Map<RetentionReceipt, RetentionReceiptDTO>(retentionReceipt);
        }

        public static IEnumerable<RetentionReceiptDTO> ToDTOs(this IEnumerable<RetentionReceipt> retentionReceipt)
        {
            return retentionReceipt.Select(ToDTO);
        }

        public static IMapper CreateMapTransfers()
        {
            var config = MapperCache.GetMapper<Transfer, TransferDTO>(cfg =>
            {
                cfg.CreateMap<Transfer, TransferDTO>();
                cfg.CreateMap<BankAccountPerson, BankAccountPersonDTO>();
                cfg.CreateMap<Individual, IndividualDTO>();
                cfg.CreateMap<DocumentType, DocumentTypeDTO>();
                cfg.CreateMap<Application.UniquePersonService.V1.Models.EconomicActivity, EconomicActivityDTO>();
                cfg.CreateMap<IdentificationDocument, IdentificationDocumentDTO>();
                cfg.CreateMap<BankAccountType, BankAccountTypeDTO>();
                cfg.CreateMap<Bank, BankDTO>();
                cfg.CreateMap<PaymentsModels.Payment, PaymentDTO>();
                cfg.CreateMap<PaymentsModels.PaymentMethod, PaymentMethodDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<ExchangeRate, ExchangeRateDTO>();
                cfg.CreateMap<PaymentTax, PaymentTaxDTO>();
                cfg.CreateMap<Tax, TaxDTO>();
            });
            return config;
        }

        public static TransferDTO ToDTO(this Transfer transfer)
        {
            var config = CreateMapTransfers();
            return config.Map<Transfer, TransferDTO>(transfer);
        }

        public static IEnumerable<TransferDTO> ToDTOs(this IEnumerable<Transfer> transfer)
        {
            return transfer.Select(ToDTO);
        }

        public static IMapper CreateMapBookAccounts()
        {
            var config = MapperCache.GetMapper<BookAccount, BookAccountDTO>(cfg =>
            {
                cfg.CreateMap<BookAccount, BookAccountDTO>();
            });
            return config;
        }

        public static BookAccountDTO ToDTO(this BookAccount bookAccount)
        {
            var config = CreateMapBookAccounts();
            return config.Map<BookAccount, BookAccountDTO>(bookAccount);
        }

        public static IEnumerable<BookAccountDTO> ToDTOs(this IEnumerable<BookAccount> bookAccount)
        {
            return bookAccount.Select(ToDTO);
        }

        public static IMapper CreateMapBrokerCheckingAccountItems()
        {
            var config = MapperCache.GetMapper<BrokerCheckingAccountItem, DTOs.Imputations.BrokerCheckingAccountItemDTO>(cfg =>
            {
                cfg.CreateMap<BrokerCheckingAccountItem, DTOs.Imputations.BrokerCheckingAccountItemDTO>();
            });
            return config;
        }

        public static DTOs.Imputations.BrokerCheckingAccountItemDTO ToDTO(this BrokerCheckingAccountItem brokerCheckingAccountItem)
        {
            var config = CreateMapBrokerCheckingAccountItems();
            return config.Map<BrokerCheckingAccountItem, DTOs.Imputations.BrokerCheckingAccountItemDTO>(brokerCheckingAccountItem);
        }

        public static IEnumerable<DTOs.Imputations.BrokerCheckingAccountItemDTO> ToDTOs(this IEnumerable<BrokerCheckingAccountItem> brokerCheckingAccountItem)
        {
            return brokerCheckingAccountItem.Select(ToDTO);
        }

        public static IMapper CreateMapBrokersCheckingAccountTransactions()
        {
            var config = MapperCache.GetMapper<BrokersCheckingAccountTransaction, BrokersCheckingAccountTransactionDTO>(cfg =>
            {
                cfg.CreateMap<BrokersCheckingAccountTransaction, BrokersCheckingAccountTransactionDTO>();
                cfg.CreateMap<BrokersCheckingAccountTransactionItem, BrokersCheckingAccountTransactionItemDTO>();
                cfg.CreateMap<BrokerCheckingAccountItem, DTOs.Imputations.BrokerCheckingAccountItemDTO>();
                cfg.CreateMap<Policy, PolicyDTO>();
                cfg.CreateMap<Beneficiary, BeneficiaryDTO>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.BeneficiaryTypeDescription))
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (int)src.CustomerType));
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<Endorsement, EndorsementDTO>();
                cfg.CreateMap<PayerComponent, PayerComponentDTO>();
                cfg.CreateMap<PaymentPlan, PaymentPlanDTO>();
                cfg.CreateMap<Quota, QuotaDTO>();
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
                cfg.CreateMap<Prefix, PrefixDTO>();
                cfg.CreateMap<IssuanceAgency, IssuanceAgencyDTO>();
                cfg.CreateMap<IssuanceAgent, IssuanceAgentDTO>();
                cfg.CreateMap<IssuanceAgentType, IssuanceAgentTypeDTO>();
                cfg.CreateMap<IssuanceCommission, IssuanceCommissionDTO>();
                cfg.CreateMap<SubLineBusiness, SubLineBusinessDTO>();
                cfg.CreateMap<LineBusiness, LineBusinessDTO>();
                cfg.CreateMap<Holder, HolderDTO>()
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (int)src.CustomerType))
                .ForMember(dest => dest.IndividualType, opt => opt.MapFrom(src => (int)src.IndividualType));
                cfg.CreateMap<BillingGroup, BillingGroupDTO>();
                cfg.CreateMap<PolicyType, PolicyTypeDTO>();
                cfg.CreateMap<Prefix, PrefixDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<Company, DTOs.CompanyDTO>()
                .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName));
                cfg.CreateMap<ExchangeRate, ExchangeRateDTO>();
                cfg.CreateMap<CheckingAccountConcept, CheckingAccountConceptDTO>();
                cfg.CreateMap<Individual, IndividualDTO>();
                cfg.CreateMap<DocumentType, DocumentTypeDTO>();
                cfg.CreateMap<Application.UniquePersonService.V1.Models.EconomicActivity, EconomicActivityDTO>();
                cfg.CreateMap<IdentificationDocument, IdentificationDocumentDTO>();
                cfg.CreateMap<Agent, AgentDTO>();
                cfg.CreateMap<TransactionType, TransactionTypeDTO>();
                cfg.CreateMap<CheckingAccountTransaction, CheckingAccountTransactionDTO>();


            });
            return config;
        }

        public static BrokersCheckingAccountTransactionDTO ToDTO(this BrokersCheckingAccountTransaction brokersCheckingAccountTransaction)
        {
            var config = CreateMapBrokersCheckingAccountTransactions();
            return config.Map<BrokersCheckingAccountTransaction, BrokersCheckingAccountTransactionDTO>(brokersCheckingAccountTransaction);
        }

        public static IEnumerable<BrokersCheckingAccountTransactionDTO> ToDTOs(this IEnumerable<BrokersCheckingAccountTransaction> brokersCheckingAccountTransaction)
        {
            return brokersCheckingAccountTransaction.Select(ToDTO);
        }

        public static IMapper CreateMapBrokersCheckingAccountTransactionItems()
        {
            var config = MapperCache.GetMapper<BrokersCheckingAccountTransactionItem, BrokersCheckingAccountTransactionItemDTO>(cfg =>
            {
                cfg.CreateMap<BrokersCheckingAccountTransactionItem, BrokersCheckingAccountTransactionItemDTO>()
                        //.ForMember(dest => dest.AccountingNature, opt => opt.MapFrom(src => (int)src.AccountingNature))
                        ;
                cfg.CreateMap<Agent, AgentDTO>();
                cfg.CreateMap<BrokersCheckingAccountTransactionItem, BrokersCheckingAccountTransactionItemDTO>();
                cfg.CreateMap<BrokerCheckingAccountItem, DTOs.Imputations.BrokerCheckingAccountItemDTO>();
                cfg.CreateMap<Policy, PolicyDTO>();
                cfg.CreateMap<Beneficiary, BeneficiaryDTO>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.BeneficiaryTypeDescription))
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (int)src.CustomerType));
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<Endorsement, EndorsementDTO>();
                cfg.CreateMap<PayerComponent, PayerComponentDTO>();
                cfg.CreateMap<PaymentPlan, PaymentPlanDTO>();
                cfg.CreateMap<Quota, QuotaDTO>();
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
                cfg.CreateMap<Prefix, PrefixDTO>();
                cfg.CreateMap<IssuanceAgency, IssuanceAgencyDTO>();
                cfg.CreateMap<IssuanceAgent, IssuanceAgentDTO>();
                cfg.CreateMap<IssuanceAgentType, IssuanceAgentTypeDTO>();
                cfg.CreateMap<IssuanceCommission, IssuanceCommissionDTO>();
                cfg.CreateMap<SubLineBusiness, SubLineBusinessDTO>();
                cfg.CreateMap<LineBusiness, LineBusinessDTO>();
                cfg.CreateMap<Holder, HolderDTO>()
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (int)src.CustomerType))
                .ForMember(dest => dest.IndividualType, opt => opt.MapFrom(src => (int)src.IndividualType));
                cfg.CreateMap<BillingGroup, BillingGroupDTO>();
                cfg.CreateMap<PolicyType, PolicyTypeDTO>();
                cfg.CreateMap<Prefix, PrefixDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<TransactionType, TransactionTypeDTO>();
                cfg.CreateMap<Company, DTOs.CompanyDTO>()
                .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName));
                cfg.CreateMap<CheckingAccountTransaction, CheckingAccountTransactionDTO>();
                cfg.CreateMap<ExchangeRate, ExchangeRateDTO>();
                cfg.CreateMap<CheckingAccountConcept, CheckingAccountConceptDTO>();
                cfg.CreateMap<Individual, IndividualDTO>();
                cfg.CreateMap<DocumentType, DocumentTypeDTO>();
                cfg.CreateMap<Application.UniquePersonService.V1.Models.EconomicActivity, EconomicActivityDTO>();
                cfg.CreateMap<IdentificationDocument, IdentificationDocumentDTO>();
            });
            return config;
        }

        public static BrokersCheckingAccountTransactionItemDTO ToDTO(this BrokersCheckingAccountTransactionItem brokersCheckingAccountTransactionItem)
        {
            var config = CreateMapBrokersCheckingAccountTransactionItems();
            return config.Map<BrokersCheckingAccountTransactionItem, BrokersCheckingAccountTransactionItemDTO>(brokersCheckingAccountTransactionItem);
        }

        public static IEnumerable<BrokersCheckingAccountTransactionItemDTO> ToDTOs(this IEnumerable<BrokersCheckingAccountTransactionItem> brokersCheckingAccountTransactionItem)
        {
            return brokersCheckingAccountTransactionItem.Select(ToDTO);
        }

        public static IMapper CreateMapCheckingAccountConcepts()
        {
            var config = MapperCache.GetMapper<CheckingAccountConcept, CheckingAccountConceptDTO>(cfg =>
            {
                cfg.CreateMap<CheckingAccountConcept, CheckingAccountConceptDTO>();
            });
            return config;
        }

        public static CheckingAccountConceptDTO ToDTO(this CheckingAccountConcept checkingAccountConcept)
        {
            var config = CreateMapCheckingAccountConcepts();
            return config.Map<CheckingAccountConcept, CheckingAccountConceptDTO>(checkingAccountConcept);
        }

        public static IEnumerable<CheckingAccountConceptDTO> ToDTOs(this IEnumerable<CheckingAccountConcept> checkingAccountConcept)
        {
            return checkingAccountConcept.Select(ToDTO);
        }

        public static IMapper CreateMapCheckingAccountTransactions()
        {
            var config = MapperCache.GetMapper<CheckingAccountTransaction, CheckingAccountTransactionDTO>(cfg =>
            {
                cfg.CreateMap<CheckingAccountTransaction, CheckingAccountTransactionDTO>()
                        .ForMember(dest => dest.AccountingNature, opt => opt.MapFrom(src => (int)src.AccountingNature));
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
                cfg.CreateMap<Company, DTOs.CompanyDTO>()
                .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName));
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<ExchangeRate, ExchangeRateDTO>();
                cfg.CreateMap<CheckingAccountConcept, CheckingAccountConceptDTO>();
                cfg.CreateMap<Individual, IndividualDTO>();
                cfg.CreateMap<DocumentType, DocumentTypeDTO>();
                cfg.CreateMap<Application.UniquePersonService.V1.Models.EconomicActivity, EconomicActivityDTO>();
                cfg.CreateMap<IdentificationDocument, IdentificationDocumentDTO>();
            });
            return config;
        }

        public static CheckingAccountTransactionDTO ToDTO(this CheckingAccountTransaction checkingAccountTransaction)
        {
            var config = CreateMapCheckingAccountTransactions();
            return config.Map<CheckingAccountTransaction, CheckingAccountTransactionDTO>(checkingAccountTransaction);
        }

        public static IEnumerable<CheckingAccountTransactionDTO> ToDTOs(this IEnumerable<CheckingAccountTransaction> checkingAccountTransaction)
        {
            return checkingAccountTransaction.Select(ToDTO);
        }

        public static IMapper CreateMapClaimsPaymentRequestTransactions()
        {
            var config = MapperCache.GetMapper<ClaimsPaymentRequestTransaction, ClaimsPaymentRequestTransactionDTO>(cfg =>
            {
                cfg.CreateMap<ClaimsPaymentRequestTransaction, ClaimsPaymentRequestTransactionDTO>();
                cfg.CreateMap<ClaimsPaymentRequestTransactionItem, ClaimsPaymentRequestTransactionItemDTO>();
                cfg.CreateMap<PRCLAIM.PaymentRequestType, PaymentRequestTypeDTO>();
                cfg.CreateMap<CLAIMODL.Claim, ClaimDTO>()
                .ForMember(dest => dest.CoveredRiskType, opt => opt.MapFrom(src => (int)src.CoveredRiskType));
                cfg.CreateMap<Transaction, TransactionDTO>();
                cfg.CreateMap<Tax, TaxDTO>();
                cfg.CreateMap<TaxCondition, TaxConditionDTO>();
                cfg.CreateMap<TaxCategory, TaxCategoryDTO>();
                cfg.CreateMap<GeneralLedgerServices.EEProvider.Models.AccountingAccount, AccountingAccountDTO>();
                cfg.CreateMap<EEProvider.Models.Imputations.CostCenter, CostCenterDTO>();
                cfg.CreateMap<VoucherType, VoucherTypeDTO>();
                cfg.CreateMap<VoucherConcept, VoucherConceptDTO>();
                cfg.CreateMap<Voucher, VoucherDTO>();
                cfg.CreateMap<VoucherConceptTax, VoucherConceptTaxDTO>();
                cfg.CreateMap<PRCLAIM.ConceptSource, ConceptSourceDTO>();
                cfg.CreateMap<EEProvider.Models.Imputations.AccountingConcept, AccountingConceptDTO>();
                cfg.CreateMap<PRCLAIM.MovementType, MovementTypeDTO>();
                cfg.CreateMap<ExchangeRate, ExchangeRateDTO>();
                cfg.CreateMap<PaymentsModels.PaymentMethod, PaymentMethodDTO>();

                cfg.CreateMap<Individual, IndividualDTO>();
                cfg.CreateMap<DocumentType, DocumentTypeDTO>();
                cfg.CreateMap<Application.UniquePersonService.V1.Models.EconomicActivity, EconomicActivityDTO>();
                cfg.CreateMap<IdentificationDocument, IdentificationDocumentDTO>();

                cfg.CreateMap<PersonType, PersonTypeDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
                cfg.CreateMap<Company, CompanyDTO>()
                .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName));

                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<PaymentRequestNumber, PaymentRequestNumberDTO>();
                cfg.CreateMap<PaymentRequest, PaymentRequestDTO>();

                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<TransactionType, TransactionTypeDTO>();

                cfg.CreateMap<Currency, CurrencyDTO>();
            });
            return config;
        }

        public static ClaimsPaymentRequestTransactionDTO ToDTO(this ClaimsPaymentRequestTransaction claimsPaymentRequestTransaction)
        {
            var config = CreateMapClaimsPaymentRequestTransactions();
            return config.Map<ClaimsPaymentRequestTransaction, ClaimsPaymentRequestTransactionDTO>(claimsPaymentRequestTransaction);
        }

        public static IEnumerable<ClaimsPaymentRequestTransactionDTO> ToDTOs(this IEnumerable<ClaimsPaymentRequestTransaction> claimsPaymentRequestTransaction)
        {
            return claimsPaymentRequestTransaction.Select(ToDTO);
        }

        public static IMapper CreateMapClaimsPaymentRequestTransactionItems()
        {
            var config = MapperCache.GetMapper<ClaimsPaymentRequestTransactionItem, ClaimsPaymentRequestTransactionItemDTO>(cfg =>
            {
                cfg.CreateMap<ClaimsPaymentRequestTransactionItem, ClaimsPaymentRequestTransactionItemDTO>();
                cfg.CreateMap<PaymentRequest, PaymentRequestDTO>();
                cfg.CreateMap<PRCLAIM.PaymentRequestType, PaymentRequestTypeDTO>();
                cfg.CreateMap<CLAIMODL.Claim, ClaimDTO>()
                .ForMember(dest => dest.CoveredRiskType, opt => opt.MapFrom(src => (int)src.CoveredRiskType));
                cfg.CreateMap<Transaction, TransactionDTO>();
                cfg.CreateMap<Tax, TaxDTO>();
                cfg.CreateMap<TaxCondition, TaxConditionDTO>();
                cfg.CreateMap<TaxCategory, TaxCategoryDTO>();
                cfg.CreateMap<GeneralLedgerServices.EEProvider.Models.AccountingAccount, AccountingAccountDTO>();
                cfg.CreateMap<EEProvider.Models.Imputations.CostCenter, CostCenterDTO>();
                cfg.CreateMap<VoucherType, VoucherTypeDTO>();
                cfg.CreateMap<VoucherConcept, VoucherConceptDTO>();
                cfg.CreateMap<Voucher, VoucherDTO>();
                cfg.CreateMap<VoucherConceptTax, VoucherConceptTaxDTO>();
                cfg.CreateMap<PRCLAIM.ConceptSource, ConceptSourceDTO>();
                cfg.CreateMap<EEProvider.Models.Imputations.AccountingConcept, AccountingConceptDTO>();
                cfg.CreateMap<PRCLAIM.MovementType, MovementTypeDTO>();
                cfg.CreateMap<ExchangeRate, ExchangeRateDTO>();
                cfg.CreateMap<PaymentsModels.PaymentMethod, PaymentMethodDTO>();

                cfg.CreateMap<Individual, IndividualDTO>();
                cfg.CreateMap<DocumentType, DocumentTypeDTO>();
                cfg.CreateMap<Application.UniquePersonService.V1.Models.EconomicActivity, EconomicActivityDTO>();
                cfg.CreateMap<IdentificationDocument, IdentificationDocumentDTO>();

                cfg.CreateMap<PersonType, PersonTypeDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
                cfg.CreateMap<Company, CompanyDTO>()
                .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName));

                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<PaymentRequestNumber, PaymentRequestNumberDTO>();
                cfg.CreateMap<PaymentRequest, PaymentRequestDTO>();

                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<TransactionType, TransactionTypeDTO>();

                cfg.CreateMap<Currency, CurrencyDTO>();
            });
            return config;
        }

        public static ClaimsPaymentRequestTransactionItemDTO ToDTO(this ClaimsPaymentRequestTransactionItem claimsPaymentRequestTransactionItem)
        {
            var config = CreateMapClaimsPaymentRequestTransactionItems();
            return config.Map<ClaimsPaymentRequestTransactionItem, ClaimsPaymentRequestTransactionItemDTO>(claimsPaymentRequestTransactionItem);
        }

        public static IEnumerable<ClaimsPaymentRequestTransactionItemDTO> ToDTOs(this IEnumerable<ClaimsPaymentRequestTransactionItem> claimsPaymentRequestTransactionItem)
        {
            return claimsPaymentRequestTransactionItem.Select(ToDTO);
        }

        public static IMapper CreateMapCoInsuranceCheckingAccountItems()
        {
            var config = MapperCache.GetMapper<CoInsuranceCheckingAccountItem, CoInsuranceCheckingAccountItemDTO>(cfg =>
            {
                cfg.CreateMap<CoInsuranceCheckingAccountItem, CoInsuranceCheckingAccountItemDTO>();
            });
            return config;
        }

        public static CoInsuranceCheckingAccountItemDTO ToDTO(this CoInsuranceCheckingAccountItem coInsuranceCheckingAccountItem)
        {
            var config = CreateMapCoInsuranceCheckingAccountItems();
            return config.Map<CoInsuranceCheckingAccountItem, CoInsuranceCheckingAccountItemDTO>(coInsuranceCheckingAccountItem);
        }

        public static IEnumerable<CoInsuranceCheckingAccountItemDTO> ToDTOs(this IEnumerable<CoInsuranceCheckingAccountItem> coInsuranceCheckingAccountItem)
        {
            return coInsuranceCheckingAccountItem.Select(ToDTO);
        }

        public static IMapper CreateMapCoInsuranceCheckingAccountTransactions()
        {
            var config = MapperCache.GetMapper<CoInsuranceCheckingAccountTransaction, CoInsuranceCheckingAccountTransactionDTO>(cfg =>
            {
                cfg.CreateMap<CoInsuranceCheckingAccountTransaction, CoInsuranceCheckingAccountTransactionDTO>();
                cfg.CreateMap<CoInsuranceCheckingAccountTransactionItem, CoInsuranceCheckingAccountTransactionItemDTO>();


                cfg.CreateMap<Individual, IndividualDTO>();
                cfg.CreateMap<DocumentType, DocumentTypeDTO>();
                cfg.CreateMap<Application.UniquePersonService.V1.Models.EconomicActivity, EconomicActivityDTO>();
                cfg.CreateMap<IdentificationDocument, IdentificationDocumentDTO>();
                cfg.CreateMap<CoInsuranceCheckingAccountItem, CoInsuranceCheckingAccountItemDTO>();


                cfg.CreateMap<Policy, PolicyDTO>();
                cfg.CreateMap<Beneficiary, BeneficiaryDTO>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.BeneficiaryTypeDescription))
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (int)src.CustomerType));
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<Endorsement, EndorsementDTO>();
                cfg.CreateMap<PayerComponent, PayerComponentDTO>();
                cfg.CreateMap<PaymentPlan, PaymentPlanDTO>();
                cfg.CreateMap<Quota, QuotaDTO>();
                cfg.CreateMap<IssuanceAgency, IssuanceAgencyDTO>();
                cfg.CreateMap<IssuanceAgent, IssuanceAgentDTO>();
                cfg.CreateMap<IssuanceAgentType, IssuanceAgentTypeDTO>();
                cfg.CreateMap<IssuanceCommission, IssuanceCommissionDTO>();
                cfg.CreateMap<SubLineBusiness, SubLineBusinessDTO>();
                cfg.CreateMap<LineBusiness, LineBusinessDTO>();
                cfg.CreateMap<Holder, HolderDTO>()
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (int)src.CustomerType))
                .ForMember(dest => dest.IndividualType, opt => opt.MapFrom(src => (int)src.IndividualType));
                cfg.CreateMap<BillingGroup, BillingGroupDTO>();
                cfg.CreateMap<PolicyType, PolicyTypeDTO>();
                cfg.CreateMap<Prefix, PrefixDTO>();

                cfg.CreateMap<PersonType, PersonTypeDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
                cfg.CreateMap<Company, CompanyDTO>()
                .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName));
                cfg.CreateMap<CheckingAccountConcept, CheckingAccountConceptDTO>();
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<PaymentRequestNumber, PaymentRequestNumberDTO>();
                cfg.CreateMap<PaymentRequest, PaymentRequestDTO>();

                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<TransactionType, TransactionTypeDTO>();

            });
            return config;
        }

        public static CoInsuranceCheckingAccountTransactionDTO ToDTO(this CoInsuranceCheckingAccountTransaction coInsuranceCheckingAccountTransaction)
        {
            var config = CreateMapCoInsuranceCheckingAccountTransactions();
            return config.Map<CoInsuranceCheckingAccountTransaction, CoInsuranceCheckingAccountTransactionDTO>(coInsuranceCheckingAccountTransaction);
        }

        public static IEnumerable<CoInsuranceCheckingAccountTransactionDTO> ToDTOs(this IEnumerable<CoInsuranceCheckingAccountTransaction> coInsuranceCheckingAccountTransaction)
        {
            return coInsuranceCheckingAccountTransaction.Select(ToDTO);
        }

        public static IMapper CreateMapCoInsuranceCheckingAccountTransactionItems()
        {
            var config = MapperCache.GetMapper<CoInsuranceCheckingAccountTransactionItem, CoInsuranceCheckingAccountTransactionItemDTO>(cfg =>
            {
                cfg.CreateMap<CoInsuranceCheckingAccountTransactionItem, CoInsuranceCheckingAccountTransactionItemDTO>()
                        .ForMember(dest => dest.CoInsuranceType, opt => opt.MapFrom(src => (int)src.CoInsuranceType))
                        //.ForMember(dest => dest.AccountingNature, opt => opt.MapFrom(src => (int)src.AccountingNature))
                        ;
                cfg.CreateMap<Company, DTOs.CompanyDTO>()
                .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName));
                cfg.CreateMap<CoInsuranceCheckingAccountTransactionItem, CoInsuranceCheckingAccountTransactionItemDTO>();
                cfg.CreateMap<Policy, PolicyDTO>();
                cfg.CreateMap<Beneficiary, BeneficiaryDTO>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.BeneficiaryTypeDescription))
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (int)src.CustomerType));
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<Endorsement, EndorsementDTO>();
                cfg.CreateMap<PayerComponent, PayerComponentDTO>();
                cfg.CreateMap<PaymentPlan, PaymentPlanDTO>();
                cfg.CreateMap<Quota, QuotaDTO>();
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
                cfg.CreateMap<Prefix, PrefixDTO>();
                cfg.CreateMap<IssuanceAgency, IssuanceAgencyDTO>();
                cfg.CreateMap<IssuanceAgent, IssuanceAgentDTO>();
                cfg.CreateMap<IssuanceAgentType, IssuanceAgentTypeDTO>();
                cfg.CreateMap<IssuanceCommission, IssuanceCommissionDTO>();
                cfg.CreateMap<SubLineBusiness, SubLineBusinessDTO>();
                cfg.CreateMap<LineBusiness, LineBusinessDTO>();
                cfg.CreateMap<Holder, HolderDTO>()
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (int)src.CustomerType))
                .ForMember(dest => dest.IndividualType, opt => opt.MapFrom(src => (int)src.IndividualType));
                cfg.CreateMap<BillingGroup, BillingGroupDTO>();
                cfg.CreateMap<PolicyType, PolicyTypeDTO>();
                cfg.CreateMap<Prefix, PrefixDTO>();

                cfg.CreateMap<CoInsuranceCheckingAccountItem, CoInsuranceCheckingAccountItemDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<ExchangeRate, ExchangeRateDTO>();
                cfg.CreateMap<CheckingAccountConcept, CheckingAccountConceptDTO>();
                cfg.CreateMap<Individual, IndividualDTO>();
                cfg.CreateMap<DocumentType, DocumentTypeDTO>();
                cfg.CreateMap<Application.UniquePersonService.V1.Models.EconomicActivity, EconomicActivityDTO>();
                cfg.CreateMap<IdentificationDocument, IdentificationDocumentDTO>();
            });
            return config;
        }

        public static CoInsuranceCheckingAccountTransactionItemDTO ToDTO(this CoInsuranceCheckingAccountTransactionItem coInsuranceCheckingAccountTransactionItem)
        {
            var config = CreateMapCoInsuranceCheckingAccountTransactionItems();
            return config.Map<CoInsuranceCheckingAccountTransactionItem, CoInsuranceCheckingAccountTransactionItemDTO>(coInsuranceCheckingAccountTransactionItem);
        }

        public static IEnumerable<CoInsuranceCheckingAccountTransactionItemDTO> ToDTOs(this IEnumerable<CoInsuranceCheckingAccountTransactionItem> coInsuranceCheckingAccountTransactionItem)
        {
            return coInsuranceCheckingAccountTransactionItem.Select(ToDTO);
        }

        public static IMapper CreateMapDailyAccountingAnalysisCodes()
        {
            var config = MapperCache.GetMapper<DailyAccountingAnalysisCode, DailyAccountingAnalysisCodeDTO>(cfg =>
            {
                cfg.CreateMap<DailyAccountingAnalysisCode, DailyAccountingAnalysisCodeDTO>();
                cfg.CreateMap<EEProvider.Models.Imputations.AnalysisCode, AnalysisCodeDTO>();
                cfg.CreateMap<EEProvider.Models.Imputations.AnalysisConcept, AnalysisConceptDTO>();

            });
            return config;
        }

        public static DailyAccountingAnalysisCodeDTO ToDTO(this DailyAccountingAnalysisCode dailyAccountingAnalysisCode)
        {
            var config = CreateMapDailyAccountingAnalysisCodes();
            return config.Map<DailyAccountingAnalysisCode, DailyAccountingAnalysisCodeDTO>(dailyAccountingAnalysisCode);
        }

        public static IEnumerable<DailyAccountingAnalysisCodeDTO> ToDTOs(this IEnumerable<DailyAccountingAnalysisCode> dailyAccountingAnalysisCode)
        {
            return dailyAccountingAnalysisCode.Select(ToDTO);
        }

        public static IMapper CreateMapApplicationAccountingAnalysis()
        {
            var config = MapperCache.GetMapper<ApplicationAccountingAnalysis, ApplicationAccountingAnalysisDTO>(cfg =>
            {
                cfg.CreateMap<ApplicationAccountingAnalysis, ApplicationAccountingAnalysisDTO>();
                cfg.CreateMap<EEProvider.Models.Imputations.AnalysisCode, AnalysisCodeDTO>();
                cfg.CreateMap<EEProvider.Models.Imputations.AnalysisConcept, AnalysisConceptDTO>();

            });
            return config;
        }

        public static ApplicationAccountingAnalysisDTO ToDTO(this ApplicationAccountingAnalysis applicationAccountingAnalysis)
        {
            var config = CreateMapApplicationAccountingAnalysis();
            return config.Map<ApplicationAccountingAnalysis, ApplicationAccountingAnalysisDTO>(applicationAccountingAnalysis);
        }

        public static IEnumerable<ApplicationAccountingAnalysisDTO> ToDTOs(this IEnumerable<ApplicationAccountingAnalysis> applicationAccountingAnalysis)
        {
            return applicationAccountingAnalysis.Select(ToDTO);
        }

        public static IMapper CreateMapApplicationAccountingCostCenters()
        {
            var config = MapperCache.GetMapper<DailyAccountingCostCenter, ApplicationAccountingCostCenterDTO>(cfg =>
            {
                cfg.CreateMap<ApplicationAccountingCostCenter, ApplicationAccountingCostCenterDTO>();
                cfg.CreateMap<CostCenter, CostCenterDTO>();
            });
            return config;
        }

        public static ApplicationAccountingCostCenterDTO ToDTO(this ApplicationAccountingCostCenter applicationAccountingCostCenter)
        {
            var config = CreateMapApplicationAccountingCostCenters();
            return config.Map<ApplicationAccountingCostCenter, ApplicationAccountingCostCenterDTO>(applicationAccountingCostCenter);
        }

        public static IEnumerable<ApplicationAccountingCostCenterDTO> ToDTOs(this IEnumerable<ApplicationAccountingCostCenter> applicationAccountingCostCenter)
        {
            return applicationAccountingCostCenter.Select(ToDTO);
        }

        public static IMapper CreateMapDailyAccountingCostCenters()
        {
            var config = MapperCache.GetMapper<DailyAccountingCostCenter, DailyAccountingCostCenterDTO>(cfg =>
            {
                cfg.CreateMap<DailyAccountingCostCenter, DailyAccountingCostCenterDTO>();
                cfg.CreateMap<EEProvider.Models.Imputations.CostCenter, CostCenterDTO>();
            });
            return config;
        }

        public static DailyAccountingCostCenterDTO ToDTO(this DailyAccountingCostCenter dailyAccountingCostCenter)
        {
            var config = CreateMapDailyAccountingCostCenters();
            return config.Map<DailyAccountingCostCenter, DailyAccountingCostCenterDTO>(dailyAccountingCostCenter);
        }

        public static IEnumerable<DailyAccountingCostCenterDTO> ToDTOs(this IEnumerable<DailyAccountingCostCenter> dailyAccountingCostCenter)
        {
            return dailyAccountingCostCenter.Select(ToDTO);
        }

        public static IMapper CreateMapDailyAccountingTransactions()
        {
            var config = MapperCache.GetMapper<DailyAccountingTransaction, DailyAccountingTransactionDTO>(cfg =>
            {
                cfg.CreateMap<DailyAccountingTransaction, DailyAccountingTransactionDTO>();
                cfg.CreateMap<DailyAccountingTransactionItem, DailyAccountingTransactionItemDTO>()
                        .ForMember(dest => dest.AccountingNature, opt => opt.MapFrom(src => (int)src.AccountingNature));
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
                cfg.CreateMap<Company, DTOs.CompanyDTO>()
                .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName));
                cfg.CreateMap<Individual, IndividualDTO>();
                cfg.CreateMap<DocumentType, DocumentTypeDTO>();
                cfg.CreateMap<UniquePersonService.V1.Models.EconomicActivity, EconomicActivityDTO>();
                cfg.CreateMap<IdentificationDocument, IdentificationDocumentDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<ExchangeRate, ExchangeRateDTO>();
                cfg.CreateMap<BookAccount, BookAccountDTO>();
                cfg.CreateMap<DailyAccountingAnalysisCode, DailyAccountingAnalysisCodeDTO>();
                cfg.CreateMap<DailyAccountingCostCenter, DailyAccountingCostCenterDTO>();
                cfg.CreateMap<TransactionType, TransactionTypeDTO>();
                cfg.CreateMap<EEProvider.Models.Imputations.CostCenter, CostCenterDTO>();
                cfg.CreateMap<EEProvider.Models.Imputations.AnalysisConcept, AnalysisConceptDTO>();
                cfg.CreateMap<EEProvider.Models.Imputations.AnalysisCode, AnalysisCodeDTO>();
            });
            return config;
        }

        public static DailyAccountingTransactionDTO ToDTO(this DailyAccountingTransaction dailyAccountingTransaction)
        {
            var config = CreateMapDailyAccountingTransactions();
            return config.Map<DailyAccountingTransaction, DailyAccountingTransactionDTO>(dailyAccountingTransaction);
        }

        public static IEnumerable<DailyAccountingTransactionDTO> ToDTOs(this IEnumerable<DailyAccountingTransaction> dailyAccountingTransaction)
        {
            return dailyAccountingTransaction.Select(ToDTO);
        }

        public static IMapper CreateMapDailyAccountingTransactionItems()
        {
            var config = MapperCache.GetMapper<DailyAccountingTransactionItem, DailyAccountingTransactionItemDTO>(cfg =>
            {
                cfg.CreateMap<DailyAccountingTransactionItem, DailyAccountingTransactionItemDTO>()
                        .ForMember(dest => dest.AccountingNature, opt => opt.MapFrom(src => (int)src.AccountingNature));
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
                cfg.CreateMap<Company, DTOs.CompanyDTO>()
                .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName));
                cfg.CreateMap<Individual, IndividualDTO>();
                cfg.CreateMap<DocumentType, DocumentTypeDTO>();
                cfg.CreateMap<Application.UniquePersonService.V1.Models.EconomicActivity, EconomicActivityDTO>();
                cfg.CreateMap<IdentificationDocument, IdentificationDocumentDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<ExchangeRate, ExchangeRateDTO>();
                cfg.CreateMap<BookAccount, BookAccountDTO>();
                cfg.CreateMap<DailyAccountingAnalysisCode, DailyAccountingAnalysisCodeDTO>();
                cfg.CreateMap<DailyAccountingCostCenter, DailyAccountingCostCenterDTO>();
                cfg.CreateMap<TransactionType, TransactionTypeDTO>();
                cfg.CreateMap<EEProvider.Models.Imputations.CostCenter, CostCenterDTO>();
                cfg.CreateMap<EEProvider.Models.Imputations.AnalysisConcept, AnalysisConceptDTO>();
                cfg.CreateMap<EEProvider.Models.Imputations.AnalysisCode, AnalysisCodeDTO>();
            });
            return config;
        }

        public static DailyAccountingTransactionItemDTO ToDTO(this DailyAccountingTransactionItem dailyAccountingTransactionItem)
        {
            var config = CreateMapDailyAccountingTransactionItems();
            return config.Map<DailyAccountingTransactionItem, DailyAccountingTransactionItemDTO>(dailyAccountingTransactionItem);
        }

        public static IMapper CreateMapTempApplicationPremiumComponent()
        {
            var config = MapperCache.GetMapper<TempApplicationPremiumComponent, TempApplicationPremiumComponentDTO>(cfg =>
            {
                cfg.CreateMap<TempApplicationPremiumComponent, TempApplicationPremiumComponentDTO>();
            });
            return config;
        }
        public static TempApplicationPremiumComponentDTO ToDTO(this TempApplicationPremiumComponent tempApplicationPremiumComponent)
        {
            var config = CreateMapTempApplicationPremiumComponent();
            return config.Map<TempApplicationPremiumComponent, TempApplicationPremiumComponentDTO>(tempApplicationPremiumComponent);
        }
        public static List<TempApplicationPremiumComponentDTO> ToDTOs(this List<TempApplicationPremiumComponent> tempApplicationPremiumComponents)
        {
            return tempApplicationPremiumComponents.Select(m => m.ToDTO()).ToList();
        }

        public static IMapper CreateMapApplicationPremium()
        {
            var config = MapperCache.GetMapper<ApplicationPremium, ApplicationPremiumDTO>(cfg =>
            {
                cfg.CreateMap<ApplicationPremium, ApplicationPremiumDTO>();
            });
            return config;
        }

        public static ApplicationPremiumDTO ToDTO(this ApplicationPremium applicationPremium)
        {
            var config = CreateMapApplicationPremium();
            return config.Map<ApplicationPremium, ApplicationPremiumDTO>(applicationPremium);
        }
        public static List<ApplicationPremiumDTO> ToDTOs(this List<ApplicationPremium> applicationPremiums)
        {
            return applicationPremiums.Select(m => m.ToDTO()).ToList();
        }

        public static IEnumerable<DailyAccountingTransactionItemDTO> ToDTOs(this IEnumerable<DailyAccountingTransactionItem> dailyAccountingTransactionItem)
        {
            return dailyAccountingTransactionItem.Select(ToDTO);
        }

        public static IMapper CreateMapDepositPremiumTransactions()
        {
            var config = MapperCache.GetMapper<DepositPremiumTransaction, DTOs.Imputations.DepositPremiumTransactionDTO>(cfg =>
            {
                cfg.CreateMap<DepositPremiumTransaction, DTOs.Imputations.DepositPremiumTransactionDTO>();
                cfg.CreateMap<Collect, CollectDTO>()
                        .ForMember(dest => dest.CollectType, opt => opt.MapFrom(src => (int)src.CollectType))
                        .ForMember(dest => dest.AccountingCompany, opt => opt.MapFrom(src => new CompanyDTO { IndividualId = src.AccountingCompany.IndividualId, Name = src.AccountingCompany.FullName }));
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
                cfg.CreateMap<CollectConcept, CollectConceptDTO>();
                cfg.CreateMap<PaymentsModels.Payment, PaymentDTO>();
                cfg.CreateMap<PaymentsModels.PaymentMethod, PaymentMethodDTO>();
                cfg.CreateMap<ExchangeRate, ExchangeRateDTO>();
                cfg.CreateMap<PaymentTax, PaymentTaxDTO>();
                cfg.CreateMap<Tax, TaxDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<Person, PersonDTO>()
                .ForMember(dest => dest.IndividualType, opt => opt.MapFrom(src => (int)src.IndividualType))
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (int)src.CustomerType));
                cfg.CreateMap<DocumentType, DocumentTypeDTO>();
                cfg.CreateMap<IdentificationDocument, DTOs.IdentificationDocumentDTO>();
                cfg.CreateMap<Company, DTOs.CompanyDTO>()
                .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName));
                cfg.CreateMap<Transaction, TransactionDTO>();
                cfg.CreateMap<PersonType, PersonTypeDTO>();
            });
            return config;
        }

        public static DTOs.Imputations.DepositPremiumTransactionDTO ToDTO(this DepositPremiumTransaction depositPremiumTransaction)
        {
            var config = CreateMapDepositPremiumTransactions();
            return config.Map<DepositPremiumTransaction, DTOs.Imputations.DepositPremiumTransactionDTO>(depositPremiumTransaction);
        }

        public static IEnumerable<DTOs.Imputations.DepositPremiumTransactionDTO> ToDTOs(this IEnumerable<DepositPremiumTransaction> depositPremiumTransaction)
        {
            return depositPremiumTransaction.Select(ToDTO);
        }

        public static IMapper CreateMapInsuredLoanTransactions()
        {
            var config = MapperCache.GetMapper<InsuredLoanTransaction, InsuredLoanTransactionDTO>(cfg =>
            {
                cfg.CreateMap<InsuredLoanTransaction, InsuredLoanTransactionDTO>();
                cfg.CreateMap<InsuredLoanTransactionItem, InsuredLoanTransactionItemDTO>();
                cfg.CreateMap<TransactionType, TransactionTypeDTO>();
                cfg.CreateMap<ExchangeRate, ExchangeRateDTO>();
                cfg.CreateMap<Individual, IndividualDTO>();
                cfg.CreateMap<DocumentType, DocumentTypeDTO>();
                cfg.CreateMap<UniquePersonService.V1.Models.EconomicActivity, EconomicActivityDTO>();
                cfg.CreateMap<IdentificationDocument, IdentificationDocumentDTO>();
                cfg.CreateMap<EEProvider.Models.Imputations.Application, ApplicationDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
            });
            return config;
        }

        public static InsuredLoanTransactionDTO ToDTO(this InsuredLoanTransaction insuredLoanTransaction)
        {
            var config = CreateMapInsuredLoanTransactions();
            return config.Map<InsuredLoanTransaction, InsuredLoanTransactionDTO>(insuredLoanTransaction);
        }

        public static IEnumerable<InsuredLoanTransactionDTO> ToDTOs(this IEnumerable<InsuredLoanTransaction> insuredLoanTransaction)
        {
            return insuredLoanTransaction.Select(ToDTO);
        }

        public static IMapper CreateMapJournalEntries()
        {
            var config = MapperCache.GetMapper<JournalEntry, JournalEntryDTO>(cfg =>
            {
                cfg.CreateMap<JournalEntry, JournalEntryDTO>();
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
                cfg.CreateMap<Company, DTOs.CompanyDTO>()
                .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName));
                cfg.CreateMap<Individual, IndividualDTO>();
                cfg.CreateMap<DocumentType, DocumentTypeDTO>();
                cfg.CreateMap<Application.UniquePersonService.V1.Models.EconomicActivity, EconomicActivityDTO>();
                cfg.CreateMap<IdentificationDocument, IdentificationDocumentDTO>();
                cfg.CreateMap<PersonType, PersonTypeDTO>();
                cfg.CreateMap<EEProvider.Models.Imputations.Application, ApplicationDTO>()
                //.ForMember(dest => dest.ImputationType, opt => opt.MapFrom(src => (int)src.ImputationType))
                ;
                cfg.CreateMap<TransactionType, TransactionTypeDTO>();
                cfg.CreateMap<Transaction, TransactionDTO>();
            });
            return config;
        }

        public static JournalEntryDTO ToDTO(this EEProvider.Models.Imputations.JournalEntry journalEntry)
        {
            var config = CreateMapJournalEntries();
            return config.Map<Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations.JournalEntry, JournalEntryDTO>(journalEntry);
        }

        public static IEnumerable<JournalEntryDTO> ToDTOs(this IEnumerable<EEProvider.Models.Imputations.JournalEntry> journalEntry)
        {
            return journalEntry.Select(ToDTO);
        }

        public static IMapper CreateMapPaymentRequestTransactions()
        {
            var config = MapperCache.GetMapper<PaymentRequestTransaction, PaymentRequestTransactionDTO>(cfg =>
            {
                cfg.CreateMap<PaymentRequestTransaction, PaymentRequestTransactionDTO>();
                cfg.CreateMap<PaymentRequestTransactionItem, PaymentRequestTransactionItemDTO>();
                cfg.CreateMap<TransactionType, TransactionTypeDTO>();
                cfg.CreateMap<PaymentRequest, PaymentRequestDTO>();
                cfg.CreateMap<PaymentRequestNumber, PaymentRequestNumberDTO>();
                cfg.CreateMap<EEProvider.Models.Claims.PaymentRequest.ConceptSource, ConceptSourceDTO>();
                cfg.CreateMap<PRCLAIM.MovementType, MovementTypeDTO>();
                cfg.CreateMap<Company, CompanyDTO>()
                .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName));
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
                cfg.CreateMap<PersonType, PersonTypeDTO>();
                cfg.CreateMap<Individual, IndividualDTO>();
                cfg.CreateMap<DocumentType, DocumentTypeDTO>();
                cfg.CreateMap<Application.UniquePersonService.V1.Models.EconomicActivity, EconomicActivityDTO>();
                cfg.CreateMap<IdentificationDocument, IdentificationDocumentDTO>();
                cfg.CreateMap<PaymentsModels.PaymentMethod, PaymentMethodDTO>();
                cfg.CreateMap<ExchangeRate, ExchangeRateDTO>();
                cfg.CreateMap<TaxCategory, TaxCategoryDTO>();
                cfg.CreateMap<Tax, TaxDTO>();
                cfg.CreateMap<VoucherConceptTax, VoucherConceptTaxDTO>();
                cfg.CreateMap<EEProvider.Models.Imputations.CostCenter, CostCenterDTO>();
                cfg.CreateMap<GeneralLedgerServices.EEProvider.Models.AccountingAccount, AccountingAccountDTO>();
                cfg.CreateMap<EEProvider.Models.Imputations.AccountingConcept, AccountingConceptDTO>();
                cfg.CreateMap<VoucherConcept, VoucherConceptDTO>();
                cfg.CreateMap<VoucherType, VoucherTypeDTO>();
                cfg.CreateMap<Voucher, VoucherDTO>();
                cfg.CreateMap<TaxCondition, TaxConditionDTO>();
                cfg.CreateMap<PRCLAIM.PaymentRequestType, PaymentRequestTypeDTO>();
                cfg.CreateMap<CLAIMODL.Claim, ClaimDTO>()
                .ForMember(dest => dest.CoveredRiskType, opt => opt.MapFrom(src => (int)src.CoveredRiskType));
                cfg.CreateMap<Transaction, TransactionDTO>();
                cfg.CreateMap<Company, CompanyDTO>()
                .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName));
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
            });
            return config;
        }

        public static PaymentRequestTransactionDTO ToDTO(this PaymentRequestTransaction paymentRequestTransaction)
        {
            var config = CreateMapPaymentRequestTransactions();
            return config.Map<PaymentRequestTransaction, PaymentRequestTransactionDTO>(paymentRequestTransaction);
        }

        public static IEnumerable<PaymentRequestTransactionDTO> ToDTOs(this IEnumerable<PaymentRequestTransaction> paymentRequestTransaction)
        {
            return paymentRequestTransaction.Select(ToDTO);
        }

        public static IMapper CreateMapPaymentRequestTransactionItems()
        {
            var config = MapperCache.GetMapper<PaymentRequestTransactionItem, PaymentRequestTransactionItemDTO>(cfg =>
            {
                cfg.CreateMap<PaymentRequestTransactionItem, PaymentRequestTransactionItemDTO>();
                cfg.CreateMap<TransactionType, TransactionTypeDTO>();
                cfg.CreateMap<PaymentRequest, PaymentRequestDTO>();
                cfg.CreateMap<PaymentRequestNumber, PaymentRequestNumberDTO>();
                cfg.CreateMap<EEProvider.Models.Claims.PaymentRequest.ConceptSource, ConceptSourceDTO>();
                cfg.CreateMap<PRCLAIM.MovementType, MovementTypeDTO>();
                cfg.CreateMap<Company, CompanyDTO>()
                .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName));
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
                cfg.CreateMap<PersonType, PersonTypeDTO>();
                cfg.CreateMap<Individual, IndividualDTO>();
                cfg.CreateMap<DocumentType, DocumentTypeDTO>();
                cfg.CreateMap<Application.UniquePersonService.V1.Models.EconomicActivity, EconomicActivityDTO>();
                cfg.CreateMap<IdentificationDocument, IdentificationDocumentDTO>();
                cfg.CreateMap<PaymentsModels.PaymentMethod, PaymentMethodDTO>();
                cfg.CreateMap<ExchangeRate, ExchangeRateDTO>();
                cfg.CreateMap<TaxCategory, TaxCategoryDTO>();
                cfg.CreateMap<Tax, TaxDTO>();

                cfg.CreateMap<VoucherConceptTax, VoucherConceptTaxDTO>();
                cfg.CreateMap<EEProvider.Models.Imputations.CostCenter, CostCenterDTO>();
                cfg.CreateMap<GeneralLedgerServices.EEProvider.Models.AccountingAccount, AccountingAccountDTO>();
                cfg.CreateMap<EEProvider.Models.Imputations.AccountingConcept, AccountingConceptDTO>();
                cfg.CreateMap<VoucherConcept, VoucherConceptDTO>();
                cfg.CreateMap<VoucherType, VoucherTypeDTO>();
                cfg.CreateMap<Voucher, VoucherDTO>();
                cfg.CreateMap<TaxCondition, TaxConditionDTO>();
                cfg.CreateMap<PRCLAIM.PaymentRequestType, PaymentRequestTypeDTO>();
                cfg.CreateMap<CLAIMODL.Claim, ClaimDTO>()
                .ForMember(dest => dest.CoveredRiskType, opt => opt.MapFrom(src => (int)src.CoveredRiskType));
                cfg.CreateMap<Transaction, TransactionDTO>();
                cfg.CreateMap<Company, CompanyDTO>()
                .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName));
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
            });
            return config;
        }

        public static PaymentRequestTransactionItemDTO ToDTO(this PaymentRequestTransactionItem paymentRequestTransactionItem)
        {
            var config = CreateMapPaymentRequestTransactionItems();
            return config.Map<PaymentRequestTransactionItem, PaymentRequestTransactionItemDTO>(paymentRequestTransactionItem);
        }

        public static IEnumerable<PaymentRequestTransactionItemDTO> ToDTOs(this IEnumerable<PaymentRequestTransactionItem> paymentRequestTransactionItem)
        {
            return paymentRequestTransactionItem.Select(ToDTO);
        }

        public static IMapper CreateMapPreLiquidations()
        {
            var config = MapperCache.GetMapper<PreLiquidation, PreLiquidationDTO>(cfg =>
            {
                cfg.CreateMap<PreLiquidation, PreLiquidationDTO>();
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
                cfg.CreateMap<Company, DTOs.CompanyDTO>()
                .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName));
                cfg.CreateMap<Individual, IndividualDTO>();
                cfg.CreateMap<DocumentType, DocumentTypeDTO>();
                cfg.CreateMap<Application.UniquePersonService.V1.Models.EconomicActivity, EconomicActivityDTO>();
                cfg.CreateMap<IdentificationDocument, IdentificationDocumentDTO>();
                cfg.CreateMap<PersonType, PersonTypeDTO>();
                cfg.CreateMap<EEProvider.Models.Imputations.Application, ApplicationDTO>()
                //.ForMember(dest => dest.ImputationType, opt => opt.MapFrom(src => (int)src.ImputationType))
                ;
                cfg.CreateMap<TransactionType, TransactionTypeDTO>();
            });
            return config;
        }

        public static PreLiquidationDTO ToDTO(this PreLiquidation preLiquidation)
        {
            var config = CreateMapPreLiquidations();
            return config.Map<PreLiquidation, PreLiquidationDTO>(preLiquidation);
        }

        public static IEnumerable<PreLiquidationDTO> ToDTOs(this IEnumerable<PreLiquidation> preLiquidation)
        {
            return preLiquidation.Select(ToDTO);
        }

        public static IMapper CreateMapPremiumReceivableTransactions()
        {
            var config = MapperCache.GetMapper<PremiumReceivableTransaction, PremiumReceivableTransactionDTO>(cfg =>
            {
                cfg.CreateMap<PremiumReceivableTransaction, PremiumReceivableTransactionDTO>();
                cfg.CreateMap<PremiumReceivableTransactionItem, PremiumReceivableTransactionItemDTO>();
                cfg.CreateMap<TransactionType, TransactionTypeDTO>();
                cfg.CreateMap<Policy, PolicyDTO>();
                cfg.CreateMap<Beneficiary, BeneficiaryDTO>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.BeneficiaryTypeDescription))
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (int)src.CustomerType));
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<Endorsement, EndorsementDTO>();
                cfg.CreateMap<PayerComponent, PayerComponentDTO>();
                cfg.CreateMap<PaymentPlan, PaymentPlanDTO>();
                cfg.CreateMap<Quota, QuotaDTO>();
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
                cfg.CreateMap<Prefix, PrefixDTO>();
                cfg.CreateMap<IssuanceAgency, IssuanceAgencyDTO>();
                cfg.CreateMap<IssuanceAgent, IssuanceAgentDTO>();
                cfg.CreateMap<IssuanceAgentType, IssuanceAgentTypeDTO>();
                cfg.CreateMap<IssuanceCommission, IssuanceCommissionDTO>();
                cfg.CreateMap<SubLineBusiness, SubLineBusinessDTO>();
                cfg.CreateMap<LineBusiness, LineBusinessDTO>();
                cfg.CreateMap<Holder, HolderDTO>()
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (int)src.CustomerType))
                .ForMember(dest => dest.IndividualType, opt => opt.MapFrom(src => (int)src.IndividualType));
                cfg.CreateMap<BillingGroup, BillingGroupDTO>();
                cfg.CreateMap<PolicyType, PolicyTypeDTO>();
                cfg.CreateMap<Prefix, PrefixDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
            });
            return config;
        }

        public static PremiumReceivableTransactionDTO ToDTO(this PremiumReceivableTransaction premiumReceivableTransaction)
        {
            var config = CreateMapPremiumReceivableTransactions();
            return config.Map<PremiumReceivableTransaction, PremiumReceivableTransactionDTO>(premiumReceivableTransaction);
        }

        public static IEnumerable<PremiumReceivableTransactionDTO> ToDTOs(this IEnumerable<PremiumReceivableTransaction> premiumReceivableTransaction)
        {
            return premiumReceivableTransaction.Select(ToDTO);
        }

        public static IMapper CreateMapPremiumReceivableTransactionItems()
        {
            var config = MapperCache.GetMapper<PremiumReceivableTransactionItem, PremiumReceivableTransactionItemDTO>(cfg =>
            {
                cfg.CreateMap<PremiumReceivableTransactionItem, PremiumReceivableTransactionItemDTO>();
                cfg.CreateMap<Policy, PolicyDTO>();
                cfg.CreateMap<Beneficiary, BeneficiaryDTO>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.BeneficiaryTypeDescription))
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (int)src.CustomerType));
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<Endorsement, EndorsementDTO>();
                cfg.CreateMap<PayerComponent, PayerComponentDTO>();
                cfg.CreateMap<PaymentPlan, PaymentPlanDTO>();
                cfg.CreateMap<Quota, QuotaDTO>();
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
                cfg.CreateMap<Prefix, PrefixDTO>();
                cfg.CreateMap<IssuanceAgency, IssuanceAgencyDTO>();
                cfg.CreateMap<IssuanceAgent, IssuanceAgentDTO>();
                cfg.CreateMap<IssuanceAgentType, IssuanceAgentTypeDTO>();
                cfg.CreateMap<IssuanceCommission, IssuanceCommissionDTO>();
                cfg.CreateMap<SubLineBusiness, SubLineBusinessDTO>();
                cfg.CreateMap<LineBusiness, LineBusinessDTO>();
                cfg.CreateMap<Holder, HolderDTO>()
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (int)src.CustomerType))
                .ForMember(dest => dest.IndividualType, opt => opt.MapFrom(src => (int)src.IndividualType));
                cfg.CreateMap<BillingGroup, BillingGroupDTO>();
                cfg.CreateMap<PolicyType, PolicyTypeDTO>();
                cfg.CreateMap<Prefix, PrefixDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
            });
            return config;
        }

        public static PremiumReceivableTransactionItemDTO ToDTO(this PremiumReceivableTransactionItem premiumReceivableTransactionItem)
        {
            var config = CreateMapPremiumReceivableTransactionItems();
            return config.Map<PremiumReceivableTransactionItem, PremiumReceivableTransactionItemDTO>(premiumReceivableTransactionItem);
        }

        public static IEnumerable<PremiumReceivableTransactionItemDTO> ToDTOs(this IEnumerable<PremiumReceivableTransactionItem> premiumReceivableTransactionItem)
        {
            return premiumReceivableTransactionItem.Select(ToDTO);
        }

        public static IMapper CreateMapReinsuranceCheckingAccountItems()
        {
            var config = MapperCache.GetMapper<ReinsuranceCheckingAccountItem, DTOs.Imputations.ReinsuranceCheckingAccountItemDTO>(cfg =>
            {
                cfg.CreateMap<ReinsuranceCheckingAccountItem, DTOs.Imputations.ReinsuranceCheckingAccountItemDTO>();
            });
            return config;
        }

        public static DTOs.Imputations.ReinsuranceCheckingAccountItemDTO ToDTO(this ReinsuranceCheckingAccountItem reinsuranceCheckingAccountItem)
        {
            var config = CreateMapReinsuranceCheckingAccountItems();
            return config.Map<ReinsuranceCheckingAccountItem, DTOs.Imputations.ReinsuranceCheckingAccountItemDTO>(reinsuranceCheckingAccountItem);
        }

        public static IEnumerable<DTOs.Imputations.ReinsuranceCheckingAccountItemDTO> ToDTOs(this IEnumerable<ReinsuranceCheckingAccountItem> reinsuranceCheckingAccountItem)
        {
            return reinsuranceCheckingAccountItem.Select(ToDTO);
        }

        public static IMapper CreateMapReInsuranceCheckingAccountTransactions()
        {
            var config = MapperCache.GetMapper<ReInsuranceCheckingAccountTransaction, ReInsuranceCheckingAccountTransactionDTO>(cfg =>
            {
                cfg.CreateMap<ReInsuranceCheckingAccountTransaction, ReInsuranceCheckingAccountTransactionDTO>();
                cfg.CreateMap<ReInsuranceCheckingAccountTransactionItem, ReInsuranceCheckingAccountTransactionItemDTO>();
                cfg.CreateMap<TransactionType, TransactionTypeDTO>();
                cfg.CreateMap<ReinsuranceCheckingAccountItem, DTOs.Imputations.ReinsuranceCheckingAccountItemDTO>();
                cfg.CreateMap<Company, CompanyDTO>()
                .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName));
                cfg.CreateMap<Prefix, PrefixDTO>();
                cfg.CreateMap<CheckingAccountTransaction, CheckingAccountTransactionDTO>();
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
                cfg.CreateMap<ExchangeRate, ExchangeRateDTO>();
                cfg.CreateMap<CheckingAccountConcept, CheckingAccountConceptDTO>();
                cfg.CreateMap<Individual, IndividualDTO>();
                cfg.CreateMap<DocumentType, DocumentTypeDTO>();
                cfg.CreateMap<UniquePersonService.V1.Models.EconomicActivity, EconomicActivityDTO>();
                cfg.CreateMap<IdentificationDocument, IdentificationDocumentDTO>();
                cfg.CreateMap<Amount, AmountDTO>();

                cfg.CreateMap<Currency, CurrencyDTO>();
            });
            return config;
        }

        public static ReInsuranceCheckingAccountTransactionDTO ToDTO(this ReInsuranceCheckingAccountTransaction reInsuranceCheckingAccountTransaction)
        {
            var config = CreateMapReInsuranceCheckingAccountTransactions();
            return config.Map<ReInsuranceCheckingAccountTransaction, ReInsuranceCheckingAccountTransactionDTO>(reInsuranceCheckingAccountTransaction);
        }

        public static IEnumerable<ReInsuranceCheckingAccountTransactionDTO> ToDTOs(this IEnumerable<ReInsuranceCheckingAccountTransaction> reInsuranceCheckingAccountTransaction)
        {
            return reInsuranceCheckingAccountTransaction.Select(ToDTO);
        }

        public static IMapper CreateMapReInsuranceCheckingAccountTransactionItems()
        {
            var config = MapperCache.GetMapper<ReInsuranceCheckingAccountTransactionItem, ReInsuranceCheckingAccountTransactionItemDTO>(cfg =>
            {
                cfg.CreateMap<ReInsuranceCheckingAccountTransactionItem, ReInsuranceCheckingAccountTransactionItemDTO>()
                        //.ForMember(dest => dest.AccountingNature, opt => opt.MapFrom(src => (int)src.AccountingNature))
                        ;
                cfg.CreateMap<Company, DTOs.CompanyDTO>()
                .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName));
                cfg.CreateMap<Prefix, PrefixDTO>();
                cfg.CreateMap<ReInsuranceCheckingAccountTransactionItem, ReInsuranceCheckingAccountTransactionItemDTO>();
                cfg.CreateMap<ReinsuranceCheckingAccountItem, DTOs.Imputations.ReinsuranceCheckingAccountItemDTO>();
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<ExchangeRate, ExchangeRateDTO>();
                cfg.CreateMap<CheckingAccountConcept, CheckingAccountConceptDTO>();
                cfg.CreateMap<Individual, IndividualDTO>();
                cfg.CreateMap<DocumentType, DocumentTypeDTO>();
                cfg.CreateMap<UniquePersonService.V1.Models.EconomicActivity, EconomicActivityDTO>();
                cfg.CreateMap<IdentificationDocument, IdentificationDocumentDTO>();
            });
            return config;
        }

        public static ReInsuranceCheckingAccountTransactionItemDTO ToDTO(this ReInsuranceCheckingAccountTransactionItem reInsuranceCheckingAccountTransactionItem)
        {
            var config = CreateMapReInsuranceCheckingAccountTransactionItems();
            return config.Map<ReInsuranceCheckingAccountTransactionItem, ReInsuranceCheckingAccountTransactionItemDTO>(reInsuranceCheckingAccountTransactionItem);
        }

        public static IEnumerable<ReInsuranceCheckingAccountTransactionItemDTO> ToDTOs(this IEnumerable<ReInsuranceCheckingAccountTransactionItem> reInsuranceCheckingAccountTransactionItem)
        {
            return reInsuranceCheckingAccountTransactionItem.Select(ToDTO);
        }

        public static IMapper CreateMapTransactionTypes()
        {
            var config = MapperCache.GetMapper<TransactionType, TransactionTypeDTO>(cfg =>
            {
                cfg.CreateMap<TransactionType, TransactionTypeDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
            });
            return config;
        }

        public static TransactionTypeDTO ToDTO(this TransactionType transactionType)
        {
            var config = CreateMapTransactionTypes();
            return config.Map<TransactionType, TransactionTypeDTO>(transactionType);
        }

        public static IEnumerable<TransactionTypeDTO> ToDTOs(this IEnumerable<TransactionType> transactionType)
        {
            return transactionType.Select(ToDTO);
        }

        public static IMapper CreateMapCreditNotes()
        {
            var config = MapperCache.GetMapper<CreditNote, CreditNoteDTO>(cfg =>
            {
                cfg.CreateMap<CreditNote, CreditNoteDTO>()
                        .ForMember(dest => dest.CreditNoteStatus, opt => opt.MapFrom(src => (int)src.CreditNoteStatus));
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<CreditNoteItem, CreditNoteItemDTO>();
                cfg.CreateMap<Policy, PolicyDTO>();
                cfg.CreateMap<Beneficiary, BeneficiaryDTO>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.BeneficiaryTypeDescription))
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (int)src.CustomerType));
                cfg.CreateMap<Endorsement, EndorsementDTO>();
                cfg.CreateMap<PayerComponent, PayerComponentDTO>();
                cfg.CreateMap<PaymentPlan, PaymentPlanDTO>();
                cfg.CreateMap<Quota, QuotaDTO>();
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
                cfg.CreateMap<Prefix, PrefixDTO>();
                cfg.CreateMap<IssuanceAgency, IssuanceAgencyDTO>();
                cfg.CreateMap<IssuanceAgent, IssuanceAgentDTO>();
                cfg.CreateMap<IssuanceAgentType, IssuanceAgentTypeDTO>();
                cfg.CreateMap<IssuanceCommission, IssuanceCommissionDTO>();
                cfg.CreateMap<SubLineBusiness, SubLineBusinessDTO>();
                cfg.CreateMap<LineBusiness, LineBusinessDTO>();
                cfg.CreateMap<Holder, HolderDTO>()
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (int)src.CustomerType))
                .ForMember(dest => dest.IndividualType, opt => opt.MapFrom(src => (int)src.IndividualType));
                cfg.CreateMap<BillingGroup, BillingGroupDTO>();
                cfg.CreateMap<PolicyType, PolicyTypeDTO>();
                cfg.CreateMap<Prefix, PrefixDTO>();
            });
            return config;
        }

        public static CreditNoteDTO ToDTO(this CreditNote creditNote)
        {
            var config = CreateMapCreditNotes();
            return config.Map<CreditNote, CreditNoteDTO>(creditNote);
        }

        public static IEnumerable<CreditNoteDTO> ToDTOs(this IEnumerable<CreditNote> creditNote)
        {
            return creditNote.Select(ToDTO);
        }

        public static IMapper CreateMapCancellationLimits()
        {
            var config = MapperCache.GetMapper<CancellationLimit, CancellationLimitDTO>(cfg =>
            {
                cfg.CreateMap<CancellationLimit, CancellationLimitDTO>();
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
            });
            return config;
        }

        public static CancellationLimitDTO ToDTO(this CancellationLimit cancellationLimit)
        {
            var config = CreateMapCancellationLimits();
            return config.Map<CancellationLimit, CancellationLimitDTO>(cancellationLimit);
        }

        public static IEnumerable<CancellationLimitDTO> ToDTOs(this IEnumerable<CancellationLimit> cancellationLimit)
        {
            return cancellationLimit.Select(ToDTO);
        }

        public static IMapper CreateMapExclusions()
        {
            var config = MapperCache.GetMapper<Exclusion, ExclusionDTO>(cfg =>
            {
                cfg.CreateMap<Exclusion, ExclusionDTO>();
                cfg.CreateMap<Person, PersonDTO>()
                .ForMember(dest => dest.IndividualType, opt => opt.MapFrom(src => (IndividualType)src.IndividualType))
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (IndividualType)src.CustomerType));
                cfg.CreateMap<DocumentType, DocumentTypeDTO>();
                cfg.CreateMap<IdentificationDocument, DTOs.IdentificationDocumentDTO>();
                cfg.CreateMap<Policy, PolicyDTO>();
                cfg.CreateMap<Beneficiary, BeneficiaryDTO>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.BeneficiaryTypeDescription))
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (int)src.CustomerType));
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<Endorsement, EndorsementDTO>();
                cfg.CreateMap<PayerComponent, PayerComponentDTO>();
                cfg.CreateMap<PaymentPlan, PaymentPlanDTO>();
                cfg.CreateMap<Quota, QuotaDTO>();
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
                cfg.CreateMap<Prefix, PrefixDTO>();
                cfg.CreateMap<IssuanceAgency, IssuanceAgencyDTO>();
                cfg.CreateMap<IssuanceAgent, IssuanceAgentDTO>();
                cfg.CreateMap<IssuanceAgentType, IssuanceAgentTypeDTO>();
                cfg.CreateMap<IssuanceCommission, IssuanceCommissionDTO>();
                cfg.CreateMap<SubLineBusiness, SubLineBusinessDTO>();
                cfg.CreateMap<LineBusiness, LineBusinessDTO>();
                cfg.CreateMap<Holder, HolderDTO>()
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (int)src.CustomerType))
                .ForMember(dest => dest.IndividualType, opt => opt.MapFrom(src => (int)src.IndividualType));
                cfg.CreateMap<BillingGroup, BillingGroupDTO>();
                cfg.CreateMap<PolicyType, PolicyTypeDTO>();
                cfg.CreateMap<Prefix, PrefixDTO>();
            });
            return config;
        }

        public static ExclusionDTO ToDTO(this Exclusion exclusion)
        {
            var config = CreateMapExclusions();
            return config.Map<Exclusion, ExclusionDTO>(exclusion);
        }

        public static IEnumerable<ExclusionDTO> ToDTOs(this IEnumerable<Exclusion> exclusion)
        {
            return exclusion.Select(ToDTO);
        }

        public static IMapper CreateMapBankAccountCompanies()
        {
            var config = MapperCache.GetMapper<BankAccountCompany, BankAccountCompanyDTO>(cfg =>
            {
                cfg.CreateMap<BankAccountCompany, BankAccountCompanyDTO>();
                cfg.CreateMap<BankAccountType, BankAccountTypeDTO>();
                cfg.CreateMap<Bank, BankDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
                cfg.CreateMap<GeneralLedgerServices.EEProvider.Models.AccountingAccount, AccountingAccountDTO>();

            });
            return config;
        }

        public static BankAccountCompanyDTO ToDTO(this BankAccountCompany bankAccountCompany)
        {
            var config = CreateMapBankAccountCompanies();
            return config.Map<BankAccountCompany, BankAccountCompanyDTO>(bankAccountCompany);
        }

        public static IEnumerable<BankAccountCompanyDTO> ToDTOs(this IEnumerable<BankAccountCompany> bankAccountCompany)
        {
            return bankAccountCompany.Select(ToDTO);
        }

        public static IMapper CreateMapPersons()
        {
            var config = MapperCache.GetMapper<Person, PersonDTO>(cfg =>
            {
                cfg.CreateMap<Person, PersonDTO>()
                .ForMember(dest => dest.IndividualType, opt => opt.MapFrom(src => (IndividualType)src.IndividualType))
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (IndividualType)src.CustomerType));
                cfg.CreateMap<DocumentType, DocumentTypeDTO>();
                cfg.CreateMap<IdentificationDocument, DTOs.IdentificationDocumentDTO>();
                cfg.CreateMap<PersonType, PersonTypeDTO>();

                cfg.CreateMap<IdentificationDocument, DTOs.IdentificationDocumentDTO>();
                cfg.CreateMap<DocumentType, DTOs.DocumentTypeDTO>();
                cfg.CreateMap<Company, DTOs.CompanyDTO>()
                .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName));
                cfg.CreateMap<Transaction, TransactionDTO>();
                cfg.CreateMap<PersonType, PersonTypeDTO>();
            });
            return config;
        }

        public static PersonDTO ToDTO(this Person collect)
        {
            var config = CreateMapPersons();
            return config.Map<Person, PersonDTO>(collect);
        }

        public static IEnumerable<PersonDTO> ToDTOs(this IEnumerable<Person> collect)
        {
            return collect.Select(ToDTO);
        }

        public static IMapper CreateMapBankAccountPersons()
        {
            var config = MapperCache.GetMapper<BankAccountPerson, BankAccountPersonDTO>(cfg =>
            {
                cfg.CreateMap<BankAccountPerson, BankAccountPersonDTO>();
                cfg.CreateMap<BankAccountType, BankAccountTypeDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<BankAccountType, BankAccountTypeDTO>();
                cfg.CreateMap<Individual, IndividualDTO>();
                cfg.CreateMap<DocumentType, DocumentTypeDTO>();
                cfg.CreateMap<Application.UniquePersonService.V1.Models.EconomicActivity, EconomicActivityDTO>();
                cfg.CreateMap<IdentificationDocument, IdentificationDocumentDTO>();
                cfg.CreateMap<Bank, BankDTO>();
            });
            return config;
        }

        public static BankAccountPersonDTO ToDTO(this BankAccountPerson bankAccountPerson)
        {
            var config = CreateMapBankAccountPersons();
            return config.Map<BankAccountPerson, BankAccountPersonDTO>(bankAccountPerson);
        }

        public static IEnumerable<BankAccountPersonDTO> ToDTOs(this IEnumerable<BankAccountPerson> bankAccountPerson)
        {
            return bankAccountPerson.Select(ToDTO);
        }

        public static IMapper CreateMapBankAccountTypes()
        {
            var config = MapperCache.GetMapper<BankAccountType, BankAccountTypeDTO>(cfg =>
            {
                cfg.CreateMap<BankAccountType, BankAccountTypeDTO>();
            });
            return config;
        }

        public static BankAccountTypeDTO ToDTO(this BankAccountType bankAccountType)
        {
            var config = CreateMapBankAccountTypes();
            return config.Map<BankAccountType, BankAccountTypeDTO>(bankAccountType);
        }

        public static IEnumerable<BankAccountTypeDTO> ToDTOs(this IEnumerable<BankAccountType> bankAccountType)
        {
            return bankAccountType.Select(ToDTO);
        }

        public static IMapper CreateMapAmortizations()
        {
            var config = MapperCache.GetMapper<Amortization, AmortizationDTO>(cfg =>
            {
                cfg.CreateMap<Amortization, AmortizationDTO>()
                        .ForMember(dest => dest.AmortizationStatus, opt => opt.MapFrom(src => (int)src.AmortizationStatus));
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<Policy, PolicyDTO>();
                cfg.CreateMap<Beneficiary, BeneficiaryDTO>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.BeneficiaryTypeDescription))
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (int)src.CustomerType));
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<Endorsement, EndorsementDTO>();
                cfg.CreateMap<PayerComponent, PayerComponentDTO>();
                cfg.CreateMap<PaymentPlan, PaymentPlanDTO>();
                cfg.CreateMap<Quota, QuotaDTO>();
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
                cfg.CreateMap<Prefix, PrefixDTO>();
                cfg.CreateMap<IssuanceAgency, IssuanceAgencyDTO>();
                cfg.CreateMap<IssuanceAgent, IssuanceAgentDTO>();
                cfg.CreateMap<IssuanceAgentType, IssuanceAgentTypeDTO>();
                cfg.CreateMap<IssuanceCommission, IssuanceCommissionDTO>();
                cfg.CreateMap<SubLineBusiness, SubLineBusinessDTO>();
                cfg.CreateMap<LineBusiness, LineBusinessDTO>();
                cfg.CreateMap<Holder, HolderDTO>()
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (int)src.CustomerType))
                .ForMember(dest => dest.IndividualType, opt => opt.MapFrom(src => (int)src.IndividualType));
                cfg.CreateMap<BillingGroup, BillingGroupDTO>();
                cfg.CreateMap<PolicyType, PolicyTypeDTO>();
                cfg.CreateMap<Prefix, PrefixDTO>();
            });
            return config;
        }

        public static AmortizationDTO ToDTO(this Amortization amortization)
        {
            var config = CreateMapAmortizations();
            return config.Map<Amortization, AmortizationDTO>(amortization);
        }

        public static IEnumerable<AmortizationDTO> ToDTOs(this IEnumerable<Amortization> amortization)
        {
            return amortization.Select(ToDTO);
        }

        public static IMapper CreateMapCheckBookControls()
        {
            var config = MapperCache.GetMapper<CheckBookControl, DTOs.AccountsPayables.CheckBookControlDTO>(cfg =>
            {
                cfg.CreateMap<CheckBookControl, DTOs.AccountsPayables.CheckBookControlDTO>();
                cfg.CreateMap<BankAccountCompany, BankAccountCompanyDTO>();
                cfg.CreateMap<BankAccountType, BankAccountTypeDTO>();
                cfg.CreateMap<Bank, BankDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
                cfg.CreateMap<GeneralLedgerServices.EEProvider.Models.AccountingAccount, AccountingAccountDTO>();
            });
            return config;
        }

        public static DTOs.AccountsPayables.CheckBookControlDTO ToDTO(this CheckBookControl checkBookControl)
        {
            var config = CreateMapCheckBookControls();
            return config.Map<CheckBookControl, DTOs.AccountsPayables.CheckBookControlDTO>(checkBookControl);
        }

        public static IEnumerable<DTOs.AccountsPayables.CheckBookControlDTO> ToDTOs(this IEnumerable<CheckBookControl> checkBookControl)
        {
            return checkBookControl.Select(ToDTO);
        }

        public static IMapper CreateMapCheckPaymentOrders()
        {
            var config = MapperCache.GetMapper<CheckPaymentOrder, CheckPaymentOrderDTO>(cfg =>
            {
                cfg.CreateMap<CheckPaymentOrder, CheckPaymentOrderDTO>();
                cfg.CreateMap<BankAccountCompany, BankAccountCompanyDTO>();
                cfg.CreateMap<Bank, BankDTO>();
                cfg.CreateMap<BankAccountType, BankAccountTypeDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
                cfg.CreateMap<GeneralLedgerServices.EEProvider.Models.AccountingAccount, AccountingAccountDTO>();
                cfg.CreateMap<PersonType, PersonTypeDTO>();
                cfg.CreateMap<Individual, IndividualDTO>();
                cfg.CreateMap<DocumentType, DocumentTypeDTO>();
                cfg.CreateMap<Application.UniquePersonService.V1.Models.EconomicActivity, EconomicActivityDTO>();
                cfg.CreateMap<IdentificationDocument, IdentificationDocumentDTO>();
                cfg.CreateMap<PaymentOrder, DTOs.AccountsPayables.PaymentOrderDTO>();
            });
            return config;
        }

        public static CheckPaymentOrderDTO ToDTO(this CheckPaymentOrder checkPaymentOrder)
        {
            var config = CreateMapCheckPaymentOrders();
            return config.Map<CheckPaymentOrder, CheckPaymentOrderDTO>(checkPaymentOrder);
        }

        public static IEnumerable<CheckPaymentOrderDTO> ToDTOs(this IEnumerable<CheckPaymentOrder> checkPaymentOrder)
        {
            return checkPaymentOrder.Select(ToDTO);
        }

        public static IMapper CreateMapPaymentOrders()
        {
            var config = MapperCache.GetMapper<PaymentOrder, DTOs.AccountsPayables.PaymentOrderDTO>(cfg =>
            {
                cfg.CreateMap<PaymentOrder, DTOs.AccountsPayables.PaymentOrderDTO>()
                        .ForMember(dest => dest.Company, opt => opt.MapFrom(src => new CompanyDTO { IndividualId = src.Company.IndividualId, Name = src.Company.FullName }));
                cfg.CreateMap<PersonType, PersonTypeDTO>();
                cfg.CreateMap<Individual, IndividualDTO>();
                cfg.CreateMap<DocumentType, DocumentTypeDTO>();
                cfg.CreateMap<Application.UniquePersonService.V1.Models.EconomicActivity, EconomicActivityDTO>();
                cfg.CreateMap<IdentificationDocument, IdentificationDocumentDTO>();
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<ExchangeRate, ExchangeRateDTO>();
                cfg.CreateMap<EEProvider.Models.Claims.PaymentRequest.ConceptSource, ConceptSourceDTO>();
                cfg.CreateMap<PaymentsModels.PaymentMethod, PaymentMethodDTO>();
                cfg.CreateMap<BankAccountPerson, BankAccountPersonDTO>();
                cfg.CreateMap<Bank, BankDTO>();
                cfg.CreateMap<BankAccountType, BankAccountTypeDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<EEProvider.Models.Imputations.Application, ApplicationDTO>()
                //.ForMember(dest => dest.ImputationType, opt => opt.MapFrom(src => (int)src.ImputationType))
                ;
                cfg.CreateMap<TransactionType, TransactionTypeDTO>();
                cfg.CreateMap<Transaction, TransactionDTO>();
            });
            return config;
        }

        public static DTOs.AccountsPayables.PaymentOrderDTO ToDTO(this PaymentOrder paymentOrder)
        {
            var config = CreateMapPaymentOrders();
            return config.Map<PaymentOrder, DTOs.AccountsPayables.PaymentOrderDTO>(paymentOrder);
        }

        public static IEnumerable<DTOs.AccountsPayables.PaymentOrderDTO> ToDTOs(this IEnumerable<PaymentOrder> paymentOrder)
        {
            return paymentOrder.Select(ToDTO);
        }

        public static IMapper CreateMapPaymentRequests()
        {
            var config = MapperCache.GetMapper<PaymentRequest, PaymentRequestDTO>(cfg =>
            {
                cfg.CreateMap<PaymentRequest, PaymentRequestDTO>()
                        .ForMember(dest => dest.PaymentRequestType, opt => opt.MapFrom(src => (int)src.PaymentRequestType));
                cfg.CreateMap<PaymentRequestNumber, PaymentRequestNumberDTO>();
                cfg.CreateMap<PRCLAIM.MovementType, MovementTypeDTO>();
                cfg.CreateMap<Company, DTOs.CompanyDTO>()
                .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName));
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
                cfg.CreateMap<PersonType, PersonTypeDTO>();
                cfg.CreateMap<Individual, IndividualDTO>();
                cfg.CreateMap<DocumentType, DocumentTypeDTO>();
                cfg.CreateMap<Application.UniquePersonService.V1.Models.EconomicActivity, EconomicActivityDTO>();
                cfg.CreateMap<IdentificationDocument, IdentificationDocumentDTO>();
                cfg.CreateMap<PaymentsModels.PaymentMethod, PaymentMethodDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<ExchangeRate, ExchangeRateDTO>();
                cfg.CreateMap<Voucher, VoucherDTO>();
                cfg.CreateMap<EEProvider.Models.Imputations.CostCenter, CostCenterDTO>();
                cfg.CreateMap<VoucherConceptTax, VoucherConceptTaxDTO>();
                cfg.CreateMap<TaxCondition, TaxConditionDTO>();
                cfg.CreateMap<TransactionType, TransactionTypeDTO>();
                cfg.CreateMap<CLAIMODL.Claim, ClaimDTO>()
                .ForMember(dest => dest.CoveredRiskType, opt => opt.MapFrom(src => (int)src.CoveredRiskType));
                cfg.CreateMap<Tax, TaxDTO>();
                cfg.CreateMap<TaxCondition, TaxConditionDTO>();
                cfg.CreateMap<TaxCategory, TaxCategoryDTO>();
                cfg.CreateMap<GeneralLedgerServices.EEProvider.Models.AccountingAccount, AccountingAccountDTO>();
                cfg.CreateMap<VoucherType, VoucherTypeDTO>();
                cfg.CreateMap<VoucherConcept, VoucherConceptDTO>();
                cfg.CreateMap<PRCLAIM.ConceptSource, ConceptSourceDTO>();
                cfg.CreateMap<EEProvider.Models.Imputations.AccountingConcept, AccountingConceptDTO>();


            });
            return config;
        }

        public static PaymentRequestDTO ToDTO(this PaymentRequest paymentRequest)
        {
            var config = CreateMapPaymentRequests();
            return config.Map<PaymentRequest, PaymentRequestDTO>(paymentRequest);
        }

        public static IEnumerable<PaymentRequestDTO> ToDTOs(this IEnumerable<PaymentRequest> paymentRequest)
        {
            return paymentRequest.Select(ToDTO);
        }

        public static IMapper CreateMapPaymentRequestNumbers()
        {
            var config = MapperCache.GetMapper<PaymentRequestNumber, PaymentRequestNumberDTO>(cfg =>
            {
                cfg.CreateMap<PaymentRequestNumber, PaymentRequestNumberDTO>();
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
            });
            return config;
        }

        public static PaymentRequestNumberDTO ToDTO(this PaymentRequestNumber paymentRequestNumber)
        {
            var config = CreateMapPaymentRequestNumbers();
            return config.Map<PaymentRequestNumber, PaymentRequestNumberDTO>(paymentRequestNumber);
        }

        public static IEnumerable<PaymentRequestNumberDTO> ToDTOs(this IEnumerable<PaymentRequestNumber> paymentRequestNumber)
        {
            return paymentRequestNumber.Select(ToDTO);
        }

        public static IMapper CreateMapTransferPaymentOrders()
        {
            var config = MapperCache.GetMapper<TransferPaymentOrder, TransferPaymentOrderDTO>(cfg =>
            {
                cfg.CreateMap<TransferPaymentOrder, TransferPaymentOrderDTO>();
                cfg.CreateMap<BankAccountCompany, BankAccountCompanyDTO>();
                cfg.CreateMap<PersonType, PersonTypeDTO>();

                cfg.CreateMap<ExchangeRate, ExchangeRateDTO>();
                cfg.CreateMap<Company, DTOs.CompanyDTO>()
                .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName));
                cfg.CreateMap<EEProvider.Models.Claims.PaymentRequest.ConceptSource, ConceptSourceDTO>();
                cfg.CreateMap<PaymentsModels.PaymentMethod, PaymentMethodDTO>();

                cfg.CreateMap<BankAccountPerson, BankAccountPersonDTO>();
                cfg.CreateMap<BankAccountType, BankAccountTypeDTO>();
                cfg.CreateMap<Bank, BankDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<Individual, IndividualDTO>();
                cfg.CreateMap<DocumentType, DocumentTypeDTO>();
                cfg.CreateMap<Application.UniquePersonService.V1.Models.EconomicActivity, EconomicActivityDTO>();
                cfg.CreateMap<IdentificationDocument, IdentificationDocumentDTO>();

                cfg.CreateMap<EEProvider.Models.Imputations.Application, ApplicationDTO>();
                cfg.CreateMap<TransactionType, TransactionTypeDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<Transaction, TransactionDTO>();



                cfg.CreateMap<BankAccountType, BankAccountTypeDTO>();
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
                cfg.CreateMap<GeneralLedgerServices.EEProvider.Models.AccountingAccount, AccountingAccountDTO>();
                cfg.CreateMap<PaymentOrder, DTOs.AccountsPayables.PaymentOrderDTO>();
            });
            return config;
        }

        public static TransferPaymentOrderDTO ToDTO(this TransferPaymentOrder transferPaymentOrder)
        {
            var config = CreateMapTransferPaymentOrders();
            return config.Map<TransferPaymentOrder, TransferPaymentOrderDTO>(transferPaymentOrder);
        }

        public static IEnumerable<TransferPaymentOrderDTO> ToDTOs(this IEnumerable<TransferPaymentOrder> transferPaymentOrder)
        {
            return transferPaymentOrder.Select(ToDTO);
        }

        public static IMapper CreateMapVouchers()
        {
            var config = MapperCache.GetMapper<Voucher, VoucherDTO>(cfg =>
            {
                cfg.CreateMap<Voucher, VoucherDTO>();
                cfg.CreateMap<VoucherType, VoucherTypeDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<VoucherConcept, VoucherConceptDTO>();
                cfg.CreateMap<EEProvider.Models.Imputations.AccountingConcept, AccountingConceptDTO>();
                cfg.CreateMap<GeneralLedgerServices.EEProvider.Models.AccountingAccount, AccountingAccountDTO>();
                cfg.CreateMap<EEProvider.Models.Imputations.CostCenter, CostCenterDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<VoucherConceptTax, VoucherConceptTaxDTO>();
                cfg.CreateMap<TaxCondition, TaxConditionDTO>();
                cfg.CreateMap<TaxCategory, TaxCategoryDTO>();

            });
            return config;
        }

        public static VoucherDTO ToDTO(this Voucher voucher)
        {
            var config = CreateMapVouchers();
            return config.Map<Voucher, VoucherDTO>(voucher);
        }

        public static IEnumerable<VoucherDTO> ToDTOs(this IEnumerable<Voucher> voucher)
        {
            return voucher.Select(ToDTO);
        }

        public static IMapper CreateMapVoucherConcepts()
        {
            var config = MapperCache.GetMapper<VoucherConcept, VoucherConceptDTO>(cfg =>
            {
                cfg.CreateMap<VoucherConcept, VoucherConceptDTO>();
                cfg.CreateMap<EEProvider.Models.Imputations.AccountingConcept, AccountingConceptDTO>();
                cfg.CreateMap<GeneralLedgerServices.EEProvider.Models.AccountingAccount, AccountingAccountDTO>();
                cfg.CreateMap<EEProvider.Models.Imputations.CostCenter, CostCenterDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<VoucherConceptTax, VoucherConceptTaxDTO>();
                cfg.CreateMap<TaxCondition, TaxConditionDTO>();
                cfg.CreateMap<TaxCategory, TaxCategoryDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
            });
            return config;
        }

        public static VoucherConceptDTO ToDTO(this VoucherConcept voucherConcept)
        {
            var config = CreateMapVoucherConcepts();
            return config.Map<VoucherConcept, VoucherConceptDTO>(voucherConcept);
        }

        public static IEnumerable<VoucherConceptDTO> ToDTOs(this IEnumerable<VoucherConcept> voucherConcept)
        {
            return voucherConcept.Select(ToDTO);
        }

        public static IMapper CreateMapVoucherConceptTaxs()
        {
            var config = MapperCache.GetMapper<VoucherConceptTax, VoucherConceptTaxDTO>(cfg =>
            {
                cfg.CreateMap<VoucherConceptTax, VoucherConceptTaxDTO>();
                cfg.CreateMap<TaxServices.Models.Tax, TaxDTO>();
                cfg.CreateMap<TaxCondition, TaxConditionDTO>();
                cfg.CreateMap<TaxServices.Models.TaxCategory, TaxCategoryDTO>();
            });
            return config;
        }

        public static VoucherConceptTaxDTO ToDTO(this VoucherConceptTax voucherConceptTax)
        {
            var config = CreateMapVoucherConceptTaxs();
            return config.Map<VoucherConceptTax, VoucherConceptTaxDTO>(voucherConceptTax);
        }

        public static IEnumerable<VoucherConceptTaxDTO> ToDTOs(this IEnumerable<VoucherConceptTax> voucherConceptTax)
        {
            return voucherConceptTax.Select(ToDTO);
        }

        public static IMapper CreateMapVoucherTypes()
        {
            var config = MapperCache.GetMapper<VoucherType, VoucherTypeDTO>(cfg =>
            {
                cfg.CreateMap<VoucherType, VoucherTypeDTO>();
            });
            return config;
        }

        public static VoucherTypeDTO ToDTO(this VoucherType voucherType)
        {
            var config = CreateMapVoucherTypes();
            return config.Map<VoucherType, VoucherTypeDTO>(voucherType);
        }

        public static IEnumerable<VoucherTypeDTO> ToDTOs(this IEnumerable<VoucherType> voucherType)
        {
            return voucherType.Select(ToDTO);
        }



        public static CompanyDTO ToDTO(this Company company)
        {
            return new CompanyDTO
            {
                IndividualId = company.IndividualId,
                Name = company.FullName
            };
        }

        public static IEnumerable<CompanyDTO> ToDTOs(this IEnumerable<Company> company)
        {
            return company.Select(ToDTO);
        }

        public static IMapper CreateMapBranch()
        {
            var config = MapperCache.GetMapper<Branch, BranchDTO>(cfg =>
            {
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
            });
            return config;
        }

        public static BranchDTO ToDTO(this Branch branch)
        {
            var config = CreateMapBranch();
            return config.Map<Branch, BranchDTO>(branch);
        }

        public static IEnumerable<BranchDTO> ToDTOs(this IEnumerable<Branch> branchs)
        {
            return branchs.Select(ToDTO);
        }

        public static IMapper CreateMapPrefix()
        {
            var config = MapperCache.GetMapper<Prefix, PrefixDTO>(cfg =>
            {
                cfg.CreateMap<Prefix, PrefixDTO>();
            });
            return config;
        }

        public static PrefixDTO ToDTO(this Prefix prefix)
        {
            var config = CreateMapPrefix();
            return config.Map<Prefix, PrefixDTO>(prefix);
        }

        public static IEnumerable<PrefixDTO> ToDTOs(this IEnumerable<Prefix> prefixs)
        {
            return prefixs.Select(ToDTO);
        }

        public static IMapper CreateMapPolicy()
        {
            var config = MapperCache.GetMapper<Policy, PolicyDTO>(cfg =>
            {
                cfg.CreateMap<Policy, PolicyDTO>();
                cfg.CreateMap<Beneficiary, BeneficiaryDTO>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.BeneficiaryTypeDescription))
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (int)src.CustomerType));
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<Endorsement, EndorsementDTO>();
                cfg.CreateMap<PayerComponent, PayerComponentDTO>();
                cfg.CreateMap<PaymentPlan, PaymentPlanDTO>();
                cfg.CreateMap<Quota, QuotaDTO>();
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
                cfg.CreateMap<Prefix, PrefixDTO>();
                cfg.CreateMap<IssuanceAgency, IssuanceAgencyDTO>();
                cfg.CreateMap<IssuanceAgent, IssuanceAgentDTO>();
                cfg.CreateMap<IssuanceAgentType, IssuanceAgentTypeDTO>();
                cfg.CreateMap<IssuanceCommission, IssuanceCommissionDTO>();
                cfg.CreateMap<SubLineBusiness, SubLineBusinessDTO>();
                cfg.CreateMap<LineBusiness, LineBusinessDTO>();
                cfg.CreateMap<Holder, HolderDTO>()
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (int)src.CustomerType))
                .ForMember(dest => dest.IndividualType, opt => opt.MapFrom(src => (int)src.IndividualType));
                cfg.CreateMap<BillingGroup, BillingGroupDTO>();
                cfg.CreateMap<PolicyType, PolicyTypeDTO>();
                cfg.CreateMap<Prefix, PrefixDTO>();
            });
            return config;
        }

        public static PolicyDTO ToDTO(this Policy policy)
        {
            var config = CreateMapPolicy();
            return config.Map<Policy, PolicyDTO>(policy);
        }

        public static IEnumerable<PolicyDTO> ToDTOs(this IEnumerable<Policy> policies)
        {
            return policies.Select(ToDTO);
        }

        public static IMapper CreateMapIndividual()
        {
            var config = MapperCache.GetMapper<Individual, IndividualDTO>(cfg =>
            {
                cfg.CreateMap<Individual, IndividualDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName));
                cfg.CreateMap<DocumentType, DocumentTypeDTO>();
                cfg.CreateMap<Application.UniquePersonService.V1.Models.EconomicActivity, EconomicActivityDTO>();
                cfg.CreateMap<IdentificationDocument, IdentificationDocumentDTO>();
            });
            return config;
        }

        public static IndividualDTO ToDTO(this Individual individual)
        {
            var config = CreateMapIndividual();
            return config.Map<Individual, IndividualDTO>(individual);
        }

        public static IEnumerable<IndividualDTO> ToDTOs(this IEnumerable<Individual> policies)
        {
            return policies.Select(ToDTO);
        }

        public static IMapper CreateMapConceptSource()
        {
            var config = MapperCache.GetMapper<EEProvider.Models.Claims.PaymentRequest.ConceptSource, ConceptSourceDTO>(cfg =>
            {
                cfg.CreateMap<EEProvider.Models.Claims.PaymentRequest.ConceptSource, ConceptSourceDTO>();
            });
            return config;
        }

        public static ConceptSourceDTO ToDTO(this EEProvider.Models.Claims.PaymentRequest.ConceptSource conceptSource)
        {
            var config = CreateMapConceptSource();
            return config.Map<EEProvider.Models.Claims.PaymentRequest.ConceptSource, ConceptSourceDTO>(conceptSource);
        }

        public static IEnumerable<ConceptSourceDTO> ToDTOs(this IEnumerable<EEProvider.Models.Claims.PaymentRequest.ConceptSource> conceptSources)
        {
            return conceptSources.Select(ToDTO);
        }

        public static IMapper CreateMapPersonTypeDTO()
        {
            var config = MapperCache.GetMapper<PersonType, PersonTypeDTO>(cfg =>
            {
                cfg.CreateMap<PersonType, PersonTypeDTO>();
            });
            return config;
        }

        public static PersonTypeDTO ToDTO(this PersonType personType)
        {
            var config = CreateMapPersonTypeDTO();
            return config.Map<PersonType, PersonTypeDTO>(personType);
        }

        public static IEnumerable<PersonTypeDTO> ToDTOs(this IEnumerable<PersonType> conceptSources)
        {
            return conceptSources.Select(ToDTO);
        }

        public static IMapper CreateMapApplication()
        {
            var config = MapperCache.GetMapper<EEProvider.Models.Imputations.Application, ApplicationDTO>(cfg =>
            {
                cfg.CreateMap<EEProvider.Models.Imputations.Application, ApplicationDTO>();
                cfg.CreateMap<ApplicationPremium, ApplicationPremiumDTO>();
                cfg.CreateMap<TransactionType, TransactionTypeDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
            });
            return config;
        }

        public static ApplicationDTO ToDTO(this EEProvider.Models.Imputations.Application application)
        {
            var config = CreateMapApplication();
            return config.Map<EEProvider.Models.Imputations.Application, ApplicationDTO>(application);
        }

        public static IEnumerable<ApplicationDTO> ToDTOs(this IEnumerable<EEProvider.Models.Imputations.Application> applications)
        {
            return applications.Select(ToDTO);
        }

        public static IMapper CreateMapTempApplication()
        {
            var config = MapperCache.GetMapper<TempApplication, TempApplicationDTO>(cfg =>
            {
                cfg.CreateMap<TempApplication, TempApplicationDTO>();
                cfg.CreateMap<TempApplicationPremium, TempApplicationPremiumDTO>();
                cfg.CreateMap<TempApplicationPremiumCommiss, TempApplicationPremiumCommissDTO>();
            });
            return config;
        }

        public static TempApplicationDTO ToDTO(this TempApplication tempApplication)
        {
            var config = CreateMapTempApplication();
            return config.Map<TempApplication, TempApplicationDTO>(tempApplication);
        }

        public static IEnumerable<TempApplicationDTO> ToDTOs(this IEnumerable<TempApplication> tempApplications)
        {
            return tempApplications.Select(ToDTO);
        }

        public static IMapper CreateMapTempApplicationPremium()
        {
            var config = MapperCache.GetMapper<TempApplicationPremium, TempApplicationPremiumDTO>(cfg =>
            {
                cfg.CreateMap<TempApplicationPremium, TempApplicationPremiumDTO>();
                cfg.CreateMap<TempApplicationPremiumCommiss, TempApplicationPremiumCommissDTO>();
            });
            return config;
        }

        public static TempApplicationPremiumDTO ToDTO(this TempApplicationPremium tempApplication)
        {
            var config = CreateMapTempApplication();
            return config.Map<TempApplicationPremium, TempApplicationPremiumDTO>(tempApplication);
        }

        public static IEnumerable<TempApplicationPremiumDTO> ToDTOs(this IEnumerable<TempApplicationPremium> tempApplications)
        {
            return tempApplications.Select(ToDTO);
        }

        public static IMapper CreateMapCollectApplication()
        {
            var config = MapperCache.GetMapper<CollectApplication, CollectApplicationDTO>(cfg =>
            {
                cfg.CreateMap<CollectApplication, CollectApplicationDTO>();
                cfg.CreateMap<Collect, CollectDTO>()
                        .ForMember(dest => dest.CollectType, opt => opt.MapFrom(src => (int)src.CollectType))
                        //.ForMember(dest => dest.AccountingCompany, opt => opt.MapFrom(src => new CompanyDTO { IndividualId = src.AccountingCompany.IndividualId, Name = src.AccountingCompany.FullName }))
                        ;
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
                cfg.CreateMap<CollectConcept, CollectConceptDTO>();
                cfg.CreateMap<PaymentsModels.Payment, PaymentDTO>();
                cfg.CreateMap<PaymentsModels.PaymentMethod, PaymentMethodDTO>();
                cfg.CreateMap<ExchangeRate, ExchangeRateDTO>();
                cfg.CreateMap<PaymentTax, PaymentTaxDTO>();
                cfg.CreateMap<Tax, TaxDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<Person, PersonDTO>()
                .ForMember(dest => dest.IndividualType, opt => opt.MapFrom(src => (int)src.IndividualType))
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (int)src.CustomerType));
                cfg.CreateMap<DocumentType, DocumentTypeDTO>();
                cfg.CreateMap<IdentificationDocument, DTOs.IdentificationDocumentDTO>();
                cfg.CreateMap<Company, DTOs.CompanyDTO>()
                    .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName));
                cfg.CreateMap<Transaction, TransactionDTO>();
                cfg.CreateMap<PersonType, PersonTypeDTO>();
                cfg.CreateMap<Transaction, TransactionDTO>();
                cfg.CreateMap<EEProvider.Models.Imputations.Application, ApplicationDTO>();
                cfg.CreateMap<ApplicationPremium, ApplicationPremiumDTO>();
                cfg.CreateMap<CollectApplication, ApplicationDTO>();
            });
            return config;
        }

        public static CollectApplicationDTO ToDTO(this CollectApplication tempApplication)
        {
            var config = CreateMapCollectApplication();
            return config.Map<CollectApplication, CollectApplicationDTO>(tempApplication);
        }

        public static IEnumerable<CollectApplicationDTO> ToDTOs(this IEnumerable<CollectApplication> tempApplications)
        {
            return tempApplications.Select(ToDTO);
        }

        public static IMapper CreateMapPolicyIntegration()
        {
            var config = MapperCache.GetMapper<PolicyDTO, ACCINTDTO.PolicyDTO>(cfg =>
            {
                cfg.CreateMap<PolicyDTO, ACCINTDTO.PolicyDTO>();
                cfg.CreateMap<BeneficiaryDTO, ACCINTDTO.BeneficiaryDTO>();
                cfg.CreateMap<ProductDTO, ACCINTDTO.ProductDTO>();
                cfg.CreateMap<BillingGroupDTO, ACCINTDTO.BillingGroupDTO>();
                cfg.CreateMap<HolderDTO, ACCINTDTO.HolderDTO>();
                cfg.CreateMap<IssuanceAgencyDTO, ACCINTDTO.IssuanceAgencyDTO>();
                cfg.CreateMap<PrefixDTO, ACCINTDTO.PrefixDTO>();
                cfg.CreateMap<BranchDTO, ACCINTDTO.BranchDTO>();
                cfg.CreateMap<PaymentPlanDTO, ACCINTDTO.PaymentPlanDTO>();
                cfg.CreateMap<PayerComponentDTO, ACCINTDTO.PayerComponentDTO>();
                cfg.CreateMap<EndorsementDTO, ACCINTDTO.EndorsementDTO>();
                cfg.CreateMap<ExchangeRateDTO, ACCINTDTO.ExchangeRateDTO>();
                cfg.CreateMap<PolicyTypeDTO, ACCINTDTO.PolicyTypeDTO>();
                cfg.CreateMap<IssuanceAgentDTO, ACCINTDTO.IssuanceAgentDTO>();
                cfg.CreateMap<IssuanceCommissionDTO, ACCINTDTO.IssuanceCommissionDTO>();
                cfg.CreateMap<SubLineBusinessDTO, ACCINTDTO.SubLineBusinessDTO>();
                cfg.CreateMap<LineBusinessDTO, ACCINTDTO.LineBusinessDTO>();
                cfg.CreateMap<SalePointDTO, ACCINTDTO.SalePointDTO>();
                cfg.CreateMap<QuotaDTO, ACCINTDTO.QuotaDTO>();
            });
            return config;
        }

        public static ACCINTDTO.PolicyDTO TODTO(this PolicyDTO policyDTO)
        {
            var config = CreateMapPolicyIntegration();
            return config.Map<PolicyDTO, ACCINTDTO.PolicyDTO>(policyDTO);
        }

        public static IEnumerable<ACCINTDTO.PolicyDTO> ToDTOs(this IEnumerable<PolicyDTO> policyDTO)
        {
            return policyDTO.Select(TODTO);
        }

        public static IMapper CreateMapTemApplicationAccounting()
        {
            var config = MapperCache.GetMapper<ApplicationAccounting, ApplicationAccountingDTO>(cfg =>
            {
                cfg.CreateMap<ApplicationAccounting, ApplicationAccountingDTO>()
                        .ForMember(dest => dest.AccountingNature, opt => opt.MapFrom(src => (AccountingNature)src.AccountingNature))
                        ;
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
                cfg.CreateMap<Individual, IndividualDTO>()
                        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName));
                cfg.CreateMap<DocumentType, DocumentTypeDTO>();
                cfg.CreateMap<UniquePersonService.V1.Models.EconomicActivity, EconomicActivityDTO>();
                cfg.CreateMap<IdentificationDocument, IdentificationDocumentDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<ExchangeRate, ExchangeRateDTO>();
                cfg.CreateMap<BookAccount, BookAccountDTO>();
                cfg.CreateMap<ApplicationAccountingAnalysis, ApplicationAccountingAnalysisDTO>();
                cfg.CreateMap<ApplicationAccountingCostCenter, ApplicationAccountingCostCenterDTO>();
                cfg.CreateMap<TransactionType, TransactionTypeDTO>();
                cfg.CreateMap<EEProvider.Models.Imputations.AccountingConcept, DTOs.Imputations.ApplicationAccountingConceptDTO>();
                cfg.CreateMap<EEProvider.Models.Imputations.CostCenter, CostCenterDTO>();
                cfg.CreateMap<EEProvider.Models.Imputations.AnalysisConcept, AnalysisConceptDTO>();
                cfg.CreateMap<EEProvider.Models.Imputations.AnalysisCode, AnalysisCodeDTO>();
                ;
            });
            return config;
        }

        public static ApplicationAccountingDTO ToDTO(this ApplicationAccounting applicationAccounting)
        {
            var config = CreateMapTemApplicationAccounting();
            return config.Map<ApplicationAccounting, ApplicationAccountingDTO>(applicationAccounting);
        }

        public static IEnumerable<ApplicationAccountingDTO> ToDTOs(this IEnumerable<ApplicationAccounting> applicationAccountings)
        {
            return applicationAccountings.Select(ToDTO);
        }

        private static IMapper CreateApplicationAccountingTransaction()
        {
            var config = MapperCache.GetMapper<ApplicationAccountingTransaction, ApplicationAccountingTransactionDTO>(cfg =>
            {
                cfg.CreateMap<ApplicationAccountingTransaction, ApplicationAccountingTransactionDTO>();
                cfg.CreateMap<ApplicationAccounting, ApplicationAccountingDTO>()
                        .ForMember(dest => dest.AccountingNature, opt => opt.MapFrom(src => (AccountingNature)src.AccountingNature))
                        ;
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
                cfg.CreateMap<Individual, IndividualDTO>()
                        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName));
                cfg.CreateMap<DocumentType, DocumentTypeDTO>();
                cfg.CreateMap<UniquePersonService.V1.Models.EconomicActivity, EconomicActivityDTO>();
                cfg.CreateMap<IdentificationDocument, IdentificationDocumentDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<ExchangeRate, ExchangeRateDTO>();
                cfg.CreateMap<BookAccount, BookAccountDTO>();
                cfg.CreateMap<ApplicationAccountingAnalysis, ApplicationAccountingAnalysisDTO>();
                cfg.CreateMap<ApplicationAccountingCostCenter, ApplicationAccountingCostCenterDTO>();
                cfg.CreateMap<TransactionType, TransactionTypeDTO>();
                cfg.CreateMap<EEProvider.Models.Imputations.AccountingConcept, DTOs.Imputations.ApplicationAccountingConceptDTO>();
                cfg.CreateMap<CostCenter, CostCenterDTO>();
                cfg.CreateMap<AnalysisConcept, AnalysisConceptDTO>();
                cfg.CreateMap<AnalysisCode, AnalysisCodeDTO>();
            });
            return config;
        }

        public static ApplicationAccountingTransactionDTO ToDTO(this ApplicationAccountingTransaction applicationAccounting)
        {
            var config = CreateApplicationAccountingTransaction();
            return config.Map<ApplicationAccountingTransaction, ApplicationAccountingTransactionDTO>(applicationAccounting);
        }

        public static IEnumerable<ApplicationAccountingTransactionDTO> ToDTOs(this IEnumerable<ApplicationAccountingTransaction> applicationAccountings)
        {
            return applicationAccountings.Select(ToDTO);
        }

        private static IMapper CreateJournalApplicationAccounting()
        {
            var config = MapperCache.GetMapper<ApplicationAccountingDTO, ApplicationJournalEntryDTO>(cfg =>
            {
                cfg.CreateMap<ApplicationAccountingDTO, ApplicationJournalEntryDTO>()
                    .ForMember(dest => dest.CurrencyId, opt => opt.MapFrom(src => src.Amount.Currency.Id))
                    .ForMember(dest => dest.AccountAccountingId, opt => opt.MapFrom(src => src.ApplicationAccountingId))
                    .ForMember(dest => dest.AccountingNature, opt => opt.MapFrom(src => (AccountingNature)src.AccountingNature))
                    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                    .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount.Value))
                    .ForMember(dest => dest.LocalAmount, opt => opt.MapFrom(src => src.LocalAmount.Value))
                    .ForMember(dest => dest.ExchangeRate, opt => opt.MapFrom(src => src.ExchangeRate.SellAmount))
                    .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.Beneficiary.IndividualId))
                    .ForMember(dest => dest.SourceCode, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.AccountingConceptId, opt => opt.MapFrom(src => src.AccountingConcept.Id))
                    .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.Branch.Id))
                    .ForMember(dest => dest.SalePointId, opt => opt.MapFrom(src => src.SalePoint.Id));
                cfg.CreateMap<ApplicationAccountingCostCenterDTO, ApplicationAccountingCostCenterDTO>()
                    .ForMember(dest => dest.CostCenterId, opt => opt.MapFrom(srce => srce.CostCenter.CostCenterId));
            });
            return config;
        }

        public static ApplicationJournalEntryDTO ToJournalDTO(this ApplicationAccountingDTO applicationAccounting)
        {
            var config = CreateJournalApplicationAccounting();
            return config.Map<ApplicationAccountingDTO, ApplicationJournalEntryDTO>(applicationAccounting);
        }

        public static IEnumerable<ApplicationJournalEntryDTO> ToJournalDTOs(this IEnumerable<ApplicationAccountingDTO> applicationAccountings)
        {
            return applicationAccountings.Select(ToJournalDTO);
        }

        internal static CollectControlDTO CreateCollectControl(CollectControl collectControl)
        {
            return new CollectControlDTO()
            {
                Id = collectControl.Id,
                UserId = Convert.ToInt32(collectControl.UserId),
                Branch = createbranch(collectControl.Branch),
                Status = Convert.ToInt32(collectControl.Status),
                AccountingDate = Convert.ToDateTime(collectControl.AccountingDate),
                OpenDate = Convert.ToDateTime(collectControl.OpenDate),
                CloseDate = Convert.ToDateTime(collectControl.CloseDate)
            };
        }

        private static BranchDTO createbranch(Branch branch)
        {
            return new BranchDTO()
            { 
                Id =  branch.Id
            };
        }

        private static IMapper CreateJournalApplicationPremium()
        {
            TempApplicationBusiness tempApplicationBusiness = new TempApplicationBusiness();

            var config = MapperCache.GetMapper<ApplicationPremiumDTO, ApplicationJournalEntryDTO>(cfg =>
            {
                cfg.CreateMap<ApplicationPremiumDTO, ApplicationJournalEntryDTO>()
                    .ForMember(dest => dest.CurrencyId, opt => opt.MapFrom(src => src.Currencyid))
                    .ForMember(dest => dest.AccountingNature, opt => opt.MapFrom(src => src.Amount < 0 ?
                            Convert.ToInt32(AccountingNature.Debit) : Convert.ToInt32(AccountingNature.Credit)))
                    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => tempApplicationBusiness.GetJournalEntryItemDescription(src.EndorsementId, src.PaymentNumber)))
                    .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                    .ForMember(dest => dest.LocalAmount, opt => opt.MapFrom(src => src.LocalAmount))
                    .ForMember(dest => dest.ExchangeRate, opt => opt.MapFrom(src => src.ExchangeRate))
                    .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.PayerId))
                    .ForMember(dest => dest.SourceCode, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src))
                    ;
            });
            return config;
        }

        public static ApplicationJournalEntryDTO ToJournalDTO(this ApplicationPremiumDTO applicationPremium, int? moduleId)
        {
            var config = CreateJournalApplicationPremium();
            ApplicationJournalEntryDTO item = config.Map<ApplicationPremiumDTO, ApplicationJournalEntryDTO>(applicationPremium);
            if (moduleId == null || moduleId == Convert.ToInt32(ApplicationTypes.Collect))
            {
                item.BridgeAccountId = CommonBusiness.GetIntParameter(AccountingKeys.ACC_APP_PREMIUM);
                item.PackageRuleCodeId = CommonBusiness.GetStringParameter(AccountingKeys.ACC_RULE_PACKAGE_COLLECT);
            }
            else if (moduleId == Convert.ToInt32(ApplicationTypes.JournalEntry))
            {
                item.BridgeAccountId = CommonBusiness.GetIntParameter(AccountingKeys.ACC_JOURNAL_ENTRY_PREMIUM);
                // item.PackageRuleCodeId = CommonBusiness.GetStringParameter(AccountingKeys.ACC_RULE_PACKAGE_JOURNAL_ENTRY);
                item.PackageRuleCodeId = CommonBusiness.GetStringParameter(AccountingKeys.ACC_RULE_PACKAGE_COLLECT);
            }
            return item;
        }

        public static IEnumerable<ApplicationJournalEntryDTO> ToJournalDTOs(this IEnumerable<ApplicationPremiumDTO> applicationPremiums, int? moduleId)
        {
            return applicationPremiums.Select(x => x.ToJournalDTO(moduleId));
        }
        #region TempApplicationPremiumCommission

        public static IMapper CreateMapTempApplicationPremiumCommission()
        {
            var config = MapperCache.GetMapper<TempApplicationPremiumCommiss, TempApplicationPremiumCommissDTO>(cfg =>
            {
                cfg.CreateMap<TempApplicationPremiumCommiss, TempApplicationPremiumCommissDTO>();
            });
            return config;
        }

        public static TempApplicationPremiumCommissDTO ToDTO(this TempApplicationPremiumCommiss tempApplicationPremiumCommiss)
        {
            var config = CreateMapTempApplicationPremiumCommission();
            return config.Map<TempApplicationPremiumCommiss, TempApplicationPremiumCommissDTO>(tempApplicationPremiumCommiss);
        }
        public static IEnumerable<TempApplicationPremiumCommissDTO> ToDTOs(this IEnumerable<TempApplicationPremiumCommiss> tempApplicationPremiumCommiss)
        {
            return tempApplicationPremiumCommiss.Select(ToDTO);
        }

        private static IMapper CreateJournalApplicationPremiumCommission()
        {
            var config = MapperCache.GetMapper<ApplicationPremiumCommisionDTO, ApplicationJournalEntryDTO>(cfg =>
            {
                cfg.CreateMap<ApplicationPremiumCommisionDTO, ApplicationJournalEntryDTO>()
                    .ForMember(dest => dest.CurrencyId, opt => opt.MapFrom(src => src.CurrencyId))
                    //.ForMember(dest => dest.AccountAccountingId, opt => opt.MapFrom(src => src.BookAccount.Id))
                    .ForMember(dest => dest.AccountingNature, opt => opt.MapFrom(src => Convert.ToInt32(AccountingNature.Credit)))
                    .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                    .ForMember(dest => dest.LocalAmount, opt => opt.MapFrom(src => src.LocalAmount))
                    .ForMember(dest => dest.ExchangeRate, opt => opt.MapFrom(src => src.ExchangeRate))
                    .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.AgentId))
                    .ForMember(dest => dest.SourceCode, opt => opt.MapFrom(src => src.Id))
                    //.ForMember(dest => dest.AccountingConceptId, opt => opt.MapFrom(src => src.AccountingConcept.Id))
                    .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src))
                    //.ForMember(dest => dest.SalePointId, opt => opt.MapFrom(src => src.))
                    ;
            });
            return config;
        }

        public static ApplicationJournalEntryDTO ToJournalDTO(this ApplicationPremiumCommisionDTO applicationPremiumCommisionDTO, int? moduleId)
        {
            var config = CreateJournalApplicationPremiumCommission();
            ApplicationJournalEntryDTO item = config.Map<ApplicationPremiumCommisionDTO, ApplicationJournalEntryDTO>(applicationPremiumCommisionDTO);
            if (moduleId == null || moduleId == Convert.ToInt32(ApplicationTypes.Collect))
            {
                item.BridgeAccountId = CommonBusiness.GetIntParameter(AccountingKeys.ACC_APP_PREMIUM_COMMISS);
                item.PackageRuleCodeId = CommonBusiness.GetStringParameter(AccountingKeys.ACC_RULE_PACKAGE_COLLECT);
            }
            else if (moduleId == null || moduleId == Convert.ToInt32(ApplicationTypes.JournalEntry))
            {
                item.BridgeAccountId = CommonBusiness.GetIntParameter(AccountingKeys.ACC_JOURNAL_ENTRY_COMMISS);
                //item.PackageRuleCodeId = CommonBusiness.GetStringParameter(AccountingKeys.ACC_RULE_PACKAGE_JOURNAL_ENTRY);
                item.PackageRuleCodeId = CommonBusiness.GetStringParameter(AccountingKeys.ACC_RULE_PACKAGE_COLLECT);
            }
            return item;
        }

        public static IEnumerable<ApplicationJournalEntryDTO> ToJournalDTOs(this IEnumerable<ApplicationPremiumCommisionDTO> applicationPremiumCommisionDTOs, int? moduleId)
        {
            return applicationPremiumCommisionDTOs.Select(x => x.ToJournalDTO(moduleId));
        }
        #endregion

        #region ApplicationPremiumCommission
        public static IMapper CreateMapApplicationPremiumCommission()
        {
            var config = MapperCache.GetMapper<ApplicationPremiumCommision, ApplicationPremiumCommisionDTO>(cfg =>
            {
                cfg.CreateMap<ApplicationPremiumCommision, ApplicationPremiumCommisionDTO>();
            });
            return config;
        }

        public static ApplicationPremiumCommisionDTO ToDTO(this ApplicationPremiumCommision applicationPremiumCommision)
        {
            var config = CreateMapApplicationPremiumCommission();
            return config.Map<ApplicationPremiumCommision, ApplicationPremiumCommisionDTO>(applicationPremiumCommision);
        }
        public static IEnumerable<ApplicationPremiumCommisionDTO> ToDTOs(this IEnumerable<ApplicationPremiumCommision> applicationPremiumCommision)
        {
            return applicationPremiumCommision.Select(ToDTO);
        }
        public static IMapper CreateMapApplicationDiscountedCommission()
        {
            var config = MapperCache.GetMapper<ApplicationPremiumCommision, DiscountedCommissionDTO>(cfg =>
            {
                cfg.CreateMap<ApplicationPremiumCommision, DiscountedCommissionDTO>()
                .ForMember(dest => dest.AgentTypeCode, opt => opt.MapFrom(src => src.AgentTypeId));
            });
            return config;
        }
        public static DiscountedCommissionDTO ToDtTO(this ApplicationPremiumCommision applicationPremiumCommision)
        {
            var config = CreateMapApplicationDiscountedCommission();
            return config.Map<ApplicationPremiumCommision, DiscountedCommissionDTO>(applicationPremiumCommision);
        }
        public static IEnumerable<DiscountedCommissionDTO> ToDtTOs(this IEnumerable<ApplicationPremiumCommision> applicationPremiumCommision)
        {
            return applicationPremiumCommision.Select(ToDtTO);
        }
        #endregion

        public static IMapper CreateMapMessage()
        {
            var config = MapperCache.GetMapper<Message, MessageDTO>(cfg =>
            {
                cfg.CreateMap<Message, MessageDTO>();
            });
            return config;
        }

        public static MessageDTO ToDTO(this Message message)
        {
            var config = CreateMapMessage();
            return config.Map<Message, MessageDTO>(message);
        }

        public static IMapper CreateMapApplicationLedger()
        {
            var config = MapperCache.GetMapper<ApplicationDTO, ApplicationLedgerDTO>(cfg =>
            {
                cfg.CreateMap<ApplicationDTO, ApplicationLedgerDTO>();
            });
            return config;
        }

        public static ApplicationLedgerDTO ToLedgerDTO(this ApplicationDTO applicationDTO)
        {
            var config = CreateMapApplicationLedger();
            return config.Map<ApplicationDTO, ApplicationLedgerDTO>(applicationDTO);
        }

        private static IMapper CreateUpdTempApplicationPremiumComponent()
        {
            var config = MapperCache.GetMapper<UpdTempApplicationPremiumComponent, UpdTempApplicationPremiumComponentDTO>(cfg =>
            {
                cfg.CreateMap<UpdTempApplicationPremiumComponent, UpdTempApplicationPremiumComponentDTO>();
            });
            return config;
        }

        public static UpdTempApplicationPremiumComponentDTO ToDTO(this UpdTempApplicationPremiumComponent updTempApplicationPremiumComponent)
        {
            var config = CreateUpdTempApplicationPremiumComponent();
            return config.Map<UpdTempApplicationPremiumComponent, UpdTempApplicationPremiumComponentDTO>(updTempApplicationPremiumComponent);
        }

        public static IMapper CreateMapCurrency()
        {
            var config = MapperCache.GetMapper<Currency, CurrencyDTO>(cfg =>
            {
                cfg.CreateMap<Currency, CurrencyDTO>();
            });
            return config;
        }

        public static CurrencyDTO ToDTO(this Currency currency)
        {
            var config = CreateMapCurrency();
            return config.Map<Currency, CurrencyDTO>(currency);
        }

        public static IEnumerable<CurrencyDTO> ToDTOs(this IEnumerable<Currency> currencies)
        {
            return currencies.Select(ToDTO);
        }

        public static IMapper CreateMapBank()
        {
            var config = MapperCache.GetMapper<Bank, BankDTO>(cfg =>
            {
                cfg.CreateMap<Bank, BankDTO>();
            });
            return config;
        }

        public static BankDTO ToDTO(this Bank bank)
        {
            var config = CreateMapBank();
            return config.Map<Bank, BankDTO>(bank);
        }

        public static IEnumerable<BankDTO> ToDTOs(this IEnumerable<Bank> banks)
        {
            return banks.Select(ToDTO);
        }

        public static IMapper CreateMapBillReport()
        {
            var config = MapperCache.GetMapper<BillReport, BillReportDTO>(cfg =>
            {
                cfg.CreateMap<BillReport, BillReportDTO>();
            });
            return config;
        }

        public static BillReportDTO ToDTO(this BillReport billReport)
        {
            var config = CreateMapBillReport();
            return config.Map<BillReport, BillReportDTO>(billReport);
        }

        public static IMapper CreateMapSaveBillParameter()
        {
            var config = MapperCache.GetMapper<SaveBillParameter, SaveBillParametersDTO>(cfg =>
            {
                cfg.CreateMap<SaveBillParameter, SaveBillParametersDTO>();
            });
            return config;
        }

        public static SaveBillParametersDTO ToDTO(this SaveBillParameter saveBillParameter)
        {
            var config = CreateMapSaveBillParameter();
            return config.Map<SaveBillParameter, SaveBillParametersDTO>(saveBillParameter);
        }
    }
}
