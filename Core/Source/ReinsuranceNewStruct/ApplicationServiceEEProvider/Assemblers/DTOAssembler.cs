using System;
using Sistran.Core.Application.ReinsuranceServices.DTOs;
using Sistran.Core.Application.Utilities.Cache;
using System.Collections.Generic;
using System.Linq;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance;
using Sistran.Core.Application.ReinsuranceServices.Models.LineAssociationTypes;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.ReportingServices.Models;
using Sistran.Core.Application.UniquePerson.IntegrationService.Models.Base;
using TEMPINTDTO = Sistran.Core.Integration.TempCommonService.DTOs;
using REINSINTDTO = Sistran.Core.Integration.ReinsuranceIntegrationServices.DTOs.Reinsurance;
using ROQINTDTO = Sistran.Core.Integration.ReinsuranceOperatingQuotaServices.DTOs;
using ACCINTDTO = Sistran.Core.Integration.AccountingServices.DTOs.Accounting;
using UUINTDTO = Sistran.Core.Integration.UniqueUserServices.DTOs;
using COMMINTDTO = Sistran.Core.Integration.CommonServices.DTOs;
using UNDDTO = Sistran.Core.Integration.UndewritingIntegrationServices.DTOs;
using AutoMapper;

namespace Sistran.Core.Application.ReinsuranceServices.Assemblers
{
    internal static class DTOAssembler
    {
        internal static IMapper CreateMapAffectationTypes()
        {
            var config = MapperCache.GetMapper<AffectationType, AffectationTypeDTO>(cfg =>
            {
                cfg.CreateMap<AffectationType, AffectationTypeDTO>();
            });
            return config;
        }

        internal static AffectationTypeDTO ToDTO(this AffectationType affectationType)
        {
            var config = CreateMapAffectationTypes();
            return config.Map<AffectationType, AffectationTypeDTO>(affectationType);
        }

        internal static IEnumerable<AffectationTypeDTO> ToDTOs(this IEnumerable<AffectationType> affectationType)
        {
            return affectationType.Select(ToDTO);
        }

        internal static IMapper CreateMapPriorityRetention()
        {
            var config = MapperCache.GetMapper<PriorityRetention, PriorityRetentionDTO>(cfg =>
            {
                cfg.CreateMap<PriorityRetention, PriorityRetentionDTO>();
                cfg.CreateMap<Prefix, PrefixDTO>();
            });
            return config;
        }

        internal static PriorityRetentionDTO ToDTO(this PriorityRetention priorityRetention)
        {
            var config = CreateMapPriorityRetention();
            return config.Map<PriorityRetention, PriorityRetentionDTO>(priorityRetention);
        }

        internal static IEnumerable<PriorityRetentionDTO> ToDTOs(this IEnumerable<PriorityRetention> priorityRetention)
        {
            return priorityRetention.Select(ToDTO);
        }

        internal static IMapper CreateMapContracts()
        {
            var config = MapperCache.GetMapper<Contract, ContractDTO>(cfg =>
            {
                cfg.CreateMap<Contract, ContractDTO>();
                cfg.CreateMap<ResettlementType, ResettlementTypeDTO>();
                cfg.CreateMap<AffectationType, AffectationTypeDTO>();
                cfg.CreateMap<EPIType, EPITypeDTO>();
                cfg.CreateMap<ContractType, ContractTypeDTO>();
                cfg.CreateMap<ContractFunctionality, ContractFunctionalityDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<Level, LevelDTO>()
                   .ForMember(dest => dest.PremiumType, opt => opt.MapFrom(src => (int)src.PremiumType))
                   .ForMember(dest => dest.ApplyOnType, opt => opt.MapFrom(src => (int)src.ApplyOnType))
                   .ForMember(dest => dest.CalculationType, opt => opt.MapFrom(src => (int)src.CalculationType));
                cfg.CreateMap<LevelPayment, LevelPaymentDTO>();
                cfg.CreateMap<LevelCompany, LevelCompanyDTO>()
                        .ForMember(dest => dest.PresentationInformationType, opt => opt.MapFrom(src => (int)src.PresentationInformationType));
                cfg.CreateMap<LevelRestore, LevelRestoreDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<Agent, AgentDTO>();

            });
            return config;
        }

        internal static ContractDTO ToDTO(this Contract contract)
        {
            var config = CreateMapContracts();
            return config.Map<Contract, ContractDTO>(contract);
        }

        internal static IEnumerable<ContractDTO> ToDTOs(this IEnumerable<Contract> contract)
        {
            return contract.Select(ToDTO);
        }

        internal static IMapper CreateMapContractFunctionality()
        {
            var config = MapperCache.GetMapper<ContractFunctionality, ContractFunctionalityDTO>(cfg =>
            {
                cfg.CreateMap<ContractFunctionality, ContractFunctionalityDTO>();
            });
            return config;
        }

        internal static ContractFunctionalityDTO ToDTO(this ContractFunctionality contractFunctionality)
        {
            var config = CreateMapContractFunctionality();
            return config.Map<ContractFunctionality, ContractFunctionalityDTO>(contractFunctionality);
        }

        internal static IEnumerable<ContractFunctionalityDTO> ToDTOs(this IEnumerable<ContractFunctionality> contractFunctionality)
        {
            return contractFunctionality.Select(ToDTO);
        }

        internal static IMapper CreateMapContractLine()
        {
            var config = MapperCache.GetMapper<ContractLine, ContractLineDTO>(cfg =>
            {
                cfg.CreateMap<ContractLine, ContractLineDTO>();
                cfg.CreateMap<Contract, ContractDTO>();
                cfg.CreateMap<ResettlementType, ResettlementTypeDTO>();
                cfg.CreateMap<AffectationType, AffectationTypeDTO>();
                cfg.CreateMap<EPIType, EPITypeDTO>();
                cfg.CreateMap<ContractType, ContractTypeDTO>();
                cfg.CreateMap<ContractFunctionality, ContractFunctionalityDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<Level, LevelDTO>()
                   .ForMember(dest => dest.PremiumType, opt => opt.MapFrom(src => (int)src.PremiumType))
                   .ForMember(dest => dest.ApplyOnType, opt => opt.MapFrom(src => (int)src.ApplyOnType))
                   .ForMember(dest => dest.CalculationType, opt => opt.MapFrom(src => (int)src.CalculationType));
                cfg.CreateMap<LevelPayment, LevelPaymentDTO>();
                cfg.CreateMap<LevelCompany, LevelCompanyDTO>()
                        .ForMember(dest => dest.PresentationInformationType, opt => opt.MapFrom(src => (int)src.PresentationInformationType));
                cfg.CreateMap<LevelRestore, LevelRestoreDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<Agent, AgentDTO>();
            });
            return config;
        }

        internal static ContractLineDTO ToDTO(this ContractLine contractLine)
        {
            var config = CreateMapContractLine();
            return config.Map<ContractLine, ContractLineDTO>(contractLine);
        }

        internal static IEnumerable<ContractLineDTO> ToDTOs(this IEnumerable<ContractLine> contractLine)
        {
            return contractLine.Select(ToDTO);
        }

        internal static IMapper CreateMapContractType()
        {
            var config = MapperCache.GetMapper<ContractType, ContractTypeDTO>(cfg =>
            {
                cfg.CreateMap<ContractType, ContractTypeDTO>();
                cfg.CreateMap<ContractFunctionality, ContractFunctionalityDTO>();
            });
            return config;
        }

        internal static ContractTypeDTO ToDTO(this ContractType contractType)
        {
            var config = CreateMapContractType();
            return config.Map<ContractType, ContractTypeDTO>(contractType);
        }

        internal static IEnumerable<ContractTypeDTO> ToDTOs(this IEnumerable<ContractType> contractType)
        {
            return contractType.Select(ToDTO);
        }

        internal static IMapper CreateMapCumulusType()
        {
            var config = MapperCache.GetMapper<CumulusType, CumulusTypeDTO>(cfg =>
            {
                cfg.CreateMap<CumulusType, CumulusTypeDTO>();
            });
            return config;
        }

        internal static CumulusTypeDTO ToDTO(this CumulusType cumulusType)
        {
            var config = CreateMapCumulusType();
            return config.Map<CumulusType, CumulusTypeDTO>(cumulusType);
        }

        internal static IEnumerable<CumulusTypeDTO> ToDTOs(this IEnumerable<CumulusType> cumulusType)
        {
            return cumulusType.Select(ToDTO);
        }

        internal static IMapper CreateMapEPIType()
        {
            var config = MapperCache.GetMapper<EPIType, EPITypeDTO>(cfg =>
            {
                cfg.CreateMap<EPIType, EPITypeDTO>();
            });
            return config;
        }

        internal static EPITypeDTO ToDTO(this EPIType ePIType)
        {
            var config = CreateMapEPIType();
            return config.Map<EPIType, EPITypeDTO>(ePIType);
        }

        internal static IEnumerable<EPITypeDTO> ToDTOs(this IEnumerable<EPIType> ePIType)
        {
            return ePIType.Select(ToDTO);
        }

        internal static IMapper CreateMapInstallment()
        {
            var config = MapperCache.GetMapper<Installment, InstallmentDTO>(cfg =>
            {
                cfg.CreateMap<Installment, InstallmentDTO>();
                cfg.CreateMap<LevelCompany, LevelCompanyDTO>()
                        .ForMember(dest => dest.PresentationInformationType, opt => opt.MapFrom(src => (int)src.PresentationInformationType));
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<Company, CompanyDTO>();
                cfg.CreateMap<Agent, AgentDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<Level, LevelDTO>()
                   .ForMember(dest => dest.PremiumType, opt => opt.MapFrom(src => (int)src.PremiumType))
                   .ForMember(dest => dest.ApplyOnType, opt => opt.MapFrom(src => (int)src.ApplyOnType))
                   .ForMember(dest => dest.CalculationType, opt => opt.MapFrom(src => (int)src.CalculationType));
                cfg.CreateMap<LevelPayment, LevelPaymentDTO>();
                cfg.CreateMap<LevelRestore, LevelRestoreDTO>();
                cfg.CreateMap<Contract, ContractDTO>();
                cfg.CreateMap<ResettlementType, ResettlementTypeDTO>();
                cfg.CreateMap<AffectationType, AffectationTypeDTO>();
                cfg.CreateMap<EPIType, EPITypeDTO>();
                cfg.CreateMap<ContractType, ContractTypeDTO>();
                cfg.CreateMap<ContractFunctionality, ContractFunctionalityDTO>();
            });
            return config;
        }

        internal static InstallmentDTO ToDTO(this Installment installment)
        {
            var config = CreateMapInstallment();
            return config.Map<Installment, InstallmentDTO>(installment);
        }

        internal static IEnumerable<InstallmentDTO> ToDTOs(this IEnumerable<Installment> installment)
        {
            return installment.Select(ToDTO);
        }

        internal static IMapper CreateMapLevel()
        {
            var config = MapperCache.GetMapper<Level, LevelDTO>(cfg =>
            {
                cfg.CreateMap<Level, LevelDTO>()
                        .ForMember(dest => dest.CalculationType, opt => opt.MapFrom(src => (int)src.CalculationType))
                        .ForMember(dest => dest.ApplyOnType, opt => opt.MapFrom(src => (int)src.ApplyOnType))
                        .ForMember(dest => dest.PremiumType, opt => opt.MapFrom(src => (int)src.PremiumType));
                cfg.CreateMap<Contract, ContractDTO>();
                cfg.CreateMap<ResettlementType, ResettlementTypeDTO>();
                cfg.CreateMap<AffectationType, AffectationTypeDTO>();
                cfg.CreateMap<EPIType, EPITypeDTO>();
                cfg.CreateMap<ContractType, ContractTypeDTO>();
                cfg.CreateMap<ContractFunctionality, ContractFunctionalityDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<LevelPayment, LevelPaymentDTO>();
                cfg.CreateMap<LevelCompany, LevelCompanyDTO>()
                        .ForMember(dest => dest.PresentationInformationType, opt => opt.MapFrom(src => (int)src.PresentationInformationType));
                cfg.CreateMap<LevelRestore, LevelRestoreDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<Agent, AgentDTO>();
            });
            return config;
        }

        internal static LevelDTO ToDTO(this Level level)
        {
            var config = CreateMapLevel();
            return config.Map<Level, LevelDTO>(level);
        }

        internal static IEnumerable<LevelDTO> ToDTOs(this IEnumerable<Level> level)
        {
            return level.Select(ToDTO);
        }

        internal static IMapper CreateMapLevelCompany()
        {
            var config = MapperCache.GetMapper<LevelCompany, LevelCompanyDTO>(cfg =>
            {
                cfg.CreateMap<LevelCompany, LevelCompanyDTO>()
                    .ForMember(dest => dest.PresentationInformationType, opt => opt.MapFrom(src => (int)src.PresentationInformationType));
                cfg.CreateMap<Agent, AgentDTO>();
                cfg.CreateMap<Company, CompanyDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<Level, LevelDTO>()
                    .ForMember(dest => dest.PremiumType, opt => opt.MapFrom(src => (int)src.PremiumType))
                    .ForMember(dest => dest.ApplyOnType, opt => opt.MapFrom(src => (int)src.ApplyOnType))
                    .ForMember(dest => dest.CalculationType, opt => opt.MapFrom(src => (int)src.CalculationType));
                cfg.CreateMap<Installment, InstallmentDTO>();
                cfg.CreateMap<Contract, ContractDTO>();
                cfg.CreateMap<ResettlementType, ResettlementTypeDTO>();
                cfg.CreateMap<AffectationType, AffectationTypeDTO>();
                cfg.CreateMap<EPIType, EPITypeDTO>();
                cfg.CreateMap<ContractType, ContractTypeDTO>();
                cfg.CreateMap<ContractFunctionality, ContractFunctionalityDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<LevelPayment, LevelPaymentDTO>();
                cfg.CreateMap<LevelRestore, LevelRestoreDTO>();

            });
            return config;
        }

        internal static LevelCompanyDTO ToDTO(this LevelCompany levelCompany)
        {
            var config = CreateMapLevelCompany();
            return config.Map<LevelCompany, LevelCompanyDTO>(levelCompany);
        }

        internal static IEnumerable<LevelCompanyDTO> ToDTOs(this IEnumerable<LevelCompany> levelCompany)
        {
            return levelCompany.Select(ToDTO);
        }

        internal static IMapper CreateMapLevelPayment()
        {
            var config = MapperCache.GetMapper<LevelPayment, LevelPaymentDTO>(cfg =>
            {
                cfg.CreateMap<LevelPayment, LevelPaymentDTO>();
                cfg.CreateMap<Level, LevelDTO>()
                    .ForMember(dest => dest.PremiumType, opt => opt.MapFrom(src => (int)src.PremiumType))
                    .ForMember(dest => dest.ApplyOnType, opt => opt.MapFrom(src => (int)src.ApplyOnType))
                    .ForMember(dest => dest.CalculationType, opt => opt.MapFrom(src => (int)src.CalculationType));
                cfg.CreateMap<CommonService.Models.Amount, AmountDTO>();
                cfg.CreateMap<Installment, InstallmentDTO>();
                cfg.CreateMap<LevelPayment, LevelPaymentDTO>();
                cfg.CreateMap<Contract, ContractDTO>();
                cfg.CreateMap<ResettlementType, ResettlementTypeDTO>();
                cfg.CreateMap<AffectationType, AffectationTypeDTO>();
                cfg.CreateMap<EPIType, EPITypeDTO>();
                cfg.CreateMap<ContractType, ContractTypeDTO>();
                cfg.CreateMap<ContractFunctionality, ContractFunctionalityDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<LevelCompany, LevelCompanyDTO>()
                        .ForMember(dest => dest.PresentationInformationType, opt => opt.MapFrom(src => (int)src.PresentationInformationType));
                cfg.CreateMap<LevelRestore, LevelRestoreDTO>();
                cfg.CreateMap<Agent, AgentDTO>();
            });
            return config;
        }

        internal static LevelPaymentDTO ToDTO(this LevelPayment levelPayment)
        {
            var config = CreateMapLevelPayment();
            return config.Map<LevelPayment, LevelPaymentDTO>(levelPayment);

        }

        internal static IEnumerable<LevelPaymentDTO> ToDTOs(this IEnumerable<LevelPayment> levelPayment)
        {
            return levelPayment.Select(ToDTO);
        }

        internal static IMapper CreateMapLevelRestore()
        {
            var config = MapperCache.GetMapper<LevelRestore, LevelRestoreDTO>(cfg =>
            {
                cfg.CreateMap<LevelRestore, LevelRestoreDTO>();
                cfg.CreateMap<Level, LevelDTO>()
                   .ForMember(dest => dest.PremiumType, opt => opt.MapFrom(src => (int)src.PremiumType))
                   .ForMember(dest => dest.ApplyOnType, opt => opt.MapFrom(src => (int)src.ApplyOnType))
                   .ForMember(dest => dest.CalculationType, opt => opt.MapFrom(src => (int)src.CalculationType));
                cfg.CreateMap<CommonService.Models.Amount, AmountDTO>();
                cfg.CreateMap<Installment, InstallmentDTO>();
                cfg.CreateMap<LevelPayment, LevelPaymentDTO>();
                cfg.CreateMap<Contract, ContractDTO>();
                cfg.CreateMap<ResettlementType, ResettlementTypeDTO>();
                cfg.CreateMap<AffectationType, AffectationTypeDTO>();
                cfg.CreateMap<EPIType, EPITypeDTO>();
                cfg.CreateMap<ContractType, ContractTypeDTO>();
                cfg.CreateMap<ContractFunctionality, ContractFunctionalityDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<LevelCompany, LevelCompanyDTO>()
                        .ForMember(dest => dest.PresentationInformationType, opt => opt.MapFrom(src => (int)src.PresentationInformationType));
                cfg.CreateMap<Agent, AgentDTO>();

            });
            return config;
        }

        internal static LevelRestoreDTO ToDTO(this LevelRestore levelRestore)
        {
            var config = CreateMapLevelRestore();
            return config.Map<LevelRestore, LevelRestoreDTO>(levelRestore);
        }

        internal static IEnumerable<LevelRestoreDTO> ToDTOs(this IEnumerable<LevelRestore> levelRestore)
        {
            return levelRestore.Select(ToDTO);
        }

        internal static IMapper CreateMapLine()
        {
            var config = MapperCache.GetMapper<Line, LineDTO>(cfg =>
            {
                cfg.CreateMap<Line, LineDTO>();
                cfg.CreateMap<CumulusType, CumulusTypeDTO>();
                cfg.CreateMap<ContractLine, ContractLineDTO>();
                cfg.CreateMap<Contract, ContractDTO>();
                cfg.CreateMap<ResettlementType, ResettlementTypeDTO>();
                cfg.CreateMap<AffectationType, AffectationTypeDTO>();
                cfg.CreateMap<EPIType, EPITypeDTO>();
                cfg.CreateMap<ContractType, ContractTypeDTO>();
                cfg.CreateMap<ContractFunctionality, ContractFunctionalityDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<Level, LevelDTO>()
                   .ForMember(dest => dest.PremiumType, opt => opt.MapFrom(src => (int)src.PremiumType))
                   .ForMember(dest => dest.ApplyOnType, opt => opt.MapFrom(src => (int)src.ApplyOnType))
                   .ForMember(dest => dest.CalculationType, opt => opt.MapFrom(src => (int)src.CalculationType));
                cfg.CreateMap<LevelPayment, LevelPaymentDTO>();
                cfg.CreateMap<LevelCompany, LevelCompanyDTO>()
                        .ForMember(dest => dest.PresentationInformationType, opt => opt.MapFrom(src => (int)src.PresentationInformationType));
                cfg.CreateMap<LevelRestore, LevelRestoreDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<Agent, AgentDTO>();
            });
            return config;
        }

        internal static LineDTO ToDTO(this Line line)
        {
            var config = CreateMapLine();
            return config.Map<Line, LineDTO>(line);
        }

        internal static IEnumerable<LineDTO> ToDTOs(this IEnumerable<Line> line)
        {
            return line.Select(ToDTO);
        }

        internal static IMapper CreateMapReinsurance()
        {
            var config = MapperCache.GetMapper<EEProvider.Models.Reinsurance.Reinsurance, ReinsuranceDTO>(cfg =>
            {
                cfg.CreateMap<EEProvider.Models.Reinsurance.Reinsurance, ReinsuranceDTO>()
                        .ForMember(dest => dest.Movements, opt => opt.MapFrom(src => (int)src.Movements));
                cfg.CreateMap<ReinsuranceLayer, ReinsuranceLayerDTO>();
                cfg.CreateMap<ReinsuranceLine, ReinsuranceLineDTO>();
                cfg.CreateMap<Line, LineDTO>();
                cfg.CreateMap<CumulusType, CumulusTypeDTO>();
                cfg.CreateMap<ContractLine, ContractLineDTO>();
                cfg.CreateMap<Contract, ContractDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<ContractFunctionality, ContractFunctionalityDTO>();
                cfg.CreateMap<ContractType, ContractTypeDTO>();
                cfg.CreateMap<ReinsuranceAllocation, ReinsuranceAllocationDTO>();
                cfg.CreateMap<ReinsuranceCumulusRiskCoverage, ReinsuranceCumulusRiskCoverageDTO>();
                cfg.CreateMap<ResettlementType, ResettlementTypeDTO>();
                cfg.CreateMap<AffectationType, AffectationTypeDTO>();
                cfg.CreateMap<EPIType, EPITypeDTO>();
                cfg.CreateMap<Level, LevelDTO>()
                   .ForMember(dest => dest.PremiumType, opt => opt.MapFrom(src => (int)src.PremiumType))
                   .ForMember(dest => dest.ApplyOnType, opt => opt.MapFrom(src => (int)src.ApplyOnType))
                   .ForMember(dest => dest.CalculationType, opt => opt.MapFrom(src => (int)src.CalculationType));
                cfg.CreateMap<Agent, AgentDTO>();
                cfg.CreateMap<Company, CompanyDTO>();
                cfg.CreateMap<LevelCompany, LevelCompanyDTO>()
                        .ForMember(dest => dest.PresentationInformationType, opt => opt.MapFrom(src => (int)src.PresentationInformationType));
                cfg.CreateMap<LevelPayment, LevelPaymentDTO>();
                cfg.CreateMap<Installment, InstallmentDTO>();
                cfg.CreateMap<LevelRestore, LevelRestoreDTO>();
                cfg.CreateMap<CommonService.Models.Amount, AmountDTO>();

            });
            return config;
        }

