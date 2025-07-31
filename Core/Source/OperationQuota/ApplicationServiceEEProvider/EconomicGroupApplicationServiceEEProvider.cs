using Sistran.Core.Application.OperationQuotaServices.DTOs.EconomicGroup;
using Sistran.Core.Application.OperationQuotaServices.DTOs.OperationQuota;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Assemblers;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Business;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.DAOs;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Enums;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Models.EconomicGroup;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Models.OperationQuota;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CU = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Core.Application.OperationQuotaServices.EEProvider
{
    public class EconomicGroupApplicationServiceEEProvider : IEconomicGroupService
    {
        /// <summary>
        /// Inserta el Grupo Economico por Event
        /// </summary>
        /// <param name="economicGroupEventDTOs"></param>
        /// <returns></returns>
        public EconomicGroupEventDTO CreateEconomicGroupEvent(EconomicGroupEventDTO economicGroupEventDTO)
        {
            try
            {
                EconomicGroupDAO economicGroupDAO = new EconomicGroupDAO();
                EconomicGroupEvent economicGroupEvent = new EconomicGroupEvent();
                List<EconomicGroupEvent> economicGroupEvents = economicGroupDAO.GetExistingGroupEconomicEventByeconomicGroupId(economicGroupEventDTO.EconomicGroupID);
                economicGroupEvents.RemoveAll(x => x.EconomicGroupEventType == (int)Enums.EnumEventEconomicGroup.DISABLED_INDIVIDUAL_TO_ECONOMIC_GROUP);
                economicGroupEventDTO.EconomicGroupEventID = economicGroupDAO.GetEconomicGroupEventId();

                economicGroupEventDTO.EconomicGroupEventID = economicGroupEventDTO.EconomicGroupEventID + 1;

                economicGroupEvent = ModelAssembler.CreateEconomicGroupEvent(economicGroupEventDTO);
                return DTOAssembler.CreateEconomicGroupEvent(economicGroupDAO.CreateEconomicGroupEvent(economicGroupEvent));

            }
            catch (Exception)
            {
                throw new BusinessException(Resources.Resources.ErrorCreateCupoIndividual);
            }
        }

        /// <summary>
        /// Asigna los integrantes deun Grupo Economico
        /// </summary>
        /// <param name="economicGroupEventDTOs"></param>
        /// <returns></returns>
        public List<EconomicGroupEventDTO> AssigendIndividualToEconomicGroupEvent(List<EconomicGroupEventDTO> economicGroupEventDTOs)
        {
            try
            {
                EconomicGroupDAO economicGroupDAO = new EconomicGroupDAO();
                EconomicGroupEvent economicGroup = new EconomicGroupEvent();
                List<EconomicGroupEvent> economicGroupEvents = new List<EconomicGroupEvent>();
                economicGroupEvents = ModelAssembler.CreateEconomicGroupEvents(economicGroupEventDTOs);

                foreach (EconomicGroupEvent economicGroupEvent in economicGroupEvents)
                {
                    EconomicGroupEvent validateParticipants = economicGroupDAO.GetExistingParticipantGroupEconomicEventByIndividualId(economicGroupEvent.IndividualId);
                    economicGroup.EconomicGroupEventId = economicGroupDAO.GetEconomicGroupEventId();
                    economicGroupEvent.EconomicGroupEventId = economicGroup.EconomicGroupEventId + 1;
                    if (economicGroupEvent.EconomicGroupEventType == (int)EnumEventEconomicGroup.ENABLED_INDIVIDUAL_TO_ECONOMIC_GROUP && validateParticipants.IndividualId == 0)
                    {
                        economicGroupDAO.AssigendIndividualToEconomicGroupEvent(economicGroupEvent);
                    }
                    else if (economicGroupEvent.EconomicGroupEventType == (int)EnumEventEconomicGroup.DISABLED_INDIVIDUAL_TO_ECONOMIC_GROUP)
                    {
                        economicGroupDAO.AssigendIndividualToEconomicGroupEvent(economicGroupEvent);
                    }
                }
                return DTOAssembler.CreateEconomicGroupEvents(economicGroupEvents);
            }
            catch (Exception)
            {
                throw new BusinessException(Resources.Resources.ErrorCreateCupoIndividual);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="individualId"></param>
        /// <param name="lineBusinessId"></param>
        /// <returns></returns>
        public OperatingQuotaEventDTO GetCumuloCupoEconomicGroupByIndividualIdByLineBusinessId(int individualId, int lineBusinessId)
        {
            try
            {
                List<OperatingQuotaEvent> operationQuotaEvents = new List<OperatingQuotaEvent>();
                OperatingQuotaEvent operatingQuotaEvent = new OperatingQuotaEvent();
                OperationQuotaBusiness operationQuotaBusiness = new OperationQuotaBusiness();
                EconomicGroupDAO economicGroupDAO = new EconomicGroupDAO();
                OperationQuotaDAO operationQuotaDAO = new OperationQuotaDAO();

                EconomicGroupEvent economicGroupEvent = economicGroupDAO.GetExistingParticipantGroupEconomicEventByIndividualId(individualId);
                DeclineInsured decline = operationQuotaDAO.GetDeclineDate(individualId);
                if (decline != null)
                {
                    operatingQuotaEvent.declineInsured = decline;
                }

                if (economicGroupEvent?.IndividualId > 0)
                {
                    //Se consultan los participantes del grupo economico
                    List<EconomicGroupEvent> economicGroupEvents = economicGroupDAO.GetExistingGroupEconomicEventByeconomicGroupId(economicGroupEvent.EconomicGroupId);

                    //Se extrae el grupo economico
                    EconomicGroupEvent economicGroup = economicGroupEvents.First(x => x.IndividualId == 0);
                    if (economicGroup.EconomicGroupOperatingQuota.Enable)
                    {
                        //se extraen los participantes del grupo Economico excepto el Individuo
                        IEnumerable<EconomicGroupEvent> economicGroupParners = economicGroupEvents.Where(x => x.IndividualId != 0 && x.EconomicGroupPartners.Enable);

                        List<OperatingQuotaEvent> operatingQuotasParners = new List<OperatingQuotaEvent>();
                        foreach (EconomicGroupEvent economicGroupParner in economicGroupParners)
                        {
                            operatingQuotasParners.Add(operationQuotaBusiness.GetOperationQuotaByIndividualIdByTransactionId(economicGroupParner.IndividualId, (int)EnumMovement.TOTAL, lineBusinessId));
                        }



                        operatingQuotaEvent.EconomicGroupEvent = economicGroup;
                        operatingQuotaEvent.IndividualOperatingQuota = new IndividualOperatingQuota
                        {
                            ValueOpQuotaAMT = Convert.ToDecimal(operatingQuotasParners.Sum(x => x.IndividualOperatingQuota?.ValueOpQuotaAMT) ?? 0),
                            EndDateOpQuota = operatingQuotasParners.Max(x => x.IndividualOperatingQuota?.EndDateOpQuota) ?? new DateTime()
                        };
                        operatingQuotaEvent.ApplyEndorsement = new ApplyEndorsement()
                        {
                            AmountCoverage = Convert.ToDecimal(operatingQuotasParners.Sum(x => x.ApplyEndorsement?.AmountCoverage) ?? 0)
                        };
                    }
                }

                return DTOAssembler.CreateOperatingQuotaEvent(operatingQuotaEvent);



                /*

                if (economicGroupEvent?.IndividualId > 0)
                {
                    List<EconomicGroupEvent> economicGroupEvents = economicGroupDAO.GetExistingGroupEconomicEventByeconomicGroupId(economicGroupEvent.EconomicGroupId);

                    if (economicGroupEvents.Count > 0)
                    {
                        economicGroupEvents = economicGroupEvents.OrderBy(x => x.IndividualId).ThenBy(x => x.IssueDate).ToList();

                        foreach (EconomicGroupEvent GroupEvent in economicGroupEvents)
                        {
                            if (GroupEvent.IndividualId != 0)
                            {
                                if (GroupEvent.IndividualId != individualId1)
                                {
                                    EconomicGroupEvent EconomicGroupEventActive = economicGroupDAO.GetExistingParticipantGroupEconomicEventByIndividualId(GroupEvent.IndividualId);
                                    if (EconomicGroupEventActive.IndividualId != 0 && EconomicGroupEventActive.EconomicGroupPartners.Enabled)
                                    {
                                        events.Add(EconomicGroupEventActive);
                                        individualId1 = GroupEvent.IndividualId;
                                    }
                                }
                            }
                            else
                            {
                                events.Add(GroupEvent);
                            }
                        }
                    }

                    foreach (var economicGroup in events)
                    {
                        OperatingQuotaEvent operatingQuotaEvent2 = new OperatingQuotaEvent
                        {
                            declineInsured = operatingQuotaEvent.declineInsured
                        };

                        if (economicGroup.IndividualId != 0)
                        {
                            operatingQuotaEvent2 = operationQuotaBusiness.GetOperationQuotaByIndividualIdByTransactionId(economicGroup.IndividualId, (int)EnumMovement.TOTAL, lineBusinessId);
                            if (operatingQuotaEvent2.IndividualOperatingQuota.EndDateOpQuota < DateTime.Now)
                            {
                                operatingQuotaEvent2.IndividualOperatingQuota.ValueOpQuotaAMT = 0;
                            }
                        }
                        else
                        {
                            operatingQuotaEvent2.EconomicGroupEvent = economicGroup;
                        }

                        operationQuotaEvents.Add(operatingQuotaEvent2);
                    }

                    operatingQuotaEvent.IndividualOperatingQuota = new IndividualOperatingQuota()
                    {
                        ValueOpQuotaAMT = Convert.ToDecimal(operationQuotaEvents.Sum(x => x.IndividualOperatingQuota?.ValueOpQuotaAMT)),
                        EndDateOpQuota = operationQuotaEvents.Max(x => x.IndividualOperatingQuota?.EndDateOpQuota) ?? new DateTime()
                    };
                    operatingQuotaEvent.ApplyEndorsement = new ApplyEndorsement()
                    {
                        AmountCoverage = Convert.ToDecimal(operationQuotaEvents.Sum(x => x.ApplyEndorsement?.AmountCoverage))// validar - individual
                    };
                    operatingQuotaEvent.EconomicGroupEvent = operationQuotaEvents[0].EconomicGroupEvent;
                }
                */

                return DTOAssembler.CreateOperatingQuotaEvent(operatingQuotaEvent);
            }
            catch (Exception ex)
            {
                throw new BusinessException(/*Resources.Resources.ErrorCreateCupoIndividual*/ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene el detalle del grupo economico por id
        /// </summary>
        /// <param name="economicGroupId"></param>
        /// <returns></returns>
        public List<EconomicGroupEventDTO> GetExistingGroupEconomicEventByeconomicGroupId(int economicGroupId)
        {
            EconomicGroupDAO economicGroupDAO = new EconomicGroupDAO();
            List<EconomicGroupEventDTO> economicGroupEventDTOs = new List<EconomicGroupEventDTO>();
            economicGroupEventDTOs = DTOAssembler.CreateEconomicGroupEvents(economicGroupDAO.GetExistingGroupEconomicEventByeconomicGroupId(economicGroupId));
            return economicGroupEventDTOs;
        }
    }
}
