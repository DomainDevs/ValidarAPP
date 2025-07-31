// -----------------------------------------------------------------------
// <copyright file="AlliancePrintFormatController.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Eduardo Rodríguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    using Sistran.Core.Application.ModelServices.Enums;
    using Sistran.Core.Application.ModelServices.Models.Param;
    using Sistran.Core.Application.ModelServices.Models.Printing;
    using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
    using Sistran.Core.Framework.UIF.Web.Models;
    using Sistran.Core.Framework.UIF.Web.Services;
    using Sistran.Core.Services.UtilitiesServices.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using UnderwritingModels = Application.ModelServices.Models.Underwriting;

    /// <summary>
    /// Controlador de la vista Alliance Print Format.
    /// </summary>
    public class AlliancePrintFormatController : Controller
    {
        /// <summary>
        /// Listado de Formatos de impresión de aliado.
        /// </summary>
        private List<CptAlliancePrintFormatServiceModel> alliancePrintFormats = new List<CptAlliancePrintFormatServiceModel>();

        /// <summary>
        /// Método defecto de la vista.
        /// </summary>
        /// <returns>Retorna la vista Alliance Print Format.</returns>
        public ActionResult AlliancePrintFormat()
        {
            return this.View();
        }

        /// <summary>
        /// Retorna la vista para la búsqueda avanzada.
        /// </summary>
        /// <returns>Lista de perfiles de asegurados</returns>
        public ActionResult AlliancePrintFormatAdvancedSearch()
        {
            return this.View();
        }

        /// <summary>
        /// Obtiene los Formatos de impresión de aliado.
        /// </summary>
        /// <returns>Lista de de Formatos de impresión de aliado.</returns>
        public ActionResult GetAlliancePrintFormats()
        {
            CptAlliancePrintFormatsServiceModel cptAlliancePrintFormatsServiceModelList = DelegateService.PrintingParamServiceWeb.GetCptAlliancePrintFormats();
            ErrorTypeService errorTypeProcess = cptAlliancePrintFormatsServiceModelList.ErrorTypeService;
            if (errorTypeProcess == ErrorTypeService.Ok)
            {
                List<AlliancePrintFormatViewModel> alliancePrintFormatTypeList = ModelAssembler.GetAlliancePrintFormats(cptAlliancePrintFormatsServiceModelList);
                return new UifJsonResult(true, alliancePrintFormatTypeList.OrderBy(x => x.Format).ToList());
            }
            else if (errorTypeProcess == ErrorTypeService.NotFound || errorTypeProcess == ErrorTypeService.BusinessFault || errorTypeProcess == ErrorTypeService.TechnicalFault)
            {
                return new UifJsonResult(false, this.ErrorMessages(cptAlliancePrintFormatsServiceModelList.ErrorDescription));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Obtiene los formatos de impresión del aliado.
        /// </summary>
        public ErrorTypeService GetListAlliancePrintFormats()
        {
            CptAlliancePrintFormatsServiceModel cptAlliancePrintFormatsServiceModelList = new CptAlliancePrintFormatsServiceModel();
            ErrorTypeService errorTypeProcess = cptAlliancePrintFormatsServiceModelList.ErrorTypeService;
            if (errorTypeProcess == ErrorTypeService.Ok)
            {
                this.alliancePrintFormats = DelegateService.PrintingParamServiceWeb.GetCptAlliancePrintFormats().CptAlliancePrintFormatServiceModel.ToList();
            }

            return cptAlliancePrintFormatsServiceModelList.ErrorTypeService;
        }

        public ActionResult GetPrefixs()
        {
            UnderwritingModels.PrefixsServiceQueryModel prefixsServiceQueryModel = DelegateService.PrintingParamServiceWeb.GetPrefixs();
            ErrorTypeService errorTypeProcess = prefixsServiceQueryModel.ErrorTypeService;
            if (errorTypeProcess == ErrorTypeService.Ok)
            {
                return new UifJsonResult(true, prefixsServiceQueryModel.PrefixServiceQueryModel.OrderBy(x => x.PrefixDescription).ToList());
            }
            else if (errorTypeProcess == ErrorTypeService.NotFound || errorTypeProcess == ErrorTypeService.BusinessFault || errorTypeProcess == ErrorTypeService.TechnicalFault)
            {
                return new UifJsonResult(false, this.ErrorMessages(prefixsServiceQueryModel.ErrorDescription));
            }
            else
            {
                return null;
            }
        }

        public ActionResult GetEndorsementTypes()
        {
            UnderwritingModels.EndorsementTypesServiceQueryModel endorsementTypesServiceQueryModel = DelegateService.PrintingParamServiceWeb.GetEndorsementTypes();
            ErrorTypeService errorTypeProcess = endorsementTypesServiceQueryModel.ErrorTypeService;
            if (errorTypeProcess == ErrorTypeService.Ok)
            {
                return new UifJsonResult(true, endorsementTypesServiceQueryModel.EndorsementTypeServiceQueryModel.OrderBy(x => x.Description).ToList());
            }
            else if (errorTypeProcess == ErrorTypeService.NotFound || errorTypeProcess == ErrorTypeService.BusinessFault || errorTypeProcess == ErrorTypeService.TechnicalFault)
            {
                return new UifJsonResult(false, this.ErrorMessages(endorsementTypesServiceQueryModel.ErrorDescription));
            }
            else
            {
                return null;
            }
        }

        #region Formato del mensaje de operaciones realizadas

        /// <summary>
        /// Realiza los procesos del CRUD para los formatos de impresion de alidos.
        /// </summary>
        /// <param name="listAdded"> Lista de alliancePrintFormats(Formatos de impresion de alidos) para ser agregados</param>
        /// <param name="listModified">Lista de alliancePrintFormats(Formatos de impresion de alidos) para ser modificados</param>
        /// <param name="listDeleted">Lista de alliancePrintFormats(Formatos de impresion de alidos) para ser eliminados</param>
        /// <returns>Respuesta con el total de procesos realizados </returns>
        public ActionResult SaveAlliancePrintFormats(List<AlliancePrintFormatViewModel> listAdded, List<AlliancePrintFormatViewModel> listModified, List<AlliancePrintFormatViewModel> listDeleted)
        {
            try
            {
                CptAlliancePrintFormatsServiceModel cptAlliancePrintFormatServiceModel = new CptAlliancePrintFormatsServiceModel();
                cptAlliancePrintFormatServiceModel.ErrorDescription = new List<string>();
                cptAlliancePrintFormatServiceModel.ErrorTypeService = ErrorTypeService.Ok;

                List<CptAlliancePrintFormatServiceModel> listToPersistCptAlliancePrintFormatServiceModel;

                List<CptAlliancePrintFormatServiceModel> ListAddedCptAlliancePrintFormatServiceModel = ModelAssembler.MappAlliancePrintFormatsToSave(listAdded, StatusTypeService.Create);
                List<CptAlliancePrintFormatServiceModel> ListModifiedCptAlliancePrintFormatServiceModel = ModelAssembler.MappAlliancePrintFormatsToSave(listModified, StatusTypeService.Update);
                List<CptAlliancePrintFormatServiceModel> ListDeletedCptAlliancePrintFormatServiceModel = ModelAssembler.MappAlliancePrintFormatsToSave(listDeleted, StatusTypeService.Delete);

                listToPersistCptAlliancePrintFormatServiceModel = ModelAssembler.MappAllAlliancePrintFormatsToSave(ListAddedCptAlliancePrintFormatServiceModel, ListModifiedCptAlliancePrintFormatServiceModel, ListDeletedCptAlliancePrintFormatServiceModel);

                cptAlliancePrintFormatServiceModel.CptAlliancePrintFormatServiceModel = listToPersistCptAlliancePrintFormatServiceModel;
                cptAlliancePrintFormatServiceModel.ErrorDescription = new List<string>();
                cptAlliancePrintFormatServiceModel.ErrorTypeService = ErrorTypeService.Ok;

                ParametrizationResponse<CptAlliancePrintFormatsServiceModel> alliancePrintFormatResponse = DelegateService.PrintingParamServiceWeb.CreateAlliancePrintFormats(cptAlliancePrintFormatServiceModel);

                string added = string.Empty;
                string edited = string.Empty;
                string deleted = string.Empty;
                string message;

                if (!string.IsNullOrEmpty(alliancePrintFormatResponse.ErrorAdded))
                {
                    alliancePrintFormatResponse.ErrorAdded = App_GlobalResources.Language.ResourceManager.GetString(alliancePrintFormatResponse.ErrorAdded);
                }

                if (!string.IsNullOrEmpty(alliancePrintFormatResponse.ErrorModify))
                {
                    alliancePrintFormatResponse.ErrorModify = App_GlobalResources.Language.ResourceManager.GetString(alliancePrintFormatResponse.ErrorModify);
                }

                if (!string.IsNullOrEmpty(alliancePrintFormatResponse.ErrorDeleted))
                {
                    alliancePrintFormatResponse.ErrorDeleted = App_GlobalResources.Language.ResourceManager.GetString(alliancePrintFormatResponse.ErrorDeleted);
                }

                if (alliancePrintFormatResponse.TotalAdded > 0)
                {
                    added = App_GlobalResources.Language.ReturnSaveAddedAlliancePrintFormats;
                }
                else
                {
                    alliancePrintFormatResponse.TotalAdded = null;
                }

                if (alliancePrintFormatResponse.TotalModify > 0)
                {
                    edited = App_GlobalResources.Language.ReturnSaveEditedAlliancePrintFormats;
                }
                else
                {
                    alliancePrintFormatResponse.TotalModify = null;
                }

                if (alliancePrintFormatResponse.TotalDeleted > 0)
                {
                    deleted = App_GlobalResources.Language.ReturnSaveDeletedAlliancePrintFormats;
                }
                else
                {
                    alliancePrintFormatResponse.TotalDeleted = null;
                }

                message = string.Format(
                    added + edited + deleted + "{3}{4}{5}",
                    alliancePrintFormatResponse.TotalAdded.ToString() ?? string.Empty,
                    alliancePrintFormatResponse.TotalModify.ToString() ?? string.Empty,
                    alliancePrintFormatResponse.TotalDeleted.ToString() ?? string.Empty,
                    alliancePrintFormatResponse.ErrorAdded ?? string.Empty,
                    alliancePrintFormatResponse.ErrorModify ?? string.Empty,
                    alliancePrintFormatResponse.ErrorDeleted ?? string.Empty);

                ErrorTypeService errorTypeGetListAlliancePrintFormats = this.GetListAlliancePrintFormats();
                var result = new List<AlliancePrintFormatViewModel>();
                if (errorTypeGetListAlliancePrintFormats == ErrorTypeService.Ok)
                {
                    CptAlliancePrintFormatsServiceModel cptAlliancePrintFormatsServiceModel = new CptAlliancePrintFormatsServiceModel();
                    cptAlliancePrintFormatsServiceModel.CptAlliancePrintFormatServiceModel = this.alliancePrintFormats;
                    List<AlliancePrintFormatViewModel> alliancePrintFormatTypeList = ModelAssembler.GetAlliancePrintFormats(cptAlliancePrintFormatsServiceModel);
                    result = alliancePrintFormatTypeList.OrderBy(x => x.Format).ToList();
                }
                return new UifJsonResult(true, new { message = message, data = result });
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSaveAlliancePrintFormat);
            }
        }

        #endregion Formato del mensaje de operaciones realizadas

        /// <summary>
        /// Genera archivo excel de Perfiles de Asgurado.
        /// </summary>
        /// <returns>Objeto tipo ActionResult.</returns>
        public ActionResult GenerateFileToExportAlliancePrintFormats(List<UnderwritingModels.EndorsementTypeServiceQueryModel> listEndorsementType, List<UnderwritingModels.PrefixServiceQueryModel> listPrefix)
        {
            try
            {
                ErrorTypeService errorTypeGetListAlliancePrintFormats = this.GetListAlliancePrintFormats();
                if (errorTypeGetListAlliancePrintFormats == ErrorTypeService.Ok)
                {
                    this.MappDescriptionsValues(listEndorsementType, listPrefix);
                    ExcelFileServiceModel exportFile = DelegateService.PrintingParamServiceWeb.GenerateFileToCptAlliancePrintFormats(this.alliancePrintFormats, App_GlobalResources.Language.FileNameAlliancePrintFormat);
                    if (exportFile.ErrorTypeService == ErrorTypeService.Ok)
                    {
                        return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + exportFile.FileData);
                    }
                    else
                    {
                        return new UifJsonResult(false, ErrorMessages(exportFile.ErrorDescription));
                    }
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorFileNotFound);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
            }
        }

        /// <summary>
        /// Metodo para adciionar las descripciones de los ramos y tipos de endoso.
        /// </summary>
        /// <param name="listEndorsementType">listado de tipos de endoso.</param>
        /// <param name="listPrefix">listado de ramos comerciales.</param>
        private void MappDescriptionsValues(List<UnderwritingModels.EndorsementTypeServiceQueryModel> listEndorsementType, List<UnderwritingModels.PrefixServiceQueryModel> listPrefix)
        {
            List<CptAlliancePrintFormatServiceModel> tempAlliancePrintFormats = new List<CptAlliancePrintFormatServiceModel>();
            foreach (CptAlliancePrintFormatServiceModel item in this.alliancePrintFormats)
            {
                item.EndorsementTypeServiceQueryModel.Description = listEndorsementType.Find(x => x.Id == item.EndorsementTypeServiceQueryModel.Id).Description;
                item.PrefixServiceQueryModel.PrefixDescription = listPrefix.Find(x => x.PrefixCode == item.PrefixServiceQueryModel.PrefixCode).PrefixDescription;
                tempAlliancePrintFormats.Add(item);
            }
            this.alliancePrintFormats = tempAlliancePrintFormats;
        }

        /// <summary>
        /// Metodo para retornar los mensajes de error.
        /// </summary>
        /// <param name="errorList"></param>
        /// <returns>Mensajes de error.</returns>
        private string ErrorMessages(List<string> errorList)
        {
            string errorMessages = string.Empty;
            foreach (string errorMessageItem in errorList)
            {
                errorMessages = errorMessages + errorMessageItem + " <br>";
            }
            return errorMessages;
        }
    }
}