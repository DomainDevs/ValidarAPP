using Sistran.Company.Application.Transports.Endorsement.Adjustment.ApplicationServices.DTOs;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.Transports.TransportApplicationService.DTOs;
using System.Collections.Generic;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using System;

namespace Sistran.Company.Application.Transports.Endorsement.Adjustment.ApplicationServices.EEProvider.Assemblers
{
    public class DTOAssembler
    {

        public static List<TransportDTO> CreateAjusmentTransports(List<TransportDTO> transports)
        {
            if (transports == null)
            {
                return null;
            }

            List<TransportDTO> transportsDto = new List<TransportDTO>();
            foreach (
                var transport in transports)
            {
                TransportDTO transportDto = new TransportDTO();
                transportDto.Description = transport.Description;
                transportDto.RiskId = transport.RiskId;
                transportDto.CoverageGroupId = transport.CoverageGroupId;
                transportDto.DeclarationPeriodId = transport.DeclarationPeriodId;
                transportDto.BillingPeriodId = transport.BillingPeriodId;
                transportsDto.Add(transportDto);
            }

            return transportsDto;
        }

        public static AdjustmentDTO CreateInsuredObjects(List<InsuredObjectDTO> insuredObjectsDTO)
        {
            if (insuredObjectsDTO == null)
            {
                return null;
            }
            AdjustmentDTO adjustment = new AdjustmentDTO();
            List<InsuredObjectDTO> insuredObjects = new List<InsuredObjectDTO>();

            foreach (var insuredobject in insuredObjectsDTO)
            {
                insuredObjects.Add(insuredobject);
            }
            adjustment.InsuredObjects = insuredObjects;
            return adjustment;
        }

        public static AdjustmentDTO CreateEndorsements(List<EndorsementDTO> endorsements)
        {
            if (endorsements == null)
            {
                return null;
            }
            AdjustmentDTO adjustment = new AdjustmentDTO();

            List<EndorsementDTO> endorsementsdto = new List<EndorsementDTO>();
            foreach (var endorsement in endorsements)
            {
                endorsementsdto.Add(endorsement);
            }
            adjustment.Endorsements = endorsementsdto;
            return adjustment;
        }


        public static AdjustmentDTO CreateAjusmentsumary(CompanyPolicy policy)
        {
            if (policy == null)
            {
                return null;
            }
            return new AdjustmentDTO()
            {

                PolicyId = policy.Id,
                PrefixId = policy.Prefix.Id,
                BranchId = policy.Branch.Id,
                Sum = policy.Summary.AmountInsured,
                Premiun = policy.Summary.Premium,
                Expense = policy.Summary.Expenses,
                Surcharge = policy.Summary.Surcharges,
                Discount = policy.Summary.Discounts,
                Tax = policy.Summary.Taxes,
                TotalPremiun = policy.Summary.FullPremium,
                Days = policy.Endorsement.EndorsementDays,
                CurrentFrom = policy.Endorsement.CurrentFrom,
                CurrentTo = policy.Endorsement.CurrentTo,
                TemporalId = policy.Endorsement.TemporalId,
                Text = policy.Endorsement.Text.TextBody,
                Observation = policy.Endorsement.Text.Observations,
                RiskCount = policy.Summary.RiskCount,
                TicketNumber = Convert.ToInt32(policy.Endorsement.TicketNumber),
                TicketDate = Convert.ToDateTime(policy.Endorsement.TicketDate)

            };
        }

        public static AdjustmentDTO CreateAdjustmentDTO(EndorsementDTO endorsement)
        {
            return new AdjustmentDTO
            {
                CurrentFrom = endorsement.CurrentFrom,
                CurrentTo = endorsement.CurrentTo
            };
        }

        public static AdjustmentDTO CreateAdjustmentSummary(CompanyPolicy policy)
        {
            if (policy == null)
            {
                return null;
            }
            return new AdjustmentDTO()
            {

                PolicyId = policy.Id,
                PrefixId = policy.Prefix.Id,
                BranchId = policy.Branch.Id,
                Sum = policy.Summary.AmountInsured,
                Premiun = policy.Summary.Premium,
                Expense = policy.Summary.Expenses,
                Surcharge = policy.Summary.Surcharges,
                Discount = policy.Summary.Discounts,
                Tax = policy.Summary.Taxes,
                TotalPremiun = policy.Summary.FullPremium,
                Days = policy.Endorsement.EndorsementDays,
                CurrentFrom = policy.Endorsement.CurrentFrom,
                CurrentTo = policy.Endorsement.CurrentTo,
                TemporalId = policy.Endorsement.TemporalId,
                Text = policy.Endorsement.Text.TextBody,
                Observation = policy.Endorsement.Text.Observations,
                RiskCount = policy.Summary.RiskCount,

            };
        }

        
    }
}
