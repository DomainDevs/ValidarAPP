using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Sistran.Core.Framework.UIF.Authorization.Server.Models;
using Sistran.Core.Application.SecurityServices.Models;
using Sistran.Core.Framework.UIF.Authorization.Server.Services;
using Sistran.Core.Framework.UIF.Authorization.Server.Enum;
using System.Web.Http;

namespace Sistran.Core.Framework.UIF.Authorization.Server.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException("publicClientId");
            }

            _publicClientId = publicClientId;
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

            ApplicationUser user = await userManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "usuario o password incorrectos");
                return;
            }

            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager, OAuthDefaults.AuthenticationType);
            ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(userManager, CookieAuthenticationDefaults.AuthenticationType);

            AuthenticationProperties properties = CreateProperties(user.UserName);
            AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(cookiesIdentity);

        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {

            //if (context.ClientId == null)
            //{
            //    context.Validated();
            //}

            //return Task.FromResult<object>(null);

            try
            {
                var username = context.Parameters["username"];
                var password = context.Parameters["password"];

                    var Login = AuthenticationValidation(new LoginModel() { 
                        UserName = context.Parameters["username"],
                        Password = context.Parameters["password"]
                    });

                    if (Login.Select(x => x.Key).FirstOrDefault() == 0)
                    {
                        context.OwinContext.Set("otc:username", username);
                        context.Validated();
                    }
                    else
                    {
                        context.SetError(Login.Select(x => x.Value).FirstOrDefault());
                        context.Rejected();
                    }
                   
              
            }
            catch
            {
                context.SetError("Error al consultar las credenciales");
                context.Rejected();
            }
            return Task.FromResult(0);
        }

        //Validacion apuntando a bd Sybase
        public Dictionary<int, string> AuthenticationValidation([FromBody]LoginModel value)
        {
            try
            {
                AuthenticationResult result = DelegateService.AuthenticationProviders.AutenthicateR2(value.UserName, value.Password, null, "42a5ozsfv2v3231fed0wz1mx");

                Dictionary<int,string> objResult = new Dictionary<int, string>();
                if (result != null)
                {

                    if (result.isAuthenticated)
                    {
                        objResult.Add((int)AuthenticationEnum.isInvalidPassword, "usuario autenticado");


                    }

                    switch ((int)result.Result)
                    {
                        case (int)AuthenticationEnum.isInvalidPassword:
                            objResult.Add((int)result.Result, Resources.Language.UserOrPasswordFailed);
                           ;
                            break;
                        case (int)AuthenticationEnum.isInvalidPasswordWithData:
                            objResult.Add((int)result.Result, string.Format(Resources.Language.PasswordLockWarning, result.data[0], result.data[1]));
                            break;
                        case (int)AuthenticationEnum.isUserBlocked:
                            objResult.Add((int)result.Result, Resources.Language.UserBlocledContact);
                            break;
                        case (int)AuthenticationEnum.isUserBlockedWithTime:
                            objResult.Add((int)result.Result, string.Format(Resources.Language.PasswordLocked, result.data[0]));
                            break;
                        case (int)AuthenticationEnum.IsUserExpired:
                            objResult.Add((int)result.Result, Resources.Language.UserError);
                           break;
                        case (int)AuthenticationEnum.isPasswordExpired:
                            objResult.Add((int)result.Result, "Su clave ha expirado");;
                            break;
                        default:
                            break;
                    }
                }

                return objResult;
            }
            catch (Exception ex)
            {
                return new Dictionary<int,string>();
            }
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }
    }
}