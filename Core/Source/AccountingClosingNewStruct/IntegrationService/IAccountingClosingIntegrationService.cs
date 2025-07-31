using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.AccountingClosingServices
{
    [ServiceContract]
    public interface IAccountingClosingIntegrationService
    {
        [OperationContract]
        void Proof();
    }
}
