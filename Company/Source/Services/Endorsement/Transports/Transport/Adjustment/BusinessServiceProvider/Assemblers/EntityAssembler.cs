using Sistran.Company.Application.UnderwritingServices.Models;
using System.Collections.Generic;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using System.Linq;
using Sistran.Company.Application.Transports.TransportBusinessService.Models.Base;
using System;
using Sistran.Core.Application.UnderwritingServices.Enums;

namespace Sistran.Company.Application.Transports.Endorsement.Adjustment.BusinessServices.EEProvider.Assemblers
{
    public class EntityAssembler
    {
        internal static  List<CompanyCoverage> Createcoverage (List<ISSEN.RiskCoverage> riskCoverages,  List<ISSEN.Endorsement> Endorsements, List<ISSEN.EndorsementRiskCoverage> endorsementRiskCoverages)
          {
            List<CompanyCoverage> companyCoverage = new List<CompanyCoverage>();
             foreach (ISSEN.EndorsementRiskCoverage endoriskcoverage in endorsementRiskCoverages)
            {
                companyCoverage.Add(new CompanyCoverage
                {
                    DeclaredAmount = riskCoverages.First(x => x.RiskCoverId == endoriskcoverage.RiskCoverId).DeclaredAmount,
                    IsMinPremiumDeposit = riskCoverages.First(x => x.RiskCoverId == endoriskcoverage.RiskCoverId).IsMinPremiumDeposit,
                    EndorsementId = Endorsements.First(x => x.EndorsementId == endoriskcoverage.EndorsementId).EndorsementId,
                    EndorsementType = (EndorsementType)Endorsements.First(x => x.EndorsementId == endoriskcoverage.EndorsementId).EndoTypeCode,

                });
            };

            return companyCoverage;

    
    }
}
}    