using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Core.Application.OperationQuotaServices.DTOs.OperationQuota;
using Sistran.Core.Application.OperationQuotaServices.DTOs.Consortium;
using Sistran.Core.Application.OperationQuotaServices.DTOs.EconomicGroup;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Models.OperationQuota;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Models.Consortium;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Models.EconomicGroup;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Enums;
using Sistran.Core.Framework.DAF;
using UPEN = Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.Utilities.Helper;
using OQDTO = Sistran.Core.Integration.OperationQuotaServices.DTOs.OperationQuota;
using Sistran.Core.Application.CommonService.Models;
using COMMEN = Sistran.Core.Application.Common.Entities;
using OQINTDTO = Sistran.Core.Integration.OperationQuotaServices.DTOs.OperationQuota;
using CONDTO = Sistran.Core.Integration.OperationQuotaServices.DTOs.Consortium;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Models;
using AutoMapper;
using Sistran.Core.Application.Utilities.Cache;
using ISSEN = Sistran.Core.Application.Issuance.Entities;

namespace Sistran.Core.Application.OperationQuotaServices.EEProvider.Assemblers

{
    public static class ModelAssembler
    {

        #region OPERATIONQUOTAEVENT
        internal static List<OperatingQuotaEvent> CreateOperatingQuotaEvents(BusinessCollection businessObjects)
        {
            List<OperatingQuotaEvent> operatingQuotaEvents = new List<OperatingQuotaEvent>();

            foreach (UPEN.OperatingQuotaEvent entityoperatingQuotaEvents in businessObjects)
            {
                operatingQuotaEvents.Add(CreateOperatingQuotaEvent(entityoperatingQuotaEvents));
            }

            return operatingQuotaEvents;
        }

        internal static OperatingQuotaEvent CreateOperatingQuotaEvent(UPEN.OperatingQuotaEvent entityOperatingQuotaEvent)
        {
            if (entityOperatingQuotaEvent == null)
            {
                return null;
            }

            if (Convert.ToInt32(entityOperatingQuotaEvent.OperatingQuotaTypeEvent) != (int)EnumEventOperationQuota.ASSIGN_INDIVIDUAL_OPERATION_QUOTA)
            {
                return new OperatingQuotaEvent
                {
                    ApplyEndorsement = JsonHelper.DeserializeJson<ApplyEndorsement>(entityOperatingQuotaEvent.Payload),
                    ApplyReinsurance = JsonHelper.DeserializeJson<ApplyReinsurance>(entityOperatingQuotaEvent.Payload),
                    Payload = entityOperatingQuotaEvent.Payload,
                    IdentificationId = Convert.ToInt32(entityOperatingQuotaEvent.IdentificationId),
                    IssueDate = entityOperatingQuotaEvent.IssueDate,
                    OperatingQuotaEventID = entityOperatingQuotaEvent.OperatingQuotaEventCode,
                    OperatingQuotaEventType = Convert.ToInt32(entityOperatingQuotaEvent.OperatingQuotaTypeEvent),
                    LineBusinessID = entityOperatingQuotaEvent.LineBusinessCode,
                    Policy_Init_Date = Convert.ToDateTime(entityOperatingQuotaEvent.PolicyInitDate),
                    Policy_End_Date = Convert.ToDateTime(entityOperatingQuotaEvent.PolicyEndDate),
                    Cov_Init_Date = Convert.ToDateTime(entityOperatingQuotaEvent.CovInitDat),
                    Cov_End_Date = entityOperatingQuotaEvent.CovEndDa
                };

            }
            else
            {
                return new OperatingQuotaEvent
                {
                    IndividualOperatingQuota = JsonHelper.DeserializeJson<IndividualOperatingQuota>(entityOperatingQuotaEvent.Payload),
                    Payload = entityOperatingQuotaEvent.Payload,
                    IdentificationId = Convert.ToInt32(entityOperatingQuotaEvent.IdentificationId),
                    IssueDate = entityOperatingQuotaEvent.IssueDate,
                    OperatingQuotaEventID = entityOperatingQuotaEvent.OperatingQuotaEventCode,
                    OperatingQuotaEventType = Convert.ToInt32(entityOperatingQuotaEvent.OperatingQuotaTypeEvent)
                };
            }
        }

