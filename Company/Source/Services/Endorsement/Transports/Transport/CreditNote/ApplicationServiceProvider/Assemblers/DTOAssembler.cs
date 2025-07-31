using COMTRAEN = Sistran.Company.Application.Transports.Endorsement.CreditNote.ApplicationServices.DTOs;
using System;
using System.Collections.Generic;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Transports.TransportApplicationService.DTOs;
using Sistran.Company.Application.Transports.Endorsement.CreditNote.BusinessServices.Models;

namespace Sistran.Company.Application.Transports.Transport.ApplicationServices.EEProvider.Assemblers
{
    public class DTOAssembler
    {
        /// <Listado-Encabezado-Polizas>
        /// 
        /// </summary>
        /// <param name="companyEndorsements"></param>
        /// <returns></returns>

        public static List<COMTRAEN.CreditNoteDTO> CreatePolicysmodel(List<CompanyEndorsement> Endorsements)
        {
            List<COMTRAEN.CreditNoteDTO> policyDTOs = new List<COMTRAEN.CreditNoteDTO>();
            foreach (var endorsement in Endorsements)
            {
                policyDTOs.Add(Policys(endorsement));
            }
            return policyDTOs;
        }

        public static COMTRAEN.CreditNoteDTO Policys(CompanyEndorsement endorsement)
        {
            if (endorsement == null)
            {
                return null;
            }

            return new COMTRAEN.CreditNoteDTO
            {
                validityDateFrom = endorsement.CurrentFrom,
                validityDateTo = endorsement.CurrentTo
            };
        }

        //public static CreditNoteDTO CreateCreditNoteTransposrt(List<TransportDTO> transports)
        //{
        //    if (transports == null)
        //    {
        //        return null;
        //    }
        //    CreditNoteDTO creditNote = new CreditNoteDTO();
        //    List<RiskDTO> risksdto = new List<RiskDTO>();
        //    foreach (var transport in transports)
        //    {
        //        RiskDTO riskdto = new RiskDTO();
        //        riskdto.InsuranceObjectId = transport.Description;
        //        riskdto.RiskId = transport.RiskId;
        //        risksdto.Add(riskdto);
        //    }
        //    creditNote.Risk = risksdto;
        //    return creditNote;
        //}

        public static COMTRAEN.EndorsementTypeDTO CreateEndorsementType(CompanyEndorsementType endorsementType)
        {
            if (endorsementType == null)
            {
                return null;
            }

            return new COMTRAEN.EndorsementTypeDTO
            {
                Id = endorsementType.Id,
                Description = endorsementType.Description,
                EndorsementId= endorsementType.EndorsementId,
                HasQuotation = endorsementType.HasQuotation,
                CurrentFrom= endorsementType.CurrentFrom,
                CurrentTo= endorsementType.CurrentTo,
               // RiskId= endorsementType.RiskId,
                //CoverageId= endorsementType.CoverageId
            };
        }

        public static List<COMTRAEN.EndorsementTypeDTO> GetEndorsmenteTypesHasQuotation(List<CompanyEndorsementType> endorsementTypes)
        {

            List<COMTRAEN.EndorsementTypeDTO> endorsementDTOs = new List<COMTRAEN.EndorsementTypeDTO>();

            foreach (var endorsement in endorsementTypes)
            {
                endorsementDTOs.Add(CreateEndorsementType(endorsement));
            }

            return endorsementDTOs;

        }


        public static List<COMTRAEN.EndorsementTypeDTO> GetAvaibleEndorsementsByPolicyId(List<CompanyEndorsementType> companyEndorsementTypes)
        {
            List<COMTRAEN.EndorsementTypeDTO> endorsementDTOs = new List<COMTRAEN.EndorsementTypeDTO>();

            foreach (var companyEndorsementType in companyEndorsementTypes)
            {
                endorsementDTOs.Add(CreateEndorsementType(companyEndorsementType));
            }

            return endorsementDTOs;

        }

        internal static COMTRAEN.CreditNoteDTO CreateCreditNoteSummary(CompanyPolicy policy)
        {
            if (policy == null)
            {
                return null;
            }
            return new COMTRAEN.CreditNoteDTO()
            {
                PolicyId = policy.Id,
                PrefixId = policy.Prefix.Id,
                BranchId = policy.Branch.Id,
                Summary = CreateSummaryDTO(policy.Summary),
                TemporalId = policy.Endorsement.TemporalId,
            };
        }

        private static COMTRAEN.SummaryDTO CreateSummaryDTO(CompanySummary summary)
        {
            COMTRAEN.SummaryDTO summaryDTO = new COMTRAEN.SummaryDTO();
            summaryDTO.Discounts = summary.Discounts;
            summaryDTO.Expenses = summary.Expenses;
            summaryDTO.Premium = summary.Premium;
            summaryDTO.AmountInsured = summary.AmountInsured;
            summaryDTO.Surcharges = summary.Surcharges;
            summaryDTO.Taxes = summary.Taxes;
            summaryDTO.FullPremium = summary.FullPremium;
            summaryDTO.RiskCount = summary.RiskCount;
            return summaryDTO;
        }

