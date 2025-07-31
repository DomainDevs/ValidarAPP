// -----------------------------------------------------------------------
// <copyright file="CoreAssembler.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>SISTRAN\Stiveen Niño</author>
// -----------------------------------------------------------------------


namespace Company.AuthorizationPoliciesParamService.EEProvider.ServicesAssemblers
{
    using Company.AuthorizationPoliciesParamServices.Models;
    using System.Collections.Generic;
    using MODEL=Sistran.Company.Application.ModelServices.Models.AuthorizationPolicies;
    using Sistran.Core.Application.AuthorizationPoliciesParamService.Models;
    
    public class ServiceModelAssembler
    {
        #region BaseEjectionCauses
        /// <summary>
        /// Convierte el listado del MOD-S ParamBaseEjection al MOD-B
        /// </summary>
        /// <param name="ParamBaseEjection">BaseEjection MOD-S</param>
        /// <returns>BaseEjection MOD-B</returns>
        public static List<CompanyParamBaseEjectionCauses> CreateParametrizationBaseEjectionCauses(List<MODEL.RejectionCauseServiceModel> RejectionCauseServiceModel)
        {
            List<CompanyParamBaseEjectionCauses> CompanyParamBaseEjectionCauses = new List<CompanyParamBaseEjectionCauses>();
            foreach (var item in RejectionCauseServiceModel)
            {
                CompanyParamBaseEjectionCauses.Add(CreateParamBaseEjectionCauses(item));
            }

            return CompanyParamBaseEjectionCauses;
        }

        /// <summary>
        /// Metodo convierte de modelo a servicio Delegacion
        /// </summary>
        /// <param name="ParamBaseEjection">Recibe Delegacion</param>
        /// <returns>Retorna Delegacion</returns>
        public static CompanyParamBaseEjectionCauses CreateParamBaseEjectionCauses(MODEL.RejectionCauseServiceModel ParamBaseEjectionCauses)
        {
            return new CompanyParamBaseEjectionCauses()
            {
               Id = ParamBaseEjectionCauses.id,
               Description = ParamBaseEjectionCauses.description,
               paramBaseGroupPolicies = new ParamBaseGroupPolicies { id= ParamBaseEjectionCauses.GroupPolicies.id , description= ParamBaseEjectionCauses.GroupPolicies.description }
            };
        }
        #endregion
    }
}
