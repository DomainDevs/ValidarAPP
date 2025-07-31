// -----------------------------------------------------------------------
// <copyright file="AuthorizationPoliciesParamServiceEEProviderWeb.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>SISTRAN\Stiveen Niño Gutierrez</author>
// -----------------------------------------------------------------------


namespace Sistran.Company.Application.AuthorizationPoliciesParamService
{
    using global::Company.AuthorizationPoliciesParamServices.Models;
    using Sistran.Company.Application.ModelServices.Models.AuthorizationPolicies;
    using Sistran.Company.Application.ModelServices.Models.Param;
    using Sistran.Core.Application.AuthorizationPoliciesParamService;
    using System.Collections.Generic;
    using System.ServiceModel;

    [ServiceContract]
    public interface IAuthorizationPoliciesParamServiceWeb : IAuthorizationPoliciesParamServiceWebCore
    {

        /// <summary>
        /// Obtiene lista de Modulos
        /// </summary>
        /// <returns>Retorna lista de modulos <see cref="RejectionCausesServiceModel"/></returns>
        [OperationContract]
        RejectionCausesServiceModel CompanyGetRejectionCauses();

        /// <summary>
        /// Obtiene lista de Modulos
        /// </summary>
        /// <returns>Retorna lista de modulos <see cref="RejectionCausesServiceModel"/></returns>
        [OperationContract]
        RejectionCausesServiceModel CompanyGetRejectionCauseByDescription(string Description, int groupPolicie);

        /// <summary>
        /// Obtiene archivo en excel
        /// </summary>
        /// <returns>Retorna lista de modulos <see cref="ExcelFileServiceModel"/></returns>
        [OperationContract]
        ExcelFileServiceModel CompanyGenerateFileToRejectionCause(string fileName);

        /// <summary>
        /// Obtiene listado para execute operation
        /// </summary>
        /// <returns>Retorna lista de modulos <see cref="ExcelFileServiceModel"/></returns>
        [OperationContract]
        List<RejectionCauseServiceModel> CompanyExecuteOperationsRejectionCausesServiceModel(List<RejectionCauseServiceModel> rejectionCauseServiceModel);

        /// <summary>
        /// Obtiene listado para grupo de politicas
        /// </summary>
        /// <returns>Retorna lista de modulos <see cref="GenericModelsServicesQueryModel"/></returns>
        [OperationContract]
        GenericModelsServicesQueryModel CompanyGetGroupPolicies();

        /// <summary>
        /// Obtiene lista de motivos de rechazo por grupo de poliza
        /// </summary>
        /// <returns>Retorna lista de motivos de rechazo <see cref="RejectionCausesServiceModel"/></returns>
        [OperationContract]
        RejectionCausesServiceModel CompanyGetRejectionCausesByGroupPolicyId(int groupPolicyId);
    }

}