        internal static ReinsuranceDTO ToDTO(this EEProvider.Models.Reinsurance.Reinsurance reinsurance)
        {
            var config = CreateMapReinsurance();
            return config.Map<EEProvider.Models.Reinsurance.Reinsurance, ReinsuranceDTO>(reinsurance);
        }

        internal static IEnumerable<ReinsuranceDTO> ToDTOs(this IEnumerable<EEProvider.Models.Reinsurance.Reinsurance> reinsurance)
        {
            return reinsurance.Select(ToDTO);
        }

        internal static IMapper CreateMapReinsuranceAllocation()
        {
            var config = MapperCache.GetMapper<ReinsuranceAllocation, ReinsuranceAllocationDTO>(cfg =>
            {
                cfg.CreateMap<ReinsuranceAllocation, ReinsuranceAllocationDTO>();
                cfg.CreateMap<Contract, ContractDTO>();
                cfg.CreateMap<ResettlementType, ResettlementTypeDTO>();
                cfg.CreateMap<AffectationType, AffectationTypeDTO>();
                cfg.CreateMap<EPIType, EPITypeDTO>();
                cfg.CreateMap<ContractType, ContractTypeDTO>();
                cfg.CreateMap<ContractFunctionality, ContractFunctionalityDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<Level, LevelDTO>()
                   .ForMember(dest => dest.PremiumType, opt => opt.MapFrom(src => (int)src.PremiumType))
                   .ForMember(dest => dest.ApplyOnType, opt => opt.MapFrom(src => (int)src.ApplyOnType))
                   .ForMember(dest => dest.CalculationType, opt => opt.MapFrom(src => (int)src.CalculationType));
                cfg.CreateMap<LevelPayment, LevelPaymentDTO>();
                cfg.CreateMap<LevelCompany, LevelCompanyDTO>()
                        .ForMember(dest => dest.PresentationInformationType, opt => opt.MapFrom(src => (int)src.PresentationInformationType));
                cfg.CreateMap<LevelRestore, LevelRestoreDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<Agent, AgentDTO>();
            });
            return config;
        }

        internal static ReinsuranceAllocationDTO ToDTO(this ReinsuranceAllocation reinsuranceAllocation)
        {
            var config = CreateMapReinsuranceAllocation();
            return config.Map<ReinsuranceAllocation, ReinsuranceAllocationDTO>(reinsuranceAllocation);
        }

        internal static IEnumerable<ReinsuranceAllocationDTO> ToDTOs(this IEnumerable<ReinsuranceAllocation> reinsuranceAllocation)
        {
            return reinsuranceAllocation.Select(ToDTO);
        }

        internal static IMapper CreateMapReinsuranceCumulusRiskCoverage()
        {
            var config = MapperCache.GetMapper<ReinsuranceCumulusRiskCoverage, ReinsuranceCumulusRiskCoverageDTO>(cfg =>
            {
                cfg.CreateMap<ReinsuranceCumulusRiskCoverage, ReinsuranceCumulusRiskCoverageDTO>();
            });
            return config;
        }

        internal static ReinsuranceCumulusRiskCoverageDTO ToDTO(this ReinsuranceCumulusRiskCoverage reinsuranceCumulusRiskCoverage)
        {
            var config = CreateMapReinsuranceCumulusRiskCoverage();
            return config.Map<ReinsuranceCumulusRiskCoverage, ReinsuranceCumulusRiskCoverageDTO>(reinsuranceCumulusRiskCoverage);
        }

        internal static IEnumerable<ReinsuranceCumulusRiskCoverageDTO> ToDTOs(this IEnumerable<ReinsuranceCumulusRiskCoverage> reinsuranceCumulusRiskCoverage)
        {
            return reinsuranceCumulusRiskCoverage.Select(ToDTO);
        }

        internal static IMapper CreateMapReinsuranceLayer()
        {
            var config = MapperCache.GetMapper<ReinsuranceLayer, ReinsuranceLayerDTO>(cfg =>
            {
                cfg.CreateMap<ReinsuranceLayer, ReinsuranceLayerDTO>();
                cfg.CreateMap<ReinsuranceLine, ReinsuranceLineDTO>();
                cfg.CreateMap<ReinsuranceAllocation, ReinsuranceAllocationDTO>();
                cfg.CreateMap<ReinsuranceCumulusRiskCoverage, ReinsuranceCumulusRiskCoverageDTO>();
                cfg.CreateMap<Line, LineDTO>();
                cfg.CreateMap<CumulusType, CumulusTypeDTO>();
                cfg.CreateMap<ContractLine, ContractLineDTO>();
                cfg.CreateMap<Contract, ContractDTO>();
                cfg.CreateMap<ResettlementType, ResettlementTypeDTO>();
                cfg.CreateMap<AffectationType, AffectationTypeDTO>();
                cfg.CreateMap<EPIType, EPITypeDTO>();
                cfg.CreateMap<ContractType, ContractTypeDTO>();
                cfg.CreateMap<ContractFunctionality, ContractFunctionalityDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<Level, LevelDTO>()
                   .ForMember(dest => dest.PremiumType, opt => opt.MapFrom(src => (int)src.PremiumType))
                   .ForMember(dest => dest.ApplyOnType, opt => opt.MapFrom(src => (int)src.ApplyOnType))
                   .ForMember(dest => dest.CalculationType, opt => opt.MapFrom(src => (int)src.CalculationType));
                cfg.CreateMap<LevelPayment, LevelPaymentDTO>();
                cfg.CreateMap<LevelCompany, LevelCompanyDTO>()
                        .ForMember(dest => dest.PresentationInformationType, opt => opt.MapFrom(src => (int)src.PresentationInformationType));
                cfg.CreateMap<LevelRestore, LevelRestoreDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<Agent, AgentDTO>();
            });
            return config;
        }

        internal static ReinsuranceLayerDTO ToDTO(this ReinsuranceLayer reinsuranceLayer)
        {
            var config = CreateMapReinsuranceLayer();
            return config.Map<ReinsuranceLayer, ReinsuranceLayerDTO>(reinsuranceLayer);
        }

        internal static IEnumerable<ReinsuranceLayerDTO> ToDTOs(this IEnumerable<ReinsuranceLayer> reinsuranceLayer)
        {
            return reinsuranceLayer.Select(ToDTO);
        }

        internal static IMapper CreateMapReinsuranceLine()
        {
            var config = MapperCache.GetMapper<ReinsuranceLine, ReinsuranceLineDTO>(cfg =>
            {
                cfg.CreateMap<ReinsuranceLine, ReinsuranceLineDTO>();
                cfg.CreateMap<ReinsuranceAllocation, ReinsuranceAllocationDTO>();
                cfg.CreateMap<ReinsuranceCumulusRiskCoverage, ReinsuranceCumulusRiskCoverageDTO>();
                cfg.CreateMap<Contract, ContractDTO>();
                cfg.CreateMap<ResettlementType, ResettlementTypeDTO>();
                cfg.CreateMap<AffectationType, AffectationTypeDTO>();
                cfg.CreateMap<EPIType, EPITypeDTO>();
                cfg.CreateMap<ContractType, ContractTypeDTO>();
                cfg.CreateMap<ContractFunctionality, ContractFunctionalityDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<Level, LevelDTO>()
                   .ForMember(dest => dest.PremiumType, opt => opt.MapFrom(src => (int)src.PremiumType))
                   .ForMember(dest => dest.ApplyOnType, opt => opt.MapFrom(src => (int)src.ApplyOnType))
                   .ForMember(dest => dest.CalculationType, opt => opt.MapFrom(src => (int)src.CalculationType));
                cfg.CreateMap<LevelPayment, LevelPaymentDTO>();
                cfg.CreateMap<LevelCompany, LevelCompanyDTO>()
                        .ForMember(dest => dest.PresentationInformationType, opt => opt.MapFrom(src => (int)src.PresentationInformationType));
                cfg.CreateMap<LevelRestore, LevelRestoreDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<Agent, AgentDTO>();
            });
            return config;
        }

        internal static ReinsuranceLineDTO ToDTO(this ReinsuranceLine reinsuranceLine)
        {
            var config = CreateMapReinsuranceLine();
            return config.Map<ReinsuranceLine, ReinsuranceLineDTO>(reinsuranceLine);
        }

        internal static IEnumerable<ReinsuranceLineDTO> ToDTOs(this IEnumerable<ReinsuranceLine> reinsuranceLine)
        {
            return reinsuranceLine.Select(ToDTO);
        }

        internal static IMapper CreateMapResettlementType()
        {
            var config = MapperCache.GetMapper<ResettlementType, ResettlementTypeDTO>(cfg =>
            {
                cfg.CreateMap<ResettlementType, ResettlementTypeDTO>();
            });
            return config;
        }

        internal static ResettlementTypeDTO ToDTO(this ResettlementType resettlementType)
        {
            var config = CreateMapResettlementType();
            return config.Map<ResettlementType, ResettlementTypeDTO>(resettlementType);
        }

        internal static IEnumerable<ResettlementTypeDTO> ToDTOs(this IEnumerable<ResettlementType> resettlementType)
        {
            return resettlementType.Select(ToDTO);
        }

        internal static IMapper CreateMapCumulus()
        {
            var config = MapperCache.GetMapper<Cumulus, CumulusDTO>(cfg =>
            {
                cfg.CreateMap<Cumulus, CumulusDTO>();
                cfg.CreateMap<IssueLayer, IssueLayerDTO>();
                cfg.CreateMap<IssueLayerLine, IssueLayerLineDTO>();
                cfg.CreateMap<IssueAllocation, IssueAllocationDTO>();
                cfg.CreateMap<Contract, ContractDTO>();
                cfg.CreateMap<ResettlementType, ResettlementTypeDTO>();
                cfg.CreateMap<AffectationType, AffectationTypeDTO>();
                cfg.CreateMap<EPIType, EPITypeDTO>();
                cfg.CreateMap<ContractType, ContractTypeDTO>();
                cfg.CreateMap<ContractFunctionality, ContractFunctionalityDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<Level, LevelDTO>()
                   .ForMember(dest => dest.PremiumType, opt => opt.MapFrom(src => (int)src.PremiumType))
                   .ForMember(dest => dest.ApplyOnType, opt => opt.MapFrom(src => (int)src.ApplyOnType))
                   .ForMember(dest => dest.CalculationType, opt => opt.MapFrom(src => (int)src.CalculationType));
                cfg.CreateMap<LevelPayment, LevelPaymentDTO>();
                cfg.CreateMap<LevelCompany, LevelCompanyDTO>()
                        .ForMember(dest => dest.PresentationInformationType, opt => opt.MapFrom(src => (int)src.PresentationInformationType));
                cfg.CreateMap<LevelRestore, LevelRestoreDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<Agent, AgentDTO>();
            });
            return config;
        }

        internal static CumulusDTO ToDTO(this Cumulus cumulus)
        {
            var config = CreateMapCumulus();
            return config.Map<Cumulus, CumulusDTO>(cumulus);
        }

        internal static IEnumerable<CumulusDTO> ToDTOs(this IEnumerable<Cumulus> cumulus)
        {
            return cumulus.Select(ToDTO);
        }

        internal static IMapper CreateMapClaimReinsurance()
        {
            var config = MapperCache.GetMapper<TempCommonServices.Models.ClaimReinsurance, ClaimReinsuranceDTO>(cfg =>
            {
                cfg.CreateMap<TempCommonServices.Models.ClaimReinsurance, ClaimReinsuranceDTO>();
                cfg.CreateMap<EstimationType, EstimationTypeDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
            });
            return config;
        }

        internal static ClaimReinsuranceDTO ToDTO(this TempCommonServices.Models.ClaimReinsurance claimReinsurance)
        {
            var config = CreateMapClaimReinsurance();
            return config.Map<TempCommonServices.Models.ClaimReinsurance, ClaimReinsuranceDTO>(claimReinsurance);
        }

        internal static IEnumerable<ClaimReinsuranceDTO> ToDTOs(this IEnumerable<TempCommonServices.Models.ClaimReinsurance> claimReinsurances)
        {
            return claimReinsurances.Select(ToDTO);
        }

        internal static IMapper CreateMapReinsurancePaymentClaim()
        {
            var config = MapperCache.GetMapper<ReinsurancePaymentClaim, ReinsurancePaymentClaimDTO>(cfg =>
            {
                cfg.CreateMap<ReinsurancePaymentClaim, ReinsurancePaymentClaimDTO>()
                        .ForMember(dest => dest.Movement, opt => opt.MapFrom(src => src.Movement));
                cfg.CreateMap<ReinsurancePaymentClaimLayer, ReinsurancePaymentClaimLayerDTO>();
            });
            return config;
        }

        internal static ReinsurancePaymentClaimDTO ToDTO(this ReinsurancePaymentClaim reinsurancePaymentClaim)
        {
            var config = CreateMapReinsurancePaymentClaim();
            return config.Map<ReinsurancePaymentClaim, ReinsurancePaymentClaimDTO>(reinsurancePaymentClaim);
        }

        internal static IEnumerable<ReinsurancePaymentClaimDTO> ToDTOs(this IEnumerable<ReinsurancePaymentClaim> reinsurancePaymentClaims)
        {
            return reinsurancePaymentClaims.Select(ToDTO);
        }

        internal static IMapper CreateMapReinsurancePaymentDistribution()
        {
            var config = MapperCache.GetMapper<ReinsurancePaymentDistribution, ReinsurancePaymentDistributionDTO>(cfg =>
            {
                cfg.CreateMap<ReinsurancePaymentDistribution, ReinsurancePaymentDistributionDTO>();
            });
            return config;
        }

        internal static ReinsurancePaymentDistributionDTO ToDTO(this ReinsurancePaymentDistribution reinsurancePaymentDistribution)
        {
            var config = CreateMapReinsurancePaymentDistribution();
            return config.Map<ReinsurancePaymentDistribution, ReinsurancePaymentDistributionDTO>(reinsurancePaymentDistribution);
        }

        internal static IEnumerable<ReinsurancePaymentDistributionDTO> ToDTOs(this IEnumerable<ReinsurancePaymentDistribution> reinsurancePaymentDistributions)
        {
            return reinsurancePaymentDistributions.Select(ToDTO);
        }

        internal static IMapper CreateMapReinsurancePaymentLayer()
        {
            var config = MapperCache.GetMapper<ReinsurancePaymentLayer, ReinsurancePaymentLayerDTO>(cfg =>
            {
                cfg.CreateMap<ReinsurancePaymentLayer, ReinsurancePaymentLayerDTO>();
                cfg.CreateMap<ReinsuranceClaimLayer, ReinsuranceClaimLayerDTO>();

            });
            return config;
        }

        internal static ReinsurancePaymentLayerDTO ToDTO(this ReinsurancePaymentLayer reinsurancePaymentLayer)
        {
            var config = CreateMapReinsurancePaymentLayer();
            return config.Map<ReinsurancePaymentLayer, ReinsurancePaymentLayerDTO>(reinsurancePaymentLayer);
        }

        internal static IEnumerable<ReinsurancePaymentLayerDTO> ToDTOs(this IEnumerable<ReinsurancePaymentLayer> reinsurancePaymentLayers)
        {
            return reinsurancePaymentLayers.Select(ToDTO);
        }

        internal static IMapper CreateMapReinsuranceClaimDistribution()
        {
            var config = MapperCache.GetMapper<ReinsuranceClaimDistribution, ReinsuranceClaimDistributionDTO>(cfg =>
            {
                cfg.CreateMap<ReinsuranceClaimDistribution, ReinsuranceClaimDistributionDTO>();
            });
            return config;
        }

        internal static ReinsuranceClaimDistributionDTO ToDTO(this ReinsuranceClaimDistribution reinsuranceClaimDistribution)
        {
            var config = CreateMapReinsuranceClaimDistribution();
            return config.Map<ReinsuranceClaimDistribution, ReinsuranceClaimDistributionDTO>(reinsuranceClaimDistribution);
        }

        internal static IEnumerable<ReinsuranceClaimDistributionDTO> ToDTOs(this IEnumerable<ReinsuranceClaimDistribution> reinsuranceClaimDistributions)
        {
            return reinsuranceClaimDistributions.Select(ToDTO);
        }

        internal static IMapper CreateMapReinsuranceClaimLayer()
        {
            var config = MapperCache.GetMapper<ReinsuranceClaimLayer, ReinsuranceClaimLayerDTO>(cfg =>
            {
                cfg.CreateMap<ReinsuranceClaimLayer, ReinsuranceClaimLayerDTO>();
            });
            return config;
        }

        internal static ReinsuranceClaimLayerDTO ToDTO(this ReinsuranceClaimLayer reinsuranceClaimLayer)
        {
            var config = CreateMapReinsuranceClaimLayer();
            return config.Map<ReinsuranceClaimLayer, ReinsuranceClaimLayerDTO>(reinsuranceClaimLayer);
        }

        internal static IEnumerable<ReinsuranceClaimLayerDTO> ToDTOs(this IEnumerable<ReinsuranceClaimLayer> reinsuranceClaimLayers)
        {
            return reinsuranceClaimLayers.Select(ToDTO);
        }

        internal static IMapper CreateMapReinsuranceDistribution()
        {
            var config = MapperCache.GetMapper<ReinsuranceDistribution, ReinsuranceDistributionDTO>(cfg =>
            {
                cfg.CreateMap<ReinsuranceDistribution, ReinsuranceDistributionDTO>();
            });
            return config;
        }

        internal static ReinsuranceDistributionDTO ToDTO(this ReinsuranceDistribution reinsuranceDistribution)
        {
            var config = CreateMapReinsuranceDistribution();
            return config.Map<ReinsuranceDistribution, ReinsuranceDistributionDTO>(reinsuranceDistribution);
        }

        internal static IEnumerable<ReinsuranceDistributionDTO> ToDTOs(this IEnumerable<ReinsuranceDistribution> reinsuranceDistributions)
        {
            return reinsuranceDistributions.Select(ToDTO);
        }

        internal static IMapper CreateMapReinsurancePrefix()
        {
            var config = MapperCache.GetMapper<ReinsurancePrefix, ReinsurancePrefixDTO>(cfg =>
            {

                cfg.CreateMap<ReinsurancePrefix, ReinsurancePrefixDTO>();
                cfg.CreateMap<Prefix, PrefixDTO>();
            });
            return config;
        }

        internal static ReinsurancePrefixDTO ToDTO(this ReinsurancePrefix reinsurancePrefix)
        {
            var config = CreateMapReinsurancePrefix();
            return config.Map<ReinsurancePrefix, ReinsurancePrefixDTO>(reinsurancePrefix);
        }

        internal static IEnumerable<ReinsurancePrefixDTO> ToDTOs(this IEnumerable<ReinsurancePrefix> reinsurancePrefix)
        {
            return reinsurancePrefix.Select(ToDTO);
        }

        internal static IMapper CreateMapReinsuranceDistributionHeader()
        {
            var config = MapperCache.GetMapper<ReinsuranceDistributionHeader, ReinsuranceDistributionHeaderDTO>(cfg =>
            {
                cfg.CreateMap<ReinsuranceDistributionHeader, ReinsuranceDistributionHeaderDTO>();
            });
            return config;
        }

        internal static ReinsuranceDistributionHeaderDTO ToDTO(this ReinsuranceDistributionHeader reinsuranceDistributionHeader)
        {
            var config = CreateMapReinsuranceDistributionHeader();
            return config.Map<ReinsuranceDistributionHeader, ReinsuranceDistributionHeaderDTO>(reinsuranceDistributionHeader);
        }

        internal static IEnumerable<ReinsuranceDistributionHeaderDTO> ToDTOs(this IEnumerable<ReinsuranceDistributionHeader> reinsuranceDistributionHeaders)
        {
            return reinsuranceDistributionHeaders.Select(ToDTO);
        }

        internal static IMapper CreateMapTempReinsuranceProcess()
        {
            var config = MapperCache.GetMapper<TempReinsuranceProcess, TempReinsuranceProcessDTO>(cfg =>
            {
                cfg.CreateMap<TempReinsuranceProcess, TempReinsuranceProcessDTO>();
            });
            return config;
        }

        internal static TempReinsuranceProcessDTO ToDTO(this TempReinsuranceProcess tempReinsuranceProcess)
        {
            var config = CreateMapTempReinsuranceProcess();
            return config.Map<TempReinsuranceProcess, TempReinsuranceProcessDTO>(tempReinsuranceProcess);
        }

        internal static IEnumerable<TempReinsuranceProcessDTO> ToDTOs(this IEnumerable<TempReinsuranceProcess> tempReinsuranceProcesses)
        {
            return tempReinsuranceProcesses.Select(ToDTO);
        }

        internal static IMapper CreateMapPaymentRequest()
        {
            var config = MapperCache.GetMapper<PaymentRequest, PaymentRequestDTO>(cfg =>
            {
                cfg.CreateMap<PaymentRequest, PaymentRequestDTO>();
            });
            return config;
        }

        internal static PaymentRequestDTO ToDTO(this PaymentRequest reinsuranceClaimPaymentRequest)
        {
            var config = CreateMapPaymentRequest();
            return config.Map<PaymentRequest, PaymentRequestDTO>(reinsuranceClaimPaymentRequest);
        }

        internal static IEnumerable<PaymentRequestDTO> ToDTOs(this IEnumerable<PaymentRequest> reinsuranceClaimPaymentRequests)
        {
            return reinsuranceClaimPaymentRequests.Select(ToDTO);
        }

        internal static IMapper CreateMapEndorsementReinsurance()
        {
            var config = MapperCache.GetMapper<EndorsementReinsurance, EndorsementReinsuranceDTO>(cfg =>
            {
                cfg.CreateMap<EndorsementReinsurance, EndorsementReinsuranceDTO>();
            });
            return config;
        }

