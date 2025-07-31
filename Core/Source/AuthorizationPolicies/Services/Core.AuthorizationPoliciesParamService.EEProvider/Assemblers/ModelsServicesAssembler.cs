// -----------------------------------------------------------------------
// <copyright file="ModelsServicesAssembler.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>SISTRAN\Camila Vergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.AuthorizationPoliciesParamService.EEProvider.Assemblers
{
    using System.Collections.Generic;
    using AutoMapper;
    using Sistran.Core.Application.AuthorizationPoliciesParamService.Models;
    using MODEN = Sistran.Core.Application.ModelServices.Enums;
    using MODUD = Sistran.Core.Application.ModelServices.Models.AuthorizationPolicies;

    /// <summary>
    /// Define la clase <see cref="ModelsServicesAssembler" />
    /// </summary>
    public class ModelsServicesAssembler
    {
        /// <summary>
        /// Metodo lista de Delegacion
        /// </summary>
        /// <param name="delegation">Recibe Delegacion</param>
        /// <returns>Retorna lista de Delegacion</returns>
        public static List<MODUD.HierarchyAssociationServiceModel> CreateDeleationsServiceModels(List<ParamHierarchyAssociation> delegation)
        {
            List<MODUD.HierarchyAssociationServiceModel> delegationsServiceModel = new List<MODUD.HierarchyAssociationServiceModel>();
            foreach (var item in delegation)
            {
                delegationsServiceModel.Add(CreateDelegationServiceModel(item));
            }

            return delegationsServiceModel;
        }

        /// <summary>
        /// Metodo convierte de modelo a servicio Delegacion
        /// </summary>
        /// <param name="delegation">Recibe Delegacion</param>
        /// <returns>Retorna Delegacion</returns>
        public static MODUD.HierarchyAssociationServiceModel CreateDelegationServiceModel(ParamHierarchyAssociation delegation)
        {
            return new MODUD.HierarchyAssociationServiceModel()
            {
                IsEnabled = delegation.IsEnabled,
                IsExclusionary = delegation.IsExclusionary,
                Description = delegation.Description,
                ModuleServiceQueryModel = new MODUD.ModuleServiceQueryModel()
                {
                    Id = delegation.ParamModulen.Id,
                    Description = delegation.ParamModulen.Description
                },
                SubModuleServicesQueryModel = new MODUD.SubModuleServicesQueryModel()
                {
                    Id = delegation.ParamSubModule.Id,
                    Description = delegation.ParamSubModule.Description
                },
                HierarchyServiceQueryModel = new MODUD.HierarchyServiceQueryModel()
                {
                    Id = delegation.ParamHierarchy.Id,
                    Description = delegation.ParamHierarchy.Description
                },
                ErrorServiceModel = new ModelServices.Models.Param.ErrorServiceModel()
                {
                    ErrorTypeService = MODEN.ErrorTypeService.Ok
                },
                StatusTypeService = ModelServices.Enums.StatusTypeService.Original
            };
        }

        /// <summary>
        /// Metodo lista de jerarquias
        /// </summary>
        /// <param name="hierarchies">Recibe jerarquias</param>
        /// <returns>Retorna lista de jerarquias</returns>
        public static List<MODUD.HierarchyServiceQueryModel> CreateHierarchiesServiceModels(List<ParamHierarchy> hierarchies)
        {
            List<MODUD.HierarchyServiceQueryModel> hierarchiesServiceModel = new List<MODUD.HierarchyServiceQueryModel>();
            foreach (var item in hierarchies)
            {
                hierarchiesServiceModel.Add(CreateHierarchyServiceModel(item));
            }

            return hierarchiesServiceModel;
        }

        /// <summary>
        /// Metodo convierte de modelo a servicio Jerarquia
        /// </summary>
        /// <param name="hierarchy">Recibe Jerarquia</param>
        /// <returns>Retorna Jerarquia</returns>
        public static MODUD.HierarchyServiceQueryModel CreateHierarchyServiceModel(ParamHierarchy hierarchy)
        {
            return new MODUD.HierarchyServiceQueryModel()
            {
                Id = hierarchy.Id,
                Description = hierarchy.Description
            };
        }

        /// <summary>
        /// Metodo Modulo
        /// </summary>
        /// <param name="module">Recibe modulo</param>
        /// <returns>Retorna modulo</returns>
        public static MODUD.ModuleServiceQueryModel CreateModuleServiceModel(ParamModule module)
        {
            return new MODUD.ModuleServiceQueryModel()
            {
                Id = module.Id,
                Description = module.Description
            };
        }

        /// <summary>
        /// Metodo lista de modulo
        /// </summary>
        /// <param name="modules">Recibe modulos</param>
        /// <returns>Retorna lista de modulos</returns>
        public static List<MODUD.ModuleServiceQueryModel> CreateModuleServiceModels(List<ParamModule> modules)
        {
            List<MODUD.ModuleServiceQueryModel> moduleServiceModel = new List<MODUD.ModuleServiceQueryModel>();
            foreach (var item in modules)
            {
                moduleServiceModel.Add(CreateModuleServiceModel(item));
            }

            return moduleServiceModel;
        }

        /// <summary>
        /// Metodo SubModulo
        /// </summary>
        /// <param name="subModule">Recibe SubModulo</param>
        /// <returns>Retorna SubModulo</returns>
        public static MODUD.SubModuleServicesQueryModel CreateSubModuleServiceModel(ParamSubModule subModule)
        {
            return new MODUD.SubModuleServicesQueryModel()
            {
                Id = subModule.Id,
                Description = subModule.Description,
                ModuleId = subModule.ModuleId
                
            };
        }

        /// <summary>
        /// Metodo lista de subModulo
        /// </summary>
        /// <param name="hierarchies">Recibe subModulo</param>
        /// <returns>Retorna lista de subModulo</returns>
        public static List<MODUD.SubModuleServicesQueryModel> CreateSubModuleServiceModels(List<ParamSubModule> hierarchies)
        {
            List<MODUD.SubModuleServicesQueryModel> subModuleServiceModel = new List<MODUD.SubModuleServicesQueryModel>();
            foreach (var item in hierarchies)
            {
                subModuleServiceModel.Add(CreateSubModuleServiceModel(item));
            }

            return subModuleServiceModel;
        }



        /// <summary>
        /// Metodo Modulo
        /// </summary>
        /// <param name="module">Recibe modulo</param>
        /// <returns>Retorna modulo</returns>
        public static MODUD.ModuleSubmoduleServiceQueryModel CreateModuleSubModuleServiceModel(ParamModule module)
        {
            MODUD.ModuleSubmoduleServiceQueryModel moduleSubModule = new MODUD.ModuleSubmoduleServiceQueryModel();
            moduleSubModule.Id = module.Id;
            moduleSubModule.Description = module.Description;
            moduleSubModule.SubModuleQueryModel = CreateSubModuleServiceModels(module.SubModules);
            return moduleSubModule;
        }

        /// <summary>
        /// Metodo lista de modulo
        /// </summary>
        /// <param name="modules">Recibe modulos</param>
        /// <returns>Retorna lista de modulos</returns>
        public static List<MODUD.ModuleSubmoduleServiceQueryModel> CreateModuleSubModuleServiceModels(List<ParamModule> modules)
        {
            List<MODUD.ModuleSubmoduleServiceQueryModel> moduleServiceModel = new List<MODUD.ModuleSubmoduleServiceQueryModel>();
            foreach (var item in modules)
            {
                moduleServiceModel.Add(CreateModuleSubModuleServiceModel(item));
            }

            return moduleServiceModel;
        }

    }
}
