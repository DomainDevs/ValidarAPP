using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Core.Application.OperationQuotaServices.DTOs.OperationQuota;
using Sistran.Core.Application.OperationQuotaServices.DTOs.Consortium;
using Sistran.Core.Application.OperationQuotaServices.DTOs.EconomicGroup;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Models.OperationQuota;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Enums;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Models.Consortium;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Models.EconomicGroup;
using OQDTO = Sistran.Core.Integration.OperationQuotaServices.DTOs.OperationQuota;
using ECODTO = Sistran.Core.Integration.OperationQuotaServices.DTOs.EconomicGroup;
using CONDTO = Sistran.Core.Integration.OperationQuotaServices.DTOs.Consortium;
using Sistran.Core.Application.OperationQuotaServices.DTOs;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Models;
using AutoMapper;
using Sistran.Core.Application.Utilities.Cache;
using CONSINTDTO = Sistran.Core.Integration.OperationQuotaServices.DTOs.Consortium;
using ECOGROUPINTDTO = Sistran.Core.Integration.OperationQuotaServices.DTOs.EconomicGroup;
using Sistran.Core.Application.UniquePerson.IntegrationService.Models;

namespace Sistran.Core.Application.OperationQuotaServices.EEProvider.Assemblers
{
    public static class DTOAssembler
    {
        #region OperatingQuotaEvent
        internal static List<OperatingQuotaEventDTO> CreateOperatingQuotaEvents(List<OperatingQuotaEvent> operatingQuotaEvents)
        {
            List<OperatingQuotaEventDTO> operatingQuotaEventDTOs = new List<OperatingQuotaEventDTO>();

            foreach (OperatingQuotaEvent operatingQuotaEvent in operatingQuotaEvents)
            {
                operatingQuotaEventDTOs.Add(CreateOperatingQuotaEvent(operatingQuotaEvent));
            }

            return operatingQuotaEventDTOs;
        }

        internal static DeclineInsuredDTO CreateDeclineInsured(DeclineInsured declineInsured)
        {
            if (declineInsured == null)
            {
                return null;
            }
            return new DeclineInsuredDTO
            {
                DeclineDate = declineInsured.DeclineDate,
                Decline = declineInsured.Decline
            };
        }

        internal static OperatingQuotaEventDTO CreateOperatingQuotaEvent(OperatingQuotaEvent operatingQuotaEvent)
        {
            if (operatingQuotaEvent == null)
            {
                return null;
            }
            return new OperatingQuotaEventDTO
            {
                IdentificationId = Convert.ToInt32(operatingQuotaEvent.IdentificationId),
                OperatingQuotaEventID = Convert.ToInt32(operatingQuotaEvent.OperatingQuotaEventID),
                OperatingQuotaEventType = Convert.ToInt32(operatingQuotaEvent.OperatingQuotaEventType),
                IssueDate = Convert.ToDateTime(operatingQuotaEvent.IssueDate),
                LineBusinessID = Convert.ToInt32(operatingQuotaEvent.LineBusinessID),
                Cov_Init_Date = Convert.ToDateTime(operatingQuotaEvent.Cov_Init_Date),
                Cov_End_Date = Convert.ToDateTime(operatingQuotaEvent.Cov_End_Date),
                Policy_Init_Date = Convert.ToDateTime(operatingQuotaEvent.Policy_Init_Date),
                Policy_End_Date = Convert.ToDateTime(operatingQuotaEvent.Policy_End_Date),
                payload = Convert.ToString(operatingQuotaEvent.Payload),
                IndividualOperatingQuota = operatingQuotaEvent.IndividualOperatingQuota != null ? CreateIndividualOperatingQuotaDTO(operatingQuotaEvent.IndividualOperatingQuota) : null,
                ApplyEndorsement = operatingQuotaEvent.ApplyEndorsement != null ? CreateApplyEndorsementDTO(operatingQuotaEvent.ApplyEndorsement) : null,
                EconomicGroupEventDTO = operatingQuotaEvent.EconomicGroupEvent != null ? CreateEconomicGroupEvent(operatingQuotaEvent.EconomicGroupEvent) : null,
                consortiumEventDTO = operatingQuotaEvent.consortiumEvent != null ? CreateConsortiumEvent(operatingQuotaEvent.consortiumEvent) : null,
                ApplyReinsurance = operatingQuotaEvent.ApplyReinsurance != null ? CreateApplyReinsuranceDTO(operatingQuotaEvent.ApplyReinsurance) : null,
                declineInsured = operatingQuotaEvent.declineInsured != null ? CreateDeclineInsured(operatingQuotaEvent.declineInsured) : null
            };
        }

        internal static List<ConsortiumEventDTO> CreateConsortiumsEventDTO(List<CONDTO.ConsortiumEventDTO> consortiumEventDTOs)
        {
            List<ConsortiumEventDTO> consortiumsEventDTOs = new List<ConsortiumEventDTO>();

            foreach (CONDTO.ConsortiumEventDTO consortiumEventDTO in consortiumEventDTOs)
            {
                consortiumsEventDTOs.Add(CreateConsortiumEventDTO(consortiumEventDTO));
            }

            return consortiumsEventDTOs;
        }