        internal static EndorsementReinsuranceDTO ToDTO(this EndorsementReinsurance endorsementReinsurance)
        {
            var config = CreateMapEndorsementReinsurance();
            return config.Map<EndorsementReinsurance, EndorsementReinsuranceDTO>(endorsementReinsurance);
        }

        internal static IEnumerable<EndorsementReinsuranceDTO> ToDTOs(this IEnumerable<EndorsementReinsurance> endorsementReinsurance)
        {
            return endorsementReinsurance.Select(ToDTO);
        }

        internal static IMapper CreateMapIssGetDistributionErrors()
        {
            var config = MapperCache.GetMapper<IssGetDistributionErrors, IssGetDistributionErrorsDTO>(cfg =>
            {
                cfg.CreateMap<IssGetDistributionErrors, IssGetDistributionErrorsDTO>();
            });
            return config;
        }

        internal static IssGetDistributionErrorsDTO ToDTO(this IssGetDistributionErrors issGetDistributionErrors)
        {
            var config = CreateMapIssGetDistributionErrors();
            return config.Map<IssGetDistributionErrors, IssGetDistributionErrorsDTO>(issGetDistributionErrors);
        }

        internal static IEnumerable<IssGetDistributionErrorsDTO> ToDTOs(this IEnumerable<IssGetDistributionErrors> issGetDistributionErrors)
        {
            return issGetDistributionErrors.Select(ToDTO);
        }

        internal static IMapper CreateMapLineCumulusType()
        {
            var config = MapperCache.GetMapper<LineCumulusType, LineCumulusTypeDTO>(cfg =>
            {
                cfg.CreateMap<LineCumulusType, LineCumulusTypeDTO>();
            });
            return config;
        }

        internal static LineCumulusTypeDTO ToDTO(this LineCumulusType lineCumulusType)
        {
            var config = CreateMapLineCumulusType();
            return config.Map<LineCumulusType, LineCumulusTypeDTO>(lineCumulusType);
        }

        internal static IEnumerable<LineCumulusTypeDTO> ToDTOs(this IEnumerable<LineCumulusType> lineCumulusType)
        {
            return lineCumulusType.Select(ToDTO);
        }

        internal static IMapper CreateMapModule()
        {
            var config = MapperCache.GetMapper<Module, ModuleDTO>(cfg =>
            {
                cfg.CreateMap<Module, ModuleDTO>();
            });
            return config;
        }

        internal static ModuleDTO ToDTO(this Module module)
        {
            var config = CreateMapModule();
            return config.Map<Module, ModuleDTO>(module);
        }

        internal static IEnumerable<ModuleDTO> ToDTOs(this IEnumerable<Module> module)
        {
            return module.Select(ToDTO);
        }

        internal static IMapper CreateMapReinsuranceAccountingParameter()
        {
            var config = MapperCache.GetMapper<ReinsuranceAccountingParameter, ReinsuranceAccountingParameterDTO>(cfg =>
            {
                cfg.CreateMap<ReinsuranceAccountingParameter, ReinsuranceAccountingParameterDTO>();
            });
            return config;
        }

        internal static ReinsuranceAccountingParameterDTO ToDTO(this ReinsuranceAccountingParameter reinsuranceAccountingParameter)
        {
            var config = CreateMapReinsuranceAccountingParameter();
            return config.Map<ReinsuranceAccountingParameter, ReinsuranceAccountingParameterDTO>(reinsuranceAccountingParameter);
        }

        internal static IEnumerable<ReinsuranceAccountingParameterDTO> ToDTOs(this IEnumerable<ReinsuranceAccountingParameter> reinsuranceAccountingParameter)
        {
            return reinsuranceAccountingParameter.Select(ToDTO);
        }

        internal static IMapper CreateMapLineAssociation()
        {
            var config = MapperCache.GetMapper<LineAssociation, LineAssociationDTO>(cfg =>
            {
                cfg.CreateMap<LineAssociation, LineAssociationDTO>();
                cfg.CreateMap<LineAssociationType, LineAssociationTypeDTO>();
                cfg.CreateMap<Line, LineDTO>();
                cfg.CreateMap<CumulusType, CumulusTypeDTO>();
                cfg.CreateMap<ContractLine, ContractLineDTO>();
                cfg.CreateMap<Contract, ContractDTO>();
                cfg.CreateMap<ResettlementType, ResettlementTypeDTO>();
                cfg.CreateMap<AffectationType, AffectationTypeDTO>();
                cfg.CreateMap<EPIType, EPITypeDTO>();
                cfg.CreateMap<ContractType, ContractTypeDTO>();
                cfg.CreateMap<ContractFunctionality, ContractFunctionalityDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<Level, LevelDTO>()
                   .ForMember(dest => dest.PremiumType, opt => opt.MapFrom(src => (int)src.PremiumType))
                   .ForMember(dest => dest.ApplyOnType, opt => opt.MapFrom(src => (int)src.ApplyOnType))
                   .ForMember(dest => dest.CalculationType, opt => opt.MapFrom(src => (int)src.CalculationType));
                cfg.CreateMap<LevelPayment, LevelPaymentDTO>();
                cfg.CreateMap<LevelCompany, LevelCompanyDTO>()
                        .ForMember(dest => dest.PresentationInformationType, opt => opt.MapFrom(src => (int)src.PresentationInformationType));
                cfg.CreateMap<LevelRestore, LevelRestoreDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<Agent, AgentDTO>();
            });
            return config;
        }

        internal static LineAssociationDTO ToDTO(this LineAssociation lineAssociation)
        {
            var config = CreateMapLineAssociation();
            return config.Map<LineAssociation, LineAssociationDTO>(lineAssociation);
        }

        internal static IEnumerable<LineAssociationDTO> ToDTOs(this IEnumerable<LineAssociation> lineAssociation)
        {
            return lineAssociation.Select(ToDTO);
        }

        internal static IMapper CreateMapAssociationLine()
        {
            var config = MapperCache.GetMapper<AssociationLine, AssociationLineDTO>(cfg =>
            {
                cfg.CreateMap<AssociationLine, AssociationLineDTO>();
                cfg.CreateMap<ByPolicyLineBusinessSubLineBusiness, ByPolicyLineBusinessSubLineBusinessDTO>();
                cfg.CreateMap<TempCommonServices.Models.Policy, PolicyDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<Prefix, PrefixDTO>();
                cfg.CreateMap<TempCommonServices.Models.Endorsement, EndorsementDTO>();
                cfg.CreateMap<LineBusiness, LineBusinessDTO>();
                cfg.CreateMap<SubLineBusiness, SubLineBusinessDTO>();
                cfg.CreateMap<InsuredObject, InsuredObjectDTO>();
                cfg.CreateMap<ByInsured, ByInsuredDTO>();
                cfg.CreateMap<Individual, IndividualDTO>();
                cfg.CreateMap<ByInsuredPrefix, ByInsuredPrefixDTO>();
                cfg.CreateMap<ByFacultativeIssue, ByFacultativeIssueDTO>();
                cfg.CreateMap<ByPolicy, ByPolicyDTO>();
                cfg.CreateMap<ByOperationTypePrefix, ByOperationTypePrefixDTO>();
                cfg.CreateMap<ByLineBusinessSubLineBusiness, ByLineBusinessSubLineBusinessDTO>();
                cfg.CreateMap<ByLineBusinessSubLineBusinessCoverage, ByLineBusinessSubLineBusinessCoverageDTO>();
                cfg.CreateMap<TempCommonServices.Models.Coverage, CoverageDTO>();
                cfg.CreateMap<ByPrefixProduct, ByPrefixProductDTO>();
            });
            return config;
        }

        internal static AssociationLineDTO ToDTO(this AssociationLine associationLine)
        {
            var config = CreateMapAssociationLine();
            return config.Map<AssociationLine, AssociationLineDTO>(associationLine);
        }

        internal static IEnumerable<AssociationLineDTO> ToDTOs(this IEnumerable<AssociationLine> lineAssociation)
        {
            return lineAssociation.Select(ToDTO);
        }

        internal static IMapper CreateMapClaim()
        {
            var config = MapperCache.GetMapper<Claim, ClaimDTO>(cfg =>
            {
                cfg.CreateMap<Claim, ClaimDTO>();
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<Prefix, PrefixDTO>();
                cfg.CreateMap<ClaimEndorsement, ClaimEndorsementDTO>();
                cfg.CreateMap<CatastrophicEvent, CatastrophicEventDTO>();
                cfg.CreateMap<Catastrophe, CatastropheDTO>();
                cfg.CreateMap<City, CityDTO>();
                cfg.CreateMap<Inspection, InspectionDTO>();
                cfg.CreateMap<Cause, CauseDTO>();
                cfg.CreateMap<ClaimModify, ClaimModifyDTO>();
                cfg.CreateMap<ClaimCoverage, ClaimCoverageDTO>();
                cfg.CreateMap<ThirdAffected, ThirdAffectedDTO>();
                cfg.CreateMap<ThirdPartyVehicle, ThirdPartyVehicleDTO>();
                cfg.CreateMap<Estimation, EstimationDTO>();
                cfg.CreateMap<EstimationType, EstimationTypeDTO>();
                cfg.CreateMap<Reason, ReasonDTO>();
                cfg.CreateMap<Status, StatusDTO>();
                cfg.CreateMap<InternalStatus, InternalStatusDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<TextOperation, TextOperationDTO>();
                cfg.CreateMap<DamageType, DamageTypeDTO>();
                cfg.CreateMap<DamageResponsibility, DamageResponsabilityDTO>();
                cfg.CreateMap<ClaimEndorsement, ClaimEndorsementDTO>();
                cfg.CreateMap<State, StateDTO>();
                cfg.CreateMap<Country, CountryDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
            });
            return config;
        }

        internal static ClaimDTO ToDTO(this Claim claim)
        {
            var config = CreateMapClaim();
            return config.Map<Claim, ClaimDTO>(claim);
        }

        internal static IEnumerable<ClaimDTO> ToDTOs(this IEnumerable<Claim> claim)
        {
            return claim.Select(ToDTO);
        }

        internal static IMapper CreateMapPolicy()
        {
            var config = MapperCache.GetMapper<TempCommonServices.Models.Policy, PolicyDTO>(cfg =>
            {
                cfg.CreateMap<TempCommonServices.Models.Policy, PolicyDTO>();
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<Prefix, PrefixDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<TempCommonServices.Models.Endorsement, EndorsementDTO>();
                cfg.CreateMap<TempCommonServices.Models.Risk, RiskDTO>();
                cfg.CreateMap<TempCommonServices.Models.Coverage, CoverageDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
            });
            return config;
        }

        internal static PolicyDTO ToDTO(this TempCommonServices.Models.Policy policy)
        {
            var config = CreateMapPolicy();
            return config.Map<TempCommonServices.Models.Policy, PolicyDTO>(policy);
        }

        internal static IEnumerable<PolicyDTO> ToDTOs(this IEnumerable<TempCommonServices.Models.Policy> policies)
        {
            return policies.Select(ToDTO);
        }

        internal static IMapper CreateMapReinsuranceLayerIssuance()
        {
            var config = MapperCache.GetMapper<ReinsuranceLayerIssuance, ReinsuranceLayerIssuanceDTO>(cfg =>
            {
                cfg.CreateMap<ReinsuranceLayerIssuance, ReinsuranceLayerIssuanceDTO>();
                cfg.CreateMap<ReinsuranceLine, ReinsuranceLineDTO>();
                cfg.CreateMap<ReinsuranceAllocation, ReinsuranceAllocationDTO>();
                cfg.CreateMap<ReinsuranceCumulusRiskCoverage, ReinsuranceCumulusRiskCoverageDTO>();
                cfg.CreateMap<Line, LineDTO>();
                cfg.CreateMap<CumulusType, CumulusTypeDTO>();
                cfg.CreateMap<ContractLine, ContractLineDTO>();
                cfg.CreateMap<Contract, ContractDTO>();
                cfg.CreateMap<ResettlementType, ResettlementTypeDTO>();
                cfg.CreateMap<AffectationType, AffectationTypeDTO>();
                cfg.CreateMap<EPIType, EPITypeDTO>();
                cfg.CreateMap<ContractType, ContractTypeDTO>();
                cfg.CreateMap<ContractFunctionality, ContractFunctionalityDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<Level, LevelDTO>()
                   .ForMember(dest => dest.PremiumType, opt => opt.MapFrom(src => (int)src.PremiumType))
                   .ForMember(dest => dest.ApplyOnType, opt => opt.MapFrom(src => (int)src.ApplyOnType))
                   .ForMember(dest => dest.CalculationType, opt => opt.MapFrom(src => (int)src.CalculationType));
                cfg.CreateMap<LevelPayment, LevelPaymentDTO>();
                cfg.CreateMap<LevelCompany, LevelCompanyDTO>()
                        .ForMember(dest => dest.PresentationInformationType, opt => opt.MapFrom(src => (int)src.PresentationInformationType));
                cfg.CreateMap<LevelRestore, LevelRestoreDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<Agent, AgentDTO>();
                cfg.CreateMap<Company, CompanyDTO>();
                cfg.CreateMap<Installment, InstallmentDTO>();
            });
            return config;
        }

        internal static ReinsuranceLayerIssuanceDTO ToDTO(this ReinsuranceLayerIssuance reinsuranceLayerIssuance)
        {
            var config = CreateMapReinsuranceLayerIssuance();
            return config.Map<ReinsuranceLayerIssuance, ReinsuranceLayerIssuanceDTO>(reinsuranceLayerIssuance);
        }

        internal static IEnumerable<ReinsuranceLayerIssuanceDTO> ToDTOs(this IEnumerable<ReinsuranceLayerIssuance> reinsuranceLayerIssuances)
        {
            return reinsuranceLayerIssuances.Select(ToDTO);
        }


        internal static IMapper CreateMapLineAssociationType()
        {
            var config = MapperCache.GetMapper<LineAssociationType, LineAssociationTypeDTO>(cfg =>
            {
                cfg.CreateMap<LineAssociationType, LineAssociationTypeDTO>();
            });
            return config;
        }

        internal static LineAssociationTypeDTO ToDTO(this LineAssociationType lineAssociationType)
        {
            var config = CreateMapLineAssociationType();
            return config.Map<LineAssociationType, LineAssociationTypeDTO>(lineAssociationType);
        }

        internal static IEnumerable<LineAssociationTypeDTO> ToDTOs(this IEnumerable<LineAssociationType> lineAssociationTypes)
        {
            return lineAssociationTypes.Select(ToDTO);
        }

        internal static IMapper CreateMapByLineBusiness()
        {
            var config = MapperCache.GetMapper<ByLineBusiness, ByLineBusinessDTO>(cfg =>
            {
                cfg.CreateMap<ByLineBusiness, ByLineBusinessDTO>();
                cfg.CreateMap<LineBusiness, LineBusinessDTO>();
                cfg.CreateMap<LineAssociationType, LineAssociationTypeDTO>();
            });
            return config;
        }

        internal static ByLineBusinessDTO ToDTO(this ByLineBusiness byLineBusiness)
        {
            var config = CreateMapByLineBusiness();
            return config.Map<ByLineBusiness, ByLineBusinessDTO>(byLineBusiness);
        }

        internal static IEnumerable<ByLineBusinessDTO> ToDTOs(this IEnumerable<ByLineBusiness> byLineBusiness)
        {
            return byLineBusiness.Select(ToDTO);
        }

        internal static IMapper CreateMapByLineBusinessSubLineBusiness()
        {
            var config = MapperCache.GetMapper<ByLineBusinessSubLineBusiness, ByLineBusinessSubLineBusinessDTO>(cfg =>
            {
                cfg.CreateMap<ByLineBusinessSubLineBusiness, ByLineBusinessSubLineBusinessDTO>();
                cfg.CreateMap<LineBusiness, LineBusinessDTO>();
                cfg.CreateMap<SubLineBusiness, SubLineBusinessDTO>();
                cfg.CreateMap<LineAssociationType, LineAssociationTypeDTO>();
            });
            return config;
        }

        internal static ByLineBusinessSubLineBusinessDTO ToDTO(this ByLineBusinessSubLineBusiness byLineBusinessSubLineBusiness)
        {
            var config = CreateMapByLineBusinessSubLineBusiness();
            return config.Map<ByLineBusinessSubLineBusiness, ByLineBusinessSubLineBusinessDTO>(byLineBusinessSubLineBusiness);
        }

        internal static IEnumerable<ByLineBusinessSubLineBusinessDTO> ToDTOs(this IEnumerable<ByLineBusinessSubLineBusiness> byLineBusinessSubLineBusiness)
        {
            return byLineBusinessSubLineBusiness.Select(ToDTO);
        }

        internal static IMapper CreateMapByOperationTypePrefix()
        {
            var config = MapperCache.GetMapper<ByOperationTypePrefix, ByOperationTypePrefixDTO>(cfg =>
            {
                cfg.CreateMap<ByOperationTypePrefix, ByOperationTypePrefixDTO>();
                cfg.CreateMap<Prefix, PrefixDTO>();
                cfg.CreateMap<LineAssociationType, LineAssociationTypeDTO>();
            });
            return config;
        }

        internal static ByOperationTypePrefixDTO ToDTO(this ByOperationTypePrefix byOperationTypePrefix)
        {
            var config = CreateMapByOperationTypePrefix();
            return config.Map<ByOperationTypePrefix, ByOperationTypePrefixDTO>(byOperationTypePrefix);
        }

        internal static IEnumerable<ByOperationTypePrefixDTO> ToDTOs(this IEnumerable<ByOperationTypePrefix> byOperationTypePrefix)
        {
            return byOperationTypePrefix.Select(ToDTO);
        }

        internal static IMapper CreateMapByInsured()
        {
            var config = MapperCache.GetMapper<ByInsured, ByInsuredDTO>(cfg =>
            {
                cfg.CreateMap<ByInsured, ByInsuredDTO>();
                cfg.CreateMap<Individual, IndividualDTO>();
                cfg.CreateMap<LineAssociationType, LineAssociationTypeDTO>();
            });
            return config;
        }

        internal static ByInsuredDTO ToDTO(this ByInsured byInsured)
        {
            var config = CreateMapByInsured();
            return config.Map<ByInsured, ByInsuredDTO>(byInsured);
        }

        internal static IEnumerable<ByInsuredDTO> ToDTOs(this IEnumerable<ByInsured> byInsured)
        {
            return byInsured.Select(ToDTO);
        }

        internal static IMapper CreateMapByPolicy()
        {
            var config = MapperCache.GetMapper<ByPolicy, ByPolicyDTO>(cfg =>
            {
                cfg.CreateMap<ByPolicy, ByPolicyDTO>();
                cfg.CreateMap<TempCommonServices.Models.Policy, PolicyDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<Prefix, PrefixDTO>();
                cfg.CreateMap<TempCommonServices.Models.Endorsement, EndorsementDTO>();
                cfg.CreateMap<LineAssociationType, LineAssociationTypeDTO>();
            });
            return config;
        }

        internal static ByPolicyDTO ToDTO(this ByPolicy byPolicy)
        {
            var config = CreateMapByPolicy();
            return config.Map<ByPolicy, ByPolicyDTO>(byPolicy);
        }

        internal static IEnumerable<ByPolicyDTO> ToDTOs(this IEnumerable<ByPolicy> byPolicy)
        {
            return byPolicy.Select(ToDTO);
        }

        internal static IMapper CreateMapByFacultativeIssue()
        {
            var config = MapperCache.GetMapper<ByFacultativeIssue, ByFacultativeIssueDTO>(cfg =>
            {
                cfg.CreateMap<ByFacultativeIssue, ByFacultativeIssueDTO>();
                cfg.CreateMap<Prefix, PrefixDTO>();
                cfg.CreateMap<LineAssociationType, LineAssociationTypeDTO>();
            });
            return config;
        }

        internal static ByFacultativeIssueDTO ToDTO(this ByFacultativeIssue byFacultativeIssue)
        {
            var config = CreateMapByFacultativeIssue();
            return config.Map<ByFacultativeIssue, ByFacultativeIssueDTO>(byFacultativeIssue);
        }

        internal static IEnumerable<ByFacultativeIssueDTO> ToDTOs(this IEnumerable<ByFacultativeIssue> byFacultativeIssue)
        {
            return byFacultativeIssue.Select(ToDTO);
        }

        internal static IMapper CreateMapByInsuredPrefix()
        {
            var config = MapperCache.GetMapper<ByInsuredPrefix, ByInsuredPrefixDTO>(cfg =>
            {
                cfg.CreateMap<ByInsuredPrefix, ByInsuredPrefixDTO>();
                cfg.CreateMap<LineAssociationType, LineAssociationTypeDTO>();
                cfg.CreateMap<Prefix, PrefixDTO>();
                cfg.CreateMap<Individual, IndividualDTO>();
            });
            return config;
        }

        internal static ByInsuredPrefixDTO ToDTO(this ByInsuredPrefix byInsuredPrefix)
        {
            var config = CreateMapByInsuredPrefix();
            return config.Map<ByInsuredPrefix, ByInsuredPrefixDTO>(byInsuredPrefix);
        }

        internal static IEnumerable<ByInsuredPrefixDTO> ToDTOs(this IEnumerable<ByInsuredPrefix> byInsuredPrefix)
        {
            return byInsuredPrefix.Select(ToDTO);
        }

        internal static IMapper CreateMapByLineBusinessSubLineBusinessInsuredObject()
        {
            var config = MapperCache.GetMapper<ByLineBusinessSubLineBusinessInsuredObject, ByLineBusinessSubLineBusinessInsuredObjectDTO>(cfg =>
            {
                cfg.CreateMap<ByLineBusinessSubLineBusinessInsuredObjectDTO, ByLineBusinessSubLineBusinessInsuredObjectDTO>();
                cfg.CreateMap<LineBusiness, LineBusinessDTO>();
                cfg.CreateMap<SubLineBusiness, SubLineBusinessDTO>();
                cfg.CreateMap<InsuredObject, InsuredObjectDTO>();
                cfg.CreateMap<LineAssociationType, LineAssociationTypeDTO>();
            });
            return config;
        }

