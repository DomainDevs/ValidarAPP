using Sistran.Company.Application.Adjustment.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.Adjustment
{
    [ServiceContract]
    public interface ICiaAdjustmentEndorsement
    {

        [OperationContract]
        EndorsementDTO GetTemporalEndorsementByPolicyId(int policyId);
    }
}
