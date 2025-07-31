// -----------------------------------------------------------------------
// <copyright file="ModelAssembler.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>SISTRAN\Camila Vergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.AuthorizationPoliciesParamService.EEProvider.Assemblers
{
    using Sistran.Core.Framework.DAF;
    using System.Collections.Generic;
    using UNUEN = Sistran.Core.Application.UniqueUser.Entities;
    using UNUENAUTO = Sistran.Core.Application.AuthorizationPolicies.Entities;

    /// <summary>
    /// Define la clase <see cref="ModelAssembler" />
    /// </summary>
    public class ModelAssembler
    {
        /// <summary>
        /// Metodo para crear Delegado
        /// </summary>
        /// <param name="delegation">Recibe Delegado</param>
        /// <returns>Retorna Delegado</returns>
        public static Models.ParamHierarchyAssociation CreateDelegation(UNUEN.CoHierarchyAssociation delegation)
        {
            Models.ParamHierarchyAssociation modules = new Models.ParamHierarchyAssociation
            {
                IsEnabled = delegation.EnabledInd,
                IsExclusionary = delegation.ExclusionaryInd,
                Description = delegation.Description,
                ParamModulen = new Models.ParamModule()
                {
                    Id = delegation.ModuleCode
                },
                ParamSubModule = new Models.ParamSubModule()
                {
                    Id = delegation.SubmoduleCode
                },
                ParamHierarchy = new Models.ParamHierarchy()
                {
                    Id = delegation.HierarchyCode
                }
            };
            return modules;
        }

        /// <summary>
        /// Metodo contiene lista de delegados
        /// </summary>
        /// <param name="collection">Recibe coleccion de delegados</param>
        /// <returns>Retorna coleccion de delegados</returns>
        public static List<Models.ParamHierarchyAssociation> CreateDelegations(BusinessCollection collection)
        {
            List<Models.ParamHierarchyAssociation> delegations = new List<Models.ParamHierarchyAssociation>();
            foreach (UNUEN.CoHierarchyAssociation item in collection)
            {
                delegations.Add(CreateDelegation(item));
            }

            return delegations;
        }

            /// <summary>
        /// Metodo contiene lista de jerarquias
        /// </summary>
        /// <param name="collection">Recibe coleccion de jerarquias</param>
        /// <returns>Retorna coleccion de jerarquias</returns>
        public static List<Models.ParamHierarchy> CreateHierarchies(BusinessCollection collection)
        {
            List<Models.ParamHierarchy> hierarchy = new List<Models.ParamHierarchy>();
            foreach (UNUEN.CoHierarchy item in collection)
            {
                hierarchy.Add(CreateHierarchy(item));
            }

            return hierarchy;
        }

        /// <summary>
        /// Metodo para crear Jerarquia
        /// </summary>
        /// <param name="hierarchuy">Recibe Jerarquia</param>
        /// <returns>Retorna Jerarquia</returns>
        public static Models.ParamHierarchy CreateHierarchy(UNUEN.CoHierarchy hierarchuy)
        {
            Models.ParamHierarchy subModules = new Models.ParamHierarchy
            {
                Id = hierarchuy.HierarchyCode,
                Description = hierarchuy.Description
            };
            return subModules;
        }

        /// <summary>
        /// Metodo para crear Modulo
        /// </summary>
        /// <param name="module">Recibe modulos</param>
        /// <returns>Retorna modulos</returns>
        public static Models.ParamModule CreateModule(UNUEN.Modules module)
        {
            Models.ParamModule modules = new Models.ParamModule
            {
                Id = module.ModuleCode,
                Description = module.Description
            };
            return modules;
        }

        /// <summary>
        /// Metodo contiene lista de modulos
        /// </summary>
        /// <param name="collection">Recibe coleccion de modulos</param>
        /// <returns>Retorna coleccion de modulos</returns>
        public static List<Models.ParamModule> CreateModules(BusinessCollection collection)
        {
            List<Models.ParamModule> modules = new List<Models.ParamModule>();
            foreach (UNUEN.Modules item in collection)
            {
                modules.Add(CreateModule(item));
            }

            return modules;
        }

        /// <summary>
        /// Metodo para crear SubModulo
        /// </summary>
        /// <param name="subModule">Recibe SubModulos</param>
        /// <returns>Retorna SubModulos</returns>
        public static Models.ParamSubModule CreateSubModule(UNUEN.Submodules subModule)
        {
            Models.ParamSubModule subModules = new Models.ParamSubModule
            {
                Id = subModule.SubmoduleCode,
                Description = subModule.Description,
                ModuleId = subModule.ModuleCode
            };
            return subModules;
        }

        /// <summary>
        /// Metodo contiene lista de modulos
        /// </summary>
        /// <param name="collection">Recibe coleccion de modulos</param>
        /// <returns>Retorna coleccion de modulos</returns>
        public static List<Models.ParamSubModule> CreateSubModules(BusinessCollection collection)
        {
            List<Models.ParamSubModule> subModules = new List<Models.ParamSubModule>();
            foreach (UNUEN.Submodules item in collection)
            {
                subModules.Add(CreateSubModule(item));
            }

            return subModules;
        }

        #region BaseRejectionCauses

        /// <summary>
        /// Metodo contiene lista de grupo de politicas
        /// </summary>
        /// <param name="collection">Recibe coleccion de modulos</param>
        /// <returns>Retorna coleccion de modulos</returns>
        public static List<Models.ParamBaseGroupPolicies> CreateBaseGroupPolicies(BusinessCollection collection)
        {
            List<Models.ParamBaseGroupPolicies> modules = new List<Models.ParamBaseGroupPolicies>();
            foreach (UNUENAUTO.GroupPolicies item in collection)
            {
                modules.Add(CreateBaseGroupPolicies(item));
            }

            return modules;
        }

        /// <summary>
        /// Metodo para crear grupo de politicas
        /// </summary>
        /// <param name="ParamBaseGroupPolicies">Recibe modulos</param>
        /// <returns>Retorna modulos</returns>
        public static Models.ParamBaseGroupPolicies CreateBaseGroupPolicies(UNUENAUTO.GroupPolicies groupPolicies)
        {
            Models.ParamBaseGroupPolicies groupPolice = new Models.ParamBaseGroupPolicies
            {
                id = groupPolicies.GroupPoliciesId,
                description = groupPolicies.Description
            };
            return groupPolice;
        }

        /// <summary>
        /// Metodo para crear metodo de rechazo
        /// </summary>
        /// <param name="ParamBaseEjectionCauses">Recibe modulos</param>
        /// <returns>Retorna modulos</returns>
        public static Models.ParamBaseEjectionCauses CreateBaseRejectionCauses(UNUENAUTO.RejectionCauses rejectionCauses)
        {
            Models.ParamBaseEjectionCauses RejectionCauses = new Models.ParamBaseEjectionCauses
            {
                Id = rejectionCauses.GroupPoliciesId,
                Description = rejectionCauses.Description,
                paramBaseGroupPolicies = new Models.ParamBaseGroupPolicies { id = rejectionCauses.GroupPoliciesId, description=rejectionCauses.Description }
            };
            return RejectionCauses;
        }

        /// <summary>
        /// Metodo para crear metodo de rechazo
        /// </summary>
        /// <param name="ParamBaseEjectionCauses">Recibe modulos</param>
        /// <returns>Retorna modulos</returns>
        public static Models.ParamBaseEjectionCauses CreateRejectionCauses(UNUENAUTO.RejectionCauses rejectionCauses, UNUENAUTO.GroupPolicies groupPolicies)
        {
            Models.ParamBaseEjectionCauses RejectionCauses = new Models.ParamBaseEjectionCauses
            {
                Id = rejectionCauses.RejectionCausesId,
                Description = rejectionCauses.Description,
                paramBaseGroupPolicies = new Models.ParamBaseGroupPolicies { id=groupPolicies.GroupPoliciesId, description=groupPolicies.Description }
            };
            return RejectionCauses;
        }
        #endregion

    }
}
