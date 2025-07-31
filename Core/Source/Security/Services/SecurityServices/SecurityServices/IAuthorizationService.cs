using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Core.Application.SecurityServices
{
    [ServiceContract]
    public interface IAuthorizationService
    {
        [OperationContract]
        IList<Models.ControlSecurity> GetControlsSecurity(string viewID, string userName);
        [OperationContract]
        IList<Models.Module> GetModules(string userName);
        [OperationContract]
        IList<Models.Module> GetModulesAccess(string userName);
    }
}
