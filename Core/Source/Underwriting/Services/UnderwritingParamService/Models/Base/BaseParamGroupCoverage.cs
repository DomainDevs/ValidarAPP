// -----------------------------------------------------------------------
// <copyright file="ParamGroupCoverage.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    using Sistran.Core.Application.Extensions;
    using Sistran.Core.Application.Utilities.Error;

    /// <summary>
    /// Modelo del grupo de coberturas.
    /// </summary>
    public class BaseParamGroupCoverage: Extension
    {
        /// <summary>
        /// Id del grupo de coverturas.
        /// </summary>
        private readonly int groupCoverageId;

        /// <summary>
        /// Descripción corta del grupo de coverturas.
        /// </summary>
        private readonly string groupCoverageSmallDescription;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamGroupCoverage"/>.
        /// </summary>
        /// <param name="groupCoverageId">Identificador del grupo de coverturas.</param>
        /// <param name="groupCoverageSmallDescription">Descripción corta del grupo de coverturas.</param>
        protected BaseParamGroupCoverage(int groupCoverageId, string groupCoverageSmallDescription)
        {
            this.groupCoverageId = groupCoverageId;
            this.groupCoverageSmallDescription = groupCoverageSmallDescription;
        }

        /// <summary>
        /// Obtiene el Id del grupo de coverturas.
        /// </summary>
        public int GroupCoverageId
        {
            get
            {
                return this.groupCoverageId;
            }
        }

        /// <summary>
        /// Obtiene la Descripción corta del grupo de coverturas.
        /// </summary>
        public string GroupCoverageSmallDescription
        {
            get
            {
                return this.groupCoverageSmallDescription;
            }
        }

        
    }
}

