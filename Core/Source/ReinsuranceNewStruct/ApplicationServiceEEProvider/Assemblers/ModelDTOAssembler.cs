using AutoMapper;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.ReinsuranceServices.DTOs;
using REINSINTDTO = Sistran.Core.Integration.ReinsuranceIntegrationServices.DTOs.Reinsurance;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Enums;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance;
using Sistran.Core.Application.ReinsuranceServices.Models.LineAssociationTypes;
using Sistran.Core.Application.TempCommonServices.Models;
using Sistran.Core.Application.Utilities.Cache;
using System.Collections.Generic;
using System.Linq;


namespace Sistran.Core.Application.ReinsuranceServices.Assemblers
{
    internal static class ModelDTOAssembler
    {

        internal static  IMapper CreateMapAffectationTypes()
        {
            var config = MapperCache.GetMapper<AffectationTypeDTO, AffectationType>(cfg =>
            {
                cfg.CreateMap<AffectationTypeDTO, AffectationType>();
            });
            return config;
        }

        internal static  AffectationType ToModel(this AffectationTypeDTO affectationTypeDTO)
        {
            var config = CreateMapAffectationTypes();
            return config.Map<AffectationTypeDTO, AffectationType>(affectationTypeDTO);
        }

        internal static  IEnumerable<AffectationType> ToModels(this IEnumerable<AffectationTypeDTO> affectationTypeDTOs)
        {
            return affectationTypeDTOs.Select(ToModel);
        }

        internal static  IMapper CreateMapContracts()
        {
            var config = MapperCache.GetMapper<ContractDTO, Contract>(cfg =>
            {
                cfg.CreateMap<ContractDTO, Contract>();
                cfg.CreateMap<ResettlementTypeDTO, ResettlementType>();
                cfg.CreateMap<AffectationTypeDTO, AffectationType>();
                cfg.CreateMap<EPITypeDTO, EPIType>();
                cfg.CreateMap<ContractTypeDTO, ContractType>();
                cfg.CreateMap<ContractFunctionalityDTO, ContractFunctionality>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<LevelDTO, Level>()
                   .ForMember(dest => dest.PremiumType, opt => opt.MapFrom(src => (int)src.PremiumType))
                   .ForMember(dest => dest.ApplyOnType, opt => opt.MapFrom(src => (int)src.ApplyOnType))
                   .ForMember(dest => dest.CalculationType, opt => opt.MapFrom(src => (int)src.CalculationType));
                cfg.CreateMap<LevelPaymentDTO, LevelPayment>();
                cfg.CreateMap<LevelCompanyDTO, LevelCompany>()
                        .ForMember(dest => dest.PresentationInformationType, opt => opt.MapFrom(src => (int)src.PresentationInformationType));
                cfg.CreateMap<LevelRestoreDTO, LevelRestore>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<AgentDTO, Agent>();
            });
            return config;
        }

        internal static  Contract ToModel(this ContractDTO contractDTO)
        {
            var config = CreateMapContracts();
            return config.Map<ContractDTO, Contract>(contractDTO);
        }

        internal static  IEnumerable<Contract> ToModels(this IEnumerable<ContractDTO> contractDTOs)
        {
            return contractDTOs.Select(ToModel);
        }

        internal static  IMapper CreateMapContractFunctionalities()
        {
            var config = MapperCache.GetMapper<ContractFunctionalityDTO, ContractFunctionality>(cfg =>
            {
                cfg.CreateMap<ContractFunctionalityDTO, ContractFunctionality>();
            });
            return config;
        }

        internal static  ContractFunctionality ToModel(this ContractFunctionalityDTO contractFunctionalityDTO)
        {
            var config = CreateMapContractFunctionalities();
            return config.Map<ContractFunctionalityDTO, ContractFunctionality>(contractFunctionalityDTO);
        }

        internal static  IEnumerable<ContractFunctionality> ToModels(this IEnumerable<ContractFunctionalityDTO> contractFunctionalityDTOs)
        {
            return contractFunctionalityDTOs.Select(ToModel);
        }

        internal static  IMapper CreateMapContractLines()
        {
            var config = MapperCache.GetMapper<ContractLineDTO, ContractLine>(cfg =>
            {
                cfg.CreateMap<ContractLineDTO, ContractLine>();
                cfg.CreateMap<ContractDTO, Contract>();
                cfg.CreateMap<ResettlementTypeDTO, ResettlementType>();
                cfg.CreateMap<AffectationTypeDTO, AffectationType>();
                cfg.CreateMap<EPITypeDTO, EPIType>();
                cfg.CreateMap<ContractTypeDTO, ContractType>();
                cfg.CreateMap<ContractFunctionalityDTO, ContractFunctionality>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<LevelDTO, Level>()
                   .ForMember(dest => dest.PremiumType, opt => opt.MapFrom(src => (int)src.PremiumType))
                   .ForMember(dest => dest.ApplyOnType, opt => opt.MapFrom(src => (int)src.ApplyOnType))
                   .ForMember(dest => dest.CalculationType, opt => opt.MapFrom(src => (int)src.CalculationType));
                cfg.CreateMap<LevelPaymentDTO, LevelPayment>();
                cfg.CreateMap<LevelCompanyDTO, LevelCompany>()
                        .ForMember(dest => dest.PresentationInformationType, opt => opt.MapFrom(src => (int)src.PresentationInformationType));
                cfg.CreateMap<LevelRestoreDTO, LevelRestore>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<AgentDTO, Agent>();
            });
            return config;
        }

        internal static  ContractLine ToModel(this ContractLineDTO contractLineDTO)
        {
            var config = CreateMapContractLines();
            return config.Map<ContractLineDTO, ContractLine>(contractLineDTO);
        }

        internal static  IEnumerable<ContractLine> ToModels(this IEnumerable<ContractLineDTO> contractLineDTOs)
        {
            return contractLineDTOs.Select(ToModel);
        }

        internal static  IMapper CreateMapContractTypes()
        {
            var config = MapperCache.GetMapper<ContractTypeDTO, ContractType>(cfg =>
            {
                cfg.CreateMap<ContractTypeDTO, ContractType>();
                cfg.CreateMap<ContractFunctionalityDTO, ContractFunctionality>();
            });
            return config;
        }

        internal static  ContractType ToModel(this ContractTypeDTO contractTypeDTO)
        {
            var config = CreateMapContractTypes();
            return config.Map<ContractTypeDTO, ContractType>(contractTypeDTO);
        }

        internal static  IEnumerable<ContractType> ToModels(this IEnumerable<ContractTypeDTO> contractTypeDTOs)
        {
            return contractTypeDTOs.Select(ToModel);
        }

        internal static  IMapper CreateMapCumulusTypes()
        {
            var config = MapperCache.GetMapper<CumulusTypeDTO, CumulusType>(cfg =>
            {
                cfg.CreateMap<CumulusTypeDTO, CumulusType>();
            });
            return config;
        }

        internal static  CumulusType ToModel(this CumulusTypeDTO cumulusTypeDTO)
        {
            var config = CreateMapCumulusTypes();
            return config.Map<CumulusTypeDTO, CumulusType>(cumulusTypeDTO);
        }

        internal static  IEnumerable<CumulusType> ToModels(this IEnumerable<CumulusTypeDTO> cumulusTypeDTOs)
        {
            return cumulusTypeDTOs.Select(ToModel);
        }

        internal static  IMapper CreateMapEPITypes()
        {
            var config = MapperCache.GetMapper<EPITypeDTO, EPIType>(cfg =>
            {
                cfg.CreateMap<EPITypeDTO, EPIType>();
            });
            return config;
        }

        internal static  EPIType ToModel(this EPITypeDTO ePITypeDTO)
        {
            var config = CreateMapEPITypes();
            return config.Map<EPITypeDTO, EPIType>(ePITypeDTO);
        }

        internal static  IEnumerable<EPIType> ToModels(this IEnumerable<EPITypeDTO> ePITypeDTOs)
        {
            return ePITypeDTOs.Select(ToModel);
        }

        internal static  IMapper CreateMapInstallments()
        {
            var config = MapperCache.GetMapper<InstallmentDTO, Installment>(cfg =>
            {
                cfg.CreateMap<InstallmentDTO, Installment>();
                cfg.CreateMap<LevelCompanyDTO, LevelCompany>()
                        .ForMember(dest => dest.PresentationInformationType, opt => opt.MapFrom(src => (int)src.PresentationInformationType));
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<CompanyDTO, Company>();
                cfg.CreateMap<AgentDTO, Agent>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<LevelDTO, Level>()
                   .ForMember(dest => dest.PremiumType, opt => opt.MapFrom(src => (int)src.PremiumType))
                   .ForMember(dest => dest.ApplyOnType, opt => opt.MapFrom(src => (int)src.ApplyOnType))
                   .ForMember(dest => dest.CalculationType, opt => opt.MapFrom(src => (int)src.CalculationType));
                cfg.CreateMap<LevelPaymentDTO, LevelPayment>();
                cfg.CreateMap<LevelRestoreDTO, LevelRestore>();
                cfg.CreateMap<ContractDTO, Contract>();
                cfg.CreateMap<ResettlementTypeDTO, ResettlementType>();
                cfg.CreateMap<AffectationTypeDTO, AffectationType>();
                cfg.CreateMap<EPITypeDTO, EPIType>();
                cfg.CreateMap<ContractTypeDTO, ContractType>();
                cfg.CreateMap<ContractFunctionalityDTO, ContractFunctionality>();
            });
            return config;
        }

        internal static  Installment ToModel(this InstallmentDTO installmentDTO)
        {
            var config = CreateMapInstallments();
            return config.Map<InstallmentDTO, Installment>(installmentDTO);
        }

        internal static  IEnumerable<Installment> ToModels(this IEnumerable<InstallmentDTO> installmentDTOs)
        {
            return installmentDTOs.Select(ToModel);
        }

        internal static  IMapper CreateMapLevels()
        {
            var config = MapperCache.GetMapper<LevelDTO, Level>(cfg =>
            {
                cfg.CreateMap<LevelDTO, Level>()
                        .ForMember(dest => dest.CalculationType, opt => opt.MapFrom(src => (int)src.CalculationType))
                        .ForMember(dest => dest.ApplyOnType, opt => opt.MapFrom(src => (int)src.ApplyOnType))
                        .ForMember(dest => dest.PremiumType, opt => opt.MapFrom(src => (int)src.PremiumType));
                cfg.CreateMap<ContractDTO, Contract>();
                cfg.CreateMap<ResettlementTypeDTO, ResettlementType>();
                cfg.CreateMap<AffectationTypeDTO, AffectationType>();
                cfg.CreateMap<EPITypeDTO, EPIType>();
                cfg.CreateMap<ContractTypeDTO, ContractType>();
                cfg.CreateMap<ContractFunctionalityDTO, ContractFunctionality>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<LevelPaymentDTO, LevelPayment>();
                cfg.CreateMap<LevelCompanyDTO, LevelCompany>()
                        .ForMember(dest => dest.PresentationInformationType, opt => opt.MapFrom(src => (int)src.PresentationInformationType));
                cfg.CreateMap<LevelRestoreDTO, LevelRestore>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<AgentDTO, Agent>();
            });
            return config;
        }

        internal static  Level ToModel(this LevelDTO levelDTO)
        {
            var config = CreateMapLevels();
            return config.Map<LevelDTO, Level>(levelDTO);
        }

        internal static  IEnumerable<Level> ToModels(this IEnumerable<LevelDTO> levelDTOs)
        {
            return levelDTOs.Select(ToModel);
        }

