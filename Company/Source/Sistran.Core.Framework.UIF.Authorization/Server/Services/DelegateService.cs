using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AUTHEPROVIDER = Sistran.Core.Application.AuthenticationServices;
using Sistran.Core.Framework.UIF2.Services;

namespace Sistran.Core.Framework.UIF.Authorization.Server.Services
{
    public class DelegateService
    {
        internal static AUTHEPROVIDER.IAuthenticationProviders AuthenticationProviders = ServiceManager.Instance.GetService<AUTHEPROVIDER.IAuthenticationProviders>();

    }
}