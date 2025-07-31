using Sistran.Core.Framework.UIF2.Services;
using Sistran.Core.Application.AuthenticationServices;

namespace Sistran.Core.Application.UIF2.Providers
{
    public class DelegateService
    {
        internal static IAuthenticationProviders authenticationProviders = ServiceManager.Instance.GetService<IAuthenticationProviders>();
        internal static IAuthorizationProvider authorizationProviders = ServiceManager.Instance.GetService<IAuthorizationProvider>();
    }
}