        internal static OQINTDTO.OperatingQuotaEventDTO CreateApllyEndorsement(OQINTDTO.OperatingQuotaEventDTO item)
        {
            return new OQINTDTO.OperatingQuotaEventDTO
            {
                OperatingQuotaEventID = item.OperatingQuotaEventID,
                OperatingQuotaEventType = item.OperatingQuotaEventType,
                IdentificationId = item.IdentificationId,
                IssueDate = item.IssueDate,
                LineBusinessID = item.LineBusinessID,
                payload = item.payload,
                Policy_Init_Date = item.Policy_Init_Date,
                Policy_End_Date = item.Policy_End_Date,
                Cov_Init_Date = item.Cov_Init_Date,
                Cov_End_Date = item.Cov_End_Date,
                IndividualOperatingQuota = item.IndividualOperatingQuota,
                ApplyEndorsement = item.ApplyEndorsement
            };
        }
        //quitar
        internal static OQINTDTO.OperatingQuotaEventDTO CreateApllyEndorsementConsortium(OQINTDTO.OperatingQuotaEventDTO item, ConsortiumEvent item1)
        {
            return new OQINTDTO.OperatingQuotaEventDTO
            {
                OperatingQuotaEventID = item.OperatingQuotaEventID,
                OperatingQuotaEventType = item.OperatingQuotaEventType,
                IdentificationId = item1.IndividualId,
                IssueDate = item.IssueDate,
                LineBusinessID = item.LineBusinessID,
                payload = item.payload,
                Policy_Init_Date = item.Policy_Init_Date,
                Policy_End_Date = item.Policy_End_Date,
                Cov_Init_Date = item.Cov_Init_Date,
                Cov_End_Date = item.Cov_End_Date,
                IndividualOperatingQuota = item.IndividualOperatingQuota,
                ApplyEndorsement = CreateApllyEndorsementConsortium(item.ApplyEndorsement, item1)
            };
        }
        internal static OQINTDTO.OperatingQuotaEventDTO CreateAplyEndorsementRiskConsortium(OQINTDTO.OperatingQuotaEventDTO item, RiskConsortium item1)
        {
            return new OQINTDTO.OperatingQuotaEventDTO
            {
                OperatingQuotaEventID = item.OperatingQuotaEventID,
                OperatingQuotaEventType = item.OperatingQuotaEventType,
                IdentificationId = item1.IndividualId,
                IssueDate = item.IssueDate,
                LineBusinessID = item.LineBusinessID,
                payload = item.payload,
                Policy_Init_Date = item.Policy_Init_Date,
                Policy_End_Date = item.Policy_End_Date,
                Cov_Init_Date = item.Cov_Init_Date,
                Cov_End_Date = item.Cov_End_Date,
                IndividualOperatingQuota = item.IndividualOperatingQuota,
                ApplyEndorsement = new OQINTDTO.ApplyEndorsementDTO
                {
                    AmountCoverage = item.ApplyEndorsement.AmountCoverage * item1.PjePart / 100,
                    PolicyID = item.ApplyEndorsement.PolicyID,
                    RiskId = item.ApplyEndorsement.RiskId,
                    Endorsement = item.ApplyEndorsement.Endorsement,
                    IndividualId = item.ApplyEndorsement.IndividualId,
                    EndorsementType = item.ApplyEndorsement.EndorsementType,
                    CurrencyType = item.ApplyEndorsement.CurrencyType,
                    CurrencyTypeDesc = item.ApplyEndorsement.CurrencyTypeDesc,
                    ParticipationPercentage = item1.PjePart,
                    CoverageId = item.ApplyEndorsement.CoverageId,
                    Cov_End_Date = item.ApplyEndorsement.Cov_End_Date,
                    Cov_Init_Date = item.ApplyEndorsement.Cov_Init_Date,
                    Policy_End_Date = item.ApplyEndorsement.Policy_End_Date,
                    Policy_Init_Date = item.ApplyEndorsement.Policy_Init_Date,
                    IsConsortium = true
                }//CreateApllyEndorsementRiskConsortium(item.ApplyEndorsement, item1)
        };
        }
       //quitar
        private static OQINTDTO.ApplyEndorsementDTO CreateApllyEndorsementRiskConsortium(OQINTDTO.ApplyEndorsementDTO applyEndorsement, RiskConsortiumDTO consortiumEvent)
        {
            if (applyEndorsement == null)
            {
                return null;
            }
            return new OQINTDTO.ApplyEndorsementDTO
            {
                AmountCoverage = Convert.ToInt64(applyEndorsement.AmountCoverage * consortiumEvent.PjePart / 100),
                PolicyID = applyEndorsement.PolicyID,
                RiskId = applyEndorsement.RiskId,
                Endorsement = applyEndorsement.Endorsement,
                IndividualId = consortiumEvent.IndividualId,
                EndorsementType = applyEndorsement.EndorsementType,
                CurrencyType = applyEndorsement.CurrencyType,
                CurrencyTypeDesc = applyEndorsement.CurrencyTypeDesc,
                ParticipationPercentage = (decimal)(consortiumEvent.PjePart),
                CoverageId = applyEndorsement.CoverageId,
                Cov_End_Date = applyEndorsement.Cov_End_Date,
                Cov_Init_Date = applyEndorsement.Cov_Init_Date,
                Policy_End_Date = applyEndorsement.Policy_End_Date,
                Policy_Init_Date = applyEndorsement.Policy_Init_Date,
                IsConsortium = true
            };
        }
       //quitar
        private static OQINTDTO.ApplyEndorsementDTO CreateApllyEndorsementConsortium(OQINTDTO.ApplyEndorsementDTO applyEndorsement, ConsortiumEvent consortiumEvent)
        {
            if (applyEndorsement == null)
            {
                return null;
            }
            return new OQINTDTO.ApplyEndorsementDTO
            {
                AmountCoverage = Convert.ToInt64(applyEndorsement.AmountCoverage * consortiumEvent.Consortiumpartners?.ParticipationRate / 100),
                PolicyID = applyEndorsement.PolicyID,
                RiskId = applyEndorsement.RiskId,
                Endorsement = applyEndorsement.Endorsement,
                IndividualId = consortiumEvent.IndividualId,
                EndorsementType = applyEndorsement.EndorsementType,
                CurrencyType = applyEndorsement.CurrencyType,
                CurrencyTypeDesc = applyEndorsement.CurrencyTypeDesc,
                ParticipationPercentage = (decimal)(consortiumEvent.Consortiumpartners?.ParticipationRate ?? 0),
                CoverageId = applyEndorsement.CoverageId,
                Cov_End_Date = applyEndorsement.Cov_End_Date,
                Cov_Init_Date = applyEndorsement.Cov_Init_Date,
                Policy_End_Date = applyEndorsement.Policy_End_Date,
                Policy_Init_Date = applyEndorsement.Policy_Init_Date,
                IsConsortium = true
            };
        }

