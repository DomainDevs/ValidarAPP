using System.Collections.Generic;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.DAOs;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Models.OperationQuota;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Models.Consortium;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Enums;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Assemblers;
using Sistran.Core.Integration.OperationQuotaServices.DTOs.OperationQuota;
using Sistran.Core.Integration.OperationQuotaServices;
using System.Linq;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.Utilities.Managers;
using System;
using Sistran.Core.Integration.OperationQuotaServices.DTOs.Consortium;
using Sistran.Core.Application.CommonService.Models;
using System.Diagnostics;

namespace Sistran.Core.Application.OperationQuotaServices.EEProvider.IntegrationProviders
{
    public class OperatingQuotaIntegrationServiceEEProvider : IOperationQuotaIntegrationService
    {
        static object obj = new object();
        public OperatingQuotaEventDTO GetOperatingQuotaEventByIndividualIdByLineBusinessId(int individualId, int lineBusinessId)
        {
            OperatingQuotaEventDTO operatingQuotaEventDTO = new OperatingQuotaEventDTO();
            return operatingQuotaEventDTO = DTOAssembler.CreateOperatingQuotaEventDTOs(DelegateService.operationQuotaService.GetOperatingQuotaEventByIndividualIdByLineBusinessId(individualId, lineBusinessId));
        }

        public List<OperatingQuotaEventDTO> InsertApplyEndorsementOperatingQuotaEvent(List<OperatingQuotaEventDTO> operatingQuotaEventDTOs)
        {
            try
            {
                OperationQuotaDAO operationQuotaDAO = new OperationQuotaDAO();
                ConsortiumDAO consortiumDAO = new ConsortiumDAO();
                List<OperatingQuotaEventDTO> operatingQuotaEventsDTO = new List<OperatingQuotaEventDTO>();
                List<OperatingQuotaEvent> operatingQuotaEvents = new List<OperatingQuotaEvent>();
      

                List<RiskConsortium> riskConsortiums = new List<RiskConsortium>();
                riskConsortiums = consortiumDAO.GetRiskConsortiumbyPolicy(operatingQuotaEventDTOs[0].ApplyEndorsement.Endorsement);
                if (riskConsortiums.Count > 0)
                {
            
                    foreach (OperatingQuotaEventDTO item in operatingQuotaEventDTOs)
                    {
                        foreach (RiskConsortium item1 in riskConsortiums)
                        {
                            if (item1.IndividualId != 0)
                            {
                                operatingQuotaEventsDTO.Add(ModelAssembler.CreateAplyEndorsementRiskConsortium(item, item1));
                            }
                        }
                        operatingQuotaEventsDTO.Add(ModelAssembler.CreateApllyEndorsement(item));
                    }

                    operatingQuotaEvents = ModelAssembler.CreateOperatingQuotaEventsDTOs(operatingQuotaEventsDTO);
                }
                else
                {
                    //VALIDA INDIVIDUAL
                    operatingQuotaEvents = ModelAssembler.CreateOperatingQuotaEventsDTOs(operatingQuotaEventDTOs);
                }
                int? eventid = 0;
                lock (obj)
                {
                    Parameter parameter = DelegateService.commonServiceCore.GetParameterByDescription("OPERATING_QUOTA_EVENT");
                    eventid = parameter.NumberParameter + 1;
                    parameter.NumberParameter += operatingQuotaEvents.Count + 1;

                    DelegateService.commonServiceCore.UpdateParameter(parameter);
                    foreach (OperatingQuotaEvent operatingQuotaEvent in operatingQuotaEvents)
                    {
                        OperatingQuotaEvent operatingQuota = new OperatingQuotaEvent();

                        operatingQuota.OperatingQuotaEventID = (int)eventid;



                        if (operatingQuota.OperatingQuotaEventID >= 0)
                        {
                            operatingQuotaEvent.OperatingQuotaEventID = operatingQuota.OperatingQuotaEventID;

                            if (operatingQuotaEvent.OperatingQuotaEventType != (int)EnumEventOperationQuota.ASSIGN_INDIVIDUAL_OPERATION_QUOTA)
                            {
                                operationQuotaDAO.InsertOperatingQuotaEventEndorsement(operatingQuotaEvent);
                            }
                        }
                        eventid += 1;
                    }
                }//);

                return DTOAssembler.CreateOperatingQuotaEventsIntegration(operatingQuotaEvents);

            }
            catch (System.Exception ex)
            {
                throw new BusinessException(Resources.Resources.ErrorCreateCupoIndividual, ex);
            }
        }

