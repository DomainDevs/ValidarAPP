// -----------------------------------------------------------------------
// <copyright file="CoreAssembler.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>SISTRAN\Stiveen Niño</author>
// -----------------------------------------------------------------------

namespace Company.AuthorizationPoliciesParamService.EEProvider.CoreAssemblers
{
    using Company.AuthorizationPoliciesParamServices.Models;
    using System.Collections.Generic;
    using ModelCore = Sistran.Core.Application.AuthorizationPoliciesParamService.Models;

    /// <summary>
    /// Define la clase <see cref="CompanyCoreAssemblers" />
    /// </summary>
    public class CompanyCoreAssemblers
    {
        #region BaseEjectionCauses
        /// <summary>
        /// Metodo Modulo
        /// </summary>
        /// <param name="module">Recibe modulo</param>
        /// <returns>Retorna listado de modelo a Core</returns>
        public static List<ModelCore.ParamBaseEjectionCauses> CreateCoreBaseEjectionCauses(List<CompanyParamBaseEjectionCauses> coreBaseEjectionCauses)
        {
            List<ModelCore.ParamBaseEjectionCauses> baseEjectionCauses = new List<ModelCore.ParamBaseEjectionCauses>();
            foreach (var item in coreBaseEjectionCauses)
            {
                baseEjectionCauses.Add(CreateCoreBaseEjectionCause(item));
            }
            return baseEjectionCauses;  
        }

        /// <summary>
        /// Metodo Modulo
        /// </summary>
        /// <param name="module">Recibe modulo</param>
        /// <returns>Retorna individual modelo core</returns>
        public static ModelCore.ParamBaseEjectionCauses CreateCoreBaseEjectionCause(CompanyParamBaseEjectionCauses coreBaseEjectionCause)
        {
            ModelCore.ParamBaseEjectionCauses BaseEjectionCause = new ModelCore.ParamBaseEjectionCauses();
            BaseEjectionCause.Id = coreBaseEjectionCause.Id;
            BaseEjectionCause.Description = coreBaseEjectionCause.Description;
            BaseEjectionCause.paramBaseGroupPolicies = new ModelCore.ParamBaseGroupPolicies { id = coreBaseEjectionCause.paramBaseGroupPolicies.id, description = coreBaseEjectionCause.paramBaseGroupPolicies.description };
            return BaseEjectionCause;
        }

        #endregion
    }
}