        internal static OperatingQuotaEvent CreateOperatingQuotaEvent(OperatingQuotaEventDTO operatingQuotaEventDTO)
        {
            if (operatingQuotaEventDTO == null)
            {
                return null;
            }
            return new OperatingQuotaEvent
            {
                IdentificationId = operatingQuotaEventDTO.IdentificationId,
                IssueDate = operatingQuotaEventDTO.IssueDate,
                OperatingQuotaEventID = operatingQuotaEventDTO.OperatingQuotaEventID,
                OperatingQuotaEventType = operatingQuotaEventDTO.OperatingQuotaEventType,
                IndividualOperatingQuota = CreateIndividualOperatingQuotaDTO(operatingQuotaEventDTO.IndividualOperatingQuota)
            };
        }

        internal static List<IndividualOperatingQuota> CreateIndividualOperatingQuotaDTOs(List<IndividualOperatingQuotaDTO> individualOperatingQuotaDTOs)
        {
            List<IndividualOperatingQuota> individualOperatingQuotas = new List<IndividualOperatingQuota>();

            foreach (IndividualOperatingQuotaDTO individualOperatingQuotaDTO in individualOperatingQuotaDTOs)
            {
                individualOperatingQuotas.Add(CreateIndividualOperatingQuotaDTO(individualOperatingQuotaDTO));
            }

            return individualOperatingQuotas;
        }

        internal static IndividualOperatingQuota CreateIndividualOperatingQuotaDTO(IndividualOperatingQuotaDTO individualOperatingQuotaDTO)
        {
            if (individualOperatingQuotaDTO == null)
            {
                return null;
            }
            return new IndividualOperatingQuota()
            {
                IndividualID = individualOperatingQuotaDTO.IndividualID,
                EndDateOpQuota = individualOperatingQuotaDTO.EndDateOpQuota,
                InitDateOpQuota = individualOperatingQuotaDTO.InitDateOpQuota,
                LineBusinessID = individualOperatingQuotaDTO.LineBusinessID,
                ValueOpQuotaAMT = individualOperatingQuotaDTO.ValueOpQuotaAMT,
                ParticipationPercentage = individualOperatingQuotaDTO.ParticipationPercentage
            };
        }

