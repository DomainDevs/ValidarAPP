using Sistran.Company.Application.Endorsement.CreditNoteApplicationService.DTOs;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using USERV = Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Company.Application.Base.Endorsement.CreditNoteApplicationService.EEProvider.Assemblers
{
    public class DTOAssembler
    {
        internal static EndorsementDTO CreateEndorsement(CompanyEndorsement companyEndorsement)
        {
            return new EndorsementDTO
            {
                Id = companyEndorsement.Id,
                Description = string.Format("{0} ({1})", companyEndorsement.EndorsementTypeDescription, companyEndorsement.Number),
                CurrentFrom = companyEndorsement.CurrentFrom,
                CurrentTo = companyEndorsement.CurrentTo,
                Days = CalculateDays(companyEndorsement)
            };
        }

        internal static List<EndorsementDTO> CreateEndorsements(List<CompanyEndorsement> companyEndorsements)
        {
            List<EndorsementDTO> endorsements = new List<EndorsementDTO>();

            foreach (var companyEndorsment in companyEndorsements)
            {
                endorsements.Add(CreateEndorsement(companyEndorsment));
            }
            return endorsements;
        }

        internal static RiskDTO CreateRisk(CompanyRisk companyRisk)
        {
            return new RiskDTO
            {
                Id = companyRisk.Id,
                Description = companyRisk.Description,
                Coverages = CreateCoverages(companyRisk.Coverages)
            };
        }

        internal static List<RiskDTO> CreateRisks(List<CompanyRisk> companyRisks)
        {
            List<RiskDTO> risks = new List<RiskDTO>();

            foreach (var companyRisk in companyRisks)
            {
                risks.Add(CreateRisk(companyRisk));
            }
            return risks;
        }

        internal static CoverageDTO CreateCoverage(CompanyCoverage companyCoverage)
        {
            return new CoverageDTO
            {
                Id = companyCoverage.Id,
                Description = companyCoverage.Description,
                PremiumAmount = companyCoverage.PremiumAmount
            };
        }

        internal static List<CoverageDTO> CreateCoverages(List<CompanyCoverage> companyCoverages)
        {
            List<CoverageDTO> coverages = new List<CoverageDTO>();

            foreach (var companyCoverage in companyCoverages)
            {
                coverages.Add(CreateCoverage(companyCoverage));
            }
            return coverages;
        }

        internal static int CalculateDays(CompanyEndorsement companyEndorsement)
        {
            TimeSpan t = companyEndorsement.CurrentTo - companyEndorsement.CurrentFrom;
            return (int)Math.Ceiling(t.TotalDays);
        }

        internal static EndorsementDTO CreateEndorsementDTO(USERV.Endorsement endorsement)
        {
            if (endorsement == null)
            {
                return null;
            }

            EndorsementDTO endorsementDTO = new EndorsementDTO()
            {
                CurrentFrom = endorsement.CurrentFrom,
                CurrentTo = endorsement.CurrentTo,
                EndorsementType = endorsement.EndorsementType,
                IdEndorsement = endorsement.Id,
                IsCurrent = endorsement.IsCurrent,
                Number = endorsement.Number,
                PolicyNumber = endorsement.PolicyId,
                TemporalId = endorsement.TemporalId
            };

            return endorsementDTO;
        }

    }
}