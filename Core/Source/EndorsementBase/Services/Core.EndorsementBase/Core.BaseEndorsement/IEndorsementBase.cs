using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Sistran.Core.Application.BaseEndorsment
{
    [ServiceContract]
    public interface IEndorsementBase
    {
        [OperationContract]
        void GetEndorsementById(int idEndorsement);
    }
}
