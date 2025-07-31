using OperatingQuoteWebServices.Models;
using OperatingQuoteWebServices.Resources;
using Sistran.Co.Previsora.Application.FullServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Web;

namespace OperatingQuoteWebServices.Delegate
{
    public class DelegateAuthorization: ServiceAuthorizationManager
    {
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            var requestMessage = operationContext.RequestContext.RequestMessage;
            var headers = (HttpRequestMessageProperty)requestMessage.Properties[HttpRequestMessageProperty.Name];

            string authorizationToken = headers.Headers[HttpRequestHeader.Authorization];

            if (authorizationToken != null && authorizationToken.StartsWith(Constants.BasicAuth))
            {
                var authDTO = DecodeAuthToken(authorizationToken);
                return DelegateService.fullServicesSupProvider.GetStatusAplication(new StatusAplication(authDTO.IdApplication, authDTO.KeyApplication));
            }

            return false;
        }

        private StatusAplicationDTO DecodeAuthToken(string token)
        {
            string encodedCredentials = token.Substring(Constants.BasicAuth.Length).Trim();
            Encoding encoding = Encoding.GetEncoding(Constants.DefaultEconding);
            string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedCredentials));
            string[] credentials = usernamePassword.Split(':');
            return new StatusAplicationDTO

            {
                IdApplication = int.Parse(credentials[0]),
                KeyApplication = credentials[1]
            };
        }
    }
}