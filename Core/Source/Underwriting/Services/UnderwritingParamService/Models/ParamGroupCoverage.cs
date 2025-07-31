// -----------------------------------------------------------------------
// <copyright file="ParamGroupCoverage.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models
{
    using Sistran.Core.Application.UnderwritingParamService.Models.Base;
    using Sistran.Core.Application.Utilities.Error;
    /// <summary>
    /// Modelo del grupo de coberturas.
    /// </summary>
    public class ParamGroupCoverage: BaseParamGroupCoverage
    {
       

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamGroupCoverage"/>.
        /// </summary>
        /// <param name="groupCoverageId">Identificador del grupo de coverturas.</param>
        /// <param name="groupCoverageSmallDescription">Descripción corta del grupo de coverturas.</param>
        private ParamGroupCoverage(int groupCoverageId, string groupCoverageSmallDescription):
            base(groupCoverageId,groupCoverageSmallDescription)
        {
        }

        /// <summary>
        /// Objeto que crea u obtiene el grupo de coverturas.
        /// </summary>
        /// <param name="groupCoverageId">Identificador del grupo de coverturas.</param>
        /// <param name="groupCoverageSmallDescription">Descripción corta del grupo de coverturas.</param>
        /// <returns>Retorna el modelo de grupo de coverturas o un error.</returns>
        public static Result<ParamGroupCoverage, ErrorModel> GetParamGroupCoverage(int groupCoverageId, string groupCoverageSmallDescription)
        {
            return new ResultValue<ParamGroupCoverage, ErrorModel>(new ParamGroupCoverage(groupCoverageId, groupCoverageSmallDescription));
        }
    }
}

