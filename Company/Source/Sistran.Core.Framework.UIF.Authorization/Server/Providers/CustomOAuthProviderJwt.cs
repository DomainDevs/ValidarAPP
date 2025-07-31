using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using Sistran.Core.Framework.UIF.Authorization.Server.Models;
using Sistran.Core.Application.SecurityServices.Models;
using Sistran.Core.Framework.UIF.Authorization.Server.Services;
using Sistran.Core.Framework.UIF.Authorization.Server.Enum;
using System;

namespace Sistran.Core.Framework.UIF.Authorization.Server
{
    internal class CustomOAuthProviderJwt : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId = string.Empty;
            string clientSecret = string.Empty;
            string symmetricKeyAsBase64 = string.Empty;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (context.ClientId == null)
            {
                context.SetError("invalid_clientId", "client_Id no puede ser nulo");
                return Task.FromResult<object>(null);
            }

            var token = context.ClientId.Split(':');

            var client_id = token.First();
            var accessKey = token.Last();

            var applicationAccess = WebApplicationAccess.Find(client_id);

            if (applicationAccess == null)
            {
                context.SetError("invalid_clientId", "client_Id no encontrado");
                return Task.FromResult<object>(null);
            }

            if (applicationAccess.AccessKey != accessKey)
            {
                context.SetError("invalid_clientId", "access key invalido");
                return Task.FromResult<object>(null);
            }

            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            var Login = AuthenticationValidation(new LoginModel()
            {
                UserName = context.UserName,
                Password = context.Password
            });

            if (Login.Count > 0 && Login.Select(x => x.Key).FirstOrDefault() == 0)
            {
                context.Validated();
            }
            else
            {                
                context.SetError(Login.Select(x => x.Value).FirstOrDefault());
                return Task.FromResult<object>(null);
            }

            var identity = new ClaimsIdentity("JWT");

            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim("sub", context.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Role, "Administrador"));
            identity.AddClaim(new Claim("Modulos", "Suscription"));
            identity.AddClaim(new Claim("SubModulo", "Emision"));
            identity.AddClaim(new Claim("SubModulo", "Cotización"));

            var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    {
                         "audience", (context.ClientId == null) ? string.Empty : context.ClientId
                    }
                });

            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);
            return Task.FromResult<object>(null);
        }

        public Dictionary<int, string> AuthenticationValidation(LoginModel value)
        {
            try
            {
                AuthenticationResult result = DelegateService.AuthenticationProviders.AutenthicateR2(value.UserName, value.Password, null, "42a5ozsfv2v3231fed0wz1mx");

                Dictionary<int, string> objResult = new Dictionary<int, string>();
                if (result != null)
                {

                    if (result.isAuthenticated)
                    {
                        objResult.Add((int)AuthenticationEnum.isAuthenticated, "usuario autenticado");


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
                            objResult.Add((int)result.Result, Resources.Language.PasswordExpired); ;
                            break;
                        default:
                            break;
                    }
                }

                return objResult;
            }
            catch (Exception ex)
            {
                return new Dictionary<int, string>();
            }
        }

    }
}