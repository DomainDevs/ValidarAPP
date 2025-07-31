// -----------------------------------------------------------------------
// <copyright file="InfringementLogBusiness.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Jeison Rodriguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.CommonParamService.Business
{
    using Sistran.Core.Application.ModelServices.Models.Common;

    /// <summary>
    /// Validacion Log Infraccion
    /// </summary>
    public class InfringementLogBusiness
    {
        /// <summary>
        /// Validacion Log Discontinuidad
        /// </summary>
        /// <param name="infringementLogLogServiceModel">Modelo InfringementLogServiceModel</param>
        /// <returns>Retorna Modelo InfringementLogServiceModel</returns>
        public InfringementLogServiceModel ValidateDiscontinuityLog(InfringementLogServiceModel infringementLogLogServiceModel)
        {
            if (infringementLogLogServiceModel == null)
            {
                infringementLogLogServiceModel = new InfringementLogServiceModel();
            }

            return infringementLogLogServiceModel;
        }
    }
}