        internal static ByLineBusinessSubLineBusinessInsuredObjectDTO ToDTO(this ByLineBusinessSubLineBusinessInsuredObject byLineBusinessSubLineBusinessInsuredObject)
        {
            var config = CreateMapByLineBusinessSubLineBusinessInsuredObject();
            return config.Map<ByLineBusinessSubLineBusinessInsuredObject, ByLineBusinessSubLineBusinessInsuredObjectDTO>(byLineBusinessSubLineBusinessInsuredObject);
        }

        internal static IEnumerable<ByLineBusinessSubLineBusinessInsuredObjectDTO> ToDTOs(this IEnumerable<ByLineBusinessSubLineBusinessInsuredObject> byLineBusinessSubLineBusinessInsuredObject)
        {
            return byLineBusinessSubLineBusinessInsuredObject.Select(ToDTO);
        }

        internal static IMapper CreateMapByLineBusinessInsuredObject()
        {
            var config = MapperCache.GetMapper<ByLineBusinessInsuredObject, ByLineBusinessInsuredObjectDTO>(cfg =>
            {
                cfg.CreateMap<ByLineBusinessInsuredObject, ByLineBusinessInsuredObjectDTO>();
                cfg.CreateMap<LineBusiness, LineBusinessDTO>();
                cfg.CreateMap<InsuredObject, InsuredObjectDTO>();
                cfg.CreateMap<LineAssociationType, LineAssociationTypeDTO>();
            });
            return config;
        }

        internal static ByLineBusinessInsuredObjectDTO ToDTO(this ByLineBusinessInsuredObject byLineBusinessInsuredObject)
        {
            var config = CreateMapByLineBusinessInsuredObject();
            return config.Map<ByLineBusinessInsuredObject, ByLineBusinessInsuredObjectDTO>(byLineBusinessInsuredObject);
        }

        internal static IEnumerable<ByLineBusinessInsuredObjectDTO> ToDTOs(this IEnumerable<ByLineBusinessInsuredObject> byLineBusinessInsuredObject)
        {
            return byLineBusinessInsuredObject.Select(ToDTO);
        }

        internal static IMapper CreateMapByPolicyLineBusinessSubLineBusiness()
        {
            var config = MapperCache.GetMapper<ByPolicyLineBusinessSubLineBusiness, ByPolicyLineBusinessSubLineBusinessDTO>(cfg =>
            {
                cfg.CreateMap<ByPolicyLineBusinessSubLineBusiness, ByPolicyLineBusinessSubLineBusinessDTO>();
                cfg.CreateMap<TempCommonServices.Models.Policy, PolicyDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<Branch, BranchDTO>();
                cfg.CreateMap<Prefix, PrefixDTO>();
                cfg.CreateMap<TempCommonServices.Models.Endorsement, EndorsementDTO>();
                cfg.CreateMap<LineBusiness, LineBusinessDTO>();
                cfg.CreateMap<SubLineBusiness, SubLineBusinessDTO>();
                cfg.CreateMap<LineAssociationType, LineAssociationTypeDTO>();
            });
            return config;
        }

        internal static ByPolicyLineBusinessSubLineBusinessDTO ToDTO(this ByPolicyLineBusinessSubLineBusiness byLineBusinessSubLineBusiness)
        {
            var config = CreateMapByPolicyLineBusinessSubLineBusiness();
            return config.Map<ByPolicyLineBusinessSubLineBusiness, ByPolicyLineBusinessSubLineBusinessDTO>(byLineBusinessSubLineBusiness);
        }

        internal static IEnumerable<ByPolicyLineBusinessSubLineBusinessDTO> ToDTOs(this IEnumerable<ByPolicyLineBusinessSubLineBusiness> byLineBusinessSubLineBusiness)
        {
            return byLineBusinessSubLineBusiness.Select(ToDTO);
        }

        internal static IMapper CreateMapByLineBusinessSubLineBusinessCoverage()
        {
            var config = MapperCache.GetMapper<ByLineBusinessSubLineBusinessCoverage, ByLineBusinessSubLineBusinessCoverageDTO>(cfg =>
            {
                cfg.CreateMap<ByLineBusinessSubLineBusinessCoverage, ByLineBusinessSubLineBusinessCoverageDTO>();
                cfg.CreateMap<LineAssociationType, LineAssociationTypeDTO>();
                cfg.CreateMap<LineBusiness, LineBusinessDTO>();
                cfg.CreateMap<SubLineBusiness, SubLineBusinessDTO>();
                cfg.CreateMap<TempCommonServices.Models.Coverage, CoverageDTO>();
            });
            return config;
        }

        internal static ByLineBusinessSubLineBusinessCoverageDTO ToDTO(this ByLineBusinessSubLineBusinessCoverage byLineBusinessSubLineBusinessCoverage)
        {
            var config = CreateMapByLineBusinessSubLineBusinessCoverage();
            return config.Map<ByLineBusinessSubLineBusinessCoverage, ByLineBusinessSubLineBusinessCoverageDTO>(byLineBusinessSubLineBusinessCoverage);
        }

        internal static IEnumerable<ByLineBusinessSubLineBusinessCoverageDTO> ToDTOs(this IEnumerable<ByLineBusinessSubLineBusinessCoverage> byLineBusinessSubLineBusinessCoverage)
        {
            return byLineBusinessSubLineBusinessCoverage.Select(ToDTO);
        }

        internal static IMapper CreateMapByPrefixProduct()
        {
            var config = MapperCache.GetMapper<ByPrefixProduct, ByPrefixProductDTO>(cfg =>
            {
                cfg.CreateMap<ByPrefixProduct, ByPrefixProductDTO>();
                cfg.CreateMap<LineAssociationType, LineAssociationTypeDTO>();
                cfg.CreateMap<Prefix, PrefixDTO>();
            });
            return config;
        }

        internal static ByPrefixProductDTO ToDTO(this ByPrefixProduct byPrefixProduct)
        {
            var config = CreateMapByPrefixProduct();
            return config.Map<ByPrefixProduct, ByPrefixProductDTO>(byPrefixProduct);
        }

        internal static IEnumerable<ByPrefixProductDTO> ToDTOs(this IEnumerable<ByPrefixProduct> byPrefixProduct)
        {
            return byPrefixProduct.Select(ToDTO);
        }

        internal static IMapper CreateMapLineBusiness()
        {
            var config = MapperCache.GetMapper<LineBusiness, LineBusinessDTO>(cfg =>
            {
                cfg.CreateMap<LineBusiness, LineBusinessDTO>();
            });
            return config;
        }

        internal static LineBusinessDTO ToDTO(this LineBusiness lineBusiness)
        {
            var config = CreateMapLineBusiness();
            return config.Map<LineBusiness, LineBusinessDTO>(lineBusiness);
        }

        internal static IEnumerable<LineBusinessDTO> ToDTOs(this IEnumerable<LineBusiness> lineBusiness)
        {
            return lineBusiness.Select(ToDTO);
        }

        internal static IMapper CreateMapSubLineBusiness()
        {
            var config = MapperCache.GetMapper<SubLineBusiness, SubLineBusinessDTO>(cfg =>
            {
                cfg.CreateMap<SubLineBusiness, SubLineBusinessDTO>();
            });
            return config;
        }

        internal static SubLineBusinessDTO ToDTO(this SubLineBusiness sublineBusiness)
        {
            var config = CreateMapSubLineBusiness();
            return config.Map<SubLineBusiness, SubLineBusinessDTO>(sublineBusiness);
        }

        internal static IEnumerable<SubLineBusinessDTO> ToDTOs(this IEnumerable<SubLineBusiness> sublineBusiness)
        {
            return sublineBusiness.Select(ToDTO);
        }

        internal static IMapper CreateMapTempLayerDistributions()
        {
            var config = MapperCache.GetMapper<TempLayerDistributions, TempLayerDistributionsDTO>(cfg =>
            {
                cfg.CreateMap<TempLayerDistributions, TempLayerDistributionsDTO>();
            });
            return config;
        }

        internal static TempLayerDistributionsDTO ToDTO(this TempLayerDistributions tempLayerDistributions)
        {
            var config = CreateMapTempLayerDistributions();
            return config.Map<TempLayerDistributions, TempLayerDistributionsDTO>(tempLayerDistributions);
        }

        internal static IEnumerable<TempLayerDistributionsDTO> ToDTOs(this IEnumerable<TempLayerDistributions> tempLayerDistributions)
        {
            return tempLayerDistributions.Select(ToDTO);
        }

        internal static IMapper CreateMapTempLineCumulusIssuances()
        {
            var config = MapperCache.GetMapper<TempLineCumulusIssuance, TempLineCumulusIssuanceDTO>(cfg =>
            {
                cfg.CreateMap<TempLineCumulusIssuance, TempLineCumulusIssuanceDTO>();
            });
            return config;
        }

        internal static TempLineCumulusIssuanceDTO ToDTO(this TempLineCumulusIssuance tempLineCumulusIssuance)
        {
            var config = CreateMapTempLineCumulusIssuances();
            return config.Map<TempLineCumulusIssuance, TempLineCumulusIssuanceDTO>(tempLineCumulusIssuance);
        }

        internal static IEnumerable<TempLineCumulusIssuanceDTO> ToDTOs(this IEnumerable<TempLineCumulusIssuance> tempLineCumulusIssuance)
        {
            return tempLineCumulusIssuance.Select(ToDTO);
        }

        internal static IMapper CreateMapSelect()
        {
            var config = MapperCache.GetMapper<Select, SelectDTO>(cfg =>
            {
                cfg.CreateMap<Select, SelectDTO>();
            });
            return config;
        }

        internal static SelectDTO ToDTO(this Select select)
        {
            var config = CreateMapSelect();
            return config.Map<Select, SelectDTO>(select);
        }

        internal static IEnumerable<SelectDTO> ToDTOs(this IEnumerable<Select> select)
        {
            return select.Select(ToDTO);
        }

        internal static IMapper CreateMapReinsuranceFacultative()
        {
            var config = MapperCache.GetMapper<ReinsuranceFacultative, ReinsuranceFacultativeDTO>(cfg =>
            {
                cfg.CreateMap<ReinsuranceFacultative, ReinsuranceFacultativeDTO>();
            });
            return config;
        }

        internal static ReinsuranceFacultativeDTO ToDTO(this ReinsuranceFacultative reinsuranceFacultative)
        {
            var config = CreateMapReinsuranceFacultative();
            return config.Map<ReinsuranceFacultative, ReinsuranceFacultativeDTO>(reinsuranceFacultative);
        }

        internal static IEnumerable<ReinsuranceFacultativeDTO> ToDTOs(this IEnumerable<ReinsuranceFacultative> reinsuranceFacultative)
        {
            return reinsuranceFacultative.Select(ToDTO);
        }

        internal static IMapper CreateMapTempFacultativeCompany()
        {
            var config = MapperCache.GetMapper<TempFacultativeCompanies, TempFacultativeCompaniesDTO>(cfg =>
            {
                cfg.CreateMap<TempFacultativeCompanies, TempFacultativeCompaniesDTO>();
            });
            return config;
        }

        internal static TempFacultativeCompaniesDTO ToDTO(this TempFacultativeCompanies tempFacultativeCompanies)
        {
            var config = CreateMapTempFacultativeCompany();
            return config.Map<TempFacultativeCompanies, TempFacultativeCompaniesDTO>(tempFacultativeCompanies);
        }

        internal static IEnumerable<TempFacultativeCompaniesDTO> ToDTOs(this IEnumerable<TempFacultativeCompanies> tempFacultativeCompanies)
        {
            return tempFacultativeCompanies.Select(ToDTO);
        }
        
        internal static IMapper CreateMapClaimDistribution()
        {
            var config = MapperCache.GetMapper<ClaimDistribution, ClaimDistributionDTO>(cfg =>
            {
                cfg.CreateMap<ClaimDistribution, ClaimDistributionDTO>();
            });
            return config;
        }

        internal static ClaimDistributionDTO ToDTO(this ClaimDistribution claimsByMovementSource)
        {
            var config = CreateMapClaimDistribution();
            return config.Map<ClaimDistribution, ClaimDistributionDTO>(claimsByMovementSource);
        }

        internal static IEnumerable<ClaimDistributionDTO> ToDTOs(this IEnumerable<ClaimDistribution> claimsByMovementSource)
        {
            return claimsByMovementSource.Select(ToDTO);
        }

        internal static IMapper CreateMapClaimAllocation()
        {
            var config = MapperCache.GetMapper<ClaimAllocation, ClaimAllocationDTO>(cfg =>
            {
                cfg.CreateMap<ClaimAllocation, ClaimAllocationDTO>();
            });
            return config;
        }

        internal static ClaimAllocationDTO ToDTO(this ClaimAllocation claimAllocations)
        {
            var config = CreateMapClaimAllocation();
            return config.Map<ClaimAllocation, ClaimAllocationDTO>(claimAllocations);
        }

        internal static IEnumerable<ClaimAllocationDTO> ToDTOs(this IEnumerable<ClaimAllocation> claimAllocations)
        {
            return claimAllocations.Select(ToDTO);
        }

        internal static IMapper CreateMapReinsuranceRealClaimByClaimCode()
        {
            var config = MapperCache.GetMapper<ReinsuranceRealClaimByClaimCode, ReinsuranceRealClaimByClaimCodeDTO>(cfg =>
            {
                cfg.CreateMap<ReinsuranceRealClaimByClaimCode, ReinsuranceRealClaimByClaimCodeDTO>();
            });
            return config;
        }

        internal static ReinsuranceRealClaimByClaimCodeDTO ToDTO(this ReinsuranceRealClaimByClaimCode reinsuranceRealClaimByClaimCode)
        {
            var config = CreateMapReinsuranceRealClaimByClaimCode();
            return config.Map<ReinsuranceRealClaimByClaimCode, ReinsuranceRealClaimByClaimCodeDTO>(reinsuranceRealClaimByClaimCode);
        }

        internal static IEnumerable<ReinsuranceRealClaimByClaimCodeDTO> ToDTOs(this IEnumerable<ReinsuranceRealClaimByClaimCode> reinsuranceRealClaimByClaimCode)
        {
            return reinsuranceRealClaimByClaimCode.Select(ToDTO);
        }

        internal static IMapper CreateMapPaymentDistribution()
        {
            var config = MapperCache.GetMapper<PaymentDistribution, PaymentDistributionDTO>(cfg =>
            {
                cfg.CreateMap<PaymentDistribution, PaymentDistributionDTO>();
            });
            return config;
        }

        internal static PaymentDistributionDTO ToDTO(this PaymentDistribution paymentsByMovementSource)
        {
            var config = CreateMapPaymentDistribution();
            return config.Map<PaymentDistribution, PaymentDistributionDTO>(paymentsByMovementSource);
        }

        internal static IEnumerable<PaymentDistributionDTO> ToDTOs(this IEnumerable<PaymentDistribution> paymentsByMovementSource)
        {
            return paymentsByMovementSource.Select(ToDTO);
        }

        internal static IMapper CreateMapPaymentAllocation()
        {
            var config = MapperCache.GetMapper<PaymentAllocation, PaymentAllocationDTO>(cfg =>
            {
                cfg.CreateMap<PaymentAllocation, PaymentAllocationDTO>();
            });
            return config;
        }

        internal static PaymentAllocationDTO ToDTO(this PaymentAllocation paymentAllocations)
        {
            var config = CreateMapPaymentAllocation();
            return config.Map<PaymentAllocation, PaymentAllocationDTO>(paymentAllocations);
        }

        internal static IEnumerable<PaymentAllocationDTO> ToDTOs(this IEnumerable<PaymentAllocation> paymentAllocations)
        {
            return paymentAllocations.Select(ToDTO);
        }

        internal static IMapper CreateMapReinsuranceMassiveHeader()
        {
            var config = MapperCache.GetMapper<ReinsuranceMassiveHeader, ReinsuranceMassiveHeaderDTO>(cfg =>
            {
                cfg.CreateMap<ReinsuranceMassiveHeader, ReinsuranceMassiveHeaderDTO>();
            });
            return config;
        }

        internal static ReinsuranceMassiveHeaderDTO ToDTO(this ReinsuranceMassiveHeader reinsuranceMassiveHeader)
        {
            var config = CreateMapReinsuranceMassiveHeader();
            return config.Map<ReinsuranceMassiveHeader, ReinsuranceMassiveHeaderDTO>(reinsuranceMassiveHeader);
        }

        internal static IEnumerable<ReinsuranceMassiveHeaderDTO> ToDTOs(this IEnumerable<ReinsuranceMassiveHeader> reinsuranceMassiveHeader)
        {
            return reinsuranceMassiveHeader.Select(ToDTO);
        }

        internal static IMapper CreateMapTempAllocation()
        {
            var config = MapperCache.GetMapper<TempAllocation, TempAllocationDTO>(cfg =>
            {
                cfg.CreateMap<TempAllocation, TempAllocationDTO>();
            });
            return config;
        }

        internal static TempAllocationDTO ToDTO(this TempAllocation tempAllocation)
        {
            var config = CreateMapTempAllocation();
            return config.Map<TempAllocation, TempAllocationDTO>(tempAllocation);
        }

        internal static IEnumerable<TempAllocationDTO> ToDTOs(this IEnumerable<TempAllocation> tempAllocation)
        {
            return tempAllocation.Select(ToDTO);
        }

        internal static IMapper CreateMapLineCumulusKey()
        {
            var config = MapperCache.GetMapper<LineCumulusKey, LineCumulusKeyDTO>(cfg =>
            {
                cfg.CreateMap<LineCumulusKey, LineCumulusKeyDTO>();
                cfg.CreateMap<CumulusType, CumulusTypeDTO>();
                cfg.CreateMap<ContractLine, ContractLineDTO>();
                cfg.CreateMap<Contract, ContractDTO>();
                cfg.CreateMap<ResettlementType, ResettlementTypeDTO>();
                cfg.CreateMap<AffectationType, AffectationTypeDTO>();
                cfg.CreateMap<EPIType, EPITypeDTO>();
                cfg.CreateMap<ContractType, ContractTypeDTO>();
                cfg.CreateMap<ContractFunctionality, ContractFunctionalityDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
                cfg.CreateMap<Level, LevelDTO>()
                   .ForMember(dest => dest.PremiumType, opt => opt.MapFrom(src => (int)src.PremiumType))
                   .ForMember(dest => dest.ApplyOnType, opt => opt.MapFrom(src => (int)src.ApplyOnType))
                   .ForMember(dest => dest.CalculationType, opt => opt.MapFrom(src => (int)src.CalculationType));
                cfg.CreateMap<LevelPayment, LevelPaymentDTO>();
                cfg.CreateMap<LevelCompany, LevelCompanyDTO>()
                        .ForMember(dest => dest.PresentationInformationType, opt => opt.MapFrom(src => (int)src.PresentationInformationType));
                cfg.CreateMap<LevelRestore, LevelRestoreDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<Agent, AgentDTO>();
                cfg.CreateMap<LineCumulusKeyRiskCoverage, LineCumulusKeyRiskCoverageDTO>();
            });
            return config;
        }

        internal static LineCumulusKeyDTO ToDTO(this LineCumulusKey lineCumulusKey)
        {
            var config = CreateMapLineCumulusKey();
            return config.Map<LineCumulusKey, LineCumulusKeyDTO>(lineCumulusKey);
        }

        internal static IEnumerable<LineCumulusKeyDTO> ToDTOs(this IEnumerable<LineCumulusKey> lineCumulusKey)
        {
            return lineCumulusKey.Select(ToDTO);
        }

        internal static IMapper CreateMapLineCumulusKeyRiskCoverage()
        {
            var config = MapperCache.GetMapper<LineCumulusKeyRiskCoverage, LineCumulusKeyRiskCoverageDTO>(cfg =>
            {
                cfg.CreateMap<LineCumulusKeyRiskCoverage, LineCumulusKeyRiskCoverageDTO>();
            });
            return config;
        }

        internal static LineCumulusKeyRiskCoverageDTO ToDTO(this LineCumulusKeyRiskCoverage lineCumulusKeyRiskCoverage)
        {
            var config = CreateMapLineCumulusKeyRiskCoverage();
            return config.Map<LineCumulusKeyRiskCoverage, LineCumulusKeyRiskCoverageDTO>(lineCumulusKeyRiskCoverage);
        }

        internal static IEnumerable<LineCumulusKeyRiskCoverageDTO> ToDTOs(this IEnumerable<LineCumulusKeyRiskCoverage> lineCumulusKeyRiskCoverage)
        {
            return lineCumulusKeyRiskCoverage.Select(ToDTO);
        }

        internal static IMapper CreateMapPolicyByTEMPIntegration()
        {
            var config = MapperCache.GetMapper<TEMPINTDTO.PolicyDTO, PolicyDTO>(cfg =>
            {
                cfg.CreateMap<TEMPINTDTO.PolicyDTO, PolicyDTO>();
                cfg.CreateMap<TEMPINTDTO.BranchDTO, BranchDTO>();
                cfg.CreateMap<TEMPINTDTO.PrefixDTO, PrefixDTO>();
                cfg.CreateMap<TEMPINTDTO.CurrencyDTO, CurrencyDTO>();
                cfg.CreateMap<TEMPINTDTO.EndorsementDTO, EndorsementDTO>();
                cfg.CreateMap<TEMPINTDTO.RiskDTO, RiskDTO>();
                cfg.CreateMap<TEMPINTDTO.CoverageDTO, CoverageDTO>();
                cfg.CreateMap<TEMPINTDTO.AmountDTO, AmountDTO>();
            });
            return config;
        }

        internal static PolicyDTO ToDTO(this TEMPINTDTO.PolicyDTO policyDTO)
        {
            var config = CreateMapPolicyByTEMPIntegration();
            return config.Map<TEMPINTDTO.PolicyDTO, PolicyDTO>(policyDTO);
        }

