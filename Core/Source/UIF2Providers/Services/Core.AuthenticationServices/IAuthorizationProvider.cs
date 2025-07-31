using Sistran.Core.Application.SecurityServices;
using System.ServiceModel;

namespace Sistran.Core.Application.AuthenticationServices
{
    [ServiceContract]
    public interface IAuthorizationProvider : IAuthorizationService
    {
    }
}