        internal static List<CONDTO.ConsortiumEventDTO> CreateConsortiumsEventIntegrationDTO(List<ConsortiumEventDTO> consortiumEventDTOs)
        {
            List<CONDTO.ConsortiumEventDTO> consortiumsEventDTOs = new List<CONDTO.ConsortiumEventDTO>();

            foreach (ConsortiumEventDTO consortiumEventDTO in consortiumEventDTOs)
            {
                consortiumsEventDTOs.Add(CreateConsortiumEventIntegrationDTO(consortiumEventDTO));
            }

            return consortiumsEventDTOs;
        }

        internal static ConsortiumEventDTO CreateConsortiumEventDTO(CONDTO.ConsortiumEventDTO consortiumEventDTO)
        {
            var mapper = AutoMapperAssembler.CreateMapConsortiumEventDTO();
            ConsortiumEventDTO consortiumEvent = mapper.Map<CONDTO.ConsortiumEventDTO, ConsortiumEventDTO>(consortiumEventDTO);
            return consortiumEvent;
        }

        internal static TypeSecureDTO CreateSecureType(TypeSecure typeSecure)
        {
            if (typeSecure == null)
            {
                return null;
            }

            return new TypeSecureDTO
            {
                IsEconomicGroup = typeSecure.IsEconomicGroup,
                IsConsortium = typeSecure.IsConsortium,
                IsIndividual = typeSecure.IsIndividual,
                IsNotIndividual = typeSecure.IsNotIndividual
            };
        }
        #endregion

        #region Asignar Cupo
        internal static List<IndividualOperatingQuotaDTO> CreateIndividualOperatingQuotaDTOs(List<IndividualOperatingQuota> individualOperatingQuotas)
        {
            List<IndividualOperatingQuotaDTO> individualOperatingQuotaDTOs = new List<IndividualOperatingQuotaDTO>();

            foreach (IndividualOperatingQuota individualOperatingQuota in individualOperatingQuotas)
            {
                individualOperatingQuotaDTOs.Add(CreateIndividualOperatingQuotaDTO(individualOperatingQuota));
            }

            return individualOperatingQuotaDTOs;
        }

        internal static IndividualOperatingQuotaDTO CreateIndividualOperatingQuotaDTO(IndividualOperatingQuota individualOperatingQuota)
        {
            var mapper = AutoMapperAssembler.CreateMapIndividualOperatingQuota();
            return mapper.Map<IndividualOperatingQuota, IndividualOperatingQuotaDTO>(individualOperatingQuota);
        }
        #endregion

        #region Aplicar Endoso
        internal static List<ApplyEndorsementDTO> CreateIndividualOperatingQuotaEndorsementDTOs(List<ApplyEndorsement> applyEndorsements)
        {
            List<ApplyEndorsementDTO> applyEndorsementDTOs = new List<ApplyEndorsementDTO>();
            foreach (ApplyEndorsement applyEndorsement in applyEndorsements)
            {
                applyEndorsementDTOs.Add(CreateApplyEndorsementDTO(applyEndorsement));
            }
            return applyEndorsementDTOs;
        }

        internal static ApplyEndorsementDTO CreateApplyEndorsementDTO(ApplyEndorsement applyEndorsement)
        {
            var mapper = AutoMapperAssembler.CreateMapApplyEndorsement();
            return mapper.Map<ApplyEndorsement, ApplyEndorsementDTO>(applyEndorsement);
        }

        internal static ApplyReinsuranceDTO CreateApplyReinsuranceDTO(ApplyReinsurance applyReinsurance)
        {
            var mapper = AutoMapperAssembler.CreateMapApplyReinsurance();
            ApplyReinsuranceDTO applyReinsuranceDTO = mapper.Map<ApplyReinsurance, ApplyReinsuranceDTO>(applyReinsurance);
            return applyReinsuranceDTO;
        }

        #endregion

        #region Consorcios
        internal static List<ConsortiumEventDTO> CreateConsortiumEvents(List<ConsortiumEvent> consortiumEvents)
        {
            List<ConsortiumEventDTO> consortiumEventDTOs = new List<ConsortiumEventDTO>();

            foreach (ConsortiumEvent consortiumEvent in consortiumEvents)
            {
                consortiumEventDTOs.Add(CreateConsortiumEvent(consortiumEvent));
            }

            return consortiumEventDTOs;
        }

        internal static ConsortiumEventDTO CreateConsortiumEvent(ConsortiumEvent consortiumEvent)
        {

            var mapper = AutoMapperAssembler.CreateMapConsortiumEvent();
            ConsortiumEventDTO consortiumEventDTO = mapper.Map<ConsortiumEvent, ConsortiumEventDTO>(consortiumEvent);
            return consortiumEventDTO;

        }
        #endregion

