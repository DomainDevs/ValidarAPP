using System;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Enums;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Models.OperationQuota;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Models.Consortium;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Models.EconomicGroup;
using UPEN = Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.Utilities.Helper;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using Sistran.Core.Integration.OperationQuotaServices.DTOs.Consortium;

namespace Sistran.Core.Application.OperationQuotaServices.EEProvider.Assemblers
{
    public static class EntityAssembler
    {
        #region OPERATIONQUOTAEVENT

        internal static Func<OperatingQuotaEvent, UPEN.OperatingQuotaEvent> CreateUPENOperatingQuotaEventByOperatingQuotaEvent()
        {
            return (OperatingQuotaEvent operatingQuotaEvent) =>
            {
                return new UPEN.OperatingQuotaEvent(operatingQuotaEvent.OperatingQuotaEventID)
                {
                    PolicyInitDate = operatingQuotaEvent.Policy_Init_Date,
                    PolicyEndDate = operatingQuotaEvent.Policy_End_Date,
                    CovInitDat = operatingQuotaEvent.Cov_Init_Date,
                    CovEndDa = operatingQuotaEvent.Cov_End_Date,
                    OperatingQuotaEventCode = operatingQuotaEvent.OperatingQuotaEventID,
                    OperatingQuotaTypeEvent = Convert.ToString(operatingQuotaEvent.OperatingQuotaEventType),
                    IdentificationId = operatingQuotaEvent.IdentificationId,
                    IssueDate = operatingQuotaEvent.IssueDate,
                    LineBusinessCode = operatingQuotaEvent.LineBusinessID,
                    SubLineBusinessCode = Convert.ToInt32(operatingQuotaEvent.SubLineBusinessID),
                    Payload = JsonHelper.SerializeObjectToJson(operatingQuotaEvent.ApplyReinsurance),
                    PrefixCode = operatingQuotaEvent.PrefixCd
                };
            };
        }

        internal static Func<ReinsuranceOperatingQuotaEvent, UPEN.ReinsOperatingQuotaEvent> CreateReinsOperatingQuotaEventByUPENReinsOperatingQuotaEvent()
        {
            return (ReinsuranceOperatingQuotaEvent reinsuranceOperatingQuotaEvent) =>
            {
                return new UPEN.ReinsOperatingQuotaEvent()
                {
                    ReinsOperatingQuotaEventId = reinsuranceOperatingQuotaEvent.ReinsOperatingQuotaEventId,
                    OperatingQuotaEventCode = reinsuranceOperatingQuotaEvent.OperatingQuotaEventCd,
                    PolicyId = reinsuranceOperatingQuotaEvent.PolicyId,
                    EndorsementId = reinsuranceOperatingQuotaEvent.EndorsementId,
                    CoverageId = reinsuranceOperatingQuotaEvent.CoverageId,
                    CanUpdateValidity = reinsuranceOperatingQuotaEvent.CanUpdateValidity
                };
            };
        }

        internal static UPEN.OperatingQuotaEvent CreateOperatingQuotaEvent(OperatingQuotaEvent operatingQuotaEvent)
        {
            if (operatingQuotaEvent == null)
            {
                return null;
            }
            if (operatingQuotaEvent.OperatingQuotaEventType != (int)EnumEventOperationQuota.ASSIGN_INDIVIDUAL_OPERATION_QUOTA)
            {
                return new UPEN.OperatingQuotaEvent(operatingQuotaEvent.OperatingQuotaEventID)
                {
                    PolicyInitDate = operatingQuotaEvent.Policy_Init_Date,
                    PolicyEndDate = operatingQuotaEvent.Policy_End_Date,
                    CovInitDat = operatingQuotaEvent.Cov_Init_Date,
                    CovEndDa = operatingQuotaEvent.Cov_End_Date,
                    OperatingQuotaEventCode = operatingQuotaEvent.OperatingQuotaEventID,
                    OperatingQuotaTypeEvent = Convert.ToString(operatingQuotaEvent.OperatingQuotaEventType),
                    IdentificationId = operatingQuotaEvent.IdentificationId,
                    IssueDate = operatingQuotaEvent.IssueDate,
                    LineBusinessCode = operatingQuotaEvent.LineBusinessID,
                    SubLineBusinessCode = Convert.ToInt32(operatingQuotaEvent.SubLineBusinessID),
                    Payload = operatingQuotaEvent.Payload
                };
            }
            else
            {
                return new UPEN.OperatingQuotaEvent(operatingQuotaEvent.OperatingQuotaEventID)
                {
                    OperatingQuotaEventCode = operatingQuotaEvent.OperatingQuotaEventID,
                    OperatingQuotaTypeEvent = Convert.ToString(operatingQuotaEvent.OperatingQuotaEventType),
                    IdentificationId = operatingQuotaEvent.IdentificationId,
                    IssueDate = operatingQuotaEvent.IssueDate,
                    LineBusinessCode = operatingQuotaEvent.LineBusinessID,
                    SubLineBusinessCode = Convert.ToInt32(operatingQuotaEvent.SubLineBusinessID),
                    Payload = operatingQuotaEvent.Payload
                };
            }
        }
        #endregion

        #region CONSORTIUMEVENT
        internal static UPEN.ConsortiumEvent CreateConsortiumEvent(ConsortiumEvent consortiumEvent)
        {
            if (consortiumEvent == null)
            {
                return null;
            }
            return new UPEN.ConsortiumEvent(consortiumEvent.ConsortiumEventID)
            {
                ConsortiumEventCode = consortiumEvent.ConsortiumEventID,
                ConsortiumEventTypeEven = Convert.ToString(consortiumEvent.ConsortiumEventEventType),
                ConsortiumId = consortiumEvent.IndividualConsortiumID,
                IndividualId = consortiumEvent.IndividualId,
                IssueDate = consortiumEvent.IssueDate,
                Payload = consortiumEvent.payload
            };
        }
        #endregion

        #region ECONOMICGROUP
        internal static UPEN.EconomicGroupEvent CreateEconomicGroupEvent(EconomicGroupEvent economicGroupEvent)
        {
            if (economicGroupEvent == null)
            {
                return null;
            }
            return new UPEN.EconomicGroupEvent(economicGroupEvent.EconomicGroupEventId)
            {
                EconomicGroupEventCode = economicGroupEvent.EconomicGroupEventId,
                EconomicGroupEventType = Convert.ToString(economicGroupEvent.EconomicGroupEventType),
                EconomicGroupId = economicGroupEvent.EconomicGroupId,
                IndividualId = economicGroupEvent.IndividualId,
                IssueDate = economicGroupEvent.IssueDate,
                Payload = economicGroupEvent.payload
            };

        }
        #endregion
        internal static ISSEN.RiskConsortium CreateConsortiumPolicyEvent(RiskConsortiumDTO riskConsortium)
        {
            if (riskConsortium == null)
            {
                return null;
            }
            return new ISSEN.RiskConsortium(riskConsortium.PolicyId, riskConsortium.EndorsementId, riskConsortium.RiskId, riskConsortium.ConsortiumId, riskConsortium.IndividualId)
            {
                PolicyId = riskConsortium.PolicyId,
                EndorsementId = riskConsortium.EndorsementId,
                RiskId = riskConsortium.RiskId,
                ConsortiumId = riskConsortium.ConsortiumId,
                IndividualId = riskConsortium.IndividualId,
                PjePart = riskConsortium.PjePart
            };
        }
    }
}
