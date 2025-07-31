using Sistran.Company.Application.DeclarationApplicationService.DTO;
using Sistran.Company.Application.Location.PropertyServices.DTO;
using Sistran.Company.Application.Location.PropertyServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.DeclarationApplicationServiceEEProvider.Assembler
{
   public class DTOAssembler
    {
        /// <summary>
        /// Crea una poliza
        /// </summary>
        /// <param name="companyPolicy"></param>
        /// <returns></returns>
        public static DeclarationDTO CreatePolicy(CompanyPolicy companyPolicy)
        {
            if (companyPolicy == null)
            {
                return null;
            }
            return new DeclarationDTO
            {
                CurrentFrom = companyPolicy.CurrentFrom,
                CurrentTo = companyPolicy.CurrentTo,
                BranchId = companyPolicy.Branch.Id,
                PrefixId = companyPolicy.Prefix.Id,
                PolicyNumber = companyPolicy.DocumentNumber
            };
        }

        /// <summary>
        /// Crea una transporte
        /// </summary>
        /// <param name="transportDTOs"></param>
        /// <returns></returns>
        public static DeclarationDTO CreateDeclarationDTO(List<CompanyPropertyRisk> companyPropertyRisks)
        {
            if (companyPropertyRisks == null)
            {
                return null;
            }

            DeclarationDTO declarationDTO = new DeclarationDTO();
            List<CompanyPropertyRisk> risks = new List<CompanyPropertyRisk>();


            foreach (CompanyPropertyRisk risk in risks)
            {
                risks.Add(CreateTransport(risk));

            }

            declarationDTO.Risks = risks;

            return declarationDTO;
        }

        /// <summary>
        /// Crea un riesgo uno a uno
        /// </summary>
        /// <param name = "transport" name = "Risk" ></ param >
        /// < returns ></ returns >
        public static CompanyPropertyRisk CreateTransport(CompanyPropertyRisk companyPropertyRisk)
        {
            CompanyPropertyRisk risk = new CompanyPropertyRisk
            {
                Risk = new CompanyRisk
                {
                    Id = companyPropertyRisk.Risk.Id,
                    Description = companyPropertyRisk.Risk.Description,
                    GroupCoverage = new GroupCoverage
                    {
                        Id = companyPropertyRisk.Risk.GroupCoverage.Id
                    }
                }
                //RiskId = transport.RiskId,
                //Description = transport.Description,
                //LimitMaxRealeaseAmount = transport.LimitMaxRealeaseAmount,
                //CoverageGroupId = transport.CoverageGroupId
            };
            return risk;
        }

        /// <summary>
        /// Crea una endoso
        /// </summary>
        public static DeclarationDTO CreateEndorsment(List<EndorsementDTO> endorsments)
        {
            if (endorsments == null)
            {
                return null;
            }

            DeclarationDTO declaration = new DeclarationDTO();
            declaration.Endorsments = endorsments;
            return declaration;
        }

        /// <summary>
        /// Crea una riesgo
        /// </summary>
        /// <param name = "transportDTOs" name = "Risk" ></ param >
        /// < returns ></ returns >
        public static DeclarationDTO CreateDeclarationDTOByRiskId(List<CompanyPropertyRisk> companyPropertyRisks, CompanyPropertyRisk companyPropertyRisk)
        {
            DeclarationDTO declarationDTO = new DeclarationDTO();
            CompanyPropertyRisk risk = new CompanyPropertyRisk();

            risk = companyPropertyRisks.Where(x => x.Risk.Id == companyPropertyRisk.Risk.Id).Select(x => new CompanyPropertyRisk
            {

                //Description = x.Description,
                //PackagingDescription = x.PackagingDescription,
                //Source = x.Source,
                //Target = x.Target,
                //TransportTypeDescriptions = x.TransportTypeDescriptions,
                //ViaDescription = x.ViaDescription,

            }).FirstOrDefault();

            declarationDTO.Risk = risk;
            return declarationDTO;
        }

        public static DeclarationDTO DeclarationDTO(CompanyPolicy policy)
        {
            SummaryDTO summary = new SummaryDTO();
            DeclarationDTO declaration = new DeclarationDTO();

            if (policy == null)
            {
                return null;
            }
            declaration.PolicyId = policy.Id;
            declaration.PrefixId = policy.Prefix.Id;
            declaration.BranchId = policy.Branch.Id;
            declaration.Days = policy.Endorsement.EndorsementDays;
            declaration.CurrentFrom = policy.Endorsement.CurrentFrom;
            declaration.CurrentTo = policy.Endorsement.CurrentTo;
            declaration.TemporalId = policy.Endorsement.TemporalId;
            declaration.TicketNumber = Convert.ToInt32(policy.Endorsement.TicketNumber);
            declaration.TicketDate = Convert.ToDateTime(policy.Endorsement.TicketDate);
            declaration.DeclaredValue = policy.Endorsement.DeclaredValue;
            declaration.RiskId = policy.Endorsement.RiskId;
            declaration.InsuranceObjectId = policy.Endorsement.InsuredObjectId;
            declaration.Text = policy.Endorsement.Text.TextBody;
            declaration.Observation = policy.Endorsement.Text.Observations;
            summary.Sum = policy.Summary.AmountInsured;
            summary.Premiun = policy.Summary.Premium;
            summary.Expense = policy.Summary.Expenses;
            summary.Surcharge = policy.Summary.Surcharges;
            summary.Discount = policy.Summary.Discounts;
            summary.Tax = policy.Summary.Taxes;
            summary.TotalPremiun = policy.Summary.FullPremium;
            summary.RiskCount = policy.Summary.RiskCount;
            declaration.Summary = summary;
            return declaration;
        }

        public static DeclarationDTO CreateDeclarationDTO(EndorsementDTO endorsement)
        {
            return new DeclarationDTO
            {
                CurrentFrom = endorsement.CurrentFrom,
                CurrentTo = endorsement.CurrentTo
            };
        }

    }
}