        public bool InsertConsortium(List<RiskConsortiumDTO> consortiums)
        {
            ConsortiumDAO consortiumDAO = new ConsortiumDAO();
            RiskConsortium cons = new RiskConsortium();

            foreach (RiskConsortiumDTO riskConsortium in consortiums)
            {
                consortiumDAO.InsertConsotiumPolicy(riskConsortium);
            };

            return true;
        }

        public List<RiskConsortiumDTO> GetRiskConsortiumbyPolicy(int endorsementid)
        {
            ConsortiumDAO consortiumDAO = new ConsortiumDAO();
            return ModelAssembler.CreateRiskConsortiumDTOs(consortiumDAO.GetRiskConsortiumbyPolicy(endorsementid));
        }
        public bool CreateOperatingQuotaEvents(List<OperatingQuotaEventDTO> operatingQuotaEventDTOs)
        {
            try
            {
                OperationQuotaDAO operationQuotaDAO = new OperationQuotaDAO();
                List<OperatingQuotaEventDTO> operatingQuotaEventResultDTOs = new List<OperatingQuotaEventDTO>();
                ConsortiumDAO consortiumDAO = new ConsortiumDAO();
                List<RiskConsortiumDTO> riskConsortiumDTOs = new List<RiskConsortiumDTO>();
                riskConsortiumDTOs = consortiumDAO.GetRiskConsortium().ToDTOs().ToIntegrationDTOs().ToList();

                if (operatingQuotaEventDTOs.Count > 0)
                {
                    operatingQuotaEventDTOs.Select(InsertOperatingQuotaEventReinsurance(riskConsortiumDTOs)).ToList().ForEach(x =>
                        operatingQuotaEventResultDTOs.AddRange(x)
                    );

                    return InsertReinsuranceOperatingQuotaEvent(operationQuotaDAO.InsertOperatingQuotaEventReinsurance(operatingQuotaEventResultDTOs.ToDTOs().ToModels()).ToDTOs().ToIntegrationDTOs());

                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Resources.ErrorCreateCupoIndividual), ex);
            }
        }

