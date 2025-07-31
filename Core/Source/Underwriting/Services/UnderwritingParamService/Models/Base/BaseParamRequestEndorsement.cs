// -----------------------------------------------------------------------
// <copyright file="ParamRequestEndorsement.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    using Sistran.Core.Application.Extensions;
    using Sistran.Core.Application.Utilities.Error;

    /// <summary>
    /// Modelo de solicitud agrupadora con endoso.
    /// </summary>
    public class BaseParamRequestEndorsement: Extension
    {
        /// <summary>
        /// Id de la solicitud agrupadora con endoso.
        /// </summary>
        private readonly int requestEndorsementId;

        /// <summary>
        /// Solicitud agrupadora.
        /// </summary>
        private readonly int requestId;

        /// <summary>
        /// Id del producto asociado.
        /// </summary>
        private readonly int productId;

        /// <summary>
        /// Id del ramo comercial.
        /// </summary>
        private readonly int prefixCode;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamRequestEndorsement"/>.
        /// </summary>
        /// <param name="requestEndorsementId">Identificador de la solicitud agrupadora con endoso.</param>
        /// <param name="requestId">Solicitud agrupadora.</param>
        /// <param name="productId">Id del producto asociado</param>
        /// <param name="prefixCode">Id del ramo comercial asociado</param>
        protected BaseParamRequestEndorsement(int requestEndorsementId, int requestId, int productId, int prefixCode)
        {
            this.requestEndorsementId = requestEndorsementId;
            this.requestId = requestId;
            this.productId = productId;
            this.prefixCode = prefixCode;
        }

        /// <summary>
        /// Obtiene el Id de la solicitud agrupadora con endoso.
        /// </summary>
        public int RequestEndorsementId
        {
            get
            {
                return this.requestEndorsementId;
            }
        }

        /// <summary>
        /// Obtiene la solicitud agrupadora.
        /// </summary>
        public int RequestId
        {
            get
            {
                return this.requestId;
            }
        }

        /// <summary>
        /// Obtiene el id del producto.
        /// </summary>
        public int ProductId
        {
            get
            {
                return this.productId;
            }
        }

        /// <summary>
        /// Obtiene el id del ramo comercial.
        /// </summary>
        public int PrefixCode
        {
            get
            {
                return this.prefixCode;
            }
        }

        
    }
}

