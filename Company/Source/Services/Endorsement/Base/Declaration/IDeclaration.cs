using Sistran.Company.Application.Declaration.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.Declaration
{
    [ServiceContract]
    public interface ICiaDeclarationEndorsement
    {
        [OperationContract]
        EndorsementDTO GetTemporalEndorsementByPolicyId(int policyId);
    }
}
