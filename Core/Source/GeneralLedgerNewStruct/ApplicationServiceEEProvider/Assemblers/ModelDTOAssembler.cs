using AutoMapper;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.GeneralLedgerServices.DTOs;
using Sistran.Core.Application.GeneralLedgerServices.DTOs.AccountingConcepts;
using Sistran.Core.Application.GeneralLedgerServices.DTOs.AccountingRules;
using Sistran.Core.Application.GeneralLedgerServices.DTOs.AccountReclassification;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models.AccountingConcepts;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models.AccountingRules;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models.AccountReclassification;
using Sistran.Core.Application.GeneralLedgerServices.Enums;
using Sistran.Core.Application.Utilities.Cache;
using System;
using System.Collections.Generic;
using System.Linq;



namespace Sistran.Core.Application.GeneralLedgerServices.Assemblers
{
    internal static class ModelDTOAssembler
    {

        public static IMapper CreateMapAccountingAccounts()
        {
            var config = MapperCache.GetMapper<AccountingAccountDTO, AccountingAccount>(cfg =>
            {
                cfg.CreateMap<AccountingAccountDTO, AccountingAccount>()
                        .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => new Branch
                        {
                            Id = (src.Branch != null) ? src.Branch.Id : 0,
                            Description = (src.Branch != null) ? src.Branch.Description : ""
                        }))
                        .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => (AccountingNatures)src.AccountingNature))
                        .ForMember(dest => dest.AccountingNature, opt => opt.MapFrom(src => (AccountingNatures)src.AccountingNature))
                        .ForMember(dest => dest.AccountingAccountApplication, opt => opt.MapFrom(src => (EEProvider.Models.AccountingAccountApplications)src.AccountingAccountApplication));