        internal static  IMapper CreateMapLevelCompanies()
        {
            var config = MapperCache.GetMapper<LevelCompanyDTO, LevelCompany>(cfg =>
            {
                cfg.CreateMap<LevelCompanyDTO, LevelCompany>()
                    .ForMember(dest => dest.PresentationInformationType, opt => opt.MapFrom(src => (int)src.PresentationInformationType));
                cfg.CreateMap<AgentDTO, Agent>();
                cfg.CreateMap<CompanyDTO, Company>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<LevelDTO, Level>()
                    .ForMember(dest => dest.PremiumType, opt => opt.MapFrom(src => (int)src.PremiumType))
                    .ForMember(dest => dest.ApplyOnType, opt => opt.MapFrom(src => (int)src.ApplyOnType))
                    .ForMember(dest => dest.CalculationType, opt => opt.MapFrom(src => (int)src.CalculationType));
                cfg.CreateMap<InstallmentDTO, Installment>();
                cfg.CreateMap<LevelPaymentDTO, LevelPayment>();
                cfg.CreateMap<ContractDTO, Contract>();
                cfg.CreateMap<ResettlementTypeDTO, ResettlementType>();
                cfg.CreateMap<AffectationTypeDTO, AffectationType>();
                cfg.CreateMap<EPITypeDTO, EPIType>();
                cfg.CreateMap<ContractTypeDTO, ContractType>();
                cfg.CreateMap<ContractFunctionalityDTO, ContractFunctionality>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<LevelRestoreDTO, LevelRestore>();
            });
            return config;
        }

        internal static  LevelCompany ToModel(this LevelCompanyDTO levelCompanyDTO)
        {
            var config = CreateMapLevelCompanies();
            return config.Map<LevelCompanyDTO, LevelCompany>(levelCompanyDTO);
        }

        internal static  IEnumerable<LevelCompany> ToModels(this IEnumerable<LevelCompanyDTO> levelCompanyDTOs)
        {
            return levelCompanyDTOs.Select(ToModel);
        }

