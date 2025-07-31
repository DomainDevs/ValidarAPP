// -----------------------------------------------------------------------
// <copyright file="CoreAssembler.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>SISTRAN\Stiveen Niño</author>
// -----------------------------------------------------------------------


namespace Company.AuthorizationPoliciesParamService.EEProvider.ServicesAssemblers
{
    using System.Collections.Generic;
    using Company.AuthorizationPoliciesParamServices.Models;
    using Sistran.Company.Application.ModelServices.Enums;
    using Sistran.Core.Framework.DAF;
    using MODEL = Sistran.Company.Application.ModelServices.Models.AuthorizationPolicies;
    using ModelParam = Sistran.Company.Application.ModelServices.Models.Param;
    public class ModelServiceAssember
    {
        #region BaseEjectionCauses
        /// <summary>
        /// Metodo lista de modulo
        /// </summary>
        /// <param name="modules">Recibe modulos</param>
        /// <returns>Retorna lista de modulos</returns>
        public static List<MODEL.RejectionCauseServiceModel> CreateBaseEjectionCauses(List<CompanyParamBaseEjectionCauses> modules)
        {
            List<MODEL.RejectionCauseServiceModel> moduleServiceModel = new List<MODEL.RejectionCauseServiceModel>();
            foreach (var item in modules)
            {
                moduleServiceModel.Add(CreateBaseEjectionCause(item));
            }
            return moduleServiceModel;
        }
        /// <summary>
        /// Metodo Modulo
        /// </summary>
        /// <param name="module">Recibe modulo</param>
        /// <returns>Retorna modulo</returns>
        public static MODEL.RejectionCauseServiceModel CreateBaseEjectionCause(CompanyParamBaseEjectionCauses module)
        {
            MODEL.RejectionCauseServiceModel moduleRejectionCause = new MODEL.RejectionCauseServiceModel();
            moduleRejectionCause.id = module.Id;
            moduleRejectionCause.description = module.Description;
            moduleRejectionCause.StatusTypeService = StatusTypeService.Original;
            moduleRejectionCause.GroupPolicies = new ModelParam.GenericModelServicesQueryModel { id = module.paramBaseGroupPolicies.id, description = module.paramBaseGroupPolicies.description };
            return moduleRejectionCause;
        }

        /// <summary>
        /// Metodo lista de modulo
        /// </summary>
        /// <param name="modules">Recibe modulos</param>
        /// <returns>Retorna lista de modulos</returns>
        public static List<ModelParam.GenericModelServicesQueryModel> CreateBaseGroupsPolicies(List<CompanyParamGroupPolicies> modules)
        {
            List<ModelParam.GenericModelServicesQueryModel> moduleServiceModel = new List<ModelParam.GenericModelServicesQueryModel>();
            foreach (var item in modules)
            {
                moduleServiceModel.Add(CreateBaseGroupPolicies(item));
            }
            return moduleServiceModel;
        }

        /// <summary>
        /// Metodo Modulo
        /// </summary>
        /// <param name="module">Recibe modulo</param>
        /// <returns>Retorna modulo</returns>
        public static ModelParam.GenericModelServicesQueryModel CreateBaseGroupPolicies(CompanyParamGroupPolicies module)
        {
            ModelParam.GenericModelServicesQueryModel moduleBaseGroupPolicies = new ModelParam.GenericModelServicesQueryModel();
            moduleBaseGroupPolicies.id = module.id;
            moduleBaseGroupPolicies.description = module.description;
            return moduleBaseGroupPolicies;
        }
        #endregion
    }
}
