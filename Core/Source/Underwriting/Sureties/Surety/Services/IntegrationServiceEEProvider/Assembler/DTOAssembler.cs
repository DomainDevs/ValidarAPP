using Sistran.Core.Integration.SuretyServices.DTOs;
using System;
using System.Collections.Generic;
using Sistran.Core.Application.Sureties.SuretyServices.Models;
using Sistran.Core.Application.Sureties.JudicialSuretyServices.Models;
namespace Sistran.Core.Integration.Sureties.SuretyServices.EEProvider.Assembler
{
    public class DTOAssembler
    {
        internal static List<SuretyDTO> CreateSureties(List<Contract> contracs)
        {
            List<SuretyDTO> suretyDTO = new List<SuretyDTO>();

            foreach (Contract contract in contracs)
            {
                suretyDTO.Add(CreateSurety(contract));
            }

            return suretyDTO;
        }

        internal static SuretyDTO CreateSurety(Contract contract)
        {
            if (contract == null)
            {
                return null;
            }

            return new SuretyDTO
            {
                Bonded = contract.Contractor?.Name,
                IdentificationDocument = contract.Contractor?.IdentificationDocument?.Number,
                IndividualId = Convert.ToInt32(contract.Contractor?.IndividualId),
                BidNumber = contract.SettledNumber,
                CourtNum = null,
                ContractAmt = Convert.ToDecimal(contract.Value?.Value),
                PremiumAmt = contract.Risk.Premium,
                EstimationAmount = Convert.ToDecimal(contract.Risk.Description),
                RiskId = contract.Risk.RiskId,
                CoveredRiskType = Convert.ToInt32(contract.Risk.CoveredRiskType),
                DocumentNum = contract.Risk.Policy?.DocumentNumber,
                EndorsementId = contract.Risk.Policy?.Endorsement?.Id,
                Endorsement = contract.Risk.Policy?.Endorsement?.Number,
                PolicyId = Convert.ToInt32(contract.Risk.Policy?.Id),
                InsuredIndividualId = Convert.ToInt32(contract.Risk?.MainInsured?.IndividualId),
                RiskNumber = Convert.ToInt32(contract.Risk?.Number),
                InsuredAmount = contract.Risk.AmountInsured
            };
        }

        internal static List<SuretyDTO> CreateJudicialSureties(List<Judgement> judgements)
        {
            List<SuretyDTO> suretyDTO = new List<SuretyDTO>();

            foreach (Judgement companyJudgement in judgements)
            {
                suretyDTO.Add(CreateJudicialSurety(companyJudgement));
            }

            return suretyDTO;
        }

        internal static SuretyDTO CreateJudicialSurety(Judgement judgement)
        {
            if (judgement == null)
            {
                return null;
            }

            return new SuretyDTO
            {
                ArticleId = judgement.Article.Id,
                ArticleDescription = judgement.Article.Description,
                Bonded = judgement.Risk.MainInsured.Name,
                IdentificationDocument = judgement.Risk.MainInsured.IdentificationDocument?.Number,                
                CourtNum = null,                
                PremiumAmt = judgement.Risk.Premium,
                InsuredAmount = judgement.Risk.AmountInsured,
                RiskId = judgement.Risk.RiskId,
                InsuredIndividualId = Convert.ToInt32(judgement.Risk?.MainInsured?.IndividualId),
                RiskNumber = judgement.Risk.Number,
                DocumentNum = judgement.Risk.Policy?.DocumentNumber,
                EndorsementId = judgement.Risk.Policy?.Endorsement?.Id,
                Endorsement = judgement.Risk.Policy?.Endorsement?.Number,
                PolicyId = Convert.ToInt32(judgement.Risk.Policy?.Id),
                CoveredRiskType = Convert.ToInt32(judgement.Risk.CoveredRiskType)
            };
        }
    }
}
