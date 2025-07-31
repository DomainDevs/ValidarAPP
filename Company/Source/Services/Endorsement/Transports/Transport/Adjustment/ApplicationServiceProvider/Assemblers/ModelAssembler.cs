using Sistran.Company.Application.Transports.Endorsement.Adjustment.ApplicationServices.DTOs;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using System.Collections.Generic;

namespace Sistran.Company.Application.Transports.Endorsement.Adjustment.ApplicationServices.EEProvider.Assemblers
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
