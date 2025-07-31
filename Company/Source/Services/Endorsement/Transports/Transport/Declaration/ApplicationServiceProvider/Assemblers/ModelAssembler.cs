using Sistran.Company.Application.Transports.Endorsement.Declaration.ApplicationServices.DTOs;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.Transports.Endorsement.Declaration.ApplicationServices.EEProvider.Assemblers
{
    public class ModelAssembler
    {
        public static CompanyPolicy CreateCompanyPolicy(DeclarationDTO declaration)
        {
            CompanyText companyText = new CompanyText();
            companyText.TextBody = declaration.Text;
            companyText.Observations = declaration.Observation;


            CompanyEndorsement endorsement = new CompanyEndorsement();
            endorsement.PolicyId = declaration.PolicyId;
            endorsement.EndorsementDays = declaration.Days;
            endorsement.CurrentFrom = declaration.CurrentFrom;
            endorsement.CurrentTo = declaration.CurrentTo;
            endorsement.Text = companyText;
            endorsement.EndorsementType = EndorsementType.DeclarationEndorsement;
            endorsement.Id = declaration.EndorsementId;
            endorsement.RiskId = declaration.RiskId;
            endorsement.InsuredObjectId = declaration.InsuranceObjectId;
            endorsement.DeclaredValue = declaration.DeclaredValue;
            endorsement.TemporalId = declaration.TemporalId == null ? 0 : (int)declaration.TemporalId;
            endorsement.TicketNumber = declaration.TicketNumber;
            endorsement.TicketDate = declaration.TicketDate;
            return new CompanyPolicy
            {
                CurrentFrom = declaration.CurrentFrom,
                CurrentTo = declaration.CurrentTo,
                Endorsement = endorsement
            };

        }

        public static Dictionary<string, object> CreateFormValues(DeclarationDTO declaration)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add("RiskId", declaration.RiskId);
            result.Add("InsuredObjectId", declaration.InsuranceObjectId);
            result.Add("DeclarationValue", declaration.DeclaredValue);
            result.Add("EndorsementType", declaration.EndorsementController);
            return result;
        }
    }
}
