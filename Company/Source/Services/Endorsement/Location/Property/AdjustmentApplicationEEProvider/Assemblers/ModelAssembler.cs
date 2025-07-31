using Sistran.Company.Application.AdjustmentApplicationService.DTO;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using PROP = Sistran.Company.Application.Location.PropertyServices.DTO;
using Sistran.Core.Application.UnderwritingServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.AdjustmentApplicationServiceEEProvider.Assemblers
{
    public class ModelAssembler
    {
        public static CompanyPolicy CreateCompanyPolicy(AdjustmentDTO adjustment)
        {
            CompanyText companyText = new CompanyText();
            companyText.TextBody = adjustment.Text;
            companyText.Observations = adjustment.Observation;
            CompanyEndorsement endorsement = new CompanyEndorsement();
            endorsement.PolicyId = adjustment.PolicyId;
            endorsement.EndorsementDays = adjustment.Days;
            endorsement.CurrentFrom = adjustment.CurrentFrom;
            endorsement.CurrentTo = adjustment.CurrentTo;
            endorsement.Text = companyText;
            endorsement.EndorsementType = EndorsementType.AdjustmentEndorsement;
            endorsement.Id = adjustment.EndorsementId;
            endorsement.TemporalId = adjustment.TemporalId;
            endorsement.TicketNumber = adjustment.TicketNumber;
            endorsement.TicketDate = adjustment.TicketDate;
            endorsement.RiskId = adjustment.RiskId;
            endorsement.InsuredObjectId = adjustment.InsuredObjectId;

            return new CompanyPolicy
            {
                Endorsement = endorsement,
                TicketDate = adjustment.TicketDate,
                TicketNumber = adjustment.TicketNumber,
                UserId = adjustment.UserId
            };

        }

        public static List<EndorsementDTO> ChangeEndorsementDTOs(List<PROP.EndorsementDTO> endorsementDTO)
        {
            List<EndorsementDTO> endorsements = new List<EndorsementDTO>();
            endorsementDTO.ForEach(X => endorsements.Add(ChangeEndorsementDTO(X)));
            return endorsements;
        }

        public static EndorsementDTO ChangeEndorsementDTO(PROP.EndorsementDTO endorsementDTO)
        {
            return new EndorsementDTO
            {
                CurrentFrom = endorsementDTO.CurrentFrom,
                CurrentTo = endorsementDTO.CurrentTo,
                EndorsementType = endorsementDTO.EndorsementType,
                IdEndorsement = endorsementDTO.IdEndorsement,
                IsCurrent = endorsementDTO.IsCurrent,
                Number = endorsementDTO.Number,
                PolicyNumber = endorsementDTO.PolicyNumber,
                TemporalId = endorsementDTO.TemporalId
            };
        }
        public static Dictionary<string, object> CreateFormValues(AdjustmentDTO declaration)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add("RiskId", declaration.RiskId);
            result.Add("InsuredObjectId", declaration.InsuredObjectId);
            result.Add("TicketNumber", declaration.TicketNumber);
            result.Add("TicketDate", declaration.TicketDate);
            return result;
        }
    }
}
