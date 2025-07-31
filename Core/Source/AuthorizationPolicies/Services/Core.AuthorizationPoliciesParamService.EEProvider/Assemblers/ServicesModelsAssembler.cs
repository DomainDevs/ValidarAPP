// -----------------------------------------------------------------------
// <copyright file="ServicesModelsAssembler.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>SISTRAN\Camila Vergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.AuthorizationPoliciesParamService.EEProvider.Assemblers
{
    using System.Collections.Generic;
    using Sistran.Core.Application.AuthorizationPoliciesParamService.Models;
    using MODUD = Sistran.Core.Application.ModelServices.Models.AuthorizationPolicies;

    /// <summary>
    /// Define la clase <see cref="ServicesModelsAssembler" />
    /// </summary>
    public class ServicesModelsAssembler
    {
        /// <summary>
        /// Convierte el MOD-S de delegaciones al MOD-B
        /// </summary>
        /// <param name="delegationServiceModel">Delegacion MOD-S</param>
        /// <returns>Delegacion MOD-B</returns>
        public static ParamHierarchyAssociation CreateParametrizationDelegation(MODUD.HierarchyAssociationServiceModel delegationServiceModel)
        {
            return new ParamHierarchyAssociation()
            {
                Description = delegationServiceModel.Description,
                ParamHierarchy = new ParamHierarchy()
                {
                    Id = delegationServiceModel.HierarchyServiceQueryModel.Id,
                    Description = delegationServiceModel.HierarchyServiceQueryModel.Description
                },
                ParamModulen = new ParamModule()
                {
                    Id = delegationServiceModel.ModuleServiceQueryModel.Id,
                    Description = delegationServiceModel.ModuleServiceQueryModel.Description
                },
                ParamSubModule = new ParamSubModule()
                {
                    Id = delegationServiceModel.SubModuleServicesQueryModel.Id,
                    Description = delegationServiceModel.SubModuleServicesQueryModel.Description
                },
                IsEnabled = delegationServiceModel.IsEnabled,
                IsExclusionary = delegationServiceModel.IsExclusionary
            };
        }

        /// <summary>
        /// Convierte el listado del MOD-S de delegaciones al MOD-B
        /// </summary>
        /// <param name="delegationServiceModels">Delegaciones MOD-S</param>
        /// <returns>Delegaciones MOD-B</returns>
        public static List<ParamHierarchyAssociation> CreateParametrizationDelegations(List<MODUD.HierarchyAssociationServiceModel> delegationServiceModels)
        {
            List<ParamHierarchyAssociation> parametrizationDelegation = new List<ParamHierarchyAssociation>();
            foreach (var item in delegationServiceModels)
            {
                parametrizationDelegation.Add(CreateParametrizationDelegation(item));
            }

            return parametrizationDelegation;
        }
    }
}
