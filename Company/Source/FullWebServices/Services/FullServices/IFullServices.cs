using System;
using System.ServiceModel;
using Sistran.Co.Previsora.Application.FullServices.Models;

namespace Sistran.Co.Previsora.Application.FullServices
{
    [ServiceContract]
    public interface IFullServices
    {
        [OperationContract]
        String ExecuteETLSistran(EntityExampleRequest entityExamp);

        #region "Interfaces Helpers"

        [OperationContract]
        String ValidateAccessUser(EntityUserAcessRequest EntityUserAcessRequest);

        [OperationContract]
        bool IsInProcess(ProcessMethodRequest processMethodRequest);

        [OperationContract]
        int UpdateProcessMethod(ProcessMethodRequest processMethodRequest);

        #endregion
    }  
}
