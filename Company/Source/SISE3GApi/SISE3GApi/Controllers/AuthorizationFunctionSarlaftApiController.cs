
namespace Sistran.Company.SISE3GApi.Controllers
{
    using Sistran.Company.SISE3GApi.WrapperAuthoPoliciesService;
    using System;
    using System.Diagnostics;
    using System.Web.Http;

    /// <summary>
    /// Servicio Web API - Módulo de Sarlaft
    /// </summary>
    public class AuthorizationFunctionSarlaftApiController : ApiController
    {
        /// <summary>
        /// Redirige el número de temporal hacia el módulo de Sarlaft
        /// </summary>
        /// <param name="operationId">Número del movimiento</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/AuthorizationFunctionSarlaftApi/CreateSarlaftAuthorization")]
        public IHttpActionResult CreateSarlaftAuthorization([FromUri]int operationId)
        {
            try
            {
                using (WrapperAuthorizationPoliciesServiceClient client = new WrapperAuthorizationPoliciesServiceClient("basicEndpoint"))
                {
                    client.CreateSarlaftAuthorization(operationId);
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", $" Error en CreateSarlaftAuthorization: {ex.Message}");
                return this.BadRequest(ex.Message);
            }

            return this.Ok();
        }
    }
}
