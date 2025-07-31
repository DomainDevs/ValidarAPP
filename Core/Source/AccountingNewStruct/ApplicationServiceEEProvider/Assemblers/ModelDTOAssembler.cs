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
using TxServ = Sistran.Core.Application.TaxServices.Models;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Underwriting;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.Cache;
using System.Collections.Generic;
using System.Linq;
using PaymentsModels = Sistran.Core.Application.AccountingServices.EEProvider.Models.Payments;
using PRCLAIM = Sistran.Core.Application.AccountingServices.EEProvider.Models.Claims.PaymentRequest;
using CLAIMODL = Sistran.Core.Application.AccountingServices.EEProvider.Models.Claims;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Application.AccountingServices.Enums;

namespace Sistran.Core.Application.AccountingServices.Assemblers
{
    internal static class ModelDTOAssembler
    {
        #region ActionType
        public static IMapper CreateMapActionTypes()
        {
            var config = MapperCache.GetMapper<ActionTypeDTO, ActionType>(cfg =>
            {
                cfg.CreateMap<ActionTypeDTO, ActionType>();
            });
            return config;
        }

        public static ActionType ToModel(this ActionTypeDTO actionTypeDTO)
        {
            var config = CreateMapActionTypes();
            return config.Map<ActionTypeDTO, ActionType>(actionTypeDTO);
        }

        public static IEnumerable<ActionType> ToModels(this IEnumerable<ActionTypeDTO> actionTypeDTOs)
        {
            return actionTypeDTOs.Select(ToModel);
        }
        #endregion

