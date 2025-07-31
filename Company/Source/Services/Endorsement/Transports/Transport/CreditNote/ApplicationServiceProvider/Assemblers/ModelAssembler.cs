using AutoMapper;
using Sistran.Company.Application.Transports.Endorsement.CreditNote.ApplicationServices.DTOs;
using STDTO = Sistran.Company.Application.Transports.TransportApplicationService.DTOs;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Company.Application.Transports.TransportApplicationService.DTOs;

namespace Sistran.Company.Application.Transports.Transport.ApplicationServices.EEProvider.Assemblers
{
    class ModelAssembler
    {
        public static CompanyPolicy CreateCompanyPolicy(CreditNoteDTO creditNoteDTO)
        {
            CompanyText companyText = new CompanyText();
            companyText.TextBody = creditNoteDTO.Text;
            companyText.Observations = creditNoteDTO.Observation;

            CompanyEndorsement endorsement = new CompanyEndorsement();
            endorsement.PolicyId = creditNoteDTO.PolicyId;
            endorsement.EndorsementDays = creditNoteDTO.Days;
            endorsement.CurrentFrom = creditNoteDTO.validityDateFrom;
            endorsement.CurrentTo = creditNoteDTO.validityDateTo;
            endorsement.Text = companyText;
            endorsement.Id = creditNoteDTO.endorsementTypes.First().Id;
            endorsement.TemporalId = creditNoteDTO.TemporalId;
            endorsement.CreditNoteEndorsementType = creditNoteDTO.EndorsementType;
            endorsement.RiskId = creditNoteDTO.RiskId;
            endorsement.CoverageId = creditNoteDTO.CoverageId;
            return new CompanyPolicy
            {
                Endorsement = endorsement,
            };
        }

        internal static CompanyRisk CreateCompanyRisk(RiskDTO risk)
        {
            CompanyRisk companyRiskrisk = new CompanyRisk();
            var imapper = CreateMapCompanyRisk();
            companyRiskrisk = imapper.Map<RiskDTO, CompanyRisk>(risk);

            return companyRiskrisk;
        }

        internal static CompanyCoverage CreateCompanyCoverage(STDTO.CoverageDTO coverageDTO)
        {
            CompanyCoverage companyCoverage = new CompanyCoverage();
            companyCoverage.Description = coverageDTO.Description;
            companyCoverage.Id = coverageDTO.Id;
            companyCoverage.PremiumAmount = coverageDTO.PremiumAmount;
            companyCoverage.Rate = coverageDTO.Rate;
            return companyCoverage;
        }

        internal static CompanyCoverage CreateCompanyCoverage(EndorsementCoverageDTO coverageDTO)
        {
            CompanyCoverage companyCoverage = new CompanyCoverage();
            companyCoverage.Description = coverageDTO.Description;
            companyCoverage.Id = coverageDTO.Id;
            companyCoverage.PremiumAmount = coverageDTO.PremiumAmount;
            return companyCoverage;
        }


        #region automapper

        #region riesgo
        public static IMapper CreateMapCompanyRisk()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RiskDTO, CompanyRisk>();
            });
            return config.CreateMapper();
        }
        #endregion riesgo

        #endregion

        public static Dictionary<string, object> CreateFormValues(CreditNoteDTO creditNote)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add("RiskId", creditNote.RiskId);
            result.Add("InsuredObjectId", creditNote.InsuranceObjectId);
            result.Add("EndorsementType", creditNote.EndorsementController);
            return result;
        }

    }
}
