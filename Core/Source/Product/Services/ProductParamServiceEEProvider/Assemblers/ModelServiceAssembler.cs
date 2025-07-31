// -----------------------------------------------------------------------
// <copyright file="ModelServiceAssembler.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Eduardo Rodríguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamServices.EEProvider.Assemblers
{
    using System.Collections.Generic;
    using Sistran.Core.Application.ModelServices.Enums;    
    using Sistran.Core.Application.ModelServices.Models.Underwriting;
    using Sistran.Core.Application.UnderwritingParamService.Models;

    /// <summary>
    /// Clase ensambladora para mapear modelos de negocio a modelos de Servicio.
    /// </summary>
    public class ModelServiceAssembler
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ModelServiceAssembler"/>.
        /// </summary>
        protected ModelServiceAssembler()
        {
        }

        /// <summary>
        /// Mapear modelos de negocio ParamCoveredRiskType a modelos de Servicio CoveredRiskTypesServiceModel.
        /// </summary>
        /// <param name="paramCoveredRiskTypes">Modelos de negocio ParamCoveredRiskType.</param>
        /// <returns>Modelos de Servicio CoveredRiskTypesServiceModel</returns>
        public static CoveredRiskTypesServiceModel MappCoveredRiskTypes(List<ParamCoveredRiskType> paramCoveredRiskTypes)
        {
            CoveredRiskTypesServiceModel coveredRiskTypesServiceModel = new CoveredRiskTypesServiceModel();            
            List<CoveredRiskTypeServiceModel> listCoveredRiskTypeServiceModel = new List<CoveredRiskTypeServiceModel>();
            foreach (ParamCoveredRiskType coveredRiskTypeBusinessModel in paramCoveredRiskTypes)
            {
                CoveredRiskTypeServiceModel itemCoveredRiskTypeServiceModel = new CoveredRiskTypeServiceModel();
                itemCoveredRiskTypeServiceModel.Id = coveredRiskTypeBusinessModel.Id;
                itemCoveredRiskTypeServiceModel.SmallDescription = coveredRiskTypeBusinessModel.SmallDescription;                
                listCoveredRiskTypeServiceModel.Add(itemCoveredRiskTypeServiceModel);
            }

            coveredRiskTypesServiceModel.ErrorDescription = new List<string>();
            coveredRiskTypesServiceModel.ErrorTypeService = ErrorTypeService.Ok;

            coveredRiskTypesServiceModel.CoveredRiskTypeServiceModel = listCoveredRiskTypeServiceModel;            

            return coveredRiskTypesServiceModel;
        }
    }
}