        internal static  IMapper CreateMapLevelPayments()
        {
            var config = MapperCache.GetMapper<LevelPaymentDTO, LevelPayment>(cfg =>
            {
                cfg.CreateMap<LevelPayment, LevelPaymentDTO>();
                cfg.CreateMap<Level, LevelDTO>()
                    .ForMember(dest => dest.PremiumType, opt => opt.MapFrom(src => (int)src.PremiumType))
                    .ForMember(dest => dest.ApplyOnType, opt => opt.MapFrom(src => (int)src.ApplyOnType))
                    .ForMember(dest => dest.CalculationType, opt => opt.MapFrom(src => (int)src.CalculationType));
                cfg.CreateMap<Amount, AmountDTO>();
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

        internal static  LevelPayment ToModel(this LevelPaymentDTO levelPaymentDTO)
        {
            var config = CreateMapLevelPayments();
            return config.Map<LevelPaymentDTO, LevelPayment>(levelPaymentDTO);
        }

        internal static  IEnumerable<LevelPayment> ToModels(this IEnumerable<LevelPaymentDTO> levelPaymentDTOs)
        {
            return levelPaymentDTOs.Select(ToModel);
        }        

        internal static  IMapper CreateMapAssociationLines()
        {
            var config = MapperCache.GetMapper<AssociationLineDTO, AssociationLine>(cfg =>
            {
                cfg.CreateMap<AssociationLineDTO, AssociationLine>();
                cfg.CreateMap<ByPolicyLineBusinessSubLineBusinessDTO, ByPolicyLineBusinessSubLineBusiness>();
                cfg.CreateMap<PolicyDTO, TempCommonServices.Models.Policy>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<PrefixDTO, Prefix>();
                cfg.CreateMap<EndorsementDTO, Endorsement>();
                cfg.CreateMap<LineBusinessDTO, LineBusiness>();
                cfg.CreateMap<SubLineBusinessDTO, SubLineBusiness>();
                cfg.CreateMap<InsuredObjectDTO, InsuredObject>();
                cfg.CreateMap<ByInsuredDTO, ByInsured>();
                cfg.CreateMap<IndividualDTO, Individual>();
                cfg.CreateMap<ByInsuredPrefixDTO, ByInsuredPrefix>();
                cfg.CreateMap<ByFacultativeIssueDTO, ByFacultativeIssue>();
                cfg.CreateMap<ByPolicyDTO, ByPolicy>();
                cfg.CreateMap<ByOperationTypePrefixDTO, ByOperationTypePrefix>();
                cfg.CreateMap<ByLineBusinessSubLineBusinessDTO, ByLineBusinessSubLineBusiness>();
                cfg.CreateMap<ByLineBusinessSubLineBusinessCoverageDTO, ByLineBusinessSubLineBusinessCoverage>();
                cfg.CreateMap<CoverageDTO, Coverage>();
                cfg.CreateMap<ByPrefixProductDTO, ByPrefixProduct>();
            });
            return config;
        }

        internal static  AssociationLine ToModel(this AssociationLineDTO associationLineDTO)
        {
            var config = CreateMapAssociationLines();
            return config.Map<AssociationLineDTO, AssociationLine>(associationLineDTO);
        }

        internal static  IEnumerable<AssociationLine> ToModels(this IEnumerable<AssociationLineDTO> associationLineDTO)
        {
            return associationLineDTO.Select(ToModel);
        }


        internal static  IMapper CreateMapLevelRestores()
        {
            var config = MapperCache.GetMapper<LevelRestoreDTO, LevelRestore>(cfg =>
            {
                cfg.CreateMap<LevelRestoreDTO, LevelRestore>();
                cfg.CreateMap<LevelDTO, Level>()
                   .ForMember(dest => dest.PremiumType, opt => opt.MapFrom(src => (int)src.PremiumType))
                   .ForMember(dest => dest.ApplyOnType, opt => opt.MapFrom(src => (int)src.ApplyOnType))
                   .ForMember(dest => dest.CalculationType, opt => opt.MapFrom(src => (int)src.CalculationType));
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<InstallmentDTO, Installment>();
                cfg.CreateMap<LevelPaymentDTO, LevelPayment>();
                cfg.CreateMap<ContractDTO, Contract>();
                cfg.CreateMap<ResettlementTypeDTO, ResettlementType>();
                cfg.CreateMap<AffectationTypeDTO, AffectationType>();
                cfg.CreateMap<EPITypeDTO, EPIType>();
                cfg.CreateMap<ContractTypeDTO, ContractType>();
                cfg.CreateMap<ContractFunctionalityDTO, ContractFunctionality>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<LevelCompanyDTO, LevelCompany>()
                        .ForMember(dest => dest.PresentationInformationType, opt => opt.MapFrom(src => (int)src.PresentationInformationType));
                cfg.CreateMap<AgentDTO, Agent>();
            });
            return config;
        }

        internal static  LevelRestore ToModel(this LevelRestoreDTO levelRestoreDTO)
        {
            var config = CreateMapLevelRestores();
            return config.Map<LevelRestoreDTO, LevelRestore>(levelRestoreDTO);
        }

        internal static  IEnumerable<LevelRestore> ToModels(this IEnumerable<LevelRestoreDTO> levelRestoreDTOs)
        {
            return levelRestoreDTOs.Select(ToModel);
        }

        internal static  IMapper CreateMapLines()
        {
            var config = MapperCache.GetMapper<LineDTO, Line>(cfg =>
            {
                cfg.CreateMap<LineDTO, Line>();
                cfg.CreateMap<CumulusTypeDTO, CumulusType>();
                cfg.CreateMap<ContractLineDTO, ContractLine>();
                cfg.CreateMap<ContractDTO, Contract>();
                cfg.CreateMap<ResettlementTypeDTO, ResettlementType>();
                cfg.CreateMap<AffectationTypeDTO, AffectationType>();
                cfg.CreateMap<EPITypeDTO, EPIType>();
                cfg.CreateMap<ContractTypeDTO, ContractType>();
                cfg.CreateMap<ContractFunctionalityDTO, ContractFunctionality>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<LevelDTO, Level>()
                   .ForMember(dest => dest.PremiumType, opt => opt.MapFrom(src => (int)src.PremiumType))
                   .ForMember(dest => dest.ApplyOnType, opt => opt.MapFrom(src => (int)src.ApplyOnType))
                   .ForMember(dest => dest.CalculationType, opt => opt.MapFrom(src => (int)src.CalculationType));
                cfg.CreateMap<LevelPaymentDTO, LevelPayment>();
                cfg.CreateMap<LevelCompanyDTO, LevelCompany>()
                        .ForMember(dest => dest.PresentationInformationType, opt => opt.MapFrom(src => (int)src.PresentationInformationType));
                cfg.CreateMap<InstallmentDTO, Installment>();
                
                cfg.CreateMap<LevelRestoreDTO, LevelRestore>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<AgentDTO, Agent>();
            });
            return config;
        }

        internal static  Line ToModel(this LineDTO lineDTO)
        {
            var config = CreateMapLines();
            return config.Map<LineDTO, Line>(lineDTO);
        }

        internal static  IEnumerable<Line> ToModels(this IEnumerable<LineDTO> lineDTOs)
        {
            return lineDTOs.Select(ToModel);
        }

        internal static  IMapper CreateMapReinsurances()
        {
            var config = MapperCache.GetMapper<ReinsuranceDTO, EEProvider.Models.Reinsurance.Reinsurance>(cfg =>
            {
                cfg.CreateMap<ReinsuranceDTO, EEProvider.Models.Reinsurance.Reinsurance>()
                        .ForMember(dest => dest.Movements, opt => opt.MapFrom(src => src.Movements));
                cfg.CreateMap<ReinsuranceLayerDTO, ReinsuranceLayer>();
                cfg.CreateMap<ReinsuranceLineDTO, ReinsuranceLine>();
                cfg.CreateMap<ReinsuranceAllocationDTO, ReinsuranceAllocation>();
                cfg.CreateMap<ReinsuranceCumulusRiskCoverageDTO, ReinsuranceCumulusRiskCoverage>();
                cfg.CreateMap<ContractDTO, Contract>();
                cfg.CreateMap<ResettlementTypeDTO, ResettlementType>();
                cfg.CreateMap<AffectationTypeDTO, AffectationType>();
                cfg.CreateMap<EPITypeDTO, EPIType>();
                cfg.CreateMap<ContractTypeDTO, ContractType>();
                cfg.CreateMap<ContractFunctionalityDTO, ContractFunctionality>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<LevelDTO, Level>()
                   .ForMember(dest => dest.PremiumType, opt => opt.MapFrom(src => (int)src.PremiumType))
                   .ForMember(dest => dest.ApplyOnType, opt => opt.MapFrom(src => (int)src.ApplyOnType))
                   .ForMember(dest => dest.CalculationType, opt => opt.MapFrom(src => (int)src.CalculationType));
                cfg.CreateMap<LevelPaymentDTO, LevelPayment>();
                cfg.CreateMap<LevelCompanyDTO, LevelCompany>()
                        .ForMember(dest => dest.PresentationInformationType, opt => opt.MapFrom(src => (int)src.PresentationInformationType));
                cfg.CreateMap<LevelRestoreDTO, LevelRestore>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<AgentDTO, Agent>();
            });
            return config;
        }

        internal static  EEProvider.Models.Reinsurance.Reinsurance ToModel(this ReinsuranceDTO reinsuranceDTO)
        {
            var config = CreateMapReinsurances();
            return config.Map<ReinsuranceDTO, EEProvider.Models.Reinsurance.Reinsurance>(reinsuranceDTO);
        }

        internal static  IEnumerable<EEProvider.Models.Reinsurance.Reinsurance> ToModels(this IEnumerable<ReinsuranceDTO> reinsuranceDTOs)
        {
            return reinsuranceDTOs.Select(ToModel);
        }

        internal static  IMapper CreateMapReinsuranceAllocations()
        {
            var config = MapperCache.GetMapper<ReinsuranceAllocationDTO, ReinsuranceAllocation>(cfg =>
            {
                cfg.CreateMap<ReinsuranceAllocationDTO, ReinsuranceAllocation>();
                cfg.CreateMap<ContractDTO, Contract>();
                cfg.CreateMap<ResettlementTypeDTO, ResettlementType>();
                cfg.CreateMap<AffectationTypeDTO, AffectationType>();
                cfg.CreateMap<EPITypeDTO, EPIType>();
                cfg.CreateMap<ContractTypeDTO, ContractType>();
                cfg.CreateMap<ContractFunctionalityDTO, ContractFunctionality>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<LevelDTO, Level>()
                   .ForMember(dest => dest.PremiumType, opt => opt.MapFrom(src => (int)src.PremiumType))
                   .ForMember(dest => dest.ApplyOnType, opt => opt.MapFrom(src => (int)src.ApplyOnType))
                   .ForMember(dest => dest.CalculationType, opt => opt.MapFrom(src => (int)src.CalculationType));
                cfg.CreateMap<LevelPaymentDTO, LevelPayment>();
                cfg.CreateMap<LevelCompanyDTO, LevelCompany>()
                        .ForMember(dest => dest.PresentationInformationType, opt => opt.MapFrom(src => (int)src.PresentationInformationType));
                cfg.CreateMap<LevelRestoreDTO, LevelRestore>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<AgentDTO, Agent>();
            });
            return config;
        }

        internal static  ReinsuranceAllocation ToModel(this ReinsuranceAllocationDTO reinsuranceAllocationDTO)
        {
            var config = CreateMapReinsuranceAllocations();
            return config.Map<ReinsuranceAllocationDTO, ReinsuranceAllocation>(reinsuranceAllocationDTO);
        }

        internal static  IEnumerable<ReinsuranceAllocation> ToModels(this IEnumerable<ReinsuranceAllocationDTO> reinsuranceAllocationDTOs)
        {
            return reinsuranceAllocationDTOs.Select(ToModel);
        }

        internal static  IMapper CreateMapReinsuranceCumulusRiskCoverages()
        {
            var config = MapperCache.GetMapper<ReinsuranceCumulusRiskCoverageDTO, ReinsuranceCumulusRiskCoverage>(cfg =>
            {
                cfg.CreateMap<ReinsuranceCumulusRiskCoverageDTO, ReinsuranceCumulusRiskCoverage>();
            });
            return config;
        }

        internal static  ReinsuranceCumulusRiskCoverage ToModel(this ReinsuranceCumulusRiskCoverageDTO reinsuranceCumulusRiskCoverageDTO)
        {
            var config = CreateMapReinsuranceCumulusRiskCoverages();
            return config.Map<ReinsuranceCumulusRiskCoverageDTO, ReinsuranceCumulusRiskCoverage>(reinsuranceCumulusRiskCoverageDTO);
        }

        internal static  IEnumerable<ReinsuranceCumulusRiskCoverage> ToModels(this IEnumerable<ReinsuranceCumulusRiskCoverageDTO> reinsuranceCumulusRiskCoverageDTOs)
        {
            return reinsuranceCumulusRiskCoverageDTOs.Select(ToModel);
        }

        internal static  IMapper CreateMapReinsuranceLayers()
        {
            var config = MapperCache.GetMapper<ReinsuranceLayerDTO, ReinsuranceLayer>(cfg =>
            {
                cfg.CreateMap<ReinsuranceLayerDTO, ReinsuranceLayer>();
                cfg.CreateMap<ReinsuranceLineDTO, ReinsuranceLine>();
                cfg.CreateMap<ReinsuranceAllocationDTO, ReinsuranceAllocation>();
                cfg.CreateMap<ReinsuranceCumulusRiskCoverageDTO, ReinsuranceCumulusRiskCoverage>();
                cfg.CreateMap<LineDTO, Line>();
                cfg.CreateMap<CumulusTypeDTO, CumulusType>();
                cfg.CreateMap<ContractLineDTO, ContractLine>();
                cfg.CreateMap<ContractDTO, Contract>();
                cfg.CreateMap<ResettlementTypeDTO, ResettlementType>();
                cfg.CreateMap<AffectationTypeDTO, AffectationType>();
                cfg.CreateMap<EPITypeDTO, EPIType>();
                cfg.CreateMap<ContractTypeDTO, ContractType>();
                cfg.CreateMap<ContractFunctionalityDTO, ContractFunctionality>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<LevelDTO, Level>()
                   .ForMember(dest => dest.PremiumType, opt => opt.MapFrom(src => (int)src.PremiumType))
                   .ForMember(dest => dest.ApplyOnType, opt => opt.MapFrom(src => (int)src.ApplyOnType))
                   .ForMember(dest => dest.CalculationType, opt => opt.MapFrom(src => (int)src.CalculationType));
                cfg.CreateMap<LevelPaymentDTO, LevelPayment>();
                cfg.CreateMap<LevelCompanyDTO, LevelCompany>()
                        .ForMember(dest => dest.PresentationInformationType, opt => opt.MapFrom(src => (int)src.PresentationInformationType));
                cfg.CreateMap<LevelRestoreDTO, LevelRestore>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<AgentDTO, Agent>();
            });
            return config;
        }

        internal static  ReinsuranceLayer ToModel(this ReinsuranceLayerDTO reinsuranceLayerDTO)
        {
            var config = CreateMapReinsuranceLayers();
            return config.Map<ReinsuranceLayerDTO, ReinsuranceLayer>(reinsuranceLayerDTO);
        }

        internal static  IEnumerable<ReinsuranceLayer> ToModels(this IEnumerable<ReinsuranceLayerDTO> reinsuranceLayerDTOs)
        {
            return reinsuranceLayerDTOs.Select(ToModel);
        }

        internal static  IMapper CreateMapReinsuranceLines()
        {
            var config = MapperCache.GetMapper<ReinsuranceLineDTO, ReinsuranceLine>(cfg =>
            {
                cfg.CreateMap<ReinsuranceLineDTO, ReinsuranceLine>();
                cfg.CreateMap<ReinsuranceAllocationDTO, ReinsuranceAllocation>();
                cfg.CreateMap<ReinsuranceCumulusRiskCoverageDTO, ReinsuranceCumulusRiskCoverage>();
                cfg.CreateMap<ContractDTO, Contract>();
                cfg.CreateMap<ResettlementTypeDTO, ResettlementType>();
                cfg.CreateMap<AffectationTypeDTO, AffectationType>();
                cfg.CreateMap<EPITypeDTO, EPIType>();
                cfg.CreateMap<ContractTypeDTO, ContractType>();
                cfg.CreateMap<ContractFunctionalityDTO, ContractFunctionality>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<LevelDTO, Level>()
                   .ForMember(dest => dest.PremiumType, opt => opt.MapFrom(src => (int)src.PremiumType))
                   .ForMember(dest => dest.ApplyOnType, opt => opt.MapFrom(src => (int)src.ApplyOnType))
                   .ForMember(dest => dest.CalculationType, opt => opt.MapFrom(src => (int)src.CalculationType));
                cfg.CreateMap<LevelPaymentDTO, LevelPayment>();
                cfg.CreateMap<LevelCompanyDTO, LevelCompany>()
                        .ForMember(dest => dest.PresentationInformationType, opt => opt.MapFrom(src => (int)src.PresentationInformationType));
                cfg.CreateMap<LevelRestoreDTO, LevelRestore>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<AgentDTO, Agent>();
            });
            return config;
        }

        internal static  ReinsuranceLine ToModel(this ReinsuranceLineDTO reinsuranceLineDTO)
        {
            var config = CreateMapReinsuranceLines();
            return config.Map<ReinsuranceLineDTO, ReinsuranceLine>(reinsuranceLineDTO);
        }

        internal static  IEnumerable<ReinsuranceLine> ToModels(this IEnumerable<ReinsuranceLineDTO> reinsuranceLineDTOs)
        {
            return reinsuranceLineDTOs.Select(ToModel);
        }

        internal static  IMapper CreateMapResettlementTypes()
        {
            var config = MapperCache.GetMapper<ResettlementTypeDTO, ResettlementType>(cfg =>
            {
                cfg.CreateMap<ResettlementTypeDTO, ResettlementType>();
            });
            return config;
        }

        internal static  ResettlementType ToModel(this ResettlementTypeDTO resettlementTypeDTO)
        {
            var config = CreateMapResettlementTypes();
            return config.Map<ResettlementTypeDTO, ResettlementType>(resettlementTypeDTO);
        }

        internal static  IEnumerable<ResettlementType> ToModels(this IEnumerable<ResettlementTypeDTO> resettlementTypeDTOs)
        {
            return resettlementTypeDTOs.Select(ToModel);
        }


        internal static  IMapper CreateMapCumuluss()
        {
            var config = MapperCache.GetMapper<Cumulus, CumulusDTO>(cfg =>
            {
                cfg.CreateMap<CumulusDTO, Cumulus>();
                cfg.CreateMap<IssueLayerDTO, IssueLayer>();
                cfg.CreateMap<IssueLayerLineDTO, IssueLayerLine>();
                cfg.CreateMap<IssueAllocationDTO, IssueAllocation>();
                cfg.CreateMap<ContractDTO, Contract>();
                cfg.CreateMap<ResettlementTypeDTO, ResettlementType>();
                cfg.CreateMap<AffectationTypeDTO, AffectationType>();
                cfg.CreateMap<EPITypeDTO, EPIType>();
                cfg.CreateMap<ContractTypeDTO, ContractType>();
                cfg.CreateMap<ContractFunctionalityDTO, ContractFunctionality>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<LevelDTO, Level>()
                   .ForMember(dest => dest.PremiumType, opt => opt.MapFrom(src => (int)src.PremiumType))
                   .ForMember(dest => dest.ApplyOnType, opt => opt.MapFrom(src => (int)src.ApplyOnType))
                   .ForMember(dest => dest.CalculationType, opt => opt.MapFrom(src => (int)src.CalculationType));
                cfg.CreateMap<LevelPaymentDTO, LevelPayment>();
                cfg.CreateMap<LevelCompanyDTO, LevelCompany>()
                        .ForMember(dest => dest.PresentationInformationType, opt => opt.MapFrom(src => (int)src.PresentationInformationType));
                cfg.CreateMap<LevelRestoreDTO, LevelRestore>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<AgentDTO, Agent>();
            });
            return config;
        }

        internal static  Cumulus ToDTO(this CumulusDTO cumulus)
        {
            var config = CreateMapCumuluss();
            return config.Map<CumulusDTO, Cumulus>(cumulus);
        }

        internal static  IEnumerable<Cumulus> ToDTOs(this IEnumerable<CumulusDTO> cumulus)
        {
            return cumulus.Select(ToDTO);
        }

        internal static  IMapper CreateMapClaimReinsurances()
        {
            var config = MapperCache.GetMapper<ClaimReinsuranceDTO, ClaimReinsurance>(cfg =>
            {
                cfg.CreateMap<ClaimReinsuranceDTO, ClaimReinsurance>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<EstimationTypeDTO, EstimationType>();
            });
            return config;
        }

        internal static  IMapper CreateMapPriorityRetentions()
        {
            var config = MapperCache.GetMapper<PriorityRetentionDTO, PriorityRetention>(cfg =>
            {
                cfg.CreateMap<PriorityRetentionDTO, ClaimReinsurance>();
                cfg.CreateMap<PrefixDTO, Prefix>();
            });
            return config;
        }

        internal static  ClaimReinsurance ToModel(this ClaimReinsuranceDTO claimReinsuranceDTO)
        {
            var config = CreateMapClaimReinsurances();
            return config.Map<ClaimReinsuranceDTO, ClaimReinsurance>(claimReinsuranceDTO);
        }

        internal static  PriorityRetention ToModel(this PriorityRetentionDTO priorityRetentionDTO)
        {
            var config = CreateMapPriorityRetentions();
            return config.Map<PriorityRetentionDTO, PriorityRetention>(priorityRetentionDTO);
        }

        internal static  IEnumerable<ClaimReinsurance> ToModels(this IEnumerable<ClaimReinsuranceDTO> claimReinsuranceDTOs)
        {
            return claimReinsuranceDTOs.Select(ToModel);
        }


        internal static  IMapper CreateMapClaim()
        {
            var config = MapperCache.GetMapper<ClaimDTO, Claim>(cfg =>
            {
                cfg.CreateMap<ClaimDTO, Claim>();
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<PrefixDTO, Prefix>();
                cfg.CreateMap<ClaimEndorsementDTO, ClaimEndorsement>();
                cfg.CreateMap<CatastrophicEventDTO, CatastrophicEvent>();
                cfg.CreateMap<InspectionDTO, Inspection>();
                cfg.CreateMap<CityDTO, City>();
                cfg.CreateMap<CauseDTO, Cause>();
                cfg.CreateMap<ClaimModifyDTO, ClaimModify>();
                cfg.CreateMap<DamageTypeDTO, DamageType>();
                cfg.CreateMap<DamageResponsabilityDTO, DamageResponsibility>();
                cfg.CreateMap<TextOperationDTO, TextOperation>();
                cfg.CreateMap<EndorsementDTO, Endorsement>();
                cfg.CreateMap<ClaimCoverageDTO, ClaimCoverage>();
                cfg.CreateMap<EstimationDTO, Estimation>();
                cfg.CreateMap<EstimationTypeDTO, EstimationType>();
                cfg.CreateMap<ReasonDTO, Reason>();
                cfg.CreateMap<StatusDTO, Status>();
                cfg.CreateMap<InternalStatusDTO, InternalStatus>();
                cfg.CreateMap<StateDTO, State>();
                cfg.CreateMap<CountryDTO, Country>();
            });
            return config;
        }

        internal static  Claim ToDTO(this ClaimDTO claim)
        {
            var config = CreateMapClaim();
            return config.Map<ClaimDTO, Claim>(claim);
        }

        internal static  IEnumerable<Claim> ToDTOs(this IEnumerable<ClaimDTO> claim)
        {
            return claim.Select(ToDTO);
        }

        internal static  IMapper CreateMapReinsurancePrefixes()
        {
            var config = MapperCache.GetMapper<ReinsurancePrefixDTO, ReinsurancePrefix>(cfg =>
            {
                cfg.CreateMap<ReinsurancePrefixDTO, ReinsurancePrefix>();
                cfg.CreateMap<PrefixDTO, Prefix>();
            });
            return config;
        }

        internal static  ReinsurancePrefix ToModel(this ReinsurancePrefixDTO reinsurancePrefixDTO)
        {
            var config = CreateMapReinsurancePrefixes();

            return config.Map<ReinsurancePrefixDTO, ReinsurancePrefix>(reinsurancePrefixDTO);
        }

        internal static  IEnumerable<ReinsurancePrefix> ToModels(this IEnumerable<ReinsurancePrefixDTO> reinsurancePrefixDTOs)
        {
            return reinsurancePrefixDTOs.Select(ToModel);
        }

        internal static  IMapper CreateMapReinsuranceDistributionHeader()
        {
            var config = MapperCache.GetMapper<ReinsuranceDistributionHeaderDTO, ReinsuranceDistributionHeader>(cfg =>
            {
                cfg.CreateMap<ReinsuranceDistributionHeaderDTO, ReinsuranceDistributionHeader>();
            });
            return config;
        }

        internal static  ReinsuranceDistributionHeader ToDTO(this ReinsuranceDistributionHeaderDTO reinsuranceDistributionHeader)
        {
            var config = CreateMapReinsuranceDistributionHeader();
            return config.Map<ReinsuranceDistributionHeaderDTO, ReinsuranceDistributionHeader>(reinsuranceDistributionHeader);
        }

        internal static  IEnumerable<ReinsuranceDistributionHeader> ToDTOs(this IEnumerable<ReinsuranceDistributionHeaderDTO> reinsuranceDistributionHeaders)
        {
            return reinsuranceDistributionHeaders.Select(ToDTO);
        }

        internal static  IMapper CreateMapTempReinsuranceProcesses()
        {
            var config = MapperCache.GetMapper<TempReinsuranceProcessDTO, TempReinsuranceProcess>(cfg =>
            {
                cfg.CreateMap<TempReinsuranceProcessDTO, TempReinsuranceProcess>();
            });
            return config;
        }

        internal static  TempReinsuranceProcess ToDTO(this TempReinsuranceProcessDTO tempReinsuranceProcess)
        {
            var config = CreateMapTempReinsuranceProcesses();
            return config.Map<TempReinsuranceProcessDTO, TempReinsuranceProcess>(tempReinsuranceProcess);
        }

        internal static  IEnumerable<TempReinsuranceProcess> ToDTOs(this IEnumerable<TempReinsuranceProcessDTO> tempReinsuranceProcesses)
        {
            return tempReinsuranceProcesses.Select(ToDTO);
        }

        internal static  IMapper CreateMapPaymentRequests()
        {
            var config = MapperCache.GetMapper<PaymentRequestDTO, PaymentRequest>(cfg =>
            {
                cfg.CreateMap<PaymentRequestDTO, PaymentRequest>();
            });
            return config;
        }

        internal static  PaymentRequest ToDTO(this PaymentRequestDTO reinsuranceClaimPaymentRequest)
        {
            var config = CreateMapPaymentRequests();
            return config.Map<PaymentRequestDTO, PaymentRequest>(reinsuranceClaimPaymentRequest);
        }

        internal static  IEnumerable<PaymentRequest> ToDTOs(this IEnumerable<PaymentRequestDTO> reinsuranceClaimPaymentRequests)
        {
            return reinsuranceClaimPaymentRequests.Select(ToDTO);
        }

       
        internal static  IMapper CreateMapIssGetDistributionErrors()
        {
            var config = MapperCache.GetMapper<IssGetDistributionErrorsDTO, IssGetDistributionErrors>(cfg =>
            {
                cfg.CreateMap<IssGetDistributionErrorsDTO, IssGetDistributionErrors>();
            });
            return config;
        }

        internal static  IssGetDistributionErrors ToDTO(this IssGetDistributionErrorsDTO issGetDistributionErrors)
        {
            var config = CreateMapIssGetDistributionErrors();
            return config.Map<IssGetDistributionErrorsDTO, IssGetDistributionErrors>(issGetDistributionErrors);
        }

        internal static  IEnumerable<IssGetDistributionErrors> ToDTOs(this IEnumerable<IssGetDistributionErrorsDTO> issGetDistributionErrors)
        {
            return issGetDistributionErrors.Select(ToDTO);
        }

        internal static  IMapper CreateMapLineCumulusType()
        {
            var config = MapperCache.GetMapper<LineCumulusTypeDTO, LineCumulusType>(cfg =>
            {
                cfg.CreateMap<LineCumulusTypeDTO, LineCumulusType>();
            });
            return config;
        }

        internal static  LineCumulusType ToDTO(this LineCumulusTypeDTO lineCumulusType)
        {
            var config = CreateMapLineCumulusType();
            return config.Map<LineCumulusTypeDTO, LineCumulusType>(lineCumulusType);
        }

        internal static  IEnumerable<LineCumulusType> ToDTOs(this IEnumerable<LineCumulusTypeDTO> lineCumulusType)
        {
            return lineCumulusType.Select(ToDTO);
        }

        internal static  IMapper CreateMapModule()
        {
            var config = MapperCache.GetMapper<ModuleDTO, Module>(cfg =>
            {
                cfg.CreateMap<ModuleDTO, Module>();
            });
            return config;
        }

        internal static  Module ToDTO(this ModuleDTO module)
        {
            var config = CreateMapModule();
            return config.Map<ModuleDTO, Module>(module);
        }

        internal static  IEnumerable<Module> ToDTOs(this IEnumerable<ModuleDTO> module)
        {
            return module.Select(ToDTO);
        }

        internal static  IMapper CreateMapReinsuranceAccountingParameter()
        {
            var config = MapperCache.GetMapper<ReinsuranceAccountingParameterDTO, ReinsuranceAccountingParameter>(cfg =>
            {
                cfg.CreateMap<ReinsuranceAccountingParameterDTO, ReinsuranceAccountingParameter>();
            });
            return config;
        }

        internal static  ReinsuranceAccountingParameter ToDTO(this ReinsuranceAccountingParameterDTO reinsuranceAccountingParameter)
        {
            var config = CreateMapReinsuranceAccountingParameter();
            return config.Map<ReinsuranceAccountingParameterDTO, ReinsuranceAccountingParameter>(reinsuranceAccountingParameter);
        }

        internal static  IEnumerable<ReinsuranceAccountingParameter> ToDTOs(this IEnumerable<ReinsuranceAccountingParameterDTO> reinsuranceAccountingParameter)
        {
            return reinsuranceAccountingParameter.Select(ToDTO);
        }

        internal static  IMapper CreateMapPolicies()
        {
            var config = MapperCache.GetMapper<PolicyDTO, TempCommonServices.Models.Policy>(cfg =>
            {
                cfg.CreateMap<PolicyDTO, TempCommonServices.Models.Policy>();
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<PrefixDTO, Prefix>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<EndorsementDTO, TempCommonServices.Models.Endorsement>();
                cfg.CreateMap<RiskDTO, TempCommonServices.Models.Risk>();
                cfg.CreateMap<CoverageDTO, TempCommonServices.Models.Coverage>();
                cfg.CreateMap<AmountDTO, Amount>();
            });
            return config;
        }

        internal static  TempCommonServices.Models.Policy ToModel(this PolicyDTO policyDTO)
        {
            var config = CreateMapPolicies();
            return config.Map<PolicyDTO, TempCommonServices.Models.Policy>(policyDTO);
        }

        internal static  IEnumerable<TempCommonServices.Models.Policy> ToModels(this IEnumerable<PolicyDTO> policiesDTO)
        {
            return policiesDTO.Select(ToModel);
        }
        

        internal static  IMapper CreateMapReinsurancePaymentClaims()
        {
            var config = MapperCache.GetMapper<ReinsurancePaymentClaimDTO, ReinsurancePaymentClaim>(cfg =>
            {
                cfg.CreateMap<ReinsurancePaymentClaimDTO, ReinsurancePaymentClaim>()
                        .ForMember(dest => dest.Movement, opt => opt.MapFrom(src => (Movements)src.Movement));
                cfg.CreateMap<ReinsurancePaymentClaimLayerDTO, ReinsurancePaymentClaimLayer>();
            });
            return config;
        }

        internal static  ReinsurancePaymentClaim ToModel(this ReinsurancePaymentClaimDTO affectationTypeDTO)
        {
            var config = CreateMapReinsurancePaymentClaims();
            return config.Map<ReinsurancePaymentClaimDTO, ReinsurancePaymentClaim>(affectationTypeDTO);
        }

        internal static  IEnumerable<ReinsurancePaymentClaim> ToModels(this IEnumerable<ReinsurancePaymentClaimDTO> affectationTypeDTOs)
        {
            return affectationTypeDTOs.Select(ToModel);
        }

        internal static  IMapper CreateMapReinsurancePaymentDistributions()
        {
            var config = MapperCache.GetMapper<ReinsurancePaymentDistributionDTO, ReinsurancePaymentDistribution>(cfg =>
            {
                cfg.CreateMap<ReinsurancePaymentDistributionDTO, ReinsurancePaymentDistribution>();
            });
            return config;
        }

        internal static  ReinsurancePaymentDistribution ToDTO(this ReinsurancePaymentDistributionDTO reinsurancePaymentDistribution)
        {
            var config = CreateMapReinsurancePaymentDistributions();
            return config.Map<ReinsurancePaymentDistributionDTO, ReinsurancePaymentDistribution>(reinsurancePaymentDistribution);
        }

        internal static  IEnumerable<ReinsurancePaymentDistribution> ToDTOs(this IEnumerable<ReinsurancePaymentDistributionDTO> reinsurancePaymentDistributions)
        {
            return reinsurancePaymentDistributions.Select(ToDTO);
        }

        internal static  IMapper CreateMapReinsurancePaymentLayers()
        {
            var config = MapperCache.GetMapper<ReinsurancePaymentLayerDTO, ReinsurancePaymentLayer>(cfg =>
            {
                cfg.CreateMap<ReinsurancePaymentLayerDTO, ReinsurancePaymentLayer>();
                cfg.CreateMap<ReinsuranceClaimLayerDTO, ReinsuranceClaimLayer>();

            });
            return config;
        }

        internal static  ReinsurancePaymentLayer ToDTO(this ReinsurancePaymentLayerDTO reinsurancePaymentLayer)
        {
            var config = CreateMapReinsurancePaymentLayers();
            return config.Map<ReinsurancePaymentLayerDTO, ReinsurancePaymentLayer>(reinsurancePaymentLayer);
        }

        internal static  IEnumerable<ReinsurancePaymentLayer> ToDTOs(this IEnumerable<ReinsurancePaymentLayerDTO> reinsurancePaymentLayers)
        {
            return reinsurancePaymentLayers.Select(ToDTO);
        }

        internal static  IMapper CreateMapReinsuranceClaimDistributions()
        {
            var config = MapperCache.GetMapper<ReinsuranceClaimDistributionDTO, ReinsuranceClaimDistribution>(cfg =>
            {
                cfg.CreateMap<ReinsuranceClaimDistributionDTO, ReinsuranceClaimDistribution>();
            });
            return config;
        }

        internal static  ReinsuranceClaimDistribution ToDTO(this ReinsuranceClaimDistributionDTO reinsuranceClaimDistribution)
        {
            var config = CreateMapReinsuranceClaimDistributions();
            return config.Map<ReinsuranceClaimDistributionDTO, ReinsuranceClaimDistribution>(reinsuranceClaimDistribution);
        }

        internal static  IEnumerable<ReinsuranceClaimDistribution> ToDTOs(this IEnumerable<ReinsuranceClaimDistributionDTO> reinsuranceClaimDistributions)
        {
            return reinsuranceClaimDistributions.Select(ToDTO);
        }

        internal static  IMapper CreateMapReinsuranceDistributions()
        {
            var config = MapperCache.GetMapper<ReinsuranceDistributionDTO, ReinsuranceDistribution>(cfg =>
            {
                cfg.CreateMap<ReinsuranceDistributionDTO, ReinsuranceDistribution>();
            });
            return config;
        }

        internal static  ReinsuranceDistribution ToDTO(this ReinsuranceDistributionDTO reinsuranceDistribution)
        {
            var config = CreateMapReinsuranceDistributions();
            return config.Map<ReinsuranceDistributionDTO, ReinsuranceDistribution>(reinsuranceDistribution);
        }

        internal static  IEnumerable<ReinsuranceDistribution> ToDTOs(this IEnumerable<ReinsuranceDistributionDTO> reinsuranceDistributions)
        {
            return reinsuranceDistributions.Select(ToDTO);
        }

        internal static  IMapper CreateMapReinsuranceLayerIssuances()
        {
            var config = MapperCache.GetMapper<ReinsuranceLayerIssuanceDTO, ReinsuranceLayerIssuance>(cfg =>
            {
                cfg.CreateMap<ReinsuranceLayerIssuanceDTO, ReinsuranceLayerIssuance>();
                cfg.CreateMap<ReinsuranceLineDTO, ReinsuranceLine>();
                cfg.CreateMap<ReinsuranceAllocationDTO, ReinsuranceAllocation>();
                cfg.CreateMap<ReinsuranceCumulusRiskCoverageDTO, ReinsuranceCumulusRiskCoverage>();
                cfg.CreateMap<LineDTO, Line>();
                cfg.CreateMap<CumulusTypeDTO, CumulusType>();
                cfg.CreateMap<ContractLineDTO, ContractLine>();
                cfg.CreateMap<ContractDTO, Contract>();
                cfg.CreateMap<ResettlementTypeDTO, ResettlementType>();
                cfg.CreateMap<AffectationTypeDTO, AffectationType>();
                cfg.CreateMap<EPITypeDTO, EPIType>();
                cfg.CreateMap<ContractTypeDTO, ContractType>();
                cfg.CreateMap<ContractFunctionalityDTO, ContractFunctionality>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<LevelDTO, Level>()
                   .ForMember(dest => dest.PremiumType, opt => opt.MapFrom(src => (int)src.PremiumType))
                   .ForMember(dest => dest.ApplyOnType, opt => opt.MapFrom(src => (int)src.ApplyOnType))
                   .ForMember(dest => dest.CalculationType, opt => opt.MapFrom(src => (int)src.CalculationType));
                cfg.CreateMap<LevelPaymentDTO, LevelPayment>();
                cfg.CreateMap<LevelCompanyDTO, LevelCompany>()
                        .ForMember(dest => dest.PresentationInformationType, opt => opt.MapFrom(src => (int)src.PresentationInformationType));
                cfg.CreateMap<LevelRestoreDTO, LevelRestore>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<AgentDTO, Agent>();
            });
            return config;
        }

        internal static  ReinsuranceLayerIssuance ToModel(this ReinsuranceLayerIssuanceDTO affectationTypeDTO)
        {
            var config = CreateMapReinsuranceLayerIssuances();
            return config.Map<ReinsuranceLayerIssuanceDTO, ReinsuranceLayerIssuance>(affectationTypeDTO);
        }

        internal static  IEnumerable<ReinsuranceLayerIssuance> ToModels(this IEnumerable<ReinsuranceLayerIssuanceDTO> affectationTypeDTOs)
        {
            return affectationTypeDTOs.Select(ToModel);
        }

        internal static  IMapper CreateMapLineAssociationTypes()
        {
            var config = MapperCache.GetMapper<LineAssociationTypeDTO, LineAssociationType>(cfg =>
            {
                cfg.CreateMap<LineAssociationTypeDTO, LineAssociationType>();
            });
            return config;
        }

        internal static  LineAssociationType ToDTO(this LineAssociationTypeDTO lineAssociationType)
        {
            var config = CreateMapLineAssociationTypes();
            return config.Map<LineAssociationTypeDTO, LineAssociationType>(lineAssociationType);
        }

        internal static  IEnumerable<LineAssociationType> ToDTOs(this IEnumerable<LineAssociationTypeDTO> lineAssociationTypes)
        {
            return lineAssociationTypes.Select(ToDTO);
        }

        internal static  IMapper CreateMapLineAssociation()
        {
            var config = MapperCache.GetMapper<LineAssociationDTO, LineAssociation>(cfg =>
            {
                cfg.CreateMap<LineAssociationDTO, LineAssociation>();
                cfg.CreateMap<LineAssociationTypeDTO, LineAssociationType>();
                cfg.CreateMap<LineDTO, Line>();
                cfg.CreateMap<CumulusTypeDTO, CumulusType>();
                cfg.CreateMap<ContractLineDTO, ContractLine>();
                cfg.CreateMap<ContractDTO, Contract>();
                cfg.CreateMap<ResettlementTypeDTO, ResettlementType>();
                cfg.CreateMap<AffectationTypeDTO, AffectationType>();
                cfg.CreateMap<EPITypeDTO, EPIType>();
                cfg.CreateMap<ContractTypeDTO, ContractType>();
                cfg.CreateMap<ContractFunctionalityDTO, ContractFunctionality>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<LevelDTO, Level>()
                   .ForMember(dest => dest.PremiumType, opt => opt.MapFrom(src => (int)src.PremiumType))
                   .ForMember(dest => dest.ApplyOnType, opt => opt.MapFrom(src => (int)src.ApplyOnType))
                   .ForMember(dest => dest.CalculationType, opt => opt.MapFrom(src => (int)src.CalculationType));
                cfg.CreateMap<LevelPaymentDTO, LevelPayment>();
                cfg.CreateMap<LevelCompanyDTO, LevelCompany>()
                        .ForMember(dest => dest.PresentationInformationType, opt => opt.MapFrom(src => (int)src.PresentationInformationType));
                cfg.CreateMap<LevelRestoreDTO, LevelRestore>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<AgentDTO, Agent>();
            });
            return config;
        }

        internal static  LineAssociation ToModel(this LineAssociationDTO lineAssociation)
        {
            var config = CreateMapLineAssociation();
            return config.Map<LineAssociationDTO, LineAssociation>(lineAssociation);
        }

        internal static  IEnumerable<LineAssociation> ToModels(this IEnumerable<LineAssociationDTO> lineAssociation)
        {
            return lineAssociation.Select(ToModel);
        }

        internal static  IMapper CreateMapAssociationLine()
        {
            var config = MapperCache.GetMapper<AssociationLineDTO, AssociationLine>(cfg =>
            {
                cfg.CreateMap<AssociationLineDTO, AssociationLine>();
            });
            return config;
        }

        internal static  AssociationLine ToDTO(this AssociationLineDTO associationLine)
        {
            var config = CreateMapAssociationLine();
            return config.Map<AssociationLineDTO, AssociationLine>(associationLine);
        }

        internal static  IEnumerable<AssociationLine> ToDTOs(this IEnumerable<AssociationLineDTO> lineAssociation)
        {
            return lineAssociation.Select(ToDTO);
        }

        internal static  IMapper CreateMapPrefix()
        {
            var config = MapperCache.GetMapper<PrefixDTO, Prefix>(cfg =>
            {
                cfg.CreateMap<PrefixDTO, Prefix>();
            });
            return config;
        }

        internal static  Prefix ToModel(this PrefixDTO prefix)
        {
            var config = CreateMapPrefix();
            return config.Map<PrefixDTO, Prefix>(prefix);
        }

        internal static  IEnumerable<Prefix> ToModels(this IEnumerable<PrefixDTO> prefix)
        {
            return prefix.Select(ToModel);
        }


        internal static  IMapper CreateMapByFacultativeIssues()
        {
            var config = MapperCache.GetMapper<ByFacultativeIssueDTO, ByFacultativeIssue>(cfg =>
            {
                cfg.CreateMap<ByFacultativeIssueDTO, ByFacultativeIssue>();
                cfg.CreateMap<PrefixDTO, Prefix>();
                cfg.CreateMap<LineAssociationTypeDTO, LineAssociationType>();
            });
            return config;
        }

        internal static  ByFacultativeIssue ToModel(this ByFacultativeIssueDTO byFacultativeIssueDTO)
        {
            var config = CreateMapByFacultativeIssues();
            return config.Map<ByFacultativeIssueDTO, ByFacultativeIssue>(byFacultativeIssueDTO);
        }

        internal static  IEnumerable<ByFacultativeIssue> ToModels(this IEnumerable<ByFacultativeIssueDTO> byFacultativeIssueDTOs)
        {
            return byFacultativeIssueDTOs.Select(ToModel);
        }

        internal static  IMapper CreateMapByInsureds()
        {
            var config = MapperCache.GetMapper<ByInsuredDTO, ByInsured>(cfg =>
            {
                cfg.CreateMap<ByInsuredDTO, ByInsured>();
                cfg.CreateMap<IndividualDTO, Individual>();
                cfg.CreateMap<LineAssociationTypeDTO, LineAssociationType>();
            });
            return config;
        }

        internal static  ByInsured ToModel(this ByInsuredDTO byInsuredDTO)
        {
            var config = CreateMapByInsureds();
            return config.Map<ByInsuredDTO, ByInsured>(byInsuredDTO);
        }

        internal static  IEnumerable<ByInsured> ToModels(this IEnumerable<ByInsuredDTO> byInsuredDTOs)
        {
            return byInsuredDTOs.Select(ToModel);
        }

        internal static  IMapper CreateMapByInsuredPrefixs()
        {
            var config = MapperCache.GetMapper<ByInsuredPrefixDTO, ByInsuredPrefix>(cfg =>
            {
                cfg.CreateMap<ByInsuredPrefixDTO, ByInsuredPrefix>();
                cfg.CreateMap<IndividualDTO, Individual>();
                cfg.CreateMap<PrefixDTO, Prefix>();
                cfg.CreateMap<LineAssociationTypeDTO, LineAssociationType>();
            });
            return config;
        }

        internal static  ByInsuredPrefix ToModel(this ByInsuredPrefixDTO byInsuredPrefixDTO)
        {
            var config = CreateMapByInsuredPrefixs();
            return config.Map<ByInsuredPrefixDTO, ByInsuredPrefix>(byInsuredPrefixDTO);
        }

        internal static  IEnumerable<ByInsuredPrefix> ToModels(this IEnumerable<ByInsuredPrefixDTO> byInsuredPrefixDTOs)
        {
            return byInsuredPrefixDTOs.Select(ToModel);
        }

        internal static  IMapper CreateMapByLineBusinesss()
        {
            var config = MapperCache.GetMapper<ByLineBusinessDTO, ByLineBusiness>(cfg =>
            {
                cfg.CreateMap<ByLineBusinessDTO, ByLineBusiness>();
                cfg.CreateMap<LineBusinessDTO, LineBusiness>();
                cfg.CreateMap<LineAssociationTypeDTO, LineAssociationType>();
            });
            return config;
        }

        internal static  ByLineBusiness ToModel(this ByLineBusinessDTO byLineBusinessDTO)
        {
            var config = CreateMapByLineBusinesss();
            return config.Map<ByLineBusinessDTO, ByLineBusiness>(byLineBusinessDTO);
        }

        internal static  IEnumerable<ByLineBusiness> ToModels(this IEnumerable<ByLineBusinessDTO> byLineBusinessDTOs)
        {
            return byLineBusinessDTOs.Select(ToModel);
        }

        internal static  IMapper CreateMapByLineBusinessInsuredObjects()
        {
            var config = MapperCache.GetMapper<ByLineBusinessInsuredObjectDTO, ByLineBusinessInsuredObject>(cfg =>
            {
                cfg.CreateMap<ByLineBusinessInsuredObjectDTO, ByLineBusinessInsuredObject>();
                cfg.CreateMap<LineBusinessDTO, LineBusiness>();
                cfg.CreateMap<InsuredObjectDTO, InsuredObject>();
            });
            return config;
        }

        internal static  ByLineBusinessInsuredObject ToModel(this ByLineBusinessInsuredObjectDTO byLineBusinessInsuredObjectDTO)
        {
            var config = CreateMapByLineBusinessInsuredObjects();
            return config.Map<ByLineBusinessInsuredObjectDTO, ByLineBusinessInsuredObject>(byLineBusinessInsuredObjectDTO);
        }

        internal static  IEnumerable<ByLineBusinessInsuredObject> ToModels(this IEnumerable<ByLineBusinessInsuredObjectDTO> byLineBusinessInsuredObjectDTOs)
        {
            return byLineBusinessInsuredObjectDTOs.Select(ToModel);
        }

        internal static  IMapper CreateMapByLineBusinessSubLineBusinesss()
        {
            var config = MapperCache.GetMapper<ByLineBusinessSubLineBusinessDTO, ByLineBusinessSubLineBusiness>(cfg =>
            {
                cfg.CreateMap<ByLineBusinessSubLineBusinessDTO, ByLineBusinessSubLineBusiness>();
                cfg.CreateMap<LineBusinessDTO, LineBusiness>();
                cfg.CreateMap<SubLineBusinessDTO, SubLineBusiness>();
                cfg.CreateMap<LineAssociationTypeDTO, LineAssociationType>();
            });
            return config;
        }

        internal static  ByLineBusinessSubLineBusiness ToModel(this ByLineBusinessSubLineBusinessDTO byLineBusinessSubLineBusinessDTO)
        {
            var config = CreateMapByLineBusinessSubLineBusinesss();
            return config.Map<ByLineBusinessSubLineBusinessDTO, ByLineBusinessSubLineBusiness>(byLineBusinessSubLineBusinessDTO);
        }

        internal static  IEnumerable<ByLineBusinessSubLineBusiness> ToModels(this IEnumerable<ByLineBusinessSubLineBusinessDTO> byLineBusinessSubLineBusinessDTOs)
        {
            return byLineBusinessSubLineBusinessDTOs.Select(ToModel);
        }

        internal static  IMapper CreateMapByLineBusinessSubLineBusinessCoverages()
        {
            var config = MapperCache.GetMapper<ByLineBusinessSubLineBusinessCoverageDTO, ByLineBusinessSubLineBusinessCoverage>(cfg =>
            {
                cfg.CreateMap<ByLineBusinessSubLineBusinessCoverageDTO, ByLineBusinessSubLineBusinessCoverage>();
                cfg.CreateMap<LineBusinessDTO, LineBusiness>();
                cfg.CreateMap<SubLineBusinessDTO, SubLineBusiness>();
                cfg.CreateMap<CoverageDTO, TempCommonServices.Models.Coverage>();
            });
            return config;
        }

        internal static  ByLineBusinessSubLineBusinessCoverage ToModel(this ByLineBusinessSubLineBusinessCoverageDTO byLineBusinessSubLineBusinessCoverageDTO)
        {
            var config = CreateMapByLineBusinessSubLineBusinessCoverages();
            return config.Map<ByLineBusinessSubLineBusinessCoverageDTO, ByLineBusinessSubLineBusinessCoverage>(byLineBusinessSubLineBusinessCoverageDTO);
        }

        internal static  IEnumerable<ByLineBusinessSubLineBusinessCoverage> ToModels(this IEnumerable<ByLineBusinessSubLineBusinessCoverageDTO> byLineBusinessSubLineBusinessCoverageDTOs)
        {
            return byLineBusinessSubLineBusinessCoverageDTOs.Select(ToModel);
        }

        internal static  IMapper CreateMapByLineBusinessSubLineBusinessInsuredObjects()
        {
            var config = MapperCache.GetMapper<ByLineBusinessSubLineBusinessInsuredObjectDTO, ByLineBusinessSubLineBusinessInsuredObject>(cfg =>
            {
                cfg.CreateMap<ByLineBusinessSubLineBusinessInsuredObjectDTO, ByLineBusinessSubLineBusinessInsuredObject>();
                cfg.CreateMap<LineBusinessDTO, LineBusiness>();
                cfg.CreateMap<SubLineBusinessDTO, SubLineBusiness>();
                cfg.CreateMap<InsuredObjectDTO, InsuredObject>();
                cfg.CreateMap<LineAssociationTypeDTO, LineAssociationType>();
            });
            return config;
        }

        internal static  ByLineBusinessSubLineBusinessInsuredObject ToModel(this ByLineBusinessSubLineBusinessInsuredObjectDTO byLineBusinessSubLineBusinessInsuredObjectDTO)
        {
            var config = CreateMapByLineBusinessSubLineBusinessInsuredObjects();
            return config.Map<ByLineBusinessSubLineBusinessInsuredObjectDTO, ByLineBusinessSubLineBusinessInsuredObject>(byLineBusinessSubLineBusinessInsuredObjectDTO);
        }

        internal static  IEnumerable<ByLineBusinessSubLineBusinessInsuredObject> ToModels(this IEnumerable<ByLineBusinessSubLineBusinessInsuredObjectDTO> byLineBusinessSubLineBusinessInsuredObjectDTOs)
        {
            return byLineBusinessSubLineBusinessInsuredObjectDTOs.Select(ToModel);
        }

        internal static  IMapper CreateMapByOperationTypePrefixs()
        {
            var config = MapperCache.GetMapper<ByOperationTypePrefixDTO, ByOperationTypePrefix>(cfg =>
            {
                cfg.CreateMap<ByOperationTypePrefixDTO, ByOperationTypePrefix>();
                cfg.CreateMap<PrefixDTO, Prefix>();
                cfg.CreateMap<LineAssociationTypeDTO, LineAssociationType>();
            });
            return config;
        }

        internal static  ByOperationTypePrefix ToModel(this ByOperationTypePrefixDTO byOperationTypePrefixDTO)
        {
            var config = CreateMapByOperationTypePrefixs();
            return config.Map<ByOperationTypePrefixDTO, ByOperationTypePrefix>(byOperationTypePrefixDTO);
        }

        internal static  IEnumerable<ByOperationTypePrefix> ToModels(this IEnumerable<ByOperationTypePrefixDTO> byOperationTypePrefixDTOs)
        {
            return byOperationTypePrefixDTOs.Select(ToModel);
        }

        internal static  IMapper CreateMapByPolicies()
        {
            var config = MapperCache.GetMapper<ByPolicyDTO, ByPolicy>(cfg =>
            {
                cfg.CreateMap<ByPolicyDTO, ByPolicy>();
                cfg.CreateMap<PolicyDTO, TempCommonServices.Models.Policy>();
            });
            return config;
        }

        internal static  ByPolicy ToModel(this ByPolicyDTO byPolicyDTO)
        {
            var config = CreateMapByPolicies();
            return config.Map<ByPolicyDTO, ByPolicy>(byPolicyDTO);
        }

        internal static  IEnumerable<ByPolicy> ToModels(this IEnumerable<ByPolicyDTO> byPolicyDTOs)
        {
            return byPolicyDTOs.Select(ToModel);
        }

        internal static  IMapper CreateMapByPolicyLineBusinessSubLineBusinesss()
        {
            var config = MapperCache.GetMapper<ByPolicyLineBusinessSubLineBusinessDTO, ByPolicyLineBusinessSubLineBusiness>(cfg =>
            {
                cfg.CreateMap<ByPolicyLineBusinessSubLineBusinessDTO, ByPolicyLineBusinessSubLineBusinessDTO>();
                cfg.CreateMap<PolicyDTO, TempCommonServices.Models.Policy>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<BranchDTO, Branch>();
                cfg.CreateMap<PrefixDTO, Prefix>();
                cfg.CreateMap<EndorsementDTO, Endorsement>();
                cfg.CreateMap<LineBusinessDTO, LineBusiness>();
                cfg.CreateMap<SubLineBusinessDTO, SubLineBusiness>();
                cfg.CreateMap<LineAssociationTypeDTO, LineAssociationType>();
            });
            return config;
        }

        internal static  ByPolicyLineBusinessSubLineBusiness ToModel(this ByPolicyLineBusinessSubLineBusinessDTO byPolicyLineBusinessSubLineBusinessDTO)
        {
            var config = CreateMapByPolicyLineBusinessSubLineBusinesss();
            return config.Map<ByPolicyLineBusinessSubLineBusinessDTO, ByPolicyLineBusinessSubLineBusiness>(byPolicyLineBusinessSubLineBusinessDTO);
        }

        internal static  IEnumerable<ByPolicyLineBusinessSubLineBusiness> ToModels(this IEnumerable<ByPolicyLineBusinessSubLineBusinessDTO> byPolicyLineBusinessSubLineBusinessDTOs)
        {
            return byPolicyLineBusinessSubLineBusinessDTOs.Select(ToModel);
        }

        internal static  IMapper CreateMapByPrefixProducts()
        {
            var config = MapperCache.GetMapper<ByPrefixProductDTO, ByPrefixProduct>(cfg =>
            {
                cfg.CreateMap<ByPrefixProductDTO, ByPrefixProduct>();
                cfg.CreateMap<PrefixDTO, Prefix>();
            });
            return config;
        }

        internal static  ByPrefixProduct ToModel(this ByPrefixProductDTO byPrefixProductDTO)
        {
            var config = CreateMapByPrefixProducts();
            return config.Map<ByPrefixProductDTO, ByPrefixProduct>(byPrefixProductDTO);
        }

        internal static  IEnumerable<ByPrefixProduct> ToModels(this IEnumerable<ByPrefixProductDTO> byPrefixProductDTOs)
        {
            return byPrefixProductDTOs.Select(ToModel);
        }

        internal static  IMapper CreateMapLineBusiness()
        {
            var config = MapperCache.GetMapper<LineBusinessDTO, LineBusiness>(cfg =>
            {
                cfg.CreateMap<LineBusinessDTO, LineBusiness>();
            });
            return config;
        }

        internal static  LineBusiness ToModel(this LineBusinessDTO lineBusinessDTO)
        {
            var config = CreateMapLineBusiness();
            return config.Map<LineBusinessDTO, LineBusiness>(lineBusinessDTO);
        }

        internal static  IEnumerable<LineBusiness> ToModels(this IEnumerable<LineBusinessDTO> lineBusinessDTO)
        {
            return lineBusinessDTO.Select(ToModel);
        }

        internal static  IMapper CreateMapTempLayerDistributions()
        {
            var config = MapperCache.GetMapper<TempLayerDistributionsDTO, TempLayerDistributions>(cfg =>
            {
                cfg.CreateMap<TempLayerDistributionsDTO, TempLayerDistributions>();
            });
            return config;
        }

        internal static  TempLayerDistributions ToModel(this TempLayerDistributionsDTO tempLayerDistributionsDTO)
        {
            var config = CreateMapTempLayerDistributions();
            return config.Map<TempLayerDistributionsDTO, TempLayerDistributions>(tempLayerDistributionsDTO);
        }

        internal static  IEnumerable<TempLayerDistributions> ToModels(this IEnumerable<TempLayerDistributionsDTO> tempLayerDistributionsDTO)
        {
            return tempLayerDistributionsDTO.Select(ToModel);
        }


        internal static  IMapper CreateMapTempLineCumulusIssuances()
        {
            var config = MapperCache.GetMapper<TempLineCumulusIssuanceDTO, TempLineCumulusIssuance>(cfg =>
            {
                cfg.CreateMap<TempLineCumulusIssuanceDTO, TempLineCumulusIssuance>();
            });
            return config;
        }

        internal static  TempLineCumulusIssuance ToModel(this TempLineCumulusIssuanceDTO tempLineCumulusIssuanceDTO)
        {
            var config = CreateMapTempLineCumulusIssuances();
            return config.Map<TempLineCumulusIssuanceDTO, TempLineCumulusIssuance>(tempLineCumulusIssuanceDTO);
        }

        internal static  IEnumerable<TempLineCumulusIssuance> ToModels(this IEnumerable<TempLineCumulusIssuanceDTO> tempLineCumulusIssuanceDTO)
        {
            return tempLineCumulusIssuanceDTO.Select(ToModel);
        }

        internal static  IMapper CreateMapSelect()
        {
            var config = MapperCache.GetMapper<SelectDTO, Select>(cfg =>
            {
                cfg.CreateMap<SelectDTO, Select>();
            });
            return config;
        }

        internal static  Select ToModel(this SelectDTO selectDTO)
        {
            var config = CreateMapSelect();
            return config.Map<SelectDTO, Select>(selectDTO);
        }

        internal static  IEnumerable<Select> ToModels(this IEnumerable<SelectDTO> selectDTO)
        {
            return selectDTO.Select(ToModel);
        }

        internal static  IMapper CreateMapReinsuranceFacultative()
        {
            var config = MapperCache.GetMapper<ReinsuranceFacultativeDTO, ReinsuranceFacultative>(cfg =>
            {
                cfg.CreateMap<ReinsuranceFacultativeDTO, ReinsuranceFacultative>();
            });
            return config;
        }

        internal static  ReinsuranceFacultative ToModel(this ReinsuranceFacultativeDTO reinsuranceFacultativeDTO)
        {
            var config = CreateMapReinsuranceFacultative();
            return config.Map<ReinsuranceFacultativeDTO, ReinsuranceFacultative>(reinsuranceFacultativeDTO);
        }

        internal static  IEnumerable<ReinsuranceFacultative> ToModels(this IEnumerable<ReinsuranceFacultativeDTO> reinsuranceFacultativeDTO)
        {
            return reinsuranceFacultativeDTO.Select(ToModel);
        }

        internal static  IMapper CreateMapTempFacultativeCompanies()
        {
            var config = MapperCache.GetMapper<TempFacultativeCompaniesDTO, TempFacultativeCompanies>(cfg =>
            {
                cfg.CreateMap<TempFacultativeCompaniesDTO, TempFacultativeCompanies>();
            });
            return config;
        }

        internal static  TempFacultativeCompanies ToModel(this TempFacultativeCompaniesDTO tempFacultativeCompanies)
        {
            var config = CreateMapTempFacultativeCompanies();
            return config.Map<TempFacultativeCompaniesDTO, TempFacultativeCompanies>(tempFacultativeCompanies);
        }

        internal static  IEnumerable<TempFacultativeCompanies> ToModels(this IEnumerable<TempFacultativeCompaniesDTO> tempFacultativeCompaniesDTO)
        {
            return tempFacultativeCompaniesDTO.Select(ToModel);
        }
        
        internal static  IMapper CreateMapClaimDistribution()
        {
            var config = MapperCache.GetMapper<ClaimDistributionDTO, ClaimDistribution>(cfg =>
            {
                cfg.CreateMap<ClaimDistributionDTO, ClaimDistribution>();
            });
            return config;
        }

        internal static  ClaimDistribution ToModel(this ClaimDistributionDTO claimsByMovementSourceDTO)
        {
            var config = CreateMapClaimDistribution();
            return config.Map<ClaimDistributionDTO, ClaimDistribution>(claimsByMovementSourceDTO);
        }

        internal static  IEnumerable<ClaimDistribution> ToModels(this IEnumerable<ClaimDistributionDTO> claimsByMovementSourceDTO)
        {
            return claimsByMovementSourceDTO.Select(ToModel);
        }

        internal static  IMapper CreateMapClaimAllocation()
        {
            var config = MapperCache.GetMapper<ClaimAllocationDTO, ClaimAllocation>(cfg =>
            {
                cfg.CreateMap<ClaimAllocationDTO, ClaimAllocation>();
            });
            return config;
        }

        internal static  ClaimAllocation ToModel(this ClaimAllocationDTO claimAllocationsDTO)
        {
            var config = CreateMapClaimAllocation();
            return config.Map<ClaimAllocationDTO, ClaimAllocation>(claimAllocationsDTO);
        }

        internal static  IEnumerable<ClaimAllocation> ToModels(this IEnumerable<ClaimAllocationDTO> claimAllocationsDTO)
        {
            return claimAllocationsDTO.Select(ToModel);
        }

        internal static  IMapper CreateMapReinsuranceRealClaimByClaimCode()
        {
            var config = MapperCache.GetMapper<ReinsuranceRealClaimByClaimCodeDTO, ReinsuranceRealClaimByClaimCode>(cfg =>
            {
                cfg.CreateMap<ReinsuranceRealClaimByClaimCodeDTO, ReinsuranceRealClaimByClaimCode>();
            });
            return config;
        }

        internal static  ReinsuranceRealClaimByClaimCode ToModel(this ReinsuranceRealClaimByClaimCodeDTO reinsuranceRealClaimByClaimCodeDTO)
        {
            var config = CreateMapReinsuranceRealClaimByClaimCode();
            return config.Map<ReinsuranceRealClaimByClaimCodeDTO, ReinsuranceRealClaimByClaimCode>(reinsuranceRealClaimByClaimCodeDTO);
        }

        internal static  IEnumerable<ReinsuranceRealClaimByClaimCode> ToModels(this IEnumerable<ReinsuranceRealClaimByClaimCodeDTO> reinsuranceRealClaimByClaimCodeDTO)
        {
            return reinsuranceRealClaimByClaimCodeDTO.Select(ToModel);
        }
        
        internal static  IMapper CreateMapPaymentDistribution()
        {
            var config = MapperCache.GetMapper<PaymentDistributionDTO, PaymentDistribution>(cfg =>
            {
                cfg.CreateMap<PaymentDistributionDTO, PaymentDistribution>();
            });
            return config;
        }

        internal static  PaymentDistribution ToModel(this PaymentDistributionDTO paymentsByMovementSourceDTO)
        {
            var config = CreateMapPaymentDistribution();
            return config.Map<PaymentDistributionDTO, PaymentDistribution>(paymentsByMovementSourceDTO);
        }

        internal static  IEnumerable<PaymentDistribution> ToModels(this IEnumerable<PaymentDistributionDTO> paymentsByMovementSourceDTO)
        {
            return paymentsByMovementSourceDTO.Select(ToModel);
        }

        internal static  IMapper CreateMapPaymentAllocation()
        {
            var config = MapperCache.GetMapper<PaymentAllocationDTO, PaymentAllocation>(cfg =>
            {
                cfg.CreateMap<PaymentAllocationDTO, PaymentAllocation>();
            });
            return config;
        }

        internal static  PaymentAllocation ToModel(this PaymentAllocationDTO paymentAllocationsDTO)
        {
            var config = CreateMapPaymentAllocation();
            return config.Map<PaymentAllocationDTO, PaymentAllocation>(paymentAllocationsDTO);
        }

        internal static  IEnumerable<PaymentAllocation> ToModels(this IEnumerable<PaymentAllocationDTO> paymentAllocationsDTO)
        {
            return paymentAllocationsDTO.Select(ToModel);
        }

        internal static  IMapper CreateMapReinsurancePaymentLayer()
        {
            var config = MapperCache.GetMapper<ReinsurancePaymentLayerDTO, ReinsurancePaymentLayer>(cfg =>
            {
                cfg.CreateMap<ReinsurancePaymentLayerDTO, ReinsurancePaymentLayer>();
            });
            return config;
        }

        internal static  ReinsurancePaymentLayer ToModel(this ReinsurancePaymentLayerDTO reinsurancePaymentLayerDTO)
        {
            var config = CreateMapReinsurancePaymentLayer();
            return config.Map<ReinsurancePaymentLayerDTO, ReinsurancePaymentLayer>(reinsurancePaymentLayerDTO);
        }

        internal static  IEnumerable<ReinsurancePaymentLayer> ToModels(this IEnumerable<ReinsurancePaymentLayerDTO> reinsurancePaymentLayerDTO)
        {
            return reinsurancePaymentLayerDTO.Select(ToModel);
        }
        internal static  IMapper CreateMapReinsurancePaymentDistribution()
        {
            var config = MapperCache.GetMapper<ReinsurancePaymentDistributionDTO, ReinsurancePaymentDistribution>(cfg =>
            {
                cfg.CreateMap<ReinsurancePaymentDistributionDTO, ReinsurancePaymentDistribution>();
            });
            return config;
        }

        internal static  ReinsurancePaymentDistribution ToModel(this ReinsurancePaymentDistributionDTO reinsurancePaymentDistributionDTO)
        {
            var config = CreateMapReinsurancePaymentDistribution();
            return config.Map<ReinsurancePaymentDistributionDTO, ReinsurancePaymentDistribution>(reinsurancePaymentDistributionDTO);
        }

        internal static  IEnumerable<ReinsurancePaymentDistribution> ToModels(this IEnumerable<ReinsurancePaymentDistributionDTO> reinsurancePaymentDistributionDTO)
        {
            return reinsurancePaymentDistributionDTO.Select(ToModel);
        }

        internal static  IMapper CreateMapEndorsementReinsurance()
        {
            var config = MapperCache.GetMapper<EndorsementReinsuranceDTO, EndorsementReinsurance>(cfg =>
            {
                cfg.CreateMap<EndorsementReinsuranceDTO, EndorsementReinsurance>();
            });
            return config;
        }

        internal static  EndorsementReinsurance ToModel(this EndorsementReinsuranceDTO endorsementReinsuranceDTO)
        {
            var config = CreateMapReinsurancePaymentDistribution();
            return config.Map<EndorsementReinsuranceDTO, EndorsementReinsurance>(endorsementReinsuranceDTO);
        }

        internal static  IEnumerable<EndorsementReinsurance> ToModels(this IEnumerable<EndorsementReinsuranceDTO> endorsementReinsuranceDTO)
        {
            return endorsementReinsuranceDTO.Select(ToModel);
        }


        internal static  IMapper CreateMapReinsuranceDistribution()
        {
            var config = MapperCache.GetMapper<ReinsuranceDistributionDTO, ReinsuranceDistribution>(cfg =>
            {
                cfg.CreateMap<ReinsuranceDistributionDTO, ReinsuranceDistribution>();
            });
            return config;
        }

        internal static  ReinsuranceDistribution ToModel(this ReinsuranceDistributionDTO reinsuranceDistributionDTO)
        {
            var config = CreateMapReinsuranceDistribution();
            return config.Map<ReinsuranceDistributionDTO, ReinsuranceDistribution>(reinsuranceDistributionDTO);
        }

        internal static  IEnumerable<ReinsuranceDistribution> ToModels(this IEnumerable<ReinsuranceDistributionDTO> reinsuranceDistributionDTO)
        {
            return reinsuranceDistributionDTO.Select(ToModel);
        }

        internal static  IMapper CreateMapReinsuranceClaimLayer()
        {
            var config = MapperCache.GetMapper<ReinsuranceClaimLayerDTO, ReinsuranceClaimLayer>(cfg =>
            {
                cfg.CreateMap<ReinsuranceClaimLayerDTO, ReinsuranceClaimLayer>();
            });
            return config;
        }

        internal static  ReinsuranceClaimLayer ToModel(this ReinsuranceClaimLayerDTO reinsuranceClaimLayerDTO)
        {
            var config = CreateMapReinsuranceClaimLayer();
            return config.Map<ReinsuranceClaimLayerDTO, ReinsuranceClaimLayer>(reinsuranceClaimLayerDTO);
        }

        internal static  IEnumerable<ReinsuranceClaimLayer> ToModels(this IEnumerable<ReinsuranceClaimLayerDTO> reinsuranceClaimLayerDTO)
        {
            return reinsuranceClaimLayerDTO.Select(ToModel);
        }

        internal static  IMapper CreateMapReinsuranceClaimDistribution()
        {
            var config = MapperCache.GetMapper<ReinsuranceClaimDistributionDTO, ReinsuranceClaimDistribution>(cfg =>
            {
                cfg.CreateMap<ReinsuranceClaimLayerDTO, ReinsuranceClaimLayer>();
            });
            return config;
        }

        internal static  ReinsuranceClaimDistribution ToModel(this ReinsuranceClaimDistributionDTO reinsuranceClaimDistributionDTO)
        {
            var config = CreateMapReinsuranceClaimDistribution();
            return config.Map<ReinsuranceClaimDistributionDTO, ReinsuranceClaimDistribution>(reinsuranceClaimDistributionDTO);
        }

        internal static  IEnumerable<ReinsuranceClaimDistribution> ToModels(this IEnumerable<ReinsuranceClaimDistributionDTO> reinsuranceClaimDistributionDTO)
        {
            return reinsuranceClaimDistributionDTO.Select(ToModel);
        }

        internal static  IMapper CreateMapReinsuranceMassiveHeader()
        {
            var config = MapperCache.GetMapper<ReinsuranceMassiveHeaderDTO, ReinsuranceMassiveHeader>(cfg =>
            {
                cfg.CreateMap<ReinsuranceMassiveHeaderDTO, ReinsuranceMassiveHeader>();
            });
            return config;
        }

        internal static  ReinsuranceMassiveHeader ToModel(this ReinsuranceMassiveHeaderDTO reinsuranceMassiveHeaderDTO)
        {
            var config = CreateMapReinsuranceMassiveHeader();
            return config.Map<ReinsuranceMassiveHeaderDTO, ReinsuranceMassiveHeader>(reinsuranceMassiveHeaderDTO);
        }

        internal static  IEnumerable<ReinsuranceMassiveHeader> ToModels(this IEnumerable<ReinsuranceMassiveHeaderDTO> reinsuranceMassiveHeaderDTO)
        {
            return reinsuranceMassiveHeaderDTO.Select(ToModel);
        }

        internal static  IMapper CreateMapTempReinsuranceProcess()
        {
            var config = MapperCache.GetMapper<TempReinsuranceProcessDTO, TempReinsuranceProcess>(cfg =>
            {
                cfg.CreateMap<TempReinsuranceProcessDTO, TempReinsuranceProcess>();
            });
            return config;
        }

        internal static  TempReinsuranceProcess ToModel(this TempReinsuranceProcessDTO tempReinsuranceProcessDTO)
        {
            var config = CreateMapTempReinsuranceProcess();
            return config.Map<TempReinsuranceProcessDTO, TempReinsuranceProcess>(tempReinsuranceProcessDTO);
        }

        internal static  IEnumerable<TempReinsuranceProcess> ToModels(this IEnumerable<TempReinsuranceProcessDTO> tempReinsuranceProcessDTO)
        {
            return tempReinsuranceProcessDTO.Select(ToModel);
        }

        internal static  IMapper CreateMapTempAllocation()
        {
            var config = MapperCache.GetMapper<TempAllocationDTO, TempAllocation>(cfg =>
            {
                cfg.CreateMap<TempAllocationDTO, TempAllocation>();
            });
            return config;
        }

        internal static  TempAllocation ToModel(this TempAllocationDTO tempAllocationDTO)
        {
            var config = CreateMapTempAllocation();
            return config.Map<TempAllocationDTO, TempAllocation>(tempAllocationDTO);
        }

        internal static  IEnumerable<TempAllocation> ToModels(this IEnumerable<TempAllocationDTO> tempAllocationDTO)
        {
            return tempAllocationDTO.Select(ToModel);
        }

        internal static  IMapper CreateMapLineCumulusKey()
        {
            var config = MapperCache.GetMapper<LineCumulusKeyDTO, LineCumulusKey>(cfg =>
            {
                cfg.CreateMap<LineCumulusKeyDTO, LineCumulusKey>();
                cfg.CreateMap<CumulusTypeDTO, CumulusType>();
                cfg.CreateMap<ContractLineDTO, ContractLine>();
                cfg.CreateMap<ContractDTO, Contract>();
                cfg.CreateMap<ResettlementTypeDTO, ResettlementType>();
                cfg.CreateMap<AffectationTypeDTO, AffectationType>();
                cfg.CreateMap<EPITypeDTO, EPIType>();
                cfg.CreateMap<ContractTypeDTO, ContractType>();
                cfg.CreateMap<ContractFunctionalityDTO, ContractFunctionality>();
                cfg.CreateMap<CurrencyDTO, Currency>();
                cfg.CreateMap<LevelDTO, Level>()
                   .ForMember(dest => dest.PremiumType, opt => opt.MapFrom(src => (int)src.PremiumType))
                   .ForMember(dest => dest.ApplyOnType, opt => opt.MapFrom(src => (int)src.ApplyOnType))
                   .ForMember(dest => dest.CalculationType, opt => opt.MapFrom(src => (int)src.CalculationType));
                cfg.CreateMap<LevelPaymentDTO, LevelPayment>();
                cfg.CreateMap<LevelCompanyDTO, LevelCompany>()
                        .ForMember(dest => dest.PresentationInformationType, opt => opt.MapFrom(src => (int)src.PresentationInformationType));
                cfg.CreateMap<LevelRestoreDTO, LevelRestore>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<AgentDTO, Agent>();
                cfg.CreateMap<LineCumulusKeyRiskCoverageDTO, LineCumulusKeyRiskCoverage>();
            });
            return config;
        }

        internal static  LineCumulusKey ToModel(this LineCumulusKeyDTO lineCumulusKeyDTO)
        {
            var config = CreateMapLineCumulusKey();
            return config.Map<LineCumulusKeyDTO, LineCumulusKey>(lineCumulusKeyDTO);
        }

        internal static  IEnumerable<LineCumulusKey> ToModels(this IEnumerable<LineCumulusKeyDTO> lineCumulusKeyDTO)
        {
            return lineCumulusKeyDTO.Select(ToModel);
        }

        internal static  IMapper CreateMapModuleDate()
        {
            var config = MapperCache.GetMapper<ModuleDateDTO, ModuleDate>(cfg =>
            {
                cfg.CreateMap<ModuleDateDTO, ModuleDate>();
            });
            return config;
        }

        internal static  ModuleDate ToModel(this ModuleDateDTO moduleDateDTO)
        {
            var config = CreateMapModuleDate();
            return config.Map<ModuleDateDTO, ModuleDate> (moduleDateDTO);
        }

        internal static  IEnumerable<ModuleDate> ToModels(this IEnumerable<ModuleDateDTO> moduleDateDTOs)
        {
            return moduleDateDTOs.Select(ToModel);
        }

        internal static  IMapper CreateMapEndorsement()
        {
            var config = MapperCache.GetMapper<EndorsementDTO, Endorsement>(cfg =>
            {
                cfg.CreateMap<EndorsementDTO, Endorsement>();
                cfg.CreateMap<RiskDTO, Application.TempCommonServices.Models.Risk>();
                cfg.CreateMap<CoverageDTO, Coverage>();
                cfg.CreateMap<AmountDTO, Amount>();
                cfg.CreateMap<CurrencyDTO, Currency>();
            });
            return config;
        }

        internal static  Endorsement ToModel(this EndorsementDTO endorsementDTO)
        {
            var config = CreateMapEndorsement();
            return config.Map<EndorsementDTO, Endorsement>(endorsementDTO);
        }

        internal static  IEnumerable<Endorsement> ToModels(this IEnumerable<EndorsementDTO> endorsementDTO)
        {
            return endorsementDTO.Select(ToModel);
        }

        internal static  IMapper CreateMapLineCumulusKeyRiskCoverage()
        {
            var config = MapperCache.GetMapper<LineCumulusKeyRiskCoverageDTO, LineCumulusKeyRiskCoverage>(cfg =>
            {
                cfg.CreateMap<LineCumulusKeyRiskCoverageDTO, LineCumulusKeyRiskCoverage>();
            });
            return config;
        }

        internal static  LineCumulusKeyRiskCoverage ToModel(this LineCumulusKeyRiskCoverageDTO lineCumulusKeyRiskCoverageDTO)
        {
            var config = CreateMapLineCumulusKeyRiskCoverage();
            return config.Map<LineCumulusKeyRiskCoverageDTO, LineCumulusKeyRiskCoverage>(lineCumulusKeyRiskCoverageDTO);
        }

        internal static  IEnumerable<LineCumulusKeyRiskCoverage> ToModels(this IEnumerable<LineCumulusKeyRiskCoverageDTO> lineCumulusKeyRiskCoverageDTO)
        {
            return lineCumulusKeyRiskCoverageDTO.Select(ToModel);
        }

        internal static  IMapper CreateMapPriorityRetentionDetail()
        {
            var config = MapperCache.GetMapper<PriorityRetentionDetailDTO, PriorityRetentionDetail>(cfg =>
            {
                cfg.CreateMap<PriorityRetentionDetailDTO, PriorityRetentionDetail>();
            });
            return config;
        }

        internal static  PriorityRetentionDetail ToModel(this PriorityRetentionDetailDTO priorityRetentionDetailDTO)
        {
            var config = CreateMapPriorityRetentionDetail();
            return config.Map<PriorityRetentionDetailDTO, PriorityRetentionDetail>(priorityRetentionDetailDTO);
        }

        internal static  IEnumerable<PriorityRetentionDetail> ToModels(this IEnumerable<PriorityRetentionDetailDTO> priorityRetentionDetailDTOs)
        {
            return priorityRetentionDetailDTOs.Select(ToModel);
        }

        internal static IMapper CreateMapIssueAllocationRiskCover()
        {
            var config = MapperCache.GetMapper<IssueAllocationRiskCover, IssueAllocationRiskCoverDTO>(cfg =>
            {
                cfg.CreateMap<IssueAllocationRiskCover, IssueAllocationRiskCoverDTO>();
            });
            return config;
        }

        internal static IssueAllocationRiskCover ToModel(this IssueAllocationRiskCoverDTO issueAllocationRiskCoverDTO)
        {
            var config = CreateMapIssueAllocationRiskCover();
            return config.Map<IssueAllocationRiskCoverDTO, IssueAllocationRiskCover>(issueAllocationRiskCoverDTO);
        }

        internal static IEnumerable<IssueAllocationRiskCover> ToModels(this IEnumerable<IssueAllocationRiskCoverDTO> issueAllocationRiskCoverDTOs)
        {
            return issueAllocationRiskCoverDTOs.Select(ToModel);
        }

        internal static IMapper CreateMapIssueAllocationRiskCoverIntegration()
        {
            var config = MapperCache.GetMapper<IssueAllocationRiskCover, REINSINTDTO.IssueAllocationRiskCoverDTO>(cfg =>
            {
                cfg.CreateMap<IssueAllocationRiskCover, REINSINTDTO.IssueAllocationRiskCoverDTO>();
            });
            return config;
        }

        internal static IssueAllocationRiskCover ToModel(this REINSINTDTO.IssueAllocationRiskCoverDTO issueAllocationRiskCoverDTO)
        {
            var config = CreateMapIssueAllocationRiskCoverIntegration();
            return config.Map<REINSINTDTO.IssueAllocationRiskCoverDTO, IssueAllocationRiskCover>(issueAllocationRiskCoverDTO);
        }

        internal static IEnumerable<IssueAllocationRiskCover> ToModels(this IEnumerable<REINSINTDTO.IssueAllocationRiskCoverDTO> issueAllocationRiskCoverDTOs)
        {
            return issueAllocationRiskCoverDTOs.Select(ToModel);
        }

        internal static IMapper CreateMapPoliciesIntegrationProvider()
        {
            var config = MapperCache.GetMapper<REINSINTDTO.PolicyDTO, Policy>(cfg =>
            {
                cfg.CreateMap<REINSINTDTO.PolicyDTO, Policy>();
                cfg.CreateMap<REINSINTDTO.BranchDTO, Branch>();
                cfg.CreateMap<REINSINTDTO.PrefixDTO, Prefix>();
                cfg.CreateMap<REINSINTDTO.CurrencyDTO, Currency>();
                cfg.CreateMap<REINSINTDTO.EndorsementDTO, Endorsement>();
                cfg.CreateMap<REINSINTDTO.RiskDTO, Risk>();
                cfg.CreateMap<REINSINTDTO.CoverageDTO, Coverage>();
                cfg.CreateMap<REINSINTDTO.AmountDTO, Amount>();
            });
            return config;
        }

        internal static Policy ToModel(this REINSINTDTO.PolicyDTO policyDTO)
        {
            var config = CreateMapPoliciesIntegrationProvider();
            return config.Map<REINSINTDTO.PolicyDTO, Policy>(policyDTO);
        }

        internal static IEnumerable<Policy> ToModels(this IEnumerable<REINSINTDTO.PolicyDTO> policiesDTO)
        {
            return policiesDTO.Select(ToModel);
        }

    }
}