        #region EconomicGroup
        internal static EconomicGroupEventDTO CreateEconomicGroupEvent(EconomicGroupEvent economicGroupEvent)
        {
            var mapper = AutoMapperAssembler.CreateMapEconomicGroupEvent();
            EconomicGroupEventDTO economicGroupEventDTO = mapper.Map<EconomicGroupEvent, EconomicGroupEventDTO>(economicGroupEvent);
            return economicGroupEventDTO;
        }

        internal static List<EconomicGroupEventDTO> CreateEconomicGroupEvents(List<EconomicGroupEvent> economicGroupEvents)
        {
            List<EconomicGroupEventDTO> economicGroupEventDTOs = new List<EconomicGroupEventDTO>();

            foreach (EconomicGroupEvent economicGroupEvent in economicGroupEvents)
            {
                economicGroupEventDTOs.Add(CreateEconomicGroupEvent(economicGroupEvent));
            }

            return economicGroupEventDTOs;
        }
        #endregion

        #region Integration
        internal static List<OQDTO.OperatingQuotaEventDTO> CreateOperatingQuotaEventsIntegration(List<OperatingQuotaEvent> operatingQuotaEvents)
        {
            List<OQDTO.OperatingQuotaEventDTO> operatingQuotaEventDTOs = new List<OQDTO.OperatingQuotaEventDTO>();

            foreach (OperatingQuotaEvent operatingQuotaEvent in operatingQuotaEvents)
            {
                operatingQuotaEventDTOs.Add(CreateOperatingQuotaEventIntegration(operatingQuotaEvent));
            }

            return operatingQuotaEventDTOs;
        }

        internal static OQDTO.OperatingQuotaEventDTO CreateOperatingQuotaEventIntegration(OperatingQuotaEvent operatingQuotaEvent)
        {
            if (operatingQuotaEvent == null)
            {
                return null;
            }
            return new OQDTO.OperatingQuotaEventDTO
            {
                Policy_Init_Date = operatingQuotaEvent.Policy_Init_Date,
                Policy_End_Date = operatingQuotaEvent.Policy_End_Date,
                Cov_Init_Date = operatingQuotaEvent.Cov_Init_Date,
                Cov_End_Date = operatingQuotaEvent.Cov_End_Date,
                IdentificationId = operatingQuotaEvent.IdentificationId,
                OperatingQuotaEventID = operatingQuotaEvent.OperatingQuotaEventID,
                OperatingQuotaEventType = operatingQuotaEvent.OperatingQuotaEventType,
                IssueDate = operatingQuotaEvent.IssueDate,
                LineBusinessID = operatingQuotaEvent.LineBusinessID,
                IndividualOperatingQuota = CreateIndividualOperatingQuotaDTOIntegration(operatingQuotaEvent.IndividualOperatingQuota),
                ApplyEndorsement = CreateApplyEndorsementDTOIntegration(operatingQuotaEvent.ApplyEndorsement)
            };
        }

        internal static OQDTO.IndividualOperatingQuotaDTO CreateIndividualOperatingQuotaDTOIntegration(IndividualOperatingQuota individualOperatingQuota)
        {
            if (individualOperatingQuota == null)
            {
                return null;
            }
            return new OQDTO.IndividualOperatingQuotaDTO()
            {
                IndividualID = individualOperatingQuota.IndividualID,
                EndDateOpQuota = individualOperatingQuota.EndDateOpQuota,
                InitDateOpQuota = individualOperatingQuota.InitDateOpQuota,
                LineBusinessID = individualOperatingQuota.LineBusinessID,
                ValueOpQuotaAMT = individualOperatingQuota.ValueOpQuotaAMT,
                ParticipationPercentage = individualOperatingQuota.ParticipationPercentage
            };
        }

        internal static OQDTO.ApplyEndorsementDTO CreateApplyEndorsementDTOIntegration(ApplyEndorsement applyEndorsement)
        {
            if (applyEndorsement == null)
            {
                return null;
            }
            return new OQDTO.ApplyEndorsementDTO
            {
                PolicyID = applyEndorsement.PolicyID,
                Endorsement = applyEndorsement.Endorsement,
                RiskId = applyEndorsement.RiskId,
                IndividualId = applyEndorsement.IndividualId,
                EndorsementType = applyEndorsement.EndorsementType,
                CurrencyType = applyEndorsement.CurrencyType,
                CurrencyTypeDesc = applyEndorsement.CurrencyTypeDesc,
                ParticipationPercentage = applyEndorsement.ParticipationPercentage,
                IsConsortium = applyEndorsement.IsConsortium
            };
        }

        internal static FilterOperationQuota CreateFilterOperationQuotaDTO(FilterOperationQuotaDTO filterOperationQuotaDTO)
        {
            if (filterOperationQuotaDTO == null)
            {
                return null;
            }
            return new FilterOperationQuota()
            {
                IndividualId = filterOperationQuotaDTO.IndividualId,
                LineBusiness = filterOperationQuotaDTO.LineBusiness,
                SubLineBusiness = filterOperationQuotaDTO.SubLineBusiness,
                DateCumulus = filterOperationQuotaDTO.DateCumulus,
                IsFuture = filterOperationQuotaDTO.IsFuture,
                IsDatePolicy = filterOperationQuotaDTO.IsDatePolicy,
                PrefixCd = filterOperationQuotaDTO.PrefixCd
            };
        }