        internal static IEnumerable<PolicyDTO> ToDTOs(this IEnumerable<TEMPINTDTO.PolicyDTO> policyDTO)
        {
            return policyDTO.Select(ToDTO);
        }

        internal static IMapper CreateMapMassiveReport()
        {
            var config = MapperCache.GetMapper<MassiveReport, MassiveReportDTO>(cfg =>
            {
                cfg.CreateMap<MassiveReport, MassiveReportDTO>();
            });
            return config;
        }

        internal static MassiveReportDTO ToDTO(this MassiveReport massiveReport)
        {
            var config = CreateMapMassiveReport();
            return config.Map<MassiveReport, MassiveReportDTO>(massiveReport);
        }

        internal static IEnumerable<MassiveReportDTO> ToDTOs(this IEnumerable<MassiveReport> massiveReport)
        {
            return massiveReport.Select(ToDTO);
        }

        internal static IMapper CreateMapEndorsement()
        {
            var config = MapperCache.GetMapper<TempCommonServices.Models.Endorsement, EndorsementDTO>(cfg =>
            {
                cfg.CreateMap<TempCommonServices.Models.Endorsement, EndorsementDTO>();
                cfg.CreateMap<TempCommonServices.Models.Risk, RiskDTO>();
                cfg.CreateMap<TempCommonServices.Models.Coverage, CoverageDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
                cfg.CreateMap<Currency, CurrencyDTO>();
            });
            return config;
        }

        internal static EndorsementDTO ToDTO(this TempCommonServices.Models.Endorsement endorsement)
        {
            var config = CreateMapEndorsement();
            return config.Map<TempCommonServices.Models.Endorsement, EndorsementDTO>(endorsement);
        }

        internal static IEnumerable<EndorsementDTO> ToDTOs(this IEnumerable<TempCommonServices.Models.Endorsement> endorsement)
        {
            return endorsement.Select(ToDTO);
        }

        internal static IMapper CreateMapModuleDate()
        {
            var config = MapperCache.GetMapper<TempCommonServices.Models.ModuleDate, ModuleDateDTO>(cfg =>
            {
                cfg.CreateMap<TempCommonServices.Models.ModuleDate, ModuleDateDTO>();
            });
            return config;
        }

        internal static ModuleDateDTO ToDTO(this TempCommonServices.Models.ModuleDate moduleDate)
        {
            var config = CreateMapModuleDate();
            return config.Map<TempCommonServices.Models.ModuleDate, ModuleDateDTO>(moduleDate);
        }

        internal static IEnumerable<ModuleDateDTO> ToDTOs(this IEnumerable<TempCommonServices.Models.ModuleDate> moduleDate)
        {
            return moduleDate.Select(ToDTO);
        }

        internal static TEMPINTDTO.EndorsementDTO CreateEndorsement(TempCommonServices.Models.Endorsement endorsement)
        {
            return new TEMPINTDTO.EndorsementDTO
            {
                Currency = endorsement.Currency,
                CurrentFrom = endorsement.CurrentFrom,
                CurrentTo = endorsement.CurrentTo,
                Description = endorsement.Description,
                EndorsementId = endorsement.EndorsementId,
                EndorsementNumber = endorsement.EndorsementNumber,
                InsuredAmount = endorsement.InsuredAmount,
                InsuredCd = endorsement.InsuredCd,
                InsuredName = endorsement.InsuredName,
                IssueDate = endorsement.IssueDate,
                OperationType = endorsement.OperationType,
                PolicyId = endorsement.PolicyId,
                Prime = endorsement.Prime,
                ResponsibilityMaximumAmount = endorsement.ResponsibilityMaximumAmount
            };
        }

        internal static List<TEMPINTDTO.EndorsementDTO> CreateEndorsements(List<TempCommonServices.Models.Endorsement> endorsements)
        {
            List<TEMPINTDTO.EndorsementDTO> endormsentDTOs = new List<TEMPINTDTO.EndorsementDTO>();
            foreach (TempCommonServices.Models.Endorsement endorsement in endorsements)
            {
                endormsentDTOs.Add(CreateEndorsement(endorsement));
            }
            return endormsentDTOs;
        }

        internal static ReinsuranceLayerIssuanceDTO CreateTempLayerDistribution(ReinsuranceLayerIssuance reinsuranceLayerIssuance)
        {
            return new ReinsuranceLayerIssuanceDTO()
            {
                TemporaryIssueId = reinsuranceLayerIssuance.TemporaryIssueId,
                LayerNumber = reinsuranceLayerIssuance.LayerNumber,
                LayerPercentage = reinsuranceLayerIssuance.LayerPercentage,
                LayerAmount = Convert.ToDecimal(reinsuranceLayerIssuance.LayerAmount.ToString() == "" ? "0" : reinsuranceLayerIssuance.LayerAmount.ToString()),
                PremiumPercentage = reinsuranceLayerIssuance.PremiumPercentage,
                PremiumAmount = Convert.ToDecimal(reinsuranceLayerIssuance.PremiumAmount.ToString() == "" ? "0" : reinsuranceLayerIssuance.PremiumAmount.ToString()),
                ReinsuranceLayerId = reinsuranceLayerIssuance.ReinsuranceLayerId
            };
        }

        internal static List<ReinsuranceLayerIssuanceDTO> CreateTempLayerDistributions(List<ReinsuranceLayerIssuance> reinsuranceLayerIssuances)
        {
            List<ReinsuranceLayerIssuanceDTO> reinsuranceLayerIssuanceDTOs = new List<ReinsuranceLayerIssuanceDTO>();
            foreach (ReinsuranceLayerIssuance reinsuranceLayerIssuance in reinsuranceLayerIssuances)
            {
                reinsuranceLayerIssuanceDTOs.Add(CreateTempLayerDistribution(reinsuranceLayerIssuance));
            }
            return reinsuranceLayerIssuanceDTOs;
        }

        internal static IMapper CreateMapModuleDateIntegration()
        {
            var config = MapperCache.GetMapper<ModuleDateDTO, TEMPINTDTO.ModuleDateDTO>(cfg =>
            {
                cfg.CreateMap<ModuleDateDTO, TEMPINTDTO.ModuleDateDTO>();
            });
            return config;
        }

        internal static TEMPINTDTO.ModuleDateDTO ToIntegrationDTO(this ModuleDateDTO moduleDate)
        {
            var config = CreateMapModuleDateIntegration();
            return config.Map<ModuleDateDTO, TEMPINTDTO.ModuleDateDTO>(moduleDate);
        }

        internal static IEnumerable<TEMPINTDTO.ModuleDateDTO> ToIntegrationDTOs(this IEnumerable<ModuleDateDTO> moduleDate)
        {
            return moduleDate.Select(ToIntegrationDTO);
        }

        internal static IMapper CreateMapReinsuranceByIntegration()
        {
            var config = MapperCache.GetMapper<REINSINTDTO.ReinsuranceDTO, ReinsuranceDTO>(cfg =>
            {
                cfg.CreateMap<REINSINTDTO.ReinsuranceDTO, ReinsuranceDTO>()
                    .ForMember(dest => dest.Movements, opt => opt.MapFrom(src => (int)src.Movements));
                cfg.CreateMap<REINSINTDTO.ReinsuranceLayerDTO, ReinsuranceLayerDTO>();
                cfg.CreateMap<REINSINTDTO.ReinsuranceLineDTO, ReinsuranceLineDTO>();
                cfg.CreateMap<REINSINTDTO.LineDTO, LineDTO>();
                cfg.CreateMap<REINSINTDTO.CumulusTypeDTO, CumulusTypeDTO>();
                cfg.CreateMap<REINSINTDTO.ContractLineDTO, ContractLineDTO>();
                cfg.CreateMap<REINSINTDTO.ContractDTO, ContractDTO>();
                cfg.CreateMap<REINSINTDTO.CurrencyDTO, CurrencyDTO>();
                cfg.CreateMap<REINSINTDTO.ContractFunctionalityDTO, ContractFunctionalityDTO>();
                cfg.CreateMap<REINSINTDTO.ContractTypeDTO, ContractTypeDTO>();
                cfg.CreateMap<REINSINTDTO.ReinsuranceAllocationDTO, ReinsuranceAllocationDTO>();
                cfg.CreateMap<REINSINTDTO.ReinsuranceCumulusRiskCoverageDTO, ReinsuranceCumulusRiskCoverageDTO>();
                cfg.CreateMap<REINSINTDTO.ResettlementTypeDTO, ResettlementTypeDTO>();
                cfg.CreateMap<REINSINTDTO.AffectationTypeDTO, AffectationTypeDTO>();
                cfg.CreateMap<REINSINTDTO.EPITypeDTO, EPITypeDTO>();
                cfg.CreateMap<REINSINTDTO.LevelDTO, LevelDTO>()
                   .ForMember(dest => dest.PremiumType, opt => opt.MapFrom(src => (int)src.PremiumType))
                   .ForMember(dest => dest.ApplyOnType, opt => opt.MapFrom(src => (int)src.ApplyOnType))
                   .ForMember(dest => dest.CalculationType, opt => opt.MapFrom(src => (int)src.CalculationType));
                cfg.CreateMap<REINSINTDTO.AgentDTO, AgentDTO>();
                cfg.CreateMap<REINSINTDTO.CompanyDTO, CompanyDTO>();
                cfg.CreateMap<REINSINTDTO.LevelCompanyDTO, LevelCompanyDTO>()
                        .ForMember(dest => dest.PresentationInformationType, opt => opt.MapFrom(src => (int)src.PresentationInformationType));
                cfg.CreateMap<REINSINTDTO.LevelPaymentDTO, LevelPaymentDTO>();
                cfg.CreateMap<REINSINTDTO.InstallmentDTO, InstallmentDTO>();
                cfg.CreateMap<REINSINTDTO.LevelRestoreDTO, LevelRestoreDTO>();
                cfg.CreateMap<REINSINTDTO.AmountDTO, AmountDTO>();


            });
            return config;
        }

        internal static ReinsuranceDTO ToDTO(this REINSINTDTO.ReinsuranceDTO reinsuranceDTO)
        {
            var config = CreateMapReinsuranceByIntegration();
            return config.Map<REINSINTDTO.ReinsuranceDTO, ReinsuranceDTO>(reinsuranceDTO);
        }

        internal static IEnumerable<ReinsuranceDTO> ToDTOs(this IEnumerable<REINSINTDTO.ReinsuranceDTO> reinsuranceDTO)
        {
            return reinsuranceDTO.Select(ToDTO);
        }

        internal static IMapper CreateMapInsuredByUNDIntegration()
        {
            var config = MapperCache.GetMapper<Integration.UndewritingIntegrationServices.DTOs.InsuredDTO, InsuredDTO>(cfg =>
            {
                cfg.CreateMap<Integration.UndewritingIntegrationServices.DTOs.InsuredDTO, InsuredDTO>();
            });
            return config;
        }

        internal static InsuredDTO ToDTO(this Integration.UndewritingIntegrationServices.DTOs.InsuredDTO insuredDTO)
        {
            var config = CreateMapInsuredByUNDIntegration();
            return config.Map<Integration.UndewritingIntegrationServices.DTOs.InsuredDTO, InsuredDTO>(insuredDTO);
        }

        internal static IEnumerable<InsuredDTO> ToDTOs(this IEnumerable<Integration.UndewritingIntegrationServices.DTOs.InsuredDTO> insuredDTO)
        {
            return insuredDTO.Select(ToDTO);
        }

        internal static IMapper CreateMapUserIntegrationByUUIntegration()
        {
            var config = MapperCache.GetMapper<UUINTDTO.UserDTO, REINSINTDTO.UserDTO>(cfg =>
            {
                cfg.CreateMap<UUINTDTO.UserDTO, REINSINTDTO.UserDTO>()
                    .ForMember(dest => dest.AuthenticationType, opt => opt.MapFrom(src => src.AuthenticationType));
                cfg.CreateMap<UUINTDTO.BranchDTO, REINSINTDTO.BranchDTO>();
                cfg.CreateMap<UUINTDTO.CoHierarchyAssociationDTO, REINSINTDTO.CoHierarchyAssociationDTO>();
                cfg.CreateMap<UUINTDTO.UniqueUserLoginDTO, REINSINTDTO.UniqueUserLoginDTO>();
                cfg.CreateMap<UUINTDTO.ProfileDTO, REINSINTDTO.ProfileDTO>();
                cfg.CreateMap<UUINTDTO.IndividualRelationAppDTO, REINSINTDTO.IndividualRelationAppDTO>();
                cfg.CreateMap<UUINTDTO.PrefixDTO, REINSINTDTO.PrefixDTO>();
                cfg.CreateMap<UUINTDTO.SalePointDTO, REINSINTDTO.SalePointDTO>();
                cfg.CreateMap<UUINTDTO.ModuleDTO, REINSINTDTO.ModuleDTO>();
                cfg.CreateMap<UUINTDTO.SubModuleDTO, REINSINTDTO.SubModuleDTO>();
                cfg.CreateMap<UUINTDTO.AccessProfileDTO, REINSINTDTO.AccessProfileDTO>();
                cfg.CreateMap<UUINTDTO.LineBusinessDTO, REINSINTDTO.LineBusinessDTO>();
                cfg.CreateMap<UUINTDTO.PrefixTypeDTO, REINSINTDTO.PrefixTypeDTO>();
            });
            return config;
        }

        internal static REINSINTDTO.UserDTO ToIntegrationDTO(this UUINTDTO.UserDTO userDTO)
        {
            var config = CreateMapUserIntegrationByUUIntegration();
            return config.Map<UUINTDTO.UserDTO, REINSINTDTO.UserDTO>(userDTO);
        }

        internal static IEnumerable<REINSINTDTO.UserDTO> ToIntegrationDTOs(this IEnumerable<UUINTDTO.UserDTO> userDTO)
        {
            return userDTO.Select(ToIntegrationDTO);
        }

        internal static IMapper CreateMapPolicyByIntegration()
        {
            var config = MapperCache.GetMapper<REINSINTDTO.PolicyDTO, PolicyDTO>(cfg =>
            {
                cfg.CreateMap<REINSINTDTO.PolicyDTO, PolicyDTO>();
                cfg.CreateMap<REINSINTDTO.ProductDTO, ProductDTO>();
                cfg.CreateMap<REINSINTDTO.BillingGroupDTO, BillingGroupDTO>();
                cfg.CreateMap<REINSINTDTO.HolderDTO, HolderDTO>();
                cfg.CreateMap<REINSINTDTO.IssuanceAgencyDTO, IssuanceAgencyDTO>();
                cfg.CreateMap<REINSINTDTO.IssuanceAgentTypeDTO, IssuanceAgentTypeDTO>();
                cfg.CreateMap<REINSINTDTO.PaymentPlanDTO, PaymentPlanDTO>();
                cfg.CreateMap<REINSINTDTO.PayerComponentDTO, PayerComponentDTO>();
                cfg.CreateMap<REINSINTDTO.BeneficiaryDTO, BeneficiaryDTO>();
                cfg.CreateMap<REINSINTDTO.ExchangeRateDTO, ExchangeRateDTO>();
                cfg.CreateMap<REINSINTDTO.BeneficiaryDTO, BeneficiaryDTO>();
                cfg.CreateMap<REINSINTDTO.CurrencyDTO, CurrencyDTO>();
                cfg.CreateMap<REINSINTDTO.PrefixDTO, PrefixDTO>();
                cfg.CreateMap<REINSINTDTO.BranchDTO, BranchDTO>();
                cfg.CreateMap<REINSINTDTO.PolicyTypeDTO, PolicyTypeDTO>();
                cfg.CreateMap<REINSINTDTO.IssuanceAgentDTO, IssuanceAgentDTO>();
                cfg.CreateMap<REINSINTDTO.IssuanceCommissionDTO, IssuanceCommissionDTO>();
                cfg.CreateMap<REINSINTDTO.LineBusinessDTO, LineBusinessDTO>();
                cfg.CreateMap<REINSINTDTO.SubLineBusinessDTO, SubLineBusinessDTO>();
                cfg.CreateMap<REINSINTDTO.PrefixTypeDTO, PrefixTypeDTO>();
                cfg.CreateMap<REINSINTDTO.SalePointDTO, SalePointDTO>();
                cfg.CreateMap<REINSINTDTO.QuotaDTO, QuotaDTO>();
                cfg.CreateMap<REINSINTDTO.EndorsementDTO, EndorsementDTO>();
                cfg.CreateMap<REINSINTDTO.RiskDTO, RiskDTO>();
                cfg.CreateMap<REINSINTDTO.CoverageDTO, CoverageDTO>();
                cfg.CreateMap<REINSINTDTO.AmountDTO, AmountDTO>();
            });
            return config;
        }

        internal static PolicyDTO ToDTO(this REINSINTDTO.PolicyDTO policyDTO)
        {
            var config = CreateMapPolicyByIntegration();
            return config.Map<REINSINTDTO.PolicyDTO, PolicyDTO>(policyDTO);
        }

        internal static IEnumerable<PolicyDTO> ToDTOs(this IEnumerable<REINSINTDTO.PolicyDTO> policyDTO)
        {
            return policyDTO.Select(ToDTO);
        }

        internal static IMapper CreateMapLinesByIntegratation()
        {
            var config = MapperCache.GetMapper<REINSINTDTO.LineDTO, LineDTO>(cfg =>
            {
                cfg.CreateMap<REINSINTDTO.LineDTO, LineDTO>();
                cfg.CreateMap<REINSINTDTO.CumulusTypeDTO, CumulusTypeDTO>();
                cfg.CreateMap<REINSINTDTO.ContractLineDTO, ContractLineDTO>();
                cfg.CreateMap<REINSINTDTO.ContractDTO, ContractDTO>();
                cfg.CreateMap<REINSINTDTO.ResettlementTypeDTO, ResettlementTypeDTO>();
                cfg.CreateMap<REINSINTDTO.AffectationTypeDTO, AffectationTypeDTO>();
                cfg.CreateMap<REINSINTDTO.EPITypeDTO, EPITypeDTO>();
                cfg.CreateMap<REINSINTDTO.ContractTypeDTO, ContractTypeDTO>();
                cfg.CreateMap<REINSINTDTO.ContractFunctionalityDTO, ContractFunctionalityDTO>();
                cfg.CreateMap<REINSINTDTO.CurrencyDTO, CurrencyDTO>();
                cfg.CreateMap<REINSINTDTO.LevelDTO, LevelDTO>()
                   .ForMember(dest => dest.PremiumType, opt => opt.MapFrom(src => (int)src.PremiumType))
                   .ForMember(dest => dest.ApplyOnType, opt => opt.MapFrom(src => (int)src.ApplyOnType))
                   .ForMember(dest => dest.CalculationType, opt => opt.MapFrom(src => (int)src.CalculationType));
                cfg.CreateMap<REINSINTDTO.LevelPaymentDTO, LevelPaymentDTO>();
                cfg.CreateMap<REINSINTDTO.LevelCompanyDTO, LevelCompanyDTO>()
                        .ForMember(dest => dest.PresentationInformationType, opt => opt.MapFrom(src => (int)src.PresentationInformationType));
                cfg.CreateMap<REINSINTDTO.LevelRestoreDTO, LevelRestoreDTO>();
                cfg.CreateMap<REINSINTDTO.AmountDTO, AmountDTO>();
                cfg.CreateMap<REINSINTDTO.AgentDTO, AgentDTO>();
            });
            return config;
        }

        internal static LineDTO ToDTO(this REINSINTDTO.LineDTO lineDTO)
        {
            var config = CreateMapLinesByIntegratation();
            return config.Map<REINSINTDTO.LineDTO, LineDTO>(lineDTO);
        }

        internal static IEnumerable<LineDTO> ToDTOs(this IEnumerable<REINSINTDTO.LineDTO> lineDTO)
        {
            return lineDTO.Select(ToDTO);
        }

        internal static OperatingQuotaEventDTO CreateOperatingQuotaEventDTO(ROQINTDTO.OperatingQuotaEventDTO operatingQuotaEventDTO)
        {
            return new OperatingQuotaEventDTO()
            {
                Cov_End_Date = operatingQuotaEventDTO.Cov_End_Date,
                Cov_Init_Date = operatingQuotaEventDTO.Cov_Init_Date,
                IdentificationId = operatingQuotaEventDTO.IdentificationId,
                IssueDate = operatingQuotaEventDTO.IssueDate,
                LineBusinessID = operatingQuotaEventDTO.LineBusinessID,
                OperatingQuotaEventID = operatingQuotaEventDTO.OperatingQuotaEventID,
                OperatingQuotaEventType = operatingQuotaEventDTO.OperatingQuotaEventType,
                payload = operatingQuotaEventDTO.payload,
                Policy_End_Date = operatingQuotaEventDTO.Policy_End_Date,
                Policy_Init_Date = operatingQuotaEventDTO.Policy_Init_Date,
                ApplyReinsurance = new ApplyReinsuranceDTO()
                {
                    ContractCoverage = CreateContractsCoverageDTO(operatingQuotaEventDTO.ApplyReinsurance.ContractCoverage),
                    CoverageId = operatingQuotaEventDTO.ApplyReinsurance.CoverageId,
                    CurrencyType = operatingQuotaEventDTO.ApplyReinsurance.CurrencyType,
                    CurrencyTypeDesc = operatingQuotaEventDTO.ApplyReinsurance.CurrencyTypeDesc,
                    DocumentNum = operatingQuotaEventDTO.ApplyReinsurance.DocumentNum,
                    EndorsementId = operatingQuotaEventDTO.ApplyReinsurance.EndorsementId,
                    EndorsementType = operatingQuotaEventDTO.ApplyReinsurance.EndorsementType,
                    IndividualId = operatingQuotaEventDTO.ApplyReinsurance.IndividualId,
                    ConsortiumId = operatingQuotaEventDTO.ApplyReinsurance.ConsortiumId,
                    EconomicGroupId = operatingQuotaEventDTO.ApplyReinsurance.EconomicGroupId,
                    ParticipationPercentage = operatingQuotaEventDTO.ApplyReinsurance.ParticipationPercentage,
                    PolicyID = operatingQuotaEventDTO.ApplyReinsurance.PolicyID,
                    PrefixId = operatingQuotaEventDTO.ApplyReinsurance.PrefixId,
                    BranchId = operatingQuotaEventDTO.ApplyReinsurance.BranchId
                }
            };
        }