        internal static List<OperatingQuotaEvent> CreateOperatingQuotaEventsDTOs(List<OperatingQuotaEventDTO> operatingQuotaEventDTOs)
        {
            List<OperatingQuotaEvent> operatingQuotaEvents = new List<OperatingQuotaEvent>();

            foreach (OperatingQuotaEventDTO operatingQuotaEventDTO in operatingQuotaEventDTOs)
            {
                operatingQuotaEvents.Add(CreateOperatingQuotaEventsDTO(operatingQuotaEventDTO));
            }
            return operatingQuotaEvents;
        }

        internal static OperatingQuotaEvent CreateOperatingQuotaEventsDTO(OperatingQuotaEventDTO operatingQuotaEventDTO)
        {
            if (operatingQuotaEventDTO == null)
            {
                return null;
            }
            return new OperatingQuotaEvent
            {
                OperatingQuotaEventID = operatingQuotaEventDTO.OperatingQuotaEventID,
                OperatingQuotaEventType = operatingQuotaEventDTO.OperatingQuotaEventType,
                IdentificationId = operatingQuotaEventDTO.IdentificationId,
                IssueDate = operatingQuotaEventDTO.IssueDate,
                LineBusinessID = operatingQuotaEventDTO.LineBusinessID,
                IndividualOperatingQuota = CreateIndividualOperatingQuotaDTO(operatingQuotaEventDTO.IndividualOperatingQuota),
                Payload = operatingQuotaEventDTO.payload
            };
        }

        internal static List<DeclineInsured> CreateDeclineInsureds(BusinessCollection businessObjects)
        {
            List<DeclineInsured> declineInsureds = new List<DeclineInsured>();

            foreach (UPEN.Insured insuredEntity in businessObjects)
            {
                declineInsureds.Add(CreateDeclineInsured(insuredEntity));
            }

            return declineInsureds;
        }

        private static DeclineInsured CreateDeclineInsured(UPEN.Insured insuredEntity)
        {
            if (insuredEntity.DeclinedDate == null)
            {
                return null;
            }
            return new DeclineInsured
            {
                DeclineDate = Convert.ToDateTime(insuredEntity.DeclinedDate)
            };

        }
        #endregion

        #region CONSORTIUMEVENT
        internal static List<ConsortiumEvent> CreateConsortiumEvents(List<ConsortiumEventDTO> consortiumEventDTOs)
        {
            List<ConsortiumEvent> consortiumEvents = new List<ConsortiumEvent>();
            if (consortiumEventDTOs != null)
            {
                foreach (ConsortiumEventDTO consortiumEventDTO in consortiumEventDTOs)
                {
                    consortiumEvents.Add(CreateConsortiumEvent(consortiumEventDTO));
                }
            }

            return consortiumEvents;
        }
        internal static ConsortiumEvent CreateConsortiumEvent(ConsortiumEventDTO consortiumEventDTO)
        {
            if (consortiumEventDTO == null)
            {
                return null;
            }
            return new ConsortiumEvent
            {
                ConsortiumEventEventType = Convert.ToInt32(consortiumEventDTO.ConsortiumEventEventType),
                consortium = new Consortium
                {
                    AssociationType = Convert.ToInt32(consortiumEventDTO.consortiumDTO?.AssociationType),
                    AssociationTypeDesc = Convert.ToString(consortiumEventDTO.consortiumDTO?.AssociationTypeDesc),
                    ConsortiumName = Convert.ToString(consortiumEventDTO.consortiumDTO?.ConsortiumName),
                    ConsotiumId = Convert.ToInt32(consortiumEventDTO.consortiumDTO?.ConsotiumId),
                    UpdateDate = Convert.ToDateTime(consortiumEventDTO.consortiumDTO?.UpdateDate)
                },
                Consortiumpartners = new Consortiumpartners
                {
                    ConsortiumId = Convert.ToInt32(consortiumEventDTO.ConsortiumpartnersDTO?.ConsortiumId),
                    IndividualConsortiumId = Convert.ToInt32(consortiumEventDTO.ConsortiumpartnersDTO?.IndividualConsortiumId),
                    IndividualPartnerId = Convert.ToInt32(consortiumEventDTO.ConsortiumpartnersDTO?.IndividualPartnerId),
                    ParticipationRate = Convert.ToDecimal(consortiumEventDTO.ConsortiumpartnersDTO?.ParticipationRate),
                    PartnerName = Convert.ToString(consortiumEventDTO.ConsortiumpartnersDTO?.PartnerName),
                    InitDate = Convert.ToDateTime(consortiumEventDTO.ConsortiumpartnersDTO?.InitDate),
                    EndDate = Convert.ToDateTime(consortiumEventDTO.ConsortiumpartnersDTO?.EndDate),
                    Enabled = Convert.ToBoolean(consortiumEventDTO.ConsortiumpartnersDTO?.Enabled)
                },
                ConsortiumEventID = Convert.ToInt32(consortiumEventDTO.ConsortiumEventID),
                IndividualId = Convert.ToInt32(consortiumEventDTO.IndividualId),
                IssueDate = Convert.ToDateTime(consortiumEventDTO.IssueDate),
                IndividualConsortiumID = Convert.ToInt32(consortiumEventDTO.IndividualConsortiumID),
                payload = Convert.ToString(consortiumEventDTO.payload)
            };

        }