        internal static ContractCoverageDTO CreateContractCoverage(ContractCoverage contractCoverage)
        {
            var mapper = AutoMapperAssembler.CreateMapCoverage();
            return mapper.Map<ContractCoverage, ContractCoverageDTO>(contractCoverage);
        }

        internal static List<ContractCoverageDTO> CreateContractsCoverage(List<ContractCoverage> contractCoverages)
        {
            List<ContractCoverageDTO> contractCoverageDTOs = new List<ContractCoverageDTO>();
            foreach (ContractCoverage contractCoverage in contractCoverages)
            {
                contractCoverageDTOs.Add(CreateContractCoverage(contractCoverage));
            }
            return contractCoverageDTOs;
        }
        internal static OQDTO.OperatingQuotaEventDTO CreateOperatingQuotaEventDTOs(OperatingQuotaEventDTO operatingQuotaEvent)
        {
            if (operatingQuotaEvent == null)
            {
                return null;
            }
            return new OQDTO.OperatingQuotaEventDTO
            {
                IdentificationId = Convert.ToInt32(operatingQuotaEvent.IdentificationId),
                OperatingQuotaEventID = Convert.ToInt32(operatingQuotaEvent.OperatingQuotaEventID),
                OperatingQuotaEventType = Convert.ToInt32(operatingQuotaEvent.OperatingQuotaEventType),
                IssueDate = Convert.ToDateTime(operatingQuotaEvent.IssueDate),
                LineBusinessID = Convert.ToInt32(operatingQuotaEvent.LineBusinessID),
                Cov_Init_Date = Convert.ToDateTime(operatingQuotaEvent.Cov_Init_Date),
                Cov_End_Date = Convert.ToDateTime(operatingQuotaEvent.Cov_End_Date),
                Policy_Init_Date = Convert.ToDateTime(operatingQuotaEvent.Policy_Init_Date),
                Policy_End_Date = Convert.ToDateTime(operatingQuotaEvent.Policy_End_Date),
                payload = Convert.ToString(operatingQuotaEvent.payload),
                IndividualOperatingQuota = CreateIndividualOperatingQuotaIntegrationDTO(operatingQuotaEvent.IndividualOperatingQuota),
                ApplyEndorsement = CreateApplyEndorsementIntegrationDTO(operatingQuotaEvent.ApplyEndorsement),
                EconomicGroupEventDTO = CreateEconomicGroupEventIntegrationDTO(operatingQuotaEvent.EconomicGroupEventDTO),
                consortiumEventDTO = CreateConsortiumEventIntegrationDTO(operatingQuotaEvent.consortiumEventDTO),
            };
        }

        internal static OQDTO.IndividualOperatingQuotaDTO CreateIndividualOperatingQuotaIntegrationDTO(IndividualOperatingQuotaDTO individualOperatingQuota)
        {
            var mapper = AutoMapperAssembler.CreateMapIndividualOperatingQuotaIntegrationDTO();
            OQDTO.IndividualOperatingQuotaDTO individualOperatingQuotaDTO = mapper.Map<IndividualOperatingQuotaDTO, OQDTO.IndividualOperatingQuotaDTO>(individualOperatingQuota);
            return individualOperatingQuotaDTO;
        }

        internal static OQDTO.ApplyEndorsementDTO CreateApplyEndorsementIntegrationDTO(ApplyEndorsementDTO applyEndorsement)
        {
            var mapper = AutoMapperAssembler.CreateMapApplyEndorsementIntegrationDTO();
            OQDTO.ApplyEndorsementDTO applyEndorsementDTO = mapper.Map<ApplyEndorsementDTO, OQDTO.ApplyEndorsementDTO>(applyEndorsement);
            return applyEndorsementDTO;
        }

        internal static ECODTO.EconomicGroupEventDTO CreateEconomicGroupEventIntegrationDTO(EconomicGroupEventDTO economicGroupEvent)
        {
            var mapper = AutoMapperAssembler.CreateMapEconomicGroupEventIntegrationDTO();
            ECODTO.EconomicGroupEventDTO economicGroupEventDTO = mapper.Map<EconomicGroupEventDTO, ECODTO.EconomicGroupEventDTO>(economicGroupEvent);
            return economicGroupEventDTO;
        }

        internal static CONDTO.ConsortiumEventDTO CreateConsortiumEventIntegrationDTO(ConsortiumEventDTO consortiumEvent)
        {
            var mapper = AutoMapperAssembler.CreteMapConsortiumEventDTO();
            CONDTO.ConsortiumEventDTO consortiumEventDTO = mapper.Map<ConsortiumEventDTO, CONDTO.ConsortiumEventDTO>(consortiumEvent);
            return consortiumEventDTO;
        }

        #endregion     

        internal static IMapper CreateMapConsortiumEventIntegration()
        {
            var config = MapperCache.GetMapper<ConsortiumEventDTO, CONSINTDTO.ConsortiumEventDTO>(cfg =>
            {
                cfg.CreateMap<ConsortiumEventDTO, CONSINTDTO.ConsortiumEventDTO>();
            });
            return config;
        }