                cfg.CreateMap<AccountingAccountTypeDTO, AccountingAccountType>();
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<AnalysisDTO, Analysis>()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.ConceptKey));
                cfg.CreateMap<CostCenterDTO, CostCenter>();
                cfg.CreateMap<PaymentConceptDTO, PaymentConcept>();
                cfg.CreateMap<AccountingAccountDTO, AccountingAccount>();
                cfg.CreateMap<PrefixDTO, Prefix>();
            });
            return config;
        }

        public static AccountingAccount ToModel(this AccountingAccountDTO accountingAccountDTO)
        {
            var config = CreateMapAccountingAccounts();
            return config.Map<AccountingAccountDTO, AccountingAccount>(accountingAccountDTO);
        }

        public static IEnumerable<AccountingAccount> ToModels(this IEnumerable<AccountingAccountDTO> accountingAccountDTOs)
        {
            return accountingAccountDTOs.Select(ToModel);
        }

        public static IMapper CreateMapAccountingAccountTypes()
        {
            var config = MapperCache.GetMapper<AccountingAccountTypeDTO, AccountingAccountType>(cfg =>
            {
                cfg.CreateMap<AccountingAccountTypeDTO, AccountingAccountType>();
            });
            return config;
        }

        public static AccountingAccountType ToModel(this AccountingAccountTypeDTO accountingAccountTypeDTO)
        {
            var config = CreateMapAccountingAccountTypes();
            return config.Map<AccountingAccountTypeDTO, AccountingAccountType>(accountingAccountTypeDTO);
        }

        public static IEnumerable<AccountingAccountType> ToModels(this IEnumerable<AccountingAccountTypeDTO> accountingAccountTypeDTOs)
        {
            return accountingAccountTypeDTOs.Select(ToModel);
        }

        public static IMapper CreateMapAccountingCompanies()
        {
            var config = MapperCache.GetMapper<AccountingCompanyDTO, AccountingCompany>(cfg =>
            {
                cfg.CreateMap<AccountingCompanyDTO, AccountingCompany>();
            });
            return config;
        }

        public static AccountingCompany ToModel(this AccountingCompanyDTO accountingCompanyDTO)
        {
            var config = CreateMapAccountingCompanies();
            return config.Map<AccountingCompanyDTO, AccountingCompany>(accountingCompanyDTO);
        }

        public static IEnumerable<AccountingCompany> ToModels(this IEnumerable<AccountingCompanyDTO> accountingCompanyDTOs)
        {
            return accountingCompanyDTOs.Select(ToModel);
        }

        public static IMapper CreateMapAccountingMovementTypes()
        {
            var config = MapperCache.GetMapper<AccountingMovementTypeDTO, AccountingMovementType>(cfg =>
            {
                cfg.CreateMap<AccountingMovementTypeDTO, AccountingMovementType>();
            });
            return config;
        }

        public static AccountingMovementType ToModel(this AccountingMovementTypeDTO accountingMovementTypeDTO)
        {
            var config = CreateMapAccountingMovementTypes();
            return config.Map<AccountingMovementTypeDTO, AccountingMovementType>(accountingMovementTypeDTO);
        }

        public static IEnumerable<AccountingMovementType> ToModels(this IEnumerable<AccountingMovementTypeDTO> accountingMovementTypeDTOs)
        {
            return accountingMovementTypeDTOs.Select(ToModel);
        }

        public static IMapper CreateMapAnalysiss()
        {
            var config = MapperCache.GetMapper<AnalysisDTO, Analysis>(cfg =>
            {
                cfg.CreateMap<AnalysisDTO, Analysis>()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.ConceptKey));
                cfg.CreateMap<AnalysisConceptDTO, AnalysisConcept>();
            });
            return config;
        }

        public static Analysis ToModel(this AnalysisDTO analysisDTO)
        {
            var config = CreateMapAnalysiss();
            return config.Map<AnalysisDTO, Analysis>(analysisDTO);
        }

        public static IEnumerable<Analysis> ToModels(this IEnumerable<AnalysisDTO> analysisDTOs)
        {
            return analysisDTOs.Select(ToModel);
        }

        public static IMapper CreateMapAnalysisCodes()
        {
            var config = MapperCache.GetMapper<AnalysisCodeDTO, AnalysisCode>(cfg =>
            {
                cfg.CreateMap<AnalysisCodeDTO, AnalysisCode>()
                        .ForMember(dest => dest.AccountingNature, opt => opt.MapFrom(src => (AccountingNatures)src.AccountingNature));
                cfg.CreateMap<AnalysisConceptDTO, AnalysisConcept>();
            });
            return config;
        }

        public static AnalysisCode ToModel(this AnalysisCodeDTO analysisCodeDTO)
        {
            var config = CreateMapAnalysisCodes();
            return config.Map<AnalysisCodeDTO, AnalysisCode>(analysisCodeDTO);
        }

        public static IEnumerable<AnalysisCode> ToModels(this IEnumerable<AnalysisCodeDTO> analysisCodeDTOs)
        {
            return analysisCodeDTOs.Select(ToModel);
        }

        public static IMapper CreateMapAnalysisConcepts()
        {
            var config = MapperCache.GetMapper<AnalysisConceptDTO, AnalysisConcept>(cfg =>
            {
                cfg.CreateMap<AnalysisConceptDTO, AnalysisConcept>();
                cfg.CreateMap<AnalysisCodeDTO, AnalysisCode>();
                cfg.CreateMap<AnalysisTreatmentDTO, AnalysisTreatment>();
            });
            return config;
        }

        public static AnalysisConcept ToModel(this AnalysisConceptDTO analysisConceptDTO)
        {
            var config = CreateMapAnalysisConcepts();
            return config.Map<AnalysisConceptDTO, AnalysisConcept>(analysisConceptDTO);
        }

        public static IEnumerable<AnalysisConcept> ToModels(this IEnumerable<AnalysisConceptDTO> analysisConceptDTOs)
        {
            return analysisConceptDTOs.Select(ToModel);
        }

        public static IMapper CreateMapAnalysisConceptKeies()
        {
            var config = MapperCache.GetMapper<AnalysisConceptKeyDTO, AnalysisConceptKey>(cfg =>
            {
                cfg.CreateMap<AnalysisConceptKeyDTO, AnalysisConceptKey>();
                cfg.CreateMap<AnalysisConceptDTO, AnalysisConcept>();
            });
            return config;
        }

        public static AnalysisConceptKey ToModel(this AnalysisConceptKeyDTO analysisConceptKeyDTO)
        {
            var config = CreateMapAnalysisConceptKeies();
            return config.Map<AnalysisConceptKeyDTO, AnalysisConceptKey>(analysisConceptKeyDTO);
        }

        public static IEnumerable<AnalysisConceptKey> ToModels(this IEnumerable<AnalysisConceptKeyDTO> analysisConceptKeyDTOs)
        {
            return analysisConceptKeyDTOs.Select(ToModel);
        }

        public static IMapper CreateMapAnalysisTreatments()
        {
            var config = MapperCache.GetMapper<AnalysisTreatmentDTO, AnalysisTreatment>(cfg =>
            {
                cfg.CreateMap<AnalysisTreatmentDTO, AnalysisTreatment>();
            });
            return config;
        }

        public static AnalysisTreatment ToModel(this AnalysisTreatmentDTO analysisTreatmentDTO)
        {
            var config = CreateMapAnalysisTreatments();
            return config.Map<AnalysisTreatmentDTO, AnalysisTreatment>(analysisTreatmentDTO);
        }

        public static IEnumerable<AnalysisTreatment> ToModels(this IEnumerable<AnalysisTreatmentDTO> analysisTreatmentDTOs)
        {
            return analysisTreatmentDTOs.Select(ToModel);
        }

        public static IMapper CreateMapCostCenters()
        {
            var config = MapperCache.GetMapper<CostCenterDTO, CostCenter>(cfg =>
            {
                cfg.CreateMap<CostCenterDTO, CostCenter>();
                cfg.CreateMap<CostCenterTypeDTO, CostCenterType>();
            });
            return config;
        }

        public static CostCenter ToModel(this CostCenterDTO costCenterDTO)
        {
            var config = CreateMapCostCenters();
            return config.Map<CostCenterDTO, CostCenter>(costCenterDTO);
        }

        public static IEnumerable<CostCenter> ToModels(this IEnumerable<CostCenterDTO> costCenterDTOs)
        {
            return costCenterDTOs.Select(ToModel);
        }

        public static IMapper CreateMapCostCenterTypes()
        {
            var config = MapperCache.GetMapper<CostCenterTypeDTO, CostCenterType>(cfg =>
            {
                cfg.CreateMap<CostCenterTypeDTO, CostCenterType>();
            });
            return config;
        }

        public static CostCenterType ToModel(this CostCenterTypeDTO costCenterTypeDTO)
        {
            var config = CreateMapCostCenterTypes();
            return config.Map<CostCenterTypeDTO, CostCenterType>(costCenterTypeDTO);
        }

        public static IEnumerable<CostCenterType> ToModels(this IEnumerable<CostCenterTypeDTO> costCenterTypeDTOs)
        {
            return costCenterTypeDTOs.Select(ToModel);
        }

        public static IMapper CreateMapEntryDestinations()
        {
            var config = MapperCache.GetMapper<EntryDestinationDTO, EntryDestination>(cfg =>
            {
                cfg.CreateMap<EntryDestinationDTO, EntryDestination>();
            });
            return config;
        }

        public static EntryDestination ToModel(this EntryDestinationDTO entryDestinationDTO)
        {
            var config = CreateMapEntryDestinations();
            return config.Map<EntryDestinationDTO, EntryDestination>(entryDestinationDTO);
        }

        public static IEnumerable<EntryDestination> ToModels(this IEnumerable<EntryDestinationDTO> entryDestinationDTOs)
        {
            return entryDestinationDTOs.Select(ToModel);
        }

        public static IMapper CreateMapEntryNumbers()
        {
            var config = MapperCache.GetMapper<EntryNumberDTO, EntryNumber>(cfg =>
            {
                cfg.CreateMap<EntryNumberDTO, EntryNumber>();
                cfg.CreateMap<AccountingMovementTypeDTO, AccountingMovementType>();
                cfg.CreateMap<EntryDestinationDTO, EntryDestination>();
            });
            return config;
        }

        public static EntryNumber ToModel(this EntryNumberDTO entryNumberDTO)
        {
            var config = CreateMapEntryNumbers();
            return config.Map<EntryNumberDTO, EntryNumber>(entryNumberDTO);
        }

        public static IEnumerable<EntryNumber> ToModels(this IEnumerable<EntryNumberDTO> entryNumberDTOs)
        {
            return entryNumberDTOs.Select(ToModel);
        }

        public static IMapper CreateMapEntryTypes()
        {
            var config = MapperCache.GetMapper<EntryTypeDTO, EntryType>(cfg =>
            {
                cfg.CreateMap<EntryTypeDTO, EntryType>();
                cfg.CreateMap<EntryTypeItemDTO, EntryTypeItem>();
                cfg.CreateMap<AccountingAccountDTO, AccountingAccount>();
            });
            return config;
        }

        public static EntryType ToModel(this EntryTypeDTO entryTypeDTO)
        {
            var config = CreateMapEntryTypes();
            return config.Map<EntryTypeDTO, EntryType>(entryTypeDTO);
        }

        public static IEnumerable<EntryType> ToModels(this IEnumerable<EntryTypeDTO> entryTypeDTOs)
        {
            return entryTypeDTOs.Select(ToModel);
        }

        public static IMapper CreateMapEntryTypeItems()
        {
            var config = MapperCache.GetMapper<EntryTypeItemDTO, EntryTypeItem>(cfg =>
            {
                cfg.CreateMap<EntryTypeItemDTO, EntryTypeItem>()
                        .ForMember(dest => dest.AccountingNature, opt => opt.MapFrom(src => (AccountingNatures)src.AccountingNature));
                cfg.CreateMap<AccountingMovementTypeDTO, AccountingMovementType>();
                cfg.CreateMap<AccountingAccountDTO, AccountingAccount>();
                cfg.CreateMap<AccountingConceptDTO, AccountingConcept>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<AnalysisDTO, Analysis>()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.ConceptKey));
                cfg.CreateMap<CostCenterDTO, CostCenter>();
            });
            return config;
        }

        public static EntryTypeItem ToModel(this EntryTypeItemDTO entryTypeItemDTO)
        {
            var config = CreateMapEntryTypeItems();
            return config.Map<EntryTypeItemDTO, EntryTypeItem>(entryTypeItemDTO);
        }

        public static IEnumerable<EntryTypeItem> ToModels(this IEnumerable<EntryTypeItemDTO> entryTypeItemDTOs)
        {
            return entryTypeItemDTOs.Select(ToModel);
        }

        public static IMapper CreateMapJournalEntries()
        {
            var config = MapperCache.GetMapper<JournalEntryDTO, JournalEntry>(cfg =>
            {
                cfg.CreateMap<JournalEntryDTO, JournalEntry>();
                cfg.CreateMap<AccountingCompanyDTO, AccountingCompany>();
                cfg.CreateMap<AccountingMovementTypeDTO, AccountingMovementType>();
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<JournalEntryItemDTO, JournalEntryItem>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<ExchangeRateDTO, ExchangeRate>();
                cfg.CreateMap<AccountingAccountDTO, AccountingAccount>();
                cfg.CreateMap<PaymentConceptDTO, PaymentConcept>();
                cfg.CreateMap<CostCenterDTO, CostCenter>();
                cfg.CreateMap<CostCenterTypeDTO, CostCenterType>();
                cfg.CreateMap<AnalysisDTO, Analysis>()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.ConceptKey));
                cfg.CreateMap<AnalysisConceptDTO, AnalysisConcept>();
                cfg.CreateMap<ReconciliationMovementTypeDTO, ReconciliationMovementType>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<IndividualDTO, UniquePersonService.V1.Models.Individual>();
                cfg.CreateMap<EntryTypeDTO, EntryType>();
                cfg.CreateMap<EntryTypeItemDTO, EntryTypeItem>();
                cfg.CreateMap<AccountingMovementTypeDTO, AccountingMovementType>();
                cfg.CreateMap<AccountingConceptDTO, AccountingConcept>();
                cfg.CreateMap<ReceiptDTO, Receipt>();
                cfg.CreateMap<PostDatedDTO, PostDated>();

            });
            return config;
        }

        public static JournalEntry ToModel(this JournalEntryDTO journalEntryDTO)
        {
            var config = CreateMapJournalEntries();
            return config.Map<JournalEntryDTO, JournalEntry>(journalEntryDTO);
        }

        public static IEnumerable<JournalEntry> ToModels(this IEnumerable<JournalEntryDTO> journalEntryDTOs)
        {
            return journalEntryDTOs.Select(ToModel);
        }

        public static IMapper CreateMapJournalEntryItems()
        {
            var config = MapperCache.GetMapper<JournalEntryItemDTO, JournalEntryItem>(cfg =>
            {
                cfg.CreateMap<JournalEntryItemDTO, JournalEntryItem>()
                        .ForMember(dest => dest.AccountingNature, opt => opt.MapFrom(src => (AccountingNatures)src.AccountingNature));
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<ExchangeRateDTO, ExchangeRate>();
                cfg.CreateMap<AccountingAccountDTO, AccountingAccount>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<PaymentConceptDTO, PaymentConcept>();
                cfg.CreateMap<ReconciliationMovementTypeDTO, ReconciliationMovementType>();
                cfg.CreateMap<EntryTypeItemDTO, EntryTypeItem>();
                cfg.CreateMap<CostCenterTypeDTO, CostCenterType>();
                cfg.CreateMap<AnalysisConceptDTO, AnalysisConcept>();
                cfg.CreateMap<AnalysisCodeDTO, AnalysisCode>();
                cfg.CreateMap<AnalysisTreatmentDTO, AnalysisTreatment>();
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<AccountingAccountTypeDTO, AccountingAccountType>();
                cfg.CreateMap<PrefixDTO, Prefix>();
                cfg.CreateMap<ReceiptDTO, Receipt>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<IndividualDTO, UniquePersonService.V1.Models.Individual>();
                cfg.CreateMap<EntryTypeDTO, EntryType>();
                cfg.CreateMap<CostCenterDTO, CostCenter>();
                cfg.CreateMap<AnalysisDTO, Analysis>()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.ConceptKey));
                cfg.CreateMap<PostDatedDTO, PostDated>();
            });
            return config;
        }

        public static JournalEntryItem ToModel(this JournalEntryItemDTO journalEntryItemDTO)
        {
            var config = CreateMapJournalEntryItems();
            return config.Map<JournalEntryItemDTO, JournalEntryItem>(journalEntryItemDTO);
        }

        public static IEnumerable<JournalEntryItem> ToModels(this IEnumerable<JournalEntryItemDTO> journalEntryItemDTOs)
        {
            return journalEntryItemDTOs.Select(ToModel);
        }

        public static IMapper CreateMapLedgerEntries()
        {
            var config = MapperCache.GetMapper<LedgerEntryDTO, LedgerEntry>(cfg =>
            {
                cfg.CreateMap<LedgerEntryDTO, LedgerEntry>();
                cfg.CreateMap<AccountingCompanyDTO, AccountingCompany>();
                cfg.CreateMap<AccountingMovementTypeDTO, AccountingMovementType>();
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<EntryDestinationDTO, EntryDestination>();
                cfg.CreateMap<LedgerEntryItemDTO, LedgerEntryItem>();
            });
            return config;
        }

        public static LedgerEntry ToModel(this LedgerEntryDTO ledgerEntryDTO)
        {
            var config = CreateMapLedgerEntries();
            return config.Map<LedgerEntryDTO, LedgerEntry>(ledgerEntryDTO);
        }

        public static IEnumerable<LedgerEntry> ToModels(this IEnumerable<LedgerEntryDTO> ledgerEntryDTOs)
        {
            return ledgerEntryDTOs.Select(ToModel);
        }

        public static IMapper CreateMapLedgerEntryItems()
        {
            var config = MapperCache.GetMapper<LedgerEntryItemDTO, LedgerEntryItem>(cfg =>
            {
                cfg.CreateMap<LedgerEntryItemDTO, LedgerEntryItem>()
                        .ForMember(dest => dest.AccountingNature, opt => opt.MapFrom(src => (AccountingNatures)src.AccountingNature));
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<ExchangeRateDTO, ExchangeRate>();
                cfg.CreateMap<AccountingAccountDTO, AccountingAccount>();
                cfg.CreateMap<ReconciliationMovementTypeDTO, ReconciliationMovementType>();
                cfg.CreateMap<ReceiptDTO, Receipt>();
                cfg.CreateMap<AmountDTO, Amount>();
                //cfg.CreateMap<IndividualDTO, Individual>();
                cfg.CreateMap<EntryTypeDTO, EntryType>();
                cfg.CreateMap<CostCenterDTO, CostCenter>();
                cfg.CreateMap<AnalysisDTO, Analysis>()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.ConceptKey));
                cfg.CreateMap<PostDatedDTO, PostDated>();
            });
            return config;
        }

        public static LedgerEntryItem ToModel(this LedgerEntryItemDTO ledgerEntryItemDTO)
        {
            var config = CreateMapLedgerEntryItems();
            return config.Map<LedgerEntryItemDTO, LedgerEntryItem>(ledgerEntryItemDTO);
        }

        public static IEnumerable<LedgerEntryItem> ToModels(this IEnumerable<LedgerEntryItemDTO> ledgerEntryItemDTOs)
        {
            return ledgerEntryItemDTOs.Select(ToModel);
        }

        public static IMapper CreateMapPostDateds()
        {
            var config = MapperCache.GetMapper<PostDatedDTO, PostDated>(cfg =>
            {
                cfg.CreateMap<PostDatedDTO, PostDated>()
                        //.ForMember(dest => dest.PostDateType, opt => opt.MapFrom(src => (PostDateTypes)src.PostDateType))
                        ;
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<ExchangeRateDTO, ExchangeRate>();
            });
            return config;
        }

        public static PostDated ToModel(this PostDatedDTO postDatedDTO)
        {
            var config = CreateMapPostDateds();
            return config.Map<PostDatedDTO, PostDated>(postDatedDTO);
        }

        public static IEnumerable<PostDated> ToModels(this IEnumerable<PostDatedDTO> postDatedDTOs)
        {
            return postDatedDTOs.Select(ToModel);
        }

        public static IMapper CreateMapProcessLogs()
        {
            var config = MapperCache.GetMapper<ProcessLogDTO, ProcessLog>(cfg =>
            {
                cfg.CreateMap<ProcessLogDTO, ProcessLog>()
                        .ForMember(dest => dest.ProcessLogStatus, opt => opt.MapFrom(src => (ProcessLogStatus)src.ProcessLogStatus));
            });
            return config;
        }

        public static ProcessLog ToModel(this ProcessLogDTO processLogDTO)
        {
            var config = CreateMapProcessLogs();
            return config.Map<ProcessLogDTO, ProcessLog>(processLogDTO);
        }

        public static IEnumerable<ProcessLog> ToModels(this IEnumerable<ProcessLogDTO> processLogDTOs)
        {
            return processLogDTOs.Select(ToModel);
        }

        public static IMapper CreateMapReceipts()
        {
            var config = MapperCache.GetMapper<ReceiptDTO, Receipt>(cfg =>
            {
                cfg.CreateMap<ReceiptDTO, Receipt>();
            });
            return config;
        }

        public static Receipt ToModel(this ReceiptDTO receiptDTO)
        {
            var config = CreateMapReceipts();
            return config.Map<ReceiptDTO, Receipt>(receiptDTO);
        }

        public static IEnumerable<Receipt> ToModels(this IEnumerable<ReceiptDTO> receiptDTOs)
        {
            return receiptDTOs.Select(ToModel);
        }

        public static IMapper CreateMapReconciliationMovementTypes()
        {
            var config = MapperCache.GetMapper<ReconciliationMovementTypeDTO, ReconciliationMovementType>(cfg =>
            {
                cfg.CreateMap<ReconciliationMovementTypeDTO, ReconciliationMovementType>()
                        .ForMember(dest => dest.AccountingNature, opt => opt.MapFrom(src => (AccountingNatures)src.AccountingNature));
            });
            return config;
        }

        public static ReconciliationMovementType ToModel(this ReconciliationMovementTypeDTO reconciliationMovementTypeDTO)
        {
            var config = CreateMapReconciliationMovementTypes();
            return config.Map<ReconciliationMovementTypeDTO, ReconciliationMovementType>(reconciliationMovementTypeDTO);
        }

        public static IEnumerable<ReconciliationMovementType> ToModels(this IEnumerable<ReconciliationMovementTypeDTO> reconciliationMovementTypeDTOs)
        {
            return reconciliationMovementTypeDTOs.Select(ToModel);
        }

        public static IMapper CreateMapAccountReclassifications()
        {
            var config = MapperCache.GetMapper<AccountReclassificationDTO, AccountReclassification>(cfg =>
            {
                cfg.CreateMap<AccountReclassificationDTO, AccountReclassification>();
                cfg.CreateMap<AccountingAccountDTO, AccountingAccount>();
            });
            return config;
        }

        public static AccountReclassification ToModel(this AccountReclassificationDTO accountReclassificationDTO)
        {
            var config = CreateMapAccountReclassifications();
            return config.Map<AccountReclassificationDTO, AccountReclassification>(accountReclassificationDTO);
        }

        public static IEnumerable<AccountReclassification> ToModels(this IEnumerable<AccountReclassificationDTO> accountReclassificationDTOs)
        {
            return accountReclassificationDTOs.Select(ToModel);
        }

        public static IMapper CreateMapAccountReclassificationResults()
        {
            var config = MapperCache.GetMapper<AccountReclassificationResultDTO, AccountReclassificationResult>(cfg =>
            {
                cfg.CreateMap<AccountReclassificationResultDTO, AccountReclassificationResult>()
                        .ForMember(dest => dest.AccountingNature, opt => opt.MapFrom(src => (AccountingNatures)src.AccountingNature));
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<ExchangeRateDTO, ExchangeRate>();
                cfg.CreateMap<AccountingAccountDTO, AccountingAccount>();
                cfg.CreateMap<AnalysisDTO, Analysis>()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.ConceptKey));
                cfg.CreateMap<CostCenterDTO, CostCenter>();
            });
            return config;
        }

        public static AccountReclassificationResult ToModel(this AccountReclassificationResultDTO accountReclassificationResultDTO)
        {
            var config = CreateMapAccountReclassificationResults();
            return config.Map<AccountReclassificationResultDTO, AccountReclassificationResult>(accountReclassificationResultDTO);
        }

        public static IEnumerable<AccountReclassificationResult> ToModels(this IEnumerable<AccountReclassificationResultDTO> accountReclassificationResultDTOs)
        {
            return accountReclassificationResultDTOs.Select(ToModel);
        }

        public static IMapper CreateMapAccountingAccountMasks()
        {
            var config = MapperCache.GetMapper<AccountingAccountMaskDTO, AccountingAccountMask>(cfg =>
            {
                cfg.CreateMap<AccountingAccountMaskDTO, AccountingAccountMask>();
                cfg.CreateMap<ParameterDTO, CommonService.Models.Parameter>();
            });
            return config;
        }

        public static AccountingAccountMask ToModel(this AccountingAccountMaskDTO accountingAccountMaskDTO)
        {
            var config = CreateMapAccountingAccountMasks();
            return config.Map<AccountingAccountMaskDTO, AccountingAccountMask>(accountingAccountMaskDTO);
        }

        public static IEnumerable<AccountingAccountMask> ToModels(this IEnumerable<AccountingAccountMaskDTO> accountingAccountMaskDTOs)
        {
            return accountingAccountMaskDTOs.Select(ToModel);
        }

        public static IMapper CreateMapAccountingRules()
        {
            var config = MapperCache.GetMapper<AccountingRuleDTO, AccountingRule>(cfg =>
            {
                cfg.CreateMap<AccountingRuleDTO, AccountingRule>();
                cfg.CreateMap<ConditionDTO, Condition>();
                cfg.CreateMap<ParameterDTO, EEProvider.Models.AccountingRules.Parameter>();
            });
            return config;
        }

        public static AccountingRule ToModel(this AccountingRuleDTO accountingRuleDTO)
        {
            var config = CreateMapAccountingRules();
            return config.Map<AccountingRuleDTO, AccountingRule>(accountingRuleDTO);
        }

        public static IEnumerable<AccountingRule> ToModels(this IEnumerable<AccountingRuleDTO> accountingRuleDTOs)
        {
            return accountingRuleDTOs.Select(ToModel);
        }

        public static IMapper CreateMapAccountingRulePackages()
        {
            var config = MapperCache.GetMapper<AccountingRulePackageDTO, AccountingRulePackage>(cfg =>
            {
                cfg.CreateMap<AccountingRulePackageDTO, AccountingRulePackage>();
                cfg.CreateMap<AccountingRuleDTO, AccountingRule>();
            });
            return config;
        }

        public static AccountingRulePackage ToModel(this AccountingRulePackageDTO accountingRulePackageDTO)
        {
            var config = CreateMapAccountingRulePackages();
            return config.Map<AccountingRulePackageDTO, AccountingRulePackage>(accountingRulePackageDTO);
        }

        public static IEnumerable<AccountingRulePackage> ToModels(this IEnumerable<AccountingRulePackageDTO> accountingRulePackageDTOs)
        {
            return accountingRulePackageDTOs.Select(ToModel);
        }

        public static IMapper CreateMapConditions()
        {
            var config = MapperCache.GetMapper<ConditionDTO, Condition>(cfg =>
            {
                cfg.CreateMap<ConditionDTO, Condition>();
                cfg.CreateMap<AccountingRuleDTO, AccountingRule>();
                cfg.CreateMap<ParameterDTO, CommonService.Models.Parameter>();
            });
            return config;
        }

        public static Condition ToModel(this ConditionDTO conditionDTO)
        {
            var config = CreateMapConditions();
            return config.Map<ConditionDTO, Condition>(conditionDTO);
        }

        public static IEnumerable<Condition> ToModels(this IEnumerable<ConditionDTO> conditionDTOs)
        {
            return conditionDTOs.Select(ToModel);
        }

        public static IMapper CreateMapParameters()
        {
            var config = MapperCache.GetMapper<ParameterDTO, CommonService.Models.Parameter>(cfg =>
            {
                cfg.CreateMap<ParameterDTO, CommonService.Models.Parameter>();
            });
            return config;
        }

        public static EEProvider.Models.AccountingRules.Parameter ToModel(this ParameterDTO parameterDTO)
        {
            var config = CreateMapParameters();
            return config.Map<ParameterDTO, EEProvider.Models.AccountingRules.Parameter>(parameterDTO);
        }

        public static IEnumerable<EEProvider.Models.AccountingRules.Parameter> ToModels(this IEnumerable<ParameterDTO> parameterDTOs)
        {
            return parameterDTOs.Select(ToModel);
        }

        public static IMapper CreateMapParametersRules()
        {
            var config = MapperCache.GetMapper<ParameterDTO, CommonService.Models.Parameter>(cfg =>
            {
                cfg.CreateMap<ParameterDTO, EEProvider.Models.AccountingRules.Parameter>();
            });
            return config;
        }

        public static EEProvider.Models.AccountingRules.Parameter ToModelRules(this ParameterDTO parameterRulesDTO)
        {
            var config = CreateMapParametersRules();
            return config.Map<ParameterDTO, EEProvider.Models.AccountingRules.Parameter>(parameterRulesDTO);
        }

        public static IEnumerable<EEProvider.Models.AccountingRules.Parameter> ToModelsRules(this IEnumerable<ParameterDTO> parameterRulesDTOs)
        {
            return parameterRulesDTOs.Select(ToModelRules);
        }


        public static IMapper CreateMapResults()
        {
            var config = MapperCache.GetMapper<ResultDTO, Result>(cfg =>
            {
                cfg.CreateMap<ResultDTO, Result>()
                        .ForMember(dest => dest.AccountingNature, opt => opt.MapFrom(src => (AccountingNatures)src.AccountingNature));
                cfg.CreateMap<AccountingAccountMaskDTO, AccountingAccountMask>();
                cfg.CreateMap<ParameterDTO, EEProvider.Models.AccountingRules.Parameter>();
            });
            return config;
        }

        public static Result ToModel(this ResultDTO resultDTO)
        {
            var config = CreateMapResults();
            return config.Map<ResultDTO, Result>(resultDTO);
        }

        public static IEnumerable<Result> ToModels(this IEnumerable<ResultDTO> resultDTOs)
        {
            return resultDTOs.Select(ToModel);
        }

        public static IMapper CreateMapAccountingConcepts()
        {
            var config = MapperCache.GetMapper<AccountingConceptDTO, AccountingConcept>(cfg =>
            {
                cfg.CreateMap<AccountingConceptDTO, AccountingConcept>();
                cfg.CreateMap<AccountingAccountDTO, AccountingAccount>();
            });
            return config;
        }

        public static AccountingConcept ToModel(this AccountingConceptDTO accountingConceptDTO)
        {
            var config = CreateMapAccountingConcepts();
            return config.Map<AccountingConceptDTO, AccountingConcept>(accountingConceptDTO);
        }

        public static IEnumerable<AccountingConcept> ToModels(this IEnumerable<AccountingConceptDTO> accountingConceptDTOs)
        {
            return accountingConceptDTOs.Select(ToModel);
        }

        public static IMapper CreateMapBranchAccountingConcepts()
        {
            var config = MapperCache.GetMapper<BranchAccountingConceptDTO, BranchAccountingConcept>(cfg =>
            {
                cfg.CreateMap<BranchAccountingConceptDTO, BranchAccountingConcept>();
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<AccountingConceptDTO, AccountingConcept>();
                cfg.CreateMap<MovementTypeDTO, MovementType>();
            });
            return config;
        }

        public static BranchAccountingConcept ToModel(this BranchAccountingConceptDTO branchAccountingConceptDTO)
        {
            var config = CreateMapBranchAccountingConcepts();
            return config.Map<BranchAccountingConceptDTO, BranchAccountingConcept>(branchAccountingConceptDTO);
        }

        public static IEnumerable<BranchAccountingConcept> ToModels(this IEnumerable<BranchAccountingConceptDTO> branchAccountingConceptDTOs)
        {
            return branchAccountingConceptDTOs.Select(ToModel);
        }

        public static IMapper CreateMapConceptSources()
        {
            var config = MapperCache.GetMapper<ConceptSourceDTO, ConceptSource>(cfg =>
            {
                cfg.CreateMap<ConceptSourceDTO, ConceptSource>();
            });
            return config;
        }

        public static ConceptSource ToModel(this ConceptSourceDTO conceptSourceDTO)
        {
            var config = CreateMapConceptSources();
            return config.Map<ConceptSourceDTO, ConceptSource>(conceptSourceDTO);
        }

        public static IEnumerable<ConceptSource> ToModels(this IEnumerable<ConceptSourceDTO> conceptSourceDTOs)
        {
            return conceptSourceDTOs.Select(ToModel);
        }

        public static IMapper CreateMapMovementTypes()
        {
            var config = MapperCache.GetMapper<MovementTypeDTO, MovementType>(cfg =>
            {
                cfg.CreateMap<MovementTypeDTO, MovementType>();
                cfg.CreateMap<ConceptSourceDTO, ConceptSource>();
            });
            return config;
        }

        public static MovementType ToModel(this MovementTypeDTO movementTypeDTO)
        {
            var config = CreateMapMovementTypes();
            return config.Map<MovementTypeDTO, MovementType>(movementTypeDTO);
        }

        public static IEnumerable<MovementType> ToModels(this IEnumerable<MovementTypeDTO> movementTypeDTOs)
        {
            return movementTypeDTOs.Select(ToModel);
        }

        public static IMapper CreateMapUserBranchAccountingConcepts()
        {
            var config = MapperCache.GetMapper<UserBranchAccountingConceptDTO, UserBranchAccountingConcept>(cfg =>
            {
                cfg.CreateMap<UserBranchAccountingConceptDTO, UserBranchAccountingConcept>();
                cfg.CreateMap<BranchAccountingConceptDTO, BranchAccountingConcept>();
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<AccountingConceptDTO, AccountingConcept>();
                cfg.CreateMap<AccountingAccountDTO, AccountingAccount>();
                cfg.CreateMap<MovementTypeDTO, MovementType>();
                cfg.CreateMap<ConceptSourceDTO, ConceptSource>();
                cfg.CreateMap<AccountingAccountTypeDTO, AccountingAccountType>();
                cfg.CreateMap<PrefixDTO, Prefix>();
                cfg.CreateMap<PaymentConceptDTO, PaymentConcept>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<AnalysisDTO, Analysis>()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.ConceptKey));
                cfg.CreateMap<AnalysisConceptDTO, AnalysisConcept>();
                cfg.CreateMap<AnalysisCodeDTO, AnalysisCode>();
                cfg.CreateMap<CostCenterDTO, CostCenter>();
                cfg.CreateMap<CostCenterTypeDTO, CostCenterType>();
                cfg.CreateMap<AnalysisTreatmentDTO, AnalysisTreatment>();
            });
            return config;
        }

        public static UserBranchAccountingConcept ToModel(this UserBranchAccountingConceptDTO userBranchAccountingConceptDTO)
        {
            var config = CreateMapUserBranchAccountingConcepts();
            return config.Map<UserBranchAccountingConceptDTO, UserBranchAccountingConcept>(userBranchAccountingConceptDTO);
        }

        public static IEnumerable<UserBranchAccountingConcept> ToModels(this IEnumerable<UserBranchAccountingConceptDTO> userBranchAccountingConceptDTOs)
        {
            return userBranchAccountingConceptDTOs.Select(ToModel);
        }

        public static IMapper CreateMapBranches()
        {
            var config = MapperCache.GetMapper<BranchDTO, Branch>(cfg =>
            {
                cfg.CreateMap<BranchDTO, Branch>();
            });
            return config;
        }

        public static Branch ToModel(this BranchDTO branch)
        {
            var config = CreateMapBranches();
            return config.Map<BranchDTO, Branch>(branch);
        }

        public static IEnumerable<Branch> ToModels(this IEnumerable<BranchDTO> branches)
        {
            return branches.Select(ToModel);
        }

        public static IMapper CreateMapAccountingAccountFilter()
        {
            var config = MapperCache.GetMapper<AccountingAccountFilterDTO, AccountingAccountFilter>(cfg =>
            {
                cfg.CreateMap<AccountingAccountFilterDTO, AccountingAccountFilter>();
            });
            return config;
        }

        public static AccountingAccountFilter ToModel(this AccountingAccountFilterDTO accountingAccountFilterDTO)
        {
            var config = CreateMapAccountingAccountFilter();
            return config.Map<AccountingAccountFilterDTO, AccountingAccountFilter>(accountingAccountFilterDTO);
        }
        #region CollectApplicationControl
        public static EEProvider.Models.Integration2G.CollectApplicationControl ToModelIntegration(int technicalTransaction, bool update = false)
        {
            return new EEProvider.Models.Integration2G.CollectApplicationControl()
            {
                CollectApplicationId = technicalTransaction,
                Action = (update) ? "U" : "I",
                Origin = 3//se envia 3 ya que es technicaltransaction
            };
        }

        public static EEProvider.Models.Integration2G.CollectApplicationControl ToModelIntegrationOnce(int technicalTransaction, bool update = false)
        {
            return new EEProvider.Models.Integration2G.CollectApplicationControl()
            {
                CollectApplicationId = technicalTransaction,
                Action = (update) ? "U" : "I",
                Origin = 0//se envia 0 para que se procese una única vez
            };
        }
        #endregion

        public static IMapper CreateMapJournalEntryParameters()
        {
            var config = MapperCache.GetMapper<JournalEntryParameter, JournalEntry>(cfg =>
            {
                cfg.CreateMap<JournalEntryParameter, JournalEntry>();
                cfg.CreateMap<AccountingCompany, AccountingCompany>();
                cfg.CreateMap<AccountingMovementType, AccountingMovementType>();
                cfg.CreateMap<Branch, Branch>();
                cfg.CreateMap<SalePoint, SalePoint>();
                cfg.CreateMap<JournalEntryItem, JournalEntryItem>();
                cfg.CreateMap<Currency, Currency>();
                cfg.CreateMap<ExchangeRate, ExchangeRate>();
                cfg.CreateMap<AccountingAccount, AccountingAccount>();
                cfg.CreateMap<PaymentConcept, PaymentConcept>();
                cfg.CreateMap<CostCenter, CostCenter>();
                cfg.CreateMap<CostCenterType, CostCenterType>();
                cfg.CreateMap<Analysis, Analysis>();
                cfg.CreateMap<AnalysisConcept, AnalysisConcept>();
                cfg.CreateMap<ReconciliationMovementType, ReconciliationMovementType>();
                cfg.CreateMap<Amount, Amount>();
                cfg.CreateMap<UniquePersonService.V1.Models.Individual, UniquePersonService.V1.Models.Individual>();
                cfg.CreateMap<EntryType, EntryType>();
                cfg.CreateMap<EntryTypeItem, EntryTypeItem>();
                cfg.CreateMap<AccountingMovementType, AccountingMovementType>();
                cfg.CreateMap<AccountingConcept, AccountingConcept>();
                cfg.CreateMap<Receipt, Receipt>();
                cfg.CreateMap<PostDated, PostDated>();

            });
            return config;
        }

        public static JournalEntry ToModel(this JournalEntryParameter journalEntryParameter)
        {
            var config = CreateMapJournalEntryParameters();
            return config.Map<JournalEntryParameter, JournalEntry>(journalEntryParameter);
        }

        public static IEnumerable<JournalEntry> ToModels(this IEnumerable<JournalEntryParameter> JournalEntryItemParameters)
        {
            return JournalEntryItemParameters.Select(ToModel);
        }


        public static IMapper CreateMapJournalEntryItemsParameter()
        {
            var config = MapperCache.GetMapper<JournalEntryItemParameter, JournalEntryItem>(cfg =>
            {
                cfg.CreateMap<JournalEntryItemParameter, JournalEntryItem>()
                        .ForMember(dest => dest.AccountingNature, opt => opt.MapFrom(src => (AccountingNatures)src.AccountingNature));
                cfg.CreateMap<Currency, Currency>();
                cfg.CreateMap<ExchangeRate, ExchangeRate>();
                cfg.CreateMap<AccountingAccount, AccountingAccount>();
                cfg.CreateMap<SalePoint, SalePoint>();
                cfg.CreateMap<PaymentConcept, PaymentConcept>();
                cfg.CreateMap<ReconciliationMovementType, ReconciliationMovementType>();
                cfg.CreateMap<EntryTypeItem, EntryTypeItem>();
                cfg.CreateMap<CostCenterType, CostCenterType>();
                cfg.CreateMap<AnalysisConcept, AnalysisConcept>();
                cfg.CreateMap<AnalysisCode, AnalysisCode>();
                cfg.CreateMap<AnalysisTreatment, AnalysisTreatment>();
                cfg.CreateMap<Branch, Branch>();
                cfg.CreateMap<AccountingAccountType, AccountingAccountType>();
                cfg.CreateMap<Prefix, Prefix>();
                cfg.CreateMap<Receipt, Receipt>();
                cfg.CreateMap<Amount, Amount>();
                cfg.CreateMap<UniquePersonService.V1.Models.Individual, UniquePersonService.V1.Models.Individual>();
                cfg.CreateMap<EntryType, EntryType>();
                cfg.CreateMap<CostCenter, CostCenter>();
                cfg.CreateMap<Analysis, Analysis>();
                cfg.CreateMap<PostDated, PostDated>();
            });
            return config;
        }

        public static JournalEntryItem ToModel(this JournalEntryItemParameter journalEntryItemParameter)
        {
            var config = CreateMapJournalEntryItemsParameter();
            return config.Map<JournalEntryItemParameter, JournalEntryItem>(journalEntryItemParameter);
        }

        public static IEnumerable<JournalEntryItem> ToModels(this IEnumerable<JournalEntryItemParameter> journalEntryItemParameters)
        {
            return journalEntryItemParameters.Select(ToModel);
        }



        public static IMapper CreateMapJournalParameter()
        {
            var config = MapperCache.GetMapper<JournalParameterDTO, JournalParameter>(cfg =>
            {
                cfg.CreateMap<JournalParameterDTO, JournalParameter>();

                cfg.CreateMap<JournalEntryDTO, JournalEntry>();
                cfg.CreateMap<AccountingCompanyDTO, AccountingCompany>();
                cfg.CreateMap<AccountingMovementTypeDTO, AccountingMovementType>();
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<SalePointDTO, SalePoint>();
                cfg.CreateMap<JournalEntryItemDTO, JournalEntryItem>()
                        .ForMember(dest => dest.AccountingNature, opt => opt.MapFrom(src => (AccountingNatures)src.AccountingNature));
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<ExchangeRateDTO, ExchangeRate>();
                cfg.CreateMap<AccountingAccountDTO, AccountingAccount>();
                cfg.CreateMap<PaymentConceptDTO, PaymentConcept>();
                cfg.CreateMap<CostCenterDTO, CostCenter>();
                cfg.CreateMap<CostCenterTypeDTO, CostCenterType>();
                cfg.CreateMap<AnalysisDTO, Analysis>()
                .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.ConceptKey));
                cfg.CreateMap<AnalysisConceptDTO, AnalysisConcept>();
                cfg.CreateMap<ReconciliationMovementTypeDTO, ReconciliationMovementType>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<IndividualDTO, UniquePersonService.V1.Models.Individual>();
                cfg.CreateMap<EntryTypeDTO, EntryType>();
                cfg.CreateMap<EntryTypeItemDTO, EntryTypeItem>();
                cfg.CreateMap<AccountingMovementTypeDTO, AccountingMovementType>();
                cfg.CreateMap<AccountingConceptDTO, AccountingConcept>();
                cfg.CreateMap<ReceiptDTO, Receipt>();
                cfg.CreateMap<PostDatedDTO, PostDated>();
                cfg.CreateMap<OriginalGeneralLedgerDTO, OriginalGeneralLedger>();
            });
            return config;
        }

        public static JournalParameter ToModel(this JournalParameterDTO journalParameterDTO)
        {
            var config = CreateMapJournalParameter();
            return config.Map<JournalParameterDTO, JournalParameter>(journalParameterDTO);
        }

        public static IEnumerable<JournalParameter> ToModels(this IEnumerable<JournalParameterDTO> journalParameterDTOs)
        {
            return journalParameterDTOs.Select(ToModel);
        }



        public static IMapper CreateMapJournalEntryReversionParameters()
        {
            var config = MapperCache.GetMapper<JournalEntryReversionParametersDTO, JournalEntryReversionParameters>(cfg =>
            {
                cfg.CreateMap<JournalEntryReversionParametersDTO, JournalEntryReversionParameters>();
            });
            return config;
        }

        public static JournalEntryReversionParameters ToModel(this JournalEntryReversionParametersDTO journalEntryReversionParametersDTO)
        {
            var config = CreateMapJournalEntryReversionParameters();
            return config.Map<JournalEntryReversionParametersDTO, JournalEntryReversionParameters>(journalEntryReversionParametersDTO);
        }
    }
}
