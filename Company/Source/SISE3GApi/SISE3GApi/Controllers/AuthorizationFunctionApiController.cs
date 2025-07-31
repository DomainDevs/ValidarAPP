namespace Sistran.Company.SISE3GApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Web.Http;
    using WrapperAuthoPoliciesService;

    /// <summary>
    /// Servicio Web API - Módulo de Emision
    /// </summary>
    public class AuthorizationFunctionApiController : ApiController
    {
        #region Interactivo
        /// <summary>
        /// Redirige el número de temporal hacia el módulo de Emisión
        /// </summary>
        /// <param name="TemporalId">Número del Temporal</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("CreatePolicyAuthorization")]
        [Route("Api/AuthorizationFunctionApi/CreatePolicyAuthorization")]
        public IHttpActionResult CreatePolicyAuthorization([FromUri]int TemporalId)
        {
            try
            {
                using (WrapperAuthorizationPoliciesServiceClient client = new WrapperAuthorizationPoliciesServiceClient("basicEndpoint"))
                {
                    client.CreatePolicyAuthorization(TemporalId);
                }

            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", string.Format(" Error en CreatePolicyAuthorization: {0}", ex.Message));
                return this.BadRequest(ex.Message);
            }

            return this.Ok();
        }

        #endregion Interactivo

        #region Masivos

        //Autorizacion Collectiva
        /// <summary>
        /// Redirige el número del cargue, número del temporal y lista de riesgos hacia el módulo de Colectivas
        /// </summary>
        /// <param name="loadId">Número del cargue</param>
        /// <param name="temporalId">Número del temporal</param>
        /// <param name="risks">Lista de riesgos</param>
        /// <returns></returns>
        [Obsolete] //se Adiciona el atributo para el proyecto NASE - No existe funcionalidad colectiva
        [HttpPost]
        [Route("Api/AuthorizationFunctionApi/UpdateCollectiveLoadAuthorization")]
        public IHttpActionResult UpdateCollectiveLoadAuthorization([FromUri]int loadId, [FromUri]int temporalId, [FromBody]IEnumerable<int> risks)
        {
            try
            {
                using (WrapperAuthorizationPoliciesServiceClient client = new WrapperAuthorizationPoliciesServiceClient("basicEndpoint"))
                {
                    ArrayOfInt arrayOfInt = new ArrayOfInt();
                    arrayOfInt.AddRange(risks);
                    client.UpdateCollectiveLoadAuthorization(loadId, temporalId, arrayOfInt);
                }

                return this.Ok();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", string.Format(" Error en UpdateCollectiveLoadAuthorization: {0}", ex.Message));
                return this.BadRequest(ex.Message);
            }
        }

        //Autorizacion Masiva
        /// <summary>
        /// Redirige el número del cargue, número del temporal hacia el módulo de Masivos
        /// </summary>
        /// <param name="loadId">Número del cargue</param>
        /// <param name="temporalId">Número del temporal</param>
        /// <returns></returns>
        [Obsolete] //se Adiciona el atributo para el proyecto NASE - No existe funcionalidad masiva
        [HttpPost]
        [Route("Api/AuthorizationFunctionApi/CompanyUpdateMassiveLoadAuthorization")]
        public IHttpActionResult CompanyUpdateMassiveLoadAuthorization([FromUri]int loadId, [FromBody]IEnumerable<string> temporalId)
        {
            try
            {

                using (WrapperAuthorizationPoliciesServiceClient client = new WrapperAuthorizationPoliciesServiceClient("basicEndpoint"))
                {
                    ArrayOfString arrayOfString = new ArrayOfString();
                    arrayOfString.AddRange(temporalId);
                    client.CompanyUpdateMassiveLoadAuthorization(loadId.ToString(), arrayOfString);
                }

                return this.Ok();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", string.Format(" Error en CompanyUpdateMassiveLoadAuthorization: {0}", ex.Message));
                return this.BadRequest(ex.Message);
            }
        }
        #endregion Masivos
    }
}