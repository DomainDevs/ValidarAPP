using Sistran.Core.Application.OperationQuotaServices.DTOs.Consortium;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.DAOs;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Assemblers;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Business;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Models.Consortium;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Enums;
using Sistran.Core.Application.OperationQuotaServices.DTOs.OperationQuota;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.CommonService.Models;

namespace Sistran.Core.Application.OperationQuotaServices.EEProvider
{
    public class ConsortiumApplicationServiceEEProvider : IConsortiumService
    {
        object obj = new object();
        /// <summary>
        /// Consulta el Consorcio Cupos y Cumulos por ConsorcioId Y LineBusinessId
        /// </summary>
        /// <param name="ConsortiumId"></param>
        /// <returns></returns>
        public OperatingQuotaEventDTO GetCumuloCupoConsortiumEventByConsortiumIdByLineBusinessId(int consortiumId, int lineBusinessId, bool? endorsement,int Id)
        {
            try
            {
                ConsortiumBusiness consortiumBusiness = new ConsortiumBusiness();
                return DTOAssembler.CreateOperatingQuotaEvent(consortiumBusiness.GetCumuloCupoConsortiumEventByConsortiumIdByLineBusinessId(consortiumId, lineBusinessId, endorsement,Id));

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Consulta el Consorciado Cupos y Cumulos por individualId Y LineBusinessId
        /// </summary>
        /// <param name="individualId"></param>
        /// <param name="lineBusinessId"></param>
        /// <returns></returns>
        public List<OperatingQuotaEventDTO> GetCumulusQuotaConsortiumByIndividualIdByLineBusinessId(int individualId, int lineBusinessId)
        {
            try
            {
                ConsortiumBusiness consortiumBusiness = new ConsortiumBusiness();
                return DTOAssembler.CreateOperatingQuotaEvents(consortiumBusiness.GetCumulusQuotaConsortiumByIndividualIdByLineBusinessId(individualId, lineBusinessId));
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Inserta el Consorcium por Event
        /// </summary>
        /// <param name="consortiumEventDTOs"></param>
        /// <returns></returns>
        public ConsortiumEventDTO CreateConsortiumEvent(ConsortiumEventDTO consortiumEventDTO)
        {
            try
            {
                ConsortiumDAO consortiumDAO = new ConsortiumDAO();
                ConsortiumEvent consortiumEvent = new ConsortiumEvent();
                consortiumEvent.consortium = new Consortium();
                consortiumEvent.Consortiumpartners = new Consortiumpartners();
                List<Task> agentTask = new List<Task>();
                agentTask.Add(Task.Run(() =>
                {
                    lock (obj)
                    {
                        Parameter parameter = DelegateService.commonServiceCore.GetParameterByDescription("CONSORTIUM_EVENT");
                        if (parameter.NumberParameter >= 0)
                        {
                            parameter.NumberParameter += 1;
                            DelegateService.commonServiceCore.UpdateParameter(parameter);
                            consortiumEventDTO.ConsortiumEventID = (int)parameter.NumberParameter;
                        }
                    }
                    
                }));
                Task.WaitAll(agentTask.ToArray());

                if (consortiumEventDTO.ConsortiumEventID >= 0)
                {
                    List<ConsortiumEvent> consortiumExists = consortiumDAO.GetConsortiumExistsByConsortiumId(consortiumEventDTO.consortiumDTO.ConsotiumId);
                    foreach (ConsortiumEvent item in consortiumExists)
                    {
                        if (item.ConsortiumEventEventType == (int)EnumEventConsortium.CREATE_CONSORTIUM)
                        {
                            consortiumEvent = item;
                        }
                    }
                    if (consortiumExists.Count == 0)
                    {
                        // validar que el consorcio no exista
                        if (consortiumEventDTO.ConsortiumEventEventType == (int)EnumEventConsortium.CREATE_CONSORTIUM)
                        {
                            consortiumEvent = ModelAssembler.CreateConsortiumEvent(consortiumEventDTO);
                            return DTOAssembler.CreateConsortiumEvent(consortiumDAO.CreateConsortiumEvent(consortiumEvent));
                        }
                    }
                }
                return DTOAssembler.CreateConsortiumEvent(consortiumEvent);
            }
            catch (Exception ex)
            {
                throw new BusinessException(Resources.Resources.ErrorCreateCupoIndividual);
            }

        }
        /// <summary>
        /// Asigna los integrantes deun consorcio
        /// </summary>
        /// <param name="consortiumEventDTOs"></param>
        /// <returns></returns>
        public List<ConsortiumEventDTO> AssigendIndividualToConsotium(List<ConsortiumEventDTO> consortiumEventDTOs)
        {
            try
            {
                ConsortiumDAO consortiumDAO = new ConsortiumDAO();
                List<ConsortiumEvent> consortiumEvents = new List<ConsortiumEvent>();
                consortiumEvents = ModelAssembler.CreateConsortiumEvents(consortiumEventDTOs);

                foreach (ConsortiumEvent consortiumEvent in consortiumEvents)
                {
                    List<Task> agentTask = new List<Task>();
                    agentTask.Add(Task.Run(() =>
                    {
                        lock (obj)
                        {
                            Parameter parameter = DelegateService.commonServiceCore.GetParameterByDescription("CONSORTIUM_EVENT");
                            if (parameter.NumberParameter >= 0)
                            {
                                parameter.NumberParameter += 1;
                                DelegateService.commonServiceCore.UpdateParameter(parameter);
                                consortiumEvent.ConsortiumEventID = (int)parameter.NumberParameter;
                            }
                        }

                    }));
                    Task.WaitAll(agentTask.ToArray());
                    //Asignacion de Individual en Consortium
                    if (consortiumEvent.ConsortiumEventEventType == (int)EnumInicialEvent.INICIAL_EVENT)
                    {
                        consortiumEvent.ConsortiumEventEventType = (int)EnumEventConsortium.ASSIGN_INDIVIDUAL_TO_CONSORTIUM;
                        consortiumDAO.AssigendIndividualToConsotium(consortiumEvent);
                    }
                    else
                    {
                        //Modificaicon de Individual en Consortium
                        if (consortiumEvent.ConsortiumEventEventType == (int)EnumEventConsortium.ASSIGN_INDIVIDUAL_TO_CONSORTIUM || (consortiumEvent.ConsortiumEventEventType == (int)EnumEventConsortium.MODIFY_INDIVIDUAL_TO_CONSORTIUM && /*consortiumEvent.Consortiumpartners.Enabled &&*/ consortiumEvent.Consortiumpartners.ParticipationRate > 0))
                        {
                            consortiumEvent.ConsortiumEventEventType = (int)EnumEventConsortium.MODIFY_INDIVIDUAL_TO_CONSORTIUM;
                            consortiumDAO.AssigendIndividualToConsotium(consortiumEvent);
                        }
                        //Remove de Individual en Consortium
                        if (consortiumEvent.ConsortiumEventEventType == (int)EnumEventConsortium.DISABLED_INDIVIDUAL_TO_CONSORTIUM && !consortiumEvent.Consortiumpartners.Enabled && consortiumEvent.Consortiumpartners.ParticipationRate == 0)
                        {
                            consortiumEvent.ConsortiumEventEventType = (int)EnumEventConsortium.DISABLED_INDIVIDUAL_TO_CONSORTIUM;
                            consortiumEvent.Consortiumpartners.EndDate = DateTime.Now;
                            consortiumDAO.AssigendIndividualToConsotium(consortiumEvent);
                        }

                    }

                }
                return DTOAssembler.CreateConsortiumEvents(consortiumEvents);
            }
            catch (Exception)
            {
                throw new BusinessException(Resources.Resources.ErrorCreateCupoIndividual);

            }

        }

        /// <summary>
        /// Recupera los Consorciados de un consorcio
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public List<ConsortiumEventDTO> GetConsortiumEventByIndividualId(int individualId)
        {
            ConsortiumDAO consortiumDAO = new ConsortiumDAO();
            return DTOAssembler.CreateConsortiumEvents(consortiumDAO.GetParticipantConsortiumEventByConsortiumId(individualId));
        }

        /// <summary>
        /// Recupera la existencia de un individual como un consorcio
        /// </summary>
        /// <param name="consortiumId"></param>
        /// <returns></returns>
        public List<ConsortiumEventDTO> GetConsortiumExistsByConsortiumId(int consortiumId)
        {
            try
            {
                ConsortiumDAO consortiumDAO = new ConsortiumDAO();
                var consortiumEvents = consortiumDAO.GetConsortiumExistsByConsortiumId(consortiumId);
                return DTOAssembler.CreateConsortiumEvents(consortiumEvents);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Valida los Consorciados Por Fecha y el Cupo Disponible VS el valor de la emision
        /// </summary>
        /// <param name="consortiumId"></param>
        /// <param name="AmountInsured"></param>
        /// <param name="lineBusinessId"></param>
        /// <returns></returns>
        public List<OperatingQuotaEventDTO> GetValidityParticipantCupoInConsortium(int consortiumId, long AmountInsured, int lineBusinessId)
        {
            ConsortiumBusiness consortiumBusiness = new ConsortiumBusiness();
            return DTOAssembler.CreateOperatingQuotaEvents(consortiumBusiness.GetValidityParticipantCupoInConsortium(consortiumId, AmountInsured, lineBusinessId));
        }
    }
}
