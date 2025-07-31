// -----------------------------------------------------------------------
// <copyright file="EntityAssembler.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>SISTRAN\Camila Vergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.AuthorizationPoliciesParamService.EEProvider.Assemblers
{
    using UNUEN = Sistran.Core.Application.UniqueUser.Entities;
    using AUTHOEN = Sistran.Core.Application.AuthorizationPolicies.Entities;
    using Sistran.Core.Application.AuthorizationPoliciesParamService.Models;

    /// <summary>
    /// Defina la clase <see cref="EntityAssembler" />
    /// </summary>
    public class EntityAssembler
    {
        /// <summary>
        /// Metodo para crear Delegacion
        /// </summary>
        /// <param name="delegation">Contiene Delegacion</param>
        /// <returns>retorna delegaciones</returns>
        public static UNUEN.CoHierarchyAssociation CreateDelegationParam(Models.ParamHierarchyAssociation delegation)
        {
            return new UNUEN.CoHierarchyAssociation(delegation.ParamModulen.Id, delegation.ParamSubModule.Id, delegation.ParamHierarchy.Id)
            {
                Description = delegation.Description,
                ExclusionaryInd = delegation.IsExclusionary,
                EnabledInd = delegation.IsEnabled
            };
        }

        /// <summary>
        /// Metodo para crear modulo
        /// </summary>
        /// <param name="module">Contiene modulo</param>
        /// <returns>retorna modulos</returns>
        public static UNUEN.Modules CreateModuleParam(Models.ParamModule module)
        {
            return new UNUEN.Modules()
            {
                ModuleCode = module.Id,
                Description = module.Description
            };
        }

        #region RejectionCauses

        /// <summary>
        /// Metodo para crear modulo
        /// </summary>
        /// <param name="module">Contiene modulo</param>
        // /// <returns>retorna modulos</returns>

        public static AUTHOEN.RejectionCauses CreateBaseRejection(ParamBaseEjectionCauses paramBaseEjectionCauses)
        {
            
            return new AUTHOEN.RejectionCauses(paramBaseEjectionCauses.Id)
            {                
                Description = paramBaseEjectionCauses.Description,
                GroupPoliciesId = paramBaseEjectionCauses.paramBaseGroupPolicies.id
            };
}
        #endregion
    }
}