        internal static List<ConsortiumEvent> CreateConsortiumEvents(BusinessCollection businessObjects)
        {
            List<ConsortiumEvent> consortiumEvents = new List<ConsortiumEvent>();

            foreach (UPEN.ConsortiumEvent entityconsortiumEvent in businessObjects)
            {
                consortiumEvents.Add(CreateConsortiumEvent(entityconsortiumEvent));
            }

            return consortiumEvents;
        }
        internal static ConsortiumEvent CreateConsortiumEvent(UPEN.ConsortiumEvent entityconsortiumEvent)
        {
            if (Convert.ToInt32(entityconsortiumEvent.ConsortiumEventTypeEven) == 1)
            {
                return new ConsortiumEvent
                {
                    consortium = JsonHelper.DeserializeJson<Consortium>(entityconsortiumEvent.Payload),
                    ConsortiumEventID = entityconsortiumEvent.ConsortiumEventCode,
                    ConsortiumEventEventType = Convert.ToInt32(entityconsortiumEvent.ConsortiumEventTypeEven),
                    IndividualConsortiumID = entityconsortiumEvent.ConsortiumId,
                    IndividualId = entityconsortiumEvent.IndividualId,
                    IssueDate = entityconsortiumEvent.IssueDate,
                    payload = entityconsortiumEvent.Payload
                };
            }
            else
            {
                return new ConsortiumEvent
                {
                    Consortiumpartners = JsonHelper.DeserializeJson<Consortiumpartners>(entityconsortiumEvent.Payload),
                    ConsortiumEventID = entityconsortiumEvent.ConsortiumEventCode,
                    ConsortiumEventEventType = Convert.ToInt32(entityconsortiumEvent.ConsortiumEventTypeEven),
                    IndividualConsortiumID = entityconsortiumEvent.ConsortiumId,
                    IndividualId = entityconsortiumEvent.IndividualId,
                    IssueDate = entityconsortiumEvent.IssueDate,
                    payload = entityconsortiumEvent.Payload
                };
            }
        }

        #endregion

