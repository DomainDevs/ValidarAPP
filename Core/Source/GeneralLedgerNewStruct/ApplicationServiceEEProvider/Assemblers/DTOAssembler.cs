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
using Sistran.Core.Application.Utilities.Cache;
using INTDTO = Sistran.Core.Integration.GeneralLedgerServices.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Core.Application.GeneralLedgerServices.Assemblers
{
    internal static class DTOAssembler
    {

        public static IMapper CreateMapAccountingAccounts()
        {
            var config = MapperCache.GetMapper<AccountingAccount, AccountingAccountDTO>(cfg =>
            {
                cfg.CreateMap<AccountingAccount, AccountingAccountDTO>()
                        .ForMember(dest => dest.AccountingNature, opt => opt.MapFrom(src => (int)src.AccountingNature))
                        .ForMember(dest => dest.AccountingAccountApplication, opt => opt.MapFrom(src => (int)src.AccountingAccountApplication));
                cfg.CreateMap<AccountingAccountType, AccountingAccountTypeDTO>();
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<Analysis, AnalysisDTO>()
                .ForMember(dest => dest.ConceptKey, opt => opt.MapFrom(src => src.Key));
                cfg.CreateMap<CostCenter, CostCenterDTO>();
                cfg.CreateMap<PaymentConcept, PaymentConceptDTO>();
                cfg.CreateMap<AccountingAccount, AccountingAccountDTO>();
                cfg.CreateMap<Prefix, PrefixDTO>();
            });
            return config;
        }

        public static AccountingAccountDTO ToDTO(this AccountingAccount accountingAccount)
        {
            var config = CreateMapAccountingAccounts();
            return config.Map<AccountingAccount, AccountingAccountDTO>(accountingAccount);
        }

        public static IEnumerable<AccountingAccountDTO> ToDTOs(this IEnumerable<AccountingAccount> accountingAccount)
        {
            return accountingAccount.Select(ToDTO);
        }

        public static IMapper CreateMapAccountingAccountTypes()
        {
            var config = MapperCache.GetMapper<AccountingAccountType, AccountingAccountTypeDTO>(cfg =>
            {
                cfg.CreateMap<AccountingAccountType, AccountingAccountTypeDTO>();
            });
            return config;
        }

        public static AccountingAccountTypeDTO ToDTO(this AccountingAccountType accountingAccountType)
        {
            var config = CreateMapAccountingAccountTypes();
            return config.Map<AccountingAccountType, AccountingAccountTypeDTO>(accountingAccountType);
        }

        public static IEnumerable<AccountingAccountTypeDTO> ToDTOs(this IEnumerable<AccountingAccountType> accountingAccountType)
        {
            return accountingAccountType.Select(ToDTO);
        }

        public static IMapper CreateMapAccountingCompanies()
        {
            var config = MapperCache.GetMapper<AccountingCompany, AccountingCompanyDTO>(cfg =>
            {
                cfg.CreateMap<AccountingCompany, AccountingCompanyDTO>();
            });
            return config;
        }

        public static AccountingCompanyDTO ToDTO(this AccountingCompany accountingCompany)
        {
            var config = CreateMapAccountingCompanies();
            return config.Map<AccountingCompany, AccountingCompanyDTO>(accountingCompany);
        }

        public static IEnumerable<AccountingCompanyDTO> ToDTOs(this IEnumerable<AccountingCompany> accountingCompany)
        {
            return accountingCompany.Select(ToDTO);
        }

        public static IMapper CreateMapAccountingMovementTypes()
        {
            var config = MapperCache.GetMapper<AccountingMovementType, AccountingMovementTypeDTO>(cfg =>
            {
                cfg.CreateMap<AccountingMovementType, AccountingMovementTypeDTO>();
            });
            return config;
        }

        public static AccountingMovementTypeDTO ToDTO(this AccountingMovementType accountingMovementType)
        {
            var config = CreateMapAccountingMovementTypes();
            return config.Map<AccountingMovementType, AccountingMovementTypeDTO>(accountingMovementType);
        }

        public static IEnumerable<AccountingMovementTypeDTO> ToDTOs(this IEnumerable<AccountingMovementType> accountingMovementType)
        {
            return accountingMovementType.Select(ToDTO);
        }

        public static IMapper CreateMapAnalysiss()
        {
            var config = MapperCache.GetMapper<Analysis, AnalysisDTO>(cfg =>
            {
                cfg.CreateMap<Analysis, AnalysisDTO>()
                .ForMember(dest => dest.ConceptKey, opt => opt.MapFrom(src => src.Key));
                cfg.CreateMap<AnalysisConcept, AnalysisConceptDTO>();
            });
            return config;
        }

        public static AnalysisDTO ToDTO(this Analysis analysis)
        {
            var config = CreateMapAnalysiss();
            return config.Map<Analysis, AnalysisDTO>(analysis);
        }

        public static IEnumerable<AnalysisDTO> ToDTOs(this IEnumerable<Analysis> analysis)
        {
            return analysis.Select(ToDTO);
        }

        public static IMapper CreateMapAnalysisCodes()
        {
            var config = MapperCache.GetMapper<AnalysisCode, AnalysisCodeDTO>(cfg =>
            {
                cfg.CreateMap<AnalysisCode, AnalysisCodeDTO>()
                        .ForMember(dest => dest.AccountingNature, opt => opt.MapFrom(src => (int)src.AccountingNature));
                cfg.CreateMap<AnalysisConcept, AnalysisConceptDTO>();
            });
            return config;
        }

        public static AnalysisCodeDTO ToDTO(this AnalysisCode analysisCode)
        {
            var config = CreateMapAnalysisCodes();
            return config.Map<AnalysisCode, AnalysisCodeDTO>(analysisCode);
        }

        public static IEnumerable<AnalysisCodeDTO> ToDTOs(this IEnumerable<AnalysisCode> analysisCode)
        {
            return analysisCode.Select(ToDTO);
        }

        public static IMapper CreateMapAnalysisConcepts()
        {
            var config = MapperCache.GetMapper<AnalysisConcept, AnalysisConceptDTO>(cfg =>
            {
                cfg.CreateMap<AnalysisConcept, AnalysisConceptDTO>();
                cfg.CreateMap<AnalysisCode, AnalysisCodeDTO>();
                cfg.CreateMap<AnalysisTreatment, AnalysisTreatmentDTO>();
            });
            return config;
        }

        public static AnalysisConceptDTO ToDTO(this AnalysisConcept analysisConcept)
        {
            var config = CreateMapAnalysisConcepts();
            return config.Map<AnalysisConcept, AnalysisConceptDTO>(analysisConcept);
        }

        public static IEnumerable<AnalysisConceptDTO> ToDTOs(this IEnumerable<AnalysisConcept> analysisConcept)
        {
            return analysisConcept.Select(ToDTO);
        }

        public static IMapper CreateMapAnalysisConceptKeies()
        {
            var config = MapperCache.GetMapper<AnalysisConceptKey, AnalysisConceptKeyDTO>(cfg =>
            {
                cfg.CreateMap<AnalysisConceptKey, AnalysisConceptKeyDTO>();
                cfg.CreateMap<AnalysisConcept, AnalysisConceptDTO>();
            });
            return config;
        }

        public static AnalysisConceptKeyDTO ToDTO(this AnalysisConceptKey analysisConceptKey)
        {
            var config = CreateMapAnalysisConceptKeies();
            return config.Map<AnalysisConceptKey, AnalysisConceptKeyDTO>(analysisConceptKey);
        }

        public static IEnumerable<AnalysisConceptKeyDTO> ToDTOs(this IEnumerable<AnalysisConceptKey> analysisConceptKey)
        {
            return analysisConceptKey.Select(ToDTO);
        }

        public static IMapper CreateMapAnalysisTreatments()
        {
            var config = MapperCache.GetMapper<AnalysisTreatment, AnalysisTreatmentDTO>(cfg =>
            {
                cfg.CreateMap<AnalysisTreatment, AnalysisTreatmentDTO>();
            });
            return config;
        }

        public static AnalysisTreatmentDTO ToDTO(this AnalysisTreatment analysisTreatment)
        {
            var config = CreateMapAnalysisTreatments();
            return config.Map<AnalysisTreatment, AnalysisTreatmentDTO>(analysisTreatment);
        }

        public static IEnumerable<AnalysisTreatmentDTO> ToDTOs(this IEnumerable<AnalysisTreatment> analysisTreatment)
        {
            return analysisTreatment.Select(ToDTO);
        }

        public static IMapper CreateMapCostCenters()
        {
            var config = MapperCache.GetMapper<CostCenter, CostCenterDTO>(cfg =>
            {
                cfg.CreateMap<CostCenter, CostCenterDTO>();
                cfg.CreateMap<CostCenterType, CostCenterTypeDTO>();
            });
            return config;
        }

        public static CostCenterDTO ToDTO(this CostCenter costCenter)
        {
            var config = CreateMapCostCenters();
            return config.Map<CostCenter, CostCenterDTO>(costCenter);
        }

        public static IEnumerable<CostCenterDTO> ToDTOs(this IEnumerable<CostCenter> costCenter)
        {
            return costCenter.Select(ToDTO);
        }

        public static IMapper CreateMapCostCenterTypes()
        {
            var config = MapperCache.GetMapper<CostCenterType, CostCenterTypeDTO>(cfg =>
            {
                cfg.CreateMap<CostCenterType, CostCenterTypeDTO>();
            });
            return config;
        }

        public static CostCenterTypeDTO ToDTO(this CostCenterType costCenterType)
        {
            var config = CreateMapCostCenterTypes();
            return config.Map<CostCenterType, CostCenterTypeDTO>(costCenterType);
        }

        public static IEnumerable<CostCenterTypeDTO> ToDTOs(this IEnumerable<CostCenterType> costCenterType)
        {
            return costCenterType.Select(ToDTO);
        }

        public static IMapper CreateMapEntryDestinations()
        {
            var config = MapperCache.GetMapper<EntryDestination, EntryDestinationDTO>(cfg =>
            {
                cfg.CreateMap<EntryDestination, EntryDestinationDTO>();
            });
            return config;
        }

        public static EntryDestinationDTO ToDTO(this EntryDestination entryDestination)
        {
            var config = CreateMapEntryDestinations();
            return config.Map<EntryDestination, EntryDestinationDTO>(entryDestination);
        }

        public static IEnumerable<EntryDestinationDTO> ToDTOs(this IEnumerable<EntryDestination> entryDestination)
        {
            return entryDestination.Select(ToDTO);
        }

        public static IMapper CreateMapEntryNumbers()
        {
            var config = MapperCache.GetMapper<EntryNumber, EntryNumberDTO>(cfg =>
            {
                cfg.CreateMap<EntryNumber, EntryNumberDTO>();
                cfg.CreateMap<AccountingMovementType, AccountingMovementTypeDTO>();
                cfg.CreateMap<EntryDestination, EntryDestinationDTO>();
            });
            return config;
        }

        public static EntryNumberDTO ToDTO(this EntryNumber entryNumber)
        {
            var config = CreateMapEntryNumbers();
            return config.Map<EntryNumber, EntryNumberDTO>(entryNumber);
        }

        public static IEnumerable<EntryNumberDTO> ToDTOs(this IEnumerable<EntryNumber> entryNumber)
        {
            return entryNumber.Select(ToDTO);
        }

        public static IMapper CreateMapEntryTypes()
        {
            var config = MapperCache.GetMapper<EntryType, EntryTypeDTO>(cfg =>
            {
                cfg.CreateMap<EntryType, EntryTypeDTO>();
                cfg.CreateMap<EntryTypeItem, EntryTypeItemDTO>();
                cfg.CreateMap<AccountingAccount, AccountingAccountDTO>();
            });
            return config;
        }

        public static EntryTypeDTO ToDTO(this EntryType entryType)
        {
            var config = CreateMapEntryTypes();
            return config.Map<EntryType, EntryTypeDTO>(entryType);
        }

        public static IEnumerable<EntryTypeDTO> ToDTOs(this IEnumerable<EntryType> entryType)
        {
            return entryType.Select(ToDTO);
        }

        public static IMapper CreateMapEntryTypeItems()
        {
            var config = MapperCache.GetMapper<EntryTypeItem, EntryTypeItemDTO>(cfg =>
            {
                cfg.CreateMap<EntryTypeItem, EntryTypeItemDTO>()
                        .ForMember(dest => dest.AccountingNature, opt => opt.MapFrom(src => (int)src.AccountingNature));
                cfg.CreateMap<AccountingMovementType, AccountingMovementTypeDTO>();
                cfg.CreateMap<AccountingAccount, AccountingAccountDTO>();
                cfg.CreateMap<AccountingConcept, AccountingConceptDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<Analysis, AnalysisDTO>()
                .ForMember(dest => dest.ConceptKey, opt => opt.MapFrom(src => src.Key));
                cfg.CreateMap<CostCenter, CostCenterDTO>();
            });
            return config;
        }

        public static EntryTypeItemDTO ToDTO(this EntryTypeItem entryTypeItem)
        {
            var config = CreateMapEntryTypeItems();
            return config.Map<EntryTypeItem, EntryTypeItemDTO>(entryTypeItem);
        }

        public static IEnumerable<EntryTypeItemDTO> ToDTOs(this IEnumerable<EntryTypeItem> entryTypeItem)
        {
            return entryTypeItem.Select(ToDTO);
        }

        public static IMapper CreateMapJournalEntries()
        {
            var config = MapperCache.GetMapper<JournalEntry, JournalEntryDTO>(cfg =>
            {
                cfg.CreateMap<JournalEntry, JournalEntryDTO>();
                cfg.CreateMap<AccountingCompany, AccountingCompanyDTO>();
                cfg.CreateMap<AccountingMovementType, AccountingMovementTypeDTO>();
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
                cfg.CreateMap<JournalEntryItem, JournalEntryItemDTO>();
                cfg.CreateMap<UniquePersonService.V1.Models.Individual, IndividualDTO>()
                 .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName));
                cfg.CreateMap<EntryType, EntryTypeDTO>();
                cfg.CreateMap<EntryTypeItem, EntryTypeItemDTO>();
                cfg.CreateMap<AccountingAccount, AccountingAccountDTO>();
                cfg.CreateMap<CostCenter, CostCenterDTO>();
                cfg.CreateMap<CostCenterType, CostCenterTypeDTO>();
                cfg.CreateMap<Analysis, AnalysisDTO>()
                .ForMember(dest => dest.ConceptKey, opt => opt.MapFrom(src => src.Key));
                cfg.CreateMap<AnalysisConcept, AnalysisConceptDTO>();
                cfg.CreateMap<AnalysisCode, AnalysisCodeDTO>();
                cfg.CreateMap<AnalysisConceptKey, AnalysisConceptKeyDTO>();
                cfg.CreateMap<AnalysisTreatment, AnalysisTreatmentDTO>();
                cfg.CreateMap<PostDated, PostDatedDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
            });
            return config;
        }

        public static JournalEntryDTO ToDTO(this JournalEntry journalEntry)
        {
            var config = CreateMapJournalEntries();
            return config.Map<JournalEntry, JournalEntryDTO>(journalEntry);
        }

        public static IEnumerable<JournalEntryDTO> ToDTOs(this IEnumerable<JournalEntry> journalEntry)
        {
            return journalEntry.Select(ToDTO);
        }

        public static IMapper CreateMapJournalEntryItems()
        {
            var config = MapperCache.GetMapper<JournalEntryItem, JournalEntryItemDTO>(cfg =>
            {
                cfg.CreateMap<JournalEntryItem, JournalEntryItemDTO>()
                        .ForMember(dest => dest.AccountingNature, opt => opt.MapFrom(src => (int)src.AccountingNature));
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<ExchangeRate, ExchangeRateDTO>();
                cfg.CreateMap<AccountingAccount, AccountingAccountDTO>();
                cfg.CreateMap<ReconciliationMovementType, ReconciliationMovementTypeDTO>();
                cfg.CreateMap<Receipt, ReceiptDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<UniquePersonService.V1.Models.Individual, IndividualDTO>();
                cfg.CreateMap<EntryType, EntryTypeDTO>();
                cfg.CreateMap<CostCenter, CostCenterDTO>();
                cfg.CreateMap<Analysis, AnalysisDTO>()
                .ForMember(dest => dest.ConceptKey, opt => opt.MapFrom(src => src.Key));
                cfg.CreateMap<PostDated, PostDatedDTO>();
                cfg.CreateMap<PaymentConcept, PaymentConceptDTO>();
                cfg.CreateMap<EntryTypeItem, EntryTypeItemDTO>();
                cfg.CreateMap<CostCenterType, CostCenterTypeDTO>();
                cfg.CreateMap<AnalysisConcept, AnalysisConceptDTO>();
                cfg.CreateMap<AnalysisCode, AnalysisCodeDTO>();
                cfg.CreateMap<AnalysisTreatment, AnalysisTreatmentDTO>();
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<AccountingAccountType, AccountingAccountTypeDTO>();
                cfg.CreateMap<Prefix, PrefixDTO>();
            });
            return config;
        }

        public static JournalEntryItemDTO ToDTO(this JournalEntryItem journalEntryItem)
        {
            var config = CreateMapJournalEntryItems();
            return config.Map<JournalEntryItem, JournalEntryItemDTO>(journalEntryItem);
        }

        public static IEnumerable<JournalEntryItemDTO> ToDTOs(this IEnumerable<JournalEntryItem> journalEntryItem)
        {
            return journalEntryItem.Select(ToDTO);
        }

        public static IMapper CreateMapLedgerEntries()
        {
            var config = MapperCache.GetMapper<LedgerEntry, LedgerEntryDTO>(cfg =>
            {
                cfg.CreateMap<LedgerEntry, LedgerEntryDTO>();
                cfg.CreateMap<AccountingCompany, AccountingCompanyDTO>();
                cfg.CreateMap<AccountingMovementType, AccountingMovementTypeDTO>();
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<SalePoint, SalePointDTO>();
                cfg.CreateMap<EntryDestination, EntryDestinationDTO>();
                cfg.CreateMap<LedgerEntryItem, LedgerEntryItemDTO>();
            });
            return config;
        }

        public static LedgerEntryDTO ToDTO(this LedgerEntry ledgerEntry)
        {
            var config = CreateMapLedgerEntries();
            return config.Map<LedgerEntry, LedgerEntryDTO>(ledgerEntry);
        }

        public static IEnumerable<LedgerEntryDTO> ToDTOs(this IEnumerable<LedgerEntry> ledgerEntry)
        {
            return ledgerEntry.Select(ToDTO);
        }

        public static IMapper CreateMapLedgerEntryItems()
        {
            var config = MapperCache.GetMapper<LedgerEntryItem, LedgerEntryItemDTO>(cfg =>
            {
                cfg.CreateMap<LedgerEntryItem, LedgerEntryItemDTO>()
                        .ForMember(dest => dest.AccountingNature, opt => opt.MapFrom(src => (int)src.AccountingNature));
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<ExchangeRate, ExchangeRateDTO>();
                cfg.CreateMap<AccountingAccount, AccountingAccountDTO>();
                cfg.CreateMap<ReconciliationMovementType, ReconciliationMovementTypeDTO>();
                cfg.CreateMap<Receipt, ReceiptDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                //cfg.CreateMap<Individual, IndividualDTO>();
                cfg.CreateMap<EntryType, EntryTypeDTO>();
                cfg.CreateMap<CostCenter, CostCenterDTO>();
                cfg.CreateMap<Analysis, AnalysisDTO>()
                .ForMember(dest => dest.ConceptKey, opt => opt.MapFrom(src => src.Key));
                cfg.CreateMap<PostDated, PostDatedDTO>();
            });
            return config;
        }

        public static LedgerEntryItemDTO ToDTO(this LedgerEntryItem ledgerEntryItem)
        {
            var config = CreateMapLedgerEntryItems();
            return config.Map<LedgerEntryItem, LedgerEntryItemDTO>(ledgerEntryItem);
        }

        public static IEnumerable<LedgerEntryItemDTO> ToDTOs(this IEnumerable<LedgerEntryItem> ledgerEntryItem)
        {
            return ledgerEntryItem.Select(ToDTO);
        }

        public static IMapper CreateMapPostDateds()
        {
            var config = MapperCache.GetMapper<PostDated, PostDatedDTO>(cfg =>
            {
                cfg.CreateMap<PostDated, PostDatedDTO>()
                        .ForMember(dest => dest.PostDateType, opt => opt.MapFrom(src => (int)src.PostDateType));
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<ExchangeRate, ExchangeRateDTO>();
            });
            return config;
        }

        public static PostDatedDTO ToDTO(this PostDated postDated)
        {
            var config = CreateMapPostDateds();
            return config.Map<PostDated, PostDatedDTO>(postDated);
        }

        public static IEnumerable<PostDatedDTO> ToDTOs(this IEnumerable<PostDated> postDated)
        {
            return postDated.Select(ToDTO);
        }

        public static IMapper CreateMapProcessLogs()
        {
            var config = MapperCache.GetMapper<ProcessLog, ProcessLogDTO>(cfg =>
            {
                cfg.CreateMap<ProcessLog, ProcessLogDTO>()
                        .ForMember(dest => dest.ProcessLogStatus, opt => opt.MapFrom(src => (int)src.ProcessLogStatus));
            });
            return config;
        }

        public static ProcessLogDTO ToDTO(this ProcessLog processLog)
        {
            var config = CreateMapProcessLogs();
            return config.Map<ProcessLog, ProcessLogDTO>(processLog);
        }

        public static IEnumerable<ProcessLogDTO> ToDTOs(this IEnumerable<ProcessLog> processLog)
        {
            return processLog.Select(ToDTO);
        }

        public static IMapper CreateMapReceipts()
        {
            var config = MapperCache.GetMapper<Receipt, ReceiptDTO>(cfg =>
            {
                cfg.CreateMap<Receipt, ReceiptDTO>();
            });
            return config;
        }

        public static ReceiptDTO ToDTO(this Receipt receipt)
        {
            var config = CreateMapReceipts();
            return config.Map<Receipt, ReceiptDTO>(receipt);
        }

        public static IEnumerable<ReceiptDTO> ToDTOs(this IEnumerable<Receipt> receipt)
        {
            return receipt.Select(ToDTO);
        }

        public static IMapper CreateMapReconciliationMovementTypes()
        {
            var config = MapperCache.GetMapper<ReconciliationMovementType, ReconciliationMovementTypeDTO>(cfg =>
            {
                cfg.CreateMap<ReconciliationMovementType, ReconciliationMovementTypeDTO>()
                        .ForMember(dest => dest.AccountingNature, opt => opt.MapFrom(src => (int)src.AccountingNature));
            });
            return config;
        }

        public static ReconciliationMovementTypeDTO ToDTO(this ReconciliationMovementType reconciliationMovementType)
        {
            var config = CreateMapReconciliationMovementTypes();
            return config.Map<ReconciliationMovementType, ReconciliationMovementTypeDTO>(reconciliationMovementType);
        }

        public static IEnumerable<ReconciliationMovementTypeDTO> ToDTOs(this IEnumerable<ReconciliationMovementType> reconciliationMovementType)
        {
            return reconciliationMovementType.Select(ToDTO);
        }

        public static IMapper CreateMapAccountReclassifications()
        {
            var config = MapperCache.GetMapper<AccountReclassification, AccountReclassificationDTO>(cfg =>
            {
                cfg.CreateMap<AccountReclassification, AccountReclassificationDTO>();
                cfg.CreateMap<AccountingAccount, AccountingAccountDTO>();
            });
            return config;
        }

        public static AccountReclassificationDTO ToDTO(this AccountReclassification accountReclassification)
        {
            var config = CreateMapAccountReclassifications();
            return config.Map<AccountReclassification, AccountReclassificationDTO>(accountReclassification);
        }

        public static IEnumerable<AccountReclassificationDTO> ToDTOs(this IEnumerable<AccountReclassification> accountReclassification)
        {
            return accountReclassification.Select(ToDTO);
        }

        public static IMapper CreateMapAccountReclassificationResults()
        {
            var config = MapperCache.GetMapper<AccountReclassificationResult, AccountReclassificationResultDTO>(cfg =>
            {
                cfg.CreateMap<AccountReclassificationResult, AccountReclassificationResultDTO>()
                        .ForMember(dest => dest.AccountingNature, opt => opt.MapFrom(src => (int)src.AccountingNature));
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<ExchangeRate, ExchangeRateDTO>();
                cfg.CreateMap<AccountingAccount, AccountingAccountDTO>();
                cfg.CreateMap<Analysis, AnalysisDTO>()
                .ForMember(dest => dest.ConceptKey, opt => opt.MapFrom(src => src.Key));
                cfg.CreateMap<CostCenter, CostCenterDTO>();
            });
            return config;
        }

        public static AccountReclassificationResultDTO ToDTO(this AccountReclassificationResult accountReclassificationResult)
        {
            var config = CreateMapAccountReclassificationResults();
            return config.Map<AccountReclassificationResult, AccountReclassificationResultDTO>(accountReclassificationResult);
        }

        public static IEnumerable<AccountReclassificationResultDTO> ToDTOs(this IEnumerable<AccountReclassificationResult> accountReclassificationResult)
        {
            return accountReclassificationResult.Select(ToDTO);
        }

        public static IMapper CreateMapAccountingAccountMasks()
        {
            var config = MapperCache.GetMapper<AccountingAccountMask, AccountingAccountMaskDTO>(cfg =>
            {
                cfg.CreateMap<AccountingAccountMask, AccountingAccountMaskDTO>();
                cfg.CreateMap<CommonService.Models.Parameter, ParameterDTO>();
            });
            return config;
        }

        public static AccountingAccountMaskDTO ToDTO(this AccountingAccountMask accountingAccountMask)
        {
            var config = CreateMapAccountingAccountMasks();
            return config.Map<AccountingAccountMask, AccountingAccountMaskDTO>(accountingAccountMask);
        }

        public static IEnumerable<AccountingAccountMaskDTO> ToDTOs(this IEnumerable<AccountingAccountMask> accountingAccountMask)
        {
            return accountingAccountMask.Select(ToDTO);
        }
        public static IMapper CreateMapAccountingParametersRules()
        {
            var config = MapperCache.GetMapper<EEProvider.Models.AccountingRules.Parameter, ParameterDTO>(cfg =>
            {
                cfg.CreateMap<EEProvider.Models.AccountingRules.Parameter, ParameterDTO>();
            });
            return config;
        }

        public static ParameterDTO ToDTO(this EEProvider.Models.AccountingRules.Parameter parameter)
        {
            var config = CreateMapAccountingAccountMasks();
            return config.Map<EEProvider.Models.AccountingRules.Parameter, ParameterDTO>(parameter);
        }

        public static IEnumerable<ParameterDTO> ToDTOs(this IEnumerable<EEProvider.Models.AccountingRules.Parameter> parameters)
        {
            return parameters.Select(ToDTO);
        }


        public static IMapper CreateMapAccountingRules()
        {
            var config = MapperCache.GetMapper<AccountingRule, AccountingRuleDTO>(cfg =>
            {
                cfg.CreateMap<AccountingRule, AccountingRuleDTO>();
                cfg.CreateMap<Condition, ConditionDTO>();
                cfg.CreateMap<EEProvider.Models.AccountingRules.Parameter, ParameterDTO>();
            });
            return config;
        }

        public static AccountingRuleDTO ToDTO(this AccountingRule accountingRule)
        {
            var config = CreateMapAccountingRules();
            return config.Map<AccountingRule, AccountingRuleDTO>(accountingRule);
        }

        public static IEnumerable<AccountingRuleDTO> ToDTOs(this IEnumerable<AccountingRule> accountingRule)
        {
            return accountingRule.Select(ToDTO);
        }

        public static IMapper CreateMapAccountingRulePackages()
        {
            var config = MapperCache.GetMapper<AccountingRulePackage, AccountingRulePackageDTO>(cfg =>
            {
                cfg.CreateMap<AccountingRulePackage, AccountingRulePackageDTO>();
                cfg.CreateMap<AccountingRule, AccountingRuleDTO>();
            });
            return config;
        }

        public static AccountingRulePackageDTO ToDTO(this AccountingRulePackage accountingRulePackage)
        {
            var config = CreateMapAccountingRulePackages();
            return config.Map<AccountingRulePackage, AccountingRulePackageDTO>(accountingRulePackage);
        }

        public static IEnumerable<AccountingRulePackageDTO> ToDTOs(this IEnumerable<AccountingRulePackage> accountingRulePackage)
        {
            return accountingRulePackage.Select(ToDTO);
        }

        public static IMapper CreateMapConditions()
        {
            var config = MapperCache.GetMapper<Condition, ConditionDTO>(cfg =>
            {
                cfg.CreateMap<Condition, ConditionDTO>();
                cfg.CreateMap<AccountingRule, AccountingRuleDTO>();
                cfg.CreateMap<CommonService.Models.Parameter, ParameterDTO>();
            });
            return config;
        }

        public static ConditionDTO ToDTO(this Condition condition)
        {
            var config = CreateMapConditions();
            return config.Map<Condition, ConditionDTO>(condition);
        }

        public static IEnumerable<ConditionDTO> ToDTOs(this IEnumerable<Condition> condition)
        {
            return condition.Select(ToDTO);
        }

        public static IMapper CreateMapParameters()
        {
            var config = MapperCache.GetMapper<CommonService.Models.Parameter, ParameterDTO>(cfg =>
            {
                cfg.CreateMap<CommonService.Models.Parameter, ParameterDTO>();
            });
            return config;
        }

        public static ParameterDTO ToDTO(this CommonService.Models.Parameter parameter)
        {
            var config = CreateMapParameters();
            return config.Map<CommonService.Models.Parameter, ParameterDTO>(parameter);
        }

        public static IEnumerable<ParameterDTO> ToDTOs(this IEnumerable<CommonService.Models.Parameter> parameter)
        {
            return parameter.Select(ToDTO);
        }

        public static IMapper CreateMapResults()
        {
            var config = MapperCache.GetMapper<Result, ResultDTO>(cfg =>
            {
                cfg.CreateMap<Result, ResultDTO>()
                        .ForMember(dest => dest.AccountingNature, opt => opt.MapFrom(src => (int)src.AccountingNature));
                cfg.CreateMap<AccountingAccountMask, AccountingAccountMaskDTO>();
                cfg.CreateMap<CommonService.Models.Parameter, ParameterDTO>();
            });
            return config;
        }

        public static ResultDTO ToDTO(this Result result)
        {
            var config = CreateMapResults();
            return config.Map<Result, ResultDTO>(result);
        }

        public static IEnumerable<ResultDTO> ToDTOs(this IEnumerable<Result> result)
        {
            return result.Select(ToDTO);
        }

        public static IMapper CreateMapAccountingConcepts()
        {
            var config = MapperCache.GetMapper<AccountingConcept, AccountingConceptDTO>(cfg =>
            {
                cfg.CreateMap<AccountingConcept, AccountingConceptDTO>();
                cfg.CreateMap<AccountingAccount, AccountingAccountDTO>();
                cfg.CreateMap<Analysis, AnalysisDTO>()
                .ForMember(dest => dest.ConceptKey, opt => opt.MapFrom(src => src.Key)); 
            });
            return config;
        }

        public static AccountingConceptDTO ToDTO(this AccountingConcept accountingConcept)
        {
            var config = CreateMapAccountingConcepts();
            return config.Map<AccountingConcept, AccountingConceptDTO>(accountingConcept);
        }

        public static IEnumerable<AccountingConceptDTO> ToDTOs(this IEnumerable<AccountingConcept> accountingConcept)
        {
            return accountingConcept.Select(ToDTO);
        }
        
        public static IMapper CreateMapBranchAccountingConcepts()
        {
            var config = MapperCache.GetMapper<BranchAccountingConcept, BranchAccountingConceptDTO>(cfg =>
            {
                cfg.CreateMap<BranchAccountingConcept, BranchAccountingConceptDTO>();
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<AccountingConcept, AccountingConceptDTO>();
                cfg.CreateMap<MovementType, MovementTypeDTO>();
            });
            return config;
        }

        public static BranchAccountingConceptDTO ToDTO(this BranchAccountingConcept branchAccountingConcept)
        {
            var config = CreateMapBranchAccountingConcepts();
            return config.Map<BranchAccountingConcept, BranchAccountingConceptDTO>(branchAccountingConcept);
        }

        public static IEnumerable<BranchAccountingConceptDTO> ToDTOs(this IEnumerable<BranchAccountingConcept> branchAccountingConcept)
        {
            return branchAccountingConcept.Select(ToDTO);
        }

        public static IMapper CreateMapConceptSources()
        {
            var config = MapperCache.GetMapper<ConceptSource, ConceptSourceDTO>(cfg =>
            {
                cfg.CreateMap<ConceptSource, ConceptSourceDTO>();
            });
            return config;
        }

        public static ConceptSourceDTO ToDTO(this ConceptSource conceptSource)
        {
            var config = CreateMapConceptSources();
            return config.Map<ConceptSource, ConceptSourceDTO>(conceptSource);
        }
        
        public static IEnumerable<ConceptSourceDTO> ToDTOs(this IEnumerable<ConceptSource> conceptSource)
        {
            return conceptSource.Select(ToDTO);
        }

        public static IMapper CreateMapMovementTypes()
        {
            var config = MapperCache.GetMapper<MovementType, MovementTypeDTO>(cfg =>
            {
                cfg.CreateMap<MovementType, MovementTypeDTO>();
                cfg.CreateMap<ConceptSource, ConceptSourceDTO>();
            });
            return config;
        }

        public static MovementTypeDTO ToDTO(this MovementType movementType)
        {
            var config = CreateMapMovementTypes();
            return config.Map<MovementType, MovementTypeDTO>(movementType);
        }

        public static IEnumerable<MovementTypeDTO> ToDTOs(this IEnumerable<MovementType> movementType)
        {
            return movementType.Select(ToDTO);
        }

        public static IMapper CreateMapUserBranchAccountingConcepts()
        {
            var config = MapperCache.GetMapper<UserBranchAccountingConcept, UserBranchAccountingConceptDTO>(cfg =>
            {
                cfg.CreateMap<UserBranchAccountingConcept, UserBranchAccountingConceptDTO>();
                cfg.CreateMap<BranchAccountingConcept, BranchAccountingConceptDTO>();
            });
            return config;
        }

        public static UserBranchAccountingConceptDTO ToDTO(this UserBranchAccountingConcept userBranchAccountingConcept)
        {
            var config = CreateMapUserBranchAccountingConcepts();
            return config.Map<UserBranchAccountingConcept, UserBranchAccountingConceptDTO>(userBranchAccountingConcept);
        }

        public static IEnumerable<UserBranchAccountingConceptDTO> ToDTOs(this IEnumerable<UserBranchAccountingConcept> userBranchAccountingConcept)
        {
            return userBranchAccountingConcept.Select(ToDTO);
        }

        #region Integration

        public static IMapper CreateMapIntAccountingConcepts()
        {
            var config = MapperCache.GetMapper<AccountingConcept, INTDTO.AccountingConcepts.AccountingConceptDTO>(cfg =>
            {
                cfg.CreateMap<AccountingConcept, INTDTO.AccountingConcepts.AccountingConceptDTO>()
                    .ForMember(dest => dest.AccountingAccountId, opt => opt.MapFrom(src => src.AccountingAccount.AccountingAccountId));
            });
            return config;
        }

        public static INTDTO.AccountingConcepts.AccountingConceptDTO ToDTOInt(this AccountingConcept accountingConcept)
        {
            var config = CreateMapIntAccountingConcepts();
            return config.Map<AccountingConcept, INTDTO.AccountingConcepts.AccountingConceptDTO>(accountingConcept);
        }

        public static IEnumerable<INTDTO.AccountingConcepts.AccountingConceptDTO> ToDTOsInt(this IEnumerable<AccountingConcept> accountingConcept)
        {
            return accountingConcept.Select(ToDTOInt);
        }

        public static IMapper CreateMapIntJournalEntry()
        {
            var config = MapperCache.GetMapper<JournalEntry, INTDTO.JournalEntryDTO>(cfg =>
            {
                cfg.CreateMap<JournalEntry, INTDTO.JournalEntryDTO>();
                cfg.CreateMap<AccountingCompany, INTDTO.AccountingCompanyDTO>();
                cfg.CreateMap<AccountingMovementType, INTDTO.AccountingMovementTypeDTO>();
                cfg.CreateMap<Branch, INTDTO.BranchDTO>();
                cfg.CreateMap<SalePoint, INTDTO.SalePointDTO>();
                cfg.CreateMap<JournalEntryItem, INTDTO.JournalEntryItemDTO>();
                cfg.CreateMap<UniquePersonService.V1.Models.Individual, INTDTO.IndividualDTO>()
                 .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName));
                cfg.CreateMap<EntryType, INTDTO.EntryTypeDTO>();
                cfg.CreateMap<EntryTypeItem, INTDTO.EntryTypeItemDTO>();
                cfg.CreateMap<AccountingAccount, INTDTO.AccountingAccountDTO>();
                cfg.CreateMap<CostCenter, INTDTO.CostCenterDTO>()
                    .ForMember(dest => dest.CostCenterTypeId, opt => opt.MapFrom(scr => scr.CostCenterType.CostCenterTypeId));
                cfg.CreateMap<Analysis, INTDTO.AnalysisDTO>();
                cfg.CreateMap<AnalysisConcept, INTDTO.AnalysisConceptDTO>()
                    .ForMember(dest => dest.AnalysisCode, opt => opt.MapFrom(src => src.AnalysisCode.AnalysisCodeId))
                    .ForMember(dest => dest.AnalysisTreatment, opt => opt.MapFrom(src => src.AnalysisTreatment.AnalysisTreatmentId));
                cfg.CreateMap<PostDated, INTDTO.PostDatedDTO>();
                cfg.CreateMap<Amount, INTDTO.AmountDTO>();
            });
            return config;
        }

        public static INTDTO.JournalEntryDTO ToDTOInt(this JournalEntry journalEntry)
        {
            var config = CreateMapIntJournalEntry();
            return config.Map<JournalEntry, INTDTO.JournalEntryDTO>(journalEntry);
        }

        public static IMapper CreateMapIntMovementTypes()
        {
            var config = MapperCache.GetMapper<MovementType, INTDTO.AccountingConcepts.MovementTypeDTO>(cfg =>
            {
                cfg.CreateMap<MovementType, INTDTO.AccountingConcepts.MovementTypeDTO>();
                cfg.CreateMap<ConceptSource, INTDTO.AccountingConcepts.ConceptSourceDTO>();
            });
            return config;
        }

        public static INTDTO.AccountingConcepts.MovementTypeDTO ToDTOInt(this MovementType movementType)
        {
            var config = CreateMapIntMovementTypes();
            return config.Map<MovementType, INTDTO.AccountingConcepts.MovementTypeDTO>(movementType);
        }

        public static IEnumerable<INTDTO.AccountingConcepts.MovementTypeDTO> ToDTOsInt(this IEnumerable<MovementType> movementType)
        {
            return movementType.Select(ToDTOInt);
        }

        public static IMapper CreateMapIntConceptSources()
        {
            var config = MapperCache.GetMapper<ConceptSource, INTDTO.AccountingConcepts.ConceptSourceDTO>(cfg =>
            {
                cfg.CreateMap<ConceptSource, INTDTO.AccountingConcepts.ConceptSourceDTO>();
            });
            return config;
        }

        public static INTDTO.AccountingConcepts.ConceptSourceDTO ToDTOInt(this ConceptSource conceptSource)
        {
            var config = CreateMapIntConceptSources();
            return config.Map<ConceptSource, INTDTO.AccountingConcepts.ConceptSourceDTO>(conceptSource);
        }

        public static IEnumerable<INTDTO.AccountingConcepts.ConceptSourceDTO> ToDTOsInt(this IEnumerable<ConceptSource> conceptSource)
        {
            return conceptSource.Select(ToDTOInt);
        }

        #endregion
    }
}