        internal static CONSINTDTO.ConsortiumEventDTO ToIntegrationDTO(this ConsortiumEventDTO consortiumEventDTO)
        {
            var config = CreateMapConsortiumEventIntegration();
            return config.Map<ConsortiumEventDTO, CONSINTDTO.ConsortiumEventDTO>(consortiumEventDTO);
        }

        internal static IEnumerable<CONSINTDTO.ConsortiumEventDTO> ToIntegrationDTOs(this IEnumerable<ConsortiumEventDTO> consortiumEventDTOs)
        {
            return consortiumEventDTOs.Select(ToIntegrationDTO);
        }

        internal static OQDTO.OperatingQuotaEventDTO CreateIntegrationOperatingQuotaEventDTOByConsortiumEventDTO(OQDTO.OperatingQuotaEventDTO operatingQuotaEventDTO, CONSINTDTO.ConsortiumEventDTO consortiumEventDTO)
        {
            return new OQDTO.OperatingQuotaEventDTO
            {
                ApplyReinsurance = new OQDTO.ApplyReinsuranceDTO
                {
                    PolicyID = operatingQuotaEventDTO.ApplyReinsurance.PolicyID,
                    EndorsementId = operatingQuotaEventDTO.ApplyReinsurance.EndorsementId,
                    EndorsementType = operatingQuotaEventDTO.ApplyReinsurance.EndorsementType,
                    CoverageId = operatingQuotaEventDTO.ApplyReinsurance.CoverageId,
                    IndividualId = operatingQuotaEventDTO.ApplyReinsurance.IndividualId,
                    ConsortiumId = consortiumEventDTO.consortiumDTO == null ? operatingQuotaEventDTO.IdentificationId : consortiumEventDTO.consortiumDTO.ConsotiumId,
                    EconomicGroupId = 0,
                    DocumentNum = operatingQuotaEventDTO.ApplyReinsurance.DocumentNum,
                    CurrencyType = operatingQuotaEventDTO.ApplyReinsurance.CurrencyType,
                    CurrencyTypeDesc = operatingQuotaEventDTO.ApplyReinsurance.CurrencyTypeDesc,
                    BranchId = operatingQuotaEventDTO.ApplyReinsurance.BranchId,
                    PrefixId = operatingQuotaEventDTO.ApplyReinsurance.PrefixId,
                    ParticipationPercentage = consortiumEventDTO.ConsortiumpartnersDTO == null ? 100 : consortiumEventDTO.ConsortiumpartnersDTO.ParticipationRate,
                    ContractCoverage = operatingQuotaEventDTO.ApplyReinsurance.ContractCoverage
                },
                IssueDate = operatingQuotaEventDTO.IssueDate,
                IdentificationId = consortiumEventDTO.IndividualId == 0 ? operatingQuotaEventDTO.IdentificationId : consortiumEventDTO.IndividualId,
                LineBusinessID = operatingQuotaEventDTO.LineBusinessID,
                SubLineBusinessID = operatingQuotaEventDTO.SubLineBusinessID,
                Policy_Init_Date = operatingQuotaEventDTO.Policy_Init_Date,
                Policy_End_Date = operatingQuotaEventDTO.Policy_End_Date,
                Cov_Init_Date = operatingQuotaEventDTO.Cov_Init_Date,
                Cov_End_Date = operatingQuotaEventDTO.Cov_End_Date,
                OperatingQuotaEventType = operatingQuotaEventDTO.OperatingQuotaEventType,
            };
        }

        internal static List<OQDTO.OperatingQuotaEventDTO> CreateIntegrationOperatingQuotaEventDTOsByConsortiumEventDTOs(List<OQDTO.OperatingQuotaEventDTO> operatingQuotaEventDTOs, List<CONSINTDTO.ConsortiumEventDTO> consortiumEventDTOs)
        {
            List<OQDTO.OperatingQuotaEventDTO> operatingQuotaEvent = new List<OQDTO.OperatingQuotaEventDTO>();

            foreach (OQDTO.OperatingQuotaEventDTO operatingQuotaEventDTO in operatingQuotaEventDTOs)
            {
                foreach (CONSINTDTO.ConsortiumEventDTO consortiumEventDTO in consortiumEventDTOs)
                {
                    operatingQuotaEvent.Add(CreateIntegrationOperatingQuotaEventDTOByConsortiumEventDTO(operatingQuotaEventDTO, consortiumEventDTO));
                }
            }

            return operatingQuotaEvent;
        }

        internal static IMapper CreateMapEconomicGroupEventIntegration()
        {
            var config = MapperCache.GetMapper<EconomicGroupEvent, ECOGROUPINTDTO.EconomicGroupEventDTO>(cfg =>
            {
                cfg.CreateMap<EconomicGroupEventDTO, ECOGROUPINTDTO.EconomicGroupEventDTO>();
            });
            return config;
        }