        public static COMTRAEN.RiskDTO CreateCreditNoteTransports(List<TransportDTO> transports)
        {
            if (transports == null)
            {
                return null;
            }
            COMTRAEN.RiskDTO TransporByEndorsement = new COMTRAEN.RiskDTO();
            List<TransportDTO> transportsDto = new List<TransportDTO>();

            foreach (var transport in transports)
            {
                TransportDTO transportDto = new TransportDTO();
                transportDto.Id = transport.Id;
                transportDto.Description = transport.Description;
                transportDto.RiskId = transport.RiskId;
                transportDto.CoverageGroupId = transport.CoverageGroupId;
                transportDto.DeclarationPeriodId = transport.DeclarationPeriodId;
                transportDto.BillingPeriodId = transport.BillingPeriodId;
                transportsDto.Add(transportDto);
            }
            TransporByEndorsement.Transports = transportsDto;
            return TransporByEndorsement;
        }



       public static COMTRAEN.CreditNoteDTO CreateCoverageCreditNote(List<TransportApplicationService.DTOs.CoverageDTO> companyCoveragesDTO)
        {
            if (companyCoveragesDTO == null)
            {
                return null;
            }
            COMTRAEN.CreditNoteDTO CoverageByRisk = new COMTRAEN.CreditNoteDTO();
            List<CoverageDTO> companyCoverages = new List<TransportApplicationService.DTOs.CoverageDTO>();
            foreach (var coverage in companyCoveragesDTO)
            {
                companyCoverages.Add(coverage);
            }
            CoverageByRisk.CoverageDTOs = companyCoverages;
            return CoverageByRisk;
        }

        public static COMTRAEN.CreditNoteDTO CreateCreditNotesumary(CompanyPolicy policy)
        {
            COMTRAEN.SummaryDTO sumary = new COMTRAEN.SummaryDTO();
            if (policy == null)
            {
                return null;
            }
            COMTRAEN.CreditNoteDTO creditNote = new COMTRAEN.CreditNoteDTO();
            creditNote.PolicyId = policy.Id;
            creditNote.PrefixId = policy.Prefix.Id;
            creditNote.BranchId = policy.Branch.Id;
            creditNote.Days = policy.Endorsement.EndorsementDays;
            creditNote.Text = policy.Endorsement.Text.TextBody;
            creditNote.Observation = policy.Endorsement.Text.Observations;
            sumary.AmountInsured = policy.Summary.AmountInsured;
            sumary.Premium = policy.Summary.Premium;
            sumary.Expenses = policy.Summary.Expenses;
            sumary.Surcharges = policy.Summary.Surcharges;
            sumary.Discounts = policy.Summary.Discounts;
            sumary.Taxes = policy.Summary.Taxes;
            sumary.FullPremium = policy.Summary.FullPremium;
            creditNote.validityDateFrom = policy.Endorsement.CurrentFrom;
            creditNote.validityDateTo = policy.Endorsement.CurrentTo;
            creditNote.TemporalId = policy.Endorsement.TemporalId;
            creditNote.EndorsementType = policy.Endorsement.CreditNoteEndorsementType;
            creditNote.RiskId = (int)policy.Endorsement.RiskId;
            creditNote.CoverageId = policy.Endorsement.CoverageId;
            creditNote.PremiumToReturn = policy.Endorsement.PremiumToReturn;
            creditNote.Summary = sumary;
            creditNote.TicketDate = Convert.ToDateTime(policy.Endorsement.TicketDate);
            creditNote.TicketNumber = Convert.ToInt32(policy.Endorsement.TicketNumber);
            creditNote.RiskId = policy.Endorsement.RiskId;
            return creditNote;



        }


        internal static COMTRAEN.EndorsementDTO CreateEndorsement(CompanyEndorsement companyEndorsement)
        {
            return new COMTRAEN.EndorsementDTO
            {
                Id = companyEndorsement.Id,
                Description = string.Format("{0} ({1})", companyEndorsement.EndorsementTypeDescription, companyEndorsement.Number),
                CurrentFrom = companyEndorsement.CurrentFrom,
                CurrentTo = companyEndorsement.CurrentTo,
                Days = CalculateDays(companyEndorsement)
            };
        }

        internal static List<COMTRAEN.EndorsementDTO> CreateEndorsements(List<CompanyEndorsement> companyEndorsements)
        {
            List<COMTRAEN.EndorsementDTO> endorsements = new List<COMTRAEN.EndorsementDTO>();

            foreach (var companyEndorsment in companyEndorsements)
            {
                endorsements.Add(CreateEndorsement(companyEndorsment));
            }
            return endorsements;
        }

        internal static COMTRAEN.EndorsementRiskDTO CreateRisk(CompanyRisk companyRisk)
        {
            return new COMTRAEN.EndorsementRiskDTO
            {
                Id = companyRisk.Id,
                Description = companyRisk.Description,
                Coverages = CreateCoverages(companyRisk.Coverages)
            };
        }

        internal static List<COMTRAEN.EndorsementRiskDTO> CreateRisks(List<CompanyRisk> companyRisks)
        {
            List<COMTRAEN.EndorsementRiskDTO> risks = new List<COMTRAEN.EndorsementRiskDTO>();

            foreach (var companyRisk in companyRisks)
            {
                risks.Add(CreateRisk(companyRisk));
            }
            return risks;
        }

        internal static COMTRAEN.EndorsementCoverageDTO CreateCoverage(CompanyCoverage companyCoverage)
        {
            return new COMTRAEN.EndorsementCoverageDTO
            {
                Id = companyCoverage.Id,
                Description = companyCoverage.Description,
                PremiumAmount = companyCoverage.PremiumAmount
            };
        }

        internal static List<COMTRAEN.EndorsementCoverageDTO> CreateCoverages(List<CompanyCoverage> companyCoverages)
        {
            List<COMTRAEN.EndorsementCoverageDTO> coverages = new List<COMTRAEN.EndorsementCoverageDTO>();

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
    }
}
