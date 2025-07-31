// -----------------------------------------------------------------------
// <copyright file="ParamBusiness.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    using System.Collections.Generic;
    using Sistran.Core.Application.Extensions;
    using Sistran.Core.Application.Utilities.Enums;
    using Sistran.Core.Application.Utilities.Error;

    /// <summary>
    /// Modelo de negocio de los tipos de riesgo cubierto.
    /// </summary>
    public class BaseParamBusiness: Extension
    {
        /// <summary>
        /// Id del negocio.
        /// </summary>
        private readonly int businessId;

        /// <summary>
        /// Descripción del negocio.
        /// </summary>
        private readonly string description;

        /// <summary>
        /// Negocio habilitado.
        /// </summary>
        private readonly bool isEnabled;

        
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamBusiness"/>.
        /// </summary>
        /// <param name="businessId">Identificador del negocio.</param>
        /// <param name="description">Descripción del negocio.</param>
        /// <param name="isEnabled">Negocio habilitado.</param>
        /// <param name="prefix">Ramo comercial asociado.</param>
        protected BaseParamBusiness(int businessId, string description, bool isEnabled)
        {
            this.businessId = businessId;
            this.description = description;
            this.isEnabled = isEnabled;
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
        /// Obtiene la Descripción del negocio.
        /// </summary>
        public string Description
        {
            get
            {
                return this.description;
            }
        }

        /// <summary>
        /// Obtiene un valor que indica si el negocio está habilitado.
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return this.isEnabled;
            }
        }
        
    }
}