        #region ECONOMICGROUP
        internal static EconomicGroupEvent CreateEconomicGroupEvent(UPEN.EconomicGroupEvent entityEconomicGroupEvent)
        {
            if (Convert.ToInt32(entityEconomicGroupEvent.EconomicGroupEventType) == 1)
            {
                return new EconomicGroupEvent
                {
                    EconomicGroupOperatingQuota = JsonHelper.DeserializeJson<Economicgroupoperatingquota>(entityEconomicGroupEvent.Payload),
                    EconomicGroupEventId = entityEconomicGroupEvent.EconomicGroupEventCode,
                    EconomicGroupEventType = Convert.ToInt32(entityEconomicGroupEvent.EconomicGroupEventType),
                    EconomicGroupId = entityEconomicGroupEvent.EconomicGroupId,
                    IndividualId = entityEconomicGroupEvent.IndividualId,
                    IssueDate = entityEconomicGroupEvent.IssueDate,
                    payload = entityEconomicGroupEvent.Payload
                };
            }
            else
            {
                return new EconomicGroupEvent
                {
                    EconomicGroupPartners = JsonHelper.DeserializeJson<Economicgrouppartners>(entityEconomicGroupEvent.Payload),
                    EconomicGroupEventId = entityEconomicGroupEvent.EconomicGroupEventCode,
                    EconomicGroupEventType = Convert.ToInt32(entityEconomicGroupEvent.EconomicGroupEventType),
                    EconomicGroupId = entityEconomicGroupEvent.EconomicGroupId,
                    IndividualId = entityEconomicGroupEvent.IndividualId,
                    IssueDate = entityEconomicGroupEvent.IssueDate,
                    payload = entityEconomicGroupEvent.Payload
                };
            }
        }
        internal static EconomicGroupEvent CreateEconomicGroupEvent(EconomicGroupEventDTO economicGroupEventDTO)
        {
            if (economicGroupEventDTO == null)
            {
                return null;
            }
            return new EconomicGroupEvent
            {
                EconomicGroupEventType = Convert.ToInt32(economicGroupEventDTO.EconomicGroupEventEventType),
                EconomicGroupOperatingQuota = new Economicgroupoperatingquota
                {
                    EconomicGroupID = Convert.ToInt32(economicGroupEventDTO.economicgroupoperatingquotaDTO?.EconomicGroupID),
                    EconomicGroupName = Convert.ToString(economicGroupEventDTO.economicgroupoperatingquotaDTO?.EconomicGroupName),
                    ValueOpQuota = Convert.ToInt64(economicGroupEventDTO.economicgroupoperatingquotaDTO?.ValueOpQuota),
                    Enable = Convert.ToBoolean(economicGroupEventDTO.economicgroupoperatingquotaDTO?.Enable),
                    InitDate = Convert.ToDateTime(economicGroupEventDTO.economicgroupoperatingquotaDTO?.InitDate),
                    DeclineDate = Convert.ToDateTime(economicGroupEventDTO.economicgroupoperatingquotaDTO?.DeclineDate)
                },
                EconomicGroupPartners = new Economicgrouppartners
                {
                    EconomicGroupId = Convert.ToInt32(economicGroupEventDTO.economicgrouppartnersDTO?.EconomicGroupId),
                    IndividualId = Convert.ToInt32(economicGroupEventDTO.economicgrouppartnersDTO?.IndividualId),
                    Enable = Convert.ToBoolean(economicGroupEventDTO.economicgrouppartnersDTO?.Enable),
                    InitDate = Convert.ToDateTime(economicGroupEventDTO.economicgrouppartnersDTO?.InitDate),
                    DeclineDate = Convert.ToDateTime(economicGroupEventDTO.economicgrouppartnersDTO?.DeclineDate)
                },
                EconomicGroupEventId = Convert.ToInt32(economicGroupEventDTO.EconomicGroupEventID),
                EconomicGroupId = Convert.ToInt32(economicGroupEventDTO.EconomicGroupID),
                IndividualId = Convert.ToInt32(economicGroupEventDTO.IndividualId),
                IssueDate = Convert.ToDateTime(economicGroupEventDTO.IssueDate),
                payload = Convert.ToString(economicGroupEventDTO.payload)
            };
        }

        internal static List<EconomicGroupEvent> CreateEconomicGroupEvents(List<EconomicGroupEventDTO> economicGroupEventDTOs)
        {
            List<EconomicGroupEvent> economicGroupEvents = new List<EconomicGroupEvent>();

            foreach (EconomicGroupEventDTO economicGroupEventDTO in economicGroupEventDTOs)
            {
                economicGroupEvents.Add(CreateEconomicGroupEvent(economicGroupEventDTO));
            }

            return economicGroupEvents;
        }
        internal static List<EconomicGroupEvent> CreateEconomicGroupEvents(BusinessCollection businessObjects)
        {
            List<EconomicGroupEvent> economicGroupEvents = new List<EconomicGroupEvent>();

            foreach (UPEN.EconomicGroupEvent economicGroupEventDTO in businessObjects)
            {
                economicGroupEvents.Add(CreateEconomicGroupEvent(economicGroupEventDTO));
            }

            return economicGroupEvents;
        }
        #endregion

