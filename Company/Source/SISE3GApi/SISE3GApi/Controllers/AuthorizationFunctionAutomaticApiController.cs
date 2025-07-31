using Sistran.Company.SISE3GApi.WrapperAuthoPoliciesService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Sistran.Company.SISE3GApi.Controllers
{
    public class AuthorizationFunctionAutomaticApiController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Número del prospecto</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("CreateAutomaticQuota")]
        [Route("Api/AuthorizationFunctionAutomaticQuotaApi/CreateAutomaticQuota")]
        public IHttpActionResult CreateAutomaticQuota([FromUri]int id)
        {
            try
            {
                using (WrapperAuthorizationPoliciesServiceClient client = new WrapperAuthorizationPoliciesServiceClient("basicEndpoint"))
                {
                    client.CreateAutomaticQuotaAuthorization(id);
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", $" Error en CreateAutomaticQuota: {ex.Message}");
                return this.BadRequest(ex.Message);
            }

            return this.Ok();
        }

    }
}