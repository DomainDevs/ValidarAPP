// -----------------------------------------------------------------------
// <copyright file="CoreAssembler.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>SISTRAN\Stiveen Niño</author>
// -----------------------------------------------------------------------

namespace Company.AuthorizationPoliciesParamService.EEProvider.CoreAssemblers
{
    using System.Collections.Generic;
    using ModelCore = Sistran.Core.Application.AuthorizationPoliciesParamService.Models;
    using Company.AuthorizationPoliciesParamServices.Models;
    using Sistran.Company.Application.ModelServices.Models.Param;
    using CORE = Sistran.Core.Application.ModelServices.Models.Param;
    using Sistran.Company.Application.ModelServices.Enums;

    /// <summary>
    /// Define la clase <see cref="CoreCompanyAssembler" />
    /// </summary>
    public class CoreCompanyAssembler
    {
        #region BaseEjectionCauses                   

        /// <summary>
        /// Metodo Modulo
        /// </summary>
        /// <param name="module">Recibe modulo</param>
        /// <returns>Retorna listado de modelo a Core</returns>
        public static List<CompanyParamBaseEjectionCauses> CreateCoreBaseEjectionCauses(List<ModelCore.ParamBaseEjectionCauses> coreBaseEjectionCauses)
        {
            List<CompanyParamBaseEjectionCauses> baseEjectionCauses = new List<CompanyParamBaseEjectionCauses>();
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
        public static CompanyParamBaseEjectionCauses CreateCoreBaseEjectionCause(ModelCore.ParamBaseEjectionCauses coreBaseEjectionCause)
        {
            CompanyParamBaseEjectionCauses BaseEjectionCause = new CompanyParamBaseEjectionCauses();
            BaseEjectionCause.Id = coreBaseEjectionCause.Id;
            BaseEjectionCause.Description = coreBaseEjectionCause.Description;
            BaseEjectionCause.paramBaseGroupPolicies = new ModelCore.ParamBaseGroupPolicies { id = coreBaseEjectionCause.paramBaseGroupPolicies.id ,  description = coreBaseEjectionCause.paramBaseGroupPolicies.description};
            return BaseEjectionCause;
        }

        /// <summary>
        /// Metodo obtiene listado group policies
        /// </summary>
        /// <param name="module">Recibe modulo</param>
        /// <returns>Retorna listado de modelo a Core</returns>
        public static List<CompanyParamGroupPolicies> CreateCoreGroupPolicies(List<ModelCore.ParamBaseGroupPolicies> coreBaseGroupPolicies)
        {
            List<CompanyParamGroupPolicies> baseGroupPolicies = new List<CompanyParamGroupPolicies>();
            foreach (var item in coreBaseGroupPolicies)
            {
                baseGroupPolicies.Add(CreateCoreBaseGroupPolicies(item));
            }
            return baseGroupPolicies;
        }

        /// <summary>
        /// Metodo Modulo
        /// </summary>
        /// <param name="module">Recibe modulo</param>
        /// <returns>Retorna individual modelo core</returns>
        public static CompanyParamGroupPolicies CreateCoreBaseGroupPolicies(ModelCore.ParamBaseGroupPolicies coreBaseGroupPolicies)
        {
            CompanyParamGroupPolicies BaseGroupPolicie = new CompanyParamGroupPolicies();
            BaseGroupPolicie.id = coreBaseGroupPolicies.id;
            BaseGroupPolicie.description = coreBaseGroupPolicies.description;

            return BaseGroupPolicie;
        }

        /// <summary>
        /// Metodo Modulo
        /// </summary>
        /// <param name="ExcelFileServiceModel">Recibe modulo</param>
        /// <returns>Retorna individual modelo core</returns>
        public static ExcelFileServiceModel ExcelFileServiceModel(CORE.ExcelFileServiceModel CoreExcelFileServiceModel)
        {
            ExcelFileServiceModel CompanyExcelFileServiceModel = new ExcelFileServiceModel();

            CompanyExcelFileServiceModel.FileData = CoreExcelFileServiceModel.FileData;
            CompanyExcelFileServiceModel.ErrorDescription = CoreExcelFileServiceModel.ErrorDescription;
            CompanyExcelFileServiceModel.ErrorTypeService = (ErrorTypeService)CoreExcelFileServiceModel.ErrorTypeService;

            return CompanyExcelFileServiceModel;
        }
        #endregion
    }
}
