using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sistran.Core.Framework.UIF2.Security;
using Sistran.Core.Framework.UIF2.Security.Authentication;

namespace Sistran.Core.Application.UIF2.Providers 
{
    public class GGGAuthenticationProvider: IAuthenticationProvider

    {

        #region IAuthenticationProvider Members

        public Framework.UIF2.Security.Authentication.AuthenticationResult Autenthicate(string loginName, string password, string domain)
        {
            AuthenticationResult result = new AuthenticationResult();

            if (loginName == "sistran" && password == "desapassword")
            {
                result.isAuthenticated = true;
                result.Result = AuthenticationEnum.isAuthenticated;
            }

            return result;
        }

        #endregion
    }
}
