// -----------------------------------------------------------------------
// <copyright file="ParamBusinessConfiguration.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    using System.Collections.Generic;
    using Sistran.Core.Application.Utilities.Enums;
    using Sistran.Core.Application.Utilities.Error;

    /// <summary>
    /// Modelo de acuerdo de negocio.
    /// </summary>
    public class BaseParamBusinessConfiguration
    {
        /// <summary>
        /// Id del acuerdo de negocio.
        /// </summary>
        private readonly int businessConfigurationId;

        /// <summary>
        /// Id del negocio.
        /// </summary>
        private readonly int businessId;

        
        /// <summary>
        /// Id producto respuesta.
        /// </summary>
        private readonly string productIdResponse;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamBusinessConfiguration"/>.
        /// </summary>
        /// <param name="businessConfigurationId">Id del acuerdo de negocio.</param>
        /// <param name="businessId">Id del negocio.</param>
        /// <param name="request">Solicitud agrupadora del acuerdo de negocio.</param>
        /// <param name="product">Producto del acuerdo de negocio.</param>
        /// <param name="groupCoverage">Grupo de cobertura del acuerdo de negocio.</param>
        /// <param name="assistance">Tipo de asistencia del acuerdo de negocio.</param>
        /// <param name="productIdResponse">Id producto respuesta.</param>
        protected BaseParamBusinessConfiguration(int businessConfigurationId, int businessId, string productIdResponse)
        {
            this.businessConfigurationId = businessConfigurationId;
            this.businessId = businessId;
            
            this.productIdResponse = productIdResponse;
        }

        /// <summary>
        /// Obtiene el Id del acuerdo de negocio.
        /// </summary>
        public int BusinessConfigurationId
        {
            get
            {
                return this.businessConfigurationId;
            }
        }

        /// <summary>
        /// Obtiene el Id del negocio.
        /// </summary>
        public int BusinessId
        {
            get
            {
                return this.businessId;
            }
        }

        

        /// <summary>
        /// Obtiene el id producto respuesta.
        /// </summary>
        public string ProductIdResponse
        {
            get
            {
                return this.productIdResponse;
            }
        }
       
    }
}

