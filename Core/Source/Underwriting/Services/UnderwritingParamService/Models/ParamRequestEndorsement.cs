// -----------------------------------------------------------------------
// <copyright file="ParamRequestEndorsement.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models
{
    using Sistran.Core.Application.UnderwritingParamService.Models.Base;
    using Sistran.Core.Application.Utilities.Error;
    /// <summary>
    /// Modelo de solicitud agrupadora con endoso.
    /// </summary>
    public class ParamRequestEndorsement: BaseParamRequestEndorsement
    {
     

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamRequestEndorsement"/>.
        /// </summary>
        /// <param name="requestEndorsementId">Identificador de la solicitud agrupadora con endoso.</param>
        /// <param name="requestId">Solicitud agrupadora.</param>
        /// <param name="productId">Id del producto asociado</param>
        /// <param name="prefixCode">Id del ramo comercial asociado</param>
        private ParamRequestEndorsement(int requestEndorsementId, int requestId, int productId, int prefixCode):
            base(requestEndorsementId, requestId, productId, prefixCode)
        {
        }

        
        /// <summary>
        /// Objeto que crea u obtiene la solicitud agrupadora con endoso.
        /// </summary>
        /// <param name="requestEndorsementId">Identificador de la solicitud agrupadora con endoso.</param>
        /// <param name="requestId">Solicitud agrupadora.</param>
        /// <param name="productId">Id del producto asociado</param>
        /// <param name="prefixCode">Id del ramo comercial asociado</param>
        /// <returns>Retorna el modelo de la solicitud agrupadora con endoso o un error.</returns>
        public static Result<ParamRequestEndorsement, ErrorModel> GetParamRequestEndorsement(int requestEndorsementId, int requestId, int productId, int prefixCode)
        {
            return new ResultValue<ParamRequestEndorsement, ErrorModel>(new ParamRequestEndorsement(requestEndorsementId, requestId, productId, prefixCode));
        }
    }
}

