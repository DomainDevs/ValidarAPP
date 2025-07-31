// -----------------------------------------------------------------------
// <copyright file="CoverageBusiness.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.Business
{
    using System.Collections.Generic;
    using ENUMSM = Sistran.Core.Application.ModelServices.Enums;
    using MODSM = Sistran.Core.Application.ModelServices.Models.Param;
    using PARUPSM = Sistran.Core.Application.ModelServices.Models.UnderwritingParam;

    /// <summary>
    /// Validaciones de negocio en modelo de cobertura
    /// </summary>
    public class CoverageBusiness
    {
        /// <summary>
        /// Validacion de cobertura a crear 
        /// </summary>
        /// <param name="validateRelation">Coberturas relacionadas existentes</param>        
        /// <returns>resultado de validacion, modelo ErrorServiceModel</returns>
        public MODSM.ErrorServiceModel ValidateCoverage(PARUPSM.CoveragesServiceModel validateRelation)
        {
            MODSM.ErrorServiceModel result = new MODSM.ErrorServiceModel();
            result.ErrorTypeService = ENUMSM.ErrorTypeService.Ok;
            result.ErrorDescription = new List<string>();
            if (validateRelation.ErrorTypeService == ENUMSM.ErrorTypeService.TechnicalFault)
            {
                result.ErrorDescription = validateRelation.ErrorDescription;
                result.ErrorTypeService = ENUMSM.ErrorTypeService.BusinessFault;
            }
            else if (validateRelation.CoverageServiceModels.Count > 0)
            {
                result.ErrorDescription.Add(Resources.Errors.ValidateRelationCoverage);
                result.ErrorTypeService = ENUMSM.ErrorTypeService.BusinessFault;
            }
            return result;
        }
    }
}
