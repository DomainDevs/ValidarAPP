using Sistran.Core.Application.SecurityServices.Models;
using System.ServiceModel;

namespace Sistran.Core.Application.SecurityServices
{
    [ServiceContract]
    public interface IAuthenticationService
    {
        [OperationContract]
        AuthenticationResult Autenthicate(string loginName, string password, string domain);

        [OperationContract]
        AuthenticationResult AutenthicateR2(string loginName, string password, string domain, string SessionID);

        [OperationContract]
        int GetUserId(string loginName);

        [OperationContract]
        void UnlockPassword(int UserId);

        [OperationContract]
        User GetUserByName(string name);
        [OperationContract]
        string GetLevelOperation();

    }
}