        internal static ECOGROUPINTDTO.EconomicGroupEventDTO ToIntegrationDTO(this EconomicGroupEventDTO economicGroupEventDTO)
        {
            var config = CreateMapEconomicGroupEventIntegration();
            return config.Map<EconomicGroupEventDTO, ECOGROUPINTDTO.EconomicGroupEventDTO>(economicGroupEventDTO);
        }

        internal static IEnumerable<ECOGROUPINTDTO.EconomicGroupEventDTO> ToIntegrationDTOs(this IEnumerable<EconomicGroupEventDTO> economicGroupEventDTOs)
        {
            return economicGroupEventDTOs.Select(ToIntegrationDTO);
        }

        internal static OQDTO.OperatingQuotaEventDTO CreateIntegrationOperatingQuotaEventDTOByEconomicGroupEventDTO(OQDTO.OperatingQuotaEventDTO operatingQuotaEventDTO, ECOGROUPINTDTO.EconomicGroupEventDTO economicGroupEventDTO)
        {
            return new OQDTO.OperatingQuotaEventDTO
            {
                ApplyReinsurance = new OQDTO.ApplyReinsuranceDTO
                {
                    PolicyID = operatingQuotaEventDTO.ApplyReinsurance.PolicyID,
                    EndorsementId = operatingQuotaEventDTO.ApplyReinsurance.EndorsementId,
                    EndorsementType = operatingQuotaEventDTO.ApplyReinsurance.EndorsementType,
                    CoverageId = operatingQuotaEventDTO.ApplyReinsurance.CoverageId,
                    IndividualId = operatingQuotaEventDTO.ApplyReinsurance.IndividualId,
                    ConsortiumId = 0,
                    EconomicGroupId = economicGroupEventDTO.EconomicGroupID,
                    DocumentNum = operatingQuotaEventDTO.ApplyReinsurance.DocumentNum,
                    CurrencyType = operatingQuotaEventDTO.ApplyReinsurance.CurrencyType,
                    CurrencyTypeDesc = operatingQuotaEventDTO.ApplyReinsurance.CurrencyTypeDesc,
                    BranchId = operatingQuotaEventDTO.ApplyReinsurance.BranchId,
                    PrefixId = operatingQuotaEventDTO.ApplyReinsurance.PrefixId,
                    ParticipationPercentage = 100,
                    ContractCoverage = operatingQuotaEventDTO.ApplyReinsurance.ContractCoverage
                },
                IssueDate = operatingQuotaEventDTO.IssueDate,
                IdentificationId = economicGroupEventDTO.IndividualId == 0 ? economicGroupEventDTO.EconomicGroupID : economicGroupEventDTO.economicgrouppartnersDTO.IndividualId,
                LineBusinessID = operatingQuotaEventDTO.LineBusinessID,
                SubLineBusinessID = operatingQuotaEventDTO.SubLineBusinessID,
                Policy_Init_Date = operatingQuotaEventDTO.Policy_Init_Date,
                Policy_End_Date = operatingQuotaEventDTO.Policy_End_Date,
                Cov_Init_Date = operatingQuotaEventDTO.Cov_Init_Date,
                Cov_End_Date = operatingQuotaEventDTO.Cov_End_Date,
                OperatingQuotaEventType = operatingQuotaEventDTO.OperatingQuotaEventType,
            };
        }

        internal static List<OQDTO.OperatingQuotaEventDTO> CreateIntegrationOperatingQuotaEventDTOsByEconomicGroupEventDTOs(List<OQDTO.OperatingQuotaEventDTO> operatingQuotaEventDTOs, List<ECOGROUPINTDTO.EconomicGroupEventDTO> economicGroupEventDTOs)
        {
            List<OQDTO.OperatingQuotaEventDTO> operatingQuotaEvent = new List<OQDTO.OperatingQuotaEventDTO>();

            foreach (OQDTO.OperatingQuotaEventDTO operatingQuotaEventDTO in operatingQuotaEventDTOs)
            {
                foreach (ECOGROUPINTDTO.EconomicGroupEventDTO economicGroupEventDTO in economicGroupEventDTOs)
                {
                    operatingQuotaEvent.Add(CreateIntegrationOperatingQuotaEventDTOByEconomicGroupEventDTO(operatingQuotaEventDTO, economicGroupEventDTO));
                }
            }

            return operatingQuotaEvent;
        }

        internal static List<OQDTO.OperatingQuotaEventDTO> CreateOperatingQuotaEventsDTOs(List<OperatingQuotaEventDTO> list)
        {
            return list.Select(CreateOperatingQuotaEventDTOs).ToList();
        }

        internal static IMapper CreateMapOperatingQuotaEventByIntegration()
        {
            var config = MapperCache.GetMapper<OperatingQuotaEventDTO, OQDTO.OperatingQuotaEventDTO>(cfg =>
            {
                cfg.CreateMap<OQDTO.OperatingQuotaEventDTO, OperatingQuotaEventDTO>();
                cfg.ValidateInlineMaps = false;
            });
            return config;
        }