        #region Integration
        internal static List<OperatingQuotaEvent> CreateOperatingQuotaEventsDTOs(List<OQDTO.OperatingQuotaEventDTO> operatingQuotaEventDTOs)
        {
            List<OperatingQuotaEvent> operatingQuotaEvents = new List<OperatingQuotaEvent>();

            foreach (OQDTO.OperatingQuotaEventDTO economicGroupEventDTO in operatingQuotaEventDTOs)
            {
                operatingQuotaEvents.Add(CreateOperatingQuotaEventsDTO(economicGroupEventDTO));
            }

            return operatingQuotaEvents;
        }
        internal static List<CONDTO.RiskConsortiumDTO> CreateRiskConsortiumDTOs(List<RiskConsortium> riskConsortium)
        {
            List<RiskConsortiumDTO> riskConsortiumDTOs = new List<RiskConsortiumDTO>();
            IMapper imapper = ModelAssembler.CreateMapRiskConsortiumDTO();
            return imapper.Map<List<RiskConsortium>, List<CONDTO.RiskConsortiumDTO>>(riskConsortium);
        
        }
        public static IMapper CreateMapRiskConsortiumDTO()
        {
            IMapper config = MapperCache.GetMapper<RiskConsortium, RiskConsortiumDTO>(cfg =>
            {
                cfg.CreateMap<RiskConsortium, RiskConsortiumDTO>();
            });
            return config;
        }

        internal static OperatingQuotaEvent CreateOperatingQuotaEventsDTO(OQDTO.OperatingQuotaEventDTO operatingQuotaEventDTO)
        {
            if (operatingQuotaEventDTO == null)
            {
                return null;
            }
            return new OperatingQuotaEvent
            {
                ApplyEndorsement = new ApplyEndorsement
                {
                    AmountCoverage = operatingQuotaEventDTO.ApplyEndorsement.AmountCoverage,
                    CurrencyType = operatingQuotaEventDTO.ApplyEndorsement.CurrencyType,
                    CurrencyTypeDesc = operatingQuotaEventDTO.ApplyEndorsement.CurrencyTypeDesc,
                    Endorsement = operatingQuotaEventDTO.ApplyEndorsement.Endorsement,
                    RiskId = operatingQuotaEventDTO.ApplyEndorsement.RiskId,
                    EndorsementType = operatingQuotaEventDTO.ApplyEndorsement.EndorsementType,
                    IndividualId = operatingQuotaEventDTO.ApplyEndorsement.IndividualId,
                    ParticipationPercentage = operatingQuotaEventDTO.ApplyEndorsement.ParticipationPercentage,
                    PolicyID = operatingQuotaEventDTO.ApplyEndorsement.PolicyID,
                    CoverageId = operatingQuotaEventDTO.ApplyEndorsement.CoverageId,
                    IsSeriousOffer = operatingQuotaEventDTO.ApplyEndorsement.IsSeriousOffer,
                    IsConsortium = operatingQuotaEventDTO.ApplyEndorsement.IsConsortium,

                },
                Policy_Init_Date = operatingQuotaEventDTO.Policy_Init_Date,
                Policy_End_Date = operatingQuotaEventDTO.Policy_End_Date,
                Cov_Init_Date = operatingQuotaEventDTO.Cov_Init_Date,
                Cov_End_Date = operatingQuotaEventDTO.Cov_End_Date,
                OperatingQuotaEventID = operatingQuotaEventDTO.OperatingQuotaEventID,
                OperatingQuotaEventType = operatingQuotaEventDTO.OperatingQuotaEventType,
                IdentificationId = operatingQuotaEventDTO.IdentificationId,
                LineBusinessID = operatingQuotaEventDTO.LineBusinessID,
                Payload = operatingQuotaEventDTO.payload,
                IssueDate = operatingQuotaEventDTO.IssueDate,
            };
        }
        internal static IMapper CreateMapRiskConsortiumIntegration()
        {
            var config = MapperCache.GetMapper<RiskConsortiumDTO, RiskConsortiumDTO>(cfg =>
            {
                cfg.CreateMap<RiskConsortiumDTO, RiskConsortiumDTO>();
            });
            return config;
        }

        #endregion

        internal static List<ExchangeRate> CreateExchangeRates(BusinessCollection businessObjects)
        {
            List<ExchangeRate> exchangeRates = new List<ExchangeRate>();

            foreach (COMMEN.ExchangeRate entityExchangeRate in businessObjects)
            {
                exchangeRates.Add(CreateExchangeRate(entityExchangeRate));
            }
            return exchangeRates;
        }

