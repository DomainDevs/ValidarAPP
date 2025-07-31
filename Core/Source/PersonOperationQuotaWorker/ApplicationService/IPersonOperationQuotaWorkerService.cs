using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.PersonOperationQuotaWorkerServices
{
    [ServiceContract]
    public interface IPersonOperationQuotaWorkerService
    {
        [OperationContract]
        void prueba();
    }
}