        internal static OperatingQuotaEventDTO ToDTO(this OQDTO.OperatingQuotaEventDTO operatingQuotaEventDTO)
        {
            var config = CreateMapOperatingQuotaEventByIntegration();
            return config.Map<OQDTO.OperatingQuotaEventDTO, OperatingQuotaEventDTO>(operatingQuotaEventDTO);
        }

        internal static List<OperatingQuotaEventDTO> ToDTOs(this List<OQDTO.OperatingQuotaEventDTO> operatingQuotaEventDTOs)
        {
            var config = CreateMapOperatingQuotaEventByIntegration();
            return config.Map<List<OQDTO.OperatingQuotaEventDTO>, List<OperatingQuotaEventDTO>>(operatingQuotaEventDTOs);
        }

        internal static IMapper CreateMapReinsuranceOperatingQuotaEventIntegration()
        {
            var config = MapperCache.GetMapper<ReinsuranceOperatingQuotaEventDTO, OQDTO.ReinsuranceOperatingQuotaEventDTO>(cfg =>
            {
                cfg.CreateMap<ReinsuranceOperatingQuotaEventDTO, OQDTO.ReinsuranceOperatingQuotaEventDTO>();
            });
            return config;
        }

        internal static OQDTO.ReinsuranceOperatingQuotaEventDTO ToIntegrationDTO(this ReinsuranceOperatingQuotaEventDTO reinsuranceOperatingQuotaEventDTO)
        {
            var config = CreateMapReinsuranceOperatingQuotaEventIntegration();
            return config.Map<ReinsuranceOperatingQuotaEventDTO, OQDTO.ReinsuranceOperatingQuotaEventDTO>(reinsuranceOperatingQuotaEventDTO);
        }

        internal static IEnumerable<OQDTO.ReinsuranceOperatingQuotaEventDTO> ToIntegrationDTOs(this IEnumerable<ReinsuranceOperatingQuotaEventDTO> reinsuranceOperatingQuotaEventDTOs)
        {
            return reinsuranceOperatingQuotaEventDTOs.Select(ToIntegrationDTO);
        }

        internal static IMapper CreateMapReinsuranceOperatingQuotaEvent()
        {
            var config = MapperCache.GetMapper<ReinsuranceOperatingQuotaEvent, ReinsuranceOperatingQuotaEventDTO>(cfg =>
            {
                cfg.CreateMap<ReinsuranceOperatingQuotaEvent, ReinsuranceOperatingQuotaEventDTO>();
            });
            return config;
        }

        internal static ReinsuranceOperatingQuotaEventDTO ToDTO(this ReinsuranceOperatingQuotaEvent reinsuranceOperatingQuotaEvent)
        {
            var config = CreateMapReinsuranceOperatingQuotaEvent();
            return config.Map<ReinsuranceOperatingQuotaEvent, ReinsuranceOperatingQuotaEventDTO>(reinsuranceOperatingQuotaEvent);
        }

        internal static IEnumerable<ReinsuranceOperatingQuotaEventDTO> ToDTOs(this IEnumerable<ReinsuranceOperatingQuotaEvent> reinsuranceOperatingQuotaEvents)
        {
            return reinsuranceOperatingQuotaEvents.Select(ToDTO);
        }

        internal static IMapper CreateMapReinsOperatingQuotaEventByIntegration()
        {
            var config = MapperCache.GetMapper<ReinsuranceOperatingQuotaEventDTO, OQDTO.ReinsuranceOperatingQuotaEventDTO>(cfg =>
            {
                cfg.CreateMap<OQDTO.ReinsuranceOperatingQuotaEventDTO, ReinsuranceOperatingQuotaEventDTO>();
                cfg.ValidateInlineMaps = false;
            });
            return config;
        }

        internal static ReinsuranceOperatingQuotaEventDTO ToDTO(this OQDTO.ReinsuranceOperatingQuotaEventDTO reinsuranceOperatingQuotaEventDTO)
        {
            var config = CreateMapReinsOperatingQuotaEventByIntegration();
            return config.Map<OQDTO.ReinsuranceOperatingQuotaEventDTO, ReinsuranceOperatingQuotaEventDTO>(reinsuranceOperatingQuotaEventDTO);
        }

        internal static List<ReinsuranceOperatingQuotaEventDTO> ToDTOs(this List<OQDTO.ReinsuranceOperatingQuotaEventDTO> reinsuranceOperatingQuotaEventDTOs)
        {
            var config = CreateMapReinsOperatingQuotaEventByIntegration();
            return config.Map<List<OQDTO.ReinsuranceOperatingQuotaEventDTO>, List<ReinsuranceOperatingQuotaEventDTO>>(reinsuranceOperatingQuotaEventDTOs);
        }

        internal static IMapper CreateMapOperatingQuotaEvent()
        {
            var config = MapperCache.GetMapper<OperatingQuotaEventDTO, OperatingQuotaEvent>(cfg =>
            {
                cfg.CreateMap<OperatingQuotaEvent, OperatingQuotaEventDTO>();
                cfg.ValidateInlineMaps = false;
            });
            return config;
        }

