namespace Sistran.Company.SISE3GApi.Controllers
{
    using WrapperAuthoPoliciesService;
    using System;
    using System.Diagnostics;
    using System.Web.Http;

    /// <summary>
    /// Servicio Web API - Módulo de Siniestros
    /// </summary>
    public class AuthorizationFunctionClaimApiController : ApiController
    {
        /// <summary>
        /// Redirige el número de temporal hacia el módulo de Siniestros Denuncia
        /// </summary>
        /// <param name="claimTemporalId">Número temporal de Siniestro Denuncia</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/AuthorizationFunctionClaimApi/CreateClaimAuthorization")]
        public IHttpActionResult CreateClaimAuthorization([FromUri] int claimTemporalId)
        {
            try
            {
                using (WrapperAuthorizationPoliciesServiceClient client = new WrapperAuthorizationPoliciesServiceClient("basicEndpoint"))
                {
                    client.CreateClaimAuthorization(claimTemporalId);
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", string.Format(" Error en CreateClaimAuthorization: {0}", ex.Message));
                return this.BadRequest(ex.Message);
            }

            return this.Ok();
        }

        /// <summary>
        /// Redirige el número de temporal hacia el módulo de Solicitud de Pago
        /// </summary>
        /// <param name="paymentRequestTemporalId">Número temporal de la Solicitud de Pago</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/AuthorizationFunctionClaimApi/CreatePaymentRequestAuthorization")]
        public IHttpActionResult CreatePaymentRequestAuthorization([FromUri] int paymentRequestTemporalId)
        {
            try
            {
                using (WrapperAuthorizationPoliciesServiceClient client = new WrapperAuthorizationPoliciesServiceClient("basicEndpoint"))
                {
                    client.CreatePaymentRequestAuthorization(paymentRequestTemporalId);
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", string.Format(" Error en CreatePaymentRequestAuthorization: {0}", ex.Message));
                return this.BadRequest(ex.Message);
            }

            return this.Ok();
        }

        /// <summary>
        /// Redirige el número de temporal hacia el módulo de Siniestros Solicitud de Cobro
        /// </summary>
        /// <param name="chargeRequestTemporalId">Número temporal de la Solicitud de Cobro</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/AuthorizationFunctionClaimApi/CreateChargeRequestAuthorization")]
        public IHttpActionResult CreateChargeRequestAuthorization([FromUri] int chargeRequestTemporalId)
        {
            try
            {
                using (WrapperAuthorizationPoliciesServiceClient client = new WrapperAuthorizationPoliciesServiceClient("basicEndpoint"))
                {
                    client.CreateChargeRequestAuthorization(chargeRequestTemporalId);
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", string.Format(" Error en CreateChargeRequestAuthorization: {0}", ex.Message));
                return this.BadRequest(ex.Message);
            }

            return this.Ok();
        }

        /// <summary>
        /// Redirige el número de temporal hacia el módulo de Siniestros Aviso
        /// </summary>
        /// <param name="claimNoticeTemporalId">Número temporal del Aviso del Siniestro</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/AuthorizationFunctionClaimApi/CreateClaimNoticeAuthorization")]
        public IHttpActionResult CreateClaimNoticeAuthorization([FromUri] int claimNoticeTemporalId)
        {
            try
            {
                using (WrapperAuthorizationPoliciesServiceClient client = new WrapperAuthorizationPoliciesServiceClient("basicEndpoint"))
                {
                    client.CreateClaimNoticeAuthorization(claimNoticeTemporalId);
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", string.Format(" Error en CreateClaimNoticeAuthorization: {0}", ex.Message));
                return this.BadRequest(ex.Message);
            }

            return this.Ok();
        }
    }
}