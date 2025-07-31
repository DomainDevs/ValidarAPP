using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.ReversionEndorsement
{
    [ServiceContract]
    public interface ICiaReversionEndorsement
    {
        [OperationContract]
        List<string> GetEndorsementWorkFlow(int PolyciId);

        [OperationContract]
        bool CreateEndorsementWorkFlow(int? PolicyId, int? EndorsementId, string filingNumber, DateTime filingDate);
    }
}
