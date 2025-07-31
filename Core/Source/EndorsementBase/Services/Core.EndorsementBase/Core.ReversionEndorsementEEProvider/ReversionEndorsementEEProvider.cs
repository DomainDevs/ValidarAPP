
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.ReversionEndorsement.EEProvider.Assemblers;
using Sistran.Core.Application.RulesScriptsServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using System.Collections.Generic;
using Rules = Sistran.Core.Framework.Rules;
using Sistran.Core.Application.Utilities.Enums;

namespace Sistran.Core.Application.ReversionEndorsement.EEProvider
{
    public class ReversionEndorsementEEProvider : IReversionEndorsement
    {
        public List<PoliciesAut> ValidateAuthorizationPolicies(Policy policy)
        {
            var key = policy.Prefix.Id + "," + (int)policy.Product.CoveredRisk.CoveredRiskType;
            List<PoliciesAut> policiesAuts = new List<PoliciesAut>();

            Rules.Facade facade = new Rules.Facade();

            EntityAssembler.CreateFacadeGeneral(facade, policy);

            policiesAuts.AddRange(DelegateService.AuthorizationPoliciesServiceCore.ValidateAuthorizationPolicies(10, key, facade, FacadeType.RULE_FACADE_GENERAL));

            return policiesAuts;
        }
    }
}