        #region collects
        public static IMapper CreateMapCollects()
        {
            var config = MapperCache.GetMapper<CollectDTO, Collect>(cfg =>
            {
                cfg.CreateMap<CollectDTO, Collect>()
                        .ForMember(dest => dest.CollectType, opt => opt.MapFrom(src => (CollectTypes)src.CollectType))
                        .ForMember(dest => dest.AccountingCompany, opt => opt.MapFrom(src => new CompanyDTO { IndividualId = src.CompanyIndividualId, Name = src.AccountingCompany.Name }));
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<CollectConceptDTO, CollectConcept>();
                cfg.CreateMap<PaymentDTO, PaymentsModels.Payment>()
                    .Include<CashDTO, Cash>()
                    .Include<CheckDTO, Check>()
                    .Include<CreditCardDTO, CreditCard>()
                    .Include<TransferDTO, Transfer>()
                    .Include<DepositVoucherDTO, DepositVoucher>()
                    .Include<RetentionReceiptDTO, RetentionReceipt>()
                    .Include<ConsignmentCheckDTO, ConsignmentCheck>();
                cfg.CreateMap<CashDTO, Cash>();
                cfg.CreateMap<CheckDTO, Check>();
                cfg.CreateMap<BankDTO, Bank>();
                cfg.CreateMap<CreditCardDTO, CreditCard>();
                cfg.CreateMap<CreditCardTypeDTO, CreditCardType>();
                cfg.CreateMap<CreditCardValidThruDTO, CreditCardValidThru>();
                cfg.CreateMap<TransferDTO, Transfer>();
                cfg.CreateMap<ConsignmentCheckDTO, ConsignmentCheck>()
                .ForMember(dest => dest.IssuingBankCode, opt => opt.MapFrom(src => (src.IssuingBank != null) ? src.IssuingBank.Id : 0));
                cfg.CreateMap<BankAccountPersonDTO, BankAccountPerson>();
                cfg.CreateMap<BankAccountTypeDTO, BankAccountType>();
                cfg.CreateMap<IndividualDTO, Individual>();
                cfg.CreateMap<DocumentTypeDTO, DocumentType>();
                cfg.CreateMap<EconomicActivityDTO, UniquePersonService.V1.Models.EconomicActivity>();
                cfg.CreateMap<IdentificationDocumentDTO, IdentificationDocument>();
                cfg.CreateMap<DepositVoucherDTO, DepositVoucher>();
                cfg.CreateMap<RetentionReceiptDTO, RetentionReceipt>();
                cfg.CreateMap<PolicyDTO, Policy>();
                cfg.CreateMap<BeneficiaryDTO, Beneficiary>()
                .ForMember(dest => dest.BeneficiaryTypeDescription, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (CustomerType)src.CustomerType));
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<EndorsementDTO, Endorsement>();
                cfg.CreateMap<PayerComponentDTO, PayerComponent>();
                cfg.CreateMap<PaymentPlanDTO, PaymentPlan>();
                cfg.CreateMap<QuotaDTO, Quota>();
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<IssuanceAgencyDTO, IssuanceAgency>();
                cfg.CreateMap<IssuanceAgentDTO, IssuanceAgent>();
                cfg.CreateMap<IssuanceAgentTypeDTO, IssuanceAgentType>();
                cfg.CreateMap<IssuanceCommissionDTO, IssuanceCommission>();
                cfg.CreateMap<SubLineBusinessDTO, SubLineBusiness>();
                cfg.CreateMap<LineBusinessDTO, LineBusiness>();
                cfg.CreateMap<HolderDTO, Holder>()
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (CustomerType)src.CustomerType))
                .ForMember(dest => dest.IndividualType, opt => opt.MapFrom(src => (IndividualType)src.IndividualType));
                cfg.CreateMap<BillingGroupDTO, BillingGroup>();
                cfg.CreateMap<PolicyTypeDTO, PolicyType>();
                cfg.CreateMap<PrefixDTO, Prefix>();
                cfg.CreateMap<RetentionBaseDTO, RetentionBase>();
                cfg.CreateMap<RetentionConceptDTO, RetentionConcept>();
                cfg.CreateMap<PaymentMethodDTO, PaymentsModels.PaymentMethod>();
                cfg.CreateMap<ExchangeRateDTO, ExchangeRate>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<PaymentTaxDTO, PaymentTax>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<TaxDTO, TxServ.Tax>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<PersonDTO, Person>()
                .ForMember(dest => dest.IndividualType, opt => opt.MapFrom(src => (IndividualType)src.IndividualType))
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (IndividualType)src.CustomerType));
                cfg.CreateMap<IdentificationDocumentDTO, IdentificationDocument>();
                cfg.CreateMap<DocumentTypeDTO, DocumentType>();
                cfg.CreateMap<CompanyDTO, Company>()
                .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name));
                cfg.CreateMap<TransactionDTO, Transaction>();
                cfg.CreateMap<PersonTypeDTO, PersonType>();
            });
            return config;
        }

        public static Collect ToModel(this CollectDTO collectDTO)
        {
            var config = CreateMapCollects();
            return config.Map<CollectDTO, Collect>(collectDTO);
        }

        public static IEnumerable<Collect> ToModels(this IEnumerable<CollectDTO> collectDTOs)
        {
            return collectDTOs.Select(ToModel);
        }

        public static IMapper CreateMapCollectConcepts()
        {
            var config = MapperCache.GetMapper<CollectConceptDTO, CollectConcept>(cfg =>
            {
                cfg.CreateMap<CollectConceptDTO, CollectConcept>();
            });
            return config;
        }

        public static CollectConcept ToModel(this CollectConceptDTO collectConceptDTO)
        {
            var config = CreateMapCollectConcepts();
            return config.Map<CollectConceptDTO, CollectConcept>(collectConceptDTO);
        }

        public static IEnumerable<CollectConcept> ToModels(this IEnumerable<CollectConceptDTO> collectConceptDTOs)
        {
            return collectConceptDTOs.Select(ToModel);
        }

        public static IMapper CreateMapCollectControls()
        {
            var config = MapperCache.GetMapper<CollectControlDTO, CollectControl>(cfg =>
            {
                cfg.CreateMap<CollectControlDTO, CollectControl>();
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<CollectDTO, Collect>()
                        .ForMember(dest => dest.CollectType, opt => opt.MapFrom(src => (CollectTypes)src.CollectType))
                        .ForMember(dest => dest.AccountingCompany, opt => opt.MapFrom(src => new CompanyDTO { IndividualId = src.CompanyIndividualId, Name = src.AccountingCompany.Name }));

                cfg.CreateMap<CollectConceptDTO, CollectConcept>();
                cfg.CreateMap<PaymentDTO, PaymentsModels.Payment>();
                cfg.CreateMap<PaymentMethodDTO, PaymentsModels.PaymentMethod>();
                cfg.CreateMap<ExchangeRateDTO, ExchangeRate>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<PaymentTaxDTO, PaymentTax>();

                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<TaxDTO, TxServ.Tax>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<PersonDTO, Person>()
                .ForMember(dest => dest.IndividualType, opt => opt.MapFrom(src => (IndividualType)src.IndividualType))
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (IndividualType)src.CustomerType));
                cfg.CreateMap<IdentificationDocumentDTO, IdentificationDocument>();
                cfg.CreateMap<DocumentTypeDTO, DocumentType>();
                cfg.CreateMap<CompanyDTO, Company>()
                .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name));
                cfg.CreateMap<TransactionDTO, Transaction>();
                cfg.CreateMap<PersonTypeDTO, PersonType>();
                cfg.CreateMap<CollectControlPaymentDTO, CollectControlPayment>();
            });
            return config;
        }

        public static CollectControl ToModel(this CollectControlDTO collectControlDTO)
        {
            var config = CreateMapCollectControls();
            return config.Map<CollectControlDTO, CollectControl>(collectControlDTO);
        }

        public static IEnumerable<CollectControl> ToModels(this IEnumerable<CollectControlDTO> collectControlDTOs)
        {
            return collectControlDTOs.Select(ToModel);
        }

        public static IMapper CreateMapCollectControlPayments()
        {
            var config = MapperCache.GetMapper<CollectControlPaymentDTO, CollectControlPayment>(cfg =>
            {
                cfg.CreateMap<CollectControlPaymentDTO, CollectControlPayment>();
                cfg.CreateMap<PaymentMethodDTO, PaymentsModels.PaymentMethod>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<CurrencyDTO, Currency>();
            });
            return config;
        }

        public static CollectControlPayment ToModel(this CollectControlPaymentDTO collectControlPaymentDTO)
        {
            var config = CreateMapCollectControlPayments();
            return config.Map<CollectControlPaymentDTO, CollectControlPayment>(collectControlPaymentDTO);
        }

        public static IEnumerable<CollectControlPayment> ToModels(this IEnumerable<CollectControlPaymentDTO> collectControlPaymentDTOs)
        {
            return collectControlPaymentDTOs.Select(ToModel);
        }

        //public static IMapper CreateMapCollectImputations()
        //{
        //    var config = MapperCache.GetMapper<CollectImputationDTO, CollectImputation>(cfg =>
        //    {
        //        cfg.CreateMap<CollectImputationDTO, CollectImputation>();
        //        cfg.CreateMap<CollectDTO, Collect>()
        //                .ForMember(dest => dest.CollectType, opt => opt.MapFrom(src => (CollectTypes)src.CollectType))
        //                .ForMember(dest => dest.AccountingCompany, opt => opt.MapFrom(src => new CompanyDTO { IndividualId = src.CompanyIndividualId, Name = src.AccountingCompany.Name }));
        //        cfg.CreateMap<BranchDTO, Branch>();
        //        cfg.CreateMap<CollectConceptDTO, CollectConcept>();
        //        cfg.CreateMap<PaymentDTO, PaymentsModels.Payment>();
        //        cfg.CreateMap<PaymentMethodDTO, PaymentsModels.PaymentMethod>();
        //        cfg.CreateMap<ExchangeRateDTO, ExchangeRate>();
        //        cfg.CreateMap<CurrencyDTO, Currency>();
        //        cfg.CreateMap<PaymentTaxDTO, PaymentTax>();
        //        cfg.CreateMap<SalePointDTO, SalePoint>();
        //        cfg.CreateMap<TaxDTO, TxServ.Tax>();
        //        cfg.CreateMap<AmountDTO, Amount>();
        //        cfg.CreateMap<PersonDTO, Person>()
        //        .ForMember(dest => dest.IndividualType, opt => opt.MapFrom(src => (IndividualType)src.IndividualType))
        //        .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (IndividualType)src.CustomerType));
        //        cfg.CreateMap<IdentificationDocumentDTO, IdentificationDocument>();
        //        cfg.CreateMap<DocumentTypeDTO, DocumentType>();
        //        cfg.CreateMap<CompanyDTO, Company>()
        //        .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
        //        .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name));
        //        cfg.CreateMap<TransactionDTO, Transaction>();
        //        cfg.CreateMap<PersonTypeDTO, PersonType>();
        //        cfg.CreateMap<TransactionDTO, Transaction>();
        //        cfg.CreateMap<ImputationDTO, Imputation>()
        //                .ForMember(dest => dest.ImputationType, opt => opt.MapFrom(src => (ImputationTypes)src.ImputationType));
        //        cfg.CreateMap<TransactionTypeDTO, TransactionType>();

        //    });
        //    return config;
        //}

        //public static CollectImputation ToModel(this CollectImputationDTO collectImputationDTO)
        //{
        //    var config = CreateMapCollectImputations();
        //    return config.Map<CollectImputationDTO, CollectImputation>(collectImputationDTO);
        //}

        //public static IEnumerable<CollectImputation> ToModels(this IEnumerable<CollectImputationDTO> collectImputationDTOs)
        //{
        //    return collectImputationDTOs.Select(ToModel);
        //}
        #endregion

        #region Payment
        public static IMapper CreateMapPaymentBallots()
        {
            var config = MapperCache.GetMapper<DTOs.PaymentBallotDTO, EEProvider.Models.PaymentBallot>(cfg =>
            {
                cfg.CreateMap<DTOs.PaymentBallotDTO, EEProvider.Models.PaymentBallot>();
                cfg.CreateMap<BankDTO, Bank>();
                cfg.CreateMap<DTOs.PaymentTicketDTO, PaymentTicket>();
                cfg.CreateMap<PaymentDTO, PaymentsModels.Payment>();
                cfg.CreateMap<PaymentMethodDTO, PaymentsModels.PaymentMethod>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<ExchangeRateDTO, ExchangeRate>();
                cfg.CreateMap<PaymentTaxDTO, PaymentTax>();
                cfg.CreateMap<TaxDTO, Tax>();
                cfg.CreateMap<TransactionDTO, Transaction>();
            });
            return config;
        }

        public static EEProvider.Models.PaymentBallot ToModel(this DTOs.PaymentBallotDTO paymentBallotDTO)
        {
            var config = CreateMapPaymentBallots();
            return config.Map<DTOs.PaymentBallotDTO, EEProvider.Models.PaymentBallot>(paymentBallotDTO);
        }

        public static IEnumerable<EEProvider.Models.PaymentBallot> ToModels(this IEnumerable<DTOs.PaymentBallotDTO> paymentBallotDTOs)
        {
            return paymentBallotDTOs.Select(ToModel);
        }

        public static IMapper CreateMapPaymentBallotsNew()
        {
            var config = MapperCache.GetMapper<DTOs.Search.PaymentBallotDTO, PaymentBallot>(cfg =>
            {
                cfg.CreateMap<DTOs.Search.PaymentBallotDTO, PaymentBallot>();
                cfg.CreateMap<BankDTO, Bank>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<DTOs.PaymentTicketDTO, PaymentTicket>();
                cfg.CreateMap<TransactionDTO, Transaction>();
            });
            return config;
        }

        public static PaymentBallot ToModelPayment(DTOs.Search.PaymentBallotDTO paymentBallot)
        {
            var config = CreateMapPaymentBallotsNew();
            return config.Map<DTOs.Search.PaymentBallotDTO, PaymentBallot>(paymentBallot);
        }

        public static IEnumerable<PaymentBallot> ToModelsPayment(IEnumerable<DTOs.Search.PaymentBallotDTO> paymentBallot)
        {
            return paymentBallot.Select(ToModelPayment);
        }

        //public static IMapper CreateMapPaymentOrders()
        //{
        //    var config = MapperCache.GetMapper<DTOs.AccountsPayables.PaymentOrderDTO, PaymentOrder>(cfg =>
        //    {
        //        cfg.CreateMap<DTOs.AccountsPayables.PaymentOrderDTO, PaymentOrder>()
        //                //.ForMember(dest => dest.PaymentType, opt => opt.MapFrom(src => (PaymentType)src.PaymentType))
        //                ;
        //        cfg.CreateMap<IndividualDTO, Individual>();
        //        cfg.CreateMap<BranchDTO, Branch>();
        //        cfg.CreateMap<DTOs.CompanyDTO, Company>();
        //        cfg.CreateMap<PaymentMethodDTO, PaymentsModels.PaymentMethod>();
        //        cfg.CreateMap<CurrencyDTO, Currency>();
        //        cfg.CreateMap<ExchangeRateDTO, ExchangeRate>();
        //        cfg.CreateMap<AmountDTO, Amount>();
        //    });
        //    return config;
        //}

        //public static PaymentOrder ToModel(this DTOs.AccountsPayables.PaymentOrderDTO paymentOrderDTO)
        //{
        //    var config = CreateMapPaymentOrders();
        //    return config.Map<DTOs.AccountsPayables.PaymentOrderDTO, PaymentOrder>(paymentOrderDTO);
        //}

        //public static IEnumerable<PaymentOrder> ToModels(this IEnumerable<DTOs.AccountsPayables.PaymentOrderDTO> paymentOrderDTOs)
        //{
        //    return paymentOrderDTOs.Select(ToModel);
        //}
        //public static IMapper CreateMapPaymentReinsurances()
        //{
        //    var config = MapperCache.GetMapper<PaymentReinsuranceDTO, PaymentReinsurance>(cfg =>
        //    {
        //        cfg.CreateMap<PaymentReinsuranceDTO, PaymentReinsurance>()
        //                .ForMember(dest => dest.Movements, opt => opt.MapFrom(src => (Movements)src.Movements));
        //        cfg.CreateMap<MovementTypeDTO, MovementType>();
        //        cfg.CreateMap<CurrencyDTO, Currency>();
        //        cfg.CreateMap<AmountDTO, Amount>();
        //    });
        //    return config;
        //}

        //public static PaymentReinsurance ToModel(this PaymentReinsuranceDTO paymentReinsuranceDTO)
        //{
        //    var config = CreateMapPaymentReinsurances();
        //    return config.Map<PaymentReinsuranceDTO, PaymentReinsurance>(paymentReinsuranceDTO);
        //}

        //public static IEnumerable<PaymentReinsurance> ToModels(this IEnumerable<PaymentReinsuranceDTO> paymentReinsuranceDTOs)
        //{
        //    return paymentReinsuranceDTOs.Select(ToModel);
        //}

        public static IMapper CreateMapPaymentTickets()
        {
            var config = MapperCache.GetMapper<DTOs.PaymentTicketDTO, PaymentTicket>(cfg =>
            {
                cfg.CreateMap<DTOs.PaymentTicketDTO, PaymentTicket>();
                cfg.CreateMap<BankDTO, Bank>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<PaymentDTO, PaymentsModels.Payment>();
                cfg.CreateMap<PaymentMethodDTO, PaymentsModels.PaymentMethod>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<ExchangeRateDTO, ExchangeRate>();
                cfg.CreateMap<PaymentTaxDTO, PaymentTax>();
                cfg.CreateMap<TaxDTO, Tax>();
            });
            return config;
        }

        public static PaymentTicket ToModel(this DTOs.PaymentTicketDTO paymentTicketDTO)
        {
            var config = CreateMapPaymentTickets();
            return config.Map<DTOs.PaymentTicketDTO, PaymentTicket>(paymentTicketDTO);
        }

        public static IEnumerable<PaymentTicket> ToModels(this IEnumerable<DTOs.PaymentTicketDTO> paymentTicketDTOs)
        {
            return paymentTicketDTOs.Select(ToModel);
        }

        public static IMapper CreateMapRejectedPayments()
        {
            var config = MapperCache.GetMapper<DTOs.RejectedPaymentDTO, RejectedPayment>(cfg =>
            {
                cfg.CreateMap<DTOs.RejectedPaymentDTO, RejectedPayment>();
                cfg.CreateMap<PaymentDTO, PaymentsModels.Payment>();
                cfg.CreateMap<PaymentMethodDTO, PaymentsModels.PaymentMethod>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<ExchangeRateDTO, ExchangeRate>();
                cfg.CreateMap<PaymentTaxDTO, PaymentTax>();
                cfg.CreateMap<TaxDTO, Tax>();
                cfg.CreateMap<RejectionDTO, Rejection>();
            });
            return config;
        }

        public static RejectedPayment ToModel(this DTOs.RejectedPaymentDTO rejectedPaymentDTO)
        {
            var config = CreateMapRejectedPayments();
            return config.Map<DTOs.RejectedPaymentDTO, RejectedPayment>(rejectedPaymentDTO);
        }

        public static IEnumerable<RejectedPayment> ToModels(this IEnumerable<DTOs.RejectedPaymentDTO> rejectedPaymentDTOs)
        {
            return rejectedPaymentDTOs.Select(ToModel);
        }

        #endregion


        

        public static IMapper CreateMapCompany()
        {
            var config = MapperCache.GetMapper<DTOs.CompanyDTO, Company>(cfg =>
           {
               cfg.CreateMap<DTOs.CompanyDTO, Company>()
                .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name));
               cfg.CreateMap<PaymentMethodDTO, PaymentsModels.PaymentMethod>();
               cfg.CreateMap<CurrencyDTO, Currency>();
               cfg.CreateMap<ExchangeRateDTO, ExchangeRate>();
               cfg.CreateMap<AmountDTO, Amount>();
           });
            return config;
        }

        public static Company ToModel(this DTOs.CompanyDTO paymentOrderDTO)
        {
            var config = CreateMapPaymentOrders();
            return config.Map<DTOs.CompanyDTO, Company>(paymentOrderDTO);
        }

        public static IEnumerable<Company> ToModels(this IEnumerable<DTOs.CompanyDTO> paymentOrderDTOs)
        {
            return paymentOrderDTOs.Select(ToModel);
        }

        public static IMapper CreateMapRanges()
        {
            var config = MapperCache.GetMapper<RangeDTO, Range>(cfg =>
            {
                cfg.CreateMap<RangeDTO, Range>();
                cfg.CreateMap<RangeItemDTO, RangeItem>();
            });
            return config;
        }

        public static Range ToModel(this RangeDTO rangeDTO)
        {
            var config = CreateMapRanges();
            return config.Map<RangeDTO, Range>(rangeDTO);
        }

        public static IEnumerable<Range> ToModels(this IEnumerable<RangeDTO> rangeDTOs)
        {
            return rangeDTOs.Select(ToModel);
        }


        //public static IMapper CreateMapRangeItems()
        //{
        //    var config = MapperCache.GetMapper<RangeItemDTO, RangeItem>(cfg =>
        //    {
        //        cfg.CreateMap<RangeItemDTO, RangeItem>();
        //    });
        //    return config;
        //}

        //public static RangeItem ToModel(this RangeItemDTO rangeItemDTO)
        //{
        //    var config = CreateMapRangeItems();
        //    return config.Map<RangeItemDTO, RangeItem>(rangeItemDTO);
        //}

        //public static IEnumerable<RangeItem> ToModels(this IEnumerable<RangeItemDTO> rangeItemDTOs)
        //{
        //    return rangeItemDTOs.Select(ToModel);
        //}



        public static IMapper CreateMapRejections()
        {
            var config = MapperCache.GetMapper<RejectionDTO, Rejection>(cfg =>
            {
                cfg.CreateMap<RejectionDTO, Rejection>();
            });
            return config;
        }

        public static Rejection ToModel(this RejectionDTO rejectionDTO)
        {
            var config = CreateMapRejections();
            return config.Map<RejectionDTO, Rejection>(rejectionDTO);
        }

        public static IEnumerable<Rejection> ToModels(this IEnumerable<RejectionDTO> rejectionDTOs)
        {
            return rejectionDTOs.Select(ToModel);
        }

        public static IMapper CreateMapTransactions()
        {
            var config = MapperCache.GetMapper<TransactionDTO, Transaction>(cfg =>
            {
                cfg.CreateMap<TransactionDTO, Transaction>();
            });
            return config;
        }

        public static Transaction ToModel(this TransactionDTO transactionDTO)
        {
            var config = CreateMapTransactions();
            return config.Map<TransactionDTO, Transaction>(transactionDTO);
        }

        public static IEnumerable<Transaction> ToModels(this IEnumerable<TransactionDTO> transactionDTOs)
        {
            return transactionDTOs.Select(ToModel);
        }

        public static IMapper CreateMapRetentionBases()
        {
            var config = MapperCache.GetMapper<RetentionBaseDTO, RetentionBase>(cfg =>
            {
                cfg.CreateMap<RetentionBaseDTO, RetentionBase>();
            });
            return config;
        }

        public static RetentionBase ToModel(this RetentionBaseDTO retentionBaseDTO)
        {
            var config = CreateMapRetentionBases();
            return config.Map<RetentionBaseDTO, RetentionBase>(retentionBaseDTO);
        }

        public static IEnumerable<RetentionBase> ToModels(this IEnumerable<RetentionBaseDTO> retentionBaseDTOs)
        {
            return retentionBaseDTOs.Select(ToModel);
        }

        public static IMapper CreateMapRetentionConcepts()
        {
            var config = MapperCache.GetMapper<RetentionConceptDTO, RetentionConcept>(cfg =>
            {
                cfg.CreateMap<RetentionConceptDTO, RetentionConcept>();
                cfg.CreateMap<RetentionBaseDTO, RetentionBase>();
            });
            return config;
        }

        public static RetentionConcept ToModel(this RetentionConceptDTO retentionConceptDTO)
        {
            var config = CreateMapRetentionConcepts();
            return config.Map<RetentionConceptDTO, RetentionConcept>(retentionConceptDTO);
        }

        public static IEnumerable<RetentionConcept> ToModels(this IEnumerable<RetentionConceptDTO> retentionConceptDTOs)
        {
            return retentionConceptDTOs.Select(ToModel);
        }

        public static IMapper CreateMapRetentionConceptPercentages()
        {
            var config = MapperCache.GetMapper<RetentionConceptPercentageDTO, RetentionConceptPercentage>(cfg =>
            {
                cfg.CreateMap<RetentionConceptPercentageDTO, RetentionConceptPercentage>();
                cfg.CreateMap<RetentionConceptDTO, RetentionConcept>();
                cfg.CreateMap<RetentionBaseDTO, RetentionBase>();
            });
            return config;
        }

        public static RetentionConceptPercentage ToModel(this RetentionConceptPercentageDTO retentionConceptPercentageDTO)
        {
            var config = CreateMapRetentionConceptPercentages();
            return config.Map<RetentionConceptPercentageDTO, RetentionConceptPercentage>(retentionConceptPercentageDTO);
        }

        public static IEnumerable<RetentionConceptPercentage> ToModels(this IEnumerable<RetentionConceptPercentageDTO> retentionConceptPercentageDTOs)
        {
            return retentionConceptPercentageDTOs.Select(ToModel);
        }

        public static IMapper CreateMapCashs()
        {
            var config = MapperCache.GetMapper<CashDTO, Cash>(cfg =>
            {
                cfg.CreateMap<CashDTO, Cash>();
                cfg.CreateMap<PaymentDTO, PaymentsModels.Payment>();
                cfg.CreateMap<PaymentMethodDTO, PaymentsModels.PaymentMethod>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<ExchangeRateDTO, ExchangeRate>();
                cfg.CreateMap<PaymentTaxDTO, PaymentTax>();
                cfg.CreateMap<TaxDTO, Tax>();
            });
            return config;
        }

        public static Cash ToModel(this CashDTO cashDTO)
        {
            var config = CreateMapCashs();
            return config.Map<CashDTO, Cash>(cashDTO);
        }

        public static IEnumerable<Cash> ToModels(this IEnumerable<CashDTO> cashDTOs)
        {
            return cashDTOs.Select(ToModel);
        }

        public static IMapper CreateMapChecks()
        {
            var config = MapperCache.GetMapper<CheckDTO, Check>(cfg =>
            {
                cfg.CreateMap<CheckDTO, Check>();
                cfg.CreateMap<BankDTO, Bank>();
                cfg.CreateMap<PaymentDTO, PaymentsModels.Payment>();
                cfg.CreateMap<PaymentMethodDTO, PaymentsModels.PaymentMethod>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<ExchangeRateDTO, ExchangeRate>();
                cfg.CreateMap<PaymentTaxDTO, PaymentTax>();
                cfg.CreateMap<TaxDTO, Tax>();
            });
            return config;
        }

        public static Check ToModel(this CheckDTO checkDTO)
        {
            var config = CreateMapChecks();
            return config.Map<CheckDTO, Check>(checkDTO);
        }

        public static IEnumerable<Check> ToModels(this IEnumerable<CheckDTO> checkDTOs)
        {
            return checkDTOs.Select(ToModel);
        }

        public static IMapper CreateMapCreditCards()
        {
            var config = MapperCache.GetMapper<CreditCardDTO, CreditCard>(cfg =>
            {
                cfg.CreateMap<CreditCardDTO, CreditCard>();
                cfg.CreateMap<CreditCardTypeDTO, CreditCardType>();
                cfg.CreateMap<BankDTO, Bank>();
                cfg.CreateMap<CreditCardValidThruDTO, CreditCardValidThru>();
                cfg.CreateMap<PaymentDTO, PaymentsModels.Payment>();
                cfg.CreateMap<PaymentMethodDTO, PaymentsModels.PaymentMethod>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<ExchangeRateDTO, ExchangeRate>();
                cfg.CreateMap<PaymentTaxDTO, PaymentTax>();
                cfg.CreateMap<TaxDTO, Tax>();
            });
            return config;
        }

        public static CreditCard ToModel(this CreditCardDTO creditCardDTO)
        {
            var config = CreateMapCreditCards();
            return config.Map<CreditCardDTO, CreditCard>(creditCardDTO);
        }

        public static IEnumerable<CreditCard> ToModels(this IEnumerable<CreditCardDTO> creditCardDTOs)
        {
            return creditCardDTOs.Select(ToModel);
        }

        public static IMapper CreateMapCreditCardTypes()
        {
            var config = MapperCache.GetMapper<CreditCardTypeDTO, CreditCardType>(cfg =>
            {
                cfg.CreateMap<CreditCardTypeDTO, CreditCardType>();
            });
            return config;
        }

        public static CreditCardType ToModel(this CreditCardTypeDTO creditCardTypeDTO)
        {
            var config = CreateMapCreditCardTypes();
            return config.Map<CreditCardTypeDTO, CreditCardType>(creditCardTypeDTO);
        }

        public static IEnumerable<CreditCardType> ToModels(this IEnumerable<CreditCardTypeDTO> creditCardTypeDTOs)
        {
            return creditCardTypeDTOs.Select(ToModel);
        }

        public static IMapper CreateMapCreditCardValidThrus()
        {
            var config = MapperCache.GetMapper<CreditCardValidThruDTO, CreditCardValidThru>(cfg =>
            {
                cfg.CreateMap<CreditCardValidThruDTO, CreditCardValidThru>();
            });
            return config;
        }

        public static CreditCardValidThru ToModel(this CreditCardValidThruDTO creditCardValidThruDTO)
        {
            var config = CreateMapCreditCardValidThrus();
            return config.Map<CreditCardValidThruDTO, CreditCardValidThru>(creditCardValidThruDTO);
        }

        public static IEnumerable<CreditCardValidThru> ToModels(this IEnumerable<CreditCardValidThruDTO> creditCardValidThruDTOs)
        {
            return creditCardValidThruDTOs.Select(ToModel);
        }

        public static IMapper CreateMapDepositVouchers()
        {
            var config = MapperCache.GetMapper<DepositVoucherDTO, DepositVoucher>(cfg =>
            {
                cfg.CreateMap<DepositVoucherDTO, DepositVoucher>();
                cfg.CreateMap<BankAccountCompanyDTO, BankAccountCompany>();
                cfg.CreateMap<BankDTO, Bank>();
                cfg.CreateMap<BankAccountTypeDTO, BankAccountType>();
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<AccountingAccountDTO, GeneralLedgerServices.EEProvider.Models.AccountingAccount>();
                cfg.CreateMap<PaymentDTO, PaymentsModels.Payment>();
                cfg.CreateMap<PaymentMethodDTO, PaymentsModels.PaymentMethod>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<ExchangeRateDTO, ExchangeRate>();
                cfg.CreateMap<PaymentTaxDTO, PaymentTax>();
                cfg.CreateMap<TaxDTO, Tax>();
            });
            return config;
        }

        public static DepositVoucher ToModel(this DepositVoucherDTO depositVoucherDTO)
        {
            var config = CreateMapDepositVouchers();
            return config.Map<DepositVoucherDTO, DepositVoucher>(depositVoucherDTO);
        }

        public static IEnumerable<DepositVoucher> ToModels(this IEnumerable<DepositVoucherDTO> depositVoucherDTOs)
        {
            return depositVoucherDTOs.Select(ToModel);
        }

        public static IMapper CreateMapPayments()
        {
            var config = MapperCache.GetMapper<PaymentDTO, PaymentsModels.Payment>(cfg =>
            {
                cfg.CreateMap<PaymentDTO, PaymentsModels.Payment>()
                    .Include<CashDTO, Cash>()
                    .Include<CheckDTO, Check>()
                    .Include<CreditCardDTO, CreditCard>()
                    .Include<TransferDTO, Transfer>()
                    .Include<DepositVoucherDTO, DepositVoucher>()
                    .Include<RetentionReceiptDTO, RetentionReceipt>()
                    .Include<ConsignmentCheckDTO, ConsignmentCheck>();
                cfg.CreateMap<CashDTO, Cash>();
                cfg.CreateMap<CheckDTO, Check>();
                cfg.CreateMap<BankDTO, Bank>();
                cfg.CreateMap<CreditCardDTO, CreditCard>();
                cfg.CreateMap<CreditCardTypeDTO, CreditCardType>();
                cfg.CreateMap<CreditCardValidThruDTO, CreditCardValidThru>();
                cfg.CreateMap<TransferDTO, Transfer>();
                cfg.CreateMap<ConsignmentCheckDTO, ConsignmentCheck>();
                cfg.CreateMap<BankAccountPersonDTO, BankAccountPerson>();
                cfg.CreateMap<BankAccountTypeDTO, BankAccountType>();
                cfg.CreateMap<IndividualDTO, Individual>();
                cfg.CreateMap<DocumentTypeDTO, DocumentType>();
                cfg.CreateMap<EconomicActivityDTO, UniquePersonService.V1.Models.EconomicActivity>();
                cfg.CreateMap<IdentificationDocumentDTO, IdentificationDocument>();
                cfg.CreateMap<DepositVoucherDTO, DepositVoucher>();
                cfg.CreateMap<RetentionReceiptDTO, RetentionReceipt>();
                cfg.CreateMap<PolicyDTO, Policy>();
                cfg.CreateMap<BeneficiaryDTO, Beneficiary>()
                .ForMember(dest => dest.BeneficiaryTypeDescription, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (CustomerType)src.CustomerType));
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<EndorsementDTO, Endorsement>();
                cfg.CreateMap<PayerComponentDTO, PayerComponent>();
                cfg.CreateMap<PaymentPlanDTO, PaymentPlan>();
                cfg.CreateMap<QuotaDTO, Quota>();
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<IssuanceAgencyDTO, IssuanceAgency>();
                cfg.CreateMap<IssuanceAgentDTO, IssuanceAgent>();
                cfg.CreateMap<IssuanceAgentTypeDTO, IssuanceAgentType>();
                cfg.CreateMap<IssuanceCommissionDTO, IssuanceCommission>();
                cfg.CreateMap<SubLineBusinessDTO, SubLineBusiness>();
                cfg.CreateMap<LineBusinessDTO, LineBusiness>();
                cfg.CreateMap<HolderDTO, Holder>()
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (CustomerType)src.CustomerType))
                .ForMember(dest => dest.IndividualType, opt => opt.MapFrom(src => (IndividualType)src.IndividualType));
                cfg.CreateMap<BillingGroupDTO, BillingGroup>();
                cfg.CreateMap<PolicyTypeDTO, PolicyType>();
                cfg.CreateMap<PrefixDTO, Prefix>();
                cfg.CreateMap<PaymentMethodDTO, PaymentsModels.PaymentMethod>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<ExchangeRateDTO, ExchangeRate>();
                cfg.CreateMap<PaymentTaxDTO, PaymentTax>();
                cfg.CreateMap<TaxDTO, Tax>();
            });
            return config;
        }

        public static PaymentsModels.Payment ToModel(this PaymentDTO paymentDTO)
        {
            var config = CreateMapPayments();
            return config.Map<PaymentDTO, PaymentsModels.Payment>(paymentDTO);
        }

        public static IEnumerable<PaymentsModels.Payment> ToModels(this IEnumerable<PaymentDTO> paymentDTOs)
        {
            return paymentDTOs.Select(ToModel);
        }

        public static IMapper CreateMapPaymentMethods()
        {
            var config = MapperCache.GetMapper<PaymentMethodDTO, PaymentsModels.PaymentMethod>(cfg =>
            {
                cfg.CreateMap<PaymentMethodDTO, PaymentsModels.PaymentMethod>();
            });
            return config;
        }

        public static PaymentsModels.PaymentMethod ToModel(this PaymentMethodDTO paymentMethodDTO)
        {
            var config = CreateMapPaymentMethods();
            return config.Map<PaymentMethodDTO, PaymentsModels.PaymentMethod>(paymentMethodDTO);
        }

        public static IEnumerable<PaymentsModels.PaymentMethod> ToModels(this IEnumerable<PaymentMethodDTO> paymentMethodDTOs)
        {
            return paymentMethodDTOs.Select(ToModel);
        }

        public static IMapper CreateMapPaymentTaxs()
        {
            var config = MapperCache.GetMapper<PaymentTaxDTO, PaymentTax>(cfg =>
            {
                cfg.CreateMap<PaymentTaxDTO, PaymentTax>();
                cfg.CreateMap<TaxDTO, TaxServices.Models.Tax>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
            });
            return config;
        }

        public static PaymentTax ToModel(this PaymentTaxDTO paymentTaxDTO)
        {
            var config = CreateMapPaymentTaxs();
            return config.Map<PaymentTaxDTO, PaymentTax>(paymentTaxDTO);
        }

        public static IEnumerable<PaymentTax> ToModels(this IEnumerable<PaymentTaxDTO> paymentTaxDTOs)
        {
            return paymentTaxDTOs.Select(ToModel);
        }

        public static IMapper CreateMapRetentionReceipts()
        {
            var config = MapperCache.GetMapper<RetentionReceiptDTO, RetentionReceipt>(cfg =>
            {
                cfg.CreateMap<RetentionReceiptDTO, RetentionReceipt>();
                cfg.CreateMap<PolicyDTO, Policy>();
                cfg.CreateMap<BeneficiaryDTO, Beneficiary>()
                .ForMember(dest => dest.BeneficiaryTypeDescription, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (CustomerType)src.CustomerType));
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<EndorsementDTO, Endorsement>();
                cfg.CreateMap<PayerComponentDTO, PayerComponent>();
                cfg.CreateMap<PaymentPlanDTO, PaymentPlan>();
                cfg.CreateMap<QuotaDTO, Quota>();
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<IssuanceAgencyDTO, IssuanceAgency>();
                cfg.CreateMap<IssuanceAgentDTO, IssuanceAgent>();
                cfg.CreateMap<IssuanceAgentTypeDTO, IssuanceAgentType>();
                cfg.CreateMap<IssuanceCommissionDTO, IssuanceCommission>();
                cfg.CreateMap<SubLineBusinessDTO, SubLineBusiness>();
                cfg.CreateMap<LineBusinessDTO, LineBusiness>();
                cfg.CreateMap<HolderDTO, Holder>()
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (CustomerType)src.CustomerType))
                .ForMember(dest => dest.IndividualType, opt => opt.MapFrom(src => (IndividualType)src.IndividualType));
                cfg.CreateMap<BillingGroupDTO, BillingGroup>();
                cfg.CreateMap<PolicyTypeDTO, PolicyType>();
                cfg.CreateMap<PrefixDTO, Prefix>();




                cfg.CreateMap<RetentionConceptDTO, RetentionConcept>();
                cfg.CreateMap<RetentionBaseDTO, RetentionBase>();
                cfg.CreateMap<PaymentDTO, PaymentsModels.Payment>();
                cfg.CreateMap<PaymentMethodDTO, PaymentsModels.PaymentMethod>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<ExchangeRateDTO, ExchangeRate>();
                cfg.CreateMap<PaymentTaxDTO, PaymentTax>();
                cfg.CreateMap<TaxDTO, Tax>();
            });
            return config;
        }

        public static RetentionReceipt ToModel(this RetentionReceiptDTO retentionReceiptDTO)
        {
            var config = CreateMapRetentionReceipts();
            return config.Map<RetentionReceiptDTO, RetentionReceipt>(retentionReceiptDTO);
        }

        public static IEnumerable<RetentionReceipt> ToModels(this IEnumerable<RetentionReceiptDTO> retentionReceiptDTOs)
        {
            return retentionReceiptDTOs.Select(ToModel);
        }

        public static IMapper CreateMapTransfers()
        {
            var config = MapperCache.GetMapper<TransferDTO, Transfer>(cfg =>
            {
                cfg.CreateMap<TransferDTO, Transfer>();
                cfg.CreateMap<BankAccountPersonDTO, BankAccountPerson>()
                  .ForMember(dest => dest.Individual.FullName, opt => opt.MapFrom(src => src.Individual.Name));
                cfg.CreateMap<BankAccountTypeDTO, BankAccountType>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<BankDTO, Bank>();
                cfg.CreateMap<PaymentDTO, PaymentsModels.Payment>();
                cfg.CreateMap<PaymentMethodDTO, PaymentsModels.PaymentMethod>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<ExchangeRateDTO, ExchangeRate>();
                cfg.CreateMap<PaymentTaxDTO, PaymentTax>();
                cfg.CreateMap<TaxDTO, Tax>();
            });
            return config;
        }

        public static Transfer ToModel(this TransferDTO transferDTO)
        {
            var config = CreateMapTransfers();
            return config.Map<TransferDTO, Transfer>(transferDTO);
        }

        public static IEnumerable<Transfer> ToModels(this IEnumerable<TransferDTO> transferDTOs)
        {
            return transferDTOs.Select(ToModel);
        }

        public static IMapper CreateMapBookAccounts()
        {
            var config = MapperCache.GetMapper<BookAccountDTO, BookAccount>(cfg =>
            {
                cfg.CreateMap<BookAccountDTO, BookAccount>();
            });
            return config;
        }

        public static BookAccount ToModel(this BookAccountDTO bookAccountDTO)
        {
            var config = CreateMapBookAccounts();
            return config.Map<BookAccountDTO, BookAccount>(bookAccountDTO);
        }

        public static IEnumerable<BookAccount> ToModels(this IEnumerable<BookAccountDTO> bookAccountDTOs)
        {
            return bookAccountDTOs.Select(ToModel);
        }

        public static IMapper CreateMapBrokerCheckingAccountItems()
        {
            var config = MapperCache.GetMapper<DTOs.Imputations.BrokerCheckingAccountItemDTO, BrokerCheckingAccountItem>(cfg =>
            {
                cfg.CreateMap<DTOs.Imputations.BrokerCheckingAccountItemDTO, BrokerCheckingAccountItem>();
            });
            return config;
        }

        public static BrokerCheckingAccountItem ToModel(this DTOs.Imputations.BrokerCheckingAccountItemDTO brokerCheckingAccountItemDTO)
        {
            var config = CreateMapBrokerCheckingAccountItems();
            return config.Map<DTOs.Imputations.BrokerCheckingAccountItemDTO, BrokerCheckingAccountItem>(brokerCheckingAccountItemDTO);
        }

        public static IEnumerable<BrokerCheckingAccountItem> ToModels(this IEnumerable<DTOs.Imputations.BrokerCheckingAccountItemDTO> brokerCheckingAccountItemDTOs)
        {
            return brokerCheckingAccountItemDTOs.Select(ToModel);
        }

        public static IMapper CreateMapBrokersCheckingAccountTransactions()
        {
            var config = MapperCache.GetMapper<BrokersCheckingAccountTransactionDTO, BrokersCheckingAccountTransaction>(cfg =>
            {
                cfg.CreateMap<BrokersCheckingAccountTransactionDTO, BrokersCheckingAccountTransaction>();
                cfg.CreateMap<BrokersCheckingAccountTransactionItemDTO, BrokersCheckingAccountTransactionItem>(); cfg.CreateMap<PolicyDTO, Policy>();
                cfg.CreateMap<BeneficiaryDTO, Beneficiary>()
                .ForMember(dest => dest.BeneficiaryTypeDescription, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (CustomerType)src.CustomerType));
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<EndorsementDTO, Endorsement>();
                cfg.CreateMap<PayerComponentDTO, PayerComponent>();
                cfg.CreateMap<PaymentPlanDTO, PaymentPlan>();
                cfg.CreateMap<QuotaDTO, Quota>();
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<IssuanceAgencyDTO, IssuanceAgency>();
                cfg.CreateMap<IssuanceAgentDTO, IssuanceAgent>();
                cfg.CreateMap<IssuanceAgentTypeDTO, IssuanceAgentType>();
                cfg.CreateMap<IssuanceCommissionDTO, IssuanceCommission>();
                cfg.CreateMap<SubLineBusinessDTO, SubLineBusiness>();
                cfg.CreateMap<LineBusinessDTO, LineBusiness>();
                cfg.CreateMap<HolderDTO, Holder>()
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (CustomerType)src.CustomerType))
                .ForMember(dest => dest.IndividualType, opt => opt.MapFrom(src => (IndividualType)src.IndividualType));
                cfg.CreateMap<BillingGroupDTO, BillingGroup>();
                cfg.CreateMap<PolicyTypeDTO, PolicyType>();
                cfg.CreateMap<PrefixDTO, Prefix>();
                cfg.CreateMap<TransactionTypeDTO, TransactionType>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<CheckingAccountTransactionDTO, CheckingAccountTransaction>();
                cfg.CreateMap<DTOs.CompanyDTO, Company>()
                .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name));
                cfg.CreateMap<ExchangeRateDTO, ExchangeRate>();
                cfg.CreateMap<CheckingAccountConceptDTO, CheckingAccountConcept>();
                cfg.CreateMap<IndividualDTO, Individual>();
                cfg.CreateMap<DocumentTypeDTO, DocumentType>();
                cfg.CreateMap<EconomicActivityDTO, UniquePersonService.V1.Models.EconomicActivity>();
                cfg.CreateMap<IdentificationDocumentDTO, IdentificationDocument>();
            });
            return config;
        }

        public static BrokersCheckingAccountTransaction ToModel(this BrokersCheckingAccountTransactionDTO brokersCheckingAccountTransactionDTO)
        {
            var config = CreateMapBrokersCheckingAccountTransactions();
            return config.Map<BrokersCheckingAccountTransactionDTO, BrokersCheckingAccountTransaction>(brokersCheckingAccountTransactionDTO);
        }

        public static IEnumerable<BrokersCheckingAccountTransaction> ToModels(this IEnumerable<BrokersCheckingAccountTransactionDTO> brokersCheckingAccountTransactionDTOs)
        {
            return brokersCheckingAccountTransactionDTOs.Select(ToModel);
        }

        public static IMapper CreateMapBrokersCheckingAccountTransactionItems()
        {
            var config = MapperCache.GetMapper<BrokersCheckingAccountTransactionItemDTO, BrokersCheckingAccountTransactionItem>(cfg =>
            {
                cfg.CreateMap<BrokersCheckingAccountTransactionItemDTO, BrokersCheckingAccountTransactionItem>()
                        //.ForMember(dest => dest.AccountingNature, opt => opt.MapFrom(src => (AccountingNatures)src.AccountingNature))
                        ;
                cfg.CreateMap<AgentDTO, Agent>();
                cfg.CreateMap<BrokersCheckingAccountTransactionItemDTO, BrokersCheckingAccountTransactionItem>();
                cfg.CreateMap<DTOs.Imputations.BrokerCheckingAccountItemDTO, BrokerCheckingAccountItem>();
                cfg.CreateMap<PolicyDTO, Policy>();
                cfg.CreateMap<BeneficiaryDTO, Beneficiary>()
                .ForMember(dest => dest.BeneficiaryTypeDescription, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (CustomerType)src.CustomerType));
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<EndorsementDTO, Endorsement>();
                cfg.CreateMap<PayerComponentDTO, PayerComponent>();
                cfg.CreateMap<PaymentPlanDTO, PaymentPlan>();
                cfg.CreateMap<QuotaDTO, Quota>();
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<IssuanceAgencyDTO, IssuanceAgency>();
                cfg.CreateMap<IssuanceAgentDTO, IssuanceAgent>();
                cfg.CreateMap<IssuanceAgentTypeDTO, IssuanceAgentType>();
                cfg.CreateMap<IssuanceCommissionDTO, IssuanceCommission>();
                cfg.CreateMap<SubLineBusinessDTO, SubLineBusiness>();
                cfg.CreateMap<LineBusinessDTO, LineBusiness>();
                cfg.CreateMap<HolderDTO, Holder>()
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (CustomerType)src.CustomerType))
                .ForMember(dest => dest.IndividualType, opt => opt.MapFrom(src => (IndividualType)src.IndividualType));
                cfg.CreateMap<BillingGroupDTO, BillingGroup>();
                cfg.CreateMap<PolicyTypeDTO, PolicyType>();
                cfg.CreateMap<PrefixDTO, Prefix>();
                cfg.CreateMap<TransactionTypeDTO, TransactionType>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<CheckingAccountTransactionDTO, CheckingAccountTransaction>();
                cfg.CreateMap<DTOs.CompanyDTO, Company>()
                .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name));
                cfg.CreateMap<ExchangeRateDTO, ExchangeRate>();
                cfg.CreateMap<CheckingAccountConceptDTO, CheckingAccountConcept>();
                cfg.CreateMap<IndividualDTO, Individual>();
                cfg.CreateMap<DocumentTypeDTO, DocumentType>();
                cfg.CreateMap<EconomicActivityDTO, UniquePersonService.V1.Models.EconomicActivity>();
                cfg.CreateMap<IdentificationDocumentDTO, IdentificationDocument>();
            });
            return config;
        }

        public static BrokersCheckingAccountTransactionItem ToModel(this BrokersCheckingAccountTransactionItemDTO brokersCheckingAccountTransactionItemDTO)
        {
            var config = CreateMapBrokersCheckingAccountTransactionItems();
            return config.Map<BrokersCheckingAccountTransactionItemDTO, BrokersCheckingAccountTransactionItem>(brokersCheckingAccountTransactionItemDTO);
        }

        public static IEnumerable<BrokersCheckingAccountTransactionItem> ToModels(this IEnumerable<BrokersCheckingAccountTransactionItemDTO> brokersCheckingAccountTransactionItemDTOs)
        {
            return brokersCheckingAccountTransactionItemDTOs.Select(ToModel);
        }

        public static IMapper CreateMapCheckingAccountConcepts()
        {
            var config = MapperCache.GetMapper<CheckingAccountConceptDTO, CheckingAccountConcept>(cfg =>
            {
                cfg.CreateMap<CheckingAccountConceptDTO, CheckingAccountConcept>();
            });
            return config;
        }

        public static CheckingAccountConcept ToModel(this CheckingAccountConceptDTO checkingAccountConceptDTO)
        {
            var config = CreateMapCheckingAccountConcepts();
            return config.Map<CheckingAccountConceptDTO, CheckingAccountConcept>(checkingAccountConceptDTO);
        }

        public static IEnumerable<CheckingAccountConcept> ToModels(this IEnumerable<CheckingAccountConceptDTO> checkingAccountConceptDTOs)
        {
            return checkingAccountConceptDTOs.Select(ToModel);
        }

        public static IMapper CreateMapCheckingAccountTransactions()
        {
            var config = MapperCache.GetMapper<CheckingAccountTransactionDTO, CheckingAccountTransaction>(cfg =>
            {
                cfg.CreateMap<CheckingAccountTransactionDTO, CheckingAccountTransaction>()
                        //.ForMember(dest => dest.AccountingNature, opt => opt.MapFrom(src => (AccountingNatures)src.AccountingNature))
                        ;
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<DTOs.CompanyDTO, Company>()
                .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name));
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<ExchangeRateDTO, ExchangeRate>();
                cfg.CreateMap<CheckingAccountConceptDTO, CheckingAccountConcept>();
                cfg.CreateMap<IndividualDTO, Individual>();
                cfg.CreateMap<DocumentTypeDTO, DocumentType>();
                cfg.CreateMap<EconomicActivityDTO, UniquePersonService.V1.Models.EconomicActivity>();
                cfg.CreateMap<IdentificationDocumentDTO, IdentificationDocument>();
            });
            return config;
        }

        public static CheckingAccountTransaction ToModel(this CheckingAccountTransactionDTO checkingAccountTransactionDTO)
        {
            var config = CreateMapCheckingAccountTransactions();
            return config.Map<CheckingAccountTransactionDTO, CheckingAccountTransaction>(checkingAccountTransactionDTO);
        }

        public static IEnumerable<CheckingAccountTransaction> ToModels(this IEnumerable<CheckingAccountTransactionDTO> checkingAccountTransactionDTOs)
        {
            return checkingAccountTransactionDTOs.Select(ToModel);
        }

        public static IMapper CreateMapClaimsPaymentRequestTransactions()
        {
            var config = MapperCache.GetMapper<ClaimsPaymentRequestTransactionDTO, ClaimsPaymentRequestTransaction>(cfg =>
            {
                cfg.CreateMap<ClaimsPaymentRequestTransactionDTO, ClaimsPaymentRequestTransaction>();
                cfg.CreateMap<ClaimsPaymentRequestTransactionItemDTO, ClaimsPaymentRequestTransactionItem>();
                cfg.CreateMap<PaymentRequestTypeDTO, PRCLAIM.PaymentRequestType>();
                cfg.CreateMap<ClaimDTO, CLAIMODL.Claim>()
                .ForMember(dest => dest.CoveredRiskType, opt => opt.MapFrom(src => (int)src.CoveredRiskType));
                cfg.CreateMap<TransactionDTO, Transaction>();
                cfg.CreateMap<TaxDTO, Tax>();
                cfg.CreateMap<TaxConditionDTO, TaxCondition>();
                cfg.CreateMap<TaxCategoryDTO, TaxCategory>();
                cfg.CreateMap<AccountingAccountDTO, GeneralLedgerServices.EEProvider.Models.AccountingAccount>();
                cfg.CreateMap<CostCenterDTO, EEProvider.Models.Imputations.CostCenter>();
                cfg.CreateMap<VoucherTypeDTO, VoucherType>();
                cfg.CreateMap<VoucherConceptDTO, VoucherConcept>();
                cfg.CreateMap<VoucherDTO, Voucher>();
                cfg.CreateMap<ConceptSourceDTO, PRCLAIM.ConceptSource>();
                cfg.CreateMap<AccountingConceptDTO, UniquePersonService.V1.Models.AccountingConcept>();
                cfg.CreateMap<MovementTypeDTO, PRCLAIM.MovementType>();
                cfg.CreateMap<ExchangeRateDTO, ExchangeRate>();
                cfg.CreateMap<PaymentMethodDTO, PaymentsModels.PaymentMethod>();

                cfg.CreateMap<IndividualDTO, Individual>();
                cfg.CreateMap<DocumentTypeDTO, DocumentType>();
                cfg.CreateMap<EconomicActivityDTO, UniquePersonService.V1.Models.EconomicActivity>();
                cfg.CreateMap<IdentificationDocumentDTO, IdentificationDocument>();

                cfg.CreateMap<PersonTypeDTO, PersonType>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<CompanyDTO, Company>()
                .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name));

                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<PaymentRequestNumberDTO, PaymentRequestNumber>();
                cfg.CreateMap<PaymentRequestDTO, PaymentRequest>();

                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<TransactionTypeDTO, TransactionType>();

                cfg.CreateMap<CurrencyDTO, Currency>();
            });
            return config;
        }

        public static ClaimsPaymentRequestTransaction ToModel(this ClaimsPaymentRequestTransactionDTO claimsPaymentRequestTransactionDTO)
        {
            var config = CreateMapClaimsPaymentRequestTransactions();
            return config.Map<ClaimsPaymentRequestTransactionDTO, ClaimsPaymentRequestTransaction>(claimsPaymentRequestTransactionDTO);
        }

        public static IEnumerable<ClaimsPaymentRequestTransaction> ToModels(this IEnumerable<ClaimsPaymentRequestTransactionDTO> claimsPaymentRequestTransactionDTOs)
        {
            return claimsPaymentRequestTransactionDTOs.Select(ToModel);
        }

        public static IMapper CreateMapClaimsPaymentRequestTransactionItems()
        {
            var config = MapperCache.GetMapper<ClaimsPaymentRequestTransactionItemDTO, ClaimsPaymentRequestTransactionItem>(cfg =>
            {
                cfg.CreateMap<ClaimsPaymentRequestTransactionItemDTO, ClaimsPaymentRequestTransactionItem>();
                cfg.CreateMap<PaymentRequestDTO, PaymentRequest>();
                cfg.CreateMap<PaymentRequestTypeDTO, PRCLAIM.PaymentRequestType>();
                cfg.CreateMap<ClaimDTO, CLAIMODL.Claim>()
                .ForMember(dest => dest.CoveredRiskType, opt => opt.MapFrom(src => (int)src.CoveredRiskType));
                cfg.CreateMap<TransactionDTO, Transaction>();
                cfg.CreateMap<TaxDTO, Tax>();
                cfg.CreateMap<TaxConditionDTO, TaxCondition>();
                cfg.CreateMap<TaxCategoryDTO, TaxCategory>();
                cfg.CreateMap<AccountingAccountDTO, GeneralLedgerServices.EEProvider.Models.AccountingAccount>();
                cfg.CreateMap<CostCenterDTO, EEProvider.Models.Imputations.CostCenter>();
                cfg.CreateMap<VoucherTypeDTO, VoucherType>();
                cfg.CreateMap<VoucherConceptDTO, VoucherConcept>();
                cfg.CreateMap<VoucherDTO, Voucher>();
                cfg.CreateMap<ConceptSourceDTO, PRCLAIM.ConceptSource>();
                cfg.CreateMap<AccountingConceptDTO, UniquePersonService.V1.Models.AccountingConcept>();
                cfg.CreateMap<MovementTypeDTO, PRCLAIM.MovementType>();
                cfg.CreateMap<ExchangeRateDTO, ExchangeRate>();
                cfg.CreateMap<PaymentMethodDTO, PaymentsModels.PaymentMethod>();

                cfg.CreateMap<IndividualDTO, Individual>();
                cfg.CreateMap<DocumentTypeDTO, DocumentType>();
                cfg.CreateMap<EconomicActivityDTO, UniquePersonService.V1.Models.EconomicActivity>();
                cfg.CreateMap<IdentificationDocumentDTO, IdentificationDocument>();

                cfg.CreateMap<PersonTypeDTO, PersonType>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<CompanyDTO, Company>()
                .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name));

                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<PaymentRequestNumberDTO, PaymentRequestNumber>();
                cfg.CreateMap<PaymentRequestDTO, PaymentRequest>();

                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<TransactionTypeDTO, TransactionType>();

                cfg.CreateMap<CurrencyDTO, Currency>();
            });
            return config;
        }

        public static ClaimsPaymentRequestTransactionItem ToModel(this ClaimsPaymentRequestTransactionItemDTO claimsPaymentRequestTransactionItemDTO)
        {
            var config = CreateMapClaimsPaymentRequestTransactionItems();
            return config.Map<ClaimsPaymentRequestTransactionItemDTO, ClaimsPaymentRequestTransactionItem>(claimsPaymentRequestTransactionItemDTO);
        }

        public static IEnumerable<ClaimsPaymentRequestTransactionItem> ToModels(this IEnumerable<ClaimsPaymentRequestTransactionItemDTO> claimsPaymentRequestTransactionItemDTOs)
        {
            return claimsPaymentRequestTransactionItemDTOs.Select(ToModel);
        }

        public static IMapper CreateMapCoInsuranceCheckingAccountItems()
        {
            var config = MapperCache.GetMapper<CoInsuranceCheckingAccountItemDTO, CoInsuranceCheckingAccountItem>(cfg =>
            {
                cfg.CreateMap<CoInsuranceCheckingAccountItemDTO, CoInsuranceCheckingAccountItem>();
            });
            return config;
        }

        public static CoInsuranceCheckingAccountItem ToModel(this CoInsuranceCheckingAccountItemDTO coInsuranceCheckingAccountItemDTO)
        {
            var config = CreateMapCoInsuranceCheckingAccountItems();
            return config.Map<CoInsuranceCheckingAccountItemDTO, CoInsuranceCheckingAccountItem>(coInsuranceCheckingAccountItemDTO);
        }

        public static IEnumerable<CoInsuranceCheckingAccountItem> ToModels(this IEnumerable<CoInsuranceCheckingAccountItemDTO> coInsuranceCheckingAccountItemDTOs)
        {
            return coInsuranceCheckingAccountItemDTOs.Select(ToModel);
        }

        public static IMapper CreateMapCoInsuranceCheckingAccountTransactions()
        {
            var config = MapperCache.GetMapper<CoInsuranceCheckingAccountTransactionDTO, CoInsuranceCheckingAccountTransaction>(cfg =>
            {
                cfg.CreateMap<CoInsuranceCheckingAccountTransactionDTO, CoInsuranceCheckingAccountTransaction>();
                cfg.CreateMap<CoInsuranceCheckingAccountTransactionItemDTO, CoInsuranceCheckingAccountTransactionItem>();
                cfg.CreateMap<IndividualDTO, Individual>();
                cfg.CreateMap<DocumentTypeDTO, DocumentType>();
                cfg.CreateMap<EconomicActivityDTO, UniquePersonService.V1.Models.EconomicActivity>();
                cfg.CreateMap<IdentificationDocumentDTO, IdentificationDocument>();

                cfg.CreateMap<PolicyDTO, Policy>();
                cfg.CreateMap<BeneficiaryDTO, Beneficiary>()
                .ForMember(dest => dest.BeneficiaryTypeDescription, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (CustomerType)src.CustomerType));
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<EndorsementDTO, Endorsement>();
                cfg.CreateMap<PayerComponentDTO, PayerComponent>();
                cfg.CreateMap<PaymentPlanDTO, PaymentPlan>();
                cfg.CreateMap<QuotaDTO, Quota>();
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<IssuanceAgencyDTO, IssuanceAgency>();
                cfg.CreateMap<IssuanceAgentDTO, IssuanceAgent>();
                cfg.CreateMap<IssuanceAgentTypeDTO, IssuanceAgentType>();
                cfg.CreateMap<IssuanceCommissionDTO, IssuanceCommission>();
                cfg.CreateMap<SubLineBusinessDTO, SubLineBusiness>();
                cfg.CreateMap<LineBusinessDTO, LineBusiness>();
                cfg.CreateMap<HolderDTO, Holder>()
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (CustomerType)src.CustomerType))
                .ForMember(dest => dest.IndividualType, opt => opt.MapFrom(src => (IndividualType)src.IndividualType));
                cfg.CreateMap<BillingGroupDTO, BillingGroup>();
                cfg.CreateMap<PolicyTypeDTO, PolicyType>();
                cfg.CreateMap<PrefixDTO, Prefix>();
                cfg.CreateMap<CoInsuranceCheckingAccountItemDTO, CoInsuranceCheckingAccountItem>();

                cfg.CreateMap<PersonTypeDTO, PersonType>();
                cfg.CreateMap<CompanyDTO, Company>()
                .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name));

                cfg.CreateMap<PaymentRequestNumberDTO, PaymentRequestNumber>();
                cfg.CreateMap<PaymentRequestDTO, PaymentRequest>();

                cfg.CreateMap<AmountDTO, Amount>();

            });
            return config;
        }

        public static CoInsuranceCheckingAccountTransaction ToModel(this CoInsuranceCheckingAccountTransactionDTO coInsuranceCheckingAccountTransactionDTO)
        {
            var config = CreateMapCoInsuranceCheckingAccountTransactions();
            return config.Map<CoInsuranceCheckingAccountTransactionDTO, CoInsuranceCheckingAccountTransaction>(coInsuranceCheckingAccountTransactionDTO);
        }

        public static IEnumerable<CoInsuranceCheckingAccountTransaction> ToModels(this IEnumerable<CoInsuranceCheckingAccountTransactionDTO> coInsuranceCheckingAccountTransactionDTOs)
        {
            return coInsuranceCheckingAccountTransactionDTOs.Select(ToModel);
        }

        public static IMapper CreateMapCoInsuranceCheckingAccountTransactionItems()
        {
            var config = MapperCache.GetMapper<CoInsuranceCheckingAccountTransactionItemDTO, CoInsuranceCheckingAccountTransactionItem>(cfg =>
            {
                cfg.CreateMap<CoInsuranceCheckingAccountTransactionItemDTO, CoInsuranceCheckingAccountTransactionItem>()
                        .ForMember(dest => dest.CoInsuranceType, opt => opt.MapFrom(src => (CoInsuranceTypes)src.CoInsuranceType))
                        //.ForMember(dest => dest.AccountingNature, opt => opt.MapFrom(src => (AccountingNatures)src.AccountingNature))
                        ;
                cfg.CreateMap<DTOs.CompanyDTO, Company>()
                .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name));
                cfg.CreateMap<CoInsuranceCheckingAccountTransactionItemDTO, CoInsuranceCheckingAccountTransactionItem>();
                cfg.CreateMap<PolicyDTO, Policy>();
                cfg.CreateMap<BeneficiaryDTO, Beneficiary>()
                .ForMember(dest => dest.BeneficiaryTypeDescription, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (CustomerType)src.CustomerType));
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<EndorsementDTO, Endorsement>();
                cfg.CreateMap<PayerComponentDTO, PayerComponent>();
                cfg.CreateMap<PaymentPlanDTO, PaymentPlan>();
                cfg.CreateMap<QuotaDTO, Quota>();
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<IssuanceAgencyDTO, IssuanceAgency>();
                cfg.CreateMap<IssuanceAgentDTO, IssuanceAgent>();
                cfg.CreateMap<IssuanceAgentTypeDTO, IssuanceAgentType>();
                cfg.CreateMap<IssuanceCommissionDTO, IssuanceCommission>();
                cfg.CreateMap<SubLineBusinessDTO, SubLineBusiness>();
                cfg.CreateMap<LineBusinessDTO, LineBusiness>();
                cfg.CreateMap<HolderDTO, Holder>()
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (CustomerType)src.CustomerType))
                .ForMember(dest => dest.IndividualType, opt => opt.MapFrom(src => (IndividualType)src.IndividualType));
                cfg.CreateMap<BillingGroupDTO, BillingGroup>();
                cfg.CreateMap<PolicyTypeDTO, PolicyType>();
                cfg.CreateMap<PrefixDTO, Prefix>();

                cfg.CreateMap<CoInsuranceCheckingAccountItemDTO, CoInsuranceCheckingAccountItem>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<ExchangeRateDTO, ExchangeRate>();
                cfg.CreateMap<CheckingAccountConceptDTO, CheckingAccountConcept>();
                cfg.CreateMap<IndividualDTO, Individual>();
                cfg.CreateMap<DocumentTypeDTO, DocumentType>();
                cfg.CreateMap<EconomicActivityDTO, UniquePersonService.V1.Models.EconomicActivity>();
                cfg.CreateMap<IdentificationDocumentDTO, IdentificationDocument>();
            });
            return config;
        }

        public static CoInsuranceCheckingAccountTransactionItem ToModel(this CoInsuranceCheckingAccountTransactionItemDTO coInsuranceCheckingAccountTransactionItemDTO)
        {
            var config = CreateMapCoInsuranceCheckingAccountTransactionItems();
            return config.Map<CoInsuranceCheckingAccountTransactionItemDTO, CoInsuranceCheckingAccountTransactionItem>(coInsuranceCheckingAccountTransactionItemDTO);
        }

        public static IEnumerable<CoInsuranceCheckingAccountTransactionItem> ToModels(this IEnumerable<CoInsuranceCheckingAccountTransactionItemDTO> coInsuranceCheckingAccountTransactionItemDTOs)
        {
            return coInsuranceCheckingAccountTransactionItemDTOs.Select(ToModel);
        }

        public static IMapper CreateMapDailyAccountingAnalysisCodes()
        {
            var config = MapperCache.GetMapper<DailyAccountingAnalysisCodeDTO, DailyAccountingAnalysisCode>(cfg =>
            {
                cfg.CreateMap<DailyAccountingAnalysisCodeDTO, DailyAccountingAnalysisCode>();
                cfg.CreateMap<AnalysisCodeDTO, EEProvider.Models.Imputations.AnalysisCode>();
                cfg.CreateMap<AnalysisConceptDTO, EEProvider.Models.Imputations.AnalysisConcept>();

            });
            return config;
        }

        public static DailyAccountingAnalysisCode ToModel(this DailyAccountingAnalysisCodeDTO dailyAccountingAnalysisCodeDTO)
        {
            var config = CreateMapDailyAccountingAnalysisCodes();
            return config.Map<DailyAccountingAnalysisCodeDTO, DailyAccountingAnalysisCode>(dailyAccountingAnalysisCodeDTO);
        }

        public static IEnumerable<DailyAccountingAnalysisCode> ToModels(this IEnumerable<DailyAccountingAnalysisCodeDTO> dailyAccountingAnalysisCodeDTOs)
        {
            return dailyAccountingAnalysisCodeDTOs.Select(ToModel);
        }

        public static IMapper CreateMapApplicationAccountingAnalysis()
        {
            var config = MapperCache.GetMapper<ApplicationAccountingAnalysisDTO, ApplicationAccountingAnalysis>(cfg =>
            {
                cfg.CreateMap<ApplicationAccountingAnalysisDTO, ApplicationAccountingAnalysis>();
                cfg.CreateMap<AnalysisCodeDTO, EEProvider.Models.Imputations.AnalysisCode>();
                cfg.CreateMap<AnalysisConceptDTO, EEProvider.Models.Imputations.AnalysisConcept>();

            });
            return config;
        }

        public static ApplicationAccountingAnalysis ToModel(this ApplicationAccountingAnalysisDTO applicationAccountingAnalysisDTO)
        {
            var config = CreateMapApplicationAccountingAnalysis();
            return config.Map<ApplicationAccountingAnalysisDTO, ApplicationAccountingAnalysis>(applicationAccountingAnalysisDTO);
        }

        public static IEnumerable<ApplicationAccountingAnalysis> ToModels(this IEnumerable<ApplicationAccountingAnalysisDTO> applicationAccountingAnalysisDTOs)
        {
            return applicationAccountingAnalysisDTOs.Select(ToModel);
        }

        public static IMapper CreateMapDailyAccountingCostCenters()
        {
            var config = MapperCache.GetMapper<DailyAccountingCostCenterDTO, DailyAccountingCostCenter>(cfg =>
            {
                cfg.CreateMap<DailyAccountingCostCenterDTO, DailyAccountingCostCenter>();
                cfg.CreateMap<CostCenterDTO, EEProvider.Models.Imputations.CostCenter>();
            });
            return config;
        }

        public static DailyAccountingCostCenter ToModel(this DailyAccountingCostCenterDTO dailyAccountingCostCenterDTO)
        {
            var config = CreateMapDailyAccountingCostCenters();
            return config.Map<DailyAccountingCostCenterDTO, DailyAccountingCostCenter>(dailyAccountingCostCenterDTO);
        }

        public static IEnumerable<DailyAccountingCostCenter> ToModels(this IEnumerable<DailyAccountingCostCenterDTO> dailyAccountingCostCenterDTOs)
        {
            return dailyAccountingCostCenterDTOs.Select(ToModel);
        }

        public static IMapper CreateMapApplicationAccountingCostCenter()
        {
            var config = MapperCache.GetMapper<ApplicationAccountingCostCenterDTO, ApplicationAccountingCostCenter>(cfg =>
            {
                cfg.CreateMap<ApplicationAccountingCostCenterDTO, ApplicationAccountingCostCenter>();
                cfg.CreateMap<CostCenterDTO, EEProvider.Models.Imputations.CostCenter>();
            });
            return config;
        }

        public static ApplicationAccountingCostCenter ToModel(this ApplicationAccountingCostCenterDTO applicationAccountingCostCenterDTO)
        {
            var config = CreateMapApplicationAccountingCostCenter();
            return config.Map<ApplicationAccountingCostCenterDTO, ApplicationAccountingCostCenter>(applicationAccountingCostCenterDTO);
        }

        public static IEnumerable<ApplicationAccountingCostCenter> ToModels(this IEnumerable<ApplicationAccountingCostCenterDTO> applicationAccountingCostCenterDTOs)
        {
            return applicationAccountingCostCenterDTOs.Select(ToModel);
        }

        public static IMapper CreateMapDailyAccountingTransactions()
        {
            var config = MapperCache.GetMapper<DailyAccountingTransactionDTO, DailyAccountingTransaction>(cfg =>
            {
                cfg.CreateMap<DailyAccountingTransactionDTO, DailyAccountingTransaction>();
                cfg.CreateMap<DailyAccountingTransactionItemDTO, DailyAccountingTransactionItem>()
                        .ForMember(dest => dest.AccountingNature, opt => opt.MapFrom(src => (AccountingNature)src.AccountingNature))
                        ;
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<DTOs.CompanyDTO, Company>()
                .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name));
                cfg.CreateMap<IndividualDTO, Individual>();
                cfg.CreateMap<DocumentTypeDTO, DocumentType>();
                cfg.CreateMap<EconomicActivityDTO, UniquePersonService.V1.Models.EconomicActivity>();
                cfg.CreateMap<IdentificationDocumentDTO, IdentificationDocument>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<ExchangeRateDTO, ExchangeRate>();
                cfg.CreateMap<BookAccountDTO, BookAccount>();
                cfg.CreateMap<DailyAccountingAnalysisCodeDTO, DailyAccountingAnalysisCode>();
                cfg.CreateMap<DailyAccountingCostCenterDTO, DailyAccountingCostCenter>();
                cfg.CreateMap<TransactionTypeDTO, TransactionType>();
                cfg.CreateMap<CostCenterDTO, EEProvider.Models.Imputations.CostCenter>();
                cfg.CreateMap<AnalysisConceptDTO, EEProvider.Models.Imputations.AnalysisConcept>();
                cfg.CreateMap<AnalysisConceptDTO, EEProvider.Models.Imputations.AnalysisCode>();
            });
            return config;
        }

        public static DailyAccountingTransaction ToModel(this DailyAccountingTransactionDTO dailyAccountingTransactionDTO)
        {
            var config = CreateMapDailyAccountingTransactions();
            return config.Map<DailyAccountingTransactionDTO, DailyAccountingTransaction>(dailyAccountingTransactionDTO);
        }

        public static IEnumerable<DailyAccountingTransaction> ToModels(this IEnumerable<DailyAccountingTransactionDTO> dailyAccountingTransactionDTOs)
        {
            return dailyAccountingTransactionDTOs.Select(ToModel);
        }

        public static IMapper CreateMapDailyAccountingTransactionItems()
        {
            var config = MapperCache.GetMapper<DailyAccountingTransactionItemDTO, DailyAccountingTransactionItem>(cfg =>
            {
                cfg.CreateMap<DailyAccountingTransactionItemDTO, DailyAccountingTransactionItem>()
                        .ForMember(dest => dest.AccountingNature, opt => opt.MapFrom(src => (AccountingNature)src.AccountingNature))
                        ;
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<DTOs.CompanyDTO, Company>()
                .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name));
                cfg.CreateMap<IndividualDTO, Individual>();
                cfg.CreateMap<DocumentTypeDTO, DocumentType>();
                cfg.CreateMap<EconomicActivityDTO, UniquePersonService.V1.Models.EconomicActivity>();
                cfg.CreateMap<IdentificationDocumentDTO, IdentificationDocument>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<ExchangeRateDTO, ExchangeRate>();
                cfg.CreateMap<BookAccountDTO, BookAccount>();
                cfg.CreateMap<DailyAccountingAnalysisCodeDTO, DailyAccountingAnalysisCode>();
                cfg.CreateMap<DailyAccountingCostCenterDTO, DailyAccountingCostCenter>();
                cfg.CreateMap<TransactionTypeDTO, TransactionType>();
            });
            return config;
        }

        public static DailyAccountingTransactionItem ToModel(this DailyAccountingTransactionItemDTO dailyAccountingTransactionItemDTO)
        {
            var config = CreateMapDailyAccountingTransactionItems();
            return config.Map<DailyAccountingTransactionItemDTO, DailyAccountingTransactionItem>(dailyAccountingTransactionItemDTO);
        }

        public static IEnumerable<DailyAccountingTransactionItem> ToModels(this IEnumerable<DailyAccountingTransactionItemDTO> dailyAccountingTransactionItemDTOs)
        {
            return dailyAccountingTransactionItemDTOs.Select(ToModel);
        }

        public static IMapper CreateMapApplicationAccountings()
        {
            var config = MapperCache.GetMapper<ApplicationAccountingDTO, ApplicationAccounting>(cfg =>
            {
                cfg.CreateMap<ApplicationAccountingDTO, ApplicationAccounting>()
                        .ForMember(dest => dest.AccountingNature, opt => opt.MapFrom(src => (AccountingNature)src.AccountingNature))
                        ;
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<IndividualDTO, Individual>();
                cfg.CreateMap<DocumentTypeDTO, DocumentType>();
                cfg.CreateMap<EconomicActivityDTO, UniquePersonService.V1.Models.EconomicActivity>();
                cfg.CreateMap<IdentificationDocumentDTO, IdentificationDocument>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<ExchangeRateDTO, ExchangeRate>();
                cfg.CreateMap<BookAccountDTO, BookAccount>();
                cfg.CreateMap<ApplicationAccountingAnalysisDTO, ApplicationAccountingAnalysis>();
                cfg.CreateMap<ApplicationAccountingCostCenterDTO, ApplicationAccountingCostCenter>();
                cfg.CreateMap<TransactionTypeDTO, TransactionType>();
                cfg.CreateMap<CostCenterDTO, EEProvider.Models.Imputations.CostCenter>();
                cfg.CreateMap<AnalysisConceptDTO, EEProvider.Models.Imputations.AnalysisConcept>();
                cfg.CreateMap<AnalysisCodeDTO, EEProvider.Models.Imputations.AnalysisCode>();
            });
            return config;
        }

        public static ApplicationAccounting ToModel(this ApplicationAccountingDTO applicationAccountingDTO)
        {
            var config = CreateMapApplicationAccountings();
            return config.Map<ApplicationAccountingDTO, ApplicationAccounting>(applicationAccountingDTO);
        }

        public static IEnumerable<ApplicationAccounting> ToModels(this IEnumerable<ApplicationAccountingDTO> applicationAccountingDTOs)
        {
            return applicationAccountingDTOs.Select(ToModel);
        }

        public static IMapper CreateMapDepositPremiumTransactions()
        {
            var config = MapperCache.GetMapper<DTOs.Imputations.DepositPremiumTransactionDTO, DepositPremiumTransaction>(cfg =>
            {
                cfg.CreateMap<DTOs.Imputations.DepositPremiumTransactionDTO, DepositPremiumTransaction>();
                cfg.CreateMap<CollectDTO, Collect>()
                        .ForMember(dest => dest.CollectType, opt => opt.MapFrom(src => (CollectTypes)src.CollectType))
                        .ForMember(dest => dest.AccountingCompany, opt => opt.MapFrom(src => new CompanyDTO { IndividualId = src.CompanyIndividualId, Name = src.AccountingCompany.Name }));
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<CollectConceptDTO, CollectConcept>();
                cfg.CreateMap<PaymentDTO, PaymentsModels.Payment>();
                cfg.CreateMap<PaymentMethodDTO, PaymentsModels.PaymentMethod>();
                cfg.CreateMap<ExchangeRateDTO, ExchangeRate>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<PaymentTaxDTO, PaymentTax>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<TaxDTO, TxServ.Tax>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<PersonDTO, Person>()
                .ForMember(dest => dest.IndividualType, opt => opt.MapFrom(src => (IndividualType)src.IndividualType))
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (IndividualType)src.CustomerType));
                cfg.CreateMap<DTOs.IdentificationDocumentDTO, IdentificationDocument>();
                cfg.CreateMap<DTOs.DocumentTypeDTO, DocumentType>();
                cfg.CreateMap<CompanyDTO, Company>()
                .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name));
                cfg.CreateMap<TransactionDTO, Transaction>();
                cfg.CreateMap<PersonTypeDTO, PersonType>();
                cfg.CreateMap<CollectControlPaymentDTO, CollectControlPayment>();
            });
            return config;
        }

        public static DepositPremiumTransaction ToModel(this DTOs.Imputations.DepositPremiumTransactionDTO depositPremiumTransactionDTO)
        {
            var config = CreateMapDepositPremiumTransactions();
            return config.Map<DTOs.Imputations.DepositPremiumTransactionDTO, DepositPremiumTransaction>(depositPremiumTransactionDTO);
        }

        public static IEnumerable<DepositPremiumTransaction> ToModels(this IEnumerable<DTOs.Imputations.DepositPremiumTransactionDTO> depositPremiumTransactionDTOs)
        {
            return depositPremiumTransactionDTOs.Select(ToModel);
        }

        //public static IMapper CreateMapImputations()
        //{
        //    var config = MapperCache.GetMapper<ImputationDTO, Imputation>(cfg =>
        //    {
        //        cfg.CreateMap<ImputationDTO, Imputation>()
        //                .ForMember(dest => dest.ImputationType, opt => opt.MapFrom(src => (ImputationTypes)src.ImputationType));
        //        cfg.CreateMap<TransactionTypeDTO, TransactionType>()
        //            .Include<PremiumReceivableTransactionDTO, PremiumReceivableTransaction>();
        //        cfg.CreateMap<PremiumReceivableTransactionDTO, PremiumReceivableTransaction>();
        //        cfg.CreateMap<PremiumReceivableTransactionItemDTO, PremiumReceivableTransactionItem>();
        //        cfg.CreateMap<AmountDTO, Amount>();
        //        cfg.CreateMap<CurrencyDTO, Currency>();
        //        cfg.CreateMap<PolicyDTO, Policy>();
        //        cfg.CreateMap<BeneficiaryDTO, Beneficiary>()
        //            .ForMember(dest => dest.BeneficiaryTypeDescription, opt => opt.MapFrom(src => src.Description))
        //            .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (CustomerType)src.CustomerType));
        //        cfg.CreateMap<EndorsementDTO, Endorsement>();
        //        cfg.CreateMap<PayerComponentDTO, PayerComponent>();
        //        cfg.CreateMap<PaymentPlanDTO, PaymentPlan>();
        //        cfg.CreateMap<QuotaDTO, Quota>();
        //        cfg.CreateMap<BranchDTO, Branch>();
        //        cfg.CreateMap<SalePointDTO, SalePoint>();
        //        cfg.CreateMap<IssuanceAgencyDTO, IssuanceAgency>();
        //        cfg.CreateMap<IssuanceAgentDTO, IssuanceAgent>();
        //        cfg.CreateMap<IssuanceAgentTypeDTO, IssuanceAgentType>(); 
        //        cfg.CreateMap<IssuanceCommissionDTO, IssuanceCommission>();
        //        cfg.CreateMap<SubLineBusinessDTO, SubLineBusiness>();
        //        cfg.CreateMap<LineBusinessDTO, LineBusiness>();
        //        cfg.CreateMap<HolderDTO, Holder>()
        //            .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (CustomerType)src.CustomerType))
        //            .ForMember(dest => dest.IndividualType, opt => opt.MapFrom(src => (IndividualType)src.IndividualType));
        //        cfg.CreateMap<BillingGroupDTO, BillingGroup>();
        //        cfg.CreateMap<PolicyTypeDTO, PolicyType>();
        //        cfg.CreateMap <ExchangeRateDTO, ExchangeRate>();
        //        cfg.CreateMap<PrefixDTO, Prefix>();
        //    });
        //    return config;
        //}

        //public static Imputation ToModel(this ImputationDTO imputationDTO)
        //{
        //    var config = CreateMapImputations();
        //    return config.Map<ImputationDTO, Imputation>(imputationDTO);
        //}

        //public static IEnumerable<Imputation> ToModels(this IEnumerable<ImputationDTO> imputationDTOs)
        //{
        //    return imputationDTOs.Select(ToModel);
        //}

        //public static IMapper CreateMapImputationTypes()
        //{
        //    var config = MapperCache.GetMapper<ImputationTypeDTO, ImputationType>(cfg =>
        //    {
        //        cfg.CreateMap<ImputationTypeDTO, ImputationType>();
        //    });
        //    return config;
        //}

        //public static ImputationType ToModel(this ImputationTypeDTO imputationTypeDTO)
        //{
        //    var config = CreateMapImputationTypes();
        //    return config.Map<ImputationTypeDTO, ImputationType>(imputationTypeDTO);
        //}

        //public static IEnumerable<ImputationType> ToModels(this IEnumerable<ImputationTypeDTO> imputationTypeDTOs)
        //{
        //    return imputationTypeDTOs.Select(ToModel);
        //}

        public static IMapper CreateMapInsuredLoanTransactions()
        {
            var config = MapperCache.GetMapper<InsuredLoanTransactionDTO, InsuredLoanTransaction>(cfg =>
            {
                cfg.CreateMap<InsuredLoanTransactionDTO, InsuredLoanTransaction>();
                cfg.CreateMap<InsuredLoanTransactionItemDTO, InsuredLoanTransactionItem>();
                cfg.CreateMap<TransactionTypeDTO, TransactionType>();
                cfg.CreateMap<ExchangeRateDTO, ExchangeRate>();
                cfg.CreateMap<IndividualDTO, Individual>();
                cfg.CreateMap<DocumentTypeDTO, DocumentType>();
                cfg.CreateMap<EconomicActivityDTO, UniquePersonService.V1.Models.EconomicActivity>();
                cfg.CreateMap<IdentificationDocumentDTO, IdentificationDocument>();
                cfg.CreateMap<ApplicationDTO, EEProvider.Models.Imputations.Application> ();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<CurrencyDTO, Currency>();
            });
            return config;
        }

        public static InsuredLoanTransaction ToModel(this InsuredLoanTransactionDTO insuredLoanTransactionDTO)
        {
            var config = CreateMapInsuredLoanTransactions();
            return config.Map<InsuredLoanTransactionDTO, InsuredLoanTransaction>(insuredLoanTransactionDTO);
        }

        public static IEnumerable<InsuredLoanTransaction> ToModels(this IEnumerable<InsuredLoanTransactionDTO> insuredLoanTransactionDTOs)
        {
            return insuredLoanTransactionDTOs.Select(ToModel);
        }

        //public static IMapper CreateMapInsuredLoanTransactionItems()
        //{
        //    var config = MapperCache.GetMapper<InsuredLoanTransactionItemDTO, InsuredLoanTransactionItem>(cfg =>
        //    {
        //        cfg.CreateMap<InsuredLoanTransactionItemDTO, InsuredLoanTransactionItem>()
        //                .ForMember(dest => dest.AccountingNature, opt => opt.MapFrom(src => (AccountingNatures)src.AccountingNature));
        //        cfg.CreateMap<ImputationDTO, Imputation>();
        //        cfg.CreateMap<IndividualDTO, Individual>();
        //        cfg.CreateMap<AmountDTO, Amount>();
        //        cfg.CreateMap<ExchangeRateDTO, ExchangeRate>();
        //    });
        //    return config;
        //}

        //public static InsuredLoanTransactionItem ToModel(this InsuredLoanTransactionItemDTO insuredLoanTransactionItemDTO)
        //{
        //    var config = CreateMapInsuredLoanTransactionItems();
        //    return config.Map<InsuredLoanTransactionItemDTO, InsuredLoanTransactionItem>(insuredLoanTransactionItemDTO);
        //}

        //public static IEnumerable<InsuredLoanTransactionItem> ToModels(this IEnumerable<InsuredLoanTransactionItemDTO> insuredLoanTransactionItemDTOs)
        //{
        //    return insuredLoanTransactionItemDTOs.Select(ToModel);
        //}

        public static IMapper CreateMapJournalEntries()
        {
            var config = MapperCache.GetMapper<JournalEntryDTO, JournalEntry>(cfg =>
            {
                cfg.CreateMap<JournalEntryDTO, JournalEntry>();
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<DTOs.CompanyDTO, Company>()
                .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name));
                cfg.CreateMap<IndividualDTO, Individual>();
                cfg.CreateMap<DocumentTypeDTO, DocumentType>();
                cfg.CreateMap<EconomicActivityDTO, UniquePersonService.V1.Models.EconomicActivity>();
                cfg.CreateMap<IdentificationDocumentDTO, IdentificationDocument>();
                cfg.CreateMap<PersonTypeDTO, PersonType>();
                cfg.CreateMap<ApplicationDTO, EEProvider.Models.Imputations.Application> ()
                        //.ForMember(dest => dest.ImputationType, opt => opt.MapFrom(src => (ImputationTypes)src.ImputationType))
                        ;
                cfg.CreateMap<TransactionTypeDTO, TransactionType>();
                cfg.CreateMap<TransactionDTO, Transaction>();
            });
            return config;
        }

        public static EEProvider.Models.Imputations.JournalEntry ToModel(this JournalEntryDTO journalEntryDTO)
        {
            var config = CreateMapJournalEntries();
            return config.Map<JournalEntryDTO, Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations.JournalEntry>(journalEntryDTO);
        }

        public static IEnumerable<EEProvider.Models.Imputations.JournalEntry> ToModels(this IEnumerable<JournalEntryDTO> journalEntryDTOs)
        {
            return journalEntryDTOs.Select(ToModel);
        }

        public static IMapper CreateMapPaymentRequestTransactions()
        {
            var config = MapperCache.GetMapper<PaymentRequestTransactionDTO, PaymentRequestTransaction>(cfg =>
            {
                cfg.CreateMap<PaymentRequestTransactionDTO, PaymentRequestTransaction>();
                cfg.CreateMap<PaymentRequestTransactionItemDTO, PaymentRequestTransactionItem>();
                cfg.CreateMap<TransactionTypeDTO, TransactionType>();
                cfg.CreateMap<PaymentRequestDTO, PaymentRequest>();
                cfg.CreateMap<PaymentRequestNumberDTO, PaymentRequestNumber>();
                cfg.CreateMap<ConceptSourceDTO, EEProvider.Models.Claims.PaymentRequest.ConceptSource>();
                cfg.CreateMap<MovementTypeDTO, PRCLAIM.MovementType>();
                cfg.CreateMap<CompanyDTO, Company>()
                .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name));
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<PersonTypeDTO, PersonType>();
                cfg.CreateMap<IndividualDTO, Individual>();
                cfg.CreateMap<DocumentTypeDTO, DocumentType>();
                cfg.CreateMap<EconomicActivityDTO, UniquePersonService.V1.Models.EconomicActivity>();
                cfg.CreateMap<IdentificationDocumentDTO, IdentificationDocument>();
                cfg.CreateMap<PaymentMethodDTO, PaymentsModels.PaymentMethod>();
                cfg.CreateMap<ExchangeRateDTO, ExchangeRate>();
                cfg.CreateMap<TaxCategoryDTO, TaxCategory>();
                cfg.CreateMap<TaxDTO, Tax>();
                cfg.CreateMap<VoucherConceptTaxDTO, VoucherConceptTax>();
                cfg.CreateMap<CostCenterDTO, EEProvider.Models.Imputations.CostCenter>();
                cfg.CreateMap<AccountingAccountDTO, GeneralLedgerServices.EEProvider.Models.AccountingAccount>();
                cfg.CreateMap<AccountingConceptDTO, Application.UniquePersonService.V1.Models.AccountingConcept>();
                cfg.CreateMap<VoucherConceptDTO, VoucherConcept>();
                cfg.CreateMap<VoucherTypeDTO, VoucherType>();
                cfg.CreateMap<VoucherDTO, Voucher>();
                cfg.CreateMap<PaymentRequestTypeDTO, PRCLAIM.PaymentRequestType>();
                cfg.CreateMap<ClaimDTO, CLAIMODL.Claim>()
                .ForMember(dest => dest.CoveredRiskType, opt => opt.MapFrom(src => (Application.CommonService.Enums.CoveredRiskType)src.CoveredRiskType));
                cfg.CreateMap<TransactionDTO, Transaction>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<CurrencyDTO, Currency>();
            });
            return config;
        }

        public static PaymentRequestTransaction ToModel(this PaymentRequestTransactionDTO paymentRequestTransactionDTO)
        {
            var config = CreateMapPaymentRequestTransactions();
            return config.Map<PaymentRequestTransactionDTO, PaymentRequestTransaction>(paymentRequestTransactionDTO);
        }

        public static IEnumerable<PaymentRequestTransaction> ToModels(this IEnumerable<PaymentRequestTransactionDTO> paymentRequestTransactionDTOs)
        {
            return paymentRequestTransactionDTOs.Select(ToModel);
        }

        public static IMapper CreateMapPaymentRequestTransactionItems()
        {
            var config = MapperCache.GetMapper<PaymentRequestTransactionItemDTO, PaymentRequestTransactionItem>(cfg =>
            {
                cfg.CreateMap<PaymentRequestTransactionItemDTO, PaymentRequestTransactionItem>();
                cfg.CreateMap<TransactionTypeDTO, TransactionType>();
                cfg.CreateMap<PaymentRequestDTO, PaymentRequest>();
                cfg.CreateMap<PaymentRequestNumberDTO, PaymentRequestNumber>();
                cfg.CreateMap<ConceptSourceDTO, EEProvider.Models.Claims.PaymentRequest.ConceptSource>();
                cfg.CreateMap<MovementTypeDTO, PRCLAIM.MovementType>();
                cfg.CreateMap<CompanyDTO, Company>()
                .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name));
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<PersonTypeDTO, PersonType>();
                cfg.CreateMap<IndividualDTO, Individual>();
                cfg.CreateMap<DocumentTypeDTO, DocumentType>();
                cfg.CreateMap<EconomicActivityDTO, UniquePersonService.V1.Models.EconomicActivity>();
                cfg.CreateMap<IdentificationDocumentDTO, IdentificationDocument>();
                cfg.CreateMap<PaymentMethodDTO, PaymentsModels.PaymentMethod>();
                cfg.CreateMap<ExchangeRateDTO, ExchangeRate>();
                cfg.CreateMap<TaxCategoryDTO, TaxCategory>();
                cfg.CreateMap<TaxDTO, Tax>();
                cfg.CreateMap<VoucherConceptTaxDTO, VoucherConceptTax>();
                cfg.CreateMap<CostCenterDTO, EEProvider.Models.Imputations.CostCenter>();
                cfg.CreateMap<AccountingAccountDTO, GeneralLedgerServices.EEProvider.Models.AccountingAccount>();
                cfg.CreateMap<AccountingConceptDTO, Application.UniquePersonService.V1.Models.AccountingConcept>();
                cfg.CreateMap<VoucherConceptDTO, VoucherConcept>();
                cfg.CreateMap<VoucherTypeDTO, VoucherType>();
                cfg.CreateMap<VoucherDTO, Voucher>();
                cfg.CreateMap<PaymentRequestTypeDTO, PRCLAIM.PaymentRequestType>();
                cfg.CreateMap<ClaimDTO, CLAIMODL.Claim>()
                .ForMember(dest => dest.CoveredRiskType, opt => opt.MapFrom(src => (Application.CommonService.Enums.CoveredRiskType)src.CoveredRiskType));
                cfg.CreateMap<TransactionDTO, Transaction>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<CurrencyDTO, Currency>();
            });
            return config;
        }

        public static PaymentRequestTransactionItem ToModel(this PaymentRequestTransactionItemDTO paymentRequestTransactionItemDTO)
        {
            var config = CreateMapPaymentRequestTransactionItems();
            return config.Map<PaymentRequestTransactionItemDTO, PaymentRequestTransactionItem>(paymentRequestTransactionItemDTO);
        }

        public static IEnumerable<PaymentRequestTransactionItem> ToModels(this IEnumerable<PaymentRequestTransactionItemDTO> paymentRequestTransactionItemDTOs)
        {
            return paymentRequestTransactionItemDTOs.Select(ToModel);
        }

        public static IMapper CreateMapPreLiquidations()
        {
            var config = MapperCache.GetMapper<PreLiquidationDTO, PreLiquidation>(cfg =>
            {
                cfg.CreateMap<PreLiquidationDTO, PreLiquidation>();
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<DTOs.CompanyDTO, Company>()
                .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name));
                cfg.CreateMap<IndividualDTO, Individual>();
                cfg.CreateMap<DocumentTypeDTO, DocumentType>();
                cfg.CreateMap<EconomicActivityDTO, UniquePersonService.V1.Models.EconomicActivity>();
                cfg.CreateMap<IdentificationDocumentDTO, IdentificationDocument>();
                cfg.CreateMap<PersonTypeDTO, PersonType>();
                cfg.CreateMap<ApplicationDTO, EEProvider.Models.Imputations.Application> ()
                        //.ForMember(dest => dest.ImputationType, opt => opt.MapFrom(src => (ImputationTypes)src.ImputationType))
                        ;
                cfg.CreateMap<TransactionTypeDTO, TransactionType>();
            });
            return config;
        }

        public static PreLiquidation ToModel(this PreLiquidationDTO preLiquidationDTO)
        {
            var config = CreateMapPreLiquidations();
            return config.Map<PreLiquidationDTO, PreLiquidation>(preLiquidationDTO);
        }

        //public static IEnumerable<PreLiquidation> ToModels(this IEnumerable<PreLiquidationDTO> preLiquidationDTOs)
        //{
        //    return preLiquidationDTOs.Select(ToModel);
        //}

        public static IMapper CreateMapPremiumReceivableTransactions()
        {
            var config = MapperCache.GetMapper<PremiumReceivableTransactionDTO, PremiumReceivableTransaction>(cfg =>
            {
                cfg.CreateMap<PremiumReceivableTransactionDTO, PremiumReceivableTransaction>();
                cfg.CreateMap<PremiumReceivableTransactionItemDTO, PremiumReceivableTransactionItem>();
                cfg.CreateMap<TransactionTypeDTO, TransactionType>();
                cfg.CreateMap<PolicyDTO, Policy>();
                cfg.CreateMap<BeneficiaryDTO, Beneficiary>()
                .ForMember(dest => dest.BeneficiaryTypeDescription, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (CustomerType)src.CustomerType));
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<EndorsementDTO, Endorsement>();
                cfg.CreateMap<PayerComponentDTO, PayerComponent>();
                cfg.CreateMap<PaymentPlanDTO, PaymentPlan>();
                cfg.CreateMap<QuotaDTO, Quota>();
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<IssuanceAgencyDTO, IssuanceAgency>();
                cfg.CreateMap<IssuanceAgentDTO, IssuanceAgent>();
                cfg.CreateMap<IssuanceAgentTypeDTO, IssuanceAgentType>();
                cfg.CreateMap<IssuanceCommissionDTO, IssuanceCommission>();
                cfg.CreateMap<SubLineBusinessDTO, SubLineBusiness>();
                cfg.CreateMap<LineBusinessDTO, LineBusiness>();
                cfg.CreateMap<HolderDTO, Holder>()
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (CustomerType)src.CustomerType))
                .ForMember(dest => dest.IndividualType, opt => opt.MapFrom(src => (IndividualType)src.IndividualType));
                cfg.CreateMap<BillingGroupDTO, BillingGroup>();
                cfg.CreateMap<PolicyTypeDTO, PolicyType>();
                cfg.CreateMap<PrefixDTO, Prefix>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<CurrencyDTO, Currency>();
            });
            return config;
        }

        public static PremiumReceivableTransaction ToModel(this PremiumReceivableTransactionDTO premiumReceivableTransactionDTO)
        {
            var config = CreateMapPremiumReceivableTransactions();
            return config.Map<PremiumReceivableTransactionDTO, PremiumReceivableTransaction>(premiumReceivableTransactionDTO);
        }

        public static IEnumerable<PremiumReceivableTransaction> ToModels(this IEnumerable<PremiumReceivableTransactionDTO> premiumReceivableTransactionDTOs)
        {
            return premiumReceivableTransactionDTOs.Select(ToModel);
        }

        public static IMapper CreateMapPremiumReceivableTransactionItems()
        {
            var config = MapperCache.GetMapper<PremiumReceivableTransactionItemDTO, PremiumReceivableTransactionItem>(cfg =>
            {
                cfg.CreateMap<PremiumReceivableTransactionItemDTO, PremiumReceivableTransactionItem>();
                cfg.CreateMap<PolicyDTO, Policy>();
                cfg.CreateMap<ExchangeRateDTO, ExchangeRate>();
                cfg.CreateMap<BeneficiaryDTO, Beneficiary>()
                .ForMember(dest => dest.BeneficiaryTypeDescription, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (CustomerType)src.CustomerType));
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<EndorsementDTO, Endorsement>();
                cfg.CreateMap<PayerComponentDTO, PayerComponent>();
                cfg.CreateMap<PaymentPlanDTO, PaymentPlan>();
                cfg.CreateMap<QuotaDTO, Quota>();
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<IssuanceAgencyDTO, IssuanceAgency>();
                cfg.CreateMap<IssuanceAgentDTO, IssuanceAgent>();
                cfg.CreateMap<IssuanceAgentTypeDTO, IssuanceAgentType>();
                cfg.CreateMap<IssuanceCommissionDTO, IssuanceCommission>();
                cfg.CreateMap<SubLineBusinessDTO, SubLineBusiness>();
                cfg.CreateMap<LineBusinessDTO, LineBusiness>();
                cfg.CreateMap<HolderDTO, Holder>()
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (CustomerType)src.CustomerType))
                .ForMember(dest => dest.IndividualType, opt => opt.MapFrom(src => (IndividualType)src.IndividualType));
                cfg.CreateMap<BillingGroupDTO, BillingGroup>();
                cfg.CreateMap<PolicyTypeDTO, PolicyType>();
                cfg.CreateMap<PrefixDTO, Prefix>();
                cfg.CreateMap<AmountDTO, Amount>();
            });
            return config;
        }

        public static PremiumReceivableTransactionItem ToModel(this PremiumReceivableTransactionItemDTO premiumReceivableTransactionItemDTO)
        {
            var config = CreateMapPremiumReceivableTransactionItems();
            return config.Map<PremiumReceivableTransactionItemDTO, PremiumReceivableTransactionItem>(premiumReceivableTransactionItemDTO);
        }

        public static IEnumerable<PremiumReceivableTransactionItem> ToModels(this IEnumerable<PremiumReceivableTransactionItemDTO> premiumReceivableTransactionItemDTOs)
        {
            return premiumReceivableTransactionItemDTOs.Select(ToModel);
        }

        public static IMapper CreateMapReinsuranceCheckingAccountItems()
        {
            var config = MapperCache.GetMapper<DTOs.Imputations.ReinsuranceCheckingAccountItemDTO, ReinsuranceCheckingAccountItem>(cfg =>
            {
                cfg.CreateMap<DTOs.Imputations.ReinsuranceCheckingAccountItemDTO, ReinsuranceCheckingAccountItem>();
            });
            return config;
        }

        public static ReinsuranceCheckingAccountItem ToModel(this DTOs.Imputations.ReinsuranceCheckingAccountItemDTO reinsuranceCheckingAccountItemDTO)
        {
            var config = CreateMapReinsuranceCheckingAccountItems();
            return config.Map<DTOs.Imputations.ReinsuranceCheckingAccountItemDTO, ReinsuranceCheckingAccountItem>(reinsuranceCheckingAccountItemDTO);
        }

        public static IEnumerable<ReinsuranceCheckingAccountItem> ToModels(this IEnumerable<DTOs.Imputations.ReinsuranceCheckingAccountItemDTO> reinsuranceCheckingAccountItemDTOs)
        {
            return reinsuranceCheckingAccountItemDTOs.Select(ToModel);
        }

        public static IMapper CreateMapReInsuranceCheckingAccountTransactions()
        {
            var config = MapperCache.GetMapper<ReInsuranceCheckingAccountTransactionDTO, ReInsuranceCheckingAccountTransaction>(cfg =>
            {
                cfg.CreateMap<ReInsuranceCheckingAccountTransactionDTO, ReInsuranceCheckingAccountTransaction>();
                cfg.CreateMap<ReInsuranceCheckingAccountTransactionItemDTO, ReInsuranceCheckingAccountTransactionItem>();
                cfg.CreateMap<TransactionTypeDTO, TransactionType>();
                cfg.CreateMap<DTOs.Imputations.ReinsuranceCheckingAccountItemDTO, ReinsuranceCheckingAccountItem>();
                cfg.CreateMap<CompanyDTO, Company>()
                .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name));
                cfg.CreateMap<PrefixDTO, Prefix>();
                cfg.CreateMap<CheckingAccountTransactionDTO, CheckingAccountTransaction>();
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<ExchangeRateDTO, ExchangeRate>();
                cfg.CreateMap<CheckingAccountConceptDTO, CheckingAccountConcept>();
                cfg.CreateMap<IndividualDTO, Individual>();
                cfg.CreateMap<DocumentTypeDTO, DocumentType>();
                cfg.CreateMap<EconomicActivityDTO, UniquePersonService.V1.Models.EconomicActivity>();
                cfg.CreateMap<IdentificationDocumentDTO, IdentificationDocument>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<CurrencyDTO, Currency>();
            });
            return config;
        }

        public static ReInsuranceCheckingAccountTransaction ToModel(this ReInsuranceCheckingAccountTransactionDTO reInsuranceCheckingAccountTransactionDTO)
        {
            var config = CreateMapReInsuranceCheckingAccountTransactions();
            return config.Map<ReInsuranceCheckingAccountTransactionDTO, ReInsuranceCheckingAccountTransaction>(reInsuranceCheckingAccountTransactionDTO);
        }

        public static IEnumerable<ReInsuranceCheckingAccountTransaction> ToModels(this IEnumerable<ReInsuranceCheckingAccountTransactionDTO> reInsuranceCheckingAccountTransactionDTOs)
        {
            return reInsuranceCheckingAccountTransactionDTOs.Select(ToModel);
        }

        public static IMapper CreateMapReInsuranceCheckingAccountTransactionItems()
        {
            var config = MapperCache.GetMapper<ReInsuranceCheckingAccountTransactionItemDTO, ReInsuranceCheckingAccountTransactionItem>(cfg =>
            {
                cfg.CreateMap<ReInsuranceCheckingAccountTransactionItemDTO, ReInsuranceCheckingAccountTransactionItem>()
                        //.ForMember(dest => dest.AccountingNature, opt => opt.MapFrom(src => (AccountingNatures)src.AccountingNature))
                        ;
                cfg.CreateMap<DTOs.CompanyDTO, Company>()
                .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name));
                cfg.CreateMap<PrefixDTO, Prefix>();
                cfg.CreateMap<ReInsuranceCheckingAccountTransactionItemDTO, ReInsuranceCheckingAccountTransactionItem>();
                cfg.CreateMap<DTOs.Imputations.ReinsuranceCheckingAccountItemDTO, ReinsuranceCheckingAccountItem>();
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<ExchangeRateDTO, ExchangeRate>();
                cfg.CreateMap<CheckingAccountConceptDTO, CheckingAccountConcept>();
                cfg.CreateMap<IndividualDTO, Individual>();
                cfg.CreateMap<DocumentTypeDTO, DocumentType>();
                cfg.CreateMap<EconomicActivityDTO, UniquePersonService.V1.Models.EconomicActivity>();
                cfg.CreateMap<IdentificationDocumentDTO, IdentificationDocument>();
            });
            return config;
        }

        public static ReInsuranceCheckingAccountTransactionItem ToModel(this ReInsuranceCheckingAccountTransactionItemDTO reInsuranceCheckingAccountTransactionItemDTO)
        {
            var config = CreateMapReInsuranceCheckingAccountTransactionItems();
            return config.Map<ReInsuranceCheckingAccountTransactionItemDTO, ReInsuranceCheckingAccountTransactionItem>(reInsuranceCheckingAccountTransactionItemDTO);
        }

        public static IEnumerable<ReInsuranceCheckingAccountTransactionItem> ToModels(this IEnumerable<ReInsuranceCheckingAccountTransactionItemDTO> reInsuranceCheckingAccountTransactionItemDTOs)
        {
            return reInsuranceCheckingAccountTransactionItemDTOs.Select(ToModel);
        }

        public static IMapper CreateMapTransactionTypes()
        {
            var config = MapperCache.GetMapper<TransactionTypeDTO, TransactionType>(cfg =>
            {
                cfg.CreateMap<TransactionTypeDTO, TransactionType>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<CurrencyDTO, Currency>();
            });
            return config;
        }

        public static TransactionType ToModel(this TransactionTypeDTO transactionTypeDTO)
        {
            var config = CreateMapTransactionTypes();
            return config.Map<TransactionTypeDTO, TransactionType>(transactionTypeDTO);
        }

        public static IEnumerable<TransactionType> ToModels(this IEnumerable<TransactionTypeDTO> transactionTypeDTOs)
        {
            return transactionTypeDTOs.Select(ToModel);
        }

        public static IMapper CreateMapCreditNotes()
        {
            var config = MapperCache.GetMapper<CreditNoteDTO, CreditNote>(cfg =>
            {
                cfg.CreateMap<CreditNoteDTO, CreditNote>()
                        .ForMember(dest => dest.CreditNoteStatus, opt => opt.MapFrom(src => (CreditNoteStatus)src.CreditNoteStatus));
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<CreditNoteItemDTO, CreditNoteItem>();
                cfg.CreateMap<PolicyDTO, Policy>();
                cfg.CreateMap<BeneficiaryDTO, Beneficiary>()
                .ForMember(dest => dest.BeneficiaryTypeDescription, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (CustomerType)src.CustomerType));
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<EndorsementDTO, Endorsement>();
                cfg.CreateMap<PayerComponentDTO, PayerComponent>();
                cfg.CreateMap<PaymentPlanDTO, PaymentPlan>();
                cfg.CreateMap<QuotaDTO, Quota>();
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<IssuanceAgencyDTO, IssuanceAgency>();
                cfg.CreateMap<IssuanceAgentDTO, IssuanceAgent>();
                cfg.CreateMap<IssuanceAgentTypeDTO, IssuanceAgentType>();
                cfg.CreateMap<IssuanceCommissionDTO, IssuanceCommission>();
                cfg.CreateMap<SubLineBusinessDTO, SubLineBusiness>();
                cfg.CreateMap<LineBusinessDTO, LineBusiness>();
                cfg.CreateMap<HolderDTO, Holder>()
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (CustomerType)src.CustomerType))
                .ForMember(dest => dest.IndividualType, opt => opt.MapFrom(src => (IndividualType)src.IndividualType));
                cfg.CreateMap<BillingGroupDTO, BillingGroup>();
                cfg.CreateMap<PolicyTypeDTO, PolicyType>();
                cfg.CreateMap<PrefixDTO, Prefix>();
            });
            return config;
        }

        public static CreditNote ToModel(this CreditNoteDTO creditNoteDTO)
        {
            var config = CreateMapCreditNotes();
            return config.Map<CreditNoteDTO, CreditNote>(creditNoteDTO);
        }

        public static IEnumerable<CreditNote> ToModels(this IEnumerable<CreditNoteDTO> creditNoteDTOs)
        {
            return creditNoteDTOs.Select(ToModel);
        }

        //public static IMapper CreateMapCreditNoteItems()
        //{
        //    var config = MapperCache.GetMapper<CreditNoteItemDTO, CreditNoteItem>(cfg =>
        //    {
        //        cfg.CreateMap<CreditNoteItemDTO, CreditNoteItem>();
        //        cfg.CreateMap<PolicyDTO, Policy>();
        //    });
        //    return config;
        //}

        //public static CreditNoteItem ToModel(this CreditNoteItemDTO creditNoteItemDTO)
        //{
        //    var config = CreateMapCreditNoteItems();
        //    return config.Map<CreditNoteItemDTO, CreditNoteItem>(creditNoteItemDTO);
        //}

        //public static IEnumerable<CreditNoteItem> ToModels(this IEnumerable<CreditNoteItemDTO> creditNoteItemDTOs)
        //{
        //    return creditNoteItemDTOs.Select(ToModel);
        //}


        //public static IMapper CreateMapCancellationPolicyTypes()
        //{
        //    var config = MapperCache.GetMapper<CancellationPolicyTypeDTO, CancellationPolicyType>(cfg =>
        //    {
        //        cfg.CreateMap<CancellationPolicyTypeDTO, CancellationPolicyType>();
        //    });
        //    return config;
        //}

        //public static CancellationPolicyType ToModel(this CancellationPolicyTypeDTO cancellationPolicyTypeDTO)
        //{
        //    var config = CreateMapCancellationPolicyTypes();
        //    return config.Map<CancellationPolicyTypeDTO, CancellationPolicyType>(cancellationPolicyTypeDTO);
        //}

        //public static IEnumerable<CancellationPolicyType> ToModels(this IEnumerable<CancellationPolicyTypeDTO> cancellationPolicyTypeDTOs)
        //{
        //    return cancellationPolicyTypeDTOs.Select(ToModel);
        //}

        public static IMapper CreateMapCancellationLimits()
        {
            var config = MapperCache.GetMapper<CancellationLimitDTO, CancellationLimit>(cfg =>
            {
                cfg.CreateMap<CancellationLimitDTO, CancellationLimit>();
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
            });
            return config;
        }

        public static CancellationLimit ToModel(this CancellationLimitDTO cancellationLimit)
        {
            var config = CreateMapCancellationLimits();
            return config.Map<CancellationLimitDTO, CancellationLimit>(cancellationLimit);
        }

        public static IEnumerable<CancellationLimit> ToModels(this IEnumerable<CancellationLimitDTO> cancellationLimit)
        {
            return cancellationLimit.Select(ToModel);
        }

        public static IMapper CreateMapExclusions()
        {
            var config = MapperCache.GetMapper<ExclusionDTO, Exclusion>(cfg =>
            {
                cfg.CreateMap<ExclusionDTO, Exclusion>();
                cfg.CreateMap<PersonDTO, Person>()
                .ForMember(dest => dest.IndividualType, opt => opt.MapFrom(src => (IndividualType)src.IndividualType))
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (IndividualType)src.CustomerType));
                cfg.CreateMap<IdentificationDocumentDTO, IdentificationDocument>();
                cfg.CreateMap<DocumentTypeDTO, DocumentType>();
                cfg.CreateMap<PolicyDTO, Policy>();
                cfg.CreateMap<BeneficiaryDTO, Beneficiary>()
                .ForMember(dest => dest.BeneficiaryTypeDescription, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (CustomerType)src.CustomerType));
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<EndorsementDTO, Endorsement>();
                cfg.CreateMap<PayerComponentDTO, PayerComponent>();
                cfg.CreateMap<PaymentPlanDTO, PaymentPlan>();
                cfg.CreateMap<QuotaDTO, Quota>();
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<IssuanceAgencyDTO, IssuanceAgency>();
                cfg.CreateMap<IssuanceAgentDTO, IssuanceAgent>();
                cfg.CreateMap<IssuanceAgentTypeDTO, IssuanceAgentType>();
                cfg.CreateMap<IssuanceCommissionDTO, IssuanceCommission>();
                cfg.CreateMap<SubLineBusinessDTO, SubLineBusiness>();
                cfg.CreateMap<LineBusinessDTO, LineBusiness>();
                cfg.CreateMap<HolderDTO, Holder>()
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (CustomerType)src.CustomerType))
                .ForMember(dest => dest.IndividualType, opt => opt.MapFrom(src => (IndividualType)src.IndividualType));
                cfg.CreateMap<BillingGroupDTO, BillingGroup>();
                cfg.CreateMap<PolicyTypeDTO, PolicyType>();
                cfg.CreateMap<PrefixDTO, Prefix>();
            });
            return config;
        }

        public static Exclusion ToModel(this ExclusionDTO exclusionDTO)
        {
            var config = CreateMapExclusions();
            return config.Map<ExclusionDTO, Exclusion>(exclusionDTO);
        }

        public static IEnumerable<Exclusion> ToModels(this IEnumerable<ExclusionDTO> exclusionDTOs)
        {
            return exclusionDTOs.Select(ToModel);
        }

        public static IMapper CreateMapBankAccountCompanies()
        {
            var config = MapperCache.GetMapper<BankAccountCompanyDTO, BankAccountCompany>(cfg =>
            {
                cfg.CreateMap<BankAccountCompanyDTO, BankAccountCompany>();
                cfg.CreateMap<BankAccountTypeDTO, BankAccountType>();
                cfg.CreateMap<BankDTO, Bank>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<AccountingAccountDTO, GeneralLedgerServices.EEProvider.Models.AccountingAccount>();
            });
            return config;
        }


        public static BankAccountCompany ToModel(this BankAccountCompanyDTO bankAccountCompanyDTO)
        {
            var config = CreateMapBankAccountCompanies();
            return config.Map<BankAccountCompanyDTO, BankAccountCompany>(bankAccountCompanyDTO);
        }

        public static IEnumerable<BankAccountCompany> ToModels(this IEnumerable<BankAccountCompanyDTO> bankAccountCompanyDTOs)
        {
            return bankAccountCompanyDTOs.Select(ToModel);
        }

        public static IMapper CreateMapPersons()
        {
            var config = MapperCache.GetMapper<PersonDTO, Person>(cfg =>
            {
                cfg.CreateMap<PersonDTO, Person>()
                .ForMember(dest => dest.IndividualType, opt => opt.MapFrom(src => (int)src.IndividualType))
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (int)src.CustomerType));
                cfg.CreateMap<DocumentTypeDTO, DocumentType>();
                cfg.CreateMap<DTOs.IdentificationDocumentDTO, IdentificationDocument>();
                cfg.CreateMap<PersonTypeDTO, PersonType>();

                cfg.CreateMap<DTOs.IdentificationDocumentDTO, IdentificationDocument>();
                cfg.CreateMap<DTOs.DocumentTypeDTO, DocumentType>();
                cfg.CreateMap<DTOs.CompanyDTO, Company>()
                .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name));
                cfg.CreateMap<TransactionDTO, Transaction>();
                cfg.CreateMap<PersonTypeDTO, PersonType>();
            });
            return config;
        }

        public static Person ToModel(this PersonDTO personDTO)
        {
            var config = CreateMapPersons();
            return config.Map<PersonDTO, Person>(personDTO);
        }

        public static IEnumerable<Person> ToModels(this IEnumerable<PersonDTO> personDTOs)
        {
            return personDTOs.Select(ToModel);
        }


        public static IMapper CreateMapBankAccountPersons()
        {
            var config = MapperCache.GetMapper<BankAccountPersonDTO, BankAccountPerson>(cfg =>
            {
                cfg.CreateMap<BankAccountPersonDTO, BankAccountPerson>()
                  .ForMember(dest => dest.Individual.FullName, opt => opt.MapFrom(src => src.Individual.Name));
                cfg.CreateMap<BankAccountTypeDTO, BankAccountType>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<BankAccountTypeDTO, BankAccountType>();
                cfg.CreateMap<IndividualDTO, Individual>();
                cfg.CreateMap<DocumentTypeDTO, DocumentType>();
                cfg.CreateMap<EconomicActivityDTO, UniquePersonService.V1.Models.EconomicActivity>();
                cfg.CreateMap<IdentificationDocumentDTO, IdentificationDocument>();
                cfg.CreateMap<BankDTO, Bank>();
            });
            return config;
        }

        public static BankAccountPerson ToModel(this BankAccountPersonDTO bankAccountPersonDTO)
        {
            var config = CreateMapBankAccountPersons();
            return config.Map<BankAccountPersonDTO, BankAccountPerson>(bankAccountPersonDTO);
        }

        public static IEnumerable<BankAccountPerson> ToModels(this IEnumerable<BankAccountPersonDTO> bankAccountPersonDTOs)
        {
            return bankAccountPersonDTOs.Select(ToModel);
        }

        public static IMapper CreateMapBankAccountTypes()
        {
            var config = MapperCache.GetMapper<BankAccountTypeDTO, BankAccountType>(cfg =>
            {
                cfg.CreateMap<BankAccountTypeDTO, BankAccountType>();
            });
            return config;
        }

        public static BankAccountType ToModel(this BankAccountTypeDTO bankAccountTypeDTO)
        {
            var config = CreateMapBankAccountTypes();
            return config.Map<BankAccountTypeDTO, BankAccountType>(bankAccountTypeDTO);
        }

        public static IEnumerable<BankAccountType> ToModels(this IEnumerable<BankAccountTypeDTO> bankAccountTypeDTOs)
        {
            return bankAccountTypeDTOs.Select(ToModel);
        }

        //public static IMapper CreateMapAutomaticDebits()
        //{
        //    var config = MapperCache.GetMapper<AutomaticDebitDTO, AutomaticDebit>(cfg =>
        //    {
        //        cfg.CreateMap<AutomaticDebitDTO, AutomaticDebit>();
        //        cfg.CreateMap<BankNetworkDTO, BankNetwork>();
        //        cfg.CreateMap<AutomaticDebitStatusDTO, AutomaticDebitStatus>();
        //        cfg.CreateMap<CouponDTO, Coupon>();
        //    });
        //    return config;
        //}

        //public static AutomaticDebit ToModel(this AutomaticDebitDTO automaticDebitDTO)
        //{
        //    var config = CreateMapAutomaticDebits();
        //    return config.Map<AutomaticDebitDTO, AutomaticDebit>(automaticDebitDTO);
        //}

        //public static IEnumerable<AutomaticDebit> ToModels(this IEnumerable<AutomaticDebitDTO> automaticDebitDTOs)
        //{
        //    return automaticDebitDTOs.Select(ToModel);
        //}

        //public static IMapper CreateMapAutomaticDebitStatuss()
        //{
        //    var config = MapperCache.GetMapper<AutomaticDebitStatusDTO, AutomaticDebitStatus>(cfg =>
        //    {
        //        cfg.CreateMap<AutomaticDebitStatusDTO, AutomaticDebitStatus>();
        //    });
        //    return config;
        //}

        //public static AutomaticDebitStatus ToModel(this AutomaticDebitStatusDTO automaticDebitStatusDTO)
        //{
        //    var config = CreateMapAutomaticDebitStatuss();
        //    return config.Map<AutomaticDebitStatusDTO, AutomaticDebitStatus>(automaticDebitStatusDTO);
        //}

        //public static IEnumerable<AutomaticDebitStatus> ToModels(this IEnumerable<AutomaticDebitStatusDTO> automaticDebitStatusDTOs)
        //{
        //    return automaticDebitStatusDTOs.Select(ToModel);
        //}

        //public static IMapper CreateMapBankNetworks()
        //{
        //    var config = MapperCache.GetMapper<BankNetworkDTO, BankNetwork>(cfg =>
        //    {
        //        cfg.CreateMap<BankNetworkDTO, BankNetwork>();
        //        cfg.CreateMap<TaxCategoryDTO, TaxCategory>();
        //        cfg.CreateMap<AmountDTO, Amount>();
        //    });
        //    return config;
        //}

        //public static BankNetwork ToModel(this BankNetworkDTO bankNetworkDTO)
        //{
        //    var config = CreateMapBankNetworks();
        //    return config.Map<BankNetworkDTO, BankNetwork>(bankNetworkDTO);
        //}

        //public static IEnumerable<BankNetwork> ToModels(this IEnumerable<BankNetworkDTO> bankNetworkDTOs)
        //{
        //    return bankNetworkDTOs.Select(ToModel);
        //}

        //public static IMapper CreateMapBankNetworkStatuss()
        //{
        //    var config = MapperCache.GetMapper<BankNetworkStatusDTO, BankNetworkStatus>(cfg =>
        //    {
        //        cfg.CreateMap<BankNetworkStatusDTO, BankNetworkStatus>();
        //        cfg.CreateMap<BankNetworkDTO, BankNetwork>();
        //        cfg.CreateMap<CouponStatusDTO, CouponStatus>();
        //    });
        //    return config;
        //}

        //public static BankNetworkStatus ToModel(this BankNetworkStatusDTO bankNetworkStatusDTO)
        //{
        //    var config = CreateMapBankNetworkStatuss();
        //    return config.Map<BankNetworkStatusDTO, BankNetworkStatus>(bankNetworkStatusDTO);
        //}

        //public static IEnumerable<BankNetworkStatus> ToModels(this IEnumerable<BankNetworkStatusDTO> bankNetworkStatusDTOs)
        //{
        //    return bankNetworkStatusDTOs.Select(ToModel);
        //}

        //public static IMapper CreateMapCoupons()
        //{
        //    var config = MapperCache.GetMapper<CouponDTO, Coupon>(cfg =>
        //    {
        //        cfg.CreateMap<CouponDTO, Coupon>();
        //        cfg.CreateMap<PolicyDTO, Policy>();
        //        cfg.CreateMap<CouponStatusDTO, CouponStatus>();
        //    });
        //    return config;
        //}

        //public static Coupon ToModel(this CouponDTO couponDTO)
        //{
        //    var config = CreateMapCoupons();
        //    return config.Map<CouponDTO, Coupon>(couponDTO);
        //}

        //public static IEnumerable<Coupon> ToModels(this IEnumerable<CouponDTO> couponDTOs)
        //{
        //    return couponDTOs.Select(ToModel);
        //}

        //public static IMapper CreateMapCouponStatuss()
        //{
        //    var config = MapperCache.GetMapper<CouponStatusDTO, CouponStatus>(cfg =>
        //    {
        //        cfg.CreateMap<CouponStatusDTO, CouponStatus>()
        //                .ForMember(dest => dest.CouponStatusType, opt => opt.MapFrom(src => (CouponStatusTypes)src.CouponStatusType));
        //    });
        //    return config;
        //}

        //public static CouponStatus ToModel(this CouponStatusDTO couponStatusDTO)
        //{
        //    var config = CreateMapCouponStatuss();
        //    return config.Map<CouponStatusDTO, CouponStatus>(couponStatusDTO);
        //}

        //public static IEnumerable<CouponStatus> ToModels(this IEnumerable<CouponStatusDTO> couponStatusDTOs)
        //{
        //    return couponStatusDTOs.Select(ToModel);
        //}

        //public static IMapper CreateMapFields()
        //{
        //    var config = MapperCache.GetMapper<FieldDTO, Field>(cfg =>
        //    {
        //        cfg.CreateMap<FieldDTO, Field>();
        //    });
        //    return config;
        //}

        //public static Field ToModel(this FieldDTO fieldDTO)
        //{
        //    var config = CreateMapFields();
        //    return config.Map<FieldDTO, Field>(fieldDTO);
        //}

        //public static IEnumerable<Field> ToModels(this IEnumerable<FieldDTO> fieldDTOs)
        //{
        //    return fieldDTOs.Select(ToModel);
        //}

        //public static IMapper CreateMapFormats()
        //{
        //    var config = MapperCache.GetMapper<FormatDTO, Format>(cfg =>
        //    {
        //        cfg.CreateMap<FormatDTO, Format>()
        //                .ForMember(dest => dest.FormatType, opt => opt.MapFrom(src => (FormatTypes)src.FormatType))
        //                .ForMember(dest => dest.FormatUsingType, opt => opt.MapFrom(src => (FormatUsingTypes)src.FormatUsingType));
        //        cfg.CreateMap<BankNetworkDTO, BankNetwork>();
        //        cfg.CreateMap<FieldDTO, Field>();
        //    });
        //    return config;
        //}

        //public static Format ToModel(this FormatDTO formatDTO)
        //{
        //    var config = CreateMapFormats();
        //    return config.Map<FormatDTO, Format>(formatDTO);
        //}

        //public static IEnumerable<Format> ToModels(this IEnumerable<FormatDTO> formatDTOs)
        //{
        //    return formatDTOs.Select(ToModel);
        //}

        //public static IMapper CreateMapPaymentMethodAccountTypes()
        //{
        //    var config = MapperCache.GetMapper<PaymentMethodAccountTypeDTO, PaymentMethodAccountType>(cfg =>
        //    {
        //        cfg.CreateMap<PaymentMethodAccountTypeDTO, PaymentMethodAccountType>();
        //        cfg.CreateMap<PaymentMethodDTO, PaymentsModels.PaymentMethod>();
        //        cfg.CreateMap<BankAccountTypeDTO, BankAccountType>();
        //    });
        //    return config;
        //}

        //public static PaymentMethodAccountType ToModel(this PaymentMethodAccountTypeDTO paymentMethodAccountTypeDTO)
        //{
        //    var config = CreateMapPaymentMethodAccountTypes();
        //    return config.Map<PaymentMethodAccountTypeDTO, PaymentMethodAccountType>(paymentMethodAccountTypeDTO);
        //}

        //public static IEnumerable<PaymentMethodAccountType> ToModels(this IEnumerable<PaymentMethodAccountTypeDTO> paymentMethodAccountTypeDTOs)
        //{
        //    return paymentMethodAccountTypeDTOs.Select(ToModel);
        //}

        //public static IMapper CreateMapPaymentMethodBankNetworks()
        //{
        //    var config = MapperCache.GetMapper<PaymentMethodBankNetworkDTO, PaymentMethodBankNetwork>(cfg =>
        //    {
        //        cfg.CreateMap<PaymentMethodBankNetworkDTO, PaymentMethodBankNetwork>();
        //        cfg.CreateMap<BankNetworkDTO, BankNetwork>();
        //        cfg.CreateMap<PaymentMethodDTO, PaymentsModels.PaymentMethod>();
        //        cfg.CreateMap<BankAccountCompanyDTO, BankAccountCompany>();
        //    });
        //    return config;
        //}

        //public static PaymentMethodBankNetwork ToModel(this PaymentMethodBankNetworkDTO paymentMethodBankNetworkDTO)
        //{
        //    var config = CreateMapPaymentMethodBankNetworks();
        //    return config.Map<PaymentMethodBankNetworkDTO, PaymentMethodBankNetwork>(paymentMethodBankNetworkDTO);
        //}

        //public static IEnumerable<PaymentMethodBankNetwork> ToModels(this IEnumerable<PaymentMethodBankNetworkDTO> paymentMethodBankNetworkDTOs)
        //{
        //    return paymentMethodBankNetworkDTOs.Select(ToModel);
        //}

        public static IMapper CreateMapAmortizations()
        {
            var config = MapperCache.GetMapper<AmortizationDTO, Amortization>(cfg =>
            {
                cfg.CreateMap<AmortizationDTO, Amortization>()
                        .ForMember(dest => dest.AmortizationStatus, opt => opt.MapFrom(src => (AmortizationStatus)src.AmortizationStatus));
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<PolicyDTO, Policy>();
                cfg.CreateMap<BeneficiaryDTO, Beneficiary>()
                .ForMember(dest => dest.BeneficiaryTypeDescription, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (CustomerType)src.CustomerType));
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<EndorsementDTO, Endorsement>();
                cfg.CreateMap<PayerComponentDTO, PayerComponent>();
                cfg.CreateMap<PaymentPlanDTO, PaymentPlan>();
                cfg.CreateMap<QuotaDTO, Quota>();
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<IssuanceAgencyDTO, IssuanceAgency>();
                cfg.CreateMap<IssuanceAgentDTO, IssuanceAgent>();
                cfg.CreateMap<IssuanceAgentTypeDTO, IssuanceAgentType>();
                cfg.CreateMap<IssuanceCommissionDTO, IssuanceCommission>();
                cfg.CreateMap<SubLineBusinessDTO, SubLineBusiness>();
                cfg.CreateMap<LineBusinessDTO, LineBusiness>();
                cfg.CreateMap<HolderDTO, Holder>()
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (CustomerType)src.CustomerType))
                .ForMember(dest => dest.IndividualType, opt => opt.MapFrom(src => (IndividualType)src.IndividualType));
                cfg.CreateMap<BillingGroupDTO, BillingGroup>();
                cfg.CreateMap<PolicyTypeDTO, PolicyType>();
                cfg.CreateMap<PrefixDTO, Prefix>();
            });
            return config;
        }

        public static Amortization ToModel(this AmortizationDTO amortizationDTO)
        {
            var config = CreateMapAmortizations();
            return config.Map<AmortizationDTO, Amortization>(amortizationDTO);
        }

        public static IEnumerable<Amortization> ToModels(this IEnumerable<AmortizationDTO> amortizationDTOs)
        {
            return amortizationDTOs.Select(ToModel);
        }

        public static IMapper CreateMapCheckBookControls()
        {
            var config = MapperCache.GetMapper<DTOs.AccountsPayables.CheckBookControlDTO, CheckBookControl>(cfg =>
            {
                cfg.CreateMap<DTOs.AccountsPayables.CheckBookControlDTO, CheckBookControl>();
                cfg.CreateMap<BankAccountCompanyDTO, BankAccountCompany>();
                cfg.CreateMap<BankAccountTypeDTO, BankAccountType>();
                cfg.CreateMap<BankDTO, Bank>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<AccountingAccountDTO, GeneralLedgerServices.EEProvider.Models.AccountingAccount>();
            });
            return config;
        }

        public static CheckBookControl ToModel(this DTOs.AccountsPayables.CheckBookControlDTO checkBookControlDTO)
        {
            var config = CreateMapCheckBookControls();
            return config.Map<DTOs.AccountsPayables.CheckBookControlDTO, CheckBookControl>(checkBookControlDTO);
        }

        public static IEnumerable<CheckBookControl> ToModels(this IEnumerable<DTOs.AccountsPayables.CheckBookControlDTO> checkBookControlDTOs)
        {
            return checkBookControlDTOs.Select(ToModel);
        }

        public static IMapper CreateMapCheckPaymentOrders()
        {
            var config = MapperCache.GetMapper<CheckPaymentOrderDTO, CheckPaymentOrder>(cfg =>
            {
                cfg.CreateMap<CheckPaymentOrderDTO, CheckPaymentOrder>();
                cfg.CreateMap<BankAccountCompanyDTO, BankAccountCompany>();
                cfg.CreateMap<Bank, Bank>();
                cfg.CreateMap<BankAccountTypeDTO, BankAccountType>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<AccountingAccountDTO, GeneralLedgerServices.EEProvider.Models.AccountingAccount>();
                cfg.CreateMap<PersonTypeDTO, PersonType>();
                cfg.CreateMap<IndividualDTO, Individual>();
                cfg.CreateMap<DocumentTypeDTO, DocumentType>();
                cfg.CreateMap<EconomicActivityDTO, UniquePersonService.V1.Models.EconomicActivity>();
                cfg.CreateMap<IdentificationDocumentDTO, IdentificationDocument>();
                cfg.CreateMap<DTOs.AccountsPayables.PaymentOrderDTO, PaymentOrder>();
            });
            return config;
        }

        public static CheckPaymentOrder ToModel(this CheckPaymentOrderDTO checkPaymentOrderDTO)
        {
            var config = CreateMapCheckPaymentOrders();
            return config.Map<CheckPaymentOrderDTO, CheckPaymentOrder>(checkPaymentOrderDTO);
        }

        public static IEnumerable<CheckPaymentOrder> ToModels(this IEnumerable<CheckPaymentOrderDTO> checkPaymentOrderDTOs)
        {
            return checkPaymentOrderDTOs.Select(ToModel);
        }

        public static IMapper CreateMapPaymentOrders()
        {
            var config = MapperCache.GetMapper<DTOs.AccountsPayables.PaymentOrderDTO, PaymentOrder>(cfg =>
            {
                cfg.CreateMap<DTOs.AccountsPayables.PaymentOrderDTO, PaymentOrder>();
                cfg.CreateMap<PersonTypeDTO, PersonType>();
                cfg.CreateMap<IndividualDTO, Individual>();
                cfg.CreateMap<DocumentTypeDTO, DocumentType>();
                cfg.CreateMap<EconomicActivityDTO, UniquePersonService.V1.Models.EconomicActivity>();
                cfg.CreateMap<IdentificationDocumentDTO, IdentificationDocument>();
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<ExchangeRateDTO, ExchangeRate>();
                cfg.CreateMap<DTOs.CompanyDTO, Company>()
                .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name));
                cfg.CreateMap<ConceptSourceDTO, EEProvider.Models.Claims.PaymentRequest.ConceptSource>();
                cfg.CreateMap<PaymentMethodDTO, PaymentsModels.PaymentMethod>();
                cfg.CreateMap<BankAccountPersonDTO, BankAccountPerson>()
                        .ForMember(dest => dest.Individual, opt => opt.MapFrom(src => new Individual { FullName =  (src.Individual != null) ? src.Individual.Name : "" }));
                cfg.CreateMap<BankAccountTypeDTO, BankAccountType>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<ApplicationDTO, EEProvider.Models.Imputations.Application> ()
                        //.ForMember(dest => dest.ImputationType, opt => opt.MapFrom(src => (ImputationTypes)src.ImputationType))
                        ;
                cfg.CreateMap<TransactionTypeDTO, TransactionType>();
                cfg.CreateMap<TransactionDTO, Transaction>();
            });
            return config;
        }

        public static PaymentOrder ToModel(this DTOs.AccountsPayables.PaymentOrderDTO paymentOrderDTO)
        {
            var config = CreateMapPaymentOrders();
            return config.Map<DTOs.AccountsPayables.PaymentOrderDTO, PaymentOrder>(paymentOrderDTO);
        }

        public static IEnumerable<PaymentOrder> ToModels(this IEnumerable<DTOs.AccountsPayables.PaymentOrderDTO> paymentOrderDTOs)
        {
            return paymentOrderDTOs.Select(ToModel);
        }


        public static IMapper CreateMapPersonTypes()
        {
            var config = MapperCache.GetMapper<PersonTypeDTO, PersonType>(cfg =>
            {
                cfg.CreateMap<PersonTypeDTO, PersonType>();
                cfg.CreateMap<IndividualDTO, Individual>();
                cfg.CreateMap<DocumentTypeDTO, DocumentType>();
                cfg.CreateMap<EconomicActivityDTO, UniquePersonService.V1.Models.EconomicActivity>();
                cfg.CreateMap<IdentificationDocumentDTO, IdentificationDocument>();
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<ExchangeRateDTO, ExchangeRate>();
            });
            return config;
        }

        public static PersonType ToModel(this PersonTypeDTO personTypeDTO)
        {
            var config = CreateMapPersonTypes();
            return config.Map<PersonTypeDTO, PersonType>(personTypeDTO);
        }

        public static IEnumerable<PersonType> ToModels(this IEnumerable<PersonTypeDTO> personTypeDTOs)
        {
            return personTypeDTOs.Select(ToModel);
        }


        public static IMapper CreateMapPaymentRequests()
        {
            var config = MapperCache.GetMapper<PaymentRequestDTO, PaymentRequest>(cfg =>
            {
                cfg.CreateMap<PaymentRequestDTO, PaymentRequest>()
                .ForMember(dest => dest.PaymentRequestType, opt => opt.MapFrom(src => (PaymentRequestTypes)src.PaymentRequestType));
                cfg.CreateMap<PaymentRequestNumberDTO, PaymentRequestNumber>();
                cfg.CreateMap<MovementTypeDTO, EEProvider.Models.Claims.PaymentRequest.MovementType>();
                cfg.CreateMap<DTOs.CompanyDTO, Company>()
                .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name));
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<PersonTypeDTO, PersonType>();
                cfg.CreateMap<IndividualDTO, Individual>();
                cfg.CreateMap<DocumentTypeDTO, DocumentType>();
                cfg.CreateMap<EconomicActivityDTO, UniquePersonService.V1.Models.EconomicActivity>();
                cfg.CreateMap<IdentificationDocumentDTO, IdentificationDocument>();
                cfg.CreateMap<PaymentMethodDTO, PaymentsModels.PaymentMethod>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<ExchangeRateDTO, ExchangeRate>();
                cfg.CreateMap<VoucherDTO, Voucher>();
                cfg.CreateMap<TransactionDTO, Transaction>();
                cfg.CreateMap<TransactionTypeDTO, TransactionType>();
                cfg.CreateMap<ClaimDTO, CLAIMODL.Claim>()
                .ForMember(dest => dest.CoveredRiskType, opt => opt.MapFrom(src => (int)src.CoveredRiskType));
                cfg.CreateMap<TaxDTO, Tax>();
                cfg.CreateMap<TaxConditionDTO, TaxCondition>();
                cfg.CreateMap<TaxCategoryDTO, TaxCategory>();
                cfg.CreateMap<AccountingAccountDTO, GeneralLedgerServices.EEProvider.Models.AccountingAccount>();
                cfg.CreateMap<CostCenterDTO, EEProvider.Models.Imputations.CostCenter>();
                cfg.CreateMap<VoucherTypeDTO, VoucherType>();
                cfg.CreateMap<VoucherConceptDTO, VoucherConcept>();
                cfg.CreateMap<ConceptSourceDTO, PRCLAIM.ConceptSource>();
                cfg.CreateMap<AccountingConceptDTO, UniquePersonService.V1.Models.AccountingConcept>();

            });
            return config;
        }

        public static PaymentRequest ToModel(this PaymentRequestDTO paymentRequestDTO)
        {
            var config = CreateMapPaymentRequests();
            return config.Map<PaymentRequestDTO, PaymentRequest>(paymentRequestDTO);
        }

        public static IEnumerable<PaymentRequest> ToModels(this IEnumerable<PaymentRequestDTO> paymentRequestDTOs)
        {
            return paymentRequestDTOs.Select(ToModel);
        }

        public static IMapper CreateMapPaymentRequestNumbers()
        {
            var config = MapperCache.GetMapper<PaymentRequestNumberDTO, PaymentRequestNumber>(cfg =>
            {
                cfg.CreateMap<PaymentRequestNumberDTO, PaymentRequestNumber>();
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
            });
            return config;
        }

        public static PaymentRequestNumber ToModel(this PaymentRequestNumberDTO paymentRequestNumberDTO)
        {
            var config = CreateMapPaymentRequestNumbers();
            return config.Map<PaymentRequestNumberDTO, PaymentRequestNumber>(paymentRequestNumberDTO);
        }

        public static IEnumerable<PaymentRequestNumber> ToModels(this IEnumerable<PaymentRequestNumberDTO> paymentRequestNumberDTOs)
        {
            return paymentRequestNumberDTOs.Select(ToModel);
        }

        public static IMapper CreateMapTransferPaymentOrders()
        {
            var config = MapperCache.GetMapper<TransferPaymentOrderDTO, TransferPaymentOrder>(cfg =>
            {
                cfg.CreateMap<TransferPaymentOrderDTO, TransferPaymentOrder>();
                cfg.CreateMap<BankAccountCompanyDTO, BankAccountCompany>();
                cfg.CreateMap<PersonTypeDTO, PersonType>();

                cfg.CreateMap<ExchangeRateDTO, ExchangeRate>();
                cfg.CreateMap<CompanyDTO, Company>()
                .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name));
                cfg.CreateMap<ConceptSourceDTO, EEProvider.Models.Claims.PaymentRequest.ConceptSource>();
                cfg.CreateMap<PaymentMethodDTO, PaymentsModels.PaymentMethod>();

                cfg.CreateMap<BankAccountPersonDTO, BankAccountPerson>().ForMember(dest => dest.Individual.FullName, opt => opt.MapFrom(src => src.Individual.Name));
                cfg.CreateMap<BankAccountTypeDTO, BankAccountType>();
                cfg.CreateMap<BankDTO, Bank>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<IndividualDTO, Individual>();
                cfg.CreateMap<DocumentTypeDTO, DocumentType>();
                cfg.CreateMap<EconomicActivityDTO, UniquePersonService.V1.Models.EconomicActivity>();
                cfg.CreateMap<IdentificationDocumentDTO, IdentificationDocument>();

                cfg.CreateMap<ApplicationDTO, EEProvider.Models.Imputations.Application> ();
                cfg.CreateMap<TransactionTypeDTO, TransactionType>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<TransactionDTO, Transaction>();



                cfg.CreateMap<BankAccountTypeDTO, BankAccountType>();
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<AccountingAccountDTO, GeneralLedgerServices.EEProvider.Models.AccountingAccount>();
                cfg.CreateMap<DTOs.AccountsPayables.PaymentOrderDTO, PaymentOrder>();
            });
            return config;
        }

        public static TransferPaymentOrder ToModel(this TransferPaymentOrderDTO transferPaymentOrderDTO)
        {
            var config = CreateMapTransferPaymentOrders();
            return config.Map<TransferPaymentOrderDTO, TransferPaymentOrder>(transferPaymentOrderDTO);
        }

        public static IEnumerable<TransferPaymentOrder> ToModels(this IEnumerable<TransferPaymentOrderDTO> transferPaymentOrderDTOs)
        {
            return transferPaymentOrderDTOs.Select(ToModel);
        }

        public static IMapper CreateMapVouchers()
        {
            var config = MapperCache.GetMapper<VoucherDTO, Voucher>(cfg =>
            {
                cfg.CreateMap<VoucherDTO, Voucher>();
                cfg.CreateMap<VoucherTypeDTO, VoucherType>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<VoucherConceptDTO, VoucherConcept>();
                cfg.CreateMap<AccountingConceptDTO, UniquePersonService.V1.Models.AccountingConcept>();
                cfg.CreateMap<AccountingAccountDTO, GeneralLedgerServices.EEProvider.Models.AccountingAccount>();
                cfg.CreateMap<CostCenterDTO, EEProvider.Models.Imputations.CostCenter>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<VoucherConceptTaxDTO, VoucherConceptTax>();
                cfg.CreateMap<TaxConditionDTO, TaxCondition>();
                cfg.CreateMap<TaxCategoryDTO, TaxCategory>();
            });
            return config;
        }

        public static Voucher ToModel(this VoucherDTO voucherDTO)
        {
            var config = CreateMapVouchers();
            return config.Map<VoucherDTO, Voucher>(voucherDTO);
        }

        public static IEnumerable<Voucher> ToModels(this IEnumerable<VoucherDTO> voucherDTOs)
        {
            return voucherDTOs.Select(ToModel);
        }

        public static IMapper CreateMapVoucherConcepts()
        {
            var config = MapperCache.GetMapper<VoucherConceptDTO, VoucherConcept>(cfg =>
            {
                cfg.CreateMap<VoucherConceptDTO, VoucherConcept>();
                cfg.CreateMap<AccountingConceptDTO, UniquePersonService.V1.Models.AccountingConcept>();
                cfg.CreateMap<AccountingAccountDTO, GeneralLedgerServices.EEProvider.Models.AccountingAccount>();
                cfg.CreateMap<CostCenterDTO, EEProvider.Models.Imputations.CostCenter>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<VoucherConceptTaxDTO, VoucherConceptTax>();
                cfg.CreateMap<TaxConditionDTO, TaxCondition>();
                cfg.CreateMap<TaxCategoryDTO, TaxCategory>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<VoucherConceptTaxDTO, VoucherConceptTax>();
            });
            return config;
        }

        public static VoucherConcept ToModel(this VoucherConceptDTO voucherConceptDTO)
        {
            var config = CreateMapVoucherConcepts();
            return config.Map<VoucherConceptDTO, VoucherConcept>(voucherConceptDTO);
        }

        public static IEnumerable<VoucherConcept> ToModels(this IEnumerable<VoucherConceptDTO> voucherConceptDTOs)
        {
            return voucherConceptDTOs.Select(ToModel);
        }

        public static IMapper CreateMapVoucherConceptTaxs()
        {
            var config = MapperCache.GetMapper<VoucherConceptTaxDTO, VoucherConceptTax>(cfg =>
            {
                cfg.CreateMap<VoucherConceptTaxDTO, VoucherConceptTax>();
                cfg.CreateMap<TaxDTO, Tax>();
                cfg.CreateMap<TaxConditionDTO, TaxCondition>();
                cfg.CreateMap<TaxCategoryDTO, TaxCategory>();
            });
            return config;
        }

        public static VoucherConceptTax ToModel(this VoucherConceptTaxDTO voucherConceptTaxDTO)
        {
            var config = CreateMapVoucherConceptTaxs();
            return config.Map<VoucherConceptTaxDTO, VoucherConceptTax>(voucherConceptTaxDTO);
        }

        public static IEnumerable<VoucherConceptTax> ToModels(this IEnumerable<VoucherConceptTaxDTO> voucherConceptTaxDTOs)
        {
            return voucherConceptTaxDTOs.Select(ToModel);
        }

        public static IMapper CreateMapVoucherTypes()
        {
            var config = MapperCache.GetMapper<VoucherTypeDTO, VoucherType>(cfg =>
            {
                cfg.CreateMap<VoucherTypeDTO, VoucherType>();
            });
            return config;
        }

        public static VoucherType ToModel(this VoucherTypeDTO voucherTypeDTO)
        {
            var config = CreateMapVoucherTypes();
            return config.Map<VoucherTypeDTO, VoucherType>(voucherTypeDTO);
        }

        public static IEnumerable<VoucherType> ToModels(this IEnumerable<VoucherTypeDTO> voucherTypeDTOs)
        {
            return voucherTypeDTOs.Select(ToModel);
        }

        //--
        public static IMapper CreateMapBranch()
        {
            var config = MapperCache.GetMapper<BranchDTO, Branch>(cfg =>
            {
                cfg.CreateMap<BranchDTO, Branch>();
            });
            return config;
        }

        public static Branch ToModel(this BranchDTO branch)
        {
            var config = CreateMapBranch();
            return config.Map<BranchDTO, Branch>(branch);
        }

        public static IEnumerable<Branch> ToModels(this IEnumerable<BranchDTO> branchs)
        {
            return branchs.Select(ToModel);
        }

        public static IMapper CreateMapPrefix()
        {
            var config = MapperCache.GetMapper<PrefixDTO, Prefix>(cfg =>
            {
                cfg.CreateMap<PrefixDTO, Prefix>();
            });
            return config;
        }

        public static Prefix ToModel(this PrefixDTO prefix)
        {
            var config = CreateMapPrefix();
            return config.Map<PrefixDTO, Prefix>(prefix);
        }

        public static IEnumerable<Prefix> ToDTOs(this IEnumerable<PrefixDTO> prefixs)
        {
            return prefixs.Select(ToModel);
        }

        public static IMapper CreateMapPolicy()
        {
            var config = MapperCache.GetMapper<PolicyDTO, Policy>(cfg =>
            {
                cfg.CreateMap<PolicyDTO, Policy>();
                cfg.CreateMap<BeneficiaryDTO, Beneficiary>()
                .ForMember(dest => dest.BeneficiaryTypeDescription, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (CustomerType)src.CustomerType));
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<EndorsementDTO, Endorsement>();
                cfg.CreateMap<PayerComponentDTO, PayerComponent>();
                cfg.CreateMap<PaymentPlanDTO, PaymentPlan>();
                cfg.CreateMap<QuotaDTO, Quota>();
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<IssuanceAgencyDTO, IssuanceAgency>();
                cfg.CreateMap<IssuanceAgentDTO, IssuanceAgent>();
                cfg.CreateMap<IssuanceAgentTypeDTO, IssuanceAgentType>();
                cfg.CreateMap<IssuanceCommissionDTO, IssuanceCommission>();
                cfg.CreateMap<SubLineBusinessDTO, SubLineBusiness>();
                cfg.CreateMap<LineBusinessDTO, LineBusiness>();
                cfg.CreateMap<HolderDTO, Holder>()
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (CustomerType)src.CustomerType))
                .ForMember(dest => dest.IndividualType, opt => opt.MapFrom(src => (IndividualType)src.IndividualType));
                cfg.CreateMap<BillingGroupDTO, BillingGroup>();
                cfg.CreateMap<PolicyTypeDTO, PolicyType>();
                cfg.CreateMap<PrefixDTO, Prefix>();
            });
            return config;
        }

        public static Policy ToModel(this PolicyDTO policy)
        {
            var config = CreateMapPolicy();
            return config.Map<PolicyDTO, Policy>(policy);
        }

        public static IEnumerable<Policy> ToModels(this IEnumerable<PolicyDTO> policies)
        {
            return policies.Select(ToModel);
        }

        public static IMapper CreateMapIndividual()
        {
            var config = MapperCache.GetMapper<IndividualDTO, Individual>(cfg =>
            {
                cfg.CreateMap<IndividualDTO, Individual>();
                cfg.CreateMap<DocumentTypeDTO, DocumentType>();
                cfg.CreateMap<EconomicActivityDTO, UniquePersonService.V1.Models.EconomicActivity>();
                cfg.CreateMap<IdentificationDocumentDTO, IdentificationDocument>();
            });
            return config;
        }

        public static Individual ToModel(this IndividualDTO individual)
        {
            var config = CreateMapIndividual();
            return config.Map<IndividualDTO, Individual>(individual);
        }

        public static IEnumerable<Individual> ToDTOs(this IEnumerable<IndividualDTO> policies)
        {
            return policies.Select(ToModel);
        }

        public static IMapper CreateConceptSource()
        {
            var config = MapperCache.GetMapper<ConceptSourceDTO, EEProvider.Models.Claims.PaymentRequest.ConceptSource>(cfg =>
            {
                cfg.CreateMap<ConceptSourceDTO, EEProvider.Models.Claims.PaymentRequest.ConceptSource>();
            });
            return config;
        }

        public static EEProvider.Models.Claims.PaymentRequest.ConceptSource ToModel(this ConceptSourceDTO conceptSourceDTO)
        {
            var config = CreateConceptSource();
            return config.Map<ConceptSourceDTO, EEProvider.Models.Claims.PaymentRequest.ConceptSource>(conceptSourceDTO);
        }

        public static IEnumerable<EEProvider.Models.Claims.PaymentRequest.ConceptSource> ToModels(this IEnumerable<ConceptSourceDTO> conceptSourceDTOs)
        {
            return conceptSourceDTOs.Select(ToModel);
        }

        public static IMapper CreateMapPersonTypeDTO()
        {
            var config = MapperCache.GetMapper<PersonTypeDTO, PersonType>(cfg =>
            {
                cfg.CreateMap<PersonTypeDTO, PersonType>();
            });
            return config;
        }

        public static PersonType ToDTO(this PersonTypeDTO personType)
        {
            var config = CreateMapPersonTypeDTO();
            return config.Map<PersonTypeDTO, PersonType>(personType);
        }

        public static IEnumerable<PersonType> ToDTOs(this IEnumerable<PersonTypeDTO> conceptSources)
        {
            return conceptSources.Select(ToDTO);
        }




        //public static IMapper CreateMapTransferPaymentOrder()
        //{
        //    var config = MapperCache.GetMapper<TransferPaymentOrderDTO, TransferPaymentOrder>(cfg =>
        //    {
        //        cfg.CreateMap<TransferPaymentOrderDTO, TransferPaymentOrder>();
        //    });
        //    return config;
        //}

        //public static TransferPaymentOrder ToModel(this TransferPaymentOrderDTO transferPaymentOrder)
        //{
        //    var config = CreateMapTransferPaymentOrder();
        //    return config.Map<TransferPaymentOrderDTO, TransferPaymentOrder>(transferPaymentOrder);
        //}

        //public static IEnumerable<TransferPaymentOrder> ToModels(this IEnumerable<TransferPaymentOrderDTO> transferPaymentOrder)
        //{
        //    return transferPaymentOrder.Select(ToModel);
        //}

        private static IMapper CreateApplication()
        {
            var config = MapperCache.GetMapper<ApplicationDTO, EEProvider.Models.Imputations.Application>(cfg =>
            {
                cfg.CreateMap<ApplicationDTO, EEProvider.Models.Imputations.Application>();
                cfg.CreateMap<ApplicationPremiumDTO, EEProvider.Models.Imputations.ApplicationPremium>();
                cfg.CreateMap<TransactionTypeDTO, TransactionType>();
                cfg.CreateMap<AmountDTO, Amount>();
            });
            return config;
        }

        public static EEProvider.Models.Imputations.Application ToModel(this ApplicationDTO applicationDTO)
        {
            var config = CreateApplication();
            return config.Map<ApplicationDTO, EEProvider.Models.Imputations.Application>(applicationDTO);
        }

        public static IEnumerable<EEProvider.Models.Imputations.Application> ToModels(this IEnumerable<ApplicationDTO> applicationDTOs)
        {
            return applicationDTOs.Select(ToModel);
        }

        private static IMapper CreateTempApplicationPremium()
        {
            var config = MapperCache.GetMapper<TempApplicationPremiumDTO, TempApplicationPremium>(cfg =>
            {
                cfg.CreateMap<ApplicationPremiumCommisionDTO, ApplicationPremiumCommision>();
                cfg.CreateMap<TempApplicationPremiumCommissDTO, ApplicationPremiumCommision>()
                .ForMember(dest => dest.CurrencyId, opt => opt.MapFrom(src => src.CurrencyCode))
                .ForMember(dest => dest.ApplicationPremiumId, opt => opt.MapFrom(src => src.TempApplicationPremiumId))
                .ForMember(dest => dest.AgentIndividualId, opt => opt.MapFrom(src => src.AgentId))
                .ForMember(dest => dest.CommissionDiscountIncomeAmount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.CommissionDiscountAmount, opt => opt.MapFrom(src => src.BaseAmount))
                .ForMember(dest => dest.DiscountedCommissionId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.BaseIncomeAmount, opt => opt.MapFrom(src => src.BaseAmount));
                cfg.CreateMap<TempApplicationPremiumDTO, TempApplicationPremium>();
                cfg.CreateMap<TempApplicationPremiumComponentDTO, TempApplicationPremiumComponent>();                
                cfg.CreateMap<AmountDTO, Amount>();
            });
            return config;
        }

        public static TempApplicationPremium ToModel(this TempApplicationPremiumDTO tempApplicationPremiumDTO)
        {
            var config = CreateTempApplicationPremium();
            return config.Map<TempApplicationPremiumDTO, TempApplicationPremium>(tempApplicationPremiumDTO);
        }

        public static IEnumerable<TempApplicationPremium> ToModels(this IEnumerable<TempApplicationPremiumDTO> applicationDTOs)
        {
            return applicationDTOs.Select(ToModel);
        }

        private static IMapper CreateTempApplication()
        {
            var config = MapperCache.GetMapper<TempApplicationDTO, TempApplication>(cfg =>
            {
                cfg.CreateMap<TempApplicationDTO, TempApplication>();
                cfg.CreateMap<TempApplicationPremiumDTO, TempApplicationPremium>();
                cfg.CreateMap<TempApplicationPremiumCommissDTO, TempApplicationPremiumCommiss>();
                cfg.CreateMap<AmountDTO, Amount>();
            });
            return config;
        }

        public static TempApplication ToModel(this TempApplicationDTO applicationDTO)
        {
            var config = CreateTempApplication();
            return config.Map<TempApplicationDTO, TempApplication>(applicationDTO);
        }

        public static IEnumerable<TempApplication> ToModels(this IEnumerable<TempApplicationDTO> applicationDTOs)
        {
            return applicationDTOs.Select(ToModel);
        }

        private static IMapper CreateCollectApplication()
        {
            var config = MapperCache.GetMapper<CollectApplicationDTO, CollectApplication>(cfg =>
            {
                cfg.CreateMap<CollectApplicationDTO, CollectApplication>();
                cfg.CreateMap<TransactionDTO, Transaction>();
                cfg.CreateMap<CollectApplicationDTO, CollectApplication>();
                cfg.CreateMap<CollectApplicationDTO, CollectApplication>();
                cfg.CreateMap<CollectApplicationDTO, CollectApplication>();
                cfg.CreateMap<CollectDTO, Collect>()
                        .ForMember(dest => dest.CollectType, opt => opt.MapFrom(src => (CollectTypes)src.CollectType))
                        .ForMember(dest => dest.AccountingCompany, opt => opt.MapFrom(src => new CompanyDTO { IndividualId = src.CompanyIndividualId, Name = src.AccountingCompany.Name }));
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<CollectConceptDTO, CollectConcept>();
                cfg.CreateMap<PaymentDTO, PaymentsModels.Payment>()
                    .Include<CashDTO, Cash>()
                    .Include<CheckDTO, Check>()
                    .Include<CreditCardDTO, CreditCard>()
                    .Include<TransferDTO, Transfer>()
                    .Include<DepositVoucherDTO, DepositVoucher>()
                    .Include<RetentionReceiptDTO, RetentionReceipt>();
                cfg.CreateMap<CashDTO, Cash>();
                cfg.CreateMap<CheckDTO, Check>();
                cfg.CreateMap<BankDTO, Bank>();
                cfg.CreateMap<CreditCardDTO, CreditCard>();
                cfg.CreateMap<CreditCardTypeDTO, CreditCardType>();
                cfg.CreateMap<CreditCardValidThruDTO, CreditCardValidThru>();
                cfg.CreateMap<TransferDTO, Transfer>();
                cfg.CreateMap<BankAccountPersonDTO, BankAccountPerson>();
                cfg.CreateMap<BankAccountTypeDTO, BankAccountType>();
                cfg.CreateMap<IndividualDTO, Individual>();
                cfg.CreateMap<DocumentTypeDTO, DocumentType>();
                cfg.CreateMap<EconomicActivityDTO, UniquePersonService.V1.Models.EconomicActivity>();
                cfg.CreateMap<IdentificationDocumentDTO, IdentificationDocument>();
                cfg.CreateMap<DepositVoucherDTO, DepositVoucher>();
                cfg.CreateMap<RetentionReceiptDTO, RetentionReceipt>();
                cfg.CreateMap<PolicyDTO, Policy>();
                cfg.CreateMap<BeneficiaryDTO, Beneficiary>()
                .ForMember(dest => dest.BeneficiaryTypeDescription, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (CustomerType)src.CustomerType));
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<EndorsementDTO, Endorsement>();
                cfg.CreateMap<PayerComponentDTO, PayerComponent>();
                cfg.CreateMap<PaymentPlanDTO, PaymentPlan>();
                cfg.CreateMap<QuotaDTO, Quota>();
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<IssuanceAgencyDTO, IssuanceAgency>();
                cfg.CreateMap<IssuanceAgentDTO, IssuanceAgent>();
                cfg.CreateMap<IssuanceAgentTypeDTO, IssuanceAgentType>();
                cfg.CreateMap<IssuanceCommissionDTO, IssuanceCommission>();
                cfg.CreateMap<SubLineBusinessDTO, SubLineBusiness>();
                cfg.CreateMap<LineBusinessDTO, LineBusiness>();
                cfg.CreateMap<HolderDTO, Holder>()
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (CustomerType)src.CustomerType))
                .ForMember(dest => dest.IndividualType, opt => opt.MapFrom(src => (IndividualType)src.IndividualType));
                cfg.CreateMap<BillingGroupDTO, BillingGroup>();
                cfg.CreateMap<PolicyTypeDTO, PolicyType>();
                cfg.CreateMap<PrefixDTO, Prefix>();
                cfg.CreateMap<RetentionBaseDTO, RetentionBase>();
                cfg.CreateMap<RetentionConceptDTO, RetentionConcept>();
                cfg.CreateMap<PaymentMethodDTO, PaymentsModels.PaymentMethod>();
                cfg.CreateMap<ExchangeRateDTO, ExchangeRate>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<PaymentTaxDTO, PaymentTax>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<TaxDTO, TxServ.Tax>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<PersonDTO, Person>()
                .ForMember(dest => dest.IndividualType, opt => opt.MapFrom(src => (IndividualType)src.IndividualType))
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => (IndividualType)src.CustomerType));
                cfg.CreateMap<IdentificationDocumentDTO, IdentificationDocument>();
                cfg.CreateMap<DocumentTypeDTO, DocumentType>();
                cfg.CreateMap<CompanyDTO, Company>()
                .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name));
                cfg.CreateMap<TransactionDTO, Transaction>();
                cfg.CreateMap<PersonTypeDTO, PersonType>();
                cfg.CreateMap<ApplicationDTO, EEProvider.Models.Imputations.Application>();
                cfg.CreateMap<ApplicationPremiumDTO, EEProvider.Models.Imputations.ApplicationPremium>();
            });
            return config;
        }

        public static CollectApplication ToModel(this CollectApplicationDTO applicationDTO)
        {
            var config = CreateCollectApplication();
            return config.Map<CollectApplicationDTO, CollectApplication>(applicationDTO);
        }

        public static IEnumerable<CollectApplication> ToModels(this IEnumerable<CollectApplicationDTO> applicationDTOs)
        {
            return applicationDTOs.Select(ToModel);
        }

        private static IMapper CreateApplicationAccountingTransaction()
        {
            var config = MapperCache.GetMapper<ApplicationAccountingTransactionDTO, ApplicationAccountingTransaction>(cfg =>
            {
                cfg.CreateMap<ApplicationAccountingTransactionDTO, ApplicationAccountingTransaction>();
                cfg.CreateMap<ApplicationAccountingDTO, ApplicationAccounting>()
                        .ForMember(dest => dest.AccountingNature, opt => opt.MapFrom(src => (AccountingNature)src.AccountingNature))
                        ;
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<IndividualDTO, Individual>();
                cfg.CreateMap<DocumentTypeDTO, DocumentType>();
                cfg.CreateMap<EconomicActivityDTO, UniquePersonService.V1.Models.EconomicActivity>();
                cfg.CreateMap<IdentificationDocumentDTO, IdentificationDocument>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<ExchangeRateDTO, ExchangeRate>();
                cfg.CreateMap<BookAccountDTO, BookAccount>();
                cfg.CreateMap<ApplicationAccountingAnalysisDTO, ApplicationAccountingAnalysis>();
                cfg.CreateMap<ApplicationAccountingCostCenterDTO, ApplicationAccountingCostCenter>();
                cfg.CreateMap<TransactionTypeDTO, TransactionType>();
                cfg.CreateMap<ApplicationItemDTO, ApplicationItem>();                
                cfg.CreateMap<DTOs.Imputations.ApplicationAccountingConceptDTO, EEProvider.Models.Imputations.AccountingConcept>();
                cfg.CreateMap<CostCenterDTO, EEProvider.Models.Imputations.CostCenter>();
                cfg.CreateMap<AnalysisConceptDTO, EEProvider.Models.Imputations.AnalysisConcept>();
                cfg.CreateMap<AnalysisCodeDTO, EEProvider.Models.Imputations.AnalysisCode>();
            });
            return config;
        }

        public static ApplicationAccountingTransaction ToModel(this ApplicationAccountingTransactionDTO applicationDTO)
        {
            var config = CreateApplicationAccountingTransaction();
            return config.Map<ApplicationAccountingTransactionDTO, ApplicationAccountingTransaction>(applicationDTO);
        }

        public static IEnumerable<ApplicationAccountingTransaction> ToModels(this IEnumerable<ApplicationAccountingTransactionDTO> applicationDTOs)
        {
            return applicationDTOs.Select(ToModel);
        }

        public static IMapper CreateMapTempApplicationPremiumComponent()
        {
            var config = MapperCache.GetMapper<TempApplicationPremiumComponentDTO, TempApplicationPremiumComponent>(cfg =>
            {
                cfg.CreateMap<TempApplicationPremiumComponentDTO, TempApplicationPremiumComponent>();
            });
            return config;
        }
        public static TempApplicationPremiumComponent ToModel(this TempApplicationPremiumComponentDTO tempApplicationPremiumComponent)
        {
            var config = CreateMapTempApplicationPremiumComponent();
            return config.Map<TempApplicationPremiumComponentDTO, TempApplicationPremiumComponent>(tempApplicationPremiumComponent);
        }

        public static List<TempApplicationPremiumComponent> ToModels(this List<TempApplicationPremiumComponentDTO> tempApplicationPremiumComponents)
        {
            return tempApplicationPremiumComponents.Select(m => m.ToModel()).ToList();
        }
        #region TempApplicationPremiumCommission
        public static IMapper CreateTempApplicationPremiumCommission()
        {
            var config = MapperCache.GetMapper<DTOs.Imputations.TempApplicationPremiumCommissDTO, TempApplicationPremiumCommiss>(cfg =>
            {
                cfg.CreateMap<DTOs.Imputations.TempApplicationPremiumCommissDTO, TempApplicationPremiumCommiss>();
            });
            return config;
        }

        public static TempApplicationPremiumCommiss ToModel(this DTOs.Imputations.TempApplicationPremiumCommissDTO tempApplicationPremiumCommissDTO)
        {
            var config = CreateTempApplicationPremiumCommission();
            return config.Map<DTOs.Imputations.TempApplicationPremiumCommissDTO, TempApplicationPremiumCommiss>(tempApplicationPremiumCommissDTO);
        }

        public static IEnumerable<TempApplicationPremiumCommiss> ToModels(this IEnumerable<DTOs.Imputations.TempApplicationPremiumCommissDTO> tempApplicationPremiumCommissDTOs)
        {
            return tempApplicationPremiumCommissDTOs.Select(ToModel);
        }
        #endregion

        #region ApplicationPremiumCommission
        public static IMapper CreateApplicationPremiumCommission()
        {
            var config = MapperCache.GetMapper<DTOs.Imputations.ApplicationPremiumCommisionDTO, ApplicationPremiumCommision>(cfg =>
            {
                cfg.CreateMap<DTOs.Imputations.ApplicationPremiumCommisionDTO, ApplicationPremiumCommision>()
                .ForMember(dest => dest.AgentIndividualId, opt => opt.MapFrom(src => src.AgentId));
            });
            return config;
        }

        public static ApplicationPremiumCommision ToModel(this DTOs.Imputations.ApplicationPremiumCommisionDTO applicationPremiumCommisionDTO)
        {
            var config = CreateApplicationPremiumCommission();
            return config.Map<DTOs.Imputations.ApplicationPremiumCommisionDTO, ApplicationPremiumCommision>(applicationPremiumCommisionDTO);
        }

        public static IEnumerable<ApplicationPremiumCommision> ToModels(this IEnumerable<DTOs.Imputations.ApplicationPremiumCommisionDTO> tempApplicationPremiumCommissDTOs)
        {
            return tempApplicationPremiumCommissDTOs.Select(ToModel);
        }
        #endregion

        #region CollectApplicationControl
        public static EEProvider.Models.Integration2G.CollectApplicationControl ToModelIntegration(this EEProvider.Models.Imputations.Application application,int origin, bool update = false)
        {
            return new EEProvider.Models.Integration2G.CollectApplicationControl()
            {
                CollectApplicationId = application.Id,
                Action = (update) ? "U" : "I",
                Origin = origin//2 application 3 technicalTransaction
            };
        }

        public static EEProvider.Models.Integration2G.CollectApplicationControl ToModelIntegration(this EEProvider.Models.Collect.Collect collect, int origin, bool update = false)
        {
            return new EEProvider.Models.Integration2G.CollectApplicationControl()
            {
                CollectApplicationId = collect.Id,
                Action = (update) ? "U" : "I",
                Origin = origin//1 collect
            };
        }
        #endregion

        private static IMapper CreateUpdTempApplicationPremiumComponent()
        {
            var config = MapperCache.GetMapper<UpdTempApplicationPremiumComponentDTO, UpdTempApplicationPremiumComponent>(cfg =>
            {
                cfg.CreateMap<UpdTempApplicationPremiumComponentDTO, UpdTempApplicationPremiumComponent>();
            });
            return config;
        }

        public static UpdTempApplicationPremiumComponent ToModel(this UpdTempApplicationPremiumComponentDTO updTempApplicationPremiumComponent)
        {
            var config = CreateUpdTempApplicationPremiumComponent();
            return config.Map<UpdTempApplicationPremiumComponentDTO, UpdTempApplicationPremiumComponent>(updTempApplicationPremiumComponent);
        }

        private static IMapper CreacteMapJournalEntryReversionParameters()
        {
            var config = MapperCache.GetMapper<JournalEntryReversionParametersDTO, JournalEntryReversionParameters>(cfg =>
            {
                cfg.CreateMap<JournalEntryReversionParametersDTO, JournalEntryReversionParameters>();
                
            });
            return config;
        }

        public static JournalEntryReversionParameters ToModel(this JournalEntryReversionParametersDTO journalEntryReversionParameters)
        {
            var config = CreacteMapJournalEntryReversionParameters();
            return config.Map<JournalEntryReversionParametersDTO, JournalEntryReversionParameters>(journalEntryReversionParameters);
        }
        
        public static IMapper CreateMapLogMassiveDataPolicy()
        {
            var config = MapperCache.GetMapper<LogMassiveDataPolicyDTO, LogMassiveDataPolicy>(cfg =>
            {
                cfg.CreateMap<LogMassiveDataPolicyDTO, LogMassiveDataPolicy>();
            });
            return config;
        }
        
        public static LogMassiveDataPolicy ToModel(this LogMassiveDataPolicyDTO saveBillParameter)
        {
            var config = CreateMapLogMassiveDataPolicy();
            return config.Map<LogMassiveDataPolicyDTO, LogMassiveDataPolicy>(saveBillParameter);
        }
    }
}