        internal static ExchangeRate CreateExchangeRate(COMMEN.ExchangeRate entityExchangeRate)
        {
            if (entityExchangeRate == null)
            {
                return null;
            }
            return new ExchangeRate
            {
                Currency = new Currency
                {
                    Id = entityExchangeRate.CurrencyCode
                },
                SellAmount = entityExchangeRate.SellAmount,
                BuyAmount = Convert.ToDecimal(entityExchangeRate.BuyAmount)
            };
        }

        internal static IMapper CreateMapOperatingQuotaByOperatingQuota()
        {
            var config = MapperCache.GetMapper<OperatingQuotaEventDTO, OperatingQuotaEvent>(cfg =>
            {
                cfg.CreateMap<OperatingQuotaEvent, OperatingQuotaEventDTO>();
                cfg.ValidateInlineMaps = false;
            });
            return config;
        }

        internal static OperatingQuotaEvent ToModel(this OperatingQuotaEventDTO operatingQuotaEventDTO)
        {
            var config = CreateMapOperatingQuotaByOperatingQuota();
            return config.Map<OperatingQuotaEventDTO, OperatingQuotaEvent>(operatingQuotaEventDTO);
        }

        internal static List<OperatingQuotaEvent> ToModels(this List<OperatingQuotaEventDTO> operatingQuotaEventDTOs)
        {
            var config = CreateMapOperatingQuotaByOperatingQuota();
            return config.Map<List<OperatingQuotaEventDTO>, List<OperatingQuotaEvent>>(operatingQuotaEventDTOs);
        }

        internal static IMapper CreateMapReinsuranceOperatingQuotaEvent()
        {
            var config = MapperCache.GetMapper<ReinsuranceOperatingQuotaEventDTO, ReinsuranceOperatingQuotaEvent>(cfg =>
            {
                cfg.CreateMap<ReinsuranceOperatingQuotaEventDTO, ReinsuranceOperatingQuotaEvent>();
            });
            return config;
        }

        internal static ReinsuranceOperatingQuotaEvent ToModel(this ReinsuranceOperatingQuotaEventDTO reinsuranceOperatingQuotaEventDTO)
        {
            var config = CreateMapReinsuranceOperatingQuotaEvent();
            return config.Map<ReinsuranceOperatingQuotaEventDTO, ReinsuranceOperatingQuotaEvent>(reinsuranceOperatingQuotaEventDTO);
        }

        internal static IEnumerable<ReinsuranceOperatingQuotaEvent> ToModels(this IEnumerable<ReinsuranceOperatingQuotaEventDTO> reinsuranceOperatingQuotaEventDTOs)
        {
            return reinsuranceOperatingQuotaEventDTOs.Select(ToModel);
        }

        internal static Func<UPEN.OperatingQuotaEvent, OperatingQuotaEvent> CreateOperatingQuotaEventByUPENOperatingQuotaEvent()
        {
            return (UPEN.OperatingQuotaEvent entityOperatingQuotaEvent) =>
            {
                return new OperatingQuotaEvent
                {
                    OperatingQuotaEventID = entityOperatingQuotaEvent.OperatingQuotaEventCode,
                    ApplyReinsurance = JsonHelper.DeserializeJson<ApplyReinsurance>(entityOperatingQuotaEvent.Payload)
                };
            };
        }

        internal static IMapper CreateMapRiskConsortium()
        {
            var config = MapperCache.GetMapper<BusinessCollection, RiskConsortium>(cfg =>
            {
                cfg.CreateMap<ISSEN.RiskConsortium, RiskConsortium>();
            });
            return config;
        }

        internal static RiskConsortium CreateRiskConsortium(this ISSEN.RiskConsortium entityRiskConsortium)
        {
            var config = CreateMapRiskConsortium();
            return config.Map<ISSEN.RiskConsortium, RiskConsortium>(entityRiskConsortium);
        }

        internal static List<RiskConsortium> CreateRiskConsortiums(this BusinessCollection entityRiskConsortiums)
        {
            var config = CreateMapRiskConsortium();
            return config.Map<BusinessCollection, List<RiskConsortium>>(entityRiskConsortiums);
        }
    }
}