        public bool MigrateReinsuranceCumulus(List<OperatingQuotaEventDTO> operatingQuotaEventDTOs)
        {
            try
            {
                OperationQuotaDAO operationQuotaDAO = new OperationQuotaDAO();
                ConsortiumDAO consortiumDAO = new ConsortiumDAO();
                List<OperatingQuotaEventDTO> operatingQuotaEventResultDTOs = new List<OperatingQuotaEventDTO>();

                List<RiskConsortiumDTO> riskConsortiumDTOs = new List<RiskConsortiumDTO>();
                riskConsortiumDTOs = consortiumDAO.GetRiskConsortium().ToDTOs().ToIntegrationDTOs().ToList();


                if (operatingQuotaEventDTOs.Count > 0)
                {
                    operatingQuotaEventDTOs.Select(InsertOperatingQuotaEventReinsurance(riskConsortiumDTOs)).ToList().ForEach(x =>
                        operatingQuotaEventResultDTOs.AddRange(x)
                    );

                    EventLog.WriteEntry("Application", "Se ha iniciado la inserción de datos para la migración de cúmulos de reaseguros ", EventLogEntryType.Information);

                    return InsertReinsuranceOperatingQuotaEvent(operationQuotaDAO.InsertOperatingQuotaEventReinsurance(operatingQuotaEventResultDTOs.ToDTOs().ToModels()).ToDTOs().ToIntegrationDTOs());

                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Resources.ErrorCreateCupoIndividual), ex);
            }
        }

        Func<OperatingQuotaEventDTO, List<OperatingQuotaEventDTO>> InsertOperatingQuotaEventReinsurance(List<RiskConsortiumDTO> riskConsortiumDTOs)
        {
            try
            {
                return (OperatingQuotaEventDTO operatingQuotaEventDTO) =>
                {
                    EconomicGroupDAO economicGroupDAO = new EconomicGroupDAO();
                    ConsortiumDAO consortiumDAO = new ConsortiumDAO();
                    List<RiskConsortiumDTO> riskConsortiumResultDTOs = new List<RiskConsortiumDTO>();
                    List<OperatingQuotaEventDTO> operatingQuotaEventDTOs = new List<OperatingQuotaEventDTO>();

                    riskConsortiumResultDTOs = GetConsortiumExistsByConsortiumId(riskConsortiumDTOs, operatingQuotaEventDTO);

                    if ((riskConsortiumResultDTOs.Count > 0))
                    {
                        List<OperatingQuotaEventDTO> operatingQuotaEventDTOsByConsortiumEvents = new List<OperatingQuotaEventDTO>();
                        riskConsortiumResultDTOs.Select(CreateIntegrationOperatingQuotaEventDTOByConsortiumEventDTO(operatingQuotaEventDTO)).ToList().ForEach(x =>
                            operatingQuotaEventDTOsByConsortiumEvents.Add(x)
                        );

                        operatingQuotaEventDTOs.AddRange(operatingQuotaEventDTOsByConsortiumEvents);

                    }

                    else if ((riskConsortiumResultDTOs.Count == 0))
                    {
                        operatingQuotaEventDTOs.Add(operatingQuotaEventDTO);
                    }

                    return operatingQuotaEventDTOs;

                };
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Resources.ErrorCreateCupoIndividual), ex);
            }
        }

        Func<RiskConsortiumDTO, OperatingQuotaEventDTO> CreateIntegrationOperatingQuotaEventDTOByConsortiumEventDTO(OperatingQuotaEventDTO operatingQuotaEventDTO)
        {
            return (RiskConsortiumDTO riskConsortiumDTO) =>
            {
                return new OperatingQuotaEventDTO
                {
                    ApplyReinsurance = new ApplyReinsuranceDTO
                    {
                        PolicyID = operatingQuotaEventDTO.ApplyReinsurance.PolicyID,
                        EndorsementId = operatingQuotaEventDTO.ApplyReinsurance.EndorsementId,
                        EndorsementType = operatingQuotaEventDTO.ApplyReinsurance.EndorsementType,
                        CoverageId = operatingQuotaEventDTO.ApplyReinsurance.CoverageId,
                        IndividualId = operatingQuotaEventDTO.ApplyReinsurance.IndividualId,
                        ConsortiumId = riskConsortiumDTO.ConsortiumId,
                        EconomicGroupId = 0,
                        DocumentNum = operatingQuotaEventDTO.ApplyReinsurance.DocumentNum,
                        CurrencyType = operatingQuotaEventDTO.ApplyReinsurance.CurrencyType,
                        CurrencyTypeDesc = operatingQuotaEventDTO.ApplyReinsurance.CurrencyTypeDesc,
                        BranchId = operatingQuotaEventDTO.ApplyReinsurance.BranchId,
                        PrefixId = operatingQuotaEventDTO.ApplyReinsurance.PrefixId,
                        ParticipationPercentage = riskConsortiumDTO.PjePart,
                        ContractCoverage = operatingQuotaEventDTO.ApplyReinsurance.ContractCoverage,
                        RiskId = operatingQuotaEventDTO.ApplyReinsurance.RiskId

                    },
                    IssueDate = operatingQuotaEventDTO.IssueDate,
                    IdentificationId = riskConsortiumDTO.IndividualId == 0 ? riskConsortiumDTO.ConsortiumId : riskConsortiumDTO.IndividualId,
                    LineBusinessID = operatingQuotaEventDTO.LineBusinessID,
                    SubLineBusinessID = operatingQuotaEventDTO.SubLineBusinessID,
                    Policy_Init_Date = operatingQuotaEventDTO.Policy_Init_Date,
                    Policy_End_Date = operatingQuotaEventDTO.Policy_End_Date,
                    Cov_Init_Date = operatingQuotaEventDTO.Cov_Init_Date,
                    Cov_End_Date = operatingQuotaEventDTO.Cov_End_Date,
                    OperatingQuotaEventType = operatingQuotaEventDTO.OperatingQuotaEventType,
                    PrefixCd = operatingQuotaEventDTO.PrefixCd
                };
            };
        }

        private List<RiskConsortiumDTO> GetConsortiumExistsByConsortiumId(List<RiskConsortiumDTO> riskConsortiumDTOs, OperatingQuotaEventDTO operatingQuotaEventDTO)
        {
            List<RiskConsortiumDTO> riskConsortiumResultDTOs = new List<RiskConsortiumDTO>();
            riskConsortiumResultDTOs = riskConsortiumDTOs.FindAll(x => x.ConsortiumId == operatingQuotaEventDTO.IdentificationId &&
                                                                       x.PolicyId == operatingQuotaEventDTO.ApplyReinsurance.PolicyID &&
                                                                       x.EndorsementId == operatingQuotaEventDTO.ApplyReinsurance.EndorsementId &&
                                                                       x.RiskId == operatingQuotaEventDTO.ApplyReinsurance.RiskId);

            return riskConsortiumResultDTOs;
        }

        Func<OperatingQuotaEventDTO, ReinsuranceOperatingQuotaEventDTO> CreateReinsuranceOperatingQuotaEventDTOByOperatingQuotaEventDTO()
        {
            try
            {
                return (OperatingQuotaEventDTO operatingQuotaEventDTO) =>
                {
                    return new ReinsuranceOperatingQuotaEventDTO
                    {
                        OperatingQuotaEventCd = operatingQuotaEventDTO.OperatingQuotaEventID,
                        PolicyId = operatingQuotaEventDTO.ApplyReinsurance.PolicyID,
                        EndorsementId = operatingQuotaEventDTO.ApplyReinsurance.EndorsementId,
                        CoverageId = operatingQuotaEventDTO.ApplyReinsurance.CoverageId,
                        CanUpdateValidity = true
                    };
                };
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Resources.ErrorCreateCupoIndividual), ex);
            }
        }

        public bool InsertReinsuranceOperatingQuotaEvent(List<OperatingQuotaEventDTO> operatingQuotaEventDTOs)
        {
            try
            {
                OperationQuotaDAO operationQuotaDAO = new OperationQuotaDAO();
                List<ReinsuranceOperatingQuotaEventDTO> reinsuranceOperatingQuotaEventsDTOs = new List<ReinsuranceOperatingQuotaEventDTO>();
                reinsuranceOperatingQuotaEventsDTOs.AddRange(operatingQuotaEventDTOs.Select(CreateReinsuranceOperatingQuotaEventDTOByOperatingQuotaEventDTO()));

                if (reinsuranceOperatingQuotaEventsDTOs.Count > 0)
                {
                    return operationQuotaDAO.InsertReinsuranceOperatingQuotaEvent(reinsuranceOperatingQuotaEventsDTOs.ToDTOs().ToModels().ToList()).Count > 0;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Resources.ErrorCreateCupoIndividual), ex);
            }
        }

    }
}
