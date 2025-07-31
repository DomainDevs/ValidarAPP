// -----------------------------------------------------------------------
// <copyright file="IPrintingParamServiceWeb.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Eduardo Rodríguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.PrintingParamServices
{
    using System.Collections.Generic;
    using System.ServiceModel;
    using Sistran.Core.Application.CommonService.Models;    
    using UnderwritingModels = ModelServices.Models.Underwriting;
    using Sistran.Core.Application.ModelServices.Models.Printing;
    using Sistran.Core.Application.ModelServices.Models.Param;
    using Sistran.Core.Services.UtilitiesServices.Models;

    /// <summary>
    /// Interfaz de parametrización.
    /// </summary>
    [ServiceContract]
    public interface IPrintingParamServiceWeb
    {
        /// <summary>
        /// /// Obtiene la lista de Formatos de impresión de aliados.
        /// </summary>
        /// <returns>Modelo de sevicio para Formatos de impresión de aliados.</returns>
        [OperationContract]
        CptAlliancePrintFormatsServiceModel GetCptAlliancePrintFormats();

        /// <summary>
        /// Genera archivo excel para los Formatos de impresión de aliados.
        /// </summary>
        /// <param name="cptAlliancePrintFormatsList">Formatos de impresión de aliados.</param>
        /// <param name="fileName">Nombre del archivo.</param>
        /// <returns>Path archivo de excel</returns>
        [OperationContract]
        ExcelFileServiceModel GenerateFileToCptAlliancePrintFormats(List<CptAlliancePrintFormatServiceModel> cptAlliancePrintFormatsList, string fileName);

        /// <summary>
        /// Adiciona y Guarda para los formatos de impresión de aliados.
        /// </summary>
        /// <returns>Modelo de sevicio para formatos de impresión de aliados.</returns>
        [OperationContract]
        ParametrizationResponse<CptAlliancePrintFormatsServiceModel> CreateAlliancePrintFormats(CptAlliancePrintFormatsServiceModel cptAlliancePrintFormatsServiceModel);

        /// <summary>
        /// /// Obtiene la lista de Ramos comerciales.
        /// </summary>
        /// <returns>Modelo de sevicio para Formatos de impresión de aliados.</returns>
        [OperationContract]
        UnderwritingModels.PrefixsServiceQueryModel GetPrefixs();

        /// <summary>
        /// /// Obtiene la lista de los tipos de endoso.
        /// </summary>
        /// <returns>Modelo de sevicio para los tipos de endoso.</returns>
        [OperationContract]
        UnderwritingModels.EndorsementTypesServiceQueryModel GetEndorsementTypes();
    }
}