        internal static List<OperatingQuotaEventDTO> CreateOperatingQuotasEventDTO(List<ROQINTDTO.OperatingQuotaEventDTO> operatingQuotaEventIntegrationDTOs)
        {
            List<OperatingQuotaEventDTO> operatingQuotaEventDTOs = new List<OperatingQuotaEventDTO>();
            foreach (ROQINTDTO.OperatingQuotaEventDTO operatingQuotaEventIntegrationDTO in operatingQuotaEventIntegrationDTOs)
            {
                operatingQuotaEventDTOs.Add(CreateOperatingQuotaEventDTO(operatingQuotaEventIntegrationDTO));
            }
            return operatingQuotaEventDTOs;
        }

        internal static IMapper CreateMapROQINTDTOOperatingQuotaByOperatingQuotaEventDTO()
        {
            var config = MapperCache.GetMapper<OperatingQuotaEventDTO, ROQINTDTO.OperatingQuotaEventDTO>(cfg =>
            {
                cfg.CreateMap<ROQINTDTO.OperatingQuotaEventDTO, OperatingQuotaEventDTO>();
                cfg.CreateMap<ROQINTDTO.ApplyReinsuranceDTO, ApplyReinsuranceDTO>();
                cfg.CreateMap<ROQINTDTO.ContractCoverageDTO, ContractCoverageDTO>();
            });
            return config;
        }

        internal static ROQINTDTO.OperatingQuotaEventDTO CreateROQINTDTOOperatingQuotaByOperatingQuotaEventDTO(this OperatingQuotaEventDTO operatingQuotaEventDTO)
        {
            var config = CreateMapROQINTDTOOperatingQuotaByOperatingQuotaEventDTO();
            return config.Map<OperatingQuotaEventDTO, ROQINTDTO.OperatingQuotaEventDTO>(operatingQuotaEventDTO);
        }

        internal static List<ROQINTDTO.OperatingQuotaEventDTO> CreateROQINTDTOOperatingQuotaByOperatingQuotaEventDTOs(this List<OperatingQuotaEventDTO> operatingQuotaEventDTOs)
        {
            var config = CreateMapROQINTDTOOperatingQuotaByOperatingQuotaEventDTO();
            return config.Map<List<OperatingQuotaEventDTO>, List<ROQINTDTO.OperatingQuotaEventDTO>>(operatingQuotaEventDTOs);
        }
        
        internal static ContractCoverageDTO CreateContractCoverageDTO(ROQINTDTO.ContractCoverageDTO contractCoverageIntegrationDTO)
        {
            return new ContractCoverageDTO()
            {
                Amount = contractCoverageIntegrationDTO.Amount,
                LevelLimit = contractCoverageIntegrationDTO.LevelLimit,
                ContractDescription = contractCoverageIntegrationDTO.ContractDescription,
                ContractCurrencyId = contractCoverageIntegrationDTO.ContractCurrencyId,
                ContractId = contractCoverageIntegrationDTO.ContractId,
                Premium = contractCoverageIntegrationDTO.Premium
            };
        }

        internal static List<ContractCoverageDTO> CreateContractsCoverageDTO(List<ROQINTDTO.ContractCoverageDTO> contractCoverageIntegrationDTOs)
        {
            List<ContractCoverageDTO> contractCoverageDTOs = new List<ContractCoverageDTO>();

            foreach (ROQINTDTO.ContractCoverageDTO contractCoverageIntegrationDTO in contractCoverageIntegrationDTOs)
            {
                contractCoverageDTOs.Add(CreateContractCoverageDTO(contractCoverageIntegrationDTO));
            }
            return contractCoverageDTOs;
        }

        internal static IMapper CreateMapIssueAllocationRiskCover()
        {
            var config = MapperCache.GetMapper<IssueAllocationRiskCoverDTO, IssueAllocationRiskCover>(cfg =>
            {
                cfg.CreateMap<IssueAllocationRiskCoverDTO, IssueAllocationRiskCover>();
            });
            return config;
        }

        internal static IssueAllocationRiskCoverDTO ToDTO(this IssueAllocationRiskCover issueAllocationRiskCover)
        {
            var config = CreateMapIssueAllocationRiskCover();
            return config.Map<IssueAllocationRiskCover, IssueAllocationRiskCoverDTO>(issueAllocationRiskCover);
        }

        internal static IEnumerable<IssueAllocationRiskCoverDTO> ToDTOs(this IEnumerable<IssueAllocationRiskCover> issueAllocationRiskCover)
        {
            return issueAllocationRiskCover.Select(ToDTO);
        }


