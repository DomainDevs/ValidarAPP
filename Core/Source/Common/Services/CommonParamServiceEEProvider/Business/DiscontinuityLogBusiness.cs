// -----------------------------------------------------------------------
// <copyright file="DiscontinuityLogBusiness.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Jeison Rodriguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.CommonParamService.Business
{
    using Sistran.Core.Application.ModelServices.Models.Common;

    /// <summary>
    /// Validaciones de log Discontinuidad
    /// </summary>
    public class DiscontinuityLogBusiness
    {
        /// <summary>
        /// Validacion Log Discontinuidad
        /// </summary>
        /// <param name="discontinuityLogSM">Modelo DiscontinuityLogServiceModel</param>
        /// <returns>Reportna modelo DiscontinuityLogServiceModel</returns>
        public DiscontinuityLogServiceModel ValidateDiscontinuityLog(DiscontinuityLogServiceModel discontinuityLogSM)
        {
            if (discontinuityLogSM == null)
            {
                discontinuityLogSM = new DiscontinuityLogServiceModel();
            }

            return discontinuityLogSM;
        }
    }
}