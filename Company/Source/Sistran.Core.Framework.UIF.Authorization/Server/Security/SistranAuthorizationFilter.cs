using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Routing;

namespace Sistran.Core.Framework.UIF.Authorization.Server.Security
{
    public class SistranAuthorizationFilter : AuthorizeAttribute
    {

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            actionContext.Response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Content = new StringContent("Autorización denegada")
            };
        }

       
    }

}