        internal static IMapper CreateMapOperatingQuotaEventDTOByIssueAllocationRiskCoverDTO()
        {
            var config = MapperCache.GetMapper<IssueAllocationRiskCover, OperatingQuotaEventDTO>(cfg =>
            {
                cfg.CreateMap<IssueAllocationRiskCover, OperatingQuotaEventDTO>()
                .ForMember(dest => dest.Policy_Init_Date, opt => opt.MapFrom(src => src.PolicyCurrentFrom))
                .ForMember(dest => dest.Policy_End_Date, opt => opt.MapFrom(src => src.PolicyCurrentTo))
                .ForMember(dest => dest.Cov_Init_Date, opt => opt.MapFrom(src => src.CoverCurrentFrom))
                .ForMember(dest => dest.Cov_End_Date, opt => opt.MapFrom(src => src.CoverCurrentTo))
                .ForMember(dest => dest.LineBusinessID, opt => opt.MapFrom(src => src.LineBusinessCd))
                .ForMember(dest => dest.SubLineBusinessID, opt => opt.MapFrom(src => src.SubLineBusinessCd));

                cfg.CreateMap<IssueAllocationRiskCover, ApplyReinsuranceDTO>()
                .ForMember(dest => dest.PolicyID, opt => opt.MapFrom(src => src.PolicyId))
                .ForMember(dest => dest.DocumentNum, opt => opt.MapFrom(src => src.DocumentNum))
                .ForMember(dest => dest.EndorsementId, opt => opt.MapFrom(src => src.EndorsementId))
                .ForMember(dest => dest.CoverageId, opt => opt.MapFrom(src => src.CoverageId))
                .ForMember(dest => dest.EndorsementType, opt => opt.MapFrom(src => src.EndorsementNumber))
                .ForMember(dest => dest.CurrencyType, opt => opt.MapFrom(src => src.CurrencyId))
                .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.BranchCd))
                .ForMember(dest => dest.PrefixId, opt => opt.MapFrom(src => src.PrefixCd))
                .ForMember(dest => dest.RiskId, opt => opt.MapFrom(src => src.RiskId));
            });
            return config;
        }

        internal static OperatingQuotaEventDTO CreateOperatingQuotaEventDTOByIssueAllocationRiskCoverDTO(this IssueAllocationRiskCover issueAllocationRiskCover)
        {
            var config = CreateMapOperatingQuotaEventDTOByIssueAllocationRiskCoverDTO();
            return config.Map<IssueAllocationRiskCover, OperatingQuotaEventDTO>(issueAllocationRiskCover);
        }

        internal static IMapper CreateMapApplyReinsuranceDTOByIssueAllocationRiskCoverDTO()
        {
            var config = MapperCache.GetMapper<IssueAllocationRiskCover, ApplyReinsuranceDTO>(cfg =>
            {
                cfg.CreateMap<IssueAllocationRiskCover, ApplyReinsuranceDTO>()
                .ForMember(dest => dest.PolicyID, opt => opt.MapFrom(src => src.PolicyId))
                .ForMember(dest => dest.DocumentNum, opt => opt.MapFrom(src => src.DocumentNum))
                .ForMember(dest => dest.EndorsementId, opt => opt.MapFrom(src => src.EndorsementId))
                .ForMember(dest => dest.CoverageId, opt => opt.MapFrom(src => src.CoverageId))
                .ForMember(dest => dest.EndorsementType, opt => opt.MapFrom(src => src.EndorsementNumber))
                .ForMember(dest => dest.CurrencyType, opt => opt.MapFrom(src => src.CurrencyId))
                .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.BranchCd))
                .ForMember(dest => dest.PrefixId, opt => opt.MapFrom(src => src.PrefixCd))
                .ForMember(dest => dest.RiskId, opt => opt.MapFrom(src => src.RiskId));
            });
            return config;
        }

        internal static ApplyReinsuranceDTO CreateApplyReinsuranceDTOByIssueAllocationRiskCoverDTO(this IssueAllocationRiskCover issueAllocationRiskCover)
        {
            var config = CreateMapOperatingQuotaEventDTOByIssueAllocationRiskCoverDTO();
            return config.Map<IssueAllocationRiskCover, ApplyReinsuranceDTO>(issueAllocationRiskCover);
        }

        internal static IMapper CreateMapContractCoverageDTOByIssueAllocationRiskCover()
        {
            var config = MapperCache.GetMapper<IssueAllocationRiskCover, ContractCoverageDTO>(cfg =>
            {
                cfg.CreateMap<IssueAllocationRiskCover, ContractCoverageDTO>()
                .ForMember(dest => dest.ContractDescription, opt => opt.MapFrom(src => src.SmallDescription))
                .ForMember(dest => dest.ContractCurrencyId, opt => opt.MapFrom(src => src.ContractCurrencyCd));
            });
            return config;
        }

        internal static ContractCoverageDTO CreateContractCoverageDTOByIssueAllocationRiskCover(this IssueAllocationRiskCover issueAllocationRiskCover)
        {
            var config = CreateMapContractCoverageDTOByIssueAllocationRiskCover();
            return config.Map<IssueAllocationRiskCover, ContractCoverageDTO>(issueAllocationRiskCover);
        }

        internal static List<ContractCoverageDTO> CreateContractCoverageDTOsByIssueAllocationRiskCovers(this List<IssueAllocationRiskCover> issueAllocationRiskCovers)
        {
            var config = CreateMapContractCoverageDTOByIssueAllocationRiskCover();
            return config.Map<List<IssueAllocationRiskCover>, List<ContractCoverageDTO>>(issueAllocationRiskCovers);
        }

        internal static CoverageReinsuranceCumulusDTO CreateCoverageReinsuranceCumulusDTOByOperatingQuotaEventDTO(OperatingQuotaEventDTO operatingQuotaEventDTO)
        {
            return new CoverageReinsuranceCumulusDTO
            {
                PolicyID = operatingQuotaEventDTO.ApplyReinsurance.PolicyID,
                PolicyCurrentFrom = operatingQuotaEventDTO.Policy_Init_Date,
                PolicyCurrentTo = operatingQuotaEventDTO.Policy_End_Date,
                EndorsementId = operatingQuotaEventDTO.ApplyReinsurance.EndorsementId,
                EndorsementType = operatingQuotaEventDTO.ApplyReinsurance.EndorsementType,
                DocumentNum = operatingQuotaEventDTO.ApplyReinsurance.DocumentNum,
                CoverageId = operatingQuotaEventDTO.ApplyReinsurance.CoverageId,
                CoverageCurrentFrom = operatingQuotaEventDTO.Cov_Init_Date,
                CoverageCurrentTo = operatingQuotaEventDTO.Cov_End_Date,
                Insured = new InsuredDTO
                {
                    IndividualId = operatingQuotaEventDTO.ApplyReinsurance.IndividualId,
                },
                Consortium = new InsuredDTO
                {
                    IndividualId = operatingQuotaEventDTO.ApplyReinsurance.ConsortiumId
                },
                EconomicGroup = new EconomicGroupDTO
                {
                    EconomicGroupId = operatingQuotaEventDTO.ApplyReinsurance.EconomicGroupId
                },
                Branch = new BranchDTO
                {
                    Id = operatingQuotaEventDTO.ApplyReinsurance.BranchId
                },
                Prefix = new PrefixDTO
                {
                    Id = operatingQuotaEventDTO.ApplyReinsurance.PrefixId
                },
                Currency = new CurrencyDTO
                {
                    Id = operatingQuotaEventDTO.ApplyReinsurance.CurrencyType,
                },
                ContractReinsuranceCumulus = new ContractReinsuranceCumulusDTO
                {

                },
                Coverage = new CoverageDTO
                {
                    Id = operatingQuotaEventDTO.ApplyReinsurance.CoverageId,
                    LimitAmount = new AmountDTO
                    {

                    }
                }

            };
        }

        internal static IMapper CreateMapOperatingQuotaByIntegration()
        {
            var config = MapperCache.GetMapper<REINSINTDTO.OperatingQuotaEventDTO, OperatingQuotaEventDTO>(cfg =>
            {
                cfg.CreateMap<REINSINTDTO.OperatingQuotaEventDTO, OperatingQuotaEventDTO>();
            });
            return config;
        }

        internal static OperatingQuotaEventDTO ToDTO(this REINSINTDTO.OperatingQuotaEventDTO operatingQuotaEventDTO)
        {
            var config = CreateMapOperatingQuotaByIntegration();
            return config.Map<REINSINTDTO.OperatingQuotaEventDTO, OperatingQuotaEventDTO>(operatingQuotaEventDTO);
        }

        internal static List<OperatingQuotaEventDTO> ToDTOs(this List<REINSINTDTO.OperatingQuotaEventDTO> operatingQuotaEventDTOs)
        {
            var config = CreateMapOperatingQuotaByIntegration();
            return config.Map<List<REINSINTDTO.OperatingQuotaEventDTO>, List<OperatingQuotaEventDTO>>(operatingQuotaEventDTOs);
        }

        internal static IMapper CreateMapOperatingQuotaIntegration()
        {
            var config = MapperCache.GetMapper<OperatingQuotaEventDTO, REINSINTDTO.OperatingQuotaEventDTO>(cfg =>
            {
                cfg.CreateMap<OperatingQuotaEventDTO, REINSINTDTO.OperatingQuotaEventDTO>();
            });
            return config;
        }

        internal static REINSINTDTO.OperatingQuotaEventDTO ToIntegrationDTO(this OperatingQuotaEventDTO operatingQuotaEventDTO)
        {
            var config = CreateMapOperatingQuotaIntegration();
            return config.Map<OperatingQuotaEventDTO, REINSINTDTO.OperatingQuotaEventDTO>(operatingQuotaEventDTO);
        }

        internal static IEnumerable<REINSINTDTO.OperatingQuotaEventDTO> ToIntegrationDTOs(this IEnumerable<OperatingQuotaEventDTO> operatingQuotaEventDTOs)
        {
            return operatingQuotaEventDTOs.Select(ToIntegrationDTO);
        }

        internal static IMapper CreateMapApplyReinsuranceIntegration()
        {
            var config = MapperCache.GetMapper<ApplyReinsuranceDTO, REINSINTDTO.ApplyReinsuranceDTO>(cfg =>
            {
                cfg.CreateMap<ApplyReinsuranceDTO, REINSINTDTO.ApplyReinsuranceDTO>();
            });
            return config;
        }

        internal static REINSINTDTO.ApplyReinsuranceDTO ToIntegrationDTO(this ApplyReinsuranceDTO applyReinsuranceDTO)
        {
            var config = CreateMapOperatingQuotaIntegration();
            return config.Map<ApplyReinsuranceDTO, REINSINTDTO.ApplyReinsuranceDTO>(applyReinsuranceDTO);
        }
        
        internal static IMapper CreateMapContractCoverageIntegration()
        {
            var config = MapperCache.GetMapper<ContractCoverageDTO, REINSINTDTO.ContractCoverageDTO>(cfg =>
            {
                cfg.CreateMap<ContractCoverageDTO, REINSINTDTO.ContractCoverageDTO>();
            });
            return config;
        }

        internal static REINSINTDTO.ContractCoverageDTO ToIntegrationDTO(this ContractCoverageDTO contractCoverageDTO)
        {
            var config = CreateMapContractCoverageIntegration();
            return config.Map<ContractCoverageDTO, REINSINTDTO.ContractCoverageDTO>(contractCoverageDTO);
        }

        internal static IEnumerable<REINSINTDTO.ContractCoverageDTO> ToIntegrationDTOs(this IEnumerable<ContractCoverageDTO> contractCoverageDTOs)
        {
            return contractCoverageDTOs.Select(ToIntegrationDTO);
        }

        internal static IMapper CreateMapIssueAllocationRiskCoverIntegration()
        {
            var config = MapperCache.GetMapper<REINSINTDTO.IssueAllocationRiskCoverDTO, IssueAllocationRiskCover>(cfg =>
            {
                cfg.CreateMap<REINSINTDTO.IssueAllocationRiskCoverDTO, IssueAllocationRiskCover>();
            });
            return config;
        }

        internal static REINSINTDTO.IssueAllocationRiskCoverDTO ToIntegrationDTO(this IssueAllocationRiskCover issueAllocationRiskCover)
        {
            var config = CreateMapIssueAllocationRiskCover();
            return config.Map<IssueAllocationRiskCover, REINSINTDTO.IssueAllocationRiskCoverDTO>(issueAllocationRiskCover);
        }

        internal static IEnumerable<REINSINTDTO.IssueAllocationRiskCoverDTO> ToIntegrationDTOs(this IEnumerable<IssueAllocationRiskCover> issueAllocationRiskCover)
        {
            return issueAllocationRiskCover.Select(ToIntegrationDTO);
        }

        internal static IMapper CreateMapEconomicGroup()
        {
            var config = MapperCache.GetMapper<BaseIntegrationEconomicGroup, EconomicGroupDTO>(cfg =>
            {
                cfg.CreateMap<BaseIntegrationEconomicGroup, EconomicGroupDTO>();
            });
            return config;
        }

        internal static EconomicGroupDTO ToDTO(this BaseIntegrationEconomicGroup integrationEconomicGroup)
        {
            var config = CreateMapEconomicGroup();
            return config.Map<BaseIntegrationEconomicGroup, EconomicGroupDTO>(integrationEconomicGroup);
        }

        internal static IEnumerable<EconomicGroupDTO> ToDTOs(this IEnumerable<BaseIntegrationEconomicGroup> integrationEconomicGroups)
        {
            return integrationEconomicGroups.Select(ToDTO);
        }

        internal static List<InsuredDTO> CreateInsuredsByEconomicGroups(List<EconomicGroupDTO> economicGroupDTOs)
        {
            List<InsuredDTO> insuredDTOs = new List<InsuredDTO>();
            foreach (EconomicGroupDTO economicGroupDTO in economicGroupDTOs)
            {
                insuredDTOs.Add(CreateInsuredByEconomicGroup(economicGroupDTO));
            }

            return insuredDTOs;
        }

        internal static InsuredDTO CreateInsuredByEconomicGroup(EconomicGroupDTO economicGroupDTO)
        {
            return new InsuredDTO
            {
                IndividualId = economicGroupDTO.EconomicGroupId,
                FullName = economicGroupDTO.EconomicGroupName,
                DocumentNumber = economicGroupDTO.TributaryIdNo
            };
        }

        internal static IMapper CreateMapPriorityRetentionIntegration()
        {
            var config = MapperCache.GetMapper<PriorityRetentionDTO, REINSINTDTO.PriorityRetentionDTO>(cfg =>
            {
                cfg.CreateMap<PriorityRetentionDTO, REINSINTDTO.PriorityRetentionDTO>();
            });
            return config;
        }

        internal static REINSINTDTO.PriorityRetentionDTO ToIntegrationDTO(this PriorityRetentionDTO priorityRetentionDTO)
        {
            var config = CreateMapPriorityRetentionIntegration();
            return config.Map<PriorityRetentionDTO, REINSINTDTO.PriorityRetentionDTO>(priorityRetentionDTO);
        }

        internal static IEnumerable<REINSINTDTO.PriorityRetentionDTO> ToIntegrationDTOs(this IEnumerable<PriorityRetentionDTO> priorityRetentionDTOs)
        {
            return priorityRetentionDTOs.Select(ToIntegrationDTO);
        }

        internal static IMapper CreateMapEconomicGroupEventByROQIntegration()
        {
            var config = MapperCache.GetMapper<ROQINTDTO.EconomicGroupEventDTO, EconomicGroupEventDTO>(cfg =>
            {
                cfg.CreateMap<ROQINTDTO.EconomicGroupEventDTO, EconomicGroupEventDTO>();
            });
            return config;
        }

        internal static EconomicGroupEventDTO ToDTO(this ROQINTDTO.EconomicGroupEventDTO economicGroupEventDTO)
        {
            var config = CreateMapEconomicGroupEventByROQIntegration();
            return config.Map<ROQINTDTO.EconomicGroupEventDTO, EconomicGroupEventDTO>(economicGroupEventDTO);
        }

        internal static IEnumerable<EconomicGroupEventDTO> ToDTOs(this IEnumerable<ROQINTDTO.EconomicGroupEventDTO> economicGroupEventDTOs)
        {
            return economicGroupEventDTOs.Select(ToDTO);
        }

        internal static IMapper CreateMapTempRiskCoverage()
        {
            var config = MapperCache.GetMapper<TempRiskCoverage, TempRiskCoverageDTO>(cfg =>
            {
                cfg.CreateMap<TempRiskCoverage, TempRiskCoverageDTO>();
            });
            return config;
        }

        internal static TempRiskCoverageDTO ToDTO(this TempRiskCoverage tempRiskCoverage)
        {
            var config = CreateMapTempRiskCoverage();
            return config.Map<TempRiskCoverage, TempRiskCoverageDTO>(tempRiskCoverage);
        }

        internal static IEnumerable<TempRiskCoverageDTO> ToDTOs(this IEnumerable<TempRiskCoverage> tempRiskCoverages)
        {
            return tempRiskCoverages.Select(ToDTO);
        }

        internal static IMapper CreateMapTempIssue()
        {
            var config = MapperCache.GetMapper<TempIssue, TempIssueDTO>(cfg =>
            {
                cfg.CreateMap<TempIssue, TempIssueDTO>();
            });
            return config;
        }

        internal static TempIssueDTO ToDTO(this TempIssue tempIssue)
        {
            var config = CreateMapTempIssue();
            return config.Map<TempIssue, TempIssueDTO>(tempIssue);
        }

        internal static IEnumerable<TempIssueDTO> ToDTOs(this IEnumerable<TempIssue> tempIssues)
        {
            return tempIssues.Select(ToDTO);
        }

        internal static IMapper CreateMapPolicyIntegration()
        {
            var config = MapperCache.GetMapper<PolicyDTO, REINSINTDTO.PolicyDTO>(cfg =>
            {
                cfg.CreateMap<PolicyDTO, REINSINTDTO.PolicyDTO>();
                cfg.CreateMap<BranchDTO, REINSINTDTO.BranchDTO>();
                cfg.CreateMap<PrefixDTO, REINSINTDTO.PrefixDTO>();
                cfg.CreateMap<CurrencyDTO, REINSINTDTO.CurrencyDTO>();
                cfg.CreateMap<EndorsementDTO, REINSINTDTO.EndorsementDTO>();
                cfg.CreateMap<RiskDTO, REINSINTDTO.RiskDTO>();
                cfg.CreateMap<CoverageDTO, REINSINTDTO.CoverageDTO>();
                cfg.CreateMap<AmountDTO, REINSINTDTO.AmountDTO>();
            });
            return config;
        }

        internal static REINSINTDTO.PolicyDTO ToIntegrationDTO(this PolicyDTO policyDTO)
        {
            var config = CreateMapPolicyIntegration();
            return config.Map<PolicyDTO, REINSINTDTO.PolicyDTO>(policyDTO);
        }

        internal static IEnumerable<REINSINTDTO.PolicyDTO> ToIntegrationDTOs(this IEnumerable<PolicyDTO> policyDTOs)
        {
            return policyDTOs.Select(ToIntegrationDTO);
        }

        internal static IMapper CreateMapPriorityRetentionDetailByIntegration()
        {
            var config = MapperCache.GetMapper<PolicyDTO, REINSINTDTO.PolicyDTO>(cfg =>
            {
                cfg.CreateMap<REINSINTDTO.PriorityRetentionDetailDTO, PriorityRetentionDetailDTO>();
            });
            return config;
        }

        internal static PriorityRetentionDetailDTO ToDTO(this REINSINTDTO.PriorityRetentionDetailDTO priorityRetentionDetailDTO)
        {
            var config = CreateMapPriorityRetentionDetailByIntegration();
            return config.Map<REINSINTDTO.PriorityRetentionDetailDTO, PriorityRetentionDetailDTO>(priorityRetentionDetailDTO);
        }

        internal static IEnumerable<PriorityRetentionDetailDTO> ToDTOs(this IEnumerable<REINSINTDTO.PriorityRetentionDetailDTO> priorityRetentionDetailDTOs)
        {
            return priorityRetentionDetailDTOs.Select(ToDTO);
        }

        public static IMapper CreateMapLineBussinesByIntegration()
        {
            var config = MapperCache.GetMapper<COMMINTDTO.LineBusinessDTO, LineBusinessDTO>(cfg =>
            {
                cfg.CreateMap<COMMINTDTO.LineBusinessDTO, LineBusinessDTO>();
            });
            return config;
        }

        public static LineBusinessDTO ToDTO(this COMMINTDTO.LineBusinessDTO lineBusinessDTO)
        {
            var config = CreateMapLineBussinesByIntegration();
            return config.Map<COMMINTDTO.LineBusinessDTO, LineBusinessDTO>(lineBusinessDTO);
        }

        public static IEnumerable<LineBusinessDTO> ToDTOs(this IEnumerable<COMMINTDTO.LineBusinessDTO> lineBusinessDTO)
        {
            return lineBusinessDTO.Select(ToDTO);
        }


        public static IMapper CreateMapLineSubBussinesByIntegration()
        {
            var config = MapperCache.GetMapper<COMMINTDTO.SubLineBusinessDTO, SubLineBusinessDTO>(cfg =>
            {
                cfg.CreateMap<COMMINTDTO.SubLineBusinessDTO, SubLineBusinessDTO>();
                cfg.CreateMap<COMMINTDTO.LineBusinessDTO, LineBusinessDTO>();
            });
            return config;
        }

        public static SubLineBusinessDTO ToDTO(this COMMINTDTO.SubLineBusinessDTO subLineBusinessDTO)
        {
            var config = CreateMapLineSubBussinesByIntegration();
            return config.Map<COMMINTDTO.SubLineBusinessDTO, SubLineBusinessDTO>(subLineBusinessDTO);
        }

        public static IEnumerable<SubLineBusinessDTO> ToDTOs(this IEnumerable<COMMINTDTO.SubLineBusinessDTO> subLineBusinessDTO)
        {
            return subLineBusinessDTO.Select(ToDTO);
        }

        internal static IMapper CreateMapCurrencies()
        {
            var config = MapperCache.GetMapper<COMMINTDTO.CurrencyDTO, CurrencyDTO>(cfg =>
            {
                cfg.CreateMap<COMMINTDTO.CurrencyDTO, CurrencyDTO>();
            });
            return config;
        }

        internal static CurrencyDTO ToDTO(this COMMINTDTO.CurrencyDTO currency)
        {
            var config = CreateMapCurrencies();
            return config.Map<COMMINTDTO.CurrencyDTO, CurrencyDTO>(currency);
        }

        internal static IEnumerable<CurrencyDTO> ToDTOs(this IEnumerable<COMMINTDTO.CurrencyDTO> currencies)
        {
            return currencies.Select(ToDTO);
        }

        internal static IMapper CreateMapBranchIntegration()
        {
            var config = MapperCache.GetMapper<COMMINTDTO.BranchDTO, BranchDTO>(cfg =>
            {
                cfg.CreateMap<COMMINTDTO.BranchDTO, BranchDTO>();
            });
            return config;
        }

        internal static BranchDTO ToDTO(this COMMINTDTO.BranchDTO branchDTO)
        {
            var config = CreateMapBranchIntegration();
            return config.Map<COMMINTDTO.BranchDTO, BranchDTO>(branchDTO);
        }

        internal static IEnumerable<BranchDTO> ToDTOs(this IEnumerable<COMMINTDTO.BranchDTO> branchDTOs)
        {
            return branchDTOs.Select(ToDTO);
        }

        internal static IMapper CreateMapPrefixIntegration()
        {
            var config = MapperCache.GetMapper<COMMINTDTO.PrefixDTO, PrefixDTO>(cfg =>
            {
                cfg.CreateMap<COMMINTDTO.PrefixDTO, PrefixDTO>();
                cfg.CreateMap<COMMINTDTO.LineBusinessDTO, LineBusinessDTO>();
                cfg.CreateMap<COMMINTDTO.PrefixTypeDTO, PrefixTypeDTO>();
            });
            return config;
        }

        internal static PrefixDTO ToDTO(this COMMINTDTO.PrefixDTO prefixDTO)
        {
            var config = CreateMapPrefixIntegration();
            return config.Map<COMMINTDTO.PrefixDTO, PrefixDTO>(prefixDTO);
        }

        internal static IEnumerable<PrefixDTO> ToDTOs(this IEnumerable<COMMINTDTO.PrefixDTO> prefixDTO)
        {
            return prefixDTO.Select(ToDTO);
        }

        internal static IMapper CreateMapPolicyByACCIntegration()
        {
            var config = MapperCache.GetMapper<ACCINTDTO.PolicyDTO, PolicyDTO>(cfg =>
            {
                cfg.CreateMap<ACCINTDTO.PolicyDTO, PolicyDTO>();
                cfg.CreateMap<ACCINTDTO.BeneficiaryDTO, BeneficiaryDTO>();
                cfg.CreateMap<ACCINTDTO.ProductDTO, ProductDTO>();
                cfg.CreateMap<ACCINTDTO.BillingGroupDTO, BillingGroupDTO>();
                cfg.CreateMap<ACCINTDTO.HolderDTO, HolderDTO>();
                cfg.CreateMap<ACCINTDTO.IssuanceAgencyDTO, IssuanceAgencyDTO>();
                cfg.CreateMap<ACCINTDTO.PrefixDTO, PrefixDTO>();
                cfg.CreateMap<ACCINTDTO.BranchDTO, BranchDTO>();
                cfg.CreateMap<ACCINTDTO.PaymentPlanDTO, PaymentPlanDTO>();
                cfg.CreateMap<ACCINTDTO.PayerComponentDTO, PayerComponentDTO>();
                cfg.CreateMap<ACCINTDTO.EndorsementDTO, EndorsementDTO>();
                cfg.CreateMap<ACCINTDTO.ExchangeRateDTO, ExchangeRateDTO>();
                cfg.CreateMap<ACCINTDTO.PolicyTypeDTO, PolicyTypeDTO>();
                cfg.CreateMap<ACCINTDTO.IssuanceAgentDTO, IssuanceAgentDTO>();
                cfg.CreateMap<ACCINTDTO.IssuanceCommissionDTO, IssuanceCommissionDTO>();
                cfg.CreateMap<ACCINTDTO.SubLineBusinessDTO, SubLineBusinessDTO>();
                cfg.CreateMap<ACCINTDTO.LineBusinessDTO, LineBusinessDTO>();
                cfg.CreateMap<ACCINTDTO.SalePointDTO, SalePointDTO>();
                cfg.CreateMap<ACCINTDTO.QuotaDTO, QuotaDTO>();
            });
            return config;
        }

        internal static PolicyDTO ToDTO(this ACCINTDTO.PolicyDTO policyDTO)
        {
            var config = CreateMapPolicyByACCIntegration();
            return config.Map<ACCINTDTO.PolicyDTO, PolicyDTO>(policyDTO);
        }

        internal static IEnumerable<PolicyDTO> ToDTOs(this IEnumerable<ACCINTDTO.PolicyDTO> policyDTO)
        {
            return policyDTO.Select(ToDTO);
        }

        internal static IMapper CreateMapModuleDateByTEMPIntegration()
        {
            var config = MapperCache.GetMapper<TEMPINTDTO.ModuleDateDTO, ModuleDateDTO>(cfg =>
            {
                cfg.CreateMap<TEMPINTDTO.ModuleDateDTO, ModuleDateDTO>();
            });
            return config;
        }

        internal static ModuleDateDTO ToDTO(this TEMPINTDTO.ModuleDateDTO moduleDate)
        {
            var config = CreateMapModuleDateByTEMPIntegration();
            return config.Map<TEMPINTDTO.ModuleDateDTO, ModuleDateDTO>(moduleDate);
        }

        internal static IEnumerable<ModuleDateDTO> ToDTOs(this IEnumerable<TEMPINTDTO.ModuleDateDTO> moduleDate)
        {
            return moduleDate.Select(ToDTO);
        }

        internal static IMapper CreateMapAgentByTEMPIntegration()
        {
            var config = MapperCache.GetMapper<TEMPINTDTO.AgentDTO, AgentDTO>(cfg =>
            {
                cfg.CreateMap<TEMPINTDTO.AgentDTO, AgentDTO>();
            });
            return config;
        }

        internal static AgentDTO ToDTO(this TEMPINTDTO.AgentDTO agentDTO)
        {
            var config = CreateMapAgentByTEMPIntegration();
            return config.Map<TEMPINTDTO.AgentDTO, AgentDTO>(agentDTO);
        }

        internal static IEnumerable<AgentDTO> ToDTOs(this IEnumerable<TEMPINTDTO.AgentDTO> agentDTO)
        {
            return agentDTO.Select(ToDTO);
        }

        internal static IMapper CreateMapProductByTEMPIntegration()
        {
            var config = MapperCache.GetMapper<TEMPINTDTO.ProductDTO, ProductDTO>(cfg =>
            {
                cfg.CreateMap<TEMPINTDTO.ProductDTO, ProductDTO>();
            });
            return config;
        }

        internal static ProductDTO ToDTO(this TEMPINTDTO.ProductDTO productDTO)
        {
            var config = CreateMapProductByTEMPIntegration();
            return config.Map<TEMPINTDTO.ProductDTO, ProductDTO>(productDTO);
        }

        internal static IEnumerable<ProductDTO> ToDTOs(this IEnumerable<TEMPINTDTO.ProductDTO> productDTO)
        {
            return productDTO.Select(ToDTO);
        }

        internal static IMapper CreateMapBranchByUUIntegration()
        {
            var config = MapperCache.GetMapper<TEMPINTDTO.BranchDTO, BranchDTO>(cfg =>
            {
                cfg.CreateMap<UUINTDTO.BranchDTO, BranchDTO>();
                cfg.CreateMap<UUINTDTO.SalePointDTO, SalePointDTO>();
            });
            return config;
        }

        internal static BranchDTO ToDTO(this UUINTDTO.BranchDTO branchDTO)
        {
            var config = CreateMapBranchByUUIntegration();
            return config.Map<UUINTDTO.BranchDTO, BranchDTO>(branchDTO);
        }

        internal static IEnumerable<BranchDTO> ToDTOs(this IEnumerable<UUINTDTO.BranchDTO> branchDTO)
        {
            return branchDTO.Select(ToDTO);
        }

        internal static IMapper CreateMapUserByUUIntegration()
        {
            var config = MapperCache.GetMapper<UUINTDTO.UserDTO, UserDTO>(cfg =>
            {
                cfg.CreateMap<UUINTDTO.UserDTO, UserDTO>()
                    .ForMember(dest => dest.AuthenticationType, opt => opt.MapFrom(src => src.AuthenticationType));
                cfg.CreateMap<UUINTDTO.BranchDTO, BranchDTO>();
                cfg.CreateMap<UUINTDTO.CoHierarchyAssociationDTO, CoHierarchyAssociationDTO>();
                cfg.CreateMap<UUINTDTO.UniqueUserLoginDTO, UniqueUserLoginDTO>();
                cfg.CreateMap<UUINTDTO.ProfileDTO, ProfileDTO>();
                cfg.CreateMap<UUINTDTO.IndividualRelationAppDTO, IndividualRelationAppDTO>();
                cfg.CreateMap<UUINTDTO.PrefixDTO, PrefixDTO>();
                cfg.CreateMap<UUINTDTO.SalePointDTO, SalePointDTO>();
                cfg.CreateMap<UUINTDTO.ModuleDTO, ModuleDTO>();
                cfg.CreateMap<UUINTDTO.SubModuleDTO, SubModuleDTO>();
                cfg.CreateMap<UUINTDTO.AccessProfileDTO, AccessProfileDTO>();
                cfg.CreateMap<UUINTDTO.LineBusinessDTO, LineBusinessDTO>();
                cfg.CreateMap<UUINTDTO.PrefixTypeDTO, PrefixTypeDTO>();
            });
            return config;
        }

        internal static UserDTO ToDTO(this UUINTDTO.UserDTO userDTO)
        {
            var config = CreateMapUserByUUIntegration();
            return config.Map<UUINTDTO.UserDTO, UserDTO>(userDTO);
        }

        internal static IEnumerable<UserDTO> ToDTOs(this IEnumerable<UUINTDTO.UserDTO> userDTO)
        {
            return userDTO.Select(ToDTO);
        }

        internal static IMapper CreateMapPolicyIntegrationByTEMPIntegration()
        {
            var config = MapperCache.GetMapper<TEMPINTDTO.PolicyDTO, REINSINTDTO.PolicyDTO>(cfg =>
            {
                cfg.CreateMap<TEMPINTDTO.PolicyDTO, REINSINTDTO.PolicyDTO>();
                cfg.CreateMap<TEMPINTDTO.BranchDTO, REINSINTDTO.BranchDTO>();
                cfg.CreateMap<TEMPINTDTO.PrefixDTO, REINSINTDTO.PrefixDTO>();
                cfg.CreateMap<TEMPINTDTO.CurrencyDTO, REINSINTDTO.CurrencyDTO>();
                cfg.CreateMap<TEMPINTDTO.EndorsementDTO, REINSINTDTO.EndorsementDTO>();
                cfg.CreateMap<TEMPINTDTO.RiskDTO, REINSINTDTO.RiskDTO>();
                cfg.CreateMap<TEMPINTDTO.CoverageDTO, REINSINTDTO.CoverageDTO>();
                cfg.CreateMap<TEMPINTDTO.AmountDTO, REINSINTDTO.AmountDTO>();
            });
            return config;
        }

        internal static REINSINTDTO.PolicyDTO ToIntegrationDTO(this TEMPINTDTO.PolicyDTO policyDTO)
        {
            var config = CreateMapPolicyIntegrationByTEMPIntegration();
            return config.Map<TEMPINTDTO.PolicyDTO, REINSINTDTO.PolicyDTO>(policyDTO);
        }

        internal static IEnumerable<REINSINTDTO.PolicyDTO> ToIntegrationDTOs(this IEnumerable<TEMPINTDTO.PolicyDTO> policyDTO)
        {
            return policyDTO.Select(ToIntegrationDTO);
        }

        internal static IMapper CreateMapPolicyIntegrationByTEMPModel()
        {
            var config = MapperCache.GetMapper<TempCommonServices.Models.Policy, REINSINTDTO.PolicyDTO>(cfg =>
            {
                cfg.CreateMap<TempCommonServices.Models.Policy, REINSINTDTO.PolicyDTO>();
                cfg.CreateMap<Branch, REINSINTDTO.BranchDTO>();
                cfg.CreateMap<Prefix, REINSINTDTO.PrefixDTO>();
                cfg.CreateMap<Currency, REINSINTDTO.CurrencyDTO>();
                cfg.CreateMap<TempCommonServices.Models.Endorsement, REINSINTDTO.EndorsementDTO>();
                cfg.CreateMap<TempCommonServices.Models.Risk, REINSINTDTO.RiskDTO>();
                cfg.CreateMap<TempCommonServices.Models.Coverage, REINSINTDTO.CoverageDTO>();
                cfg.CreateMap<Amount, AmountDTO>();
            });
            return config;
        }

        internal static REINSINTDTO.PolicyDTO ToIntegrationDTO(this TempCommonServices.Models.Policy policy)
        {
            var config = CreateMapPolicyIntegrationByTEMPModel();
            return config.Map<TempCommonServices.Models.Policy, REINSINTDTO.PolicyDTO>(policy);
        }

        internal static IEnumerable<REINSINTDTO.PolicyDTO> ToIntegrationDTOs(this IEnumerable<TempCommonServices.Models.Policy> policies)
        {
            return policies.Select(ToIntegrationDTO);
        }

        internal static IMapper CreateMapLinesIntegration()
        {
            var config = MapperCache.GetMapper<Line, REINSINTDTO.LineDTO>(cfg =>
            {
                cfg.CreateMap<Line, REINSINTDTO.LineDTO>();
                cfg.CreateMap<CumulusType, REINSINTDTO.CumulusTypeDTO>();
                cfg.CreateMap<ContractLine, REINSINTDTO.ContractLineDTO>();
                cfg.CreateMap<Contract, REINSINTDTO.ContractDTO>();
                cfg.CreateMap<ResettlementType, REINSINTDTO.ResettlementTypeDTO>();
                cfg.CreateMap<AffectationType, REINSINTDTO.AffectationTypeDTO>();
                cfg.CreateMap<EPIType, REINSINTDTO.EPITypeDTO>();
                cfg.CreateMap<ContractType, REINSINTDTO.ContractTypeDTO>();
                cfg.CreateMap<ContractFunctionality, REINSINTDTO.ContractFunctionalityDTO>();
                cfg.CreateMap<Currency, REINSINTDTO.CurrencyDTO>();
                cfg.CreateMap<Level, REINSINTDTO.LevelDTO>()
                   .ForMember(dest => dest.PremiumType, opt => opt.MapFrom(src => (int)src.PremiumType))
                   .ForMember(dest => dest.ApplyOnType, opt => opt.MapFrom(src => (int)src.ApplyOnType))
                   .ForMember(dest => dest.CalculationType, opt => opt.MapFrom(src => (int)src.CalculationType));
                cfg.CreateMap<LevelPayment, REINSINTDTO.LevelPaymentDTO>();
                cfg.CreateMap<LevelCompany, REINSINTDTO.LevelCompanyDTO>()
                        .ForMember(dest => dest.PresentationInformationType, opt => opt.MapFrom(src => (int)src.PresentationInformationType));
                cfg.CreateMap<LevelRestore, REINSINTDTO.LevelRestoreDTO>();
                cfg.CreateMap<Amount, REINSINTDTO.AmountDTO>();
                cfg.CreateMap<Agent, REINSINTDTO.AgentDTO>();
            });
            return config;
        }

        internal static REINSINTDTO.LineDTO ToIntegrationDTO(this Line line)
        {
            var config = CreateMapLinesIntegration();
            return config.Map<Line, REINSINTDTO.LineDTO>(line);
        }

        internal static IEnumerable<REINSINTDTO.LineDTO> ToIntegrationDTOs(this IEnumerable<Line> line)
        {
            return line.Select(ToIntegrationDTO);
        }

        internal static IMapper CreateMapContractsIntegration()
        {
            var config = MapperCache.GetMapper<Contract, REINSINTDTO.ContractDTO>(cfg =>
            {
                cfg.CreateMap<Contract, REINSINTDTO.ContractDTO>();
                cfg.CreateMap<ResettlementType, REINSINTDTO.ResettlementTypeDTO>();
                cfg.CreateMap<AffectationType, REINSINTDTO.AffectationTypeDTO>();
                cfg.CreateMap<EPIType, REINSINTDTO.EPITypeDTO>();
                cfg.CreateMap<ContractType, REINSINTDTO.ContractTypeDTO>();
                cfg.CreateMap<ContractFunctionality, REINSINTDTO.ContractFunctionalityDTO>();
                cfg.CreateMap<Currency, REINSINTDTO.CurrencyDTO>();
                cfg.CreateMap<Level, REINSINTDTO.LevelDTO>()
                   .ForMember(dest => dest.PremiumType, opt => opt.MapFrom(src => (int)src.PremiumType))
                   .ForMember(dest => dest.ApplyOnType, opt => opt.MapFrom(src => (int)src.ApplyOnType))
                   .ForMember(dest => dest.CalculationType, opt => opt.MapFrom(src => (int)src.CalculationType));
                cfg.CreateMap<LevelPayment, REINSINTDTO.LevelPaymentDTO>();
                cfg.CreateMap<LevelCompany, REINSINTDTO.LevelCompanyDTO>()
                        .ForMember(dest => dest.PresentationInformationType, opt => opt.MapFrom(src => (int)src.PresentationInformationType));
                cfg.CreateMap<LevelRestore, REINSINTDTO.LevelRestoreDTO>();
                cfg.CreateMap<Amount, REINSINTDTO.AmountDTO>();
                cfg.CreateMap<Agent, REINSINTDTO.AgentDTO>();

            });
            return config;
        }

        internal static REINSINTDTO.ContractDTO ToIntegrationDTO(this Contract contract)
        {
            var config = CreateMapContractsIntegration();
            return config.Map<Contract, REINSINTDTO.ContractDTO>(contract);
        }

        internal static IEnumerable<REINSINTDTO.ContractDTO> ToIntegrationDTOs(this IEnumerable<Contract> contract)
        {
            return contract.Select(ToIntegrationDTO);
        }

        internal static IMapper CreateMapLevelsIntegration()
        {
            var config = MapperCache.GetMapper<Level, REINSINTDTO.LevelDTO>(cfg =>
            {
                cfg.CreateMap<Level, REINSINTDTO.LevelDTO>()
                        .ForMember(dest => dest.CalculationType, opt => opt.MapFrom(src => (int)src.CalculationType))
                        .ForMember(dest => dest.ApplyOnType, opt => opt.MapFrom(src => (int)src.ApplyOnType))
                        .ForMember(dest => dest.PremiumType, opt => opt.MapFrom(src => (int)src.PremiumType));
                cfg.CreateMap<Contract, REINSINTDTO.ContractDTO>();
                cfg.CreateMap<ResettlementType, REINSINTDTO.ResettlementTypeDTO>();
                cfg.CreateMap<AffectationType, REINSINTDTO.AffectationTypeDTO>();
                cfg.CreateMap<EPIType, REINSINTDTO.EPITypeDTO>();
                cfg.CreateMap<ContractType, REINSINTDTO.ContractTypeDTO>();
                cfg.CreateMap<ContractFunctionality, REINSINTDTO.ContractFunctionalityDTO>();
                cfg.CreateMap<Currency, REINSINTDTO.CurrencyDTO>();
                cfg.CreateMap<LevelPayment, REINSINTDTO.LevelPaymentDTO>();
                cfg.CreateMap<LevelCompany, REINSINTDTO.LevelCompanyDTO>()
                        .ForMember(dest => dest.PresentationInformationType, opt => opt.MapFrom(src => (int)src.PresentationInformationType));
                cfg.CreateMap<LevelRestore, REINSINTDTO.LevelRestoreDTO>();
                cfg.CreateMap<Amount, REINSINTDTO.AmountDTO>();
                cfg.CreateMap<Agent, REINSINTDTO.AgentDTO>();
            });
            return config;
        }

        internal static REINSINTDTO.LevelDTO ToIntegrationDTO(this Level level)
        {
            var config = CreateMapLevelsIntegration();
            return config.Map<Level, REINSINTDTO.LevelDTO>(level);
        }

        internal static IEnumerable<REINSINTDTO.LevelDTO> ToIntegrationDTOs(this IEnumerable<Level> level)
        {
            return level.Select(ToIntegrationDTO);
        }

        internal static IMapper CreateMapLevelCompanyIntegration()
        {
            var config = MapperCache.GetMapper<LevelCompany, REINSINTDTO.LevelCompanyDTO>(cfg =>
            {
                cfg.CreateMap<LevelCompany, REINSINTDTO.LevelCompanyDTO>()
                    .ForMember(dest => dest.PresentationInformationType, opt => opt.MapFrom(src => (int)src.PresentationInformationType));
                cfg.CreateMap<Agent, REINSINTDTO.AgentDTO>();
                cfg.CreateMap<Company, REINSINTDTO.CompanyDTO>();
                cfg.CreateMap<Amount, REINSINTDTO.AmountDTO>();
                cfg.CreateMap<Level, REINSINTDTO.LevelDTO>()
                    .ForMember(dest => dest.PremiumType, opt => opt.MapFrom(src => (int)src.PremiumType))
                    .ForMember(dest => dest.ApplyOnType, opt => opt.MapFrom(src => (int)src.ApplyOnType))
                    .ForMember(dest => dest.CalculationType, opt => opt.MapFrom(src => (int)src.CalculationType));
                cfg.CreateMap<Installment, REINSINTDTO.InstallmentDTO>();
                cfg.CreateMap<Contract, REINSINTDTO.ContractDTO>();
                cfg.CreateMap<ResettlementType, REINSINTDTO.ResettlementTypeDTO>();
                cfg.CreateMap<AffectationType, REINSINTDTO.AffectationTypeDTO>();
                cfg.CreateMap<EPIType, REINSINTDTO.EPITypeDTO>();
                cfg.CreateMap<ContractType, REINSINTDTO.ContractTypeDTO>();
                cfg.CreateMap<ContractFunctionality, REINSINTDTO.ContractFunctionalityDTO>();
                cfg.CreateMap<Currency, REINSINTDTO.CurrencyDTO>();
                cfg.CreateMap<LevelPayment, REINSINTDTO.LevelPaymentDTO>();
                cfg.CreateMap<LevelRestore, REINSINTDTO.LevelRestoreDTO>();

            });
            return config;
        }

        internal static REINSINTDTO.LevelCompanyDTO ToIntegrationDTO(this LevelCompany levelCompany)
        {
            var config = CreateMapLevelCompanyIntegration();
            return config.Map<LevelCompany, REINSINTDTO.LevelCompanyDTO>(levelCompany);
        }

        internal static IEnumerable<REINSINTDTO.LevelCompanyDTO> ToIntegrationDTOs(this IEnumerable<LevelCompany> levelCompany)
        {
            return levelCompany.Select(ToIntegrationDTO);
        }

        internal static IMapper CreateMapReinsuranceIntegration()
        {
            var config = MapperCache.GetMapper<Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance.Reinsurance, REINSINTDTO.ReinsuranceDTO>(cfg =>
            {
                cfg.CreateMap<EEProvider.Models.Reinsurance.Reinsurance, REINSINTDTO.ReinsuranceDTO>()
                        .ForMember(dest => dest.Movements, opt => opt.MapFrom(src => (int)src.Movements));
                cfg.CreateMap<ReinsuranceLayer, REINSINTDTO.ReinsuranceLayerDTO>();
                cfg.CreateMap<ReinsuranceLine, REINSINTDTO.ReinsuranceLineDTO>();
                cfg.CreateMap<Line, REINSINTDTO.LineDTO>();
                cfg.CreateMap<CumulusType, REINSINTDTO.CumulusTypeDTO>();
                cfg.CreateMap<ContractLine, REINSINTDTO.ContractLineDTO>();
                cfg.CreateMap<Contract, REINSINTDTO.ContractDTO>();
                cfg.CreateMap<Currency, REINSINTDTO.CurrencyDTO>();
                cfg.CreateMap<ContractFunctionality, REINSINTDTO.ContractFunctionalityDTO>();
                cfg.CreateMap<ContractType, REINSINTDTO.ContractTypeDTO>();
                cfg.CreateMap<ReinsuranceAllocation, REINSINTDTO.ReinsuranceAllocationDTO>();
                cfg.CreateMap<ReinsuranceCumulusRiskCoverage, REINSINTDTO.ReinsuranceCumulusRiskCoverageDTO>();
                cfg.CreateMap<ResettlementType, REINSINTDTO.ResettlementTypeDTO>();
                cfg.CreateMap<AffectationType, REINSINTDTO.AffectationTypeDTO>();
                cfg.CreateMap<EPIType, REINSINTDTO.EPITypeDTO>();
                cfg.CreateMap<Level, REINSINTDTO.LevelDTO>()
                   .ForMember(dest => dest.PremiumType, opt => opt.MapFrom(src => (int)src.PremiumType))
                   .ForMember(dest => dest.ApplyOnType, opt => opt.MapFrom(src => (int)src.ApplyOnType))
                   .ForMember(dest => dest.CalculationType, opt => opt.MapFrom(src => (int)src.CalculationType));
                cfg.CreateMap<Agent, REINSINTDTO.AgentDTO>();
                cfg.CreateMap<Company, REINSINTDTO.CompanyDTO>();
                cfg.CreateMap<LevelCompany, REINSINTDTO.LevelCompanyDTO>()
                        .ForMember(dest => dest.PresentationInformationType, opt => opt.MapFrom(src => (int)src.PresentationInformationType));
                cfg.CreateMap<LevelPayment, REINSINTDTO.LevelPaymentDTO>();
                cfg.CreateMap<Installment, REINSINTDTO.InstallmentDTO>();
                cfg.CreateMap<LevelRestore, REINSINTDTO.LevelRestoreDTO>();
                cfg.CreateMap<CommonService.Models.Amount, REINSINTDTO.AmountDTO>();

            });
            return config;
        }

        internal static REINSINTDTO.ReinsuranceDTO ToIntegrationDTO(this Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance.Reinsurance reinsurance)
        {
            var config = CreateMapReinsuranceIntegration();
            return config.Map<EEProvider.Models.Reinsurance.Reinsurance, REINSINTDTO.ReinsuranceDTO>(reinsurance);
        }

        internal static IEnumerable<REINSINTDTO.ReinsuranceDTO> ToIntegrationDTOs(this IEnumerable<Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance.Reinsurance> reinsurance)
        {
            return reinsurance.Select(ToIntegrationDTO);
        }

        internal static IMapper CreateMapPriorityRetentionDetail()
        {
            var config = MapperCache.GetMapper<PriorityRetentionDetail, PriorityRetentionDetailDTO>(cfg =>
            {
                cfg.CreateMap<PriorityRetentionDetail, PriorityRetentionDetailDTO>();
            });
            return config;
        }

        internal static PriorityRetentionDetailDTO ToDTO(this PriorityRetentionDetail priorityRetentionDetail)
        {
            var config = CreateMapPriorityRetentionDetail();
            return config.Map<PriorityRetentionDetail, PriorityRetentionDetailDTO>(priorityRetentionDetail);
        }

        internal static IEnumerable<PriorityRetentionDetailDTO> ToDTOs(this IEnumerable<PriorityRetentionDetail> priorityRetentionDetails)
        {
            return priorityRetentionDetails.Select(ToDTO);
        }

        internal static List<EndorsementDTO> CreateEndorsementDTOsByUNDDTOEndorsementDTOs(List<UNDDTO.EndorsementBaseDTO> endorsementBaseDTOs)
        {
            List<EndorsementDTO> endorsementDTOs = new List<EndorsementDTO>();
            foreach (UNDDTO.EndorsementBaseDTO endorsementBaseDTO in endorsementBaseDTOs)
            {
                endorsementDTOs.Add(CreateEndorsementDTOByUNDDTOEndorsementDTO(endorsementBaseDTO));
            }

            return endorsementDTOs;
        }

        internal static EndorsementDTO CreateEndorsementDTOByUNDDTOEndorsementDTO(UNDDTO.EndorsementBaseDTO endorsementBaseDTO)
        {
            return new EndorsementDTO
            {
                Id = endorsementBaseDTO.Id,
                EndorsementNumber = endorsementBaseDTO.DocumentNum,
                PolicyId = endorsementBaseDTO.PolicyId
            };
        }

        internal static IMapper CreateMapPriorityRetentionDetailIntegration()
        {
            var config = MapperCache.GetMapper<PriorityRetentionDetailDTO, REINSINTDTO.PriorityRetentionDetailDTO>(cfg =>
            {
                cfg.CreateMap<PriorityRetentionDetailDTO, REINSINTDTO.PriorityRetentionDetailDTO>();
            });
            return config;
        }

        internal static REINSINTDTO.PriorityRetentionDetailDTO ToIntegrationDTO(this PriorityRetentionDetailDTO priorityRetentionDetail)
        {
            var config = CreateMapPriorityRetentionDetailIntegration();
            return config.Map<PriorityRetentionDetailDTO, REINSINTDTO.PriorityRetentionDetailDTO>(priorityRetentionDetail);
        }

        internal static IEnumerable<REINSINTDTO.PriorityRetentionDetailDTO> ToIntegrationDTOs(this IEnumerable<PriorityRetentionDetailDTO> priorityRetentionDetails)
        {
            return priorityRetentionDetails.Select(ToIntegrationDTO);
        }

        internal static IMapper CreateMapTempRiskCoverageIntegration()
        {
            var config = MapperCache.GetMapper<TempRiskCoverageDTO, REINSINTDTO.TempRiskCoverageDTO>(cfg =>
            {
                cfg.CreateMap<TempRiskCoverageDTO, REINSINTDTO.TempRiskCoverageDTO>();
            });
            return config;
        }

        internal static REINSINTDTO.TempRiskCoverageDTO ToIntegrationDTO(this TempRiskCoverageDTO tempRiskCoverage)
        {
            var config = CreateMapTempRiskCoverageIntegration();
            return config.Map<TempRiskCoverageDTO, REINSINTDTO.TempRiskCoverageDTO>(tempRiskCoverage);
        }

        internal static IEnumerable<REINSINTDTO.TempRiskCoverageDTO> ToIntegrationDTOs(this IEnumerable<TempRiskCoverageDTO> tempRiskCoverages)
        {
            return tempRiskCoverages.Select(ToIntegrationDTO);
        }

        internal static PolicyDTO CreatePolicyDTOByUNDDTOPolicyDTO(UNDDTO.PolicyDTO policyDTO)
        {
            return new PolicyDTO
            {
                Id = policyDTO.Id,
                DocumentNumber = Convert.ToInt32(policyDTO.DocumentNumber),
                EndorsementId = policyDTO.EndorsementId,
                Branch = new DTOs.BranchDTO() { Id = policyDTO.BranchId },
                Prefix = new PrefixDTO() { Id = policyDTO.PrefixId },
                CurrentFrom = Convert.ToDateTime(policyDTO.CurrentFrom),
                CurrentTo = Convert.ToDateTime(policyDTO.CurrentTo),
                Endorsement = new EndorsementDTO() { Number = policyDTO.EndorsementDocumentNum }
            };
        }

        internal static LineAssociationDTO CreateLineAssociationDTOByAssociationLineDTO(AssociationLineDTO associationLineDTO)
        {
            return new LineAssociationDTO
            {
                DateFrom = Convert.ToDateTime(associationLineDTO.DateFrom),
                DateTo = Convert.ToDateTime(associationLineDTO.DateTo),
                LineAssociationId = associationLineDTO.AssociationLineId,
                AssociationType = new LineAssociationTypeDTO
                {
                    LineAssociationTypeId = associationLineDTO.AssociationTypeId
                },
                Line = new LineDTO
                {
                    LineId = associationLineDTO.LineId
                }
            };
        }

        internal static IMapper CreateMapReportTypes()
        {
            var config = MapperCache.GetMapper<ReportType, ReportTypeDTO>(cfg =>
            {
                cfg.CreateMap<ReportType, ReportTypeDTO>();
            });
            return config;
        }

        internal static ReportTypeDTO ToDTO(this ReportType reportType)
        {
            var config = CreateMapReportTypes();
            return config.Map<ReportType, ReportTypeDTO>(reportType);
        }

        internal static IEnumerable<ReportTypeDTO> ToDTOs(this IEnumerable<ReportType> reportTypes)
        {
            return reportTypes.Select(ToDTO);
        }


        internal static IMapper CreateMapExchangeRate()
        {
            var config = MapperCache.GetMapper<COMMINTDTO.ExchangeRateDTO, ExchangeRateDTO>(cfg =>
            {
                cfg.CreateMap<COMMINTDTO.ExchangeRateDTO, ExchangeRateDTO>();
            });
            return config;
        }

        internal static ExchangeRateDTO ToDTO(this COMMINTDTO.ExchangeRateDTO exchangeRate)
        {
            var config = CreateMapExchangeRate();
            return config.Map<COMMINTDTO.ExchangeRateDTO, ExchangeRateDTO>(exchangeRate);
        }

        internal static List<ExchangeRateDTO> ToDTOs(this List<COMMINTDTO.ExchangeRateDTO> exchangeRates)
        {
            var config = CreateMapExchangeRate();
            return config.Map<List<COMMINTDTO.ExchangeRateDTO>, List<ExchangeRateDTO>>(exchangeRates);
        }

        internal static IMapper CreateMapExchangeRateIntegration()
        {
            var config = MapperCache.GetMapper<REINSINTDTO.ExchangeRateDTO, ExchangeRateDTO>(cfg =>
            {
                cfg.CreateMap<ExchangeRateDTO, REINSINTDTO.ExchangeRateDTO>();
            });
            return config;
        }

        internal static REINSINTDTO.ExchangeRateDTO ToIntegrationDTO(this ExchangeRateDTO exchangeRate)
        {
            var config = CreateMapExchangeRateIntegration();
            return config.Map<ExchangeRateDTO, REINSINTDTO.ExchangeRateDTO>(exchangeRate);
        }

        internal static List<REINSINTDTO.ExchangeRateDTO> ToIntegrationDTOs(this List<ExchangeRateDTO> exchangeRates)
        {
            var config = CreateMapExchangeRateIntegration();
            return config.Map<List<ExchangeRateDTO>, List<REINSINTDTO.ExchangeRateDTO>>(exchangeRates);
        }

        internal static IMapper CreateMapCurrencyIntegration()
        {
            var config = MapperCache.GetMapper<COMMINTDTO.CurrencyDTO, CurrencyDTO>(cfg =>
            {
                cfg.CreateMap<CurrencyDTO, REINSINTDTO.CurrencyDTO>();
            });
            return config;
        }

        internal static REINSINTDTO.CurrencyDTO ToIntegrationDTO(this CurrencyDTO currency)
        {
            var config = CreateMapCurrencyIntegration();
            return config.Map<CurrencyDTO, REINSINTDTO.CurrencyDTO>(currency);
        }

        internal static IEnumerable<REINSINTDTO.CurrencyDTO> ToIntegrationDTOs(this IEnumerable<CurrencyDTO> currencies)
        {
            return currencies.Select(ToIntegrationDTO);
        }

        internal static TempFacultativeCompaniesDTO CreateTempFacultativeCompaniesByLayerIssuance(ReinsuranceLayerIssuanceDTO reinsuranceLayerIssuanceDTO)
        {
            return new TempFacultativeCompaniesDTO()
            {
                TempFacultativeCompanyId = reinsuranceLayerIssuanceDTO.ReinsuranceLayerId,
                TempFacultativeId = reinsuranceLayerIssuanceDTO.TemporaryIssueId,
                BrokerReinsuranceId = reinsuranceLayerIssuanceDTO.Lines[0].ReinsuranceAllocations[0].Contract.ContractLevels[0].ContractLevelCompanies[0].Agent.IndividualId,
                BrokerDescription = reinsuranceLayerIssuanceDTO.Lines[0].ReinsuranceAllocations[0].Contract.ContractLevels[0].ContractLevelCompanies[0].Agent.FullName,
                ReinsuranceCompanyId = reinsuranceLayerIssuanceDTO.Lines[0].ReinsuranceAllocations[0].Contract.ContractLevels[0].ContractLevelCompanies[0].Company.IndividualId,
                DescriptionCompany = reinsuranceLayerIssuanceDTO.Lines[0].ReinsuranceAllocations[0].Contract.ContractLevels[0].ContractLevelCompanies[0].Company.FullName,
                ParticipationPercentage = reinsuranceLayerIssuanceDTO.LayerPercentage.ToString().Replace(",", "."),
                PremiumPercentage = reinsuranceLayerIssuanceDTO.PremiumPercentage.ToString().Replace(",", "."),
                CommissionPercentage = reinsuranceLayerIssuanceDTO.Lines[0].ReinsuranceAllocations[0].Contract.ContractLevels[0].ContractLevelCompanies[0].ComissionPercentage.ToString().Replace(",", "."),
                SumValueParticipation = reinsuranceLayerIssuanceDTO.LayerAmount.ToString(),
                SumValuePremium = reinsuranceLayerIssuanceDTO.PremiumAmount.ToString(),
                Comments = reinsuranceLayerIssuanceDTO.Lines[0].ReinsuranceAllocations[0].Contract.Description,
                FacultativePercentage = reinsuranceLayerIssuanceDTO.Lines[0].ReinsuranceAllocations[0].Contract.FacultativePercentage,
                FacultativePremiumPercentage = reinsuranceLayerIssuanceDTO.Lines[0].ReinsuranceAllocations[0].Contract.FacultativePremiumPercentage
            };
        }

        internal static IMapper CreateMapSlips()
        {
            var config = MapperCache.GetMapper<Slip, SlipDTO>(cfg =>
            {
                cfg.CreateMap<Slip, SlipDTO>();
            });
            return config;
        }

        internal static SlipDTO ToDTO(this Slip slip)
        {
            var config = CreateMapSlips();
            return config.Map<Slip, SlipDTO>(slip);
        }

        internal static IEnumerable<SlipDTO> ToDTOs(this IEnumerable<Slip> slips)
        {
            return slips.Select(ToDTO);
        }

        internal static IMapper CreateMapEconomicGroupDetail()
        {
            var config = MapperCache.GetMapper<BaseIntegrationEconomicGroupDetail, EconomicGroupDetailDTO>(cfg =>
            {
                cfg.CreateMap<BaseIntegrationEconomicGroupDetail, EconomicGroupDetailDTO>();
            });
            return config;
        }

        internal static EconomicGroupDetailDTO ToDTO(this BaseIntegrationEconomicGroupDetail economicGroupDetail)
        {
            var config = CreateMapEconomicGroupDetail();
            return config.Map<BaseIntegrationEconomicGroupDetail, EconomicGroupDetailDTO>(economicGroupDetail);
        }

        internal static IEnumerable<EconomicGroupDetailDTO> ToDTOs(this IEnumerable<BaseIntegrationEconomicGroupDetail> economicGroupDetails)
        {
            return economicGroupDetails.Select(ToDTO);
        }

    }
}
