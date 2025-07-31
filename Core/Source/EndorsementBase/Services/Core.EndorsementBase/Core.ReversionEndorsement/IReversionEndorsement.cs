

using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using System.Collections.Generic;

namespace Sistran.Core.Application.ReversionEndorsement
{
    public interface IReversionEndorsement
    {
        List<PoliciesAut> ValidateAuthorizationPolicies(Policy policy);
    }
}
