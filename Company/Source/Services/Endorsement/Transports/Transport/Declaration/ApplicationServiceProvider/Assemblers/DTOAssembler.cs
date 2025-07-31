using Sistran.Company.Application.Transports.Endorsement.Declaration.ApplicationServices.DTOs;
using Sistran.Company.Application.Transports.TransportApplicationService.DTOs;
using Sistran.Company.Application.Transports.TransportBusinessService.Models.Base;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Company.Application.Transports.Endorsement.Declaration.ApplicationServices.EEProvider.Assemblers
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
        public static DeclarationDTO CreateDeclarationDTO(List<TransportDTO> transportDTOs)
        {
            if (transportDTOs == null)
            {
                return null;
            }
            DeclarationDTO declarationDTO = new DeclarationDTO();
            List<TransportDTO> transportDTO = new List<TransportDTO>();
           

            foreach (TransportDTO transport in transportDTOs)
            {
                transportDTO.Add(CreateTransport(transport));

            }
            declarationDTO.Transports = transportDTO;
            return declarationDTO;
        }

        /// <summary>
        /// Crea un riesgo uno a uno
        /// </summary>
        /// <param name = "transport" name = "Risk" ></ param >
        /// < returns ></ returns >
        public static TransportDTO CreateTransport(TransportDTO transport)
        {
            TransportDTO transportDTO = new TransportDTO
            {
                RiskId = transport.RiskId,
                MinimumPremium = transport.MinimumPremium,
                
                Description = transport.Description,
                LimitMaxRealeaseAmount = transport.LimitMaxRealeaseAmount,
                
                CoverageGroupId = transport.CoverageGroupId
            };
            return transportDTO;
        }

        /// <summary>
        /// Crea una riesgo
        /// </summary>
        /// <param name = "transportDTOs" name = "Risk" ></ param >
        /// < returns ></ returns >
        public static DeclarationDTO CreateDeclarationDTOByRiskId(List<TransportDTO> transportDTOs, TransportDTO Risk)
        {
            DeclarationDTO declarationDTO = new DeclarationDTO();
            TransportDTO transports = new TransportDTO();

            transports =  transportDTOs.Where(x => x.RiskId == Risk.RiskId).Select(x => new TransportDTO
            {
                Description = x.Description,
                PackagingDescription = x.PackagingDescription,
                Source = x.Source,
                Target = x.Target,
                TransportTypeDescriptions = x.TransportTypeDescriptions,
                ViaDescription = x.ViaDescription,

            }).FirstOrDefault();

            declarationDTO.Transport = transports;
            return declarationDTO;
        }

        /// <summary>
        /// Crea una lista de objetos del seguro
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DeclarationDTO CreateInsured(List<InsuredObjectDTO> insured)
        {
            if (insured == null)
            {
                return null;
            }

            DeclarationDTO declaration = new DeclarationDTO();
            declaration.Insured = insured;
            return declaration;
        }

        /// <summary>
        /// Crea una endoso
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
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

        public static DeclarationDTO DeclarationDTO (CompanyPolicy policy)
        {
            SummaryDTO sumary = new SummaryDTO();
            DeclarationDTO declarationModel = new DeclarationDTO();

            if (policy == null)
            {
                return null;
            }
               declarationModel.PolicyId = policy.Id;
               declarationModel.PrefixId = policy.Prefix.Id;
               declarationModel.BranchId = policy.Branch.Id;
               declarationModel.Days = policy.Endorsement.EndorsementDays;
               declarationModel.CurrentFrom = policy.Endorsement.CurrentFrom;
               declarationModel.CurrentTo = policy.Endorsement.CurrentTo;
               declarationModel.TemporalId = policy.Endorsement.TemporalId;
               declarationModel.TicketNumber = Convert.ToInt32(policy.Endorsement.TicketNumber);
               declarationModel.TicketDate = Convert.ToDateTime(policy.Endorsement.TicketDate);
               declarationModel.DeclaredValue= policy.Endorsement.DeclaredValue;
               declarationModel.RiskId= policy.Endorsement.RiskId;
               declarationModel.InsuranceObjectId = policy.Endorsement.InsuredObjectId;
               declarationModel.Text = policy.Endorsement.Text.TextBody;
               declarationModel.Observation = policy.Endorsement.Text.Observations;
               sumary.Sum = policy.Summary.AmountInsured;
               sumary.Premiun = policy.Summary.Premium;
               sumary.Expense = policy.Summary.Expenses;
               sumary.Surcharge = policy.Summary.Surcharges;
               sumary.Discount = policy.Summary.Discounts;
               sumary.Tax = policy.Summary.Taxes;
               sumary.TotalPremiun = policy.Summary.FullPremium;
               sumary.RiskCount = policy.Summary.RiskCount;
               declarationModel.Summary = sumary;
               return  declarationModel;
        }
/// <summary>
        /// Crea un endoso de declaración con la vigencia del endoso parametrizado
        /// </summary>
        /// <param name="endorsement">Endoso origen</param>
        /// <returns>Endoso de declaración</returns>
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
