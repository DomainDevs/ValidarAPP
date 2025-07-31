using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Web;
using Thinktecture.IdentityModel.Tokens;

namespace Sistran.Core.Framework.UIF.Authorization.Server
{
    public class CustomJwtFormat : ISecureDataFormat<AuthenticationTicket>
    {
        private readonly string _issuer = string.Empty;
        public CustomJwtFormat(string issuer)
        {
            _issuer = issuer;
        }

        public string Protect(AuthenticationTicket data)
        {
            if (data == null)
                throw new ArgumentNullException("data");


            string audience = data.Properties.Dictionary["audience"];

            if (string.IsNullOrWhiteSpace(audience)) throw new InvalidOperationException("ClientId no encontrado");

            var keys = audience.Split(':');

            var client_id = keys.First();
            var accessKey = keys.Last(); // Client password

            var applicationAccess = WebApplicationAccess.Find(client_id);

            var keyByteArray = TextEncodings.Base64Url.Decode(applicationAccess.SecretKey);

            var signingKey = new HmacSigningCredentials(keyByteArray);

            var issued = data.Properties.IssuedUtc;

            var expires = data.Properties.ExpiresUtc;

            //Modificacion
    
            var signingCredentials = new SigningCredentials(
                                   new InMemorySymmetricSecurityKey(keyByteArray),
                                   SignatureAlgorithm,
                                   DigestAlgorithm);

            var token = new JwtSecurityToken(_issuer, client_id, data.Identity.Claims, issued.Value.UtcDateTime, expires.Value.UtcDateTime, signingCredentials);

            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.WriteToken(token);

            return jwt;
        }

        public string SignatureAlgorithm
        {
            get { return "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256"; }
        }

        public string DigestAlgorithm
        {
            get { return "http://www.w3.org/2001/04/xmlenc#sha256"; }
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            throw new NotImplementedException();
        }
    }
}