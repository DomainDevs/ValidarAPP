// -----------------------------------------------------------------------
// <copyright file="ParamClauseModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres F. Gonzalez R.</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    using Sistran.Core.Application.Extensions;
    using Sistran.Core.Application.Utilities.Error;

    /// <summary>
    /// Contiene las propiedades de la Cláusula
    /// </summary>
    public class BaseParamClauseModel: Extension
    {
        /// <summary>
        /// ID de Cláusula.
        /// </summary>
        private readonly int id;

        /// <summary>
        /// Descripción de Cláusula.
        /// </summary>
        private readonly string description;

        /// <summary>
        /// Obligatorio de Cláusula.
        /// </summary>
        private readonly bool isMandatory;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamClauseModel"/>.
        /// </summary>
        /// <param name="id">Id de Cláusula.</param>
        /// <param name="description">Descripción de Cláusula.</param>
        /// <param name="isMandatory">Obligatorio de Cláusula.</param>
        protected BaseParamClauseModel(int id, string description, bool isMandatory)
        {
            this.id = id;
            this.description = description;
            this.isMandatory = isMandatory;
        }

        /// <summary>
        /// Obtiene el Id de Cláusula.
        /// </summary>
        public int Id
        {
            get
            {
                return this.id;
            }
        }

        /// <summary>
        /// Obtiene la Descripción de Cláusula.
        /// </summary>
        public string Description
        {
            get
            {
                return this.description;
            }
        }

        /// <summary>
        /// Obtiene el Obligatorio de Cláusula.
        /// </summary>
        public bool IsMandatory
        {
            get
            {
                return this.isMandatory;
            }
        }

       
    }
}