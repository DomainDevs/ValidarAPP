namespace Sistran.Company.SISE3GApi.Controllers
{
    using System;
    using System.Diagnostics;
    using System.Web.Http;
    using WrapperAuthoPoliciesService;

    /// <summary>
    /// Servicio Web API - Módulo de Personas
    /// </summary>
    public class AuthorizationFunctionPersonApiController : ApiController
    {
        /// <summary>
        /// Redirige el número de temporal hacia el módulo de Personas - Principal
        /// </summary>
        /// <param name="operationId">Número del movimiento</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/AuthorizationFunctionPerssonApi/CreatePersonAuthorization")]
        public IHttpActionResult CreatePersonAuthorization([FromUri]int operationId)
        {
            try
            {
                using (WrapperAuthorizationPoliciesServiceClient client = new WrapperAuthorizationPoliciesServiceClient("basicEndpoint"))
                {
                    client.CreatePersonAuthorization(operationId);
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", $" Error en CreatePersonAuthorization: {ex.Message}");
                return this.BadRequest(ex.Message);
            }

            return this.Ok();
        }

        /// <summary>
        /// Redirige el número de temporal hacia el módulo de Personas - Asegurado
        /// </summary>
        /// <param name="operationId">Número del movimiento</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/AuthorizationFunctionPerssonApi/CreateInsuredAuthorization")]
        public IHttpActionResult CreateInsuredAuthorization([FromUri]int operationId)
        {
            try
            {
                using (WrapperAuthorizationPoliciesServiceClient client = new WrapperAuthorizationPoliciesServiceClient("basicEndpoint"))
                {
                    client.CreateInsuredAuthorization(operationId);
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", $" Error en CreateInsuredAuthorization: {ex.Message}");
                return this.BadRequest(ex.Message);
            }

            return this.Ok();
        }

        /// <summary>
        /// Redirige el número de temporal hacia el módulo de Personas - Proveedor
        /// </summary>
        /// <param name="operationId">Número del movimiento</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/AuthorizationFunctionPerssonApi/CreateProviderAuthorization")]
        public IHttpActionResult CreateProviderAuthorization([FromUri]int operationId)
        {
            try
            {
                using (WrapperAuthorizationPoliciesServiceClient client = new WrapperAuthorizationPoliciesServiceClient("basicEndpoint"))
                {
                    client.CreateProviderAuthorization(operationId);
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", $" Error en CreateProviderAuthorization: {ex.Message}");
                return this.BadRequest(ex.Message);
            }

            return this.Ok();
        }

        /// <summary>
        /// Redirige el número de temporal hacia el módulo de Personas - Agente
        /// </summary>
        /// <param name="operationId">Número del movimiento</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/AuthorizationFunctionPerssonApi/CreateAgentAuthorization")]
        public IHttpActionResult CreateAgentAuthorization([FromUri]int operationId)
        {
            try
            {
                using (WrapperAuthorizationPoliciesServiceClient client = new WrapperAuthorizationPoliciesServiceClient("basicEndpoint"))
                {
                    client.CreateAgentAuthorization(operationId);
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", $" Error en CreateAgentAuthorization: {ex.Message}");
                return this.BadRequest(ex.Message);
            }

            return this.Ok();
        }

        /// <summary>
        /// Redirige el número de temporal hacia el módulo de Personas - Reasegurador
        /// </summary>
        /// <param name="operationId">Número del movimiento</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/AuthorizationFunctionPerssonApi/CreateReInsurerAuthorization")]
        public IHttpActionResult CreateReInsurerAuthorization([FromUri]int operationId)
        {
            try
            {
                using (WrapperAuthorizationPoliciesServiceClient client = new WrapperAuthorizationPoliciesServiceClient("basicEndpoint"))
                {
                    client.CreateReInsurerAuthorization(operationId);
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", $" Error en CreateReInsurerAuthorization: {ex.Message}");
                return this.BadRequest(ex.Message);
            }

            return this.Ok();
        }

        /// <summary>
        /// Redirige el número de temporal hacia el módulo de Personas - Cupo Operativo
        /// </summary>
        /// <param name="operationId">Número del movimiento</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/AuthorizationFunctionPerssonApi/CreateQuotaAuthorization")]
        public IHttpActionResult CreateQuotaAuthorization([FromUri]int operationId)
        {
            try
            {
                using (WrapperAuthorizationPoliciesServiceClient client = new WrapperAuthorizationPoliciesServiceClient("basicEndpoint"))
                {
                    client.CreateQuotaAuthorization(operationId);
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", $" Error en CreateQuotaAuthorization: {ex.Message}");
                return this.BadRequest(ex.Message);
            }

            return this.Ok();
        }

        /// <summary>
        /// Redirige el número de temporal hacia el módulo de Personas - Impuestos
        /// </summary>
        /// <param name="operationId">Número del movimiento</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/AuthorizationFunctionPerssonApi/CreateTaxAuthorization")]
        public IHttpActionResult CreateTaxAuthorization([FromUri]int operationId)
        {
            try
            {
                using (WrapperAuthorizationPoliciesServiceClient client = new WrapperAuthorizationPoliciesServiceClient("basicEndpoint"))
                {
                    client.CreateTaxAuthorization(operationId);
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", $" Error en CreateTaxAuthorization: {ex.Message}");
                return this.BadRequest(ex.Message);
            }

            return this.Ok();
        }

        /// <summary>
        /// Redirige el número de temporal hacia el módulo de Personas - Coasegurador
        /// </summary>
        /// <param name="operationId">Número del movimiento</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/AuthorizationFunctionPerssonApi/CreateCoInsuredAuthorization")]
        public IHttpActionResult CreateCoInsuredAuthorization([FromUri]int operationId)
        {
            try
            {
                using (WrapperAuthorizationPoliciesServiceClient client = new WrapperAuthorizationPoliciesServiceClient("basicEndpoint"))
                {
                    client.CreateCoInsuredAuthorization(operationId);
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", $" Error en CreateCoInsuredAuthorization: {ex.Message}");
                return this.BadRequest(ex.Message);
            }

            return this.Ok();
        }

        /// <summary>
        /// Redirige el número de temporal hacia el módulo de Personas - Tercero
        /// </summary>
        /// <param name="operationId">Número del movimiento</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/AuthorizationFunctionPerssonApi/CreateThirdAuthorization")]
        public IHttpActionResult CreateThirdAuthorization([FromUri]int operationId)
        {
            try
            {
                using (WrapperAuthorizationPoliciesServiceClient client = new WrapperAuthorizationPoliciesServiceClient("basicEndpoint"))
                {
                    client.CreateThirdAuthorization(operationId);
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", $" Error en CreateThirdAuthorization: {ex.Message}");
                return this.BadRequest(ex.Message);
            }

            return this.Ok();
        }

        /// <summary>
        /// Redirige el número de temporal hacia el módulo de Personas - Empleado
        /// </summary>
        /// <param name="operationId">Número del movimiento</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/AuthorizationFunctionPerssonApi/CreateEmployedAuthorization")]
        public IHttpActionResult CreateEmployedAuthorization([FromUri]int operationId)
        {
            try
            {
                using (WrapperAuthorizationPoliciesServiceClient client = new WrapperAuthorizationPoliciesServiceClient("basicEndpoint"))
                {
                    client.CreateEmployedAuthorization(operationId);
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", $" Error en CreateEmployedAuthorization: {ex.Message}");
                return this.BadRequest(ex.Message);
            }

            return this.Ok();
        }

        /// <summary>
        /// Redirige el número de temporal hacia el módulo de Personas - Informacion Personal
        /// </summary>
        /// <param name="operationId">Número del movimiento</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/AuthorizationFunctionPerssonApi/CreatePersonalInformationAuthorization")]
        public IHttpActionResult CreatePersonalInformationAuthorization([FromUri]int operationId)
        {
            try
            {
                using (WrapperAuthorizationPoliciesServiceClient client = new WrapperAuthorizationPoliciesServiceClient("basicEndpoint"))
                {
                    client.CreatePersonalInformationAuthorization(operationId);
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", $" Error en CreatePersonalInformationAuthorization: {ex.Message}");
                return this.BadRequest(ex.Message);
            }

            return this.Ok();
        }

        /// <summary>
        /// Redirige el número de temporal hacia el módulo de Personas - Metodos de pago
        /// </summary>
        /// <param name="operationId">Número del movimiento</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/AuthorizationFunctionPerssonApi/CreatePaymentMethodsAuthorization")]
        public IHttpActionResult CreatePaymentMethodsAuthorization([FromUri]int operationId)
        {
            try
            {
                using (WrapperAuthorizationPoliciesServiceClient client = new WrapperAuthorizationPoliciesServiceClient("basicEndpoint"))
                {
                    client.CreatePaymentMethodsAuthorization(operationId);
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", $" Error en CreatePaymentMethodsAuthorization: {ex.Message}");
                return this.BadRequest(ex.Message);
            }

            return this.Ok();
        }

        /// <summary>
        /// Redirige el número de temporal hacia el módulo de Personas - Transferencias bancarias
        /// </summary>
        /// <param name="operationId">Número del movimiento</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/AuthorizationFunctionPerssonApi/CreateBankTransfersAuthorization")]
        public IHttpActionResult CreateBankTransfersAuthorization([FromUri]int operationId)
        {
            try
            {
                using (WrapperAuthorizationPoliciesServiceClient client = new WrapperAuthorizationPoliciesServiceClient("basicEndpoint"))
                {
                    client.CreateBankTransfersAuthorization(operationId);
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", $" Error en CreateBankTransfersAuthorization: {ex.Message}");
                return this.BadRequest(ex.Message);
            }

            return this.Ok();
        }

        /// <summary>
        /// Redirige el número de temporal hacia el módulo de Personas - Consorciados
        /// </summary>
        /// <param name="operationId">Número del movimiento</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/AuthorizationFunctionPerssonApi/CreateConsortiatesAuthorization")]
        public IHttpActionResult CreateConsortiatesAuthorization([FromUri]int operationId)
        {
            try
            {
                using (WrapperAuthorizationPoliciesServiceClient client = new WrapperAuthorizationPoliciesServiceClient("basicEndpoint"))
                {
                    client.CreateConsortiatesAuthorization(operationId);
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", $" Error en CreateConsortiatesAuthorization: {ex.Message}");
                return this.BadRequest(ex.Message);
            }

            return this.Ok();
        }

        /// <summary>
        /// Redirige el número de temporal hacia el módulo de Personas - Razón Social
        /// </summary>
        /// <param name="operationId">Número del movimiento</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/AuthorizationFunctionPerssonApi/CreateBusinessNameAuthorization")]
        public IHttpActionResult CreateBusinessNameAuthorization([FromUri]int operationId)
        {
            try
            {
                using (WrapperAuthorizationPoliciesServiceClient client = new WrapperAuthorizationPoliciesServiceClient("basicEndpoint"))
                {
                    client.CreateBusinessNameAuthorization(operationId);
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", $" Error en CreateBusinessNameAuthorization: {ex.Message}");
                return this.BadRequest(ex.Message);
            }

            return this.Ok();
        }

        /// <summary>
        /// Redirige el número de temporal hacia el módulo de Personas - Contragarantias
        /// </summary>
        /// <param name="operationId">Número del movimiento</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/AuthorizationFunctionPerssonApi/CreateGuaranteeAuthorization")]
        public IHttpActionResult CreateGuaranteeAuthorization([FromUri]int operationId)
        {
            try
            {
                using (WrapperAuthorizationPoliciesServiceClient client = new WrapperAuthorizationPoliciesServiceClient("basicEndpoint"))
                {
                    client.CreateGuaranteeAuthorization(operationId);
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", $" Error en CreateGuaranteeAuthorization: {ex.Message}");
                return this.BadRequest(ex.Message);
            }

            return this.Ok();
        }

        /// <summary>
        /// Redirige el número de temporal hacia el módulo de Personas - Información Basica
        /// </summary>
        /// <param name="operationId">Número del movimiento</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Api/AuthorizationFunctionBasicInfoApi/CreateBasicInfoAuthorization")]
        public IHttpActionResult CreateBasicInfoAuthorization([FromUri]int operationId)
        {
            try
            {
                using (WrapperAuthorizationPoliciesServiceClient client = new WrapperAuthorizationPoliciesServiceClient("basicEndpoint"))
                {
                    client.CreateBasicInfoAuthorization(operationId);
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", $" Error en CreateBasicInfoAuthorization: {ex.Message}");
                return this.BadRequest(ex.Message);
            }

            return this.Ok();
        }

    }
}