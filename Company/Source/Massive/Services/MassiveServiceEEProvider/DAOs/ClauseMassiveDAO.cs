using Sistran.Company.Application.MassiveServices.EEProvider.Assemblers;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnumsCore = Sistran.Core.Application.UnderwritingServices.Enums;

namespace Sistran.Company.Application.MassiveServices.EEProvider.DAOs
{
    public class ClauseMassiveDAO
    {
        public List<CompanyClause> GetClauses(EnumsCore.EmissionLevel emissionLevel, int prefixId, int? conditionLevel)
        {
            var mapper = ModelAssembler.CreateMapCompanyClause();
            List<CompanyClause> clauses = mapper.Map<List<Clause>, List<CompanyClause>>(DelegateService.underwritingServiceCore.GetClausesByEmissionLevelConditionLevelId(emissionLevel, prefixId));
            List<CompanyClause> clausesObligatory = new List<CompanyClause>();
            
            if (clauses.Count > 0)
            {
                clausesObligatory.AddRange(clauses.Where(x => x.IsMandatory == true).ToList());
            }
            else
            {
                clausesObligatory = new List<CompanyClause>();
            }

            return clausesObligatory;
        }
    }
}