        internal static OperatingQuotaEventDTO ToDTO(this OperatingQuotaEvent operatingQuotaEvent)
        {
            var config = CreateMapOperatingQuotaEvent();
            return config.Map<OperatingQuotaEvent, OperatingQuotaEventDTO>(operatingQuotaEvent);
        }

        internal static List<OperatingQuotaEventDTO> ToDTOs(this List<OperatingQuotaEvent> operatingQuotaEvents)
        {
            var config = CreateMapOperatingQuotaEvent();
            return config.Map<List<OperatingQuotaEvent>, List<OperatingQuotaEventDTO>>(operatingQuotaEvents);
        }

        internal static IMapper CreateMapOperatingQuotaEventIntegration()
        {
            var config = MapperCache.GetMapper<OQDTO.OperatingQuotaEventDTO, OperatingQuotaEventDTO>(cfg =>
            {
                cfg.CreateMap<OperatingQuotaEventDTO, OQDTO.OperatingQuotaEventDTO>();
                cfg.ValidateInlineMaps = false;
            });
            return config;
        }

        internal static OQDTO.OperatingQuotaEventDTO ToIntegrationDTO(this OperatingQuotaEventDTO operatingQuotaEventDTO)
        {
            var config = CreateMapOperatingQuotaEventIntegration();
            return config.Map<OperatingQuotaEventDTO, OQDTO.OperatingQuotaEventDTO>(operatingQuotaEventDTO);
        }

        internal static List<OQDTO.OperatingQuotaEventDTO> ToIntegrationDTOs(this List<OperatingQuotaEventDTO> operatingQuotaEventDTOs)
        {
            var config = CreateMapOperatingQuotaEvent();
            return config.Map<List<OperatingQuotaEventDTO>, List<OQDTO.OperatingQuotaEventDTO>>(operatingQuotaEventDTOs);
        }

        internal static IMapper CreateMapEconomicGroup()
        {
            var config = MapperCache.GetMapper<IntegrationEconomicGroup, EconomicGroupDTO>(cfg =>
            {
                cfg.CreateMap<IntegrationEconomicGroup, EconomicGroupDTO>();
            });
            return config;
        }

        internal static EconomicGroupDTO ToDTO(this IntegrationEconomicGroup economicGroup)
        {
            var config = CreateMapEconomicGroup();
            return config.Map<IntegrationEconomicGroup, EconomicGroupDTO>(economicGroup);
        }

        internal static IEnumerable<EconomicGroupDTO> ToDTOs(this IEnumerable<IntegrationEconomicGroup> economicGroups)
        {
            return economicGroups.Select(ToDTO);
        }

        internal static IMapper CreateMapEconomicGroupDetail()
        {
            var config = MapperCache.GetMapper<IntegrationEconomicGroup, EconomicGroupDTO>(cfg =>
            {
                cfg.CreateMap<IntegrationEconomicGroup, EconomicGroupDTO>();
            });
            return config;
        }

        internal static EconomicGroupDetailDTO ToDTO(this IntegrationEconomicGroupDetail economicGroupDetail)
        {
            var config = CreateMapEconomicGroupDetail();
            return config.Map<IntegrationEconomicGroupDetail, EconomicGroupDetailDTO>(economicGroupDetail);
        }

        internal static IEnumerable<EconomicGroupDetailDTO> ToDTOs(this IEnumerable<IntegrationEconomicGroupDetail> economicGroupDetails)
        {
            return economicGroupDetails.Select(ToDTO);
        }

        internal static IMapper CreateMapRiskConsortium()
        {
            var config = MapperCache.GetMapper<RiskConsortium, RiskConsortiumDTO>(cfg =>
            {
                cfg.CreateMap<RiskConsortium, RiskConsortiumDTO>();
            });
            return config;
        }

        internal static RiskConsortiumDTO ToDTO(this RiskConsortium riskConsortium)
        {
            var config = CreateMapRiskConsortium();
            return config.Map<RiskConsortium, RiskConsortiumDTO>(riskConsortium);
        }

        internal static IEnumerable<RiskConsortiumDTO> ToDTOs(this IEnumerable<RiskConsortium> riskConsortiums)
        {
            return riskConsortiums.Select(ToDTO);
        }

        internal static IMapper CreateMapRiskConsortiumIntegration()
        {
            var config = MapperCache.GetMapper<RiskConsortiumDTO, CONDTO.RiskConsortiumDTO>(cfg =>
            {
                cfg.CreateMap<RiskConsortiumDTO, CONDTO.RiskConsortiumDTO>();
            });
            return config;
        }

        internal static CONDTO.RiskConsortiumDTO ToIntegrationDTO(this RiskConsortiumDTO riskConsortium)
        {
            var config = CreateMapReinsuranceOperatingQuotaEventIntegration();
            return config.Map<RiskConsortiumDTO, CONDTO.RiskConsortiumDTO>(riskConsortium);
        }

        internal static IEnumerable<CONDTO.RiskConsortiumDTO> ToIntegrationDTOs(this IEnumerable<RiskConsortiumDTO> riskConsortiums)
        {
            return riskConsortiums.Select(ToIntegrationDTO);
        }
    }
}
