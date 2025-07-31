// -----------------------------------------------------------------------
// <copyright file="IAuthorizationPoliciesParamServiceWeb.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>SISTRAN\Camila Vergara</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.AuthorizationPoliciesParamService
{
    using System.ServiceModel;
    using System.Collections.Generic;
    using MODPA = Sistran.Core.Application.ModelServices.Models.Param;
    using MODUD = Sistran.Core.Application.ModelServices.Models.AuthorizationPolicies;
    using Sistran.Core.Application.AuthorizationPoliciesParamService.Models;

    /// <summary>
    /// Defines the <see cref="IAuthorizationPoliciesParamServiceWebCore" />
    /// </summary>
    [ServiceContract]
    public interface IAuthorizationPoliciesParamServiceWebCore
    {
        /// <summary>
        /// The Prueba
        /// </summary>
        /// <returns>The <see cref="string"/></returns>
        [OperationContract]
        string Prueba();

        /// <summary>
        /// Obtiene operacion a realizar para delegaciones 
        /// </summary>
        /// <param name="delegationServiceModel">Lista de delegaciones <see cref="List{MODUD.HierarchyAssociationServiceModel}"/></param>
        /// <returns>La lista <see cref="List{MODUD.HierarchyAssociationServiceModel}"/></returns>
        [OperationContract]
        List<MODUD.HierarchyAssociationServiceModel> ExecuteOperationsDelegationServiceModel(List<MODUD.HierarchyAssociationServiceModel> delegationServiceModel);

        /// <summary>
        /// Genera archivo excel delegaciones
        /// </summary>
        /// <param name="paymentPlans">Lista Delegaciones <see cref="List{MODUD.HierarchyAssociationServiceModel}"/></param>
        /// <param name="fileName">Nombre archivo <see cref="string"/></param>
        /// <returns>The <see cref="MODPA.ExcelFileServiceModel"/></returns>
        [OperationContract]
        MODPA.ExcelFileServiceModel GenerateFileToDelegation(List<MODUD.HierarchyAssociationServiceModel> paymentPlans, string fileName);

        /// <summary>
        /// Obtiene lisyta de delegaciones por nombre
        /// </summary>
        /// <param name="description">Nombre delegado <see cref="string"/></param>
        /// <returns>Lista de delagados <see cref="MODUD.HierarchiesAssociationServiceModel"/></returns>
        [OperationContract]
        MODUD.HierarchiesAssociationServiceModel GetDelegationByNameServiceModel(string description);

        /// <summary>
        /// Obtiene lista de delegaciones
        /// </summary>
        /// <returns>Lista de delegados registrados en BD <see cref="MODUD.HierarchiesAssociationServiceModel"/></returns>
        [OperationContract]
        MODUD.HierarchiesAssociationServiceModel GetDelegationServiceModel();

        /// <summary>
        /// Obtiene lista de Jerarquía
        /// </summary>
        /// <returns>Lista de Jerarquía en BD <see cref="MODUD.HierarchiesServiceQueryModel"/></returns>
        [OperationContract]
        MODUD.HierarchiesServiceQueryModel GetHierarchyServiceModel();

        /// <summary>
        /// Obtiene lista de Modulos
        /// </summary>
        /// <returns>Retorna lista de modulos <see cref="MODUD.ModulesServiceQueryModel"/></returns>
        [OperationContract]
        MODUD.ModuleSubmoduleServicesQueryModel GetModuleServiceModel();

        /// <summary>
        /// Obtiene lista de Submodulos
        /// </summary>
        /// <returns>Retorna lista de Submodulos <see cref="MODUD.SubModulesServiceQueryModel"/></returns>
        [OperationContract]
        MODUD.SubModulesServiceQueryModel GetSubModuleServiceModel();

        [OperationContract]
        MODUD.SubModulesServiceQueryModel GetSubModuleForItemIdModuleServiceModel(int idModule);
    }
}
