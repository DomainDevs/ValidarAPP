using Sistran.Company.Application.Transports.Endorsement.CreditNote.BusinessServices.Models;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Sistran.Company.Application.Transports.Endorsement.CreditNote.BusinessServices.EEProvider.Assemblers
{
    public class EntityAssembler
    {

        internal static List<CompanyEndorsementType> CreateEndorsementTypes (List<ISSEN.EndorsementType> entityEndorsementTypes,List<ISSEN.Endorsement> entityEndorsements)
        {
            List<CompanyEndorsementType> companyEndorsementTypes = new List<CompanyEndorsementType>();
            foreach (ISSEN.EndorsementType endorsementType in entityEndorsementTypes)
            {
                companyEndorsementTypes.Add(new CompanyEndorsementType { 
                 Id = entityEndorsementTypes.First(x => x.EndoTypeCode == endorsementType.EndoTypeCode).EndoTypeCode,
                 Description = entityEndorsementTypes.First(x => x.Description == endorsementType.Description).Description,
                 EndorsementId= entityEndorsements.Last(x=> x.EndoTypeCode == endorsementType.EndoTypeCode).EndorsementId,
                 CurrentFrom = entityEndorsements.Last(x=>x.EndoTypeCode == endorsementType.EndoTypeCode).CurrentFrom,
                 CurrentTo = (DateTime)entityEndorsements.Last(x => x.EndoTypeCode == endorsementType.EndoTypeCode).CurrentTo
                });


            }
            return companyEndorsementTypes;
        }

    
     

        internal static List<CompanyEndorsementType> CreateRiskType(List<ISSEN.RiskDetailDescription> entityriskDetailDescriptions)
        {
            List<CompanyEndorsementType> companyEndorsementTypes = new List<CompanyEndorsementType>();
            foreach (ISSEN.RiskDetailDescription endorsementRisk in entityriskDetailDescriptions)
            {
                companyEndorsementTypes.Add(new CompanyEndorsementType
                {
                    Description = entityriskDetailDescriptions.First(x => x.Description == endorsementRisk.Description).Description
                });
            }

            return companyEndorsementTypes;
        }
    }
}
