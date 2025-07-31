using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Sistran.Core.Framework.UIF.Authorization.Server.Providers;
using Sistran.Core.Framework.UIF.Authorization.Server.Models;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using System.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace Sistran.Core.Framework.UIF.Authorization.Server
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions authServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/oauth2/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(1),
                Provider = new CustomOAuthProviderJwt(),
                AccessTokenFormat = new CustomJwtFormat("http://localhost:1741/")
            };

            app.UseOAuthAuthorizationServer(authServerOptions);
                       

            //app.UseCookieAuthentication(new CookieAuthenticationOptions
            //{
            //    AuthenticationType = "Application",
            //    AuthenticationMode = AuthenticationMode.Passive,
            //    LoginPath = new PathString("/oauth2/token"),
            //    LogoutPath = new PathString("/oauth2/token"),
            //    ExpireTimeSpan = TimeSpan.FromSeconds(60)
            //});

            var issuer = "http://localhost:1741/";
            var audience = WebApplicationAccess.WebApplicationAccessList.Select(x => x.Value.ClientId).AsEnumerable();
            var secretsSymmetricKey = (from x in WebApplicationAccess.WebApplicationAccessList
                                       select new SymmetricKeyIssuerSecurityTokenProvider(issuer, TextEncodings.Base64Url.Decode(x.Value.SecretKey))).ToArray();


            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    AllowedAudiences = audience,
                    IssuerSecurityTokenProviders = secretsSymmetricKey
                });
        }
    }
}
