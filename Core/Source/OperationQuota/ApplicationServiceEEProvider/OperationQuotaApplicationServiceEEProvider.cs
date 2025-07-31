using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.OperationQuotaServices.DTOs;
using Sistran.Core.Application.OperationQuotaServices.DTOs.Consortium;
using Sistran.Core.Application.OperationQuotaServices.DTOs.EconomicGroup;
using Sistran.Core.Application.OperationQuotaServices.DTOs.OperationQuota;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Assemblers;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Business;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.DAOs;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Enums;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Models.OperationQuota;
using Sistran.Core.Application.Utilities.Managers;

using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sistran.Core.Application.OperationQuotaServices.EEProvider
{
    public class OperationQuotaApplicationServiceEEProvider : IOperationQuotaService
    {
        static object obj = new object();
        /// <summary>
        /// Consulta el cupo y cumulo de Individual
        /// </summary>
        /// <param name="IndividualId"></param>
        /// <returns></returns>
        public OperatingQuotaEventDTO GetOperatingQuotaEventByIndividualIdByLineBusinessId(int individualId, int lineBusinessId)
        {
            try
            {
                OperatingQuotaEventDTO operatingQuotaEventDTO = new OperatingQuotaEventDTO();
                OperationQuotaBusiness operationQuotaBusiness = new OperationQuotaBusiness();
                OperationQuotaDAO operationQuotaDAO = new OperationQuotaDAO();
                DeclineInsuredDTO declineInsuredDTO = DTOAssembler.CreateDeclineInsured(operationQuotaDAO.GetDeclineDate(individualId));
                operatingQuotaEventDTO = DTOAssembler.CreateOperatingQuotaEvent(operationQuotaBusiness.GetOperationQuotaByIndividualIdByTransactionId(individualId, (int)EnumMovement.TOTAL, lineBusinessId));
                if (declineInsuredDTO != null)
                {
                    operatingQuotaEventDTO.declineInsured = declineInsuredDTO;
                }
                return operatingQuotaEventDTO;
            }
            catch (Exception)
            {
                throw new BusinessException(Resources.Resources.ErrorGetCupoCumulo);
            }
        }

        /// <summary>
        /// Inserta Evento Asignar Cupo Operativo 
        /// </summary>
        /// <param name="operatingQuotaEventDTO"></param>
        /// <returns></returns>
        public List<OperatingQuotaEventDTO> InsertOperatingQuotaEvent(List<OperatingQuotaEventDTO> operatingQuotaEventDTOs)
        {
            try
            {
                OperationQuotaDAO operationQuotaDAO = new OperationQuotaDAO();
                List<OperatingQuotaEvent> operatingQuotaEvents = new List<OperatingQuotaEvent>();
                operatingQuotaEvents = ModelAssembler.CreateOperatingQuotaEventsDTOs(operatingQuotaEventDTOs);

                int operatingQuotaEventID = 0;
                lock (obj)
                {
                    Parameter parameter = DelegateService.commonServiceCore.GetParameterByDescription("OPERATING_QUOTA_EVENT");
                    operatingQuotaEventID = Convert.ToInt32(parameter.NumberParameter+1);
                    parameter.NumberParameter += operatingQuotaEvents.Count + 1;
                    DelegateService.commonServiceCore.UpdateParameter(parameter);
                }

                Parallel.ForEach(operatingQuotaEvents, Utilities.Helper.ParallelHelper.DebugParallelFor(), operatingQuotaEvent =>
                {
                    List<Task> agentTask = new List<Task>();
                    agentTask.Add(Task.Run(() =>
                    {
                        lock (obj)
                        {
                            operatingQuotaEventID += 1;
                            operatingQuotaEvent.OperatingQuotaEventID = operatingQuotaEventID;
                        }
                    }));
                    Task.WaitAll(agentTask.ToArray());

                    if (operatingQuotaEvent.OperatingQuotaEventID >= 0)
                    {
                        if (operatingQuotaEvent.OperatingQuotaEventType == (int)(EnumEventOperationQuota.ASSIGN_INDIVIDUAL_OPERATION_QUOTA))
                        {
                            operationQuotaDAO.InsertOperatingQuotaEvent(operatingQuotaEvent);
                        }
                        else if (operatingQuotaEvent.OperatingQuotaEventType == (int)(EnumEventOperationQuota.APPLY_ENDORSEMENT))
                        {
                            operationQuotaDAO.InsertOperatingQuotaEventEndorsement(operatingQuotaEvent);
                        }
                    }
                });
                return DTOAssembler.CreateOperatingQuotaEvents(operatingQuotaEvents);
            }
            catch (Exception ex)
            {
                throw new BusinessException(Resources.Resources.ErrorCreateCupoIndividual, ex);
            }

        }

        /// <summary>
        /// GetCumulusCoveragesByIndividual
        /// </summary>
        /// <param name="filterOperationQuotaDTO"></param>
        /// <returns></returns>
        public List<OperatingQuotaEventDTO> GetCumulusCoveragesByIndividual(FilterOperationQuotaDTO filterOperationQuotaDTO, bool validatePriorityRetention = false)
        {
            try
            {
                OperationQuotaDAO operationQuotaDAO = new OperationQuotaDAO();
                ConsortiumDAO consortiumDAO = new ConsortiumDAO();
                EconomicGroupDAO economicGroupDAO = new EconomicGroupDAO();
                List<OperatingQuotaEventDTO> operatingQuotaEventDTOs = new List<OperatingQuotaEventDTO>();
                List<OperatingQuotaEventDTO> operatingQuotaEventResultDTOs = new List<OperatingQuotaEventDTO>();
                List<ExchangeRate> exchangeRates = GetExchangeRates();
                List<int> idParticipants = new List<int>();


                EconomicGroupDTO economicGroupDTO = new EconomicGroupDTO();
                List<EconomicGroupDetailDTO> economicGroupDetailDTOs = new List<EconomicGroupDetailDTO>();
                bool economicGroupEnabled = false;
                economicGroupDTO = DelegateService.uniquePersonIntegrationService.GetEconomicGroupById(filterOperationQuotaDTO.IndividualId).ToDTO();
                economicGroupDetailDTOs = DelegateService.uniquePersonIntegrationService.GetEconomicGroupDetailByIndividual(filterOperationQuotaDTO.IndividualId).ToDTOs()
                                                                                                                                                                 .ToList()
                                                                                                                                                                 .FindAll(x => x.Enabled == true);

                if (validatePriorityRetention)
                {
                    List<RiskConsortiumDTO> riskConsortiumDTOs = new List<RiskConsortiumDTO>();
                    riskConsortiumDTOs = consortiumDAO.GetRiskConsortium().ToDTOs().ToList();
                    idParticipants = riskConsortiumDTOs.FindAll(x => x.ConsortiumId == filterOperationQuotaDTO.IndividualId && x.IndividualId != 0).Select(y => y.IndividualId).Distinct().ToList();

                    if (idParticipants.Count > 0)
                    {
                        List<int> economicGroupIdParticipants = new List<int>();
                        List<int> economicGroupIds = new List<int>();

                        foreach (int id in idParticipants)
                        {
                            List<EconomicGroupDetailDTO> participantDetails = new List<EconomicGroupDetailDTO>();
                            participantDetails = DelegateService.uniquePersonIntegrationService.GetEconomicGroupDetailByIndividual(id).ToDTOs()
                                                                                                                                      .ToList()
                                                                                                                                      .FindAll(x => x.Enabled == true);


                            economicGroupIds.AddRange(participantDetails.Select(x => x.EconomicGroupId).ToList());
                        }

                        economicGroupIds.Distinct().ToList();

                        if (economicGroupIds.Count > 0)
                        {
                            foreach (int id in economicGroupIds)
                            {
                                bool enabled = DelegateService.uniquePersonIntegrationService.GetEconomicGroupById(id).Enabled;

                                if (enabled)
                                {
                                    economicGroupIdParticipants.AddRange(DelegateService.uniquePersonIntegrationService.GetEconomicGroupDetailById(id).FindAll(x => x.Enabled == true)
                                                                                                                                                      .Select(y => y.IndividualId)
                                                                                                                                                      .ToList());
                                }
                            }
                        }

                        idParticipants.AddRange(economicGroupIdParticipants);
                        idParticipants.Distinct();
                    }
                }


                if (economicGroupDetailDTOs.Count > 0)
                {
                    int economicGroupId = economicGroupDetailDTOs.OrderByDescending(x => x.EconomicGroupId).FirstOrDefault().EconomicGroupId;
                    economicGroupEnabled = DelegateService.uniquePersonIntegrationService.GetEconomicGroupById(economicGroupId).Enabled;

                    if (economicGroupEnabled)
                    {
                        idParticipants = DelegateService.uniquePersonIntegrationService.GetEconomicGroupDetailById(economicGroupId).FindAll(x => x.Enabled == true)
                                                                                                                                                .Select(y => y.IndividualId)
                                                                                                                                                .ToList();

                        operatingQuotaEventDTOs = operationQuotaDAO.GetCumulusCoveragesByIndividual
                                                                   (DTOAssembler.CreateFilterOperationQuotaDTO(filterOperationQuotaDTO), idParticipants)
                                                                   .Select(DTOAssembler.CreateOperatingQuotaEvent)
                                                                   .Select(ValidateCurrencyApplyReinsurance(exchangeRates, filterOperationQuotaDTO.DateCumulus)).ToList();


                        operatingQuotaEventDTOs = operatingQuotaEventDTOs.Select(y =>
                        {
                            if (y.IdentificationId != filterOperationQuotaDTO.IndividualId)
                            {
                                y.ApplyReinsurance.EconomicGroupId = economicGroupId;
                                y.ApplyReinsurance.ConsortiumId = 0;
                            }
                            return y;
                        }).ToList();
                    }
                    else
                    {
                        operatingQuotaEventDTOs = operationQuotaDAO.GetCumulusCoveragesByIndividual
                                                                   (DTOAssembler.CreateFilterOperationQuotaDTO(filterOperationQuotaDTO), idParticipants)
                                                                   .Select(DTOAssembler.CreateOperatingQuotaEvent)
                                                                   .Select(ValidateCurrencyApplyReinsurance(exchangeRates, filterOperationQuotaDTO.DateCumulus)).ToList();
                    }
                }
                else if (economicGroupDTO.EconomicGroupId > 0)
                {

                    economicGroupEnabled = economicGroupDTO.Enabled;

                    if (economicGroupEnabled)
                    {

                        idParticipants = DelegateService.uniquePersonIntegrationService.GetEconomicGroupDetailById(economicGroupDTO.EconomicGroupId).FindAll(x => x.Enabled == true)
                                                                                                                                                                 .Select(y => y.IndividualId)
                                                                                                                                                                 .ToList();

                        operatingQuotaEventDTOs = operationQuotaDAO.GetCumulusCoveragesByIndividual
                                                                   (DTOAssembler.CreateFilterOperationQuotaDTO(filterOperationQuotaDTO), idParticipants)
                                                                   .Select(DTOAssembler.CreateOperatingQuotaEvent)
                                                                   .Select(ValidateCurrencyApplyReinsurance(exchangeRates, filterOperationQuotaDTO.DateCumulus)).ToList();

                    }
                    else
                    {
                        filterOperationQuotaDTO.IndividualId = 0;
                        operatingQuotaEventDTOs = operationQuotaDAO.GetCumulusCoveragesByIndividual
                                                                   (DTOAssembler.CreateFilterOperationQuotaDTO(filterOperationQuotaDTO), idParticipants)
                                                                   .Select(DTOAssembler.CreateOperatingQuotaEvent)
                                                                   .Select(ValidateCurrencyApplyReinsurance(exchangeRates, filterOperationQuotaDTO.DateCumulus)).ToList();

                    }
                }
                else
                {
                    operatingQuotaEventDTOs = operationQuotaDAO.GetCumulusCoveragesByIndividual
                                          (DTOAssembler.CreateFilterOperationQuotaDTO(filterOperationQuotaDTO), idParticipants)
                                          .Select(DTOAssembler.CreateOperatingQuotaEvent)
                                          .Select(ValidateCurrencyApplyReinsurance(exchangeRates, filterOperationQuotaDTO.DateCumulus)).ToList();
                }

                if (filterOperationQuotaDTO.IsFuture == true)
                {
                    List<OperatingQuotaEventDTO> groupedOperatingQuotaEventDTOs = new List<OperatingQuotaEventDTO>();
                    groupedOperatingQuotaEventDTOs = operatingQuotaEventDTOs.GroupBy(x => new { x.ApplyReinsurance.DocumentNum, x.ApplyReinsurance.CoverageId })
                                                                        .Select(oqe => new OperatingQuotaEventDTO
                                                                        {
                                                                            ApplyReinsurance = new ApplyReinsuranceDTO
                                                                            {
                                                                                DocumentNum = oqe.FirstOrDefault().ApplyReinsurance.DocumentNum,
                                                                                EndorsementType = oqe.Max(x => x.ApplyReinsurance.EndorsementType),
                                                                                EndorsementId = oqe.Max(x => x.ApplyReinsurance.EndorsementId),
                                                                                CoverageId = oqe.FirstOrDefault().ApplyReinsurance.CoverageId
                                                                            }
                                                                        }).ToList();

                    groupedOperatingQuotaEventDTOs.Select(x =>
                    {
                        x.Cov_End_Date = operatingQuotaEventDTOs.Find(y => y.ApplyReinsurance.CoverageId == x.ApplyReinsurance.CoverageId &&
                                                                           y.ApplyReinsurance.DocumentNum == x.ApplyReinsurance.DocumentNum &&
                                                                           y.ApplyReinsurance.EndorsementId == x.ApplyReinsurance.EndorsementId &&
                                                                           y.ApplyReinsurance.EndorsementType == x.ApplyReinsurance.EndorsementType).Cov_End_Date;

                        return x;
                    }).ToList();

                    operatingQuotaEventResultDTOs = operatingQuotaEventDTOs.Select(DiscardOperatingQuotaEventDTO(groupedOperatingQuotaEventDTOs, filterOperationQuotaDTO.DateCumulus)).ToList();
                    operatingQuotaEventResultDTOs.RemoveAll(x => x.IdentificationId == 0);
                }
                else
                {
                    operatingQuotaEventResultDTOs = operatingQuotaEventDTOs;
                }

                return operatingQuotaEventResultDTOs;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, ""), ex);
            }
        }

        private Func<OperatingQuotaEventDTO, OperatingQuotaEventDTO> DiscardOperatingQuotaEventDTO(List<OperatingQuotaEventDTO> operatingQuotaEventDTOs, DateTime dateCumulus)
        {
            return (OperatingQuotaEventDTO operatingQuotaEventDTO) =>
            {

                OperatingQuotaEventDTO operatingQuotaEventResultDTO = new OperatingQuotaEventDTO();

                operatingQuotaEventResultDTO = operatingQuotaEventDTOs.Find(x => x.ApplyReinsurance.DocumentNum == operatingQuotaEventDTO.ApplyReinsurance.DocumentNum &&
                                                                            x.ApplyReinsurance.CoverageId == operatingQuotaEventDTO.ApplyReinsurance.CoverageId);


                if (operatingQuotaEventResultDTO.Cov_End_Date < dateCumulus)
                {
                    operatingQuotaEventResultDTO.Cov_End_Date = dateCumulus;
                }

                if (operatingQuotaEventDTO.ApplyReinsurance.DocumentNum == operatingQuotaEventResultDTO.ApplyReinsurance.DocumentNum &&
                   operatingQuotaEventDTO.ApplyReinsurance.CoverageId == operatingQuotaEventResultDTO.ApplyReinsurance.CoverageId &&
                   operatingQuotaEventDTO.Cov_End_Date < operatingQuotaEventResultDTO.Cov_End_Date)
                {
                    operatingQuotaEventResultDTO = new OperatingQuotaEventDTO();
                }
                else
                {
                    operatingQuotaEventResultDTO = operatingQuotaEventDTO;
                }

                return operatingQuotaEventResultDTO;
            };
        }


        private Func<OperatingQuotaEventDTO, OperatingQuotaEventDTO> ValidateCurrencyApplyReinsurance(List<ExchangeRate> exchangeRates, DateTime dateCumulus)
        {
            return (OperatingQuotaEventDTO operatingQuotaEventDTO) =>
            {
                if (operatingQuotaEventDTO.ApplyReinsurance.CurrencyType != (int)EnumExchangeRateCurrency.CURRENCY_PESOS)
                {
                    ExchangeRate exchangeRate = new ExchangeRate();
                    if (exchangeRates.FindAll(x => x?.Currency.Id == operatingQuotaEventDTO.ApplyReinsurance.CurrencyType && x?.RateDate == dateCumulus).Count > 0)
                    {
                        exchangeRate = exchangeRates.FindAll(x => x?.Currency.Id == operatingQuotaEventDTO.ApplyReinsurance.CurrencyType && x?.RateDate == dateCumulus)
                                                    .OrderByDescending(y => y.RateDate)
                                                    .FirstOrDefault();
                    }
                    else
                    {
                        exchangeRate = exchangeRates.FindAll(x => x?.Currency.Id == operatingQuotaEventDTO.ApplyReinsurance.CurrencyType && x?.RateDate <= dateCumulus)
                                                    .OrderByDescending(y => y.RateDate)
                                                    .FirstOrDefault();
                    }

                    operatingQuotaEventDTO.ApplyReinsurance.ContractCoverage.Select(CalculateContractCoverageAmount(exchangeRate)).ToList();
                }

                return operatingQuotaEventDTO;
            };
        }

        private Func<ContractCoverageDTO, ContractCoverageDTO> CalculateContractCoverageAmount(ExchangeRate exchangeRate)
        {
            return (ContractCoverageDTO contractCoverage) =>
            {
                contractCoverage.Amount *= exchangeRate.BuyAmount;
                contractCoverage.Premium *= exchangeRate.BuyAmount;

                return contractCoverage;
            };
        }

        private List<ExchangeRate> GetExchangeRates()
        {
            try
            {
                return DelegateService.commonServiceCore.GetExchangeRates();
            }
            catch (Exception)
            {
                return new List<ExchangeRate>();
            }
        }

        /// <summary>
        /// Consulta la cantidad de decimales por Producto
        /// </summary>
        /// <param name="productId">The product identifier.</param>
        /// <param name="currencyId">The currency identifier.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public int GetDecimalByProductIdCurrencyId(int productId, short currencyId)
        {
            try
            {
                OperationQuotaBusiness operationQuotaBusiness = new OperationQuotaBusiness();
                return OperationQuotaBusiness.GetDecimalByProductIdCurrencyId(productId, currencyId);
            }
            catch (Exception)
            {
                throw new BusinessException(Resources.Resources.ErrorCreateCupoIndividual);
            }
        }

        /// <summary>
        /// Valida si se encuentra el afianzado en un Grupo Economico ,Consorcio o Individual
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public TypeSecureDTO GetSecureType(int individualId, int lineBusinessId)
        {
            try
            {
                OperationQuotaBusiness operationQuotaBusiness = new OperationQuotaBusiness();
                return DTOAssembler.CreateSecureType(operationQuotaBusiness.GetSecureType(individualId, lineBusinessId));
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Valida si se encuentra el afianzado con cupo
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public bool GetOperatingQuotaEventByIndividualId(int individualId)
        {
            try
            {
                OperationQuotaBusiness operationQuotaBusiness = new OperationQuotaBusiness();
                return operationQuotaBusiness.GetOperatingQuotaEventByIndividualId(individualId);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
