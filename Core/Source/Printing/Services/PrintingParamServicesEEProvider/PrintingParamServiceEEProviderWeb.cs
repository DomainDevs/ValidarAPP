// -----------------------------------------------------------------------
// <copyright file="PrintingParamServiceEEProviderWeb.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Eduardo Rodríguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.PrintingParamServices.EEProvider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using Sistran.Core.Application.CommonService.Models;
    using Sistran.Core.Application.ModelServices;
    using Sistran.Core.Application.ModelServices.Enums;
    using Sistran.Core.Application.ModelServices.Models.Printing;
    using UnderwritingModels = Sistran.Core.Application.ModelServices.Models.Underwriting;
    using Sistran.Core.Application.PrintingParamServices.EEProvider.Assemblers;
    using Sistran.Core.Application.PrintingParamServices.EEProvider.DAOs;
    using Sistran.Core.Application.PrintingParamServices.Models;
    using Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Application.ModelServices.Models.Param;
    using Sistran.Core.Services.UtilitiesServices.Models;

    /// <summary>
    /// Clase que implementa la interfaz IPrintingParamServiceWeb.
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class PrintingParamServiceEEProviderWeb : IPrintingParamServiceWeb
    {
        /// <summary>
        /// /// Obtiene la lista de Formatos de impresión de aliados.
        /// </summary>
        /// <returns>Modelo de sevicio para Formatos de impresión de aliados.</returns>
        public CptAlliancePrintFormatsServiceModel GetCptAlliancePrintFormats()
        {
            CptAlliancePrintFormatDAO cptAlliancePrintFormatDAO = new CptAlliancePrintFormatDAO();
            CptAlliancePrintFormatsServiceModel cptAlliancePrintFormatsServiceModel = new CptAlliancePrintFormatsServiceModel();

            Result<List<ParamCptAlliancePrintFormat>, ErrorModel> resultGetCptAlliancePrintFormats = cptAlliancePrintFormatDAO.GetCptAlliancePrintFormats();
            if (resultGetCptAlliancePrintFormats is ResultError<List<ParamCptAlliancePrintFormat>, ErrorModel>)
            {
                ErrorModel errorModelResult = (resultGetCptAlliancePrintFormats as ResultError<List<ParamCptAlliancePrintFormat>, ErrorModel>).Message;
                cptAlliancePrintFormatsServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                cptAlliancePrintFormatsServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                List<ParamCptAlliancePrintFormat> resultValue = (resultGetCptAlliancePrintFormats as ResultValue<List<ParamCptAlliancePrintFormat>, ErrorModel>).Value;
                cptAlliancePrintFormatsServiceModel = ModelServiceAssembler.MappCptAlliancePrintFormats(resultValue);
            }
            
            return cptAlliancePrintFormatsServiceModel;
        }

        /// <summary>
        /// /// Obtiene la lista de Ramos comerciales.
        /// </summary>
        /// <returns>Modelo de sevicio para Formatos de impresión de aliados.</returns>
        public UnderwritingModels.PrefixsServiceQueryModel GetPrefixs()
        {
            PrefixDAO prefixDAO = new PrefixDAO();
            UnderwritingModels.PrefixsServiceQueryModel prefixsServiceQueryModel = new UnderwritingModels.PrefixsServiceQueryModel();

            Result<List<ParamPrefix>, ErrorModel> resultGetPrefixs = prefixDAO.GetPrefixs();
            if (resultGetPrefixs is ResultError<List<ParamPrefix>, ErrorModel>)
            {
                ErrorModel errorModelResult = (resultGetPrefixs as ResultError<List<ParamPrefix>, ErrorModel>).Message;
                prefixsServiceQueryModel.ErrorDescription = errorModelResult.ErrorDescription;
                prefixsServiceQueryModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                List<ParamPrefix> resultValue = (resultGetPrefixs as ResultValue<List<ParamPrefix>, ErrorModel>).Value;
                prefixsServiceQueryModel = ModelServiceAssembler.MappPrefixs(resultValue);
            }

            return prefixsServiceQueryModel;            
        }

        /// <summary>
        /// /// Obtiene la lista de los tipos de endoso.
        /// </summary>
        /// <returns>Modelo de sevicio para los tipos de endoso.</returns>
        public UnderwritingModels.EndorsementTypesServiceQueryModel GetEndorsementTypes()
        {
            EndorsementTypeDAO endorsementTypeDAO = new EndorsementTypeDAO();
            UnderwritingModels.EndorsementTypesServiceQueryModel endorsementTypesServiceQueryModel = new UnderwritingModels.EndorsementTypesServiceQueryModel();

            Result<List<ParamEndoresementType>, ErrorModel> resultGetEndorsementTypes = endorsementTypeDAO.GetEndoresementTypes();
            if (resultGetEndorsementTypes is ResultError<List<ParamEndoresementType>, ErrorModel>)
            {
                ErrorModel errorModelResult = (resultGetEndorsementTypes as ResultError<List<ParamEndoresementType>, ErrorModel>).Message;
                endorsementTypesServiceQueryModel.ErrorDescription = errorModelResult.ErrorDescription;
                endorsementTypesServiceQueryModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                List<ParamEndoresementType> resultValue = (resultGetEndorsementTypes as ResultValue<List<ParamEndoresementType>, ErrorModel>).Value;
                endorsementTypesServiceQueryModel = ModelServiceAssembler.MappPrefixs(resultValue);
            }

            return endorsementTypesServiceQueryModel;
        }

        /// <summary>
        /// Genera archivo excel para los Formatos de impresión de aliados.
        /// </summary>
        /// <param name="cptAlliancePrintFormatsList">Formatos de impresión de aliados.</param>
        /// <param name="fileName">Nombre del archivo.</param>
        /// <returns>Path archivo de excel</returns>        
        public ExcelFileServiceModel GenerateFileToCptAlliancePrintFormats(List<CptAlliancePrintFormatServiceModel> cptAlliancePrintFormatsList, string fileName)
        {
            List<string> listErrors = new List<string>();
            ExcelFileServiceModel cptAlliancePrintFormatExcelFileServiceModel = new ExcelFileServiceModel();
            try
            {
                CptAlliancePrintFormatDAO fileDAO = new CptAlliancePrintFormatDAO();
                cptAlliancePrintFormatExcelFileServiceModel.FileData = fileDAO.GenerateFileToCptAlliancePrintFormats(cptAlliancePrintFormatsList, fileName);
                cptAlliancePrintFormatExcelFileServiceModel.ErrorTypeService = ErrorTypeService.Ok;
                cptAlliancePrintFormatExcelFileServiceModel.ErrorDescription = listErrors;

                return cptAlliancePrintFormatExcelFileServiceModel;
            }
            catch (Exception)
            {
                listErrors.Add(Resources.Errors.ErrorGeneratingFile);                
                cptAlliancePrintFormatExcelFileServiceModel.ErrorDescription = listErrors;
                cptAlliancePrintFormatExcelFileServiceModel.ErrorTypeService = ErrorTypeService.TechnicalFault;
                return cptAlliancePrintFormatExcelFileServiceModel;
            }
        }


        /// <summary>
        /// Adiciona y Guarda para los formatos de impresión de aliados.
        /// </summary>
        /// <returns>Modelo de sevicio para formatos de impresión de aliados.</returns>        
        public ParametrizationResponse<CptAlliancePrintFormatsServiceModel> CreateAlliancePrintFormats(CptAlliancePrintFormatsServiceModel cptAlliancePrintFormatsServiceModel)
        {
            ParametrizationResponse<CptAlliancePrintFormatsServiceModel> cptAlliancePrintFormatsServiceModelReturn = new ParametrizationResponse<CptAlliancePrintFormatsServiceModel>();
            CptAlliancePrintFormatDAO cptAlliancePrintFormatDAO = new CptAlliancePrintFormatDAO();
            List<ParamCptAlliancePrintFormat> resultValueListAdd = new List<ParamCptAlliancePrintFormat>();
            List<ParamCptAlliancePrintFormat> resultValueListModify = new List<ParamCptAlliancePrintFormat>();
            List<ParamCptAlliancePrintFormat> resultValueListDeleted = new List<ParamCptAlliancePrintFormat>();

            List<CptAlliancePrintFormatServiceModel> filterListAdd = ModelServiceAssembler.MappAlliancePrintFormatsByStatusType(cptAlliancePrintFormatsServiceModel.CptAlliancePrintFormatServiceModel,StatusTypeService.Create);
            List<CptAlliancePrintFormatServiceModel> filterListModify = ModelServiceAssembler.MappAlliancePrintFormatsByStatusType(cptAlliancePrintFormatsServiceModel.CptAlliancePrintFormatServiceModel, StatusTypeService.Update);
            List<CptAlliancePrintFormatServiceModel> filterListDelete = ModelServiceAssembler.MappAlliancePrintFormatsByStatusType(cptAlliancePrintFormatsServiceModel.CptAlliancePrintFormatServiceModel, StatusTypeService.Delete);
            
            Result<List<ParamCptAlliancePrintFormat>, ErrorModel> resultListAdd = ServicesModelsAssembler.MappListParamCptAlliancePrintFormats(filterListAdd);
            if (resultListAdd is ResultError<List<ParamCptAlliancePrintFormat>, ErrorModel>)
            {
                ErrorModel errorModelResult = (resultListAdd as ResultError<List<ParamCptAlliancePrintFormat>, ErrorModel>).Message;
                cptAlliancePrintFormatsServiceModelReturn.ErrorAdded = string.Join(" <br/> ",errorModelResult.ErrorDescription);                
            }
            else
            {
                resultValueListAdd = (resultListAdd as ResultValue<List<ParamCptAlliancePrintFormat>, ErrorModel>).Value;                
            }

            Result<List<ParamCptAlliancePrintFormat>, ErrorModel> resultListModify = ServicesModelsAssembler.MappListParamCptAlliancePrintFormats(filterListModify);
            if (resultListModify is ResultError<List<ParamCptAlliancePrintFormat>, ErrorModel>)
            {
                ErrorModel errorModelResult = (resultListModify as ResultError<List<ParamCptAlliancePrintFormat>, ErrorModel>).Message;
                cptAlliancePrintFormatsServiceModelReturn.ErrorModify = string.Join(" <br/> ", errorModelResult.ErrorDescription);
            }
            else
            {
                resultValueListModify = (resultListModify as ResultValue<List<ParamCptAlliancePrintFormat>, ErrorModel>).Value;
            }

            Result<List<ParamCptAlliancePrintFormat>, ErrorModel> resultListDelete = ServicesModelsAssembler.MappListParamCptAlliancePrintFormats(filterListDelete);
            if (resultListDelete is ResultError<List<ParamCptAlliancePrintFormat>, ErrorModel>)
            {
                ErrorModel errorModelResult = (resultListDelete as ResultError<List<ParamCptAlliancePrintFormat>, ErrorModel>).Message;
                cptAlliancePrintFormatsServiceModelReturn.ErrorDeleted = string.Join(" <br/> ", errorModelResult.ErrorDescription);
            }
            else
            {
                resultValueListDeleted = (resultListDelete as ResultValue<List<ParamCptAlliancePrintFormat>, ErrorModel>).Value;
            }

            ParametrizationResponse<ParamCptAlliancePrintFormat> parametrizationBusinessReturn = cptAlliancePrintFormatDAO.SaveCptAlliancePrintFormat(resultValueListAdd, resultValueListModify, resultValueListDeleted);

            cptAlliancePrintFormatsServiceModelReturn.ErrorAdded = parametrizationBusinessReturn.ErrorAdded;
            cptAlliancePrintFormatsServiceModelReturn.ErrorDeleted = parametrizationBusinessReturn.ErrorDeleted;
            cptAlliancePrintFormatsServiceModelReturn.ErrorModify = parametrizationBusinessReturn.ErrorModify;            
            cptAlliancePrintFormatsServiceModelReturn.TotalAdded = parametrizationBusinessReturn.TotalAdded;
            cptAlliancePrintFormatsServiceModelReturn.TotalDeleted = parametrizationBusinessReturn.TotalDeleted;
            cptAlliancePrintFormatsServiceModelReturn.TotalModify = parametrizationBusinessReturn.TotalModify;

            return cptAlliancePrintFormatsServiceModelReturn;
        }
    }
}
